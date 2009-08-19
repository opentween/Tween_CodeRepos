Imports System.Net
Imports System.Collections.Generic
Imports System.IO

Public Class HttpConnectionOAuth
    Inherits HttpConnection

    '''<summary>
    '''OAuth署名のoauth_timestamp算出用基準日付（1970/1/1 00:00:00）
    '''</summary>
    Private Shared ReadOnly UnixEpoch As New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)

    '''<summary>
    '''OAuth署名のoauth_nonce算出用乱数クラス
    '''</summary>
    Private Shared ReadOnly NonceRandom As New Random

    '''<summary>
    '''OAuthの認証プロセス時のみ使用するリクエストトークン
    '''</summary>
    Private Shared requestToken As String

    '''<summary>
    '''OAuthのアクセストークン。永続化可能（ユーザー取り消しの可能性はある）。
    '''</summary>
    Public Shared Token As String = ""

    '''<summary>
    '''OAuthの署名作成用秘密アクセストークン。永続化可能（ユーザー取り消しの可能性はある）。
    '''</summary>
    Public Shared TokenSecret As String = ""

    '''<summary>
    '''OAuthのコンシューマー鍵
    '''</summary>
    Private Const ConsumerKey As String = "EANjQEa5LokuVld682tTDA"

    '''<summary>
    '''OAuthの署名作成用秘密コンシューマーデータ
    '''</summary>
    Private Const ConsumerSecret As String = "zXfwkzmuO6FcHtoikleV3EVgdh5vVAs6ft6ZxtYTYM"

    '''<summary>
    '''OAuthのリクエストトークン取得先URI
    '''</summary>
    Private Const RequestTokenUrl As String = "http://twitter.com/oauth/request_token"

    '''<summary>
    '''OAuthのユーザー認証用ページURI
    '''</summary>
    '''<remarks>
    '''クエリ「oauth_token=リクエストトークン」を付加して、このURIをブラウザで開く。ユーザーが承認操作を行うとPINコードが表示される。
    '''</remarks>
    Private Const AuthorizeUrl As String = "http://twitter.com/oauth/authorize"

    '''<summary>
    '''OAuthのアクセストークン取得先URI
    '''</summary>
    Private Const AccessTokenUrl As String = "http://twitter.com/oauth/access_token"

    '''<summary>
    '''HTTP通信してコンテンツを取得する（文字列コンテンツ）
    '''</summary>
    '''<remarks>
    '''通信タイムアウトなどWebExceptionをハンドルしていないため、呼び出し元で処理が必要。
    '''タイムアウト指定やレスポンスヘッダ取得は省略している。
    '''レスポンスのボディストリームを文字列に変換してcontent引数に格納して戻す。文字エンコードは未指定
    '''</remarks>
    '''<param name="method">HTTPのメソッド</param>
    '''<param name="requestUri">URI</param>
    '''<param name="param">key=valueに展開されて、クエリ（GET時）・ボディ（POST時）に付加される送信情報</param>
    '''<param name="content">HTTPレスポンスのボディ部データ返却用。呼び出し元で初期化が必要</param>
    '''<returns>通信結果のHttpStatusCode</returns>
    Protected Overloads Function GetContent(ByVal method As RequestMethod, _
            ByVal requestUri As Uri, _
            ByVal param As SortedList(Of String, String), _
            ByRef content As String) As HttpStatusCode
        'ToDo:雛形。要修正
        If content Is Nothing Then Throw New ArgumentNullException("content")
        Using stream As New MemoryStream
            Dim statusCode As HttpStatusCode = HttpConnection.GetResponse( _
                                                HttpConnection.CreateRequest(method, _
                                                                            requestUri, _
                                                                            param, _
                                                                            False), _
                                                stream, _
                                                Nothing, _
                                                False)
            Using reader As New StreamReader(content)
                content = reader.ReadToEnd
            End Using
            Return statusCode
        End Using
    End Function
End Class
