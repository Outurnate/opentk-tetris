//|-----------------------------------------------------------------------|\\
//| GameComponent.cs                                                      |\\
//| Manages a component that mush load resources, update and draw         |\\
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

using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;

namespace GameFramework
{
  public class GameComponentWindow : GameWindow
  {
    public List<GameComponent> Components
    {
      get;
      private set;
    }

    public GameComponentWindow(int width, int height, GraphicsMode mode, string title)
      : base(width, height, mode, title)
    {
      this.Components = new List<GameComponent>();
    }

    protected override void OnUnload(EventArgs e)
    {
      foreach(GameComponent g in this.Components)
	g.UnLoad();
    }

    protected override void OnLoad(EventArgs e)
    {
      foreach(GameComponent g in this.Components)
	g.Load();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      foreach(GameComponent g in this.Components)
	g.Update(e);
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      foreach(GameComponent g in this.Components)
	g.Draw(e);
    }
  }
}
