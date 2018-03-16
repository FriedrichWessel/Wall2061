package main

import "testing"

func TestRegisterLocationShouldAddLocationToMission(t *testing.T) {
	testMission := NewMission()
	testLocation := &MockLocation{0, 0, "T1", make(map[string]int)}
	testMission.RegisterLocation(testLocation)
	if _, ok := testMission.locations[testLocation.ID]; !ok {
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
