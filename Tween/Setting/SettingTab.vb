<Serializable()> _
Public Class SettingTab
    Inherits SettingBase(Of SettingTab)

    Public Notify As Boolean = True
    Public SoundFile As String = ""
    Public UnreadManage As Boolean = True
    Public Filters() As Filter
End Class

<Serializable()> _
Public Class Filter
    Public BodyFilter As List(Of String)
    Public MoveFrom As Boolean = False
    Public NameFilter As String = ""
    Public SearchBoth As Boolean = True
    Public SearchUrl As Boolean = False
    Public SetMark As Boolean = False
    Public UseRegex As Boolean = False
End Class
