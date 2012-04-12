//|-----------------------------------------------------------------------|\\
//| GUIManager.cs                                                         |\\
//| Manages Glade# and GTK# components                                    |\\
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
using Gtk;
using Glade;

namespace Tetris
{
  class GUIManager
  {
    [STAThread]
    static void Main(string[] args)
    {
      new GUIManager();
    }

    GUIManager()
    {
      DoSetupGlade();
      DoShowMainScreen();
    }

    void DoGame()
    {
      using (Game game = new Game())
      {
        game.Run(30.0, 60.0);
      }
    }

    void DoSetupGlade()
    {
      Application.Init();
      Glade.XML gxml = new Glade.XML(null, "main.glade", "window1", null);
      gxml.Autoconnect(this);
      Application.Run();
    }

    void DoShowMainScreen()
    {
      
    }
  }
}