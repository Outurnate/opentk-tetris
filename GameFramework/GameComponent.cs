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

using OpenTK;

namespace GameFramework
{
  public abstract class GameComponent
  {
    public bool Visible
    {
      get;
      set;
    }

    public bool Enabled
    {
      get;
      set;
    }

    public GameWindow Window
    {
      get;
      private set;
    }

    public GameComponent(GameWindow window)
    {
      this.Window = window;
    }

    public void Load()
    {
      DoLoad();
    }

    public void Update(FrameEventArgs e)
    {
      if (Enabled)
        DoUpdate(e);
    }

    public void Draw(FrameEventArgs e)
    {
      if (Visible)
        DoDraw(e);
    }

    protected abstract void DoLoad();

    protected abstract void DoUpdate(FrameEventArgs e);

    protected abstract void DoDraw(FrameEventArgs e);
  }
}
