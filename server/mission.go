package main

import (
	"encoding/json"
	"io"
)

type IMission interface {
	GetLocation(locationId string) (location ILocation, found bool)
	GetLocations() map[string]ILocation
	Save(fileName string, fileWriter io.Writer)
	Load(fileName string, fileReader IFileReader) Mission
	UpdateLocation(location ILocation) Mission
}

type Mission struct {
	Locations map[string]ILocation
}

type SerializedMission struct {
	Locations map[string]Location
}

func NewSerializedMission() (m SerializedMission) {
	m.Locations = make(map[string]Location)
	return m
}
func NewMission() (m Mission) {
	m.Locations = make(map[string]ILocation)
	return m
}

func NewMissionFromJson(json string) (m Mission) {
	m.Locations = make(map[string]ILocation)
	return m
}

func (mission Mission) RegisterLocation(location ILocation) {
	mission.Locations[location.GetId()] = location
}

func (mission Mission) GetLocations() map[string]ILocation {
	return mission.Locations
}

func (mission Mission) GetLocation(locationID string) (location ILocation, found bool) {
	location, found = mission.Locations[locationID]
	return location, found
}

func (mission Mission) UpdateLocation(location ILocation) Mission {
	mission.Locations[location.GetId()] = location
	return mission
}

func (mission Mission) Save(fileName string, fileWriter io.Writer) {
	marshaledMission, _ := json.Marshal(mission)
	fileWriter.Write(marshaledMission)
}

func (mission Mission) Load(fileName string, fileReader IFileReader) Mission {
	content, fErr := fileReader.ReadFile(fileName)
	if fErr != nil {
		//panic(fErr)
		println("could not load mission file")
		return mission
	}
	newMission := NewMission()
	err := json.Unmarshal(content, &newMission)
	if err != nil {
		//panic(err)
		println("could not unmarhal mission file")
		return mission
	}

	return newMission
}

func (mission *Mission) UnmarshalJSON(bytes []byte) error {
	secMission := NewSerializedMission()
	err := json.Unmarshal(bytes, &secMission)
	if err != nil {
		return err
	}
	for key, _ := range secMission.Locations {
		loc := secMission.Locations[key]
		mission.RegisterLocation(&loc)
	}
	return nil
}
