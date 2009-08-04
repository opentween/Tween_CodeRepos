Imports System.Net

Public Class HttpConnection
    Private Shared _proxy As System.Net.WebProxy = Nothing
    Private Shared _proxyType As ProxyType = ProxyType.IE
    Private Shared _defaultTimeOut As Integer = 20000
    Private Shared _cookieContainer As New CookieContainer

    Protected Enum RequestMethod
        ReqGet
        ReqPOST
    End Enum

    Protected Overridable Function GetContent(ByVal method As RequestMethod, _
            ByVal url As System.Uri, _
            ByVal param As System.Collections.Generic.SortedList(Of String, String), _
            ByRef content As String) As HttpStatusCode
        If content Is Nothing Then Throw New ArgumentNullException("content")
        Using stream As New System.IO.MemoryStream
            Dim statusCode As HttpStatusCode = HttpConnection.GetResponse( _
                                                HttpConnection.CreateRequest(method, _
                                                                            url, _
                                                                            param), _
                                                stream)
            Using reader As New System.IO.StreamReader(content)
                content = reader.ReadToEnd
            End Using
            Return statusCode
        End Using
    End Function

    Protected Overridable Function GetContent(ByVal method As RequestMethod, _
            ByVal url As System.Uri, _
            ByVal param As System.Collections.Generic.SortedList(Of String, String), _
            ByVal content As Bitmap) As HttpStatusCode
        Using stream As New System.IO.MemoryStream
            Dim statusCode As HttpStatusCode = HttpConnection.GetResponse( _
                                                HttpConnection.CreateRequest(method, _
                                                                            url, _
                                                                            param), _
                                                stream)
            content = New System.Drawing.Bitmap(stream)
            Return statusCode
        End Using
    End Function

    Protected Shared Function CreateRequest(ByVal method As RequestMethod, _
                                            ByVal url As System.Uri, _
                                            ByVal param As System.Collections.Generic.SortedList(Of String, String)) As HttpWebRequest

        Dim ub As New System.UriBuilder(url.AbsoluteUri)
        If method = RequestMethod.ReqGet Then
            ub.Query = CreateQueryString(param)
        End If
        Dim webReq As HttpWebRequest = DirectCast(WebRequest.Create(ub.Uri), HttpWebRequest)

        webReq.Timeout = _defaultTimeOut
        If _proxyType <> ProxyType.IE Then webReq.Proxy = _proxy

        If method = RequestMethod.ReqGet Then
            webReq.Method = "GET"
        Else
            webReq.Method = "POST"
            webReq.ContentType = "application/x-www-form-urlencoded"
            Using writer As New System.IO.StreamWriter(webReq.GetRequestStream)
                writer.Write(CreateQueryString(param))
            End Using
        End If
        webReq.CookieContainer = _cookieContainer

        Return webReq
    End Function

    Protected Shared Function GetResponse(ByVal webRequest As HttpWebRequest, _
                                    ByVal contentStream As System.IO.Stream _
                                ) As HttpStatusCode
        Return HttpConnection.GetResponse(webRequest, _
                                        contentStream, _
                                        Nothing)
    End Function

    Protected Shared Function GetResponse(ByVal webRequest As HttpWebRequest, _
                                        ByVal contentStream As System.IO.Stream, _
                                        ByVal headerInfo As System.Collections.Generic.Dictionary(Of String, String) _
                                    ) As HttpStatusCode
        Using webRes As HttpWebResponse = CType(webRequest.GetResponse(), HttpWebResponse)
            Dim statusCode As HttpStatusCode = webRes.StatusCode

            If headerInfo IsNot Nothing Then
                GetHeaderInfo(webRes, headerInfo)
                If statusCode = HttpStatusCode.MovedPermanently OrElse _
                   statusCode = HttpStatusCode.Found OrElse _
                   statusCode = HttpStatusCode.SeeOther OrElse _
                   statusCode = HttpStatusCode.TemporaryRedirect Then
                    If headerInfo.ContainsKey("Location") Then
                        headerInfo.Item("Location") = webRes.Headers.Item("Location")
                    Else
                        headerInfo.Add("Location", webRes.Headers.Item("Location"))
                    End If
                    Return statusCode
                End If
            End If

            If webRes.ContentLength > 0 Then
                Using stream As System.IO.Stream = webRes.GetResponseStream()
                    If stream IsNot Nothing Then CopyStream(stream, contentStream)
                End Using
            End If
            Return statusCode
        End Using
    End Function

    Private Shared Sub CopyStream(ByVal inStream As System.IO.Stream, _
                                            ByVal outStream As System.IO.Stream)
        If inStream Is Nothing Then Throw New ArgumentNullException("inStream")
        If outStream Is Nothing Then Throw New ArgumentNullException("outStream")
        If Not inStream.CanRead Then Throw New ArgumentException("Input stream can not read.")
        If Not outStream.CanWrite Then Throw New ArgumentException("Output stream can not write.")
        If inStream.CanSeek AndAlso inStream.Length = 0 Then Throw New ArgumentException("Input stream do not have data.")

        Do
            Dim buffer(1024) As Byte
            Dim i As Integer = buffer.Length
            i = inStream.Read(buffer, 0, i)
            If i = 0 Then Exit Do
            outStream.Write(buffer, 0, i)
        Loop
    End Sub

    Private Shared Sub GetHeaderInfo(ByVal webResponse As HttpWebResponse, _
                            ByVal headerInfo As System.Collections.Generic.Dictionary(Of String, String))

        If headerInfo Is Nothing OrElse headerInfo.Count = 0 Then Exit Sub

        Dim keys(headerInfo.Count - 1) As String
        headerInfo.Keys.CopyTo(keys, 0)
        For Each key As String In keys
            If Array.IndexOf(webResponse.Headers.AllKeys, key) > -1 Then
                headerInfo.Item(key) = webResponse.Headers.Item(key)
            Else
                headerInfo.Item(key) = ""
            End If
        Next
    End Sub

    Private Shared Function CreateQueryString(ByVal param As System.Collections.Generic.SortedList(Of String, String)) As String
        If param Is Nothing OrElse param.Count = 0 Then Return String.Empty

        Dim query As New System.Text.StringBuilder
        For Each key As String In param.Keys
            query.AppendFormat("{0}={1}&", UrlEncode(key), UrlEncode(param(key)))
        Next
        Return query.ToString(0, query.Length - 1)
    End Function

    Private Shared Function ParseQueryString(ByVal queryString As String) As System.Collections.Specialized.NameValueCollection
        Dim query As New System.Collections.Specialized.NameValueCollection
        Dim parts() As String = queryString.Split("&"c)
        For Each part As String In parts
            Dim index As Integer = part.IndexOf("="c)
            If index = -1 Then
                query.Add(Uri.UnescapeDataString(part), "")
            Else
                query.Add(Uri.UnescapeDataString(part.Substring(0, index)), Uri.UnescapeDataString(part.Substring(index + 1)))
            End If
        Next
        Return query
    End Function

    Protected Shared Function UrlEncode(ByVal str As String) As String
        Const _unreservedChars As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~"
        Dim sb As New System.Text.StringBuilder
        Dim bytes As Byte() = System.Text.Encoding.UTF8.GetBytes(str)

        For Each b As Byte In bytes
            If _unreservedChars.IndexOf(Chr(b)) <> -1 Then
                sb.Append(Chr(b))
            Else
                sb.AppendFormat("%{0:X2}", b)
            End If
        Next
        Return sb.ToString()
    End Function

    Private Shared WriteOnly Property DefaultTimeout() As Integer
        Set(ByVal value As Integer)
            Const TimeoutMinValue As Integer = 10
            Const TimeoutMaxValue As Integer = 120
            Const TimeoutDefaultValue As Integer = 20000
            If value < TimeoutMinValue OrElse value > TimeoutMaxValue Then
                ' 範囲外ならデフォルト値設定
                _defaultTimeOut = TimeoutDefaultValue
            Else
                _defaultTimeOut = value * 1000
            End If
        End Set
    End Property

    Public Shared Sub InitializeConnection( _
            ByVal timeout As Integer, _
            ByVal proxyType As ProxyType, _
            ByVal proxyAddress As String, _
            ByVal proxyPort As Integer, _
            ByVal proxyUser As String, _
            ByVal proxyPassword As String)

        ServicePointManager.Expect100Continue = False
        DefaultTimeout = timeout
        Select Case proxyType
            Case proxyType.None
                _proxy = Nothing
            Case proxyType.Specified
                _proxy = New WebProxy("http://" + proxyAddress + ":" + proxyPort.ToString)
                If Not String.IsNullOrEmpty(proxyUser) OrElse Not String.IsNullOrEmpty(proxyPassword) Then
                    _proxy.Credentials = New NetworkCredential(proxyUser, proxyPassword)
                End If
            Case proxyType.IE
                'IE設定（システム設定）はデフォルト値なので処理しない
        End Select
        _proxyType = proxyType
    End Sub
End Class
