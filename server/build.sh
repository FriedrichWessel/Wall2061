#!/bin/bash
echo 'Build windows 64bit...'
GOOS=windows GOARCH=amd64 go build -o server_builds/wall2061_missioncontrol.exe main.go LocationModel.go mission_control.go mission.go UserModel.go
echo '...done'
echo 'Build linux 64bit...'
GOOS=linux GOARCH=amd64 go build -o server_builds/wall2061_missioncontrol.lnx main.go LocationModel.go mission_control.go mission.go UserModel.go
echo '...done'
echo 'Build osX darwin 64bit...'
GOOS=darwin GOARCH=amd64 go build -o server_builds/wall2061_missioncontrol.app main.go LocationModel.go mission_control.go mission.go UserModel.go
echo '...done'