Imports System.Configuration
Imports System.Text.RegularExpressions
Imports Tween.TweenCustomControl

Public Class TweenMain
    Private clsTw As Twitter            'Twitter用通信データ処理カスタムクラス
    Private clsTwPost As Twitter            'Twitter用通信データ処理カスタムクラス
    Private _username As String         'ユーザー名
    Private _password As String         'パスワード（デクリプト済み）
    Private _mySize As Size             '画面サイズ
    Private _myLoc As Point             '画面位置
    Private _mySpDis As Integer         '区切り位置
    Private _initial As Boolean         'True:起動時処理中
    Private listViewItemSorter As ListViewItemComparer      'リストソート用カスタムクラス
    Private _config As Configuration    'アプリケーション構成ファイルクラス
    Private _section As ListSection     '構成ファイル中のユーザー定義ListSectionクラス
    Private SettingDialog As New Setting()      '設定画面インスタンス
    Private TabDialog As New TabsDialog         'タブ選択ダイアログインスタンス
    Private SearchDialog As New SearchWord()    '検索画面インスタンス
    Private _tabs As New List(Of TabStructure)  '要素TabStructureクラスのジェネリックリストインスタンス（タブ情報用）
    'Private _notId As New Collections.Specialized.StringCollection  'Recentタブから移動されたユーザーIDを保持するコレクションインスタンス
    Private _fntUnread As Font          '未読用フォント
    Private _clUnread As Color          '未読用文字色
    Private _fntReaded As Font          '既読用フォント
    Private _clReaded As Color          '既読用文字色
    Private _clFav As Color             'Fav用文字色
    Private _clOWL As Color             '片思い用文字色
    Private _fntDetail As Font          '発言詳細部用フォント
    Private _clSelf As Color            '自分の発言用背景色
    Private _clAtSelf As Color          '自分宛返信用背景色
    Private _clTarget As Color          '選択発言者の他の発言用背景色
    Private _clAtTarget As Color        '選択発言中の返信先用背景色
    Private _clAtFromTarget As Color    '選択発言者への返信発言用背景色
    Private _postCounter As Integer = 0 '取得発言数カウンタ（カウントしているが未使用。タブ別カウンタに変更＆未読数カウントとして未読アイコン表示パフォーマンスUPできるように改善したい）
    Private TIconList As ImageList      '発言詳細部用アイコン画像リスト
    Private TIconSmallList As ImageList 'リスト表示用アイコン画像リスト
    Private _iconSz As Integer          'アイコンサイズ（現在は16、24、48の3種類。将来直接数字指定可能とする）
    Private _iconCol As Boolean         '1列表示の時True（48サイズのとき）
    Private NIconAt As Icon             'At.ico             タスクトレイアイコン：通常時
    Private NIconAtRed As Icon          'AtRed.ico          タスクトレイアイコン：通信エラー時
    Private NIconAtSmoke As Icon        'AtSmoke.ico        タスクトレイアイコン：オフライン時
    Private NIconRefresh(3) As Icon     'Refresh.ico        タスクトレイアイコン：更新中（アニメーション用に4種類を保持するリスト）
    Private TabIcon As Icon             'Tab.ico            未読のあるタブ用アイコン
    Private MainIcon As Icon            'Main.ico           画面左上のアイコン
    Private _anchorItem As ListViewItem '関連発言移動開始時のリストアイテム
    Private _anchorFlag As Boolean      'True:関連発言移動中（関連移動以外のオペレーションをするとFalseへ。Trueだとリスト背景色をアンカー発言選択中として描画）
    Private _tabDrag As Boolean         'タブドラッグ中フラグ（DoDragDropを実行するかの判定用）
    Private _refreshIconCnt As Integer  '更新中アイコンのアニメーション用カウンタ
    Private _rclickTabName As String    '右クリックしたタブの名前
    Private fDialog As New FilterDialog 'フィルター編集画面
    Private _endingFlag As Boolean      '終了フラグ
    Private _curTabText As String = "Recent"
    Private _history As New Collections.Specialized.StringCollection
    Private _hisIdx As Integer
    Private UrlDialog As New OpenURL
    Private Const _replyHtml As String = "@<a target=""_self"" href=""https://twitter.com/"
    Private _reply_to_id As Integer     ' リプライ先のステータスID 0の場合はリプライではない 注：複数あてのものはリプライではない
    Private _reply_to_name As String    ' リプライ先ステータスの書き込み者の名前
    Private _getDM As Boolean
    Private RemainPostNum As Integer   ' POST残り回数
    Private __PostCounter As Integer = 59  '割り込みカウンタ　タイマ割り込みでカウントダウン
    Private _postTimestamps As New List(Of Date)
    Private _tlTimestamps As New Dictionary(Of Date, Integer)
    Private _tlCount As Integer

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
        Public ids As Collections.Specialized.StringCollection  'Fav追加・削除時のID
        Public sIds As Collections.Specialized.StringCollection 'Fav追加・削除成功分のID
    End Structure

    'Backgroundworkerへ処理内容を通知するための引数用構造体
    Private Structure GetWorkerArg
        Public page As Integer                      '処理対象ページ番号
        Public endPage As Integer                   '処理終了ページ番号（起動時の読み込みページ数。通常時はpageと同じ値をセット）
        Public type As WORKERTYPE                   '処理種別
        Public status As String                     '発言POST時の発言内容
        Public ids As Collections.Specialized.StringCollection  'Fav追加・削除時のID
        Public sIds As Collections.Specialized.StringCollection 'Fav追加・削除成功分のID
        Public tName As String                      'Fav追加・削除時のタブ名
    End Structure

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'OSバージョン判定。ビジュアルスタイルのRenderMode切り替え（メニュー関連の描画方法）
        If System.Environment.OSVersion.Version.Major = 4 Or _
           System.Environment.OSVersion.Version.Major = 5 And System.Environment.OSVersion.Version.Minor = 0 Then
            'Win9x,NT,2k
            ToolStripManager.RenderMode = ToolStripManagerRenderMode.Professional
        ElseIf System.Environment.OSVersion.Version.Major = 5 And System.Environment.OSVersion.Version.Minor > 0 Or _
               System.Environment.OSVersion.Version.Major >= 6 Then
            'XP,2003,Vista
            ToolStripManager.RenderMode = ToolStripManagerRenderMode.System
        End If

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

        _history.Add("")
        _hisIdx = 0
        _reply_to_id = 0
        _reply_to_name = Nothing

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
        'NewReplyPopMenuItem.Checked = _section.NewReplyPop  'Reply新着通知（タブ別設定にするため削除予定）
        'NewDMPopMenuItem.Checked = _section.NewDMPop        'DM新着通知（タブ別設定にするため削除予定）
        ''全新着通知のチェック状態により、Reply＆DMの新着通知有効無効切り替え（タブ別設定にするため削除予定）
        'If NewPostPopMenuItem.Checked Then
        '    NewReplyPopMenuItem.Enabled = False
        '    NewDMPopMenuItem.Enabled = False
        'Else
        '    NewReplyPopMenuItem.Enabled = True
        '    NewDMPopMenuItem.Enabled = True
        'End If

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

        '設定画面への反映
        SettingDialog.UserID = _username                                'ユーザ名
        SettingDialog.PasswordStr = _password                           'パスワード
        SettingDialog.TimelinePeriodInt = _section.TimelinePeriod       'Recent&Reply取得間隔
        SettingDialog.DMPeriodInt = _section.DMPeriod                   'DM取得間隔
        SettingDialog.NextPageThreshold = _section.NextPageThreshold    '次頁以降を取得するための新着件数閾値
        SettingDialog.NextPagesInt = _section.NextPages                 '閾値を超えた場合の取得ページ数
        SettingDialog.MaxPostNum = _section.MaxPostNum                  '時間当たりPOST回数最大値
        RemainPostNum = SettingDialog.MaxPostNum
        'ログ保存関連（扱いが難しいので機能削除）
        'SettingDialog.LogDays = _section.LogDays
        'Select Case _section.LogUnit
        '    Case ListSection.LogUnitEnum.Minute
        '        SettingDialog.LogUnit = Setting.LogUnitEnum.Minute
        '    Case ListSection.LogUnitEnum.Hour
        '        SettingDialog.LogUnit = Setting.LogUnitEnum.Hour
        '    Case ListSection.LogUnitEnum.Day
        '        SettingDialog.LogUnit = Setting.LogUnitEnum.Day
        'End Select
        '起動時読み込みページ数
        SettingDialog.ReadPages = _section.ReadPages            'Recent
        SettingDialog.ReadPagesReply = _section.ReadPagesReply  'Reply
        SettingDialog.ReadPagesDM = _section.ReadPagesDM        'DM
        '起動時読み込み分を既読にするか。Trueなら既読として処理
        SettingDialog.Readed = _section.Readed
        '新着取得時のリストスクロールをするか。Trueならスクロールしない
        ListLockMenuItem.Checked = _section.ListLock
        'リストのアイコンサイズ（いずれ直接数値指定へ）
        'Select Case _section.IconSize
        '    Case IconSizes.IconNone
        '        SettingDialog.IconSz = Setting.IconSizes.IconNone
        '    Case ListSection.IconSizes.Icon16
        '        SettingDialog.IconSz = Setting.IconSizes.Icon16
        '    Case ListSection.IconSizes.Icon24
        '        SettingDialog.IconSz = Setting.IconSizes.Icon24
        '    Case ListSection.IconSizes.Icon48
        '        SettingDialog.IconSz = Setting.IconSizes.Icon48
        '    Case ListSection.IconSizes.Icon48_2
        '        SettingDialog.IconSz = Setting.IconSizes.Icon48_2
        'End Select
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
        '新着通知バルーンに表示する発言者
        'Select Case _section.NameBalloon
        '    Case ListSection.NameBalloonEnum.None
        '        SettingDialog.NameBalloon = Setting.NameBalloonEnum.None
        '    Case ListSection.NameBalloonEnum.UserID
        '        SettingDialog.NameBalloon = Setting.NameBalloonEnum.UserID
        '    Case ListSection.NameBalloonEnum.NickName
        '        SettingDialog.NameBalloon = Setting.NameBalloonEnum.NickName
        'End Select
        SettingDialog.NameBalloon = _section.NameBalloon
        SettingDialog.PostCtrlEnter = _section.PostCtrlEnter
        'SettingDialog.UseAPI = _section.UseAPI
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
            '他の設定項目は、随時設定画面で保持している値を読み出して使用
        End If

        'バージョンチェック（引数：起動時チェックの場合はTrue･･･チェック結果のメッセージを表示しない）
        If SettingDialog.StartupVersion Then
            Call CheckNewVersion(True)
        End If

        'ウィンドウ設定
        Me.WindowState = FormWindowState.Normal     '通常状態
        Me.ClientSize = _section.FormSize           'サイズ設定
        _mySize = Me.ClientSize                     'サイズ保持（最小化・最大化されたまま終了した場合の対応用）
        Me.Location = _section.FormLocation         '位置設定
        _myLoc = Me.Location                        '位置保持（最小化・最大化されたまま終了した場合の対応用）
        Me.SplitContainer1.SplitterDistance = _section.SplitterDistance     'Splitterの位置設定
        _mySpDis = Me.SplitContainer1.SplitterDistance

        '全新着通知のチェック状態により、Reply＆DMの新着通知有効無効切り替え（タブ別設定にするため削除予定）
        If SettingDialog.UnreadManage = False Then
            ReadedStripMenuItem.Enabled = False
            UnreadStripMenuItem.Enabled = False
        End If

        'タイマー設定
        'Recent&Reply取得間隔
        TimerTimeline.Interval = IIf(SettingDialog.TimelinePeriodInt > 0, SettingDialog.TimelinePeriodInt * 1000, 600000)
        'DM取得間隔
        TimerDM.Interval = IIf(SettingDialog.DMPeriodInt > 0, SettingDialog.DMPeriodInt * 1000, 600000)
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
        If SettingDialog.StartupKey Then
            Call clsTw.GetWedata()
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
        listViewItemSorter.Order = _section.SortOrder
        'Timeline.ListViewItemSorter = listViewItemSorter
        'Timeline.Columns(0).Width = _section.Width1
        'Timeline.Columns(0).DisplayIndex = _section.DisplayIndex1
        'If _iconCol = False Then
        '    Timeline.Columns(1).Width = _section.Width2
        '    Timeline.Columns(2).Width = _section.Width3
        '    Timeline.Columns(3).Width = _section.Width4
        '    Timeline.Columns(4).Width = _section.Width5
        '    Timeline.Columns(1).DisplayIndex = _section.DisplayIndex2
        '    Timeline.Columns(2).DisplayIndex = _section.DisplayIndex3
        '    Timeline.Columns(3).DisplayIndex = _section.DisplayIndex4
        '    Timeline.Columns(4).DisplayIndex = _section.DisplayIndex5
        'Else
        '    Timeline.Columns.RemoveAt(4)
        '    Timeline.Columns.RemoveAt(3)
        '    Timeline.Columns.RemoveAt(2)
        '    Timeline.Columns.RemoveAt(1)
        'End If
        'Dim myTabRecent As New TabStructure
        'myTabRecent.notify = _section.ListElement("Recent").Notify
        'myTabRecent.unreadManage = _section.ListElement("Recent").UnreadManage
        'myTabRecent.soundFile = _section.ListElement("Recent").SoundFile
        'myTabRecent.tabName = "Recent"
        'myTabRecent.tabPage = ListTab.TabPages(0)
        'myTabRecent.listCustom = Timeline
        '_tabs.Add(myTabRecent)

        ''Replyタブ
        If _section.ListElement.Item("Reply") Is Nothing Then
            _section.ListElement.Add(New ListElement("Reply"))
        End If
        'Reply.SmallImageList = TIconSmallList
        'Reply.ListViewItemSorter = listViewItemSorter
        'Reply.Columns(0).Width = _section.Width1
        'Reply.Columns(0).DisplayIndex = _section.DisplayIndex1
        'If _iconCol = False Then
        '    Reply.Columns(1).Width = _section.Width2
        '    Reply.Columns(2).Width = _section.Width3
        '    Reply.Columns(3).Width = _section.Width4
        '    Reply.Columns(4).Width = _section.Width5
        '    Reply.Columns(1).DisplayIndex = _section.DisplayIndex2
        '    Reply.Columns(2).DisplayIndex = _section.DisplayIndex3
        '    Reply.Columns(3).DisplayIndex = _section.DisplayIndex4
        '    Reply.Columns(4).DisplayIndex = _section.DisplayIndex5
        'Else
        '    Reply.Columns.RemoveAt(4)
        '    Reply.Columns.RemoveAt(3)
        '    Reply.Columns.RemoveAt(2)
        '    Reply.Columns.RemoveAt(1)
        'End If
        'Dim myTabReply As New TabStructure
        'myTabReply.notify = _section.ListElement("Reply").Notify
        'myTabReply.unreadManage = _section.ListElement("Reply").UnreadManage
        'myTabReply.soundFile = _section.ListElement("Reply").SoundFile
        'myTabReply.tabName = "Reply"
        'myTabReply.tabPage = ListTab.TabPages(1)
        'myTabReply.listCustom = Reply
        '_tabs.Add(myTabReply)

        ''DirectMsgタブ
        If _section.ListElement.Item("Direct") Is Nothing Then
            _section.ListElement.Add(New ListElement("Direct"))
        End If
        'DirectMsg.SmallImageList = TIconSmallList
        'DirectMsg.ListViewItemSorter = listViewItemSorter
        'DirectMsg.Columns(0).Width = _section.Width1
        'DirectMsg.Columns(0).DisplayIndex = _section.DisplayIndex1
        'If _iconCol = False Then
        '    DirectMsg.Columns(1).Width = _section.Width2
        '    DirectMsg.Columns(2).Width = _section.Width3
        '    DirectMsg.Columns(3).Width = _section.Width4
        '    DirectMsg.Columns(4).Width = _section.Width5
        '    DirectMsg.Columns(1).DisplayIndex = _section.DisplayIndex2
        '    DirectMsg.Columns(2).DisplayIndex = _section.DisplayIndex3
        '    DirectMsg.Columns(3).DisplayIndex = _section.DisplayIndex4
        '    DirectMsg.Columns(4).DisplayIndex = _section.DisplayIndex5
        'Else
        '    DirectMsg.Columns.RemoveAt(4)
        '    DirectMsg.Columns.RemoveAt(3)
        '    DirectMsg.Columns.RemoveAt(2)
        '    DirectMsg.Columns.RemoveAt(1)
        'End If
        'Dim myTabDM As New TabStructure
        'myTabDM.notify = _section.ListElement("Direct").Notify
        'myTabDM.unreadManage = _section.ListElement("Direct").UnreadManage
        'myTabDM.soundFile = _section.ListElement("Direct").SoundFile
        'myTabDM.tabName = "Direct"
        'myTabDM.tabPage = ListTab.TabPages(2)
        'myTabDM.listCustom = DirectMsg
        '_tabs.Add(myTabDM)

        Dim idx As Integer = 0
        For idx = 0 To _section.ListElement.Count - 1
            Dim name As String = _section.ListElement(idx).Name
            'If name <> "Recent" And name <> "Reply" And name <> "Direct" Then
            Dim myTab As New TabStructure
            myTab.tabPage = New TabPage
            myTab.listCustom = New DetailsListView
            myTab.colHd1 = New ColumnHeader
            If _iconCol = False Then
                myTab.colHd2 = New ColumnHeader
                myTab.colHd3 = New ColumnHeader
                myTab.colHd4 = New ColumnHeader
                myTab.colHd5 = New ColumnHeader
            End If
            myTab.notify = _section.ListElement(idx).Notify
            myTab.unreadManage = _section.ListElement(idx).UnreadManage
            myTab.soundFile = _section.ListElement(idx).SoundFile
            For Each flt As Tween.SelectedUser In _section.SelectedUser
                If flt.TabName = name Then
                    Dim fcls As New FilterClass
                    Dim bflt() As String = flt.BodyFilter.Split(" ")
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
            'myTab.sorter = New ListViewItemComparer
            myTab.tabName = name
            _tabs.Add(myTab)
            'End If
        Next

        AddCustomTabs()

        'For idx = 0 To _section.SelectedUser.Count - 1
        '    If _section.SelectedUser(idx).Name.StartsWith("Recent->") Then
        '        _notId.Add(_section.SelectedUser(idx).Name.Substring(8))
        '    End If
        'Next

        'PostBrowser.AllowNavigation = False

        'If SettingDialog.LogDays > 0 Then
        '    GetLogWorker.RunWorkerAsync(0)
        'End If

        _initial = True
        If My.Computer.Network.IsAvailable Then
            NotifyIcon1.Icon = NIconRefresh(0)
            _refreshIconCnt = 0
            TimerRefreshIcon.Enabled = True
            Dim args As New GetWorkerArg
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
            Dim args As New GetWorkerArg
            args.type = WORKERTYPE.CreateNewSocket
            Do While GetTimelineWorker.IsBusy
                Threading.Thread.Sleep(100)
                Application.DoEvents()
            Loop
            GetTimelineWorker.RunWorkerAsync(args)
            Do While PostWorker.IsBusy
                Threading.Thread.Sleep(100)
                Application.DoEvents()
            Loop
            PostWorker.RunWorkerAsync(args)
            PostButton.Enabled = True
            'ReplyStripMenuItem.Enabled = True
            'DMStripMenuItem.Enabled = True
            FavAddToolStripMenuItem.Enabled = True
            FavRemoveToolStripMenuItem.Enabled = True
            MoveToHomeToolStripMenuItem.Enabled = True
            MoveToFavToolStripMenuItem.Enabled = True
            DeleteStripMenuItem.Enabled = True
            RefreshStripMenuItem.Enabled = True
            TimerRefreshIcon.Enabled = False
            NotifyIcon1.Icon = NIconAt
            If _initial = False Then
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
                    Threading.Thread.Sleep(100)
                    Application.DoEvents()
                Loop
                GetTimelineWorker.RunWorkerAsync(args)
            End If
        Else
            TimerRefreshIcon.Enabled = False
            NotifyIcon1.Icon = NIconAtSmoke
            PostButton.Enabled = False
            'ReplyStripMenuItem.Enabled = False
            'DMStripMenuItem.Enabled = False
            FavAddToolStripMenuItem.Enabled = False
            FavRemoveToolStripMenuItem.Enabled = False
            MoveToHomeToolStripMenuItem.Enabled = False
            MoveToFavToolStripMenuItem.Enabled = False
            DeleteStripMenuItem.Enabled = False
            RefreshStripMenuItem.Enabled = False
            'TimerDM.Enabled = False
            'TimerTimeline.Enabled = False
        End If
    End Sub

    Private Sub TimerTimeline_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerTimeline.Tick
        If My.Computer.Network.IsAvailable = False Then Exit Sub
        'TimerTimeline.Enabled = False
        Dim args As New GetWorkerArg
        args.page = 1
        args.endPage = 1
        args.type = WORKERTYPE.Timeline
        StatusLabel.Text = "Recent更新中..."
        NotifyIcon1.Icon = NIconRefresh(0)
        _refreshIconCnt = 0
        TimerRefreshIcon.Enabled = True
        Do While GetTimelineWorker.IsBusy
            Threading.Thread.Sleep(100)
            Application.DoEvents()
        Loop
        GetTimelineWorker.RunWorkerAsync(args)
    End Sub

    Private Sub TimerDM_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerDM.Tick
        If My.Computer.Network.IsAvailable = False Then Exit Sub
        'TimerDM.Enabled = False
        GC.Collect()
        Dim args As New GetWorkerArg
        args.page = 1
        args.endPage = 1
        args.type = WORKERTYPE.DirectMessegeRcv
        StatusLabel.Text = "DMRcv更新中..."
        NotifyIcon1.Icon = NIconRefresh(0)
        _refreshIconCnt = 0
        TimerRefreshIcon.Enabled = True
        Do While GetTimelineWorker.IsBusy
            Threading.Thread.Sleep(100)
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
            If clsTw.follower.Count > 1 Then
                If clsTw.follower.Contains(lItem.Name) = False Then
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
            If (_initial = False Or (_initial And readed = False)) And SettingDialog.UnreadManage Then
                _readed = False
            End If
            If _onewaylove And SettingDialog.OneWayLove = True Then
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
                    Dim tBody As String = IIf(ft.SearchURL, lItem.OrgData, lItem.Data)
                    If ft.SearchBoth Then
                        If ft.IDFilter = "" Or lItem.Name = ft.IDFilter Then
                            For Each fs As String In ft.BodyFilter
                                If ft.UseRegex Then
                                    If Regex.IsMatch(tBody, fs, RegexOptions.IgnoreCase) = False Then bHit = False
                                Else
                                    If tBody.ToLower.Contains(fs.ToLower) = False Then bHit = False
                                End If
                                If bHit = False Then Exit For
                            Next
                        Else
                            bHit = False
                        End If
                    Else
                        For Each fs As String In ft.BodyFilter
                            If ft.UseRegex Then
                                If Regex.IsMatch(lItem.Name + tBody, fs, RegexOptions.IgnoreCase) = False Then bHit = False
                            Else
                                If (lItem.Name + tBody).ToLower.Contains(fs.ToLower) = False Then bHit = False
                            End If
                            If bHit = False Then Exit For
                        Next
                    End If
                    If bHit = True Then
                        hit = True
                        If ft.SetMark Then mk = True
                        If ft.moveFrom Then mv = True
                    End If
                    If hit And mv And mk Then Exit For
                Next
                If hit Then
                    ts.allCount += 1
                    Dim lvItem2 As ListViewItem = lvItem.Clone
                    If _readed = False And ts.unreadManage Then
                        lvItem2.Font = _fntUnread
                        lvItem2.ForeColor = _clUnread
                        lvItem2.SubItems(8).Text = "False"
                        'ts.tabPage.ImageIndex = 0
                        ts.unreadCount += 1
                    Else
                        lvItem2.Font = _fntReaded
                        lvItem2.ForeColor = _clReaded
                        lvItem2.SubItems(8).Text = "True"
                        'ts.tabPage.ImageIndex = -1
                    End If
                    If _onewaylove And SettingDialog.OneWayLove = True Then
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

            If lItem.Reply Or Regex.IsMatch(lItem.Data, "@" + _username + "([^a-zA-Z0-9_]|$)", RegexOptions.IgnoreCase) Then
                Dim lvItem2 As ListViewItem = lvItem.Clone
                _tabs(1).allCount += 1
                If _readed = False And _tabs(1).unreadManage Then
                    lvItem2.Font = _fntUnread
                    lvItem2.ForeColor = _clUnread
                    lvItem2.SubItems(8).Text = "False"
                    'ListTab.TabPages(1).ImageIndex = 0
                    _tabs(1).unreadCount += 1
                Else
                    lvItem2.Font = _fntReaded
                    lvItem2.ForeColor = _clReaded
                    lvItem2.SubItems(8).Text = "True"
                    'ListTab.TabPages(1).ImageIndex = -1
                End If
                If _onewaylove And SettingDialog.OneWayLove = True Then
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
                '_reply = True
                RepCnt += 1
            End If

            If mv = False Then
                _tabs(0).allCount += 1
                If _readed = False And _tabs(0).unreadManage Then
                    lvItem.Font = _fntUnread
                    lvItem.ForeColor = _clUnread
                    lvItem.SubItems(8).Text = "False"
                    'ListTab.TabPages(0).ImageIndex = 0
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
                    'ListTab.TabPages(0).ImageIndex = -1
                End If
                If _onewaylove And SettingDialog.OneWayLove = True Then
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
            Dim pmsg As String
            pmsg = nm + " : " + lItem.Data
            If NewPostPopMenuItem.Checked = True And nf = True Then
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
            If Not topItem Is Nothing Then
                If myList.Items.Count > 0 And topItem.Index > -1 Then
                    myList.EnsureVisible(myList.Items.Count - 1)
                    myList.EnsureVisible(topItem.Index)
                End If
            Else

                If listViewItemSorter.Column = 3 And listViewItemSorter.Order = SortOrder.Ascending And myList.Items.Count > 0 Then
                    myList.EnsureVisible(myList.Items.Count - 1)
                End If
            End If
            '新着バルーン通知
            If _initial = False And _pop <> "" Then
                If RepCnt > 0 And _tabs(1).notify Then
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
            If _initial = False And SettingDialog.PlaySound = True And snd <> "" Then
                Try
                    My.Computer.Audio.Play(My.Application.Info.DirectoryPath.ToString() + "\" + snd, AudioPlayMode.Background)
                Catch ex As Exception

                End Try
            End If
            '*** 暫定 ***
            If OldItems = True Then
                StatusLabel.Text = "ログ読込 [" + firstDate + "] ～ [" + endDate + "]"
            End If
            If _initial = True And OldItems = False Then
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
            'Dim fnt As Font
            'Dim fcl As Color

            'fnt = _fntReaded
            'fcl = _clReaded
            '_item.SubItems(8).Text = "True"
            'If _item.SubItems(10).Text = "True" And SettingDialog.OneWayLove Then fcl = _clOWL
            'If _item.SubItems(9).Text = "True" Then fcl = _clFav
            'MyList.ChangeItemStyles(_item.Index, _item.BackColor, fcl, fnt)

            '最古未読ID再設定
            For Each ts As TabStructure In _tabs
                'タブ特定
                If ts.listCustom.Equals(MyList) Then
                    Call ItemReaded(ts, _item)
                    'ts.unreadCount -= 1         '未読数減
                    ''未読数0
                    'If ts.unreadCount = 0 Then
                    '    ts.oldestUnreadItem = Nothing
                    '    If ts.tabPage.ImageIndex = 0 Then ts.tabPage.ImageIndex = -1
                    '    Exit For
                    'End If
                    ''最古未読IDが既読になった場合、新たな最古未読IDを探索
                    'If ts.oldestUnreadItem IsNot Nothing AndAlso ts.oldestUnreadItem.Equals(_item) Then
                    '    Dim stp As Integer = 1
                    '    Dim frmi As Integer = ts.oldestUnreadItem.Index
                    '    Dim toi As Integer = 0
                    '    '日時ソート（＝ID順）の場合
                    '    If listViewItemSorter.Column = 3 Then
                    '        If listViewItemSorter.Order = SortOrder.Ascending Then
                    '            '昇順
                    '            frmi += 1
                    '            toi = ts.listCustom.Items.Count - 1
                    '        Else
                    '            '降順
                    '            stp = -1
                    '            frmi -= 1
                    '        End If
                    '    Else
                    '        '日時以外が基準の場合は頭から探索
                    '        frmi = 0
                    '        toi = ts.listCustom.Items.Count - 1
                    '    End If

                    '    For i As Integer = frmi To toi Step stp
                    '        If ts.listCustom.Items(i).SubItems(8).Text = "False" Then
                    '            ts.oldestUnreadItem = ts.listCustom.Items(i)
                    '            Exit For
                    '        End If
                    '    Next
                    'End If
                    Exit For
                End If
            Next

            ''他のタブに同発言IDがあるかチェック。未読状態の整合性取る
            'If MyList.Name <> "DirectMsg" Then
            '    For Each ts As TabStructure In _tabs
            '        If ts.listCustom.Equals(MyList) = False And ts.tabPage.Text <> "Direct" And ts.unreadCount > 0 Then
            '            Dim rdItem As ListViewItem = Nothing    '今回既読になったアイテム
            '            '最古未読アイテムが既読になったら、次の未読を探索
            '            If ts.oldestUnreadItem IsNot Nothing AndAlso _item.SubItems(5).Text = ts.oldestUnreadItem.SubItems(5).Text Then
            '                ts.unreadCount -= 1
            '                rdItem = ts.oldestUnreadItem
            '                If ts.unreadCount = 0 Then
            '                    ts.oldestUnreadItem = Nothing
            '                Else
            '                    Dim stp As Integer = 1
            '                    Dim frmi As Integer = ts.oldestUnreadItem.Index
            '                    Dim toi As Integer = 0
            '                    '日時ソート（＝ID順）の場合
            '                    If listViewItemSorter.Column = 3 Then
            '                        If listViewItemSorter.Order = SortOrder.Ascending Then
            '                            '昇順
            '                            frmi += 1
            '                            toi = ts.listCustom.Items.Count - 1
            '                        Else
            '                            '降順
            '                            stp = -1
            '                            frmi -= 1
            '                        End If
            '                    Else
            '                        '日時以外が基準の場合は頭から探索
            '                        frmi = 0
            '                        toi = ts.listCustom.Items.Count - 1
            '                    End If

            '                    For i As Integer = frmi To toi Step stp
            '                        If ts.listCustom.Items(i).SubItems(8).Text = "False" Then
            '                            ts.oldestUnreadItem = ts.listCustom.Items(i)
            '                            Exit For
            '                        End If
            '                    Next
            '                End If
            '            Else
            '                '最古未読以外の場合、既読にすべきアイテムが存在するかチェック
            '                Dim stp As Integer = 1
            '                Dim frmi As Integer = 0
            '                Dim toi As Integer = 0
            '                If ts.oldestUnreadItem IsNot Nothing Then
            '                    frmi = ts.oldestUnreadItem.Index
            '                End If
            '                '日時ソート（＝ID順）の場合
            '                If listViewItemSorter.Column = 3 Then
            '                    If listViewItemSorter.Order = SortOrder.Ascending Then
            '                        '昇順
            '                        frmi += 1
            '                        toi = ts.listCustom.Items.Count - 1
            '                    Else
            '                        '降順
            '                        stp = -1
            '                        frmi -= 1
            '                    End If
            '                Else
            '                    '日時以外が基準の場合は頭から探索
            '                    frmi = 0
            '                    toi = ts.listCustom.Items.Count - 1
            '                End If
            '                For i As Integer = frmi To toi Step stp
            '                    If ts.listCustom.Items(i).SubItems(8).Text = "False" Then
            '                        rdItem = ts.listCustom.Items(i)
            '                        ts.unreadCount -= 1
            '                        Exit For
            '                    End If
            '                Next
            '            End If
            '            '既読化
            '            If rdItem IsNot Nothing Then
            '                rdItem.Font = _fntReaded
            '                rdItem.ForeColor = fcl
            '                rdItem.SubItems(8).Text = "True"
            '            End If
            '            'タブ見出しアイコンクリア
            '            If ts.unreadCount = 0 And ts.tabPage.ImageIndex = 0 Then ts.tabPage.ImageIndex = -1
            '        End If
            '    Next
            'End If
        End If

        TimerColorize.Start()

    End Sub

    Private Sub ColorizeList(ByRef DispDetail As Boolean)
        Dim myList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        Dim name As String
        Dim at As New Collections.Specialized.StringCollection
        Dim dTxt As String

        Dim _item As ListViewItem = Nothing
        'Dim dTxt As String

        If myList.SelectedItems.Count > 0 Or _anchorFlag = True Then
            If _anchorFlag = True Then
                _item = _anchorItem
            Else
                _item = myList.SelectedItems(0)
            End If
            dTxt = "<html><head><style type=""text/css"">p {font-family: """ + _fntDetail.Name + """, sans-serif; font-size: " + _fntDetail.Size.ToString + "pt;} --></style></head><body style=""margin:0px""><p>" + _item.SubItems(7).Text + "</p></body></html>"
            'PostedText.Text = _item.SubItems(2).Text
            If DispDetail = True Then
                TimerColorize.Start()
                '''NameLabel.Text = _item.SubItems(1).Text + "/" + _item.SubItems(4).Text
                '''UserPicture.Image = TIconList.Images(_item.SubItems(6).Text)
                '''If myList.Name = "DirectMsg" Then
                '''    NameLabel.Text = _item.SubItems(1).Text
                '''    DateTimeLabel.Text = ""
                '''Else
                '''    NameLabel.Text = _item.SubItems(1).Text + "/" + _item.SubItems(4).Text
                '''    DateTimeLabel.Text = _item.SubItems(3).Text.ToString()
                '''End If

                '''NameLabel.ForeColor = System.Drawing.SystemColors.ControlText
                '''If _item.SubItems(10).Text = "True" And SettingDialog.OneWayLove Then NameLabel.ForeColor = _clOWL
                '''If _item.SubItems(9).Text = "True" Then NameLabel.ForeColor = _clFav

                ''''''''
                ''''        PostBrowser.DocumentText = "<html><head></head><body style=""margin:0px""><font size=""2"" face=""MS UI Gothic"">" + _item.SubItems(7).Text + "</font></body></html>"

                '''If PostBrowser.DocumentText <> dTxt Then
                '''    PostBrowser.Visible = False
                '''    PostBrowser.DocumentText = dTxt
                '''    PostBrowser.Visible = True
                '''End If
                ''''PostBrowser.DocumentText = "<html><head></head><body style=""margin:0px""><font size=""2"" face=""sans-serif"">" + _item.SubItems(7).Text + "</font></body></html>"
            End If

            Dim pos1 As Integer
            Dim pos2 As Integer

            name = _item.SubItems(4).Text
            'data = _item.SubItems(7).Text
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
                If myList.Items(cnt).SubItems(4).Text.Equals(_username, StringComparison.CurrentCultureIgnoreCase) = False Then
                    '自分以外
                    If myList.Items(cnt).SubItems(11).Text = "False" And Regex.IsMatch(myList.Items(cnt).SubItems(2).Text, "@" + _username + "([^a-zA-Z0-9_]|$)") = False Then
                        '通常発言
                        If at.Contains(myList.Items(cnt).SubItems(4).Text) Then
                            '返信先
                            cl = _clAtFromTarget
                        Else
                            If Regex.IsMatch(myList.Items(cnt).SubItems(2).Text, "@" + name + "([^a-zA-Z0-9_]|$)") = False Or name = "" Then
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
            'myList.ChangeItemBackColor(cnt, cl)
            'myList.Items(cnt).BackColor = cl
            'myList.CustomUpdate(cnt)

            'If myList.Items(cnt).BackColor <> cl And myList.Items(cnt).Selected = False Then
            If myList.Items(cnt).BackColor <> cl Then
                myList.ChangeItemBackColor(cnt, cl)
                'myList.Invalidate(myList.Items(cnt).Bounds)
            End If

        Next
        at.Clear()

        'myList.EndUpdate()
    End Sub

    Private Sub PostButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PostButton.Click
        If StatusText.Text.Trim.Length = 0 Then Exit Sub

        _history(_history.Count - 1) = StatusText.Text.Trim

        Dim args As New GetWorkerArg
        args.page = 0
        args.endPage = 0
        args.type = WORKERTYPE.PostMessage
        If (StatusText.Text.StartsWith("D ") = True) Or (My.Computer.Keyboard.ShiftKeyDown = True) Then
            args.status = StatusText.Text.Trim
        ElseIf SettingDialog.UseRecommendStatus() = True Then
            ' 推奨ステータスを使用する
            Dim statregex As New Regex("^0*")
            args.status = StatusText.Text.Trim + " [TWNv" + statregex.Replace(My.Application.Info.Version.Major.ToString + My.Application.Info.Version.Minor.ToString + My.Application.Info.Version.Build.ToString + My.Application.Info.Version.Revision.ToString, "") + "]"
        Else
            ' テキストボックスに入力されている文字列を使用する
            args.status = StatusText.Text.Trim + " " + SettingDialog.Status.Trim
        End If

        Dim regex As New Regex("([^A-Za-z0-9@_:;\-]|^)(get|g|fav|follow|f|on|off|stop|quit|leave|l|whois|w|nudge|n|stats|invite|track|untrack|tracks|tracking|d)([^A-Za-z0-9_:\-]|$)", RegexOptions.IgnoreCase)
        Dim regex2 As New Regex("https?:\/\/[-_.!~*'()a-zA-Z0-9;\/?:\@&=+\$,%#]+")
        Dim regex3 As New Regex("^\S*\*[^A-Za-z0-9]")
        Dim regex4 As New Regex("\*\S*\s\S+\s")
        Dim mc As Match = regex.Match(args.status)
        Dim mc2 As Match = regex2.Match(args.status)
        If mc.Success Then
            If mc2.Success Then
                If mc.Index >= mc2.Index And mc.Index < mc2.Index + mc2.Length Then
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
            'Dim regex2 As New Regex("(get|g|fav|follow|f|on|off|stop|quit|leave|l|whois|w|nudge|n|stats|invite|track|untrack|tracks|tracking|d)", RegexOptions.IgnoreCase)
        End If
        If regex3.IsMatch(args.status) = True Then
            If regex4.IsMatch(args.status) = False Then
                If args.status.EndsWith(" .") = True Then
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
                Threading.Thread.Sleep(100)
                Application.DoEvents()
            Loop
            PostWorker.RunWorkerAsync(args)
        Else
            Do While GetTimelineWorker.IsBusy
                Threading.Thread.Sleep(100)
                Application.DoEvents()
            Loop
            GetTimelineWorker.RunWorkerAsync(args)
        End If


    End Sub

    Private Sub EndToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EndToolStripMenuItem.Click
        Application.Exit()
    End Sub

    Private Sub Tween_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If SettingDialog.CloseToExit = False AndAlso e.CloseReason = CloseReason.UserClosing Then
            e.Cancel = True
            Me.Visible = False

        Else
            _endingFlag = True
            If clsTw IsNot Nothing Then clsTw.Ending = True
            If clsTwPost IsNot Nothing Then clsTwPost.Ending = True

            NotifyIcon1.Visible = False
            Me.Visible = False
            TimerTimeline.Enabled = False
            TimerDM.Enabled = False

            Call SaveConfigs()

            'Do While GetTimelineWorker.IsBusy
            '    Threading.Thread.Sleep(100)
            '    Application.DoEvents()
            'Loop
        End If
    End Sub

    Private Sub NotifyIcon1_BalloonTipClicked(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NotifyIcon1.BalloonTipClicked
        Me.TopMost = True
        Me.Visible = True
        If Me.WindowState = FormWindowState.Minimized Then
            Me.WindowState = FormWindowState.Normal
        End If
        Me.Activate()
        Me.TopMost = False
    End Sub

    Private Sub GetTimelineWorker_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles GetTimelineWorker.DoWork
        If _endingFlag Then
            e.Cancel = True
            Exit Sub
        End If

        Dim ret As String = ""
        Dim tlList As New List(Of Twitter.MyListItem)
        Dim rslt As New GetWorkerResult
        Dim imgs As New ImageList
        Dim getDM As Boolean = False
        imgs.ImageSize = New Size(48, 48)
        imgs.ColorDepth = ColorDepth.Depth32Bit

        Dim args As GetWorkerArg = DirectCast(e.Argument, GetWorkerArg)
        Try
            If args.type = WORKERTYPE.PostMessage Then CheckReplyTo(args.status)
            For i As Integer = 0 To 1
                Select Case args.type
                    Case WORKERTYPE.Timeline
                        ret = clsTw.GetTimeline(tlList, args.page, _initial, args.endPage, Twitter.GetTypes.GET_TIMELINE, TIconList.Images.Keys, imgs, getDM)
                    Case WORKERTYPE.Reply
                        ret = clsTw.GetTimeline(tlList, args.page, _initial, args.endPage, Twitter.GetTypes.GET_REPLY, TIconList.Images.Keys, imgs, getDM)
                    Case WORKERTYPE.DirectMessegeRcv
                        ret = clsTw.GetDirectMessage(tlList, args.page, args.endPage, Twitter.GetTypes.GET_DMRCV, TIconList.Images.Keys, imgs)
                    Case WORKERTYPE.DirectMessegeSnt
                        ret = clsTw.GetDirectMessage(tlList, args.page, args.endPage, Twitter.GetTypes.GET_DMSNT, TIconList.Images.Keys, imgs)
                    Case WORKERTYPE.PostMessage
                        ret = clsTw.PostStatus(args.status, _reply_to_id)
                    Case WORKERTYPE.FavAdd
                        ret = clsTw.PostFavAdd(args.ids(args.page))
                    Case WORKERTYPE.FavRemove
                        ret = clsTw.PostFavRemove(args.ids(args.page))
                    Case WORKERTYPE.CreateNewSocket
                        Call clsTw.CreateNewSocket()
                End Select
                If args.type = WORKERTYPE.PostMessage Then
                    _reply_to_id = 0
                    _reply_to_name = Nothing
                End If
                If ret = "" Or (ret <> "" And (args.type = WORKERTYPE.PostMessage Or args.type = WORKERTYPE.FavAdd Or args.type = WORKERTYPE.FavRemove)) Then Exit For
                Threading.Thread.Sleep(500)
            Next

            If args.type = WORKERTYPE.FavAdd Or args.type = WORKERTYPE.FavRemove Then
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
        Catch ex As Exception
            If _endingFlag Then
                e.Cancel = True
                Exit Sub
            End If
            My.Application.Log.DefaultFileLogWriter.Location = Logging.LogFileLocation.ExecutableDirectory
            My.Application.Log.DefaultFileLogWriter.MaxFileSize = 102400
            My.Application.Log.DefaultFileLogWriter.AutoFlush = True
            My.Application.Log.DefaultFileLogWriter.Append = False
            'My.Application.Log.WriteException(ex, _
            '    Diagnostics.TraceEventType.Critical, _
            '    "Source=" + ex.Source + " StackTrace=" + ex.StackTrace + " InnerException=" + IIf(ex.InnerException Is Nothing, "", ex.InnerException.Message))
            My.Application.Log.WriteException(ex, _
                Diagnostics.TraceEventType.Critical, _
                ex.StackTrace + vbCrLf + Now.ToString + vbCrLf + args.type.ToString + vbCrLf + clsTw.savePost)
            rslt.retMsg = "Tween 例外発生(GetTimelineWorker_DoWork)"
            rslt.TLine = tlList
            rslt.page = args.page
            rslt.endPage = args.endPage
            rslt.type = args.type

            e.Result = rslt
        End Try
    End Sub

    Private Function dmy() As Boolean
        Return False
    End Function

    Private Sub GetTimelineWorker_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles GetTimelineWorker.RunWorkerCompleted
        If _endingFlag Then
            Exit Sub
        End If

        Dim rslt As GetWorkerResult = DirectCast(e.Result, GetWorkerResult)
        Dim args As New GetWorkerArg

        TimerRefreshIcon.Enabled = False
        If My.Computer.Network.IsAvailable Then
            NotifyIcon1.Icon = NIconAt
        Else
            NotifyIcon1.Icon = NIconAtSmoke
        End If

        If rslt.retMsg <> "" Then
            '''''エラー通知方法の変更も設定できるように！
            If My.Computer.Network.IsAvailable Then
                'TimerRefreshIcon.Enabled = False
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
            If rslt.retMsg.StartsWith("Tween 例外発生") Then
                MessageBox.Show("エラーが発生しました。申し訳ありません。ログをexeファイルのある場所にTween.logとして作ったので、kiri.feather@gmail.comまで送っていただけると助かります。ご面倒なら@kiri_featherまでお知らせ頂くだけでも助かります。", "エラー発生", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
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
                    TIconList.Images.Add(strKey, rslt.imgs.Images(strKey).Clone)
                Catch ex As Exception
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
                    If My.Computer.Network.IsAvailable = False Then Exit Sub
                    If SettingDialog.TimelinePeriodInt > 0 And _
                       SettingDialog.CheckReply = False Then
                        TimerTimeline.Enabled = True
                    End If
                    If _initial = True Then
                        _getDM = False
                        If rslt.page + 1 <= rslt.endPage And SettingDialog.ReadPages >= rslt.page + 1 Then
                            If rslt.page Mod 10 = 0 Then
                                Dim flashRslt As Integer
                                flashRslt = Win32Api.FlashWindow(Me.Handle, 1)
                                If MessageBox.Show((rslt.page * 20).ToString + " ポストまで読み込み完了。さらに読み込みますか？", _
                                                   "読み込み継続確認", _
                                                   MessageBoxButtons.YesNo, _
                                                   MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                                    If SettingDialog.CheckReply = True Then
                                        args.page = 1
                                        args.endPage = 1
                                        args.type = WORKERTYPE.Reply
                                        StatusLabel.Text = "Reply更新中..."
                                        NotifyIcon1.Icon = NIconRefresh(0)
                                        _refreshIconCnt = 0
                                        TimerRefreshIcon.Enabled = True
                                        Do While GetTimelineWorker.IsBusy
                                            Threading.Thread.Sleep(100)
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
                                Threading.Thread.Sleep(100)
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
                                Threading.Thread.Sleep(100)
                                Application.DoEvents()
                            Loop
                            GetTimelineWorker.RunWorkerAsync(args)
                        Else
                            _initial = False
                        End If
                    Else
                        _tlTimestamps.Add(Now, statusCount)
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
                        If rslt.page + 1 <= rslt.endPage Then
                            If statusCount = 20 And rslt.page = 1 And SettingDialog.PeriodAdjust Then
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
                                Threading.Thread.Sleep(100)
                                Application.DoEvents()
                            Loop
                            GetTimelineWorker.RunWorkerAsync(args)
                        Else
                            If rslt.page = 1 And statusCount < 18 And SettingDialog.PeriodAdjust Then
                                TimerTimeline.Interval += 2000
                                If TimerTimeline.Interval > SettingDialog.TimelinePeriodInt * 1000 + 60000 Then TimerTimeline.Interval = SettingDialog.TimelinePeriodInt * 1000 + 60000
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
                                    Threading.Thread.Sleep(100)
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
                                        Threading.Thread.Sleep(100)
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
                    If My.Computer.Network.IsAvailable = False Then Exit Sub
                    If _initial = True Then
                        _getDM = False
                        If rslt.page + 1 <= rslt.endPage And SettingDialog.ReadPagesReply >= rslt.page + 1 Then
                            If rslt.page Mod 10 = 0 Then
                                Dim flashRslt As Integer
                                flashRslt = Win32Api.FlashWindow(Me.Handle, 1)
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
                                Threading.Thread.Sleep(100)
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
                                Threading.Thread.Sleep(100)
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
                        If SettingDialog.DMPeriodInt > 0 Or _initial Then TimerDM.Enabled = True
                    End If
                    Exit Sub
                End If
                If My.Computer.Network.IsAvailable = False Then Exit Sub

                TimerDM.Interval = IIf(SettingDialog.DMPeriodInt > 0, SettingDialog.DMPeriodInt * 1000, 600000)

                If (rslt.page < rslt.endPage And _initial = False) Or _
                   (rslt.page + 1 < SettingDialog.ReadPagesDM And _initial = True) Then
                    args.page = rslt.endPage
                    args.endPage = rslt.endPage
                    args.type = WORKERTYPE.DirectMessegeRcv
                    StatusLabel.Text = "DMRcv更新中..."
                    NotifyIcon1.Icon = NIconRefresh(0)
                    _refreshIconCnt = 0
                    TimerRefreshIcon.Enabled = True
                    Do While GetTimelineWorker.IsBusy
                        Threading.Thread.Sleep(100)
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
                    Threading.Thread.Sleep(100)
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
                        If SettingDialog.DMPeriodInt > 0 Or _initial Then TimerDM.Enabled = True
                    End If
                    Exit Sub
                End If
                If My.Computer.Network.IsAvailable = False Then Exit Sub
                If (rslt.page < rslt.endPage And _initial = False) Or _
                   (rslt.page + 1 < SettingDialog.ReadPagesDM And _initial = True) Then
                    args.page = rslt.endPage
                    args.endPage = rslt.endPage
                    args.type = WORKERTYPE.DirectMessegeSnt
                    StatusLabel.Text = "DMSnt更新中..."
                    NotifyIcon1.Icon = NIconRefresh(0)
                    _refreshIconCnt = 0
                    TimerRefreshIcon.Enabled = True
                    Do While GetTimelineWorker.IsBusy
                        Threading.Thread.Sleep(100)
                        Application.DoEvents()
                    Loop
                    GetTimelineWorker.RunWorkerAsync(args)
                    Exit Sub
                End If

                TimerDM.Interval = IIf(SettingDialog.DMPeriodInt > 0, SettingDialog.DMPeriodInt * 1000, 600000)
                If SettingDialog.DMPeriodInt > 0 Then TimerDM.Enabled = True

                If _initial = True Then
                    If SettingDialog.ReadPages > 0 Then
                        args.page = 1
                        args.endPage = 1
                        args.type = WORKERTYPE.Timeline
                        StatusLabel.Text = "Recent更新中..."
                        NotifyIcon1.Icon = NIconRefresh(0)
                        _refreshIconCnt = 0
                        TimerRefreshIcon.Enabled = True
                        Do While GetTimelineWorker.IsBusy
                            Threading.Thread.Sleep(100)
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
                            Threading.Thread.Sleep(100)
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
                    If RemainPostNum > 1 Then RemainPostNum -= 1
                    If TimerPostCounter.Enabled = False Then TimerPostCounter.Enabled = True
                    StatusLabel.Text = "POST完了"
                    _history.Add("")
                    _hisIdx = _history.Count - 1
                    SetMainWindowTitle()
                End If

                args.page = 1
                args.endPage = 1
                args.type = WORKERTYPE.Timeline
                If GetTimelineWorker.IsBusy = False Then
                    'TimerTimeline.Enabled = False
                    StatusLabel.Text = "Recent更新中..."
                    NotifyIcon1.Icon = NIconRefresh(0)
                    _refreshIconCnt = 0
                    TimerRefreshIcon.Enabled = True
                    Do While GetTimelineWorker.IsBusy
                        Threading.Thread.Sleep(100)
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
                        Threading.Thread.Sleep(100)
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
                                        If ListTab.TabPages(idx).Text <> rslt.tName And ListTab.TabPages(idx).Text <> "Direct" Then
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
                        Threading.Thread.Sleep(100)
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
                                    If clsTw.follower.Contains(itm.SubItems(4).Text) Then
                                        flw = True
                                        itm.SubItems(10).Text = "False"
                                    End If
                                    If SettingDialog.UnreadManage And _tabs(idxt).unreadManage And itm.SubItems(8).Text = "False" Then
                                        _cl = _clUnread
                                    Else
                                        _cl = _clReaded
                                    End If
                                    If flw = False And SettingDialog.OneWayLove Then
                                        _cl = _clOWL
                                    End If
                                    itm.ForeColor = _cl
                                    For idx As Integer = 0 To ListTab.TabCount - 1
                                        If ListTab.TabPages(idx).Text <> rslt.tName And ListTab.TabPages(idx).Text <> "Direct" Then
                                            Dim MyList2 As DetailsListView = DirectCast(ListTab.TabPages(idx).Controls(0), DetailsListView)
                                            Dim idxt2 As Integer = 0
                                            Dim _cl2 As Color
                                            For idxt2 = 0 To _tabs.Count - 1
                                                If _tabs(idxt2).tabName = ListTab.TabPages(idx).Text Then
                                                    If SettingDialog.UnreadManage And _tabs(idxt2).unreadManage And itm.SubItems(8).Text = "False" Then
                                                        _cl2 = _clUnread
                                                    Else
                                                        _cl2 = _clReaded
                                                    End If
                                                    If flw = False And SettingDialog.OneWayLove Then
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

    'Private Sub PostedText_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkClickedEventArgs) Handles PostedText.LinkClicked
    '    Try
    '        System.Diagnostics.Process.Start(e.LinkText.Substring(e.LinkText.IndexOf("#http") + 1))
    '    Catch ex As Exception
    '        'MessageBox.Show("ブラウザの起動に失敗、またはタイムアウトしました。" + vbCrLf + ex.ToString())
    '    End Try
    'End Sub

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
        Call MakeReplyOrDirectStatus()
    End Sub

    Private Sub FavAddToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FavAddToolStripMenuItem.Click

        Dim cnt As Integer = 0
        'Dim rtn As String = ""
        'Dim msg As String = ""
        'Dim cnt2 As Integer = 0
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        'Dim MyList2 As DetailsListView = Nothing
        'Dim cnt3 As Integer = 0
        'Dim tabName As String = ListTab.SelectedTab.Text
        'Dim idx As Integer = 0

        If ListTab.SelectedTab.Text = "Direct" Then Exit Sub
        If MyList.SelectedItems.Count = 0 Then Exit Sub

        If MyList.SelectedItems.Count > 1 Then
            If MessageBox.Show("選択された発言をFavoritesに追加します。よろしいですか？", "Fav確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Cancel Then
                Exit Sub
            End If
        End If

        NotifyIcon1.Icon = NIconRefresh(0)
        _refreshIconCnt = 0
        TimerRefreshIcon.Enabled = True
        StatusLabel.Text = "Fav追加中..."

        'Do While GetTimelineWorker.IsBusy
        '    Threading.Thread.Sleep(100)
        '    Application.DoEvents()
        'Loop

        Dim args As New GetWorkerArg
        args.ids = New Collections.Specialized.StringCollection
        args.sIds = New Collections.Specialized.StringCollection
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
            Threading.Thread.Sleep(100)
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
        'Dim _cl As Color

        If ListTab.SelectedTab.Text = "Direct" Then Exit Sub
        If MyList.SelectedItems.Count = 0 Then Exit Sub

        'Select Case ListTab.SelectedIndex
        '    Case 0
        '        MyList = Timeline
        '        MyList2 = Reply
        '    Case 1
        '        MyList = Reply
        '        MyList2 = Timeline
        '    Case 2
        '        Exit Sub
        'End Select

        If MyList.SelectedItems.Count > 1 Then
            If MessageBox.Show("選択された発言をFavoritesから削除します。よろしいですか？", "Fav確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Cancel Then
                Exit Sub
            End If
        End If

        StatusLabel.Text = "Fav削除中..."
        NotifyIcon1.Icon = NIconRefresh(0)
        _refreshIconCnt = 0
        TimerRefreshIcon.Enabled = True

        Dim args As New GetWorkerArg
        args.ids = New Collections.Specialized.StringCollection
        args.sIds = New Collections.Specialized.StringCollection
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
            Threading.Thread.Sleep(100)
            Application.DoEvents()
        Loop

        GetTimelineWorker.RunWorkerAsync(args)

    End Sub

    Private Sub MoveToHomeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveToHomeToolStripMenuItem.Click
        Do While ExecWorker.IsBusy
            Threading.Thread.Sleep(100)
            Application.DoEvents()
        Loop

        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)

        '後でタブ追加して独自読み込みにする
        If MyList.SelectedItems.Count > 0 Then
            ExecWorker.RunWorkerAsync("http://twitter.com/" + MyList.SelectedItems(0).SubItems(4).Text)
            'Try
            '    System.Diagnostics.Process.Start("http://twitter.com/" + MyList.SelectedItems(0).SubItems(4).Text)
            'Catch ex As Exception
            '    '                MessageBox.Show("ブラウザの起動に失敗、またはタイムアウトしました。" + ex.ToString())
            'End Try
        End If
    End Sub

    Private Sub MoveToFavToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveToFavToolStripMenuItem.Click
        Do While ExecWorker.IsBusy
            Threading.Thread.Sleep(100)
            Application.DoEvents()
        Loop

        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)

        '後でタブ追加して独自読み込みにする
        If MyList.SelectedItems.Count > 0 Then
            ExecWorker.RunWorkerAsync("http://twitter.com/" + MyList.SelectedItems(0).SubItems(4).Text + "/favorites")
            'Try
            '    System.Diagnostics.Process.Start("http://twitter.com/" + MyList.SelectedItems(0).SubItems(4).Text + "/favorites")
            'Catch ex As Exception
            '    '                MessageBox.Show("ブラウザの起動に失敗、またはタイムアウトしました。" + ex.ToString())
            'End Try
        End If
    End Sub

    Private Sub Tween_ClientSizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ClientSizeChanged
        If Me.WindowState = FormWindowState.Normal Then
            _mySize = Me.ClientSize
            _mySpDis = Me.SplitContainer1.SplitterDistance
        End If
    End Sub

    'Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click
    '    If PostedText.SelectedText.Length > 0 Then
    '        Clipboard.SetText(PostedText.SelectedText)
    '    End If
    'End Sub

    Private Sub MyList_ColumnClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs)
        '        If ListTab.SelectedTab.Text <> "Direct" Then
        If SettingDialog.SortOrderLock Then Exit Sub

        If _iconCol = False Then
            listViewItemSorter.Column = e.Column
        Else
            listViewItemSorter.Column = 3
        End If

        For Each _tab As TabPage In ListTab.TabPages
            'If _tab.Text <> "Direct" Then
            Dim MyList As DetailsListView = DirectCast(_tab.Controls(0), DetailsListView)
            'CType(MyList.ListViewItemSorter, ListViewItemComparer).Column = e.Column
            'listViewItemSorter.Column = e.Column
            MyList.Sort()
            'End If
        Next
        '        End If
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
            Dim sItem() As String = {"", lItem.Nick, lItem.Data, "", lItem.Name, lItem.Id, lItem.ImageUrl, lItem.OrgData, _readed.ToString, _fav.ToString, _onewaylove.ToString, "False"}
            Dim lvItem As New ListViewItem(sItem)
            lvItem.Font = _fntReaded
            lvItem.ForeColor = _clReaded
            If IsReceive = False Then
                lvItem.ForeColor = _clOWL
            End If
            lvItem.ToolTipText = lItem.Data
            lvItem.ImageKey = lItem.ImageUrl
            _tabs(2).allCount += 1
            If SettingDialog.UnreadManage And _initial = False And _tabs(2).unreadManage Then
                lvItem.Font = _fntUnread
                lvItem.ForeColor = _clUnread
                If IsReceive = False Then
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
            If Not topItem Is Nothing Then
                If _tabs(2).listCustom.Items.Count > 0 And topItem.Index > -1 Then
                    _tabs(2).listCustom.EnsureVisible(_tabs(2).listCustom.Items.Count - 1)
                    _tabs(2).listCustom.EnsureVisible(topItem.Index)
                End If
            Else
                If listViewItemSorter.Column = 3 And listViewItemSorter.Order = SortOrder.Ascending And _tabs(2).listCustom.Items.Count > 0 Then
                    _tabs(2).listCustom.EnsureVisible(_tabs(2).listCustom.Items.Count - 1)
                End If
            End If
            If _initial = False And NewPostPopMenuItem.Checked And _tabs(2).notify Then
                NotifyIcon1.BalloonTipIcon = ToolTipIcon.Warning
                NotifyIcon1.BalloonTipTitle = "Tween [DM] 新着 " + newCnt.ToString() + "件"
                NotifyIcon1.BalloonTipText = _pop
                NotifyIcon1.ShowBalloonTip(500)
            End If

        End If
        If _initial = False And SettingDialog.PlaySound = True And _tabs(2).soundFile <> "" Then
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
        Call MakeReplyOrDirectStatus(False, True)
    End Sub

    Private Sub DMStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DMStripMenuItem.Click
        Call MakeReplyOrDirectStatus(False, False)
    End Sub

    Private Sub DeleteStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteStripMenuItem.Click
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)

        If MessageBox.Show("選択されている発言(DM)を削除してもよろしいですか？" + vbCrLf + _
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
                    rtn = clsTw.RemoveStatus(MyList.SelectedItems(cnt2).SubItems(5).Text)
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
                            If ts.listCustom.Equals(MyList) = False And ts.tabPage.Text <> "Direct" Then
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
                StatusLabel.Text = "削除失敗 " + msg
            Else
                StatusLabel.Text = "削除成功"
            End If
        ElseIf ListTab.SelectedTab.Text = "Direct" Then
            Dim cnt As Integer = 0
            Dim cnt2 As Integer = 0
            Dim rtn As String = ""
            Dim msg As String = ""

            For cnt = 0 To MyList.SelectedItems.Count - 1
                rtn = clsTw.RemoveDirectMessage(MyList.SelectedItems(cnt2).SubItems(5).Text)
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
                StatusLabel.Text = "削除失敗 " + msg
            Else
                StatusLabel.Text = "削除成功"
            End If
        End If

        TimerRefreshIcon.Enabled = False
        NotifyIcon1.Icon = NIconAt

    End Sub

    Private Sub ReadedStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReadedStripMenuItem.Click
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)

        'Select Case ListTab.SelectedIndex
        '    Case 0
        '        MyList = Timeline
        '    Case 1
        '        MyList = Reply
        '    Case 2
        '        MyList = DirectMsg
        'End Select

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
                Call ItemReaded(_tabs(idx), lItem)
                'If lItem.SubItems(8).Text = "False" Then
                '    lItem.SubItems(8).Text = "True"
                '    lItem.Font = _fntReaded
                '    Dim fcl As Color = _clReaded
                '    If lItem.SubItems(10).Text = "True" And SettingDialog.OneWayLove Then fcl = _clOWL
                '    If lItem.SubItems(9).Text = "True" Then fcl = _clFav
                '    lItem.ForeColor = fcl
                '    _tabs(idx).unreadCount -= 1
                '    If _tabs(idx).oldestUnreadItem IsNot Nothing AndAlso _tabs(idx).oldestUnreadItem.Equals(lItem) Or _
                '       _tabs(idx).oldestUnreadItem Is Nothing And _tabs(idx).unreadCount > 0 Then
                '        '次の未読アイテム探索
                '        Dim stp As Integer = 1
                '        Dim frmi As Integer = 0
                '        Dim toi As Integer = 0
                '        If _tabs(idx).oldestUnreadItem IsNot Nothing Then
                '            frmi = _tabs(idx).oldestUnreadItem.Index
                '        End If
                '        '日時ソート（＝ID順）の場合
                '        If listViewItemSorter.Column = 3 Then
                '            If listViewItemSorter.Order = SortOrder.Ascending Then
                '                '昇順
                '                If _tabs(idx).oldestUnreadItem Is Nothing Then
                '                    frmi = 0
                '                Else
                '                    frmi += 1
                '                End If
                '                toi = _tabs(idx).listCustom.Items.Count - 1
                '            Else
                '                '降順
                '                stp = -1
                '                If _tabs(idx).oldestUnreadItem Is Nothing Then
                '                    frmi = _tabs(idx).listCustom.Items.Count - 1
                '                Else
                '                    frmi -= 1
                '                End If
                '            End If
                '        Else
                '            '日時以外が基準の場合は頭から探索
                '            frmi = 0
                '            toi = _tabs(idx).listCustom.Items.Count - 1
                '        End If
                '        _tabs(idx).oldestUnreadItem = Nothing
                '        For i As Integer = frmi To toi Step stp
                '            If _tabs(idx).listCustom.Items(i).SubItems(8).Text = "False" Then
                '                _tabs(idx).oldestUnreadItem = _tabs(idx).listCustom.Items(i)
                '                Exit For
                '            End If
                '        Next
                '    End If
                '    If MyList.Name <> "DirectMsg" Then
                '        '全タブの未読状態を合わせる
                '        For Each ts As TabStructure In _tabs
                '            If ts.listCustom.Equals(MyList) = False And ts.tabName <> "Direct" And ts.unreadCount > 0 Then
                '                '最古未読アイテムから探索
                '                Dim stp As Integer = 1
                '                Dim frmi As Integer = 0
                '                Dim toi As Integer = 0
                '                If ts.oldestUnreadItem IsNot Nothing Then frmi = ts.oldestUnreadItem.Index
                '                '日時ソート（＝ID順）の場合
                '                If listViewItemSorter.Column = 3 Then
                '                    If listViewItemSorter.Order = SortOrder.Ascending Then
                '                        '昇順
                '                        If ts.oldestUnreadItem Is Nothing Then frmi = 0
                '                        toi = ts.listCustom.Items.Count - 1
                '                    Else
                '                        '降順
                '                        If ts.oldestUnreadItem Is Nothing Then frmi = ts.listCustom.Items.Count - 1
                '                        stp = -1
                '                    End If
                '                Else
                '                    '日時以外が基準の場合は頭から探索
                '                    frmi = 0
                '                    toi = ts.listCustom.Items.Count - 1
                '                End If
                '                For i As Integer = frmi To toi Step stp
                '                    If ts.listCustom.Items(i).SubItems(5).Text = lItem.SubItems(5).Text Then
                '                        If ts.listCustom.Items(i).SubItems(8).Text = "False" Then
                '                            ts.unreadCount -= 1
                '                            ts.listCustom.Items(i).SubItems(8).Text = "True"
                '                            ts.listCustom.Items(i).Font = _fntReaded
                '                            ts.listCustom.Items(i).ForeColor = fcl
                '                            If i = frmi And ts.unreadCount > 0 Then
                '                                Dim stp2 As Integer = stp
                '                                Dim frmi2 As Integer = frmi
                '                                Dim toi2 As Integer = toi
                '                                ts.oldestUnreadItem = Nothing
                '                                For i2 As Integer = frmi2 To toi2 Step stp2
                '                                    If ts.listCustom.Items(i2).SubItems(8).Text = "False" Then
                '                                        ts.oldestUnreadItem = ts.listCustom.Items(i)
                '                                        Exit For
                '                                    End If
                '                                Next
                '                            End If
                '                        End If
                '                        Exit For
                '                    End If
                '                Next
                '                If ts.unreadCount = 0 AndAlso ts.tabPage.ImageIndex = 0 Then
                '                    ts.tabPage.ImageIndex = -1
                '                End If
                '            End If
                '        Next
                '    End If
                'End If
            Next
        End If

        '■■■■ItemReadedでやっているが、別にやった方がいいかも■■■
        'If _tabs(idx).unreadCount = 0 AndAlso _tabs(idx).tabPage.ImageIndex = 0 Then
        '    _tabs(idx).tabPage.ImageIndex = -1
        'End If

    End Sub

    Private Sub ItemReaded(ByVal ts As TabStructure, ByVal lItem As ListViewItem)
        If ts.unreadManage = False Or SettingDialog.UnreadManage = False Then Exit Sub

        If lItem.SubItems(8).Text = "False" Then
            lItem.SubItems(8).Text = "True"
            'lItem.Font = _fntReaded
            Dim fcl As Color = _clReaded
            If lItem.SubItems(10).Text = "True" And SettingDialog.OneWayLove Then fcl = _clOWL
            If lItem.SubItems(9).Text = "True" Then fcl = _clFav
            'lItem.ForeColor = fcl
            ts.listCustom.ChangeItemStyles(lItem.Index, lItem.BackColor, fcl, _fntReaded)
            ts.unreadCount -= 1
            If ts.oldestUnreadItem IsNot Nothing AndAlso ts.oldestUnreadItem.Equals(lItem) Or _
               ts.oldestUnreadItem Is Nothing And ts.unreadCount > 0 Then
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
                    If ts2.listCustom.Equals(ts.listCustom) = False And ts2.tabName <> "Direct" And ts2.unreadCount > 0 And ts2.unreadManage Then
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
                                    'ts2.listCustom.Items(i).Font = _fntReaded
                                    'ts2.listCustom.Items(i).ForeColor = fcl
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

        If _tabs(idx).unreadManage = False Or SettingDialog.UnreadManage = False Then Exit Sub

        If MyList.SelectedItems.Count > 0 Then
            For Each lItem As ListViewItem In MyList.SelectedItems
                If lItem.SubItems(8).Text = "True" Then
                    lItem.SubItems(8).Text = "False"
                    'lItem.Font = _fntUnread
                    Dim fcl As Color = _clUnread
                    If lItem.SubItems(10).Text = "True" And SettingDialog.OneWayLove Then fcl = _clOWL
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
                            If ts.listCustom.Equals(MyList) = False And ts.tabName <> "Direct" And ts.unreadManage Then
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

    Private Sub GetLogWorker_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles GetLogWorker.DoWork
        'Dim cnt As Integer = 0
        'Dim tline As New List(Of Twitter.MyListItem)
        'Dim newTLine As New List(Of Twitter.MyListItem)
        'Dim toCnt As Integer = CInt(e.Argument) + 100
        'Dim rslt As New GetWorkerResult
        'Dim dtold As DateTime

        'Select Case SettingDialog.LogUnit
        '    Case Setting.LogUnitEnum.Minute
        '        dtold = Now().AddMinutes(SettingDialog.LogDays * -1)
        '    Case Setting.LogUnitEnum.Hour
        '        dtold = Now().AddHours(SettingDialog.LogDays * -1)
        '    Case Setting.LogUnitEnum.Day
        '        dtold = Now().AddDays(SettingDialog.LogDays * -1)
        'End Select

        'For cnt = CInt(e.Argument) To _section.LogPosts.Count - 1
        '    If _section.LogPosts(cnt).Name <> "" Then

        '        If cnt >= toCnt Then Exit For

        '        Dim lItem As New Twitter.MyListItem
        '        lItem.Fav = IIf(_section.LogPosts(cnt).IsFav = LogPost.Fav.Fav, True, False)
        '        lItem.Id = _section.LogPosts(cnt).Id
        '        lItem.ImageUrl = _section.LogPosts(cnt).ImageUrl
        '        lItem.Name = _section.LogPosts(cnt).Name2
        '        lItem.Nick = _section.LogPosts(cnt).Nick
        '        lItem.OrgData = _section.LogPosts(cnt).Post
        '        lItem.PDate = CDate(_section.LogPosts(cnt).PostDate)
        '        lItem.Unread = IIf(_section.LogPosts(cnt).IsUnread = LogPost.Unread.Unread, True, False)
        '        If lItem.PDate > dtold Then
        '            tline.Add(lItem)
        '        End If
        '    End If
        'Next
        'If tline.Count > 0 Then
        '    clsTw.SetOldTimeline(tline, newTLine)
        '    rslt.TLine = newTLine
        '    rslt.page = cnt
        '    e.Result = rslt
        'Else
        '    e.Result = Nothing
        'End If

    End Sub

    Private Sub GetLogWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles GetLogWorker.RunWorkerCompleted
        'If Not e.Result Is Nothing Then
        '    RefreshTimeline(CType(e.Result, GetWorkerResult).TLine, True)
        '    Do While GetTimelineWorker.IsBusy Or GetDMWorker.IsBusy
        '        Threading.Thread.Sleep(100)
        '        Application.DoEvents()
        '    Loop
        '    GetLogWorker.RunWorkerAsync(CType(e.Result, GetWorkerResult).page)
        'Else
        '    StatusLabel.Text = "ログ読込完了"
        'End If
    End Sub

    Private Sub RefreshStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshStripMenuItem.Click
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)

        NotifyIcon1.Icon = NIconRefresh(0)
        _refreshIconCnt = 0
        TimerRefreshIcon.Enabled = True

        If ListTab.SelectedTab.Text <> "Direct" Then
            If ListTab.SelectedTab.Text <> "Reply" Then
                'TimerTimeline.Enabled = False
                Dim args As New GetWorkerArg
                args.page = 1
                args.endPage = 1
                args.type = WORKERTYPE.Timeline
                StatusLabel.Text = "Recent更新中..."
                'Do While GetTimelineWorker.IsBusy Or GetDMWorker.IsBusy Or GetLogWorker.IsBusy
                Do While GetTimelineWorker.IsBusy
                    Threading.Thread.Sleep(100)
                    Application.DoEvents()
                Loop
                GetTimelineWorker.RunWorkerAsync(args)
            Else
                'TimerTimeline.Enabled = False
                Dim args As New GetWorkerArg
                args.page = 1
                args.endPage = 1
                args.type = WORKERTYPE.Reply
                StatusLabel.Text = "Reply更新中..."
                'Do While GetTimelineWorker.IsBusy Or GetDMWorker.IsBusy Or GetLogWorker.IsBusy
                Do While GetTimelineWorker.IsBusy
                    Threading.Thread.Sleep(100)
                    Application.DoEvents()
                Loop
                GetTimelineWorker.RunWorkerAsync(args)
            End If
        Else
            'TimerDM.Enabled = False
            Dim args As New GetWorkerArg
            args.page = 1
            args.endPage = 1
            args.type = WORKERTYPE.DirectMessegeRcv
            StatusLabel.Text = "DMRcv更新中..."
            'Do While GetTimelineWorker.IsBusy Or GetDMWorker.IsBusy Or GetLogWorker.IsBusy
            Do While GetTimelineWorker.IsBusy
                Threading.Thread.Sleep(100)
                Application.DoEvents()
            Loop
            GetTimelineWorker.RunWorkerAsync(args)
        End If

        TimerRefreshIcon.Enabled = False
        NotifyIcon1.Icon = NIconAt
    End Sub

    Private Sub SettingStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingStripMenuItem.Click
        TimerColorize.Stop()

        If SettingDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            _username = SettingDialog.UserID
            _password = SettingDialog.PasswordStr
            clsTw.Username = _username
            clsTw.Password = _password
            clsTwPost.Username = _username
            clsTwPost.Password = _password
            'TimerTimeline.Interval = IIf(SettingDialog.TimelinePeriodInt > 0, SettingDialog.TimelinePeriodInt * 1000, 600000)
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
            clsTw.TinyUrlResolve = SettingDialog.TinyUrlResolve
            clsTw.ProxyType = SettingDialog.ProxyType
            clsTw.ProxyAddress = SettingDialog.ProxyAddress
            clsTw.ProxyPort = SettingDialog.ProxyPort
            clsTw.ProxyUser = SettingDialog.ProxyUser
            clsTw.ProxyPassword = SettingDialog.ProxyPassword
            Dim args As New GetWorkerArg
            args.type = WORKERTYPE.CreateNewSocket
            Do While GetTimelineWorker.IsBusy
                Threading.Thread.Sleep(100)
                Application.DoEvents()
            Loop
            GetTimelineWorker.RunWorkerAsync(args)
            clsTwPost.ProxyType = SettingDialog.ProxyType
            clsTwPost.ProxyAddress = SettingDialog.ProxyAddress
            clsTwPost.ProxyPort = SettingDialog.ProxyPort
            clsTwPost.ProxyUser = SettingDialog.ProxyUser
            clsTwPost.ProxyPassword = SettingDialog.ProxyPassword
            Do While PostWorker.IsBusy
                Threading.Thread.Sleep(100)
                Application.DoEvents()
            Loop
            PostWorker.RunWorkerAsync(args)
            'If isz <> SettingDialog.IconSz Then
            '    Select Case SettingDialog.IconSz
            '        Case Setting.IconSizes.IconNone
            '            _iconSz = 0
            '        Case Setting.IconSizes.Icon16
            '            _iconSz = 16
            '        Case Setting.IconSizes.Icon24
            '            _iconSz = 26
            '        Case Setting.IconSizes.Icon48
            '            _iconSz = 48
            '    End Select
            '    ChangeImageSize()

            '    Dim idx As Integer = 0
            '    For idx = 0 To ListTab.TabCount - 1
            '        Dim myList As DetailsListView = CType(ListTab.TabPages(idx).Controls(0), DetailsListView)
            '        myList.SmallImageList = TIconSmallList
            '    Next

            '    Dim myList2 As DetailsListView = CType(ListTab.SelectedTab.Controls(0), DetailsListView)
            '    If myList2.SelectedItems.Count > 0 Then
            '        myList2.EnsureVisible(myList2.SelectedItems(0).Index)
            '    Else
            '        If myList2.Items.Count > 0 Then
            '            myList2.EnsureVisible(0)
            '        End If
            '    End If

            '    CType(ListTab.SelectedTab.Controls(0), DetailsListView).Refresh()
            '    'CType(ListTab.SelectedTab.Controls(0), ListView).Focus()
            'End If
            If SettingDialog.UnreadManage = False Then
                ReadedStripMenuItem.Enabled = False
                UnreadStripMenuItem.Enabled = False
                For Each myTab As TabPage In ListTab.TabPages
                    myTab.ImageIndex = -1
                Next
            Else
                ReadedStripMenuItem.Enabled = True
                UnreadStripMenuItem.Enabled = True
            End If
            If SettingDialog.OneWayLove = True Then
                For Each myTab As TabPage In ListTab.TabPages
                    If myTab.Text <> "Direct" Then
                        Dim myList As DetailsListView = DirectCast(myTab.Controls(0), DetailsListView)
                        For Each myItem As ListViewItem In myList.Items
                            If clsTw.follower.Contains(myItem.SubItems(4).Text) Then
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
            For Each ts As TabStructure In _tabs
                For Each myItem As ListViewItem In ts.listCustom.Items
                    If SettingDialog.UnreadManage = True And ts.unreadManage Then
                        'If myItem.SubItems(8).Text = "True" Then
                        '    myItem.ForeColor = _clReaded
                        '    myItem.Font = _fntReaded
                        'Else
                        '    myItem.ForeColor = _clUnread
                        '    myItem.Font = _fntUnread
                        'End If
                    Else
                        'myItem.ForeColor = _clReaded
                        'myItem.Font = _fntReaded
                        Dim fcl As Color = _clReaded
                        If myItem.SubItems(10).Text = "True" And SettingDialog.OneWayLove Then fcl = _clOWL
                        If myItem.SubItems(9).Text = "True" Then fcl = _clFav
                        ts.listCustom.ChangeItemStyles(myItem.Index, myItem.BackColor, fcl, _fntReaded)
                        ts.oldestUnreadItem = Nothing
                        ts.unreadCount = 0
                    End If
                Next
            Next
            'ColorizeList(False)
            TimerColorize.Start()

            SetMainWindowTitle()
            SetNotifyIconText()
        End If

        Call SaveConfigs()
    End Sub

    Private Sub PostBrowser_Navigating(ByVal sender As System.Object, ByVal e As System.Windows.Forms.WebBrowserNavigatingEventArgs) Handles PostBrowser.Navigating
        If e.Url.AbsoluteUri <> "about:blank" Then
            e.Cancel = True
            Do While ExecWorker.IsBusy
                Threading.Thread.Sleep(100)
                Application.DoEvents()
            Loop
            ExecWorker.RunWorkerAsync(e.Url.AbsoluteUri)
            'Try
            '    System.Diagnostics.Process.Start(e.Url.AbsoluteUri)
            'Catch ex As Exception
            '    'MessageBox.Show("ブラウザの起動に失敗、またはタイムアウトしました。" + vbCrLf + ex.ToString())
            'End Try
        End If

    End Sub

    Private Sub AddCustomTabs()
        Dim cnt As Integer = 0

        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.ListTab.SuspendLayout()
        'Me.TabPage1.SuspendLayout()
        Me.SuspendLayout()

        For Each myTab As TabStructure In _tabs
            'If myTab.tabName <> "Recent" And _
            '   myTab.tabName <> "Reply" And _
            '   myTab.tabName <> "Direct" Then
            cnt += 1
            myTab.tabPage.SuspendLayout()

            Me.ListTab.Controls.Add(myTab.tabPage)

            myTab.tabPage.Controls.Add(myTab.listCustom)
            'myTab.tabPage.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            myTab.tabPage.Location = New System.Drawing.Point(4, 4)
            myTab.tabPage.Name = "CTab" + cnt.ToString
            myTab.tabPage.Size = New System.Drawing.Size(380, 260)
            myTab.tabPage.TabIndex = 2 + cnt.ToString
            myTab.tabPage.Text = myTab.tabName
            myTab.tabPage.UseVisualStyleBackColor = True

            myTab.listCustom.AllowColumnReorder = True
            If _iconCol = False Then
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
            AddHandler myTab.listCustom.MouseClick, AddressOf MyList_MouseDown

            myTab.colHd1.Text = ""
            myTab.colHd1.Width = 26
            If _iconCol = False Then
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
            'myTab.sorter.ColumnModes = New ListViewItemComparer.ComparerMode() _
            '        {ListViewItemComparer.ComparerMode.None, _
            '         ListViewItemComparer.ComparerMode.String, _
            '         ListViewItemComparer.ComparerMode.String, _
            '         ListViewItemComparer.ComparerMode.DateTime, _
            '         ListViewItemComparer.ComparerMode.String}
            'myTab.sorter.Column = _section.ListElement("Recent").SortColumn
            'myTab.sorter.Order = _section.ListElement("Recent").SortOrder
            'myTab.listCustom.ListViewItemSorter = myTab.sorter
            myTab.listCustom.ListViewItemSorter = listViewItemSorter
            myTab.listCustom.Columns(0).Width = _section.Width1
            myTab.listCustom.Columns(0).DisplayIndex = _section.DisplayIndex1
            If _iconCol = False Then
                myTab.listCustom.Columns(1).Width = _section.Width2
                myTab.listCustom.Columns(2).Width = _section.Width3
                myTab.listCustom.Columns(3).Width = _section.Width4
                myTab.listCustom.Columns(4).Width = _section.Width5
                myTab.listCustom.Columns(1).DisplayIndex = _section.DisplayIndex2
                myTab.listCustom.Columns(2).DisplayIndex = _section.DisplayIndex3
                myTab.listCustom.Columns(3).DisplayIndex = _section.DisplayIndex4
                myTab.listCustom.Columns(4).DisplayIndex = _section.DisplayIndex5
            End If

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

        If tabName = "(新規タブ)" Or _
           tabName = "Recent" Or _
           tabName = "Reply" Or _
           tabName = "Direct" Then Return False

        Dim myTab As New TabStructure

        myTab.tabPage = New TabPage
        myTab.listCustom = New DetailsListView
        myTab.colHd1 = New ColumnHeader
        If _iconCol = False Then
            myTab.colHd2 = New ColumnHeader
            myTab.colHd3 = New ColumnHeader
            myTab.colHd4 = New ColumnHeader
            myTab.colHd5 = New ColumnHeader
        End If
        'myTab.sorter = New ListViewItemComparer
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
        'myTab.tabPage.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        myTab.tabPage.Location = New System.Drawing.Point(4, 4)
        myTab.tabPage.Name = "CTab" + cnt.ToString()
        myTab.tabPage.Size = New System.Drawing.Size(380, 260)
        myTab.tabPage.TabIndex = 2 + cnt
        myTab.tabPage.Text = myTab.tabName
        myTab.tabPage.UseVisualStyleBackColor = True

        myTab.listCustom.AllowColumnReorder = True
        If _iconCol = False Then
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
        AddHandler myTab.listCustom.MouseClick, AddressOf MyList_MouseDown

        myTab.colHd1.Text = ""
        myTab.colHd1.Width = 26
        If _iconCol = False Then
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
        'myTab.sorter.ColumnModes = New ListViewItemComparer.ComparerMode() _
        '        {ListViewItemComparer.ComparerMode.None, _
        '         ListViewItemComparer.ComparerMode.String, _
        '         ListViewItemComparer.ComparerMode.String, _
        '         ListViewItemComparer.ComparerMode.DateTime, _
        '         ListViewItemComparer.ComparerMode.String}
        'myTab.sorter.Column = _section.ListElement("Recent").SortColumn
        'myTab.sorter.Order = _section.ListElement("Recent").SortOrder
        'myTab.listCustom.ListViewItemSorter = myTab.sorter
        myTab.listCustom.ListViewItemSorter = listViewItemSorter
        myTab.listCustom.Columns(0).Width = _tabs(0).listCustom.Columns(0).Width
        myTab.listCustom.Columns(0).DisplayIndex = _tabs(0).listCustom.Columns(0).DisplayIndex
        If _iconCol = False Then
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
        'Me.TabPage1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

        Return True
    End Function

    Private Sub RemoveSpecifiedTab(ByVal TabName As String)
        Dim idx As Integer = 0

        If TabName = "Recent" Or _
           TabName = "Reply" Or _
           TabName = "Direct" Then Exit Sub

        If MessageBox.Show("このタブを削除してもよろしいですか？" + vbCrLf + _
                        "（このタブの発言はRecentへ戻されます。）", "タブの削除確認", _
                         MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        Call SetListProperty()

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
            If otherEx = False Then
                _tabs(0).listCustom.Items.Add(itm.Clone)
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
        RemoveHandler _tabs(idx).listCustom.MouseClick, AddressOf MyList_MouseDown

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
        If _iconCol = False Then
            _tabs(idx).colHd2.Dispose()
            _tabs(idx).colHd3.Dispose()
            _tabs(idx).colHd4.Dispose()
            _tabs(idx).colHd5.Dispose()
        End If
        '_tabs(idx).idCol.Clear()
        '_tabs(idx).idCol = Nothing
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
                If rect.Left <= cpos.X And cpos.X <= rect.Right And _
                   rect.Top <= cpos.Y And cpos.Y <= rect.Bottom Then
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

        Call SetListProperty()

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
            Call SetStatusLabel()
        End If
    End Sub

    Private Sub ExecWorker_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles ExecWorker.DoWork
        Dim myPath As String = CStr(e.Argument)

        Try
            If SettingDialog.BrowserPath <> "" Then
                'System.Diagnostics.Process.Start(SettingDialog.BrowserPath, myPath)
                Shell(SettingDialog.BrowserPath & " " & myPath)
            Else
                System.Diagnostics.Process.Start(myPath)
            End If
        Catch ex As Exception
            '                MessageBox.Show("ブラウザの起動に失敗、またはタイムアウトしました。" + ex.ToString())
        End Try
    End Sub

    Private Sub StatusText_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles StatusText.KeyPress
        'If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Enter) Then
        '    e.Handled = True
        'End If
    End Sub

    Private Sub StatusText_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles StatusText.KeyUp
        If e.Alt = False And e.Control = False And e.Shift = False Then
            If e.KeyCode = Keys.Space Or e.KeyCode = Keys.ProcessKey Then
                If StatusText.Text = " " Or StatusText.Text = "　" Then
                    e.Handled = True
                    'e.SuppressKeyPress = True
                    StatusText.Text = ""
                    Call JumpUnreadMenuItem_Click(Nothing, Nothing)
                End If
            End If
        End If
    End Sub

    Private Sub StatusText_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusText.TextChanged
        Dim pLen As Integer = 140 - StatusText.Text.Length
        lblLen.Text = pLen.ToString
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
        'e.DrawDefault = True
        If _iconSz = 48 Or _
           _iconSz = 26 Then
            If e.State = 0 Then Exit Sub



            'Dim dspstr As String = e.Item.SubItems(0).Text + " " + e.Item.SubItems(1).Text + "/" + e.Item.SubItems(4).Text + "     " + e.Item.SubItems(3).Text + vbCrLf + _
            '                       e.Item.SubItems(2).Text

            'Dim rct As New Rectangle(e.Item.Bounds.X + 2, e.Item.Bounds.Y + 1, MyList.SmallImageList.ImageSize.Width, MyList.SmallImageList.ImageSize.Height)
            'Dim sRct As New Rectangle(e.Item.Bounds.X + MyList.SmallImageList.ImageSize.Width + 5, e.Item.Bounds.Y + 1, e.Item.Bounds.Width - MyList.SmallImageList.ImageSize.Width - 7, e.Item.Bounds.Height - 2)

            'sf.Alignment = StringAlignment.Near
            'sf.LineAlignment = StringAlignment.Near

            'If e.Item.Selected = True Then
            '    brs = New SolidBrush(Color.FromKnownColor(KnownColor.Highlight))
            '    e.Graphics.FillRectangle(brs, e.Bounds)
            '    brs.Dispose()
            '    If MyList.SmallImageList.Images.ContainsKey(e.Item.ImageKey) Then
            '        e.Graphics.DrawImageUnscaledAndClipped(MyList.SmallImageList.Images(e.Item.ImageKey), rct)
            '    End If
            '    brs = New SolidBrush(Color.FromKnownColor(KnownColor.HighlightText))

            '    e.Graphics.DrawString(dspstr, e.Item.Font, brs, sRct, sf)
            '    brs.Dispose()
            'Else
            '    e.DrawBackground()

            '    If MyList.SmallImageList.Images.ContainsKey(e.Item.ImageKey) Then
            '        e.Graphics.DrawImageUnscaledAndClipped(MyList.SmallImageList.Images(e.Item.ImageKey), rct)
            '    End If



            '    brs = New SolidBrush(e.Item.ForeColor)
            '    e.Graphics.DrawString(dspstr, e.Item.Font, brs, sRct, sf)
            '    brs.Dispose()
            '    'e.DrawText()

            '    'End If
            'End If
            'e.DrawFocusRectangle()

            'sf.Dispose()

            'アイコンカラム位置取得
            Dim rct As Rectangle = Nothing
            Dim MyList As DetailsListView = e.Item.ListView
            Dim sf As New StringFormat
            sf.Alignment = StringAlignment.Near
            sf.LineAlignment = StringAlignment.Near
            If _iconCol = False Then
                'Dim MyList As DetailsListView = CType(sender, DetailsListView)
                Dim cnt As Integer
                Dim brs As SolidBrush
                Dim x As Integer = 0
                Dim wd As Integer
                Dim wd2 As Integer
                Dim idx As Integer

                For cnt = 0 To 4
                    If e.Item.ListView.Columns(cnt).Text = "" Then
                        idx = MyList.Columns(cnt).DisplayIndex
                        wd2 = MyList.Columns(cnt).Width - 2
                        Exit For
                    End If
                Next
                x = e.Item.Bounds.X
                For cnt = 0 To 4
                    If MyList.Columns(cnt).DisplayIndex < idx Then
                        x += MyList.Columns(cnt).Width
                    End If
                Next
                If wd2 > MyList.SmallImageList.ImageSize.Width Then
                    wd = MyList.SmallImageList.ImageSize.Width
                Else
                    wd = wd2
                End If
                rct = New Rectangle(x, e.Item.SubItems(idx).Bounds.Y + 1, wd, MyList.SmallImageList.ImageSize.Height)

                sf.Alignment = StringAlignment.Near
                sf.LineAlignment = StringAlignment.Near

                If e.Item.Selected = True Then
                    brs = New SolidBrush(Color.FromKnownColor(KnownColor.Highlight))
                    e.Graphics.FillRectangle(brs, e.Bounds)
                    brs.Dispose()
                    If MyList.SmallImageList.Images.ContainsKey(e.Item.ImageKey) Then
                        e.Graphics.DrawImageUnscaledAndClipped(MyList.SmallImageList.Images(e.Item.ImageKey), rct)
                    End If
                    brs = New SolidBrush(Color.FromKnownColor(KnownColor.HighlightText))
                    For i As Integer = 0 To 4
                        If i = 0 Then
                            If wd2 - MyList.SmallImageList.ImageSize.Width > 0 Then
                                Dim sRct As New Rectangle(x + 1 + MyList.SmallImageList.ImageSize.Width, e.Item.SubItems(i).Bounds.Y, wd2 - MyList.SmallImageList.ImageSize.Width, e.Item.SubItems(i).Bounds.Height - 3)
                                e.Graphics.DrawString(e.Item.SubItems(i).Text, e.Item.Font, brs, sRct, sf)
                            End If
                        Else
                            Dim sRct As New Rectangle(e.Item.SubItems(i).Bounds.X + 1, e.Item.SubItems(i).Bounds.Y, e.Item.SubItems(i).Bounds.Width - 2, e.Item.SubItems(i).Bounds.Height - 3)
                            e.Graphics.DrawString(e.Item.SubItems(i).Text, e.Item.Font, brs, sRct, sf)
                        End If
                    Next
                    brs.Dispose()
                Else
                    e.DrawBackground()

                    If MyList.SmallImageList.Images.ContainsKey(e.Item.ImageKey) Then
                        e.Graphics.DrawImageUnscaledAndClipped(MyList.SmallImageList.Images(e.Item.ImageKey), rct)
                    End If



                    brs = New SolidBrush(e.Item.ForeColor)
                    For i As Integer = 0 To 4
                        If i = 0 Then
                            If wd2 - MyList.SmallImageList.ImageSize.Width > 0 Then
                                Dim sRct As New Rectangle(x + 1 + MyList.SmallImageList.ImageSize.Width, e.Item.SubItems(i).Bounds.Y, wd2 - MyList.SmallImageList.ImageSize.Width, e.Item.SubItems(i).Bounds.Height - 3)
                                e.Graphics.DrawString(e.Item.SubItems(i).Text, e.Item.Font, brs, sRct, sf)
                            End If
                        Else
                            Dim sRct As New Rectangle(e.Item.SubItems(i).Bounds.X + 1, e.Item.SubItems(i).Bounds.Y, e.Item.SubItems(i).Bounds.Width - 2, e.Item.SubItems(i).Bounds.Height - 3)
                            e.Graphics.DrawString(e.Item.SubItems(i).Text, e.Item.Font, brs, sRct, sf)
                        End If
                        'Dim sRct As New Rectangle(e.Item.SubItems(i).Bounds.X + 1, e.Item.SubItems(i).Bounds.Y + 1, e.Item.SubItems(i).Bounds.Width - 2, e.Item.SubItems(i).Bounds.Height - 2)
                        'e.Graphics.DrawString(e.Item.SubItems(i).Text, e.Item.Font, brs, sRct, sf)
                    Next
                    brs.Dispose()
                    'e.DrawText()

                    'End If
                End If
            Else
                Dim brs As SolidBrush
                Dim wd As Integer
                Dim wd2 As Integer
                Dim x As Integer
                wd2 = e.Item.Bounds.Width - 2
                If wd2 > _iconSz Then wd = _iconSz
                x = e.Item.Bounds.X
                rct = New Rectangle(e.Item.Bounds.X, e.Item.Bounds.Y + 1, wd, _iconSz)

                If e.Item.Selected = True Then
                    brs = New SolidBrush(Color.FromKnownColor(KnownColor.Highlight))
                    e.Graphics.FillRectangle(brs, e.Bounds)
                    brs.Dispose()
                    If MyList.SmallImageList.Images.ContainsKey(e.Item.ImageKey) Then
                        e.Graphics.DrawImageUnscaledAndClipped(MyList.SmallImageList.Images(e.Item.ImageKey), rct)
                    End If
                    brs = New SolidBrush(Color.FromKnownColor(KnownColor.HighlightText))

                    If wd2 - _iconSz - 5 > 0 Then
                        Dim sRct As New Rectangle(x + 5 + _iconSz, e.Item.Bounds.Y, wd2 - _iconSz - 5, e.Item.Font.Height)
                        Dim sRct2 As New Rectangle(x + 5 + _iconSz, e.Item.Bounds.Y + e.Item.Font.Height, wd2 - _iconSz - 5, _iconSz - e.Item.Font.Height)
                        Dim fnt As New Font(e.Item.Font, FontStyle.Bold)
                        e.Graphics.DrawString(e.Item.SubItems(1).Text + "(" + e.Item.SubItems(4).Text + ") " + e.Item.SubItems(0).Text + " " + e.Item.SubItems(3).Text, fnt, brs, sRct, sf)
                        e.Graphics.DrawString(e.Item.SubItems(2).Text, e.Item.Font, brs, sRct2, sf)
                    End If
                    brs.Dispose()
                Else
                    e.DrawBackground()

                    If MyList.SmallImageList.Images.ContainsKey(e.Item.ImageKey) Then
                        e.Graphics.DrawImageUnscaledAndClipped(MyList.SmallImageList.Images(e.Item.ImageKey), rct)
                    End If
                    brs = New SolidBrush(e.Item.ForeColor)
                    If wd2 - _iconSz - 5 > 0 Then
                        Dim sRct As New Rectangle(x + 5 + _iconSz, e.Item.Bounds.Y, wd2 - _iconSz - 5, e.Item.Font.Height)
                        Dim sRct2 As New Rectangle(x + 5 + _iconSz, e.Item.Bounds.Y + e.Item.Font.Height, wd2 - _iconSz - 5, _iconSz - e.Item.Font.Height)
                        Dim fnt As New Font(e.Item.Font, FontStyle.Bold)
                        e.Graphics.DrawString(e.Item.SubItems(1).Text + "(" + e.Item.SubItems(4).Text + ") " + e.Item.SubItems(0).Text + " " + e.Item.SubItems(3).Text, fnt, brs, sRct, sf)
                        e.Graphics.DrawString(e.Item.SubItems(2).Text, e.Item.Font, brs, sRct2, sf)
                    End If
                    brs.Dispose()
                    'e.DrawText()

                    'End If
                End If
            End If


            e.DrawFocusRectangle()

            sf.Dispose()
        Else
            e.DrawDefault = True
        End If
    End Sub

    Private Sub MenuItemSubSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemSubSearch.Click
        Dim myList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        Dim _word As String
        Dim cidx As Integer = 0
        Dim fnd As Boolean = False
        Dim toIdx As Integer

        If SearchDialog.ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
        _word = SearchDialog.SWord

        If _word = "" Then Exit Sub

        If myList.SelectedItems.Count > 0 Then
            cidx = myList.SelectedItems(0).Index
        End If

        toIdx = myList.Items.Count - 1
RETRY:
        If SearchDialog.CheckSearchCaseSensitive.Checked = True Then
            If SearchDialog.CheckSearchRegex.Checked = True Then
                ' 正規表現検索（CaseSensitive）
                Dim _search As Regex
                Try
                    _search = New Regex(_word)
                    For idx As Integer = cidx To toIdx
                        If ((_search.IsMatch(myList.Items(idx).SubItems(1).Text) = True) Or (_search.IsMatch(myList.Items(idx).SubItems(2).Text) = True) Or (_search.IsMatch(myList.Items(idx).SubItems(4).Text) = True)) Then
                            For Each itm As ListViewItem In myList.SelectedItems
                                itm.Selected = False
                            Next
                            myList.Items(idx).Selected = True
                            myList.Items(idx).Focused = True
                            myList.EnsureVisible(idx)
                            Exit Sub
                        End If
                    Next
                Catch Err As ArgumentException
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
            If SearchDialog.CheckSearchRegex.Checked = True Then
                ' 正規表現検索（IgnoreCase）
                Try
                    For idx As Integer = cidx To toIdx
                        If ((Regex.IsMatch(myList.Items(idx).SubItems(1).Text, _word, RegexOptions.IgnoreCase) = True) Or (Regex.IsMatch(myList.Items(idx).SubItems(2).Text, _word, RegexOptions.IgnoreCase) = True) Or (Regex.IsMatch(myList.Items(idx).SubItems(4).Text, _word, RegexOptions.IgnoreCase) = True)) Then
                            For Each itm As ListViewItem In myList.SelectedItems
                                itm.Selected = False
                            Next
                            myList.Items(idx).Selected = True
                            myList.Items(idx).Focused = True
                            myList.EnsureVisible(idx)
                            Exit Sub
                        End If
                    Next
                Catch Err As ArgumentException
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

        If fnd = False Then
            If cidx > 0 And toIdx > -1 Then
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

    Private Sub MenuItemSearchNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemSearchNext.Click
        Dim myList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        Dim _word As String
        Dim cidx As Integer = 0
        Dim fnd As Boolean = False
        Dim toIdx As Integer

        _word = SearchDialog.SWord

        If _word = "" Then
            If SearchDialog.ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
            _word = SearchDialog.SWord
            If _word = "" Then Exit Sub
        End If

        If myList.SelectedItems.Count > 0 Then
            cidx = myList.SelectedItems(0).Index + 1
        End If

        toIdx = myList.Items.Count - 1
RETRY:
        If SearchDialog.CheckSearchCaseSensitive.Checked = True Then
            If SearchDialog.CheckSearchRegex.Checked = True Then
                ' 正規表現検索（CaseSensitive）
                Dim _search As Regex
                Try
                    _search = New Regex(_word)
                    For idx As Integer = cidx To toIdx
                        If ((_search.IsMatch(myList.Items(idx).SubItems(1).Text) = True) Or (_search.IsMatch(myList.Items(idx).SubItems(2).Text) = True) Or (_search.IsMatch(myList.Items(idx).SubItems(4).Text) = True)) Then
                            For Each itm As ListViewItem In myList.SelectedItems
                                itm.Selected = False
                            Next
                            myList.Items(idx).Selected = True
                            myList.Items(idx).Focused = True
                            myList.EnsureVisible(idx)
                            Exit Sub
                        End If
                    Next
                Catch Err As ArgumentException
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
            If SearchDialog.CheckSearchRegex.Checked = True Then
                ' 正規表現検索（IgnoreCase）
                Try
                    For idx As Integer = cidx To toIdx
                        If ((Regex.IsMatch(myList.Items(idx).SubItems(1).Text, _word, RegexOptions.IgnoreCase) = True) Or (Regex.IsMatch(myList.Items(idx).SubItems(2).Text, _word, RegexOptions.IgnoreCase) = True) Or (Regex.IsMatch(myList.Items(idx).SubItems(4).Text, _word, RegexOptions.IgnoreCase) = True)) Then
                            For Each itm As ListViewItem In myList.SelectedItems
                                itm.Selected = False
                            Next
                            myList.Items(idx).Selected = True
                            myList.Items(idx).Focused = True
                            myList.EnsureVisible(idx)
                            Exit Sub
                        End If
                    Next
                Catch Err As ArgumentException
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

        If fnd = False Then
            If cidx > 0 And toIdx > -1 Then
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
            If SearchDialog.ShowDialog() = Windows.Forms.DialogResult.Cancel Then Exit Sub
            _word = SearchDialog.SWord
            If _word = "" Then Exit Sub
        End If

        If myList.SelectedItems.Count > 0 Then
            cidx = myList.SelectedItems(0).Index - 1
        End If

        toIdx = 0
RETRY:
        If SearchDialog.CheckSearchCaseSensitive.Checked = True Then
            If SearchDialog.CheckSearchRegex.Checked = True Then
                ' 正規表現検索（CaseSensitive）
                Dim _search As Regex
                Try
                    _search = New Regex(_word)
                    If myList.Items.Count > 0 Then
                        For idx As Integer = cidx To toIdx Step -1
                            If ((_search.IsMatch(myList.Items(idx).SubItems(1).Text) = True) Or (_search.IsMatch(myList.Items(idx).SubItems(2).Text) = True) Or (_search.IsMatch(myList.Items(idx).SubItems(4).Text) = True)) Then
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
                            If ((Regex.IsMatch(myList.Items(idx).SubItems(1).Text, _word, RegexOptions.IgnoreCase) = True) Or (Regex.IsMatch(myList.Items(idx).SubItems(2).Text, _word, RegexOptions.IgnoreCase) = True) Or (Regex.IsMatch(myList.Items(idx).SubItems(4).Text, _word, RegexOptions.IgnoreCase) = True)) Then
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



        If fnd = False Then
            If cidx > 0 And toIdx > -1 Then
                'If MessageBox.Show("検索条件に一致するデータは見つかりません。" + vbCrLf + "もう一度先頭から検索しますか？", "検索", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Yes Then
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
        If _itnm = _ntnm And fnd = False Then
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
                        If idx <= idx1 Or idx >= idx2 Then
                            Call MoveTop()
                        End If
                        Exit Sub
                    End If
                Next
                Exit For
            End If
        Next

        If fnd = False Then
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
            Threading.Thread.Sleep(100)
            Application.DoEvents()
        Loop
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)

        '後でタブ追加して独自読み込みにする
        If MyList.SelectedItems.Count > 0 Then
            ExecWorker.RunWorkerAsync("http://twitter.com/" + MyList.SelectedItems(0).SubItems(4).Text + "/statuses/" + MyList.SelectedItems(0).SubItems(5).Text)
            'Try
            '    System.Diagnostics.Process.Start("http://twitter.com/" + MyList.SelectedItems(0).SubItems(4).Text)
            'Catch ex As Exception
            '    '                MessageBox.Show("ブラウザの起動に失敗、またはタイムアウトしました。" + ex.ToString())
            'End Try
        End If
    End Sub

    Private Sub FavorareMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FavorareMenuItem.Click
        Do While ExecWorker.IsBusy
            Threading.Thread.Sleep(100)
            Application.DoEvents()
        Loop

        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)

        '後でタブ追加して独自読み込みにする
        If MyList.SelectedItems.Count > 0 Then
            ExecWorker.RunWorkerAsync("http://favotter.matope.com/user.php?user=" + MyList.SelectedItems(0).SubItems(4).Text)
            'Try
            '    System.Diagnostics.Process.Start("http://twitter.com/" + MyList.SelectedItems(0).SubItems(4).Text)
            'Catch ex As Exception
            '    '                MessageBox.Show("ブラウザの起動に失敗、またはタイムアウトしました。" + ex.ToString())
            'End Try
        End If
    End Sub

    Private Sub ChangeImageSize()
        Dim sz As Integer = IIf(_iconSz = 0, 16, _iconSz)

        If TIconSmallList IsNot Nothing Then
            TIconSmallList.Dispose()
            TIconSmallList = Nothing
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
        'Else
        '    For Each key As String In TIconList.Images.Keys
        '        TIconSmallList.Images.Add(key, TIconList.Images(key).Clone)
        '    Next
        'End If
    End Sub

    'Private Sub TimerColorize_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerColorize.Tick
    '    TimerColorize.Enabled = False
    '    TimerColorize.Stop()

    '    Dim cnt As Integer = 0
    '    Dim at As New Collections.Specialized.StringCollection
    '    Dim pos1 As Integer = -1
    '    Dim pos2 As Integer = 0
    '    Dim myList As DetailsListView = CType(ListTab.SelectedTab.Controls(0), DetailsListView)
    '    Dim _item As ListViewItem
    '    Dim dTxt As String

    '    If myList.SelectedItems.Count > 0 Then
    '        _item = myList.SelectedItems(0)
    '        dTxt = "<html><head><style type=""text/css"">p {font-family: """ + _fntDetail.Name + """, sans-serif; font-size: " + _fntDetail.Size.ToString + "pt;} --></style></head><body style=""margin:0px""><p>" + _item.SubItems(7).Text + "</p></body></html>"
    '        NameLabel.Text = _item.SubItems(1).Text + "/" + _item.SubItems(4).Text
    '        'PostedText.Text = _item.SubItems(2).Text
    '        UserPicture.Image = TIconList.Images(_item.SubItems(6).Text)
    '        If myList.Name = "DirectMsg" Then
    '            NameLabel.Text = _item.SubItems(1).Text
    '            DateTimeLabel.Text = ""
    '        Else
    '            NameLabel.Text = _item.SubItems(1).Text + "/" + _item.SubItems(4).Text
    '            DateTimeLabel.Text = _item.SubItems(3).Text.ToString()
    '        End If

    '        NameLabel.ForeColor = System.Drawing.SystemColors.ControlText
    '        If _item.SubItems(10).Text = "True" And SettingDialog.OneWayLove Then NameLabel.ForeColor = _clOWL
    '        If _item.SubItems(9).Text = "True" Then NameLabel.ForeColor = _clFav

    '        '''''
    '        '        PostBrowser.DocumentText = "<html><head></head><body style=""margin:0px""><font size=""2"" face=""MS UI Gothic"">" + _item.SubItems(7).Text + "</font></body></html>"

    '        If PostBrowser.DocumentText <> dTxt Then
    '            PostBrowser.DocumentText = dTxt
    '        End If
    '        'PostBrowser.DocumentText = "<html><head></head><body style=""margin:0px""><font size=""2"" face=""sans-serif"">" + _item.SubItems(7).Text + "</font></body></html>"


    '    End If
    '    'ColorizeList()
    'End Sub


    Private Sub VerUpMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VerUpMenuItem.Click
        Call CheckNewVersion()
    End Sub

    Private Sub CheckNewVersion(Optional ByVal startup As Boolean = False)
        Dim _mySock As New MySocket("Shift_JIS", "", "", _
                                SettingDialog.ProxyType, _
                                SettingDialog.ProxyAddress, _
                                SettingDialog.ProxyPort, _
                                SettingDialog.ProxyUser, _
                                SettingDialog.ProxyPassword)
        Dim retMsg As String
        Dim resStatus As String = ""
        Dim strVer As String

        retMsg = _mySock.GetWebResponse("http://www.asahi-net.or.jp/~ne5h-ykmz/version2.txt?" + Now.ToString("yyMMddHHmmss") + Environment.TickCount.ToString(), resStatus)
        If retMsg.Length > 0 Then
            strVer = retMsg.Substring(0, 4)
            If strVer.CompareTo(My.Application.Info.Version.ToString.Replace(".", "")) > 0 Then
                If MessageBox.Show("新しいバージョン " + strVer + " が公開されています。更新しますか？", "Tween更新確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    retMsg = _mySock.GetWebResponse("http://www.asahi-net.or.jp/~ne5h-ykmz/Tween" + strVer + ".gz", resStatus, MySocket.REQ_TYPE.ReqGETFile)
                    If retMsg.Length = 0 Then
                        retMsg = _mySock.GetWebResponse("http://www.asahi-net.or.jp/~ne5h-ykmz/TweenUp.gz?" + Now.ToString("yyMMddHHmmss") + Environment.TickCount.ToString(), resStatus, MySocket.REQ_TYPE.ReqGETFileUp)
                        If retMsg.Length = 0 Then
                            System.Diagnostics.Process.Start(My.Application.Info.DirectoryPath + "\TweenUp.exe")
                            Application.Exit()
                            Exit Sub
                        Else
                            If startup = False Then MessageBox.Show("アップデーターのダウンロードに失敗しました。しばらく待ってから再度お試しください。", "Tween更新結果", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        End If
                    Else
                        If startup = False Then MessageBox.Show("最新版が公開されていますが、ダウンロードに失敗しました。しばらく待ってから再度お試しください。", "Tween更新結果", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If
                End If
            Else
                If startup = False Then
                    MessageBox.Show("最新版をお使いです。更新の必要はありませんでした。使用中Ver：" + My.Application.Info.Version.ToString.Replace(".", "") + " 最新Ver：" + strVer, "Tween更新結果", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    'If retMsg.Length > 4 Then
                    '    strVer = retMsg.Substring(4)
                    '    MessageBox.Show("互換性のない最新バージョンがあります。配布サイトからダウンロードして導入してください。使用中Ver：" + My.Application.Info.Version.ToString.Replace(".", "") + " 最新Ver：" + strVer, "Tween更新結果", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    'Else
                    '    MessageBox.Show("最新版をお使いです。更新の必要はありませんでした。使用中Ver：" + My.Application.Info.Version.ToString.Replace(".", "") + " 最新Ver：" + strVer, "Tween更新結果", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    'End If
                End If
            End If
        Else
            StatusLabel.Text = "バージョンチェック失敗"
            If startup = False Then MessageBox.Show("更新版のバージョン取得に失敗しました。しばらく待ってから再度お試しください。", "Tween更新結果", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub TimerColorize_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerColorize.Tick
        If TimerColorize.Enabled = False Then Exit Sub
        TimerColorize.Stop()
        TimerColorize.Enabled = False
        TimerColorize.Interval = 100
        'TimerColorize.Stop()
        'TimerColorize.Enabled = False
        '''Call ColorizeList(True)
        'PostBrowser.DocumentText = "<html><head></head><body style=""margin:0px""><font size=""2"" face=""sans-serif"">" + _item.SubItems(7).Text + "</font></body></html>"
        Call ColorizeList(False)
        Call DispSelectedPost()
        '件数関連の場合、タイトル即時書き換え
        If SettingDialog.DispLatestPost <> DispTitleEnum.None And _
           SettingDialog.DispLatestPost <> DispTitleEnum.Post And _
           SettingDialog.DispLatestPost <> DispTitleEnum.Ver Then
            Call SetMainWindowTitle()
        End If
        If StatusLabelUrl.Text.StartsWith("http") = False Then Call SetStatusLabel()
    End Sub

    Private Sub DispSelectedPost()
        Dim _item As ListViewItem
        Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
        Dim dTxt As String

        If MyList.SelectedItems.Count = 0 Then Exit Sub

        _item = MyList.SelectedItems(0)
        dTxt = "<html><head><style type=""text/css"">p {font-family: """ + _fntDetail.Name + """, sans-serif; font-size: " + _fntDetail.Size.ToString + "pt;} --></style></head><body style=""margin:0px""><p>" + _item.SubItems(7).Text + "</p></body></html>"
        'PostedText.Text = _item.SubItems(2).Text
        NameLabel.Text = _item.SubItems(1).Text + "/" + _item.SubItems(4).Text
        UserPicture.Image = TIconList.Images(_item.SubItems(6).Text)
        If ListTab.SelectedTab.Text = "Direct" Then
            NameLabel.Text = _item.SubItems(1).Text
            DateTimeLabel.Text = ""
        Else
            NameLabel.Text = _item.SubItems(1).Text + "/" + _item.SubItems(4).Text
            DateTimeLabel.Text = _item.SubItems(3).Text.ToString()
        End If

        NameLabel.ForeColor = System.Drawing.SystemColors.ControlText
        If _item.SubItems(10).Text = "True" And (SettingDialog.OneWayLove Or ListTab.SelectedTab.Text = "Direct") Then NameLabel.ForeColor = _clOWL
        If _item.SubItems(9).Text = "True" Then NameLabel.ForeColor = _clFav

        '''''
        '        PostBrowser.DocumentText = "<html><head></head><body style=""margin:0px""><font size=""2"" face=""MS UI Gothic"">" + _item.SubItems(7).Text + "</font></body></html>"

        If PostBrowser.DocumentText <> dTxt Then
            PostBrowser.Visible = False
            PostBrowser.DocumentText = dTxt
            PostBrowser.Visible = True
        End If
    End Sub
    Private Sub MatomeMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MatomeMenuItem.Click
        Do While ExecWorker.IsBusy
            Threading.Thread.Sleep(100)
            Application.DoEvents()
        Loop

        ExecWorker.RunWorkerAsync("http://www5.atwiki.jp/tween/")
    End Sub

    Private Sub OfficialMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OfficialMenuItem.Click
        Do While ExecWorker.IsBusy
            Threading.Thread.Sleep(100)
            Application.DoEvents()
        Loop

        ExecWorker.RunWorkerAsync("http://d.hatena.ne.jp/Kiri_Feather/20071121")
    End Sub

    Private Sub DLPageMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DLPageMenuItem.Click
        Do While ExecWorker.IsBusy
            Threading.Thread.Sleep(100)
            Application.DoEvents()
        Loop

        ExecWorker.RunWorkerAsync("http://www.asahi-net.or.jp/~ne5h-ykmz/index.html")
    End Sub

    Private Sub ListTab_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ListTab.KeyDown
        If e.Alt = False And e.Control = False And e.Shift = False Then
            ' ModifierKeyが押されていない場合
            If e.KeyCode = Keys.N Or e.KeyCode = Keys.Right Then
                e.Handled = True
                e.SuppressKeyPress = True
                Call GoNextRelPost()
                Exit Sub
            End If
            If e.KeyCode = Keys.P Or e.KeyCode = Keys.Left Then
                e.Handled = True
                e.SuppressKeyPress = True
                Call GoPreviousRelPost()
                Exit Sub
            End If
            If e.KeyCode = Keys.OemPeriod Then
                e.Handled = True
                e.SuppressKeyPress = True
                Call GoAnchor()
                Exit Sub
            End If
            _anchorFlag = False
            If e.KeyCode = Keys.Space Or e.KeyCode = Keys.ProcessKey Then
                e.Handled = True
                e.SuppressKeyPress = True
                Call JumpUnreadMenuItem_Click(Nothing, Nothing)
            End If
            If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Return Then
                e.Handled = True
                e.SuppressKeyPress = True
                Call MakeReplyOrDirectStatus()
            End If
            If e.KeyCode = Keys.L Then
                e.Handled = True
                e.SuppressKeyPress = True
                Call GoNextPost()
            End If
            If e.KeyCode = Keys.H Then
                e.Handled = True
                e.SuppressKeyPress = True
                Call GoPreviousPost()
            End If
            If e.KeyCode = Keys.Z Or e.KeyCode = Keys.Oemcomma Then
                e.Handled = True
                e.SuppressKeyPress = True
                Call MoveTop()
            End If
        End If
        _anchorFlag = False
        If e.Alt = False And e.Control = True And e.Shift = False Then
            ' CTRLキーが押されている場合
            If e.KeyCode = Keys.Home Or e.KeyCode = Keys.End Then
                'Call ColorizeList(False)
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
        If e.Alt = False And e.Control = False And e.Shift = True Then
            ' SHIFTキーが押されている場合
            If e.KeyCode = Keys.H Then
                e.Handled = True
                e.SuppressKeyPress = True
                Call GoTopEnd(True)
            End If
            If e.KeyCode = Keys.L Then
                e.Handled = True
                e.SuppressKeyPress = True
                Call GoTopEnd(False)
            End If
            If e.KeyCode = Keys.M Then
                e.Handled = True
                e.SuppressKeyPress = True
                Call GoMiddle()
            End If
            If e.KeyCode = Keys.G Then
                e.Handled = True
                e.SuppressKeyPress = True
                Call GoLast()
            End If
            If e.KeyCode = Keys.Z Then
                e.Handled = True
                e.SuppressKeyPress = True
                Call MoveMiddle()
            End If

            ' お気に入り前後ジャンプ(SHIFT+N←/P→)
            If e.KeyCode = Keys.N Or e.KeyCode = Keys.Right Then
                e.Handled = True
                e.SuppressKeyPress = True
                Call GoNextFav()
            End If
            If e.KeyCode = Keys.P Or e.KeyCode = Keys.Left Then
                e.Handled = True
                e.SuppressKeyPress = True
                Call GoPreviousFav()
            End If

        End If
        If e.Alt = False Then
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
            If e.Alt = False And e.Control = True And e.Shift = False Then
                e.Handled = True
                e.SuppressKeyPress = True
                For Each itm As ListViewItem In MyList.SelectedItems
                    If clstr <> "" Then clstr += vbCrLf
                    clstr += itm.SubItems(4).Text + ":" + itm.SubItems(2).Text + " [http://twitter.com/" + itm.SubItems(4).Text + "/statuses/" + itm.SubItems(5).Text + "]"
                Next
            End If
            If e.Alt = False And e.Control = True And e.Shift = True Then
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
                'MyList.EnsureVisible(MyList.Items.Count - 1)
                MyList.EnsureVisible(idx)
                MyList.Update()
                'Call ColorizeList(True)
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
                'MyList.EnsureVisible(MyList.Items.Count - 1)
                MyList.EnsureVisible(idx)
                MyList.Update()
                'Call ColorizeList(True)
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
                'MyList.EnsureVisible(MyList.Items.Count - 1)
                MyList.EnsureVisible(idx)
                MyList.Update()
                'Call ColorizeList(True)
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
                'MyList.EnsureVisible(MyList.Items.Count - 1)
                MyList.EnsureVisible(idx)
                MyList.Update()
                'Call ColorizeList(True)
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

        'data = _item.SubItems(7).Text
        Do While True
            pos1 = dTxt.IndexOf(_replyHtml, pos2)
            If pos1 = -1 Then Exit Do
            pos2 = dTxt.IndexOf(""">", pos1 + _replyHtml.Length)
            If pos2 > -1 Then
                at.Add(dTxt.Substring(pos1 + _replyHtml.Length, pos2 - pos1 - _replyHtml.Length))
            End If
        Loop

        For idx As Integer = fIdx To MyList.Items.Count - 1
            If MyList.Items(idx).SubItems(4).Text = user Or _
               at.Contains(MyList.Items(idx).SubItems(4).Text) Or _
               Regex.IsMatch(MyList.Items(idx).SubItems(2).Text, "@" + user + "([^a-zA-Z0-9_]|$)") Then
                For Each itm As ListViewItem In MyList.SelectedItems
                    itm.Selected = False
                Next
                MyList.Items(idx).Selected = True
                MyList.Items(idx).Focused = True
                'MyList.EnsureVisible(MyList.Items.Count - 1)
                MyList.EnsureVisible(idx)
                MyList.Update()
                'Call ColorizeList(True)
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

        If _anchorFlag = False Then
            _anchorItem = Nothing
            _anchorItem = MyList.SelectedItems(0).Clone
            _anchorFlag = True
        End If
        Dim user As String = _anchorItem.SubItems(4).Text
        Dim dTxt As String = _anchorItem.SubItems(7).Text
        Dim at As New Collections.Specialized.StringCollection
        Dim pos1 As Integer
        Dim pos2 As Integer

        'data = _item.SubItems(7).Text
        Do While True
            pos1 = dTxt.IndexOf(_replyHtml, pos2)
            If pos1 = -1 Then Exit Do
            pos2 = dTxt.IndexOf(""">", pos1 + _replyHtml.Length)
            If pos2 > -1 Then
                at.Add(dTxt.Substring(pos1 + _replyHtml.Length, pos2 - pos1 - _replyHtml.Length))
            End If
        Loop

        For idx As Integer = fIdx To 0 Step -1
            If MyList.Items(idx).SubItems(4).Text = user Or _
               at.Contains(MyList.Items(idx).SubItems(4).Text) Or _
               Regex.IsMatch(MyList.Items(idx).SubItems(2).Text, "@" + user + "([^a-zA-Z0-9_]|$)") Then
                For Each itm As ListViewItem In MyList.SelectedItems
                    itm.Selected = False
                Next
                MyList.Items(idx).Selected = True
                MyList.Items(idx).Focused = True
                'MyList.EnsureVisible(MyList.Items.Count - 1)
                MyList.EnsureVisible(idx)
                MyList.Update()
                'Call ColorizeList(True)
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
                'MyList.EnsureVisible(MyList.Items.Count - 1)
                MyList.EnsureVisible(idx)
                MyList.Update()
                'Call ColorizeList(True)
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
        'Call ColorizeList(True)
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
        'Call ColorizeList(True)
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
        'Call ColorizeList(True)
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
        'Call ColorizeList(True)
        TimerColorize.Start()

    End Sub

    Private Sub MyList_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        _anchorFlag = False
    End Sub

    Private Sub StatusText_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusText.Enter
        StatusText.BackColor = Color.LemonChiffon
    End Sub

    Private Sub StatusText_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusText.Leave
        StatusText.BackColor = Color.FromKnownColor(KnownColor.Window)
    End Sub

    Private Sub StatusText_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles StatusText.KeyDown
        'If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Return Then
        '    If e.Alt = False And e.Control = SettingDialog.PostCtrlEnter And e.Shift = False Then
        '        Dim MyList As DetailsListView = CType(ListTab.SelectedTab.Controls(0), DetailsListView)
        '        'e.Handled = True
        '        e.SuppressKeyPress = True
        '        'MyList.Focus()
        '        Call PostButton_Click(Nothing, Nothing)
        '    Else
        '        e.SuppressKeyPress = True
        '    End If
        'End If
        If e.Alt = False And e.Control = True And e.Shift = False Then
            If e.KeyCode = Keys.A Then
                StatusText.SelectAll()
                'StatusText.SelectionStart = StatusText.Text.Length
                'StatusText.Focus()
            End If
        End If
        If e.Alt = False And e.Control = False And e.Shift = False Then
            If e.KeyCode = Keys.Up Or e.KeyCode = Keys.Down Then
                If StatusText.Text.Trim <> "" Then _history(_hisIdx) = StatusText.Text
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
                'StatusText.SelectionStart = StatusText.Text.Length
                'StatusText.Focus()
            End If
        End If
    End Sub

    Private Sub SaveConfigs()
        If _username <> "" And _password <> "" Then

            _section.FormSize = _mySize
            _section.FormLocation = _myLoc
            _section.SplitterDistance = _mySpDis
            _section.UserName = _username
            _section.Password = _password
            _section.NextPageThreshold = clsTw.NextThreshold
            _section.NextPages = clsTw.NextPages
            _section.TimelinePeriod = SettingDialog.TimelinePeriodInt
            _section.DMPeriod = SettingDialog.DMPeriodInt
            _section.MaxPostNum = SettingDialog.MaxPostNum
            '_section.LogDays = SettingDialog.LogDays
            'Select Case SettingDialog.LogUnit
            '    Case Setting.LogUnitEnum.Minute
            '        _section.LogUnit = ListSection.LogUnitEnum.Minute
            '    Case Setting.LogUnitEnum.Hour
            '        _section.LogUnit = ListSection.LogUnitEnum.Hour
            '    Case Setting.LogUnitEnum.Day
            '        _section.LogUnit = ListSection.LogUnitEnum.Day
            'End Select
            _section.ReadPages = SettingDialog.ReadPages
            _section.Readed = SettingDialog.Readed
            _section.ListLock = ListLockMenuItem.Checked
            'Select Case SettingDialog.IconSz
            '    Case Setting.IconSizes.IconNone
            '        _section.IconSize = ListSection.IconSizes.IconNone
            '    Case Setting.IconSizes.Icon16
            '        _section.IconSize = ListSection.IconSizes.Icon16
            '    Case Setting.IconSizes.Icon24
            '        _section.IconSize = ListSection.IconSizes.Icon24
            '    Case Setting.IconSizes.Icon48
            '        _section.IconSize = ListSection.IconSizes.Icon48
            '    Case Setting.IconSizes.Icon48_2
            '        _section.IconSize = ListSection.IconSizes.Icon48_2
            'End Select
            _section.IconSize = SettingDialog.IconSz
            '_section.selecteduser（collection)
            '_section.favuser
            _section.StatusText = SettingDialog.Status
            _section.NewAllPop = NewPostPopMenuItem.Checked
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

            'Select Case SettingDialog.NameBalloon
            '    Case Setting.NameBalloonEnum.None
            '        _section.NameBalloon = ListSection.NameBalloonEnum.None
            '    Case Setting.NameBalloonEnum.UserID
            '        _section.NameBalloon = ListSection.NameBalloonEnum.UserID
            '    Case Setting.NameBalloonEnum.NickName
            '        _section.NameBalloon = ListSection.NameBalloonEnum.NickName
            'End Select
            _section.NameBalloon = SettingDialog.NameBalloon

            _section.PostCtrlEnter = SettingDialog.PostCtrlEnter
            '_section.UseAPI = SettingDialog.UseAPI
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

            Dim tmpList As DetailsListView = Nothing
            For Each myTab As TabPage In ListTab.TabPages
                If myTab.Text = _curTabText Then
                    tmpList = DirectCast(myTab.Controls(0), DetailsListView)
                    Exit For
                End If
            Next
            _section.DisplayIndex1 = tmpList.Columns(0).DisplayIndex
            _section.Width1 = tmpList.Columns(0).Width
            If _iconCol = False Then
                _section.DisplayIndex2 = tmpList.Columns(1).DisplayIndex
                _section.DisplayIndex3 = tmpList.Columns(2).DisplayIndex
                _section.DisplayIndex4 = tmpList.Columns(3).DisplayIndex
                _section.DisplayIndex5 = tmpList.Columns(4).DisplayIndex
                _section.Width2 = tmpList.Columns(1).Width
                _section.Width3 = tmpList.Columns(2).Width
                _section.Width4 = tmpList.Columns(3).Width
                _section.Width5 = tmpList.Columns(4).Width
            End If
            _section.SortColumn = listViewItemSorter.Column
            _section.SortOrder = listViewItemSorter.Order

            _section.ListElement.Clear()

            ''Recentタブ
            '_section.ListElement.Add(New ListElement("Recent"))
            'For Each ts As TabStructure In _tabs
            '    If ts.tabName = "Recent" Then
            '        _section.ListElement("Recent").Notify = ts.notify
            '        _section.ListElement("Recent").SoundFile = ts.soundFile
            '        _section.ListElement("Recent").UnreadManage = ts.unreadManage
            '        Exit For
            '    End If
            'Next

            ''Replyタブ
            '_section.ListElement.Add(New ListElement("Reply"))
            'For Each ts As TabStructure In _tabs
            '    If ts.tabName = "Reply" Then
            '        _section.ListElement("Reply").Notify = ts.notify
            '        _section.ListElement("Reply").SoundFile = ts.soundFile
            '        _section.ListElement("Reply").UnreadManage = ts.unreadManage
            '        Exit For
            '    End If
            'Next


            ''DirectMsgタブ
            '_section.ListElement.Add(New ListElement("DirectMsg"))
            'For Each ts As TabStructure In _tabs
            '    If ts.tabName = "DirectMsg" Then
            '        _section.ListElement("DirectMsg").Notify = ts.notify
            '        _section.ListElement("DirectMsg").SoundFile = ts.soundFile
            '        _section.ListElement("DirectMsg").UnreadManage = ts.unreadManage
            '        Exit For
            '    End If
            'Next


            _section.SelectedUser.Clear()
            'Dim sID As String
            'For Each sID In _notId
            '    _section.SelectedUser.Add(New SelectedUser("Recent->" + sID))
            'Next

            Dim cnt As Integer = 0
            For idx As Integer = 0 To ListTab.TabCount - 1
                Dim tabName As String = ListTab.TabPages(idx).Text
                Dim myList As DetailsListView = DirectCast(ListTab.TabPages(idx).Controls(0), DetailsListView)
                _section.ListElement.Add(New ListElement(tabName))
                For Each myTab As TabStructure In _tabs
                    If myTab.tabName = tabName Then
                        _section.ListElement(tabName).Notify = myTab.notify
                        _section.ListElement(tabName).SoundFile = myTab.soundFile
                        _section.ListElement(tabName).UnreadManage = myTab.unreadManage
                        For Each fc As FilterClass In myTab.filters
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

            'RemoveAllTabs()

            'listViewItemSorter.Column = 3
            'listViewItemSorter.Order = SortOrder.Descending
            'Timeline.Sort()

            'Dim cnt As Integer
            'For cnt = 0 To Timeline.Items.Count - 1
            '    If Timeline.Items(cnt).Font.Underline = False Then
            '        _section.LastName = Timeline.Items(cnt).SubItems(4).Text
            '        _section.LastID = Timeline.Items(cnt).SubItems(5).Text
            '        Exit For
            '    End If
            'Next
            'If cnt = Timeline.Items.Count Then
            '    If cnt > 0 Then
            '        _section.LastName = Timeline.Items(cnt - 1).SubItems(4).Text
            '        _section.LastID = Timeline.Items(cnt - 1).SubItems(5).Text
            '    End If
            'End If

            '_section.LogPosts.Clear()

            'Dim dtold As DateTime

            'Select Case SettingDialog.LogUnit
            '    Case Setting.LogUnitEnum.Minute
            '        dtold = Now().AddMinutes(SettingDialog.LogDays * -1)
            '    Case Setting.LogUnitEnum.Hour
            '        dtold = Now().AddHours(SettingDialog.LogDays * -1)
            '    Case Setting.LogUnitEnum.Day
            '        dtold = Now().AddDays(SettingDialog.LogDays * -1)
            'End Select

            'For cnt = 0 To Timeline.Items.Count - 1
            '    If CDate(Timeline.Items(cnt).SubItems(3).Text) > dtold Then
            '        Dim lpost As New LogPost
            '        lpost.IsFav = IIf(Timeline.Items(cnt).ForeColor = Color.Red, LogPost.Fav.Fav, LogPost.Fav.NoFav)
            '        lpost.Id = Timeline.Items(cnt).SubItems(5).Text
            '        lpost.ImageUrl = Timeline.Items(cnt).SubItems(6).Text
            '        lpost.Name = Timeline.Items(cnt).SubItems(4).Text + Timeline.Items(cnt).SubItems(5).Text
            '        lpost.Name2 = Timeline.Items(cnt).SubItems(4).Text
            '        lpost.Nick = Timeline.Items(cnt).SubItems(1).Text
            '        lpost.Post = Timeline.Items(cnt).SubItems(7).Text
            '        lpost.PostDate = Timeline.Items(cnt).SubItems(3).Text
            '        lpost.IsUnread = IIf(Timeline.Items(cnt).Font.Bold, LogPost.Unread.Unread, LogPost.Unread.Readed)

            '        _section.LogPosts.Add(lpost)
            '    End If
            'Next

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
            Dim MyList As DetailsListView = DirectCast(ListTab.SelectedTab.Controls(0), DetailsListView)
            Dim srm As System.IO.Stream
            srm = SaveFileDialog1.OpenFile
            If Not (srm Is Nothing) Then
                Dim sw As New System.IO.StreamWriter(srm)
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
                sw.Close()
                sw.Dispose()
            End If
            srm.Close()
            srm.Dispose()
        End If
    End Sub

    Private Sub PostBrowser_PreviewKeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles PostBrowser.PreviewKeyDown
        If e.KeyCode = Keys.F5 Then
            e.IsInputKey = True
        End If
        If e.Alt = False And e.Control = False And e.Shift = False And (e.KeyCode = Keys.Space Or e.KeyCode = Keys.ProcessKey) Then
            e.IsInputKey = True
            Call JumpUnreadMenuItem_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub Tabs_DoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListTab.MouseDoubleClick
        If ListTab.SelectedTab.Text = "Recent" Or ListTab.SelectedTab.Text = "Reply" Or ListTab.SelectedTab.Text = "Direct" Then Exit Sub
        Dim newTabText As String = InputBox("タブの名前を指定してください。", "タブ名変更", ListTab.SelectedTab.Text)
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
                If ListTab.TabPages(i).Text <> "Recent" And _
                   ListTab.TabPages(i).Text <> "Reply" And _
                   ListTab.TabPages(i).Text <> "Direct" And _
                   ListTab.TabPages(i).Text <> newTabText Then
                    TabDialog.RemoveTab(ListTab.TabPages(i).Text)
                End If
            Next
            For i As Integer = 0 To ListTab.TabCount - 1
                If ListTab.TabPages(i).Text <> "Recent" And _
                   ListTab.TabPages(i).Text <> "Reply" And _
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
                If rect.Left <= cpos.X And cpos.X <= rect.Right And _
                   rect.Top <= cpos.Y And cpos.Y <= rect.Bottom Then
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
                If rect.Left <= spos.X And spos.X <= rect.Right And _
                   rect.Top <= spos.Y And spos.Y <= rect.Bottom Then
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
            If tn = "Recent" Or tn = "Reply" Or tn = "Direct" Then
                tn = "Direct"
                bef = False
                i = 2
            End If

            Dim ts As TabStructure = e.Data.GetData(GetType(TabStructure))

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

        If StatusText.Enabled = False Then Exit Sub

        ' 複数あてリプライはReplyではなく通常ポスト

        If MyList.SelectedItems.Count > 0 Then
            ' アイテムが1件以上選択されている
            If MyList.SelectedItems.Count = 1 And isAll = False Then
                ' 単独ユーザー宛リプライまたはDM
                If (ListTab.SelectedTab.Text = "Direct" And isAuto) Or (isAuto = False And isReply = False) Then
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
                    _reply_to_id = MyList.SelectedItems(0).SubItems(5).Text
                    _reply_to_name = MyList.SelectedItems(0).SubItems(4).Text
                Else
                    If isAuto Then
                        If StatusText.Text.IndexOf("@" + MyList.SelectedItems(0).SubItems(4).Text + " ") > -1 Then Exit Sub
                        If StatusText.Text.StartsWith("@") = False Then
                            If StatusText.Text.StartsWith(". ") Then
                                ' 複数リプライ
                                StatusText.Text = StatusText.Text.Insert(2, "@" + MyList.SelectedItems(0).SubItems(4).Text + " ")
                                _reply_to_id = 0
                                _reply_to_name = Nothing
                            Else
                                ' 単独リプライ
                                StatusText.Text = "@" + MyList.SelectedItems(0).SubItems(4).Text + " " + StatusText.Text
                                _reply_to_id = MyList.SelectedItems(0).SubItems(5).Text
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
                            'StatusText.Text = ". " + StatusText.Text + IIf(StatusText.Text.EndsWith(" "), "@", " @") + MyList.SelectedItems(0).SubItems(4).Text + " "
                            StatusText.Text = ". " + StatusText.Text.Insert(sidx, " @" + MyList.SelectedItems(0).SubItems(4).Text + " ")
                            sidx += 5 + MyList.SelectedItems(0).SubItems(4).Text.Length
                        Else
                            ' 複数リプライ
                            'StatusText.Text = StatusText.Text + IIf(StatusText.Text.EndsWith(" "), "@", " @") + MyList.SelectedItems(0).SubItems(4).Text + " "
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
                If isAuto = False And isReply = False Then Exit Sub

                If isAuto Then
                    Dim sTxt As String = StatusText.Text
                    If sTxt.StartsWith(". ") = False Then
                        sTxt = ". " + sTxt
                    End If
                    For cnt As Integer = 0 To MyList.SelectedItems.Count - 1
                        If sTxt.IndexOf("@" + MyList.SelectedItems(cnt).SubItems(4).Text + " ") = -1 Then
                            sTxt = sTxt.Insert(2, "@" + MyList.SelectedItems(cnt).SubItems(4).Text + " ")
                            'StatusText.Text += "@" + MyList.SelectedItems(cnt).SubItems(4).Text + " "
                        End If
                    Next
                    StatusText.Text = sTxt
                Else
                    'If sTxt.EndsWith(" ") = False Then sTxt = sTxt + " "
                    Dim ids As String = ""
                    Dim sidx As Integer = StatusText.SelectionStart
                    If StatusText.Text.StartsWith(". ") = False Then
                        StatusText.Text = ". " + StatusText.Text
                        sidx += 2
                    End If
                    For cnt As Integer = 0 To MyList.SelectedItems.Count - 1
                        If ids.Contains("@" + MyList.SelectedItems(cnt).SubItems(4).Text + " ") = False Then
                            ids += "@" + MyList.SelectedItems(cnt).SubItems(4).Text + " "
                        End If
                        'sTxt = sTxt + "@" + MyList.SelectedItems(cnt).SubItems(4).Text + " "
                        'StatusText.Text += "@" + MyList.SelectedItems(cnt).SubItems(4).Text + " "
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
                                    If ids.Contains(atId) = False And atId <> "@" + _username + " " Then
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
                        'StatusText.Text = ". " + StatusText.Text + IIf(StatusText.Text.EndsWith(" "), "@", " @") + MyList.SelectedItems(0).SubItems(4).Text + " "
                        StatusText.Text = ". " + StatusText.Text.Insert(sidx, ids)
                        sidx += 2 + ids.Length
                    Else
                        'StatusText.Text = StatusText.Text + IIf(StatusText.Text.EndsWith(" "), "@", " @") + MyList.SelectedItems(0).SubItems(4).Text + " "
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
        If TimerRefreshIcon.Enabled = False Then Exit Sub

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
                'For Each tp As TabPage In ListTab.TabPages
                '    If tp.Text = ts.tabName Then
                'Dim myList As DetailsListView = CType(tp.Controls(0), DetailsListView)
                For Each itm As ListViewItem In ts.listCustom.Items
                    If SettingDialog.UnreadManage And ts.unreadManage Then
                        'If itm.SubItems(8).Text = "True" Then
                        '    itm.ForeColor = _clReaded
                        '    itm.Font = _fntReaded
                        'Else
                        '    itm.ForeColor = _clUnread
                        '    itm.Font = _fntUnread
                        'End If
                    Else
                        Dim fcl As Color = _clReaded
                        If itm.SubItems(10).Text = "True" And SettingDialog.OneWayLove Then fcl = _clOWL
                        If itm.SubItems(9).Text = "True" Then fcl = _clFav
                        'itm.ForeColor = _clReaded
                        'itm.Font = _fntReaded
                        ts.listCustom.ChangeItemStyles(itm.Index, itm.BackColor, fcl, _fntReaded)
                        itm.SubItems(8).Text = "True"
                        ts.unreadCount = 0
                        ts.oldestUnreadItem = Nothing
                        If ts.tabPage.ImageIndex = 0 Then ts.tabPage.ImageIndex = -1
                    End If
                Next
                Exit Sub
                '    End If
                'Next
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
                'SoundFileComboBox.SelectedIndex = SoundFileComboBox.Items.IndexOf(ts.soundFile)
                ts.soundFile = SoundFileComboBox.SelectedItem
                Exit For
            End If
        Next
    End Sub

    Private Sub DeleteTabMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DeleteTabMenuItem.Click
        If _rclickTabName = "" Then Exit Sub

        Call RemoveSpecifiedTab(_rclickTabName)
    End Sub

    Private Sub FilterEditMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FilterEditMenuItem.Click
        If _rclickTabName = "" Then Exit Sub

        fDialog.Tabs = _tabs
        fDialog.CurrentTab = _rclickTabName
        fDialog.ShowDialog()
        _tabs = fDialog.Tabs
        Call ReFilter()
    End Sub

    Private Sub AddTabMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddTabMenuItem.Click
        Dim tabName As String = InputBox("タブ名を指定してください。", "タブ追加", "MyTab" + _tabs.Count.ToString)
        If tabName <> "" Then
            If AddNewTab(tabName) = False Then
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
                If TabDialog.ShowDialog = Windows.Forms.DialogResult.Cancel Then Exit Sub
                tabName = TabDialog.SelectedTabName

                ListTab.SelectedTab.Focus()
                If tabName = "(新規タブ)" Then

                    tabName = InputBox("タブ名を指定してください。", "タブ追加", "MyTab" + _tabs.Count.ToString)
                    If tabName <> "" Then
                        If AddNewTab(tabName) = False Then
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
            _tabs = fDialog.Tabs
            Call ReFilter()
        Next

    End Sub

    Protected Overrides Function ProcessDialogKey( _
        ByVal keyData As Keys) As Boolean
        'TextBox1でEnterを押してもビープ音が鳴らないようにする
        If StatusText.Focused AndAlso _
            (keyData And Keys.KeyCode) = Keys.Enter Then
            If ((keyData And Keys.Control) = Keys.Control And SettingDialog.PostCtrlEnter) Or _
               ((keyData And Keys.Control) <> Keys.Control And SettingDialog.PostCtrlEnter = False) Then
                Call PostButton_Click(Nothing, Nothing)
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
        Call MakeReplyOrDirectStatus(False, True, True)
    End Sub

    Private Sub PostWorker_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles PostWorker.DoWork
        If _endingFlag Then
            e.Cancel = True
            Exit Sub
        End If

        Dim ret As String = ""
        Dim rslt As New GetWorkerResult

        Dim args As GetWorkerArg = DirectCast(e.Argument, GetWorkerArg)
        Try
            If args.type = WORKERTYPE.CreateNewSocket Then
                Call clsTwPost.CreateNewSocket()
            Else
                '            For i As Integer = 0 To 2
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
        Catch ex As Exception
            If _endingFlag Then
                e.Cancel = True
                Exit Sub
            End If
            My.Application.Log.DefaultFileLogWriter.Location = Logging.LogFileLocation.ExecutableDirectory
            My.Application.Log.DefaultFileLogWriter.MaxFileSize = 102400
            My.Application.Log.DefaultFileLogWriter.AutoFlush = True
            My.Application.Log.DefaultFileLogWriter.Append = False
            'My.Application.Log.WriteException(ex, _
            '    Diagnostics.TraceEventType.Critical, _
            '    "Source=" + ex.Source + " StackTrace=" + ex.StackTrace + " InnerException=" + IIf(ex.InnerException Is Nothing, "", ex.InnerException.Message))
            My.Application.Log.WriteException(ex, _
                Diagnostics.TraceEventType.Critical, _
                ex.StackTrace + vbCrLf + Now.ToString + vbCrLf + args.type.ToString + vbCrLf + args.status)
            rslt.retMsg = "Tween 例外発生(PostWorker_DoWork)"
            rslt.TLine = Nothing
            rslt.page = args.page
            rslt.endPage = args.endPage
            rslt.type = args.type

            e.Result = rslt
        End Try
    End Sub

    Private Sub PostWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles PostWorker.RunWorkerCompleted
        If _endingFlag Then
            Exit Sub
        End If

        Dim rslt As GetWorkerResult = DirectCast(e.Result, GetWorkerResult)
        Dim args As New GetWorkerArg

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
                    If RemainPostNum > 1 Then RemainPostNum -= 1
                    If TimerPostCounter.Enabled = False Then TimerPostCounter.Enabled = True
                    StatusLabel.Text = "POST完了"
                    StatusText.Text = ""
                    _history.Add("")
                    _hisIdx = _history.Count - 1
                    SetMainWindowTitle()
                End If

                args.page = 1
                args.endPage = 1
                args.type = WORKERTYPE.Timeline
                If GetTimelineWorker.IsBusy = False Then
                    'TimerTimeline.Enabled = False
                    StatusLabel.Text = "Recent更新中..."
                    NotifyIcon1.Icon = NIconRefresh(0)
                    _refreshIconCnt = 0
                    TimerRefreshIcon.Enabled = True
                    Do While GetTimelineWorker.IsBusy
                        Threading.Thread.Sleep(100)
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
            If TabDialog.ShowDialog = Windows.Forms.DialogResult.Cancel Then Exit Sub
            tabName = TabDialog.SelectedTabName

            ListTab.SelectedTab.Focus()
            If tabName = "(新規タブ)" Then
                tabName = InputBox("タブ名を指定してください。", "タブ追加", "MyTab" + _tabs.Count.ToString)
                If tabName <> "" Then
                    If AddNewTab(tabName) = False Then
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
        If mv = False Then
            If MessageBox.Show("マークをつけますか？" + vbCrLf + "  「はい」　：つける" + vbCrLf + "  「いいえ」：つけない", _
               "マーク確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                mk = True
            Else
                mk = False
            End If
        End If
        For Each ts As TabStructure In _tabs
            If ts.tabName = tabName Then
                Dim ids As New Collections.Specialized.StringCollection
                For Each itm As ListViewItem In myList.SelectedItems
                    If ids.Contains(itm.SubItems(4).Text) = False Then
                        ids.Add(itm.SubItems(4).Text)
                        Dim flt As New FilterClass
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
                Call ReFilter()
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
        If clsTw IsNot Nothing Then
            Call clsTw.GetWedata()
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

        Dim itms As New List(Of ListViewItem)
        For Each ts As TabStructure In _tabs
            If ts.tabName <> "Recent" And ts.tabName <> "Reply" And ts.tabName <> "Direct" And ts.modified Then
                For Each itmo As ListViewItem In ts.listCustom.Items
                    itms.Add(itmo.Clone)
                    'Dim i As Integer
                    'For i = 0 To _tabs(0).listCustom.Items.Count - 1
                    '    If itm.SubItems(5).Text = _tabs(0).listCustom.Items(i).SubItems(5).Text Then
                    '        Exit For
                    '    End If
                    'Next
                    'If i > _tabs(0).listCustom.Items.Count - 1 Then
                    '    Dim itm2 As ListViewItem = itm.Clone
                    '    _tabs(0).allCount += 1
                    '    If itm.SubItems(8).Text = "False" Then
                    '        If _tabs(0).unreadManage And SettingDialog.UnreadManage Then
                    '            _tabs(0).unreadCount += 1
                    '            _tabs(0).oldestUnreadItem = Nothing
                    '            'If _tabs(0).oldestUnreadItem Is Nothing Then
                    '            '    _tabs(0).oldestUnreadItem = itm2
                    '            'Else
                    '            '    If _tabs(0).oldestUnreadItem.SubItems(5).Text > itm2.SubItems(5).Text Then
                    '            '        _tabs(0).oldestUnreadItem = itm2
                    '            '    End If
                    '            'End If
                    '        Else
                    '            itm2.SubItems(8).Text = "True"
                    '        End If
                    '    End If
                    '    _tabs(0).listCustom.Items.Add(itm2)
                    'End If
                Next
                ts.oldestUnreadItem = Nothing
                ts.listCustom.Items.Clear()
                ts.unreadCount = 0
                ts.allCount = 0
                ts.tabPage.ImageIndex = -1

                For Each ts2 As TabStructure In _tabs
                    If ts2.Equals(ts) = False And ts2.tabName <> "Reply" And ts2.tabName <> "Direct" Then
                        For Each itm As ListViewItem In ts2.listCustom.Items
                            Dim mv As Boolean = False
                            Dim nf As Boolean = False
                            Dim mk As Boolean = False
                            Dim lItem As New Twitter.MyListItem

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
                            lItem.Readed = IIf(itm.SubItems(8).Text = "True", True, False)
                            itm.SubItems(0).Text = itm.SubItems(0).Text.Replace("♪", "")

                            '                            For Each ts As TabStructure In _tabs
                            Dim hit As Boolean = False

                            For Each ft As FilterClass In ts.filters
                                Dim bHit As Boolean = True
                                Dim tBody As String = IIf(ft.SearchURL, lItem.OrgData, lItem.Data)
                                If ft.SearchBoth Then
                                    If ft.IDFilter = "" Or lItem.Name = ft.IDFilter Then
                                        For Each fs As String In ft.BodyFilter
                                            If ft.UseRegex Then
                                                If Regex.IsMatch(tBody, fs, RegexOptions.IgnoreCase) = False Then bHit = False
                                            Else
                                                If tBody.ToLower.Contains(fs.ToLower) = False Then bHit = False
                                            End If
                                            If bHit = False Then Exit For
                                        Next
                                    Else
                                        bHit = False
                                    End If
                                Else
                                    For Each fs As String In ft.BodyFilter
                                        If ft.UseRegex Then
                                            If Regex.IsMatch(lItem.Name + tBody, fs, RegexOptions.IgnoreCase) = False Then bHit = False
                                        Else
                                            If (lItem.Name + tBody).ToLower.Contains(fs.ToLower) = False Then bHit = False
                                        End If
                                        If bHit = False Then Exit For
                                    Next
                                End If
                                If bHit = True Then
                                    hit = True
                                    If ft.SetMark Then mk = True
                                    If ft.moveFrom Then mv = True
                                End If
                                If hit And mv And mk Then Exit For
                            Next
                            For Each itmo2 As ListViewItem In ts.listCustom.Items
                                If itmo2.SubItems(5).Text = itm.SubItems(5).Text Then
                                    hit = False
                                    Exit For
                                End If
                            Next
                            If hit Then
                                Dim itm2 As ListViewItem = itm.Clone
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
                            If ts.unreadCount > 0 And ts.tabPage.ImageIndex = -1 Then ts.tabPage.ImageIndex = 0
                            'Next
                            If ts2.tabName = "Recent" Then
                                If mv = False Then
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
                    Dim lItem As New Twitter.MyListItem

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
                    lItem.Readed = IIf(itm.SubItems(8).Text = "True", True, False)
                    itm.SubItems(0).Text = itm.SubItems(0).Text.Replace("♪", "")

                    '                            For Each ts As TabStructure In _tabs
                    Dim hit As Boolean = False

                    For Each ft As FilterClass In ts.filters
                        Dim bHit As Boolean = True
                        Dim tBody As String = IIf(ft.SearchURL, lItem.OrgData, lItem.Data)
                        If ft.SearchBoth Then
                            If ft.IDFilter = "" Or lItem.Name = ft.IDFilter Then
                                For Each fs As String In ft.BodyFilter
                                    If ft.UseRegex Then
                                        If Regex.IsMatch(tBody, fs, RegexOptions.IgnoreCase) = False Then bHit = False
                                    Else
                                        If tBody.ToLower.Contains(fs.ToLower) = False Then bHit = False
                                    End If
                                    If bHit = False Then Exit For
                                Next
                            Else
                                bHit = False
                            End If
                        Else
                            For Each fs As String In ft.BodyFilter
                                If ft.UseRegex Then
                                    If Regex.IsMatch(lItem.Name + tBody, fs, RegexOptions.IgnoreCase) = False Then bHit = False
                                Else
                                    If (lItem.Name + tBody).ToLower.Contains(fs.ToLower) = False Then bHit = False
                                End If
                                If bHit = False Then Exit For
                            Next
                        End If
                        If bHit = True Then
                            hit = True
                            If ft.SetMark Then mk = True
                            If ft.moveFrom Then mv = True
                        End If
                        If hit And mv And mk Then Exit For
                    Next
                    For Each itmo2 As ListViewItem In ts.listCustom.Items
                        If itmo2.SubItems(5).Text = itm.SubItems(5).Text Then
                            hit = False
                            Exit For
                        End If
                    Next
                    If hit Then
                        Dim itm2 As ListViewItem = itm.Clone
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
                                If _tabs(0).unreadManage And SettingDialog.UnreadManage Then
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
                            _tabs(0).listCustom.Items.Add(itm.Clone)
                        End If
                    End If
                    If ts.unreadCount > 0 And ts.tabPage.ImageIndex = -1 Then ts.tabPage.ImageIndex = 0
                    If _tabs(0).unreadCount > 0 And _tabs(0).tabPage.ImageIndex = -1 Then _tabs(0).tabPage.ImageIndex = 0
                Next
            End If
        Next

        For Each ts As TabStructure In _tabs
            If ts.modified Then
                ts.modified = False
            End If
            ts.listCustom.EndUpdate()
        Next

        'For Each itm As ListViewItem In _tabs(0).listCustom.Items
        '    Dim mv As Boolean = False
        '    Dim nf As Boolean = False
        '    Dim mk As Boolean = False
        '    Dim lItem As New Twitter.MyListItem

        '    lItem.Data = itm.SubItems(2).Text
        '    lItem.Fav = CBool(itm.SubItems(9).Text)
        '    lItem.Id = itm.SubItems(5).Text
        '    lItem.ImageUrl = itm.SubItems(6).Text
        '    lItem.Name = itm.SubItems(4).Text
        '    lItem.Nick = itm.SubItems(1).Text
        '    lItem.OrgData = itm.SubItems(7).Text
        '    lItem.PDate = CDate(itm.SubItems(3).Text)
        '    lItem.Protect = itm.SubItems(0).Text.Contains("Ю")
        '    lItem.Reply = CBool(itm.SubItems(11).Text)
        '    lItem.Readed = IIf(itm.SubItems(8).Text = "True", True, False)
        '    itm.SubItems(0).Text = itm.SubItems(0).Text.Replace("♪", "")

        '    For Each ts As TabStructure In _tabs
        '        Dim hit As Boolean = False

        '        For Each ft As FilterClass In ts.filters
        '            Dim bHit As Boolean = True
        '            Dim tBody As String = IIf(ft.SearchURL, lItem.OrgData, lItem.Data)
        '            If ft.SearchBoth Then
        '                If ft.IDFilter = "" Or lItem.Name = ft.IDFilter Then
        '                    For Each fs As String In ft.BodyFilter
        '                        If ft.UseRegex Then
        '                            If Regex.IsMatch(tBody, fs, RegexOptions.IgnoreCase) = False Then bHit = False
        '                        Else
        '                            If tBody.ToLower.Contains(fs.ToLower) = False Then bHit = False
        '                        End If
        '                        If bHit = False Then Exit For
        '                    Next
        '                Else
        '                    bHit = False
        '                End If
        '            Else
        '                For Each fs As String In ft.BodyFilter
        '                    If ft.UseRegex Then
        '                        If Regex.IsMatch(lItem.Name + tBody, fs, RegexOptions.IgnoreCase) = False Then bHit = False
        '                    Else
        '                        If (lItem.Name + tBody).ToLower.Contains(fs.ToLower) = False Then bHit = False
        '                    End If
        '                    If bHit = False Then Exit For
        '                Next
        '            End If
        '            If bHit = True Then
        '                hit = True
        '                If ft.SetMark Then mk = True
        '                If ft.moveFrom Then mv = True
        '            End If
        '            If hit And mv And mk Then Exit For
        '        Next
        '        If hit Then
        '            Dim itm2 As ListViewItem = itm.Clone
        '            ts.allCount += 1
        '            If itm2.SubItems(8).Text = "False" Then
        '                If ts.unreadManage And SettingDialog.UnreadManage Then
        '                    ts.unreadCount += 1
        '                    If ts.oldestUnreadItem Is Nothing Then
        '                        ts.oldestUnreadItem = itm2
        '                    Else
        '                        If ts.oldestUnreadItem.SubItems(5).Text > itm2.SubItems(5).Text Then
        '                            ts.oldestUnreadItem = itm2
        '                        End If
        '                    End If
        '                Else
        '                    itm2.SubItems(8).Text = "True"
        '                End If
        '            End If
        '            ts.listCustom.Items.Add(itm2)
        '        End If
        '        If ts.unreadCount > 0 And ts.tabPage.ImageIndex = -1 Then ts.tabPage.ImageIndex = 0
        '    Next
        '    If mv = False Then
        '        If mk Then itm.SubItems(0).Text += "♪"
        '    Else
        '        _tabs(0).allCount -= 1
        '        If itm.SubItems(8).Text = "False" Then _tabs(0).unreadCount -= 1
        '        _tabs(0).listCustom.Items.Remove(itm)
        '    End If
        'Next
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
            End If

            If openUrlStr <> "" Then
                Do While ExecWorker.IsBusy
                    Threading.Thread.Sleep(100)
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
        If SettingDialog.DispLatestPost <> DispTitleEnum.None And _
           SettingDialog.DispLatestPost <> DispTitleEnum.Post And _
           SettingDialog.DispLatestPost <> DispTitleEnum.Ver Then
            For Each ts As TabStructure In _tabs
                ur += ts.unreadCount
                al += ts.allCount
            Next
        End If
        If SettingDialog.DispUsername = True Then ttl = _username + " - "
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
        For Each ts As TabStructure In _tabs
            ur += ts.unreadCount
            al += ts.allCount
            If ts.tabPage.Equals(ListTab.SelectedTab) Then
                tur = ts.unreadCount
                tal = ts.allCount
            End If
        Next
        'StatusLabelUrl.Text = "タブ: " + tur.ToString() + "/" + tal.ToString() + " 全体:" + ur.ToString() + "/" + al.ToString() + _
        '        " (返信: " + urat.ToString() + ") POST残り " + RemainPostNum.ToString() + "回/最大 " + SettingDialog.MaxPostNum.ToString() + "回 更新間隔:" + (TimerTimeline.Interval / 1000).ToString
        StatusLabelUrl.Text = "[タブ: " + tur.ToString() + "/" + tal.ToString() + " 全体: " + ur.ToString() + "/" + al.ToString() + _
                " (返信: " + urat.ToString() + ")] [時速: " + _postTimestamps.Count.ToString() + "/" + _tlCount.ToString + "] [間隔: " + IIf(SettingDialog.TimelinePeriodInt = 0, "-", (TimerTimeline.Interval / 1000).ToString) + "]"
    End Sub

    Private Sub SetNotifyIconText()
        ' タスクトレイアイコンのツールチップテキスト書き換え

        If SettingDialog.DispUsername = True Then
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

        If _reply_to_name = Nothing Then
            _reply_to_id = 0
            Exit Sub
        End If

        m = id.Matches(StatusText)

        If m IsNot Nothing AndAlso m.Count = 1 AndAlso m.Item(0).Value = "@" + _reply_to_name AndAlso StatusText.StartsWith(". ") = False Then
            Exit Sub
        End If

        _reply_to_id = 0
        _reply_to_name = Nothing

    End Sub

    Private Sub TweenMain_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        If SettingDialog.MinimizeToTray = True And WindowState = FormWindowState.Minimized Then
            Me.Visible = False
        End If
    End Sub

    Private Sub PlaySoundMenuItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlaySoundMenuItem.CheckedChanged
        If PlaySoundMenuItem.Checked = True Then
            SettingDialog.PlaySound = True
        Else
            SettingDialog.PlaySound = False
        End If
    End Sub

    Private Sub TimerPostCounter_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerPostCounter.Tick
        __PostCounter -= 1          'カウントダウン
        If __PostCounter < 0 Then
            '1時間経過(=60回割り込み発生)したら残りPOST数リセット
            RemainPostNum = SettingDialog.MaxPostNum
            __PostCounter = 59
        End If
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
    'Public idCol As System.Collections.Specialized.StringCollection
    Public tabName As String
    'Public sorter As ListViewItemComparer
    Public unreadManage As Boolean
    Public notify As Boolean
    Public soundFile As String
    Public filters As New List(Of FilterClass)
    Public modified As Boolean
    Public oldestUnreadItem As ListViewItem
    Public unreadCount As Integer
    Public allCount As Integer
End Class

Public Class FilterClass
    Public IDFilter As String
    Public BodyFilter As New Collections.Specialized.StringCollection
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
        If Not (_columnModes Is Nothing) And _
                _columnModes.Length > _column Then
            _mode = _columnModes(_column)
        End If
        '並び替えの方法別に、xとyを比較する
        Select Case _mode
            Case ComparerMode.String
                result = String.Compare(itemx.SubItems(_column).Text, _
                    itemy.SubItems(_column).Text)
            Case ComparerMode.Integer
                result = Integer.Parse(itemx.SubItems(_column).Text) - _
                    Integer.Parse(itemy.SubItems(_column).Text)
            Case ComparerMode.DateTime
                'result = DateTime.Compare( _
                '    DateTime.Parse(itemx.SubItems(_column).Text, datetimeformatinfo), _
                '    DateTime.Parse(itemy.SubItems(_column).Text))
                'result = String.Compare( _
                '    itemx.SubItems(_column).Text, _
                '    itemy.SubItems(_column).Text)
                'StatusID(?)でソート
                result = String.Compare( _
                    itemx.SubItems(5).Text, _
                    itemy.SubItems(5).Text)
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
