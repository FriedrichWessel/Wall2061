package main

import (
	"encoding/json"
	"fmt"
	"log"
	"net/http"
)

type locationAction struct {
	LocationID string
	UserID     string
}

type Routes struct {
	attackLocation string
	finishAttack   string
	queryLocations string
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
	}
	control.activeMission = mission
	return control
}

func (control MissionControl) StartMissionControl() {
	fmt.Printf("Mission control running")
	http.HandleFunc(control.routes.attackLocation, control.attackLocation)
	http.HandleFunc(control.routes.finishAttack, control.finishAttackSuccessfull)
	http.HandleFunc(control.routes.queryLocations, control.getRegisteredLocations)
	err := http.ListenAndServe(":9090", nil)
	if err != nil {
		log.Fatal("Error on listen&Server", err)
	}
}

func (control *MissionControl) activateMission(mission IMission) {
	control.activeMission = mission
}

func (control MissionControl) attackLocation(writer http.ResponseWriter, request *http.Request) {
	action := loadLocationAction(request)
	location, found := control.activeMission.GetLocation(action.LocationID)
	if found {
		location.StartAttack(action.UserID)
	}

}

func (control MissionControl) finishAttackSuccessfull(writer http.ResponseWriter, request *http.Request) {
	action := loadLocationAction(request)
	location, found := control.activeMission.GetLocation(action.LocationID)
	if found {
		location.FinishAttackSuccessfull(action.UserID)
	}
}

func (control MissionControl) getRegisteredLocations(writer http.ResponseWriter, request *http.Request) {
	writer.Header().Set("Content-Type", "application/json")
	json.NewEncoder(writer).Encode(control.activeMission.GetLocations())
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
