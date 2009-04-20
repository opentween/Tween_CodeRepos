' Tween - Client of Twitter
' Copyright (c) 2007-2009 kiri_feather (@kiri_feather) <kiri_feather@gmail.com>
'           (c) 2008-2009 Moz (@syo68k) <http://iddy.jp/profile/moz/>
'           (c) 2008-2009 takeshik (@takeshik) <http://www.takeshik.org/>
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
            Dim filename As String = System.IO.Path.Combine(Application.Info.DirectoryPath, "TweenConf.xml")
            If IO.File.Exists(filename) Then
                Try
                    Using config As New IO.StreamReader(filename)
                        Dim xmlDoc As New Xml.XmlDocument
                        xmlDoc.Load(config)
                        ChangeUICulture(xmlDoc.SelectSingleNode("/configuration/entry[@key='cultureCode']").SelectSingleNode("string").InnerText)
                    End Using
                Catch ex As Exception

                End Try
            End If

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

            'csc.exeタイムアウト回避
            Static done As Boolean = False
            Try
                If Not done Then
                    Dim prcInfo As System.Diagnostics.ProcessStartInfo = New System.Diagnostics.ProcessStartInfo("cmd.exe")
                    With prcInfo
                        .CreateNoWindow = True
                        .Arguments = "/C exit"
                        .WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
                    End With
                    System.Diagnostics.Process.Start(prcInfo)
                    done = True
                End If
            Catch ex As System.ComponentModel.Win32Exception
                'リソース不足で起動できない
            End Try
        End Sub

        Private Sub MyApplication_UnhandledException(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            If e.Exception.Message <> "A generic error occurred in GDI+." AndAlso _
               e.Exception.Message <> "GDI+ で汎用エラーが発生しました。" Then
                e.ExitApplication = ExceptionOut(e.Exception)
            End If
        End Sub
    End Class

End Namespace

