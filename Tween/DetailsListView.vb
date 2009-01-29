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

Imports System
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Windows.Forms

Namespace TweenCustomControl
    Friend Class Win32Api
        'Friend Declare Function SendMessage Lib "user32.dll" ( _
        '    ByVal window As IntPtr, ByVal msg As Integer, _
        '    ByVal itemIndex As Integer, ByRef bounds As Rect) As Integer
        Friend Declare Function ValidateRect Lib "user32.dll" ( _
            ByVal window As IntPtr, ByVal rect As IntPtr) As Boolean
    End Class


    Public Class DetailsListView

        Private changeBounds As Rectangle
        Private multiSelected As Boolean
        Private _handlers As New System.ComponentModel.EventHandlerList()

        Custom Event Scrolled As System.EventHandler
            AddHandler(ByVal value As System.EventHandler)
                Me._handlers.AddHandler("Scrolled", value)
            End AddHandler

            RemoveHandler(ByVal value As System.EventHandler)
                Me._handlers.RemoveHandler("Scrolled", value)
            End RemoveHandler

            RaiseEvent(ByVal sender As Object, ByVal e As System.EventArgs)
                Dim value As System.Delegate = Me._handlers("Scrolled")
                Dim handler As System.EventHandler = DirectCast(value, System.EventHandler)
                handler.Invoke(Me, e)
            End RaiseEvent
        End Event

        ' うまくいかない？
        'Protected Overrides ReadOnly Property ShowFocusCues() As Boolean
        '    Get
        '        Return False
        '    End Get
        'End Property

        Public Sub New()

            ' この呼び出しは、Windows フォーム デザイナで必要です。
            InitializeComponent()

            ' InitializeComponent() 呼び出しの後で初期化を追加します。
            View = Windows.Forms.View.Details
            FullRowSelect = True
            HideSelection = False
            DoubleBuffered = True
        End Sub


        Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
            Const WM_VSCROLL As Integer = &H115
            Const WM_MOUSEWHEEL As Integer = &H20A
            'Const WM_SETFOCUS As Integer = &H7

            If m.Msg = WM_VSCROLL OrElse m.Msg = WM_MOUSEWHEEL Then
                RaiseEvent Scrolled(Me, New System.EventArgs)
            End If
            'If m.Msg = WM_SETFOCUS Then
            '    Return
            'End If
            MyBase.WndProc(m)
        End Sub

    End Class
End Namespace
