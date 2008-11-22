' Tween - Client of Twitter
' Copyright © 2007-2008 kiri_feather (@kiri_feather) <kiri_feather@gmail.com>
'           © 2008      Moz (@syo68k) <http://iddy.jp/profile/moz/>
'           © 2008      takeshik (@takeshik) <http://www.takeshik.org/>
' All rights reserved.
' 
' This file is part of Tween.
' 
' This program is free software; you can redistribute it and/or modify it
' under the terms of the GNU General Public License as published by the Free
' Software Foundation; either version 3 of the License, or (at your option)
' any later version.
' 
' This program is distributed in the hope that it will be useful, but
' WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
' or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License
' for more details. 
' 
' You should have received a copy of the GNU General Public License along
' with this program. If not, see <http://www.gnu.org/licenses/>, or write to
' the Free Software Foundation, Inc., 51 Franklin Street - Fifth Floor,
' Boston, MA 02110-1301, USA.

Imports Tween.StorageDataSetTableAdapters
Imports System.Data.SQLite

Public Class Storage
    Private ReadOnly _connectionString As String = "data source=Tween.db"

    Public Sub CreateDataBase()
        Using connection As SQLiteConnection = New SQLiteConnection(Me._connectionString)
            Using Command As SQLiteCommand = connection.CreateCommand()
                connection.Open()

                ' 投稿テーブルの作成
                Command.CommandText = _
                    "CREATE TABLE IF NOT EXISTS Posts (" + _
                        "Id INTEGER NOT NULL PRIMARY KEY," + _
                        "Timestamp DATETIME NOT NULL," + _
                        "Name TEXT NOT NULL," + _
                        "ScreenName TEXT NOT NULL," + _
                        "ImageUri TEXT NOT NULL," + _
                        "Text TEXT NOT NULL," + _
                        "HyperText TEXT NOT NULL," + _
                        "Source TEXT NOT NULL," + _
                        "IsRead BIT NOT NULL," + _
                        "IsFavorited BIT NOT NULL," + _
                        "IsReply BIT NOT NULL," + _
                        "IsProtected BIT NOT NULL," + _
                        "IsOneWayLove BIT NOT NULL," + _
                        "Tags TEXT NOT NULL" + _
                    ")"
                Command.ExecuteNonQuery()

                ' 返信先マップテーブルの作成
                Command.CommandText = _
                    "CREATE TABLE IF NOT EXISTS ReplyMap (" + _
                        "PostId INTEGER NOT NULL," + _
                        "InReplyToUser TEXT NOT NULL," + _
                        "InReplyToId INTEGER NOT NULL" + _
                    ")"
                Command.ExecuteNonQuery()

                ' アイコンテーブルの作成
                Command.CommandText = _
                    "CREATE TABLE IF NOT EXISTS Icons (" + _
                        "ImageUri TEXT NOT NULL PRIMARY KEY," + _
                        "Width INT NOT NULL," + _
                        "Height INT NOT NULL," + _
                        "Image BLOB NOT NULL" + _
                    ")"
                Command.ExecuteNonQuery()
            End Using
        End Using
    End Sub
End Class