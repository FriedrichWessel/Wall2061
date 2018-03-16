package main

import (
	"encoding/json"
	"testing"
)

func TestCreateNewUserModelShouldCreateModelWithGivenID(t *testing.T) {
	var testId string = "d100"
	testModel := NewUser(testId)
	if testModel.ID != testId {
		t.Errorf("UserModel is not created with given id %d, got id: %d", testId, testModel.ID)
	}
}

func TestHackLocationShouldStartAttackOnLocation(t *testing.T) {
	testModel := NewUser("t")
	testLocation := &MockLocation{0, 0, "T1", make(map[string]int)}
	testModel.hackLocation(testLocation)
	if testLocation.StartAttackCalled != 1 {
		t.Errorf("TestLocation received no Attack call")
	}
}

func TestMarshaledUserStringShouldBeTheSameUserIfUnMarshaled(t *testing.T) {
	testModel := NewUser("TestUser1")
	marshaled := testModel.marshal()
	var unmarshaled UserModel
	err := json.Unmarshal(marshaled, &unmarshaled)
	if err != nil {
		t.Errorf("Could not unmarshal marshaled object")
	}
	if unmarshaled.ID != testModel.ID {
		t.Errorf("Expected ID %s, got ID: %s", testModel.ID, unmarshaled.ID)
	}
}

func TestToJsonShouldReturnMatchingString(t *testing.T) {
	testModel := NewUser("TestUser1")
	jsonString := testModel.toJson()
	expectedResult := "{\"ID\":\"TestUser1\"}"
	if jsonString != expectedResult {
		t.Errorf("Expected json: %s, got json: %s", expectedResult, jsonString)
	}
}

type MockLocation struct {
	StartAttackCalled  int
	FinishAttackCalled int
	ID                 string
	HackAttempts       map[string]int
}

func (m *MockLocation) GetId() string                              { return m.ID }
func (m *MockLocation) GetHackAttemptsForUserId(userId string) int { return 0 }
func (m *MockLocation) StartAttack(userId string) bool {
	m.StartAttackCalled++
	return false
}
func (m *MockLocation) FinishAttackSuccessfull(userId string) {
	m.FinishAttackCalled++
}
