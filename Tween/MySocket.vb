Imports System.IO
Imports System.Net
Imports System.Text

Public Class MySocket
    Private _enc As Encoding
    Private _cCon As New CookieContainer()
    Private _version As String
    Private _cre As String
    Private _uid As String
    Private _pwd As String
    Private _proxy As System.Net.WebProxy
    Private _proxyType As ProxyTypeEnum

    Public Enum REQ_TYPE
        ReqGET
        ReqGETBinary
        ReqPOST
        ReqPOSTEncode
        ReqPOSTEncodeProtoVer1
        ReqPOSTEncodeProtoVer2
        ReqGETForwardTo
        ReqGETFile
        ReqGETFileUp
        ReqGetNoCache
        ReqPOSTAPI
    End Enum

    Public Sub New(ByVal EncodeType As String, _
            ByVal Username As String, _
            ByVal Password As String, _
            ByVal ProxyType As ProxyTypeEnum, _
            ByVal ProxyAddress As String, _
            ByVal ProxyPort As Integer, _
            ByVal ProxyUser As String, _
            ByVal ProxyPassword As String)
        _enc = Encoding.GetEncoding(EncodeType)
        ServicePointManager.Expect100Continue = False
        _version = My.Application.Info.Version.ToString
        If Username <> "" Then
            _cre = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(Username + ":" + Password))
            _uid = Username
            _pwd = Password
        End If
        Select Case ProxyType
            Case ProxyTypeEnum.None
                _proxy = Nothing
            Case ProxyTypeEnum.Specified
                _proxy = New WebProxy("http://" + ProxyAddress + ":" + ProxyPort.ToString)
                If ProxyUser <> "" Or ProxyPassword <> "" Then
                    _proxy.Credentials = New NetworkCredential(ProxyUser, ProxyPassword)
                End If
                'IE設定（システム設定）はデフォルト値なので処理しない
        End Select
        _proxyType = ProxyType
    End Sub

    Public Function GetWebResponse(ByVal url As String, _
            ByRef resStatus As String, _
            Optional ByVal reqType As REQ_TYPE = REQ_TYPE.ReqGET, _
            Optional ByVal data As String = "", _
            Optional ByVal referer As String = "", _
            Optional ByVal timeout As Integer = 15000) As Object
        Dim webReq As HttpWebRequest
        Dim dataB As Byte()
        Dim strm As Stream
        Dim cpolicy As System.Net.Cache.HttpRequestCachePolicy = New Cache.HttpRequestCachePolicy(Cache.HttpRequestCacheLevel.NoCacheNoStore)

        Try
            webReq = _
                CType(WebRequest.Create(url), HttpWebRequest)
            webReq.Timeout = timeout
            If reqType <> REQ_TYPE.ReqPOSTAPI Then
                webReq.CookieContainer = _cCon
                webReq.AutomaticDecompression = DecompressionMethods.Deflate Or DecompressionMethods.GZip
            End If
            webReq.KeepAlive = False
            webReq.AllowAutoRedirect = False
            webReq.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; ja; rv:1.9) Gecko/2008051206 Firefox/3.0"
            If reqType = REQ_TYPE.ReqGetNoCache Then
                webReq.CachePolicy = cpolicy
            End If
            If _proxyType <> ProxyTypeEnum.IE Then
                webReq.Proxy = _proxy
            End If

            If referer <> "" Then webReq.Referer = referer
            'POST系
            If reqType = REQ_TYPE.ReqPOST Or reqType = REQ_TYPE.ReqPOSTEncode Or _
               reqType = REQ_TYPE.ReqPOSTEncodeProtoVer1 Or reqType = REQ_TYPE.ReqPOSTEncodeProtoVer2 Or _
               reqType = REQ_TYPE.ReqPOSTAPI Then
                webReq.Method = "POST"
                webReq.Timeout = timeout
                dataB = Encoding.ASCII.GetBytes(data)
                webReq.ContentLength = dataB.Length
                Select Case reqType
                    Case REQ_TYPE.ReqPOST
                        webReq.ContentType = "application/x-www-form-urlencoded"
                    Case REQ_TYPE.ReqPOSTEncode
                        '                        webReq.ContentType = "application/x-www-form-urlencoded; charset=" + _enc.WebName
                        webReq.ContentType = "application/x-www-form-urlencoded"
                        webReq.Accept = "text/xml,application/xml,application/xhtml+xml,text/html,text/plain,image/png,*/*"
                    Case REQ_TYPE.ReqPOSTEncodeProtoVer1
                        webReq.ContentType = "application/x-www-form-urlencoded; charset=" + _enc.WebName
                        webReq.Accept = "text/javascript, text/html, application/xml, text/xml, */*"
                        webReq.Headers.Add("x-prototype-version", "1.6.0.1")
                        webReq.Headers.Add("x-requested-with", "XMLHttpRequest")
                        webReq.Headers.Add("Accept-Language", "ja,en-us;q=0.7,en;q=0.3")
                        webReq.Headers.Add("Accept-Charset", "Shift_JIS,utf-8;q=0.7,*;q=0.7")
                    Case REQ_TYPE.ReqPOSTEncodeProtoVer2
                        webReq.ContentType = "application/x-www-form-urlencoded; charset=" + _enc.WebName
                        webReq.Accept = "text/javascript, text/html, application/xml, text/xml, */*"
                        webReq.Headers.Add("x-prototype-version", "1.6.0.1")
                        webReq.Headers.Add("x-requested-with", "XMLHttpRequest")
                        webReq.Headers.Add("Accept-Language", "ja,en-us;q=0.7,en;q=0.3")
                        webReq.Headers.Add("Accept-Charset", "Shift_JIS,utf-8;q=0.7,*;q=0.7")
                    Case REQ_TYPE.ReqPOSTAPI
                        webReq.ContentType = "application/x-www-form-urlencoded"
                        webReq.Accept = "text/html, */*"
                        webReq.Headers.Add("X-Twitter-Client", "Tween")
                        webReq.Headers.Add("X-Twitter-Client-Version", _version)
                        webReq.Headers.Add("X-Twitter-Client-URL", "http://www.asahi-net.or.jp/~ne5h-ykmz/tween.xml")
                        webReq.Headers.Add(HttpRequestHeader.Authorization, _cre)
                End Select
                Dim st As Stream = webReq.GetRequestStream()
                st.Write(dataB, 0, dataB.Length)
                st.Close()
            ElseIf reqType = REQ_TYPE.ReqGET Or reqType = REQ_TYPE.ReqGetNoCache Then
                webReq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"
                webReq.Headers.Add("Accept-Language", "ja,en-us;q=0.7,en;q=0.3")
                webReq.Headers.Add("Accept-Charset", "Shift_JIS,utf-8;q=0.7,*;q=0.7")
            End If

            Using webRes As HttpWebResponse = CType(webReq.GetResponse(), HttpWebResponse)
                'Cookieの処理
                '*** 暫定　http://twitter.com　へアクセスすると、Cookie.Domain = ".twitter.com"になるため
                'If webReq.RequestUri.Host = "twitter.com" And reqType <> REQ_TYPE.ReqPOSTAPI Then
                '    For Each ck As Cookie In webRes.Cookies
                '        If ck.Domain = ".twitter.com" Then
                '            ck.Domain = "twitter.com"
                '            _cCon.Add(ck)
                '        End If
                '    Next
                'End If
                If reqType <> REQ_TYPE.ReqPOSTAPI Then
                    For Each ck As Cookie In webRes.Cookies
                        If ck.Domain.StartsWith(".") Then
                            ck.Domain = ck.Domain.Substring(1, ck.Domain.Length - 1)
                            _cCon.Add(ck)
                        End If
                    Next
                End If
                resStatus = webRes.StatusCode.ToString() + " " + webRes.ResponseUri.AbsoluteUri
                strm = webRes.GetResponseStream()

                Select Case reqType
                    Case REQ_TYPE.ReqGET, REQ_TYPE.ReqPOST, REQ_TYPE.ReqPOSTEncode, REQ_TYPE.ReqPOSTEncodeProtoVer1, REQ_TYPE.ReqPOSTEncodeProtoVer2, REQ_TYPE.ReqGetNoCache, REQ_TYPE.ReqPOSTAPI
                        Dim rtStr As String
                        Using sr As New StreamReader(strm, _enc)
                            rtStr = sr.ReadToEnd()
                        End Using
                        strm.Close()
                        strm.Dispose()
                        webRes.Close()
                        Return rtStr
                    Case REQ_TYPE.ReqGETBinary
                        Dim readData(1023) As Byte
                        Dim readSize As Integer = 0
                        Dim img As Image
                        Using mem As New MemoryStream
                            While True
                                readSize = strm.Read(readData, 0, readData.Length)
                                If readSize = 0 Then
                                    Exit While
                                End If
                                mem.Write(readData, 0, readSize)
                            End While
                            img = Image.FromStream(mem, True)
                            strm.Close()
                            strm.Dispose()
                            webRes.Close()
                        End Using
                        Return img
                    Case REQ_TYPE.ReqGETFile
                        Dim fs As New System.IO.FileStream(My.Application.Info.DirectoryPath + "\TweenNew.exe", FileMode.Create, FileAccess.Write)
                        Dim b As Integer
                        Using fs
                            While True
                                b = strm.ReadByte()
                                If b = -1 Then Exit While
                                fs.WriteByte(Convert.ToByte(b))
                            End While
                        End Using
                        strm.Close()
                        strm.Dispose()
                        webRes.Close()
                    Case REQ_TYPE.ReqGETFileUp
                        Dim fs As New System.IO.FileStream(My.Application.Info.DirectoryPath + "\TweenUp.exe", FileMode.Create, FileAccess.Write)
                        Dim b As Integer
                        Using fs
                            While True
                                b = strm.ReadByte()
                                If b = -1 Then Exit While
                                fs.WriteByte(Convert.ToByte(b))
                            End While
                        End Using
                        strm.Close()
                        strm.Dispose()
                        webRes.Close()
                    Case REQ_TYPE.ReqGETForwardTo
                        strm.Close()
                        strm.Dispose()
                        Dim rtStr As String = ""
                        If webRes.StatusCode = HttpStatusCode.Moved Then
                            rtStr = webRes.Headers.GetValues("Location")(0)
                            Return rtStr
                        End If
                        webRes.Close()
                End Select
            End Using

        Catch ex As System.Net.WebException
            If ex.Status = WebExceptionStatus.ProtocolError Then
                Dim eres As HttpWebResponse = CType(ex.Response, HttpWebResponse)
                resStatus = "Err: " + eres.StatusCode.ToString() + " " + eres.ResponseUri.AbsoluteUri
                If reqType = REQ_TYPE.ReqGETBinary Then
                    Return Nothing
                Else
                    Return ""
                End If
            Else
                resStatus = "Err: ProtocolError(" + ex.Message + ") " + url
                If reqType = REQ_TYPE.ReqGETBinary Then
                    Return Nothing
                Else
                    Return ""
                End If
            End If
        Catch ex As Exception
            resStatus = "Err: " + ex.Message + " " + url
            If reqType = REQ_TYPE.ReqGETBinary Then
                Return Nothing
            Else
                Return ""
            End If
        End Try

        Return ""
    End Function

    Public WriteOnly Property Username() As String
        Set(ByVal value As String)
            _uid = value
        End Set
    End Property

    Public WriteOnly Property Password() As String
        Set(ByVal value As String)
            _pwd = value
        End Set
    End Property

    Public Sub CreateCredentialInfo()
        _cre = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(_uid + ":" + _pwd))
    End Sub
End Class
