Option Strict On
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Setting
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使用して変更できます。  
    'コード エディタを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Setting))
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Username = New System.Windows.Forms.TextBox
        Me.Password = New System.Windows.Forms.TextBox
        Me.Save = New System.Windows.Forms.Button
        Me.Cancel = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.TimelinePeriod = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.NextThreshold = New System.Windows.Forms.TextBox
        Me.DMPeriod = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.NextPages = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.ReadLogDays = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.StartupReadPages = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.StartupReaded = New System.Windows.Forms.CheckBox
        Me.ReadLogUnit = New System.Windows.Forms.ComboBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.StatusText = New System.Windows.Forms.TextBox
        Me.PlaySnd = New System.Windows.Forms.CheckBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.OneWayLv = New System.Windows.Forms.CheckBox
        Me.Label16 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.btnInputFont = New System.Windows.Forms.Button
        Me.lblInputFont = New System.Windows.Forms.Label
        Me.Label65 = New System.Windows.Forms.Label
        Me.btnInputBackcolor = New System.Windows.Forms.Button
        Me.lblInputBackcolor = New System.Windows.Forms.Label
        Me.Label52 = New System.Windows.Forms.Label
        Me.btnUnread = New System.Windows.Forms.Button
        Me.lblUnread = New System.Windows.Forms.Label
        Me.Label20 = New System.Windows.Forms.Label
        Me.Button4 = New System.Windows.Forms.Button
        Me.Label48 = New System.Windows.Forms.Label
        Me.Label49 = New System.Windows.Forms.Label
        Me.Button2 = New System.Windows.Forms.Button
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label37 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.Label18 = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.btnAtFromTarget = New System.Windows.Forms.Button
        Me.lblAtFromTarget = New System.Windows.Forms.Label
        Me.Label28 = New System.Windows.Forms.Label
        Me.btnAtTarget = New System.Windows.Forms.Button
        Me.lblAtTarget = New System.Windows.Forms.Label
        Me.Label30 = New System.Windows.Forms.Label
        Me.btnTarget = New System.Windows.Forms.Button
        Me.lblTarget = New System.Windows.Forms.Label
        Me.Label32 = New System.Windows.Forms.Label
        Me.btnAtSelf = New System.Windows.Forms.Button
        Me.lblAtSelf = New System.Windows.Forms.Label
        Me.Label34 = New System.Windows.Forms.Label
        Me.btnSelf = New System.Windows.Forms.Button
        Me.lblSelf = New System.Windows.Forms.Label
        Me.Label36 = New System.Windows.Forms.Label
        Me.btnDetail = New System.Windows.Forms.Button
        Me.lblDetail = New System.Windows.Forms.Label
        Me.Label26 = New System.Windows.Forms.Label
        Me.btnOWL = New System.Windows.Forms.Button
        Me.lblOWL = New System.Windows.Forms.Label
        Me.Label24 = New System.Windows.Forms.Label
        Me.btnFav = New System.Windows.Forms.Button
        Me.lblFav = New System.Windows.Forms.Label
        Me.Label22 = New System.Windows.Forms.Label
        Me.btnListFont = New System.Windows.Forms.Button
        Me.lblListFont = New System.Windows.Forms.Label
        Me.Label61 = New System.Windows.Forms.Label
        Me.FontDialog1 = New System.Windows.Forms.FontDialog
        Me.ColorDialog1 = New System.Windows.Forms.ColorDialog
        Me.cmbNameBalloon = New System.Windows.Forms.ComboBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.CheckUseRecommendStatus = New System.Windows.Forms.CheckBox
        Me.CheckSortOrderLock = New System.Windows.Forms.CheckBox
        Me.Label21 = New System.Windows.Forms.Label
        Me.CmbDateTimeFormat = New System.Windows.Forms.ComboBox
        Me.Label23 = New System.Windows.Forms.Label
        Me.CheckBox3 = New System.Windows.Forms.CheckBox
        Me.Label25 = New System.Windows.Forms.Label
        Me.CheckPostCtrlEnter = New System.Windows.Forms.CheckBox
        Me.Label27 = New System.Windows.Forms.Label
        Me.Label31 = New System.Windows.Forms.Label
        Me.Label33 = New System.Windows.Forms.Label
        Me.Label35 = New System.Windows.Forms.Label
        Me.StartupReadReply = New System.Windows.Forms.TextBox
        Me.StartupReadDM = New System.Windows.Forms.TextBox
        Me.TextBox3 = New System.Windows.Forms.TextBox
        Me.IconSize = New System.Windows.Forms.ComboBox
        Me.Label38 = New System.Windows.Forms.Label
        Me.UReadMng = New System.Windows.Forms.CheckBox
        Me.Label39 = New System.Windows.Forms.Label
        Me.CheckBox6 = New System.Windows.Forms.CheckBox
        Me.Label40 = New System.Windows.Forms.Label
        Me.CheckCloseToExit = New System.Windows.Forms.CheckBox
        Me.Label41 = New System.Windows.Forms.Label
        Me.CheckMinimizeToTray = New System.Windows.Forms.CheckBox
        Me.BrowserPathText = New System.Windows.Forms.TextBox
        Me.Label44 = New System.Windows.Forms.Label
        Me.CheckboxReply = New System.Windows.Forms.CheckBox
        Me.CheckDispUsername = New System.Windows.Forms.CheckBox
        Me.Label46 = New System.Windows.Forms.Label
        Me.Label45 = New System.Windows.Forms.Label
        Me.ComboDispTitle = New System.Windows.Forms.ComboBox
        Me.Label47 = New System.Windows.Forms.Label
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.Label69 = New System.Windows.Forms.Label
        Me.ReplyPeriod = New System.Windows.Forms.TextBox
        Me.CheckPostAndGet = New System.Windows.Forms.CheckBox
        Me.Label67 = New System.Windows.Forms.Label
        Me.TextCountApi = New System.Windows.Forms.TextBox
        Me.Label66 = New System.Windows.Forms.Label
        Me.CheckPostMethod = New System.Windows.Forms.CheckBox
        Me.Label43 = New System.Windows.Forms.Label
        Me.CheckUseApi = New System.Windows.Forms.CheckBox
        Me.Label54 = New System.Windows.Forms.Label
        Me.CheckStartupFollowers = New System.Windows.Forms.CheckBox
        Me.Label53 = New System.Windows.Forms.Label
        Me.CheckStartupKey = New System.Windows.Forms.CheckBox
        Me.Label51 = New System.Windows.Forms.Label
        Me.CheckStartupVersion = New System.Windows.Forms.CheckBox
        Me.CheckPeriodAdjust = New System.Windows.Forms.CheckBox
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.CheckProtectNotInclude = New System.Windows.Forms.CheckBox
        Me.Label42 = New System.Windows.Forms.Label
        Me.CheckAutoConvertUrl = New System.Windows.Forms.CheckBox
        Me.Label29 = New System.Windows.Forms.Label
        Me.CheckAlwaysTop = New System.Windows.Forms.CheckBox
        Me.Label58 = New System.Windows.Forms.Label
        Me.Label57 = New System.Windows.Forms.Label
        Me.Label56 = New System.Windows.Forms.Label
        Me.CheckFavRestrict = New System.Windows.Forms.CheckBox
        Me.CheckTinyURL = New System.Windows.Forms.CheckBox
        Me.Label50 = New System.Windows.Forms.Label
        Me.Button3 = New System.Windows.Forms.Button
        Me.TabPage3 = New System.Windows.Forms.TabPage
        Me.Label68 = New System.Windows.Forms.Label
        Me.CheckBalloonLimit = New System.Windows.Forms.CheckBox
        Me.LabelDateTimeFormatApplied = New System.Windows.Forms.Label
        Me.Label62 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.chkUnreadStyle = New System.Windows.Forms.CheckBox
        Me.TabPage4 = New System.Windows.Forms.TabPage
        Me.TabPage5 = New System.Windows.Forms.TabPage
        Me.Label64 = New System.Windows.Forms.Label
        Me.ConnectionTimeOut = New System.Windows.Forms.TextBox
        Me.Label63 = New System.Windows.Forms.Label
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.Label55 = New System.Windows.Forms.Label
        Me.TextProxyPassword = New System.Windows.Forms.TextBox
        Me.LabelProxyPassword = New System.Windows.Forms.Label
        Me.TextProxyUser = New System.Windows.Forms.TextBox
        Me.LabelProxyUser = New System.Windows.Forms.Label
        Me.TextProxyPort = New System.Windows.Forms.TextBox
        Me.LabelProxyPort = New System.Windows.Forms.Label
        Me.TextProxyAddress = New System.Windows.Forms.TextBox
        Me.LabelProxyAddress = New System.Windows.Forms.Label
        Me.RadioProxySpecified = New System.Windows.Forms.RadioButton
        Me.RadioProxyIE = New System.Windows.Forms.RadioButton
        Me.RadioProxyNone = New System.Windows.Forms.RadioButton
        Me.TabPage6 = New System.Windows.Forms.TabPage
        Me.Label60 = New System.Windows.Forms.Label
        Me.ComboBoxOutputzUrlmode = New System.Windows.Forms.ComboBox
        Me.Label59 = New System.Windows.Forms.Label
        Me.TextBoxOutputzKey = New System.Windows.Forms.TextBox
        Me.CheckOutputz = New System.Windows.Forms.CheckBox
        Me.GroupBox1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.TabPage5.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.TabPage6.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'Username
        '
        resources.ApplyResources(Me.Username, "Username")
        Me.Username.Name = "Username"
        '
        'Password
        '
        resources.ApplyResources(Me.Password, "Password")
        Me.Password.Name = "Password"
        Me.Password.UseSystemPasswordChar = True
        '
        'Save
        '
        Me.Save.DialogResult = System.Windows.Forms.DialogResult.OK
        resources.ApplyResources(Me.Save, "Save")
        Me.Save.Name = "Save"
        Me.Save.UseVisualStyleBackColor = True
        '
        'Cancel
        '
        Me.Cancel.CausesValidation = False
        Me.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        resources.ApplyResources(Me.Cancel, "Cancel")
        Me.Cancel.Name = "Cancel"
        Me.Cancel.UseVisualStyleBackColor = True
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'TimelinePeriod
        '
        resources.ApplyResources(Me.TimelinePeriod, "TimelinePeriod")
        Me.TimelinePeriod.Name = "TimelinePeriod"
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'NextThreshold
        '
        resources.ApplyResources(Me.NextThreshold, "NextThreshold")
        Me.NextThreshold.Name = "NextThreshold"
        '
        'DMPeriod
        '
        resources.ApplyResources(Me.DMPeriod, "DMPeriod")
        Me.DMPeriod.Name = "DMPeriod"
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        '
        'NextPages
        '
        resources.ApplyResources(Me.NextPages, "NextPages")
        Me.NextPages.Name = "NextPages"
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.Name = "Label6"
        '
        'ReadLogDays
        '
        resources.ApplyResources(Me.ReadLogDays, "ReadLogDays")
        Me.ReadLogDays.Name = "ReadLogDays"
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.Name = "Label7"
        '
        'StartupReadPages
        '
        resources.ApplyResources(Me.StartupReadPages, "StartupReadPages")
        Me.StartupReadPages.Name = "StartupReadPages"
        '
        'Label8
        '
        resources.ApplyResources(Me.Label8, "Label8")
        Me.Label8.Name = "Label8"
        '
        'Label9
        '
        resources.ApplyResources(Me.Label9, "Label9")
        Me.Label9.Name = "Label9"
        '
        'StartupReaded
        '
        resources.ApplyResources(Me.StartupReaded, "StartupReaded")
        Me.StartupReaded.Name = "StartupReaded"
        Me.StartupReaded.UseVisualStyleBackColor = True
        '
        'ReadLogUnit
        '
        resources.ApplyResources(Me.ReadLogUnit, "ReadLogUnit")
        Me.ReadLogUnit.FormattingEnabled = True
        Me.ReadLogUnit.Items.AddRange(New Object() {resources.GetString("ReadLogUnit.Items"), resources.GetString("ReadLogUnit.Items1"), resources.GetString("ReadLogUnit.Items2")})
        Me.ReadLogUnit.Name = "ReadLogUnit"
        '
        'Label11
        '
        resources.ApplyResources(Me.Label11, "Label11")
        Me.Label11.Name = "Label11"
        '
        'Label12
        '
        resources.ApplyResources(Me.Label12, "Label12")
        Me.Label12.Name = "Label12"
        '
        'StatusText
        '
        resources.ApplyResources(Me.StatusText, "StatusText")
        Me.StatusText.Name = "StatusText"
        '
        'PlaySnd
        '
        resources.ApplyResources(Me.PlaySnd, "PlaySnd")
        Me.PlaySnd.Name = "PlaySnd"
        Me.PlaySnd.UseVisualStyleBackColor = True
        '
        'Label14
        '
        resources.ApplyResources(Me.Label14, "Label14")
        Me.Label14.Name = "Label14"
        '
        'Label15
        '
        Me.Label15.ForeColor = System.Drawing.SystemColors.ActiveCaption
        resources.ApplyResources(Me.Label15, "Label15")
        Me.Label15.Name = "Label15"
        '
        'OneWayLv
        '
        resources.ApplyResources(Me.OneWayLv, "OneWayLv")
        Me.OneWayLv.Name = "OneWayLv"
        Me.OneWayLv.UseVisualStyleBackColor = True
        '
        'Label16
        '
        resources.ApplyResources(Me.Label16, "Label16")
        Me.Label16.Name = "Label16"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnInputFont)
        Me.GroupBox1.Controls.Add(Me.lblInputFont)
        Me.GroupBox1.Controls.Add(Me.Label65)
        Me.GroupBox1.Controls.Add(Me.btnInputBackcolor)
        Me.GroupBox1.Controls.Add(Me.lblInputBackcolor)
        Me.GroupBox1.Controls.Add(Me.Label52)
        Me.GroupBox1.Controls.Add(Me.btnUnread)
        Me.GroupBox1.Controls.Add(Me.lblUnread)
        Me.GroupBox1.Controls.Add(Me.Label20)
        Me.GroupBox1.Controls.Add(Me.Button4)
        Me.GroupBox1.Controls.Add(Me.Label48)
        Me.GroupBox1.Controls.Add(Me.Label49)
        Me.GroupBox1.Controls.Add(Me.Button2)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.Label37)
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Controls.Add(Me.Label18)
        Me.GroupBox1.Controls.Add(Me.Label19)
        Me.GroupBox1.Controls.Add(Me.btnAtFromTarget)
        Me.GroupBox1.Controls.Add(Me.lblAtFromTarget)
        Me.GroupBox1.Controls.Add(Me.Label28)
        Me.GroupBox1.Controls.Add(Me.btnAtTarget)
        Me.GroupBox1.Controls.Add(Me.lblAtTarget)
        Me.GroupBox1.Controls.Add(Me.Label30)
        Me.GroupBox1.Controls.Add(Me.btnTarget)
        Me.GroupBox1.Controls.Add(Me.lblTarget)
        Me.GroupBox1.Controls.Add(Me.Label32)
        Me.GroupBox1.Controls.Add(Me.btnAtSelf)
        Me.GroupBox1.Controls.Add(Me.lblAtSelf)
        Me.GroupBox1.Controls.Add(Me.Label34)
        Me.GroupBox1.Controls.Add(Me.btnSelf)
        Me.GroupBox1.Controls.Add(Me.lblSelf)
        Me.GroupBox1.Controls.Add(Me.Label36)
        Me.GroupBox1.Controls.Add(Me.btnDetail)
        Me.GroupBox1.Controls.Add(Me.lblDetail)
        Me.GroupBox1.Controls.Add(Me.Label26)
        Me.GroupBox1.Controls.Add(Me.btnOWL)
        Me.GroupBox1.Controls.Add(Me.lblOWL)
        Me.GroupBox1.Controls.Add(Me.Label24)
        Me.GroupBox1.Controls.Add(Me.btnFav)
        Me.GroupBox1.Controls.Add(Me.lblFav)
        Me.GroupBox1.Controls.Add(Me.Label22)
        Me.GroupBox1.Controls.Add(Me.btnListFont)
        Me.GroupBox1.Controls.Add(Me.lblListFont)
        Me.GroupBox1.Controls.Add(Me.Label61)
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        '
        'btnInputFont
        '
        resources.ApplyResources(Me.btnInputFont, "btnInputFont")
        Me.btnInputFont.Name = "btnInputFont"
        Me.btnInputFont.UseVisualStyleBackColor = True
        '
        'lblInputFont
        '
        Me.lblInputFont.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        resources.ApplyResources(Me.lblInputFont, "lblInputFont")
        Me.lblInputFont.Name = "lblInputFont"
        '
        'Label65
        '
        resources.ApplyResources(Me.Label65, "Label65")
        Me.Label65.Name = "Label65"
        '
        'btnInputBackcolor
        '
        resources.ApplyResources(Me.btnInputBackcolor, "btnInputBackcolor")
        Me.btnInputBackcolor.Name = "btnInputBackcolor"
        Me.btnInputBackcolor.UseVisualStyleBackColor = True
        '
        'lblInputBackcolor
        '
        Me.lblInputBackcolor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        resources.ApplyResources(Me.lblInputBackcolor, "lblInputBackcolor")
        Me.lblInputBackcolor.Name = "lblInputBackcolor"
        '
        'Label52
        '
        resources.ApplyResources(Me.Label52, "Label52")
        Me.Label52.Name = "Label52"
        '
        'btnUnread
        '
        resources.ApplyResources(Me.btnUnread, "btnUnread")
        Me.btnUnread.Name = "btnUnread"
        Me.btnUnread.UseVisualStyleBackColor = True
        '
        'lblUnread
        '
        Me.lblUnread.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        resources.ApplyResources(Me.lblUnread, "lblUnread")
        Me.lblUnread.Name = "lblUnread"
        '
        'Label20
        '
        resources.ApplyResources(Me.Label20, "Label20")
        Me.Label20.Name = "Label20"
        '
        'Button4
        '
        resources.ApplyResources(Me.Button4, "Button4")
        Me.Button4.Name = "Button4"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Label48
        '
        Me.Label48.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        resources.ApplyResources(Me.Label48, "Label48")
        Me.Label48.Name = "Label48"
        '
        'Label49
        '
        resources.ApplyResources(Me.Label49, "Label49")
        Me.Label49.Name = "Label49"
        '
        'Button2
        '
        resources.ApplyResources(Me.Button2, "Button2")
        Me.Button2.Name = "Button2"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Label13
        '
        Me.Label13.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        resources.ApplyResources(Me.Label13, "Label13")
        Me.Label13.Name = "Label13"
        '
        'Label37
        '
        resources.ApplyResources(Me.Label37, "Label37")
        Me.Label37.Name = "Label37"
        '
        'Button1
        '
        resources.ApplyResources(Me.Button1, "Button1")
        Me.Button1.Name = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label18
        '
        Me.Label18.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        resources.ApplyResources(Me.Label18, "Label18")
        Me.Label18.Name = "Label18"
        '
        'Label19
        '
        resources.ApplyResources(Me.Label19, "Label19")
        Me.Label19.Name = "Label19"
        '
        'btnAtFromTarget
        '
        resources.ApplyResources(Me.btnAtFromTarget, "btnAtFromTarget")
        Me.btnAtFromTarget.Name = "btnAtFromTarget"
        Me.btnAtFromTarget.UseVisualStyleBackColor = True
        '
        'lblAtFromTarget
        '
        Me.lblAtFromTarget.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        resources.ApplyResources(Me.lblAtFromTarget, "lblAtFromTarget")
        Me.lblAtFromTarget.Name = "lblAtFromTarget"
        '
        'Label28
        '
        resources.ApplyResources(Me.Label28, "Label28")
        Me.Label28.Name = "Label28"
        '
        'btnAtTarget
        '
        resources.ApplyResources(Me.btnAtTarget, "btnAtTarget")
        Me.btnAtTarget.Name = "btnAtTarget"
        Me.btnAtTarget.UseVisualStyleBackColor = True
        '
        'lblAtTarget
        '
        Me.lblAtTarget.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        resources.ApplyResources(Me.lblAtTarget, "lblAtTarget")
        Me.lblAtTarget.Name = "lblAtTarget"
        '
        'Label30
        '
        resources.ApplyResources(Me.Label30, "Label30")
        Me.Label30.Name = "Label30"
        '
        'btnTarget
        '
        resources.ApplyResources(Me.btnTarget, "btnTarget")
        Me.btnTarget.Name = "btnTarget"
        Me.btnTarget.UseVisualStyleBackColor = True
        '
        'lblTarget
        '
        Me.lblTarget.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        resources.ApplyResources(Me.lblTarget, "lblTarget")
        Me.lblTarget.Name = "lblTarget"
        '
        'Label32
        '
        resources.ApplyResources(Me.Label32, "Label32")
        Me.Label32.Name = "Label32"
        '
        'btnAtSelf
        '
        resources.ApplyResources(Me.btnAtSelf, "btnAtSelf")
        Me.btnAtSelf.Name = "btnAtSelf"
        Me.btnAtSelf.UseVisualStyleBackColor = True
        '
        'lblAtSelf
        '
        Me.lblAtSelf.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        resources.ApplyResources(Me.lblAtSelf, "lblAtSelf")
        Me.lblAtSelf.Name = "lblAtSelf"
        '
        'Label34
        '
        resources.ApplyResources(Me.Label34, "Label34")
        Me.Label34.Name = "Label34"
        '
        'btnSelf
        '
        resources.ApplyResources(Me.btnSelf, "btnSelf")
        Me.btnSelf.Name = "btnSelf"
        Me.btnSelf.UseVisualStyleBackColor = True
        '
        'lblSelf
        '
        Me.lblSelf.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        resources.ApplyResources(Me.lblSelf, "lblSelf")
        Me.lblSelf.Name = "lblSelf"
        '
        'Label36
        '
        resources.ApplyResources(Me.Label36, "Label36")
        Me.Label36.Name = "Label36"
        '
        'btnDetail
        '
        resources.ApplyResources(Me.btnDetail, "btnDetail")
        Me.btnDetail.Name = "btnDetail"
        Me.btnDetail.UseVisualStyleBackColor = True
        '
        'lblDetail
        '
        Me.lblDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        resources.ApplyResources(Me.lblDetail, "lblDetail")
        Me.lblDetail.Name = "lblDetail"
        '
        'Label26
        '
        resources.ApplyResources(Me.Label26, "Label26")
        Me.Label26.Name = "Label26"
        '
        'btnOWL
        '
        resources.ApplyResources(Me.btnOWL, "btnOWL")
        Me.btnOWL.Name = "btnOWL"
        Me.btnOWL.UseVisualStyleBackColor = True
        '
        'lblOWL
        '
        Me.lblOWL.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        resources.ApplyResources(Me.lblOWL, "lblOWL")
        Me.lblOWL.Name = "lblOWL"
        '
        'Label24
        '
        resources.ApplyResources(Me.Label24, "Label24")
        Me.Label24.Name = "Label24"
        '
        'btnFav
        '
        resources.ApplyResources(Me.btnFav, "btnFav")
        Me.btnFav.Name = "btnFav"
        Me.btnFav.UseVisualStyleBackColor = True
        '
        'lblFav
        '
        Me.lblFav.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        resources.ApplyResources(Me.lblFav, "lblFav")
        Me.lblFav.Name = "lblFav"
        '
        'Label22
        '
        resources.ApplyResources(Me.Label22, "Label22")
        Me.Label22.Name = "Label22"
        '
        'btnListFont
        '
        resources.ApplyResources(Me.btnListFont, "btnListFont")
        Me.btnListFont.Name = "btnListFont"
        Me.btnListFont.UseVisualStyleBackColor = True
        '
        'lblListFont
        '
        Me.lblListFont.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        resources.ApplyResources(Me.lblListFont, "lblListFont")
        Me.lblListFont.Name = "lblListFont"
        '
        'Label61
        '
        resources.ApplyResources(Me.Label61, "Label61")
        Me.Label61.Name = "Label61"
        '
        'cmbNameBalloon
        '
        Me.cmbNameBalloon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbNameBalloon.FormattingEnabled = True
        Me.cmbNameBalloon.Items.AddRange(New Object() {resources.GetString("cmbNameBalloon.Items"), resources.GetString("cmbNameBalloon.Items1"), resources.GetString("cmbNameBalloon.Items2")})
        resources.ApplyResources(Me.cmbNameBalloon, "cmbNameBalloon")
        Me.cmbNameBalloon.Name = "cmbNameBalloon"
        '
        'Label10
        '
        resources.ApplyResources(Me.Label10, "Label10")
        Me.Label10.Name = "Label10"
        '
        'CheckUseRecommendStatus
        '
        resources.ApplyResources(Me.CheckUseRecommendStatus, "CheckUseRecommendStatus")
        Me.CheckUseRecommendStatus.Name = "CheckUseRecommendStatus"
        Me.CheckUseRecommendStatus.UseVisualStyleBackColor = True
        '
        'CheckSortOrderLock
        '
        resources.ApplyResources(Me.CheckSortOrderLock, "CheckSortOrderLock")
        Me.CheckSortOrderLock.Name = "CheckSortOrderLock"
        Me.CheckSortOrderLock.UseVisualStyleBackColor = True
        '
        'Label21
        '
        resources.ApplyResources(Me.Label21, "Label21")
        Me.Label21.Name = "Label21"
        '
        'CmbDateTimeFormat
        '
        resources.ApplyResources(Me.CmbDateTimeFormat, "CmbDateTimeFormat")
        Me.CmbDateTimeFormat.Items.AddRange(New Object() {resources.GetString("CmbDateTimeFormat.Items"), resources.GetString("CmbDateTimeFormat.Items1"), resources.GetString("CmbDateTimeFormat.Items2"), resources.GetString("CmbDateTimeFormat.Items3"), resources.GetString("CmbDateTimeFormat.Items4"), resources.GetString("CmbDateTimeFormat.Items5"), resources.GetString("CmbDateTimeFormat.Items6"), resources.GetString("CmbDateTimeFormat.Items7"), resources.GetString("CmbDateTimeFormat.Items8"), resources.GetString("CmbDateTimeFormat.Items9"), resources.GetString("CmbDateTimeFormat.Items10")})
        Me.CmbDateTimeFormat.Name = "CmbDateTimeFormat"
        '
        'Label23
        '
        resources.ApplyResources(Me.Label23, "Label23")
        Me.Label23.Name = "Label23"
        '
        'CheckBox3
        '
        resources.ApplyResources(Me.CheckBox3, "CheckBox3")
        Me.CheckBox3.Name = "CheckBox3"
        Me.CheckBox3.UseVisualStyleBackColor = True
        '
        'Label25
        '
        resources.ApplyResources(Me.Label25, "Label25")
        Me.Label25.Name = "Label25"
        '
        'CheckPostCtrlEnter
        '
        resources.ApplyResources(Me.CheckPostCtrlEnter, "CheckPostCtrlEnter")
        Me.CheckPostCtrlEnter.Name = "CheckPostCtrlEnter"
        Me.CheckPostCtrlEnter.UseVisualStyleBackColor = True
        '
        'Label27
        '
        resources.ApplyResources(Me.Label27, "Label27")
        Me.Label27.Name = "Label27"
        '
        'Label31
        '
        resources.ApplyResources(Me.Label31, "Label31")
        Me.Label31.Name = "Label31"
        '
        'Label33
        '
        resources.ApplyResources(Me.Label33, "Label33")
        Me.Label33.Name = "Label33"
        '
        'Label35
        '
        resources.ApplyResources(Me.Label35, "Label35")
        Me.Label35.Name = "Label35"
        '
        'StartupReadReply
        '
        resources.ApplyResources(Me.StartupReadReply, "StartupReadReply")
        Me.StartupReadReply.Name = "StartupReadReply"
        '
        'StartupReadDM
        '
        resources.ApplyResources(Me.StartupReadDM, "StartupReadDM")
        Me.StartupReadDM.Name = "StartupReadDM"
        '
        'TextBox3
        '
        resources.ApplyResources(Me.TextBox3, "TextBox3")
        Me.TextBox3.Name = "TextBox3"
        '
        'IconSize
        '
        Me.IconSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.IconSize.FormattingEnabled = True
        Me.IconSize.Items.AddRange(New Object() {resources.GetString("IconSize.Items"), resources.GetString("IconSize.Items1"), resources.GetString("IconSize.Items2"), resources.GetString("IconSize.Items3"), resources.GetString("IconSize.Items4")})
        resources.ApplyResources(Me.IconSize, "IconSize")
        Me.IconSize.Name = "IconSize"
        '
        'Label38
        '
        resources.ApplyResources(Me.Label38, "Label38")
        Me.Label38.Name = "Label38"
        '
        'UReadMng
        '
        resources.ApplyResources(Me.UReadMng, "UReadMng")
        Me.UReadMng.Name = "UReadMng"
        Me.UReadMng.UseVisualStyleBackColor = True
        '
        'Label39
        '
        resources.ApplyResources(Me.Label39, "Label39")
        Me.Label39.Name = "Label39"
        '
        'CheckBox6
        '
        resources.ApplyResources(Me.CheckBox6, "CheckBox6")
        Me.CheckBox6.Name = "CheckBox6"
        Me.CheckBox6.UseVisualStyleBackColor = True
        '
        'Label40
        '
        resources.ApplyResources(Me.Label40, "Label40")
        Me.Label40.Name = "Label40"
        '
        'CheckCloseToExit
        '
        resources.ApplyResources(Me.CheckCloseToExit, "CheckCloseToExit")
        Me.CheckCloseToExit.Name = "CheckCloseToExit"
        Me.CheckCloseToExit.UseVisualStyleBackColor = True
        '
        'Label41
        '
        resources.ApplyResources(Me.Label41, "Label41")
        Me.Label41.Name = "Label41"
        '
        'CheckMinimizeToTray
        '
        resources.ApplyResources(Me.CheckMinimizeToTray, "CheckMinimizeToTray")
        Me.CheckMinimizeToTray.Name = "CheckMinimizeToTray"
        Me.CheckMinimizeToTray.UseVisualStyleBackColor = True
        '
        'BrowserPathText
        '
        resources.ApplyResources(Me.BrowserPathText, "BrowserPathText")
        Me.BrowserPathText.Name = "BrowserPathText"
        '
        'Label44
        '
        resources.ApplyResources(Me.Label44, "Label44")
        Me.Label44.Name = "Label44"
        '
        'CheckboxReply
        '
        resources.ApplyResources(Me.CheckboxReply, "CheckboxReply")
        Me.CheckboxReply.Name = "CheckboxReply"
        Me.CheckboxReply.UseVisualStyleBackColor = True
        '
        'CheckDispUsername
        '
        resources.ApplyResources(Me.CheckDispUsername, "CheckDispUsername")
        Me.CheckDispUsername.Name = "CheckDispUsername"
        Me.CheckDispUsername.UseVisualStyleBackColor = True
        '
        'Label46
        '
        resources.ApplyResources(Me.Label46, "Label46")
        Me.Label46.Name = "Label46"
        '
        'Label45
        '
        resources.ApplyResources(Me.Label45, "Label45")
        Me.Label45.Name = "Label45"
        '
        'ComboDispTitle
        '
        Me.ComboDispTitle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboDispTitle.FormattingEnabled = True
        Me.ComboDispTitle.Items.AddRange(New Object() {resources.GetString("ComboDispTitle.Items"), resources.GetString("ComboDispTitle.Items1"), resources.GetString("ComboDispTitle.Items2"), resources.GetString("ComboDispTitle.Items3"), resources.GetString("ComboDispTitle.Items4"), resources.GetString("ComboDispTitle.Items5"), resources.GetString("ComboDispTitle.Items6")})
        resources.ApplyResources(Me.ComboDispTitle, "ComboDispTitle")
        Me.ComboDispTitle.Name = "ComboDispTitle"
        '
        'Label47
        '
        Me.Label47.ForeColor = System.Drawing.SystemColors.ActiveCaption
        resources.ApplyResources(Me.Label47, "Label47")
        Me.Label47.Name = "Label47"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Controls.Add(Me.TabPage5)
        Me.TabControl1.Controls.Add(Me.TabPage6)
        resources.ApplyResources(Me.TabControl1, "TabControl1")
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Label69)
        Me.TabPage1.Controls.Add(Me.ReplyPeriod)
        Me.TabPage1.Controls.Add(Me.CheckPostAndGet)
        Me.TabPage1.Controls.Add(Me.Label67)
        Me.TabPage1.Controls.Add(Me.TextCountApi)
        Me.TabPage1.Controls.Add(Me.Label66)
        Me.TabPage1.Controls.Add(Me.CheckPostMethod)
        Me.TabPage1.Controls.Add(Me.Label43)
        Me.TabPage1.Controls.Add(Me.CheckUseApi)
        Me.TabPage1.Controls.Add(Me.Label54)
        Me.TabPage1.Controls.Add(Me.CheckStartupFollowers)
        Me.TabPage1.Controls.Add(Me.Label53)
        Me.TabPage1.Controls.Add(Me.CheckStartupKey)
        Me.TabPage1.Controls.Add(Me.Label51)
        Me.TabPage1.Controls.Add(Me.CheckStartupVersion)
        Me.TabPage1.Controls.Add(Me.CheckPeriodAdjust)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Controls.Add(Me.Username)
        Me.TabPage1.Controls.Add(Me.Password)
        Me.TabPage1.Controls.Add(Me.Label3)
        Me.TabPage1.Controls.Add(Me.TimelinePeriod)
        Me.TabPage1.Controls.Add(Me.CheckboxReply)
        Me.TabPage1.Controls.Add(Me.Label4)
        Me.TabPage1.Controls.Add(Me.NextThreshold)
        Me.TabPage1.Controls.Add(Me.Label5)
        Me.TabPage1.Controls.Add(Me.DMPeriod)
        Me.TabPage1.Controls.Add(Me.Label6)
        Me.TabPage1.Controls.Add(Me.NextPages)
        Me.TabPage1.Controls.Add(Me.Label7)
        Me.TabPage1.Controls.Add(Me.ReadLogDays)
        Me.TabPage1.Controls.Add(Me.Label8)
        Me.TabPage1.Controls.Add(Me.StartupReadPages)
        Me.TabPage1.Controls.Add(Me.Label9)
        Me.TabPage1.Controls.Add(Me.StartupReaded)
        Me.TabPage1.Controls.Add(Me.ReadLogUnit)
        Me.TabPage1.Controls.Add(Me.Label31)
        Me.TabPage1.Controls.Add(Me.Label33)
        Me.TabPage1.Controls.Add(Me.Label35)
        Me.TabPage1.Controls.Add(Me.StartupReadReply)
        Me.TabPage1.Controls.Add(Me.StartupReadDM)
        resources.ApplyResources(Me.TabPage1, "TabPage1")
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Label69
        '
        resources.ApplyResources(Me.Label69, "Label69")
        Me.Label69.Name = "Label69"
        '
        'ReplyPeriod
        '
        resources.ApplyResources(Me.ReplyPeriod, "ReplyPeriod")
        Me.ReplyPeriod.Name = "ReplyPeriod"
        '
        'CheckPostAndGet
        '
        resources.ApplyResources(Me.CheckPostAndGet, "CheckPostAndGet")
        Me.CheckPostAndGet.Name = "CheckPostAndGet"
        Me.CheckPostAndGet.UseVisualStyleBackColor = True
        '
        'Label67
        '
        resources.ApplyResources(Me.Label67, "Label67")
        Me.Label67.Name = "Label67"
        '
        'TextCountApi
        '
        resources.ApplyResources(Me.TextCountApi, "TextCountApi")
        Me.TextCountApi.Name = "TextCountApi"
        '
        'Label66
        '
        resources.ApplyResources(Me.Label66, "Label66")
        Me.Label66.Name = "Label66"
        '
        'CheckPostMethod
        '
        resources.ApplyResources(Me.CheckPostMethod, "CheckPostMethod")
        Me.CheckPostMethod.Name = "CheckPostMethod"
        Me.CheckPostMethod.UseVisualStyleBackColor = True
        '
        'Label43
        '
        resources.ApplyResources(Me.Label43, "Label43")
        Me.Label43.Name = "Label43"
        '
        'CheckUseApi
        '
        resources.ApplyResources(Me.CheckUseApi, "CheckUseApi")
        Me.CheckUseApi.Name = "CheckUseApi"
        Me.CheckUseApi.UseVisualStyleBackColor = True
        '
        'Label54
        '
        resources.ApplyResources(Me.Label54, "Label54")
        Me.Label54.Name = "Label54"
        '
        'CheckStartupFollowers
        '
        resources.ApplyResources(Me.CheckStartupFollowers, "CheckStartupFollowers")
        Me.CheckStartupFollowers.Name = "CheckStartupFollowers"
        Me.CheckStartupFollowers.UseVisualStyleBackColor = True
        '
        'Label53
        '
        resources.ApplyResources(Me.Label53, "Label53")
        Me.Label53.Name = "Label53"
        '
        'CheckStartupKey
        '
        resources.ApplyResources(Me.CheckStartupKey, "CheckStartupKey")
        Me.CheckStartupKey.Name = "CheckStartupKey"
        Me.CheckStartupKey.UseVisualStyleBackColor = True
        '
        'Label51
        '
        resources.ApplyResources(Me.Label51, "Label51")
        Me.Label51.Name = "Label51"
        '
        'CheckStartupVersion
        '
        resources.ApplyResources(Me.CheckStartupVersion, "CheckStartupVersion")
        Me.CheckStartupVersion.Name = "CheckStartupVersion"
        Me.CheckStartupVersion.UseVisualStyleBackColor = True
        '
        'CheckPeriodAdjust
        '
        resources.ApplyResources(Me.CheckPeriodAdjust, "CheckPeriodAdjust")
        Me.CheckPeriodAdjust.Name = "CheckPeriodAdjust"
        Me.CheckPeriodAdjust.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.CheckProtectNotInclude)
        Me.TabPage2.Controls.Add(Me.Label42)
        Me.TabPage2.Controls.Add(Me.CheckAutoConvertUrl)
        Me.TabPage2.Controls.Add(Me.Label29)
        Me.TabPage2.Controls.Add(Me.CheckAlwaysTop)
        Me.TabPage2.Controls.Add(Me.Label58)
        Me.TabPage2.Controls.Add(Me.Label57)
        Me.TabPage2.Controls.Add(Me.Label56)
        Me.TabPage2.Controls.Add(Me.CheckFavRestrict)
        Me.TabPage2.Controls.Add(Me.CheckTinyURL)
        Me.TabPage2.Controls.Add(Me.Label50)
        Me.TabPage2.Controls.Add(Me.Button3)
        Me.TabPage2.Controls.Add(Me.PlaySnd)
        Me.TabPage2.Controls.Add(Me.Label14)
        Me.TabPage2.Controls.Add(Me.Label15)
        Me.TabPage2.Controls.Add(Me.Label21)
        Me.TabPage2.Controls.Add(Me.CheckSortOrderLock)
        Me.TabPage2.Controls.Add(Me.Label38)
        Me.TabPage2.Controls.Add(Me.BrowserPathText)
        Me.TabPage2.Controls.Add(Me.UReadMng)
        Me.TabPage2.Controls.Add(Me.Label44)
        Me.TabPage2.Controls.Add(Me.CheckCloseToExit)
        Me.TabPage2.Controls.Add(Me.Label40)
        Me.TabPage2.Controls.Add(Me.CheckMinimizeToTray)
        Me.TabPage2.Controls.Add(Me.Label41)
        Me.TabPage2.Controls.Add(Me.Label27)
        Me.TabPage2.Controls.Add(Me.Label39)
        Me.TabPage2.Controls.Add(Me.CheckPostCtrlEnter)
        Me.TabPage2.Controls.Add(Me.CheckBox6)
        Me.TabPage2.Controls.Add(Me.Label12)
        Me.TabPage2.Controls.Add(Me.StatusText)
        Me.TabPage2.Controls.Add(Me.CheckUseRecommendStatus)
        resources.ApplyResources(Me.TabPage2, "TabPage2")
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'CheckProtectNotInclude
        '
        resources.ApplyResources(Me.CheckProtectNotInclude, "CheckProtectNotInclude")
        Me.CheckProtectNotInclude.Name = "CheckProtectNotInclude"
        Me.CheckProtectNotInclude.UseVisualStyleBackColor = True
        '
        'Label42
        '
        resources.ApplyResources(Me.Label42, "Label42")
        Me.Label42.Name = "Label42"
        '
        'CheckAutoConvertUrl
        '
        resources.ApplyResources(Me.CheckAutoConvertUrl, "CheckAutoConvertUrl")
        Me.CheckAutoConvertUrl.Name = "CheckAutoConvertUrl"
        Me.CheckAutoConvertUrl.UseVisualStyleBackColor = True
        '
        'Label29
        '
        resources.ApplyResources(Me.Label29, "Label29")
        Me.Label29.Name = "Label29"
        '
        'CheckAlwaysTop
        '
        resources.ApplyResources(Me.CheckAlwaysTop, "CheckAlwaysTop")
        Me.CheckAlwaysTop.Name = "CheckAlwaysTop"
        Me.CheckAlwaysTop.UseVisualStyleBackColor = True
        '
        'Label58
        '
        resources.ApplyResources(Me.Label58, "Label58")
        Me.Label58.Name = "Label58"
        '
        'Label57
        '
        Me.Label57.ForeColor = System.Drawing.SystemColors.ActiveCaption
        resources.ApplyResources(Me.Label57, "Label57")
        Me.Label57.Name = "Label57"
        '
        'Label56
        '
        resources.ApplyResources(Me.Label56, "Label56")
        Me.Label56.Name = "Label56"
        '
        'CheckFavRestrict
        '
        resources.ApplyResources(Me.CheckFavRestrict, "CheckFavRestrict")
        Me.CheckFavRestrict.Name = "CheckFavRestrict"
        Me.CheckFavRestrict.UseVisualStyleBackColor = True
        '
        'CheckTinyURL
        '
        resources.ApplyResources(Me.CheckTinyURL, "CheckTinyURL")
        Me.CheckTinyURL.Name = "CheckTinyURL"
        Me.CheckTinyURL.UseVisualStyleBackColor = True
        '
        'Label50
        '
        resources.ApplyResources(Me.Label50, "Label50")
        Me.Label50.Name = "Label50"
        '
        'Button3
        '
        resources.ApplyResources(Me.Button3, "Button3")
        Me.Button3.Name = "Button3"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.Label68)
        Me.TabPage3.Controls.Add(Me.CheckBalloonLimit)
        Me.TabPage3.Controls.Add(Me.LabelDateTimeFormatApplied)
        Me.TabPage3.Controls.Add(Me.Label62)
        Me.TabPage3.Controls.Add(Me.Label17)
        Me.TabPage3.Controls.Add(Me.chkUnreadStyle)
        Me.TabPage3.Controls.Add(Me.Label10)
        Me.TabPage3.Controls.Add(Me.ComboDispTitle)
        Me.TabPage3.Controls.Add(Me.Label47)
        Me.TabPage3.Controls.Add(Me.CmbDateTimeFormat)
        Me.TabPage3.Controls.Add(Me.Label45)
        Me.TabPage3.Controls.Add(Me.Label23)
        Me.TabPage3.Controls.Add(Me.cmbNameBalloon)
        Me.TabPage3.Controls.Add(Me.Label46)
        Me.TabPage3.Controls.Add(Me.CheckDispUsername)
        Me.TabPage3.Controls.Add(Me.Label11)
        Me.TabPage3.Controls.Add(Me.Label16)
        Me.TabPage3.Controls.Add(Me.OneWayLv)
        Me.TabPage3.Controls.Add(Me.Label25)
        Me.TabPage3.Controls.Add(Me.IconSize)
        Me.TabPage3.Controls.Add(Me.CheckBox3)
        Me.TabPage3.Controls.Add(Me.TextBox3)
        resources.ApplyResources(Me.TabPage3, "TabPage3")
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'Label68
        '
        resources.ApplyResources(Me.Label68, "Label68")
        Me.Label68.Name = "Label68"
        '
        'CheckBalloonLimit
        '
        resources.ApplyResources(Me.CheckBalloonLimit, "CheckBalloonLimit")
        Me.CheckBalloonLimit.Name = "CheckBalloonLimit"
        Me.CheckBalloonLimit.UseVisualStyleBackColor = True
        '
        'LabelDateTimeFormatApplied
        '
        resources.ApplyResources(Me.LabelDateTimeFormatApplied, "LabelDateTimeFormatApplied")
        Me.LabelDateTimeFormatApplied.Name = "LabelDateTimeFormatApplied"
        '
        'Label62
        '
        resources.ApplyResources(Me.Label62, "Label62")
        Me.Label62.Name = "Label62"
        '
        'Label17
        '
        resources.ApplyResources(Me.Label17, "Label17")
        Me.Label17.Name = "Label17"
        '
        'chkUnreadStyle
        '
        resources.ApplyResources(Me.chkUnreadStyle, "chkUnreadStyle")
        Me.chkUnreadStyle.Name = "chkUnreadStyle"
        Me.chkUnreadStyle.UseVisualStyleBackColor = True
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.GroupBox1)
        resources.ApplyResources(Me.TabPage4, "TabPage4")
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.Label64)
        Me.TabPage5.Controls.Add(Me.ConnectionTimeOut)
        Me.TabPage5.Controls.Add(Me.Label63)
        Me.TabPage5.Controls.Add(Me.GroupBox2)
        resources.ApplyResources(Me.TabPage5, "TabPage5")
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'Label64
        '
        resources.ApplyResources(Me.Label64, "Label64")
        Me.Label64.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.Label64.Name = "Label64"
        '
        'ConnectionTimeOut
        '
        resources.ApplyResources(Me.ConnectionTimeOut, "ConnectionTimeOut")
        Me.ConnectionTimeOut.Name = "ConnectionTimeOut"
        '
        'Label63
        '
        resources.ApplyResources(Me.Label63, "Label63")
        Me.Label63.Name = "Label63"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label55)
        Me.GroupBox2.Controls.Add(Me.TextProxyPassword)
        Me.GroupBox2.Controls.Add(Me.LabelProxyPassword)
        Me.GroupBox2.Controls.Add(Me.TextProxyUser)
        Me.GroupBox2.Controls.Add(Me.LabelProxyUser)
        Me.GroupBox2.Controls.Add(Me.TextProxyPort)
        Me.GroupBox2.Controls.Add(Me.LabelProxyPort)
        Me.GroupBox2.Controls.Add(Me.TextProxyAddress)
        Me.GroupBox2.Controls.Add(Me.LabelProxyAddress)
        Me.GroupBox2.Controls.Add(Me.RadioProxySpecified)
        Me.GroupBox2.Controls.Add(Me.RadioProxyIE)
        Me.GroupBox2.Controls.Add(Me.RadioProxyNone)
        resources.ApplyResources(Me.GroupBox2, "GroupBox2")
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.TabStop = False
        '
        'Label55
        '
        resources.ApplyResources(Me.Label55, "Label55")
        Me.Label55.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.Label55.Name = "Label55"
        '
        'TextProxyPassword
        '
        resources.ApplyResources(Me.TextProxyPassword, "TextProxyPassword")
        Me.TextProxyPassword.Name = "TextProxyPassword"
        Me.TextProxyPassword.UseSystemPasswordChar = True
        '
        'LabelProxyPassword
        '
        resources.ApplyResources(Me.LabelProxyPassword, "LabelProxyPassword")
        Me.LabelProxyPassword.Name = "LabelProxyPassword"
        '
        'TextProxyUser
        '
        resources.ApplyResources(Me.TextProxyUser, "TextProxyUser")
        Me.TextProxyUser.Name = "TextProxyUser"
        '
        'LabelProxyUser
        '
        resources.ApplyResources(Me.LabelProxyUser, "LabelProxyUser")
        Me.LabelProxyUser.Name = "LabelProxyUser"
        '
        'TextProxyPort
        '
        resources.ApplyResources(Me.TextProxyPort, "TextProxyPort")
        Me.TextProxyPort.Name = "TextProxyPort"
        '
        'LabelProxyPort
        '
        resources.ApplyResources(Me.LabelProxyPort, "LabelProxyPort")
        Me.LabelProxyPort.Name = "LabelProxyPort"
        '
        'TextProxyAddress
        '
        resources.ApplyResources(Me.TextProxyAddress, "TextProxyAddress")
        Me.TextProxyAddress.Name = "TextProxyAddress"
        '
        'LabelProxyAddress
        '
        resources.ApplyResources(Me.LabelProxyAddress, "LabelProxyAddress")
        Me.LabelProxyAddress.Name = "LabelProxyAddress"
        '
        'RadioProxySpecified
        '
        resources.ApplyResources(Me.RadioProxySpecified, "RadioProxySpecified")
        Me.RadioProxySpecified.Name = "RadioProxySpecified"
        Me.RadioProxySpecified.UseVisualStyleBackColor = True
        '
        'RadioProxyIE
        '
        resources.ApplyResources(Me.RadioProxyIE, "RadioProxyIE")
        Me.RadioProxyIE.Checked = True
        Me.RadioProxyIE.Name = "RadioProxyIE"
        Me.RadioProxyIE.TabStop = True
        Me.RadioProxyIE.UseVisualStyleBackColor = True
        '
        'RadioProxyNone
        '
        resources.ApplyResources(Me.RadioProxyNone, "RadioProxyNone")
        Me.RadioProxyNone.Name = "RadioProxyNone"
        Me.RadioProxyNone.UseVisualStyleBackColor = True
        '
        'TabPage6
        '
        Me.TabPage6.Controls.Add(Me.Label60)
        Me.TabPage6.Controls.Add(Me.ComboBoxOutputzUrlmode)
        Me.TabPage6.Controls.Add(Me.Label59)
        Me.TabPage6.Controls.Add(Me.TextBoxOutputzKey)
        Me.TabPage6.Controls.Add(Me.CheckOutputz)
        resources.ApplyResources(Me.TabPage6, "TabPage6")
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.UseVisualStyleBackColor = True
        '
        'Label60
        '
        resources.ApplyResources(Me.Label60, "Label60")
        Me.Label60.Name = "Label60"
        '
        'ComboBoxOutputzUrlmode
        '
        Me.ComboBoxOutputzUrlmode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxOutputzUrlmode.FormattingEnabled = True
        Me.ComboBoxOutputzUrlmode.Items.AddRange(New Object() {resources.GetString("ComboBoxOutputzUrlmode.Items"), resources.GetString("ComboBoxOutputzUrlmode.Items1")})
        resources.ApplyResources(Me.ComboBoxOutputzUrlmode, "ComboBoxOutputzUrlmode")
        Me.ComboBoxOutputzUrlmode.Name = "ComboBoxOutputzUrlmode"
        '
        'Label59
        '
        resources.ApplyResources(Me.Label59, "Label59")
        Me.Label59.Name = "Label59"
        '
        'TextBoxOutputzKey
        '
        resources.ApplyResources(Me.TextBoxOutputzKey, "TextBoxOutputzKey")
        Me.TextBoxOutputzKey.Name = "TextBoxOutputzKey"
        Me.TextBoxOutputzKey.UseSystemPasswordChar = True
        '
        'CheckOutputz
        '
        resources.ApplyResources(Me.CheckOutputz, "CheckOutputz")
        Me.CheckOutputz.Name = "CheckOutputz"
        Me.CheckOutputz.UseVisualStyleBackColor = True
        '
        'Setting
        '
        Me.AcceptButton = Me.Save
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Cancel)
        Me.Controls.Add(Me.Save)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Setting"
        Me.TopMost = True
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        Me.TabPage4.ResumeLayout(False)
        Me.TabPage5.ResumeLayout(False)
        Me.TabPage5.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.TabPage6.ResumeLayout(False)
        Me.TabPage6.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Username As System.Windows.Forms.TextBox
    Friend WithEvents Password As System.Windows.Forms.TextBox
    Friend WithEvents Save As System.Windows.Forms.Button
    Friend WithEvents Cancel As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TimelinePeriod As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents NextThreshold As System.Windows.Forms.TextBox
    Friend WithEvents DMPeriod As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents NextPages As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ReadLogDays As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents StartupReadPages As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents StartupReaded As System.Windows.Forms.CheckBox
    Friend WithEvents ReadLogUnit As System.Windows.Forms.ComboBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents StatusText As System.Windows.Forms.TextBox
    Friend WithEvents PlaySnd As System.Windows.Forms.CheckBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents OneWayLv As System.Windows.Forms.CheckBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnDetail As System.Windows.Forms.Button
    Friend WithEvents lblDetail As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents btnOWL As System.Windows.Forms.Button
    Friend WithEvents lblOWL As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents btnFav As System.Windows.Forms.Button
    Friend WithEvents lblFav As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents FontDialog1 As System.Windows.Forms.FontDialog
    Friend WithEvents ColorDialog1 As System.Windows.Forms.ColorDialog
    Friend WithEvents btnAtFromTarget As System.Windows.Forms.Button
    Friend WithEvents lblAtFromTarget As System.Windows.Forms.Label
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents btnAtTarget As System.Windows.Forms.Button
    Friend WithEvents lblAtTarget As System.Windows.Forms.Label
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents btnTarget As System.Windows.Forms.Button
    Friend WithEvents lblTarget As System.Windows.Forms.Label
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents btnAtSelf As System.Windows.Forms.Button
    Friend WithEvents lblAtSelf As System.Windows.Forms.Label
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents btnSelf As System.Windows.Forms.Button
    Friend WithEvents lblSelf As System.Windows.Forms.Label
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents cmbNameBalloon As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents CheckUseRecommendStatus As System.Windows.Forms.CheckBox
    Friend WithEvents CheckSortOrderLock As System.Windows.Forms.CheckBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents CmbDateTimeFormat As System.Windows.Forms.ComboBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents CheckBox3 As System.Windows.Forms.CheckBox
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents CheckPostCtrlEnter As System.Windows.Forms.CheckBox
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents StartupReadReply As System.Windows.Forms.TextBox
    Friend WithEvents StartupReadDM As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents IconSize As System.Windows.Forms.ComboBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents UReadMng As System.Windows.Forms.CheckBox
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents CheckBox6 As System.Windows.Forms.CheckBox
    Friend WithEvents Label40 As System.Windows.Forms.Label
    Friend WithEvents CheckCloseToExit As System.Windows.Forms.CheckBox
    Friend WithEvents Label41 As System.Windows.Forms.Label
    Friend WithEvents CheckMinimizeToTray As System.Windows.Forms.CheckBox
    Friend WithEvents BrowserPathText As System.Windows.Forms.TextBox
    Friend WithEvents Label44 As System.Windows.Forms.Label
    Friend WithEvents CheckboxReply As System.Windows.Forms.CheckBox
    Friend WithEvents CheckDispUsername As System.Windows.Forms.CheckBox
    Friend WithEvents Label46 As System.Windows.Forms.Label
    Friend WithEvents Label45 As System.Windows.Forms.Label
    Friend WithEvents ComboDispTitle As System.Windows.Forms.ComboBox
    Friend WithEvents Label47 As System.Windows.Forms.Label
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Label48 As System.Windows.Forms.Label
    Friend WithEvents Label49 As System.Windows.Forms.Label
    Friend WithEvents CheckTinyURL As System.Windows.Forms.CheckBox
    Friend WithEvents Label50 As System.Windows.Forms.Label
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents RadioProxySpecified As System.Windows.Forms.RadioButton
    Friend WithEvents RadioProxyIE As System.Windows.Forms.RadioButton
    Friend WithEvents RadioProxyNone As System.Windows.Forms.RadioButton
    Friend WithEvents TextProxyPort As System.Windows.Forms.TextBox
    Friend WithEvents LabelProxyPort As System.Windows.Forms.Label
    Friend WithEvents TextProxyAddress As System.Windows.Forms.TextBox
    Friend WithEvents LabelProxyAddress As System.Windows.Forms.Label
    Friend WithEvents TextProxyPassword As System.Windows.Forms.TextBox
    Friend WithEvents LabelProxyPassword As System.Windows.Forms.Label
    Friend WithEvents TextProxyUser As System.Windows.Forms.TextBox
    Friend WithEvents LabelProxyUser As System.Windows.Forms.Label
    Friend WithEvents Label55 As System.Windows.Forms.Label
    Friend WithEvents CheckPeriodAdjust As System.Windows.Forms.CheckBox
    Friend WithEvents Label53 As System.Windows.Forms.Label
    Friend WithEvents CheckStartupKey As System.Windows.Forms.CheckBox
    Friend WithEvents Label51 As System.Windows.Forms.Label
    Friend WithEvents CheckStartupVersion As System.Windows.Forms.CheckBox
    Friend WithEvents Label54 As System.Windows.Forms.Label
    Friend WithEvents CheckStartupFollowers As System.Windows.Forms.CheckBox
    Friend WithEvents Label56 As System.Windows.Forms.Label
    Friend WithEvents CheckFavRestrict As System.Windows.Forms.CheckBox
    Friend WithEvents Label57 As System.Windows.Forms.Label
    Friend WithEvents CheckAlwaysTop As System.Windows.Forms.CheckBox
    Friend WithEvents Label58 As System.Windows.Forms.Label
    Friend WithEvents CheckAutoConvertUrl As System.Windows.Forms.CheckBox
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents TabPage6 As System.Windows.Forms.TabPage
    Friend WithEvents Label59 As System.Windows.Forms.Label
    Friend WithEvents TextBoxOutputzKey As System.Windows.Forms.TextBox
    Friend WithEvents CheckOutputz As System.Windows.Forms.CheckBox
    Friend WithEvents Label60 As System.Windows.Forms.Label
    Friend WithEvents ComboBoxOutputzUrlmode As System.Windows.Forms.ComboBox
    Friend WithEvents btnListFont As System.Windows.Forms.Button
    Friend WithEvents lblListFont As System.Windows.Forms.Label
    Friend WithEvents Label61 As System.Windows.Forms.Label
    Friend WithEvents btnUnread As System.Windows.Forms.Button
    Friend WithEvents lblUnread As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents chkUnreadStyle As System.Windows.Forms.CheckBox
    Friend WithEvents LabelDateTimeFormatApplied As System.Windows.Forms.Label
    Friend WithEvents Label62 As System.Windows.Forms.Label
    Friend WithEvents Label63 As System.Windows.Forms.Label
    Friend WithEvents Label64 As System.Windows.Forms.Label
    Friend WithEvents ConnectionTimeOut As System.Windows.Forms.TextBox
    Friend WithEvents CheckProtectNotInclude As System.Windows.Forms.CheckBox
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents btnInputBackcolor As System.Windows.Forms.Button
    Friend WithEvents lblInputBackcolor As System.Windows.Forms.Label
    Friend WithEvents Label52 As System.Windows.Forms.Label
    Friend WithEvents btnInputFont As System.Windows.Forms.Button
    Friend WithEvents lblInputFont As System.Windows.Forms.Label
    Friend WithEvents Label65 As System.Windows.Forms.Label
    Friend WithEvents Label66 As System.Windows.Forms.Label
    Friend WithEvents CheckPostMethod As System.Windows.Forms.CheckBox
    Friend WithEvents Label43 As System.Windows.Forms.Label
    Friend WithEvents CheckUseApi As System.Windows.Forms.CheckBox
    Friend WithEvents Label67 As System.Windows.Forms.Label
    Friend WithEvents TextCountApi As System.Windows.Forms.TextBox
    Friend WithEvents Label68 As System.Windows.Forms.Label
    Friend WithEvents CheckBalloonLimit As System.Windows.Forms.CheckBox
    Friend WithEvents CheckPostAndGet As System.Windows.Forms.CheckBox
    Friend WithEvents Label69 As System.Windows.Forms.Label
    Friend WithEvents ReplyPeriod As System.Windows.Forms.TextBox
End Class
