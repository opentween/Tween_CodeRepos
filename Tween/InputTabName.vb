Imports System.Windows.Forms

Public Class InputTabName

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        TextTabName.Text = ""
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public Property TabName() As String
        Get
            Return Me.TextTabName.Text.Trim()
        End Get
        Set(ByVal value As String)
            TextTabName.Text = value.Trim()
        End Set
    End Property

End Class
