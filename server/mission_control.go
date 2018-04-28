package main

import (
	"encoding/json"
	"fmt"
	"io/ioutil"
	"log"
	"net/http"
	"os"
	"strconv"
	"strings"
)

type locationAction struct {
	LocationID string
	UserID     string
}

type fileAction struct {
	FileName string
}

type IFileReader interface {
	ReadFile(filename string) ([]byte, error)
}

type FileReader struct{}

func (f FileReader) ReadFile(filename string) ([]byte, error) {
	return ioutil.ReadFile(filename)
}

type Routes struct {
	attackLocation string
	finishAttack   string
	queryLocations string
	saveMission    string
	loadMission    string
}
type MissionControl struct {
	routes        Routes
	activeMission IMission
	activeFile    string
}

func NewMissionControl(mission IMission) (control MissionControl) {
	control.routes = Routes{
		"/enterLocation",
		"/finishAttack",
		"/getLocations",
		"/saveMission",
		"/loadMission",
	}
	control.activeMission = mission
	return control
}

func (control *MissionControl) StartMissionControl(port int, missionFile string) {

	fmt.Printf("Mission control running on port %d loading file %s", port, missionFile)

	control.loadMissionFile(missionFile)
	http.HandleFunc("/Kombinat/", serve)
	http.HandleFunc("/Nova/", serve)
	http.HandleFunc("/Wertheim/", serve)
	http.HandleFunc(control.routes.attackLocation, control.attackLocation)
	http.HandleFunc(control.routes.finishAttack, control.finishAttackSuccessfull)
	http.HandleFunc(control.routes.queryLocations, control.getRegisteredLocations)
	http.HandleFunc(control.routes.saveMission, control.saveMission)
	http.HandleFunc(control.routes.loadMission, control.loadMission)
	err := http.ListenAndServe(":"+strconv.Itoa(port), nil)
	if err != nil {
		log.Fatal("Error on listen&Server", err)
	}
}

func serve(response http.ResponseWriter, request *http.Request) {
	path := request.URL.Path[1:]
	fmt.Println("Serving path: " + path + "\n")
	http.ServeFile(response, request, path)
}

func (control *MissionControl) activateMission(mission IMission) {
	control.activeMission = mission
}

func (control *MissionControl) attackLocation(writer http.ResponseWriter, request *http.Request) {
	action := loadLocationAction(request)
	printMissionDump("AttackLocation", control.activeMission)
	location, found := control.activeMission.GetLocation(action.LocationID)
	writer.WriteHeader(200)
	if found {
		location.StartAttack(action.UserID)
		control.activeMission = control.activeMission.UpdateLocation(location)
	}
	writeCurrentLocationListAsAnswer(writer, control.activeMission)
	printMissionDump("AttackLocation", control.activeMission)
	control.autoSaveMission()
}

func (control *MissionControl) finishAttackSuccessfull(writer http.ResponseWriter, request *http.Request) {
	action := loadLocationAction(request)
	location, found := control.activeMission.GetLocation(action.LocationID)
	writer.WriteHeader(200)
	if found {
		location.FinishAttackSuccessfull(action.UserID)
		control.activeMission = control.activeMission.UpdateLocation(location)
	}
	writeCurrentLocationListAsAnswer(writer, control.activeMission)
	printMissionDump("FinishAttack", control.activeMission)
	control.autoSaveMission()
}

func (control *MissionControl) getRegisteredLocations(writer http.ResponseWriter, request *http.Request) {
	writer.Header().Set("Content-Type", "application/json")
	json.NewEncoder(writer).Encode(control.activeMission.GetLocations())
}

func (control MissionControl) autoSaveMission() {
	control.saveMissionFile(control.activeFile)
	fmt.Println("Mission file saved to", control.activeFile)
}

func (control *MissionControl) saveMission(writer http.ResponseWriter, request *http.Request) {
	action := loadFileAction(request)
	control.saveMissionFile(action.FileName)
	writer.Write([]byte(fmt.Sprint("File %s saved successfull", action.FileName)))
}

func (control MissionControl) saveMissionFile(fileName string) {
	file, err := os.Create(fileName)
	if err != nil {
		//panic(err)
		println("Could not save mission file")
		return
	}
	printMissionDump("Save Mission", control.activeMission)
	control.activeMission.Save(fileName, file)

}

func (control *MissionControl) loadMission(writer http.ResponseWriter, request *http.Request) {
	action := loadFileAction(request)
	control.loadMissionFile(action.FileName)
	writeCurrentLocationListAsAnswer(writer, control.activeMission)
	printMissionDump("Load Mission", control.activeMission)
}

func (control *MissionControl) loadMissionFile(fileName string) {
	fileReader := FileReader{}
	control.activeFile = getAutoSaveFileName(fileName)
	control.activeMission = control.activeMission.Load(fileName, fileReader)
}

func getAutoSaveFileName(fileName string) (autoSaveFile string) {
	var fileWithoutEnding = strings.Replace(fileName, ".mf", "", 3)
	return fileWithoutEnding + "_lastActive.mf"
}

func loadLocationAction(request *http.Request) locationAction {
	decoder := json.NewDecoder(request.Body)

	var action locationAction
	err := decoder.Decode(&action)
	if err != nil {
		//panic(err)
		println("Could not load locationAction")
		return action
	}
	defer request.Body.Close()
	return action
}

func loadFileAction(request *http.Request) (result fileAction) {
	decoder := json.NewDecoder(request.Body)
	var action fileAction
	err := decoder.Decode(&action)
	if err != nil {
		//panic(err)
		println("could not load file action was not decodeable")
		return action
	}
	defer request.Body.Close()
	return action
}

func writeCurrentLocationListAsAnswer(writer http.ResponseWriter, mission IMission) {
	marshaledMission, _ := json.Marshal(mission.GetLocations())
	writer.Write(marshaledMission)
}

func printMissionDump(action string, m IMission) {
	dump, _ := json.Marshal(m)
	println("\n", action)
	println("MissionDump")
	println(string(dump))
	println("***")
}
