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
        Dim cnt As Integer = 0
        Do
            cnt += 1
            Using fs As New IO.FileStream(GetSettingFilePath(FileId), IO.FileMode.Create, IO.FileAccess.Write)
                Dim xs As New Xml.Serialization.XmlSerializer(GetType(T))
                xs.Serialize(fs, Instance)
            End Using
            If cnt > 3 Then Throw New System.InvalidOperationException("Can't write setting XML.")
        Loop Until ValidateXml(GetSettingFilePath(FileId))
    End Sub

    Private Shared Function ValidateXml(ByVal fileName As String) As Boolean
        Try
            Dim xdoc As New Xml.XmlDocument()
            xdoc.Load(fileName)
            Return True
        Catch ex As Exception
            Threading.Thread.Sleep(0)
            Return False
        End Try
    End Function

    Protected Shared Sub SaveSettings(ByVal Instance As T)
        SaveSettings(Instance, "")
    End Sub

    Public Shared Function GetSettingFilePath(ByVal FileId As String) As String
        Return IO.Path.Combine(My.Application.Info.DirectoryPath, GetType(T).Name + FileId + ".xml")
    End Function
End Class
