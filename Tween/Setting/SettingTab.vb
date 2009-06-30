<Serializable()> _
Public Class SettingTab
    Inherits SettingBase(Of SettingTab)

#Region "Settingクラス基本"
    Public Shared Function Load(ByVal TabName As String) As SettingTab
        Dim setting As SettingTab = LoadSettings(TabName)
        Return setting
    End Function

    Public Sub Save()
        SaveSettings(Me, Me.Tab.TabName)
    End Sub

    Public Sub New()
        Tab = New TabClass
    End Sub

    Public Sub New(ByVal TabName As String)
        Me.Tab = New TabClass
        Tab.TabName = TabName
    End Sub

#End Region

    Public Shared Sub DeleteConfigFile()
        For Each FileName As String In System.IO.Directory.GetFiles( _
                       My.Application.Info.DirectoryPath, "SettingTab*.config")

            'オプションはお好みで
            My.Computer.FileSystem.DeleteFile(FileName, FileIO.UIOption.OnlyErrorDialogs, _
                 FileIO.RecycleOption.DeletePermanently, FileIO.UICancelOption.DoNothing)

        Next
    End Sub

    Public Tab As TabClass

End Class
