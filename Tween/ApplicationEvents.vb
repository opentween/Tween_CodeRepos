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

Option Strict On
Namespace My

    ' 次のイベントは MyApplication に対して利用できます:
    ' 
    ' Startup: アプリケーションが開始されたとき、スタートアップ フォームが作成される前に発生します。
    ' Shutdown: アプリケーション フォームがすべて閉じられた後に発生します。このイベントは、通常の終了以外の方法でアプリケーションが終了されたときには発生しません。
    ' UnhandledException: ハンドルされていない例外がアプリケーションで発生したときに発生するイベントです。
    ' StartupNextInstance: 単一インスタンス アプリケーションが起動され、それが既にアクティブであるときに発生します。 
    ' NetworkAvailabilityChanged: ネットワーク接続が接続されたとき、または切断されたときに発生します。
    Partial Friend Class MyApplication
        Private mt As System.Threading.Mutex

        Private Sub MyApplication_Shutdown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shutdown
            Try
                mt.ReleaseMutex()
                mt.Close()
            Catch ex As Exception

            End Try
        End Sub

        Private Sub MyApplication_Startup(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
            Dim filename As String = Application.Info.DirectoryPath + "\" + Application.Info.ProductName + ".exe.config"
            Using config As New IO.StreamReader(filename)
                Dim xmlDoc As New Xml.XmlDocument
                Try
                    xmlDoc.Load(config)
                    ChangeUICulture(xmlDoc.DocumentElement.Item("TwitterSetting").GetAttribute("culturecode"))
                Catch
                    '
                End Try
            End Using

            Dim pt As String = Application.Info.DirectoryPath.Replace("\", "/") + "/" + Application.Info.ProductName
            mt = New System.Threading.Mutex(False, pt)
            Try
                If mt.WaitOne(0, False) = False Then
                    MessageBox.Show(My.Resources.StartupText1, My.Resources.StartupText2, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    e.Cancel = True
                    Exit Sub
                End If
            Catch ex As Exception
            End Try

            GC.KeepAlive(mt)
        End Sub

        Private Sub MyApplication_UnhandledException(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            Dim now As DateTime = DateTime.Now
            Dim fileName As String = String.Format("Tween-{0:0000}{1:00}{2:00}-{3:00}{4:00}{5:00}.log", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second)

            Using writer As IO.StreamWriter = New IO.StreamWriter(fileName)
                writer.WriteLine(My.Resources.UnhandledExceptionText1, DateTime.Now.ToString())
                writer.WriteLine(My.Resources.UnhandledExceptionText2)
                writer.WriteLine(My.Resources.UnhandledExceptionText3)
                writer.WriteLine()
                writer.WriteLine(My.Resources.UnhandledExceptionText4)
                writer.WriteLine(My.Resources.UnhandledExceptionText5, Environment.OSVersion.VersionString)
                writer.WriteLine(My.Resources.UnhandledExceptionText6, Environment.Version.ToString())
                writer.WriteLine(My.Resources.UnhandledExceptionText7, Application.Info.Version.ToString())
                writer.WriteLine(My.Resources.UnhandledExceptionText8, e.Exception.GetType().FullName, e.Exception.Message)
                writer.WriteLine(e.Exception.StackTrace)
                writer.WriteLine()
            End Using

            If MessageBox.Show(String.Format(My.Resources.UnhandledExceptionText9, fileName, Environment.NewLine), _
                               My.Resources.UnhandledExceptionText10, MessageBoxButtons.OKCancel, MessageBoxIcon.Error) = DialogResult.OK _
            Then
                Diagnostics.Process.Start(fileName)
            End If

            e.ExitApplication = False
        End Sub
    End Class

End Namespace

