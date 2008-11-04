Imports System.Windows.Forms

Public Class OpenURL

    Private _selUrl As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If UrlList.SelectedItems.Count = 0 Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Else
            _selUrl = UrlList.SelectedItem.ToString
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
        End If
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public Sub ClearUrl()
        UrlList.Items.Clear()
    End Sub

    Public Sub AddUrl(ByVal strUrl As String)
        UrlList.Items.Add(strUrl)
    End Sub

    Public ReadOnly Property SelectedUrl() As String
        Get
            If UrlList.SelectedItems.Count = 1 Then
                Return _selUrl
            Else
                Return ""
            End If
        End Get
    End Property

    Private Sub OpenURL_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        UrlList.Focus()
        If UrlList.Items.Count > 0 Then
            UrlList.SelectedIndex = 0
        End If
    End Sub

    Private Sub UrlList_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UrlList.DoubleClick
        If UrlList.SelectedItem Is Nothing Then
            Exit Sub
        End If

        If UrlList.IndexFromPoint(UrlList.PointToClient(Control.MousePosition)) = ListBox.NoMatches Then
            Exit Sub
        End If

        If UrlList.Items(UrlList.IndexFromPoint(UrlList.PointToClient(Control.MousePosition))) Is Nothing Then
            Exit Sub
        End If
        Call OK_Button_Click(sender, e)
    End Sub
End Class
