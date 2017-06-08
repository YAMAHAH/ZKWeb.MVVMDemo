#!bin/sh
set -e
#set -x
for package in $(npm outdated --parseable --depth=0 | cut -d: -f4)
do
    npm install "$package"
done