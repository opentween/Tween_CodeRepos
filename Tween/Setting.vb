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

Imports System.Xml

Public Class Setting
    Private _xrootElement As XmlElement

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

    'Public Enum LogUnitEnum
    '    Minute
    '    Hour
    '    Day
    'End Enum

    'Public Enum IconSizes
    '    IconNone = 0
    '    Icon16 = 1
    '    Icon24 = 2
    '    Icon48 = 3
    '    Icon48_2 = 4
    'End Enum

    'Public Enum NameBalloonEnum
    '    None
    '    UserID
    '    NickName
    'End Enum

    'Public Enum DispTitleEnum
    '    None
    '    Ver
    '    Post
    '    UnreadRepCount
    '    UnreadAllCount
    '    UnreadAllRepCount
    '    UnreadCountAllCount
    'End Enum

    Public Function GetValue(ByVal path As String, ByVal defaultValue As String) As String
        Dim xnode As XmlNode = Me._xrootElement.SelectSingleNode("/tween-configuration/" + path)
        If xnode Is Nothing Then
            Me.SetValue(path, defaultValue)
            Return defaultValue
        Else
            Return xnode.Value
        End If
    End Function

    Public Function GetValue(ByVal path As String, ByVal defaultValue As Integer) As Integer
        Dim xnode As XmlNode = Me._xrootElement.SelectSingleNode("/tween-configuration/" + path)
        If xnode Is Nothing Then
            Me.SetValue(path, defaultValue)
            Return defaultValue
        Else
            Return Integer.Parse(xnode.InnerXml)
        End If
    End Function

    Public Function GetValue(ByVal path As String, ByVal defaultValue As Boolean) As Boolean
        Dim xnode As XmlNode = Me._xrootElement.SelectSingleNode("/tween-configuration/" + path)
        If xnode Is Nothing Then
            Me.SetValue(path, defaultValue)
            Return defaultValue
        Else
            Return Boolean.Parse(xnode.InnerXml)
        End If
    End Function

    Public Function GetValue(ByVal path As String, ByVal defaultValue As Date) As Date
        Dim xnode As XmlNode = Me._xrootElement.SelectSingleNode("/tween-configuration/" + path)
        If xnode Is Nothing Then
            Me.SetValue(path, defaultValue)
            Return defaultValue
        Else
            Return Date.Parse(xnode.InnerXml)
        End If
    End Function

    Public Function GetElement(ByVal path As String) As XmlElement
        Dim xnode As XmlNode = Me._xrootElement.SelectSingleNode("/tween-configuration/" + path)
        If xnode Is Nothing Then
            Me.RetrievePath(path)
            Return Nothing
        Else
            Return DirectCast(xnode, XmlElement)
        End If
    End Function

    Public Sub SetValue(ByVal path As String, ByVal value As String)
        Me.RetrievePath(path).SetValue(value)
    End Sub

    Public Sub SetValue(ByVal path As String, ByVal value As Integer)
        Me.RetrievePath(path).SetValue(value.ToString())
    End Sub

    Public Sub SetValue(ByVal path As String, ByVal value As Boolean)
        Me.RetrievePath(path).SetValue(value.ToString())
    End Sub

    Public Sub SetValue(ByVal path As String, ByVal value As DateTime)
        Me.RetrievePath(path).SetValue(value.ToString())
    End Sub

    Public Sub SetElement(ByVal path As String, ByVal xelement As XmlElement)
        Me.RetrievePath(path).ReplaceSelf(xelement.ToString())
    End Sub

    Public Sub LoadConfiguration(ByVal fileName As String)
        Dim xdocument As XmlDocument = New XmlDocument()
        If IO.File.Exists(fileName) Then
            xdocument.Load(fileName)
        Else
            xdocument.AppendChild(xdocument.CreateXmlDeclaration("1.0", "utf-8", Nothing))
            xdocument.AppendChild(xdocument.CreateComment(My.Resources.LoadConfigurationText1))
            xdocument.AppendChild(xdocument.CreateElement("tween-configuration"))
            xdocument.Save(fileName)
        End If
        Me._xrootElement = xdocument.DocumentElement
    End Sub

    Public Sub SaveConfiguration(ByVal fileName As String)
        Me._xrootElement.OwnerDocument.Save(fileName)
        Me.LoadConfiguration(fileName)
    End Sub

    Private Function RetrievePath(ByVal path As String) As XPath.XPathNavigator
        Dim xnav As XPath.XPathNavigator = Me._xrootElement.OwnerDocument.CreateNavigator()
        xnav = xnav.SelectSingleNode("/tween-configuration")
        For Each fragment As String In path.Split(New Char() {"/"c}, StringSplitOptions.RemoveEmptyEntries)
            If (xnav.SelectSingleNode(fragment) Is Nothing) Then
                xnav.AppendChildElement("", fragment, Nothing, Nothing)
            End If
            xnav = xnav.SelectSingleNode(fragment)
        Next
        Return xnav
    End Function

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
            _MyMaxPostNum = CType(MaxPost.Text, Integer)

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
            TweenMain.PlaySoundMenuItem.Checked = _MyPlaySound
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
            _Outputz.Enable = _MyOutputz
            _MyOutputzKey = TextBoxOutputzKey.Text.Trim()
            _Outputz.key = _MyOutputzKey

            Select Case ComboBoxOutputzUrlmode.SelectedIndex
                Case 0
                    _MyOutputzUrlmode = OutputzUrlmode.twittercom
                    _Outputz.url = "http://twitter.com/"
                Case 1
                    _MyOutputzUrlmode = OutputzUrlmode.twittercomWithUsername
                    _Outputz.url = "http://twitter.com/" + _MyuserID
            End Select
            'TweenMain.SetMainWindowTitle()
            'TweenMain.SetNotifyIconText()

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
        MaxPost.Text = _MyMaxPostNum.ToString()
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
        _Outputz.Enable = _MyOutputz
        TextBoxOutputzKey.Text = _MyOutputzKey
        _Outputz.key = _MyOutputzKey

        Select Case _MyOutputzUrlmode
            Case OutputzUrlmode.twittercom
                ComboBoxOutputzUrlmode.SelectedIndex = 0
            Case OutputzUrlmode.twittercomWithUsername
                ComboBoxOutputzUrlmode.SelectedIndex = 1
        End Select
        'TweenMain.SetMainWindowTitle()
        'TweenMain.SetNotifyIconText()

        TabControl1.SelectedIndex = 0
        ActiveControl = Username

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
    Public Property Outputz() As Boolean
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
End Class

