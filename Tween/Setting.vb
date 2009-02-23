' Tween - Client of Twitter
' Copyright © 2007-2009 kiri_feather (@kiri_feather) <kiri_feather@gmail.com>
'           © 2008-2009 Moz (@syo68k) <http://iddy.jp/profile/moz/>
'           © 2008-2009 takeshik (@takeshik) <http://www.takeshik.org/>
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

Imports System.Xml
Imports System.Xml.Serialization
Imports System.Xml.Schema
Imports System.IO
Imports System.Runtime.InteropServices

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
    Private _fntUnread As Font  '未使用（バージョン互換性のため残してある）
    Private _clUnread As Color
    Private _fntReaded As Font  'リストフォントとして扱う
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
    Private _MyTinyUrlResolve As Boolean
    Private _MyProxyType As ProxyTypeEnum
    Private _MyProxyAddress As String
    Private _MyProxyPort As Integer
    Private _MyProxyUser As String
    Private _MyProxyPassword As String
    Private _MyMaxPostNum As Integer
    Private _MyPeriodAdjust As Boolean
    Private _MyStartupVersion As Boolean
    Private _MyStartupKey As Boolean
    Private _MyStartupFollowers As Boolean
    Private _MyRestrictFavCheck As Boolean
    Private _MyAlwaysTop As Boolean
    Private _MyUrlConvertAuto As Boolean
    Private _MyOutputz As Boolean
    Private _MyOutputzKey As String
    Private _MyOutputzUrlmode As OutputzUrlmode
    Private _MyUnreadStyle As Boolean
    Private _MyDateTimeFormat As String
    Private _MyDefaultTimeOut As Integer
    Private _MyProtectNotInclude As Boolean

    ''''Private _MyWidth8 As Integer
    ''''Private _MyWidth9 As Integer
    ''''Private _MyDisplayIndex1 As Integer
    ''''Private _MyDisplayIndex2 As Integer
    ''''Private _MyDisplayIndex3 As Integer
    ''''Private _MyDisplayIndex4 As Integer
    ''''Private _MyDisplayIndex5 As Integer
    ''''Private _MyDisplayIndex6 As Integer
    ''''Private _MyDisplayIndex7 As Integer
    ''''Private _MyDisplayIndex8 As Integer
    ''''Private _MyDisplayIndex9 As Integer
    ''''Private _MySortOrder As SortOrder
    ''''Private _MyStatusMultiline As Boolean
    ''''Private _MyStatusTextHeight As Integer
    ''''Private _MycultureCode As String

    '''''タブのリスト
    '''''フィルターのリスト

    Private Sub Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Save.Click
        If Username.Text.Trim = "" Or _
           Password.Text.Trim = "" Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            MessageBox.Show(My.Resources.Save_ClickText1)
            Exit Sub
        End If
        If Username.Text.Contains("@") Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            MessageBox.Show(My.Resources.Save_ClickText2)
            Exit Sub
        End If
        Try
            _MyuserID = Username.Text.Trim()
            _Mypassword = Password.Text.Trim()
            _MytimelinePeriod = CType(TimelinePeriod.Text, Integer)
            _MyDMPeriod = CType(DMPeriod.Text, Integer)
            _MynextThreshold = CType(NextThreshold.Text, Integer)
            _MyNextPages = CType(NextPages.Text, Integer)
            _MyMaxPostNum = 125

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
            TweenMain.PlaySoundMenuItem.Checked = _MyPlaySound  'これは勘弁
            _MyUnreadManage = UReadMng.Checked
            _MyOneWayLove = OneWayLv.Checked

            _fntUnread = lblUnread.Font     '未使用
            _clUnread = lblUnread.ForeColor
            _fntReaded = lblListFont.Font     'リストフォントとして使用
            _clReaded = lblListFont.ForeColor
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
            _useAPI = True
            _hubServer = "twitter.com"
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
            _MyTinyUrlResolve = CheckTinyURL.Checked
            If RadioProxyNone.Checked Then
                _MyProxyType = ProxyTypeEnum.None
            ElseIf RadioProxyIE.Checked Then
                _MyProxyType = ProxyTypeEnum.IE
            Else
                _MyProxyType = ProxyTypeEnum.Specified
            End If
            _MyProxyAddress = TextProxyAddress.Text.Trim()
            _MyProxyPort = Integer.Parse(TextProxyPort.Text.Trim())
            _MyProxyUser = TextProxyUser.Text.Trim()
            _MyProxyPassword = TextProxyPassword.Text.Trim()
            _MyPeriodAdjust = CheckPeriodAdjust.Checked
            _MyStartupVersion = CheckStartupVersion.Checked
            _MyStartupKey = CheckStartupKey.Checked
            _MyStartupFollowers = CheckStartupFollowers.Checked
            _MyRestrictFavCheck = CheckFavRestrict.Checked
            _MyAlwaysTop = CheckAlwaysTop.Checked
            _MyUrlConvertAuto = CheckAutoConvertUrl.Checked
            _MyOutputz = CheckOutputz.Checked
            Outputz.Enabled = _MyOutputz
            _MyOutputzKey = TextBoxOutputzKey.Text.Trim()
            Outputz.key = _MyOutputzKey

            Select Case ComboBoxOutputzUrlmode.SelectedIndex
                Case 0
                    _MyOutputzUrlmode = OutputzUrlmode.twittercom
                    Outputz.url = "http://twitter.com/"
                Case 1
                    _MyOutputzUrlmode = OutputzUrlmode.twittercomWithUsername
                    Outputz.url = "http://twitter.com/" + _MyuserID
            End Select

            _MyUnreadStyle = chkUnreadStyle.Checked
            _MyDateTimeFormat = CmbDateTimeFormat.Text
            _MyDefaultTimeOut = CType(ConnectionTimeOut.Text, Integer)      ' 0の場合はGetWebResponse()側でTimeOut.Infiniteへ読み替える
            _MyProtectNotInclude = CheckProtectNotInclude.Checked

        Catch ex As Exception
            MessageBox.Show(My.Resources.Save_ClickText3)
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
        'MaxPost.Text = _MyMaxPostNum.ToString()
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
        TweenMain.PlaySoundMenuItem.Checked = _MyPlaySound
        OneWayLv.Checked = _MyOneWayLove

        lblListFont.Font = _fntReaded
        lblUnread.Font = _fntUnread
        lblUnread.ForeColor = _clUnread
        'lblReaded.Font = _fntReaded
        'lblReaded.ForeColor = _clReaded
        lblListFont.ForeColor = _clReaded
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
        'CheckUseAPI.Checked = _useAPI
        'HubServerDomain.Text = _hubServer
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
        CheckTinyURL.Checked = _MyTinyUrlResolve
        Select Case _MyProxyType
            Case ProxyTypeEnum.None
                RadioProxyNone.Checked = True
            Case ProxyTypeEnum.IE
                RadioProxyIE.Checked = True
            Case Else
                RadioProxySpecified.Checked = True
        End Select
        Dim chk As Boolean = RadioProxySpecified.Checked
        LabelProxyAddress.Enabled = chk
        TextProxyAddress.Enabled = chk
        LabelProxyPort.Enabled = chk
        TextProxyPort.Enabled = chk
        LabelProxyUser.Enabled = chk
        TextProxyUser.Enabled = chk
        LabelProxyPassword.Enabled = chk
        TextProxyPassword.Enabled = chk

        TextProxyAddress.Text = _MyProxyAddress
        TextProxyPort.Text = _MyProxyPort.ToString
        TextProxyUser.Text = _MyProxyUser
        TextProxyPassword.Text = _MyProxyPassword

        CheckPeriodAdjust.Checked = _MyPeriodAdjust
        CheckStartupVersion.Checked = _MyStartupVersion
        CheckStartupKey.Checked = _MyStartupKey
        CheckStartupFollowers.Checked = _MyStartupFollowers
        CheckFavRestrict.Checked = _MyRestrictFavCheck
        CheckAlwaysTop.Checked = _MyAlwaysTop
        CheckAutoConvertUrl.Checked = _MyUrlConvertAuto
        CheckOutputz.Checked = _MyOutputz
        Outputz.Enabled = _MyOutputz
        TextBoxOutputzKey.Text = _MyOutputzKey
        Outputz.key = _MyOutputzKey

        Select Case _MyOutputzUrlmode
            Case OutputzUrlmode.twittercom
                ComboBoxOutputzUrlmode.SelectedIndex = 0
            Case OutputzUrlmode.twittercomWithUsername
                ComboBoxOutputzUrlmode.SelectedIndex = 1
        End Select

        chkUnreadStyle.Checked = _MyUnreadStyle
        CmbDateTimeFormat.Text = _MyDateTimeFormat
        ConnectionTimeOut.Text = _MyDefaultTimeOut.ToString
        CheckProtectNotInclude.Checked = _MyProtectNotInclude

        'TweenMain.SetMainWindowTitle()
        'TweenMain.SetNotifyIconText()

        TabControl1.SelectedIndex = 0
        ActiveControl = Username

        CheckOutputz_CheckedChanged(sender, e)
    End Sub

    Private Sub TimelinePeriod_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TimelinePeriod.Validating
        Dim prd As Integer
        Try
            prd = CType(TimelinePeriod.Text, Integer)
        Catch ex As Exception
            MessageBox.Show(My.Resources.TimelinePeriod_ValidatingText1)
            e.Cancel = True
            Exit Sub
        End Try

        If prd <> 0 And (prd < 15 Or prd > 600) Then
            MessageBox.Show(My.Resources.TimelinePeriod_ValidatingText2)
            e.Cancel = True
        End If
    End Sub

    Private Sub NextThreshold_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles NextThreshold.Validating
        Dim thr As Integer
        Try
            thr = CType(NextThreshold.Text, Integer)
        Catch ex As Exception
            MessageBox.Show(My.Resources.NextThreshold_ValidatingText1)
            e.Cancel = True
            Exit Sub
        End Try

        If thr < 1 Or thr > 20 Then
            MessageBox.Show(My.Resources.NextThreshold_ValidatingText2)
            e.Cancel = True
        End If
    End Sub

    Private Sub NextPages_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles NextPages.Validating
        Dim thr As Integer
        Try
            thr = CType(NextPages.Text, Integer)
        Catch ex As Exception
            MessageBox.Show(My.Resources.NextPages_ValidatingText1)
            e.Cancel = True
            Exit Sub
        End Try

        If thr < 1 Or thr > 20 Then
            MessageBox.Show(My.Resources.NextPages_ValidatingText2)
            e.Cancel = True
        End If
    End Sub

    Private Sub DMPeriod_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles DMPeriod.Validating
        Dim prd As Integer
        Try
            prd = CType(DMPeriod.Text, Integer)
        Catch ex As Exception
            MessageBox.Show(My.Resources.DMPeriod_ValidatingText1)
            e.Cancel = True
            Exit Sub
        End Try

        If prd <> 0 And (prd < 15 Or prd > 600) Then
            MessageBox.Show(My.Resources.DMPeriod_ValidatingText2)
            e.Cancel = True
        End If
    End Sub

    Private Sub ReadLogDays_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ReadLogDays.Validating
        'Dim days As Integer
        'Try
        '    days = CType(ReadLogDays.Text, Integer)
        'Catch ex As Exception
        '    MessageBox.Show("読み込み日数には数値（0～7）を指定してください。")
        '    e.Cancel = True
        '    Exit Sub
        'End Try

        'If days < 0 Or days > 7 Then
        '    MessageBox.Show("読み込み日数には数値（0～7）を指定してください。")
        '    e.Cancel = True
        'End If
    End Sub

    Private Sub StartupReadPages_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles StartupReadPages.Validating
        Dim pages As Integer
        Try
            pages = CType(StartupReadPages.Text, Integer)
        Catch ex As Exception
            MessageBox.Show(My.Resources.StartupReadPages_ValidatingText1)
            e.Cancel = True
            Exit Sub
        End Try

        If pages < 1 Or pages > 999 Then
            MessageBox.Show(My.Resources.StartupReadPages_ValidatingText2)
            e.Cancel = True
        End If
    End Sub

    Private Sub StartupReadReply_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles StartupReadReply.Validating
        Dim pages As Integer
        Try
            pages = CType(StartupReadReply.Text, Integer)
        Catch ex As Exception
            MessageBox.Show(My.Resources.StartupReadReply_ValidatingText1)
            e.Cancel = True
            Exit Sub
        End Try

        If pages < 0 Or pages > 999 Then
            MessageBox.Show(My.Resources.StartupReadReply_ValidatingText2)
            e.Cancel = True
        End If
    End Sub

    Private Sub StartupReadDM_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles StartupReadDM.Validating
        Dim pages As Integer
        Try
            pages = CType(StartupReadDM.Text, Integer)
        Catch ex As Exception
            MessageBox.Show(My.Resources.StartupReadDM_ValidatingText1)
            e.Cancel = True
            Exit Sub
        End Try

        If pages < 1 Or pages > 999 Then
            MessageBox.Show(My.Resources.StartupReadDM_ValidatingText2)
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

    Private Sub btnFontAndColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDetail.Click, btnListFont.Click, btnUnread.Click
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
                FontDialog1.Color = lblUnread.ForeColor
                FontDialog1.Font = lblUnread.Font
                'Case "btnReaded"
                '    FontDialog1.Color = lblReaded.ForeColor
                '    FontDialog1.Font = lblReaded.Font
            Case "btnDetail"
                FontDialog1.Font = lblDetail.Font
                FontDialog1.ShowEffects = False
                FontDialog1.ShowColor = False
            Case "btnListFont"
                FontDialog1.Color = lblListFont.ForeColor
                FontDialog1.Font = lblListFont.Font
        End Select

        rtn = FontDialog1.ShowDialog

        If rtn = Windows.Forms.DialogResult.Cancel Then Exit Sub

        Select Case Btn.Name
            Case "btnUnread"
                lblUnread.ForeColor = FontDialog1.Color
                lblUnread.Font = FontDialog1.Font
                'Case "btnReaded"
                '    lblReaded.ForeColor = FontDialog1.Color
                '    lblReaded.Font = FontDialog1.Font
            Case "btnDetail"
                lblDetail.Font = FontDialog1.Font
            Case "btnListFont"
                lblListFont.ForeColor = FontDialog1.Color
                lblListFont.Font = FontDialog1.Font
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
                'Case "btnUnread"
                '    'ColorDialog1.Color = lblUnRead.ForeColor
                'Case "btnReaded"
                '    'ColorDialog1.Color = lblReaded.ForeColor
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
                'Case "btnUnread"
                '    'lblUnRead.ForeColor = ColorDialog1.Color
                'Case "btnReaded"
                '    'lblReaded.ForeColor = ColorDialog1.Color
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

    Public Property MaxPostNum() As Integer
        Get
            Return _MyMaxPostNum
        End Get
        Set(ByVal value As Integer)
            _MyMaxPostNum = value
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

    '''''未使用
    Public Property FontUnread() As Font
        Get
            Return _fntUnread
        End Get
        Set(ByVal value As Font)
            _fntUnread = value
            '無視
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

    '''''リストフォントとして使用
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

    Public Property BrowserPath() As String
        Get
            Return _browserpath
        End Get
        Set(ByVal value As String)
            _browserpath = value
        End Set
    End Property

    Public Property TinyUrlResolve() As Boolean
        Get
            Return _MyTinyUrlResolve
        End Get
        Set(ByVal value As Boolean)
            _MyTinyUrlResolve = value
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

    Public Property ProxyType() As ProxyTypeEnum
        Get
            Return _MyProxyType
        End Get
        Set(ByVal value As ProxyTypeEnum)
            _MyProxyType = value
        End Set
    End Property

    Public Property ProxyAddress() As String
        Get
            Return _MyProxyAddress
        End Get
        Set(ByVal value As String)
            _MyProxyAddress = value
        End Set
    End Property

    Public Property ProxyPort() As Integer
        Get
            Return _MyProxyPort
        End Get
        Set(ByVal value As Integer)
            _MyProxyPort = value
        End Set
    End Property

    Public Property ProxyUser() As String
        Get
            Return _MyProxyUser
        End Get
        Set(ByVal value As String)
            _MyProxyUser = value
        End Set
    End Property

    Public Property ProxyPassword() As String
        Get
            Return _MyProxyPassword
        End Get
        Set(ByVal value As String)
            _MyProxyPassword = value
        End Set
    End Property

    Public Property PeriodAdjust() As Boolean
        Get
            Return _MyPeriodAdjust
        End Get
        Set(ByVal value As Boolean)
            _MyPeriodAdjust = value
        End Set
    End Property

    Public Property StartupVersion() As Boolean
        Get
            Return _MyStartupVersion
        End Get
        Set(ByVal value As Boolean)
            _MyStartupVersion = value
        End Set
    End Property

    Public Property StartupKey() As Boolean
        Get
            Return _MyStartupKey
        End Get
        Set(ByVal value As Boolean)
            _MyStartupKey = value
        End Set
    End Property

    Public Property StartupFollowers() As Boolean
        Get
            Return _MyStartupFollowers
        End Get
        Set(ByVal value As Boolean)
            _MyStartupFollowers = value
        End Set
    End Property

    Public Property RestrictFavCheck() As Boolean
        Get
            Return _MyRestrictFavCheck
        End Get
        Set(ByVal value As Boolean)
            _MyRestrictFavCheck = value
        End Set
    End Property

    Public Property AlwaysTop() As Boolean
        Get
            Return _MyAlwaysTop
        End Get
        Set(ByVal value As Boolean)
            _MyAlwaysTop = value
        End Set
    End Property

    Public Property UrlConvertAuto() As Boolean
        Get
            Return _MyUrlConvertAuto
        End Get
        Set(ByVal value As Boolean)
            _MyUrlConvertAuto = value
        End Set
    End Property
    Public Property OutputzEnabled() As Boolean
        Get
            Return _MyOutputz
        End Get
        Set(ByVal value As Boolean)
            _MyOutputz = value
        End Set
    End Property
    Public Property OutputzKey() As String
        Get
            Return _MyOutputzKey
        End Get
        Set(ByVal value As String)
            _MyOutputzKey = value
        End Set
    End Property
    Public Property OutputzUrlmode() As OutputzUrlmode
        Get
            Return _MyOutputzUrlmode
        End Get
        Set(ByVal value As OutputzUrlmode)
            _MyOutputzUrlmode = value
        End Set
    End Property

    Public Property UseUnreadStyle() As Boolean
        Get
            Return _MyUnreadStyle
        End Get
        Set(ByVal value As Boolean)
            _MyUnreadStyle = value
        End Set
    End Property

    Public Property DateTimeFormat() As String
        Get
            Return _MyDateTimeFormat
        End Get
        Set(ByVal value As String)
            _MyDateTimeFormat = value
        End Set
    End Property

    Public Property DefaultTimeOut() As Integer
        Get
            Return _MyDefaultTimeOut
        End Get
        Set(ByVal value As Integer)
            _MyDefaultTimeOut = value
        End Set
    End Property

    Public Property ProtectNotInclude() As Boolean
        Get
            Return _MyProtectNotInclude
        End Get
        Set(ByVal value As Boolean)
            _MyProtectNotInclude = value
        End Set
    End Property

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim filedlg As New OpenFileDialog()

        filedlg.Filter = My.Resources.Button3_ClickText1
        filedlg.FilterIndex = 1
        filedlg.Title = My.Resources.Button3_ClickText2
        filedlg.RestoreDirectory = True

        If filedlg.ShowDialog() = Windows.Forms.DialogResult.OK Then
            BrowserPathText.Text = filedlg.FileName

        End If
    End Sub

    Private Sub RadioProxySpecified_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioProxySpecified.CheckedChanged
        Dim chk As Boolean = RadioProxySpecified.Checked
        LabelProxyAddress.Enabled = chk
        TextProxyAddress.Enabled = chk
        LabelProxyPort.Enabled = chk
        TextProxyPort.Enabled = chk
        LabelProxyUser.Enabled = chk
        TextProxyUser.Enabled = chk
        LabelProxyPassword.Enabled = chk
        TextProxyPassword.Enabled = chk
    End Sub

    Private Sub TextProxyPort_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextProxyPort.Validating
        Dim port As Integer
        If TextProxyPort.Text.Trim() = "" Then TextProxyPort.Text = "0"
        If Integer.TryParse(TextProxyPort.Text.Trim(), port) = False Then
            MessageBox.Show(My.Resources.TextProxyPort_ValidatingText1)
            e.Cancel = True
            Exit Sub
        End If
        If port < 0 Or port > 65535 Then
            MessageBox.Show(My.Resources.TextProxyPort_ValidatingText2)
            e.Cancel = True
            Exit Sub
        End If
    End Sub

    Private Sub CheckOutputz_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckOutputz.CheckedChanged
        If CheckOutputz.Checked = True Then
            Label59.Enabled = True
            Label60.Enabled = True
            TextBoxOutputzKey.Enabled = True
            ComboBoxOutputzUrlmode.Enabled = True
        Else
            Label59.Enabled = False
            Label60.Enabled = False
            TextBoxOutputzKey.Enabled = False
            ComboBoxOutputzUrlmode.Enabled = False
        End If
    End Sub

    Private Sub TextBoxOutputzKey_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextBoxOutputzKey.Validating
        If CheckOutputz.Checked Then
            TextBoxOutputzKey.Text = Trim(TextBoxOutputzKey.Text)
            If TextBoxOutputzKey.Text.Length = 0 Then
                MessageBox.Show(My.Resources.TextBoxOutputzKey_Validating)
                e.Cancel = True
                Exit Sub
            End If
        End If
    End Sub

    ''''    Private Function EncryptString(ByVal str As String) As String
    ''''        '文字列をバイト型配列にする
    ''''        Dim bytesIn As Byte() = System.Text.Encoding.UTF8.GetBytes(str)
    ''''        'DESCryptoServiceProviderオブジェクトの作成
    ''''        Dim des As New System.Security.Cryptography.DESCryptoServiceProvider

    ''''        '共有キーと初期化ベクタを決定
    ''''        'パスワードをバイト配列にする
    ''''        Dim bytesKey As Byte() = System.Text.Encoding.UTF8.GetBytes("_tween_encrypt_key_")
    ''''        '共有キーと初期化ベクタを設定
    ''''        des.Key = ResizeBytesArray(bytesKey, des.Key.Length)
    ''''        des.IV = ResizeBytesArray(bytesKey, des.IV.Length)

    ''''        '暗号化されたデータを書き出すためのMemoryStream
    ''''        Using msOut As New System.IO.MemoryStream
    ''''            'DES暗号化オブジェクトの作成
    ''''            Using desdecrypt As System.Security.Cryptography.ICryptoTransform = _
    ''''                des.CreateEncryptor()

    ''''                '書き込むためのCryptoStreamの作成
    ''''                Using cryptStream As New System.Security.Cryptography.CryptoStream( _
    ''''                    msOut, desdecrypt, _
    ''''                    System.Security.Cryptography.CryptoStreamMode.Write)
    ''''                    '書き込む
    ''''                    cryptStream.Write(bytesIn, 0, bytesIn.Length)
    ''''                    cryptStream.FlushFinalBlock()
    ''''                    '暗号化されたデータを取得
    ''''                    Dim bytesOut As Byte() = msOut.ToArray()

    ''''                    '閉じる
    ''''                    cryptStream.Close()
    ''''                    msOut.Close()

    ''''                    'Base64で文字列に変更して結果を返す
    ''''                    Return System.Convert.ToBase64String(bytesOut)
    ''''                End Using
    ''''            End Using
    ''''        End Using
    ''''    End Function

    ''''    Private Function DecryptString(ByVal str As String) As String
    ''''        'DESCryptoServiceProviderオブジェクトの作成
    ''''        Dim des As New System.Security.Cryptography.DESCryptoServiceProvider

    ''''        '共有キーと初期化ベクタを決定
    ''''        'パスワードをバイト配列にする
    ''''        Dim bytesKey As Byte() = System.Text.Encoding.UTF8.GetBytes("_tween_encrypt_key_")
    ''''        '共有キーと初期化ベクタを設定
    ''''        des.Key = ResizeBytesArray(bytesKey, des.Key.Length)
    ''''        des.IV = ResizeBytesArray(bytesKey, des.IV.Length)

    ''''        'Base64で文字列をバイト配列に戻す
    ''''        Dim bytesIn As Byte() = System.Convert.FromBase64String(str)
    ''''        '暗号化されたデータを読み込むためのMemoryStream
    ''''        Using msIn As New System.IO.MemoryStream(bytesIn)
    ''''            'DES復号化オブジェクトの作成
    ''''            Using desdecrypt As System.Security.Cryptography.ICryptoTransform = _
    ''''                des.CreateDecryptor()
    ''''                '読み込むためのCryptoStreamの作成
    ''''                Using cryptStreem As New System.Security.Cryptography.CryptoStream( _
    ''''                    msIn, desdecrypt, _
    ''''                    System.Security.Cryptography.CryptoStreamMode.Read)

    ''''                    '復号化されたデータを取得するためのStreamReader
    ''''                    Using srOut As New System.IO.StreamReader( _
    ''''                        cryptStreem, System.Text.Encoding.UTF8)
    ''''                        '復号化されたデータを取得する
    ''''                        Dim result As String = srOut.ReadToEnd()

    ''''                        '閉じる
    ''''                        srOut.Close()
    ''''                        cryptStreem.Close()
    ''''                        msIn.Close()

    ''''                        Return result
    ''''                    End Using
    ''''                End Using
    ''''            End Using
    ''''        End Using
    ''''    End Function

    ''''    Private Function ResizeBytesArray(ByVal bytes() As Byte, _
    ''''                                ByVal newSize As Integer) As Byte()
    ''''        Dim newBytes(newSize - 1) As Byte
    ''''        If bytes.Length <= newSize Then
    ''''            Dim i As Integer
    ''''            For i = 0 To bytes.Length - 1
    ''''                newBytes(i) = bytes(i)
    ''''            Next i
    ''''        Else
    ''''            Dim pos As Integer = 0
    ''''            Dim i As Integer
    ''''            For i = 0 To bytes.Length - 1
    ''''                newBytes(pos) = newBytes(pos) Xor bytes(i)
    ''''                pos += 1
    ''''                If pos >= newBytes.Length Then
    ''''                    pos = 0
    ''''                End If
    ''''            Next i
    ''''        End If
    ''''        Return newBytes
    ''''    End Function

    Private Function CreateDateTimeFormatSample() As Boolean
        Try
            LabelDateTimeFormatApplied.Text = DateTime.Now.ToString(CmbDateTimeFormat.Text)
        Catch ex As FormatException
            LabelDateTimeFormatApplied.Text = My.Resources.CreateDateTimeFormatSampleText1
            Return False
        End Try
        Return True
    End Function

    Private Sub CmbDateTimeFormat_TextUpdate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbDateTimeFormat.TextUpdate
        CreateDateTimeFormatSample()
    End Sub

    Private Sub CmbDateTimeFormat_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbDateTimeFormat.SelectedIndexChanged
        CreateDateTimeFormatSample()
    End Sub

    Private Sub CmbDateTimeFormat_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles CmbDateTimeFormat.Validating
        If Not CreateDateTimeFormatSample() Then
            MessageBox.Show(My.Resources.CmbDateTimeFormat_Validating)
            e.Cancel = True
        End If
    End Sub

    Private Sub ConnectionTimeOut_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ConnectionTimeOut.Validating
        Dim tm As Integer
        Try
            tm = CInt(ConnectionTimeOut.Text)
        Catch ex As Exception
            MessageBox.Show(My.Resources.ConnectionTimeOut_ValidatingText1)
            e.Cancel = True
            Exit Sub
        End Try

        If tm < HttpTimeOut.MinValue OrElse tm > HttpTimeOut.MaxValue Then
            MessageBox.Show(My.Resources.ConnectionTimeOut_ValidatingText1)
            e.Cancel = True
        End If
    End Sub

    Private Sub LabelDateTimeFormatApplied_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles LabelDateTimeFormatApplied.VisibleChanged
        CreateDateTimeFormatSample()
    End Sub
End Class

<XmlRoot(ElementName:="configuration", Namespace:="urn:XSpect.Configuration.XmlConfiguration")> _
Public NotInheritable Class XmlConfiguration
    Implements IDictionary(Of String, Object),  _
               IXmlSerializable

    Private ReadOnly _dictionary As Dictionary(Of String, KeyValuePair(Of Type, Object)) = New Dictionary(Of String, KeyValuePair(Of Type, Object))

    Private _filePath As String

    Private ReadOnly Property _dictionaryCollection() As ICollection(Of KeyValuePair(Of String, KeyValuePair(Of Type, Object)))
        Get
            Return Me._dictionary
        End Get
    End Property

#Region "IDictionary<string,object> メンバ"

    Public Sub Add(ByVal key As String, ByVal value As Object) _
        Implements IDictionary(Of String, Object).Add

        Me._dictionary.Add(key, Me.GetInternalValue(value))
    End Sub

    Public Function ContainsKey(ByVal key As String) As Boolean _
        Implements IDictionary(Of String, Object).ContainsKey

        Return Me._dictionary.ContainsKey(key)
    End Function

    Public ReadOnly Property Keys() As ICollection(Of String) _
        Implements IDictionary(Of String, Object).Keys

        Get
            Return Me._dictionary.Keys
        End Get
    End Property

    Public Function Remove(ByVal key As String) As Boolean _
        Implements IDictionary(Of String, Object).Remove

        Return Me._dictionary.Remove(key)
    End Function

    Public Function TryGetValue(ByVal key As String, <Out()> ByRef value As Object) As Boolean _
        Implements IDictionary(Of String, Object).TryGetValue

        Return Me.TryGetValue(Of Object)(key, value)
    End Function

    Public ReadOnly Property Values() As ICollection(Of Object) _
        Implements IDictionary(Of String, Object).Values

        Get
            Dim list As List(Of Object) = New List(Of Object)(Me._dictionary.Values.Count)

            For Each p As KeyValuePair(Of Type, Object) In Me._dictionary.Values
                list.Add(p.Value)
            Next
            Return list
        End Get
    End Property

    Default Public Property Item(ByVal key As String) As Object _
        Implements IDictionary(Of String, Object).Item

        Get
            Return Me.GetValue(key)
        End Get
        Set(ByVal value As Object)
            Me.SetValue(key, value)
        End Set
    End Property

#End Region

#Region "ICollection<KeyValuePair<string,object>> メンバ"

    Public Sub Add(ByVal item As KeyValuePair(Of String, Object)) _
        Implements ICollection(Of KeyValuePair(Of String, Object)).Add

        Me._dictionaryCollection.Add(Me.GetInternalValue(item))
    End Sub

    Public Sub Clear() _
        Implements ICollection(Of KeyValuePair(Of String, Object)).Clear

        Me._dictionary.Clear()
    End Sub

    Public Function Contains(ByVal item As KeyValuePair(Of String, Object)) As Boolean _
        Implements ICollection(Of KeyValuePair(Of String, Object)).Contains

        Return Me._dictionaryCollection.Contains(Me.GetInternalValue(item))
    End Function

    Public Sub CopyTo(ByVal array As KeyValuePair(Of String, Object)(), ByVal arrayIndex As Integer) _
        Implements ICollection(Of KeyValuePair(Of String, Object)).CopyTo

        Dim list As List(Of KeyValuePair(Of String, KeyValuePair(Of Type, Object))) _
            = New List(Of KeyValuePair(Of String, KeyValuePair(Of Type, Object)))(array.Length)

        For Each p As KeyValuePair(Of String, Object) In array
            list.Add(Me.GetInternalValue(p))
        Next
        Me._dictionaryCollection.CopyTo(list.ToArray(), arrayIndex)
    End Sub

    Public ReadOnly Property Count() As Integer _
        Implements ICollection(Of KeyValuePair(Of String, Object)).Count
        Get
            Return Me._dictionary.Count
        End Get
    End Property

    Public ReadOnly Property IsReadOnly() As Boolean _
        Implements ICollection(Of KeyValuePair(Of String, Object)).IsReadOnly

        Get
            Return Me._dictionaryCollection.IsReadOnly
        End Get
    End Property

    Public Function Remove(ByVal item As KeyValuePair(Of String, Object)) As Boolean _
        Implements ICollection(Of KeyValuePair(Of String, Object)).Remove

        Return Me._dictionaryCollection.Remove(Me.GetInternalValue(item))
    End Function

#End Region

#Region "IEnumerable<KeyValuePair<string,object>> メンバ"

    Public Function GetUntypedEnumerator() As IEnumerator(Of KeyValuePair(Of String, Object)) _
        Implements IEnumerable(Of KeyValuePair(Of String, Object)).GetEnumerator

        Dim list As List(Of KeyValuePair(Of String, Object)) = New List(Of KeyValuePair(Of String, Object))(Me.Count)
        For Each p As KeyValuePair(Of String, KeyValuePair(Of Type, Object)) In Me._dictionary
            list.Add(New KeyValuePair(Of String, Object)(p.Key, p.Value.Value))
        Next
        Return list.GetEnumerator()
    End Function

#End Region

#Region "IEnumerable メンバ"

    Private Function GetEnumerator() As IEnumerator _
        Implements IEnumerable.GetEnumerator

        Return Me.GetEnumerator()
    End Function

#End Region

#Region "IXmlSerializable メンバ"

    Public Function GetSchema() As XmlSchema _
        Implements IXmlSerializable.GetSchema

        Return Nothing
    End Function

    Public Sub ReadXml(ByVal reader As XmlReader) _
        Implements IXmlSerializable.ReadXml

        Dim xdoc As XmlDocument = New XmlDocument()

        xdoc.Load(reader)
        For Each xentryNode As XmlNode In xdoc.SelectNodes("//entry")
            Dim xentry As XmlElement = CType(xentryNode, XmlElement)
            Me.Add( _
                xentry.Attributes.ItemOf("key").Value, _
                New XmlSerializer(Type.GetType(xentry.Attributes.ItemOf("type").Value)) _
                    .Deserialize(New XmlNodeReader(xentry.GetElementsByTagName("*").Item(0))) _
            )
        Next
    End Sub

    Public Sub WriteXml(ByVal writer As XmlWriter) _
        Implements IXmlSerializable.WriteXml

        Dim xdoc As XmlDocument = New XmlDocument()
        For Each entry As KeyValuePair(Of String, Object) In Me
            Dim xentry As XmlElement = xdoc.CreateElement("entry")
            xentry.SetAttributeNode("key", entry.Key)
            Using stream As MemoryStream = New MemoryStream()
                Dim serializer As XmlSerializer = New XmlSerializer(entry.Value.GetType())
                serializer.Serialize(stream, entry.Value)
                stream.Seek(0, SeekOrigin.Begin)
                Dim xserialized As XmlDocument = New XmlDocument()
                xserialized.Load(stream)
                xentry.AppendChild(xserialized.DocumentElement)
            End Using
        Next
    End Sub

#End Region

    Public Sub New()
        Me._dictionary = New Dictionary(Of String, KeyValuePair(Of Type, Object))()
    End Sub

    Public Shared Function Load(ByVal path As String) As XmlConfiguration
        Dim config As XmlConfiguration = DirectCast(New XmlSerializer(GetType(XmlConfiguration)).Deserialize(XmlReader.Create(path)), XmlConfiguration)
        config._filePath = path
        Return config
    End Function

    Public Sub Save(ByVal path As String)
        Using stream As MemoryStream = New MemoryStream()
            Dim serializer As XmlSerializer = New XmlSerializer(GetType(XmlConfiguration))
            serializer.Serialize(XmlWriter.Create(stream), Me)
            stream.Seek(0, SeekOrigin.Begin)
            Dim xdoc As XmlDocument = New XmlDocument()
            xdoc.Load(stream)
            xdoc.Save(path)
        End Using

        Me._filePath = path
    End Sub

    Public Sub Save()
        Me.Save(Me._filePath)
    End Sub

    Private Function GetInternalValue(ByVal value As Object) As KeyValuePair(Of Type, Object)
        Return New KeyValuePair(Of Type, Object)(value.GetType, value)
    End Function

    Private Function GetInternalValue(ByVal item As KeyValuePair(Of String, Object)) As KeyValuePair(Of String, KeyValuePair(Of Type, Object))
        Return New KeyValuePair(Of String, KeyValuePair(Of Type, Object))(item.Key, Me.GetInternalValue(item.Value))
    End Function

    Public Function GetValue(Of T)(ByVal key As String) As T
        Return DirectCast(Me._dictionary.Item(key).Value, T)
    End Function

    Public Function GetValue(ByVal key As String) As Object
        Return Me.GetValue(Of Object)(key)
    End Function

    Public Sub SetValue(Of T)(ByVal key As String, ByVal value As T)
        Me._dictionary.Item(key) = Me.GetInternalValue(value)
    End Sub

    Public Sub SetValue(ByVal key As String, ByVal value As Object)
        Me.SetValue(Of Object)(key, value)
    End Sub

    Public Function TryGetValue(Of T)(ByVal key As String, <Out()> ByRef value As T) As Boolean
        Dim outValue As KeyValuePair(Of Type, Object)
        Dim result As Boolean = Me._dictionary.TryGetValue(key, outValue)
        value = DirectCast(outValue.Value, T)
        Return result
    End Function

    Public Function GetValueOrDefault(Of T)(ByVal key As String, ByVal defaultValue As T) As T
        Dim value As T
        If Me.TryGetValue(Of T)(key, value) Then
            Return value
        End If
        Me.Add(key, defaultValue)
        Return defaultValue
    End Function

    Public Function GetValueOrDefault(Of T)(ByVal key As String) As T
        Return Me.GetValueOrDefault(Of T)(key, CType(Nothing, T))
    End Function

    Public Function GetValueOrDefault(ByVal key As String, ByVal defaultValue As Object) As Object
        Return Me.GetValueOrDefault(Of Object)(key, defaultValue)
    End Function

    Public Function GetValueOrDefault(ByVal key As String) As Object
        Return Me.GetValueOrDefault(Of Object)(key)
    End Function

End Class
