//|-----------------------------------------------------------------------|\\
//| ScoreFunction.cs                                                      |\\
//| Calculates score based on input from FieldLogic                       |\\
//|                                                                       |\\
//| Joseph Dillon                                                         |\\
//|-----------------------------------------------------------------------|\\
//| opentk-tetris, a tetris game                                          |\\
//| Copyright (C) 2012  Joseph Dillon                                     |\\
//|                                                                       |\\
//| This program is free software: you can redistribute it and/or modify  |\\
//| it under the terms of the GNU General Public License as published by  |\\
//| the Free Software Foundation, either version 3 of the License, or     |\\
//| (at your option) any later version.                                   |\\
//|                                                                       |\\
//| This program is distributed in the hope that it will be useful,       |\\
//| but WITHOUT ANY WARRANTY; without even the implied warranty of        |\\
//| MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the         |\\
//| GNU General Public License for more details.                          |\\
//|                                                                       |\\
//| You should have received a copy of the GNU General Public License     |\\
//| along with this program.  If not, see <http://www.gnu.org/licenses/>. |\\
//|-----------------------------------------------------------------------|\\

using GameFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;

using Texture = System.Int32;

namespace Tetris
{
  static class ScoreFunction
  {
    const double minSpeed = 1;
    const double maxSpeed = 1 / 50;
    const byte minLevel = 1;
    const byte maxLevel = 50;

    public static ulong Calculate(uint rowsCleared, ref byte level, ref ulong lines, ref double speed)
    {
      speed = 1 / ((double)(level = (byte)Math.Floor((lines += (ulong)rowsCleared) / 5d)));
      if (speed > minSpeed) speed = minSpeed;
      if (speed < maxSpeed) speed = maxSpeed;
      if (level < minLevel) level = minLevel;
      if (level > maxLevel) level = maxLevel;
      return (ulong)Math.Ceiling((double)rowsCleared * 1d / speed);
    }
  }
}