//|-----------------------------------------------------------------------|\\
//| Program.cs                                                            |\\
//| Manages window and components                                         |\\
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
using GameFramework.GUI;
using System;
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
  class Game : GameComponentWindow
  {
    Vector3 pos_field;
    InterleavedFieldManager manager;
    CursorComponent cursor;

    public Game() : base(800, 600, GraphicsMode.Default, "opentk-tetris")
    {
      VSync = VSyncMode.On;
      pos_field = new Vector3(0.0f, 0.0f, 0.0f);
      this.Components.Add(new ResourceCommonsLoader(this));
      this.Components.Add(manager = new InterleavedFieldManager(this, InterleavedFieldManager.NumPlayers.OnePlayer) /*{ Enabled = true, Visible = true }*/);
      this.Components.Add(cursor = new CursorComponent(this) { Enabled = true, Visible = true });
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      GL.ShadeModel(ShadingModel.Smooth);
      GL.Enable(EnableCap.Texture2D);
      GL.Enable(EnableCap.DepthTest);
      GL.Enable(EnableCap.CullFace);

      GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);

      manager.StartGame();
    }

    protected override void OnUnload(EventArgs e)
    {
    }

    protected override void OnResize(EventArgs e)
    {
      base.OnResize(e);

      GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

      Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 1.0f, 64.0f);
      GL.MatrixMode(MatrixMode.Projection);
      GL.LoadMatrix(ref projection);
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      base.OnUpdateFrame(e);

      if (Keyboard[Key.Escape])
        Exit();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

      GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

      Matrix4 modelview = Matrix4.LookAt(new Vector3(-5.0f, 0.0f, -30.0f), Vector3.Add(pos_field, new Vector3(-5.0f, 0.0f, 0.0f)), Vector3.UnitY);
      GL.MatrixMode(MatrixMode.Modelview);
      GL.LoadMatrix(ref modelview);

      base.OnRenderFrame(e);

      base.SwapBuffers();
    }

    [STAThread]
    static void Main()
    {
      using (Game game = new Game())
      {
        game.Run(30.0, 60.0);
      }
    }
  }
}
