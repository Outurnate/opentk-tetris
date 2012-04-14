//|-----------------------------------------------------------------------|\\
//| CoordinatedInputManager.cs                                            |\\
//| Manages keyboard controls for multiple games and auto-repeat          |\\
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
using System.Xml.Serialization;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;

namespace Tetris
{
  class CoordinatedInputManager : GameComponent
  {
    public enum PlayerNumber { PlayerOne = 1, PlayerTwo = 2, PlayerThree = 3, PlayerFour = 4 }

    struct KeyMap   { public Key  rotate, softDrop, left, right, hardDrop, hold; }
    struct KeyState { public bool rotate, softDrop, left, right, hardDrop, hold; }
    Dictionary<PlayerNumber, KeyMap> maps = new Dictionary<PlayerNumber, KeyMap>()
    {
      {
	PlayerNumber.PlayerOne,
	new KeyMap()
	{
	  rotate   = Key.W,
	  softDrop = Key.S,
	  left     = Key.A,
	  right    = Key.D,
	  hardDrop = Key.Space,
	  hold     = Key.LShift
	}
      },
      {
	PlayerNumber.PlayerTwo,
	new KeyMap()
	{
	  rotate   = Key.Up,
	  softDrop = Key.Down,
	  left     = Key.Left,
	  right    = Key.Right,
	  hardDrop = Key.RControl,
	  hold     = Key.Keypad0
	}
      },
      {
	PlayerNumber.PlayerThree,
	new KeyMap()
	{
	  rotate   = Key.Y,
	  softDrop = Key.H,
	  left     = Key.G,
	  right    = Key.J,
	  hardDrop = Key.T,
	  hold     = Key.U
	}
      },
      {
	PlayerNumber.PlayerFour,
	new KeyMap()
	{
	  rotate   = Key.L,
	  softDrop = Key.Period,
	  left     = Key.Comma,
	  right    = Key.Slash,
	  hardDrop = Key.K,
	  hold     = Key.Semicolon
	}
      }
    };
    Dictionary<PlayerNumber, KeyState> currentState = new Dictionary<PlayerNumber, KeyState>()
    {
      {
	PlayerNumber.PlayerOne,
	new KeyState()
      },
      {
	PlayerNumber.PlayerTwo,
	new KeyState()
      },
      {
	PlayerNumber.PlayerThree,
	new KeyState()
      },
      {
	PlayerNumber.PlayerFour,
	new KeyState()
      }
    };
    Dictionary<PlayerNumber, KeyState> previousState = new Dictionary<PlayerNumber, KeyState>()
    {
      {
	PlayerNumber.PlayerOne,
	new KeyState()
      },
      {
	PlayerNumber.PlayerTwo,
	new KeyState()
      },
      {
	PlayerNumber.PlayerThree,
	new KeyState()
      },
      {
	PlayerNumber.PlayerFour,
	new KeyState()
      }
    };

    public CoordinatedInputManager(GameWindow window) : base (window) { }

    protected override void DoUnLoad() { }

    protected override void DoLoad() { }

    protected override void DoUpdate(FrameEventArgs e) { }

    protected override void DoDraw(FrameEventArgs e) { }
  }
}