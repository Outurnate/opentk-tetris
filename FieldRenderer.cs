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
  class FieldRenderer : IGameComponent
  {
    public struct Cell
    {
      public bool inUse;
      public Color color;

      public Cell(Color color)
      {
	inUse = false;
        this.color = color;
      }
    }

    Vector3 position;
    Vector3[] corners = new Vector3[]
    {
      new Vector3( .5f,  .5f, 0f),
      new Vector3(-.5f,  .5f, 0f),
      new Vector3(-.5f, -.5f, 0f),
      new Vector3( .5f, -.5f, 0f)
    };
    float[][] texCoords = new float[][]
    {
      new float[] { 0.0f, 0.0f },
      new float[] { 1.0f, 0.0f },
      new float[] { 1.0f, 1.0f },
      new float[] { 0.0f, 1.0f }
    };
    Cell[,] shownCells;
    Cell[,] committedCells;
    int width;
    int height;

    public Cell this[bool commit, int x, int y]
    {
      get
      {
	return commit ? committedCells[x, y] : shownCells[x, y];
      }
      set
      {
	if (commit)
          committedCells[x, y] = value;
        else
          shownCells[x, y] = value;
      }
    }

    public FieldRenderer(Vector3 position, int width, int height)
    {
      this.position = Vector3.Add(position, new Vector3(-(width / 2) + .5f, -(height / 2) + .5f, 0.0f));
      shownCells = new Cell[width, height];
      committedCells = new Cell[width, height];
      this.width = width;
      this.height = height;
    }

    public void Update(FrameEventArgs e) {}

    public void Draw(FrameEventArgs e)
    {
      for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
	{
          RenderCube(Vector3.Add(new Vector3((float)x, (float)y, 0), position));
	}
    }

    void RenderCube(Vector3 cubePos)
    {
      GL.BindTexture(TextureTarget.Texture2D, ResourceCommons.Cell);
      for (int i = 0; i < 4; i++)
      {
        GL.TexCoord2(texCoords[i]);
        GL.Vertex3(Vector3.Add(corners[i], cubePos));
      }
    }
  }
}