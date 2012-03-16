//|-----------------------------------------------------------------------|\\
//| ResourceCommons.cs                                                    |\\
//| Stores resources common to all components                             |\\
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
  static class ResourceCommons
  {
    const string RESOURCE_DIR = "assets";
    const string TEXTURES_DIR = "textures";
    const string MODELS_DIR   = "models";
    const string SHADERS_DIR  = "shaders";

    public static Texture Cell;
    public static Dictionary<TetraminoColor, MeshRenderer> Blocks = new Dictionary<TetraminoColor, MeshRenderer>();
    public static MeshRenderer Tetrion;
    public static MeshRenderer Panel;
    public static int Simple_Shader;
    public static Bitmap PanelBase;

    static int Simple_vs;
    static int Simple_fs;

    static XmlSerializer modelSerializer = new XmlSerializer(typeof(Mesh));

    public static void Load()
    {
      LoadTexture(Path.Combine(Path.Combine(Path.Combine(".", RESOURCE_DIR), TEXTURES_DIR), "cell.png"), out Cell);
      PanelBase = new Bitmap(Path.Combine(Path.Combine(Path.Combine(".", RESOURCE_DIR), TEXTURES_DIR), "panel.png"));
      using (Stream tmp = File.Open(Path.Combine(Path.Combine(Path.Combine(".", RESOURCE_DIR), MODELS_DIR), "blockCyan.xml"), FileMode.Open))
	Blocks.Add(TetraminoColor.Cyan, new MeshRenderer((Mesh)modelSerializer.Deserialize(tmp)));
      using (Stream tmp = File.Open(Path.Combine(Path.Combine(Path.Combine(".", RESOURCE_DIR), MODELS_DIR), "blockYellow.xml"), FileMode.Open))
	Blocks.Add(TetraminoColor.Yellow, new MeshRenderer((Mesh)modelSerializer.Deserialize(tmp)));
      using (Stream tmp = File.Open(Path.Combine(Path.Combine(Path.Combine(".", RESOURCE_DIR), MODELS_DIR), "blockPurple.xml"), FileMode.Open))
	Blocks.Add(TetraminoColor.Purple, new MeshRenderer((Mesh)modelSerializer.Deserialize(tmp)));
      using (Stream tmp = File.Open(Path.Combine(Path.Combine(Path.Combine(".", RESOURCE_DIR), MODELS_DIR), "blockGreen.xml"), FileMode.Open))
	Blocks.Add(TetraminoColor.Green, new MeshRenderer((Mesh)modelSerializer.Deserialize(tmp)));
      using (Stream tmp = File.Open(Path.Combine(Path.Combine(Path.Combine(".", RESOURCE_DIR), MODELS_DIR), "blockRed.xml"), FileMode.Open))
	Blocks.Add(TetraminoColor.Red, new MeshRenderer((Mesh)modelSerializer.Deserialize(tmp)));
      using (Stream tmp = File.Open(Path.Combine(Path.Combine(Path.Combine(".", RESOURCE_DIR), MODELS_DIR), "blockBlue.xml"), FileMode.Open))
	Blocks.Add(TetraminoColor.Blue, new MeshRenderer((Mesh)modelSerializer.Deserialize(tmp)));
      using (Stream tmp = File.Open(Path.Combine(Path.Combine(Path.Combine(".", RESOURCE_DIR), MODELS_DIR), "blockOrange.xml"), FileMode.Open))
	Blocks.Add(TetraminoColor.Orange, new MeshRenderer((Mesh)modelSerializer.Deserialize(tmp)));
      using (Stream tmp = File.Open(Path.Combine(Path.Combine(Path.Combine(".", RESOURCE_DIR), MODELS_DIR), "tetrion.xml"), FileMode.Open))
	Tetrion = new MeshRenderer((Mesh)modelSerializer.Deserialize(tmp));
      using (Stream tmp = File.Open(Path.Combine(Path.Combine(Path.Combine(".", RESOURCE_DIR), MODELS_DIR), "panel.xml"), FileMode.Open))
	Panel = new MeshRenderer((Mesh)modelSerializer.Deserialize(tmp));
      using (StreamReader vs = new StreamReader(Path.Combine(Path.Combine(Path.Combine(".", RESOURCE_DIR), SHADERS_DIR), "vs_simple.glsl")))
	using (StreamReader fs = new StreamReader(Path.Combine(Path.Combine(Path.Combine(".", RESOURCE_DIR), SHADERS_DIR), "fs_simple.glsl")))
	  LoadShader(vs.ReadToEnd(), fs.ReadToEnd(), out Simple_vs, out Simple_fs, out Simple_Shader);
    }

    public static void Unload()
    {
      GL.DeleteTextures(1, ref Cell);
      foreach (KeyValuePair<TetraminoColor, MeshRenderer> block in Blocks)
	block.Value.Free();
      Panel.Free();
      Tetrion.Free(); // RELEASE THE TETRION!! (sorry, no kraken)
      GL.DeleteShader(Simple_vs);
      GL.DeleteShader(Simple_fs);
      GL.DeleteProgram(Simple_Shader);
    }

    public static void LoadShader(string vs, string fs, out int vertexObject, out int fragmentObject, out int program)
    {
      int status_code;
      string info;
      vertexObject = GL.CreateShader(ShaderType.VertexShader);
      fragmentObject = GL.CreateShader(ShaderType.FragmentShader);
      GL.ShaderSource(vertexObject, vs);
      GL.CompileShader(vertexObject);
      GL.GetShaderInfoLog(vertexObject, out info);
      GL.GetShader(vertexObject, ShaderParameter.CompileStatus, out status_code);
      if (status_code != 1)
	throw new ApplicationException(info);
      GL.ShaderSource(fragmentObject, fs);
      GL.CompileShader(fragmentObject);
      GL.GetShaderInfoLog(fragmentObject, out info);
      GL.GetShader(fragmentObject, ShaderParameter.CompileStatus, out status_code);
      if (status_code != 1)
	throw new ApplicationException(info);
      program = GL.CreateProgram();
      GL.AttachShader(program, fragmentObject);
      GL.AttachShader(program, vertexObject);
      GL.LinkProgram(program);
      //GL.UseProgram(program);
    }

    public static void LoadTexture(string filename, out Texture id)
    {
      if (String.IsNullOrEmpty(filename))
        throw new ArgumentException(filename);
      LoadTexture(new Bitmap(filename), out id);
    }

    public static void LoadTexture(Bitmap bmp, out Texture id)
    {
      id = GL.GenTexture();
      GL.BindTexture(TextureTarget.Texture2D, id);
      BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
      GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0, GLPixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);
      bmp.UnlockBits(bmp_data);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
    }
  }
}
