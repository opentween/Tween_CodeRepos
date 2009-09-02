<Serializable()> _
Public Class SettingTab
    Inherits SettingBase(Of SettingTab)

#Region "Settingクラス基本"
    Public Shared Function Load(ByVal tabName As String) As SettingTab
        Dim setting As SettingTab = LoadSettings(tabName)
        setting.Tab.TabName = TabName
        Return setting
    End Function

    Public Sub Save()
        SaveSettings(Me, Me.Tab.TabName)
    End Sub

    Public Sub New()
        TAB = New TabClass
    End Sub

    Public Sub New(ByVal TabName As String)
        Me.Tab = New TabClass
        TAB.TabName = TabName
    End Sub

#End Region

    Public Shared Sub DeleteConfigFile()
        Try
            For Each FileName As String In System.IO.Directory.GetFiles( _
                           My.Application.Info.DirectoryPath, "SettingTab*.xml")

                'オプションはお好みで
                My.Computer.FileSystem.DeleteFile(FileName, FileIO.UIOption.OnlyErrorDialogs, _
                     FileIO.RecycleOption.DeletePermanently, FileIO.UICancelOption.DoNothing)

            Next
        Catch ex As Exception
            '削除権限がない場合
        End Try
    End Sub

    Public Tab As TabClass

End Class
