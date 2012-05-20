//|-----------------------------------------------------------------------|\\
//| HighscoreManager.cs                                                   |\\
//| Interacts with and manages sqlite database for highscore              |\\
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
using System.IO;
using SQLite;

namespace Tetris
{
  public class Highscore
  {
    [PrimaryKey, AutoIncrement]
    public int Id
    {
      get;
      set;
    }

    [MaxLength(8)]
    public string Name
    {
      get;
      set;
    }

    public long Score
    {
      get;
      set;
    }
  }

  static class HighscoreManager
  {
    static SQLiteConnection db;
    const string scoreQuery = "SELECT * FROM Highscore ORDER BY Score DESC LIMIT 10";

    public static void Init()
    {
      db = new SQLiteConnection(Path.Combine(Path.Combine(ResourceCommons.START_DIR, ResourceCommons.RESOURCE_DIR), "highscores"));
      db.CreateTable<Highscore>();
    }

    public static void AddScore(string name, ulong score)
    {
      db.Insert(new Highscore()
      {
	Name = name,
	Score = (long)score
      });
    }

    public static IEnumerable<Highscore> ReadScores()
    {
      return db.Query<Highscore>(scoreQuery);
    }

    public static void Close()
    {
      db.Close();
      db.Dispose();
    }
  }
}