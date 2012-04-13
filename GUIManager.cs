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
using System;
using System.Threading;

namespace Tetris
{
  class GUIManager
  {
    [Widget]
    Window mainWindow;
    [Widget]
    Button onePlayer;
    [Widget]
    Button twoPlayer;
    [Widget]
    Button threePlayer;
    [Widget]
    Button fourPlayer;

    [STAThread]
    static void Main(string[] args)
    {
      new GUIManager();
    }

    GUIManager()
    {
      Application.Init();
      Glade.XML gxml = new Glade.XML(null, "main.glade", "mainWindow", null);
      gxml.Autoconnect(this);

      onePlayer.Clicked   += onePlayerClick;
      twoPlayer.Clicked   += twoPlayerClick;
      threePlayer.Clicked += threePlayerClick;
      fourPlayer.Clicked  += fourPlayerClick;

      Application.Run();
    }

    void onePlayerClick(object o, EventArgs e)
    {
      StartGame(InterleavedFieldManager.NumPlayers.OnePlayer);
    }

    void twoPlayerClick(object o, EventArgs e)
    {
      StartGame(InterleavedFieldManager.NumPlayers.TwoPlayer);
    }

    void threePlayerClick(object o, EventArgs e)
    {
      StartGame(InterleavedFieldManager.NumPlayers.ThreePlayer);
    }

    void fourPlayerClick(object o, EventArgs e)
    {
      StartGame(InterleavedFieldManager.NumPlayers.FourPlayer);
    }

    void StartGame(InterleavedFieldManager.NumPlayers players)
    {
      mainWindow.HideAll();
      Thread gameThread = new Thread(new ThreadStart(delegate()
      {
	using (Game game = new Game(players))
	{
	  game.Run(30.0, 60.0);
	}
	mainWindow.ShowAll();
      }));
      gameThread.Start();
    }
  }
}