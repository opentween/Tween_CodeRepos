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

Public Class FilterDialog

    Private _mode As EDITMODE
    Private _directAdd As Boolean
    Private _sts As TabInformations
    Private _cur As String

    Private Enum EDITMODE
        AddNew
        Edit
        None
    End Enum

    Private Sub SetFilters(ByVal tabName As String)
        If ComboTabs.Items.Count = 0 Then Exit Sub

        ListFilters.Items.Clear()
        For Each fc As FiltersClass In _sts.Tabs(tabName).Filters
            ListFilters.Items.Add(fc.Summary)
        Next
        If ListFilters.Items.Count > 0 Then ListFilters.SelectedIndex = 0

        If _directAdd Then Exit Sub

        ComboTabs.Enabled = True
        ListFilters.Enabled = True
        ListFilters.Focus()
        If ListFilters.SelectedIndex <> -1 Then
            ShowDetail()
        End If
        EditFilterGroup.Enabled = False
        ButtonNew.Enabled = True
        ButtonEdit.Enabled = True
        ButtonDelete.Enabled = True
        ButtonClose.Enabled = True
    End Sub

    Public Sub SetCurrent(ByVal TabName As String)
        _cur = TabName
    End Sub

    Public Sub AddNewFilter(ByVal id As String, ByVal msg As String)
        '元フォームから直接呼ばれる
        ButtonNew.Enabled = False
        ButtonEdit.Enabled = False
        ButtonDelete.Enabled = False
        ButtonClose.Enabled = False
        EditFilterGroup.Enabled = True
        ComboTabs.Enabled = False
        ListFilters.Enabled = False
        RadioAND.Checked = True
        RadioPLUS.Checked = False
        UID.Text = id
        UID.SelectAll()
        MSG1.Text = msg
        MSG1.SelectAll()
        MSG2.Text = id + msg
        MSG2.SelectAll()
        UID.Enabled = True
        MSG1.Enabled = True
        MSG2.Enabled = False
        CheckRegex.Checked = False
        CheckURL.Checked = False
        OptNone.Checked = True
        UID.Focus()
        _mode = EDITMODE.AddNew
        _directAdd = True
    End Sub

    'Public Sub AddNewIDFilter(ByVal id As String, ByVal TabText As String, ByVal move As Boolean, ByVal mark As Boolean)
    '    For Each ts As TabStructure In _tabs
    '        If ts.tabName = TabText Then
    '            Dim ft As FilterClass

    '            ft = New FilterClass

    '            ft.moveFrom = move
    '            ft.SetMark = mark

    '            ft.IDFilter = id
    '            ft.SearchBoth = True
    '            ft.UseRegex = False
    '            ft.SearchURL = False

    '            ts.filters.Add(ft)

    '            Exit For
    '        End If
    '    Next
    'End Sub

    Private Sub ButtonNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNew.Click
        ButtonNew.Enabled = False
        ButtonEdit.Enabled = False
        ButtonDelete.Enabled = False
        ButtonClose.Enabled = False
        EditFilterGroup.Enabled = True
        ComboTabs.Enabled = False
        ListFilters.Enabled = False
        RadioAND.Checked = True
        RadioPLUS.Checked = False
        UID.Text = ""
        MSG1.Text = ""
        MSG2.Text = ""
        UID.Enabled = True
        MSG1.Enabled = True
        MSG2.Enabled = False
        CheckRegex.Checked = False
        CheckURL.Checked = False
        OptNone.Checked = True
        UID.Focus()
        _mode = EDITMODE.AddNew
    End Sub

    Private Sub ButtonEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEdit.Click
        If ListFilters.SelectedIndex = -1 Then Exit Sub

        ButtonNew.Enabled = False
        ButtonEdit.Enabled = False
        ButtonDelete.Enabled = False
        ButtonClose.Enabled = False
        EditFilterGroup.Enabled = True
        ComboTabs.Enabled = False
        ListFilters.Enabled = False

        ShowDetail()
        _mode = EDITMODE.Edit
    End Sub

    Private Sub ButtonDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDelete.Click
        If ListFilters.SelectedIndex = -1 Then Exit Sub
        Dim tmp As String = String.Format(My.Resources.ButtonDelete_ClickText1, vbCrLf, ListFilters.SelectedItem.ToString)

        If MessageBox.Show(tmp, My.Resources.ButtonDelete_ClickText2, _
            MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Cancel Then Exit Sub

        Dim i As Integer = ListFilters.SelectedIndex

        ListFilters.Items.RemoveAt(i)
        _sts.Tabs(ComboTabs.SelectedItem.ToString()).Filters.RemoveAt(i)
        _sts.Tabs(ComboTabs.SelectedItem.ToString()).FilterModified = True
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        ComboTabs.Enabled = True
        ListFilters.Enabled = True
        ListFilters.Focus()
        If ListFilters.SelectedIndex <> -1 Then
            ShowDetail()
        End If
        EditFilterGroup.Enabled = False
        ButtonNew.Enabled = True
        ButtonEdit.Enabled = True
        ButtonDelete.Enabled = True
        ButtonClose.Enabled = True
        If _directAdd Then
            Me.Close()
        End If
    End Sub

    Private Sub ShowDetail()

        If ListFilters.SelectedIndex > -1 Then
            Dim fc As FiltersClass = _sts.Tabs(ComboTabs.SelectedItem.ToString()).Filters(ListFilters.SelectedIndex)
            If fc.SearchBoth Then
                RadioAND.Checked = True
                RadioPLUS.Checked = False
                UID.Enabled = True
                MSG1.Enabled = True
                MSG2.Enabled = False
                UID.Text = fc.NameFilter
                UID.SelectAll()
                MSG1.Text = ""
                MSG2.Text = ""
                For Each bf As String In fc.BodyFilter
                    MSG1.Text += bf + " "
                Next
                MSG1.Text = MSG1.Text.Trim
                MSG1.SelectAll()
            Else
                RadioPLUS.Checked = True
                RadioAND.Checked = False
                UID.Enabled = False
                MSG1.Enabled = False
                MSG2.Enabled = True
                UID.Text = ""
                MSG1.Text = ""
                MSG2.Text = ""
                For Each bf As String In fc.BodyFilter
                    MSG2.Text += bf + " "
                Next
                MSG2.Text = MSG2.Text.Trim
                MSG2.SelectAll()
            End If
            CheckRegex.Checked = fc.UseRegex
            CheckURL.Checked = fc.SearchURL
            If fc.moveFrom Then
                OptMove.Checked = True
            ElseIf fc.SetMark Then
                OptMark.Checked = True
            Else
                OptNone.Checked = True
            End If
        Else
            RadioAND.Checked = True
            RadioPLUS.Checked = False
            UID.Enabled = True
            MSG1.Enabled = True
            MSG2.Enabled = False
            UID.Text = ""
            MSG1.Text = ""
            MSG2.Text = ""
            CheckRegex.Checked = False
            CheckURL.Checked = False
            OptNone.Checked = True
        End If
    End Sub

    Private Sub RadioAND_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioAND.CheckedChanged
        Dim flg As Boolean = RadioAND.Checked
        UID.Enabled = flg
        MSG1.Enabled = flg
        MSG2.Enabled = Not flg
    End Sub

    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        'チェック
        If RadioAND.Checked Then
            MSG1.Text = MSG1.Text.Replace("　", " ").Trim()
            UID.Text = UID.Text.Trim()
            If UID.Text = "" AndAlso MSG1.Text = "" Then
                MessageBox.Show(My.Resources.ButtonOK_ClickText1, My.Resources.ButtonOK_ClickText2, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
            If CheckRegex.Checked AndAlso MSG1.Text <> "" Then
                Try
                    Dim rgx As New System.Text.RegularExpressions.Regex(MSG1.Text)
                Catch ex As Exception
                    MessageBox.Show(My.Resources.ButtonOK_ClickText3 + ex.Message, My.Resources.ButtonOK_ClickText2, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End Try
            End If
        Else
            MSG2.Text = MSG2.Text.Replace("　", " ").Trim()
            If MSG2.Text.Trim = "" Then
                MessageBox.Show(My.Resources.ButtonOK_ClickText1, My.Resources.ButtonOK_ClickText2, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
            If CheckRegex.Checked And MSG2.Text <> "" Then
                Try
                    Dim rgx As New System.Text.RegularExpressions.Regex(MSG2.Text)
                Catch ex As Exception
                    MessageBox.Show(My.Resources.ButtonOK_ClickText3 + ex.Message, My.Resources.ButtonOK_ClickText2, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End Try
            End If
        End If
        Dim i As Integer = ListFilters.SelectedIndex
        Dim ft As FiltersClass

        If _mode = EDITMODE.AddNew Then
            ft = New FiltersClass()
        Else
            ft = _sts.Tabs(ComboTabs.SelectedItem.ToString()).Filters(i)
            ft.BodyFilter.Clear()
        End If

        ft.moveFrom = OptMove.Checked
        ft.SetMark = OptMark.Checked

        If RadioAND.Checked Then
            ft.NameFilter = UID.Text
            ft.SearchBoth = True
            Dim bf() As String = MSG1.Text.Trim.Split(Chr(32))
            For Each bfs As String In bf
                If bfs <> "" Then ft.BodyFilter.Add(bfs.Trim)
            Next
        Else
            ft.NameFilter = ""
            ft.SearchBoth = False
            Dim bf() As String = MSG2.Text.Trim.Split(Chr(32))
            For Each bfs As String In bf
                If bfs <> "" Then ft.BodyFilter.Add(bfs.Trim)
            Next
        End If
        ft.UseRegex = CheckRegex.Checked
        ft.SearchURL = CheckURL.Checked

        If _mode = EDITMODE.AddNew Then
            _sts.Tabs(ComboTabs.SelectedItem.ToString()).Filters.Add(ft)
        End If

        SetFilters(ComboTabs.SelectedItem.ToString)
        If _mode = EDITMODE.AddNew Then
            ListFilters.SelectedIndex = ListFilters.Items.Count - 1
        Else
            ListFilters.SelectedIndex = i
        End If
        _mode = EDITMODE.None
        'ComboTabs.Enabled = True
        'ListFilters.Enabled = True
        'ButtonNew.Enabled = True
        'ButtonEdit.Enabled = True
        'ButtonDelete.Enabled = True
        'ButtonClose.Enabled = True

        _sts.Tabs(ComboTabs.SelectedItem.ToString()).FilterModified = True

        If _directAdd Then
            Me.Close()
        End If
    End Sub

    Private Sub ListFilters_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListFilters.SelectedIndexChanged
        ShowDetail()
    End Sub

    Private Sub ButtonClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonClose.Click
        Me.Close()
    End Sub

    Private Sub ComboTabs_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboTabs.SelectedIndexChanged
        SetFilters(ComboTabs.SelectedItem.ToString)
    End Sub

    Private Sub FilterDialog_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        _directAdd = False
    End Sub

    Private Sub FilterDialog_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            If EditFilterGroup.Enabled Then
                ButtonOK_Click(Nothing, Nothing)
            End If
        End If
        If e.KeyCode = Keys.Escape Then
            If EditFilterGroup.Enabled Then
                ButtonCancel_Click(Nothing, Nothing)
            Else
                ButtonClose_Click(Nothing, Nothing)
            End If
        End If
    End Sub

    Private Sub ListFilters_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListFilters.DoubleClick
        If ListFilters.SelectedItem Is Nothing Then
            Exit Sub
        End If

        If ListFilters.IndexFromPoint(ListFilters.PointToClient(Control.MousePosition)) = ListBox.NoMatches Then
            Exit Sub
        End If

        If ListFilters.Items(ListFilters.IndexFromPoint(ListFilters.PointToClient(Control.MousePosition))) Is Nothing Then
            Exit Sub
        End If
        ButtonEdit_Click(sender, e)
    End Sub

    Private Sub FilterDialog_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        _sts = TabInformations.GetInstance()
        ComboTabs.Items.Clear()
        For Each key As String In _sts.Tabs.Keys
            If key <> "Recent" AndAlso key <> "Reply" AndAlso key <> "Direct" Then
                ComboTabs.Items.Add(key)
            End If
        Next
        '選択タブ変更
        If ComboTabs.Items.Count > 0 Then
            If _cur.Length > 0 Then
                For i As Integer = 0 To ComboTabs.Items.Count - 1
                    If _cur = ComboTabs.Items(i).ToString() Then
                        ComboTabs.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If
        End If
    End Sub
End Class
