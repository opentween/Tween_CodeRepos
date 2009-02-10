Public Class ToolStripLabelHistory
    Inherits ToolStripLabel

    Private sList As New List(Of String)
    Private Const MAXCNT As Integer = 10

    Public Overrides Property Text() As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            sList.Add(value)
            Do While sList.Count > MAXCNT
                sList.RemoveAt(0)
            Loop
            Dim hst As String = ""
            For Each sts As String In sList
                If hst <> "" Then hst += System.Environment.NewLine
                hst += sts
            Next
            Me.ToolTipText = hst
            MyBase.Text = value
        End Set
    End Property
End Class
