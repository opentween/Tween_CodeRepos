Imports System.Net

Public Class HttpConnectionOAuth
    Inherits HttpConnection

    '''<summary>
    '''oAuth署名のoauth_timestamp算出用基準日付（1970/1/1 00:00:00）
    '''</summary>
    Private Shared ReadOnly _unixEpoch As New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)

    '''<summary>
    '''oAuth署名のoauth_nonce算出用乱数クラス
    '''</summary>
    Private Shared ReadOnly _random As New Random

    '''<summary>
    '''oAuthの認証プロセス時のみ使用するリクエストトークン
    '''</summary>
    Private Shared _requestToken As String

    '''<summary>
    '''oAuthのアクセストークン。永続化可能（ユーザー取り消しの可能性はある）。
    '''</summary>
    Public Shared Token As String = ""

    '''<summary>
    '''oAuthの署名作成用秘密アクセストークン。永続化可能（ユーザー取り消しの可能性はある）。
    '''</summary>
    Public Shared TokenSecret As String = ""

    '''<summary>
    '''oAuthのコンシューマー鍵
    '''</summary>
    Private Const CONSUMER_KEY As String = "EANjQEa5LokuVld682tTDA"

    '''<summary>
    '''oAuthの署名作成用秘密コンシューマーデータ
    '''</summary>
    Private Const CONSUMER_SECRET As String = "zXfwkzmuO6FcHtoikleV3EVgdh5vVAs6ft6ZxtYTYM"

    '''<summary>
    '''oAuthのリクエストトークン取得先URL
    '''</summary>
    Private Const REQUESTTOKEN_URL As String = "http://twitter.com/oauth/request_token"

    '''<summary>
    '''oAuthのユーザー認証用ページURL
    '''</summary>
    '''<remarks>
    '''クエリ「oauth_token=リクエストトークン」を付加して、このURLをブラウザで開く。ユーザーが承認操作を行うとPINコードが表示される。
    '''</remarks>
    Private Const AUTHORIZE_URL As String = "http://twitter.com/oauth/authorize"

    '''<summary>
    '''oAuthのアクセストークン取得先URL
    '''</summary>
    Private Const ACCESSTOKEN_URL As String = "http://twitter.com/oauth/access_token"

    '''<summary>
    '''HTTP通信してコンテンツを取得する（文字列コンテンツ）
    '''</summary>
    '''<remarks>
    '''通信タイムアウトなどWebExceptionをハンドルしていないため、呼び出し元で処理が必要。
    '''タイムアウト指定やレスポンスヘッダ取得は省略している。
    '''レスポンスのボディストリームを文字列に変換してcontent引数に格納して戻す。文字エンコードは未指定
    '''</remarks>
    '''<param name="method">HTTPのメソッド</param>
    '''<param name="url">URL</param>
    '''<param name="param">key=valueに展開されて、クエリ（GET時）・ボディ（POST時）に付加される送信情報</param>
    '''<param name="content">HTTPレスポンスのボディ部データ返却用。呼び出し元で初期化が必要</param>
    '''<returns>通信結果のHttpStatusCode</returns>
    Protected Overloads Function GetContent(ByVal method As RequestMethod, _
            ByVal url As System.Uri, _
            ByVal param As System.Collections.Generic.SortedList(Of String, String), _
            ByRef content As String) As HttpStatusCode
        'ToDo:雛形。要修正
        If content Is Nothing Then Throw New ArgumentNullException("content")
        Using stream As New System.IO.MemoryStream
            Dim statusCode As HttpStatusCode = HttpConnection.GetResponse( _
                                                HttpConnection.CreateRequest(method, _
                                                                            url, _
                                                                            param, _
                                                                            False), _
                                                stream, _
                                                Nothing, _
                                                False)
            Using reader As New System.IO.StreamReader(content)
                content = reader.ReadToEnd
            End Using
            Return statusCode
        End Using
    End Function
End Class
