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
  class Game : GameWindow
  {
    FieldRenderer field;
    FieldLogic field_logic;
    Vector3 pos_field;

    public Game() : base(800, 600, GraphicsMode.Default, "tk-tetris")
    {
      VSync = VSyncMode.On;
      field = new FieldRenderer(this, pos_field = new Vector3(0.0f, 0.0f, 0.0f), 10, 20) { Enabled = true, Visible = true };
      field_logic = new FieldLogic(this, field) { Enabled = true, Visible = true };
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      GL.ShadeModel(ShadingModel.Smooth);
      GL.Enable(EnableCap.Texture2D);
      GL.Enable(EnableCap.DepthTest);

      ResourceCommons.Load();

      GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);

      field_logic.Start();
    }

    protected override void OnUnload(EventArgs e)
    {
      ResourceCommons.Unload();
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

      field_logic.Update(e);
      field.Update(e);

      if (Keyboard[Key.Escape])
        Exit();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      base.OnRenderFrame(e);

      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

      GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

      Matrix4 modelview = Matrix4.LookAt(new Vector3(0.0f, 0.0f, -50.0f), pos_field, Vector3.UnitY);
      GL.MatrixMode(MatrixMode.Modelview);
      GL.LoadMatrix(ref modelview);

      field_logic.Draw(e);
      field.Draw(e);

      GL.BindTexture(TextureTarget.Texture2D, 0);

      base.SwapBuffers();
    }

    [STAThread]
    static void Main()
    {
      using (Game game = new Game())
      {
        game.Run(30.0);
      }
    }
  }
}