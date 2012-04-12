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
    public Vector3 Normal;
    public uint Color;

    public VertexPositionColorCoordNormal(float x, float y, float z, float u, float v, Color color, float nx, float ny, float nz)
    {
      Position = new Vector3(x, y, z);
      Color = MeshRenderer.ToRgba(color);
      TexCoord = new Vector2(u, v);
      Normal = new Vector3(nx, ny, nz);
    }
  }

  public struct Material
  {
    public Color Diffuse;
    public Color Emission;
    public Color Ambient;
    public Color Specular;
    public float Shininess;
  }

  public struct Mesh
  {
    public VertexPositionColorCoordNormal[] Verticies;
    public short[] Indicies;
    public BeginMode Mode;
    public Material Material;
  }

  public class MeshRenderer
  {
    static readonly IntPtr i_POSITION = new IntPtr(0);
    static readonly IntPtr i_TEXCOORD = new IntPtr(BlittableValueType.StrideOf(default(Vector3)));
    static readonly IntPtr i_NORMAL = new IntPtr(BlittableValueType.StrideOf(default(Vector3)) + BlittableValueType.StrideOf(default(Vector2)));
    static readonly IntPtr i_COLOR = new IntPtr(BlittableValueType.StrideOf(default(Vector3)) + BlittableValueType.StrideOf(default(Vector2)) + BlittableValueType.StrideOf(default(Vector3)));

    struct VBO { public int VboID, EboID, NumElements; }

    VertexPositionColorCoordNormal[] verticies;
    VBO handle;
    BeginMode mode;
    Material material;

    public MeshRenderer(Mesh m)
    {
      this.verticies = m.Verticies;
      this.material = m.Material;
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
      mode = m.Mode;
    }

    public void Draw()
    {
      GL.EnableClientState(ArrayCap.ColorArray);
      GL.EnableClientState(ArrayCap.VertexArray);
      GL.EnableClientState(ArrayCap.TextureCoordArray);
      GL.Material(MaterialFace.Front, MaterialParameter.Ambient, ColorToFloat(material.Ambient));
      GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, ColorToFloat(material.Diffuse));
      GL.Material(MaterialFace.Front, MaterialParameter.Specular, ColorToFloat(material.Specular));
      GL.Material(MaterialFace.Front, MaterialParameter.Emission, ColorToFloat(material.Emission));
      GL.BindBuffer(BufferTarget.ArrayBuffer, handle.VboID);
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, handle.EboID);
      GL.VertexPointer(3, VertexPointerType.Float, BlittableValueType.StrideOf(verticies), i_POSITION);
      GL.TexCoordPointer(2, TexCoordPointerType.Float, BlittableValueType.StrideOf(verticies), i_TEXCOORD);
      GL.NormalPointer(NormalPointerType.Float, BlittableValueType.StrideOf(verticies), i_NORMAL);
      GL.ColorPointer(4, ColorPointerType.UnsignedByte, BlittableValueType.StrideOf(verticies), i_COLOR);
      GL.DrawElements(mode, handle.NumElements, DrawElementsType.UnsignedShort, IntPtr.Zero);
      GL.DisableClientState(ArrayCap.ColorArray);
      GL.DisableClientState(ArrayCap.VertexArray);
      GL.DisableClientState(ArrayCap.TextureCoordArray);
    }

    public void Free()
    {
      GL.DeleteBuffers(1, ref handle.VboID);
      GL.DeleteBuffers(1, ref handle.EboID);
    }

    internal static float[] ColorToFloat(Color c)
    {
      return new float[] { c.R / byte.MaxValue, c.G / byte.MaxValue, c.B / byte.MaxValue, c.A / byte.MaxValue };
    }

    internal static uint ToRgba(Color color)
    {
      return (uint)color.A << 24 | (uint)color.B << 16 | (uint)color.G << 8 | (uint)color.R;
    }
  }
}
