Imports System.Net

Public Class HttpConnection
    Protected Shared _defaultTimeOut As Integer = 20000
    Protected Shared _proxy As System.Net.WebProxy = Nothing
    Protected Shared _proxyType As ProxyTypeEnum = ProxyTypeEnum.IE
    Private Shared _cookieContainer As New CookieContainer

    Protected Enum WEBACCESS_REQ_TYPE
        ReqGET
        ReqPOST
    End Enum

    Public Enum ProxyTypeEnum
        None
        IE
        Specified
    End Enum

    Protected Shared Function CreateRequestObject(ByVal reqType As WEBACCESS_REQ_TYPE, _
                                            ByVal url As System.Uri, _
                                            ByVal param As System.Collections.Generic.SortedList(Of String, String)) As HttpWebRequest

        Dim ub As New System.UriBuilder(url.AbsoluteUri)
        If reqType = WEBACCESS_REQ_TYPE.ReqGET Then
            ub.Query = CreateQueryString(param)
        End If
        Dim webReq As HttpWebRequest = CType(WebRequest.Create(ub.Uri), HttpWebRequest)

        webReq.Timeout = _defaultTimeOut
        If _proxyType <> ProxyTypeEnum.IE Then webReq.Proxy = _proxy

        If reqType = WEBACCESS_REQ_TYPE.ReqGET Then
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
                                        ByVal resultStream As System.IO.Stream, _
                                        ByVal headerInfo As System.Collections.Generic.Dictionary(Of String, String) _
                                    ) As HttpStatusCode
        Using webRes As HttpWebResponse = CType(webRequest.GetResponse(), HttpWebResponse)
            GetHeaderInfo(webRes, headerInfo)

            Dim statusCode As HttpStatusCode = webRes.StatusCode
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

            Using strm As System.IO.Stream = webRes.GetResponseStream()
                If strm IsNot Nothing Then CopyStream(strm, resultStream)
            End Using
            Return statusCode
        End Using
    End Function

    Private Shared Sub CopyStream(ByVal inStream As System.IO.Stream, _
                                            ByVal outStream As System.IO.Stream)
        If inStream Is Nothing Then Throw New ArgumentNullException("Input stream is null.")
        If outStream Is Nothing Then Throw New ArgumentNullException("Output stream is null.")
        If Not inStream.CanRead Then Throw New ArgumentException("Input stream can not read.")
        If Not outStream.CanWrite Then Throw New ArgumentException("Output stream can not write.")
        If inStream.CanSeek AndAlso inStream.Length = 0 Then Throw New ArgumentException("Input stream do not have data.")

        Dim buffer(1023) As Byte
        Dim i As Integer = buffer.Length
        Do
            i = inStream.Read(buffer, 0, i)
            If i = 0 Then Exit Do
            outStream.Write(buffer, 0, i)
        Loop
    End Sub

    Private Shared Sub GetHeaderInfo(ByVal webResponse As HttpWebResponse, _
                            ByVal headerInfo As System.Collections.Generic.Dictionary(Of String, String))
        If headerInfo IsNot Nothing AndAlso headerInfo.Count > 0 Then
            Dim keys(headerInfo.Count - 1) As String
            headerInfo.Keys.CopyTo(keys, 0)
            For Each key As String In keys
                If Array.IndexOf(webResponse.Headers.AllKeys, key) > -1 Then
                    headerInfo.Item(key) = webResponse.Headers.Item(key)
                Else
                    headerInfo.Item(key) = ""
                End If
            Next
        End If
    End Sub

    Protected Shared Function CreateQueryString(ByVal param As System.Collections.Generic.SortedList(Of String, String)) As String
        If param Is Nothing OrElse param.Count = 0 Then Return String.Empty

        Dim query As New System.Text.StringBuilder
        For Each key As String In param.Keys
            query.AppendFormat("{0}={1}&", UrlEncode(key), UrlEncode(param(key)))
        Next
        Return query.ToString(0, query.Length - 1)
    End Function

    Protected Shared Function ParseQueryString(ByVal queryString As String) As System.Collections.Specialized.NameValueCollection
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
                sb.appendformat("%{0:X2}", b)
            End If
        Next
        Return sb.ToString()
    End Function

    Public Shared Property DefaultTimeOut() As Integer
        Get
            Return _defaultTimeOut
        End Get
        Set(ByVal value As Integer)
            If value < HttpTimeOut.MinValue OrElse value > HttpTimeOut.MaxValue Then
                ' 範囲外ならデフォルト値設定
                _defaultTimeOut = HttpTimeOut.DefaultValue * 1000
            Else
                _defaultTimeOut = value * 1000
            End If
        End Set
    End Property

End Class
