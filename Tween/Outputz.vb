Imports System.Web

Public Class Outputz
    Private myOuturl As String
    Private myOuturlEncoded As String
    Private myApikey As String
    Private myApikeyEncoded As String

    Private state As Boolean


    Public Sub New(ByVal _key As String, ByVal _url As String)
        url = _url
        key = _key
        Enabled = False
    End Sub

    Public Property url() As String
        Get
            Return myOuturl
        End Get
        Set(ByVal value As String)
            myOuturl = value
            myOuturlEncoded = HttpUtility.UrlEncode(value)
        End Set
    End Property

    Public Property key() As String
        Get
            Return myApikey
        End Get
        Set(ByVal value As String)
            myApikey = value
            myApikeyEncoded = HttpUtility.UrlEncode(value)
        End Set
    End Property

    Public Property Enabled() As Boolean
        Get
            Return state
        End Get
        Set(ByVal value As Boolean)
            state = value
        End Set
    End Property

    Public Function Post(ByVal obj As MySocket, ByVal length As Integer) As String

        If state = False Then Return ""

        Dim resStatus As String = ""
        Dim output As String = "http://outputz.com/api/post"
        Dim data As String = String.Format("key={0}&uri={1}&size={2}", myApikeyEncoded, myOuturlEncoded, length)

        obj.GetWebResponse(output, resStatus, MySocket.REQ_TYPE.ReqPOST, data, userAgent:="Tween/" + My.Application.Info.Version.ToString())

        If resStatus.StartsWith("OK") Then
            Return ""
        Else
            Return resStatus
        End If
    End Function
End Class
