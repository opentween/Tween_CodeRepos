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
                        If prevProcess IsNot Nothing Then
                            Dim rslt As Boolean = ClickTasktrayIcon()
                            If Not rslt Then
                                ' 警告を表示
                                MessageBox.Show(My.Resources.StartupText1, My.Resources.StartupText2, MessageBoxButtons.OK, MessageBoxIcon.Information)
                            End If
                        Else
                            ' 警告を表示
                            MessageBox.Show(My.Resources.StartupText1, My.Resources.StartupText2, MessageBoxButtons.OK, MessageBoxIcon.Information)
                            'MessageBox.Show("すでに起動しています。2つ同時には起動できません。", "多重起動禁止")
                        End If

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
#Region "タスクトレイアイコンのクリック"
        <DllImport("user32.dll")> _
        Private Shared Function _
            FindWindow(ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
        End Function
        <DllImport("user32.dll")> _
        Private Shared Function _
            FindWindowEx(ByVal hWnd1 As IntPtr, ByVal hWnd2 As IntPtr, ByVal lpsz1 As String, ByVal lpsz2 As String) As IntPtr
        End Function
        <DllImport("user32.dll")> _
        Private Shared Function _
            SendMessage(ByVal hwnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Integer
        End Function
        Const WM_USER As UInteger = &H400
        Const TB_GETBUTTON As Integer = WM_USER + 23
        Const TB_BUTTONCOUNT As UInteger = WM_USER + 24
        Const TB_GETBUTTONINFO As Integer = WM_USER + 65
        <StructLayout(LayoutKind.Sequential, Pack:=1)> _
        Public Structure TBBUTTON
            Public iBitmap As Integer
            Public idCommand As IntPtr
            Public fsState As Byte
            Public fsStyle As Byte
            Public bReserved0 As Byte
            Public bReserved1 As Byte
            Public dwData As Integer
            Public iString As Integer
        End Structure
        <StructLayout(LayoutKind.Sequential)> _
          Public Structure TBBUTTONINFO
            Public cbSize As Int32
            Public dwMask As Int32
            Public idCommand As Int32
            Public iImage As Int32
            Public fsState As Byte
            Public fsStyle As Byte
            Public cx As Short
            Public lParam As IntPtr
            Public pszText As IntPtr
            Public cchText As Int32
        End Structure
        <StructLayout(LayoutKind.Sequential)> _
          Public Structure TRAYNOTIFY
            Public hWnd As IntPtr
            Public uID As UInt32
            Public uCallbackMessage As UInt32
            Public dwDummy1 As UInt32
            Public dwDummy2 As UInt32
            Public hIcon As IntPtr
        End Structure
        <Flags()> _
        Public Enum ToolbarButtonMask As Int32
            TBIF_COMMAND = &H20
            TBIF_LPARAM = &H10
            TBIF_TEXT = &H2
        End Enum
        <DllImport("user32.dll", SetLastError:=True)> _
        Private Shared Function GetWindowThreadProcessId(ByVal hwnd As IntPtr, _
                                  ByRef lpdwProcessId As Integer) As Integer
        End Function
        <DllImport("kernel32.dll")> _
        Private Shared Function OpenProcess(ByVal dwDesiredAccess As ProcessAccess, <MarshalAs(UnmanagedType.Bool)> ByVal bInheritHandle As Boolean, ByVal dwProcessId As Integer) As IntPtr
        End Function
        <Flags()> _
        Public Enum ProcessAccess As Integer
            ''' <summary>Specifies all possible access flags for the process object.</summary>
            AllAccess = CreateThread Or DuplicateHandle Or QueryInformation Or SetInformation Or Terminate Or VMOperation Or VMRead Or VMWrite Or Synchronize
            ''' <summary>Enables usage of the process handle in the CreateRemoteThread function to create a thread in the process.</summary>
            CreateThread = &H2
            ''' <summary>Enables usage of the process handle as either the source or target process in the DuplicateHandle function to duplicate a handle.</summary>
            DuplicateHandle = &H40
            ''' <summary>Enables usage of the process handle in the GetExitCodeProcess and GetPriorityClass functions to read information from the process object.</summary>
            QueryInformation = &H400
            ''' <summary>Enables usage of the process handle in the SetPriorityClass function to set the priority class of the process.</summary>
            SetInformation = &H200
            ''' <summary>Enables usage of the process handle in the TerminateProcess function to terminate the process.</summary>
            Terminate = &H1
            ''' <summary>Enables usage of the process handle in the VirtualProtectEx and WriteProcessMemory functions to modify the virtual memory of the process.</summary>
            VMOperation = &H8
            ''' <summary>Enables usage of the process handle in the ReadProcessMemory function to' read from the virtual memory of the process.</summary>
            VMRead = &H10
            ''' <summary>Enables usage of the process handle in the WriteProcessMemory function to write to the virtual memory of the process.</summary>
            VMWrite = &H20
            ''' <summary>Enables usage of the process handle in any of the wait functions to wait for the process to terminate.</summary>
            Synchronize = &H100000
        End Enum
        <DllImport("kernel32.dll", SetLastError:=True, ExactSpelling:=True)> _
        Private Shared Function VirtualAllocEx(ByVal hProcess As IntPtr, ByVal lpAddress As IntPtr, _
             ByVal dwSize As Integer, ByVal flAllocationType As AllocationTypes, _
             ByVal flProtect As MemoryProtectionTypes) As IntPtr
        End Function
        <Flags()> _
        Public Enum AllocationTypes As UInteger
            Commit = &H1000
            Reserve = &H2000
            Decommit = &H4000
            Release = &H8000
            Reset = &H80000
            Physical = &H400000
            TopDown = &H100000
            WriteWatch = &H200000
            LargePages = &H20000000
        End Enum
        <Flags()> _
        Public Enum MemoryProtectionTypes As UInteger
            Execute = &H10
            ExecuteRead = &H20
            ExecuteReadWrite = &H40
            ExecuteWriteCopy = &H80
            NoAccess = &H1
            [ReadOnly] = &H2
            ReadWrite = &H4
            WriteCopy = &H8
            GuardModifierflag = &H100
            NoCacheModifierflag = &H200
            WriteCombineModifierflag = &H400
        End Enum
        Private Declare Function CloseHandle Lib "Kernel32.dll" ( _
                ByVal handle As IntPtr) As Boolean
        Private Declare Auto Function VirtualFreeEx Lib "Kernel32.dll" ( _
                ByVal process As IntPtr, ByVal address As IntPtr, _
                ByVal size As Integer, ByVal freeType As MemoryFreeTypes) As Boolean
        <Flags()> Private Enum MemoryFreeTypes
            Release = &H8000
        End Enum
        '指定したプロセスのメモリ領域にデータをコピーする
        Private Declare Auto Function WriteProcessMemory Lib "Kernel32.dll" ( _
            ByVal process As IntPtr, ByVal baseAddress As IntPtr, _
            ByRef buffer As TBBUTTONINFO, ByVal size As Integer, _
            ByRef writtenSize As Integer) As Boolean
        '指定したプロセスのメモリ領域のデータを呼び出し側プロセスのバッファにコピーする
        Private Declare Auto Function ReadProcessMemory Lib "Kernel32.dll" ( _
            ByVal process As IntPtr, ByVal baseAddress As IntPtr, _
            ByVal buffer As IntPtr, ByVal size As Integer, _
            ByRef readSize As Integer) As Boolean
        <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)> _
        Private Shared Function PostMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As UInt32, ByVal lParam As UInt32) As Boolean
        End Function
        Const WM_LBUTTONDOWN As Integer = &H201
        Const WM_LBUTTONUP As Integer = &H202

        Public Shared Function ClickTasktrayIcon() As Boolean
            Dim taskbarWin As IntPtr = FindWindow("Shell_TrayWnd", Nothing)
            If taskbarWin.Equals(IntPtr.Zero) Then Return False
            Dim trayWin As IntPtr = FindWindowEx(taskbarWin, IntPtr.Zero, "TrayNotifyWnd", Nothing)
            If trayWin.Equals(IntPtr.Zero) Then Return False
            Dim tempWin As IntPtr = FindWindowEx(trayWin, IntPtr.Zero, "SysPager", Nothing)
            If tempWin.Equals(IntPtr.Zero) Then tempWin = trayWin
            Dim toolWin As IntPtr = FindWindowEx(tempWin, IntPtr.Zero, "ToolbarWindow32", Nothing)
            If toolWin.Equals(IntPtr.Zero) Then Return False
            Dim expPid As Integer = 0
            GetWindowThreadProcessId(toolWin, expPid)
            Dim hProc As IntPtr = OpenProcess(ProcessAccess.VMOperation Or ProcessAccess.VMRead Or ProcessAccess.VMWrite, False, expPid)
            If hProc.Equals(IntPtr.Zero) Then Return False

            Try
                Dim tbButtonLocal As New TBBUTTON
                Dim ptbSysButton As IntPtr = VirtualAllocEx(hProc, IntPtr.Zero, Marshal.SizeOf(tbButtonLocal), AllocationTypes.Reserve Or AllocationTypes.Commit, MemoryProtectionTypes.ReadWrite)
                If ptbSysButton.Equals(IntPtr.Zero) Then Return False
                Try
                    Dim tbButtonInfoLocal As New TBBUTTONINFO
                    Dim ptbSysInfo As IntPtr = VirtualAllocEx(hProc, IntPtr.Zero, Marshal.SizeOf(tbButtonInfoLocal), AllocationTypes.Reserve Or AllocationTypes.Commit, MemoryProtectionTypes.ReadWrite)
                    If ptbSysInfo.Equals(IntPtr.Zero) Then Return False
                    Try
                        Const titleSize As Integer = 256
                        Dim title As String = ""
                        Dim pszTitle As IntPtr = Marshal.AllocCoTaskMem(titleSize)
                        Try
                            Dim pszSysTitle As IntPtr = VirtualAllocEx(hProc, IntPtr.Zero, titleSize, AllocationTypes.Reserve Or AllocationTypes.Commit, MemoryProtectionTypes.ReadWrite)
                            If pszSysTitle.Equals(IntPtr.Zero) Then Return False
                            Try
                                Dim iCount As Integer = SendMessage(toolWin, TB_BUTTONCOUNT, New IntPtr(0), New IntPtr(0))


                                For i As Integer = 0 To iCount - 1
                                    Dim dwBytes As Integer = 0
                                    Dim tbButtonLocal2 As TBBUTTON
                                    Dim tbButtonInfoLocal2 As TBBUTTONINFO
                                    Dim ptrLocal As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(tbButtonLocal))
                                    If ptrLocal.Equals(IntPtr.Zero) Then Return False
                                    Try
                                        Marshal.StructureToPtr(tbButtonLocal, ptrLocal, True)
                                        SendMessage(toolWin, TB_GETBUTTON, New IntPtr(i), ptbSysButton)
                                        ReadProcessMemory(hProc, ptbSysButton, ptrLocal, Marshal.SizeOf(tbButtonLocal), dwBytes)
                                        tbButtonLocal2 = DirectCast(Marshal.PtrToStructure(ptrLocal, GetType(TBBUTTON)), TBBUTTON)
                                    Finally
                                        Marshal.FreeCoTaskMem(ptrLocal)
                                    End Try


                                    tbButtonInfoLocal.cbSize = Marshal.SizeOf(tbButtonInfoLocal)
                                    tbButtonInfoLocal.dwMask = ToolbarButtonMask.TBIF_COMMAND Or ToolbarButtonMask.TBIF_LPARAM Or ToolbarButtonMask.TBIF_TEXT
                                    tbButtonInfoLocal.pszText = pszSysTitle
                                    tbButtonInfoLocal.cchText = titleSize
                                    WriteProcessMemory(hProc, ptbSysInfo, tbButtonInfoLocal, Marshal.SizeOf(tbButtonInfoLocal), dwBytes)

                                    SendMessage(toolWin, TB_GETBUTTONINFO, tbButtonLocal2.idCommand, ptbSysInfo)
                                    Dim ptrInfo As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(tbButtonInfoLocal))
                                    If ptrInfo.Equals(IntPtr.Zero) Then Return False
                                    Try
                                        Marshal.StructureToPtr(tbButtonInfoLocal, ptrInfo, True)
                                        ReadProcessMemory(hProc, ptbSysInfo, ptrInfo, Marshal.SizeOf(tbButtonInfoLocal), dwBytes)
                                        tbButtonInfoLocal2 = DirectCast(Marshal.PtrToStructure(ptrInfo, GetType(TBBUTTONINFO)), TBBUTTONINFO)
                                    Finally
                                        Marshal.FreeCoTaskMem(ptrInfo)
                                    End Try
                                    ReadProcessMemory(hProc, pszSysTitle, pszTitle, titleSize, dwBytes)
                                    title = Marshal.PtrToStringAnsi(pszTitle, titleSize)

                                    If title.Contains("Tween") Then
                                        Dim tNotify As New TRAYNOTIFY
                                        Dim tNotify2 As TRAYNOTIFY
                                        Dim ptNotify As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(tNotify))
                                        If ptNotify.Equals(IntPtr.Zero) Then Return False
                                        Try
                                            Marshal.StructureToPtr(tNotify, ptNotify, True)
                                            ReadProcessMemory(hProc, tbButtonInfoLocal2.lParam, ptNotify, Marshal.SizeOf(tNotify), dwBytes)
                                            tNotify2 = DirectCast(Marshal.PtrToStructure(ptNotify, GetType(TRAYNOTIFY)), TRAYNOTIFY)
                                        Finally
                                            Marshal.FreeCoTaskMem(ptNotify)
                                        End Try
                                        SetForegroundWindow(tNotify2.hWnd)
                                        PostMessage(tNotify2.hWnd, tNotify2.uCallbackMessage, tNotify2.uID, WM_LBUTTONDOWN)
                                        PostMessage(tNotify2.hWnd, tNotify2.uCallbackMessage, tNotify2.uID, WM_LBUTTONUP)
                                        Return True
                                    End If
                                Next
                                Return False
                            Finally
                                VirtualFreeEx(hProc, pszSysTitle, titleSize, MemoryFreeTypes.Release)
                            End Try
                        Finally
                            Marshal.FreeCoTaskMem(pszTitle)
                        End Try
                    Finally
                        VirtualFreeEx(hProc, ptbSysInfo, Marshal.SizeOf(tbButtonInfoLocal), MemoryFreeTypes.Release)
                    End Try
                Finally
                    VirtualFreeEx(hProc, ptbSysButton, Marshal.SizeOf(tbButtonLocal), MemoryFreeTypes.Release)
                End Try
            Finally
                CloseHandle(hProc)
            End Try
        End Function
#End Region
    End Class

End Namespace

