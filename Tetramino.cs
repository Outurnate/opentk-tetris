//|-----------------------------------------------------------------------|\\
//| Tetramino.cs                                                          |\\
//| Contains the state of all tetraminos                                  |\\
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
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;

using Texture = System.Int32;

namespace Tetris
{
  enum TetraminoColor : ushort { Cyan = 0, Yellow = 1, Purple = 2, Green = 3, Red = 4, Blue = 5, Orange = 6 };
  enum TetraminoType : ushort { I = 0, O = 1, T = 2, S = 3, Z = 4, L = 5, J = 6, Null = 7 };
  enum TetraminoRotation : ushort { Up = 0, Left = 1, Down = 2, Right = 3 };

  struct Tetramino
  {
    public TetraminoType type;
    public TetraminoColor color;
    public TetraminoRotation rotation;
    public int x;
    public int y;
    public bool ghost;
  }

  class TetraminoIgnoreColorPosition : EqualityComparer<Tetramino>
  {
    public override bool Equals(Tetramino t1, Tetramino t2)
    {
      return t1.type == t2.type && t1.rotation == t2.rotation;
    }

    public override int GetHashCode(Tetramino tx)
    {
      return ((ushort)tx.type ^ (ushort)tx.rotation).GetHashCode();
    }
  }

  class TetraminoManager
  {
    public bool[,] this[Tetramino tetramino]
    {
      get
      {
        return Definitions[tetramino];
      }
    }

    public Dictionary<TetraminoType, Tetramino> SpawnDictionary
    {
      get
      {
	return spawnDictionary;
      }
    }

    static readonly Dictionary<TetraminoType, Tetramino> spawnDictionary = new Dictionary<TetraminoType, Tetramino>()
    {
      {
	TetraminoType.I,
	new Tetramino()
	{
	  type = TetraminoType.I,
	  color = TetraminoColor.Cyan,
	  rotation = TetraminoRotation.Left,
	  x = 3,
	  y = 18,
	  ghost = false
	}
      },
      {
	TetraminoType.O,
	new Tetramino()
	{
	  type = TetraminoType.O,
	  color = TetraminoColor.Yellow,
	  rotation = TetraminoRotation.Up,
	  x = 4,
	  y = 18,
	  ghost = false
	}
      },
      {
	TetraminoType.T,
	new Tetramino()
	{
	  type = TetraminoType.T,
	  color = TetraminoColor.Purple,
	  rotation = TetraminoRotation.Right,
	  x = 4,
	  y = 17,
	  ghost = false
	}
      },
      {
	TetraminoType.S,
	new Tetramino()
	{
	  type = TetraminoType.S,
	  color = TetraminoColor.Green,
	  rotation = TetraminoRotation.Right,
	  x = 4,
	  y = 17,
	  ghost = false
	}
      },
      {
	TetraminoType.Z,
	new Tetramino()
	{
	  type = TetraminoType.Z,
	  color = TetraminoColor.Red,
	  rotation = TetraminoRotation.Right,
	  x = 4,
	  y = 17,
	  ghost = false
	}
      },
      {
	TetraminoType.L,
	new Tetramino()
	{
	  type = TetraminoType.L,
	  color = TetraminoColor.Orange,
	  rotation = TetraminoRotation.Right,
	  x = 4,
	  y = 17,
	  ghost = false
	}
      },
      {
	TetraminoType.J,
	new Tetramino()
	{
	  type = TetraminoType.J,
	  color = TetraminoColor.Blue,
	  rotation = TetraminoRotation.Right,
	  x = 4,
	  y = 17,
	  ghost = false
	}
      }
    };

    static Dictionary<Tetramino, bool[,]> Definitions = new Dictionary<Tetramino, bool[,]>(new TetraminoIgnoreColorPosition())
      {
        {
          new Tetramino()
	  {
	    type = TetraminoType.I,
            color = TetraminoColor.Cyan,
            rotation = TetraminoRotation.Up
	  },
          new bool[,]
          {
            { false, false, false, false },
            { true,  true,  true,  true  },
            { false, false, false, false },
            { false, false, false, false }
          }
        },
        {
          new Tetramino()
	  {
	    type = TetraminoType.I,
            color = TetraminoColor.Cyan,
            rotation = TetraminoRotation.Left
	  },
          new bool[,]
          {
            { false, true,  false, false },
            { false, true,  false, false },
            { false, true,  false, false },
            { false, true,  false, false }
          }
        },
        {
          new Tetramino()
	  {
	    type = TetraminoType.I,
            color = TetraminoColor.Cyan,
            rotation = TetraminoRotation.Down
	  },
          new bool[,]
          {
            { false, false, false, false },
            { false, false, false, false },
            { true,  true,  true,  true  },
            { false, false, false, false }
          }
        },
        {
          new Tetramino()
	  {
	    type = TetraminoType.I,
            color = TetraminoColor.Cyan,
            rotation = TetraminoRotation.Right
	  },
          new bool[,]
          {
            { false, false, true,  false },
            { false, false, true,  false },
            { false, false, true,  false },
            { false, false, true,  false }
          }
        },
        {
          new Tetramino()
	  {
	    type = TetraminoType.T,
            color = TetraminoColor.Purple,
            rotation = TetraminoRotation.Up
	  },
          new bool[,]
          {
            { false, true,  false },
            { true,  true,  true  },
            { false, false, false }
          }
        },
        {
          new Tetramino()
	  {
	    type = TetraminoType.T,
            color = TetraminoColor.Purple,
            rotation = TetraminoRotation.Left
	  },
          new bool[,]
          {
            { false, true,  false },
            { true,  true,  false },
            { false, true,  false }
          }
        },
        {
          new Tetramino()
	  {
	    type = TetraminoType.T,
            color = TetraminoColor.Purple,
            rotation = TetraminoRotation.Down
	  },
          new bool[,]
          {
            { false, false, false },
            { true,  true,  true  },
            { false, true,  false }
          }
        },
        {
          new Tetramino()
	  {
	    type = TetraminoType.T,
            color = TetraminoColor.Purple,
            rotation = TetraminoRotation.Right
	  },
          new bool[,]
          {
            { false, true,  false },
            { false, true,  true  },
            { false, true,  false }
          }
        },
        {
          new Tetramino()
	  {
	    type = TetraminoType.S,
            color = TetraminoColor.Green,
            rotation = TetraminoRotation.Up
	  },
          new bool[,]
          {
            { false, true,  true  },
            { true,  true,  false },
            { false, false, false }
          }
        },
        {
          new Tetramino()
	  {
	    type = TetraminoType.S,
            color = TetraminoColor.Green,
            rotation = TetraminoRotation.Left
	  },
          new bool[,]
          {
            { true,  false, false },
            { true,  true,  false },
            { false, true,  false }
          }
        },
        {
          new Tetramino()
	  {
	    type = TetraminoType.S,
            color = TetraminoColor.Green,
            rotation = TetraminoRotation.Down
	  },
          new bool[,]
          {
            { false, false, false },
            { false, true,  true  },
            { true,  true,  false }
          }
        },
        {
          new Tetramino()
	  {
	    type = TetraminoType.S,
            color = TetraminoColor.Green,
            rotation = TetraminoRotation.Right
	  },
          new bool[,]
          {
            { false, true,  false },
            { false, true,  true  },
            { false, false, true  }
          }
        },
        {
          new Tetramino()
	  {
	    type = TetraminoType.Z,
            color = TetraminoColor.Red,
            rotation = TetraminoRotation.Up
	  },
          new bool[,]
          {
            { true,  true,  false },
            { false, true,  true  },
            { false, false, false }
          }
        },
        {
          new Tetramino()
	  {
	    type = TetraminoType.Z,
            color = TetraminoColor.Red,
            rotation = TetraminoRotation.Left
	  },
          new bool[,]
          {
            { false, true,  false },
            { true,  true,  false },
            { true,  false, false }
          }
        },
        {
          new Tetramino()
	  {
	    type = TetraminoType.Z,
            color = TetraminoColor.Red,
            rotation = TetraminoRotation.Down
	  },
          new bool[,]
          {
            { false, false, false },
            { true,  true,  false },
            { false, true,  true  }
          }
        },
        {
          new Tetramino()
	  {
	    type = TetraminoType.Z,
            color = TetraminoColor.Red,
            rotation = TetraminoRotation.Right
	  },
          new bool[,]
          {
            { false, false, true  },
            { false, true,  true  },
            { false, true,  false }
          }
        },
        {
          new Tetramino()
	  {
	    type = TetraminoType.J,
            color = TetraminoColor.Blue,
            rotation = TetraminoRotation.Up
	  },
          new bool[,]
          {
            { true,  false, false },
            { true,  true,  true  },
            { false, false, false }
          }
        },
        {
          new Tetramino()
	  {
	    type = TetraminoType.J,
            color = TetraminoColor.Blue,
            rotation = TetraminoRotation.Left
	  },
          new bool[,]
          {
            { false, true,  false },
            { false, true,  false },
            { true,  true,  false }
          }
        },
        {
          new Tetramino()
	  {
	    type = TetraminoType.J,
            color = TetraminoColor.Blue,
            rotation = TetraminoRotation.Down
	  },
          new bool[,]
          {
            { false, false, false },
            { true,  true,  true  },
            { false, false, true  }
          }
        },
        {
          new Tetramino()
	  {
	    type = TetraminoType.J,
            color = TetraminoColor.Blue,
            rotation = TetraminoRotation.Right
	  },
          new bool[,]
          {
            { false, true,  true },
            { false, true,  false },
            { false, true,  false }
          }
        },
        {
          new Tetramino()
	  {
	    type = TetraminoType.L,
            color = TetraminoColor.Orange,
            rotation = TetraminoRotation.Up
	  },
          new bool[,]
          {
            { false, false, true  },
            { true,  true,  true  },
            { false, false, false }
          }
        },
        {
          new Tetramino()
	  {
	    type = TetraminoType.L,
            color = TetraminoColor.Orange,
            rotation = TetraminoRotation.Left
	  },
          new bool[,]
          {
            { true,  true,  false },
            { false, true,  false },
            { false, true,  false }
          }
        },
        {
          new Tetramino()
	  {
	    type = TetraminoType.L,
            color = TetraminoColor.Orange,
            rotation = TetraminoRotation.Down
	  },
          new bool[,]
          {
            { false, false, false },
            { true,  true,  true  },
            { true,  false, false }
          }
        },
        {
          new Tetramino()
	  {
	    type = TetraminoType.L,
            color = TetraminoColor.Orange,
            rotation = TetraminoRotation.Right
	  },
          new bool[,]
          {
            { false, true,  false },
            { false, true,  false },
            { false, true,  true  }
          }
        },
        {
          new Tetramino()
	  {
	    type = TetraminoType.O,
            color = TetraminoColor.Yellow,
            rotation = TetraminoRotation.Up
	  },
          new bool[,]
          {
            { true, true },
            { true, true }
          }
        },
        {
          new Tetramino()
	  {
	    type = TetraminoType.O,
            color = TetraminoColor.Yellow,
            rotation = TetraminoRotation.Left
	  },
          new bool[,]
          {
            { true, true },
            { true, true }
          }
        },
        {
          new Tetramino()
	  {
	    type = TetraminoType.O,
            color = TetraminoColor.Yellow,
            rotation = TetraminoRotation.Down
	  },
          new bool[,]
          {
            { true, true },
            { true, true }
          }
        },
        {
          new Tetramino()
	  {
	    type = TetraminoType.O,
            color = TetraminoColor.Yellow,
            rotation = TetraminoRotation.Right
	  },
          new bool[,]
          {
            { true, true },
            { true, true }
          }
        }
      };
  }
}
