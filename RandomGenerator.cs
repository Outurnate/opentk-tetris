//|-----------------------------------------------------------------------|\\
//| RandomGenerator.cs                                                    |\\
//| Spawns tetraminos                                                     |\\
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
  class RandomGenerator
  {
    public struct TetraminoData
    {
      public ushort newTetramino;
      public ushort nextTetramino;
    }

    static Random random = new Random();
    int[] currentSequence;
    int c = 0;
    Queue<ushort> next = new Queue<ushort>();

    public RandomGenerator()
    {
      FillBag();
      for (int i = 0; i < 2; i++)
	next.Enqueue(GenerateU());
    }

    public TetraminoData Generate()
    {
      TetraminoData n = new TetraminoData()
      {
        newTetramino = next.Dequeue(),
	nextTetramino = next.Peek()
      };
      next.Enqueue(GenerateU());
      return n;
    }

    ushort GenerateU()
    {
      int result = currentSequence[c++];
      if (c > 6)
      {
	c = 0;
	FillBag();
      }
      return (ushort)result;
    }

    void FillBag()
    {
      currentSequence = Enumerable.Range(0, 7).OrderBy(n => random.Next()).ToArray();
    }
  }
}
