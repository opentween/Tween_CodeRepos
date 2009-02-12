Namespace TweenCustomControl

    Public NotInheritable Class ToolStripLabelHistory
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
                Dim his As String = ""
                For Each hstr As String In sList
                    If his <> "" Then his += System.Environment.NewLine
                    his += hstr
                Next
                Me.ToolTipText = his
                MyBase.Text = value
            End Set
        End Property
    End Class
End Namespace
