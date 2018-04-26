#!/bin/sh
#scp -r server_builds/ root@51.15.97.184:~/
rsync -avz -e ssh --progress server_builds/ root@51.15.97.184:~/server_builds/
