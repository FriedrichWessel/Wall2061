package main

import (
	"encoding/json"
	"net/http"
	"net/http/httptest"
	"strings"
	"testing"
)

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
