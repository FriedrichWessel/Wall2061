package main

import (
	"encoding/json"
	"io"
	"net/http"
	"net/http/httptest"
	"os"
	"strings"
	"testing"
)

var TestMissionJSON = `{"Locations":{"Location1":{"Id":"Location1","HackAttempts":{"User1":1}},"Location2":{"Id":"Location2","HackAttempts":{}}}}`

func TestNewMissionControlShouldActivateGivenMission(t *testing.T) {
	dummyMission := &MockMission{&MockLocation{0, 0, "L1", make(map[string]int)}}
	testControl := NewMissionControl(dummyMission)
	testControl.activateMission(dummyMission)
	if testControl.activeMission != dummyMission {
		t.Errorf("Expected mission was not activated")
	}
}

func TestStartAttackShouldStartAttackOnLocation(t *testing.T) {
	dummyLocation := &MockLocation{0, 0, "L1", make(map[string]int)}
	dummyMission := &MockMission{dummyLocation}
	testControl := NewMissionControl(dummyMission)

	locationAction := &locationAction{"L1", "User1"}
	server := httptest.NewServer(http.HandlerFunc(testControl.attackLocation))
	defer server.Close()
	data, _ := json.Marshal(locationAction)
	reader := strings.NewReader(string(data))
	resp, err := http.Post(server.URL+testControl.routes.attackLocation, "application/json", reader)
	if err != nil {
		t.Fatal(err)
	}
	if resp.StatusCode != 200 {
		t.Fatalf("Received non 200 status code: %d", resp.StatusCode)
	}
	if dummyLocation.StartAttackCalled != 1 {
		t.Errorf("Dummy location should have received 1 attack but it was %d", dummyLocation.StartAttackCalled)
	}
}

func TestFinishAttackSuccessfullShouldCallFinishAttackOnLocation(t *testing.T) {
	dummyLocation := &MockLocation{0, 0, "L1", make(map[string]int)}
	dummyMission := &MockMission{dummyLocation}
	testControl := NewMissionControl(dummyMission)

	locationAction := &locationAction{"L1", "User1"}
	server := httptest.NewServer(http.HandlerFunc(testControl.finishAttackSuccessfull))
	defer server.Close()
	data, _ := json.Marshal(locationAction)
	reader := strings.NewReader(string(data))
	resp, err := http.Post(server.URL+testControl.routes.finishAttack, "application/json", reader)
	if err != nil {
		t.Fatal(err)
	}
	if resp.StatusCode != 200 {
		t.Fatalf("Received non 200 status code: %d", resp.StatusCode)
	}
	if dummyLocation.FinishAttackCalled != 1 {
		t.Errorf("Expect 1 call to FinishAttack but got %d", dummyLocation.FinishAttackCalled)
	}

}

func TestQueryRegisteredLocationsShouldReturnJsonList(t *testing.T) {
	dummyLocation := NewLocation("L1")
	dummyLocation.StartAttack("TestUser")
	dummyMission := MockMission{&dummyLocation}
	testControl := NewMissionControl(dummyMission)
	server := httptest.NewServer(http.HandlerFunc(testControl.getRegisteredLocations))
	resp, err := http.Get(server.URL + testControl.routes.queryLocations)
	if err != nil {
		t.Fatal(err)
	}

	defer resp.Body.Close()

	decoder := json.NewDecoder(resp.Body)
	locations := make(map[string]Location)
	decodeErr := decoder.Decode(&locations)
	if decodeErr != nil {
		t.Fatal(decodeErr)
	}
	location := locations["L1"]
	if location.GetId() != "L1" {
		t.Errorf("Returned location does not match")
	}
	if location.GetHackAttemptsForUserId("TestUser") != 1 {
		t.Errorf("Attack Attempt on location does not match")
	}

}

func TestSaveMissionShouldSaveMissionToFile(t *testing.T) {
	testMission := NewMission()
	json.Unmarshal([]byte(TestMissionJSON), &testMission)
	testControl := NewMissionControl(testMission)
	server := httptest.NewServer(http.HandlerFunc(testControl.saveMission))
	defer server.Close()
	fileAction := fileAction{"mainMission.mf"}
	payload, _ := json.Marshal(&fileAction)
	reader := strings.NewReader(string(payload))
	resp, err := http.Post(server.URL+testControl.routes.saveMission, "application/json", reader)
	if err != nil {
		t.Fatal(err)
	}
	if resp.StatusCode != 200 {
		t.Fatalf("Received non 200 status code: %d", resp.StatusCode)
	}
}

func TestLaodMissionFromFileShouldLoadMission(t *testing.T) {
	action := fileAction{"mainMission.mf"}
	storeMission := NewMission()
	json.Unmarshal([]byte(TestMissionJSON), &storeMission)
	file, _ := os.Create(action.FileName)
	storeMission.Save(action.FileName, file)

	var testMission = NewMission()
	testControl := NewMissionControl(testMission)

	server := httptest.NewServer(http.HandlerFunc(testControl.loadMission))
	defer server.Close()

	payload, _ := json.Marshal(&action)
	reader := strings.NewReader(string(payload))
	resp, err := http.Post(server.URL+testControl.routes.loadMission, "application/json", reader)
	if err != nil {
		t.Fatal(err)
	}
	if resp.StatusCode != 200 {
		t.Fatalf("Received non 200 status code: %d", resp.StatusCode)
	}
	_, found := testControl.activeMission.GetLocation("Location1")
	if !found {
		t.Errorf("Location1 was not loaded correctly")
	}
}

type MockMission struct {
	mockedLocation ILocation
}

func (m MockMission) GetLocation(id string) (ILocation, bool) {
	return m.mockedLocation, true
}

func (m MockMission) GetLocations() map[string]ILocation {
	var locations = make(map[string]ILocation)
	locations[m.mockedLocation.GetId()] = m.mockedLocation
	return locations
}

func (m MockMission) Save(fileName string, fileWriter io.Writer) {}
func (m MockMission) Load(fileName string, fileReader IFileReader) (loadedMission Mission) {
	return loadedMission
}
