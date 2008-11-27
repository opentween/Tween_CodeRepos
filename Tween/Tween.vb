' Tween - Client of Twitter
' Copyright © 2007-2008 kiri_feather (@kiri_feather) <kiri_feather@gmail.com>
'           © 2008      Moz (@syo68k) <http://iddy.jp/profile/moz/>
'           © 2008      takeshik (@takeshik) <http://www.takeshik.org/>
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

Public Class TweenMain
    Private clsTw As Twitter            'Twitter用通信データ処理カスタムクラス
    Private clsTwPost As Twitter            'Twitter用通信データ処理カスタムクラス
    Private clsTwSync As Twitter            'Twitter用通信データ処理カスタムクラス
    Private _username As String         'ユーザー名
    Private _password As String         'パスワード（デクリプト済み）
    Private _mySize As Size             '画面サイズ
    Private _myLoc As Point             '画面位置
    Private _mySpDis As Integer         '区切り位置
    Private _initial As Boolean         'True:起動時処理中
    Private listViewItemSorter As ListViewItemComparer      'リストソート用カスタムクラス
    Private _config As Configuration    'アプリケーション構成ファイルクラス
    Private _section As ListSection     '構成ファイル中のユーザー定義ListSectionクラス
    Private SettingDialog As New Setting()       '設定画面インスタンス
    Private TabDialog As New TabsDialog()        'タブ選択ダイアログインスタンス
    Private SearchDialog As New SearchWord()     '検索画面インスタンス
    Private _tabs As New List(Of TabStructure)() '要素TabStructureクラスのジェネリックリストインスタンス（タブ情報用）
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
    Private _postCounter As Integer       '取得発言数カウンタ（カウントしているが未使用。タブ別カウンタに変更＆未読数カウントとして未読アイコン表示パフォーマンスUPできるように改善したい）
    Private TIconList As ImageList        '発言詳細部用アイコン画像リスト
    Private TIconSmallList As ImageList   'リスト表示用アイコン画像リスト
    Private _iconSz As Integer            'アイコンサイズ（現在は16、24、48の3種類。将来直接数字指定可能とする 注：24x24の場合に26と指定しているのはMSゴシック系フォントのための仕様）
    Private _iconCol As Boolean           '1列表示の時True（48サイズのとき）
    Private NIconAt As Icon               'At.ico             タスクトレイアイコン：通常時
    Private NIconAtRed As Icon            'AtRed.ico          タスクトレイアイコン：通信エラー時
    Private NIconAtSmoke As Icon          'AtSmoke.ico        タスクトレイアイコン：オフライン時
    Private NIconRefresh(3) As Icon       'Refresh.ico        タスクトレイアイコン：更新中（アニメーション用に4種類を保持するリスト）
    Private TabIcon As Icon               'Tab.ico            未読のあるタブ用アイコン
    Private MainIcon As Icon              'Main.ico           画面左上のアイコン
    Private _anchorItem As ListViewItem   '関連発言移動開始時のリストアイテム
    Private _anchorFlag As Boolean        'True:関連発言移動中（関連移動以外のオペレーションをするとFalseへ。Trueだとリスト背景色をアンカー発言選択中として描画）
    Private _tabDrag As Boolean           'タブドラッグ中フラグ（DoDragDropを実行するかの判定用）
    Private _refreshIconCnt As Integer    '更新中アイコンのアニメーション用カウンタ
    Private _rclickTabName As String      '右クリックしたタブの名前
    Private fDialog As New FilterDialog() 'フィルター編集画面
    Private _endingFlag As Boolean        '終了フラグ
    Private _curTabText As String = "Recent"
    Private _history As New List(Of String)()
    Private _hisIdx As Integer
    Private UrlDialog As New OpenURL()
    Private Const _replyHtml As String = "@<a target=""_self"" href=""https://twitter.com/"
    Private _reply_to_id As Integer     ' リプライ先のステータスID 0の場合はリプライではない 注：複数あてのものはリプライではない
    Private _reply_to_name As String    ' リプライ先ステータスの書き込み者の名前
    Private _getDM As Boolean
    Private _postTimestamps As New List(Of Date)()
    Private _favTimestamps As New List(Of Date)()
    Private _tlTimestamps As New Dictionary(Of Date, Integer)()
    Private _tlCount As Integer
    Private ReadOnly _syncObject As New Object()    'ロック用  
    Private _StatusSelectionStart As Integer        ' 一時退避用
    Private _StatusSelectionLength As Integer       ' 一時退避用

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
    Private sf As New StringFormat()
    Private _columnIdx As Integer   'ListviewのDisplayIndex退避用（DrawItemで使用）
    Private _columnChangeFlag As Boolean

#If DEBUG Then
    Private _drawcount As Long = 0
    Private _drawtime As Long = 0
#End If

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

    'Backgroundworkerへ処理種別を通知するための引数用Enum
    Private Enum WORKERTYPE
        Timeline                'タイムライン取得
        Reply                   '返信取得
        DirectMessegeRcv        '受信DM取得
        DirectMessegeSnt        '送信DM取得
        PostMessage             '発言POST
        FavAdd                  'Fav追加
        FavRemove               'Fav削除
        CreateNewSocket         'Socket再作成
    End Enum

    'Backgroundworkerの処理結果通知用引数構造体
    Private Structure GetWorkerResult
        Public retMsg As String                     '処理結果詳細メッセージ。エラー時に値がセットされる
        Public TLine As List(Of Twitter.MyListItem) '取得した発言。Twitter.MyListItem構造体を要素としたジェネリックリスト
        Public page As Integer                      '取得対象ページ番号
        Public endPage As Integer                   '取得終了ページ番号（継続可能ならインクリメントされて返る。pageと比較して継続判定）
        Public type As WORKERTYPE                   '処理種別
        Public imgs As ImageList                    '新規取得したアイコンイメージ
        Public tName As String                      'Fav追加・削除時のタブ名
        Public ids As List(Of String)               'Fav追加・削除時のID
        Public sIds As List(Of String)                  'Fav追加・削除成功分のID
    End Structure

    'Backgroundworkerへ処理内容を通知するための引数用構造体
    Private Structure GetWorkerArg
        Public page As Integer                      '処理対象ページ番号
        Public endPage As Integer                   '処理終了ページ番号（起動時の読み込みページ数。通常時はpageと同じ値をセット）
        Public type As WORKERTYPE                   '処理種別
        Public status As String                     '発言POST時の発言内容
        Public ids As List(Of String)               'Fav追加・削除時のID
        Public sIds As List(Of String)              'Fav追加・削除成功分のID
        Public tName As String                      'Fav追加・削除時のタブ名
    End Structure

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '着せ替えアイコン対応
        'タスクトレイ通常時アイコン
        Try
            NIconAt = New Icon(".\Icons\At.ico")
        Catch ex As Exception
            NIconAt = My.Resources.At
        End Try
        'タスクトレイエラー時アイコン
        Try
            NIconAtRed = New Icon(".\Icons\AtRed.ico")
        Catch ex As Exception
            NIconAtRed = My.Resources.AtRed
        End Try
        'タスクトレイオフライン時アイコン
        Try
            NIconAtSmoke = New Icon(".\Icons\AtSmoke.ico")
        Catch ex As Exception
            NIconAtSmoke = My.Resources.AtSmoke
        End Try
        'タスクトレイ更新中アイコン
        'アニメーション対応により4種類読み込み
        Try
            NIconRefresh(0) = New Icon(".\Icons\Refresh.ico")
        Catch ex As Exception
            NIconRefresh(0) = My.Resources.Refresh
        End Try
        Try
            NIconRefresh(1) = New Icon(".\Icons\Refresh2.ico")
        Catch ex As Exception
            NIconRefresh(1) = My.Resources.Refresh2
        End Try
        Try
            NIconRefresh(2) = New Icon(".\Icons\Refresh3.ico")
        Catch ex As Exception
            NIconRefresh(2) = My.Resources.Refresh3
        End Try
        Try
            NIconRefresh(3) = New Icon(".\Icons\Refresh4.ico")
        Catch ex As Exception
            NIconRefresh(3) = My.Resources.Refresh4
        End Try
        'タブ見出し未読表示アイコン
        Try
            TabIcon = New Icon(".\Icons\Tab.ico")
        Catch ex As Exception
            TabIcon = My.Resources.TabIcon
        End Try
        '画面のアイコン
        Try
            MainIcon = New Icon(".\Icons\MIcon.ico")
        Catch ex As Exception
            MainIcon = My.Resources.MIcon
        End Try

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

        _columnChangeFlag = True

        '<<<<<<<<<設定関連>>>>>>>>>
        '設定読み出し
        _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
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
        _brsBackColorNone = New SolidBrush(Color.White)

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
        '発言欄複数行
        MultiLineMenuItem.Checked = _section.StatusMultiline
        StatusText.Multiline = _section.StatusMultiline
        If StatusText.Multiline Then
            SplitContainer2.SplitterDistance = SplitContainer2.Height - _section.StatusTextHeight - SplitContainer2.SplitterWidth
        Else
            SplitContainer2.SplitterDistance = SplitContainer2.Height - SplitContainer2.Panel2MinSize - SplitContainer2.SplitterWidth
        End If
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

        _initial = True

        'ユーザー名、パスワードが未設定なら設定画面を表示（初回起動時など）
        If _username = "" Or _password = "" Then
            '設定せずにキャンセルされた場合はプログラム終了
            If SettingDialog.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
                Application.Exit()
                Exit Sub
            End If
            _username = SettingDialog.UserID
            _password = SettingDialog.PasswordStr
            '設定されたが、依然ユーザー名とパスワードが未設定ならプログラム終了
            If _username = "" Or _password = "" Then
                Application.Exit()
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
            '他の設定項目は、随時設定画面で保持している値を読み出して使用
        End If


        'ウィンドウ設定
        Me.WindowState = FormWindowState.Normal     '通常状態
        Me.ClientSize = _section.FormSize           'サイズ設定
        _mySize = Me.ClientSize                     'サイズ保持（最小化・最大化されたまま終了した場合の対応用）
        Me.Location = _section.FormLocation         '位置設定
        _myLoc = Me.Location                        '位置保持（最小化・最大化されたまま終了した場合の対応用）
        Me.SplitContainer1.SplitterDistance = _section.SplitterDistance     'Splitterの位置設定
        _mySpDis = Me.SplitContainer1.SplitterDistance
        Me.TopMost = SettingDialog.AlwaysTop

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

        '発言詳細部のアイコンリスト作成
        TIconList = New ImageList
        TIconList.ImageSize = New Size(48, 48)
        TIconList.ColorDepth = ColorDepth.Depth32Bit

        'Twitter用通信クラス初期化
        clsTw = New Twitter(_username, _password, SettingDialog.ProxyType, SettingDialog.ProxyAddress, SettingDialog.ProxyPort, SettingDialog.ProxyUser, SettingDialog.ProxyPassword)
        clsTwPost = New Twitter(_username, _password, SettingDialog.ProxyType, SettingDialog.ProxyAddress, SettingDialog.ProxyPort, SettingDialog.ProxyUser, SettingDialog.ProxyPassword)
        clsTwSync = New Twitter(_username, _password, SettingDialog.ProxyType, SettingDialog.ProxyAddress, SettingDialog.ProxyPort, SettingDialog.ProxyUser, SettingDialog.ProxyPassword)
        If SettingDialog.StartupKey Then
            clsTw.GetWedata()
        End If
        clsTw.NextThreshold = SettingDialog.NextPageThreshold   '次頁取得閾値
        clsTw.NextPages = SettingDialog.NextPagesInt    '閾値オーバー時の読み込みページ数（未使用）

        _iconCol = False
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
            clsTw.GetIcon = False
        Else
            clsTw.GetIcon = True
        End If
        clsTwPost.UseAPI = SettingDialog.UseAPI
        clsTw.HubServer = SettingDialog.HubServer
        clsTwPost.HubServer = SettingDialog.HubServer
        clsTwSync.HubServer = SettingDialog.HubServer
        clsTw.TinyUrlResolve = SettingDialog.TinyUrlResolve

        '発言詳細部アイコンをリストアイコンにサイズ変更
        ChangeImageSize()

        StatusLabel.Text = "更新中..."      '画面右下の状態表示を変更
        StatusLabelUrl.Text = ""            '画面左下のリンク先URL表示部を初期化
        'PostedText.Text = ""
        PostBrowser.DocumentText = ""       '発言詳細部初期化
        'PostedText.RemoveLinks()
        NameLabel.Text = ""                 '発言詳細部名前ラベル初期化
        DateTimeLabel.Text = ""             '発言詳細部日時ラベル初期化

        '<<<<<<<<タブ関連>>>>>>>
        'Recentタブ
        'Timeline.SmallImageList = TIconSmallList
        listViewItemSorter = New ListViewItemComparer
        listViewItemSorter.ColumnModes = New ListViewItemComparer.ComparerMode() _
                {ListViewItemComparer.ComparerMode.None, _
                 ListViewItemComparer.ComparerMode.String, _
                 ListViewItemComparer.ComparerMode.String, _
                 ListViewItemComparer.ComparerMode.DateTime, _
                 ListViewItemComparer.ComparerMode.String}
        listViewItemSorter.Column = _section.SortColumn
        listViewItemSorter.Order = DirectCast(_section.SortOrder, System.Windows.Forms.SortOrder)

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
            Dim myTab As New TabStructure()
            myTab.tabPage = New TabPage()
            myTab.listCustom = New DetailsListView()
            myTab.colHd1 = New ColumnHeader()
            If Not _iconCol Then
                myTab.colHd2 = New ColumnHeader()
                myTab.colHd3 = New ColumnHeader()
                myTab.colHd4 = New ColumnHeader()
                myTab.colHd5 = New ColumnHeader()
            End If
            myTab.notify = _section.ListElement(idx).Notify
            myTab.unreadManage = _section.ListElement(idx).UnreadManage
            myTab.soundFile = _section.ListElement(idx).SoundFile
            For Each flt As Tween.SelectedUser In _section.SelectedUser
                If flt.TabName = name Then
                    Dim fcls As New FilterClass()
                    Dim bflt() As String = flt.BodyFilter.Split(Chr(32))
                    For Each tmpFlt As String In bflt
                        If tmpFlt.Trim <> "" Then fcls.BodyFilter.Add(tmpFlt.Trim)
                    Next
                    fcls.IDFilter = flt.IdFilter
                    fcls.SearchBoth = flt.SearchBoth
                    fcls.SearchURL = flt.UrlSearch
                    fcls.UseRegex = flt.RegexEnable
                    fcls.moveFrom = flt.MoveFrom
                    fcls.SetMark = flt.SetMark
                    myTab.filters.Add(fcls)
                End If
            Next
            myTab.tabName = name
            _tabs.Add(myTab)
        Next

        AddCustomTabs()

        'バージョンチェック（引数：起動時チェックの場合はTrue･･･チェック結果のメッセージを表示しない）
        If SettingDialog.StartupVersion Then
            CheckNewVersion(True)
        End If

        If My.Computer.Network.IsAvailable Then
            NotifyIcon1.Icon = NIconRefresh(0)
            _refreshIconCnt = 0
            TimerRefreshIcon.Enabled = True
            Dim args As New GetWorkerArg()
            args.page = 1
            args.endPage = 1
            args.type = WORKERTYPE.DirectMessegeRcv
            GetTimelineWorker.RunWorkerAsync(args)
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

        SetMainWindowTitle()
        SetNotifyIconText()

        AddHandler My.Computer.Network.NetworkAvailabilityChanged, AddressOf Network_NetworkAvailabilityChanged
    End Sub

    Private Sub Network_NetworkAvailabilityChanged(ByVal sender As Object, ByVal e As Devices.NetworkAvailableEventArgs)
        If e.IsNetworkAvailable Then
            Dim args As New GetWorkerArg()
            args.type = WORKERTYPE.CreateNewSocket
            Do While GetTimelineWorker.IsBusy
                Threading.Thread.Sleep(1)
                Application.DoEvents()
            Loop
            GetTimelineWorker.RunWorkerAsync(args)
            Do While PostWorker.IsBusy
                Threading.Thread.Sleep(1)
                Application.DoEvents()
            Loop
            PostWorker.RunWorkerAsync(args)
            clsTwSync.CreateNewSocket()
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
                NotifyIcon1.Icon = NIconRefresh(0)
                _refreshIconCnt = 0
                TimerRefreshIcon.Enabled = True
                args.page = 1
                args.endPage = 1
                args.type = WORKERTYPE.DirectMessegeRcv
                Do While GetTimelineWorker.IsBusy
                    Threading.Thread.Sleep(1)
                    Application.DoEvents()
                Loop
                GetTimelineWorker.RunWorkerAsync(args)
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
        If Not My.Computer.Network.IsAvailable Then Exit Sub
        Dim args As New GetWorkerArg()
        args.page = 1
        args.endPage = 1
        args.type = WORKERTYPE.Timeline
        StatusLabel.Text = "Recent更新中..."
        NotifyIcon1.Icon = NIconRefresh(0)
        _refreshIconCnt = 0
        TimerRefreshIcon.Enabled = True
        Do While GetTimelineWorker.IsBusy
            Threading.Thread.Sleep(1)
            Application.DoEvents()
        Loop
        GetTimelineWorker.RunWorkerAsync(args)
    End Sub

    Private Sub TimerDM_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerDM.Tick
        If My.Computer.Network.IsAvailable = False Then Exit Sub
        GC.Collect()
        Dim args As New GetWorkerArg()
        args.page = 1
        args.endPage = 1
        args.type = WORKERTYPE.DirectMessegeRcv
        StatusLabel.Text = "DMRcv更新中..."
        NotifyIcon1.Icon = NIconRefresh(0)
        _refreshIconCnt = 0
        TimerRefreshIcon.Enabled = True
        Do While GetTimelineWorker.IsBusy
            Threading.Thread.Sleep(1)
            Application.DoEvents()
        Loop
        GetTimelineWorker.RunWorkerAsync(args)
    End Sub

    Private Sub RefreshTimeline(ByVal tlList As List(Of Twitter.MyListItem), Optional ByVal OldItems As Boolean = False)
        Dim lItem As Twitter.MyListItem
        Dim cnt As Integer = 0
        Dim RepCnt As Integer = 0
        Dim _pop As String = ""
        Dim topItem As ListViewItem
        Dim firstDate As String = ""
        Dim endDate As String = ""
        Dim readed As Boolean = SettingDialog.Readed
        Dim myList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        Dim _readed As Boolean
        Dim _fav As Boolean
        Dim _onewaylove As Boolean
        Dim nm As String
        Dim snd As String = ""
        Dim Protect As String = ""
        Dim ImgTag As New Regex("<img src=.*?/>", RegexOptions.IgnoreCase)   'ハロウィン

        TimerColorize.Stop()

        Dim _item As ListViewItem

        If myList.Items.Count > 0 Then
            If ListLockMenuItem.Checked Then
                topItem = myList.TopItem
            Else
                If listViewItemSorter.Column = 3 Then
                    If listViewItemSorter.Order = SortOrder.Ascending Then
                        '日時昇順
                        _item = myList.GetItemAt(0, myList.ClientSize.Height - 1)
                        If _item Is Nothing Then _item = myList.Items(myList.Items.Count - 1)
                        If _item.Index = myList.Items.Count - 1 Then
                            topItem = Nothing
                        Else
                            topItem = myList.TopItem
                        End If
                    Else
                        '日時降順
                        _item = myList.GetItemAt(0, 25)
                        If _item Is Nothing Then _item = myList.Items(0)
                        If _item.Index = 0 Then
                            topItem = Nothing
                        Else
                            topItem = myList.TopItem
                        End If
                    End If
                Else
                    topItem = myList.TopItem
                End If

            End If
        Else
            topItem = Nothing
        End If

        myList.BeginUpdate()

        If tlList.Count > 0 Then
            firstDate = tlList(0).PDate.ToString("yy-MM-dd HH:mm:ss")
        End If

        For cnt = 0 To tlList.Count - 1
            '_reply = False
            _onewaylove = False
            _fav = False
            lItem = tlList(cnt)
            If follower.Count > 1 Then
                If follower.Contains(lItem.Name) = False Then
                    _onewaylove = True
                End If
            End If
            If lItem.Fav Then
                _fav = True
            End If
            _readed = True
            If lItem.Protect = True Then
                Protect = "Ю"
            Else
                Protect = ""
            End If
            ' Imageタグ除去（ハロウィン）
            If ImgTag.IsMatch(lItem.Data) Then
                lItem.Data = ImgTag.Replace(lItem.Data, "<img>")
            End If
            Dim sItem() As String = {Protect, _
                                     lItem.Nick, _
                                     lItem.Data, _
                                     lItem.PDate.ToString("yy-MM-dd HH:mm:ss"), _
                                     lItem.Name, _
                                     lItem.Id, _
                                     lItem.ImageUrl, _
                                     lItem.OrgData, _
                                     _readed.ToString, _
                                     _fav.ToString, _
                                     _onewaylove.ToString, _
                                     lItem.Reply.ToString}
            Dim lvItem As New ListViewItem(sItem)
            lvItem.ToolTipText = lItem.Data
            If (Not _initial OrElse (_initial AndAlso Not readed)) AndAlso SettingDialog.UnreadManage Then
                _readed = False
            End If
            If _onewaylove AndAlso SettingDialog.OneWayLove Then
                lvItem.ForeColor = _clOWL
            End If
            If _fav Then
                lvItem.ForeColor = _clFav
            End If
            lvItem.ImageKey = lItem.ImageUrl

            Dim mv As Boolean = False
            Dim nf As Boolean = False
            Dim mk As Boolean = False
            For Each ts As TabStructure In _tabs
                Dim hit As Boolean = False
                For Each ft As FilterClass In ts.filters
                    Dim bHit As Boolean = True
                    Dim tBody As String
                    If ft.SearchURL Then
                        tBody = lItem.OrgData
                    Else
                        tBody = lItem.Data
                    End If
                    If ft.SearchBoth Then
                        If ft.IDFilter = "" OrElse lItem.Name.Equals(ft.IDFilter, StringComparison.CurrentCultureIgnoreCase) Then
                            For Each fs As String In ft.BodyFilter
                                If ft.UseRegex Then
                                    If Not Regex.IsMatch(tBody, fs, RegexOptions.IgnoreCase) Then bHit = False
                                Else
                                    If Not tBody.ToLower.Contains(fs.ToLower) Then bHit = False
                                End If
                                If Not bHit Then Exit For
                            Next
                        Else
                            bHit = False
                        End If
                    Else
                        For Each fs As String In ft.BodyFilter
                            If ft.UseRegex Then
                                If Not Regex.IsMatch(lItem.Name + tBody, fs, RegexOptions.IgnoreCase) Then bHit = False
                            Else
                                If Not (lItem.Name + tBody).ToLower.Contains(fs.ToLower) Then bHit = False
                            End If
                            If Not bHit Then Exit For
                        Next
                    End If
                    If bHit Then
                        hit = True
                        If ft.SetMark Then mk = True
                        If ft.moveFrom Then mv = True
                    End If
                    If hit AndAlso mv AndAlso mk Then Exit For
                Next
                If hit Then
                    ts.allCount += 1
                    Dim lvItem2 As ListViewItem = DirectCast(lvItem.Clone, System.Windows.Forms.ListViewItem)
                    If Not _readed AndAlso ts.unreadManage Then
                        lvItem2.Font = _fntUnread
                        lvItem2.ForeColor = _clUnread
                        lvItem2.SubItems(8).Text = "False"
                        ts.unreadCount += 1
                    Else
                        lvItem2.Font = _fntReaded
                        lvItem2.ForeColor = _clReaded
                        lvItem2.SubItems(8).Text = "True"
                    End If
                    If _onewaylove AndAlso SettingDialog.OneWayLove Then
                        lvItem2.ForeColor = _clOWL
                    End If
                    If _fav Then
                        lvItem2.ForeColor = _clFav
                    End If

                    If lvItem2.SubItems(8).Text = "False" Then
                        If ts.oldestUnreadItem IsNot Nothing Then
                            If lvItem2.SubItems(5).Text < ts.oldestUnreadItem.SubItems(5).Text Then ts.oldestUnreadItem = lvItem2
                        Else
                            ts.oldestUnreadItem = lvItem2
                        End If
                    End If
                    ts.listCustom.Items.Add(lvItem2)
                    If ts.notify Then nf = True
                    If snd = "" Then snd = ts.soundFile
                End If
            Next

            If lItem.Reply OrElse Regex.IsMatch(lItem.Data, "@" + _username + "([^a-zA-Z0-9_]|$)", RegexOptions.IgnoreCase) Then
                Dim lvItem2 As ListViewItem = DirectCast(lvItem.Clone, System.Windows.Forms.ListViewItem)
                _tabs(1).allCount += 1
                If Not _readed AndAlso _tabs(1).unreadManage Then
                    lvItem2.Font = _fntUnread
                    lvItem2.ForeColor = _clUnread
                    lvItem2.SubItems(8).Text = "False"
                    _tabs(1).unreadCount += 1
                Else
                    lvItem2.Font = _fntReaded
                    lvItem2.ForeColor = _clReaded
                    lvItem2.SubItems(8).Text = "True"
                End If
                If _onewaylove AndAlso SettingDialog.OneWayLove Then
                    lvItem2.ForeColor = _clOWL
                End If
                If _fav Then
                    lvItem2.ForeColor = _clFav
                End If
                If lvItem2.SubItems(8).Text = "False" Then
                    If _tabs(1).oldestUnreadItem IsNot Nothing Then
                        If lvItem2.SubItems(5).Text < _tabs(1).oldestUnreadItem.SubItems(5).Text Then _tabs(1).oldestUnreadItem = lvItem2
                    Else
                        _tabs(1).oldestUnreadItem = lvItem2
                    End If
                End If
                _tabs(1).listCustom.Items.Add(lvItem2)
                If _tabs(1).notify Then nf = True
                If _tabs(1).soundFile <> "" Then snd = _tabs(1).soundFile '優先度高
                RepCnt += 1
            End If

            If Not mv Then
                _tabs(0).allCount += 1
                If Not _readed AndAlso _tabs(0).unreadManage Then
                    lvItem.Font = _fntUnread
                    lvItem.ForeColor = _clUnread
                    lvItem.SubItems(8).Text = "False"
                    _tabs(0).unreadCount += 1
                    If _tabs(0).oldestUnreadItem IsNot Nothing Then
                        If lvItem.SubItems(5).Text < _tabs(0).oldestUnreadItem.SubItems(5).Text Then _tabs(0).oldestUnreadItem = lvItem
                    Else
                        _tabs(0).oldestUnreadItem = lvItem
                    End If
                Else
                    lvItem.Font = _fntReaded
                    lvItem.ForeColor = _clReaded
                    lvItem.SubItems(8).Text = "True"
                End If
                If _onewaylove AndAlso SettingDialog.OneWayLove Then
                    lvItem.ForeColor = _clOWL
                End If
                If _fav Then
                    lvItem.ForeColor = _clFav
                End If
                If mk Then lvItem.SubItems(0).Text += "♪"
                _tabs(0).listCustom.Items.Add(lvItem)
                If _tabs(0).notify Then nf = True
                If snd = "" Then snd = _tabs(0).soundFile
            End If


            nm = ""
            Select Case SettingDialog.NameBalloon
                Case NameBalloonEnum.None
                    nm = ""
                Case NameBalloonEnum.UserID
                    nm = lItem.Name
                Case NameBalloonEnum.NickName
                    nm = lItem.Nick
            End Select
            Dim pmsg As String = nm + " : " + lItem.Data
            If NewPostPopMenuItem.Checked And nf Then
                If _pop = "" Then
                    _pop = pmsg
                Else
                    _pop += vbCrLf + pmsg
                End If
            End If

        Next

        endDate = lItem.PDate.ToString("yy-MM-dd HH:mm:ss")


        myList.EndUpdate()

        If cnt > 0 Then
            TimerColorize.Start()
            If topItem IsNot Nothing Then
                If myList.Items.Count > 0 AndAlso topItem.Index > -1 Then
                    myList.EnsureVisible(myList.Items.Count - 1)
                    myList.EnsureVisible(topItem.Index)
                End If
            Else

                If listViewItemSorter.Column = 3 AndAlso listViewItemSorter.Order = SortOrder.Ascending AndAlso myList.Items.Count > 0 Then
                    myList.EnsureVisible(myList.Items.Count - 1)
                End If
            End If
            '新着バルーン通知
            If Not _initial AndAlso _pop <> "" Then
                If RepCnt > 0 AndAlso _tabs(1).notify Then
                    NotifyIcon1.BalloonTipIcon = ToolTipIcon.Warning
                    NotifyIcon1.BalloonTipTitle = "Tween [Reply!] 新着 " + cnt.ToString() + "件"
                Else
                    NotifyIcon1.BalloonTipIcon = ToolTipIcon.Info
                    NotifyIcon1.BalloonTipTitle = "Tween 新着 " + cnt.ToString() + "件"
                End If
                NotifyIcon1.BalloonTipText = _pop
                NotifyIcon1.ShowBalloonTip(500)
            End If
            'サウンド再生
            If Not _initial AndAlso SettingDialog.PlaySound AndAlso snd <> "" Then
                Try
                    My.Computer.Audio.Play(My.Application.Info.DirectoryPath.ToString() + "\" + snd, AudioPlayMode.Background)
                Catch ex As Exception

                End Try
            End If
            '*** 暫定 ***
            If OldItems Then
                StatusLabel.Text = "ログ読込 [" + firstDate + "] ～ [" + endDate + "]"
            End If
            If _initial And Not OldItems Then
                StatusLabel.Text = "起動読込 [" + firstDate + "] ～ [" + endDate + "]"
            End If
        End If

        If SettingDialog.UnreadManage Then
            For Each ts As TabStructure In _tabs
                If ts.unreadManage AndAlso ts.unreadCount > 0 AndAlso ts.tabPage.ImageIndex = -1 Then
                    ts.tabPage.ImageIndex = 0
                End If
            Next
        End If

        SetMainWindowTitle()

        tlList.Clear()
    End Sub

    Private Sub Mylist_Scrolled(ByVal sender As Object, ByVal e As System.EventArgs)
        TimerColorize.Stop()
        TimerColorize.Start()
    End Sub

    Private Sub MyList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        TimerColorize.Stop()
        TimerColorize.Interval = 100

        Dim MyList As DetailsListView = DirectCast(sender, DetailsListView)
        MyList.Update()
        If MyList.SelectedItems.Count <> 1 Then Exit Sub

        Dim _item As ListViewItem = MyList.SelectedItems(0)
        If _item.SubItems(8).Text = "False" Then
            '最古未読ID再設定
            For Each ts As TabStructure In _tabs
                'タブ特定
                If ts.listCustom.Equals(MyList) Then
                    ItemReaded(ts, _item)
                    Exit For
                End If
            Next

        End If

        TimerColorize.Start()

    End Sub

    Private Sub ColorizeList(ByRef DispDetail As Boolean)
        Dim myList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        Dim name As String
        Dim at As New List(Of String)
        Dim dTxt As String

        Dim _item As ListViewItem = Nothing
        'Dim dTxt As String

        If myList.SelectedItems.Count > 0 OrElse _anchorFlag Then
            If _anchorFlag Then
                _item = _anchorItem
            Else
                _item = myList.SelectedItems(0)
            End If
            dTxt = "<html><head><style type=""text/css"">p {font-family: """ + _fntDetail.Name + """, sans-serif; font-size: " + _fntDetail.Size.ToString + "pt;} --></style></head><body style=""margin:0px""><p>" + _item.SubItems(7).Text + "</p></body></html>"
            If DispDetail Then
                TimerColorize.Start()
            End If

            Dim pos1 As Integer
            Dim pos2 As Integer

            name = _item.SubItems(4).Text
            Do While True
                pos1 = dTxt.IndexOf(_replyHtml, pos2)
                If pos1 = -1 Then Exit Do
                pos2 = dTxt.IndexOf(""">", pos1 + _replyHtml.Length)
                If pos2 > -1 Then
                    at.Add(dTxt.Substring(pos1 + _replyHtml.Length, pos2 - pos1 - _replyHtml.Length))
                End If
            Loop

        Else
            name = ""
        End If

        If _item Is Nothing Then
            at.Clear()
            Exit Sub
        End If

        'myList.BeginUpdate()
        Dim litem As ListViewItem = myList.GetItemAt(0, 0)
        Dim fromIndex As Integer
        If litem Is Nothing Then
            fromIndex = 0
        Else
            fromIndex = litem.Index
        End If
        Dim toIndex As Integer
        Dim litem2 As ListViewItem = myList.GetItemAt(0, myList.ClientSize.Height - 1)
        If litem2 Is Nothing Then
            toIndex = myList.Items.Count - 1
        Else
            toIndex = litem2.Index
        End If

        For cnt As Integer = fromIndex To toIndex
            Dim cl As Color
            If myList.Items(cnt).SubItems(4).Text.Equals(name, StringComparison.CurrentCultureIgnoreCase) Then
                '発言者
                If String.Equals(name, _username, StringComparison.CurrentCultureIgnoreCase) Then
                    '自分=発言者
                    cl = _clSelf
                Else
                    '発言者
                    cl = _clTarget
                End If
            Else
                'その他の人
                If Not myList.Items(cnt).SubItems(4).Text.Equals(_username, StringComparison.CurrentCultureIgnoreCase) Then
                    '自分以外
                    If myList.Items(cnt).SubItems(11).Text = "False" AndAlso Not Regex.IsMatch(myList.Items(cnt).SubItems(2).Text, "@" + _username + "([^a-zA-Z0-9_]|$)") Then
                        '通常発言
                        If at.Contains(myList.Items(cnt).SubItems(4).Text) Then
                            '返信先
                            cl = _clAtFromTarget
                        Else
                            If Not Regex.IsMatch(myList.Items(cnt).SubItems(2).Text, "@" + name + "([^a-zA-Z0-9_]|$)") OrElse name = "" Then
                                'その他
                                cl = System.Drawing.SystemColors.Window
                            Else
                                'その人への返信
                                cl = _clAtTarget
                            End If
                        End If
                    Else
                        '自分宛返信
                        cl = _clAtSelf
                    End If
                Else
                    '自分
                    cl = _clSelf
                End If
            End If

            If myList.Items(cnt).BackColor <> cl Then
                myList.ChangeItemBackColor(cnt, cl)
            End If

        Next
        at.Clear()

        'myList.EndUpdate()
    End Sub

    Private Sub PostButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PostButton.Click
        If StatusText.Text.Trim.Length = 0 Then Exit Sub

        _history(_history.Count - 1) = StatusText.Text.Trim

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

        Dim regex As New Regex("([^A-Za-z0-9@_:;\-]|^)(get|g|fav|follow|f|on|off|stop|quit|leave|l|whois|w|nudge|n|stats|invite|track|untrack|tracks|tracking|d)([^A-Za-z0-9_:\-]|$)", RegexOptions.IgnoreCase)
        Dim regex2 As New Regex("https?:\/\/[-_.!~*'()a-zA-Z0-9;\/?:\@&=+\$,%#]+")
        Dim regex3 As New Regex("^\S*\*[^A-Za-z0-9]")
        Dim regex4 As New Regex("\*\S*\s\S+\s")
        Dim mc As Match = regex.Match(args.status)
        Dim mc2 As Match = regex2.Match(args.status)
        If mc.Success Then
            If mc2.Success Then
                If mc.Index >= mc2.Index AndAlso mc.Index < mc2.Index + mc2.Length Then
                    'args.status.Insert(mc2.Index + mc2.Length, " ")
                    args.status = regex2.Replace(args.status, "$& ")
                Else
                    args.status = regex.Replace(args.status, "$1 $2 $3", 1)
                    args.status += " ."
                    mc2 = regex2.Match(args.status)
                    If mc2.Success Then
                        args.status = regex2.Replace(args.status, "$& ")
                    End If
                End If
            Else
                args.status = regex.Replace(args.status, "$1 $2 $3", 1)
                args.status += " ."
            End If
        Else
            If mc2.Success Then
                args.status = regex2.Replace(args.status, "$& ")
            End If
        End If
        If regex3.IsMatch(args.status) Then
            If Not regex4.IsMatch(args.status) Then
                If args.status.EndsWith(" .") Then
                    args.status += " ."
                Else
                    args.status += " . ."
                End If
            End If
        End If

        StatusLabel.Text = "Posting..."
        StatusText.Enabled = False
        PostButton.Enabled = False
        ReplyStripMenuItem.Enabled = False
        DMStripMenuItem.Enabled = False
        NotifyIcon1.Icon = NIconRefresh(0)
        _refreshIconCnt = 0
        TimerRefreshIcon.Enabled = True
        If SettingDialog.UseAPI Then
            Do While PostWorker.IsBusy
                Threading.Thread.Sleep(1)
                Application.DoEvents()
            Loop
            PostWorker.RunWorkerAsync(args)
        Else
            Do While GetTimelineWorker.IsBusy
                Threading.Thread.Sleep(1)
                Application.DoEvents()
            Loop
            GetTimelineWorker.RunWorkerAsync(args)
        End If

        ListTab.SelectedTab.Controls(0).Focus()
    End Sub

    Private Sub EndToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EndToolStripMenuItem.Click
        Application.Exit()
    End Sub

    Private Sub Tween_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If Not SettingDialog.CloseToExit AndAlso e.CloseReason = CloseReason.UserClosing Then
            e.Cancel = True
            Me.Visible = False

        Else
            _endingFlag = True
            GetTimelineWorker.CancelAsync()
            If clsTw IsNot Nothing Then clsTw.Ending = True
            If clsTwPost IsNot Nothing Then clsTwPost.Ending = True

            TimerTimeline.Enabled = False
            TimerDM.Enabled = False

            NotifyIcon1.Visible = False

            SaveConfigs()

            Me.Visible = False
        End If
    End Sub

    Private Sub NotifyIcon1_BalloonTipClicked(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NotifyIcon1.BalloonTipClicked
        Me.Visible = True
        If Me.WindowState = FormWindowState.Minimized Then
            Me.WindowState = FormWindowState.Normal
        End If
        Me.Activate()
    End Sub

    Private Sub GetTimelineWorker_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles GetTimelineWorker.DoWork
        If _endingFlag Then
            e.Cancel = True
            Exit Sub
        End If

        Dim ret As String = ""
        Dim tlList As New List(Of Twitter.MyListItem)()
        Dim rslt As New GetWorkerResult()
        Dim imgs As New ImageList()
        Dim getDM As Boolean = False
        imgs.ImageSize = New Size(48, 48)
        imgs.ColorDepth = ColorDepth.Depth32Bit

        Dim args As GetWorkerArg = DirectCast(e.Argument, GetWorkerArg)
        '        Try
        If args.type = WORKERTYPE.PostMessage Then CheckReplyTo(args.status)
        For i As Integer = 0 To 1
            Select Case args.type
                Case WORKERTYPE.Timeline
                    ret = clsTw.GetTimeline(tlList, args.page, _initial, args.endPage, Twitter.GetTypes.GET_TIMELINE, TIconList.Images.Keys, imgs, getDM)
                Case WORKERTYPE.Reply
                    ret = clsTw.GetTimeline(tlList, args.page, _initial, args.endPage, Twitter.GetTypes.GET_REPLY, TIconList.Images.Keys, imgs, getDM)
                Case WORKERTYPE.DirectMessegeRcv
                    ret = clsTw.GetDirectMessage(tlList, args.page, args.endPage, Twitter.GetTypes.GET_DMRCV, TIconList.Images.Keys, imgs)
                    If _initial AndAlso SettingDialog.StartupFollowers Then
                        ret = clsTw.GetFollowers()
                    End If
                Case WORKERTYPE.DirectMessegeSnt
                    ret = clsTw.GetDirectMessage(tlList, args.page, args.endPage, Twitter.GetTypes.GET_DMSNT, TIconList.Images.Keys, imgs)
                Case WORKERTYPE.PostMessage
                    ret = clsTw.PostStatus(args.status, _reply_to_id)
                Case WORKERTYPE.FavAdd
                    ret = clsTw.PostFavAdd(args.ids(args.page))
                    If ret = "" Then
                        _favTimestamps.Add(Now)
                    End If
                    Dim oneHour As Date = Now.Subtract(New TimeSpan(1, 0, 0))
                    For _i As Integer = _favTimestamps.Count - 1 To 0 Step -1
                        If _favTimestamps(_i).CompareTo(oneHour) < 0 Then
                            _favTimestamps.RemoveAt(_i)
                        End If
                    Next
                Case WORKERTYPE.FavRemove
                    ret = clsTw.PostFavRemove(args.ids(args.page))
                Case WORKERTYPE.CreateNewSocket
                    clsTw.CreateNewSocket()
            End Select
            If args.type = WORKERTYPE.PostMessage Then
                _reply_to_id = 0
                _reply_to_name = Nothing
            End If
            If ret = "" OrElse (ret <> "" AndAlso (args.type = WORKERTYPE.PostMessage OrElse args.type = WORKERTYPE.FavAdd OrElse args.type = WORKERTYPE.FavRemove)) Then Exit For
            Threading.Thread.Sleep(500)
        Next

        If args.type = WORKERTYPE.FavAdd OrElse args.type = WORKERTYPE.FavRemove Then
            rslt.ids = args.ids
            rslt.sIds = args.sIds
            If ret = "" Then rslt.sIds.Add(args.ids(args.page))
            args.page += 1
        End If
        rslt.retMsg = ret
        rslt.TLine = tlList
        rslt.page = args.page
        rslt.endPage = args.endPage
        rslt.type = args.type
        rslt.imgs = imgs
        rslt.tName = args.tName
        If getDM Then _getDM = True

        If _endingFlag Then
            e.Cancel = True
            Exit Sub
        End If

        e.Result = rslt
        'Catch ex As Exception
        '    If _endingFlag Then
        '        e.Cancel = True
        '        Exit Sub
        '    End If
        '    My.Application.Log.DefaultFileLogWriter.Location = Logging.LogFileLocation.ExecutableDirectory
        '    My.Application.Log.DefaultFileLogWriter.MaxFileSize = 102400
        '    My.Application.Log.DefaultFileLogWriter.AutoFlush = True
        '    My.Application.Log.DefaultFileLogWriter.Append = False
        '    'My.Application.Log.WriteException(ex, _
        '    '    Diagnostics.TraceEventType.Critical, _
        '    '    "Source=" + ex.Source + " StackTrace=" + ex.StackTrace + " InnerException=" + IIf(ex.InnerException Is Nothing, "", ex.InnerException.Message))
        '    My.Application.Log.WriteException(ex, _
        '        Diagnostics.TraceEventType.Critical, _
        '        ex.StackTrace + vbCrLf + Now.ToString + vbCrLf + args.type.ToString + vbCrLf + clsTw.savePost)
        '    rslt.retMsg = "Tween 例外発生(GetTimelineWorker_DoWork)"
        '    rslt.TLine = tlList
        '    rslt.page = args.page
        '    rslt.endPage = args.endPage
        '    rslt.type = args.type

        '    e.Result = rslt
        'End Try
    End Sub

    Private Function dmy() As Boolean
        Return False
    End Function

    Private Sub GetTimelineWorker_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles GetTimelineWorker.RunWorkerCompleted

        If e.Error IsNot Nothing Then
            If My.Computer.Network.IsAvailable Then
                NotifyIcon1.Icon = NIconAtRed
            End If
            Throw e.Error
            Exit Sub
        End If

        If _endingFlag OrElse e.Cancelled Then
            Exit Sub
        End If

        TimerRefreshIcon.Enabled = False
        If My.Computer.Network.IsAvailable Then
            NotifyIcon1.Icon = NIconAt
        Else
            NotifyIcon1.Icon = NIconAtSmoke
        End If

        Dim rslt As GetWorkerResult = DirectCast(e.Result, GetWorkerResult)
        Dim args As New GetWorkerArg()


        If rslt.retMsg <> "" Then
            '''''エラー通知方法の変更も設定できるように！
            If My.Computer.Network.IsAvailable Then
                NotifyIcon1.Icon = NIconAtRed
            End If
            'NotifyIcon1.BalloonTipIcon = ToolTipIcon.Warning
            'Select Case rslt.type
            '    Case WORKERTYPE.Timeline
            '        NotifyIcon1.BalloonTipTitle = "Tween エラー(Recent)"
            '    Case WORKERTYPE.Reply
            '        NotifyIcon1.BalloonTipTitle = "Tween エラー(Replies)"
            '    Case WORKERTYPE.DirectMessege
            '        NotifyIcon1.BalloonTipTitle = "Tween エラー(DirectMessage)"
            'End Select
            'NotifyIcon1.BalloonTipText = StatusLabel.Text + IIf(_initial, vbCrLf + "初期読み込みを中断します。", "")
            'NotifyIcon1.ShowBalloonTip(500)
            'If rslt.retMsg.StartsWith("Tween 例外発生") Then
            '    MessageBox.Show("エラーが発生しました。申し訳ありません。ログをexeファイルのある場所にTween.logとして作ったので、kiri.feather@gmail.comまで送っていただけると助かります。ご面倒なら@kiri_featherまでお知らせ頂くだけでも助かります。", "エラー発生", MessageBoxButtons.OK, MessageBoxIcon.Error)
            'End If
        End If

        If _iconSz <> 0 Then
            For i As Integer = 0 To rslt.imgs.Images.Count - 1
                'Dim img2 As New Bitmap(_iconSz, _iconSz)
                'Dim g As Graphics = Graphics.FromImage(img2)
                'g.InterpolationMode = Drawing2D.InterpolationMode.Default
                'g.DrawImage(rslt.imgs.Images(rslt.imgs.Images.Keys(i)), 0, 0, _iconSz, _iconSz)
                'TIconSmallList.Images.Add(rslt.imgs.Images.Keys(i), img2)
                Dim strKey As String = rslt.imgs.Images.Keys(i)
                Try
                    TIconSmallList.Images.Add(strKey, rslt.imgs.Images(strKey).GetThumbnailImage(_iconSz, _iconSz, New Image.GetThumbnailImageAbort(AddressOf dmy), IntPtr.Zero))
                    TIconList.Images.Add(strKey, DirectCast(rslt.imgs.Images(strKey).Clone, System.Drawing.Image))
                Finally
                    rslt.imgs.Images(strKey).Dispose()
                End Try
            Next
            rslt.imgs.Images.Clear()
            rslt.imgs.Dispose()
        End If

        Select Case rslt.type
            Case WORKERTYPE.CreateNewSocket
                Exit Sub
            Case WORKERTYPE.Timeline
                StatusLabel.Text = "Recent更新完了"
                Dim statusCount As Integer = rslt.TLine.Count
                If rslt.TLine.Count > 0 Then
                    RefreshTimeline(rslt.TLine)
                End If
                TimerTimeline.Enabled = False
                If rslt.retMsg <> "" Then
                    If My.Computer.Network.IsAvailable Then
                        If SettingDialog.TimelinePeriodInt > 0 Then TimerTimeline.Enabled = True
                    End If
                    StatusLabel.Text = rslt.retMsg
                Else
                    If Not My.Computer.Network.IsAvailable Then Exit Sub
                    If SettingDialog.TimelinePeriodInt > 0 AndAlso _
                       Not SettingDialog.CheckReply Then
                        TimerTimeline.Enabled = True
                    End If
                    If _initial Then
                        _getDM = False
                        If rslt.page + 1 <= rslt.endPage AndAlso SettingDialog.ReadPages >= rslt.page + 1 Then
                            If rslt.page Mod 10 = 0 Then
                                Dim flashRslt As Integer = Win32Api.FlashWindow(Me.Handle.ToInt32, 1)
                                If MessageBox.Show((rslt.page * 20).ToString + " ポストまで読み込み完了。さらに読み込みますか？", _
                                                   "読み込み継続確認", _
                                                   MessageBoxButtons.YesNo, _
                                                   MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                                    If SettingDialog.CheckReply Then
                                        args.page = 1
                                        args.endPage = 1
                                        args.type = WORKERTYPE.Reply
                                        StatusLabel.Text = "Reply更新中..."
                                        NotifyIcon1.Icon = NIconRefresh(0)
                                        _refreshIconCnt = 0
                                        TimerRefreshIcon.Enabled = True
                                        Do While GetTimelineWorker.IsBusy
                                            Threading.Thread.Sleep(1)
                                            Application.DoEvents()
                                        Loop
                                        GetTimelineWorker.RunWorkerAsync(args)
                                        Exit Sub
                                    Else
                                        _initial = False
                                    End If
                                End If
                            End If
                            args.page = rslt.page + 1
                            args.endPage = rslt.endPage
                            args.type = WORKERTYPE.Timeline
                            'Do While GetDMWorker.IsBusy Or GetLogWorker.IsBusy
                            NotifyIcon1.Icon = NIconRefresh(0)
                            _refreshIconCnt = 0
                            TimerRefreshIcon.Enabled = True
                            Do While GetTimelineWorker.IsBusy
                                Threading.Thread.Sleep(1)
                                Application.DoEvents()
                            Loop
                            GetTimelineWorker.RunWorkerAsync(args)
                            Exit Sub
                        End If
                        '_initial = False
                        If SettingDialog.CheckReply Then
                            args.page = 1
                            args.endPage = 1
                            args.type = WORKERTYPE.Reply
                            StatusLabel.Text = "Reply更新中..."
                            NotifyIcon1.Icon = NIconRefresh(0)
                            _refreshIconCnt = 0
                            TimerRefreshIcon.Enabled = True
                            Do While GetTimelineWorker.IsBusy
                                Threading.Thread.Sleep(1)
                                Application.DoEvents()
                            Loop
                            GetTimelineWorker.RunWorkerAsync(args)
                        Else
                            _initial = False
                        End If
                    Else
                        _tlTimestamps.Add(Now, statusCount)
                        Dim oneHour As Date = Now.Subtract(New TimeSpan(1, 0, 0))
                        Dim keys As New List(Of Date)()
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
                        If rslt.page + 1 <= rslt.endPage Then
                            If statusCount = 20 AndAlso rslt.page = 1 AndAlso SettingDialog.PeriodAdjust AndAlso SettingDialog.TimelinePeriodInt > 0 Then
                                Dim itv As Integer = TimerTimeline.Interval
                                itv -= 5000
                                If itv < 15000 Then itv = 15000
                                TimerTimeline.Interval = itv
                            End If
                            args.page = rslt.page + 1
                            args.endPage = rslt.endPage
                            args.type = WORKERTYPE.Timeline
                            StatusLabel.Text = "Recent更新中..." + args.page.ToString() + "pages"
                            NotifyIcon1.Icon = NIconRefresh(0)
                            _refreshIconCnt = 0
                            TimerRefreshIcon.Enabled = True
                            'Do While GetDMWorker.IsBusy Or GetLogWorker.IsBusy
                            Do While GetTimelineWorker.IsBusy
                                Threading.Thread.Sleep(1)
                                Application.DoEvents()
                            Loop
                            GetTimelineWorker.RunWorkerAsync(args)
                        Else
                            If rslt.page = 1 AndAlso statusCount < 17 AndAlso SettingDialog.PeriodAdjust AndAlso SettingDialog.TimelinePeriodInt > 0 Then
                                TimerTimeline.Interval += 1000
                                If TimerTimeline.Interval > SettingDialog.TimelinePeriodInt * 1000 Then TimerTimeline.Interval = SettingDialog.TimelinePeriodInt * 1000
                            End If
                            If SettingDialog.CheckReply Then
                                args.page = 1
                                args.endPage = 1
                                args.type = WORKERTYPE.Reply
                                StatusLabel.Text = "Reply更新中..."
                                NotifyIcon1.Icon = NIconRefresh(0)
                                _refreshIconCnt = 0
                                TimerRefreshIcon.Enabled = True
                                Do While GetTimelineWorker.IsBusy
                                    Threading.Thread.Sleep(1)
                                    Application.DoEvents()
                                Loop
                                GetTimelineWorker.RunWorkerAsync(args)
                            Else
                                If _getDM Then
                                    _getDM = False
                                    args.page = 1
                                    args.endPage = 1
                                    args.type = WORKERTYPE.DirectMessegeRcv
                                    StatusLabel.Text = "DMRcv更新中..."
                                    NotifyIcon1.Icon = NIconRefresh(0)
                                    _refreshIconCnt = 0
                                    TimerRefreshIcon.Enabled = True
                                    Do While GetTimelineWorker.IsBusy
                                        Threading.Thread.Sleep(1)
                                        Application.DoEvents()
                                    Loop
                                    GetTimelineWorker.RunWorkerAsync(args)
                                End If
                            End If
                        End If
                    End If
                End If
            Case WORKERTYPE.Reply
                StatusLabel.Text = "Reply更新完了"
                If rslt.TLine.Count > 0 Then
                    RefreshTimeline(rslt.TLine)
                End If
                TimerTimeline.Enabled = False
                If My.Computer.Network.IsAvailable Then
                    If SettingDialog.TimelinePeriodInt > 0 Then TimerTimeline.Enabled = True
                End If
                If rslt.retMsg <> "" Then
                    StatusLabel.Text = rslt.retMsg
                    _initial = False
                Else
                    If Not My.Computer.Network.IsAvailable Then Exit Sub
                    If _initial Then
                        _getDM = False
                        If rslt.page + 1 <= rslt.endPage AndAlso SettingDialog.ReadPagesReply >= rslt.page + 1 Then
                            If rslt.page Mod 10 = 0 Then
                                Dim flashRslt As Integer = Win32Api.FlashWindow(Me.Handle.ToInt32, 1)
                                If MessageBox.Show((rslt.page * 20).ToString + " ポストまで読み込み完了。さらに読み込みますか？", _
                                                   "読み込み継続確認", _
                                                   MessageBoxButtons.YesNo, _
                                                   MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                                    StatusLabel.Text = "起動時読込完了"
                                    Exit Sub
                                End If
                            End If
                            args.page = rslt.page + 1
                            args.endPage = rslt.endPage
                            args.type = WORKERTYPE.Reply
                            'Do While GetDMWorker.IsBusy Or GetLogWorker.IsBusy

                            NotifyIcon1.Icon = NIconRefresh(0)
                            _refreshIconCnt = 0
                            TimerRefreshIcon.Enabled = True
                            Do While GetTimelineWorker.IsBusy
                                Threading.Thread.Sleep(1)
                                Application.DoEvents()
                            Loop
                            GetTimelineWorker.RunWorkerAsync(args)
                            Exit Sub
                        End If
                        _initial = False
                    Else
                        If _getDM Then
                            _getDM = False
                            args.page = 1
                            args.endPage = 1
                            args.type = WORKERTYPE.DirectMessegeRcv
                            StatusLabel.Text = "DMRcv更新中..."
                            NotifyIcon1.Icon = NIconRefresh(0)
                            _refreshIconCnt = 0
                            TimerRefreshIcon.Enabled = True
                            Do While GetTimelineWorker.IsBusy
                                Threading.Thread.Sleep(1)
                                Application.DoEvents()
                            Loop
                            GetTimelineWorker.RunWorkerAsync(args)
                        End If
                    End If
                End If
            Case WORKERTYPE.DirectMessegeRcv
                StatusLabel.Text = "DMRcv更新完了"
                If rslt.TLine.Count > 0 Then
                    RefreshDirectMessage(rslt.TLine, True)
                End If
                TimerDM.Enabled = False
                If rslt.retMsg <> "" Then
                    StatusLabel.Text = rslt.retMsg
                    If _initial Then TimerDM.Interval = 30000
                    If My.Computer.Network.IsAvailable Then
                        If SettingDialog.DMPeriodInt > 0 OrElse _initial Then TimerDM.Enabled = True
                    End If
                    Exit Sub
                End If
                If Not My.Computer.Network.IsAvailable Then Exit Sub

                If SettingDialog.DMPeriodInt > 0 Then
                    TimerDM.Interval = SettingDialog.DMPeriodInt * 1000
                Else
                    TimerDM.Interval = 600000
                End If
                If (rslt.page < rslt.endPage AndAlso Not _initial) OrElse _
                   (rslt.page + 1 < SettingDialog.ReadPagesDM AndAlso _initial) Then
                    args.page = rslt.endPage
                    args.endPage = rslt.endPage
                    args.type = WORKERTYPE.DirectMessegeRcv
                    StatusLabel.Text = "DMRcv更新中..."
                    NotifyIcon1.Icon = NIconRefresh(0)
                    _refreshIconCnt = 0
                    TimerRefreshIcon.Enabled = True
                    Do While GetTimelineWorker.IsBusy
                        Threading.Thread.Sleep(1)
                        Application.DoEvents()
                    Loop
                    GetTimelineWorker.RunWorkerAsync(args)
                    Exit Sub
                End If

                args.page = 1
                args.endPage = 1
                args.type = WORKERTYPE.DirectMessegeSnt
                StatusLabel.Text = "DMSnt更新中..."
                NotifyIcon1.Icon = NIconRefresh(0)
                _refreshIconCnt = 0
                TimerRefreshIcon.Enabled = True
                Do While GetTimelineWorker.IsBusy
                    Threading.Thread.Sleep(1)
                    Application.DoEvents()
                Loop
                GetTimelineWorker.RunWorkerAsync(args)
            Case WORKERTYPE.DirectMessegeSnt
                StatusLabel.Text = "DMSnt更新完了"
                If rslt.TLine.Count > 0 Then
                    RefreshDirectMessage(rslt.TLine, False)
                End If
                TimerDM.Enabled = False
                If rslt.retMsg <> "" Then
                    StatusLabel.Text = rslt.retMsg
                    If _initial Then TimerDM.Interval = 30000
                    If My.Computer.Network.IsAvailable Then
                        If SettingDialog.DMPeriodInt > 0 OrElse _initial Then TimerDM.Enabled = True
                    End If
                    Exit Sub
                End If
                If Not My.Computer.Network.IsAvailable Then Exit Sub
                If (rslt.page < rslt.endPage AndAlso Not _initial) OrElse _
                   (rslt.page + 1 < SettingDialog.ReadPagesDM AndAlso _initial) Then
                    args.page = rslt.endPage
                    args.endPage = rslt.endPage
                    args.type = WORKERTYPE.DirectMessegeSnt
                    StatusLabel.Text = "DMSnt更新中..."
                    NotifyIcon1.Icon = NIconRefresh(0)
                    _refreshIconCnt = 0
                    TimerRefreshIcon.Enabled = True
                    Do While GetTimelineWorker.IsBusy
                        Threading.Thread.Sleep(1)
                        Application.DoEvents()
                    Loop
                    GetTimelineWorker.RunWorkerAsync(args)
                    Exit Sub
                End If

                If SettingDialog.DMPeriodInt > 0 Then
                    TimerDM.Interval = SettingDialog.DMPeriodInt * 1000
                Else
                    TimerDM.Interval = 600000
                End If
                If SettingDialog.DMPeriodInt > 0 Then TimerDM.Enabled = True

                If _initial Then
                    If SettingDialog.ReadPages > 0 Then
                        args.page = 1
                        args.endPage = 1
                        args.type = WORKERTYPE.Timeline
                        StatusLabel.Text = "Recent更新中..."
                        NotifyIcon1.Icon = NIconRefresh(0)
                        _refreshIconCnt = 0
                        TimerRefreshIcon.Enabled = True
                        Do While GetTimelineWorker.IsBusy
                            Threading.Thread.Sleep(1)
                            Application.DoEvents()
                        Loop
                        GetTimelineWorker.RunWorkerAsync(args)
                        Exit Sub
                    End If
                    If SettingDialog.ReadPagesReply > 0 Then
                        args.page = 1
                        args.endPage = 1
                        args.type = WORKERTYPE.Reply
                        StatusLabel.Text = "Reply更新中..."
                        NotifyIcon1.Icon = NIconRefresh(0)
                        _refreshIconCnt = 0
                        TimerRefreshIcon.Enabled = True
                        Do While GetTimelineWorker.IsBusy
                            Threading.Thread.Sleep(1)
                            Application.DoEvents()
                        Loop
                        GetTimelineWorker.RunWorkerAsync(args)
                        Exit Sub
                    End If
                    StatusLabel.Text = "起動時読込完了"
                    _initial = False
                End If
            Case WORKERTYPE.PostMessage
                StatusText.Enabled = True
                PostButton.Enabled = True
                ReplyStripMenuItem.Enabled = True
                DMStripMenuItem.Enabled = True

                If rslt.retMsg.Length > 0 Then
                    StatusLabel.Text = rslt.retMsg
                    TimerRefreshIcon.Enabled = False
                    NotifyIcon1.Icon = NIconAtRed
                Else
                    StatusLabel.Text = "POST完了"
                    _history.Add("")
                    _hisIdx = _history.Count - 1
                    SetMainWindowTitle()
                End If

                args.page = 1
                args.endPage = 1
                args.type = WORKERTYPE.Timeline
                If Not GetTimelineWorker.IsBusy Then
                    'TimerTimeline.Enabled = False
                    StatusLabel.Text = "Recent更新中..."
                    NotifyIcon1.Icon = NIconRefresh(0)
                    _refreshIconCnt = 0
                    TimerRefreshIcon.Enabled = True
                    Do While GetTimelineWorker.IsBusy
                        Threading.Thread.Sleep(1)
                        Application.DoEvents()
                    Loop
                    GetTimelineWorker.RunWorkerAsync(args)
                End If
            Case WORKERTYPE.FavAdd
                StatusLabel.Text = "Fav追加(" + rslt.page.ToString + "/" + rslt.ids.Count.ToString + ") 失敗:" + (rslt.page - rslt.sIds.Count).ToString
                If rslt.page < rslt.ids.Count Then
                    args.page = rslt.page
                    args.ids = rslt.ids
                    args.sIds = rslt.sIds
                    args.tName = rslt.tName
                    args.type = WORKERTYPE.FavAdd
                    NotifyIcon1.Icon = NIconRefresh(0)
                    _refreshIconCnt = 0
                    TimerRefreshIcon.Enabled = True
                    Do While GetTimelineWorker.IsBusy
                        Threading.Thread.Sleep(1)
                        Application.DoEvents()
                    Loop
                    GetTimelineWorker.RunWorkerAsync(args)
                Else
                    For Each tp As TabPage In ListTab.TabPages
                        If tp.Text = rslt.tName Then
                            Dim MyList As DetailsListView = DirectCast(tp.Controls(0), DetailsListView)
                            For Each itm As ListViewItem In MyList.Items
                                If rslt.sIds.Contains(itm.SubItems(5).Text) Then
                                    itm.ForeColor = _clFav
                                    itm.SubItems(9).Text = "True"
                                    For idx As Integer = 0 To ListTab.TabCount - 1
                                        If ListTab.TabPages(idx).Text <> rslt.tName AndAlso ListTab.TabPages(idx).Text <> "Direct" Then
                                            Dim MyList2 As DetailsListView = DirectCast(ListTab.TabPages(idx).Controls(0), DetailsListView)
                                            For cnt3 As Integer = 0 To MyList2.Items.Count - 1
                                                If itm.SubItems(5).Text = MyList2.Items(cnt3).SubItems(5).Text Then
                                                    MyList2.Items(cnt3).ForeColor = _clFav
                                                    MyList2.Items(cnt3).SubItems(9).Text = "True"
                                                    Exit For
                                                End If
                                            Next
                                        End If
                                    Next
                                End If
                            Next
                            Exit For
                        End If
                    Next
                    If DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView).SelectedItems.Count = 1 Then
                        Dim itm As ListViewItem = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView).SelectedItems(0)
                        If itm.SubItems(9).Text = "True" Then
                            NameLabel.ForeColor = _clFav
                        ElseIf itm.SubItems(10).Text = "True" Then
                            NameLabel.ForeColor = _clOWL
                        Else
                            NameLabel.ForeColor = _clReaded
                        End If
                    End If
                End If
            Case WORKERTYPE.FavRemove
                StatusLabel.Text = "Fav削除(" + rslt.page.ToString + "/" + rslt.ids.Count.ToString + ") 失敗:" + (rslt.page - rslt.sIds.Count).ToString
                If rslt.page < rslt.ids.Count Then
                    args.page = rslt.page
                    args.ids = rslt.ids
                    args.sIds = rslt.sIds
                    args.tName = rslt.tName
                    args.type = WORKERTYPE.FavRemove
                    NotifyIcon1.Icon = NIconRefresh(0)
                    _refreshIconCnt = 0
                    TimerRefreshIcon.Enabled = True
                    Do While GetTimelineWorker.IsBusy
                        Threading.Thread.Sleep(1)
                        Application.DoEvents()
                    Loop
                    GetTimelineWorker.RunWorkerAsync(args)
                Else
                    Dim _cl As Color
                    Dim flw As Boolean = False
                    For Each tp As TabPage In ListTab.TabPages
                        If tp.Text = rslt.tName Then
                            Dim MyList As DetailsListView = DirectCast(tp.Controls(0), DetailsListView)
                            Dim idxt As Integer = 0
                            For idxt = 0 To _tabs.Count - 1
                                If _tabs(idxt).tabName = rslt.tName Then
                                    Exit For
                                End If
                            Next
                            For Each itm As ListViewItem In MyList.Items
                                If rslt.sIds.Contains(itm.SubItems(5).Text) Then
                                    itm.SubItems(9).Text = "False"
                                    flw = False
                                    If follower.Contains(itm.SubItems(4).Text) Then
                                        flw = True
                                        itm.SubItems(10).Text = "False"
                                    End If
                                    If SettingDialog.UnreadManage AndAlso _tabs(idxt).unreadManage AndAlso itm.SubItems(8).Text = "False" Then
                                        _cl = _clUnread
                                    Else
                                        _cl = _clReaded
                                    End If
                                    If Not flw AndAlso SettingDialog.OneWayLove Then
                                        _cl = _clOWL
                                    End If
                                    itm.ForeColor = _cl
                                    For idx As Integer = 0 To ListTab.TabCount - 1
                                        If ListTab.TabPages(idx).Text <> rslt.tName AndAlso ListTab.TabPages(idx).Text <> "Direct" Then
                                            Dim MyList2 As DetailsListView = DirectCast(ListTab.TabPages(idx).Controls(0), DetailsListView)
                                            Dim idxt2 As Integer = 0
                                            Dim _cl2 As Color
                                            For idxt2 = 0 To _tabs.Count - 1
                                                If _tabs(idxt2).tabName = ListTab.TabPages(idx).Text Then
                                                    If SettingDialog.UnreadManage AndAlso _tabs(idxt2).unreadManage AndAlso itm.SubItems(8).Text = "False" Then
                                                        _cl2 = _clUnread
                                                    Else
                                                        _cl2 = _clReaded
                                                    End If
                                                    If Not flw And SettingDialog.OneWayLove Then
                                                        _cl2 = _clOWL
                                                    End If
                                                    Exit For
                                                End If
                                            Next
                                            For cnt3 As Integer = 0 To MyList2.Items.Count - 1
                                                If itm.SubItems(5).Text = MyList2.Items(cnt3).SubItems(5).Text Then
                                                    MyList2.Items(cnt3).ForeColor = _cl2
                                                    MyList2.Items(cnt3).SubItems(9).Text = "False"
                                                    Exit For
                                                End If
                                            Next
                                        End If
                                    Next
                                End If
                            Next
                            Exit For
                        End If
                    Next
                    If DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView).SelectedItems.Count = 1 Then
                        Dim itm As ListViewItem = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView).SelectedItems(0)
                        If itm.SubItems(9).Text = "True" Then
                            NameLabel.ForeColor = _clFav
                        ElseIf itm.SubItems(10).Text = "True" Then
                            NameLabel.ForeColor = _clOWL
                        Else
                            NameLabel.ForeColor = _clReaded
                        End If
                    End If
                End If
        End Select

    End Sub

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

        Dim cnt As Integer = 0
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)

        If ListTab.SelectedTab.Text = "Direct" OrElse MyList.SelectedItems.Count = 0 Then Exit Sub

        If MyList.SelectedItems.Count > 1 Then
            If MessageBox.Show("選択された発言をFavoritesに追加します。よろしいですか？", "Fav確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Cancel Then
                Exit Sub
            End If
        End If

        NotifyIcon1.Icon = NIconRefresh(0)
        _refreshIconCnt = 0
        TimerRefreshIcon.Enabled = True
        StatusLabel.Text = "Fav追加中..."

        Dim args As New GetWorkerArg()
        args.ids = New List(Of String)()
        args.sIds = New List(Of String)()
        args.tName = ListTab.SelectedTab.Text
        For cnt = 0 To MyList.SelectedItems.Count - 1
            If MyList.SelectedItems(cnt).SubItems(9).Text = "False" Then
                args.ids.Add(MyList.SelectedItems(cnt).SubItems(5).Text)
            End If
        Next
        args.type = WORKERTYPE.FavAdd
        If args.ids.Count = 0 Then
            StatusLabel.Text = "Fav追加なし"
            Exit Sub
        End If

        Do While GetTimelineWorker.IsBusy
            Threading.Thread.Sleep(1)
            Application.DoEvents()
        Loop

        GetTimelineWorker.RunWorkerAsync(args)

    End Sub

    Private Sub FavRemoveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FavRemoveToolStripMenuItem.Click

        Dim cnt As Integer = 0
        Dim rtn As String = ""
        Dim msg As String = ""
        Dim cnt2 As Integer = 0
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        Dim MyList2 As DetailsListView = Nothing
        Dim cnt3 As Integer = 0
        Dim tabName As String = ListTab.SelectedTab.Text
        Dim idx As Integer = 0
        Dim flw As Boolean = False

        If ListTab.SelectedTab.Text = "Direct" OrElse MyList.SelectedItems.Count = 0 Then Exit Sub

        If MyList.SelectedItems.Count > 1 Then
            If MessageBox.Show("選択された発言をFavoritesから削除します。よろしいですか？", "Fav確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Cancel Then
                Exit Sub
            End If
        End If

        StatusLabel.Text = "Fav削除中..."
        NotifyIcon1.Icon = NIconRefresh(0)
        _refreshIconCnt = 0
        TimerRefreshIcon.Enabled = True

        Dim args As New GetWorkerArg()
        args.ids = New List(Of String)()
        args.sIds = New List(Of String)()
        args.tName = ListTab.SelectedTab.Text
        For cnt = 0 To MyList.SelectedItems.Count - 1
            If MyList.SelectedItems(cnt).SubItems(9).Text = "True" Then
                args.ids.Add(MyList.SelectedItems(cnt).SubItems(5).Text)
            End If
        Next
        args.type = WORKERTYPE.FavRemove
        If args.ids.Count = 0 Then
            StatusLabel.Text = "Fav削除なし"
            Exit Sub
        End If

        Do While GetTimelineWorker.IsBusy
            Threading.Thread.Sleep(1)
            Application.DoEvents()
        Loop

        GetTimelineWorker.RunWorkerAsync(args)

    End Sub

    Private Sub MoveToHomeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveToHomeToolStripMenuItem.Click
        Do While ExecWorker.IsBusy
            Threading.Thread.Sleep(1)
            Application.DoEvents()
        Loop

        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)

        '後でタブ追加して独自読み込みにする
        If MyList.SelectedItems.Count > 0 Then
            ExecWorker.RunWorkerAsync("http://twitter.com/" + MyList.SelectedItems(0).SubItems(4).Text)
        End If
    End Sub

    Private Sub MoveToFavToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveToFavToolStripMenuItem.Click
        Do While ExecWorker.IsBusy
            Threading.Thread.Sleep(1)
            Application.DoEvents()
        Loop

        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)

        '後でタブ追加して独自読み込みにする
        If MyList.SelectedItems.Count > 0 Then
            ExecWorker.RunWorkerAsync("http://twitter.com/" + MyList.SelectedItems(0).SubItems(4).Text + "/favorites")
        End If
    End Sub

    Private Sub Tween_ClientSizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ClientSizeChanged
        If Me.WindowState = FormWindowState.Normal Then
            _mySize = Me.ClientSize
            _mySpDis = Me.SplitContainer1.SplitterDistance
        End If
    End Sub

    Private Sub MyList_ColumnClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs)
        If SettingDialog.SortOrderLock Then Exit Sub

        If Not _iconCol Then
            listViewItemSorter.Column = e.Column
        Else
            listViewItemSorter.Column = 3
        End If

        For Each _tab As TabPage In ListTab.TabPages
            Dim MyList As DetailsListView = DirectCast(_tab.Controls(0), DetailsListView)
            MyList.Sort()
        Next
    End Sub

    Private Sub Tween_LocationChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LocationChanged
        If Me.WindowState = FormWindowState.Normal Then
            _myLoc = Me.Location
        End If
    End Sub

    Private Sub RefreshDirectMessage(ByVal tlList As List(Of Twitter.MyListItem), ByVal IsReceive As Boolean)
        Dim lItem As Twitter.MyListItem
        Dim cnt As Integer = 0
        Dim unread As Integer = 0
        Dim newCnt As Integer = 0
        Dim _pop As String = ""
        Dim topItem As ListViewItem
        Dim dmFont As Boolean = False
        Dim _readed As Boolean
        Dim _fav As Boolean
        Dim _onewaylove As Boolean
        Dim nm As String = ""
        Dim Protect As String

        Dim _item As ListViewItem

        If _tabs(2).listCustom.Items.Count > 0 Then
            If ListLockMenuItem.Checked Then
                topItem = _tabs(2).listCustom.TopItem
            Else
                If listViewItemSorter.Column = 3 Then
                    If listViewItemSorter.Order = SortOrder.Ascending Then
                        '日時昇順
                        _item = _tabs(2).listCustom.GetItemAt(0, _tabs(2).listCustom.ClientSize.Height - 1)
                        If _item Is Nothing Then _item = _tabs(2).listCustom.Items(_tabs(2).listCustom.Items.Count - 1)
                        If _item.Index = _tabs(2).listCustom.Items.Count - 1 Then
                            topItem = Nothing
                        Else
                            topItem = _tabs(2).listCustom.TopItem
                        End If
                    Else
                        '日時降順
                        _item = _tabs(2).listCustom.GetItemAt(0, 25)
                        If _item Is Nothing Then _item = _tabs(2).listCustom.Items(0)
                        If _item.Index = 0 Then
                            topItem = Nothing
                        Else
                            topItem = _tabs(2).listCustom.TopItem
                        End If
                    End If
                Else
                    topItem = _tabs(2).listCustom.TopItem
                End If

            End If
        Else
            topItem = Nothing
        End If

        'DirectMsg.SuspendLayout()
        _tabs(2).listCustom.BeginUpdate()

        For cnt = 0 To tlList.Count - 1
            _readed = True
            _fav = False
            _onewaylove = Not IsReceive
            lItem = tlList(cnt)
            If lItem.Protect = True Then
                Protect = "Ю"
            Else
                Protect = ""
            End If
            Dim sItem() As String = {Protect, lItem.Nick, lItem.Data, lItem.PDate.ToString("yy-MM-dd HH:mm:ss"), lItem.Name, lItem.Id, lItem.ImageUrl, lItem.OrgData, _readed.ToString, _fav.ToString, _onewaylove.ToString, "False"}
            Dim lvItem As New ListViewItem(sItem)
            lvItem.Font = _fntReaded
            lvItem.ForeColor = _clReaded
            If Not IsReceive Then
                lvItem.ForeColor = _clOWL
            End If
            lvItem.ToolTipText = lItem.Data
            lvItem.ImageKey = lItem.ImageUrl
            _tabs(2).allCount += 1
            If SettingDialog.UnreadManage AndAlso Not _initial AndAlso _tabs(2).unreadManage Then
                lvItem.Font = _fntUnread
                lvItem.ForeColor = _clUnread
                If Not IsReceive Then
                    lvItem.ForeColor = _clOWL
                End If
                lvItem.SubItems(8).Text = "False"
                _readed = False
                If dmFont = False Then
                    dmFont = True
                    ListTab.TabPages(2).ImageIndex = 0
                End If
                _tabs(2).unreadCount += 1
                If _tabs(2).oldestUnreadItem IsNot Nothing Then
                    If lvItem.SubItems(5).Text < _tabs(2).oldestUnreadItem.SubItems(5).Text Then _tabs(2).oldestUnreadItem = lvItem
                Else
                    _tabs(2).oldestUnreadItem = lvItem
                End If
            End If
            _tabs(2).listCustom.Items.Add(lvItem)

            newCnt += 1
            Select Case SettingDialog.NameBalloon
                Case NameBalloonEnum.None
                    nm = ""
                Case NameBalloonEnum.UserID
                    nm = lItem.Name
                Case NameBalloonEnum.NickName
                    nm = lItem.Nick
            End Select
            If newCnt = 1 Then
                _pop = nm + " : " + lItem.Data
            Else
                _pop += vbCrLf + nm + " : " + lItem.Data
            End If
        Next

        If newCnt > 0 Then
            If topItem IsNot Nothing Then
                If _tabs(2).listCustom.Items.Count > 0 AndAlso topItem.Index > -1 Then
                    _tabs(2).listCustom.EnsureVisible(_tabs(2).listCustom.Items.Count - 1)
                    _tabs(2).listCustom.EnsureVisible(topItem.Index)
                End If
            Else
                If listViewItemSorter.Column = 3 AndAlso listViewItemSorter.Order = SortOrder.Ascending AndAlso _tabs(2).listCustom.Items.Count > 0 Then
                    _tabs(2).listCustom.EnsureVisible(_tabs(2).listCustom.Items.Count - 1)
                End If
            End If
            If Not _initial AndAlso NewPostPopMenuItem.Checked AndAlso _tabs(2).notify Then
                NotifyIcon1.BalloonTipIcon = ToolTipIcon.Warning
                NotifyIcon1.BalloonTipTitle = "Tween [DM] 新着 " + newCnt.ToString() + "件"
                NotifyIcon1.BalloonTipText = _pop
                NotifyIcon1.ShowBalloonTip(500)
            End If

        End If
        If Not _initial AndAlso SettingDialog.PlaySound AndAlso _tabs(2).soundFile <> "" Then
            Try
                My.Computer.Audio.Play(My.Application.Info.DirectoryPath.ToString() + "\" + _tabs(2).soundFile, AudioPlayMode.Background)
            Catch ex As Exception

            End Try
        End If
        'DirectMsg.ResumeLayout(True)
        _tabs(2).listCustom.EndUpdate()

        If SettingDialog.UnreadManage Then
            If _tabs(2).unreadManage AndAlso _tabs(2).unreadCount > 0 AndAlso _tabs(2).tabPage.ImageIndex = -1 Then
                _tabs(2).tabPage.ImageIndex = 0
            End If
        End If

        SetMainWindowTitle()
    End Sub

    Private Sub ContextMenuStrip2_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip2.Opening
        If ListTab.SelectedTab.Text = "Direct" Then
            FavAddToolStripMenuItem.Enabled = False
            FavRemoveToolStripMenuItem.Enabled = False
            StatusOpenMenuItem.Enabled = False
            FavorareMenuItem.Enabled = False
        Else
            If My.Computer.Network.IsAvailable Then
                FavAddToolStripMenuItem.Enabled = True
                FavRemoveToolStripMenuItem.Enabled = True
                StatusOpenMenuItem.Enabled = True
                FavorareMenuItem.Enabled = True
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
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        Dim myPost As Boolean = False

        If ListTab.SelectedTab.Text <> "Direct" Then
            For Each item As ListViewItem In MyList.SelectedItems
                If item.SubItems(4).Text.Equals(_username, StringComparison.CurrentCultureIgnoreCase) Then
                    myPost = True
                    Exit For
                End If
            Next

            If Not myPost Then Exit Sub
        End If

        If MessageBox.Show("選択されている自発言(またはDM)を削除してもよろしいですか？" + vbCrLf + _
             "注意　：　Twitterサーバからも削除されます！" + vbCrLf + _
             "　タブからIDを削除する場合は、「IDを移動」を使ってください。" + vbCrLf + _
             "　タブを削除する場合は、「タブを削除」を使ってください。" + vbCrLf + vbCrLf + _
             "削除処理を中止するには、「キャンセル」ボタンを押してください。", _
             "削除確認", _
              MessageBoxButtons.OKCancel, _
              MessageBoxIcon.Question) = Windows.Forms.DialogResult.Cancel Then Exit Sub

        NotifyIcon1.Icon = NIconRefresh(0)
        _refreshIconCnt = 0
        TimerRefreshIcon.Enabled = True

        If ListTab.SelectedTab.Text <> "Direct" Then
            Dim cnt As Integer = 0
            Dim cnt2 As Integer = 0
            'Dim cnt3 As Integer = 0
            Dim rtn As String = ""
            Dim msg As String = ""
            'Dim idx As Integer = 0

            For cnt = 0 To MyList.SelectedItems.Count - 1
                If MyList.SelectedItems(cnt2).SubItems(4).Text.Equals(_username, StringComparison.CurrentCultureIgnoreCase) Then    'IgnoreCase
                    rtn = clsTwSync.RemoveStatus(MyList.SelectedItems(cnt2).SubItems(5).Text)
                    If rtn.Length > 0 Then
                        'エラー
                        msg = rtn + vbCrLf
                        msg += MyList.SelectedItems(cnt2).SubItems(1).Text + ":"
                        If MyList.SelectedItems(cnt2).SubItems(2).Text.Length > 5 Then
                            msg += MyList.SelectedItems(cnt2).SubItems(2).Text.Substring(0, 5) + "..." + vbCrLf
                        Else
                            msg += MyList.SelectedItems(cnt2).SubItems(2).Text + vbCrLf
                        End If
                        cnt2 += 1
                    Else
                        '削除成功→リストから削除（全タブチェック）
                        For Each ts As TabStructure In _tabs
                            If Not ts.listCustom.Equals(MyList) AndAlso ts.tabPage.Text <> "Direct" Then
                                For Each itm As ListViewItem In ts.listCustom.Items
                                    If MyList.SelectedItems(cnt2).SubItems(5).Text = itm.SubItems(5).Text Then
                                        If ts.oldestUnreadItem IsNot Nothing AndAlso ts.oldestUnreadItem.Equals(itm) Then ts.oldestUnreadItem = Nothing
                                        If itm.SubItems(8).Text = "False" Then ts.unreadCount -= 1
                                        ts.allCount -= 1
                                        ts.listCustom.Items.Remove(itm)
                                        Exit For
                                    End If
                                Next
                            End If
                        Next
                        For Each ts As TabStructure In _tabs
                            If ts.listCustom.Equals(MyList) Then
                                If ts.oldestUnreadItem IsNot Nothing AndAlso ts.oldestUnreadItem.Equals(MyList.SelectedItems(cnt2)) Then ts.oldestUnreadItem = Nothing
                                If MyList.SelectedItems(cnt2).SubItems(8).Text = "False" Then ts.unreadCount -= 1
                                ts.allCount -= 1
                                Exit For
                            End If
                        Next
                        MyList.Items.Remove(MyList.SelectedItems(cnt2))
                    End If
                Else
                    cnt2 += 1
                End If
            Next

            If msg <> "" Then
                'StatusLabel.Text = "削除失敗 " + msg
                StatusLabel.Text = "削除失敗"
            Else
                StatusLabel.Text = "削除成功"
            End If
        ElseIf ListTab.SelectedTab.Text = "Direct" Then
            Dim cnt As Integer = 0
            Dim cnt2 As Integer = 0
            Dim rtn As String = ""
            Dim msg As String = ""

            For cnt = 0 To MyList.SelectedItems.Count - 1
                rtn = clsTwSync.RemoveDirectMessage(MyList.SelectedItems(cnt2).SubItems(5).Text)
                If rtn.Length > 0 Then
                    msg = rtn + vbCrLf
                    msg += MyList.SelectedItems(cnt2).SubItems(1).Text + ":"
                    If MyList.SelectedItems(cnt2).SubItems(2).Text.Length > 5 Then

                        msg += MyList.SelectedItems(cnt2).SubItems(2).Text.Substring(0, 5) + "..." + vbCrLf
                    Else
                        msg += MyList.SelectedItems(cnt2).SubItems(2).Text + vbCrLf
                    End If
                    cnt2 += 1
                Else
                    For Each ts As TabStructure In _tabs
                        If ts.listCustom.Equals(MyList) Then
                            If ts.oldestUnreadItem IsNot Nothing AndAlso ts.oldestUnreadItem.Equals(MyList.SelectedItems(cnt2)) Then ts.oldestUnreadItem = Nothing
                            If MyList.SelectedItems(cnt2).SubItems(8).Text = "False" Then ts.unreadCount -= 1
                            ts.allCount -= 1
                            Exit For
                        End If
                    Next
                    MyList.Items.Remove(MyList.SelectedItems(cnt2))
                End If
            Next

            If msg <> "" Then
                'StatusLabel.Text = "削除失敗 " + msg
                StatusLabel.Text = "削除失敗"
            Else
                StatusLabel.Text = "削除成功"
            End If
        End If

        TimerRefreshIcon.Enabled = False
        NotifyIcon1.Icon = NIconAt
    End Sub

    Private Sub ReadedStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReadedStripMenuItem.Click
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)

        '現在のタブのIndexを保持
        Dim idx As Integer = 0
        For idx = 0 To _tabs.Count - 1
            If _tabs(idx).tabName = ListTab.SelectedTab.Text Then
                Exit For
            End If
        Next

        If _tabs(idx).unreadManage = False Or SettingDialog.UnreadManage = False Then Exit Sub

        If MyList.SelectedItems.Count > 0 Then
            For Each lItem As ListViewItem In MyList.SelectedItems
                ItemReaded(_tabs(idx), lItem)
            Next
        End If

    End Sub

    Private Sub ItemReaded(ByVal ts As TabStructure, ByVal lItem As ListViewItem)
        If Not ts.unreadManage OrElse Not SettingDialog.UnreadManage Then Exit Sub

        If lItem.SubItems(8).Text = "False" Then
            lItem.SubItems(8).Text = "True"
            Dim fcl As Color = _clReaded
            If lItem.SubItems(10).Text = "True" AndAlso SettingDialog.OneWayLove Then fcl = _clOWL
            If lItem.SubItems(9).Text = "True" Then fcl = _clFav
            ts.listCustom.ChangeItemStyles(lItem.Index, lItem.BackColor, fcl, _fntReaded)
            ts.unreadCount -= 1
            If ts.oldestUnreadItem IsNot Nothing AndAlso ts.oldestUnreadItem.Equals(lItem) OrElse _
               ts.oldestUnreadItem Is Nothing AndAlso ts.unreadCount > 0 Then
                '次の未読アイテム探索
                Dim stp As Integer = 1
                Dim frmi As Integer = 0
                Dim toi As Integer = 0
                '日時ソート（＝ID順）の場合
                If listViewItemSorter.Column = 3 Then
                    If listViewItemSorter.Order = SortOrder.Ascending Then
                        '昇順
                        If ts.oldestUnreadItem Is Nothing Then
                            frmi = 0
                        Else
                            frmi = ts.oldestUnreadItem.Index
                        End If
                        toi = ts.listCustom.Items.Count - 1
                    Else
                        '降順
                        stp = -1
                        If ts.oldestUnreadItem Is Nothing Then
                            frmi = ts.listCustom.Items.Count - 1
                        Else
                            frmi = ts.oldestUnreadItem.Index
                        End If
                    End If
                Else
                    '日時以外が基準の場合は頭から探索
                    frmi = 0
                    toi = ts.listCustom.Items.Count - 1
                End If
                ts.oldestUnreadItem = Nothing
                For i As Integer = frmi To toi Step stp
                    If ts.listCustom.Items(i).SubItems(8).Text = "False" Then
                        ts.oldestUnreadItem = ts.listCustom.Items(i)
                        Exit For
                    End If
                Next
            End If
            If ts.tabName <> "Direct" Then
                '全タブの未読状態を合わせる
                For Each ts2 As TabStructure In _tabs
                    If Not ts2.listCustom.Equals(ts.listCustom) AndAlso ts2.tabName <> "Direct" AndAlso ts2.unreadCount > 0 AndAlso ts2.unreadManage Then
                        '最古未読アイテムから探索
                        Dim stp As Integer = 1
                        Dim frmi As Integer = 0
                        Dim toi As Integer = 0
                        '日時ソート（＝ID順）の場合
                        If listViewItemSorter.Column = 3 Then
                            If listViewItemSorter.Order = SortOrder.Ascending Then
                                '昇順
                                If ts2.oldestUnreadItem Is Nothing Then
                                    frmi = 0
                                Else
                                    frmi = ts2.oldestUnreadItem.Index
                                End If
                                toi = ts2.listCustom.Items.Count - 1
                            Else
                                '降順
                                If ts2.oldestUnreadItem Is Nothing Then
                                    frmi = ts2.listCustom.Items.Count - 1
                                Else
                                    frmi = ts2.oldestUnreadItem.Index
                                End If
                                stp = -1
                            End If
                        Else
                            '日時以外が基準の場合は頭から探索
                            frmi = 0
                            toi = ts2.listCustom.Items.Count - 1
                        End If
                        For i As Integer = frmi To toi Step stp
                            If ts2.listCustom.Items(i).SubItems(5).Text = lItem.SubItems(5).Text Then
                                If ts2.listCustom.Items(i).SubItems(8).Text = "False" Then
                                    ts2.unreadCount -= 1
                                    ts2.listCustom.Items(i).SubItems(8).Text = "True"
                                    ts2.listCustom.ChangeItemStyles(i, ts2.listCustom.Items(i).BackColor, fcl, _fntReaded)
                                    If i = frmi Then
                                        ts2.oldestUnreadItem = Nothing
                                        If ts2.unreadCount > 0 Then
                                            Dim stp2 As Integer = stp
                                            Dim frmi2 As Integer = frmi
                                            Dim toi2 As Integer = toi
                                            For i2 As Integer = frmi2 To toi2 Step stp2
                                                If ts2.listCustom.Items(i2).SubItems(8).Text = "False" Then
                                                    ts2.oldestUnreadItem = ts2.listCustom.Items(i2)
                                                    Exit For
                                                End If
                                            Next
                                        End If
                                    End If
                                End If
                                Exit For
                            End If
                        Next
                        If ts2.unreadCount = 0 AndAlso ts2.tabPage.ImageIndex = 0 Then
                            ts2.tabPage.ImageIndex = -1
                        End If
                    End If
                Next
            End If
        End If
        If ts.unreadCount = 0 AndAlso ts.tabPage.ImageIndex = 0 Then
            ts.tabPage.ImageIndex = -1
        End If

    End Sub

    Private Sub UnreadStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UnreadStripMenuItem.Click
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        Dim idx As Integer = 0
        For idx = 0 To _tabs.Count - 1
            If _tabs(idx).tabName = ListTab.SelectedTab.Text Then
                Exit For
            End If
        Next

        If Not _tabs(idx).unreadManage OrElse Not SettingDialog.UnreadManage Then Exit Sub

        If MyList.SelectedItems.Count > 0 Then
            For Each lItem As ListViewItem In MyList.SelectedItems
                If lItem.SubItems(8).Text = "True" Then
                    lItem.SubItems(8).Text = "False"
                    'lItem.Font = _fntUnread
                    Dim fcl As Color = _clUnread
                    If lItem.SubItems(10).Text = "True" AndAlso SettingDialog.OneWayLove Then fcl = _clOWL
                    If lItem.SubItems(9).Text = "True" Then fcl = _clFav
                    'lItem.ForeColor = fcl
                    MyList.ChangeItemStyles(lItem.Index, lItem.BackColor, fcl, _fntUnread)
                    _tabs(idx).unreadCount += 1
                    If _tabs(idx).oldestUnreadItem Is Nothing Then
                        _tabs(idx).oldestUnreadItem = lItem
                    Else
                        If _tabs(idx).oldestUnreadItem.SubItems(5).Text > lItem.SubItems(5).Text Then
                            _tabs(idx).oldestUnreadItem = lItem
                        End If
                    End If
                    If _tabs(idx).tabName <> "Direct" Then
                        '全タブの未読状態を合わせる
                        For Each ts As TabStructure In _tabs
                            If Not ts.listCustom.Equals(MyList) AndAlso ts.tabName <> "Direct" AndAlso ts.unreadManage Then
                                For Each itm As ListViewItem In ts.listCustom.Items
                                    If itm.SubItems(5).Text = lItem.SubItems(5).Text Then
                                        itm.SubItems(8).Text = "False"
                                        'itm.Font = _fntUnread
                                        'itm.ForeColor = fcl
                                        ts.listCustom.ChangeItemStyles(itm.Index, itm.BackColor, fcl, _fntUnread)
                                        ts.unreadCount += 1
                                        If ts.tabPage.ImageIndex = -1 Then
                                            ts.tabPage.ImageIndex = 0
                                        End If
                                        If ts.oldestUnreadItem Is Nothing Then
                                            ts.oldestUnreadItem = itm
                                        Else
                                            If ts.oldestUnreadItem.SubItems(5).Text > itm.SubItems(5).Text Then
                                                ts.oldestUnreadItem = itm
                                            End If
                                        End If
                                        Exit For
                                    End If
                                Next
                                If ts.unreadCount > 0 AndAlso ts.tabPage.ImageIndex = -1 Then
                                    ts.tabPage.ImageIndex = 0
                                End If
                            End If
                        Next
                    End If
                End If
            Next
        End If
        If _tabs(idx).unreadCount > 0 AndAlso _tabs(idx).tabPage.ImageIndex = -1 Then
            _tabs(idx).tabPage.ImageIndex = 0
        End If

    End Sub

    Private Sub RefreshStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshStripMenuItem.Click
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)

        NotifyIcon1.Icon = NIconRefresh(0)
        _refreshIconCnt = 0
        TimerRefreshIcon.Enabled = True

        If ListTab.SelectedTab.Text <> "Direct" Then
            If ListTab.SelectedTab.Text <> "Reply" Then
                'TimerTimeline.Enabled = False
                Dim args As New GetWorkerArg()
                args.page = 1
                args.endPage = 1
                args.type = WORKERTYPE.Timeline
                StatusLabel.Text = "Recent更新中..."
                Do While GetTimelineWorker.IsBusy
                    Threading.Thread.Sleep(1)
                    Application.DoEvents()
                Loop
                GetTimelineWorker.RunWorkerAsync(args)
            Else
                Dim args As New GetWorkerArg()
                args.page = 1
                args.endPage = 1
                args.type = WORKERTYPE.Reply
                StatusLabel.Text = "Reply更新中..."
                Do While GetTimelineWorker.IsBusy
                    Threading.Thread.Sleep(1)
                    Application.DoEvents()
                Loop
                GetTimelineWorker.RunWorkerAsync(args)
            End If
        Else
            Dim args As New GetWorkerArg()
            args.page = 1
            args.endPage = 1
            args.type = WORKERTYPE.DirectMessegeRcv
            StatusLabel.Text = "DMRcv更新中..."
            Do While GetTimelineWorker.IsBusy
                Threading.Thread.Sleep(1)
                Application.DoEvents()
            Loop
            GetTimelineWorker.RunWorkerAsync(args)
        End If

    End Sub

    Private Sub SettingStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingStripMenuItem.Click
        TimerColorize.Stop()

        If SettingDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            SyncLock _syncObject
                _username = SettingDialog.UserID
                _password = SettingDialog.PasswordStr
                clsTw.Username = _username
                clsTw.Password = _password
                clsTwPost.Username = _username
                clsTwPost.Password = _password
                clsTwSync.Username = _username
                clsTwSync.Password = _password
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
                clsTw.NextThreshold = SettingDialog.NextPageThreshold
                clsTw.NextPages = SettingDialog.NextPagesInt
                clsTwPost.UseAPI = SettingDialog.UseAPI
                clsTw.HubServer = SettingDialog.HubServer
                clsTwPost.HubServer = SettingDialog.HubServer
                clsTwSync.HubServer = SettingDialog.HubServer
                clsTw.TinyUrlResolve = SettingDialog.TinyUrlResolve
                clsTw.RestrictFavCheck = SettingDialog.RestrictFavCheck

                clsTw.ProxyType = SettingDialog.ProxyType
                clsTw.ProxyAddress = SettingDialog.ProxyAddress
                clsTw.ProxyPort = SettingDialog.ProxyPort
                clsTw.ProxyUser = SettingDialog.ProxyUser
                clsTw.ProxyPassword = SettingDialog.ProxyPassword
                Dim args As New GetWorkerArg()
                args.type = WORKERTYPE.CreateNewSocket
                Do While GetTimelineWorker.IsBusy
                    Threading.Thread.Sleep(1)
                    Application.DoEvents()
                Loop
                GetTimelineWorker.RunWorkerAsync(args)
                clsTwPost.ProxyType = SettingDialog.ProxyType
                clsTwPost.ProxyAddress = SettingDialog.ProxyAddress
                clsTwPost.ProxyPort = SettingDialog.ProxyPort
                clsTwPost.ProxyUser = SettingDialog.ProxyUser
                clsTwPost.ProxyPassword = SettingDialog.ProxyPassword

                Do While PostWorker.IsBusy
                    Threading.Thread.Sleep(1)
                    Application.DoEvents()
                Loop
                PostWorker.RunWorkerAsync(args)

                clsTwSync.ProxyType = SettingDialog.ProxyType
                clsTwSync.ProxyAddress = SettingDialog.ProxyAddress
                clsTwSync.ProxyPort = SettingDialog.ProxyPort
                clsTwSync.ProxyUser = SettingDialog.ProxyUser
                clsTwSync.ProxyPassword = SettingDialog.ProxyPassword
                clsTw.CreateNewSocket()

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
                If SettingDialog.OneWayLove Then
                    For Each ts As TabStructure In _tabs
                        If ts.tabName <> "Direct" Then
                            For Each myItem As ListViewItem In ts.listCustom.Items
                                If follower.Contains(myItem.SubItems(4).Text) Then
                                    myItem.SubItems(10).Text = "False"
                                Else
                                    myItem.SubItems(10).Text = "True"
                                End If
                            Next
                        End If
                    Next
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
                For Each ts As TabStructure In _tabs
                    For Each myItem As ListViewItem In ts.listCustom.Items
                        If SettingDialog.UnreadManage = False OrElse ts.unreadManage = False Then
                            Dim fcl As Color = _clReaded
                            If myItem.SubItems(10).Text = "True" AndAlso SettingDialog.OneWayLove Then fcl = _clOWL
                            If myItem.SubItems(9).Text = "True" Then fcl = _clFav
                            ts.listCustom.ChangeItemStyles(myItem.Index, myItem.BackColor, fcl, _fntReaded)
                            ts.oldestUnreadItem = Nothing
                            ts.unreadCount = 0
                        End If
                    Next
                Next
                TimerColorize.Start()

                SetMainWindowTitle()
                SetNotifyIconText()
            End SyncLock
        End If

        Me.TopMost = SettingDialog.AlwaysTop
        SaveConfigs()
    End Sub

    Private Sub PostBrowser_Navigating(ByVal sender As System.Object, ByVal e As System.Windows.Forms.WebBrowserNavigatingEventArgs) Handles PostBrowser.Navigating
        If e.Url.AbsoluteUri <> "about:blank" Then
            e.Cancel = True
            Do While ExecWorker.IsBusy
                Threading.Thread.Sleep(1)
                Application.DoEvents()
            Loop
            ExecWorker.RunWorkerAsync(e.Url.AbsoluteUri)
        End If

    End Sub

    Private Sub AddCustomTabs()
        Dim cnt As Integer = 0

        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.ListTab.SuspendLayout()
        Me.SuspendLayout()

        For Each myTab As TabStructure In _tabs
            cnt += 1
            myTab.tabPage.SuspendLayout()

            Me.ListTab.Controls.Add(myTab.tabPage)

            myTab.tabPage.Controls.Add(myTab.listCustom)
            myTab.tabPage.Location = New System.Drawing.Point(4, 4)
            myTab.tabPage.Name = "CTab" + cnt.ToString
            myTab.tabPage.Size = New System.Drawing.Size(380, 260)
            myTab.tabPage.TabIndex = 2 + cnt
            myTab.tabPage.Text = myTab.tabName
            myTab.tabPage.UseVisualStyleBackColor = True

            myTab.listCustom.AllowColumnReorder = True
            If Not _iconCol Then
                myTab.listCustom.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {myTab.colHd1, myTab.colHd2, myTab.colHd3, myTab.colHd4, myTab.colHd5})
            Else
                myTab.listCustom.Columns.Add(myTab.colHd1)
            End If

            myTab.listCustom.ContextMenuStrip = Me.ContextMenuStrip2
            myTab.listCustom.Dock = System.Windows.Forms.DockStyle.Fill
            myTab.listCustom.FullRowSelect = True
            myTab.listCustom.HideSelection = False
            myTab.listCustom.Location = New System.Drawing.Point(0, 0)
            myTab.listCustom.Margin = New System.Windows.Forms.Padding(0)
            myTab.listCustom.Name = "CList" + Environment.TickCount.ToString()
            myTab.listCustom.ShowItemToolTips = True
            myTab.listCustom.Size = New System.Drawing.Size(380, 260)
            myTab.listCustom.TabIndex = 4                                   'これ大丈夫？
            myTab.listCustom.UseCompatibleStateImageBehavior = False
            myTab.listCustom.View = System.Windows.Forms.View.Details
            myTab.listCustom.OwnerDraw = True

            AddHandler myTab.listCustom.SelectedIndexChanged, AddressOf MyList_SelectedIndexChanged
            AddHandler myTab.listCustom.MouseDoubleClick, AddressOf MyList_MouseDoubleClick
            AddHandler myTab.listCustom.ColumnClick, AddressOf MyList_ColumnClick
            AddHandler myTab.listCustom.DrawColumnHeader, AddressOf MyList_DrawColumnHeader
            AddHandler myTab.listCustom.DrawItem, AddressOf MyList_DrawItem
            AddHandler myTab.listCustom.Scrolled, AddressOf Mylist_Scrolled
            AddHandler myTab.listCustom.MouseClick, AddressOf MyList_MouseClick
            AddHandler myTab.listCustom.ColumnReordered, AddressOf MyList_ColumnReordered
            AddHandler myTab.listCustom.ColumnWidthChanging, AddressOf MyList_CoumnWidthChanging

            myTab.colHd1.Text = ""
            myTab.colHd1.Width = 26
            If Not _iconCol Then
                myTab.colHd2.Text = "名前"
                myTab.colHd2.Width = 80
                myTab.colHd3.Text = "投稿"
                myTab.colHd3.Width = 300
                myTab.colHd4.Text = "日時"
                myTab.colHd4.Width = 50
                myTab.colHd5.Text = "ユーザ名"
                myTab.colHd5.Width = 50
            End If

            TabDialog.AddTab(myTab.tabName)

            myTab.listCustom.SmallImageList = TIconSmallList
            myTab.listCustom.ListViewItemSorter = listViewItemSorter
            myTab.listCustom.Columns(0).Width = _section.Width1
            myTab.listCustom.Columns(0).DisplayIndex = _section.DisplayIndex1
            If Not _iconCol Then
                myTab.listCustom.Columns(1).Width = _section.Width2
                myTab.listCustom.Columns(2).Width = _section.Width3
                myTab.listCustom.Columns(3).Width = _section.Width4
                myTab.listCustom.Columns(4).Width = _section.Width5
                myTab.listCustom.Columns(1).DisplayIndex = _section.DisplayIndex2
                myTab.listCustom.Columns(2).DisplayIndex = _section.DisplayIndex3
                myTab.listCustom.Columns(3).DisplayIndex = _section.DisplayIndex4
                myTab.listCustom.Columns(4).DisplayIndex = _section.DisplayIndex5
            End If
            _columnIdx = myTab.colHd1.DisplayIndex
            myTab.tabPage.ResumeLayout(False)
            'End If
        Next

        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.ListTab.ResumeLayout(False)
        'Me.TabPage1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Private Function AddNewTab(ByVal tabName As String) As Boolean
        For Each myT As TabStructure In _tabs
            If myT.tabName = tabName Then
                Return False
            End If
        Next

        If tabName = "(新規タブ)" OrElse _
           tabName = "Recent" OrElse _
           tabName = "Reply" OrElse _
           tabName = "Direct" Then Return False

        Dim myTab As New TabStructure()

        myTab.tabPage = New TabPage()
        myTab.listCustom = New DetailsListView()
        myTab.colHd1 = New ColumnHeader()
        If Not _iconCol Then
            myTab.colHd2 = New ColumnHeader()
            myTab.colHd3 = New ColumnHeader()
            myTab.colHd4 = New ColumnHeader()
            myTab.colHd5 = New ColumnHeader()
        End If
        myTab.tabName = tabName
        myTab.filters.Clear()
        myTab.notify = True
        myTab.soundFile = ""
        myTab.unreadManage = True
        _tabs.Add(myTab)

        _section.ListElement.Add(New ListElement(tabName))

        Dim cnt As Integer = _tabs.Count

        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.ListTab.SuspendLayout()
        Me.SuspendLayout()

        myTab.tabPage.SuspendLayout()

        Me.ListTab.Controls.Add(myTab.tabPage)

        myTab.tabPage.Controls.Add(myTab.listCustom)
        myTab.tabPage.Location = New Point(4, 4)
        myTab.tabPage.Name = "CTab" + cnt.ToString()
        myTab.tabPage.Size = New Size(380, 260)
        myTab.tabPage.TabIndex = 2 + cnt
        myTab.tabPage.Text = myTab.tabName
        myTab.tabPage.UseVisualStyleBackColor = True

        myTab.listCustom.AllowColumnReorder = True
        If Not _iconCol Then
            myTab.listCustom.Columns.AddRange(New ColumnHeader() {myTab.colHd1, myTab.colHd2, myTab.colHd3, myTab.colHd4, myTab.colHd5})
        Else
            myTab.listCustom.Columns.Add(myTab.colHd1)
        End If
        myTab.listCustom.ContextMenuStrip = Me.ContextMenuStrip2
        myTab.listCustom.Dock = DockStyle.Fill
        myTab.listCustom.FullRowSelect = True
        myTab.listCustom.HideSelection = False
        myTab.listCustom.Location = New Point(0, 0)
        myTab.listCustom.Margin = New Padding(0)
        myTab.listCustom.Name = "CList" + Environment.TickCount.ToString()
        myTab.listCustom.ShowItemToolTips = True
        myTab.listCustom.Size = New Size(380, 260)
        myTab.listCustom.TabIndex = 4                                   'これ大丈夫？
        myTab.listCustom.UseCompatibleStateImageBehavior = False
        myTab.listCustom.View = View.Details
        myTab.listCustom.OwnerDraw = True

        AddHandler myTab.listCustom.SelectedIndexChanged, AddressOf MyList_SelectedIndexChanged
        AddHandler myTab.listCustom.MouseDoubleClick, AddressOf MyList_MouseDoubleClick
        AddHandler myTab.listCustom.ColumnClick, AddressOf MyList_ColumnClick
        AddHandler myTab.listCustom.DrawColumnHeader, AddressOf MyList_DrawColumnHeader
        AddHandler myTab.listCustom.DrawItem, AddressOf MyList_DrawItem
        AddHandler myTab.listCustom.Scrolled, AddressOf Mylist_Scrolled
        AddHandler myTab.listCustom.MouseClick, AddressOf MyList_MouseClick
        AddHandler myTab.listCustom.ColumnReordered, AddressOf MyList_ColumnReordered
        AddHandler myTab.listCustom.ColumnWidthChanging, AddressOf MyList_CoumnWidthChanging

        myTab.colHd1.Text = ""
        myTab.colHd1.Width = 26
        If Not _iconCol Then
            myTab.colHd2.Text = "名前"
            myTab.colHd2.Width = 80
            myTab.colHd3.Text = "投稿"
            myTab.colHd3.Width = 300
            myTab.colHd4.Text = "日時"
            myTab.colHd4.Width = 50
            myTab.colHd5.Text = "ユーザ名"
            myTab.colHd5.Width = 50
        End If

        TabDialog.AddTab(myTab.tabName)

        myTab.listCustom.SmallImageList = TIconSmallList
        myTab.listCustom.ListViewItemSorter = listViewItemSorter
        myTab.listCustom.Columns(0).Width = _tabs(0).listCustom.Columns(0).Width
        myTab.listCustom.Columns(0).DisplayIndex = _tabs(0).listCustom.Columns(0).DisplayIndex
        If Not _iconCol Then
            myTab.listCustom.Columns(1).Width = _tabs(0).listCustom.Columns(1).Width
            myTab.listCustom.Columns(2).Width = _tabs(0).listCustom.Columns(2).Width
            myTab.listCustom.Columns(3).Width = _tabs(0).listCustom.Columns(3).Width
            myTab.listCustom.Columns(4).Width = _tabs(0).listCustom.Columns(4).Width
            myTab.listCustom.Columns(1).DisplayIndex = _tabs(0).listCustom.Columns(1).DisplayIndex
            myTab.listCustom.Columns(2).DisplayIndex = _tabs(0).listCustom.Columns(2).DisplayIndex
            myTab.listCustom.Columns(3).DisplayIndex = _tabs(0).listCustom.Columns(3).DisplayIndex
            myTab.listCustom.Columns(4).DisplayIndex = _tabs(0).listCustom.Columns(4).DisplayIndex
        End If

        myTab.tabPage.ResumeLayout(False)

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

        If TabName = "Recent" OrElse _
           TabName = "Reply" OrElse _
           TabName = "Direct" Then Exit Sub

        If MessageBox.Show("このタブを削除してもよろしいですか？" + vbCrLf + _
                        "（このタブの発言はRecentへ戻されます。）", "タブの削除確認", _
                         MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        SetListProperty()

        'タブ配列のIndexを取得
        For idx = 0 To _tabs.Count - 1
            If _tabs(idx).tabName = TabName Then Exit For
        Next

        '削除するタブに含まれる発言をRecentへ追加
        For Each itm As ListViewItem In _tabs(idx).listCustom.Items
            Dim otherEx As Boolean = False
            Dim pid As String = itm.SubItems(5).Text        'ID
            For Each titm As ListViewItem In _tabs(0).listCustom.Items
                If titm.SubItems(5).Text = pid Then
                    otherEx = True
                    Exit For
                End If
            Next
            If Not otherEx Then
                _tabs(0).listCustom.Items.Add(DirectCast(itm.Clone, System.Windows.Forms.ListViewItem))
            End If
        Next

        'configから削除（多分不要）
        _section.ListElement.Remove(TabName)

        'オブジェクトインスタンスの削除
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.ListTab.SuspendLayout()
        Me.SuspendLayout()

        _tabs(idx).tabPage.SuspendLayout()

        Me.ListTab.Controls.Remove(_tabs(idx).tabPage)

        _tabs(idx).oldestUnreadItem = Nothing
        _tabs(idx).tabPage.Controls.Remove(_tabs(idx).listCustom)

        _tabs(idx).listCustom.Columns.Clear()

        _tabs(idx).listCustom.ContextMenuStrip = Nothing

        RemoveHandler _tabs(idx).listCustom.SelectedIndexChanged, AddressOf MyList_SelectedIndexChanged
        RemoveHandler _tabs(idx).listCustom.MouseDoubleClick, AddressOf MyList_MouseDoubleClick
        RemoveHandler _tabs(idx).listCustom.ColumnClick, AddressOf MyList_ColumnClick
        RemoveHandler _tabs(idx).listCustom.DrawColumnHeader, AddressOf MyList_DrawColumnHeader
        RemoveHandler _tabs(idx).listCustom.DrawItem, AddressOf MyList_DrawItem
        RemoveHandler _tabs(idx).listCustom.Scrolled, AddressOf Mylist_Scrolled
        RemoveHandler _tabs(idx).listCustom.MouseClick, AddressOf MyList_MouseClick
        RemoveHandler _tabs(idx).listCustom.ColumnReordered, AddressOf MyList_ColumnReordered
        RemoveHandler _tabs(idx).listCustom.ColumnWidthChanging, AddressOf MyList_CoumnWidthChanging

        TabDialog.RemoveTab(_tabs(idx).tabName)

        _tabs(idx).listCustom.SmallImageList = Nothing
        '_tabs(idx).sorter = Nothing
        _tabs(idx).listCustom.ListViewItemSorter = Nothing

        _tabs(idx).tabPage.ResumeLayout(False)

        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.ListTab.ResumeLayout(False)
        'Me.TabPage1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

        _tabs(idx).tabPage.Dispose()
        _tabs(idx).listCustom.Dispose()
        _tabs(idx).colHd1.Dispose()
        If Not _iconCol Then
            _tabs(idx).colHd2.Dispose()
            _tabs(idx).colHd3.Dispose()
            _tabs(idx).colHd4.Dispose()
            _tabs(idx).colHd5.Dispose()
        End If
        _tabs(idx).filters.Clear()
        _tabs.RemoveAt(idx)
    End Sub

    Private Sub ListTab_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListTab.MouseMove
        Dim cpos As New Point(e.X, e.Y)
        Dim spos As Point = ListTab.PointToClient(cpos)

        If e.Button = Windows.Forms.MouseButtons.Left And _tabDrag Then
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

            For Each ts As TabStructure In _tabs
                If ts.tabName = tn Then
                    ListTab.DoDragDrop(ts, DragDropEffects.All)
                    Exit For
                End If
            Next
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
        TimerColorize.Stop()

        SetListProperty()

        '新しく表示されたリストを再描画
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        MyList.Update()
        TimerColorize.Start()
        '新しいタブ名を保管
        _curTabText = ListTab.SelectedTab.Text
    End Sub

    Private Sub SetListProperty()
        '直前のリスト特定
        Dim MyList As DetailsListView = Nothing
        For Each myTab As TabPage In ListTab.TabPages
            If myTab.Text = _curTabText Then
                MyList = DirectCast(myTab.Controls(0), DetailsListView)
                Exit For
            End If
        Next

        '削除などで見つからない場合は処理せず
        If MyList Is Nothing Then Exit Sub

        '列幅、列並びを他のタブに設定
        For Each _tab As TabPage In ListTab.TabPages
            If _tab.Text <> _curTabText Then
                Dim lst As DetailsListView = DirectCast(_tab.Controls(0), DetailsListView)
                For i As Integer = 0 To lst.Columns.Count - 1
                    lst.Columns(i).DisplayIndex = MyList.Columns(i).DisplayIndex
                    lst.Columns(i).Width = MyList.Columns(i).Width
                Next
            End If
        Next
    End Sub
    Private Sub PostBrowser_StatusTextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles PostBrowser.StatusTextChanged
        'tinyURLに対応する？
        If PostBrowser.StatusText.StartsWith("http") Then
            StatusLabelUrl.Text = PostBrowser.StatusText
        End If
        If PostBrowser.StatusText = "" Then
            'StatusLabelUrl.Text = ""
            SetStatusLabel()
        End If
    End Sub

    Private Sub ExecWorker_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles ExecWorker.DoWork
        Dim myPath As String = Convert.ToString(e.Argument)

        Try
            If SettingDialog.BrowserPath <> "" Then
                Shell(SettingDialog.BrowserPath & " " & myPath)
            Else
                System.Diagnostics.Process.Start(myPath)
            End If
        Catch ex As Exception
            '                MessageBox.Show("ブラウザの起動に失敗、またはタイムアウトしました。" + ex.ToString())
        End Try
    End Sub

    Private Sub StatusText_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles StatusText.KeyUp
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
        Dim pLen As Integer = 140 - StatusText.Text.Length
        lblLen.Text = pLen.ToString()
        If pLen < 0 Then
            StatusText.ForeColor = Color.Red
        Else
            StatusText.ForeColor = Color.FromKnownColor(KnownColor.ControlText)
        End If
    End Sub

    Private Sub MyList_DrawColumnHeader(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawListViewColumnHeaderEventArgs)
        e.DrawDefault = True
    End Sub

    Private Sub MyList_DrawItem(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawListViewItemEventArgs)
#If DEBUG Then
        Dim iStart As Integer = System.Environment.TickCount
#End If
        Static iSize As Integer = _iconSz

        If iSize = 48 OrElse _
           iSize = 26 Then
            If e.State = 0 Then Exit Sub

            'アイコンカラム位置取得
            Dim rct As Rectangle = Nothing
            'Dim MyList As DetailsListView = DirectCast(e.Item.ListView, Tween.TweenCustomControl.DetailsListView)

            If Not _iconCol Then
                'Dim cnt As Integer
                Static x As Integer = 0
                Static wd As Integer = 0
                Static wd2 As Integer = 0
                'Dim idx As Integer = _columnIdx     '手抜き

                'For cnt = 0 To 4
                '   If e.Item.ListView.Columns(cnt).Text = "" Then
                '       idx = MyList.Columns(cnt).DisplayIndex
                '       wd2 = MyList.Columns(cnt).Width - 2
                '   Exit For
                '   End If
                'Next
                If _columnChangeFlag = True Then
                    Dim MyList As DetailsListView = DirectCast(e.Item.ListView, Tween.TweenCustomControl.DetailsListView)
                    wd2 = MyList.Columns(_columnIdx).Width - 2
                    x = e.Item.Bounds.X
                    For cnt As Integer = 0 To 4
                        If MyList.Columns(cnt).DisplayIndex < _columnIdx Then
                            x += MyList.Columns(cnt).Width
                        End If
                    Next
                    If wd2 > iSize Then
                        wd = iSize
                    Else
                        wd = wd2
                    End If
                    rct = New Rectangle(x, e.Item.SubItems(_columnIdx).Bounds.Y + 1, wd, iSize)
                    iSize = iSize
                End If

                If e.Item.Selected Then
                    e.Graphics.FillRectangle(_brsHighLight, e.Bounds)
                    'If MyList.SmallImageList.Images.ContainsKey(e.Item.ImageKey) Then
                    If e.Item.ImageKey <> "" Then e.Graphics.DrawImageUnscaledAndClipped(TIconSmallList.Images(e.Item.ImageKey), rct)
                    'If e.Item.ImageKey <> "" Then e.Graphics.DrawImageUnscaled(TIconSmallList.Images(e.Item.ImageKey), rct)
                    'End If
                    For i As Integer = 0 To 4
                        If i = 0 Then
                            If wd2 - iSize > 0 Then
                                Dim sRct As New Rectangle(x + 1 + iSize, e.Item.SubItems(i).Bounds.Y, wd2 - iSize, e.Item.SubItems(i).Bounds.Height - 3)
                                e.Graphics.DrawString(e.Item.SubItems(i).Text, e.Item.Font, _brsHighLightText, sRct, sf)
                            End If
                        Else
                            Dim sRct As New Rectangle(e.Item.SubItems(i).Bounds.X + 1, e.Item.SubItems(i).Bounds.Y, e.Item.SubItems(i).Bounds.Width - 2, e.Item.SubItems(i).Bounds.Height - 3)
                            e.Graphics.DrawString(e.Item.SubItems(i).Text, e.Item.Font, _brsHighLightText, sRct, sf)
                        End If
                    Next
                Else
                    'e.DrawBackground()
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

                    'If MyList.SmallImageList.Images.ContainsKey(e.Item.ImageKey) Then
                    If e.Item.ImageKey <> "" Then e.Graphics.DrawImageUnscaledAndClipped(TIconSmallList.Images(e.Item.ImageKey), rct)
                    'If e.Item.ImageKey <> "" Then e.Graphics.DrawImageUnscaled(TIconSmallList.Images(e.Item.ImageKey), rct)
                    'End If

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
                    End Select
                    For i As Integer = 0 To 4
                        If i = 0 Then
                            If wd2 - iSize > 0 Then
                                Dim sRct As New Rectangle(x + 1 + iSize, e.Item.SubItems(i).Bounds.Y, wd2 - iSize, e.Item.SubItems(i).Bounds.Height - 3)
                                e.Graphics.DrawString(e.Item.SubItems(i).Text, e.Item.Font, brs, sRct, sf)
                            End If
                        Else
                            Dim sRct As New Rectangle(e.Item.SubItems(i).Bounds.X + 1, e.Item.SubItems(i).Bounds.Y, e.Item.SubItems(i).Bounds.Width - 2, e.Item.SubItems(i).Bounds.Height - 3)
                            e.Graphics.DrawString(e.Item.SubItems(i).Text, e.Item.Font, brs, sRct, sf)
                        End If
                    Next
                End If
            Else
                Dim wd As Integer
                Dim wd2 As Integer
                Dim x As Integer
                wd2 = e.Item.Bounds.Width - 2
                If wd2 > iSize Then wd = iSize
                x = e.Item.Bounds.X
                rct = New Rectangle(e.Item.Bounds.X, e.Item.Bounds.Y + 1, wd, iSize)

                If e.Item.Selected = True Then
                    e.Graphics.FillRectangle(_brsHighLight, e.Bounds)
                    'If MyList.SmallImageList.Images.ContainsKey(e.Item.ImageKey) Then
                    If e.Item.ImageKey <> "" Then e.Graphics.DrawImageUnscaledAndClipped(TIconSmallList.Images(e.Item.ImageKey), rct)
                    'End If
                    If wd2 - iSize - 5 > 0 Then
                        Dim sRct As New Rectangle(x + 5 + iSize, e.Item.Bounds.Y, wd2 - iSize - 5, e.Item.Font.Height)
                        Dim sRct2 As New Rectangle(x + 5 + iSize, e.Item.Bounds.Y + e.Item.Font.Height, wd2 - iSize - 5, iSize - e.Item.Font.Height)
                        Dim fnt As New Font(e.Item.Font, FontStyle.Bold)
                        e.Graphics.DrawString(e.Item.SubItems(1).Text + "(" + e.Item.SubItems(4).Text + ") " + e.Item.SubItems(0).Text + " " + e.Item.SubItems(3).Text, fnt, _brsHighLightText, sRct, sf)
                        e.Graphics.DrawString(e.Item.SubItems(2).Text, e.Item.Font, _brsHighLightText, sRct2, sf)
                    End If
                Else
                    'e.DrawBackground()
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

                    'If MyList.SmallImageList.Images.ContainsKey(e.Item.ImageKey) Then
                    If e.Item.ImageKey <> "" Then e.Graphics.DrawImageUnscaledAndClipped(TIconSmallList.Images(e.Item.ImageKey), rct)
                    'End If
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
                    End Select
                    If wd2 - iSize - 5 > 0 Then
                        Dim sRct As New Rectangle(x + 5 + iSize, e.Item.Bounds.Y, wd2 - iSize - 5, e.Item.Font.Height)
                        Dim sRct2 As New Rectangle(x + 5 + iSize, e.Item.Bounds.Y + e.Item.Font.Height, wd2 - iSize - 5, iSize - e.Item.Font.Height)
                        Dim fnt As New Font(e.Item.Font, FontStyle.Bold)
                        e.Graphics.DrawString(e.Item.SubItems(1).Text + "(" + e.Item.SubItems(4).Text + ") " + e.Item.SubItems(0).Text + " " + e.Item.SubItems(3).Text, fnt, brs, sRct, sf)
                        e.Graphics.DrawString(e.Item.SubItems(2).Text, e.Item.Font, brs, sRct2, sf)
                    End If
                End If
            End If
            e.DrawFocusRectangle()
        Else
            e.DrawDefault = True
        End If
#If DEBUG Then
        _drawcount += 1
        _drawtime += System.Environment.TickCount - iStart
        System.Diagnostics.Debug.WriteLine("呼び出し回数" & _drawcount.ToString() & "total処理時間：" & _drawtime.ToString() & "ミリ秒")
#End If
    End Sub

    Private Sub MenuItemSubSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemSubSearch.Click
        Dim myList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        Dim _word As String
        Dim cidx As Integer = 0
        Dim fnd As Boolean = False
        Dim toIdx As Integer
        SearchDialog.Owner = Me
        If SearchDialog.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
            Me.TopMost = SettingDialog.AlwaysTop
            Exit Sub
        End If
        Me.TopMost = SettingDialog.AlwaysTop
        _word = SearchDialog.SWord

        If _word = "" Then Exit Sub

        If myList.SelectedItems.Count > 0 Then
            cidx = myList.SelectedItems(0).Index
        End If

        toIdx = myList.Items.Count - 1
RETRY:
        If SearchDialog.CheckSearchCaseSensitive.Checked Then
            If SearchDialog.CheckSearchRegex.Checked Then
                ' 正規表現検索（CaseSensitive）
                Dim _search As Regex
                Try
                    _search = New Regex(_word)
                    For idx As Integer = cidx To toIdx
                        If _search.IsMatch(myList.Items(idx).SubItems(1).Text) _
                            OrElse _search.IsMatch(myList.Items(idx).SubItems(2).Text) _
                            OrElse _search.IsMatch(myList.Items(idx).SubItems(4).Text) _
                        Then
                            For Each itm As ListViewItem In myList.SelectedItems
                                itm.Selected = False
                            Next
                            myList.Items(idx).Selected = True
                            myList.Items(idx).Focused = True
                            myList.EnsureVisible(idx)
                            Exit Sub
                        End If
                    Next
                Catch ex As ArgumentException
                    MsgBox("正規表現パターンが間違っています。", MsgBoxStyle.Critical)
                    Exit Sub
                End Try
            Else
                ' 通常検索（CaseSensitive）
                For idx As Integer = cidx To toIdx
                    If (myList.Items(idx).SubItems(1).Text + myList.Items(idx).SubItems(2).Text + myList.Items(idx).SubItems(4).Text).IndexOf(_word, StringComparison.CurrentCulture) > -1 Then
                        For Each itm As ListViewItem In myList.SelectedItems
                            itm.Selected = False
                        Next
                        myList.Items(idx).Selected = True
                        myList.Items(idx).Focused = True
                        myList.EnsureVisible(idx)
                        Exit Sub
                    End If
                Next
            End If
        Else
            If SearchDialog.CheckSearchRegex.Checked Then
                ' 正規表現検索（IgnoreCase）
                Try
                    For idx As Integer = cidx To toIdx
                        If Regex.IsMatch(myList.Items(idx).SubItems(1).Text, _word, RegexOptions.IgnoreCase) _
                            OrElse Regex.IsMatch(myList.Items(idx).SubItems(2).Text, _word, RegexOptions.IgnoreCase) _
                            OrElse Regex.IsMatch(myList.Items(idx).SubItems(4).Text, _word, RegexOptions.IgnoreCase) _
                        Then
                            For Each itm As ListViewItem In myList.SelectedItems
                                itm.Selected = False
                            Next
                            myList.Items(idx).Selected = True
                            myList.Items(idx).Focused = True
                            myList.EnsureVisible(idx)
                            Exit Sub
                        End If
                    Next
                Catch ex As ArgumentException
                    MsgBox("正規表現パターンが間違っています。", MsgBoxStyle.Critical)
                    Exit Sub
                End Try
            Else
                ' 通常検索（IgnoreCase）
                For idx As Integer = cidx To toIdx
                    If (myList.Items(idx).SubItems(1).Text + myList.Items(idx).SubItems(2).Text + myList.Items(idx).SubItems(4).Text).IndexOf(_word, StringComparison.CurrentCultureIgnoreCase) > -1 Then
                        For Each itm As ListViewItem In myList.SelectedItems
                            itm.Selected = False
                        Next
                        myList.Items(idx).Selected = True
                        myList.Items(idx).Focused = True
                        myList.EnsureVisible(idx)
                        Exit Sub
                    End If
                Next
            End If
        End If

        If Not fnd Then
            If cidx > 0 AndAlso toIdx > -1 Then
                toIdx = cidx
                cidx = 0
                fnd = True
                GoTo RETRY
                'End If
            End If
        End If

        MessageBox.Show("検索条件に一致するデータは見つかりません。", "検索", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub MenuItemSearchNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemSearchNext.Click
        Dim myList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        Dim _word As String
        Dim cidx As Integer = 0
        Dim fnd As Boolean = False
        Dim toIdx As Integer

        _word = SearchDialog.SWord

        If _word = "" Then
            If SearchDialog.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
                Me.TopMost = SettingDialog.AlwaysTop
                Exit Sub
            End If
            Me.TopMost = SettingDialog.AlwaysTop
            _word = SearchDialog.SWord
            If _word = "" Then Exit Sub
        End If

        If myList.SelectedItems.Count > 0 Then
            cidx = myList.SelectedItems(0).Index + 1
        End If

        toIdx = myList.Items.Count - 1
RETRY:
        If SearchDialog.CheckSearchCaseSensitive.Checked Then
            If SearchDialog.CheckSearchRegex.Checked Then
                ' 正規表現検索（CaseSensitive）
                Dim _search As Regex
                Try
                    _search = New Regex(_word)
                    For idx As Integer = cidx To toIdx
                        If _search.IsMatch(myList.Items(idx).SubItems(1).Text) _
                            OrElse _search.IsMatch(myList.Items(idx).SubItems(2).Text) _
                            OrElse _search.IsMatch(myList.Items(idx).SubItems(4).Text) _
                        Then
                            For Each itm As ListViewItem In myList.SelectedItems
                                itm.Selected = False
                            Next
                            myList.Items(idx).Selected = True
                            myList.Items(idx).Focused = True
                            myList.EnsureVisible(idx)
                            Exit Sub
                        End If
                    Next
                Catch ex As ArgumentException
                    MsgBox("正規表現パターンが間違っています。", MsgBoxStyle.Critical)
                    Exit Sub
                End Try
            Else
                ' 通常検索（CaseSensitive）
                For idx As Integer = cidx To toIdx
                    If (myList.Items(idx).SubItems(1).Text + myList.Items(idx).SubItems(2).Text + myList.Items(idx).SubItems(4).Text).IndexOf(_word, StringComparison.CurrentCulture) > -1 Then
                        For Each itm As ListViewItem In myList.SelectedItems
                            itm.Selected = False
                        Next
                        myList.Items(idx).Selected = True
                        myList.Items(idx).Focused = True
                        myList.EnsureVisible(idx)
                        Exit Sub
                    End If
                Next
            End If
        Else
            If SearchDialog.CheckSearchRegex.Checked Then
                ' 正規表現検索（IgnoreCase）
                Try
                    For idx As Integer = cidx To toIdx
                        If Regex.IsMatch(myList.Items(idx).SubItems(1).Text, _word, RegexOptions.IgnoreCase) _
                            OrElse Regex.IsMatch(myList.Items(idx).SubItems(2).Text, _word, RegexOptions.IgnoreCase) _
                            OrElse Regex.IsMatch(myList.Items(idx).SubItems(4).Text, _word, RegexOptions.IgnoreCase) _
                        Then
                            For Each itm As ListViewItem In myList.SelectedItems
                                itm.Selected = False
                            Next
                            myList.Items(idx).Selected = True
                            myList.Items(idx).Focused = True
                            myList.EnsureVisible(idx)
                            Exit Sub
                        End If
                    Next
                Catch ex As ArgumentException
                    MsgBox("正規表現パターンが間違っています。", MsgBoxStyle.Critical)
                    Exit Sub
                End Try
            Else
                ' 通常検索（IgnoreCase）
                For idx As Integer = cidx To toIdx
                    If (myList.Items(idx).SubItems(1).Text + myList.Items(idx).SubItems(2).Text + myList.Items(idx).SubItems(4).Text).IndexOf(_word, StringComparison.CurrentCultureIgnoreCase) > -1 Then
                        For Each itm As ListViewItem In myList.SelectedItems
                            itm.Selected = False
                        Next
                        myList.Items(idx).Selected = True
                        myList.Items(idx).Focused = True
                        myList.EnsureVisible(idx)
                        Exit Sub
                    End If
                Next
            End If
        End If

        If Not fnd Then
            If cidx > 0 AndAlso toIdx > -1 Then
                'If MessageBox.Show("検索条件に一致するデータは見つかりません。" + vbCrLf + "もう一度先頭から検索しますか？", "検索", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Yes Then
                toIdx = cidx
                cidx = 0
                fnd = True
                GoTo RETRY
                'End If
            End If
        End If

        MessageBox.Show("検索条件に一致するデータは見つかりません。", "検索", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub MenuItemSearchPrev_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemSearchPrev.Click
        Dim myList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        Dim _word As String
        Dim cidx As Integer = 0
        Dim fnd As Boolean = False
        Dim toIdx As Integer

        _word = SearchDialog.SWord

        If _word = "" Then
            If SearchDialog.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
                Me.TopMost = SettingDialog.AlwaysTop
                Exit Sub
            End If
            Me.TopMost = SettingDialog.AlwaysTop
            _word = SearchDialog.SWord
            If _word = "" Then Exit Sub
        End If

        If myList.SelectedItems.Count > 0 Then
            cidx = myList.SelectedItems(0).Index - 1
        End If

        toIdx = 0
RETRY:
        If SearchDialog.CheckSearchCaseSensitive.Checked Then
            If SearchDialog.CheckSearchRegex.Checked Then
                ' 正規表現検索（CaseSensitive）
                Dim _search As Regex
                Try
                    _search = New Regex(_word)
                    If myList.Items.Count > 0 Then
                        For idx As Integer = cidx To toIdx Step -1
                            If _search.IsMatch(myList.Items(idx).SubItems(1).Text) _
                                OrElse _search.IsMatch(myList.Items(idx).SubItems(2).Text) _
                                OrElse _search.IsMatch(myList.Items(idx).SubItems(4).Text) _
                            Then
                                For Each itm As ListViewItem In myList.SelectedItems
                                    itm.Selected = False
                                Next
                                myList.Items(idx).Selected = True
                                myList.Items(idx).Focused = True
                                myList.EnsureVisible(idx)
                                Exit Sub
                            End If
                        Next
                    End If
                Catch Err As ArgumentException
                    MsgBox("正規表現パターンが間違っています。", MsgBoxStyle.Critical)
                    Exit Sub
                End Try
            Else
                ' 通常検索（CaseSensitive）
                If myList.Items.Count > 0 Then
                    For idx As Integer = cidx To toIdx Step -1
                        If (myList.Items(idx).SubItems(1).Text + myList.Items(idx).SubItems(2).Text + myList.Items(idx).SubItems(4).Text).IndexOf(_word) > -1 Then
                            For Each itm As ListViewItem In myList.SelectedItems
                                itm.Selected = False
                            Next
                            myList.Items(idx).Selected = True
                            myList.Items(idx).Focused = True
                            myList.EnsureVisible(idx)
                            Exit Sub
                        End If
                    Next
                End If
            End If
        Else
            If SearchDialog.CheckSearchRegex.Checked = True Then
                ' 正規表現検索（IgnoreCase）
                Try
                    If myList.Items.Count > 0 Then
                        For idx As Integer = cidx To toIdx Step -1
                            If Regex.IsMatch(myList.Items(idx).SubItems(1).Text, _word, RegexOptions.IgnoreCase) _
                                OrElse Regex.IsMatch(myList.Items(idx).SubItems(2).Text, _word, RegexOptions.IgnoreCase) _
                                OrElse Regex.IsMatch(myList.Items(idx).SubItems(4).Text, _word, RegexOptions.IgnoreCase) _
                            Then
                                For Each itm As ListViewItem In myList.SelectedItems
                                    itm.Selected = False
                                Next
                                myList.Items(idx).Selected = True
                                myList.Items(idx).Focused = True
                                myList.EnsureVisible(idx)
                                Exit Sub
                            End If
                        Next
                    End If
                Catch Err As ArgumentException
                    MsgBox("正規表現パターンが間違っています。", MsgBoxStyle.Critical)
                    Exit Sub
                End Try
            Else
                ' 通常検索（CaseSensitive）
                If myList.Items.Count > 0 Then
                    For idx As Integer = cidx To toIdx Step -1
                        If (myList.Items(idx).SubItems(1).Text + myList.Items(idx).SubItems(2).Text + myList.Items(idx).SubItems(4).Text).IndexOf(_word, StringComparison.CurrentCultureIgnoreCase) > -1 Then
                            For Each itm As ListViewItem In myList.SelectedItems
                                itm.Selected = False
                            Next
                            myList.Items(idx).Selected = True
                            myList.Items(idx).Focused = True
                            myList.EnsureVisible(idx)
                            Exit Sub
                        End If
                    Next
                End If
            End If
        End If



        If Not fnd Then
            If cidx > 0 AndAlso toIdx > -1 Then
                toIdx = cidx
                cidx = myList.Items.Count - 1
                fnd = True
                GoTo RETRY
                'End If
            End If
        End If

        MessageBox.Show("検索条件に一致するデータは見つかりません。", "検索", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub AboutMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutMenuItem.Click
        TweenAboutBox.ShowDialog()
        Me.TopMost = SettingDialog.AlwaysTop
    End Sub

    Private Sub JumpUnreadMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles JumpUnreadMenuItem.Click
        Dim myList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        Dim _itnm As String = ListTab.SelectedTab.Text
        Dim _ntnm As String = _itnm
        Dim cidx As Integer = 0
        Dim fnd As Boolean = True
        Dim toIdx As Integer
        Dim stp As Integer = 1
        Dim tabIdx As Integer = 0

        myList.Focus()
RETRY:
        If _itnm = _ntnm AndAlso Not fnd Then
            '全て既読の場合、Recentの最新発言を選択
            ListTab.SelectedIndex = 0
            If _tabs(0).listCustom.Items.Count > 0 Then
                '選択済みのものがあったら、選択状態クリア
                For Each itm As ListViewItem In _tabs(0).listCustom.SelectedItems
                    itm.Selected = False
                Next
                If listViewItemSorter.Column = 3 Then
                    If listViewItemSorter.Order = SortOrder.Ascending Then
                        _tabs(0).listCustom.Items(_tabs(0).listCustom.Items.Count - 1).Selected = True
                        _tabs(0).listCustom.Items(_tabs(0).listCustom.Items.Count - 1).Focused = True
                        _tabs(0).listCustom.EnsureVisible(_tabs(0).listCustom.Items.Count - 1)
                    Else
                        _tabs(0).listCustom.Items(0).Selected = True
                        _tabs(0).listCustom.Items(0).Focused = True
                        _tabs(0).listCustom.EnsureVisible(0)
                    End If
                Else
                    _tabs(0).listCustom.Items(_tabs(0).listCustom.Items.Count - 1).Selected = True
                    _tabs(0).listCustom.Items(_tabs(0).listCustom.Items.Count - 1).Focused = True
                    _tabs(0).listCustom.EnsureVisible(_tabs(0).listCustom.Items.Count - 1)
                End If
            End If
            Exit Sub
        End If
        fnd = False

        For Each ts As TabStructure In _tabs
            If ts.listCustom.Equals(myList) Then
                If ts.unreadCount = 0 Then Exit For
                If listViewItemSorter.Column = 3 Then
                    If listViewItemSorter.Order = SortOrder.Ascending Then
                        If ts.oldestUnreadItem Is Nothing Then
                            cidx = 0
                        Else
                            cidx = ts.oldestUnreadItem.Index
                        End If
                        toIdx = myList.Items.Count - 1
                        stp = 1
                    Else
                        If ts.oldestUnreadItem Is Nothing Then
                            cidx = myList.Items.Count - 1
                        Else
                            cidx = ts.oldestUnreadItem.Index
                        End If
                        toIdx = 0
                        stp = -1
                    End If
                Else
                    cidx = 0
                    toIdx = myList.Items.Count - 1
                    stp = 1
                End If

                For idx As Integer = cidx To toIdx Step stp
                    If myList.Items(idx).SubItems(8).Text = "False" Then
                        If ListTab.SelectedTab.Text <> _ntnm Then
                            For i As Integer = 0 To ListTab.TabPages.Count - 1
                                If ListTab.TabPages(i).Text = _ntnm Then
                                    ListTab.SelectedIndex = i
                                    Exit For
                                End If
                            Next
                        End If
                        For Each itm As ListViewItem In myList.SelectedItems
                            itm.Selected = False
                        Next
                        myList.Items(idx).Selected = True
                        myList.Items(idx).Focused = True

                        Dim _item As ListViewItem
                        Dim idx1 As Integer
                        Dim idx2 As Integer
                        _item = myList.GetItemAt(0, 25)
                        If _item Is Nothing Then _item = myList.Items(0)
                        idx1 = _item.Index
                        _item = myList.GetItemAt(0, myList.ClientSize.Height - 1)
                        If _item Is Nothing Then _item = myList.Items(myList.Items.Count - 1)
                        idx2 = _item.Index
                        If idx <= idx1 OrElse idx >= idx2 Then
                            MoveTop()
                        End If
                        Exit Sub
                    End If
                Next
                Exit For
            End If
        Next

        If Not fnd Then
            For i As Integer = 0 To ListTab.TabPages.Count - 1
                If ListTab.TabPages(i).Text = _ntnm Then
                    If i = ListTab.TabPages.Count - 1 Then
                        i = 0
                    Else
                        i += 1
                    End If
                    myList = DirectCast(ListTab.TabPages(i).Controls(0), DetailsListView)
                    _ntnm = ListTab.TabPages(i).Text
                    GoTo RETRY
                End If
            Next
        End If
    End Sub

    Private Sub StatusOpenMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusOpenMenuItem.Click
        Do While ExecWorker.IsBusy
            Threading.Thread.Sleep(1)
            Application.DoEvents()
        Loop
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)

        '後でタブ追加して独自読み込みにする
        If MyList.SelectedItems.Count > 0 Then
            ExecWorker.RunWorkerAsync("http://twitter.com/" + MyList.SelectedItems(0).SubItems(4).Text + "/statuses/" + MyList.SelectedItems(0).SubItems(5).Text)
        End If
    End Sub

    Private Sub FavorareMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FavorareMenuItem.Click
        Do While ExecWorker.IsBusy
            Threading.Thread.Sleep(1)
            Application.DoEvents()
        Loop

        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)

        '後でタブ追加して独自読み込みにする
        If MyList.SelectedItems.Count > 0 Then
            ExecWorker.RunWorkerAsync("http://favotter.matope.com/user.php?user=" + MyList.SelectedItems(0).SubItems(4).Text)
        End If
    End Sub

    Private Sub ChangeImageSize()
        Dim sz As Integer = DirectCast(IIf(_iconSz = 0, 16, _iconSz), Integer)

        If TIconSmallList IsNot Nothing Then
            TIconSmallList.Dispose()
        End If
        TIconSmallList = New ImageList
        TIconSmallList.ImageSize = New Size(sz, sz)
        TIconSmallList.ColorDepth = ColorDepth.Depth32Bit

        If _iconSz = 0 Then Exit Sub

        If _iconSz <> 48 Then
            For Each key As String In TIconList.Images.Keys
                Dim img2 As New Bitmap(sz, sz)
                Dim g As Graphics = Graphics.FromImage(img2)

                g.InterpolationMode = Drawing2D.InterpolationMode.Default
                g.DrawImage(TIconList.Images(key), 0, 0, sz, sz)
                TIconSmallList.Images.Add(key, img2)
            Next
        End If
    End Sub

    Private Sub VerUpMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VerUpMenuItem.Click
        CheckNewVersion()
    End Sub

    Private Sub CheckNewVersion(Optional ByVal startup As Boolean = False)
        Dim retMsg As String
        Dim resStatus As String = ""
        Dim strVer As String
        Dim forceUpdate As Boolean = My.Computer.Keyboard.ShiftKeyDown

        retMsg = clsTwSync.GetVersionInfo()
        If retMsg.Length > 0 Then
            strVer = retMsg.Substring(0, 4)
            If strVer.CompareTo(My.Application.Info.Version.ToString.Replace(".", "")) > 0 Then
                If MessageBox.Show("新しいバージョン " + strVer + " が公開されています。更新しますか？", "Tween更新確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    retMsg = clsTwSync.GetTweenBinary(strVer)
                    If retMsg.Length = 0 Then
                        retMsg = clsTwSync.GetTweenUpBinary()
                        If retMsg.Length = 0 Then
                            System.Diagnostics.Process.Start(My.Application.Info.DirectoryPath + "\TweenUp.exe")
                            Application.Exit()
                            Exit Sub
                        Else
                            If Not startup Then MessageBox.Show("アップデーターのダウンロードに失敗しました。しばらく待ってから再度お試しください。", "Tween更新結果", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        End If
                    Else
                        If Not startup Then MessageBox.Show("最新版が公開されていますが、ダウンロードに失敗しました。しばらく待ってから再度お試しください。", "Tween更新結果", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If
                End If
            Else
                If forceUpdate Then
                    If MessageBox.Show("新しいバージョンは見つかりません。 " + strVer + " が公開されています。強制的に更新しますか？", "Tween更新確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        retMsg = clsTwSync.GetTweenBinary(strVer)
                        If retMsg.Length = 0 Then
                            retMsg = clsTwSync.GetTweenUpBinary()
                            If retMsg.Length = 0 Then
                                System.Diagnostics.Process.Start(My.Application.Info.DirectoryPath + "\TweenUp.exe")
                                Application.Exit()
                                Exit Sub
                            Else
                                If Not startup Then MessageBox.Show("アップデーターのダウンロードに失敗しました。しばらく待ってから再度お試しください。", "Tween更新結果", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            End If
                        Else
                            If Not startup Then MessageBox.Show("最新版が公開されていますが、ダウンロードに失敗しました。しばらく待ってから再度お試しください。", "Tween更新結果", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        End If
                    End If
                ElseIf Not startup Then
                    MessageBox.Show("最新版をお使いです。更新の必要はありませんでした。使用中Ver：" + My.Application.Info.Version.ToString.Replace(".", "") + " 最新Ver：" + strVer, "Tween更新結果", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
        Else
            StatusLabel.Text = "バージョンチェック失敗"
            If Not startup Then MessageBox.Show("更新版のバージョン取得に失敗しました。しばらく待ってから再度お試しください。", "Tween更新結果", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub TimerColorize_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerColorize.Tick
        If TimerColorize.Enabled = False Then Exit Sub
        TimerColorize.Stop()
        TimerColorize.Enabled = False
        TimerColorize.Interval = 100
        ColorizeList(False)
        DispSelectedPost()
        '件数関連の場合、タイトル即時書き換え
        If SettingDialog.DispLatestPost <> DispTitleEnum.None AndAlso _
           SettingDialog.DispLatestPost <> DispTitleEnum.Post AndAlso _
           SettingDialog.DispLatestPost <> DispTitleEnum.Ver Then
            SetMainWindowTitle()
        End If
        If Not StatusLabelUrl.Text.StartsWith("http") Then SetStatusLabel()
    End Sub

    Private Sub DispSelectedPost()
        Dim _item As ListViewItem
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        Dim dTxt As String

        If MyList.SelectedItems.Count = 0 Then Exit Sub

        _item = MyList.SelectedItems(0)
        dTxt = "<html><head><style type=""text/css"">p {font-family: """ + _fntDetail.Name + """, sans-serif; font-size: " + _fntDetail.Size.ToString + "pt;} --></style></head><body style=""margin:0px""><p>" + _item.SubItems(7).Text + "</p></body></html>"
        NameLabel.Text = _item.SubItems(1).Text + "/" + _item.SubItems(4).Text
        UserPicture.Image = TIconList.Images(_item.SubItems(6).Text)
        NameLabel.Text = _item.SubItems(1).Text + "/" + _item.SubItems(4).Text

        NameLabel.ForeColor = System.Drawing.SystemColors.ControlText
        DateTimeLabel.Text = _item.SubItems(3).Text.ToString()
        If _item.SubItems(10).Text = "True" AndAlso (SettingDialog.OneWayLove OrElse ListTab.SelectedTab.Text = "Direct") Then NameLabel.ForeColor = _clOWL
        If _item.SubItems(9).Text = "True" Then NameLabel.ForeColor = _clFav

        If PostBrowser.DocumentText <> dTxt Then
            PostBrowser.Visible = False
            PostBrowser.DocumentText = dTxt
            PostBrowser.Visible = True
        End If
    End Sub

    Private Sub MatomeMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MatomeMenuItem.Click
        Do While ExecWorker.IsBusy
            Threading.Thread.Sleep(1)
            Application.DoEvents()
        Loop

        ExecWorker.RunWorkerAsync("http://www5.atwiki.jp/tween/")
    End Sub

    Private Sub OfficialMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OfficialMenuItem.Click
        Do While ExecWorker.IsBusy
            Threading.Thread.Sleep(1)
            Application.DoEvents()
        Loop

        ExecWorker.RunWorkerAsync("http://d.hatena.ne.jp/Kiri_Feather/20071121")
    End Sub

    Private Sub DLPageMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DLPageMenuItem.Click
        Do While ExecWorker.IsBusy
            Threading.Thread.Sleep(1)
            Application.DoEvents()
        Loop

        ExecWorker.RunWorkerAsync("http://www.asahi-net.or.jp/~ne5h-ykmz/index.html")
    End Sub

    Private Sub ListTab_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ListTab.KeyDown
        If e.Modifiers = Keys.None Then
            ' ModifierKeyが押されていない場合
            If e.KeyCode = Keys.N OrElse e.KeyCode = Keys.Right Then
                e.Handled = True
                e.SuppressKeyPress = True
                GoNextRelPost()
                Exit Sub
            End If
            If e.KeyCode = Keys.P OrElse e.KeyCode = Keys.Left Then
                e.Handled = True
                e.SuppressKeyPress = True
                GoPreviousRelPost()
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
                GoNextPost()
            End If
            If e.KeyCode = Keys.H Then
                e.Handled = True
                e.SuppressKeyPress = True
                GoPreviousPost()
            End If
            If e.KeyCode = Keys.Z Or e.KeyCode = Keys.Oemcomma Then
                e.Handled = True
                e.SuppressKeyPress = True
                MoveTop()
            End If
        End If
        _anchorFlag = False
        If e.Control AndAlso Not e.Alt AndAlso Not e.Shift Then
            ' CTRLキーが押されている場合
            If e.KeyCode = Keys.Home OrElse e.KeyCode = Keys.End Then
                'ColorizeList(False)
                TimerColorize.Stop()
                TimerColorize.Start()
            End If
            If e.KeyCode = Keys.A Then
                Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
                For Each lItem As ListViewItem In MyList.Items
                    lItem.Selected = True
                Next
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
                GoNextFav()
            End If
            If e.KeyCode = Keys.P OrElse e.KeyCode = Keys.Left Then
                e.Handled = True
                e.SuppressKeyPress = True
                GoPreviousFav()
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
            Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
            Dim clstr As String = ""
            If e.Control AndAlso Not e.Alt AndAlso Not e.Shift Then
                e.Handled = True
                e.SuppressKeyPress = True
                For Each itm As ListViewItem In MyList.SelectedItems
                    If clstr <> "" Then clstr += vbCrLf
                    clstr += itm.SubItems(4).Text + ":" + itm.SubItems(2).Text + " [http://twitter.com/" + itm.SubItems(4).Text + "/statuses/" + itm.SubItems(5).Text + "]"
                Next
            End If
            If e.Control AndAlso e.Shift AndAlso Not e.Alt Then
                e.Handled = True
                e.SuppressKeyPress = True
                For Each itm As ListViewItem In MyList.SelectedItems
                    If clstr <> "" Then clstr += vbCrLf
                    clstr += "http://twitter.com/" + itm.SubItems(4).Text + "/statuses/" + itm.SubItems(5).Text
                Next
            End If
            If clstr <> "" Then
                Dim i As Integer = 0
RETRY:
                Try
                    Clipboard.Clear()
                    Clipboard.SetText(clstr)
                Catch ex As Exception
                    i += 1
                    If i < 3 Then
                        System.Threading.Thread.Sleep(500)
                        My.Application.DoEvents()
                        GoTo RETRY
                    End If
                End Try
            End If
        End If
    End Sub

    Private Sub GoNextFav()
        TimerColorize.Stop()
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        If MyList.SelectedItems.Count = 0 Then Exit Sub

        Dim user As String = MyList.SelectedItems(0).SubItems(4).Text
        Dim fIdx As Integer = MyList.SelectedItems(0).Index + 1
        If fIdx > MyList.Items.Count - 1 Then Exit Sub

        For idx As Integer = fIdx To MyList.Items.Count - 1
            If MyList.Items(idx).SubItems(9).Text = "True" Then
                For Each itm As ListViewItem In MyList.SelectedItems
                    itm.Selected = False
                Next
                MyList.Items(idx).Selected = True
                MyList.Items(idx).Focused = True
                MyList.EnsureVisible(idx)
                MyList.Update()
                TimerColorize.Start()
                Exit For
            End If
        Next
    End Sub

    Private Sub GoPreviousFav()
        TimerColorize.Stop()
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        If MyList.SelectedItems.Count = 0 Then Exit Sub

        Dim user As String = MyList.SelectedItems(0).SubItems(4).Text
        Dim fIdx As Integer = MyList.SelectedItems(0).Index - 1
        If fIdx < 0 Then Exit Sub

        For idx As Integer = fIdx To 0 Step -1
            If MyList.Items(idx).SubItems(9).Text = "True" Then
                For Each itm As ListViewItem In MyList.SelectedItems
                    itm.Selected = False
                Next
                MyList.Items(idx).Selected = True
                MyList.Items(idx).Focused = True
                MyList.EnsureVisible(idx)
                MyList.Update()
                TimerColorize.Start()
                Exit For
            End If
        Next
    End Sub

    Private Sub GoNextPost()
        TimerColorize.Stop()
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        If MyList.SelectedItems.Count = 0 Then Exit Sub

        Dim user As String = MyList.SelectedItems(0).SubItems(4).Text
        Dim fIdx As Integer = MyList.SelectedItems(0).Index + 1
        If fIdx > MyList.Items.Count - 1 Then Exit Sub

        For idx As Integer = fIdx To MyList.Items.Count - 1
            If MyList.Items(idx).SubItems(4).Text = user Then
                For Each itm As ListViewItem In MyList.SelectedItems
                    itm.Selected = False
                Next
                MyList.Items(idx).Selected = True
                MyList.Items(idx).Focused = True
                MyList.EnsureVisible(idx)
                MyList.Update()
                TimerColorize.Start()
                Exit For
            End If
        Next
    End Sub

    Private Sub GoPreviousPost()
        TimerColorize.Stop()
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        If MyList.SelectedItems.Count = 0 Then Exit Sub

        Dim user As String = MyList.SelectedItems(0).SubItems(4).Text
        Dim fIdx As Integer = MyList.SelectedItems(0).Index - 1
        If fIdx < 0 Then Exit Sub

        For idx As Integer = fIdx To 0 Step -1
            If MyList.Items(idx).SubItems(4).Text = user Then
                For Each itm As ListViewItem In MyList.SelectedItems
                    itm.Selected = False
                Next
                MyList.Items(idx).Selected = True
                MyList.Items(idx).Focused = True
                MyList.EnsureVisible(idx)
                MyList.Update()
                TimerColorize.Start()
                Exit For
            End If
        Next
    End Sub

    Private Sub GoNextRelPost()
        TimerColorize.Stop()
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        If MyList.SelectedItems.Count = 0 Then Exit Sub

        Dim fIdx As Integer = MyList.SelectedItems(0).Index + 1
        If fIdx > MyList.Items.Count - 1 Then Exit Sub

        If _anchorFlag = False Then
            _anchorItem = Nothing
            _anchorItem = MyList.SelectedItems(0)
            _anchorFlag = True
        End If
        Dim user As String = _anchorItem.SubItems(4).Text
        Dim dTxt As String = _anchorItem.SubItems(7).Text
        Dim at As New Collections.Specialized.StringCollection
        Dim pos1 As Integer
        Dim pos2 As Integer

        Do While True
            pos1 = dTxt.IndexOf(_replyHtml, pos2)
            If pos1 = -1 Then Exit Do
            pos2 = dTxt.IndexOf(""">", pos1 + _replyHtml.Length)
            If pos2 > -1 Then
                at.Add(dTxt.Substring(pos1 + _replyHtml.Length, pos2 - pos1 - _replyHtml.Length))
            End If
        Loop

        For idx As Integer = fIdx To MyList.Items.Count - 1
            If MyList.Items(idx).SubItems(4).Text = user OrElse _
               at.Contains(MyList.Items(idx).SubItems(4).Text) OrElse _
               Regex.IsMatch(MyList.Items(idx).SubItems(2).Text, "@" + user + "([^a-zA-Z0-9_]|$)") Then
                For Each itm As ListViewItem In MyList.SelectedItems
                    itm.Selected = False
                Next
                MyList.Items(idx).Selected = True
                MyList.Items(idx).Focused = True
                MyList.EnsureVisible(idx)
                MyList.Update()
                TimerColorize.Start()
                Exit For
            End If
        Next
        at.Clear()
    End Sub

    Private Sub GoPreviousRelPost()
        TimerColorize.Stop()
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        If MyList.SelectedItems.Count = 0 Then Exit Sub


        Dim fIdx As Integer = MyList.SelectedItems(0).Index - 1
        If fIdx < 0 Then Exit Sub

        If Not _anchorFlag Then
            _anchorItem = Nothing
            _anchorItem = DirectCast(MyList.SelectedItems(0).Clone, System.Windows.Forms.ListViewItem)
            _anchorFlag = True
        End If
        Dim user As String = _anchorItem.SubItems(4).Text
        Dim dTxt As String = _anchorItem.SubItems(7).Text
        Dim at As New Collections.Specialized.StringCollection
        Dim pos1 As Integer
        Dim pos2 As Integer

        Do While True
            pos1 = dTxt.IndexOf(_replyHtml, pos2)
            If pos1 = -1 Then Exit Do
            pos2 = dTxt.IndexOf(""">", pos1 + _replyHtml.Length)
            If pos2 > -1 Then
                at.Add(dTxt.Substring(pos1 + _replyHtml.Length, pos2 - pos1 - _replyHtml.Length))
            End If
        Loop

        For idx As Integer = fIdx To 0 Step -1
            If MyList.Items(idx).SubItems(4).Text = user OrElse _
               at.Contains(MyList.Items(idx).SubItems(4).Text) OrElse _
               Regex.IsMatch(MyList.Items(idx).SubItems(2).Text, "@" + user + "([^a-zA-Z0-9_]|$)") Then
                For Each itm As ListViewItem In MyList.SelectedItems
                    itm.Selected = False
                Next
                MyList.Items(idx).Selected = True
                MyList.Items(idx).Focused = True
                MyList.EnsureVisible(idx)
                MyList.Update()
                TimerColorize.Start()
                Exit For
            End If
        Next
        at.Clear()
    End Sub

    Private Sub GoAnchor()
        TimerColorize.Stop()
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        If _anchorItem Is Nothing Then Exit Sub

        Dim aid As String = _anchorItem.SubItems(5).Text

        For idx As Integer = 0 To MyList.Items.Count - 1
            If MyList.Items(idx).SubItems(5).Text = aid Then
                For Each itm As ListViewItem In MyList.SelectedItems
                    itm.Selected = False
                Next
                MyList.Items(idx).Selected = True
                MyList.Items(idx).Focused = True
                MyList.EnsureVisible(idx)
                MyList.Update()
                TimerColorize.Start()
                Exit For
            End If
        Next
    End Sub

    Private Sub GoTopEnd(ByVal GoTop As Boolean)
        TimerColorize.Stop()
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        Dim _item As ListViewItem

        If GoTop Then
            _item = MyList.GetItemAt(0, 25)
            If _item Is Nothing Then _item = MyList.Items(0)
        Else
            _item = MyList.GetItemAt(0, MyList.ClientSize.Height - 1)
            If _item Is Nothing Then _item = MyList.Items(MyList.Items.Count - 1)
        End If
        For Each _itm As ListViewItem In MyList.SelectedItems
            _itm.Selected = False
        Next
        _item.Selected = True
        _item.Focused = True
        MyList.Update()
        TimerColorize.Start()

    End Sub

    Private Sub GoMiddle()
        TimerColorize.Stop()
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        Dim _item As ListViewItem
        Dim idx1 As Integer
        Dim idx2 As Integer

        _item = MyList.GetItemAt(0, 0)
        If _item Is Nothing Then _item = MyList.Items(0)
        idx1 = _item.Index
        _item = MyList.GetItemAt(0, MyList.ClientSize.Height - 1)
        If _item Is Nothing Then _item = MyList.Items(MyList.Items.Count - 1)
        idx2 = _item.Index
        _item = MyList.Items((idx1 + idx2) \ 2)

        For Each _itm As ListViewItem In MyList.SelectedItems
            _itm.Selected = False
        Next
        _item.Selected = True
        _item.Focused = True
        MyList.Update()
        TimerColorize.Start()

    End Sub

    Private Sub GoLast()
        TimerColorize.Stop()
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        Dim _item As ListViewItem
        If listViewItemSorter.Column = 3 Then
            If listViewItemSorter.Order = SortOrder.Ascending Then
                _item = MyList.Items(MyList.Items.Count - 1)
                MyList.EnsureVisible(MyList.Items.Count - 1)
            Else
                _item = MyList.Items(0)
                MyList.EnsureVisible(0)
            End If
        Else
            _item = MyList.Items(MyList.Items.Count - 1)
            MyList.EnsureVisible(MyList.Items.Count - 1)
        End If

        For Each _itm As ListViewItem In MyList.SelectedItems
            _itm.Selected = False
        Next
        _item.Selected = True
        _item.Focused = True

        MyList.Update()
        TimerColorize.Start()

    End Sub

    Private Sub MoveTop()
        TimerColorize.Stop()
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        If MyList.SelectedItems.Count = 0 Then Exit Sub
        Dim _item As ListViewItem = MyList.SelectedItems(0)
        If listViewItemSorter.Column = 3 Then
            If listViewItemSorter.Order = SortOrder.Ascending Then
                MyList.EnsureVisible(MyList.Items.Count - 1)
            Else
                MyList.EnsureVisible(0)
            End If
        Else
            MyList.EnsureVisible(MyList.Items.Count - 1)
        End If
        MyList.EnsureVisible(_item.Index)
        MyList.Update()
        TimerColorize.Start()

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
            End If
        End If
    End Sub

    Private Sub SaveConfigs()
        If _username <> "" AndAlso _password <> "" Then
            SyncLock _syncObject
                _section.FormSize = _mySize
                _section.FormLocation = _myLoc
                _section.SplitterDistance = _mySpDis
                _section.UserName = _username
                _section.Password = _password
                _section.NextPageThreshold = SettingDialog.NextPageThreshold
                _section.NextPages = SettingDialog.NextPagesInt
                _section.TimelinePeriod = SettingDialog.TimelinePeriodInt
                _section.DMPeriod = SettingDialog.DMPeriodInt
                _section.MaxPostNum = SettingDialog.MaxPostNum
                _section.ReadPages = SettingDialog.ReadPages
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

                _section.DisplayIndex1 = _tabs(0).colHd1.DisplayIndex
                _section.Width1 = _tabs(0).colHd1.Width
                If _iconCol = False Then
                    _section.DisplayIndex2 = _tabs(0).colHd2.DisplayIndex
                    _section.DisplayIndex3 = _tabs(0).colHd3.DisplayIndex
                    _section.DisplayIndex4 = _tabs(0).colHd4.DisplayIndex
                    _section.DisplayIndex5 = _tabs(0).colHd5.DisplayIndex
                    _section.Width2 = _tabs(0).colHd2.Width
                    _section.Width3 = _tabs(0).colHd3.Width
                    _section.Width4 = _tabs(0).colHd4.Width
                    _section.Width5 = _tabs(0).colHd5.Width
                End If
                _section.SortColumn = listViewItemSorter.Column
                _section.SortOrder = listViewItemSorter.Order

                _section.ListElement.Clear()

                Dim cnt As Integer = 0
                If ListTab IsNot Nothing AndAlso _
                   ListTab.TabPages IsNot Nothing AndAlso _
                   ListTab.TabPages.Count > 0 Then
                    _section.SelectedUser.Clear()
                    For Each tp As TabPage In ListTab.TabPages
                        Dim tabName As String = tp.Text
                        _section.ListElement.Add(New ListElement(tabName))
                        For Each ts As TabStructure In _tabs
                            If ts.tabName = tabName Then
                                _section.ListElement(tabName).Notify = ts.notify
                                _section.ListElement(tabName).SoundFile = ts.soundFile
                                _section.ListElement(tabName).UnreadManage = ts.unreadManage
                                For Each fc As FilterClass In ts.filters
                                    Dim bf As String = ""
                                    For Each bfs As String In fc.BodyFilter
                                        bf += " " + bfs
                                    Next
                                    Dim su As New SelectedUser(cnt.ToString)
                                    cnt += 1
                                    su.BodyFilter = bf
                                    su.IdFilter = fc.IDFilter
                                    su.MoveFrom = fc.moveFrom
                                    su.SetMark = fc.SetMark
                                    su.SearchBoth = fc.SearchBoth
                                    su.UrlSearch = fc.SearchURL
                                    su.RegexEnable = fc.UseRegex
                                    su.TabName = tabName
                                    _section.SelectedUser.Add(su)
                                Next
                            End If
                        Next
                    Next
                End If

            End SyncLock

            _config.Save()
        End If
    End Sub

    Private Sub SaveLogMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveLogMenuItem.Click
        Dim rslt As DialogResult = MessageBox.Show("選択中タブの全発言を保存しますか？" + vbCrLf + _
                "　「はい」　　　：全発言を保存する" + vbCrLf + _
                "　「いいえ」　　：選択している発言のみ保存する" + vbCrLf + _
                "　「キャンセル」：保存処理をキャンセル" + vbCrLf + _
                "（タブ区切りのテキストファイル形式で保存します）", _
                "保存対象選択", _
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
        If rslt = Windows.Forms.DialogResult.Cancel Then Exit Sub

        SaveFileDialog1.FileName = "TweenPosts" + Format(Now, "yyMMdd-HHmmss") + ".tsv"
        SaveFileDialog1.InitialDirectory = My.Application.Info.DirectoryPath
        SaveFileDialog1.Filter = "TSVファイル(*.tsv)|*.tsv|すべてのファイル(*.*)|*.*"
        SaveFileDialog1.FilterIndex = 0
        SaveFileDialog1.Title = "保存先のファイルを選択してください"
        SaveFileDialog1.RestoreDirectory = True

        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            If Not SaveFileDialog1.ValidateNames Then Exit Sub
            Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
            Using sw As StreamWriter = New StreamWriter(SaveFileDialog1.FileName, False, Encoding.UTF8)
                If rslt = Windows.Forms.DialogResult.Yes Then
                    'All
                    For Each itm As ListViewItem In MyList.Items
                        sw.WriteLine(itm.SubItems(1).Text & vbTab & _
                                 """" & itm.SubItems(2).Text.Replace(vbLf, "").Replace("""", """""") + """" & vbTab & _
                                 itm.SubItems(3).Text & vbTab & _
                                 itm.SubItems(4).Text & vbTab & _
                                 itm.SubItems(5).Text & vbTab & _
                                 itm.SubItems(6).Text & vbTab & _
                                 """" & itm.SubItems(7).Text.Replace(vbLf, "").Replace("""", """""") + """")
                    Next
                Else
                    For Each itm As ListViewItem In MyList.SelectedItems
                        sw.WriteLine(itm.SubItems(1).Text & vbTab & _
                                 """" & itm.SubItems(2).Text.Replace(vbLf, "").Replace("""", """""") + """" & vbTab & _
                                 itm.SubItems(3).Text & vbTab & _
                                 itm.SubItems(4).Text & vbTab & _
                                 itm.SubItems(5).Text & vbTab & _
                                 itm.SubItems(6).Text & vbTab & _
                                 """" & itm.SubItems(7).Text.Replace(vbLf, "").Replace("""", """""") + """")
                    Next
                End If
            End Using
        End If
        Me.TopMost = SettingDialog.AlwaysTop
    End Sub

    Private Sub PostBrowser_PreviewKeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles PostBrowser.PreviewKeyDown
        If e.KeyCode = Keys.F5 Then
            e.IsInputKey = True
        End If
        If e.Modifiers = Keys.None AndAlso (e.KeyCode = Keys.Space OrElse e.KeyCode = Keys.ProcessKey) Then
            e.IsInputKey = True
            JumpUnreadMenuItem_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub Tabs_DoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListTab.MouseDoubleClick
        If ListTab.SelectedTab.Text = "Recent" OrElse ListTab.SelectedTab.Text = "Reply" OrElse ListTab.SelectedTab.Text = "Direct" Then Exit Sub
        Dim inputName As New InputTabName()
        inputName.TabName = ListTab.SelectedTab.Text
        inputName.ShowDialog()
        Dim newTabText As String = inputName.TabName
        inputName.Dispose()
        Me.TopMost = SettingDialog.AlwaysTop
        If newTabText <> "" Then
            For i As Integer = 0 To _tabs.Count - 1
                If _tabs(i).tabName = ListTab.SelectedTab.Text Then
                    Dim _ts As TabStructure = _tabs(i)
                    TabDialog.RemoveTab(ListTab.SelectedTab.Text)
                    _tabs.Remove(_tabs(i))
                    _ts.tabName = newTabText
                    ListTab.SelectedTab.Text = newTabText
                    _tabs.Add(_ts)
                    Exit For
                End If
            Next
            For i As Integer = 0 To ListTab.TabCount - 1
                If ListTab.TabPages(i).Text <> "Recent" AndAlso _
                   ListTab.TabPages(i).Text <> "Reply" AndAlso _
                   ListTab.TabPages(i).Text <> "Direct" AndAlso _
                   ListTab.TabPages(i).Text <> newTabText Then
                    TabDialog.RemoveTab(ListTab.TabPages(i).Text)
                End If
            Next
            For i As Integer = 0 To ListTab.TabCount - 1
                If ListTab.TabPages(i).Text <> "Recent" AndAlso _
                   ListTab.TabPages(i).Text <> "Reply" AndAlso _
                   ListTab.TabPages(i).Text <> "Direct" Then
                    TabDialog.AddTab(ListTab.TabPages(i).Text)
                End If
            Next
        End If
    End Sub

    Private Sub Tabs_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListTab.MouseDown
        Dim cpos As New Point(e.X, e.Y)
        Dim spos As Point = ListTab.PointToClient(cpos)
        If e.Button = Windows.Forms.MouseButtons.Left Then
            For i As Integer = 0 To ListTab.TabPages.Count - 1
                Dim rect As Rectangle = ListTab.GetTabRect(i)
                If rect.Left <= cpos.X AndAlso cpos.X <= rect.Right AndAlso _
                   rect.Top <= cpos.Y AndAlso cpos.Y <= rect.Bottom Then
                    If i < 3 Then
                        _tabDrag = False
                    Else
                        _tabDrag = True
                    End If
                    Exit For
                End If
            Next
        Else
            _tabDrag = False
        End If
    End Sub

    Private Sub Tabs_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles ListTab.DragEnter
        If e.Data.GetDataPresent(GetType(TabStructure)) Then
            e.Effect = DragDropEffects.Move
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub Tabs_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles ListTab.DragDrop
        If e.Data.GetDataPresent(GetType(TabStructure)) Then
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
            'Recent,Reply,Directタブは固定
            If tn = "Recent" OrElse tn = "Reply" OrElse tn = "Direct" Then
                tn = "Direct"
                bef = False
                i = 2
            End If

            Dim ts As TabStructure = DirectCast(e.Data.GetData(GetType(TabStructure)), Tween.TabStructure)

            If ts.tabName = tn Then Exit Sub

            Dim mTp As TabPage = Nothing
            ListTab.SuspendLayout()
            For j As Integer = 0 To ListTab.TabPages.Count - 1
                If ListTab.TabPages(j).Text = ts.tabName Then
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
        End If
    End Sub

    Private Sub MakeReplyOrDirectStatus(Optional ByVal isAuto As Boolean = True, Optional ByVal isReply As Boolean = True, Optional ByVal isAll As Boolean = False)
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)

        If Not StatusText.Enabled Then Exit Sub

        ' 複数あてリプライはReplyではなく通常ポスト

        If MyList.SelectedItems.Count > 0 Then
            ' アイテムが1件以上選択されている
            If MyList.SelectedItems.Count = 1 AndAlso Not isAll Then
                ' 単独ユーザー宛リプライまたはDM
                If (ListTab.SelectedTab.Text = "Direct" AndAlso isAuto) OrElse (Not isAuto AndAlso Not isReply) Then
                    ' ダイレクトメッセージ
                    StatusText.Text = "D " + MyList.SelectedItems(0).SubItems(4).Text + " " + StatusText.Text
                    StatusText.SelectionStart = StatusText.Text.Length
                    StatusText.Focus()
                    _reply_to_id = 0
                    _reply_to_name = Nothing
                    Exit Sub
                End If
                If StatusText.Text = "" Then
                    ' ステータステキストが入力されていない場合先頭に@ユーザー名を追加する
                    StatusText.Text = "@" + MyList.SelectedItems(0).SubItems(4).Text + " "
                    _reply_to_id = Integer.Parse(MyList.SelectedItems(0).SubItems(5).Text)
                    _reply_to_name = MyList.SelectedItems(0).SubItems(4).Text
                Else
                    If isAuto Then
                        If StatusText.Text.IndexOf("@" + MyList.SelectedItems(0).SubItems(4).Text + " ") > -1 Then Exit Sub
                        If Not StatusText.Text.StartsWith("@") Then
                            If StatusText.Text.StartsWith(". ") Then
                                ' 複数リプライ
                                StatusText.Text = StatusText.Text.Insert(2, "@" + MyList.SelectedItems(0).SubItems(4).Text + " ")
                                _reply_to_id = 0
                                _reply_to_name = Nothing
                            Else
                                ' 単独リプライ
                                StatusText.Text = "@" + MyList.SelectedItems(0).SubItems(4).Text + " " + StatusText.Text
                                _reply_to_id = Integer.Parse(MyList.SelectedItems(0).SubItems(5).Text)
                                _reply_to_name = MyList.SelectedItems(0).SubItems(4).Text
                            End If
                        Else
                            ' 複数リプライ
                            StatusText.Text = ". @" + MyList.SelectedItems(0).SubItems(4).Text + " " + StatusText.Text
                            _reply_to_id = 0
                            _reply_to_name = Nothing
                        End If
                    Else
                        Dim sidx As Integer = StatusText.SelectionStart
                        If StatusText.Text.StartsWith("@") Then
                            '複数リプライ
                            StatusText.Text = ". " + StatusText.Text.Insert(sidx, " @" + MyList.SelectedItems(0).SubItems(4).Text + " ")
                            sidx += 5 + MyList.SelectedItems(0).SubItems(4).Text.Length
                        Else
                            ' 複数リプライ
                            StatusText.Text = StatusText.Text.Insert(sidx, " @" + MyList.SelectedItems(0).SubItems(4).Text + " ")
                            sidx += 3 + MyList.SelectedItems(0).SubItems(4).Text.Length
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
                    For cnt As Integer = 0 To MyList.SelectedItems.Count - 1
                        If sTxt.IndexOf("@" + MyList.SelectedItems(cnt).SubItems(4).Text + " ") = -1 Then
                            sTxt = sTxt.Insert(2, "@" + MyList.SelectedItems(cnt).SubItems(4).Text + " ")
                        End If
                    Next
                    StatusText.Text = sTxt
                Else
                    Dim ids As String = ""
                    Dim sidx As Integer = StatusText.SelectionStart
                    If Not StatusText.Text.StartsWith(". ") Then
                        StatusText.Text = ". " + StatusText.Text
                        sidx += 2
                    End If
                    For cnt As Integer = 0 To MyList.SelectedItems.Count - 1
                        If Not ids.Contains("@" + MyList.SelectedItems(cnt).SubItems(4).Text + " ") Then
                            ids += "@" + MyList.SelectedItems(cnt).SubItems(4).Text + " "
                        End If
                        If isAll Then
                            Dim pos1 As Integer = 0
                            Dim pos2 As Integer = 0
                            Dim dTxt As String = MyList.SelectedItems(cnt).SubItems(7).Text
                            Dim atId As String = ""
                            Do While True
                                pos1 = dTxt.IndexOf(_replyHtml, pos2)
                                If pos1 = -1 Then Exit Do
                                pos2 = dTxt.IndexOf(""">", pos1 + _replyHtml.Length)
                                If pos2 > -1 Then
                                    atId = "@" + dTxt.Substring(pos1 + _replyHtml.Length, pos2 - pos1 - _replyHtml.Length) + " "
                                    If Not ids.Contains(atId) AndAlso atId <> "@" + _username + " " Then
                                        ids += atId
                                    End If
                                End If
                            Loop
                        End If
                    Next
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

        _refreshIconCnt += 1
        If _refreshIconCnt > 3 Then _refreshIconCnt = 0

        NotifyIcon1.Icon = NIconRefresh(_refreshIconCnt)
    End Sub

    Private Sub ContextMenuTabProperty_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuTabProperty.Opening
        '右クリックの場合はタブ名が設定済。アプリケーションキーの場合は現在のタブを対象とする
        If _rclickTabName = "" Then _rclickTabName = ListTab.SelectedTab.Text

        For Each ts As TabStructure In _tabs
            If ts.tabName = _rclickTabName Then
                NotifyDispMenuItem.Checked = ts.notify
                SoundFileComboBox.Items.Clear()
                SoundFileComboBox.Items.Add("")
                Dim oDir As IO.DirectoryInfo = New IO.DirectoryInfo(My.Application.Info.DirectoryPath)
                For Each oFile As IO.FileInfo In oDir.GetFiles("*.wav")
                    SoundFileComboBox.Items.Add(oFile.Name)
                Next
                Dim idx As Integer = SoundFileComboBox.Items.IndexOf(ts.soundFile)
                If idx = -1 Then idx = 0
                SoundFileComboBox.SelectedIndex = idx
                UreadManageMenuItem.Checked = ts.unreadManage
                Exit For
            End If
        Next
    End Sub

    Private Sub ListTab_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListTab.MouseClick
    End Sub

    Private Sub UreadManageMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UreadManageMenuItem.Click
        If _rclickTabName = "" Then Exit Sub

        For Each ts As TabStructure In _tabs
            If ts.tabName = _rclickTabName Then
                ts.unreadManage = UreadManageMenuItem.Checked
                For Each itm As ListViewItem In ts.listCustom.Items
                    If SettingDialog.UnreadManage AndAlso ts.unreadManage Then
                    Else
                        Dim fcl As Color = _clReaded
                        If itm.SubItems(10).Text = "True" AndAlso SettingDialog.OneWayLove Then fcl = _clOWL
                        If itm.SubItems(9).Text = "True" Then fcl = _clFav
                        ts.listCustom.ChangeItemStyles(itm.Index, itm.BackColor, fcl, _fntReaded)
                        itm.SubItems(8).Text = "True"
                        ts.unreadCount = 0
                        ts.oldestUnreadItem = Nothing
                        If ts.tabPage.ImageIndex = 0 Then ts.tabPage.ImageIndex = -1
                    End If
                Next
                Exit Sub
            End If
        Next
    End Sub

    Private Sub NotifyDispMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NotifyDispMenuItem.Click
        If _rclickTabName = "" Then Exit Sub

        For Each ts As TabStructure In _tabs
            If ts.tabName = _rclickTabName Then
                ts.notify = Not ts.notify
                NotifyDispMenuItem.Checked = ts.notify
                Exit For
            End If
        Next
    End Sub

    Private Sub SoundFileComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SoundFileComboBox.SelectedIndexChanged
        If _rclickTabName = "" Then Exit Sub

        For Each ts As TabStructure In _tabs
            If ts.tabName = _rclickTabName Then
                ts.soundFile = DirectCast(SoundFileComboBox.SelectedItem, String)
                Exit For
            End If
        Next
    End Sub

    Private Sub DeleteTabMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DeleteTabMenuItem.Click
        If _rclickTabName = "" Then Exit Sub

        RemoveSpecifiedTab(_rclickTabName)
    End Sub

    Private Sub FilterEditMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FilterEditMenuItem.Click
        If _rclickTabName = "" Then Exit Sub

        fDialog.Tabs = _tabs
        fDialog.CurrentTab = _rclickTabName
        fDialog.ShowDialog()
        Me.TopMost = SettingDialog.AlwaysTop
        _tabs = fDialog.Tabs
        ReFilter()
    End Sub

    Private Sub AddTabMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddTabMenuItem.Click
        Dim inputName As New InputTabName()
        inputName.TabName = "MyTab" + _tabs.Count.ToString
        inputName.ShowDialog()
        Dim tabName As String = inputName.TabName
        inputName.Dispose()
        Me.TopMost = SettingDialog.AlwaysTop
        If tabName <> "" Then
            If Not AddNewTab(tabName) Then
                MessageBox.Show("タブ　""" + tabName + """　は既に存在するため、追加できません。別の名前を指定してください。", _
                                "タブ追加", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End If
    End Sub

    Private Sub TabMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabMenuItem.Click
        Dim myList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        Dim tabName As String = ""

        For Each itm As ListViewItem In myList.SelectedItems

            Do
                If TabDialog.ShowDialog = Windows.Forms.DialogResult.Cancel Then
                    Me.TopMost = SettingDialog.AlwaysTop
                    Exit Sub
                End If
                Me.TopMost = SettingDialog.AlwaysTop
                tabName = TabDialog.SelectedTabName

                ListTab.SelectedTab.Focus()
                If tabName = "(新規タブ)" Then

                    Dim inputName As New InputTabName()
                    inputName.TabName = "MyTab" + _tabs.Count.ToString
                    inputName.ShowDialog()
                    tabName = inputName.TabName
                    inputName.Dispose()
                    Me.TopMost = SettingDialog.AlwaysTop
                    If tabName <> "" Then
                        If Not AddNewTab(tabName) Then
                            MessageBox.Show("タブ　""" + tabName + """　は既に存在するため、追加できません。別の名前を指定してください。", _
                                            "タブ追加", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Else
                            Exit Do
                        End If
                    End If
                Else
                    Exit Do
                End If
            Loop While True
            fDialog.Tabs = _tabs
            fDialog.CurrentTab = tabName
            fDialog.AddNewFilter(itm.SubItems(4).Text, itm.SubItems(2).Text)
            fDialog.ShowDialog()
            Me.TopMost = SettingDialog.AlwaysTop
            _tabs = fDialog.Tabs
            ReFilter()
        Next

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
        If clsTw.InfoTwitter.Trim() = "" Then
            MessageBox.Show("Twitterからのお知らせはありません。", "Twitterからのお知らせ", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            Dim inf As String = clsTw.InfoTwitter.Trim()
            inf = "<html><head></head><body>" + inf + "</body></html>"
            PostBrowser.Visible = False
            PostBrowser.DocumentText = inf
            PostBrowser.Visible = True
        End If
    End Sub

    Private Sub ReplyAllStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReplyAllStripMenuItem.Click
        MakeReplyOrDirectStatus(False, True, True)
    End Sub

    Private Sub PostWorker_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles PostWorker.DoWork
        If _endingFlag Then
            e.Cancel = True
            Exit Sub
        End If

        Dim ret As String = ""
        Dim rslt As New GetWorkerResult()

        Dim args As GetWorkerArg = DirectCast(e.Argument, GetWorkerArg)
        '        Try
        If args.type = WORKERTYPE.CreateNewSocket Then
            clsTwPost.CreateNewSocket()
        Else
            CheckReplyTo(args.status)

            ret = clsTwPost.PostStatus(args.status, _reply_to_id)

            _reply_to_id = 0
            _reply_to_name = Nothing
        End If
        rslt.retMsg = ret
        rslt.TLine = Nothing
        rslt.page = args.page
        rslt.endPage = args.endPage
        rslt.type = args.type
        rslt.imgs = Nothing
        rslt.tName = args.tName

        If _endingFlag Then
            e.Cancel = True
            Exit Sub
        End If

        e.Result = rslt
        'Catch ex As Exception
        '    If _endingFlag Then
        '        e.Cancel = True
        '        Exit Sub
        '    End If
        '    My.Application.Log.DefaultFileLogWriter.Location = Logging.LogFileLocation.ExecutableDirectory
        '    My.Application.Log.DefaultFileLogWriter.MaxFileSize = 102400
        '    My.Application.Log.DefaultFileLogWriter.AutoFlush = True
        '    My.Application.Log.DefaultFileLogWriter.Append = False
        '    'My.Application.Log.WriteException(ex, _
        '    '    Diagnostics.TraceEventType.Critical, _
        '    '    "Source=" + ex.Source + " StackTrace=" + ex.StackTrace + " InnerException=" + IIf(ex.InnerException Is Nothing, "", ex.InnerException.Message))
        '    My.Application.Log.WriteException(ex, _
        '        Diagnostics.TraceEventType.Critical, _
        '        ex.StackTrace + vbCrLf + Now.ToString + vbCrLf + args.type.ToString + vbCrLf + args.status)
        '    rslt.retMsg = "Tween 例外発生(PostWorker_DoWork)"
        '    rslt.TLine = Nothing
        '    rslt.page = args.page
        '    rslt.endPage = args.endPage
        '    rslt.type = args.type

        '    e.Result = rslt
        'End Try
    End Sub

    Private Sub PostWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles PostWorker.RunWorkerCompleted
        If e.Error IsNot Nothing Then
            If My.Computer.Network.IsAvailable Then
                NotifyIcon1.Icon = NIconAtRed
            End If
            Throw e.Error
            Exit Sub
        End If

        If _endingFlag OrElse e.Cancelled Then
            Exit Sub
        End If

        Dim rslt As GetWorkerResult = DirectCast(e.Result, GetWorkerResult)
        Dim args As New GetWorkerArg()

        urlUndoBuffer = Nothing
        UrlUndoToolStripMenuItem.Enabled = False  'Undoをできないように設定

        TimerRefreshIcon.Enabled = False
        If My.Computer.Network.IsAvailable Then
            NotifyIcon1.Icon = NIconAt
        Else
            NotifyIcon1.Icon = NIconAtSmoke
        End If

        If rslt.retMsg <> "" Then
            '''''エラー通知方法の変更も設定できるように！
            If My.Computer.Network.IsAvailable Then
                TimerRefreshIcon.Enabled = False
                NotifyIcon1.Icon = NIconAtRed
            End If
            If rslt.retMsg.StartsWith("Tween 例外発生") Then
                MessageBox.Show("エラーが発生しました。申し訳ありません。ログをexeファイルのある場所にTween.logとして作ったので、kiri.feather@gmail.comまで送っていただけると助かります。ご面倒なら@kiri_featherまでお知らせ頂くだけでも助かります。", "エラー発生", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If

        Select Case rslt.type
            Case WORKERTYPE.CreateNewSocket
                Exit Sub
            Case WORKERTYPE.PostMessage
                StatusText.Enabled = True
                PostButton.Enabled = True
                ReplyStripMenuItem.Enabled = True
                DMStripMenuItem.Enabled = True

                If rslt.retMsg.Length > 0 Then
                    StatusLabel.Text = rslt.retMsg
                    TimerRefreshIcon.Enabled = False
                    NotifyIcon1.Icon = NIconAtRed
                Else
                    _postTimestamps.Add(Now)
                    Dim oneHour As Date = Now.Subtract(New TimeSpan(1, 0, 0))
                    For i As Integer = _postTimestamps.Count - 1 To 0 Step -1
                        If _postTimestamps(i).CompareTo(oneHour) < 0 Then
                            _postTimestamps.RemoveAt(i)
                        End If
                    Next
                    StatusLabel.Text = "POST完了"
                    StatusText.Text = ""
                    _history.Add("")
                    _hisIdx = _history.Count - 1
                    SetMainWindowTitle()
                End If

                args.page = 1
                args.endPage = 1
                args.type = WORKERTYPE.Timeline
                If Not GetTimelineWorker.IsBusy Then
                    'TimerTimeline.Enabled = False
                    StatusLabel.Text = "Recent更新中..."
                    NotifyIcon1.Icon = NIconRefresh(0)
                    _refreshIconCnt = 0
                    TimerRefreshIcon.Enabled = True
                    Do While GetTimelineWorker.IsBusy
                        Threading.Thread.Sleep(1)
                        Application.DoEvents()
                    Loop
                    GetTimelineWorker.RunWorkerAsync(args)
                End If
        End Select
    End Sub

    Private Sub IDRuleMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IDRuleMenuItem.Click
        Dim myList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        Dim tabName As String = ""

        Do
            If TabDialog.ShowDialog = Windows.Forms.DialogResult.Cancel Then
                Me.TopMost = SettingDialog.AlwaysTop
                Exit Sub
            End If
            Me.TopMost = SettingDialog.AlwaysTop
            tabName = TabDialog.SelectedTabName

            ListTab.SelectedTab.Focus()
            If tabName = "(新規タブ)" Then
                Dim inputName As New InputTabName()
                inputName.TabName = "MyTab" + _tabs.Count.ToString
                inputName.ShowDialog()
                tabName = inputName.TabName
                inputName.Dispose()
                Me.TopMost = SettingDialog.AlwaysTop
                If tabName <> "" Then
                    If Not AddNewTab(tabName) Then
                        MessageBox.Show("タブ　""" + tabName + """　は既に存在するため、追加できません。別の名前を指定してください。", _
                                        "タブ追加", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Else
                        Exit Do
                    End If
                End If
            Else
                Exit Do
            End If
        Loop While True
        Dim mv As Boolean = False
        If MessageBox.Show("Recentに残しますか？" + vbCrLf + "  「はい」　：残す" + vbCrLf + "  「いいえ」：残さない", _
           "移動確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            mv = False
        Else
            mv = True
        End If
        Dim mk As Boolean = False
        If Not mv Then
            If MessageBox.Show("マークをつけますか？" + vbCrLf + "  「はい」　：つける" + vbCrLf + "  「いいえ」：つけない", _
               "マーク確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                mk = True
            Else
                mk = False
            End If
        End If
        For Each ts As TabStructure In _tabs
            If ts.tabName = tabName Then
                Dim ids As New List(Of String)
                For Each itm As ListViewItem In myList.SelectedItems
                    If Not ids.Contains(itm.SubItems(4).Text) Then
                        ids.Add(itm.SubItems(4).Text)
                        Dim flt As New FilterClass()
                        flt.BodyFilter.Clear()
                        flt.IDFilter = itm.SubItems(4).Text
                        flt.SearchBoth = True
                        flt.moveFrom = mv
                        flt.SetMark = mk
                        flt.UseRegex = False
                        flt.SearchURL = False
                        ts.filters.Add(flt)
                    End If
                Next
                ReFilter()
                Exit Sub
            End If
        Next
    End Sub

    Private Sub CopySTOTMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopySTOTMenuItem.Click
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        Dim clstr As String = ""
        For Each itm As ListViewItem In MyList.SelectedItems
            If clstr <> "" Then clstr += vbCrLf
            clstr += itm.SubItems(4).Text + ":" + itm.SubItems(2).Text + " [http://twitter.com/" + itm.SubItems(4).Text + "/statuses/" + itm.SubItems(5).Text + "]"
        Next
        If clstr <> "" Then
            Dim i As Integer = 0
RETRY:
            Try
                Clipboard.Clear()
                Clipboard.SetText(clstr)
            Catch ex As Exception
                i += 1
                If i < 3 Then
                    System.Threading.Thread.Sleep(500)
                    My.Application.DoEvents()
                    GoTo RETRY
                End If
            End Try
        End If
    End Sub

    Private Sub CopyURLMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyURLMenuItem.Click
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        Dim clstr As String = ""
        For Each itm As ListViewItem In MyList.SelectedItems
            If clstr <> "" Then clstr += vbCrLf
            clstr += "http://twitter.com/" + itm.SubItems(4).Text + "/statuses/" + itm.SubItems(5).Text
        Next
        If clstr <> "" Then
            Dim i As Integer = 0
RETRY:
            Try
                Clipboard.Clear()
                Clipboard.SetText(clstr)
            Catch ex As Exception
                i += 1
                If i < 3 Then
                    System.Threading.Thread.Sleep(500)
                    My.Application.DoEvents()
                    GoTo RETRY
                End If
            End Try
        End If
    End Sub

    Private Sub SelectAllMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectAllMenuItem.Click
        If StatusText.Focused Then
            StatusText.SelectAll()
        Else
            Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
            For Each lItem As ListViewItem In MyList.Items
                lItem.Selected = True
            Next
        End If
    End Sub

    Private Sub MoveMiddle()
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        Dim _item As ListViewItem
        Dim idx1 As Integer
        Dim idx2 As Integer

        If MyList.SelectedItems.Count = 0 Then Exit Sub

        Dim idx As Integer = MyList.SelectedItems(0).Index

        _item = MyList.GetItemAt(0, 25)
        If _item Is Nothing Then _item = MyList.Items(0)
        idx1 = _item.Index
        _item = MyList.GetItemAt(0, MyList.ClientSize.Height - 1)
        If _item Is Nothing Then _item = MyList.Items(MyList.Items.Count - 1)
        idx2 = _item.Index

        idx -= Math.Abs(idx1 - idx2) \ 2
        If idx < 0 Then idx = 0

        MyList.EnsureVisible(MyList.Items.Count - 1)
        MyList.EnsureVisible(idx)

        MyList.Update()
        TimerColorize.Start()
    End Sub

    Private Sub WedataMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WedataMenuItem.Click
        If clsTwSync IsNot Nothing Then
            clsTwSync.GetWedata()
        End If
    End Sub

    Private Sub ReFilter()

        '☆☆☆☆　暫定対応　☆☆☆☆
        '発言保持をクラス化したとき、ちゃんと対応する
        'このコードではメンテできない


        'Dim modify As Boolean = False
        'For Each ts As TabStructure In _tabs
        '    If ts.modified Then
        '        modify = True
        '        ts.modified = False
        '    End If
        'Next
        'If modify = False Then Exit Sub


        For Each ts As TabStructure In _tabs
            ts.listCustom.BeginUpdate()
        Next

        Dim itms As New List(Of ListViewItem)()
        For Each ts As TabStructure In _tabs
            If ts.tabName <> "Recent" AndAlso ts.tabName <> "Reply" AndAlso ts.tabName <> "Direct" AndAlso ts.modified Then
                For Each itmo As ListViewItem In ts.listCustom.Items
                    itms.Add(DirectCast(itmo.Clone, System.Windows.Forms.ListViewItem))
                Next
                ts.oldestUnreadItem = Nothing
                ts.listCustom.Items.Clear()
                ts.unreadCount = 0
                ts.allCount = 0
                ts.tabPage.ImageIndex = -1

                For Each ts2 As TabStructure In _tabs
                    If Not ts2.Equals(ts) AndAlso ts2.tabName <> "Reply" AndAlso ts2.tabName <> "Direct" Then
                        For Each itm As ListViewItem In ts2.listCustom.Items
                            Dim mv As Boolean = False
                            Dim nf As Boolean = False
                            Dim mk As Boolean = False
                            Dim lItem As New Twitter.MyListItem()

                            lItem.Data = itm.SubItems(2).Text
                            lItem.Fav = Boolean.Parse(itm.SubItems(9).Text)
                            lItem.Id = itm.SubItems(5).Text
                            lItem.ImageUrl = itm.SubItems(6).Text
                            lItem.Name = itm.SubItems(4).Text
                            lItem.Nick = itm.SubItems(1).Text
                            lItem.OrgData = itm.SubItems(7).Text
                            lItem.PDate = Date.Parse(itm.SubItems(3).Text)
                            lItem.Protect = itm.SubItems(0).Text.Contains("Ю")
                            lItem.Reply = Boolean.Parse(itm.SubItems(11).Text)
                            If itm.SubItems(8).Text = "True" Then
                                lItem.Readed = True
                            Else
                                lItem.Readed = False
                            End If
                            itm.SubItems(0).Text = itm.SubItems(0).Text.Replace("♪", "")

                            '                            For Each ts As TabStructure In _tabs
                            Dim hit As Boolean = False

                            For Each ft As FilterClass In ts.filters
                                Dim bHit As Boolean = True
                                Dim tBody As String
                                If ft.SearchURL Then
                                    tBody = lItem.OrgData
                                Else
                                    tBody = lItem.Data
                                End If
                                If ft.SearchBoth Then
                                    If ft.IDFilter = "" OrElse lItem.Name.Equals(ft.IDFilter, StringComparison.CurrentCultureIgnoreCase) Then
                                        For Each fs As String In ft.BodyFilter
                                            If ft.UseRegex Then
                                                If Regex.IsMatch(tBody, fs, RegexOptions.IgnoreCase) = False Then bHit = False
                                            Else
                                                If tBody.ToLower.Contains(fs.ToLower) = False Then bHit = False
                                            End If
                                            If Not bHit Then Exit For
                                        Next
                                    Else
                                        bHit = False
                                    End If
                                Else
                                    For Each fs As String In ft.BodyFilter
                                        If ft.UseRegex Then
                                            If Not Regex.IsMatch(lItem.Name + tBody, fs, RegexOptions.IgnoreCase) Then bHit = False
                                        Else
                                            If Not (lItem.Name + tBody).ToLower().Contains(fs.ToLower) Then bHit = False
                                        End If
                                        If Not bHit Then Exit For
                                    Next
                                End If
                                If bHit Then
                                    hit = True
                                    If ft.SetMark Then mk = True
                                    If ft.moveFrom Then mv = True
                                End If
                                If hit AndAlso mv AndAlso mk Then Exit For
                            Next
                            For Each itmo2 As ListViewItem In ts.listCustom.Items
                                If itmo2.SubItems(5).Text = itm.SubItems(5).Text Then
                                    hit = False
                                    Exit For
                                End If
                            Next
                            If hit Then
                                Dim itm2 As ListViewItem = DirectCast(itm.Clone, ListViewItem)
                                ts.allCount += 1
                                If itm2.SubItems(8).Text = "False" Then
                                    If ts.unreadManage And SettingDialog.UnreadManage Then
                                        ts.unreadCount += 1
                                        If ts.oldestUnreadItem Is Nothing Then
                                            ts.oldestUnreadItem = itm2
                                        Else
                                            If ts.oldestUnreadItem.SubItems(5).Text > itm2.SubItems(5).Text Then
                                                ts.oldestUnreadItem = itm2
                                            End If
                                        End If
                                    Else
                                        itm2.SubItems(8).Text = "True"
                                    End If
                                End If
                                ts.listCustom.Items.Add(itm2)
                            End If
                            If ts.unreadCount > 0 AndAlso ts.tabPage.ImageIndex = -1 Then ts.tabPage.ImageIndex = 0
                            'Next
                            If ts2.tabName = "Recent" Then
                                If Not mv Then
                                    If mk Then itm.SubItems(0).Text += "♪"
                                Else
                                    _tabs(0).allCount -= 1
                                    If itm.SubItems(8).Text = "False" Then _tabs(0).unreadCount -= 1
                                    _tabs(0).listCustom.Items.Remove(itm)
                                End If
                            End If
                        Next
                    End If
                Next
                For Each itm As ListViewItem In itms
                    Dim mv As Boolean = False
                    Dim nf As Boolean = False
                    Dim mk As Boolean = False
                    Dim lItem As New Twitter.MyListItem()

                    lItem.Data = itm.SubItems(2).Text
                    lItem.Fav = CBool(itm.SubItems(9).Text)
                    lItem.Id = itm.SubItems(5).Text
                    lItem.ImageUrl = itm.SubItems(6).Text
                    lItem.Name = itm.SubItems(4).Text
                    lItem.Nick = itm.SubItems(1).Text
                    lItem.OrgData = itm.SubItems(7).Text
                    lItem.PDate = CDate(itm.SubItems(3).Text)
                    lItem.Protect = itm.SubItems(0).Text.Contains("Ю")
                    lItem.Reply = CBool(itm.SubItems(11).Text)
                    If itm.SubItems(8).Text = "True" Then
                        lItem.Readed = True
                    Else
                        lItem.Readed = False
                    End If
                    itm.SubItems(0).Text = itm.SubItems(0).Text.Replace("♪", "")

                    '                            For Each ts As TabStructure In _tabs
                    Dim hit As Boolean = False

                    For Each ft As FilterClass In ts.filters
                        Dim bHit As Boolean = True
                        Dim tBody As String
                        If ft.SearchURL Then
                            tBody = lItem.OrgData
                        Else
                            tBody = lItem.Data
                        End If
                        If ft.SearchBoth Then
                            If ft.IDFilter = "" OrElse lItem.Name.Equals(ft.IDFilter, StringComparison.CurrentCultureIgnoreCase) Then
                                For Each fs As String In ft.BodyFilter
                                    If ft.UseRegex Then
                                        If Not Regex.IsMatch(tBody, fs, RegexOptions.IgnoreCase) Then bHit = False
                                    Else
                                        If Not tBody.ToLower.Contains(fs.ToLower()) Then bHit = False
                                    End If
                                    If Not bHit Then Exit For
                                Next
                            Else
                                bHit = False
                            End If
                        Else
                            For Each fs As String In ft.BodyFilter
                                If ft.UseRegex Then
                                    If Not Regex.IsMatch(lItem.Name + tBody, fs, RegexOptions.IgnoreCase) Then bHit = False
                                Else
                                    If Not (lItem.Name + tBody).ToLower.Contains(fs.ToLower()) Then bHit = False
                                End If
                                If Not bHit Then Exit For
                            Next
                        End If
                        If bHit Then
                            hit = True
                            If ft.SetMark Then mk = True
                            If ft.moveFrom Then mv = True
                        End If
                        If hit AndAlso mv AndAlso mk Then Exit For
                    Next
                    For Each itmo2 As ListViewItem In ts.listCustom.Items
                        If itmo2.SubItems(5).Text = itm.SubItems(5).Text Then
                            hit = False
                            Exit For
                        End If
                    Next
                    If hit Then
                        Dim itm2 As ListViewItem = DirectCast(itm.Clone, ListViewItem)
                        ts.allCount += 1
                        If itm2.SubItems(8).Text = "False" Then
                            If ts.unreadManage AndAlso SettingDialog.UnreadManage Then
                                ts.unreadCount += 1
                                If ts.oldestUnreadItem Is Nothing Then
                                    ts.oldestUnreadItem = itm2
                                Else
                                    If ts.oldestUnreadItem.SubItems(5).Text > itm2.SubItems(5).Text Then
                                        ts.oldestUnreadItem = itm2
                                    End If
                                End If
                            Else
                                itm2.SubItems(8).Text = "True"
                            End If
                        End If
                        ts.listCustom.Items.Add(itm2)
                    Else
                        For Each itmr As ListViewItem In _tabs(0).listCustom.Items
                            If itmr.SubItems(5).Text = itm.SubItems(5).Text Then
                                hit = True
                                Exit For
                            End If
                        Next
                        If hit = False Then
                            _tabs(0).allCount += 1
                            If itm.SubItems(8).Text = "False" Then
                                If _tabs(0).unreadManage AndAlso SettingDialog.UnreadManage Then
                                    _tabs(0).unreadCount += 1
                                    If _tabs(0).oldestUnreadItem Is Nothing Then
                                        _tabs(0).oldestUnreadItem = itm
                                    Else
                                        If _tabs(0).oldestUnreadItem.SubItems(5).Text > itm.SubItems(5).Text Then
                                            _tabs(0).oldestUnreadItem = itm
                                        End If
                                    End If
                                Else
                                    itm.SubItems(8).Text = "True"
                                End If
                            End If
                            _tabs(0).listCustom.Items.Add(DirectCast(itm.Clone, System.Windows.Forms.ListViewItem))
                        End If
                    End If
                    If ts.unreadCount > 0 AndAlso ts.tabPage.ImageIndex = -1 Then ts.tabPage.ImageIndex = 0
                    If _tabs(0).unreadCount > 0 AndAlso _tabs(0).tabPage.ImageIndex = -1 Then _tabs(0).tabPage.ImageIndex = 0
                Next
            End If
        Next

        For Each ts As TabStructure In _tabs
            If ts.modified Then
                ts.modified = False
            End If
            ts.listCustom.EndUpdate()
        Next

    End Sub

    Private Sub OpenURLMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenURLMenuItem.Click
        If PostBrowser.Document.Links.Count > 0 Then
            UrlDialog.ClearUrl()

            Dim openUrlStr As String = ""

            If PostBrowser.Document.Links.Count = 1 Then
                openUrlStr = PostBrowser.Document.Links(0).GetAttribute("href")
            Else
                For Each linkElm As System.Windows.Forms.HtmlElement In PostBrowser.Document.Links
                    UrlDialog.AddUrl(linkElm.GetAttribute("href"))
                Next
                If UrlDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    openUrlStr = UrlDialog.SelectedUrl
                End If
                Me.TopMost = SettingDialog.AlwaysTop
            End If

            If openUrlStr <> "" Then
                Do While ExecWorker.IsBusy
                    Threading.Thread.Sleep(1)
                    Application.DoEvents()
                Loop

                ExecWorker.RunWorkerAsync(openUrlStr)
            End If
        End If
    End Sub

    Private Sub ClearTabMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearTabMenuItem.Click
        If _rclickTabName = "" Then Exit Sub

        If MessageBox.Show("このタブの発言をクリアしてもよろしいですか？" + vbCrLf + _
                        "（サーバーから発言は削除しません。）", "タブクリア確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        For Each ts As TabStructure In _tabs
            If ts.tabName = _rclickTabName Then
                ts.oldestUnreadItem = Nothing
                ts.allCount = 0
                ts.unreadCount = 0
                ts.listCustom.Items.Clear()
                ts.tabPage.ImageIndex = -1
                Exit For
            End If
        Next
    End Sub

    Private Sub SetMainWindowTitle()
        'メインウインドウタイトルの書き換え
        Dim ttl As String = ""
        Dim urat As Integer = _tabs(1).unreadCount + _tabs(2).unreadCount
        Dim ur As Integer = 0
        Dim al As Integer = 0
        If SettingDialog.DispLatestPost <> DispTitleEnum.None AndAlso _
           SettingDialog.DispLatestPost <> DispTitleEnum.Post AndAlso _
           SettingDialog.DispLatestPost <> DispTitleEnum.Ver Then
            For Each ts As TabStructure In _tabs
                ur += ts.unreadCount
                al += ts.allCount
            Next
        End If
        If SettingDialog.DispUsername Then ttl = _username + " - "
        ttl += "Tween  "
        Select Case SettingDialog.DispLatestPost
            Case DispTitleEnum.Ver
                ttl += "Ver:" + My.Application.Info.Version.ToString()
            Case DispTitleEnum.Post
                If _history IsNot Nothing AndAlso _history.Count > 1 Then
                    ttl += _history(_history.Count - 2)
                End If
            Case DispTitleEnum.UnreadRepCount
                ttl += urat.ToString() + "件 (未読＠)"
            Case DispTitleEnum.UnreadAllCount
                ttl += ur.ToString() + "件 (未読)"
            Case DispTitleEnum.UnreadAllRepCount
                ttl += ur.ToString() + " (" + urat.ToString() + ")件 (未読)"
            Case DispTitleEnum.UnreadCountAllCount
                ttl += ur.ToString() + "/" + al.ToString() + "件 (未読/総件数)"
        End Select

        Me.Text = ttl
    End Sub

    Private Sub SetStatusLabel()
        'ステータス欄にカウント表示
        'タブ未読数/タブ発言数 全未読数/総発言数 (未読＠＋未読DM数)
        Dim urat As Integer = _tabs(1).unreadCount + _tabs(2).unreadCount
        Dim ur As Integer = 0
        Dim al As Integer = 0
        Dim tur As Integer = 0
        Dim tal As Integer = 0
        Dim slbl As StringBuilder = New StringBuilder()
        For Each ts As TabStructure In _tabs
            ur += ts.unreadCount
            al += ts.allCount
            If ts.tabPage.Equals(ListTab.SelectedTab) Then
                tur = ts.unreadCount
                tal = ts.allCount
            End If
        Next

        slbl.AppendFormat("[タブ: {0}/{1} 全体: {2}/{3} (返信: {4})] [時速: 投 {5}/ ☆ {6}/ 流 {7}] [間隔: ", tur, tal, ur, al, urat, _postTimestamps.Count, _favTimestamps.Count, _tlCount)
        If SettingDialog.TimelinePeriodInt = 0 Then
            slbl.Append("-]")
        Else
            slbl.Append((TimerTimeline.Interval / 1000).ToString() + "]")
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

    Private Sub UpdateFollowersMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdateFollowersMenuItem1.Click
        StatusLabel.Text = "Followers取得中..."
        Dim ret As String
        ret = clsTwSync.GetFollowers()
        If ret <> "" Then
            StatusLabel.Text = "Followers取得エラー：" & ret
            Exit Sub
        End If
        StatusLabel.Text = "Followers取得完了"
    End Sub

    Private Sub SplitContainer1_SplitterMoved(ByVal sender As Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles SplitContainer1.SplitterMoved
        If Me.WindowState = FormWindowState.Normal Then _mySpDis = SplitContainer1.SplitterDistance
    End Sub

    Private Sub RepliedStatusOpenMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RepliedStatusOpenMenuItem.Click
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        Dim id As Integer = clsTwSync.GetReplyStatusID(Integer.Parse(MyList.SelectedItems(0).SubItems(5).Text))
        If id > 0 Then
            ExecWorker.RunWorkerAsync("http://twitter.com/" + Regex.Match(MyList.SelectedItems(0).SubItems(2).Text, "@[A-Za-z0-9_]+").Value.Substring(1) + "/statuses/" + id.ToString())
        End If
    End Sub

    Private Sub ContextMenuStrip3_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip3.Opening
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)

        If MyList.SelectedItems.Count > 0 Then
            Dim name As String = MyList.SelectedItems(0).SubItems(6).Text
            name = IO.Path.GetFileNameWithoutExtension(name.Substring(name.LastIndexOf("/"c)))
            name = name.Substring(0, name.Length - 7) ' "_normal".Length
            Me.IconNameToolStripMenuItem.Enabled = True
            If Me.TIconList.Images.Item(name) Is Nothing Then
                Me.SaveIconPictureToolStripMenuItem.Enabled = True
            Else
                Me.SaveIconPictureToolStripMenuItem.Enabled = False
            End If
            Me.IconNameToolStripMenuItem.Text = name
        Else
            Me.IconNameToolStripMenuItem.Enabled = False
            Me.SaveIconPictureToolStripMenuItem.Enabled = False
            Me.IconNameToolStripMenuItem.Text = "(発言を選択してください)"
        End If
    End Sub

    Private Sub IconNameToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IconNameToolStripMenuItem.Click
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        Dim name As String = MyList.SelectedItems(0).SubItems(6).Text
        ExecWorker.RunWorkerAsync(name.Remove(name.LastIndexOf("_normal"), 7)) ' "_normal".Length
    End Sub

    Private Sub SaveOriginalSizeIconPictureToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        Dim name As String = MyList.SelectedItems(0).SubItems(6).Text
        name = IO.Path.GetFileNameWithoutExtension(name.Substring(name.LastIndexOf("/"c)))

        Me.SaveFileDialog1.FileName = name.Substring(0, name.Length - 8) ' "_normal".Length + 1

        If Me.SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            ' STUB
        End If
    End Sub

    Private Sub SaveIconPictureToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveIconPictureToolStripMenuItem.Click
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        Dim name As String = MyList.SelectedItems(0).SubItems(6).Text

        Me.SaveFileDialog1.FileName = name.Substring(name.LastIndexOf("/"c) + 1)

        If Me.SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Me.TIconList.Images.Item(name).Save(Me.SaveFileDialog1.FileName)
        End If
    End Sub

    Private Sub SplitContainer2_Panel2_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitContainer2.Panel2.Resize
        Me.StatusText.Multiline = Me.SplitContainer2.Panel2.Height > Me.SplitContainer2.Panel2MinSize + 2
        MultiLineMenuItem.Checked = Me.StatusText.Multiline
        If _section IsNot Nothing Then
            _section.StatusMultiline = MultiLineMenuItem.Checked
            If StatusText.Multiline Then _section.StatusTextHeight = SplitContainer2.Panel2.Height
        End If
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
            SplitContainer2.SplitterDistance = SplitContainer2.Height - _section.StatusTextHeight - SplitContainer2.SplitterWidth
        Else
            SplitContainer2.SplitterDistance = SplitContainer2.Height - SplitContainer2.Panel2MinSize - SplitContainer2.SplitterWidth
        End If
    End Sub

    Private Function UrlConvert(ByVal Converter_Type As UrlConverter) As Boolean
        Dim result As String = ""
        Dim url As Regex = New Regex("\b(?:https?|shttp)://(?:(?:[-_.!~*'()a-zA-Z0-9;:&=+$,]|%[0-9A-Fa-f" + _
                                     "][0-9A-Fa-f])*@)?(?:(?:[a-zA-Z0-9](?:[-a-zA-Z0-9]*[a-zA-Z0-9])?\.)" + _
                                     "*[a-zA-Z](?:[-a-zA-Z0-9]*[a-zA-Z0-9])?\.?|[0-9]+\.[0-9]+\.[0-9]+\." + _
                                     "[0-9]+)(?::[0-9]*)?(?:/(?:[-_.!~*'()a-zA-Z0-9:@&=+$,]|%[0-9A-Fa-f]" + _
                                     "[0-9A-Fa-f])*(?:;(?:[-_.!~*'()a-zA-Z0-9:@&=+$,]|%[0-9A-Fa-f][0-9A-" + _
                                     "Fa-f])*)*(?:/(?:[-_.!~*'()a-zA-Z0-9:@&=+$,]|%[0-9A-Fa-f][0-9A-Fa-f" + _
                                     "])*(?:;(?:[-_.!~*'()a-zA-Z0-9:@&=+$,]|%[0-9A-Fa-f][0-9A-Fa-f])*)*)" + _
                                     "*)?(?:\?(?:[-_.!~*'()a-zA-Z0-9;/?:@&=+$,]|%[0-9A-Fa-f][0-9A-Fa-f])" + _
                                     "*)?(?:#(?:[-_.!~*'()a-zA-Z0-9;/?:@&=+$,]|%[0-9A-Fa-f][0-9A-Fa-f])*)?")

        Dim src As String = ""

        If StatusText.SelectionLength > 0 Then
            Dim tmp As String = StatusText.SelectedText
            ' httpから始まらない場合、ExcludeStringで指定された文字列で始まる場合は対象としない
            If Not tmp.StartsWith("http") Then
                ' Nothing
            Else
                ' 文字列が選択されている場合はその文字列について処理

                '短縮URL変換 日本語を含むかもしれないのでURLエンコードする
                result = clsTwSync.MakeShortUrl(Converter_Type, StatusText.SelectedText)

                If result.Equals("Can't convert") Then
                    Return False
                End If

                If Not result = "" Then
                    Dim undotmp As New urlUndo

                    StatusText.Select(StatusText.Text.IndexOf(tmp), tmp.Length)
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
                StatusText.Select(StatusText.Text.IndexOf(tmp), tmp.Length)

                '短縮URL変換
                result = clsTwSync.MakeShortUrl(Converter_Type, StatusText.SelectedText)

                If result.Equals("Can't convert") Then
                    Return False
                End If

                If Not result = "" Then
                    StatusText.Select(StatusText.Text.IndexOf(tmp), tmp.Length)
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
        Dim MyList As DetailsListView = DirectCast(sender, DetailsListView)

        If e.Header.Text = "" Then
            _columnIdx = e.NewDisplayIndex
        Else
            Dim cIdx As Integer = 0
            For Each clm As ColumnHeader In MyList.Columns
                If clm.Text = "" Then
                    cIdx = clm.DisplayIndex
                    If cIdx >= e.NewDisplayIndex AndAlso cIdx < e.OldDisplayIndex Then
                        _columnIdx = cIdx + 1
                    ElseIf cIdx <= e.NewDisplayIndex AndAlso cIdx > e.OldDisplayIndex Then
                        _columnIdx = cIdx - 1
                    End If
                    Exit For
                End If
            Next
        End If

        _columnChangeFlag = True
    End Sub

    Private Sub MyList_CoumnWidthChanging(ByVal sender As System.Object, ByVal e As ColumnWidthChangingEventArgs)
        _columnChangeFlag = True
    End Sub

    Private Sub ToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem3.Click
        PostBrowser.Document.ExecCommand("Copy", False, Nothing)
    End Sub

    Private Sub doSearchToolStrip(ByVal url As String)
        Dim typ As Type = PostBrowser.ActiveXInstance.GetType()
        Dim _SelObj As Object = typ.InvokeMember("selection", BindingFlags.GetProperty, Nothing, PostBrowser.Document.DomDocument, Nothing)
        Dim _objRange As Object = _SelObj.GetType().InvokeMember("createRange", BindingFlags.InvokeMethod, Nothing, _SelObj, Nothing)
        Dim _selText As String = DirectCast(_objRange.GetType().InvokeMember("text", BindingFlags.GetProperty, Nothing, _objRange, Nothing), String)
        Dim tmp As String

        tmp = String.Format(url, _selText)
        'MessageBox.Show("Selection String is " & _selText)
        ExecWorker.RunWorkerAsync(tmp)
    End Sub

    Private Sub ToolStripMenuItem5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem5.Click
        PostBrowser.Document.ExecCommand("SelectAll", False, Nothing)
    End Sub

    Private Sub SearchItem1ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchItem1ToolStripMenuItem.Click
        Dim resources As ComponentResourceManager = New ComponentResourceManager(GetType(TweenMain))
        doSearchToolStrip(resources.GetString("SearchItem1Url"))
    End Sub

    Private Sub SearchItem2ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchItem2ToolStripMenuItem.Click
        Dim resources As ComponentResourceManager = New ComponentResourceManager(GetType(TweenMain))
        doSearchToolStrip(resources.GetString("SearchItem2Url"))
        'Dim _tmp As String = PostBrowser.StatusText
    End Sub

    Private Sub SearchItem3ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchItem3ToolStripMenuItem.Click
        Dim resources As ComponentResourceManager = New ComponentResourceManager(GetType(TweenMain))
        doSearchToolStrip(resources.GetString("SearchItem3Url"))
    End Sub

    Private Sub SearchItem4ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchItem4ToolStripMenuItem.Click
        Dim resources As ComponentResourceManager = New ComponentResourceManager(GetType(TweenMain))
        doSearchToolStrip(resources.GetString("SearchItem4Url"))
    End Sub
End Class

Public Class TabStructure
    Public tabPage As System.Windows.Forms.TabPage
    Public listCustom As DetailsListView
    Public colHd1 As System.Windows.Forms.ColumnHeader
    Public colHd2 As System.Windows.Forms.ColumnHeader
    Public colHd3 As System.Windows.Forms.ColumnHeader
    Public colHd4 As System.Windows.Forms.ColumnHeader
    Public colHd5 As System.Windows.Forms.ColumnHeader
    Public tabName As String
    Public unreadManage As Boolean
    Public notify As Boolean
    Public soundFile As String
    Public filters As New List(Of FilterClass)()
    Public modified As Boolean
    Public oldestUnreadItem As ListViewItem
    Public unreadCount As Integer
    Public allCount As Integer
End Class

Public Class FilterClass
    Public IDFilter As String
    Public BodyFilter As New List(Of String)
    Public SearchBoth As Boolean
    Public moveFrom As Boolean
    Public SetMark As Boolean
    Public SearchURL As Boolean
    Public UseRegex As Boolean
End Class

Public Class ListViewItemComparer
    Implements IComparer

    ''' <summary>
    ''' 比較する方法
    ''' </summary>
    Public Enum ComparerMode
        [String]
        [Integer]
        DateTime
        None
    End Enum

    Private _column As Integer
    Private _order As SortOrder
    Private _mode As ComparerMode
    Private _columnModes() As ComparerMode

    ''' <summary>
    ''' 並び替えるListView列の番号
    ''' </summary>
    Public Property Column() As Integer
        Get
            Return _column
        End Get
        Set(ByVal Value As Integer)
            If _column = Value Then
                If _order = SortOrder.Ascending Then
                    _order = SortOrder.Descending
                Else
                    If _order = SortOrder.Descending Then
                        _order = SortOrder.Ascending
                    End If
                End If
            End If
            _column = Value
        End Set
    End Property

    ''' <summary>
    ''' 昇順か降順か
    ''' </summary>
    Public Property Order() As SortOrder
        Get
            Return _order
        End Get
        Set(ByVal Value As SortOrder)
            _order = Value
        End Set
    End Property

    ''' <summary>
    ''' 並び替えの方法
    ''' </summary>
    Public Property Mode() As ComparerMode
        Get
            Return _mode
        End Get
        Set(ByVal Value As ComparerMode)
            _mode = Value
        End Set
    End Property

    ''' <summary>
    ''' 列ごとの並び替えの方法
    ''' </summary>
    Public WriteOnly Property ColumnModes() As ComparerMode()
        Set(ByVal Value As ComparerMode())
            _columnModes = Value
        End Set
    End Property

    ''' <summary>
    ''' ListViewItemComparerクラスのコンストラクタ
    ''' </summary>
    ''' <param name="col">並び替える列番号</param>
    ''' <param name="ord">昇順か降順か</param>
    ''' <param name="cmod">並び替えの方法</param>
    Public Sub New(ByVal col As Integer, ByVal ord As SortOrder, _
            ByVal cmod As ComparerMode)
        _column = col
        _order = ord
        _mode = cmod
    End Sub

    Public Sub New()
        _column = 0
        _order = SortOrder.Ascending
        _mode = ComparerMode.String
    End Sub

    'xがyより小さいときはマイナスの数、大きいときはプラスの数、
    '同じときは0を返す
    Public Function Compare(ByVal x As Object, ByVal y As Object) _
            As Integer Implements IComparer.Compare
        Dim result As Integer = 0
        'ListViewItemの取得
        Dim itemx As ListViewItem = DirectCast(x, ListViewItem)
        Dim itemy As ListViewItem = DirectCast(y, ListViewItem)

        '並べ替えの方法を決定
        If Not (_columnModes Is Nothing) AndAlso _
                _columnModes.Length > _column Then
            _mode = _columnModes(_column)
        End If
        '並び替えの方法別に、xとyを比較する
        Select Case _mode
            Case ComparerMode.String
                result = String.Compare(itemx.SubItems(_column).Text, _
                    itemy.SubItems(_column).Text)
            Case ComparerMode.Integer
                result = Long.Parse(itemx.SubItems(_column).Text).CompareTo(Long.Parse(itemy.SubItems(_column).Text))
            Case ComparerMode.DateTime
                'result = DateTime.Compare( _
                '    DateTime.Parse(itemx.SubItems(_column).Text, datetimeformatinfo), _
                '    DateTime.Parse(itemy.SubItems(_column).Text))
                'result = String.Compare( _
                '    itemx.SubItems(_column).Text, _
                '    itemy.SubItems(_column).Text)
                'StatusID(?)でソート
                result = Long.Parse(itemx.SubItems(5).Text).CompareTo(Long.Parse(itemy.SubItems(5).Text))
            Case ComparerMode.None
                result = 0
        End Select

        '降順の時は結果を+-逆にする
        If _order = SortOrder.Descending Then
            result = -result
        Else
            If _order = SortOrder.None Then
                result = 0
            End If
        End If
        '結果を返す
        Return result
    End Function
End Class
