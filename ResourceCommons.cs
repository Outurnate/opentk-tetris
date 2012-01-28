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

using GLPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace Tetris
{
  static class ResourceCommons
  {
    public static Texture Cell;

    public static void Load()
    {
      Cell = LoadTexture(Path.Combine(".", "cell.png"));
    }

    public static void Unload()
    {
      GL.DeleteTextures(1, ref Cell);
    }

    public static Texture LoadTexture(string filename)
    {
      if (String.IsNullOrEmpty(filename))
        throw new ArgumentException(filename);
      Texture id = GL.GenTexture();
      GL.BindTexture(TextureTarget.Texture2D, id);
      Bitmap bmp = new Bitmap(filename);
      BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
      GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0, GLPixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);
      bmp.UnlockBits(bmp_data);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
      return id;
    }
  }
}