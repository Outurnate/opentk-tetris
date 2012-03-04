using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;

namespace GameFramework
{
  struct Vertex
  {
    public Vector3 Position, Normal;
    public Vector2 TexCoord;

    public static readonly int Stride = Marshal.SizeOf(default(Vertex));
  }
}