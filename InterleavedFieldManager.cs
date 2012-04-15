//|-----------------------------------------------------------------------|\\
//| InterleavedFieldManager.cs                                            |\\
//| Renders and manages multiple fields.                                  |\\
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

using Texture = System.Int32;

using GLPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace Tetris
{
  class InterleavedFieldManager : GameComponent
  {
    public enum NumPlayers { OnePlayer = 1, TwoPlayer = 2, ThreePlayer = 3, FourPlayer = 4 };

    NumPlayers players;
    FieldLogic[] logic;
    FieldRenderer[] renderer;

    readonly Vector3[][] positions = new Vector3[][]
    {
      new Vector3[] { new Vector3(0.0f, 0.0f, 0.0f) },
      new Vector3[] { new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f) },
      new Vector3[] { new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f) },
      new Vector3[] { new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f) }
    };

    public InterleavedFieldManager(GameWindow window, NumPlayers players)
      : base(window)
    {
      this.players = players;
      this.logic = new FieldLogic[(int)players];
      this.renderer = new FieldRenderer[(int)players];
      for (int i = 0; i < (int)players; i++)
      {
	this.logic[i] = new FieldLogic(window, renderer[i] = new FieldRenderer(window, positions[i][(int)players - 1]) { Enabled = true, Visible = players == NumPlayers.OnePlayer }, (CoordinatedInputManager.PlayerNumber)(i + 1)) { Enabled = true, Visible = false };
	this.Components.Add(logic[i]);
	this.Components.Add(renderer[i]);
      }
    }

    public void StartGame()
    {
      for (int i = 0; i < (int)players; i++)
	logic[i].Start();
    }

    protected override void DoUnLoad() { }

    protected override void DoLoad() { }

    protected override void DoUpdate(FrameEventArgs e) { }

    protected override void DoDraw(FrameEventArgs e)
    {
      if (players != NumPlayers.OnePlayer)
      {
	for (int i = 0; i < (int)players; i++)
	  renderer[i].DrawScoreUI();
	GL.BindTexture(TextureTarget.Texture2D, ResourceCommons.TetrionTexture);
	for (int i = 0; i < (int)players; i++)
	  renderer[i].DrawTetrion();
	GL.BindTexture(TextureTarget.Texture2D, ResourceCommons.Block);
	for (int i = 0; i < (int)players; i++)
	  renderer[i].DrawBlock();
	GL.BindTexture(TextureTarget.Texture2D, ResourceCommons.BlockGhost);
	for (int i = 0; i < (int)players; i++)
	{
	  renderer[i].DrawGhostBlock();
	  renderer[i].DrawOther();
	}
      }
    }
  }
}