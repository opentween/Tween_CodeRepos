Imports System.Web

Public Class Outputz
    Private myOuturl As String
    Private myApikey As String

    Private state As Boolean


    Public Sub New(ByVal _key As String, ByVal _url As String)
        myApikey = _key
        myOuturl = _url
        state = False
    End Sub

    Public Property url() As String
        Get
            Return myOuturl
        End Get
        Set(ByVal value As String)
            myOuturl = value
        End Set
    End Property

    Public Property key() As String
        Get
            Return myApikey
        End Get
        Set(ByVal value As String)
            myApikey = value
        End Set
    End Property

    Public Property Enable() As Boolean
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
        Dim output As String = String.Format("http://outputz.com/api/post?key={0}&uri={1}&size={2}", myApikey, HttpUtility.UrlEncode(myOuturl), length)

        obj.GetWebResponse(output, resStatus, MySocket.REQ_TYPE.ReqPOST)

        If resStatus.StartsWith("OK") Then
            Return ""
        Else
            Return resStatus
        End If
    End Function
End Class
