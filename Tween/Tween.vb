' Tween - Client of Twitter
' Copyright c 2007-2009 kiri_feather (@kiri_feather) <kiri_feather@gmail.com>
'           c 2008-2009 Moz (@syo68k) <http://iddy.jp/profile/moz/>
'           c 2008-2009 takeshik (@takeshik) <http://www.takeshik.org/>
' All rights reserved.
' 
' This file is part of Tween.
' 
' This program is free software; you can redistribute it and/or modify it
' under the terms of the GNU General Public License as published by the Free
' Software Foundation; either version 3 of the License, or (at your option)
' any later version.
' 
' This program is distributed in the hope that it will be useful, but
' WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
' or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License
' for more details. 
' 
' You should have received a copy of the GNU General Public License along
' with this program. If not, see <http://www.gnu.org/licenses/>, or write to
' the Free Software Foundation, Inc., 51 Franklin Street - Fifth Floor,
' Boston, MA 02110-1301, USA.

Imports System
Imports System.Configuration
Imports System.Text
Imports System.Text.RegularExpressions
Imports Tween.TweenCustomControl
Imports System.IO
Imports System.Web
Imports System.Reflection
Imports System.ComponentModel
Imports System.Xml.XPath

Public Class TweenMain
    '各種設定
    Private _username As String         'ユーザー名
    Private _password As String         'パスワード（デクリプト済み）

    Private _mySize As Size             '画面サイズ
    Private _myLoc As Point             '画面位置
    Private _mySpDis As Integer         '区切り位置
    Private _mySpDis2 As Integer        '発言欄区切り位置
    Private _iconSz As Integer            'アイコンサイズ（現在は16、24、48の3種類。将来直接数字指定可能とする 注：24x24の場合に26と指定しているのはMSゴシック系フォントのための仕様）
    Private _iconCol As Boolean           '1列表示の時True（48サイズのとき）

    '雑多なフラグ類
    Private _initial As Boolean         'True:起動時処理中
    Private _endingFlag As Boolean        '終了フラグ
    'Private listViewItemSorter As ListViewItemComparer      'リストソート用カスタムクラス
    Private _tabDrag As Boolean           'タブドラッグ中フラグ（DoDragDropを実行するかの判定用）
    Private _rclickTabName As String      '右クリックしたタブの名前（Tabコントロール機能不足対応）
    Private ReadOnly _syncObject As New Object()    'ロック用
    Private Const detailHtmlFormat1 As String = "<html><head><style type=""text/css""><!-- p {font-family: """
    Private Const detailHtmlFormat2 As String = """, sans-serif; font-size: "
    Private Const detailHtmlFormat3 As String = "pt;} --></style></head><body style=""margin:0px""><p>"
    Private Const detailHtmlFormat4 As String = "</p></body></html>"
    Private detailHtmlFormat As String

    '設定ファイル関連
    Private _config As Configuration    'アプリケーション構成ファイルクラス
    Private _section As ListSection     '構成ファイル中のユーザー定義ListSectionクラス

    'サブ画面インスタンス
    Private SettingDialog As New Setting()       '設定画面インスタンス
    Private TabDialog As New TabsDialog()        'タブ選択ダイアログインスタンス
    Private SearchDialog As New SearchWord()     '検索画面インスタンス
    'Private _tabs As New List(Of TabStructure)() '要素TabStructureクラスのジェネリックリストインスタンス（タブ情報用）
    Private fDialog As New FilterDialog() 'フィルター編集画面
    Private UrlDialog As New OpenURL()

    '表示フォント、色、アイコン
    Private _fntUnread As Font            '未読用フォント
    Private _clUnread As Color            '未読用文字色
    Private _fntReaded As Font            '既読用フォント
    Private _clReaded As Color            '既読用文字色
    Private _clFav As Color               'Fav用文字色
    Private _clOWL As Color               '片思い用文字色
    Private _fntDetail As Font            '発言詳細部用フォント
    Private _clSelf As Color              '自分の発言用背景色
    Private _clAtSelf As Color            '自分宛返信用背景色
    Private _clTarget As Color            '選択発言者の他の発言用背景色
    Private _clAtTarget As Color          '選択発言中の返信先用背景色
    Private _clAtFromTarget As Color      '選択発言者への返信発言用背景色
    'Private TIconList As ImageList        '発言詳細部用アイコン画像リスト
    Private TIconDic As Dictionary(Of String, Image)        '発言詳細部用アイコン画像リスト
    Private TIconSmallList As ImageList   'リスト表示用アイコン画像リスト
    Private NIconAt As Icon               'At.ico             タスクトレイアイコン：通常時
    Private NIconAtRed As Icon            'AtRed.ico          タスクトレイアイコン：通信エラー時
    Private NIconAtSmoke As Icon          'AtSmoke.ico        タスクトレイアイコン：オフライン時
    Private NIconRefresh(3) As Icon       'Refresh.ico        タスクトレイアイコン：更新中（アニメーション用に4種類を保持するリスト）
    Private TabIcon As Icon               'Tab.ico            未読のあるタブ用アイコン
    Private MainIcon As Icon              'Main.ico           画面左上のアイコン

    Private _anchorPost As PostClass
    Private _anchorFlag As Boolean        'True:関連発言移動中（関連移動以外のオペレーションをするとFalseへ。Trueだとリスト背景色をアンカー発言選択中として描画）

    Private _history As New List(Of String)()   '発言履歴
    Private _hisIdx As Integer                  '発言履歴カレントインデックス

    Private Const _replyHtml As String = "@<a target=""_self"" href=""https://twitter.com/"

    '発言投稿時のAPI引数（発言編集時に設定。手書きreplyでは設定されない）
    Private _reply_to_id As Long     ' リプライ先のステータスID 0の場合はリプライではない 注：複数あてのものはリプライではない
    Private _reply_to_name As String    ' リプライ先ステータスの書き込み者の名前

    '時速表示用
    Private _postTimestamps As New List(Of Date)()
    Private _favTimestamps As New List(Of Date)()
    Private _tlTimestamps As New Dictionary(Of Date, Integer)()
    Private _tlCount As Integer

    ' 以下DrawItem関連
    Private _brsHighLight As New SolidBrush(Color.FromKnownColor(KnownColor.Highlight))
    Private _brsHighLightText As New SolidBrush(Color.FromKnownColor(KnownColor.HighlightText))
    Private _brsForeColorUnread As SolidBrush
    Private _brsForeColorReaded As SolidBrush
    Private _brsForeColorFav As SolidBrush
    Private _brsForeColorOWL As SolidBrush
    Private _brsBackColorMine As SolidBrush
    Private _brsBackColorAt As SolidBrush
    Private _brsBackColorYou As SolidBrush
    Private _brsBackColorAtYou As SolidBrush
    Private _brsBackColorAtTo As SolidBrush
    Private _brsBackColorNone As SolidBrush
    Private _brsDeactiveSelection As New SolidBrush(Color.FromKnownColor(KnownColor.ButtonFace))
    Private sf As New StringFormat()
    'Private _columnIdx As Integer   'ListviewのDisplayIndex退避用（DrawItemで使用）
    'Private _columnChangeFlag As Boolean

    '''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private _statuses As TabInformations
    Private _itemCache() As ListViewItem
    Private _itemCacheIndex As Integer
    Private _postCache() As PostClass
    Private _curTab As TabPage
    Private _curItemIndex As Integer
    Private _curList As DetailsListView
    Private _curPost As PostClass
    Private _waitFollower As Boolean = False
    Private _waitTimeline As Boolean = False
    Private _waitReply As Boolean = False
    Private _waitDm As Boolean = False
    Private _bw(9) As BackgroundWorker
    Private cMode As Integer
    Private StatusLabel As New ToolStripLabelHistory
    '''''''''''''''''''''''''''''''''''''''''''''''''''''

#If DEBUG Then
    Private _drawcount As Long = 0
    Private _drawtime As Long = 0
#End If

    'URL短縮のUndo用
    Private Structure urlUndo
        Public Before As String
        Public After As String
    End Structure

    Private urlUndoBuffer As Generic.List(Of urlUndo) = Nothing

    Friend Class Win32Api
        '画面をブリンクするためのWin32API。起動時に10ページ読み取りごとに継続確認メッセージを表示する際の通知強調用
        Friend Declare Function FlashWindow Lib "user32.dll" ( _
            ByVal hwnd As Integer, ByVal bInvert As Integer) As Integer
    End Class

    'Backgroundworkerの処理結果通知用引数構造体
    Private Structure GetWorkerResult
        Public retMsg As String                     '処理結果詳細メッセージ。エラー時に値がセットされる
        'Public notifyPosts As List(Of PostClass) '取得した発言。Twitter.MyListItem構造体を要素としたジェネリックリスト
        Public page As Integer                      '取得対象ページ番号
        Public endPage As Integer                   '取得終了ページ番号（継続可能ならインクリメントされて返る。pageと比較して継続判定）
        Public type As WORKERTYPE                   '処理種別
        Public imgs As Dictionary(Of String, Image)                    '新規取得したアイコンイメージ
        Public tName As String                      'Fav追加・削除時のタブ名
        Public ids As List(Of Long)               'Fav追加・削除時のID
        Public sIds As List(Of Long)                  'Fav追加・削除成功分のID
        Public newDM As Boolean
        'Public soundFile As String
        Public addCount As Integer
    End Structure

    'Backgroundworkerへ処理内容を通知するための引数用構造体
    Private Structure GetWorkerArg
        Public page As Integer                      '処理対象ページ番号
        Public endPage As Integer                   '処理終了ページ番号（起動時の読み込みページ数。通常時はpageと同じ値をセット）
        Public type As WORKERTYPE                   '処理種別
        Public status As String                     '発言POST時の発言内容
        Public ids As List(Of Long)               'Fav追加・削除時のItemIndex
        Public sIds As List(Of Long)              'Fav追加・削除成功分のItemIndex
        Public tName As String                      'Fav追加・削除時のタブ名
    End Structure

    '検索処理タイプ
    Private Enum SEARCHTYPE
        DialogSearch
        NextSearch
        PrevSearch
    End Enum

    Private Sub TweenMain_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        If UserPicture.Image IsNot Nothing Then
            UserPicture.Invalidate(False)
        End If
    End Sub

    Private Sub TweenMain_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        '後始末
        SettingDialog.Dispose()
        TabDialog.Dispose()
        SearchDialog.Dispose()
        fDialog.Dispose()
        UrlDialog.Dispose()
        If TIconDic IsNot Nothing AndAlso TIconDic.Keys.Count > 0 Then
            For Each key As String In TIconDic.Keys
                TIconDic(key).Dispose()
            Next
            TIconDic.Clear()
        End If
        If TIconSmallList IsNot Nothing Then TIconSmallList.Dispose()
        If NIconAt IsNot Nothing Then NIconAt.Dispose()
        If NIconAtRed IsNot Nothing Then NIconAtRed.Dispose()
        If NIconAtSmoke IsNot Nothing Then NIconAtSmoke.Dispose()
        If NIconRefresh(0) IsNot Nothing Then NIconRefresh(0).Dispose()
        If NIconRefresh(1) IsNot Nothing Then NIconRefresh(1).Dispose()
        If NIconRefresh(2) IsNot Nothing Then NIconRefresh(2).Dispose()
        If NIconRefresh(3) IsNot Nothing Then NIconRefresh(3).Dispose()
        If TabIcon IsNot Nothing Then TabIcon.Dispose()
        If MainIcon IsNot Nothing Then MainIcon.Dispose()
        _brsHighLight.Dispose()
        _brsHighLightText.Dispose()
        If _brsForeColorUnread IsNot Nothing Then _brsForeColorUnread.Dispose()
        If _brsForeColorReaded IsNot Nothing Then _brsForeColorReaded.Dispose()
        If _brsForeColorFav IsNot Nothing Then _brsForeColorFav.Dispose()
        If _brsForeColorOWL IsNot Nothing Then _brsForeColorOWL.Dispose()
        If _brsBackColorMine IsNot Nothing Then _brsBackColorMine.Dispose()
        If _brsBackColorAt IsNot Nothing Then _brsBackColorAt.Dispose()
        If _brsBackColorYou IsNot Nothing Then _brsBackColorYou.Dispose()
        If _brsBackColorAtYou IsNot Nothing Then _brsBackColorAtYou.Dispose()
        If _brsBackColorAtTo IsNot Nothing Then _brsBackColorAtTo.Dispose()
        If _brsBackColorNone IsNot Nothing Then _brsBackColorNone.Dispose()
        If _brsDeactiveSelection IsNot Nothing Then _brsDeactiveSelection.Dispose()
        StatusLabel.Dispose()
        sf.Dispose()
    End Sub

    Private Sub LoadIcons()
        '着せ替えアイコン対応
        'タスクトレイ通常時アイコン
        Dim dir As String = My.Application.Info.DirectoryPath

        Try
            NIconAt = New Icon(dir + "\Icons\At.ico")
        Catch ex As Exception
            NIconAt = My.Resources.At
        End Try
        'タスクトレイエラー時アイコン
        Try
            NIconAtRed = New Icon(dir + "\Icons\AtRed.ico")
        Catch ex As Exception
            NIconAtRed = My.Resources.AtRed
        End Try
        'タスクトレイオフライン時アイコン
        Try
            NIconAtSmoke = New Icon(dir + "\Icons\AtSmoke.ico")
        Catch ex As Exception
            NIconAtSmoke = My.Resources.AtSmoke
        End Try
        'タスクトレイ更新中アイコン
        'アニメーション対応により4種類読み込み
        Try
            NIconRefresh(0) = New Icon(dir + "\Icons\Refresh.ico")
        Catch ex As Exception
            NIconRefresh(0) = My.Resources.Refresh
        End Try
        Try
            NIconRefresh(1) = New Icon(dir + "\Icons\Refresh2.ico")
        Catch ex As Exception
            NIconRefresh(1) = My.Resources.Refresh2
        End Try
        Try
            NIconRefresh(2) = New Icon(dir + "\Icons\Refresh3.ico")
        Catch ex As Exception
            NIconRefresh(2) = My.Resources.Refresh3
        End Try
        Try
            NIconRefresh(3) = New Icon(dir + "\Icons\Refresh4.ico")
        Catch ex As Exception
            NIconRefresh(3) = My.Resources.Refresh4
        End Try
        'タブ見出し未読表示アイコン
        Try
            TabIcon = New Icon(dir + "\Icons\Tab.ico")
        Catch ex As Exception
            TabIcon = My.Resources.TabIcon
        End Try
        '画面のアイコン
        Try
            MainIcon = New Icon(dir + "\Icons\MIcon.ico")
        Catch ex As Exception
            MainIcon = My.Resources.MIcon
        End Try

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Visible = False

        Me.StatusStrip1.Items.Add(StatusLabel)

        LoadIcons() ' アイコン読み込み

        '発言保持クラス
        _statuses = TabInformations.GetInstance()

        'アイコン設定
        Me.Icon = MainIcon              'メインフォーム（TweenMain）
        NotifyIcon1.Icon = NIconAt      'タスクトレイ
        TabImage.Images.Add(TabIcon)    'タブ見出し

        ContextMenuStrip1.OwnerItem = Nothing
        ContextMenuStrip2.OwnerItem = Nothing
        ContextMenuTabProperty.OwnerItem = Nothing

        SettingDialog.Owner = Me
        SearchDialog.Owner = Me
        fDialog.Owner = Me
        TabDialog.Owner = Me
        UrlDialog.Owner = Me

        _history.Add("")
        _hisIdx = 0
        _reply_to_id = 0
        _reply_to_name = Nothing

        '_columnChangeFlag = True

        '<<<<<<<<<設定関連>>>>>>>>>
        '設定読み出し
        _config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath)
        _section = DirectCast(_config.GetSection("TwitterSetting"), ListSection)
        '初回起動時で設定ファイルがない場合、"TwitterSetting"セクションを作成。構成要素も作成することで他の要素もデフォルト値での取得が可能
        If _section Is Nothing Then
            _section = New ListSection()
            _config.Sections.Add("TwitterSetting", _section)
            _section = DirectCast(_config.GetSection("TwitterSetting"), ListSection)
            _section.SectionInformation.ForceSave = True
            _section.ListElement = New ListElementCollection("Recent")
        End If
        'ユーザー名とパスワードの取得
        _username = _section.UserName
        _password = _section.Password
        '新着バルーン通知のチェック状態設定
        NewPostPopMenuItem.Checked = _section.NewAllPop     '全新着通知

        'フォント＆文字色＆背景色保持
        _fntUnread = _section.FontUnread                '未読フォント
        _clUnread = _section.ColorUnread                '未読文字色
        _fntReaded = _section.FontReaded                '既読フォント
        _clReaded = _section.ColorReaded                '既読文字色
        _clFav = _section.ColorFav                      'Fav文字色
        _clOWL = _section.ColorOWL                      '片思い文字色
        _fntDetail = _section.FontDetail                '詳細部フォント
        _clSelf = _section.ColorSelf                    '自発言背景色
        _clAtSelf = _section.ColorAtSelf                '自分宛返信背景色
        _clTarget = _section.ColorTarget                '選択発言者の他の発言背景色
        _clAtTarget = _section.ColorAtTarget            '選択発言の＠先
        _clAtFromTarget = _section.ColorAtFromTarget    '選択発言者への＠
        _brsForeColorUnread = New SolidBrush(_clUnread)
        _brsForeColorReaded = New SolidBrush(_clReaded)
        _brsForeColorFav = New SolidBrush(_clFav)
        _brsForeColorOWL = New SolidBrush(_clOWL)
        _brsBackColorMine = New SolidBrush(_clSelf)
        _brsBackColorAt = New SolidBrush(_clAtSelf)
        _brsBackColorYou = New SolidBrush(_clTarget)
        _brsBackColorAtYou = New SolidBrush(_clAtTarget)
        _brsBackColorAtTo = New SolidBrush(_clAtFromTarget)
        _brsBackColorNone = New SolidBrush(Color.FromKnownColor(KnownColor.Window))
        detailHtmlFormat = detailHtmlFormat1 + _fntDetail.Name + detailHtmlFormat2 + _fntDetail.Size.ToString() + detailHtmlFormat3

        ' StringFormatオブジェクトへの事前設定
        sf.Alignment = StringAlignment.Near
        sf.LineAlignment = StringAlignment.Near

        '設定画面への反映
        SettingDialog.UserID = _username                                'ユーザ名
        SettingDialog.PasswordStr = _password                           'パスワード
        SettingDialog.TimelinePeriodInt = _section.TimelinePeriod       'Recent&Reply取得間隔
        SettingDialog.DMPeriodInt = _section.DMPeriod                   'DM取得間隔
        SettingDialog.NextPageThreshold = _section.NextPageThreshold    '次頁以降を取得するための新着件数閾値
        SettingDialog.NextPagesInt = _section.NextPages                 '閾値を超えた場合の取得ページ数
        SettingDialog.MaxPostNum = _section.MaxPostNum                  '時間当たりPOST回数最大値
        '起動時読み込みページ数
        SettingDialog.ReadPages = _section.ReadPages            'Recent
        SettingDialog.ReadPagesReply = _section.ReadPagesReply  'Reply
        SettingDialog.ReadPagesDM = _section.ReadPagesDM        'DM
        '起動時読み込み分を既読にするか。Trueなら既読として処理
        SettingDialog.Readed = _section.Readed
        '新着取得時のリストスクロールをするか。Trueならスクロールしない
        ListLockMenuItem.Checked = _section.ListLock
        SettingDialog.IconSz = _section.IconSize
        '文末ステータス
        SettingDialog.Status = _section.StatusText
        '未読管理。Trueなら未読管理する
        SettingDialog.UnreadManage = _section.UnreadManage
        'サウンド再生（タブ別設定より優先）
        SettingDialog.PlaySound = _section.PlaySound
        PlaySoundMenuItem.Checked = SettingDialog.PlaySound
        '片思い表示。Trueなら片思い表示する
        SettingDialog.OneWayLove = _section.OneWayLove
        'フォント＆文字色＆背景色
        SettingDialog.FontUnread = _fntUnread
        SettingDialog.ColorUnread = _clUnread
        SettingDialog.FontReaded = _fntReaded
        SettingDialog.ColorReaded = _clReaded
        SettingDialog.ColorFav = _clFav
        SettingDialog.ColorOWL = _clOWL
        SettingDialog.FontDetail = _fntDetail
        SettingDialog.ColorSelf = _clSelf
        SettingDialog.ColorAtSelf = _clAtSelf
        SettingDialog.ColorTarget = _clTarget
        SettingDialog.ColorAtTarget = _clAtTarget
        SettingDialog.ColorAtFromTarget = _clAtFromTarget
        SettingDialog.NameBalloon = _section.NameBalloon
        SettingDialog.PostCtrlEnter = _section.PostCtrlEnter
        SettingDialog.UseAPI = True
        SettingDialog.HubServer = _section.HubServer
        SettingDialog.BrowserPath = _section.BrowserPath
        SettingDialog.CheckReply = _section.CheckReply
        SettingDialog.UseRecommendStatus = _section.UseRecommendStatus
        SettingDialog.DispUsername = _section.DispUsername
        SettingDialog.CloseToExit = _section.CloseToExit
        SettingDialog.MinimizeToTray = _section.MinimizeToTray
        SettingDialog.DispLatestPost = _section.DispLatestPost
        SettingDialog.SortOrderLock = _section.SortOrderLock
        SettingDialog.TinyUrlResolve = _section.TinyURLResolve
        SettingDialog.ProxyType = _section.ProxyType
        SettingDialog.ProxyAddress = _section.ProxyAddress
        SettingDialog.ProxyPort = _section.ProxyPort
        SettingDialog.ProxyUser = _section.ProxyUser
        SettingDialog.ProxyPassword = _section.ProxyPassword
        SettingDialog.PeriodAdjust = _section.PeriodAdjust
        SettingDialog.StartupVersion = _section.StartupVersion
        SettingDialog.StartupKey = _section.StartupKey
        SettingDialog.StartupFollowers = _section.StartupFollowers
        SettingDialog.RestrictFavCheck = _section.RestrictFavCheck
        SettingDialog.AlwaysTop = _section.AlwaysTop
        SettingDialog.UrlConvertAuto = _section.UrlConvertAuto
        SettingDialog.OutputzEnabled = _section.Outputz
        SettingDialog.OutputzKey = _section.OutputzKey
        SettingDialog.OutputzUrlmode = _section.OutputzUrlmode
        SettingDialog.UseUnreadStyle = _section.UseUnreadStyle

        SettingDialog.DateTimeFormat = _section.DateTimeFormat
        '書式指定文字列エラーチェック
        Try
            If DateTime.Now.ToString(SettingDialog.DateTimeFormat).Length = 0 Then
                ' このブロックは絶対に実行されないはず
                ' 変換が成功した場合にLengthが0にならない
                SettingDialog.DateTimeFormat = "yyyy/MM/dd H:mm:ss"
            End If
        Catch ex As FormatException
            ' FormatExceptionが発生したら初期値を設定 (=yyyy/MM/dd H:mm:ssとみなされる)
            SettingDialog.DateTimeFormat = "yyyy/MM/dd H:mm:ss"
        End Try

        Outputz.key = SettingDialog.OutputzKey
        Outputz.Enabled = SettingDialog.OutputzEnabled
        Select Case SettingDialog.OutputzUrlmode
            Case OutputzUrlmode.twittercom
                Outputz.url = "http://twitter.com/"
            Case OutputzUrlmode.twittercomWithUsername
                Outputz.url = "http://twitter.com/" + SettingDialog.UserID
        End Select

        _initial = True

        'ユーザー名、パスワードが未設定なら設定画面を表示（初回起動時など）
        If _username = "" Or _password = "" Then
            '設定せずにキャンセルされた場合はプログラム終了
            If SettingDialog.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
                Application.Exit()  '強制終了
                Exit Sub
            End If
            _username = SettingDialog.UserID
            _password = SettingDialog.PasswordStr
            '設定されたが、依然ユーザー名とパスワードが未設定ならプログラム終了
            If _username = "" Or _password = "" Then
                Application.Exit()  '強制終了
                Exit Sub
            End If
            '新しい設定を反映
            'フォント＆文字色＆背景色保持
            _fntUnread = SettingDialog.FontUnread
            _clUnread = SettingDialog.ColorUnread
            _fntReaded = SettingDialog.FontReaded
            _clReaded = SettingDialog.ColorReaded
            _clFav = SettingDialog.ColorFav
            _clOWL = SettingDialog.ColorOWL
            _fntDetail = SettingDialog.FontDetail
            _clSelf = SettingDialog.ColorSelf
            _clAtSelf = SettingDialog.ColorAtSelf
            _clTarget = SettingDialog.ColorTarget
            _clAtTarget = SettingDialog.ColorAtTarget
            _clAtFromTarget = SettingDialog.ColorAtFromTarget
            _brsForeColorUnread.Dispose()
            _brsForeColorReaded.Dispose()
            _brsForeColorFav.Dispose()
            _brsForeColorOWL.Dispose()
            _brsForeColorUnread = New SolidBrush(_clUnread)
            _brsForeColorReaded = New SolidBrush(_clReaded)
            _brsForeColorFav = New SolidBrush(_clFav)
            _brsForeColorOWL = New SolidBrush(_clOWL)
            _brsBackColorMine.Dispose()
            _brsBackColorAt.Dispose()
            _brsBackColorYou.Dispose()
            _brsBackColorAtYou.Dispose()
            _brsBackColorAtTo.Dispose()
            _brsBackColorMine = New SolidBrush(_clSelf)
            _brsBackColorAt = New SolidBrush(_clAtSelf)
            _brsBackColorYou = New SolidBrush(_clTarget)
            _brsBackColorAtYou = New SolidBrush(_clAtTarget)
            _brsBackColorAtTo = New SolidBrush(_clAtFromTarget)
            detailHtmlFormat = detailHtmlFormat1 + _fntDetail.Name + detailHtmlFormat2 + _fntDetail.Size.ToString() + detailHtmlFormat3
            '他の設定項目は、随時設定画面で保持している値を読み出して使用
        End If

        'Twitter用通信クラス初期化
        Twitter.Username = _username
        Twitter.Password = _password
        Twitter.ProxyType = SettingDialog.ProxyType
        Twitter.ProxyAddress = SettingDialog.ProxyAddress
        Twitter.ProxyPort = SettingDialog.ProxyPort
        Twitter.ProxyUser = SettingDialog.ProxyUser
        Twitter.ProxyPassword = SettingDialog.ProxyPassword
        Twitter.NextThreshold = SettingDialog.NextPageThreshold   '次頁取得閾値
        Twitter.NextPages = SettingDialog.NextPagesInt    '閾値オーバー時の読み込みページ数（未使用）
        If IsNetworkAvailable() Then
            If SettingDialog.StartupFollowers Then
                _waitFollower = True
                GetTimeline(WORKERTYPE.Follower, 0, 0)
            End If
        End If

        'ウィンドウ設定
        Me.ClientSize = _section.FormSize           'サイズ設定
        _mySize = Me.ClientSize                     'サイズ保持（最小化・最大化されたまま終了した場合の対応用）
        Me.Location = _section.FormLocation         '位置設定
        _myLoc = Me.Location                        '位置保持（最小化・最大化されたまま終了した場合の対応用）
        Me.TopMost = SettingDialog.AlwaysTop
        _mySpDis = _section.SplitterDistance
        _mySpDis2 = _section.StatusTextHeight
        MultiLineMenuItem.Checked = _section.StatusMultiline
        Me.Tween_ClientSizeChanged(Me, Nothing)

        '全新着通知のチェック状態により、Reply＆DMの新着通知有効無効切り替え（タブ別設定にするため削除予定）
        If SettingDialog.UnreadManage = False Then
            ReadedStripMenuItem.Enabled = False
            UnreadStripMenuItem.Enabled = False
        End If

        'タイマー設定
        'Recent&Reply取得間隔
        If SettingDialog.TimelinePeriodInt > 0 Then
            TimerTimeline.Interval = SettingDialog.TimelinePeriodInt * 1000
        Else
            TimerTimeline.Interval = 600000
        End If
        'DM取得間隔
        If SettingDialog.DMPeriodInt > 0 Then
            TimerDM.Interval = SettingDialog.DMPeriodInt * 1000
        Else
            TimerDM.Interval = 600000
        End If
        '更新中アイコンアニメーション間隔
        TimerRefreshIcon.Interval = 85

        '状態表示部の初期化（画面右下）
        StatusLabel.Text = ""
        '文字カウンタ初期化
        lblLen.Text = "140"

        If SettingDialog.StartupKey Then
            Twitter.GetWedata()
        End If

        ''''''''''''''''''''''''''''''''''''''''
        _statuses.SortOrder = DirectCast(_section.SortOrder, System.Windows.Forms.SortOrder)
        Dim mode As IdComparerClass.ComparerMode
        Select Case _section.SortColumn
            Case 0, 5, 6    '0:アイコン,5:未読マーク,6:プロテクト・フィルターマーク
                'ソートしない
                mode = IdComparerClass.ComparerMode.Id  'Idソートに読み替え
            Case 1  'ニックネーム
                mode = IdComparerClass.ComparerMode.Nickname
            Case 2  '本文
                mode = IdComparerClass.ComparerMode.Data
            Case 3  '時刻=発言Id
                mode = IdComparerClass.ComparerMode.Id
            Case 4  '名前
                mode = IdComparerClass.ComparerMode.Name
            Case 7  'Source
                mode = IdComparerClass.ComparerMode.Source
        End Select
        _statuses.SortMode = mode
        ''''''''''''''''''''''''''''''''''''''''

        '_iconCol = False
        Select Case SettingDialog.IconSz
            Case IconSizes.IconNone
                _iconSz = 0
            Case IconSizes.Icon16
                _iconSz = 16
            Case IconSizes.Icon24
                _iconSz = 26
            Case IconSizes.Icon48
                _iconSz = 48
            Case IconSizes.Icon48_2
                _iconSz = 48
                _iconCol = True
        End Select
        If _iconSz = 0 Then
            Twitter.GetIcon = False
        Else
            Twitter.GetIcon = True
            Twitter.IconSize = _iconSz
        End If
        Twitter.UseAPI = SettingDialog.UseAPI
        Twitter.HubServer = SettingDialog.HubServer
        Twitter.TinyUrlResolve = SettingDialog.TinyUrlResolve

        '発言詳細部アイコンをリストアイコンにサイズ変更
        Dim sz As Integer = _iconSz
        If _iconSz = 0 Then
            sz = 16
        End If
        TIconSmallList = New ImageList
        TIconSmallList.ImageSize = New Size(sz, sz)
        TIconSmallList.ColorDepth = ColorDepth.Depth32Bit
        '発言詳細部のアイコンリスト作成
        TIconDic = New Dictionary(Of String, Image)

        Twitter.ListIcon = TIconSmallList
        Twitter.DetailIcon = TIconDic

        StatusLabel.Text = My.Resources.Form1_LoadText1       '画面右下の状態表示を変更
        StatusLabelUrl.Text = ""            '画面左下のリンク先URL表示部を初期化
        'PostedText.Text = ""
        PostBrowser.DocumentText = ""       '発言詳細部初期化
        'PostedText.RemoveLinks()
        NameLabel.Text = ""                 '発言詳細部名前ラベル初期化
        DateTimeLabel.Text = ""             '発言詳細部日時ラベル初期化

        '<<<<<<<<タブ関連>>>>>>>
        'Recentタブ
        'Timeline.SmallImageList = TIconSmallList
        'listViewItemSorter = New ListViewItemComparer
        'listViewItemSorter.ColumnModes = New ListViewItemComparer.ComparerMode() _
        '        {ListViewItemComparer.ComparerMode.None, _
        '         ListViewItemComparer.ComparerMode.String, _
        '         ListViewItemComparer.ComparerMode.String, _
        '         ListViewItemComparer.ComparerMode.DateTime, _
        '         ListViewItemComparer.ComparerMode.String}
        'listViewItemSorter.Column = _section.SortColumn
        'listViewItemSorter.Order = DirectCast(_section.SortOrder, System.Windows.Forms.SortOrder)

        ''Replyタブ
        If _section.ListElement.Item("Reply") Is Nothing Then
            _section.ListElement.Add(New ListElement("Reply"))
        End If

        ''DirectMsgタブ
        If _section.ListElement.Item("Direct") Is Nothing Then
            _section.ListElement.Add(New ListElement("Direct"))
        End If

        For idx As Integer = 0 To _section.ListElement.Count - 1
            Dim name As String = _section.ListElement(idx).Name
            Dim tb As New TabClass
            tb.Notify = _section.ListElement(idx).Notify
            tb.SoundFile = _section.ListElement(idx).SoundFile
            tb.UnreadManage = _section.ListElement(idx).UnreadManage
            If Not AddNewTab(name, True) Then Throw New Exception("タブ作成エラー") 'GUI部品作成
            For Each flt As Tween.SelectedUser In _section.SelectedUser
                If flt.TabName = name Then
                    Dim bflt() As String = flt.BodyFilter.Split(Chr(32))
                    Dim body As New List(Of String)
                    For Each tmpFlt As String In bflt
                        Try
                            Dim dmy As Boolean = False
                            If flt.RegexEnable Then
                                ' 正規表現が正しいかどうかチェック 不正な場合はArgumentExceptionが発生するのでフィルタを無視する
                                Dim rx As Regex = New Regex(tmpFlt)
                                dmy = rx.IsMatch(tmpFlt)
                            End If
                            ' フィルタ追加 ArgumentExceptionが発生した場合はCatchされるのでここに来ない
                            If tmpFlt.Trim <> "" Then body.Add(tmpFlt.Trim)
                        Catch ex As ArgumentException
                            ' ArgumentExceptionが発生した場合は該当フィルタを無視
                        End Try
                    Next
                    tb.AddFilter(New FiltersClass(flt.IdFilter, _
                            body, _
                            flt.SearchBoth, _
                            flt.MoveFrom, _
                            flt.SetMark, _
                            flt.UrlSearch, _
                            flt.RegexEnable))
                End If
            Next
            _statuses.AddTab(name, tb)
        Next

        AddHandler My.Computer.Network.NetworkAvailabilityChanged, AddressOf Network_NetworkAvailabilityChanged
        If SettingDialog.MinimizeToTray = False OrElse Me.WindowState <> FormWindowState.Minimized Then
            Me.Visible = True
        End If
        _curTab = ListTab.SelectedTab
        _curItemIndex = -1
        _curList = DirectCast(_curTab.Controls(0), DetailsListView)
        SetMainWindowTitle()
        SetNotifyIconText()

        TimerColorize.Interval = 200
        TimerColorize.Start()
    End Sub

    Private Sub Network_NetworkAvailabilityChanged(ByVal sender As Object, ByVal e As Devices.NetworkAvailableEventArgs)
        If e.IsNetworkAvailable Then
            Dim args As New GetWorkerArg()
            PostButton.Enabled = True
            FavAddToolStripMenuItem.Enabled = True
            FavRemoveToolStripMenuItem.Enabled = True
            MoveToHomeToolStripMenuItem.Enabled = True
            MoveToFavToolStripMenuItem.Enabled = True
            DeleteStripMenuItem.Enabled = True
            RefreshStripMenuItem.Enabled = True
            TimerRefreshIcon.Enabled = False
            NotifyIcon1.Icon = NIconAt
            If Not _initial Then
                If SettingDialog.DMPeriodInt > 0 Then TimerDM.Enabled = True
                If SettingDialog.TimelinePeriodInt > 0 Then TimerTimeline.Enabled = True
            Else
                GetTimeline(WORKERTYPE.DirectMessegeRcv, 1, 0)
            End If
        Else
            TimerRefreshIcon.Enabled = False
            NotifyIcon1.Icon = NIconAtSmoke
            PostButton.Enabled = False
            FavAddToolStripMenuItem.Enabled = False
            FavRemoveToolStripMenuItem.Enabled = False
            MoveToHomeToolStripMenuItem.Enabled = False
            MoveToFavToolStripMenuItem.Enabled = False
            DeleteStripMenuItem.Enabled = False
            RefreshStripMenuItem.Enabled = False
        End If
    End Sub

    Private Sub TimerTimeline_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerTimeline.Tick

        If Not IsNetworkAvailable() Then Exit Sub

        GetTimeline(WORKERTYPE.Timeline, 1, 0)
    End Sub

    Private Sub TimerDM_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerDM.Tick
        GC.Collect()

        If Not IsNetworkAvailable() Then Exit Sub

        GetTimeline(WORKERTYPE.DirectMessegeRcv, 1, 0)
    End Sub

    Private Sub RefreshTimeline()
        'スクロール制御準備
        Dim smode As Integer = -1    '-1:制御しない,-2:最新へ,その他:topitem使用
        Dim topId As Long = -1
        Dim befCnt As Integer = _curList.VirtualListSize

        '_curList.BeginUpdate()
        If _curList.VirtualListSize > 0 Then
            If _statuses.SortMode = IdComparerClass.ComparerMode.Id Then
                If _statuses.SortOrder = SortOrder.Ascending Then
                    'Id昇順
                    If ListLockMenuItem.Checked Then
                        '制御しない
                        'smode = -1
                        '現在表示位置へ強制スクロール
                        topId = _statuses.GetId(_curTab.Text, _curList.TopItem.Index)
                        smode = 0
                    Else
                        '最下行が表示されていたら、最下行へ強制スクロール。最下行が表示されていなかったら制御しない
                        Dim _item As ListViewItem
                        _item = _curList.GetItemAt(0, _curList.ClientSize.Height - 1)   '一番下
                        If _item Is Nothing Then _item = _curList.Items(_curList.Items.Count - 1)
                        If _item.Index = _curList.Items.Count - 1 Then
                            smode = -2
                        Else
                            'smode = -1
                            topId = _statuses.GetId(_curTab.Text, _curList.TopItem.Index)
                            smode = 0
                        End If
                    End If
                Else
                    'Id降順
                    If ListLockMenuItem.Checked Then
                        '現在表示位置へ強制スクロール
                        topId = _statuses.GetId(_curTab.Text, _curList.TopItem.Index)
                        smode = 0
                    Else
                        '最上行が表示されていたら、制御しない。最上行が表示されていなかったら、現在表示位置へ強制スクロール
                        Dim _item As ListViewItem

                        _item = _curList.GetItemAt(0, 10)     '一番上
                        If _item Is Nothing Then _item = _curList.Items(0)
                        If _item.Index = 0 Then
                            smode = -3  '最上行
                        Else
                            topId = _statuses.GetId(_curTab.Text, _curList.TopItem.Index)
                            smode = 0
                        End If
                    End If
                End If
            Else
                '現在表示位置へ強制スクロール
                topId = _statuses.GetId(_curTab.Text, _curList.TopItem.Index)
                smode = 0
            End If
        Else
            smode = -1
        End If

        '現在の選択状態を退避
        Dim selId() As Long = Nothing
        Dim focusedId As Long
        If _curList.SelectedIndices.Count < 31 Then
            selId = _statuses.GetId(_curTab.Text, _curList.SelectedIndices)
        Else
            selId = New Long(0) {-1}
        End If
        If _curList.FocusedItem IsNot Nothing Then
            focusedId = _statuses.GetId(_curTab.Text, _curList.FocusedItem.Index)
        Else
            focusedId = -1
        End If
        '_curList.EndUpdate()

        '更新確定
        Dim notifyPosts() As PostClass = Nothing
        Dim soundFile As String = ""
        Dim addCount As Integer = 0
        addCount = _statuses.SubmitUpdate(soundFile, notifyPosts)

        'リストに反映＆選択状態復元
        For Each tab As TabPage In ListTab.TabPages
            Dim lst As DetailsListView = DirectCast(tab.Controls(0), DetailsListView)
            Dim tabInfo As TabClass = _statuses.Tabs(tab.Text)
            lst.BeginUpdate()
            If lst.VirtualListSize <> tabInfo.AllCount Then
                If lst.Equals(_curList) Then
                    '_curList.BeginUpdate()
                    _itemCache = Nothing
                    _postCache = Nothing
                End If
                lst.VirtualListSize = tabInfo.AllCount 'リスト件数更新
                If lst.Equals(_curList) Then
                    Me.SelectListItem(lst, _
                                      _statuses.GetIndex(tab.Text, selId), _
                                      _statuses.GetIndex(tab.Text, focusedId))
                    '_curList.EndUpdate()
                End If
            End If
            lst.EndUpdate()
            If tabInfo.UnreadCount > 0 AndAlso tab.ImageIndex = -1 Then tab.ImageIndex = 0 'タブアイコン
        Next

        'スクロール制御後処理
        '_curList.BeginUpdate()
        If befCnt <> _curList.VirtualListSize Then
            Select Case smode
                Case -3
                    '最上行
                    _curList.EnsureVisible(0)
                Case -2
                    '最下行へ
                    _curList.EnsureVisible(_curList.VirtualListSize - 1)
                Case -1
                    '制御しない
                Case Else
                    '表示位置キープ
                    If _curList.VirtualListSize > 0 Then
                        _curList.EnsureVisible(_curList.VirtualListSize - 1)
                        _curList.EnsureVisible(_statuses.GetIndex(_curTab.Text, topId))
                    End If
            End Select
        End If
        '_curList.EndUpdate()

        '新着通知
        If NewPostPopMenuItem.Checked AndAlso _
           notifyPosts IsNot Nothing AndAlso notifyPosts.Length > 0 AndAlso _
           Not _initial Then
            Dim sb As New StringBuilder
            Dim reply As Boolean = False
            Dim dm As Boolean = False
            For Each post As PostClass In notifyPosts
                If post.IsReply Then reply = True
                If post.IsDm Then dm = True
                If sb.Length > 0 Then sb.Append(System.Environment.NewLine)
                Select Case SettingDialog.NameBalloon
                    Case NameBalloonEnum.UserID
                        sb.Append(post.Name).Append(" : ")
                    Case NameBalloonEnum.NickName
                        sb.Append(post.Nickname).Append(" : ")
                End Select
                sb.Append(post.Data)
            Next
            If SettingDialog.DispUsername Then NotifyIcon1.BalloonTipTitle = _username + " - " Else NotifyIcon1.BalloonTipTitle = ""
            If dm Then
                NotifyIcon1.BalloonTipIcon = ToolTipIcon.Warning
                NotifyIcon1.BalloonTipTitle += "Tween [DM] " + My.Resources.RefreshDirectMessageText1 + " " + addCount.ToString() + My.Resources.RefreshDirectMessageText2
            ElseIf reply Then
                NotifyIcon1.BalloonTipIcon = ToolTipIcon.Warning
                NotifyIcon1.BalloonTipTitle += "Tween [Reply!] " + My.Resources.RefreshTimelineText1 + " " + addCount.ToString() + My.Resources.RefreshTimelineText2
            Else
                NotifyIcon1.BalloonTipIcon = ToolTipIcon.Info
                NotifyIcon1.BalloonTipTitle += "Tween " + My.Resources.RefreshTimelineText1 + " " + addCount.ToString() + My.Resources.RefreshTimelineText2
            End If
            NotifyIcon1.BalloonTipText = sb.ToString()
            NotifyIcon1.ShowBalloonTip(500)
        End If

        '★★★リストリフレッシュ必要か？要検証★★★

        'サウンド再生
        If Not _initial AndAlso SettingDialog.PlaySound AndAlso soundFile <> "" Then
            Try
                My.Computer.Audio.Play(My.Application.Info.DirectoryPath.ToString() + "\" + soundFile, AudioPlayMode.Background)
            Catch ex As Exception

            End Try
        End If

        SetMainWindowTitle()
        If Not StatusLabelUrl.Text.StartsWith("http") Then SetStatusLabel()
        'TimerColorize.Stop()
        'TimerColorize.Start()
    End Sub

    Private Sub Mylist_Scrolled(ByVal sender As Object, ByVal e As System.EventArgs)
        'TimerColorize.Stop()
        'TimerColorize.Start()
    End Sub

    Private Sub MyList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If _curList.SelectedIndices.Count <> 1 Then Exit Sub
        'If _curList.SelectedIndices.Count = 0 Then Exit Sub

        _curItemIndex = _curList.SelectedIndices(0)
        _curPost = GetCurTabPost(_curItemIndex)
        If SettingDialog.UnreadManage Then _statuses.SetRead(True, _curTab.Text, _curItemIndex)
        'MyList.RedrawItems(MyList.SelectedIndices(0), MyList.SelectedIndices(0), False)   'RetrieveVirtualItemが発生することを期待
        'キャッシュの書き換え
        ChangeCacheStyleRead(True, _curItemIndex, _curTab)   '既読へ（フォント、文字色）

        'ColorizeList(-1)    '全キャッシュ更新（背景色）
        'DispSelectedPost()
        ColorizeList()
        TimerColorize.Stop()
        TimerColorize.Start()
        'cMode = 1
    End Sub

    Private Sub ChangeCacheStyleRead(ByVal Read As Boolean, ByVal Index As Integer, ByVal Tab As TabPage)
        'Read:True=既読 False=未読
        '未読管理していなかったら既読として扱う
        If Not _statuses.Tabs(_curTab.Text).UnreadManage OrElse _
           Not SettingDialog.UnreadManage Then Read = True

        '対象の特定
        Dim itm As ListViewItem
        Dim post As PostClass
        If Tab.Equals(_curTab) AndAlso _itemCache IsNot Nothing AndAlso Index >= _itemCacheIndex AndAlso Index < _itemCacheIndex + _itemCache.Length Then
            itm = _itemCache(Index - _itemCacheIndex)
            post = _postCache(Index - _itemCacheIndex)
        Else
            itm = DirectCast(Tab.Controls(0), DetailsListView).Items(Index)
            post = _statuses.Item(Tab.Text, Index)
        End If

        ChangeItemStyleRead(Read, itm, post, DirectCast(Tab.Controls(0), DetailsListView))
    End Sub

    Private Sub ChangeItemStyleRead(ByVal Read As Boolean, ByVal Item As ListViewItem, ByVal Post As PostClass, ByVal DList As DetailsListView)
        Dim fnt As Font
        'フォント
        If Read Then
            fnt = _fntReaded
            Item.SubItems(5).Text = ""
        Else
            fnt = _fntUnread
            Item.SubItems(5).Text = "★"
        End If
        '文字色
        Dim cl As Color
        If Post.IsFav Then
            cl = _clFav
        ElseIf Post.IsOwl AndAlso SettingDialog.OneWayLove Then
            cl = _clOWL
        ElseIf Read OrElse Not SettingDialog.UseUnreadStyle Then
            cl = _clReaded
        Else
            cl = _clUnread
        End If
        If DList Is Nothing OrElse Item.Index = -1 Then
            Item.ForeColor = cl
            If SettingDialog.UseUnreadStyle Then
                Item.Font = fnt
            End If
        Else
            DList.Update()
            If SettingDialog.UseUnreadStyle Then
                DList.ChangeItemFontAndColor(Item.Index, cl, fnt)
            Else
                DList.ChangeItemForeColor(Item.Index, cl)
            End If
            'If _itemCache IsNot Nothing Then DList.RedrawItems(_itemCacheIndex, _itemCacheIndex + _itemCache.Length - 1, False)
        End If
    End Sub

    Private Sub ColorizeList()
        'Index:更新対象のListviewItem.Index。Colorを返す。
        '-1は全キャッシュ。Colorは返さない（ダミーを戻す）
        Dim _post As PostClass
        If _anchorFlag Then
            _post = _anchorPost
        Else
            _post = _curPost
        End If

        If _itemCache Is Nothing Then Exit Sub

        'For cnt As Integer = 0 To _itemCache.Length - 1
        '    If Not _postCache(cnt).IsRead AndAlso SettingDialog.UnreadManage AndAlso _statuses.Tabs(_curTab.Text).UnreadManage Then
        '        _itemCache(cnt).Font = _fntUnread
        '    Else
        '        _itemCache(cnt).Font = _fntReaded
        '    End If
        'Next

        If _post Is Nothing Then Exit Sub

        For cnt As Integer = 0 To _itemCache.Length - 1
            '_itemCache(cnt).BackColor = JudgeColor(_post, _postCache(cnt))
            _curList.ChangeItemBackColor(_itemCacheIndex + cnt, JudgeColor(_post, _postCache(cnt)))
        Next
    End Sub

    Private Sub ColorizeList(ByVal Item As ListViewItem, ByVal Index As Integer)
        'Index:更新対象のListviewItem.Index。Colorを返す。
        '-1は全キャッシュ。Colorは返さない（ダミーを戻す）
        Dim _post As PostClass
        If _anchorFlag Then
            _post = _anchorPost
        Else
            _post = _curPost
        End If

        Dim tPost As PostClass = GetCurTabPost(Index)

        'If Not tPost.IsRead AndAlso SettingDialog.UnreadManage AndAlso _statuses.Tabs(_curTab.Text).UnreadManage Then
        '    Item.Font = _fntUnread
        'Else
        '    Item.Font = _fntReaded
        'End If

        If _post Is Nothing Then Exit Sub

        If Item.Index = -1 Then
            Item.BackColor = JudgeColor(_post, tPost)
        Else
            _curList.ChangeItemBackColor(Item.Index, JudgeColor(_post, tPost))
        End If
    End Sub

    Private Function JudgeColor(ByVal BasePost As PostClass, ByVal TargetPost As PostClass) As Color
        Dim cl As Color
        If TargetPost.IsMe Then
            '自分=発言者
            cl = _clSelf
        ElseIf TargetPost.Name.Equals(BasePost.Name, StringComparison.OrdinalIgnoreCase) Then
            '発言者
            cl = _clTarget
        ElseIf TargetPost.IsReply Then
            '自分宛返信
            cl = _clAtSelf
        ElseIf BasePost.ReplyToList.Contains(TargetPost.Name.ToLower()) Then
            '返信先
            cl = _clAtFromTarget
        ElseIf TargetPost.ReplyToList.Contains(BasePost.Name.ToLower()) Then
            'その人への返信
            cl = _clAtTarget
        Else
            'その他
            cl = System.Drawing.SystemColors.Window
        End If
        Return cl
    End Function

    Private Sub PostButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PostButton.Click
        If StatusText.Text.Trim.Length = 0 Then
            DoRefresh()
            Exit Sub
        End If

        _history(_history.Count - 1) = StatusText.Text.Trim

        If SettingDialog.UrlConvertAuto Then UrlConvertAutoToolStripMenuItem_Click(Nothing, Nothing)

        Dim args As New GetWorkerArg()
        args.page = 0
        args.endPage = 0
        args.type = WORKERTYPE.PostMessage
        If (StatusText.Text.StartsWith("D ")) OrElse (My.Computer.Keyboard.ShiftKeyDown) Then
            args.status = StatusText.Text.Trim
        ElseIf SettingDialog.UseRecommendStatus() Then
            ' 推奨ステータスを使用する
            Dim statregex As New Regex("^0*")
            args.status = StatusText.Text.Trim() + " [TWNv" + statregex.Replace(My.Application.Info.Version.Major.ToString() + My.Application.Info.Version.Minor.ToString() + My.Application.Info.Version.Build.ToString() + My.Application.Info.Version.Revision.ToString(), "") + "]"
        Else
            ' テキストボックスに入力されている文字列を使用する
            args.status = StatusText.Text.Trim() + " " + SettingDialog.Status.Trim()
        End If

        Dim regex As New Regex("^[+\-\[\]\s\\.,*/(){}^~|='&%$#""<>?]*(get|g|fav|follow|f|on|off|stop|quit|leave|l|whois|w|nudge|n|stats|invite|track|untrack|tracks|tracking|\*)([+\-\[\]\s\\.,*/(){}^~|='&%$#""<>?]+|$)", RegexOptions.IgnoreCase)
        'Dim regex2 As New Regex("https?:\/\/[-_.!~*'()a-zA-Z0-9;\/?:\@&=+\$,%#]+")
        'Dim mc2 As Match = regex2.Match(args.status)

        If regex.IsMatch(args.status) AndAlso args.status.EndsWith(" .") = False Then args.status += " ."
        'If mc2.Success Then args.status = regex2.Replace(args.status, "$& ")

        RunAsync(args)

        ListTab.SelectedTab.Controls(0).Focus()
    End Sub

    Private Sub EndToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EndToolStripMenuItem.Click
        _endingFlag = True
        Me.Close()
    End Sub

    Private Sub Tween_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If Not SettingDialog.CloseToExit AndAlso e.CloseReason = CloseReason.UserClosing AndAlso _endingFlag = False Then
            '_endingFlag=False:フォームの×ボタン
            e.Cancel = True
            Me.Visible = False
        Else
            TimerTimeline.Enabled = False
            TimerDM.Enabled = False

            _endingFlag = True
            Twitter.Ending = True

            SaveConfigs()

            For i As Integer = 0 To _bw.Length - 1
                If _bw(i) IsNot Nothing Then _bw(i).CancelAsync()
            Next

            Dim flg As Boolean = False
            Do
                flg = True
                For i As Integer = 0 To _bw.Length - 1
                    If _bw(i) IsNot Nothing AndAlso _bw(i).IsBusy Then
                        flg = False
                        Exit For
                    End If
                Next
                Threading.Thread.Sleep(500)
                Application.DoEvents()
            Loop Until flg = True

            Me.Visible = False
            NotifyIcon1.Visible = False
        End If
    End Sub

    Private Sub NotifyIcon1_BalloonTipClicked(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NotifyIcon1.BalloonTipClicked
        Me.Visible = True
        If Me.WindowState = FormWindowState.Minimized Then
            Me.WindowState = FormWindowState.Normal
        End If
        Me.Activate()
    End Sub

    Private Sub GetTimelineWorker_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        Dim bw As BackgroundWorker = DirectCast(sender, BackgroundWorker)
        If bw.CancellationPending OrElse _endingFlag Then
            e.Cancel = True
            Exit Sub
        End If

        Threading.Thread.CurrentThread.Priority = Threading.ThreadPriority.BelowNormal

        Dim ret As String = ""
        Dim rslt As New GetWorkerResult()

        Dim read As Boolean = Not SettingDialog.UnreadManage
        If _initial AndAlso SettingDialog.UnreadManage Then read = SettingDialog.Readed

        Dim args As GetWorkerArg = DirectCast(e.Argument, GetWorkerArg)


        If args.type <> WORKERTYPE.OpenUri Then bw.ReportProgress(0, "") 'Notifyアイコンアニメーション開始

        Select Case args.type
            Case WORKERTYPE.Timeline, WORKERTYPE.Reply
                bw.ReportProgress(50, MakeStatusMessage(args, False))
                ret = Twitter.GetTimeline(args.page, read, args.endPage, args.type, rslt.newDM)
                rslt.addCount = _statuses.DistributePosts()
            Case WORKERTYPE.DirectMessegeRcv    '送信分もまとめて取得
                bw.ReportProgress(50, MakeStatusMessage(args, False))
                ret = Twitter.GetDirectMessage(args.page, read, args.endPage, args.type)
                rslt.addCount = _statuses.DistributePosts()
            Case WORKERTYPE.FavAdd
                'スレッド処理はしない
                For i As Integer = 0 To args.ids.Count - 1
                    Dim post As PostClass = _statuses.Item(args.ids(i))
                    args.page = i + 1
                    bw.ReportProgress(50, MakeStatusMessage(args, False))
                    If Not post.IsFav Then
                        ret = Twitter.PostFavAdd(post.Id)
                        If ret.Length = 0 Then
                            args.sIds.Add(post.Id)
                            post.IsFav = True    'リスト再描画必要
                            _favTimestamps.Add(Now)
                        End If
                    End If
                Next
                rslt.sIds = args.sIds
            Case WORKERTYPE.FavRemove
                'スレッド処理はしない
                For i As Integer = 0 To args.ids.Count - 1
                    Dim post As PostClass = _statuses.Item(args.ids(i))
                    args.page = i + 1
                    bw.ReportProgress(50, MakeStatusMessage(args, False))
                    If post.IsFav Then
                        ret = Twitter.PostFavRemove(post.Id)
                        If ret.Length = 0 Then
                            args.sIds.Add(post.Id)
                            post.IsFav = False    'リスト再描画必要
                        End If
                    End If
                Next
                rslt.sIds = args.sIds
                ' Contributed by shuyoko <http://twitter.com/shuyoko> BEGIN:
            Case WORKERTYPE.BlackFavAdd
                'スレッド処理はしない
                For i As Integer = 0 To args.ids.Count - 1
                    Dim post As PostClass = _statuses.Item(args.ids(i))
                    Dim blackid As Long = 0
                    args.page = i + 1
                    bw.ReportProgress(50, MakeStatusMessage(args, False))
                    If Not post.IsFav Then
                        ret = Twitter.GetBlackFavId(post.Id, blackid)
                        If ret.Length = 0 Then
                            ret = Twitter.PostFavAdd(blackid)
                            If ret.Length = 0 Then
                                args.sIds.Add(post.Id)
                                post.IsFav = True    'リスト再描画必要
                                _favTimestamps.Add(Now)
                            End If
                        End If
                    End If
                Next
                rslt.sIds = args.sIds
                ' Contributed by shuyoko <http://twitter.com/shuyoko> END.
            Case WORKERTYPE.PostMessage
                bw.ReportProgress(200)
                CheckReplyTo(args.status)
                ret = Twitter.PostStatus(args.status, _reply_to_id)
                _reply_to_id = 0
                _reply_to_name = Nothing
                bw.ReportProgress(300)
            Case WORKERTYPE.Follower
                bw.ReportProgress(50, My.Resources.UpdateFollowersMenuItem1_ClickText1)
                ret = Twitter.GetFollowers(False)       ' Followersリストキャッシュ有効
            Case WORKERTYPE.OpenUri
                Dim myPath As String = Convert.ToString(args.status)

                Try
                    If SettingDialog.BrowserPath <> "" Then
                        Shell(SettingDialog.BrowserPath & " " & myPath)
                    Else
                        System.Diagnostics.Process.Start(myPath)
                    End If
                Catch ex As Exception
                    '                MessageBox.Show("ブラウザの起動に失敗、またはタイムアウトしました。" + ex.ToString())
                End Try
        End Select

        'キャンセル要求
        If bw.CancellationPending Then
            e.Cancel = True
            Exit Sub
        End If

        '時速表示用
        If args.type = WORKERTYPE.FavAdd OrElse args.type = WORKERTYPE.BlackFavAdd Then
            Dim oneHour As Date = Now.Subtract(New TimeSpan(1, 0, 0))
            For i As Integer = _favTimestamps.Count - 1 To 0 Step -1
                If _favTimestamps(i).CompareTo(oneHour) < 0 Then
                    _favTimestamps.RemoveAt(i)
                End If
            Next
        End If
        If args.type = WORKERTYPE.Timeline AndAlso Not _initial Then
            SyncLock _syncObject
                Dim tm As Date = Now
                If _tlTimestamps.ContainsKey(tm) Then
                    _tlTimestamps(tm) += rslt.addCount
                Else
                    _tlTimestamps.Add(Now, rslt.addCount)
                End If
                Dim oneHour As Date = Now.Subtract(New TimeSpan(1, 0, 0))
                Dim keys As New List(Of Date)
                _tlCount = 0
                For Each key As Date In _tlTimestamps.Keys
                    If key.CompareTo(oneHour) < 0 Then
                        keys.Add(key)
                    Else
                        _tlCount += _tlTimestamps(key)
                    End If
                Next
                For Each key As Date In keys
                    _tlTimestamps.Remove(key)
                Next
                keys.Clear()
            End SyncLock
        End If

        '終了ステータス
        If args.type <> WORKERTYPE.OpenUri Then bw.ReportProgress(100, MakeStatusMessage(args, True)) 'ステータス書き換え、Notifyアイコンアニメーション開始

        rslt.retMsg = ret
        rslt.type = args.type
        rslt.tName = args.tName
        If args.type = WORKERTYPE.DirectMessegeRcv OrElse _
           args.type = WORKERTYPE.DirectMessegeSnt OrElse _
           args.type = WORKERTYPE.Reply OrElse _
           args.type = WORKERTYPE.Timeline Then
            rslt.page = args.page - 1   '値が正しいか後でチェック。10ページ毎の継続確認
        End If

        e.Result = rslt

    End Sub

    Private Function MakeStatusMessage(ByVal AsyncArg As GetWorkerArg, ByVal Finish As Boolean) As String
        Dim smsg As String = ""
        If Not Finish Then
            '継続中メッセージ
            Select Case AsyncArg.type
                Case WORKERTYPE.Timeline
                    smsg = My.Resources.GetTimelineWorker_RunWorkerCompletedText5 + AsyncArg.page.ToString() + My.Resources.GetTimelineWorker_RunWorkerCompletedText6
                Case WORKERTYPE.Reply
                    smsg = My.Resources.GetTimelineWorker_RunWorkerCompletedText4 + AsyncArg.page.ToString() + My.Resources.GetTimelineWorker_RunWorkerCompletedText6
                Case WORKERTYPE.DirectMessegeRcv
                    smsg = My.Resources.GetTimelineWorker_RunWorkerCompletedText8 + AsyncArg.page.ToString() + My.Resources.GetTimelineWorker_RunWorkerCompletedText6
                    'Case WORKERTYPE.DirectMessegeSnt
                    '    smsg = My.Resources.GetTimelineWorker_RunWorkerCompletedText12 + AsyncArg.page.ToString() + My.Resources.GetTimelineWorker_RunWorkerCompletedText6
                Case WORKERTYPE.FavAdd
                    smsg = My.Resources.GetTimelineWorker_RunWorkerCompletedText15 + AsyncArg.page.ToString() + "/" + AsyncArg.ids.Count.ToString() + _
                                        My.Resources.GetTimelineWorker_RunWorkerCompletedText16 + (AsyncArg.page - AsyncArg.sIds.Count - 1).ToString()
                Case WORKERTYPE.FavRemove
                    smsg = My.Resources.GetTimelineWorker_RunWorkerCompletedText17 + AsyncArg.page.ToString() + "/" + AsyncArg.ids.Count.ToString() + _
                                        My.Resources.GetTimelineWorker_RunWorkerCompletedText18 + (AsyncArg.page - AsyncArg.sIds.Count - 1).ToString()
                Case WORKERTYPE.BlackFavAdd
                    smsg = My.Resources.GetTimelineWorker_RunWorkerCompletedText15_black + AsyncArg.page.ToString() + "/" + AsyncArg.ids.Count.ToString() + _
                                        My.Resources.GetTimelineWorker_RunWorkerCompletedText16 + (AsyncArg.page - AsyncArg.sIds.Count - 1).ToString()
            End Select
        Else
            '完了メッセージ
            Select Case AsyncArg.type
                Case WORKERTYPE.Timeline
                    smsg = My.Resources.GetTimelineWorker_RunWorkerCompletedText1
                Case WORKERTYPE.Reply
                    smsg = My.Resources.GetTimelineWorker_RunWorkerCompletedText9
                Case WORKERTYPE.DirectMessegeRcv
                    smsg = My.Resources.GetTimelineWorker_RunWorkerCompletedText11
                Case WORKERTYPE.DirectMessegeSnt
                    smsg = My.Resources.GetTimelineWorker_RunWorkerCompletedText13
                Case WORKERTYPE.FavAdd
                    '進捗メッセージ残す
                Case WORKERTYPE.FavRemove
                    '進捗メッセージ残す
                Case WORKERTYPE.BlackFavAdd
                    '進捗メッセージ残す
            End Select
        End If
        Return smsg
    End Function

    Private Sub GetTimelineWorker_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs)
        If _endingFlag Then Exit Sub
        If e.ProgressPercentage > 100 Then
            '発言投稿
            If e.ProgressPercentage = 200 Then    '開始
                StatusLabel.Text = "Posting..."
                StatusText.Enabled = False
                PostButton.Enabled = False
                ReplyStripMenuItem.Enabled = False
                DMStripMenuItem.Enabled = False
            End If
            If e.ProgressPercentage = 300 Then  '終了
                StatusLabel.Text = My.Resources.PostWorker_RunWorkerCompletedText4
                StatusText.Enabled = True
                PostButton.Enabled = True
                ReplyStripMenuItem.Enabled = True
                DMStripMenuItem.Enabled = True
                NotifyIcon1.Icon = NIconAt
            End If
        Else
            Dim smsg As String = DirectCast(e.UserState, String)
            If smsg.Length > 0 Then StatusLabel.Text = smsg
            If e.ProgressPercentage = 0 Then    '開始
                TimerRefreshIcon.Enabled = True
            End If
            If e.ProgressPercentage = 100 Then  '終了
                Dim cnt As Integer = 0
                For Each bw As BackgroundWorker In _bw
                    If bw IsNot Nothing AndAlso bw.IsBusy Then cnt += 1
                Next
                If cnt < 2 Then
                    TimerRefreshIcon.Enabled = False
                    NotifyIcon1.Icon = NIconAt
                End If
            End If
        End If
    End Sub

    Private Sub GetTimelineWorker_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)

        If _endingFlag OrElse e.Cancelled Then Exit Sub 'キャンセル

        Dim nw As Boolean = IsNetworkAvailable()

        If e.Error IsNot Nothing Then
            If nw Then NotifyIcon1.Icon = NIconAtRed
            ExceptionOut(e.Error)
            _waitTimeline = False
            _waitReply = False
            _waitFollower = False
            _waitDm = False
            '_initial = False
            Exit Sub
        End If

        Dim rslt As GetWorkerResult = DirectCast(e.Result, GetWorkerResult)
        Dim args As New GetWorkerArg()

        If rslt.type = WORKERTYPE.OpenUri Then Exit Sub

        If nw Then
            NotifyIcon1.Icon = NIconAt
            'タイマー再始動
            If SettingDialog.TimelinePeriodInt > 0 AndAlso Not TimerTimeline.Enabled Then TimerTimeline.Enabled = True
            If SettingDialog.DMPeriodInt > 0 AndAlso Not TimerDM.Enabled Then TimerDM.Enabled = True
        Else
            NotifyIcon1.Icon = NIconAtSmoke
        End If

        'エラー
        If rslt.retMsg.Length > 0 Then
            If nw Then NotifyIcon1.Icon = NIconAtRed
            StatusLabel.Text = rslt.retMsg
            _waitTimeline = False
            _waitReply = False
            _waitFollower = False
            _waitDm = False
            '_initial = False    '起動時モード終了
        End If

        'リストに反映
        Dim busy As Boolean = False
        For Each bw As BackgroundWorker In _bw
            If bw IsNot Nothing AndAlso bw.IsBusy Then
                busy = True
                Exit For
            End If
        Next
        If Not busy Then RefreshTimeline() 'background処理なければ、リスト反映

        Select Case rslt.type
            Case WORKERTYPE.Timeline
                _waitTimeline = False
                If Not _initial Then
                    '通常時
                    '自動調整
                    If SettingDialog.PeriodAdjust AndAlso SettingDialog.TimelinePeriodInt > 0 Then
                        If rslt.addCount >= 20 Then
                            Dim itv As Integer = TimerTimeline.Interval
                            itv -= 5000
                            If itv < 15000 Then itv = 15000
                            TimerTimeline.Interval = itv
                        Else
                            TimerTimeline.Interval += 1000
                            If TimerTimeline.Interval > SettingDialog.TimelinePeriodInt * 1000 Then TimerTimeline.Interval = SettingDialog.TimelinePeriodInt * 1000
                        End If
                    End If
                    If rslt.newDM Then
                        GetTimeline(WORKERTYPE.DirectMessegeRcv, 1, 0)
                    End If
                End If
            Case WORKERTYPE.Reply
                _waitReply = False
                If rslt.newDM AndAlso Not _initial Then
                    GetTimeline(WORKERTYPE.DirectMessegeRcv, 1, 0)
                End If
            Case WORKERTYPE.DirectMessegeRcv
                _waitDm = False
                'Case WORKERTYPE.DirectMessegeSnt
                'If _initial Then
                '    If SettingDialog.ReadPagesDM >= rslt.page + 1 Then
                '        If rslt.page Mod 10 = 0 Then
                '            If NextPageMessage(rslt.page) = Windows.Forms.DialogResult.No Then
                '                If SettingDialog.ReadPages > 0 Then
                '                    GetTimeline(WORKERTYPE.Timeline, 1, 1)
                '                ElseIf SettingDialog.ReadPagesReply > 0 Then
                '                    GetTimeline(WORKERTYPE.Reply, 1, 1)
                '                Else
                '                    _initial = False
                '                End If
                '                Exit Sub   '抜ける
                '            End If
                '        End If
                '        GetTimeline(WORKERTYPE.DirectMessegeSnt, rslt.page + 1, rslt.endPage)
                '    Else
                '        If SettingDialog.ReadPages > 0 Then
                '            GetTimeline(WORKERTYPE.Timeline, 1, 1)
                '        ElseIf SettingDialog.ReadPagesReply > 0 Then
                '            GetTimeline(WORKERTYPE.Reply, 1, 1)
                '        Else
                '            _initial = False
                '        End If
                '    End If
                'End If
                ' Contributed by shuyoko <http://twitter.com/shuyoko> BEGIN:
                ' Contributed by shuyoko <http://twitter.com/shuyoko> END.
            Case WORKERTYPE.FavAdd, WORKERTYPE.BlackFavAdd, WORKERTYPE.FavRemove
                _curList.BeginUpdate()
                For i As Integer = 0 To rslt.sIds.Count - 1
                    If _curTab.Text.Equals(rslt.tName) Then
                        Dim idx As Integer = _statuses.Tabs(rslt.tName).GetIndex(rslt.sIds(i))
                        Dim post As PostClass = _statuses.Item(rslt.sIds(i))
                        ChangeCacheStyleRead(post.IsRead, idx, _curTab)
                        If idx = _curItemIndex Then DispSelectedPost() '選択アイテム再表示
                    End If
                Next
                _curList.EndUpdate()
            Case WORKERTYPE.PostMessage
                urlUndoBuffer = Nothing
                UrlUndoToolStripMenuItem.Enabled = False  'Undoをできないように設定

                If rslt.retMsg.Length > 0 AndAlso Not rslt.retMsg.StartsWith("Outputz") Then
                    StatusLabel.Text = rslt.retMsg
                Else
                    _postTimestamps.Add(Now)
                    Dim oneHour As Date = Now.Subtract(New TimeSpan(1, 0, 0))
                    For i As Integer = _postTimestamps.Count - 1 To 0 Step -1
                        If _postTimestamps(i).CompareTo(oneHour) < 0 Then
                            _postTimestamps.RemoveAt(i)
                        End If
                    Next

                    If rslt.retMsg.Length > 0 Then StatusLabel.Text = rslt.retMsg 'Outputz失敗時

                    StatusText.Text = ""
                    _history.Add("")
                    _hisIdx = _history.Count - 1
                    SetMainWindowTitle()
                End If
                If rslt.retMsg.Length = 0 Then GetTimeline(WORKERTYPE.Timeline, 1, 0)
            Case WORKERTYPE.Follower
                _waitFollower = False
        End Select

    End Sub

    Private Sub GetTimeline(ByVal WkType As WORKERTYPE, ByVal fromPage As Integer, ByVal toPage As Integer)
        'toPage=0:通常モード
        If Not IsNetworkAvailable() Then Exit Sub
        'タイマー停止
        Select Case WkType
            Case WORKERTYPE.Timeline, WORKERTYPE.Reply
                TimerTimeline.Enabled = False
            Case WORKERTYPE.DirectMessegeRcv, WORKERTYPE.DirectMessegeSnt
                TimerDM.Enabled = False
        End Select
        '非同期実行引数設定
        Dim args As New GetWorkerArg
        args.page = fromPage
        args.endPage = toPage
        args.type = WkType

        RunAsync(args)
         'Timeline取得モードの場合はReplyも同時に取得
        If Not _initial AndAlso WkType = WORKERTYPE.Timeline Then
            Dim _args As New GetWorkerArg
            _args.page = fromPage
            _args.endPage = toPage
            _args.type = WORKERTYPE.Reply
            RunAsync(_args)
        End If
    End Sub

    Private Function NextPageMessage(ByVal page As Integer) As DialogResult
        Dim flashRslt As Integer = Win32Api.FlashWindow(Me.Handle.ToInt32, 1)
        Return MessageBox.Show((page * 20).ToString + My.Resources.GetTimelineWorker_RunWorkerCompletedText2, _
                           My.Resources.GetTimelineWorker_RunWorkerCompletedText3, _
                           MessageBoxButtons.YesNo, _
                           MessageBoxIcon.Question)
    End Function

    Private Sub NotifyIcon1_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles NotifyIcon1.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Me.Visible = True
            If Me.WindowState = FormWindowState.Minimized Then
                Me.WindowState = FormWindowState.Normal
            End If
            Me.Activate()
        End If
    End Sub

    Private Sub MyList_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        MakeReplyOrDirectStatus()
    End Sub

    Private Sub FavAddToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FavAddToolStripMenuItem.Click
        If _curTab.Text = "Direct" OrElse _curList.SelectedIndices.Count = 0 Then Exit Sub

        '複数fav確認msg
        If _curList.SelectedIndices.Count > 1 Then
            If MessageBox.Show(My.Resources.FavAddToolStripMenuItem_ClickText1, My.Resources.FavAddToolStripMenuItem_ClickText2, _
                               MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Cancel Then
                Exit Sub
            End If
        End If

        Dim args As New GetWorkerArg
        args.ids = New List(Of Long)
        args.sIds = New List(Of Long)
        args.tName = _curTab.Text
        args.type = WORKERTYPE.FavAdd
        For Each idx As Integer In _curList.SelectedIndices
            Dim post As PostClass = GetCurTabPost(idx)
            If Not post.IsFav Then args.ids.Add(post.Id)
        Next
        If args.ids.Count = 0 Then
            StatusLabel.Text = My.Resources.FavAddToolStripMenuItem_ClickText4
            Exit Sub
        End If

        RunAsync(args)
    End Sub

    Private Sub FavRemoveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FavRemoveToolStripMenuItem.Click
        If _curTab.Text = "Direct" OrElse _curList.SelectedIndices.Count = 0 Then Exit Sub

        If _curList.SelectedIndices.Count > 1 Then
            If MessageBox.Show(My.Resources.FavRemoveToolStripMenuItem_ClickText1, My.Resources.FavRemoveToolStripMenuItem_ClickText2, _
                               MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Cancel Then
                Exit Sub
            End If
        End If

        Dim args As New GetWorkerArg()
        args.ids = New List(Of Long)()
        args.sIds = New List(Of Long)()
        args.tName = _curTab.Text
        args.type = WORKERTYPE.FavRemove
        For Each idx As Integer In _curList.SelectedIndices
            Dim post As PostClass = GetCurTabPost(idx)
            If post.IsFav Then args.ids.Add(post.Id)
        Next
        If args.ids.Count = 0 Then
            StatusLabel.Text = My.Resources.FavRemoveToolStripMenuItem_ClickText4
            Exit Sub
        End If

        RunAsync(args)
    End Sub

    Private Function GetCurTabPost(ByVal Index As Integer) As PostClass
        If _postCache IsNot Nothing AndAlso Index >= _itemCacheIndex AndAlso Index < _itemCacheIndex + _postCache.Length Then
            Return _postCache(Index - _itemCacheIndex)
        Else
            Return _statuses.Item(_curTab.Text, Index)
        End If
    End Function


    Private Sub MoveToHomeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveToHomeToolStripMenuItem.Click
        If _curList.SelectedIndices.Count > 0 Then
            OpenUriAsync("http://twitter.com/" + GetCurTabPost(_curList.SelectedIndices(0)).Name)
        End If
    End Sub

    Private Sub MoveToFavToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveToFavToolStripMenuItem.Click
        If _curList.SelectedIndices.Count > 0 Then
            OpenUriAsync("http://twitter.com/" + GetCurTabPost(_curList.SelectedIndices(0)).Name + "/favorites")
        End If
    End Sub

    Private Sub Tween_ClientSizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ClientSizeChanged
        'ショートカットから最小化状態で起動した際の対応
        Static initialize As Boolean = False

        If Me.WindowState <> FormWindowState.Minimized Then
            If initialize Then
                If Me.WindowState = FormWindowState.Normal Then
                    _mySize = Me.ClientSize
                    _mySpDis = Me.SplitContainer1.SplitterDistance
                    If StatusText.Multiline Then _mySpDis2 = Me.StatusText.Height
                End If
            ElseIf _section IsNot Nothing Then
                '初回フォームレイアウト復元
                Try
                    Me.SplitContainer1.SplitterDistance = _section.SplitterDistance     'Splitterの位置設定
                    '発言欄複数行
                    StatusText.Multiline = _section.StatusMultiline
                    If StatusText.Multiline Then
                        SplitContainer2.SplitterDistance = SplitContainer2.Height - _section.StatusTextHeight - SplitContainer2.SplitterWidth
                    Else
                        SplitContainer2.SplitterDistance = SplitContainer2.Height - SplitContainer2.Panel2MinSize - SplitContainer2.SplitterWidth
                    End If
                    initialize = True
                Catch ex As Exception
                End Try
            End If
        End If
    End Sub

    Private Sub MyList_ColumnClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs)
        If SettingDialog.SortOrderLock Then Exit Sub
        Dim mode As IdComparerClass.ComparerMode
        If _iconCol Then
            mode = IdComparerClass.ComparerMode.Id
        Else
            Select Case e.Column
                Case 0, 5, 6    '0:アイコン,5:未読マーク,6:プロテクト・フィルターマーク
                    'ソートしない
                    Exit Sub
                Case 1  'ニックネーム
                    mode = IdComparerClass.ComparerMode.Nickname
                Case 2  '本文
                    mode = IdComparerClass.ComparerMode.Data
                Case 3  '時刻=発言Id
                    mode = IdComparerClass.ComparerMode.Id
                Case 4  '名前
                    mode = IdComparerClass.ComparerMode.Name
                Case 7  'Source
                    mode = IdComparerClass.ComparerMode.Source
            End Select
        End If
        _statuses.ToggleSortOrder(mode)
        _itemCache = Nothing
        _postCache = Nothing
        _curList.Refresh()
    End Sub

    Private Sub Tween_LocationChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LocationChanged
        If Me.WindowState = FormWindowState.Normal Then
            _myLoc = Me.Location
        End If
    End Sub

    Private Sub ContextMenuStrip2_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip2.Opening
        If ListTab.SelectedTab.Text = "Direct" Then
            FavAddToolStripMenuItem.Enabled = False
            FavRemoveToolStripMenuItem.Enabled = False
            StatusOpenMenuItem.Enabled = False
            FavorareMenuItem.Enabled = False
            BlackFavAddToolStripMenuItem.Enabled = False
            'BlackFavRemoveToolStripMenuItem.Enabled = False
        Else
            If IsNetworkAvailable() Then
                FavAddToolStripMenuItem.Enabled = True
                FavRemoveToolStripMenuItem.Enabled = True
                StatusOpenMenuItem.Enabled = True
                FavorareMenuItem.Enabled = True
                BlackFavAddToolStripMenuItem.Enabled = True
                'BlackFavRemoveToolStripMenuItem.Enabled = True
            End If
        End If
    End Sub

    Private Sub ReplyStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReplyStripMenuItem.Click
        MakeReplyOrDirectStatus(False, True)
    End Sub

    Private Sub DMStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DMStripMenuItem.Click
        MakeReplyOrDirectStatus(False, False)
    End Sub

    Private Sub DeleteStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteStripMenuItem.Click
        If _curTab.Text <> "Direct" Then
            Dim myPost As Boolean = False
            For Each idx As Integer In _curList.SelectedIndices
                If GetCurTabPost(idx).IsMe Then
                    myPost = True
                    Exit For
                End If
            Next
            If Not myPost Then Exit Sub
        End If

        Dim tmp As String = String.Format(My.Resources.DeleteStripMenuItem_ClickText1, Environment.NewLine)

        If MessageBox.Show(tmp, My.Resources.DeleteStripMenuItem_ClickText2, _
              MessageBoxButtons.OKCancel, _
              MessageBoxIcon.Question) = Windows.Forms.DialogResult.Cancel Then Exit Sub

        Try
            Me.Cursor = Cursors.WaitCursor

            Dim rslt As Boolean = True
            For Each idx As Integer In _curList.SelectedIndices
                Dim Id As Long = GetCurTabPost(idx).Id
                Dim rtn As String = ""
                If _curTab.Text = "Direct" Then
                    rtn = Twitter.RemoveDirectMessage(Id)
                Else
                    rtn = Twitter.RemoveStatus(Id)
                End If
                If rtn.Length > 0 Then
                    'エラー
                    rslt = False
                Else
                    _statuses.RemovePost(Id)
                End If
            Next

            If Not rslt Then
                StatusLabel.Text = My.Resources.DeleteStripMenuItem_ClickText3  '失敗
            Else
                StatusLabel.Text = My.Resources.DeleteStripMenuItem_ClickText4  '成功
            End If

            _itemCache = Nothing    'キャッシュ破棄
            _postCache = Nothing
            _curPost = Nothing
            _curItemIndex = -1
            For Each tb As TabPage In ListTab.TabPages
                DirectCast(tb.Controls(0), DetailsListView).VirtualListSize = _statuses.Tabs(tb.Text).AllCount
                If _statuses.Tabs(tb.Text).UnreadCount = 0 AndAlso tb.ImageIndex = 0 Then tb.ImageIndex = -1
            Next
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub ReadedStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReadedStripMenuItem.Click
        _curList.BeginUpdate()
        If SettingDialog.UnreadManage Then
            For Each idx As Integer In _curList.SelectedIndices
                _statuses.SetRead(True, _curTab.Text, idx)
            Next
        End If
        For Each idx As Integer In _curList.SelectedIndices
            ChangeCacheStyleRead(True, idx, _curTab)
        Next
        ColorizeList()
        _curList.EndUpdate()
        For Each tb As TabPage In ListTab.TabPages
            If _statuses.Tabs(tb.Text).UnreadCount = 0 AndAlso tb.ImageIndex = 0 Then tb.ImageIndex = -1
        Next
    End Sub

    Private Sub UnreadStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UnreadStripMenuItem.Click
        _curList.BeginUpdate()
        If SettingDialog.UnreadManage Then
            For Each idx As Integer In _curList.SelectedIndices
                _statuses.SetRead(False, _curTab.Text, idx)
            Next
        End If
        For Each idx As Integer In _curList.SelectedIndices
            ChangeCacheStyleRead(False, idx, _curTab)
        Next
        ColorizeList()
        _curList.EndUpdate()
        For Each tb As TabPage In ListTab.TabPages
            If _statuses.Tabs(tb.Text).UnreadCount > 0 AndAlso tb.ImageIndex = -1 Then tb.ImageIndex = 0
        Next
    End Sub

    Private Sub RefreshStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshStripMenuItem.Click
        DoRefresh()
    End Sub

    Private Sub DoRefresh()
        Select Case _curTab.Text
            Case "Reply"
                GetTimeline(WORKERTYPE.Reply, 1, 0)
            Case "Direct"
                GetTimeline(WORKERTYPE.DirectMessegeRcv, 1, 0)
            Case Else
                GetTimeline(WORKERTYPE.Timeline, 1, 0)
        End Select
    End Sub

    Private Sub SettingStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingStripMenuItem.Click
        If SettingDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            SyncLock _syncObject
                _username = SettingDialog.UserID
                _password = SettingDialog.PasswordStr
                Twitter.Username = _username
                Twitter.Password = _password
                If SettingDialog.TimelinePeriodInt > 0 Then
                    If SettingDialog.PeriodAdjust Then
                        If SettingDialog.TimelinePeriodInt * 1000 < TimerTimeline.Interval Then
                            TimerTimeline.Interval = SettingDialog.TimelinePeriodInt * 1000
                        End If
                    Else
                        TimerTimeline.Interval = SettingDialog.TimelinePeriodInt * 1000
                    End If
                    TimerTimeline.Enabled = True
                Else
                    TimerTimeline.Interval = 600000
                    TimerTimeline.Enabled = False
                End If
                If SettingDialog.DMPeriodInt > 0 Then
                    TimerDM.Interval = SettingDialog.DMPeriodInt * 1000
                    TimerDM.Enabled = True
                Else
                    TimerDM.Interval = 600000
                    TimerDM.Enabled = False
                End If
                Twitter.NextThreshold = SettingDialog.NextPageThreshold
                Twitter.NextPages = SettingDialog.NextPagesInt
                Twitter.UseAPI = SettingDialog.UseAPI
                Twitter.HubServer = SettingDialog.HubServer
                Twitter.TinyUrlResolve = SettingDialog.TinyUrlResolve
                Twitter.RestrictFavCheck = SettingDialog.RestrictFavCheck

                Twitter.ProxyType = SettingDialog.ProxyType
                Twitter.ProxyAddress = SettingDialog.ProxyAddress
                Twitter.ProxyPort = SettingDialog.ProxyPort
                Twitter.ProxyUser = SettingDialog.ProxyUser
                Twitter.ProxyPassword = SettingDialog.ProxyPassword

                If Not SettingDialog.UnreadManage Then
                    ReadedStripMenuItem.Enabled = False
                    UnreadStripMenuItem.Enabled = False
                    For Each myTab As TabPage In ListTab.TabPages
                        myTab.ImageIndex = -1
                    Next
                Else
                    ReadedStripMenuItem.Enabled = True
                    UnreadStripMenuItem.Enabled = True
                End If
                _fntUnread = SettingDialog.FontUnread
                _clUnread = SettingDialog.ColorUnread
                _fntReaded = SettingDialog.FontReaded
                _clReaded = SettingDialog.ColorReaded
                _clFav = SettingDialog.ColorFav
                _clOWL = SettingDialog.ColorOWL
                _fntDetail = SettingDialog.FontDetail
                _clSelf = SettingDialog.ColorSelf
                _clAtSelf = SettingDialog.ColorAtSelf
                _clTarget = SettingDialog.ColorTarget
                _clAtTarget = SettingDialog.ColorAtTarget
                _clAtFromTarget = SettingDialog.ColorAtFromTarget
                _brsForeColorUnread.Dispose()
                _brsForeColorReaded.Dispose()
                _brsForeColorFav.Dispose()
                _brsForeColorOWL.Dispose()
                _brsForeColorUnread = New SolidBrush(_clUnread)
                _brsForeColorReaded = New SolidBrush(_clReaded)
                _brsForeColorFav = New SolidBrush(_clFav)
                _brsForeColorOWL = New SolidBrush(_clOWL)
                _brsBackColorMine.Dispose()
                _brsBackColorAt.Dispose()
                _brsBackColorYou.Dispose()
                _brsBackColorAtYou.Dispose()
                _brsBackColorAtTo.Dispose()
                _brsBackColorMine = New SolidBrush(_clSelf)
                _brsBackColorAt = New SolidBrush(_clAtSelf)
                _brsBackColorYou = New SolidBrush(_clTarget)
                _brsBackColorAtYou = New SolidBrush(_clAtTarget)
                _brsBackColorAtTo = New SolidBrush(_clAtFromTarget)
                detailHtmlFormat = detailHtmlFormat1 + _fntDetail.Name + detailHtmlFormat2 + _fntDetail.Size.ToString() + detailHtmlFormat3
                _statuses.SetUnreadManage(SettingDialog.UnreadManage)
                For Each tb As TabPage In ListTab.TabPages
                    If _statuses.Tabs(tb.Text).UnreadCount = 0 Then
                        tb.ImageIndex = -1
                    Else
                        tb.ImageIndex = 0
                    End If
                    If tb.Controls IsNot Nothing AndAlso tb.Controls.Count > 0 Then
                        DirectCast(tb.Controls(0), DetailsListView).Font = _fntReaded
                    End If
                Next

                SetMainWindowTitle()
                SetNotifyIconText()

                _itemCache = Nothing
                _postCache = Nothing
                _curList.Refresh()
            End SyncLock
        End If

        Me.TopMost = SettingDialog.AlwaysTop
        SaveConfigs()
    End Sub

    Private Sub PostBrowser_Navigating(ByVal sender As System.Object, ByVal e As System.Windows.Forms.WebBrowserNavigatingEventArgs) Handles PostBrowser.Navigating
        If e.Url.AbsoluteUri <> "about:blank" Then
            e.Cancel = True
            OpenUriAsync(e.Url.AbsoluteUri)
        End If

    End Sub

    Private Function AddNewTab(ByVal tabName As String, ByVal startup As Boolean) As Boolean
        '重複チェック
        For Each tb As TabPage In ListTab.TabPages
            If tb.Text = tabName Then Return False
        Next

        '新規タブ名チェック
        If tabName = My.Resources.AddNewTabText1 Then Return False

        'Dim myTab As New TabStructure()

        Dim _tabPage As TabPage = New TabPage
        Dim _listCustom As DetailsListView = New DetailsListView
        Dim _colHd1 As ColumnHeader = New ColumnHeader()  'アイコン
        Dim _colHd2 As ColumnHeader = New ColumnHeader()   'ニックネーム
        Dim _colHd3 As ColumnHeader = New ColumnHeader()   '本文
        Dim _colHd4 As ColumnHeader = New ColumnHeader()   '日付
        Dim _colHd5 As ColumnHeader = New ColumnHeader()   'ユーザID
        Dim _colHd6 As ColumnHeader = New ColumnHeader()   '未読
        Dim _colHd7 As ColumnHeader = New ColumnHeader()   'マーク＆プロテクト
        Dim _colHd8 As ColumnHeader = New ColumnHeader()   'ソース
        'If Not _iconCol Then
        '_colHd2 = New ColumnHeader()
        '_colHd3 = New ColumnHeader()
        '_colHd4 = New ColumnHeader()
        '_colHd5 = New ColumnHeader()
        '_colHd6 = New ColumnHeader()
        '_colHd7 = New ColumnHeader()
        '_colHd8 = New ColumnHeader()
        '_colHd9 = New ColumnHeader()
        'End If

        If Not startup Then _section.ListElement.Add(New ListElement(tabName))

        Dim cnt As Integer = ListTab.TabPages.Count

        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.ListTab.SuspendLayout()
        Me.SuspendLayout()

        _tabPage.SuspendLayout()

        Me.ListTab.Controls.Add(_tabPage)

        _tabPage.Controls.Add(_listCustom)
        _tabPage.Location = New Point(4, 4)
        _tabPage.Name = "CTab" + cnt.ToString()
        _tabPage.Size = New Size(380, 260)
        _tabPage.TabIndex = 2 + cnt
        _tabPage.Text = tabName
        _tabPage.UseVisualStyleBackColor = True

        _listCustom.AllowColumnReorder = True
        If Not _iconCol Then
            _listCustom.Columns.AddRange(New ColumnHeader() {_colHd1, _colHd2, _colHd3, _colHd4, _colHd5, _colHd6, _colHd7, _colHd8})
        Else
            _listCustom.Columns.AddRange(New ColumnHeader() {_colHd1, _colHd3})
        End If
        _listCustom.ContextMenuStrip = Me.ContextMenuStrip2
        _listCustom.Dock = DockStyle.Fill
        _listCustom.FullRowSelect = True
        _listCustom.HideSelection = False
        _listCustom.Location = New Point(0, 0)
        _listCustom.Margin = New Padding(0)
        _listCustom.Name = "CList" + Environment.TickCount.ToString()
        _listCustom.ShowItemToolTips = True
        _listCustom.Size = New Size(380, 260)
        _listCustom.TabIndex = 4                                   'これ大丈夫？
        _listCustom.UseCompatibleStateImageBehavior = False
        _listCustom.View = View.Details
        _listCustom.OwnerDraw = True
        _listCustom.VirtualMode = True
        _listCustom.Font = _fntReaded

        AddHandler _listCustom.SelectedIndexChanged, AddressOf MyList_SelectedIndexChanged
        AddHandler _listCustom.MouseDoubleClick, AddressOf MyList_MouseDoubleClick
        AddHandler _listCustom.ColumnClick, AddressOf MyList_ColumnClick
        AddHandler _listCustom.DrawColumnHeader, AddressOf MyList_DrawColumnHeader

        Select Case _iconSz
            Case 26, 48
                AddHandler _listCustom.DrawItem, AddressOf MyList_DrawItem
            Case Else
                AddHandler _listCustom.DrawItem, AddressOf MyList_DrawItemDefault
        End Select

        AddHandler _listCustom.Scrolled, AddressOf Mylist_Scrolled
        AddHandler _listCustom.MouseClick, AddressOf MyList_MouseClick
        AddHandler _listCustom.ColumnReordered, AddressOf MyList_ColumnReordered
        AddHandler _listCustom.ColumnWidthChanged, AddressOf MyList_ColumnWidthChanged
        AddHandler _listCustom.CacheVirtualItems, AddressOf MyList_CacheVirtualItems
        AddHandler _listCustom.RetrieveVirtualItem, AddressOf MyList_RetrieveVirtualItem
        AddHandler _listCustom.DrawSubItem, AddressOf MyList_DrawSubItem

        _colHd1.Text = ""
        _colHd1.Width = 48
        'If Not _iconCol Then
        _colHd2.Text = My.Resources.AddNewTabText2
        _colHd2.Width = 80
        _colHd3.Text = My.Resources.AddNewTabText3
        _colHd3.Width = 300
        _colHd4.Text = My.Resources.AddNewTabText4
        _colHd4.Width = 50
        _colHd5.Text = My.Resources.AddNewTabText5
        _colHd5.Width = 50
        _colHd6.Text = ""
        _colHd6.Width = 16
        _colHd7.Text = ""
        _colHd7.Width = 16
        _colHd8.Text = "Source"
        _colHd8.Width = 50
        'End If

        If Not IsDefaultTab(tabName) Then
            TabDialog.AddTab(tabName)
        End If

        _listCustom.SmallImageList = TIconSmallList
        '_listCustom.ListViewItemSorter = listViewItemSorter
        Dim dispOrder(7) As Integer
        If Not startup Then
            For i As Integer = 0 To _curList.Columns.Count - 1
                For j As Integer = 0 To _curList.Columns.Count - 1
                    If _curList.Columns(j).DisplayIndex = i Then
                        dispOrder(i) = j
                        Exit For
                    End If
                Next
            Next
            For i As Integer = 0 To _curList.Columns.Count - 1
                _listCustom.Columns(i).Width = _curList.Columns(i).Width
                _listCustom.Columns(dispOrder(i)).DisplayIndex = i
            Next
        Else
            If _iconCol Then
                _listCustom.Columns(0).Width = _section.Width1
                _listCustom.Columns(1).Width = _section.Width3
                _listCustom.Columns(0).DisplayIndex = _section.DisplayIndex1
                _listCustom.Columns(1).DisplayIndex = _section.DisplayIndex3
            Else
                For i As Integer = 0 To 7
                    If _section.DisplayIndex1 = i Then
                        dispOrder(i) = 0
                    ElseIf _section.DisplayIndex2 = i Then
                        dispOrder(i) = 1
                    ElseIf _section.DisplayIndex3 = i Then
                        dispOrder(i) = 2
                    ElseIf _section.DisplayIndex4 = i Then
                        dispOrder(i) = 3
                    ElseIf _section.DisplayIndex5 = i Then
                        dispOrder(i) = 4
                    ElseIf _section.DisplayIndex6 = i Then
                        dispOrder(i) = 5
                    ElseIf _section.DisplayIndex7 = i Then
                        dispOrder(i) = 6
                    ElseIf _section.DisplayIndex8 = i Then
                        dispOrder(i) = 7
                    End If
                Next
                _listCustom.Columns(0).Width = _section.Width1
                _listCustom.Columns(1).Width = _section.Width2
                _listCustom.Columns(2).Width = _section.Width3
                _listCustom.Columns(3).Width = _section.Width4
                _listCustom.Columns(4).Width = _section.Width5
                _listCustom.Columns(5).Width = _section.Width6
                _listCustom.Columns(6).Width = _section.Width7
                _listCustom.Columns(7).Width = _section.Width8
                For i As Integer = 0 To 7
                    _listCustom.Columns(dispOrder(i)).DisplayIndex = i
                Next
            End If
        End If

        _tabPage.ResumeLayout(False)

        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.ListTab.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()
        Return True
    End Function

    Private Sub RemoveSpecifiedTab(ByVal TabName As String)
        Dim idx As Integer = 0
        For idx = 0 To ListTab.TabPages.Count - 1
            If ListTab.TabPages(idx).Text = TabName Then Exit For
        Next

        If IsDefaultTab(TabName) Then Exit Sub

        Dim tmp As String = String.Format(My.Resources.RemoveSpecifiedTabText1, Environment.NewLine)
        If MessageBox.Show(tmp, My.Resources.RemoveSpecifiedTabText2, _
                         MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        SetListProperty()   '他のタブに列幅等を反映

        'オブジェクトインスタンスの削除
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.ListTab.SuspendLayout()
        Me.SuspendLayout()

        Dim _tabPage As TabPage = ListTab.TabPages(idx)
        Dim _listCustom As DetailsListView = DirectCast(_tabPage.Controls(0), DetailsListView)

        _tabPage.SuspendLayout()

        Me.ListTab.Controls.Remove(_tabPage)
        _tabPage.Controls.Remove(_listCustom)
        _listCustom.Columns.Clear()
        _listCustom.ContextMenuStrip = Nothing

        RemoveHandler _listCustom.SelectedIndexChanged, AddressOf MyList_SelectedIndexChanged
        RemoveHandler _listCustom.MouseDoubleClick, AddressOf MyList_MouseDoubleClick
        RemoveHandler _listCustom.ColumnClick, AddressOf MyList_ColumnClick
        RemoveHandler _listCustom.DrawColumnHeader, AddressOf MyList_DrawColumnHeader

        Select Case _iconSz
            Case 26, 48
                RemoveHandler _listCustom.DrawItem, AddressOf MyList_DrawItem
            Case Else
                RemoveHandler _listCustom.DrawItem, AddressOf MyList_DrawItemDefault
        End Select

        RemoveHandler _listCustom.Scrolled, AddressOf Mylist_Scrolled
        RemoveHandler _listCustom.MouseClick, AddressOf MyList_MouseClick
        RemoveHandler _listCustom.ColumnReordered, AddressOf MyList_ColumnReordered
        RemoveHandler _listCustom.ColumnWidthChanged, AddressOf MyList_ColumnWidthChanged
        RemoveHandler _listCustom.CacheVirtualItems, AddressOf MyList_CacheVirtualItems
        RemoveHandler _listCustom.RetrieveVirtualItem, AddressOf MyList_RetrieveVirtualItem
        RemoveHandler _listCustom.DrawSubItem, AddressOf MyList_DrawSubItem

        TabDialog.RemoveTab(TabName)

        _listCustom.SmallImageList = Nothing
        _listCustom.ListViewItemSorter = Nothing

        'キャッシュのクリア
        If _curTab.Equals(_tabPage) Then
            _curTab = Nothing
            _curItemIndex = -1
            _curList = Nothing
            _curPost = Nothing
        End If
        _itemCache = Nothing
        _itemCacheIndex = -1
        _postCache = Nothing

        _tabPage.ResumeLayout(False)

        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.ListTab.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

        _tabPage.Dispose()
        _listCustom.Dispose()
        _statuses.RemoveTab(TabName)

        SaveConfigs()

        For Each tp As TabPage In ListTab.TabPages
            Dim lst As DetailsListView = DirectCast(tp.Controls(0), DetailsListView)
            If lst.VirtualListSize <> _statuses.Tabs(tp.Text).AllCount Then
                lst.VirtualListSize = _statuses.Tabs(tp.Text).AllCount
            End If
        Next
    End Sub

    Private Sub ListTab_Deselected(ByVal sender As Object, ByVal e As System.Windows.Forms.TabControlEventArgs) Handles ListTab.Deselected
        _itemCache = Nothing
        _itemCacheIndex = -1
        _postCache = Nothing
    End Sub

    Private Sub ListTab_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListTab.MouseMove
        'タブのD&D
        Dim cpos As New Point(e.X, e.Y)

        If e.Button = Windows.Forms.MouseButtons.Left AndAlso _tabDrag Then
            Dim tn As String = ""
            For i As Integer = 0 To ListTab.TabPages.Count - 1
                Dim rect As Rectangle = ListTab.GetTabRect(i)
                If rect.Left <= cpos.X AndAlso cpos.X <= rect.Right AndAlso _
                   rect.Top <= cpos.Y AndAlso cpos.Y <= rect.Bottom Then
                    tn = ListTab.TabPages(i).Text
                    Exit For
                End If
            Next

            If tn = "" Then Exit Sub

            For Each tb As TabPage In ListTab.TabPages
                If tb.Text = tn Then
                    ListTab.DoDragDrop(tb, DragDropEffects.All)
                    Exit For
                End If
            Next
            SaveConfigs()
        Else
            _tabDrag = False
        End If

        For i As Integer = 0 To ListTab.TabPages.Count - 1
            Dim rect As Rectangle = ListTab.GetTabRect(i)
            If rect.Left <= cpos.X And cpos.X <= rect.Right And _
               rect.Top <= cpos.Y And cpos.Y <= rect.Bottom Then
                _rclickTabName = ListTab.TabPages(i).Text
                Exit For
            End If
        Next
    End Sub

    Private Sub ListTab_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListTab.SelectedIndexChanged
        '_curList.Refresh()
        DispSelectedPost()
        SetMainWindowTitle()
        SetStatusLabel()
    End Sub

    Private Sub SetListProperty()
        '削除などで見つからない場合は処理せず
        If _curList Is Nothing Then Exit Sub

        Dim dispOrder(_curList.Columns.Count - 1) As Integer
        For i As Integer = 0 To _curList.Columns.Count - 1
            For j As Integer = 0 To _curList.Columns.Count - 1
                If _curList.Columns(j).DisplayIndex = i Then
                    dispOrder(i) = j
                    Exit For
                End If
            Next
        Next

        '列幅、列並びを他のタブに設定
        For Each tb As TabPage In ListTab.TabPages
            If Not tb.Equals(_curTab) Then
                Dim lst As DetailsListView = DirectCast(tb.Controls(0), DetailsListView)
                For i As Integer = 0 To lst.Columns.Count - 1
                    lst.Columns(dispOrder(i)).DisplayIndex = i
                    lst.Columns(i).Width = _curList.Columns(i).Width
                Next
            End If
        Next
    End Sub

    Private Sub PostBrowser_StatusTextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles PostBrowser.StatusTextChanged
        If PostBrowser.StatusText.StartsWith("http") OrElse PostBrowser.StatusText.StartsWith("ftp") Then
            StatusLabelUrl.Text = PostBrowser.StatusText
            ToolStripMenuItem4.Enabled = True
        Else
            ToolStripMenuItem4.Enabled = False
        End If
        If PostBrowser.StatusText = "" Then
            SetStatusLabel()
        End If
    End Sub

    Private Sub StatusText_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles StatusText.KeyUp
        'スペースキーで未読ジャンプ
        If Not e.Alt AndAlso Not e.Control AndAlso Not e.Shift Then
            If e.KeyCode = Keys.Space OrElse e.KeyCode = Keys.ProcessKey Then
                If StatusText.Text = " " OrElse StatusText.Text = "　" Then
                    e.Handled = True
                    StatusText.Text = ""
                    JumpUnreadMenuItem_Click(Nothing, Nothing)
                End If
            End If
        End If
    End Sub

    Private Sub StatusText_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusText.TextChanged
        '文字数カウント
        Dim pLen As Integer = 140 - StatusText.Text.Length
        lblLen.Text = pLen.ToString()
        If pLen < 0 Then
            StatusText.ForeColor = Color.Red
        Else
            StatusText.ForeColor = Color.FromKnownColor(KnownColor.ControlText)
        End If
    End Sub

    Private Sub MyList_CacheVirtualItems(ByVal sender As System.Object, ByVal e As System.Windows.Forms.CacheVirtualItemsEventArgs)
        If _itemCache IsNot Nothing AndAlso _
           e.StartIndex >= _itemCacheIndex AndAlso _
           e.EndIndex < _itemCacheIndex + _itemCache.Length AndAlso _
           _curList.Equals(sender) Then
            'If the newly requested cache is a subset of the old cache, 
            'no need to rebuild everything, so do nothing.
            Return
        End If

        'Now we need to rebuild the cache.
        If _curList.Equals(sender) Then CreateCache(e.StartIndex, e.EndIndex)
    End Sub

    Private Sub MyList_RetrieveVirtualItem(ByVal sender As System.Object, ByVal e As System.Windows.Forms.RetrieveVirtualItemEventArgs)
        If _itemCache IsNot Nothing AndAlso e.ItemIndex >= _itemCacheIndex AndAlso e.ItemIndex < _itemCacheIndex + _itemCache.Length AndAlso _curList.Equals(sender) Then
            'A cache hit, so get the ListViewItem from the cache instead of making a new one.
            e.Item = _itemCache(e.ItemIndex - _itemCacheIndex)
        Else
            'A cache miss, so create a new ListViewItem and pass it back.
            Dim tb As TabPage = DirectCast(DirectCast(sender, Tween.TweenCustomControl.DetailsListView).Parent, TabPage)
            e.Item = CreateItem(tb, _
                                _statuses.Item(tb.Text, e.ItemIndex), _
                                e.ItemIndex)
        End If
    End Sub

    Private Sub CreateCache(ByVal StartIndex As Integer, ByVal EndIndex As Integer)
        If _curList.VirtualListSize <> _statuses.Tabs(_curTab.Text).AllCount Then
            'フィルタ操作後に不一致発生（スレッド関係？）のため対処
            _postCache = Nothing
            _itemCache = Nothing
            _curList.VirtualListSize = _statuses.Tabs(_curTab.Text).AllCount
            Exit Sub
        End If
        'キャッシュ要求（要求範囲±30を作成）
        StartIndex -= 30
        If StartIndex < 0 Then StartIndex = 0
        EndIndex += 30
        If EndIndex >= _statuses.Tabs(_curTab.Text).AllCount Then EndIndex = _statuses.Tabs(_curTab.Text).AllCount - 1
        _postCache = _statuses.Item(_curTab.Text, StartIndex, EndIndex) '配列で取得
        _itemCacheIndex = StartIndex

        _itemCache = New ListViewItem(_postCache.Length - 1) {}
        For i As Integer = 0 To _postCache.Length - 1
            _itemCache(i) = CreateItem(_curTab, _postCache(i), StartIndex + i)
        Next i
    End Sub

    Private Function CreateItem(ByVal Tab As TabPage, ByVal Post As PostClass, ByVal Index As Integer) As ListViewItem
        Dim mk As String = ""
        If Post.IsMark Then mk += "♪"
        If Post.IsProtect Then mk += "Ю"
        Dim sitem() As String = {"", Post.Nickname, Post.Data, Post.PDate.ToString(SettingDialog.DateTimeFormat), Post.Name, "", mk, Post.Source}
        Dim itm As ListViewItem = New ListViewItem(sitem, Post.ImageIndex)
        Dim read As Boolean = Post.IsRead
        '未読管理していなかったら既読として扱う
        If Not _statuses.Tabs(Tab.Text).UnreadManage OrElse _
           Not SettingDialog.UnreadManage Then read = True
        ChangeItemStyleRead(read, itm, Post, Nothing)
        If Tab.Equals(_curTab) Then ColorizeList(itm, Index)
        Return itm
    End Function

    Private Sub MyList_DrawColumnHeader(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawListViewColumnHeaderEventArgs)
        e.DrawDefault = True
    End Sub

    Private Sub MyList_DrawItemDefault(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawListViewItemEventArgs)
        e.DrawDefault = True
    End Sub

    Private Sub MyList_DrawItem(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawListViewItemEventArgs)
        'アイコンサイズ26,48はオーナードロー（DrawSubItem発生させる）
        If e.State = 0 Then Exit Sub
        e.DrawDefault = False
        If Not e.Item.Selected Then     'e.ItemStateでうまく判定できない？？？
            Dim brs2 As SolidBrush = Nothing
            Select Case e.Item.BackColor
                Case _clSelf
                    brs2 = _brsBackColorMine
                Case _clAtSelf
                    brs2 = _brsBackColorAt
                Case _clTarget
                    brs2 = _brsBackColorYou
                Case _clAtTarget
                    brs2 = _brsBackColorAtYou
                Case _clAtFromTarget
                    brs2 = _brsBackColorAtTo
                Case Else
                    brs2 = _brsBackColorNone
            End Select
            e.Graphics.FillRectangle(brs2, e.Bounds)
        Else
            '選択中の行
            If DirectCast(sender, Windows.Forms.Control).Focused Then
                e.Graphics.FillRectangle(_brsHighLight, e.Bounds)
            Else
                e.Graphics.FillRectangle(_brsDeactiveSelection, e.Bounds)
            End If
        End If
        If (e.State And ListViewItemStates.Focused) = ListViewItemStates.Focused Then e.DrawFocusRectangle()
    End Sub

    Private Sub MyList_DrawSubItem(ByVal sender As Object, ByVal e As DrawListViewSubItemEventArgs)
        If e.ItemState = 0 Then Exit Sub
        If e.ColumnIndex > 0 Then
            Dim rct As RectangleF = e.Bounds
            rct.Width = e.Header.Width
            'アイコン以外の列
            If Not e.Item.Selected Then     'e.ItemStateでうまく判定できない？？？
                '選択されていない行
                '文字色
                Dim brs As SolidBrush = Nothing
                Select Case e.Item.ForeColor
                    Case _clUnread
                        brs = _brsForeColorUnread
                    Case _clReaded
                        brs = _brsForeColorReaded
                    Case _clFav
                        brs = _brsForeColorFav
                    Case _clOWL
                        brs = _brsForeColorOWL
                    Case Else
                        brs = New SolidBrush(e.Item.ForeColor)
                End Select
                If rct.Width > 0 Then
                    If _iconCol Then
                        e.Graphics.DrawString(e.Item.SubItems(4).Text + "/" + e.Item.SubItems(1).Text + " (" + e.Item.SubItems(3).Text + ") <" + e.Item.SubItems(5).Text + e.Item.SubItems(6).Text + "> from " + e.Item.SubItems(7).Text + System.Environment.NewLine + e.Item.SubItems(2).Text, e.Item.Font, brs, rct, sf)
                    Else
                        e.Graphics.DrawString(e.SubItem.Text, e.Item.Font, brs, rct, sf)
                    End If
                End If
            Else
                If rct.Width > 0 Then
                    '選択中の行
                    If DirectCast(sender, Windows.Forms.Control).Focused Then
                        If _iconCol Then
                            e.Graphics.DrawString(e.Item.SubItems(4).Text + "/" + e.Item.SubItems(1).Text + " (" + e.Item.SubItems(3).Text + ") <" + e.Item.SubItems(5).Text + e.Item.SubItems(6).Text + "> from " + e.Item.SubItems(7).Text + System.Environment.NewLine + e.Item.SubItems(2).Text, e.Item.Font, _brsHighLightText, rct, sf)
                        Else
                            e.Graphics.DrawString(e.SubItem.Text, e.Item.Font, _brsHighLightText, rct, sf)
                        End If
                    Else
                        If _iconCol Then
                            e.Graphics.DrawString(e.Item.SubItems(4).Text + "/" + e.Item.SubItems(1).Text + " (" + e.Item.SubItems(3).Text + ") <" + e.Item.SubItems(5).Text + e.Item.SubItems(6).Text + "> from " + e.Item.SubItems(7).Text + System.Environment.NewLine + e.Item.SubItems(2).Text, e.Item.Font, _brsForeColorUnread, rct, sf)
                        Else
                            e.Graphics.DrawString(e.SubItem.Text, e.Item.Font, _brsForeColorUnread, rct, sf)
                        End If
                    End If
                End If
            End If
        Else
            'アイコン列はデフォルト描画
            e.DrawDefault = True
        End If
    End Sub

    Private Sub DoTabSearch(ByVal _word As String, _
                            ByVal CaseSensitive As Boolean, _
                            ByVal UseRegex As Boolean, _
                            ByVal SType As SEARCHTYPE)
        'Dim myList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        Dim cidx As Integer = 0
        Dim fnd As Boolean = False
        Dim toIdx As Integer
        Dim stp As Integer = 1

        If _curList.VirtualListSize = 0 Then
            MessageBox.Show(My.Resources.DoTabSearchText2, My.Resources.DoTabSearchText3, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        If _curList.SelectedIndices.Count > 0 Then
            cidx = _curList.SelectedIndices(0)
        End If
        toIdx = _curList.VirtualListSize - 1

        Select Case SType
            Case SEARCHTYPE.DialogSearch    'ダイアログからの検索
                If _curList.SelectedIndices.Count > 0 Then
                    cidx = _curList.SelectedIndices(0)
                Else
                    cidx = 0
                End If
            Case SEARCHTYPE.NextSearch      '次を検索
                If _curList.SelectedIndices.Count > 0 Then
                    cidx = _curList.SelectedIndices(0) + 1
                    If cidx > toIdx Then cidx = toIdx
                Else
                    cidx = 0
                End If
            Case SEARCHTYPE.PrevSearch      '前を検索
                If _curList.SelectedIndices.Count > 0 Then
                    cidx = _curList.SelectedIndices(0) - 1
                    If cidx < 0 Then cidx = 0
                Else
                    cidx = toIdx
                End If
                toIdx = 0
                stp = -1
        End Select

        Dim regOpt As RegexOptions = RegexOptions.None
        Dim fndOpt As StringComparison = StringComparison.Ordinal
        If Not CaseSensitive Then
            regOpt = RegexOptions.IgnoreCase
            fndOpt = StringComparison.OrdinalIgnoreCase
        End If
RETRY:
        If UseRegex Then
            ' 正規表現検索
            Dim _search As Regex
            Try
                _search = New Regex(_word)
                For idx As Integer = cidx To toIdx Step stp
                    Dim post As PostClass = _statuses.Item(_curTab.Text, idx)
                    If _search.IsMatch(post.Nickname, regOpt) _
                        OrElse _search.IsMatch(post.Data, regOpt) _
                        OrElse _search.IsMatch(post.Name, regOpt) _
                    Then
                        SelectListItem(_curList, idx)
                        _curList.EnsureVisible(idx)
                        Exit Sub
                    End If
                Next
            Catch ex As ArgumentException
                MsgBox(My.Resources.DoTabSearchText1, MsgBoxStyle.Critical)
                Exit Sub
            End Try
        Else
            ' 通常検索
            For idx As Integer = cidx To toIdx Step stp
                Dim post As PostClass = _statuses.Item(_curTab.Text, idx)
                If post.Nickname.IndexOf(_word, fndOpt) > -1 _
                    OrElse post.Data.IndexOf(_word, fndOpt) > -1 _
                    OrElse post.Name.IndexOf(_word, fndOpt) > -1 _
                Then
                    SelectListItem(_curList, idx)
                    _curList.EnsureVisible(idx)
                    Exit Sub
                End If
            Next
        End If

        If Not fnd Then
            Select Case SType
                Case SEARCHTYPE.DialogSearch, SEARCHTYPE.NextSearch
                    toIdx = cidx
                    cidx = 0
                Case SEARCHTYPE.PrevSearch
                    toIdx = cidx
                    cidx = _curList.Items.Count - 1
            End Select
            fnd = True
            GoTo RETRY
        End If

        MessageBox.Show(My.Resources.DoTabSearchText2, My.Resources.DoTabSearchText3, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub MenuItemSubSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemSubSearch.Click
        '検索メニュー
        SearchDialog.Owner = Me
        If SearchDialog.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
            Me.TopMost = SettingDialog.AlwaysTop
            Exit Sub
        End If
        Me.TopMost = SettingDialog.AlwaysTop

        If SearchDialog.SWord <> "" Then
            DoTabSearch(SearchDialog.SWord, _
                        SearchDialog.CheckCaseSensitive, _
                        SearchDialog.CheckRegex, _
                        SEARCHTYPE.DialogSearch)
        End If
    End Sub

    Private Sub MenuItemSearchNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemSearchNext.Click
        '次を検索
        If SearchDialog.SWord = "" Then
            If SearchDialog.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
                Me.TopMost = SettingDialog.AlwaysTop
                Exit Sub
            End If
            Me.TopMost = SettingDialog.AlwaysTop
            If SearchDialog.SWord = "" Then Exit Sub

            DoTabSearch(SearchDialog.SWord, _
                        SearchDialog.CheckCaseSensitive, _
                        SearchDialog.CheckRegex, _
                        SEARCHTYPE.DialogSearch)
        Else
            DoTabSearch(SearchDialog.SWord, _
                        SearchDialog.CheckCaseSensitive, _
                        SearchDialog.CheckRegex, _
                        SEARCHTYPE.NextSearch)
        End If
    End Sub

    Private Sub MenuItemSearchPrev_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemSearchPrev.Click
        '前を検索
        If SearchDialog.SWord = "" Then
            If SearchDialog.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
                Me.TopMost = SettingDialog.AlwaysTop
                Exit Sub
            End If
            Me.TopMost = SettingDialog.AlwaysTop
            If SearchDialog.SWord = "" Then Exit Sub
        End If

        DoTabSearch(SearchDialog.SWord, _
                    SearchDialog.CheckCaseSensitive, _
                    SearchDialog.CheckRegex, _
                    SEARCHTYPE.PrevSearch)
    End Sub

    Private Sub AboutMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutMenuItem.Click
        TweenAboutBox.ShowDialog()
        Me.TopMost = SettingDialog.AlwaysTop
    End Sub

    Private Sub JumpUnreadMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles JumpUnreadMenuItem.Click
        Dim tb As TabClass = _statuses.Tabs(_curTab.Text)
        Dim lst As DetailsListView = _curList
        Dim idx As Integer = 0
RETRY:
        If tb.OldestUnreadId > -1 AndAlso tb.Contains(tb.OldestUnreadId) AndAlso tb.UnreadCount > 0 Then
            '未読アイテムへ
            If _statuses.Item(tb.OldestUnreadId).IsRead Then
                '状態不整合
                _statuses.SetNextUnreadId(-1, tb)
                GoTo RETRY
            End If
            idx = tb.GetIndex(tb.OldestUnreadId)
        Else
RETRY2:
            Dim tidx As Integer = ListTab.TabPages.IndexOf(ListTab.SelectedTab)
            For i As Integer = tidx To ListTab.TabPages.Count - 1
                tb = _statuses.Tabs(ListTab.TabPages(i).Text)   'tb書き換え
                If tb.UnreadCount > 0 Then
                    ListTab.SelectedIndex = i
                    lst = DirectCast(ListTab.TabPages(i).Controls(0), DetailsListView)
                    _statuses.SetNextUnreadId(-1, tb)   '頭から未読探索
                    GoTo RETRY
                End If
            Next
            If tidx > 0 Then
                '最終タブなら、先頭タブから再探索
                ListTab.SelectedIndex = 0
                GoTo RETRY2
            End If
            '未読なし
            ListTab.SelectedIndex = 0
            lst = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
            idx = 0
            If _statuses.SortOrder = SortOrder.Ascending Then idx = lst.VirtualListSize - 1
        End If

        If lst.VirtualListSize > 0 AndAlso idx > -1 Then
            SelectListItem(lst, idx)
            If _statuses.SortMode = IdComparerClass.ComparerMode.Id Then
                If _statuses.SortOrder = SortOrder.Ascending AndAlso lst.Items(idx).Position.Y > lst.ClientSize.Height - _iconSz - 10 OrElse _
                   _statuses.SortOrder = SortOrder.Descending AndAlso lst.Items(idx).Position.Y < _iconSz + 10 Then
                    MoveTop()
                Else
                    lst.EnsureVisible(idx)
                End If
            Else
                lst.EnsureVisible(idx)
            End If
        End If
        lst.Focus()
        'lst.Update()
    End Sub

    Private Sub StatusOpenMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusOpenMenuItem.Click
        If _curList.SelectedIndices.Count > 0 Then
            Dim post As PostClass = _statuses.Item(_curTab.Text, _curList.SelectedIndices(0))
            OpenUriAsync("http://twitter.com/" + post.Name + "/statuses/" + post.Id.ToString)
        End If
    End Sub

    Private Sub FavorareMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FavorareMenuItem.Click
        If _curList.SelectedIndices.Count > 0 Then
            Dim post As PostClass = _statuses.Item(_curTab.Text, _curList.SelectedIndices(0))
            OpenUriAsync("http://favotter.matope.com/user.php?user=" + post.Name)
        End If
    End Sub

    Private Sub VerUpMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VerUpMenuItem.Click
        CheckNewVersion()
    End Sub

    Private Sub CheckNewVersion(Optional ByVal startup As Boolean = False)
        Dim retMsg As String
        Dim strVer As String
        Dim forceUpdate As Boolean = My.Computer.Keyboard.ShiftKeyDown

        retMsg = Twitter.GetVersionInfo()
        If retMsg.Length > 0 Then
            strVer = retMsg.Substring(0, 4)
            If strVer.CompareTo(My.Application.Info.Version.ToString.Replace(".", "")) > 0 Then
                Dim tmp As String = String.Format(My.Resources.CheckNewVersionText3, strVer)
                If MessageBox.Show(tmp, My.Resources.CheckNewVersionText1, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    retMsg = Twitter.GetTweenBinary(strVer)
                    If retMsg.Length = 0 Then
                        retMsg = Twitter.GetTweenUpBinary()
                        If retMsg.Length = 0 Then
                            System.Diagnostics.Process.Start(My.Application.Info.DirectoryPath + "\TweenUp.exe")
                            If startup Then
                                Application.Exit()
                            Else
                                _endingFlag = True
                                Me.Close()
                            End If
                            Exit Sub
                        Else
                            If Not startup Then MessageBox.Show(My.Resources.CheckNewVersionText4, My.Resources.CheckNewVersionText2, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        End If
                    Else
                        If Not startup Then MessageBox.Show(My.Resources.CheckNewVersionText5, My.Resources.CheckNewVersionText2, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If
                End If
            Else
                If forceUpdate Then
                    Dim tmp As String = String.Format(My.Resources.CheckNewVersionText6, strVer)
                    If MessageBox.Show(tmp, My.Resources.CheckNewVersionText1, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        retMsg = Twitter.GetTweenBinary(strVer)
                        If retMsg.Length = 0 Then
                            retMsg = Twitter.GetTweenUpBinary()
                            If retMsg.Length = 0 Then
                                System.Diagnostics.Process.Start(My.Application.Info.DirectoryPath + "\TweenUp.exe")
                                If startup Then
                                    Application.Exit()
                                Else
                                    _endingFlag = True
                                    Me.Close()
                                End If
                                Exit Sub
                            Else
                                If Not startup Then MessageBox.Show(My.Resources.CheckNewVersionText4, My.Resources.CheckNewVersionText2, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            End If
                        Else
                            If Not startup Then MessageBox.Show(My.Resources.CheckNewVersionText5, My.Resources.CheckNewVersionText2, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        End If
                    End If
                ElseIf Not startup Then
                    MessageBox.Show(My.Resources.CheckNewVersionText7 + My.Application.Info.Version.ToString.Replace(".", "") + My.Resources.CheckNewVersionText8 + strVer, My.Resources.CheckNewVersionText2, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
        Else
            StatusLabel.Text = My.Resources.CheckNewVersionText9
            If Not startup Then MessageBox.Show(My.Resources.CheckNewVersionText10, My.Resources.CheckNewVersionText2, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub TimerColorize_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerColorize.Tick
        If TimerColorize.Enabled = False Then Exit Sub
        'If cMode = 0 Then Exit Sub
        'My.Application.DoEvents()
        'If cMode = 1 Then
        '    cMode = 2
        '    Exit Sub
        'End If
        'cMode = 0

        TimerColorize.Stop()
        TimerColorize.Enabled = False
        TimerColorize.Interval = 200
        'If _itemCache IsNot Nothing Then CreateCache(-1, 0)
        '_curList.BeginUpdate()
        'ColorizeList()
        'If _itemCache IsNot Nothing Then _curList.RedrawItems(_itemCacheIndex, _itemCacheIndex + _itemCache.Length - 1, False)
        DispSelectedPost()
        '_curList.EndUpdate()
        '件数関連の場合、タイトル即時書き換え
        If SettingDialog.DispLatestPost <> DispTitleEnum.None AndAlso _
           SettingDialog.DispLatestPost <> DispTitleEnum.Post AndAlso _
           SettingDialog.DispLatestPost <> DispTitleEnum.Ver Then
            SetMainWindowTitle()
        End If
        If Not StatusLabelUrl.Text.StartsWith("http") Then SetStatusLabel()
        For Each tb As TabPage In ListTab.TabPages
            If _statuses.Tabs(tb.Text).UnreadCount = 0 AndAlso tb.ImageIndex = 0 Then tb.ImageIndex = -1
        Next
    End Sub

    Private Sub DispSelectedPost()

        If _curList.SelectedIndices.Count = 0 OrElse _curPost Is Nothing Then Exit Sub

        Dim dTxt As String = detailHtmlFormat + _curPost.OriginalData + detailHtmlFormat4
        NameLabel.Text = _curPost.Name + "/" + _curPost.Nickname
        'If UserPicture.Image IsNot Nothing Then UserPicture.Image.Dispose()
        If _curPost.ImageIndex > -1 Then
            UserPicture.Image = TIconDic(_curPost.ImageUrl)
        Else
            UserPicture.Image = Nothing
        End If
        'UserPicture.Refresh()

        NameLabel.ForeColor = System.Drawing.SystemColors.ControlText
        DateTimeLabel.Text = _curPost.PDate.ToString()
        If _curPost.IsOwl AndAlso (SettingDialog.OneWayLove OrElse _curTab.Text = "Direct") Then NameLabel.ForeColor = _clOWL
        If _curPost.IsFav Then NameLabel.ForeColor = _clFav

        If PostBrowser.DocumentText <> dTxt Then
            PostBrowser.Visible = False
            PostBrowser.DocumentText = dTxt
            PostBrowser.Visible = True
        End If
    End Sub

    Private Sub MatomeMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MatomeMenuItem.Click
        OpenUriAsync("http://www5.atwiki.jp/tween/")
    End Sub

    Private Sub OfficialMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OfficialMenuItem.Click
        OpenUriAsync("http://d.hatena.ne.jp/Kiri_Feather/20071121")
    End Sub

    Private Sub DLPageMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DLPageMenuItem.Click
        OpenUriAsync("http://tween.sourceforge.jp/index.html")
    End Sub

    Private Sub ListTab_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ListTab.KeyDown
        If e.Modifiers = Keys.None Then
            ' ModifierKeyが押されていない場合
            If e.KeyCode = Keys.N OrElse e.KeyCode = Keys.Right Then
                e.Handled = True
                e.SuppressKeyPress = True
                GoRelPost(True)
                Exit Sub
            End If
            If e.KeyCode = Keys.P OrElse e.KeyCode = Keys.Left Then
                e.Handled = True
                e.SuppressKeyPress = True
                GoRelPost(False)
                Exit Sub
            End If
            If e.KeyCode = Keys.OemPeriod Then
                e.Handled = True
                e.SuppressKeyPress = True
                GoAnchor()
                Exit Sub
            End If
            _anchorFlag = False
            If e.KeyCode = Keys.Space OrElse e.KeyCode = Keys.ProcessKey Then
                e.Handled = True
                e.SuppressKeyPress = True
                JumpUnreadMenuItem_Click(Nothing, Nothing)
            End If
            If e.KeyCode = Keys.Enter OrElse e.KeyCode = Keys.Return Then
                e.Handled = True
                e.SuppressKeyPress = True
                MakeReplyOrDirectStatus()
            End If
            If e.KeyCode = Keys.L Then
                e.Handled = True
                e.SuppressKeyPress = True
                GoPost(True)
            End If
            If e.KeyCode = Keys.H Then
                e.Handled = True
                e.SuppressKeyPress = True
                GoPost(False)
            End If
            If e.KeyCode = Keys.Z Or e.KeyCode = Keys.Oemcomma Then
                e.Handled = True
                e.SuppressKeyPress = True
                MoveTop()
            End If
            If e.KeyCode = Keys.R Then
                e.Handled = True
                e.SuppressKeyPress = True
                DoRefresh()
            End If
        End If
        _anchorFlag = False
        If e.Control AndAlso Not e.Alt AndAlso Not e.Shift Then
            ' CTRLキーが押されている場合
            If e.KeyCode = Keys.Home OrElse e.KeyCode = Keys.End Then
                TimerColorize.Stop()
                TimerColorize.Start()
                'cMode = 1
            End If
            If e.KeyCode = Keys.A Then
                For i As Integer = 0 To _curList.VirtualListSize - 1
                    _curList.SelectedIndices.Add(i)
                Next
            End If
        End If
        If Not e.Control AndAlso e.Alt AndAlso Not e.Shift Then
            ' ALTキーが押されている場合
            ' 別タブの同じ書き込みへ(ALT+←/→)
            If e.KeyCode = Keys.Right Then
                e.Handled = True
                e.SuppressKeyPress = True
                GoSamePostToAnotherTab(False)
            End If
            If e.KeyCode = Keys.Left Then
                e.Handled = True
                e.SuppressKeyPress = True
                GoSamePostToAnotherTab(True)
            End If
        End If
        If e.Shift AndAlso Not e.Control AndAlso Not e.Alt Then
            ' SHIFTキーが押されている場合
            If e.KeyCode = Keys.H Then
                e.Handled = True
                e.SuppressKeyPress = True
                GoTopEnd(True)
            End If
            If e.KeyCode = Keys.L Then
                e.Handled = True
                e.SuppressKeyPress = True
                GoTopEnd(False)
            End If
            If e.KeyCode = Keys.M Then
                e.Handled = True
                e.SuppressKeyPress = True
                GoMiddle()
            End If
            If e.KeyCode = Keys.G Then
                e.Handled = True
                e.SuppressKeyPress = True
                GoLast()
            End If
            If e.KeyCode = Keys.Z Then
                e.Handled = True
                e.SuppressKeyPress = True
                MoveMiddle()
            End If

            ' お気に入り前後ジャンプ(SHIFT+N←/P→)
            If e.KeyCode = Keys.N OrElse e.KeyCode = Keys.Right Then
                e.Handled = True
                e.SuppressKeyPress = True
                GoFav(True)
            End If
            If e.KeyCode = Keys.P OrElse e.KeyCode = Keys.Left Then
                e.Handled = True
                e.SuppressKeyPress = True
                GoFav(False)
            End If

        End If
        If Not e.Alt Then
            If e.KeyCode = Keys.J Then
                e.Handled = True
                e.SuppressKeyPress = True
                SendKeys.Send("{DOWN}")
            End If
            If e.KeyCode = Keys.K Then
                e.Handled = True
                e.SuppressKeyPress = True
                SendKeys.Send("{UP}")
            End If
        End If
        If e.KeyCode = Keys.C Then
            Dim clstr As String = ""
            If e.Control AndAlso Not e.Alt AndAlso Not e.Shift Then
                e.Handled = True
                e.SuppressKeyPress = True
                CopyStot()
            End If
            If e.Control AndAlso e.Shift AndAlso Not e.Alt Then
                e.Handled = True
                e.SuppressKeyPress = True
                CopyIdUri()
            End If
        End If
    End Sub

    Private Sub CopyStot()
        Dim clstr As String = ""
        Dim sb As New StringBuilder()
        For Each idx As Integer In _curList.SelectedIndices
            Dim post As PostClass = _statuses.Item(_curTab.Text, idx)
            sb.AppendFormat("{0}:{1} [http://twitter.com/{0}/statuses/{2}]{3}", post.Name, post.Data, post.Id, Environment.NewLine)
        Next
        If sb.Length > 0 Then
            clstr = sb.ToString()
            Clipboard.SetDataObject(clstr, False, 5, 100)
        End If
    End Sub

    Private Sub CopyIdUri()
        Dim clstr As String = ""
        Dim sb As New StringBuilder()
        For Each idx As Integer In _curList.SelectedIndices
            Dim post As PostClass = _statuses.Item(_curTab.Text, idx)
            sb.AppendFormat("http://twitter.com/{0}/statuses/{1}{2}", post.Name, post.Id, Environment.NewLine)
        Next
        If sb.Length > 0 Then
            clstr = sb.ToString()
            Clipboard.SetDataObject(clstr, False, 5, 100)
        End If
    End Sub

    Private Sub GoFav(ByVal forward As Boolean)
        If _curList.VirtualListSize = 0 Then Exit Sub
        Dim fIdx As Integer = 0
        Dim toIdx As Integer = 0
        Dim stp As Integer = 1

        If forward Then
            If _curList.SelectedIndices.Count = 0 Then
                fIdx = 0
            Else
                fIdx = _curList.SelectedIndices(0) + 1
                If fIdx > _curList.VirtualListSize - 1 Then Exit Sub
            End If
            toIdx = _curList.VirtualListSize - 1
            stp = 1
        Else
            If _curList.SelectedIndices.Count = 0 Then
                fIdx = _curList.VirtualListSize - 1
            Else
                fIdx = _curList.SelectedIndices(0) - 1
                If fIdx < 0 Then Exit Sub
            End If
            toIdx = 0
            stp = -1
        End If

        For idx As Integer = fIdx To toIdx Step stp
            If _statuses.Item(_curTab.Text, idx).IsFav Then
                SelectListItem(_curList, idx)
                _curList.EnsureVisible(idx)
                Exit For
            End If
        Next
    End Sub

    Private Sub GoSamePostToAnotherTab(ByVal left As Boolean)
        If _curList.VirtualListSize = 0 Then Exit Sub
        Dim fIdx As Integer = 0
        Dim toIdx As Integer = 0
        Dim stp As Integer = 1
        Dim targetId As Long = 0

        If _curTab.Text = "Direct" Then Exit Sub ' Directタブは対象外（見つかるはずがない）
        If _curList.SelectedIndices.Count = 0 Then Exit Sub '未選択も処理しない

        targetId = GetCurTabPost(_curList.SelectedIndices(0)).Id

        If left Then
            ' 左のタブへ
            If ListTab.SelectedIndex = 0 Then
                Exit Sub
            Else
                fIdx = ListTab.SelectedIndex - 1
            End If
            toIdx = 0
            stp = -1
        Else
            ' 右のタブへ
            If ListTab.SelectedIndex = ListTab.TabCount - 1 Then
                Exit Sub
            Else
                fIdx = ListTab.SelectedIndex + 1
            End If
            toIdx = ListTab.TabCount - 1
            stp = 1
        End If

        Dim found As Boolean = False
        For tabidx As Integer = fIdx To toIdx Step stp
            If ListTab.TabPages(tabidx).Text = "Direct" Then Continue For ' Directタブは対象外
            '_itemCache = Nothing
            '_postCache = Nothing
            For idx As Integer = 0 To DirectCast(ListTab.TabPages(tabidx).Controls(0), DetailsListView).VirtualListSize - 1
                If _statuses.Item(ListTab.TabPages(tabidx).Text, idx).Id = targetId Then
                    ListTab.SelectedIndex = tabidx
                    ListTabSelect(ListTab.TabPages(tabidx))
                    SelectListItem(_curList, idx)
                    _curList.EnsureVisible(idx)
                    found = True
                    Exit For
                End If
            Next
            If found Then Exit For
        Next
        '_itemCache = Nothing
        '_postCache = Nothing
    End Sub

    Private Sub GoPost(ByVal forward As Boolean)
        If _curList.SelectedIndices.Count = 0 OrElse _curPost Is Nothing Then Exit Sub
        Dim fIdx As Integer = 0
        Dim toIdx As Integer = 0
        Dim stp As Integer = 1

        If forward Then
            fIdx = _curList.SelectedIndices(0) + 1
            If fIdx > _curList.VirtualListSize - 1 Then Exit Sub
            toIdx = _curList.VirtualListSize - 1
            stp = 1
        Else
            fIdx = _curList.SelectedIndices(0) - 1
            If fIdx < 0 Then Exit Sub
            toIdx = 0
            stp = -1
        End If

        For idx As Integer = fIdx To toIdx Step stp
            If _statuses.Item(_curTab.Text, idx).Name = _curPost.Name Then
                SelectListItem(_curList, idx)
                _curList.EnsureVisible(idx)
                Exit For
            End If
        Next
    End Sub

    Private Sub GoRelPost(ByVal forward As Boolean)
        If _curList.SelectedIndices.Count = 0 Then Exit Sub

        Dim fIdx As Integer = 0
        Dim toIdx As Integer = 0
        Dim stp As Integer = 1
        If forward Then
            fIdx = _curList.SelectedIndices(0) + 1
            If fIdx > _curList.VirtualListSize - 1 Then Exit Sub
            toIdx = _curList.VirtualListSize - 1
            stp = 1
        Else
            fIdx = _curList.SelectedIndices(0) - 1
            If fIdx < 0 Then Exit Sub
            toIdx = 0
            stp = -1
        End If

        If Not _anchorFlag Then
            _anchorPost = _curPost
            _anchorFlag = True
        End If

        For idx As Integer = fIdx To toIdx Step stp
            Dim post As PostClass = _statuses.Item(_curTab.Text, idx)
            If post.Name = _anchorPost.Name OrElse _
               _anchorPost.ReplyToList.Contains(post.Name.ToLower()) OrElse _
               post.ReplyToList.Contains(_anchorPost.Name.ToLower()) Then
                SelectListItem(_curList, idx)
                _curList.EnsureVisible(idx)
                Exit For
            End If
        Next
    End Sub

    Private Sub GoAnchor()
        If _anchorPost Is Nothing Then Exit Sub
        Dim idx As Integer = _statuses.Tabs(_curTab.Text).GetIndex(_anchorPost.Id)
        If idx = -1 Then Exit Sub

        SelectListItem(_curList, idx)
        _curList.EnsureVisible(idx)
    End Sub

    Private Sub GoTopEnd(ByVal GoTop As Boolean)
        Dim _item As ListViewItem
        Dim idx As Integer

        If GoTop Then
            _item = _curList.GetItemAt(0, 25)
            If _item Is Nothing Then
                idx = 0
            Else
                idx = _item.Index
            End If
        Else
            _item = _curList.GetItemAt(0, _curList.ClientSize.Height - 1)
            If _item Is Nothing Then
                idx = _curList.VirtualListSize - 1
            Else
                idx = _item.Index
            End If
        End If
        SelectListItem(_curList, idx)
    End Sub

    Private Sub GoMiddle()
        Dim _item As ListViewItem
        Dim idx1 As Integer
        Dim idx2 As Integer
        Dim idx3 As Integer

        _item = _curList.GetItemAt(0, 0)
        If _item Is Nothing Then
            idx1 = 0
        Else
            idx1 = _item.Index
        End If
        _item = _curList.GetItemAt(0, _curList.ClientSize.Height - 1)
        If _item Is Nothing Then
            idx2 = _curList.VirtualListSize - 1
        Else
            idx2 = _item.Index
        End If
        idx3 = (idx1 + idx2) \ 2

        SelectListItem(_curList, idx3)
    End Sub

    Private Sub GoLast()
        If _curList.VirtualListSize = 0 Then Exit Sub

        If _statuses.SortOrder = SortOrder.Ascending Then
            SelectListItem(_curList, _curList.VirtualListSize - 1)
            _curList.EnsureVisible(_curList.VirtualListSize - 1)
        Else
            SelectListItem(_curList, 0)
            _curList.EnsureVisible(0)
        End If
    End Sub

    Private Sub MoveTop()
        If _curList.SelectedIndices.Count = 0 Then Exit Sub
        Dim idx As Integer = _curList.SelectedIndices(0)
        If _statuses.SortOrder = SortOrder.Ascending Then
            _curList.EnsureVisible(_curList.VirtualListSize - 1)
        Else
            _curList.EnsureVisible(0)
        End If
        _curList.EnsureVisible(idx)
    End Sub

    Private Sub MyList_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        _anchorFlag = False
    End Sub

    Private Sub StatusText_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusText.Enter
        ' フォーカスの戻り先を StatusText に設定
        Me.Tag = StatusText
        StatusText.BackColor = Color.LemonChiffon
    End Sub

    Private Sub StatusText_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusText.Leave
        ' フォーカスがメニューに遷移しないならばフォーカスはタブに移ることを期待
        If ListTab.SelectedTab IsNot Nothing AndAlso MenuStrip1.Tag Is Nothing Then Me.Tag = ListTab.SelectedTab.Controls(0)
        StatusText.BackColor = Color.FromKnownColor(KnownColor.Window)
    End Sub

    Private Sub StatusText_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles StatusText.KeyDown
        If e.Control AndAlso Not e.Alt AndAlso Not e.Shift Then
            If e.KeyCode = Keys.A Then
                StatusText.SelectAll()
            ElseIf e.KeyCode = Keys.Up OrElse e.KeyCode = Keys.Down Then
                If StatusText.Text.Trim() <> "" Then _history(_hisIdx) = StatusText.Text
                If e.KeyCode = Keys.Up Then
                    _hisIdx -= 1
                    If _hisIdx < 0 Then _hisIdx = 0
                Else
                    _hisIdx += 1
                    If _hisIdx > _history.Count - 1 Then _hisIdx = _history.Count - 1
                End If
                StatusText.Text = _history(_hisIdx)
                StatusText.SelectionStart = StatusText.Text.Length
                e.Handled = True
                e.SuppressKeyPress = True
            ElseIf e.KeyCode = Keys.PageUp Then
                If ListTab.SelectedIndex = 0 Then
                    ListTab.SelectedIndex = ListTab.TabCount - 1
                Else
                    ListTab.SelectedIndex -= 1
                End If
                e.Handled = True
                e.SuppressKeyPress = True
                StatusText.Focus()
            ElseIf e.KeyCode = Keys.PageDown Then
                If ListTab.SelectedIndex = ListTab.TabCount - 1 Then
                    ListTab.SelectedIndex = 0
                Else
                    ListTab.SelectedIndex += 1
                End If
                e.Handled = True
                e.SuppressKeyPress = True
                StatusText.Focus()
            End If
        End If
    End Sub

    Private Sub SaveConfigs()
        If _username <> "" AndAlso _password <> "" Then
            SyncLock _syncObject
                _section.FormSize = _mySize
                _section.FormLocation = _myLoc
                _section.SplitterDistance = _mySpDis
                _section.StatusMultiline = StatusText.Multiline
                _section.StatusTextHeight = _mySpDis2
                _section.UserName = _username
                _section.Password = _password
                _section.NextPageThreshold = SettingDialog.NextPageThreshold
                _section.NextPages = SettingDialog.NextPagesInt
                _section.TimelinePeriod = SettingDialog.TimelinePeriodInt
                _section.DMPeriod = SettingDialog.DMPeriodInt
                _section.MaxPostNum = SettingDialog.MaxPostNum
                _section.ReadPages = SettingDialog.ReadPages
                _section.ReadPagesReply = SettingDialog.ReadPagesReply
                _section.ReadPagesDM = SettingDialog.ReadPagesDM
                _section.Readed = SettingDialog.Readed
                _section.IconSize = SettingDialog.IconSz
                _section.StatusText = SettingDialog.Status
                _section.UnreadManage = SettingDialog.UnreadManage
                _section.PlaySound = SettingDialog.PlaySound
                _section.OneWayLove = SettingDialog.OneWayLove

                _section.FontUnread = _fntUnread
                _section.ColorUnread = _clUnread
                _section.FontReaded = _fntReaded
                _section.ColorReaded = _clReaded
                _section.FontDetail = _fntDetail
                _section.ColorFav = _clFav
                _section.ColorOWL = _clOWL
                _section.ColorSelf = _clSelf
                _section.ColorAtSelf = _clAtSelf
                _section.ColorTarget = _clTarget
                _section.ColorAtTarget = _clAtTarget
                _section.ColorAtFromTarget = _clAtFromTarget

                _section.NameBalloon = SettingDialog.NameBalloon

                _section.PostCtrlEnter = SettingDialog.PostCtrlEnter
                _section.UseAPI = True
                _section.HubServer = SettingDialog.HubServer
                _section.BrowserPath = SettingDialog.BrowserPath
                _section.CheckReply = SettingDialog.CheckReply
                _section.UseRecommendStatus = SettingDialog.UseRecommendStatus
                _section.DispUsername = SettingDialog.DispUsername
                _section.MinimizeToTray = SettingDialog.MinimizeToTray
                _section.CloseToExit = SettingDialog.CloseToExit
                _section.DispLatestPost = SettingDialog.DispLatestPost
                _section.SortOrderLock = SettingDialog.SortOrderLock
                _section.TinyURLResolve = SettingDialog.TinyUrlResolve
                _section.ProxyType = SettingDialog.ProxyType
                _section.ProxyAddress = SettingDialog.ProxyAddress
                _section.ProxyPort = SettingDialog.ProxyPort
                _section.ProxyUser = SettingDialog.ProxyUser
                _section.ProxyPassword = SettingDialog.ProxyPassword
                _section.PeriodAdjust = SettingDialog.PeriodAdjust
                _section.StartupVersion = SettingDialog.StartupVersion
                _section.StartupKey = SettingDialog.StartupKey
                _section.StartupFollowers = SettingDialog.StartupFollowers
                _section.RestrictFavCheck = SettingDialog.RestrictFavCheck
                _section.AlwaysTop = SettingDialog.AlwaysTop
                _section.UrlConvertAuto = SettingDialog.UrlConvertAuto
                _section.Outputz = SettingDialog.OutputzEnabled
                _section.OutputzKey = SettingDialog.OutputzKey
                _section.OutputzUrlmode = SettingDialog.OutputzUrlmode
                _section.UseUnreadStyle = SettingDialog.UseUnreadStyle
                _section.DateTimeFormat = SettingDialog.DateTimeFormat

                _section.SortOrder = _statuses.SortOrder
                Select Case _statuses.SortMode
                    Case IdComparerClass.ComparerMode.Nickname  'ニックネーム
                        _section.SortColumn = 1
                    Case IdComparerClass.ComparerMode.Data  '本文
                        _section.SortColumn = 2
                    Case IdComparerClass.ComparerMode.Id  '時刻=発言Id
                        _section.SortColumn = 3
                    Case IdComparerClass.ComparerMode.Name  '名前
                        _section.SortColumn = 4
                    Case IdComparerClass.ComparerMode.Source  'Source
                        _section.SortColumn = 7
                End Select

                Dim cnt As Integer = 0
                If ListTab IsNot Nothing AndAlso _
                   ListTab.TabPages IsNot Nothing AndAlso _
                   ListTab.TabPages.Count > 0 Then
                    _section.ListElement.Clear()
                    _section.SelectedUser.Clear()
                    For Each tp As TabPage In ListTab.TabPages
                        Dim tabName As String = tp.Text
                        _section.ListElement.Add(New ListElement(tabName))
                        Dim tab As TabClass = _statuses.Tabs(tabName)
                        _section.ListElement(tabName).Notify = tab.Notify
                        _section.ListElement(tabName).SoundFile = tab.SoundFile
                        _section.ListElement(tabName).UnreadManage = tab.UnreadManage
                        For Each fc As FiltersClass In tab.GetFilters
                            Dim bf As String = ""
                            For Each bfs As String In fc.BodyFilter
                                bf += " " + bfs
                            Next
                            Dim su As New SelectedUser(cnt.ToString)
                            cnt += 1
                            su.BodyFilter = bf
                            su.IdFilter = fc.NameFilter
                            su.MoveFrom = fc.MoveFrom
                            su.SetMark = fc.SetMark
                            su.SearchBoth = fc.SearchBoth
                            su.UrlSearch = fc.SearchUrl
                            su.RegexEnable = fc.UseRegex
                            su.TabName = tabName
                            _section.SelectedUser.Add(su)
                        Next
                    Next
                End If
                _config.Save()
            End SyncLock
        End If
    End Sub

    Private Sub SaveLogMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveLogMenuItem.Click
        Dim rslt As DialogResult = MessageBox.Show(String.Format(My.Resources.SaveLogMenuItem_ClickText1, Environment.NewLine), _
                My.Resources.SaveLogMenuItem_ClickText2, _
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
        If rslt = Windows.Forms.DialogResult.Cancel Then Exit Sub

        SaveFileDialog1.FileName = "TweenPosts" + Format(Now, "yyMMdd-HHmmss") + ".tsv"
        SaveFileDialog1.InitialDirectory = My.Application.Info.DirectoryPath
        SaveFileDialog1.Filter = My.Resources.SaveLogMenuItem_ClickText3
        SaveFileDialog1.FilterIndex = 0
        SaveFileDialog1.Title = My.Resources.SaveLogMenuItem_ClickText4
        SaveFileDialog1.RestoreDirectory = True

        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            If Not SaveFileDialog1.ValidateNames Then Exit Sub
            Using sw As StreamWriter = New StreamWriter(SaveFileDialog1.FileName, False, Encoding.UTF8)
                If rslt = Windows.Forms.DialogResult.Yes Then
                    'All
                    For idx As Integer = 0 To _curList.VirtualListSize - 1
                        Dim post As PostClass = _statuses.Item(_curTab.Text, idx)
                        sw.WriteLine(post.Nickname & vbTab & _
                                 """" & post.Data.Replace(vbLf, "").Replace("""", """""") + """" & vbTab & _
                                 post.PDate.ToString() & vbTab & _
                                 post.Name & vbTab & _
                                 post.Id.ToString() & vbTab & _
                                 post.ImageUrl & vbTab & _
                                 """" & post.OriginalData.Replace(vbLf, "").Replace("""", """""") + """")
                    Next
                Else
                    For Each idx As Integer In _curList.SelectedIndices
                        Dim post As PostClass = _statuses.Item(_curTab.Text, idx)
                        sw.WriteLine(post.Nickname & vbTab & _
                                 """" & post.Data.Replace(vbLf, "").Replace("""", """""") + """" & vbTab & _
                                 post.PDate.ToString() & vbTab & _
                                 post.Name & vbTab & _
                                 post.Id.ToString() & vbTab & _
                                 post.ImageUrl & vbTab & _
                                 """" & post.OriginalData.Replace(vbLf, "").Replace("""", """""") + """")
                    Next
                End If
            End Using
        End If
        Me.TopMost = SettingDialog.AlwaysTop
    End Sub

    Private Sub PostBrowser_PreviewKeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles PostBrowser.PreviewKeyDown
        If e.KeyCode = Keys.F5 Then
            e.IsInputKey = True
            DoRefresh()
        End If
        If e.Modifiers = Keys.None AndAlso (e.KeyCode = Keys.Space OrElse e.KeyCode = Keys.ProcessKey) Then
            e.IsInputKey = True
            JumpUnreadMenuItem_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub Tabs_DoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListTab.MouseDoubleClick
        'タブ名変更
        If IsDefaultTab(ListTab.SelectedTab.Text) Then Exit Sub
        Dim inputName As New InputTabName()
        inputName.TabName = ListTab.SelectedTab.Text
        inputName.ShowDialog()
        Dim newTabText As String = inputName.TabName
        inputName.Dispose()
        Me.TopMost = SettingDialog.AlwaysTop
        If newTabText <> "" Then
            _statuses.RenameTab(ListTab.SelectedTab.Text, newTabText)
            ListTab.SelectedTab.Text = newTabText
            'タブ名のリスト作り直し
            For i As Integer = 0 To ListTab.TabCount - 1
                If Not IsDefaultTab(ListTab.TabPages(i).Text) AndAlso _
                   ListTab.TabPages(i).Text <> newTabText Then
                    TabDialog.RemoveTab(ListTab.TabPages(i).Text)
                End If
            Next
            For i As Integer = 0 To ListTab.TabCount - 1
                If Not IsDefaultTab(ListTab.TabPages(i).Text) Then
                    TabDialog.AddTab(ListTab.TabPages(i).Text)
                End If
            Next
            SaveConfigs()
            _rclickTabName = newTabText
        End If
    End Sub

    Private Sub Tabs_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListTab.MouseDown
        Dim cpos As New Point(e.X, e.Y)
        If e.Button = Windows.Forms.MouseButtons.Left Then
            For i As Integer = 0 To ListTab.TabPages.Count - 1
                Dim rect As Rectangle = ListTab.GetTabRect(i)
                If rect.Left <= cpos.X AndAlso cpos.X <= rect.Right AndAlso _
                   rect.Top <= cpos.Y AndAlso cpos.Y <= rect.Bottom Then
                    _tabDrag = True
                    Exit For
                End If
            Next
        Else
            _tabDrag = False
        End If
    End Sub

    Private Sub Tabs_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles ListTab.DragEnter
        If e.Data.GetDataPresent(GetType(TabPage)) Then
            e.Effect = DragDropEffects.Move
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub Tabs_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles ListTab.DragDrop
        If Not e.Data.GetDataPresent(GetType(TabPage)) Then Exit Sub

        _tabDrag = False
        Dim tn As String = ""
        Dim bef As Boolean
        Dim cpos As New Point(e.X, e.Y)
        Dim spos As Point = ListTab.PointToClient(cpos)
        Dim i As Integer
        For i = 0 To ListTab.TabPages.Count - 1
            Dim rect As Rectangle = ListTab.GetTabRect(i)
            If rect.Left <= spos.X AndAlso spos.X <= rect.Right AndAlso _
               rect.Top <= spos.Y AndAlso spos.Y <= rect.Bottom Then
                tn = ListTab.TabPages(i).Text
                If spos.X <= (rect.Left + rect.Right) / 2 Then
                    bef = True
                Else
                    bef = False
                End If
                Exit For
            End If
        Next

        'タブのないところにドロップ->最後尾へ移動
        If tn = "" Then
            tn = ListTab.TabPages(ListTab.TabPages.Count - 1).Text
            bef = False
            i = ListTab.TabPages.Count - 1
        End If

        'Dim ts As TabStructure = DirectCast(e.Data.GetData(GetType(TabStructure)), Tween.TabStructure)
        Dim tp As TabPage = DirectCast(e.Data.GetData(GetType(TabPage)), TabPage)
        If tp.Text = tn Then Exit Sub

        Dim mTp As TabPage = Nothing
        ListTab.SuspendLayout()
        For j As Integer = 0 To ListTab.TabPages.Count - 1
            If ListTab.TabPages(j).Text = tp.Text Then
                mTp = ListTab.TabPages(j)
                ListTab.TabPages.Remove(mTp)
                If j < i Then i -= 1
                Exit For
            End If
        Next
        If bef Then
            ListTab.TabPages.Insert(i, mTp)
        Else
            ListTab.TabPages.Insert(i + 1, mTp)
        End If

        ListTab.ResumeLayout()
    End Sub

    Private Sub MakeReplyOrDirectStatus(Optional ByVal isAuto As Boolean = True, Optional ByVal isReply As Boolean = True, Optional ByVal isAll As Boolean = False)
        'isAuto:True=先頭に挿入、False=カーソル位置に挿入
        'isReply:True=@,False=DM
        If Not StatusText.Enabled Then Exit Sub

        ' 複数あてリプライはReplyではなく通常ポスト

        If _curList.SelectedIndices.Count > 0 Then
            ' アイテムが1件以上選択されている
            If _curList.SelectedIndices.Count = 1 AndAlso Not isAll AndAlso _curPost IsNot Nothing Then
                ' 単独ユーザー宛リプライまたはDM
                If (ListTab.SelectedTab.Text = "Direct" AndAlso isAuto) OrElse (Not isAuto AndAlso Not isReply) Then
                    ' ダイレクトメッセージ
                    StatusText.Text = "D " + _curPost.Name + " " + StatusText.Text
                    StatusText.SelectionStart = StatusText.Text.Length
                    StatusText.Focus()
                    _reply_to_id = 0
                    _reply_to_name = Nothing
                    Exit Sub
                End If
                If StatusText.Text = "" Then
                    ' ステータステキストが入力されていない場合先頭に@ユーザー名を追加する
                    StatusText.Text = "@" + _curPost.Name + " "
                    _reply_to_id = _curPost.Id
                    _reply_to_name = _curPost.Name
                Else
                    If isAuto Then
                        If StatusText.Text.Contains("@" + _curPost.Name + " ") Then Exit Sub
                        If Not StatusText.Text.StartsWith("@") Then
                            If StatusText.Text.StartsWith(". ") Then
                                ' 複数リプライ
                                StatusText.Text = StatusText.Text.Insert(2, "@" + _curPost.Name + " ")
                                _reply_to_id = 0
                                _reply_to_name = Nothing
                            Else
                                ' 単独リプライ
                                StatusText.Text = "@" + _curPost.Name + " " + StatusText.Text
                                _reply_to_id = _curPost.Id
                                _reply_to_name = _curPost.Name
                            End If
                        Else
                            ' 複数リプライ
                            StatusText.Text = ". @" + _curPost.Name + " " + StatusText.Text
                            _reply_to_id = 0
                            _reply_to_name = Nothing
                        End If
                    Else
                        Dim sidx As Integer = StatusText.SelectionStart
                        If StatusText.Text.StartsWith("@") Then
                            '複数リプライ
                            StatusText.Text = ". " + StatusText.Text.Insert(sidx, " @" + _curPost.Name + " ")
                            sidx += 5 + _curPost.Name.Length
                        Else
                            ' 複数リプライ
                            StatusText.Text = StatusText.Text.Insert(sidx, " @" + _curPost.Name + " ")
                            sidx += 3 + _curPost.Name.Length
                        End If
                        StatusText.SelectionStart = sidx
                        StatusText.Focus()
                        _reply_to_id = 0
                        _reply_to_name = Nothing
                        Exit Sub
                    End If
                End If
            Else
                ' 複数リプライ
                If Not isAuto AndAlso Not isReply Then Exit Sub

                If isAuto Then
                    Dim sTxt As String = StatusText.Text
                    If Not sTxt.StartsWith(". ") Then
                        sTxt = ". " + sTxt
                    End If
                    For cnt As Integer = 0 To _curList.SelectedIndices.Count - 1
                        Dim post As PostClass = _statuses.Item(_curTab.Text, _curList.SelectedIndices(cnt))
                        If Not sTxt.Contains("@" + post.Name + " ") Then
                            sTxt = sTxt.Insert(2, "@" + post.Name + " ")
                        End If
                    Next
                    StatusText.Text = sTxt
                Else
                    Dim ids As String = ""
                    Dim sidx As Integer = StatusText.SelectionStart
                    For cnt As Integer = 0 To _curList.SelectedIndices.Count - 1
                        Dim post As PostClass = _statuses.Item(_curTab.Text, _curList.SelectedIndices(cnt))
                        If Not ids.Contains("@" + post.Name + " ") AndAlso _
                           post.Name <> _username Then
                            ids += "@" + post.Name + " "
                        End If
                        If isAll Then
                            For Each nm As String In post.ReplyToList
                                If Not ids.Contains("@" + nm + " ") AndAlso _
                                   nm <> _username Then
                                    ids += "@" + nm + " "
                                End If
                            Next
                        End If
                    Next
                    If ids.Length = 0 Then Exit Sub
                    If Not StatusText.Text.StartsWith(". ") Then
                        StatusText.Text = ". " + StatusText.Text
                        sidx += 2
                    End If
                    If sidx > 0 Then
                        If StatusText.Text.Substring(sidx - 1, 1) <> " " Then
                            ids = " " + ids
                        End If
                    End If
                    If StatusText.Text.StartsWith("@") Then
                        StatusText.Text = ". " + StatusText.Text.Insert(sidx, ids)
                        sidx += 2 + ids.Length
                    Else
                        StatusText.Text = StatusText.Text.Insert(sidx, ids)
                        sidx += 1 + ids.Length
                    End If
                    StatusText.SelectionStart = sidx
                    StatusText.Focus()
                    Exit Sub
                End If
            End If
            StatusText.SelectionStart = StatusText.Text.Length
            StatusText.Focus()
        End If
    End Sub

    Private Sub ListTab_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListTab.MouseUp
        _tabDrag = False
    End Sub

    Private Sub TimerRefreshIcon_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerRefreshIcon.Tick
        If Not TimerRefreshIcon.Enabled Then Exit Sub
        Static iconCnt As Integer = 0

        iconCnt += 1
        If iconCnt > 3 Then iconCnt = 0

        NotifyIcon1.Icon = NIconRefresh(iconCnt)
    End Sub

    Private Sub ContextMenuTabProperty_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuTabProperty.Opening
        '右クリックの場合はタブ名が設定済。アプリケーションキーの場合は現在のタブを対象とする
        If _rclickTabName = "" OrElse ContextMenuTabProperty.OwnerItem IsNot Nothing Then _rclickTabName = ListTab.SelectedTab.Text

        Dim tb As TabClass = _statuses.Tabs(_rclickTabName)

        NotifyDispMenuItem.Checked = tb.Notify
        SoundFileComboBox.Items.Clear()
        SoundFileComboBox.Items.Add("")
        Dim oDir As IO.DirectoryInfo = New IO.DirectoryInfo(My.Application.Info.DirectoryPath)
        For Each oFile As IO.FileInfo In oDir.GetFiles("*.wav")
            SoundFileComboBox.Items.Add(oFile.Name)
        Next
        Dim idx As Integer = SoundFileComboBox.Items.IndexOf(tb.SoundFile)
        If idx = -1 Then idx = 0
        SoundFileComboBox.SelectedIndex = idx
        UreadManageMenuItem.Checked = tb.UnreadManage
        If IsDefaultTab(_rclickTabName) Then
            FilterEditMenuItem.Enabled = False
            DeleteTabMenuItem.Enabled = False
        Else
            FilterEditMenuItem.Enabled = True
            DeleteTabMenuItem.Enabled = True
        End If
    End Sub

    Private Sub UreadManageMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UreadManageMenuItem.Click
        If _rclickTabName = "" Then Exit Sub

        _statuses.SetTabUnreadManage(_rclickTabName, UreadManageMenuItem.Checked)
        If _curTab.Text = _rclickTabName Then
            If _statuses.Tabs(_rclickTabName).UnreadCount > 0 Then
                _curTab.ImageIndex = 0
            Else
                _curTab.ImageIndex = -1
            End If
            _itemCache = Nothing
            _postCache = Nothing
            _curList.Refresh()
        End If
        SetMainWindowTitle()
        SetStatusLabel()
    End Sub

    Private Sub NotifyDispMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NotifyDispMenuItem.Click
        If _rclickTabName = "" Then Exit Sub

        Dim tb As TabClass = _statuses.Tabs(_rclickTabName)
        tb.Notify = NotifyDispMenuItem.Checked
    End Sub

    Private Sub SoundFileComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SoundFileComboBox.SelectedIndexChanged
        If _rclickTabName = "" Then Exit Sub

        Dim tb As TabClass = _statuses.Tabs(_rclickTabName)
        tb.SoundFile = DirectCast(SoundFileComboBox.SelectedItem, String)
    End Sub

    Private Sub DeleteTabMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DeleteTabMenuItem.Click
        If _rclickTabName = "" Then Exit Sub

        RemoveSpecifiedTab(_rclickTabName)
        _rclickTabName = ""
    End Sub

    Private Sub FilterEditMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FilterEditMenuItem.Click
        If _rclickTabName = "" OrElse IsDefaultTab(_rclickTabName) Then Exit Sub

        fDialog.SetCurrent(_rclickTabName)
        fDialog.ShowDialog()
        SaveConfigs()
        Me.TopMost = SettingDialog.AlwaysTop

        Try
            Me.Cursor = Cursors.WaitCursor
            _itemCache = Nothing
            _postCache = Nothing
            _curPost = Nothing
            _curItemIndex = -1
            _statuses.FilterAll()
            For Each tb As TabPage In ListTab.TabPages
                DirectCast(tb.Controls(0), DetailsListView).VirtualListSize = _statuses.Tabs(tb.Text).AllCount
                If _statuses.Tabs(tb.Text).UnreadCount > 0 Then
                    tb.ImageIndex = 0
                Else
                    tb.ImageIndex = -1
                End If
            Next
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub AddTabMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddTabMenuItem.Click
        Dim inputName As New InputTabName()
        inputName.TabName = "MyTab" + ListTab.TabPages.Count.ToString
        inputName.ShowDialog()
        Dim tabName As String = inputName.TabName
        inputName.Dispose()
        Me.TopMost = SettingDialog.AlwaysTop
        If tabName <> "" Then
            If Not AddNewTab(tabName, False) Then
                Dim tmp As String = String.Format(My.Resources.AddTabMenuItem_ClickText1, tabName)
                MessageBox.Show(tmp, My.Resources.AddTabMenuItem_ClickText2, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Else
                '成功
                _statuses.AddTab(tabName)
                SaveConfigs()
            End If
        End If
    End Sub

    Private Sub TabMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabMenuItem.Click
        '選択発言を元にフィルタ追加
        For Each idx As Integer In _curList.SelectedIndices
            Dim tabName As String = ""
            Do
                '振り分け先タブ選択
                If TabDialog.ShowDialog = Windows.Forms.DialogResult.Cancel Then
                    Me.TopMost = SettingDialog.AlwaysTop
                    Exit Sub
                End If
                Me.TopMost = SettingDialog.AlwaysTop
                tabName = TabDialog.SelectedTabName

                ListTab.SelectedTab.Focus()
                '新規タブが選択→タブ追加
                If tabName = My.Resources.TabMenuItem_ClickText1 Then
                    Dim inputName As New InputTabName()
                    inputName.TabName = "MyTab" + ListTab.TabPages.Count.ToString
                    inputName.ShowDialog()
                    tabName = inputName.TabName
                    inputName.Dispose()
                    Me.TopMost = SettingDialog.AlwaysTop
                    If tabName.Length > 0 Then
                        If Not AddNewTab(tabName, False) Then
                            Dim tmp As String = String.Format(My.Resources.TabMenuItem_ClickText2, tabName)
                            MessageBox.Show(tmp, My.Resources.TabMenuItem_ClickText3, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Else
                            _statuses.AddTab(tabName)
                            SaveConfigs()
                            Exit Do
                        End If
                    End If
                Else
                    Exit Do
                End If
            Loop While True
            fDialog.SetCurrent(tabName)
            fDialog.AddNewFilter(_statuses.Item(_curTab.Text, idx).Name, _statuses.Item(_curTab.Text, idx).Data)
            fDialog.ShowDialog()
            Me.TopMost = SettingDialog.AlwaysTop
        Next

        Try
            Me.Cursor = Cursors.WaitCursor
            _itemCache = Nothing
            _postCache = Nothing
            _curPost = Nothing
            _curItemIndex = -1
            _statuses.FilterAll()
            For Each tb As TabPage In ListTab.TabPages
                DirectCast(tb.Controls(0), DetailsListView).VirtualListSize = _statuses.Tabs(tb.Text).AllCount
                If _statuses.Tabs(tb.Text).UnreadCount > 0 Then
                    tb.ImageIndex = 0
                Else
                    tb.ImageIndex = -1
                End If
            Next
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Protected Overrides Function ProcessDialogKey( _
        ByVal keyData As Keys) As Boolean
        'TextBox1でEnterを押してもビープ音が鳴らないようにする
        If StatusText.Focused AndAlso _
            (keyData And Keys.KeyCode) = Keys.Enter Then
            '改行
            If StatusText.Multiline AndAlso _
               ((keyData And Keys.Shift) = Keys.Shift OrElse _
               ((keyData And Keys.Control) = Keys.Control AndAlso Not SettingDialog.PostCtrlEnter) OrElse _
               ((keyData And Keys.Shift) <> Keys.Shift AndAlso (keyData And Keys.Control) <> Keys.Control AndAlso SettingDialog.PostCtrlEnter)) Then
                Dim pos1 As Integer = StatusText.SelectionStart
                If StatusText.SelectionLength > 0 Then
                    StatusText.Text = StatusText.Text.Remove(pos1, StatusText.SelectionLength)  '選択状態文字列削除
                End If
                StatusText.Text = StatusText.Text.Insert(pos1, Environment.NewLine)  '改行挿入
                StatusText.SelectionStart = pos1 + Environment.NewLine.Length    'カーソルを改行の次の文字へ移動
                Return True
            End If
            '投稿
            If ((keyData And Keys.Control) = Keys.Control AndAlso SettingDialog.PostCtrlEnter) OrElse _
               ((keyData And Keys.Control) <> Keys.Control AndAlso Not SettingDialog.PostCtrlEnter) Then
                PostButton_Click(Nothing, Nothing)
                Return True
            End If
        End If
        Return MyBase.ProcessDialogKey(keyData)
    End Function

    Private Sub InfoTwitterMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InfoTwitterMenuItem.Click
        If Twitter.InfoTwitter.Trim() = "" Then
            MessageBox.Show(My.Resources.InfoTwitterMenuItem_ClickText1, My.Resources.InfoTwitterMenuItem_ClickText2, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            Dim inf As String = Twitter.InfoTwitter.Trim()
            inf = "<html><head></head><body>" + inf + "</body></html>"
            PostBrowser.Visible = False
            PostBrowser.DocumentText = inf
            PostBrowser.Visible = True
        End If
    End Sub

    Private Sub ReplyAllStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReplyAllStripMenuItem.Click
        MakeReplyOrDirectStatus(False, True, True)
    End Sub

    Private Sub IDRuleMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IDRuleMenuItem.Click
        Dim tabName As String = ""
        Do
            '振り分け先タブ選択
            If TabDialog.ShowDialog = Windows.Forms.DialogResult.Cancel Then
                Me.TopMost = SettingDialog.AlwaysTop
                Exit Sub
            End If
            Me.TopMost = SettingDialog.AlwaysTop
            tabName = TabDialog.SelectedTabName

            ListTab.SelectedTab.Focus()
            '新規タブを選択→タブ作成
            If tabName = My.Resources.IDRuleMenuItem_ClickText1 Then
                Dim inputName As New InputTabName()
                inputName.TabName = "MyTab" + ListTab.TabPages.Count.ToString
                inputName.ShowDialog()
                tabName = inputName.TabName
                inputName.Dispose()
                Me.TopMost = SettingDialog.AlwaysTop
                If tabName <> "" Then
                    If Not AddNewTab(tabName, False) Then
                        Dim tmp As String = String.Format(My.Resources.IDRuleMenuItem_ClickText2, tabName)
                        MessageBox.Show(tmp, My.Resources.IDRuleMenuItem_ClickText3, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Else
                        _statuses.AddTab(tabName)
                        SaveConfigs()
                        Exit Do
                    End If
                End If
            Else
                '既存タブを選択
                Exit Do
            End If
        Loop While True
        Dim mv As Boolean = False
        With Block
            '移動するか？
            Dim _tmp As String = String.Format(My.Resources.IDRuleMenuItem_ClickText4, Environment.NewLine)
            If MessageBox.Show(_tmp, My.Resources.IDRuleMenuItem_ClickText5, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                mv = False
            Else
                mv = True
            End If
        End With
        Dim mk As Boolean = False
        If Not mv Then
            'マークするか？
            Dim _tmp As String = String.Format(My.Resources.IDRuleMenuItem_ClickText6, vbCrLf)
            If MessageBox.Show(_tmp, My.Resources.IDRuleMenuItem_ClickText7, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                mk = True
            Else
                mk = False
            End If
        End If
        Dim ids As New List(Of String)
        For Each idx As Integer In _curList.SelectedIndices
            Dim post As PostClass = _statuses.Item(_curTab.Text, idx)
            If Not ids.Contains(post.Name) Then
                Dim fc As New FiltersClass
                ids.Add(post.Name)
                fc.NameFilter = post.Name
                fc.SearchBoth = True
                fc.MoveFrom = mv
                fc.SetMark = mk
                fc.UseRegex = False
                fc.SearchUrl = False
                _statuses.Tabs(tabName).AddFilter(fc)
            End If
        Next

        Try
            Me.Cursor = Cursors.WaitCursor
            _itemCache = Nothing
            _postCache = Nothing
            _curPost = Nothing
            _curItemIndex = -1
            _statuses.FilterAll()
            For Each tb As TabPage In ListTab.TabPages
                DirectCast(tb.Controls(0), DetailsListView).VirtualListSize = _statuses.Tabs(tb.Text).AllCount
                If _statuses.Tabs(tb.Text).UnreadCount > 0 Then
                    tb.ImageIndex = 0
                Else
                    tb.ImageIndex = -1
                End If
            Next
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub CopySTOTMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopySTOTMenuItem.Click
        Me.CopyStot()
    End Sub

    Private Sub CopyURLMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyURLMenuItem.Click
        Me.CopyIdUri()
    End Sub

    Private Sub SelectAllMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectAllMenuItem.Click
        If StatusText.Focused Then
            StatusText.SelectAll()
        Else
            For i As Integer = _curList.SelectedIndices.Count - 1 To 0 Step -1
                _curList.Items(i).Selected = False
            Next
            _curList.SelectedIndices.Clear()
            For i As Integer = 0 To _curList.VirtualListSize - 1
                _curList.Items(i).Selected = True
            Next
        End If
    End Sub

    Private Sub MoveMiddle()
        Dim _item As ListViewItem
        Dim idx1 As Integer
        Dim idx2 As Integer

        If _curList.SelectedIndices.Count = 0 Then Exit Sub

        Dim idx As Integer = _curList.SelectedIndices(0)

        _item = _curList.GetItemAt(0, 25)
        If _item Is Nothing Then
            idx1 = 0
        Else
            idx1 = _item.Index
        End If
        _item = _curList.GetItemAt(0, _curList.ClientSize.Height - 1)
        If _item Is Nothing Then
            idx2 = _curList.VirtualListSize - 1
        Else
            idx2 = _item.Index
        End If

        idx -= Math.Abs(idx1 - idx2) \ 2
        If idx < 0 Then idx = 0

        _curList.EnsureVisible(_curList.VirtualListSize - 1)
        _curList.EnsureVisible(idx)
    End Sub

    Private Sub WedataMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WedataMenuItem.Click
        Twitter.GetWedata()
    End Sub

    Private Sub OpenURLMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenURLMenuItem.Click
        If PostBrowser.Document.Links.Count > 0 Then
            UrlDialog.ClearUrl()

            Dim openUrlStr As String = ""

            If PostBrowser.Document.Links.Count = 1 Then
                openUrlStr = urlEncodeMultibyteChar(PostBrowser.Document.Links(0).GetAttribute("href"))
            Else
                For Each linkElm As System.Windows.Forms.HtmlElement In PostBrowser.Document.Links
                    UrlDialog.AddUrl(urlEncodeMultibyteChar(linkElm.GetAttribute("href")))
                Next
                If UrlDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    openUrlStr = UrlDialog.SelectedUrl
                End If
                Me.TopMost = SettingDialog.AlwaysTop
            End If

            If openUrlStr <> "" Then OpenUriAsync(openUrlStr)
        End If
    End Sub

    Private Sub ClearTabMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearTabMenuItem.Click
        If _rclickTabName = "" Then Exit Sub
        Dim tmp As String = String.Format(My.Resources.ClearTabMenuItem_ClickText1, Environment.NewLine)
        If MessageBox.Show(tmp, My.Resources.ClearTabMenuItem_ClickText2, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        _statuses.ClearTabIds(_rclickTabName)
        If ListTab.SelectedTab.Text = _rclickTabName Then
            _anchorPost = Nothing
            _anchorFlag = False
            _itemCache = Nothing
            _postCache = Nothing
            _itemCacheIndex = -1
            _curItemIndex = -1
            _curPost = Nothing
        End If
        For Each tb As TabPage In ListTab.TabPages
            If tb.Text = _rclickTabName Then
                tb.ImageIndex = -1
                DirectCast(tb.Controls(0), DetailsListView).VirtualListSize = 0
                Exit For
            End If
        Next

        SetMainWindowTitle()
        SetStatusLabel()
    End Sub

    Private Sub SetMainWindowTitle()
        'メインウインドウタイトルの書き換え
        Dim ttl As New StringBuilder(256)
        Dim ur As Integer = 0
        Dim al As Integer = 0
        Static myVer As String = My.Application.Info.Version.ToString()
        If SettingDialog.DispLatestPost <> DispTitleEnum.None AndAlso _
           SettingDialog.DispLatestPost <> DispTitleEnum.Post AndAlso _
           SettingDialog.DispLatestPost <> DispTitleEnum.Ver Then
            For Each key As String In _statuses.Tabs.Keys
                ur += _statuses.Tabs(key).UnreadCount
                al += _statuses.Tabs(key).AllCount
            Next
        End If

        If SettingDialog.DispUsername Then ttl.Append(_username).Append(" - ")
        ttl.Append("Tween  ")
        Select Case SettingDialog.DispLatestPost
            Case DispTitleEnum.Ver
                ttl.Append("Ver:").Append(myVer)
            Case DispTitleEnum.Post
                If _history IsNot Nothing AndAlso _history.Count > 1 Then
                    ttl.Append(_history(_history.Count - 2))
                End If
            Case DispTitleEnum.UnreadRepCount
                ttl.AppendFormat(My.Resources.SetMainWindowTitleText1, _statuses.Tabs("Reply").UnreadCount + _statuses.Tabs("Direct").UnreadCount)
            Case DispTitleEnum.UnreadAllCount
                ttl.AppendFormat(My.Resources.SetMainWindowTitleText2, ur)
            Case DispTitleEnum.UnreadAllRepCount
                ttl.AppendFormat(My.Resources.SetMainWindowTitleText3, ur, _statuses.Tabs("Reply").UnreadCount + _statuses.Tabs("Direct").UnreadCount)
            Case DispTitleEnum.UnreadCountAllCount
                ttl.AppendFormat(My.Resources.SetMainWindowTitleText4, ur, al)
        End Select

        Me.Text = ttl.ToString()
    End Sub

    Private Sub SetStatusLabel()
        'ステータス欄にカウント表示
        'タブ未読数/タブ発言数 全未読数/総発言数 (未読＠＋未読DM数)
        Dim urat As Integer = _statuses.Tabs("Reply").UnreadCount + _statuses.Tabs("Direct").UnreadCount
        Dim ur As Integer = 0
        Dim al As Integer = 0
        Dim tur As Integer = 0
        Dim tal As Integer = 0
        Dim slbl As StringBuilder = New StringBuilder(256)
        For Each key As String In _statuses.Tabs.Keys
            ur += _statuses.Tabs(key).UnreadCount
            al += _statuses.Tabs(key).AllCount
            If key.Equals(_curTab.Text) Then
                tur = _statuses.Tabs(key).UnreadCount
                tal = _statuses.Tabs(key).AllCount
            End If
        Next

        slbl.AppendFormat(My.Resources.SetStatusLabelText1, tur, tal, ur, al, urat, _postTimestamps.Count, _favTimestamps.Count, _tlCount)
        If SettingDialog.TimelinePeriodInt = 0 Then
            slbl.Append(My.Resources.SetStatusLabelText2)
        Else
            slbl.Append((TimerTimeline.Interval / 1000).ToString() + My.Resources.SetStatusLabelText3)
        End If
        StatusLabelUrl.Text = slbl.ToString()
    End Sub

    Private Sub SetNotifyIconText()
        ' タスクトレイアイコンのツールチップテキスト書き換え
        If SettingDialog.DispUsername Then
            NotifyIcon1.Text = _username + " - Tween"
        Else
            NotifyIcon1.Text = "Tween"
        End If
    End Sub

    Friend Sub CheckReplyTo(ByVal StatusText As String)
        ' 本当にリプライ先指定すべきかどうかの判定
        Dim id As New Regex("@[a-zA-Z0-9_]+")
        Dim m As MatchCollection

        If _reply_to_id = 0 Then Exit Sub

        If _reply_to_name Is Nothing Then
            _reply_to_id = 0
            Exit Sub
        End If

        m = id.Matches(StatusText)

        If m IsNot Nothing AndAlso m.Count = 1 AndAlso m.Item(0).Value = "@" + _reply_to_name AndAlso Not StatusText.StartsWith(". ") Then
            Exit Sub
        End If

        _reply_to_id = 0
        _reply_to_name = Nothing

    End Sub

    Private Sub TweenMain_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        If SettingDialog.MinimizeToTray AndAlso WindowState = FormWindowState.Minimized Then
            Me.Visible = False
        End If
    End Sub

    Private Sub PlaySoundMenuItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlaySoundMenuItem.CheckedChanged
        If PlaySoundMenuItem.Checked Then
            SettingDialog.PlaySound = True
        Else
            SettingDialog.PlaySound = False
        End If
    End Sub

    Private Sub SplitContainer1_SplitterMoved(ByVal sender As Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles SplitContainer1.SplitterMoved
        If Me.WindowState = FormWindowState.Normal Then
            _mySpDis = SplitContainer1.SplitterDistance
            If StatusText.Multiline Then _mySpDis2 = StatusText.Height
        End If
    End Sub

    Private Sub RepliedStatusOpenMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RepliedStatusOpenMenuItem.Click
        If _curPost IsNot Nothing AndAlso _curPost.InReplyToUser IsNot Nothing AndAlso _curPost.InReplyToId > 0 Then
            OpenUriAsync("http://twitter.com/" + _curPost.InReplyToUser + "/statuses/" + _curPost.InReplyToId.ToString())
        End If
    End Sub

    Private Sub ContextMenuStrip3_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip3.Opening
        '発言詳細のアイコン右クリック時のメニュー制御
        If _curList.SelectedIndices.Count > 0 AndAlso _curPost IsNot Nothing Then
            Dim name As String = _curPost.ImageUrl
            If name.Length > 0 Then
                name = IO.Path.GetFileNameWithoutExtension(name.Substring(name.LastIndexOf("/"c)))
                name = name.Substring(0, name.Length - 7) ' "_normal".Length
                Me.IconNameToolStripMenuItem.Enabled = True
                If Me.TIconDic.ContainsKey(_curPost.ImageUrl) AndAlso Me.TIconDic(_curPost.ImageUrl) IsNot Nothing Then
                    Me.SaveIconPictureToolStripMenuItem.Enabled = True
                Else
                    Me.SaveIconPictureToolStripMenuItem.Enabled = False
                End If
                Me.IconNameToolStripMenuItem.Text = name
            Else
                Me.IconNameToolStripMenuItem.Enabled = False
                Me.SaveIconPictureToolStripMenuItem.Enabled = False
                Me.IconNameToolStripMenuItem.Text = My.Resources.ContextMenuStrip3_OpeningText1
            End If
        Else
            Me.IconNameToolStripMenuItem.Enabled = False
            Me.SaveIconPictureToolStripMenuItem.Enabled = False
            Me.IconNameToolStripMenuItem.Text = My.Resources.ContextMenuStrip3_OpeningText2
        End If
    End Sub

    Private Sub IconNameToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IconNameToolStripMenuItem.Click
        If _curPost Is Nothing Then Exit Sub
        Dim name As String = _curPost.ImageUrl
        OpenUriAsync(name.Remove(name.LastIndexOf("_normal"), 7)) ' "_normal".Length
    End Sub

    Private Sub SaveOriginalSizeIconPictureToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If _curPost Is Nothing Then Exit Sub
        Dim name As String = _curPost.ImageUrl
        name = IO.Path.GetFileNameWithoutExtension(name.Substring(name.LastIndexOf("/"c)))

        Me.SaveFileDialog1.FileName = name.Substring(0, name.Length - 8) ' "_normal".Length + 1

        If Me.SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            ' STUB
        End If
    End Sub

    Private Sub SaveIconPictureToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveIconPictureToolStripMenuItem.Click
        If _curPost Is Nothing Then Exit Sub
        Dim name As String = _curPost.ImageUrl

        Me.SaveFileDialog1.FileName = name.Substring(name.LastIndexOf("/"c) + 1)

        If Me.SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Me.TIconDic(name).Save(Me.SaveFileDialog1.FileName)
        End If
    End Sub

    Private Sub SplitContainer2_Panel2_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitContainer2.Panel2.Resize
        Me.StatusText.Multiline = Me.SplitContainer2.Panel2.Height > Me.SplitContainer2.Panel2MinSize + 2
        MultiLineMenuItem.Checked = Me.StatusText.Multiline
    End Sub

    Private Sub StatusText_MultilineChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusText.MultilineChanged
        If Me.StatusText.Multiline Then
            Me.StatusText.ScrollBars = ScrollBars.Vertical
        Else
            Me.StatusText.ScrollBars = ScrollBars.None
        End If
    End Sub

    Private Sub MultiLineMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MultiLineMenuItem.Click
        '発言欄複数行
        StatusText.Multiline = MultiLineMenuItem.Checked
        _section.StatusMultiline = MultiLineMenuItem.Checked
        If MultiLineMenuItem.Checked Then
            If SplitContainer2.Height - _mySpDis2 - SplitContainer2.SplitterWidth < 0 Then
                SplitContainer2.SplitterDistance = 0
            Else
                SplitContainer2.SplitterDistance = SplitContainer2.Height - _mySpDis2 - SplitContainer2.SplitterWidth
            End If
        Else
            SplitContainer2.SplitterDistance = SplitContainer2.Height - SplitContainer2.Panel2MinSize - SplitContainer2.SplitterWidth
        End If
    End Sub

    Private Function UrlConvert(ByVal Converter_Type As UrlConverter) As Boolean
        Dim result As String = ""
        Dim url As Regex = New Regex("(?<![0-9A-Za-z])(?:https?|shttp)://(?:(?:[-_.!~*'()a-zA-Z0-9;:&=+$,]|%[0-9A-Fa-f" + _
                                     "][0-9A-Fa-f])*@)?(?:(?:[a-zA-Z0-9](?:[-a-zA-Z0-9]*[a-zA-Z0-9])?\.)" + _
                                     "*[a-zA-Z](?:[-a-zA-Z0-9]*[a-zA-Z0-9])?\.?|[0-9]+\.[0-9]+\.[0-9]+\." + _
                                     "[0-9]+)(?::[0-9]*)?(?:/(?:[-_.!~*'()a-zA-Z0-9:@&=+$,]|%[0-9A-Fa-f]" + _
                                     "[0-9A-Fa-f])*(?:;(?:[-_.!~*'()a-zA-Z0-9:@&=+$,]|%[0-9A-Fa-f][0-9A-" + _
                                     "Fa-f])*)*(?:/(?:[-_.!~*'()a-zA-Z0-9:@&=+$,]|%[0-9A-Fa-f][0-9A-Fa-f" + _
                                     "])*(?:;(?:[-_.!~*'()a-zA-Z0-9:@&=+$,]|%[0-9A-Fa-f][0-9A-Fa-f])*)*)" + _
                                     "*)?(?:\?(?:[-_.!~*'()a-zA-Z0-9;/?:@&=+$,]|%[0-9A-Fa-f][0-9A-Fa-f])" + _
                                     "*)?(?:#(?:[-_.!~*'()a-zA-Z0-9;/?:@&=+$,]|%[0-9A-Fa-f][0-9A-Fa-f])*)?")


        If StatusText.SelectionLength > 0 Then
            Dim tmp As String = StatusText.SelectedText
            ' httpから始まらない場合、ExcludeStringで指定された文字列で始まる場合は対象としない
            If tmp.StartsWith("http") Then
                ' 文字列が選択されている場合はその文字列について処理

                '短縮URL変換 日本語を含むかもしれないのでURLエンコードする
                result = Twitter.MakeShortUrl(Converter_Type, StatusText.SelectedText)

                If result.Equals("Can't convert") Then
                    Return False
                End If

                If Not result = "" Then
                    Dim undotmp As New urlUndo

                    StatusText.Select(StatusText.Text.IndexOf(tmp, StringComparison.Ordinal), tmp.Length)
                    StatusText.SelectedText = result

                    'undoバッファにセット
                    undotmp.Before = tmp
                    undotmp.After = result

                    If urlUndoBuffer Is Nothing Then
                        urlUndoBuffer = New List(Of urlUndo)
                        UrlUndoToolStripMenuItem.Enabled = True
                    End If

                    urlUndoBuffer.Add(undotmp)
                End If
            End If
        Else
            Dim urls As RegularExpressions.MatchCollection = Nothing
            urls = url.Matches(StatusText.Text)

            ' 正規表現にマッチしたURL文字列をtinyurl化
            For Each tmp2 As Match In urls
                Dim tmp As String = tmp2.ToString
                Dim undotmp As New urlUndo

                '選んだURLを選択（？）
                StatusText.Select(StatusText.Text.IndexOf(tmp, StringComparison.Ordinal), tmp.Length)

                '短縮URL変換
                result = Twitter.MakeShortUrl(Converter_Type, StatusText.SelectedText)

                If result.Equals("Can't convert") Then
                    Return False
                End If

                If Not result = "" Then
                    StatusText.Select(StatusText.Text.IndexOf(tmp, StringComparison.Ordinal), tmp.Length)
                    StatusText.SelectedText = result
                    'undoバッファにセット
                    undotmp.Before = tmp
                    undotmp.After = result

                    If urlUndoBuffer Is Nothing Then
                        urlUndoBuffer = New List(Of urlUndo)
                        UrlUndoToolStripMenuItem.Enabled = True
                    End If

                    urlUndoBuffer.Add(undotmp)
                End If
            Next
        End If

        Return True

    End Function

    Private Sub doUrlUndo()
        If urlUndoBuffer IsNot Nothing Then
            Dim tmp As String = StatusText.Text
            For Each data As urlUndo In urlUndoBuffer
                tmp = tmp.Replace(data.After, data.Before)
            Next
            StatusText.Text = tmp
            urlUndoBuffer = Nothing
            UrlUndoToolStripMenuItem.Enabled = False
        End If
    End Sub

    Private Sub TinyURLToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TinyURLToolStripMenuItem.Click
        UrlConvert(UrlConverter.TinyUrl)
    End Sub

    Private Sub IsgdToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IsgdToolStripMenuItem.Click
        UrlConvert(UrlConverter.Isgd)
    End Sub

    Private Sub UrlConvertAutoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UrlConvertAutoToolStripMenuItem.Click
        If Not UrlConvert(UrlConverter.TinyUrl) Then
            UrlConvert(UrlConverter.Isgd)
        End If
    End Sub

    Private Sub UrlUndoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UrlUndoToolStripMenuItem.Click
        doUrlUndo()
    End Sub

    Private Sub NewPostPopMenuItem_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles NewPostPopMenuItem.CheckStateChanged
        _section.NewAllPop = NewPostPopMenuItem.Checked
    End Sub

    Private Sub ListLockMenuItem_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListLockMenuItem.CheckStateChanged
        _section.ListLock = ListLockMenuItem.Checked
    End Sub

    Private Sub MenuStrip1_MenuActivate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuStrip1.MenuActivate
        ' フォーカスがメニューに移る (MenuStrip1.Tag フラグを立てる)
        MenuStrip1.Tag = New Object()
        MenuStrip1.Select() ' StatusText がフォーカスを持っている場合 Leave が発生
    End Sub

    Private Sub MenuStrip1_MenuDeactivate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuStrip1.MenuDeactivate
        If Me.Tag IsNot Nothing Then ' 設定された戻り先へ遷移
            DirectCast(Me.Tag, Control).Select()
        Else ' 戻り先が指定されていない (初期状態) 場合はタブに遷移
            Me.Tag = ListTab.SelectedTab.Controls(0)
            DirectCast(Me.Tag, Control).Select()
        End If
        ' フォーカスがメニューに遷移したかどうかを表すフラグを降ろす
        MenuStrip1.Tag = Nothing
    End Sub

    Private Sub MyList_ColumnReordered(ByVal sender As System.Object, ByVal e As ColumnReorderedEventArgs)
        Dim lst As DetailsListView = DirectCast(sender, DetailsListView)
        'Dim darr(lst.Columns.Count - 1) As Integer
        'For i As Integer = 0 To lst.Columns.Count - 1
        '    darr(lst.Columns(i).DisplayIndex) = i
        'Next
        'MoveArrayItem(darr, e.OldDisplayIndex, e.NewDisplayIndex)

        If _section Is Nothing Then Exit Sub

        If _iconCol Then
            _section.Width1 = lst.Columns(0).Width
            _section.Width3 = lst.Columns(1).Width
            _section.DisplayIndex1 = lst.Columns(0).DisplayIndex
            _section.DisplayIndex3 = lst.Columns(1).DisplayIndex
        Else
            _section.DisplayIndex1 = lst.Columns(0).DisplayIndex
            _section.DisplayIndex2 = lst.Columns(1).DisplayIndex
            _section.DisplayIndex3 = lst.Columns(2).DisplayIndex
            _section.DisplayIndex4 = lst.Columns(3).DisplayIndex
            _section.DisplayIndex5 = lst.Columns(4).DisplayIndex
            _section.DisplayIndex6 = lst.Columns(5).DisplayIndex
            _section.DisplayIndex7 = lst.Columns(6).DisplayIndex
            _section.DisplayIndex8 = lst.Columns(7).DisplayIndex
            _section.Width1 = lst.Columns(0).Width
            _section.Width2 = lst.Columns(1).Width
            _section.Width3 = lst.Columns(2).Width
            _section.Width4 = lst.Columns(3).Width
            _section.Width5 = lst.Columns(4).Width
            _section.Width6 = lst.Columns(5).Width
            _section.Width7 = lst.Columns(6).Width
            _section.Width8 = lst.Columns(7).Width
        End If

    End Sub

    Private Sub MyList_ColumnWidthChanged(ByVal sender As System.Object, ByVal e As ColumnWidthChangedEventArgs)
        Dim lst As DetailsListView = DirectCast(sender, DetailsListView)
        If _section Is Nothing Then Exit Sub
        If _iconCol Then
            _section.Width1 = lst.Columns(0).Width
            _section.Width3 = lst.Columns(1).Width
        Else
            _section.Width1 = lst.Columns(0).Width
            _section.Width2 = lst.Columns(1).Width
            _section.Width3 = lst.Columns(2).Width
            _section.Width4 = lst.Columns(3).Width
            _section.Width5 = lst.Columns(4).Width
            _section.Width6 = lst.Columns(5).Width
            _section.Width7 = lst.Columns(6).Width
            _section.Width8 = lst.Columns(7).Width
        End If
    End Sub

    Private Sub ToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem3.Click
        '発言詳細で「選択文字列をコピー」
        PostBrowser.Document.ExecCommand("Copy", False, Nothing)
    End Sub

    Private Sub doSearchToolStrip(ByVal url As String)
        '発言詳細で「選択文字列で検索」（選択文字列取得）
        Dim typ As Type = PostBrowser.ActiveXInstance.GetType()
        Dim _SelObj As Object = typ.InvokeMember("selection", BindingFlags.GetProperty, Nothing, PostBrowser.Document.DomDocument, Nothing)
        Dim _objRange As Object = _SelObj.GetType().InvokeMember("createRange", BindingFlags.InvokeMethod, Nothing, _SelObj, Nothing)
        Dim _selText As String = DirectCast(_objRange.GetType().InvokeMember("text", BindingFlags.GetProperty, Nothing, _objRange, Nothing), String)
        Dim tmp As String

        If _selText IsNot Nothing Then
            tmp = String.Format(url, HttpUtility.UrlEncode(_selText))
            OpenUriAsync(tmp)
        End If
    End Sub

    Private Sub ToolStripMenuItem5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem5.Click
        '発言詳細ですべて選択
        PostBrowser.Document.ExecCommand("SelectAll", False, Nothing)
    End Sub

    Private Sub SearchItem1ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchItem1ToolStripMenuItem.Click
        doSearchToolStrip(My.Resources.SearchItem1Url)
    End Sub

    Private Sub SearchItem2ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchItem2ToolStripMenuItem.Click
        doSearchToolStrip(My.Resources.SearchItem2Url)
    End Sub

    Private Sub SearchItem3ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchItem3ToolStripMenuItem.Click
        doSearchToolStrip(My.Resources.SearchItem3Url)
    End Sub

    Private Sub SearchItem4ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchItem4ToolStripMenuItem.Click
        doSearchToolStrip(My.Resources.SearchItem4Url)
    End Sub

    Private Sub ToolStripMenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem4.Click
        'URLをコピー
        'If PostBrowser.StatusText.StartsWith("http") Then   '念のため
        Clipboard.SetDataObject(PostBrowser.StatusText, False, 5, 100)
        'End If
    End Sub

    Private Sub ContextMenuStrip4_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip4.Opening
        ' URLコピーの項目の表示/非表示
        If PostBrowser.StatusText.StartsWith("http") Then
            ToolStripMenuItem4.Enabled = True
        Else
            ToolStripMenuItem4.Enabled = False
        End If
        e.Cancel = False
    End Sub

    Private Sub CurrentTabToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CurrentTabToolStripMenuItem.Click
        '発言詳細の選択文字列で現在のタブを検索
        Dim typ As Type = PostBrowser.ActiveXInstance.GetType()
        Dim _SelObj As Object = typ.InvokeMember("selection", BindingFlags.GetProperty, Nothing, PostBrowser.Document.DomDocument, Nothing)
        Dim _objRange As Object = _SelObj.GetType().InvokeMember("createRange", BindingFlags.InvokeMethod, Nothing, _SelObj, Nothing)
        Dim _selText As String = DirectCast(_objRange.GetType().InvokeMember("text", BindingFlags.GetProperty, Nothing, _objRange, Nothing), String)

        If _selText IsNot Nothing Then
            SearchDialog.SWord = _selText
            SearchDialog.CheckCaseSensitive = False
            SearchDialog.CheckRegex = False

            DoTabSearch(SearchDialog.SWord, _
                        SearchDialog.CheckCaseSensitive, _
                        SearchDialog.CheckRegex, _
                        SEARCHTYPE.NextSearch)
        End If
    End Sub

    Private Sub SplitContainer2_SplitterMoved(ByVal sender As Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles SplitContainer2.SplitterMoved
        If StatusText.Multiline Then _mySpDis2 = StatusText.Height
    End Sub

    Private Sub TweenMain_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles MyBase.DragDrop
        Dim data As String = TryCast(e.Data.GetData(DataFormats.StringFormat, True), String)
        If data IsNot Nothing Then
            StatusText.Text += data
        End If
    End Sub

    Private Sub TweenMain_DragOver(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles MyBase.DragOver
        Dim data As String = TryCast(e.Data.GetData(DataFormats.StringFormat, True), String)
        If data IsNot Nothing Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    ' Contributed by shuyoko <http://twitter.com/shuyoko> BEGIN:
    Private Sub BlackFavAddToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BlackFavAddToolStripMenuItem.Click

        Dim cnt As Integer = 0
        'Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)

        If _curTab.Text = "Direct" OrElse _curList.SelectedIndices.Count = 0 Then Exit Sub

        If _curList.SelectedIndices.Count > 1 Then
            If MessageBox.Show(My.Resources.BlackFavAddToolStripMenuItem_ClickText1, My.Resources.BlackFavAddToolStripMenuItem_ClickText2, _
                               MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Cancel Then
                Exit Sub
            End If
        End If

        Dim args As New GetWorkerArg()
        args.ids = New List(Of Long)
        args.sIds = New List(Of Long)
        args.tName = _curTab.Text
        For Each idx As Integer In _curList.SelectedIndices
            If Not _statuses.Item(_curTab.Text, idx).IsFav Then
                args.ids.Add(_statuses.Item(_curTab.Text, idx).Id)
            End If
        Next
        args.type = WORKERTYPE.BlackFavAdd
        If args.ids.Count = 0 Then
            StatusLabel.Text = My.Resources.BlackFavAddToolStripMenuItem_ClickText4
            Exit Sub
        End If

        RunAsync(args)
    End Sub
    ' Contributed by shuyoko <http://twitter.com/shuyoko> END.

    Private Sub BlackFavRemoveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BlackFavRemoveToolStripMenuItem.Click
        ' STUB
    End Sub

    Private Function IsNetworkAvailable() As Boolean
        Dim nw As Boolean = True
        Try
            nw = My.Computer.Network.IsAvailable
        Catch ex As Exception
            nw = True
        End Try
        Return nw
    End Function

    Private Sub OpenUriAsync(ByVal UriString As String)
        Dim args As New GetWorkerArg
        args.type = WORKERTYPE.OpenUri
        args.status = UriString

        RunAsync(args)
    End Sub

    Private Sub ListTabSelect(ByVal _tab As TabPage)
        SetListProperty()

        _itemCache = Nothing
        _itemCacheIndex = -1
        _postCache = Nothing
        _curTab = _tab
        _curList = DirectCast(_tab.Controls(0), DetailsListView)
        If _curList.SelectedIndices.Count > 0 Then
            _curItemIndex = _curList.SelectedIndices(0)
            _curPost = GetCurTabPost(_curItemIndex)
        Else
            _curItemIndex = -1
            _curPost = Nothing
        End If

        _anchorPost = Nothing
        _anchorFlag = False
    End Sub

    Private Sub ListTab_Selecting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TabControlCancelEventArgs) Handles ListTab.Selecting
        ListTabSelect(e.TabPage)
    End Sub

    Private Sub SelectListItem(ByVal LView As DetailsListView, ByVal Index As Integer)
        '単一
        Dim bnd As Rectangle
        Dim flg As Boolean = False
        If LView.FocusedItem IsNot Nothing Then
            bnd = LView.FocusedItem.Bounds
            flg = True
        End If
        'For i As Integer = _curList.SelectedIndices.Count - 1 To 0 Step -1
        '    LView.Items(i).Selected = False
        'Next
        LView.SelectedIndices.Clear()
        LView.Items(Index).Selected = True
        LView.Items(Index).Focused = True
        If flg Then LView.Invalidate(bnd)
    End Sub

    Private Sub SelectListItem(ByVal LView As DetailsListView, ByVal Index() As Integer, ByVal FocusedIndex As Integer)
        '複数
        Dim bnd As Rectangle
        Dim flg As Boolean = False
        If LView.FocusedItem IsNot Nothing Then
            bnd = LView.FocusedItem.Bounds
            flg = True
        End If
        'For i As Integer = LView.SelectedIndices.Count - 1 To 0 Step -1
        '    LView.Items(LView.SelectedIndices(i)).Selected = False
        'Next

        If Index IsNot Nothing Then
            If Index(0) > -1 Then
                LView.SelectedIndices.Clear()
                For Each idx As Integer In Index
                    'LView.Items(idx).Selected = True
                    LView.SelectedIndices.Add(idx)
                Next
            End If
        End If
        If FocusedIndex > -1 Then
            LView.Items(FocusedIndex).Focused = True
        End If
        If flg Then LView.Invalidate(bnd)
    End Sub

    Private Sub RunAsync(ByVal args As GetWorkerArg)
        Dim bw As BackgroundWorker = Nothing
        For i As Integer = 0 To _bw.Length - 1
            If _bw(i) IsNot Nothing AndAlso Not _bw(i).IsBusy Then
                bw = _bw(i)
                Exit For
            End If
        Next
        If bw Is Nothing Then
            For i As Integer = 0 To _bw.Length - 1
                If _bw(i) Is Nothing Then
                    _bw(i) = New BackgroundWorker
                    bw = _bw(i)
                    bw.WorkerReportsProgress = True
                    bw.WorkerSupportsCancellation = True
                    AddHandler bw.DoWork, AddressOf GetTimelineWorker_DoWork
                    AddHandler bw.ProgressChanged, AddressOf GetTimelineWorker_ProgressChanged
                    AddHandler bw.RunWorkerCompleted, AddressOf GetTimelineWorker_RunWorkerCompleted
                    Exit For
                End If
            Next
        End If
        If bw Is Nothing Then Exit Sub

        bw.RunWorkerAsync(args)
    End Sub

    'Public Delegate Function GetImageIndexDelegate(ByVal post As PostClass) As Integer

    'Public Function GetImageIndex(ByVal post As PostClass) As Integer
    '    Return TIconSmallList.Images.IndexOfKey(post.ImageUrl)
    'End Function

    'Public Delegate Sub SetImageDelegate(ByVal post As PostClass, ByVal Img As Image, ByVal ImgBmp As Bitmap)

    'Public Sub SetImage(ByVal post As PostClass, ByVal Img As Image, ByVal ImgBmp As Bitmap)
    '    post.ImageIndex = GetImageIndex(post)
    '    If post.ImageIndex > -1 Then Exit Sub

    '    TIconDic.Add(post.ImageUrl, Img)  '詳細表示用ディクショナリに追加
    '    TIconSmallList.Images.Add(post.ImageUrl, ImgBmp)
    '    post.ImageIndex = TIconSmallList.Images.IndexOfKey(post.ImageUrl)
    'End Sub

    Private Sub TweenMain_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If IsNetworkAvailable() Then
            'If SettingDialog.StartupFollowers Then
            '    _waitFollower = True
            '    GetTimeline(WORKERTYPE.Follower, 0, 0)
            'End If
            If SettingDialog.ReadPages > 0 Then
                _waitTimeline = True
                GetTimeline(WORKERTYPE.Timeline, 1, SettingDialog.ReadPages)
            End If
            If SettingDialog.ReadPagesReply > 0 Then
                _waitReply = True
                GetTimeline(WORKERTYPE.Reply, 1, SettingDialog.ReadPagesReply)
            End If
            If SettingDialog.ReadPagesDM > 0 Then
                _waitDm = True
                GetTimeline(WORKERTYPE.DirectMessegeRcv, 1, SettingDialog.ReadPagesDM)
            End If
            'Do While _waitFollower AndAlso Not _endingFlag
            '    System.Threading.Thread.Sleep(1)
            '    My.Application.DoEvents()
            'Loop
            Dim i As Integer = 0
            Do While (_waitTimeline OrElse _waitReply OrElse _waitDm) AndAlso Not _endingFlag
                System.Threading.Thread.Sleep(100)
                My.Application.DoEvents()
                i += 1
                If i > 50 Then
                    _statuses.DistributePosts()
                    RefreshTimeline()
                    i = 0
                End If
            Loop

            _statuses.DistributePosts()
            RefreshTimeline()

            If _endingFlag Then Exit Sub
            'バージョンチェック（引数：起動時チェックの場合はTrue･･･チェック結果のメッセージを表示しない）
            If SettingDialog.StartupVersion Then
                CheckNewVersion(True)
            End If

        Else
            TimerRefreshIcon.Enabled = False
            NotifyIcon1.Icon = NIconAtSmoke
            PostButton.Enabled = False
            FavAddToolStripMenuItem.Enabled = False
            FavRemoveToolStripMenuItem.Enabled = False
            MoveToHomeToolStripMenuItem.Enabled = False
            MoveToFavToolStripMenuItem.Enabled = False
            DeleteStripMenuItem.Enabled = False
            RefreshStripMenuItem.Enabled = False
        End If
        _initial = False
    End Sub

    Private Sub doGetFollowersMenu(ByVal CacheInvalidate As Boolean)
        Try
            StatusLabel.Text = My.Resources.UpdateFollowersMenuItem1_ClickText1
            My.Application.DoEvents()
            Me.Cursor = Cursors.WaitCursor
            Dim ret As String
            ret = Twitter.GetFollowers(CacheInvalidate)
            If ret <> "" Then
                StatusLabel.Text = My.Resources.UpdateFollowersMenuItem1_ClickText2 & ret
                Exit Sub
            End If
            StatusLabel.Text = My.Resources.UpdateFollowersMenuItem1_ClickText3
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub GetFollowersDiffToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GetFollowersDiffToolStripMenuItem.Click
        doGetFollowersMenu(False)       ' Followersリストキャッシュ有効
    End Sub

    Private Sub GetFollowersAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GetFollowersAllToolStripMenuItem.Click
        doGetFollowersMenu(True)        ' Followersリストキャッシュ無効
    End Sub

End Class
