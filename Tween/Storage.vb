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

Imports System.Data.SQLite
Imports System.Text.RegularExpressions
Imports Tween.StorageDataSetTableAdapters

Public Class Storage
    Implements IDisposable

    Private ReadOnly _connectionString As String

    Private _postsTableAdapter As PostsTableAdapter

    Private _replyMapTableAdapter As ReplyMapTableAdapter

    Private _iconsTableAdapter As IconsTableAdapter

    Public ReadOnly Property ConnectionString() As String
        Get
            Return Me._connectionString
        End Get
    End Property

    Public ReadOnly Property Posts() As PostsTableAdapter
        Get
            Return Me._postsTableAdapter
        End Get
    End Property

    Public ReadOnly Property ReplyMap() As ReplyMapTableAdapter
        Get
            Return Me._replyMapTableAdapter
        End Get
    End Property

    Public ReadOnly Property Icons() As IconsTableAdapter
        Get
            Return Me._iconsTableAdapter
        End Get
    End Property

    Public Sub New(ByVal connectionString As String)
        Me._connectionString = connectionString
        Me._iconsTableAdapter = New IconsTableAdapter(Me._connectionString)
        Me._replyMapTableAdapter = New ReplyMapTableAdapter(Me._connectionString)
        Me._postsTableAdapter = New PostsTableAdapter(Me._connectionString)
    End Sub

    Public Sub CreateTables()
        Using connection As SQLiteConnection = New SQLiteConnection(Me._connectionString)
            Using Command As SQLiteCommand = connection.CreateCommand()
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

    Public Sub DropTables()
        Using connection As SQLiteConnection = New SQLiteConnection(Me._connectionString)
            Using Command As SQLiteCommand = connection.CreateCommand()
                ' 投稿テーブルの削除
                Command.CommandText = "DROP TABLE IF EXISTS Posts"
                Command.ExecuteNonQuery()

                ' 返信先マップテーブルの削除
                Command.CommandText = "DROP TABLE IF EXISTS ReplyMap"
                Command.ExecuteNonQuery()

                ' アイコンテーブルの削除
                Command.CommandText = "DROP TABLE IF EXISTS Icons"
                Command.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Sub Vacuum()
        Using connection As SQLiteConnection = New SQLiteConnection(Me._connectionString)
            Using Command As SQLiteCommand = connection.CreateCommand()
                Command.CommandText = "VACUUM"
                Command.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Sub Attach(ByVal name As String, ByVal path As String)
        Using connection As SQLiteConnection = New SQLiteConnection(Me._connectionString)
            Using Command As SQLiteCommand = connection.CreateCommand()
                Command.CommandText = String.Format("ATTACH DATABASE {0} AS {1}", path, name)
                Command.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Sub Detach(ByVal name As String)
        Using connection As SQLiteConnection = New SQLiteConnection(Me._connectionString)
            Using Command As SQLiteCommand = connection.CreateCommand()
                Command.CommandText = String.Format("DETACH DATABASE {0}", name)
                Command.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Overridable Sub Dispose() Implements IDisposable.Dispose
        Me._postsTableAdapter.Dispose()
        Me._replyMapTableAdapter.Dispose()
        Me._iconsTableAdapter.Dispose()
    End Sub
End Class

<SQLiteFunction(Arguments:=2, FuncType:=FunctionType.Scalar, Name:="REGEXP")> _
Public Class SQLiteRegexpFunction
    Inherits SQLiteFunction

    Public Overrides Function Invoke(ByVal args() As Object) As Object
        Return Regex.IsMatch(CType(args(1), String), CType(args(0), String))
    End Function
End Class
