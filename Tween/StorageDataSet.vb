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

Imports System.Data
Imports System.Data.SQLite

Namespace StorageDataSetTableAdapters
    Partial Class PostsTableAdapter
        Public Overridable Overloads Function FillBy(ByVal dataTable As StorageDataSet.PostsDataTable, ByVal query As String) As Integer
            Dim command As SQLiteCommand = New SQLiteCommand(query, Me.Connection)
            command.CommandType = CommandType.Text
            Me.Adapter.SelectCommand = command
            If Me.ClearBeforeFill Then dataTable.Clear()
            Return Me.Adapter.Fill(dataTable)
        End Function

        Public Overridable Overloads Function GetData(ByVal query As String) As StorageDataSet.PostsDataTable
            Dim command As SQLiteCommand = New SQLiteCommand(query, Me.Connection)
            command.CommandType = CommandType.Text
            Me.Adapter.SelectCommand = command
            Dim dataTable As StorageDataSet.PostsDataTable = New StorageDataSet.PostsDataTable()
            Me.Adapter.Fill(dataTable)
            Return dataTable
        End Function
    End Class

    Partial Class ReplyMapTableAdapter
        Public Overridable Overloads Function FillBy(ByVal dataTable As StorageDataSet.ReplyMapDataTable, ByVal query As String) As Integer
            Dim command As SQLiteCommand = New SQLiteCommand(query, Me.Connection)
            command.CommandType = CommandType.Text
            Me.Adapter.SelectCommand = command
            If Me.ClearBeforeFill Then dataTable.Clear()
            Return Me.Adapter.Fill(dataTable)
        End Function

        Public Overridable Overloads Function GetData(ByVal query As String) As StorageDataSet.ReplyMapDataTable
            Dim command As SQLiteCommand = New SQLiteCommand(query, Me.Connection)
            command.CommandType = CommandType.Text
            Me.Adapter.SelectCommand = command
            Dim dataTable As StorageDataSet.ReplyMapDataTable = New StorageDataSet.ReplyMapDataTable()
            Me.Adapter.Fill(dataTable)
            Return dataTable
        End Function
    End Class

    Partial Class IconsTableAdapter
        Public Overridable Overloads Function FillBy(ByVal dataTable As StorageDataSet.IconsDataTable, ByVal query As String) As Integer
            Dim command As SQLiteCommand = New SQLiteCommand(query, Me.Connection)
            command.CommandType = CommandType.Text
            Me.Adapter.SelectCommand = command
            If Me.ClearBeforeFill Then dataTable.Clear()
            Return Me.Adapter.Fill(dataTable)
        End Function

        Public Overridable Overloads Function GetData(ByVal query As String) As StorageDataSet.IconsDataTable
            Dim command As SQLiteCommand = New SQLiteCommand(query, Me.Connection)
            command.CommandType = CommandType.Text
            Me.Adapter.SelectCommand = command
            Dim dataTable As StorageDataSet.IconsDataTable = New StorageDataSet.IconsDataTable()
            Me.Adapter.Fill(dataTable)
            Return dataTable
        End Function
    End Class
End Namespace
