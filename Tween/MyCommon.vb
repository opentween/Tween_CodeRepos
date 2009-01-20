' Tween - Client of Twitter
' Copyright © 2007-2009 kiri_feather (@kiri_feather) <kiri_feather@gmail.com>
'           © 2008-2009 Moz (@syo68k) <http://iddy.jp/profile/moz/>
'           © 2008-2009 takeshik (@takeshik) <http://www.takeshik.org/>
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

Imports System.Text

Public Module MyCommon
    Public Enum IconSizes
        IconNone = 0
        Icon16 = 1
        Icon24 = 2
        Icon48 = 3
        Icon48_2 = 4
    End Enum

    Public Enum NameBalloonEnum
        None
        UserID
        NickName
    End Enum

    Public Enum DispTitleEnum
        None
        Ver
        Post
        UnreadRepCount
        UnreadAllCount
        UnreadAllRepCount
        UnreadCountAllCount
    End Enum

    Public Enum LogUnitEnum
        Minute
        Hour
        Day
    End Enum

    Public Enum ProxyTypeEnum
        None
        IE
        Specified
    End Enum

    Public Enum UrlConverter
        TinyUrl
        Isgd
    End Enum

    Public Enum OutputzUrlmode
        twittercom
        twittercomWithUsername
    End Enum

    Public Enum HITRESULT
        None
        Copy
        CopyAndMark
        Move
    End Enum

    Public follower As New Collections.Specialized.StringCollection
    Public cCon As New System.Net.CookieContainer()

    Public Const Block As Object = Nothing

    Public Sub TraceOut(ByVal Message As String)
        If My.Application.CommandLineArgs.Count = 0 OrElse My.Application.CommandLineArgs.Contains("/d") = False Then Exit Sub
        Dim now As DateTime = DateTime.Now
        Dim fileName As String = String.Format("TweenTrace-{0:0000}{1:00}{2:00}-{3:00}{4:00}{5:00}.log", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second)

        Using writer As IO.StreamWriter = New IO.StreamWriter(fileName)
            writer.WriteLine("**** TraceOut: {0} ****", DateTime.Now.ToString())
            writer.WriteLine(My.Resources.TraceOutText1)
            writer.WriteLine(My.Resources.TraceOutText2)
            writer.WriteLine()
            writer.WriteLine(My.Resources.TraceOutText3)
            writer.WriteLine(My.Resources.TraceOutText4, Environment.OSVersion.VersionString)
            writer.WriteLine(My.Resources.TraceOutText5, Environment.Version.ToString())
            writer.WriteLine(My.Resources.TraceOutText6, My.Application.Info.Version.ToString())
            writer.WriteLine(Message)
            writer.WriteLine()
        End Using
    End Sub

    ''' <summary>
    ''' URLに含まれているマルチバイト文字列を%xx形式でエンコードします。
    ''' <newpara>
    ''' マルチバイト文字のコードはUTF-8またはUnicodeで自動的に判断します。
    ''' </newpara>
    ''' </summary>
    ''' <param name = input>エンコード対象のURL</param>
    ''' <returns>マルチバイト文字の部分をUTF-8/%xx形式でエンコードした文字列を返します。</returns>

    Public Function urlEncodeMultibyteChar(ByVal input As String) As String
        Dim uri As Uri
        Dim sb As StringBuilder = New StringBuilder(256)
retry:
        For Each c As Char In input
            If Convert.ToInt32(c) > 255 Then
                uri = New Uri(input)
                input = uri.AbsoluteUri
                sb.Length = 0
                GoTo retry
            ElseIf Convert.ToInt32(c) > 127 Then
                sb.Append("%" + Convert.ToInt16(c).ToString("X2"))
            Else
                sb.Append(c)
            End If
        Next
        Return sb.ToString()
    End Function

    Public Sub MoveArrayItem(ByVal values() As Integer, ByVal idx_fr As Integer, ByVal idx_to As Integer)
        Dim moved_value As Integer = values(idx_fr)
        Dim num_moved As Integer = Math.Abs(idx_fr - idx_to)

        If idx_to < idx_fr Then
            Array.Copy(values, idx_to, values, _
                idx_to + 1, num_moved)
        Else
            Array.Copy(values, idx_fr + 1, values, _
                idx_fr, num_moved)
        End If

        values(idx_to) = moved_value
    End Sub

    Public _Outputz As Outputz
End Module
