using GameFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;

using Texture = System.Int32;

namespace Tetris
{
  class FieldLogic : GameComponent
  {
    FieldRenderer field;
    TetraminoManager manager;
    Tetramino currentTetramino;

    double dropSpeed = .25;

    double dropTimer;

    bool prev_right = false;
    bool prev_left = false;
    bool prev_up = false;
    bool prev_down = false;

    bool deferredLock = false;

    public FieldLogic(GameWindow window, FieldRenderer field) : base(window)
    {
      this.field = field;
      this.manager = new TetraminoManager();
      dropTimer = dropSpeed;
    }

    public void Start()
    {
      SpawnTetramino();
    }

    void SpawnTetramino()
    {
      currentTetramino = new Tetramino()
      {
	type = (TetraminoType)new Random().Next(0, 6),
	color = TetraminoColor.Purple,
        rotation = TetraminoRotation.Up,
        x = 6,
        y = 16
      };
    }

    bool IsOnFieldXComponent(ref Tetramino tetramino)
    {
      bool[,] map = manager[currentTetramino];
      for (int x = 0; x < map.GetLength(0); x++)
        for (int y = 0; y < map.GetLength(1); y++)
	  if (!(tetramino.x + x >= 0 && tetramino.x + x <= 9) && map[x, y])
	    return false;
      return true;
    }

    bool IsOnFieldYComponent(ref Tetramino tetramino)
    {
      bool[,] map = manager[currentTetramino];
      for (int x = 0; x < map.GetLength(0); x++)
      {
	FieldRenderer.Cell[] column = new FieldRenderer.Cell[20];
        for (int y = 0; y < 20; y++)
	  column[y] = field[tetramino.x + x, y, true];
        for (int y = 0; y < map.GetLength(1); y++)
	  if (!(tetramino.y + y >= 1 && tetramino.y + y <= 19 && !column[tetramino.y + y].inUse) && map[x, y])
	    return false;
      }
      return true;
    }

    bool VerifyNoOverlap(ref Tetramino tetramino)
    {
      bool[,] map = manager[currentTetramino];
      for (int x = 0; x < map.GetLength(0); x++)
        for (int y = 0; y < map.GetLength(1); y++)
	  if (field[tetramino.x + x, tetramino.y + y, true].inUse && map[x, y])
	    return false;
      return true;
    }

    void TryMove(int x, int y)
    {
      Tetramino newTetramino = currentTetramino;
      newTetramino.x += x;
      newTetramino.y += y;
      if (IsOnFieldXComponent(ref newTetramino)) // prevents IsOnFieldYComponent from crashing
      {
	if (!IsOnFieldYComponent(ref newTetramino) && y != 0)
	  deferredLock = true;
	if (VerifyNoOverlap(ref newTetramino))
	  currentTetramino = newTetramino;
      }
      else
	Console.WriteLine("x");
    }

    void TryRotate()
    {
      Tetramino newTetramino = currentTetramino;
      newTetramino.rotation = (TetraminoRotation)(((ushort)newTetramino.rotation + 1) % 4) - 0;
      currentTetramino = newTetramino;
    }

    void LockTetramino()
    {
      field.CopyCommit();
      SpawnTetramino();
      deferredLock = false;
    }

    protected override void DoUpdate(FrameEventArgs e)
    {
      field.Clear();
      dropTimer -= e.Time;
      if (dropTimer <= 0)
      {
	TryMove(0, -1);
        dropTimer += dropSpeed;
      }
      if (Window.Keyboard[Key.W] && !prev_up)
	TryRotate();
      if (Window.Keyboard[Key.D] && !prev_right)
        TryMove(-1, 0);
      if (Window.Keyboard[Key.A] && !prev_left)
        TryMove(1, 0);
      bool[,] map = manager[currentTetramino];
      for(int x = 0; x < map.GetLength(0); x++)
        for(int y = 0; y < map.GetLength(1); y++)
	{
	  field[currentTetramino.x + x, currentTetramino.y + y, false].inUse = map[x, y];
	  field[currentTetramino.x + x, currentTetramino.y + y, false].color = manager.ColorDictionary[currentTetramino.color];
	}
      if (deferredLock)
	LockTetramino();
      prev_up = Window.Keyboard[Key.W];
      prev_left = Window.Keyboard[Key.A];
      prev_down = Window.Keyboard[Key.S];
      prev_right = Window.Keyboard[Key.D];
    }

    protected override void DoDraw(FrameEventArgs e) { }
  }
}