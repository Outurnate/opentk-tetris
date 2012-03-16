using GameFramework;
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
  class FieldRenderer : GameComponent
  {
    public class Cell
    {
      public bool inUse = false;
      public TetraminoColor color = TetraminoColor.Cyan;
    }

    Vector3 position;
    Cell[,] shownCells;
    Cell[,] committedCells;
    int width;
    int height;
    Texture scoreUI;

    public Cell this[int x, int y, bool commit]
    {
      get
      {
	try
	{
	  if (commit)
	    return committedCells[x, y];
	  else
	    return shownCells[x, y];
	}
	catch (IndexOutOfRangeException)
	{
	  return default(Cell);
	}
      }
      set
      {
	try
	{
	  shownCells[x, y] = value;
	}
	catch (IndexOutOfRangeException e)
	{
	  if (value.inUse)
	    throw e;
	}
      }
    }

    public FieldRenderer(GameWindow window, Vector3 position, int width, int height) : base(window)
    {
      this.position = Vector3.Add(position, new Vector3(-(width / 2) + .5f, -(height / 2) + .5f, 0.0f));
      shownCells = new Cell[width, height];
      committedCells = new Cell[width, height];
      this.width = width;
      this.height = height;
      for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
	{
          committedCells[x, y] = new Cell();
	  shownCells[x, y] = new Cell();
	}
    }

    public void Clear()
    {
      for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
          shownCells[x, y].inUse = false;
    }

    public void CopyCommit()
    {
      for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
	  if (shownCells[x, y].inUse)
	  {
	    committedCells[x, y].inUse = shownCells[x, y].inUse;
	    committedCells[x, y].color = shownCells[x, y].color;
	  }
    }

    protected override void DoLoad()
    {
      ResourceCommons.LoadTexture(ResourceCommons.PanelBase, out scoreUI);
    }

    protected override void DoUpdate(FrameEventArgs e)
    {
    }

    protected override void DoDraw(FrameEventArgs e)
    {
      GL.BindTexture(TextureTarget.Texture2D, scoreUI);
      ResourceCommons.Panel.Draw();
      GL.BindTexture(TextureTarget.Texture2D, 0);
      ResourceCommons.Tetrion.Draw();
      for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
	{
          if (committedCells[x, y].inUse)
	  {
	    Vector3 point = Vector3.Add(new Vector3((float)x, (float)y, -1f), position);
            GL.Translate(point);
	    ResourceCommons.Blocks[committedCells[x, y].color].Draw();
	    GL.Translate(-point);
	  }
          if (shownCells[x, y].inUse)
	  {
            Vector3 point = Vector3.Add(new Vector3((float)x, (float)y, -1f), position);
            GL.Translate(point);
	    ResourceCommons.Blocks[shownCells[x, y].color].Draw();
            GL.Translate(-point);
	  }
	}
    }
  }
}