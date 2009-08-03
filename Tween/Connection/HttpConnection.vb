Imports System.Net

Public Class HttpConnection
    Protected Shared _defaultTimeOut As Integer = 20000
    Protected Shared _proxy As System.Net.WebProxy = Nothing
    Protected Shared _proxyType As ProxyTypeEnum = ProxyTypeEnum.IE

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

        Return webReq
    End Function

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
