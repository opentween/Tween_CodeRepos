Imports System.Windows.Forms

Public Class TabsDialog

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub TabsDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If TabList.SelectedIndex = -1 Then TabList.SelectedIndex = 0
    End Sub

    Public Sub AddTab(ByVal tabName As String)
        For Each obj As String In TabList.Items
            If obj = tabName Then Exit Sub
        Next
        TabList.Items.Add(tabName)
    End Sub

    Public Sub RemoveTab(ByVal tabName As String)
        For Each obj As String In TabList.Items
            If obj = tabName Then
                TabList.Items.Remove(obj)
                Exit Sub
            End If
        Next
    End Sub

    Public ReadOnly Property SelectedTabName() As String
        Get
            If TabList.SelectedIndex = -1 Then
                Return ""
            Else
                Return CStr(TabList.SelectedItem)
            End If
        End Get
    End Property

    Private Sub TabList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabList.SelectedIndexChanged

    End Sub

    Private Sub TabList_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabList.DoubleClick
        If TabList.SelectedItem = Nothing Then
            Exit Sub
        End If

        If TabList.IndexFromPoint(TabList.PointToClient(Control.MousePosition)) = ListBox.NoMatches Then
            Exit Sub
        End If

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub TabsDialog_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        TabList.Focus()
    End Sub
End Class
