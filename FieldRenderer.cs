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
  class FieldRenderer : IGameComponent
  {
    Vector3 position;
    Vector3[] corners = new Vector3[]
    {
      new Vector3(-.5f,  .5f, 3.5f),
      new Vector3( .5f,  .5f, 3.5f),
      new Vector3( .5f, -.5f, 3.5f),
      new Vector3(-.5f, -.5f, 3.5f)
    };
    float[][] texCoords = new float[][]
    {
      new float[] { 0.0f, 1.0f },
      new float[] { 1.0f, 1.0f },
      new float[] { 1.0f, 0.0f },
      new float[] { 0.0f, 0.0f }
    };

    public FieldRenderer(Vector3 position)
    {
      this.position = position;
    }

    public void Update(FrameEventArgs e)
    {
      
    }

    public void Draw(FrameEventArgs e)
    {
      RenderCube(new Vector3(-1f,0,0));
      RenderCube(new Vector3(1f,0,0));
      RenderCube(new Vector3(-1f,1f,0));
      RenderCube(new Vector3(1f,-1f,0));
    }

    void RenderCube(Vector3 cubePos)
    {
      for (int i = 0; i < 4; i++)
      {
        GL.TexCoord2(texCoords[i]);
        GL.Vertex3(Vector3.Add(corners[i], cubePos));
      }
    }
  }
}