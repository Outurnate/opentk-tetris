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
    Vector3 pos_eye;
    InterleavedFieldManager manager;
    Random random = new Random();

    public Game(InterleavedFieldManager.NumPlayers players) : base(800, 600, GraphicsMode.Default, "opentk-tetris")
    {
      VSync = VSyncMode.On;
      pos_field = new Vector3(-5.0f, 0.0f, 0.0f);
      pos_eye = new Vector3(-5.0f, 0.0f, -30.0f * (players == InterleavedFieldManager.NumPlayers.OnePlayer ? 1 : 2));
      this.Components.Add(new ResourceCommonsLoader(this));
      this.Components.Add(manager = new InterleavedFieldManager(this, players) { Enabled = true, Visible = true });
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      GL.ShadeModel(ShadingModel.Smooth);

      GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);

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

      if (this.Keyboard[Key.Escape])
        Exit();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      GL.Enable(EnableCap.Texture2D);
      GL.Enable(EnableCap.DepthTest);
      GL.Enable(EnableCap.CullFace);
      GL.Enable(EnableCap.Blend);
      //GL.Enable(EnableCap.FramebufferSrgb);
      GL.BlendEquation(BlendEquationMode.FuncAdd);
      GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

      GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

      Matrix4 modelview = Matrix4.LookAt(pos_eye, pos_field, Vector3.UnitY);
      GL.MatrixMode(MatrixMode.Modelview);
      GL.LoadMatrix(ref modelview);

      Vector3 lightPos = new Vector3((float)random.NextDouble() * (random.Next(0, 2) == 1 ? 1 : -1), (float)random.NextDouble() * (random.Next(0, 2) == 1 ? 1 : -1), (float)random.NextDouble() * (random.Next(0, 2) == 1 ? 1 : -1));
      Vector4 lightDiff  = new Vector4(1.0f,  1.0f,  1.0f,  1.0f);
      GL.UseProgram(ResourceCommons.Simple_Shader);
      GL.Uniform3(ResourceCommons.LightPositionUniform, ref lightPos);
      GL.Uniform4(ResourceCommons.LightDiffuseUniform, ref lightDiff);
      GL.Uniform1(ResourceCommons.SamplerUniform, 0);

      base.OnRenderFrame(e);
      //GL.Disable(EnableCap.FramebufferSrgb);

      base.SwapBuffers();
    }
  }
}
