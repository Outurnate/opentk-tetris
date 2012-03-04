#!/bin/bash
rm -rf "opentk"
wget --trust-server-name http://sourceforge.net/projects/opentk/files/opentk/opentk-1.0/2010-10-06/opentk-2010-10-06.zip/download
unzip opentk-2010-10-06.zip
rm opentk-2010-10-06.zip
mv "./opentk/Binaries/OpenTK/Release/OpenTK.dll" "../bin/OpenTK.dll"
mv "./opentk/Binaries/OpenTK/Release/OpenTK.dll.config" "../bin/OpenTK.dll.config"
