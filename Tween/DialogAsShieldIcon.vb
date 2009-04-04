Imports System.Windows.Forms

Public Class DialogAsShieldIcon
    Private shield As New ShieldIcon

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Hide()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Hide()
    End Sub

    Private Sub DialogAsShieldIcon_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        OK_Button.Image = shield.Icon
        PictureBox1.Image = System.Drawing.SystemIcons.Question.ToBitmap()
    End Sub

    Public Shadows Function Show(ByVal text As String, Optional ByVal caption As String = "DialogAsShieldIcon", _
                                 Optional ByVal Buttons As Windows.Forms.MessageBoxButtons = MessageBoxButtons.OKCancel, _
                                 Optional ByVal icon As Windows.Forms.MessageBoxIcon = MessageBoxIcon.Question _
                                ) As System.Windows.Forms.DialogResult
        Label1.Text = text
        Me.Text = caption
        Select Case Buttons
            Case MessageBoxButtons.OKCancel
                OK_Button.Text = "OK"
                Cancel_Button.Text = "キャンセル"
            Case MessageBoxButtons.YesNo
                OK_Button.Text = "はい"
                Cancel_Button.Text = "いいえ"
            Case Else
                OK_Button.Text = "OK"
                Cancel_Button.Text = "キャンセル"
        End Select
        ' とりあえずアイコンは処理しない（互換性のためパラメータだけ指定できる）

        MyBase.Show()
        Do While Me.DialogResult = Windows.Forms.DialogResult.None
            Application.DoEvents()
        Loop
        If Buttons = MessageBoxButtons.YesNo Then
            Select Case MyBase.DialogResult
                Case Windows.Forms.DialogResult.OK
                    Return Windows.Forms.DialogResult.Yes
                Case Windows.Forms.DialogResult.Cancel
                    Return Windows.Forms.DialogResult.No
            End Select
        Else
            Return MyBase.DialogResult
        End If
    End Function
End Class
