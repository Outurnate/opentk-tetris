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
  class FieldLogic : GameComponent
  {
    FieldRenderer field;

    public FieldLogic(FieldRenderer field)
    {
      this.field = field;
    }

    protected override void DoUpdate(FrameEventArgs e)
    {
      
    }

    protected override void DoDraw(FrameEventArgs e) { }
  }
}