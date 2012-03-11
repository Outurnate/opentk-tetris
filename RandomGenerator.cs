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
  class RandomGenerator
  {
    public struct TetraminoData
    {
      public ushort newTetramino;
      public ushort nextTetramino;
    }

    static Random random = new Random();
    int[] currentSequence;
    int c = 0;
    Queue<ushort> next = new Queue<ushort>();

    public RandomGenerator()
    {
      FillBag();
      for (int i = 0; i < 2; i++)
	next.Enqueue(GenerateU());
    }

    public TetraminoData Generate()
    {
      TetraminoData n = new TetraminoData()
      {
        newTetramino = next.Dequeue(),
	nextTetramino = next.Peek()
      };
      next.Enqueue(GenerateU());
      return n;
    }

    ushort GenerateU()
    {
      int result = currentSequence[c++];
      if (c > 6)
      {
	c = 0;
	FillBag();
      }
      return (ushort)result;
    }

    void FillBag()
    {
      currentSequence = Enumerable.Range(0, 7).OrderBy(n => random.Next()).ToArray();
    }
  }
}