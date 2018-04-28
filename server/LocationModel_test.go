package main

import (
	"testing"
)

func TestNewLocationShouldReturnLocationWithGivenId(t *testing.T) {
	testID := "testLocation1"
	testLocation := NewLocation(testID)
	if testLocation.Id != testID {
		t.Errorf("New location should have given Id %s, but has: %s", testID, testLocation.Id)
	}
}

func TestNewLocationShouldHaveZeroHackAttempts(t *testing.T) {
	testLocation := NewLocation("T1")
	if len(testLocation.HackAttempts) > 0 {
		t.Errorf("New Location should not have any HackAttempts")
	}
}

func TestGetAttemptsForUserShouldReturnZeroOnNonRegisteredUser(t *testing.T) {
	testLocation := NewLocation("T1")
	attempts := testLocation.GetHackAttemptsForUserId("NiceGuy")
	if attempts != 0 {
		t.Errorf("Expected 0 hack attempts for not registered user, but got %d", attempts)
	}
}

func TestStartAttackShouldBeDeclinedForKnownHackers(t *testing.T) {
	testLocation := NewLocation("T1")
	discovered := testLocation.StartAttack("User1")
	if !discovered {
		t.Errorf("Hacker was not discovered to early")
	}
}
func TestStartAttackShouldNotIncreaseHackAttemptsForOtherUsers(t *testing.T) {
	testLocation := NewLocation("T2")
	testLocation.StartAttack("User1")
	testLocation.StartAttack("User2")
	testLocation.StartAttack("User2")
	attemptsU1 := testLocation.GetHackAttemptsForUserId("User1")
	attemptsU2 := testLocation.GetHackAttemptsForUserId("User2")
	if attemptsU1 != 1 {
		t.Errorf("Expected 1 attack attempt for first user, but got %d", attemptsU1)
	}
	if attemptsU2 != 2 {
		t.Errorf("Expected 1 attack attempt for first user, but got %d", attemptsU2)
	}
}
func TestStartAttackShouldIncreaseTheHackAttemptsForThatUser(t *testing.T) {
	testLocation := NewLocation("T2")
	testLocation.StartAttack("User1")
	testLocation.StartAttack("User1")
	attempts := testLocation.GetHackAttemptsForUserId("User1")
	if attempts != 2 {
		t.Errorf("HackAttempt should be 1 but was %d", attempts)
	}
}

func TestFinishAttackSuccessdullShouldResetHackAttempts(t *testing.T) {
	testLocation := NewLocation("T1")
	testLocation.StartAttack("User1")
	testLocation.FinishAttackSuccessfull("User1")
	attempts := testLocation.GetHackAttemptsForUserId("User1")
	if attempts != 0 {
		t.Errorf("HackAttempts should be 0 after successfull attack. But it was %d", attempts)
	}
}
