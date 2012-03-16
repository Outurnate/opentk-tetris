//|-----------------------------------------------------------------------|\\
//| MeshRenderer.cs                                                       |\\
//| Manages loading and rendering of VBOs                                 |\\
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

using Texture = System.Int32;

namespace GameFramework
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct VertexPositionColorCoordNormal
  {
    public Vector3 Position;
    public Vector2 TexCoord;
    public uint Color;

    public VertexPositionColorCoordNormal(float x, float y, float z, float u, float v, Color color)
    {
      Position = new Vector3(x, y, z);
      Color = ToRgba(color);
      TexCoord = new Vector2(u, v);
    }

    static uint ToRgba(Color color)
    {
      return (uint)color.A << 24 | (uint)color.B << 16 | (uint)color.G << 8 | (uint)color.R;
    }
  }

  public struct Mesh
  {
    public VertexPositionColorCoordNormal[] Verticies;
    public short[] Indicies;
  }

  public class MeshRenderer
  {
    struct VBO { public int VboID, EboID, NumElements; }

    VertexPositionColorCoordNormal[] verticies;
    VBO handle;

    public MeshRenderer(Mesh m)
    {
      this.verticies = m.Verticies;
      handle = new VBO();
      int size;
      GL.GenBuffers(1, out handle.VboID);
      GL.BindBuffer(BufferTarget.ArrayBuffer, handle.VboID);
      GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(verticies.Length * BlittableValueType.StrideOf(m.Verticies)), verticies, BufferUsageHint.StaticDraw);
      GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
      if (m.Verticies.Length * BlittableValueType.StrideOf(m.Verticies) != size)
	throw new ApplicationException("Vertex data not uploaded correctly");
      GL.GenBuffers(1, out handle.EboID);
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, handle.EboID);
      GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(m.Indicies.Length * sizeof(short)), m.Indicies, BufferUsageHint.StaticDraw);
      GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
      if (m.Indicies.Length * sizeof(short) != size)
	throw new ApplicationException("Element data not uploaded correctly");
      handle.NumElements = m.Indicies.Length;
    }

    public void Draw()
    {
      GL.EnableClientState(ArrayCap.ColorArray);
      GL.EnableClientState(ArrayCap.VertexArray);
      GL.EnableClientState(ArrayCap.TextureCoordArray);
      GL.BindBuffer(BufferTarget.ArrayBuffer, handle.VboID);
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, handle.EboID);
      GL.VertexPointer(3, VertexPointerType.Float, BlittableValueType.StrideOf(verticies), new IntPtr(0));
      GL.ColorPointer(4, ColorPointerType.UnsignedByte, BlittableValueType.StrideOf(verticies), new IntPtr(BlittableValueType.StrideOf(default(Vector3)) + BlittableValueType.StrideOf(default(Vector2))));
      GL.TexCoordPointer(4, TexCoordPointerType.Float, BlittableValueType.StrideOf(verticies), new IntPtr(BlittableValueType.StrideOf(default(Vector3))));
      GL.DrawElements(BeginMode.Triangles, handle.NumElements, DrawElementsType.UnsignedShort, IntPtr.Zero);
    }

    public void Free()
    {
      GL.DeleteBuffers(1, ref handle.VboID);
      GL.DeleteBuffers(1, ref handle.EboID);
    }
  }
}
