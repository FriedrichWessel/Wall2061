package main

import "flag"

var portPtr = flag.Int("port", 8080, "port to serve")
var pathPtr = flag.String("missionFile", "test.mf", "mission file that should be used")

func main() {
	flag.Parse()

	activeMission := NewMission()
	control := NewMissionControl(activeMission)
	control.StartMissionControl(*portPtr, *pathPtr)
}
