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
        Dim err As Boolean = False
        Dim fileName As String = GetSettingFilePath(FileId)
        Do
            err = False
            cnt += 1
            Try
                Using fs As New IO.FileStream(fileName, IO.FileMode.Create, IO.FileAccess.Write)
                    Dim xs As New Xml.Serialization.XmlSerializer(GetType(T))
                    xs.Serialize(fs, Instance)
                    fs.Flush()
                    fs.Close()
                End Using
                '検証
                Dim xdoc As New Xml.XmlDocument()
                xdoc.Load(fileName)
            Catch ex As Exception
                '検証エラー or 書き込みエラー
                If cnt > 3 Then
                    'リトライオーバー
                    Throw New System.InvalidOperationException("Can't write setting XML.(" + fileName + ")")
                    Exit Sub
                End If
                'リトライ
                Threading.Thread.Sleep(1000)
                err = True
            End Try
        Loop While err
    End Sub

    Protected Shared Sub SaveSettings(ByVal Instance As T)
        SaveSettings(Instance, "")
    End Sub

    Public Shared Function GetSettingFilePath(ByVal FileId As String) As String
        Return IO.Path.Combine(My.Application.Info.DirectoryPath, GetType(T).Name + FileId + ".xml")
    End Function
End Class
