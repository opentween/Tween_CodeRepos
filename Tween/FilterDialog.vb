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
        If ListTabs.Items.Count = 0 Then Exit Sub

        ListFilters.Items.Clear()
        ListFilters.Items.AddRange(_sts.Tabs(tabName).GetFilters())
        If ListFilters.Items.Count > 0 Then ListFilters.SelectedIndex = 0

        If _directAdd Then Exit Sub

        ListTabs.Enabled = True
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

        CheckManageRead.Checked = _sts.Tabs(tabName).UnreadManage
        CheckNotifyNew.Checked = _sts.Tabs(tabName).Notify

        Dim idx As Integer = ComboSound.Items.IndexOf(_sts.Tabs(tabName).SoundFile)
        If idx = -1 Then idx = 0
        ComboSound.SelectedIndex = idx
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
        ListTabs.Enabled = False
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
        'OptNone.Checked = True
        OptMark.Checked = True
        UID.Focus()
        _mode = EDITMODE.AddNew
        _directAdd = True
    End Sub

    Private Sub ButtonNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNew.Click
        ButtonNew.Enabled = False
        ButtonEdit.Enabled = False
        ButtonDelete.Enabled = False
        ButtonClose.Enabled = False
        EditFilterGroup.Enabled = True
        ListTabs.Enabled = False
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
        'OptNone.Checked = True
        OptMark.Checked = True
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
        ListTabs.Enabled = False
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

        _sts.Tabs(ListTabs.SelectedItem.ToString()).RemoveFilter(DirectCast(ListFilters.SelectedItem, FiltersClass))
        ListFilters.Items.RemoveAt(i)
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        ListTabs.Enabled = True
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
            Dim fc As FiltersClass = DirectCast(ListFilters.SelectedItem, FiltersClass)
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
            'If fc.moveFrom Then
            '    OptMove.Checked = True
            'ElseIf fc.SetMark Then
            '    OptMark.Checked = True
            'Else
            '    OptNone.Checked = True
            'End If
            If fc.MoveFrom Then
                OptMove.Checked = True
            Else
                OptMark.Checked = True
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
            'OptNone.Checked = True
            OptMark.Checked = True
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
            If Not CheckRegex.Checked Then MSG1.Text = MSG1.Text.Replace("　", " ").Trim()
            UID.Text = UID.Text.Trim()
            If UID.Text = "" AndAlso MSG1.Text = "" Then
                MessageBox.Show(My.Resources.ButtonOK_ClickText1, My.Resources.ButtonOK_ClickText2, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
            If CheckRegex.Checked Then
                If UID.Text <> "" Then
                    Try
                        Dim rgx As New System.Text.RegularExpressions.Regex(UID.Text)
                    Catch ex As Exception
                        MessageBox.Show(My.Resources.ButtonOK_ClickText3 + ex.Message, My.Resources.ButtonOK_ClickText2, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    End Try
                End If
                If MSG1.Text <> "" Then
                    Try
                        Dim rgx As New System.Text.RegularExpressions.Regex(MSG1.Text)
                    Catch ex As Exception
                        MessageBox.Show(My.Resources.ButtonOK_ClickText3 + ex.Message, My.Resources.ButtonOK_ClickText2, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    End Try
                End If
            End If
        Else
            If Not CheckRegex.Checked Then MSG2.Text = MSG2.Text.Replace("　", " ").Trim()
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

        ft = New FiltersClass()

        ft.MoveFrom = OptMove.Checked
        ft.SetMark = OptMark.Checked

        Dim bdy As String = ""
        If RadioAND.Checked Then
            ft.NameFilter = UID.Text
            ft.SearchBoth = True
            bdy = MSG1.Text
        Else
            ft.NameFilter = ""
            ft.SearchBoth = False
            bdy = MSG2.Text
        End If

        If CheckRegex.Checked Then
            ft.BodyFilter.Add(bdy)
        Else
            Dim bf() As String = bdy.Trim.Split(Chr(32))
            For Each bfs As String In bf
                If bfs <> "" Then ft.BodyFilter.Add(bfs.Trim)
            Next
        End If

        ft.UseRegex = CheckRegex.Checked
        ft.SearchUrl = CheckURL.Checked

        If _mode = EDITMODE.AddNew Then
            If Not _sts.Tabs(ListTabs.SelectedItem.ToString()).AddFilter(ft) Then
                MessageBox.Show(My.Resources.ButtonOK_ClickText4, My.Resources.ButtonOK_ClickText2, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Else
            _sts.Tabs(ListTabs.SelectedItem.ToString()).EditFilter(DirectCast(ListFilters.SelectedItem, FiltersClass), ft)
        End If

        SetFilters(ListTabs.SelectedItem.ToString)
        If _mode = EDITMODE.AddNew Then
            ListFilters.SelectedIndex = ListFilters.Items.Count - 1
        Else
            ListFilters.SelectedIndex = i
        End If
        _mode = EDITMODE.None


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
        ListTabs.Items.Clear()
        For Each key As String In _sts.Tabs.Keys
            If key <> DEFAULTTAB.RECENT AndAlso key <> DEFAULTTAB.DM AndAlso key <> DEFAULTTAB.FAV Then
                ListTabs.Items.Add(key)
            End If
        Next

        ComboSound.Items.Clear()
        ComboSound.Items.Add("")
        Dim oDir As IO.DirectoryInfo = New IO.DirectoryInfo(My.Application.Info.DirectoryPath)
        For Each oFile As IO.FileInfo In oDir.GetFiles("*.wav")
            ComboSound.Items.Add(oFile.Name)
        Next

        '選択タブ変更
        If ListTabs.Items.Count > 0 Then
            If _cur.Length > 0 Then
                For i As Integer = 0 To ListTabs.Items.Count - 1
                    If _cur = ListTabs.Items(i).ToString() Then
                        ListTabs.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub ListTabs_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListTabs.SelectedIndexChanged
        If ListTabs.SelectedIndex > -1 Then
            SetFilters(ListTabs.SelectedItem.ToString)
        Else
            ListTabs.Items.Clear()
        End If
    End Sub

    Private Sub ButtonAddTab_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAddTab.Click
        Dim tabName As String = Nothing
        Using inputName As New InputTabName()
            inputName.TabName = "MyTab" + (ListTabs.Items.Count + 1).ToString
            inputName.ShowDialog()
            tabName = inputName.TabName
        End Using
        If tabName <> "" Then
            If Not DirectCast(Me.Owner, TweenMain).AddNewTab(tabName, False) Then
                Dim tmp As String = String.Format(My.Resources.AddTabMenuItem_ClickText1, tabName)
                MessageBox.Show(tmp, My.Resources.AddTabMenuItem_ClickText2, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            Else
                '成功
                _sts.AddTab(tabName)
                ListTabs.Items.Add(tabName)
            End If
        End If
    End Sub

    Private Sub ButtonDeleteTab_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDeleteTab.Click
        If ListTabs.SelectedIndex > -1 AndAlso ListTabs.SelectedItem.ToString <> "" Then
            Dim tb As String = ListTabs.SelectedItem.ToString
            Dim idx As Integer = ListTabs.SelectedIndex
            If DirectCast(Me.Owner, TweenMain).RemoveSpecifiedTab(tb) Then
                ListTabs.Items.RemoveAt(idx)
                idx -= 1
                If idx < 0 Then idx = 0
                ListTabs.SelectedIndex = idx
            End If
        End If
    End Sub

    Private Sub ButtonRenameTab_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRenameTab.Click
        If ListTabs.SelectedIndex > -1 AndAlso ListTabs.SelectedItem.ToString <> "" Then
            Dim tb As String = ListTabs.SelectedItem.ToString
            Dim idx As Integer = ListTabs.SelectedIndex
            If DirectCast(Me.Owner, TweenMain).TabRename(tb) Then
                ListTabs.Items.RemoveAt(idx)
                ListTabs.Items.Insert(idx, tb)
                ListTabs.SelectedIndex = idx
            End If
        End If
    End Sub

    Private Sub CheckManageRead_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckManageRead.CheckedChanged

    End Sub
End Class
