package main

import (
	"encoding/json"
	"fmt"
)

type IMission interface {
	GetLocation(locationId string) (location ILocation, found bool)
	GetLocations() map[string]ILocation
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

func (mission Mission) ToJson() string {
	resultJSON := ""
	for key, loc := range mission.GetLocations() {
		jsonLoc, _ := json.Marshal(loc)
		if resultJSON != "" {
			resultJSON += fmt.Sprintf(",\"%s\": %s", key, string(jsonLoc))
		} else {
			resultJSON += "{\"locations\": {"
			resultJSON += fmt.Sprintf("\"%s\": %s", key, string(jsonLoc))
		}

	}
	resultJSON += "}}"
	return resultJSON
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
