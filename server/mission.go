package main

type IMission interface {
	GetLocation(locationId string) (location ILocation, found bool)
	GetLocations() map[string]ILocation
}

type Mission struct {
	locations map[string]ILocation
}

func NewMission() (m Mission) {
	m.locations = make(map[string]ILocation)
	return m
}

func (mission Mission) RegisterLocation(location ILocation) {
	mission.locations[location.GetId()] = location
}

func (mission Mission) GetLocations() map[string]ILocation {
	return mission.locations
}

func (mission Mission) GetLocation(locationID string) (location ILocation, found bool) {
	location, found = mission.locations[locationID]
	return location, found
}
