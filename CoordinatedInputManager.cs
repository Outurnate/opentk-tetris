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
  class CoordinatedInputManager
  {
    public enum PlayerNumber { PlayerOne = 1, PlayerTwo = 2, PlayerThree = 3, PlayerFour = 4 }

    class KeyMap   { public Key    rotate, softDrop, left, right, hardDrop, hold; }
    class KeyState { public bool   rotate, softDrop, left, right, hardDrop, hold; }
    class KeyDelay { public double rotate, softDrop, left, right, hardDrop, hold; }
    public class RepeatedKeyState
    {
      public bool Rotate
      {
	get;
	internal set;
      }
      public bool SoftDrop
      {
	get;
	internal set;
      }
      public bool Left
      {
	get;
	internal set;
      }
      public bool Right
      {
	get;
	internal set;
      }
      public bool HardDrop
      {
	get;
	internal set;
      }
      public bool Hold
      {
	get;
	internal set;
      }
    }
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
    Dictionary<PlayerNumber, KeyDelay> delayState = new Dictionary<PlayerNumber, KeyDelay>()
    {
      {
	PlayerNumber.PlayerOne,
	new KeyDelay()
      },
      {
	PlayerNumber.PlayerTwo,
	new KeyDelay()
      },
      {
	PlayerNumber.PlayerThree,
	new KeyDelay()
      },
      {
	PlayerNumber.PlayerFour,
	new KeyDelay()
      }
    };
    public Dictionary<PlayerNumber, RepeatedKeyState> KeyStates
    {
      get;
      private set;
    }
    double delayTime = 1;
    InterleavedFieldManager.NumPlayers players;
    GameWindow window;

    public CoordinatedInputManager(GameWindow window, InterleavedFieldManager.NumPlayers players)
    {
      this.players = players;
      this.window = window;
      this.KeyStates = new Dictionary<PlayerNumber, RepeatedKeyState>()
      {
	{
	  PlayerNumber.PlayerOne,
	  new RepeatedKeyState()
	},
	{
	  PlayerNumber.PlayerTwo,
	  new RepeatedKeyState()
	},
	{
	  PlayerNumber.PlayerThree,
	  new RepeatedKeyState()
	},
	{
	  PlayerNumber.PlayerFour,
	  new RepeatedKeyState()
	}
      };
    }

    public void Begin(FrameEventArgs e)
    {
      for (int i = 1; i < (int)players + 1; i++)
      {
	currentState[(PlayerNumber)i].rotate   = this.window.Keyboard[maps[(PlayerNumber)i].rotate];
	currentState[(PlayerNumber)i].softDrop = this.window.Keyboard[maps[(PlayerNumber)i].softDrop];
	currentState[(PlayerNumber)i].left     = this.window.Keyboard[maps[(PlayerNumber)i].left];
	currentState[(PlayerNumber)i].right    = this.window.Keyboard[maps[(PlayerNumber)i].right];
	currentState[(PlayerNumber)i].hardDrop = this.window.Keyboard[maps[(PlayerNumber)i].hardDrop];
	currentState[(PlayerNumber)i].hold     = this.window.Keyboard[maps[(PlayerNumber)i].hold];
	if (KeyStates[(PlayerNumber)i].Rotate   = (currentState[(PlayerNumber)i].rotate   && !previousState[(PlayerNumber)i].rotate))   delayState[(PlayerNumber)i].rotate   += delayTime;
	if (KeyStates[(PlayerNumber)i].SoftDrop = (currentState[(PlayerNumber)i].softDrop && !previousState[(PlayerNumber)i].softDrop)) delayState[(PlayerNumber)i].softDrop += delayTime;
	if (KeyStates[(PlayerNumber)i].Left     = (currentState[(PlayerNumber)i].left     && !previousState[(PlayerNumber)i].left))     delayState[(PlayerNumber)i].left     += delayTime;
	if (KeyStates[(PlayerNumber)i].Right    = (currentState[(PlayerNumber)i].right    && !previousState[(PlayerNumber)i].right))    delayState[(PlayerNumber)i].right    += delayTime;
	if (KeyStates[(PlayerNumber)i].HardDrop = (currentState[(PlayerNumber)i].hardDrop && !previousState[(PlayerNumber)i].hardDrop)) delayState[(PlayerNumber)i].hardDrop += delayTime;
	if (KeyStates[(PlayerNumber)i].Hold     = (currentState[(PlayerNumber)i].hold     && !previousState[(PlayerNumber)i].hold))     delayState[(PlayerNumber)i].hold     += delayTime;
	if (currentState[(PlayerNumber)i].rotate)   delayState[(PlayerNumber)i].rotate   -= e.Time;
	if (currentState[(PlayerNumber)i].softDrop) delayState[(PlayerNumber)i].softDrop -= e.Time;
	if (currentState[(PlayerNumber)i].left)     delayState[(PlayerNumber)i].left     -= e.Time;
	if (currentState[(PlayerNumber)i].right)    delayState[(PlayerNumber)i].right    -= e.Time;
	if (currentState[(PlayerNumber)i].hardDrop) delayState[(PlayerNumber)i].hardDrop -= e.Time;
	if (currentState[(PlayerNumber)i].hold)     delayState[(PlayerNumber)i].hold     -= e.Time;
	if (delayState[(PlayerNumber)i].rotate < 0)
	{
	  delayState[(PlayerNumber)i].rotate += delayTime;
	  KeyStates[(PlayerNumber)i].Rotate = true;
	}
	if (delayState[(PlayerNumber)i].softDrop < 0)
	{
	  delayState[(PlayerNumber)i].softDrop += delayTime;
	  KeyStates[(PlayerNumber)i].SoftDrop = true;
	}
	if (delayState[(PlayerNumber)i].left < 0)
	{
	  delayState[(PlayerNumber)i].left += delayTime;
	  KeyStates[(PlayerNumber)i].Left = true;
	}
	if (delayState[(PlayerNumber)i].right < 0)
	{
	  delayState[(PlayerNumber)i].right += delayTime;
	  KeyStates[(PlayerNumber)i].Right = true;
	}
	if (delayState[(PlayerNumber)i].hardDrop < 0)
	{
	  delayState[(PlayerNumber)i].hardDrop += delayTime;
	  KeyStates[(PlayerNumber)i].HardDrop = true;
	}
	if (delayState[(PlayerNumber)i].hold < 0)
	{
	  delayState[(PlayerNumber)i].hold += delayTime;
	  KeyStates[(PlayerNumber)i].Hold = true;
	}
      }
    }

    public void End()
    {
      for (int i = 1; i < (int)players + 1; i++)
      {
	previousState[(PlayerNumber)i].rotate   = currentState[(PlayerNumber)i].rotate;
	previousState[(PlayerNumber)i].softDrop = currentState[(PlayerNumber)i].softDrop;
	previousState[(PlayerNumber)i].left     = currentState[(PlayerNumber)i].left;
	previousState[(PlayerNumber)i].right    = currentState[(PlayerNumber)i].right;
	previousState[(PlayerNumber)i].hardDrop = currentState[(PlayerNumber)i].hardDrop;
	previousState[(PlayerNumber)i].hold     = currentState[(PlayerNumber)i].hold;
      }
    }
  }
}