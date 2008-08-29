﻿
Public Class Setting
    Private _MyuserID As String
    Private _Mypassword As String
    Private _MytimelinePeriod As Integer
    Private _MyDMPeriod As Integer
    Private _MynextThreshold As Integer
    Private _MyNextPages As Integer
    Private _MyLogDays As Integer
    Private _MyLogUnit As LogUnitEnum
    Private _MyReadPages As Integer
    Private _MyReadPagesReply As Integer
    Private _MyReadPagesDM As Integer
    Private _MyReaded As Boolean
    Private _MyIconSize As IconSizes
    Private _MyStatusText As String
    Private _MyUnreadManage As Boolean
    Private _MyPlaySound As Boolean
    Private _MyOneWayLove As Boolean
    Private _fntUnread As Font
    Private _clUnread As Color
    Private _fntReaded As Font
    Private _clReaded As Color
    Private _clFav As Color
    Private _clOWL As Color
    Private _fntDetail As Font
    Private _clSelf As Color
    Private _clAtSelf As Color
    Private _clTarget As Color
    Private _clAtTarget As Color
    Private _clAtFromTarget As Color
    Private _MyNameBalloon As NameBalloonEnum
    Private _MyPostCtrlEnter As Boolean
    Private _useAPI As Boolean
    Private _hubServer As String
    Private _browserpath As String
    Private _MyCheckReply As Boolean
    Private _MyUseRecommendStatus As Boolean
    Private _MyDispUsername As Boolean
    Private _MyDispLatestPost As DispTitleEnum
    Private _MySortOrderLock As Boolean
    Private _MyMinimizeToTray As Boolean
    Private _MyCloseToExit As Boolean

    Public Enum LogUnitEnum
        Minute
        Hour
        Day
    End Enum

    Public Enum IconSizes
        IconNone = 0
        Icon16 = 1
        Icon24 = 2
        Icon48 = 3
        Icon48_2 = 4
    End Enum

    Public Enum NameBalloonEnum
        None
        UserID
        NickName
    End Enum

    Public Enum DispTitleEnum
        None
        Ver
        Post
        UnreadRepCount
        UnreadAllCount
        UnreadAllRepCount
        UnreadCountAllCount
    End Enum

    Private Sub Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Save.Click
        If Username.Text.Trim = "" Or _
           Password.Text.Trim = "" Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            MessageBox.Show("ユーザー名とパスワードを指定してください。")
            Exit Sub
        End If
        If Username.Text.Contains("@") Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            MessageBox.Show("ユーザー名に『@』を含めないでください。（メールアドレス不可）")
            Exit Sub
        End If
        Try
            _MyuserID = Username.Text.Trim()
            _Mypassword = Password.Text.Trim()
            _MytimelinePeriod = CType(TimelinePeriod.Text, Integer)
            _MyDMPeriod = CType(DMPeriod.Text, Integer)
            _MynextThreshold = CType(NextThreshold.Text, Integer)
            _MyNextPages = CType(NextPages.Text, Integer)
            '_MyLogDays = CType(ReadLogDays.Text, Integer)
            'Select Case ReadLogUnit.SelectedIndex
            '    Case 0
            '        _MyLogUnit = LogUnitEnum.Minute
            '    Case 1
            '        _MyLogUnit = LogUnitEnum.Hour
            '    Case 2
            '        _MyLogUnit = LogUnitEnum.Day
            '    Case Else
            '        _MyLogUnit = LogUnitEnum.Hour
            'End Select
            _MyReadPages = CType(StartupReadPages.Text, Integer)
            _MyReadPagesReply = CType(StartupReadReply.Text, Integer)
            _MyReadPagesDM = CType(StartupReadDM.Text, Integer)
            _MyReaded = StartupReaded.Checked
            Select Case IconSize.SelectedIndex
                Case 0
                    _MyIconSize = IconSizes.IconNone
                Case 1
                    _MyIconSize = IconSizes.Icon16
                Case 2
                    _MyIconSize = IconSizes.Icon24
                Case 3
                    _MyIconSize = IconSizes.Icon48
                Case 4
                    _MyIconSize = IconSizes.Icon48_2
            End Select
            _MyStatusText = StatusText.Text
            _MyPlaySound = PlaySnd.Checked
            _MyUnreadManage = UReadMng.Checked
            _MyOneWayLove = OneWayLv.Checked

            _fntUnread = lblUnRead.Font
            _clUnread = lblUnRead.ForeColor
            _fntReaded = lblReaded.Font
            _clReaded = lblReaded.ForeColor
            _clFav = lblFav.ForeColor
            _clOWL = lblOWL.ForeColor
            _fntDetail = lblDetail.Font
            _clSelf = lblSelf.BackColor
            _clAtSelf = lblAtSelf.BackColor
            _clTarget = lblTarget.BackColor
            _clAtTarget = lblAtTarget.BackColor
            _clAtFromTarget = lblAtFromTarget.BackColor
            Select Case cmbNameBalloon.SelectedIndex
                Case 0
                    _MyNameBalloon = NameBalloonEnum.None
                Case 1
                    _MyNameBalloon = NameBalloonEnum.UserID
                Case 2
                    _MyNameBalloon = NameBalloonEnum.NickName
            End Select
            _MyPostCtrlEnter = CheckPostCtrlEnter.Checked
            _useAPI = CheckUseAPI.Checked
            _hubServer = HubServerDomain.Text.Trim
            _browserpath = BrowserPathText.Text.Trim
            _MyCheckReply = CheckboxReply.Checked
            _MyUseRecommendStatus = CheckUseRecommendStatus.Checked
            _MyDispUsername = CheckDispUsername.Checked
            _MyCloseToExit = CheckCloseToExit.Checked
            _MyMinimizeToTray = CheckMinimizeToTray.Checked
            Select Case ComboDispTitle.SelectedIndex
                Case 0  'None
                    _MyDispLatestPost = DispTitleEnum.None
                Case 1  'Ver
                    _MyDispLatestPost = DispTitleEnum.Ver
                Case 2  'Post
                    _MyDispLatestPost = DispTitleEnum.Post
                Case 3  'RepCount
                    _MyDispLatestPost = DispTitleEnum.UnreadRepCount
                Case 4  'AllCount
                    _MyDispLatestPost = DispTitleEnum.UnreadAllCount
                Case 5  'Rep+All
                    _MyDispLatestPost = DispTitleEnum.UnreadAllRepCount
                Case 6  'Unread/All
                    _MyDispLatestPost = DispTitleEnum.UnreadCountAllCount
            End Select
            _MySortOrderLock = CheckSortOrderLock.Checked

            'TweenMain.SetMainWindowTitle()
            'TweenMain.SetNotifyIconText()

        Catch ex As Exception
            MessageBox.Show("設定値に誤りがあります。")
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Exit Sub
        End Try
    End Sub

    Private Sub Setting_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Username.Text = _MyuserID
        Password.Text = _Mypassword
        TimelinePeriod.Text = _MytimelinePeriod.ToString()
        DMPeriod.Text = _MyDMPeriod.ToString()
        NextThreshold.Text = _MynextThreshold.ToString()
        NextPages.Text = _MyNextPages.ToString()
        'ReadLogDays.Text = _MyLogDays.ToString()
        'Select Case _MyLogUnit
        '    Case LogUnitEnum.Minute
        '        ReadLogUnit.SelectedIndex = 0
        '    Case LogUnitEnum.Hour
        '        ReadLogUnit.SelectedIndex = 1
        '    Case LogUnitEnum.Day
        '        ReadLogUnit.SelectedIndex = 2
        'End Select
        StartupReadPages.Text = _MyReadPages.ToString()
        StartupReadReply.Text = _MyReadPagesReply.ToString()
        StartupReadDM.Text = _MyReadPagesDM.ToString()
        StartupReaded.Checked = _MyReaded
        Select Case _MyIconSize
            Case IconSizes.IconNone
                IconSize.SelectedIndex = 0
            Case IconSizes.Icon16
                IconSize.SelectedIndex = 1
            Case IconSizes.Icon24
                IconSize.SelectedIndex = 2
            Case IconSizes.Icon48
                IconSize.SelectedIndex = 3
            Case IconSizes.Icon48_2
                IconSize.SelectedIndex = 4
        End Select
        StatusText.Text = _MyStatusText
        UReadMng.Checked = _MyUnreadManage
        If _MyUnreadManage = False Then
            StartupReaded.Enabled = False
        Else
            StartupReaded.Enabled = True
        End If
        PlaySnd.Checked = _MyPlaySound
        OneWayLv.Checked = _MyOneWayLove

        lblUnRead.Font = _fntUnread
        lblUnRead.ForeColor = _clUnread
        lblReaded.Font = _fntReaded
        lblReaded.ForeColor = _clReaded
        lblFav.ForeColor = _clFav
        lblOWL.ForeColor = _clOWL
        lblDetail.Font = _fntDetail
        lblSelf.BackColor = _clSelf
        lblAtSelf.BackColor = _clAtSelf
        lblTarget.BackColor = _clTarget
        lblAtTarget.BackColor = _clAtTarget
        lblAtFromTarget.BackColor = _clAtFromTarget

        Select Case _MyNameBalloon
            Case NameBalloonEnum.None
                cmbNameBalloon.SelectedIndex = 0
            Case NameBalloonEnum.UserID
                cmbNameBalloon.SelectedIndex = 1
            Case NameBalloonEnum.NickName
                cmbNameBalloon.SelectedIndex = 2
        End Select

        CheckPostCtrlEnter.Checked = _MyPostCtrlEnter
        CheckUseAPI.Checked = _useAPI
        HubServerDomain.Text = _hubServer
        BrowserPathText.Text = _browserpath
        CheckboxReply.Checked = _MyCheckReply
        CheckUseRecommendStatus.Checked = _MyUseRecommendStatus
        CheckDispUsername.Checked = _MyDispUsername
        CheckCloseToExit.Checked = _MyCloseToExit
        CheckMinimizeToTray.Checked = _MyMinimizeToTray
        Select Case _MyDispLatestPost
            Case DispTitleEnum.None
                ComboDispTitle.SelectedIndex = 0
            Case DispTitleEnum.Ver
                ComboDispTitle.SelectedIndex = 1
            Case DispTitleEnum.Post
                ComboDispTitle.SelectedIndex = 2
            Case DispTitleEnum.UnreadRepCount
                ComboDispTitle.SelectedIndex = 3
            Case DispTitleEnum.UnreadAllCount
                ComboDispTitle.SelectedIndex = 4
            Case DispTitleEnum.UnreadAllRepCount
                ComboDispTitle.SelectedIndex = 5
            Case DispTitleEnum.UnreadCountAllCount
                ComboDispTitle.SelectedIndex = 6
        End Select
        CheckSortOrderLock.Checked = _MySortOrderLock

        'TweenMain.SetMainWindowTitle()
        'TweenMain.SetNotifyIconText()

    End Sub

    Private Sub TimelinePeriod_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TimelinePeriod.Validating
        Dim prd As Integer
        Try
            prd = CType(TimelinePeriod.Text, Integer)
        Catch ex As Exception
            MessageBox.Show("更新間隔には数値（0または15〜600）を指定してください。")
            e.Cancel = True
            Exit Sub
        End Try

        If prd <> 0 And (prd < 15 Or prd > 600) Then
            MessageBox.Show("更新間隔には数値（0または15〜600）を指定してください。")
            e.Cancel = True
        End If
    End Sub

    Private Sub NextThreshold_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles NextThreshold.Validating
        Dim thr As Integer
        Try
            thr = CType(NextThreshold.Text, Integer)
        Catch ex As Exception
            MessageBox.Show("閾値には数値（1〜20）を指定してください。")
            e.Cancel = True
            Exit Sub
        End Try

        If thr < 1 Or thr > 20 Then
            MessageBox.Show("閾値には数値（1〜20）を指定してください。")
            e.Cancel = True
        End If
    End Sub

    Private Sub NextPages_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles NextPages.Validating
        Dim thr As Integer
        Try
            thr = CType(NextPages.Text, Integer)
        Catch ex As Exception
            MessageBox.Show("ページ数には数値（1〜20）を指定してください。")
            e.Cancel = True
            Exit Sub
        End Try

        If thr < 1 Or thr > 20 Then
            MessageBox.Show("ページ数には数値（1〜20）を指定してください。")
            e.Cancel = True
        End If
    End Sub

    Private Sub DMPeriod_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles DMPeriod.Validating
        Dim prd As Integer
        Try
            prd = CType(DMPeriod.Text, Integer)
        Catch ex As Exception
            MessageBox.Show("更新間隔には数値（0または15〜600）を指定してください。")
            e.Cancel = True
            Exit Sub
        End Try

        If prd <> 0 And (prd < 15 Or prd > 600) Then
            MessageBox.Show("更新間隔には数値（0または15〜600）を指定してください。")
            e.Cancel = True
        End If
    End Sub

    Private Sub ReadLogDays_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ReadLogDays.Validating
        'Dim days As Integer
        'Try
        '    days = CType(ReadLogDays.Text, Integer)
        'Catch ex As Exception
        '    MessageBox.Show("読み込み日数には数値（0〜7）を指定してください。")
        '    e.Cancel = True
        '    Exit Sub
        'End Try

        'If days < 0 Or days > 7 Then
        '    MessageBox.Show("読み込み日数には数値（0〜7）を指定してください。")
        '    e.Cancel = True
        'End If
    End Sub

    Private Sub StartupReadPages_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles StartupReadPages.Validating
        Dim pages As Integer
        Try
            pages = CType(StartupReadPages.Text, Integer)
        Catch ex As Exception
            MessageBox.Show("読み込みページ数には数値（1〜999）を指定してください。")
            e.Cancel = True
            Exit Sub
        End Try

        If pages < 1 Or pages > 999 Then
            MessageBox.Show("読み込みページ数には数値（1〜999）を指定してください。")
            e.Cancel = True
        End If
    End Sub

    Private Sub StartupReadReply_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles StartupReadReply.Validating
        Dim pages As Integer
        Try
            pages = CType(StartupReadReply.Text, Integer)
        Catch ex As Exception
            MessageBox.Show("読み込みページ数には数値（0〜999）を指定してください。")
            e.Cancel = True
            Exit Sub
        End Try

        If pages < 0 Or pages > 999 Then
            MessageBox.Show("読み込みページ数には数値（0〜999）を指定してください。")
            e.Cancel = True
        End If
    End Sub

    Private Sub StartupReadDM_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles StartupReadDM.Validating
        Dim pages As Integer
        Try
            pages = CType(StartupReadDM.Text, Integer)
        Catch ex As Exception
            MessageBox.Show("読み込みページ数には数値（1〜999）を指定してください。")
            e.Cancel = True
            Exit Sub
        End Try

        If pages < 1 Or pages > 999 Then
            MessageBox.Show("読み込みページ数には数値（1〜999）を指定してください。")
            e.Cancel = True
        End If
    End Sub

    Private Sub UReadMng_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If UReadMng.Checked = True Then
            StartupReaded.Enabled = True
        Else
            StartupReaded.Enabled = False
        End If
    End Sub

    Private Sub btnFontAndColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUnread.Click, btnReaded.Click, btnDetail.Click
        Dim Btn As Button = CType(sender, Button)
        Dim rtn As DialogResult

        FontDialog1.AllowVerticalFonts = False
        FontDialog1.AllowScriptChange = True
        FontDialog1.AllowSimulations = True
        FontDialog1.AllowVectorFonts = True
        FontDialog1.FixedPitchOnly = False
        FontDialog1.FontMustExist = True
        FontDialog1.ScriptsOnly = False
        FontDialog1.ShowApply = False
        FontDialog1.ShowEffects = True
        FontDialog1.ShowColor = True

        Select Case Btn.Name
            Case "btnUnread"
                FontDialog1.Color = lblUnRead.ForeColor
                FontDialog1.Font = lblUnRead.Font
            Case "btnReaded"
                FontDialog1.Color = lblReaded.ForeColor
                FontDialog1.Font = lblReaded.Font
            Case "btnDetail"
                FontDialog1.Font = lblDetail.Font
                FontDialog1.ShowColor = False
                FontDialog1.ShowEffects = False
        End Select

        rtn = FontDialog1.ShowDialog

        If rtn = Windows.Forms.DialogResult.Cancel Then Exit Sub

        Select Case Btn.Name
            Case "btnUnread"
                lblUnRead.ForeColor = FontDialog1.Color
                lblUnRead.Font = FontDialog1.Font
            Case "btnReaded"
                lblReaded.ForeColor = FontDialog1.Color
                lblReaded.Font = FontDialog1.Font
            Case "btnDetail"
                lblDetail.Font = FontDialog1.Font
        End Select

    End Sub

    Private Sub btnColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelf.Click, btnAtSelf.Click, btnTarget.Click, btnAtTarget.Click, btnAtFromTarget.Click, btnFav.Click, btnOWL.Click
        Dim Btn As Button = CType(sender, Button)
        Dim rtn As DialogResult

        ColorDialog1.AllowFullOpen = True
        ColorDialog1.AnyColor = True
        ColorDialog1.FullOpen = False
        ColorDialog1.SolidColorOnly = False

        Select Case Btn.Name
            Case "btnSelf"
                ColorDialog1.Color = lblSelf.BackColor
            Case "btnAtSelf"
                ColorDialog1.Color = lblAtSelf.BackColor
            Case "btnTarget"
                ColorDialog1.Color = lblTarget.BackColor
            Case "btnAtTarget"
                ColorDialog1.Color = lblAtTarget.BackColor
            Case "btnAtFromTarget"
                ColorDialog1.Color = lblAtFromTarget.BackColor
            Case "btnFav"
                ColorDialog1.Color = lblFav.ForeColor
            Case "btnOWL"
                ColorDialog1.Color = lblOWL.ForeColor
        End Select

        rtn = ColorDialog1.ShowDialog

        If rtn = Windows.Forms.DialogResult.Cancel Then Exit Sub

        Select Case Btn.Name
            Case "btnSelf"
                lblSelf.BackColor = ColorDialog1.Color
            Case "btnAtSelf"
                lblAtSelf.BackColor = ColorDialog1.Color
            Case "btnTarget"
                lblTarget.BackColor = ColorDialog1.Color
            Case "btnAtTarget"
                lblAtTarget.BackColor = ColorDialog1.Color
            Case "btnAtFromTarget"
                lblAtFromTarget.BackColor = ColorDialog1.Color
            Case "btnFav"
                lblFav.ForeColor = ColorDialog1.Color
            Case "btnOWL"
                lblOWL.ForeColor = ColorDialog1.Color
        End Select
    End Sub

    Public Property UserID() As String
        Get
            Return _MyuserID
        End Get
        Set(ByVal value As String)
            _MyuserID = value
        End Set
    End Property

    Public Property PasswordStr() As String
        Get
            Return _Mypassword
        End Get
        Set(ByVal value As String)
            _Mypassword = value
        End Set
    End Property

    Public Property TimelinePeriodInt() As Integer
        Get
            Return _MytimelinePeriod
        End Get
        Set(ByVal value As Integer)
            _MytimelinePeriod = value
        End Set
    End Property

    Public Property DMPeriodInt() As Integer
        Get
            Return _MyDMPeriod
        End Get
        Set(ByVal value As Integer)
            _MyDMPeriod = value
        End Set
    End Property

    Public Property NextPageThreshold() As Integer
        Get
            Return _MynextThreshold
        End Get
        Set(ByVal value As Integer)
            _MynextThreshold = value
        End Set
    End Property

    Public Property NextPagesInt() As Integer
        Get
            Return _MyNextPages
        End Get
        Set(ByVal value As Integer)
            _MyNextPages = value
        End Set
    End Property

    Public Property LogDays() As Integer
        Get
            Return _MyLogDays
        End Get
        Set(ByVal value As Integer)
            _MyLogDays = value
        End Set
    End Property

    Public Property LogUnit() As LogUnitEnum
        Get
            Return _MyLogUnit
        End Get
        Set(ByVal value As LogUnitEnum)
            _MyLogUnit = value
        End Set
    End Property

    Public Property ReadPages() As Integer
        Get
            Return _MyReadPages
        End Get
        Set(ByVal value As Integer)
            _MyReadPages = value
        End Set
    End Property

    Public Property ReadPagesReply() As Integer
        Get
            Return _MyReadPagesReply
        End Get
        Set(ByVal value As Integer)
            _MyReadPagesReply = value
        End Set
    End Property

    Public Property ReadPagesDM() As Integer
        Get
            Return _MyReadPagesDM
        End Get
        Set(ByVal value As Integer)
            _MyReadPagesDM = value
        End Set
    End Property

    Public Property Readed() As Boolean
        Get
            Return _MyReaded
        End Get
        Set(ByVal value As Boolean)
            _MyReaded = value
        End Set
    End Property

    'Public Property ListLock() As Boolean
    '    Get
    '        Return _MyListLock
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _MyListLock = value
    '    End Set
    'End Property

    Public Property IconSz() As IconSizes
        Get
            Return _MyIconSize
        End Get
        Set(ByVal value As IconSizes)
            _MyIconSize = value
        End Set
    End Property

    Public Property Status() As String
        Get
            Return _MyStatusText
        End Get
        Set(ByVal value As String)
            _MyStatusText = value
        End Set
    End Property

    Public Property UnreadManage() As Boolean
        Get
            Return _MyUnreadManage
        End Get
        Set(ByVal value As Boolean)
            _MyUnreadManage = value
        End Set
    End Property

    Public Property PlaySound() As Boolean
        Get
            Return _MyPlaySound
        End Get
        Set(ByVal value As Boolean)
            _MyPlaySound = value
        End Set
    End Property

    Public Property OneWayLove() As Boolean
        Get
            Return _MyOneWayLove
        End Get
        Set(ByVal value As Boolean)
            _MyOneWayLove = value
        End Set
    End Property

    Public Property FontUnread() As Font
        Get
            Return _fntUnread
        End Get
        Set(ByVal value As Font)
            _fntUnread = value
        End Set
    End Property

    Public Property ColorUnread() As Color
        Get
            Return _clUnread
        End Get
        Set(ByVal value As Color)
            _clUnread = value
        End Set
    End Property

    Public Property FontReaded() As Font
        Get
            Return _fntReaded
        End Get
        Set(ByVal value As Font)
            _fntReaded = value
        End Set
    End Property

    Public Property ColorReaded() As Color
        Get
            Return _clReaded
        End Get
        Set(ByVal value As Color)
            _clReaded = value
        End Set
    End Property

    Public Property ColorFav() As Color
        Get
            Return _clFav
        End Get
        Set(ByVal value As Color)
            _clFav = value
        End Set
    End Property

    Public Property ColorOWL() As Color
        Get
            Return _clOWL
        End Get
        Set(ByVal value As Color)
            _clOWL = value
        End Set
    End Property

    Public Property FontDetail() As Font
        Get
            Return _fntDetail
        End Get
        Set(ByVal value As Font)
            _fntDetail = value
        End Set
    End Property

    Public Property ColorSelf() As Color
        Get
            Return _clSelf
        End Get
        Set(ByVal value As Color)
            _clSelf = value
        End Set
    End Property

    Public Property ColorAtSelf() As Color
        Get
            Return _clAtSelf
        End Get
        Set(ByVal value As Color)
            _clAtSelf = value
        End Set
    End Property

    Public Property ColorTarget() As Color
        Get
            Return _clTarget
        End Get
        Set(ByVal value As Color)
            _clTarget = value
        End Set
    End Property

    Public Property ColorAtTarget() As Color
        Get
            Return _clAtTarget
        End Get
        Set(ByVal value As Color)
            _clAtTarget = value
        End Set
    End Property

    Public Property ColorAtFromTarget() As Color
        Get
            Return _clAtFromTarget
        End Get
        Set(ByVal value As Color)
            _clAtFromTarget = value
        End Set
    End Property

    Public Property NameBalloon() As NameBalloonEnum
        Get
            Return _MyNameBalloon
        End Get
        Set(ByVal value As NameBalloonEnum)
            _MyNameBalloon = value
        End Set
    End Property

    Public Property PostCtrlEnter() As Boolean
        Get
            Return _MyPostCtrlEnter
        End Get
        Set(ByVal value As Boolean)
            _MyPostCtrlEnter = value
        End Set
    End Property

    Public Property UseAPI() As Boolean
        Get
            Return _useAPI
        End Get
        Set(ByVal value As Boolean)
            _useAPI = value
        End Set
    End Property

    Public Property CheckReply() As Boolean
        Get
            Return _MyCheckReply
        End Get
        Set(ByVal value As Boolean)
            _MyCheckReply = value
        End Set
    End Property

    Public Property UseRecommendStatus() As Boolean
        Get
            Return _MyUseRecommendStatus
        End Get
        Set(ByVal value As Boolean)
            _MyUseRecommendStatus = value
        End Set
    End Property

    Public Property DispUsername() As Boolean
        Get
            Return _MyDispUsername
        End Get
        Set(ByVal value As Boolean)
            _MyDispUsername = value
        End Set
    End Property

    Public Property CloseToExit() As Boolean
        Get
            Return _MyCloseToExit
        End Get
        Set(ByVal value As Boolean)
            _MyCloseToExit = value
        End Set
    End Property

    Public Property MinimizeToTray() As Boolean
        Get
            Return _MyMinimizeToTray
        End Get
        Set(ByVal value As Boolean)
            _MyMinimizeToTray = value
        End Set
    End Property
    Public Property DispLatestPost() As DispTitleEnum
        Get
            Return _MyDispLatestPost
        End Get
        Set(ByVal value As DispTitleEnum)
            _MyDispLatestPost = value
        End Set
    End Property

    Public Property HubServer() As String
        Get
            Return _hubServer
        End Get
        Set(ByVal value As String)
            _hubServer = value
        End Set
    End Property

    Private Sub HubServerDomain_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles HubServerDomain.Validating
        If HubServerDomain.Text.Trim = "" Then
            MessageBox.Show("空欄にはできません。デフォルト値「twitter.com」が設定されます。", "ドメイン指定", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            HubServerDomain.Text = "twitter.com"
        End If
    End Sub

    Public Property BrowserPath() As String
        Get
            Return _browserpath
        End Get
        Set(ByVal value As String)
            _browserpath = value
        End Set
    End Property

    Private Sub CheckUseRecommendStatus_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckUseRecommendStatus.CheckedChanged
        If CheckUseRecommendStatus.Checked = True Then
            StatusText.Enabled = False
        Else
            StatusText.Enabled = True
        End If
    End Sub

    Public Property SortOrderLock() As Boolean
        Get
            Return _MySortOrderLock
        End Get
        Set(ByVal value As Boolean)
            _MySortOrderLock = value
        End Set
    End Property

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim filedlg As New OpenFileDialog()

        filedlg.Filter = "実行形式ファイル(*.exe)|*.exe|すべてのファイル(*.*)|*.*"
        filedlg.FilterIndex = 1
        filedlg.Title = "ブラウザを指定してください"
        filedlg.RestoreDirectory = True

        If filedlg.ShowDialog() = Windows.Forms.DialogResult.OK Then
            BrowserPathText.Text = filedlg.FileName

        End If
    End Sub
End Class
