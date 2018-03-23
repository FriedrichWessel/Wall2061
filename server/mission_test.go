package main

import (
	"encoding/json"
	"testing"
)

func TestRegisterLocationShouldAddLocationToMission(t *testing.T) {
	testMission := NewMission()
	testLocation := &MockLocation{0, 0, "T1", make(map[string]int)}
	testMission.RegisterLocation(testLocation)
	if _, ok := testMission.Locations[testLocation.ID]; !ok {
		t.Errorf("Location was not added to Mission")
	}
}

func TestGetLocationShouldReturnMatchingLocation(t *testing.T) {
	testMission := NewMission()
	testLocation1 := &MockLocation{0, 0, "L1", make(map[string]int)}
	testLocation2 := &MockLocation{0, 0, "L2", make(map[string]int)}
	testMission.RegisterLocation(testLocation1)
	testMission.RegisterLocation(testLocation2)
	resultLocation, found := testMission.GetLocation("L1")
	if !found {
		t.Errorf("Expected location with ID %s was not found", testLocation1.GetId())
	}
	if resultLocation != testLocation1 {
		t.Errorf("Expeceted location with id %s, but got %s", testLocation1.GetId(), resultLocation.GetId())
	}
}

func TestGetLocationShouldReturnErrorIfNoMatchingLocationExists(t *testing.T) {
	testMission := NewMission()
	testLocation1 := &MockLocation{0, 0, "L1", make(map[string]int)}
	testMission.RegisterLocation(testLocation1)
	_, found := testMission.GetLocation("X1")
	if found {
		t.Errorf("Expected error if not registered location was querried")
	}
}

func TestGetLocationsShouldReturnAllRegisteredLocations(t *testing.T) {
	testMission := NewMission()
	testLocation1 := &MockLocation{0, 0, "T1", make(map[string]int)}
	testLocation2 := &MockLocation{0, 0, "T2", make(map[string]int)}
	testMission.RegisterLocation(testLocation1)
	testMission.RegisterLocation(testLocation2)
	missions := testMission.GetLocations()
	if len(missions) != 2 {
		t.Errorf("Expected 2 locations but got %d", len(missions))
	}
	if val, ok := missions["T1"]; ok {
		if val != testLocation1 {
			t.Errorf("Location one was expected to be")
		}
	}
}

func TestNewMissionFromJsonShouldLoadMissionStateFromFile(t *testing.T) {
	var originalMission = NewMission()
	var location1 = NewLocation("Location1")
	var location2 = NewLocation("Location2")
	location1.StartAttack("User1")
	originalMission.RegisterLocation(&location1)
	originalMission.RegisterLocation(&location2)
}

func TestToJsonShouldReturnCurrentMissionAsJson(t *testing.T) {
	var originalMission = NewMission()
	var location1 = NewLocation("Location1")
	var location2 = NewLocation("Location2")
	location1.StartAttack("User1")
	originalMission.RegisterLocation(&location1)
	originalMission.RegisterLocation(&location2)
	missionJSON, _ := json.Marshal(originalMission)
	if string(missionJSON) != TestMissionJSON {
		t.Errorf("Json is not matching expected:\n %s \n got:\n %s", TestMissionJSON, missionJSON)
	}
}

func TestFromJsonShouldLoadMission(t *testing.T) {
	mission := NewMission()
	error := json.Unmarshal([]byte(TestMissionJSON), &mission)
	if error != nil {
		println(error.Error())
	}
	loc1, foundLoc1 := mission.GetLocation("Location1")
	if !foundLoc1 {
		t.Errorf("First location is not unmarshaled correct")
	}
	if loc1.GetHackAttemptsForUserId("User1") != 1 {
		t.Errorf("Hack attempts for loc1 are not unmarshaled correct")
	}
	_, foundLoc2 := mission.GetLocation("Location2")
	if !foundLoc2 {
		t.Errorf("Second location is not unmarshaled correct")
	}
}

func TestSaveToFileShouldSaveMissionToNamedFile(t *testing.T) {
	mission := NewMission()
	json.Unmarshal([]byte(TestMissionJSON), &mission)
	var writer MockWriter
	mission.Save("TestMissionFile.mf", &writer)
	if writer.WriteCalls < 1 {
		t.Errorf("File writer was not called")
	}
	marshaledMission, _ := json.Marshal(mission)
	if string(writer.WriteData) != string(marshaledMission) {
		t.Errorf("written data does not match. Expected \n%s\nGot\n%s", string(marshaledMission), string(writer.WriteData))
	}
}

func TestLoadMissionFromFileShouldLoadMission(t *testing.T) {
	mission := NewMission()
	var reader MockReader
	mission = mission.Load("testFile", &reader)
	loc, found := mission.GetLocation("Location1")
	if !found {
		t.Errorf("Location1 was not loaded correct from file")
	}
	if loc.GetHackAttemptsForUserId("User1") != 1 {
		t.Errorf("Hack Attempts for User1 loaded incorrect")
	}
}

type MockWriter struct {
	WriteCalls int
	WriteData  []byte
}

func (m *MockWriter) Write(data []byte) (bytesWritten int, err error) {
	m.WriteCalls++
	m.WriteData = data
	return 0, nil
}

type MockReader struct {
	ReadCalls int
}

func (m *MockReader) ReadFile(fileName string) (content []byte, err error) {
	m.ReadCalls++
	content = []byte(TestMissionJSON)
	return content, nil
}
