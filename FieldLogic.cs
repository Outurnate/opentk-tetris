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
  class FieldLogic : GameComponent
  {
    FieldRenderer field;
    TetraminoManager manager;
    Tetramino currentTetramino;

    double dropSpeed = 1;

    double dropTimer;

    bool prev_right = false;
    bool prev_left = false;

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
	type = TetraminoType.O,
	color = TetraminoColor.Cyan,
        rotation = TetraminoRotation.Up,
        x = 8,
        y = 18
      };
    }

    bool IsOnFieldXComponent(Tetramino tetramino)
    {
      bool[,] map = manager[currentTetramino];
      for(int x = 0; x < map.GetLength(0); x++)
        for(int y = 0; y < map.GetLength(1); y++)
	  if (!(tetramino.x + x >= 0 && tetramino.x + x <= 9 && map[x, y]))
	    return false;
      return true;
    }

    void TryMove(int x, int y)
    {
      Tetramino newTetramino = currentTetramino;
      newTetramino.x += x;
      newTetramino.y += y;
      if (IsOnFieldXComponent(newTetramino))
        currentTetramino = newTetramino;
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
      if (Window.Keyboard[Key.D] && !prev_right)
        TryMove(-1, 0);
      if (Window.Keyboard[Key.A] && !prev_left)
        TryMove(1, 0);
      bool[,] map = manager[currentTetramino];
      for(int x = 0; x < map.GetLength(0); x++)
        for(int y = 0; y < map.GetLength(1); y++)
	  field[false, currentTetramino.x + x, currentTetramino.y + y].inUse = map[x, y];
      prev_right = Window.Keyboard[Key.D];
      prev_left = Window.Keyboard[Key.A];
    }

    protected override void DoDraw(FrameEventArgs e) { }
  }
}