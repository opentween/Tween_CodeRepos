Imports System.Windows.Forms

Public Class FilterDialog

    Private _tabs As List(Of TabStructure)
    Private _mode As EDITMODE
    Private _directAdd As Boolean

    Private Enum EDITMODE
        AddNew
        Edit
        None
    End Enum

    Private Sub SetFilters(ByVal tabName As String)
        If ComboTabs.Items.Count = 0 Then Exit Sub

        'ComboTabs.SelectedIndex = 0
        'For i As Integer = 0 To ComboTabs.Items.Count - 1
        '    If ComboTabs.Items(i) = tabName Then
        '        ComboTabs.SelectedIndex = i
        '        Exit For
        '    End If
        'Next
        tabName = DirectCast(ComboTabs.SelectedItem, String)

        ListFilters.Items.Clear()
        Dim fs As New System.Text.StringBuilder(512)
        For Each ts As TabStructure In _tabs
            If ts.tabName = tabName Then
                For Each ft As FilterClass In ts.filters
                    fs.Length = 0
                    If ft.SearchBoth Then
                        If ft.IDFilter <> "" Then
                            fs.AppendFormat(My.Resources.SetFiltersText1, ft.IDFilter)
                        Else
                            fs.Append(My.Resources.SetFiltersText2)
                        End If
                    End If
                    If ft.BodyFilter.Count > 0 Then
                        fs.Append(My.Resources.SetFiltersText3)
                        For Each bf As String In ft.BodyFilter
                            fs.Append(bf)
                            fs.Append(" ")
                        Next
                        fs.Length -= 1
                        fs.Append(My.Resources.SetFiltersText4)
                    End If
                    fs.Append("(")
                    If ft.SearchBoth Then
                        fs.Append(My.Resources.SetFiltersText5)
                    Else
                        fs.Append(My.Resources.SetFiltersText6)
                    End If
                    If ft.UseRegex Then
                        fs.Append(My.Resources.SetFiltersText7)
                    End If
                    If ft.SearchURL Then
                        fs.Append(My.Resources.SetFiltersText8)
                    End If
                    If ft.moveFrom Then
                        fs.Append(My.Resources.SetFiltersText9)
                    ElseIf ft.SetMark Then
                        fs.Append(My.Resources.SetFiltersText10)
                    Else
                        fs.Append(My.Resources.SetFiltersText11)
                    End If
                    fs.Append(")")
                    ListFilters.Items.Add(fs.ToString())
                Next
                Exit For
            End If
        Next
        If ListFilters.Items.Count > 0 Then
            ListFilters.SelectedIndex = 0
        End If

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

    Public Property Tabs() As List(Of TabStructure)
        Get
            Return _tabs
        End Get
        Set(ByVal value As List(Of TabStructure))
            _directAdd = False
            _tabs = value
            ComboTabs.Items.Clear()
            Dim tnm As String = ""
            For Each ts As TabStructure In _tabs
                If ts.tabName <> "Recent" And ts.tabName <> "Reply" And ts.tabName <> "Direct" Then
                    If tnm = "" Then tnm = ts.tabName
                    ComboTabs.Items.Add(ts.tabName)
                End If
            Next
            Me.CurrentTab = tnm
        End Set
    End Property

    Public Property CurrentTab() As String
        Get
            Return DirectCast(ComboTabs.SelectedItem, String)
        End Get
        Set(ByVal value As String)
            For i As Integer = 0 To ComboTabs.Items.Count - 1
                If ComboTabs.Items(i) Is value Then
                    ComboTabs.SelectedIndex = i
                    Exit For
                End If
            Next
        End Set
    End Property

    Public Sub AddNewFilter(ByVal id As String, ByVal msg As String)
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
        For Each ts As TabStructure In _tabs
            If ts.tabName Is ComboTabs.SelectedItem Then
                ts.filters.RemoveAt(i)
                ts.modified = True
                Exit For
            End If
        Next
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
        For Each ts As TabStructure In _tabs
            If ts.tabName Is ComboTabs.SelectedItem Then
                If ListFilters.SelectedIndex > -1 Then
                    Dim fc As FilterClass = ts.filters(ListFilters.SelectedIndex)
                    If fc.SearchBoth Then
                        RadioAND.Checked = True
                        RadioPLUS.Checked = False
                        UID.Enabled = True
                        MSG1.Enabled = True
                        MSG2.Enabled = False
                        UID.Text = fc.IDFilter
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
                Exit For
            End If
        Next
    End Sub

    Private Sub RadioAND_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioAND.CheckedChanged
        Dim flg As Boolean = RadioAND.Checked
        UID.Enabled = flg
        MSG1.Enabled = flg
        MSG2.Enabled = Not flg
    End Sub

    Private Sub ButtonOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        If RadioAND.Checked Then
            MSG1.Text = MSG1.Text.Replace("　", " ")
            If UID.Text.Trim = "" And MSG1.Text.Trim = "" Then
                MessageBox.Show(My.Resources.ButtonOK_ClickText1, My.Resources.ButtonOK_ClickText2, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
            If CheckRegex.Checked And MSG1.Text <> "" Then
                Try
                    Dim rgx As New System.Text.RegularExpressions.Regex(MSG1.Text)
                Catch ex As Exception
                    MessageBox.Show(My.Resources.ButtonOK_ClickText3 + ex.Message, My.Resources.ButtonOK_ClickText2, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End Try
            End If
        Else
            MSG2.Text = MSG2.Text.Replace("　", " ")
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
        For Each ts As TabStructure In _tabs
            If ts.tabName Is ComboTabs.SelectedItem Then
                Dim ft As FilterClass
                Dim ftOrg As FilterClass = Nothing

                If _mode = EDITMODE.AddNew Then
                    ft = New FilterClass
                Else
                    ft = ts.filters(i)
                    ftOrg = New FilterClass
                    For Each bs As String In ts.filters(i).BodyFilter
                        ftOrg.BodyFilter.Add(bs)
                    Next
                    ftOrg.IDFilter = ft.IDFilter
                    ftOrg.moveFrom = ft.moveFrom
                    ftOrg.SearchBoth = ft.SearchBoth
                    ftOrg.SearchURL = ft.SearchURL
                    ftOrg.SetMark = ft.SetMark
                    ftOrg.UseRegex = ft.UseRegex
                    ft.BodyFilter.Clear()
                End If

                ft.moveFrom = OptMove.Checked
                ft.SetMark = OptMark.Checked

                If RadioAND.Checked Then
                    ft.IDFilter = UID.Text
                    ft.SearchBoth = True
                    Dim bf() As String = MSG1.Text.Trim.Split(Chr(32))
                    For Each bfs As String In bf
                        If bfs <> "" Then ft.BodyFilter.Add(bfs.Trim)
                    Next
                Else
                    ft.IDFilter = ""
                    ft.SearchBoth = False
                    Dim bf() As String = MSG2.Text.Trim.Split(Chr(32))
                    For Each bfs As String In bf
                        If bfs <> "" Then ft.BodyFilter.Add(bfs.Trim)
                    Next
                End If
                ft.UseRegex = CheckRegex.Checked
                ft.SearchURL = CheckURL.Checked

                If _mode = EDITMODE.AddNew Then
                    ts.filters.Add(ft)
                    ts.modified = True
                Else
                    If ts.modified = False Then
                        If ft.BodyFilter.Count = ftOrg.BodyFilter.Count Then
                            For cnt As Integer = 0 To ft.BodyFilter.Count - 1
                                If ft.BodyFilter(cnt) <> ftOrg.BodyFilter(cnt) Then
                                    ts.modified = True
                                    Exit For
                                End If
                            Next
                            If ts.modified = False Then
                                If ft.IDFilter <> ftOrg.IDFilter Or _
                                   ft.moveFrom <> ftOrg.moveFrom Or _
                                   ft.SearchBoth <> ftOrg.SearchBoth Or _
                                   ft.SearchURL <> ftOrg.SearchURL Or _
                                   ft.SetMark <> ftOrg.SetMark Or _
                                   ft.UseRegex <> ftOrg.UseRegex Then
                                    ts.modified = True
                                End If
                            End If
                        Else
                            ts.modified = True
                        End If
                    End If
                End If

                Exit For
            End If
        Next
        SetFilters(ComboTabs.SelectedItem.ToString)
        If _mode = EDITMODE.AddNew Then
            ListFilters.SelectedIndex = ListFilters.Items.Count - 1
        Else
            ListFilters.SelectedIndex = i
        End If
        _mode = EDITMODE.None
        ComboTabs.Enabled = True
        ListFilters.Enabled = True
        ButtonNew.Enabled = True
        ButtonEdit.Enabled = True
        ButtonDelete.Enabled = True
        ButtonClose.Enabled = True

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
End Class
