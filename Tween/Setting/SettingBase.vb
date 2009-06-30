Public MustInherit Class SettingBase(Of T As {Class, New})

    Protected Shared Function LoadSettings(ByVal FileId As String) As T
        Try
            Using fs As New IO.FileStream(GetSettingFilePath(FileId), IO.FileMode.Open, IO.FileAccess.Read)
                Dim xs As New Xml.Serialization.XmlSerializer(GetType(T))
                Return DirectCast(xs.Deserialize(fs), T)
            End Using
        Catch ex As System.IO.FileNotFoundException
            Return New T()
        Catch ex As Exception
            Throw
        End Try
    End Function

    Protected Shared Function LoadSettings() As T
        Return LoadSettings("")
    End Function

    Protected Shared Sub SaveSettings(ByVal Instance As T, ByVal FileId As String)
        Using fs As New IO.FileStream(GetSettingFilePath(FileId), IO.FileMode.Create, IO.FileAccess.Write)
            Dim xs As New Xml.Serialization.XmlSerializer(GetType(T))
            xs.Serialize(fs, Instance)
        End Using
    End Sub

    Protected Shared Sub SaveSettings(ByVal Instance As T)
        SaveSettings(Instance, "")
    End Sub

    Public Shared Function GetSettingFilePath(ByVal FileId As String) As String
        Return IO.Path.Combine(My.Application.Info.DirectoryPath, GetType(T).Name + FileId + ".xml")
    End Function
End Class
