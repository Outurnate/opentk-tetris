//|-----------------------------------------------------------------------|\\
//| FieldRenderer.cs                                                      |\\
//| Renders tetraminos and related elements to the screen                 |\\
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

    const string UI_FORMAT = "000000";

    Vector3 position;
    Cell[,] shownCells;
    Cell[,] committedCells;
    int width;
    int height;
    Texture scoreUI;
    Bitmap scoreUIBase;
    Rectangle scoreRect = new Rectangle(134, 256, 144, 32);
    Rectangle levelRect = new Rectangle(134, 320, 144, 32);
    Rectangle linesRect = new Rectangle(134, 384, 144, 32);
    Point overlayPoint = new Point(64, 32);
    Font font = new Font("Sans", 24);
    StringFormat strFormat = new StringFormat()
    {
      Alignment = StringAlignment.Far,
      LineAlignment = StringAlignment.Center
    };

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

    public ulong Score = 0;

    public byte Level = 1;

    public ulong Lines = 0;

    public bool UpdateUI
    {
      get;
      set;
    }

    public TetraminoType NextTetramino
    {
      get;
      set;
    }

    public FieldRenderer(GameWindow window, Vector3 position, int width, int height) : base(window)
    {
      this.position = Vector3.Add(position, new Vector3(-(width / 2) + .5f, -(height / 2) + .5f, 0.0f));
      shownCells = new Cell[width, height];
      committedCells = new Cell[width, height];
      this.width = width;
      this.height = height;
      UpdateUI = true;
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
      ResourceCommons.LoadTexture(scoreUIBase = ResourceCommons.PanelBase, out scoreUI);
    }

    protected override void DoUnLoad()
    {
      GL.DeleteTextures(1, ref scoreUI);
    }

    protected override void DoUpdate(FrameEventArgs e)
    {
      if (UpdateUI)
      {
	Bitmap current = (Bitmap)scoreUIBase.Clone();
	Graphics g = Graphics.FromImage(current);
	g.DrawString(Score.ToString(UI_FORMAT), font, Brushes.White, scoreRect, strFormat);
	g.DrawString(Level.ToString(UI_FORMAT), font, Brushes.White, levelRect, strFormat);
	g.DrawString(Lines.ToString(UI_FORMAT), font, Brushes.White, linesRect, strFormat);
	g.DrawImage(ResourceCommons.BlockOverlays[NextTetramino], overlayPoint);
	ResourceCommons.UpdateTexture(current, scoreUI, 0, 0, current.Width, current.Height);
	UpdateUI = false;
      }
    }

    protected override void DoDraw(FrameEventArgs e)
    {
      GL.BindTexture(TextureTarget.Texture2D, scoreUI);
      ResourceCommons.Panel.Draw();
      GL.BindTexture(TextureTarget.Texture2D, ResourceCommons.TetrionTexture);
      ResourceCommons.Tetrion.Draw();
      GL.BindTexture(TextureTarget.Texture2D, ResourceCommons.Block);
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
