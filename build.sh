#!/bin/bash
gmcs -r:System.Drawing.dll -r:./bin/OpenTK.dll -out:./bin/breakout.exe Program.cs
