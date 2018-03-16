package main

func main() {
	activeMission := NewMission()
	control := NewMissionControl(activeMission)
	control.StartMissionControl()
}
