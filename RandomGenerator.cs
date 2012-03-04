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
    static Random random = new Random();
    int[] currentSequence;
    int c = 0;

    public RandomGenerator()
    {
      FillBag();
    }

    public ushort Generate()
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