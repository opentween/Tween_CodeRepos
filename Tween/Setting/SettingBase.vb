Public MustInherit Class SettingBase(Of T As {Class, New})

    Private Shared lockObj As New Object

    Protected Shared Function LoadSettings(ByVal FileId As String) As T
        Try
            SyncLock lockObj
                Using fs As New IO.FileStream(GetSettingFilePath(FileId), IO.FileMode.Open)
                    fs.Position = 0
                    Dim xs As New Xml.Serialization.XmlSerializer(GetType(T))
                    Dim instance As T = DirectCast(xs.Deserialize(fs), T)
                    fs.Close()
                    Return instance
                End Using
            End SyncLock
        Catch ex As System.IO.FileNotFoundException
            Return New T()
        Catch ex As Exception
            ex.Data.Add("FilePath", GetSettingFilePath(FileId))
            Dim fi As New IO.FileInfo(GetSettingFilePath(FileId))
            ex.Data.Add("FileSize", fi.Length.ToString())
            ex.Data.Add("FileData", IO.File.ReadAllText(GetSettingFilePath(FileId)))
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
        If Instance Is Nothing Then Exit Sub
        Do
            err = False
            cnt += 1
            Try
                SyncLock lockObj
                    Using fs As New IO.FileStream(fileName, IO.FileMode.Create)
                        fs.Position = 0
                        Dim xs As New Xml.Serialization.XmlSerializer(GetType(T))
                        xs.Serialize(fs, Instance)
                        fs.Flush()
                        fs.Close()
                    End Using
                    Dim fi As New IO.FileInfo(fileName)
                    If fi.Length = 0 Then
                        If cnt > 3 Then
                            Throw New Exception
                            Exit Sub
                        End If
                        Threading.Thread.Sleep(1000)
                        err = True
                    End If
                End SyncLock
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
