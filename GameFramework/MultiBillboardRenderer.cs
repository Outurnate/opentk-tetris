//|-----------------------------------------------------------------------|\\
//| MultiBillboardRenderer.cs                                             |\\
//| Renders multiple point sprites to the screen.                         |\\
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

using OpenTK;
using System.Collections.Generic;

namespace GameFramework
{
  public class MultiBillboardRenderer : GameComponent
  {
    public int Texture
    {
      get;
      set;
    }

    int vboID;

    public MultiBillboardRenderer(GameWindow window) : base (window) { }

    protected override void DoUnLoad() { }

    protected override void DoLoad()
    {
      if (this.Texture == null)
	throw new InvalidOperationException("Texture must be set before resources are loaded");
      int size;
      GL.GenBuffers(1, out vboID);
      GL.BindBuffer(BufferTarget.ArrayBuffer, handle.VboID);
      GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(verticies.Length * BlittableValueType.StrideOf(m.Verticies)), verticies, BufferUsageHint.StaticDraw);
      GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
      if (m.Verticies.Length * BlittableValueType.StrideOf(m.Verticies) != size)
	throw new ApplicationException("Vertex data not uploaded correctly");
    }

    protected override void DoUpdate(FrameEventArgs e) { }

    protected override void DoDraw(FrameEventArgs e) { }
  }
}
