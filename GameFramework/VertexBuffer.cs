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
  sealed class VertexBuffer
  {
    int id;

    int Id
    {
      get 
      {
        if (id == 0)
        {
          GraphicsContext.Assert();

          GL.GenBuffers(1, out id);
          if (id == 0)
            throw new Exception("Could not create VBO.");
        }

        return id;
      }
    }

    int vbo_length;
 
    public void SetData(Vertex[] data)
    {
      if (data == null)
        throw new ArgumentNullException("data");

      vbo_length = data.Length;
      GL.BindBuffer(BufferTarget.ArrayBuffer, Id);
      GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vbo_length * Vertex.Stride), data, BufferUsageHint.StaticDraw);
    }

    public void Render()
    {
      GL.EnableClientState(ArrayCap.VertexArray);
      GL.EnableClientState(ArrayCap.NormalArray);
      GL.EnableClientState(ArrayCap.TextureCoordArray);

      GL.BindBuffer(BufferTarget.ArrayBuffer, Id);
      GL.VertexPointer(3, VertexPointerType.Float, Vertex.Stride, new IntPtr(0));
      GL.NormalPointer(NormalPointerType.Float, Vertex.Stride, new IntPtr(Vector3.SizeInBytes));
      GL.TexCoordPointer(2, TexCoordPointerType.Float, Vertex.Stride, new IntPtr(2 * Vector3.SizeInBytes));
      GL.DrawArrays(BeginMode.Triangles, 0, vbo_length);
    }
  }
}