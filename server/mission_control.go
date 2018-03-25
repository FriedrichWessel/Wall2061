package main

import (
	"encoding/json"
	"fmt"
	"io/ioutil"
	"log"
	"net/http"
	"os"
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

func (control MissionControl) StartMissionControl() {
	fmt.Printf("Mission control running")
	http.HandleFunc(control.routes.attackLocation, control.attackLocation)
	http.HandleFunc(control.routes.finishAttack, control.finishAttackSuccessfull)
	http.HandleFunc(control.routes.queryLocations, control.getRegisteredLocations)
	http.HandleFunc(control.routes.saveMission, control.saveMission)
	http.HandleFunc(control.routes.loadMission, control.loadMission)
	err := http.ListenAndServe(":8080", nil)
	if err != nil {
		log.Fatal("Error on listen&Server", err)
	}
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
}

func (control *MissionControl) getRegisteredLocations(writer http.ResponseWriter, request *http.Request) {
	writer.Header().Set("Content-Type", "application/json")
	json.NewEncoder(writer).Encode(control.activeMission.GetLocations())
}

func (control *MissionControl) saveMission(writer http.ResponseWriter, request *http.Request) {
	action := loadFileAction(request)
	file, err := os.Create(action.FileName)
	if err != nil {
		panic(err)
	}
	printMissionDump("Save Mission", control.activeMission)
	control.activeMission.Save(action.FileName, file)
	writer.Write([]byte(fmt.Sprint("File %s saved successfull", action.FileName)))
}

func (control *MissionControl) loadMission(writer http.ResponseWriter, request *http.Request) {
	action := loadFileAction(request)
	fileReader := FileReader{}
	control.activeMission = control.activeMission.Load(action.FileName, fileReader)
	writeCurrentLocationListAsAnswer(writer, control.activeMission)
	printMissionDump("Load Mission", control.activeMission)
}

func loadLocationAction(request *http.Request) locationAction {
	decoder := json.NewDecoder(request.Body)

	var action locationAction
	err := decoder.Decode(&action)
	if err != nil {
		panic(err)
	}
	defer request.Body.Close()
	return action
}

func loadFileAction(request *http.Request) (result fileAction) {
	decoder := json.NewDecoder(request.Body)
	var action fileAction
	err := decoder.Decode(&action)
	if err != nil {
		panic(err)
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
