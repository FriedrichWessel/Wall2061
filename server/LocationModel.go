package main

type ILocation interface {
	GetId() string
	GetHackAttemptsForUserId(userId string) int
	StartAttack(userId string) (discovered bool)
	FinishAttackSuccessfull(userId string)
}

type Location struct {
	Id           string
	HackAttempts map[string]int
}

func NewLocation(id string) (location Location) {
	location.Id = id
	location.HackAttempts = make(map[string]int)
	return location
}

func (location *Location) GetId() string {
	return location.Id
}

func (location *Location) GetHackAttemptsForUserId(userId string) int {
	return location.HackAttempts[userId]
}

func (location *Location) StartAttack(userId string) (discovered bool) {
	location.HackAttempts[userId]++
	return location.HackAttempts[userId] >= 3
}

func (location *Location) FinishAttackSuccessfull(userId string) {
	location.HackAttempts[userId] = 0
}
