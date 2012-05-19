#!/bin/bash
povray render[8x8];
povray render[16x16];
povray render[32x32];
povray render[64x64];
povray render[128x128];
povray render[256x256];
povray render[Glade];
convert ./icon/icon8x8.png ./icon/icon16x16.png ./icon/icon32x32.png ./icon/icon64x64.png ./icon/icon128x128.png ./icon/icon256x256.png ../icon.ico
