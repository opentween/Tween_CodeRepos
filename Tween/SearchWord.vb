Option Strict On
Imports System.Windows.Forms

Public Class SearchWord

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public Property SWord() As String
        Get
            Return SWordText.Text
        End Get
        Set(ByVal value As String)
            SWordText.Text = value
        End Set
    End Property

    Private Sub SearchWord_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        SWordText.SelectAll()
        SWordText.Focus()
    End Sub
End Class
