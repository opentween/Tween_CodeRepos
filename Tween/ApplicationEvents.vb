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
Imports System.Threading
Imports System.Runtime.InteropServices
Imports System.Diagnostics
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

            InitCulture()

            Dim pt As String = Application.Info.DirectoryPath.Replace("\", "/") + "/" + Application.Info.ProductName
            mt = New System.Threading.Mutex(False, pt)
            Try
                If Not mt.WaitOne(0, False) Then
                    ' 実行中の同じアプリケーションのウィンドウ・ハンドルの取得
                    Dim prevProcess As Process = GetPreviousProcess()
                    If prevProcess IsNot Nothing AndAlso _
                        IntPtr.op_Inequality(prevProcess.MainWindowHandle, IntPtr.Zero) Then
                        ' 起動中のアプリケーションを最前面に表示
                        WakeupWindow(prevProcess.MainWindowHandle)
                    Else
                        ' 警告を表示
                        MessageBox.Show("prevProcess is nothing:" + (prevProcess Is Nothing).ToString)
                        MessageBox.Show(My.Resources.StartupText1, My.Resources.StartupText2, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        'MessageBox.Show("すでに起動しています。2つ同時には起動できません。", "多重起動禁止")
                    End If

                    e.Cancel = True
                    Exit Sub
                End If
            Catch ex As Exception
            End Try

            GC.KeepAlive(mt)

        End Sub

        Private Sub MyApplication_UnhandledException(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            If e.Exception.Message <> "A generic error occurred in GDI+." AndAlso _
               e.Exception.Message <> "GDI+ で汎用エラーが発生しました。" Then
                e.ExitApplication = ExceptionOut(e.Exception)
            End If
        End Sub

        Public ReadOnly Property CultureCode() As String
            Get
                Static _ccode As String = Nothing
                If _ccode Is Nothing Then
                    Dim filename As String = System.IO.Path.Combine(Application.Info.DirectoryPath, "TweenConf.xml")
                    If IO.File.Exists(filename) Then
                        Try
                            Using config As New IO.StreamReader(filename)
                                Dim xmlDoc As New Xml.XmlDocument
                                xmlDoc.Load(config)
                                Dim ns As New Xml.XmlNamespaceManager(xmlDoc.NameTable)
                                ns.AddNamespace("conf", "urn:XSpect.Configuration.XmlConfiguration")
                                _ccode = xmlDoc.SelectSingleNode("//conf:configuration/entry[@key='cultureCode']", ns).SelectSingleNode("string").InnerText
                            End Using
                        Catch ex As Exception

                        End Try

                    End If
                End If
                Return _ccode
            End Get
        End Property

        Public Overloads Sub InitCulture(ByVal code As String)
            Try
                ChangeUICulture(code)
            Catch ex As Exception

            End Try
        End Sub
        Public Overloads Sub InitCulture()
            Try
                ChangeUICulture(Me.CultureCode)
            Catch ex As Exception

            End Try
        End Sub

#Region "先行起動プロセスをアクティブにする"
        ' 外部プロセスのウィンドウを起動する
        Public Shared Sub WakeupWindow(ByVal hWnd As IntPtr)
            ' メイン・ウィンドウが最小化されていれば元に戻す
            If IsIconic(hWnd) Then
                ShowWindowAsync(hWnd, SW_RESTORE)
            End If

            ' メイン・ウィンドウを最前面に表示する
            SetForegroundWindow(hWnd)
        End Sub

        ' 外部プロセスのメイン・ウィンドウを起動するためのWin32 API
        <DllImport("user32.dll")> _
        Private Shared Function _
            SetForegroundWindow(ByVal hWnd As IntPtr) As Boolean
        End Function
        <DllImport("user32.dll")> _
        Private Shared Function _
            ShowWindowAsync(ByVal hWnd As IntPtr, ByVal nCmdShow As Integer) As Boolean
        End Function
        <DllImport("user32.dll")> _
        Private Shared Function _
            IsIconic(ByVal hWnd As IntPtr) As Boolean
        End Function
        ' ShowWindowAsync関数のパラメータに渡す定義値
        Private Const SW_RESTORE As Integer = 9 ' 画面を元の大きさに戻す

        ' 実行中の同じアプリケーションのプロセスを取得する
        Public Shared Function GetPreviousProcess() As Process
            Dim curProcess As Process = Process.GetCurrentProcess()
            Dim allProcesses() As Process = Process.GetProcessesByName(curProcess.ProcessName)

            Dim checkProcess As Process
            For Each checkProcess In allProcesses
                ' 自分自身のプロセスIDは無視する
                If checkProcess.Id <> curProcess.Id Then
                    ' プロセスのフルパス名を比較して同じアプリケーションか検証
                    If String.Compare( _
                            checkProcess.MainModule.FileName, _
                            curProcess.MainModule.FileName, True) = 0 Then
                        ' 同じフルパス名のプロセスを取得
                        Return checkProcess
                    End If
                End If
            Next

            ' 同じアプリケーションのプロセスが見つからない！  
            Return Nothing
        End Function
#End Region
    End Class

End Namespace

