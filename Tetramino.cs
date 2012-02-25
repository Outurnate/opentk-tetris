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
  enum TetraminoColor { Cyan, Yellow, Purple, Green, Red, Blue, Orange };
  enum TetraminoType { I, O, T, S, Z, L, J };
  enum TetraminoRotation { Up, Left, Down, Right };

  struct Tetramino
  {
    public TetraminoType type { get; set; }
    public TetraminoColor color { get; set; }
    public TetraminoRotation rotation { get; set; }
  }

  class TetraminoManager
  {
    static Dictionary<Tetramino, bool[,]> Definitions = new Dictionary<Tetramino, bool[,]>()
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

    public bool[,] this[Tetramino tetramino]
    {
      get
      {
        return Definitions[tetramino];
      }
    }
  }
}