package main

import (
	"encoding/json"
)

type UserModel struct {
	ID string
}

func NewUser(id string) (user UserModel) {
	user.ID = id
	return user
}

func (user UserModel) hackLocation(location ILocation) {
	location.StartAttack(user.ID)
}

func (user UserModel) marshal() (jsonUser []byte) {
	jsonUser, err := json.Marshal(user)
	if err != nil {
		println("Could not serialize user")
		return jsonUser
		//panic("Usermodel cannot be serialized.")
	}
	return jsonUser
}

func (user UserModel) toJson() (jsonString string) {
	return string(user.marshal())
}
