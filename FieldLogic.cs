//|-----------------------------------------------------------------------|\\
//| FieldLogic.cs                                                         |\\
//| Calculates tetramino movement and updates renderer                    |\\
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
    RandomGenerator rand;
    FieldRenderer field;
    TetraminoManager manager;
    CoordinatedInputManager inputManager;
    Tetramino currentTetramino;
    CoordinatedInputManager.PlayerNumber player;

    double dropSpeed = 1;

    double dropTimer;
    double lockTimer;

    bool prev_right = false;
    bool prev_left = false;
    bool prev_up = false;
    bool prev_down = false;

    bool deferredLock = false;

    public FieldLogic(GameWindow window, FieldRenderer field, CoordinatedInputManager inputManager, CoordinatedInputManager.PlayerNumber player) : base(window)
    {
      this.field = field;
      this.manager = new TetraminoManager();
      this.dropTimer = dropSpeed;
      this.rand = new RandomGenerator();
      this.inputManager = inputManager;
      this.player = player;
    }

    public void Start()
    {
      field.Score += ScoreFunction.Calculate(0, ref field.Level, ref field.Lines, ref dropSpeed);
      SpawnTetramino();
    }

    void SpawnTetramino()
    {
      RandomGenerator.TetraminoData d = rand.Generate();
      currentTetramino = manager.SpawnDictionary[(TetraminoType)d.newTetramino];
      field.NextTetramino = (TetraminoType)d.nextTetramino;
      field.UpdateUI = true;
    }

    bool IsOnFieldXComponent(ref Tetramino tetramino)
    {
      bool[,] map = manager[tetramino];
      for (int x = 0; x < map.GetLength(0); x++)
        for (int y = 0; y < map.GetLength(1); y++)
	  if (!(tetramino.x + x >= 0 && tetramino.x + x <= 9) && map[x, y])
	    return false;
      return true;
    }

    bool IsOnFieldYComponent(ref Tetramino tetramino)
    {
      bool[,] map = manager[tetramino];
      for (int x = 0; x < map.GetLength(0); x++)
	if (tetramino.x + x >= 0 && tetramino.x + x <= 9)
	{
	  FieldRenderer.Cell[] column = new FieldRenderer.Cell[20];
	  for (int y = 0; y < 20; y++)
	    column[y] = field[tetramino.x + x, y, true];
	  for (int y = 0; y < map.GetLength(1); y++)
	    if (!(tetramino.y + y >= 0 && tetramino.y + y <= 19 && !column[tetramino.y + y].inUse) && map[x, y])
	      return false;
	}
      return true;
    }

    bool VerifyNoOverlap(ref Tetramino tetramino)
    {
      bool[,] map = manager[tetramino];
      for (int x = 0; x < map.GetLength(0); x++)
	if (tetramino.x + x >= 0 && tetramino.x + x <= 9)
	  for (int y = 0; y < map.GetLength(1); y++)
	    if (field[tetramino.x + x, tetramino.y + y, true] != default(FieldRenderer.Cell))
	      if (field[tetramino.x + x, tetramino.y + y, true].inUse && map[x, y])
		return false;
      return true;
    }

    bool TryMove(int x, int y, bool rotate, ref Tetramino tetramino)
    {
      Tetramino newTetramino = tetramino;
      newTetramino.x += x;
      newTetramino.y += y;
      if (rotate)
	newTetramino.rotation = (TetraminoRotation)(((ushort)newTetramino.rotation + 1) % 4) - 0;
      if (IsOnFieldXComponent(ref newTetramino)) // prevents IsOnFieldYComponent from crashing
      {
	bool yCheck = IsOnFieldYComponent(ref newTetramino);
	if (!yCheck && y != 0)
	  if (!deferredLock)
	  {
	    lockTimer = dropSpeed / 2;
	    deferredLock = true;
	  }
	if (VerifyNoOverlap(ref newTetramino) && yCheck)
	{
	  tetramino = newTetramino;
	  return true;
	}
      }
      else if (rotate)
      {
	//TODO: Wallkick
      }
      return false;
    }

    void LockTetramino()
    {
      field.CopyCommit();
      SpawnTetramino();
      deferredLock = false;
    }

    void CalculateClears()
    {
      uint rows = 0;
      for (int y = 0; y < 20; y++)
      {
	bool clear = true;
	for (int x = 0; x < 10; x++)
	  if (!field[x, y, true].inUse)
	  {
	    clear = false;
	    break;
	  }
	if (clear)
	{
	  for (int x = 0; x < 10; x++)
	    field[x, y, true].inUse = false;
	  for (int ny = y; ny < 20; ny++)
	    for (int x = 0; x < 10; x++)
	      if (field[x, ny + 1, true] != default(FieldRenderer.Cell))
	      {
		field[x, ny, true].inUse = field[x, ny + 1, true].inUse;
		field[x, ny, true].color = field[x, ny + 1, true].color;
	      }
	  rows++;
	  y--;
	}
      }
      if (rows != 0)
	field.Score += ScoreFunction.Calculate(rows, ref field.Level, ref field.Lines, ref dropSpeed);
    }

    protected override void DoUnLoad() { }

    protected override void DoLoad() { }

    protected override void DoUpdate(FrameEventArgs e)
    {
      field.Clear();
      dropTimer -= e.Time;
      if (Window.Keyboard[Key.S])
	dropTimer = 0;
      if (Window.Keyboard[Key.S] && !prev_down)
	dropTimer = dropSpeed;
      if (dropTimer <= 0)
      {
	TryMove(0, -1, false, ref currentTetramino);
        dropTimer += dropSpeed;
      }
      if (Window.Keyboard[Key.W] && !prev_up)
	TryMove(0, 0, true, ref currentTetramino);
      if (Window.Keyboard[Key.D] && !prev_right)
        TryMove(-1, 0, false, ref currentTetramino);
      if (Window.Keyboard[Key.A] && !prev_left)
        TryMove(1, 0, false, ref currentTetramino);
      bool[,] map = manager[currentTetramino];
      for (int x = 0; x < map.GetLength(0); x++)
	if (currentTetramino.x + x >= 0 && currentTetramino.x + x <= 9)
	  for (int y = 0; y < map.GetLength(1); y++)
	    if (field[currentTetramino.x + x, currentTetramino.y + y, false] != default(FieldRenderer.Cell))
	    {
	      field[currentTetramino.x + x, currentTetramino.y + y, false].inUse = map[x, y];
	      field[currentTetramino.x + x, currentTetramino.y + y, false].color = currentTetramino.color;
	    }
      if (deferredLock)
      {
	lockTimer -= e.Time;
	if (lockTimer <= 0)
	  LockTetramino();
      }
      CalculateClears();
      prev_up = Window.Keyboard[Key.W];
      prev_left = Window.Keyboard[Key.A];
      prev_down = Window.Keyboard[Key.S];
      prev_right = Window.Keyboard[Key.D];
    }

    protected override void DoDraw(FrameEventArgs e) { }
  }
}
