#!/bin/bash
rm -rf "EJ Random.Org Consumer" "opentk"
wget http://www.random.org/clients/http/archive/2008-06-26/ej_random_org_consumer.zip
unzip ej_random_org_consumer.zip
rm -rf ej_random_org_consumer.zip "Compiled Test App"
cp "./Compiled Class Library/EJ Random.Org Consumer.dll" "../bin/EJ Random.Org Consumer.dll"
mkdir "EJ Random.Org Consumer"
mv "Compiled Class Library" "EJ Random.Org Consumer"
mv "Source Code" "EJ Random.Org Consumer"
wget --trust-server-name http://sourceforge.net/projects/opentk/files/opentk/opentk-1.0/2010-10-06/opentk-2010-10-06.zip/download
unzip opentk-2010-10-06.zip
rm opentk-2010-10-06.zip
mv "./opentk/Binaries/OpenTK/Release/OpenTK.dll" "../bin/OpenTK.dll"
mv "./opentk/Binaries/OpenTK/Release/OpenTK.dll.config" "../bin/OpenTK.dll.config"
