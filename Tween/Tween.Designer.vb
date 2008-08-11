<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TweenMain
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
        Me.components = New System.ComponentModel.Container
        Me.PostButton = New System.Windows.Forms.Button
        Me.StatusText = New System.Windows.Forms.TextBox
        Me.TimerTimeline = New System.Windows.Forms.Timer(Me.components)
        Me.UserPicture = New System.Windows.Forms.PictureBox
        Me.NameLabel = New System.Windows.Forms.Label
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.ListTab = New System.Windows.Forms.TabControl
        Me.ContextMenuTabProperty = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AddTabMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator20 = New System.Windows.Forms.ToolStripSeparator
        Me.UreadManageMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NotifyDispMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SoundFileComboBox = New System.Windows.Forms.ToolStripComboBox
        Me.ToolStripSeparator18 = New System.Windows.Forms.ToolStripSeparator
        Me.FilterEditMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator19 = New System.Windows.Forms.ToolStripSeparator
        Me.DeleteTabMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TabRecent = New System.Windows.Forms.TabPage
        Me.Timeline = New Tween.TweenCustomControl.DetailsListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ReplyStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ReplyAllStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DMStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.FavAddToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.FavRemoveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.MoveToHomeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MoveToFavToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.StatusOpenMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.FavorareMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenURLMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.TabMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.IDRuleMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me.ReadedStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.UnreadStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.JumpUnreadMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator
        Me.SelectAllMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator
        Me.DeleteStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator
        Me.RefreshStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TabReply = New System.Windows.Forms.TabPage
        Me.Reply = New Tween.TweenCustomControl.DetailsListView
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader8 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader9 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader10 = New System.Windows.Forms.ColumnHeader
        Me.TabDirect = New System.Windows.Forms.TabPage
        Me.DirectMsg = New Tween.TweenCustomControl.DetailsListView
        Me.ColumnHeader11 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader12 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader13 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader14 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader15 = New System.Windows.Forms.ColumnHeader
        Me.TabImage = New System.Windows.Forms.ImageList(Me.components)
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.lblLen = New System.Windows.Forms.Label
        Me.PostBrowser = New System.Windows.Forms.WebBrowser
        Me.DateTimeLabel = New System.Windows.Forms.Label
        Me.MenuItemTab = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemOperate = New System.Windows.Forms.ToolStripMenuItem
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SettingStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator
        Me.SaveLogMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator17 = New System.Windows.Forms.ToolStripSeparator
        Me.NewPostPopMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ListLockMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator15 = New System.Windows.Forms.ToolStripSeparator
        Me.EndToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemFile = New System.Windows.Forms.ToolStripMenuItem
        Me.GetTimelineWorker = New System.ComponentModel.BackgroundWorker
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.MenuItemEdit = New System.Windows.Forms.ToolStripMenuItem
        Me.CopySTOTMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CopyURLMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator
        Me.MenuItemSubSearch = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemSearchNext = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemSearchPrev = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemCommand = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemHelp = New System.Windows.Forms.ToolStripMenuItem
        Me.MatomeMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator12 = New System.Windows.Forms.ToolStripSeparator
        Me.OfficialMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DLPageMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator16 = New System.Windows.Forms.ToolStripSeparator
        Me.VerUpMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.WedataMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator14 = New System.Windows.Forms.ToolStripSeparator
        Me.InfoTwitterMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator
        Me.AboutMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.StatusLabelUrl = New System.Windows.Forms.ToolStripStatusLabel
        Me.StatusLabel = New System.Windows.Forms.ToolStripStatusLabel
        Me.TimerDM = New System.Windows.Forms.Timer(Me.components)
        Me.GetLogWorker = New System.ComponentModel.BackgroundWorker
        Me.ExecWorker = New System.ComponentModel.BackgroundWorker
        Me.TimerColorize = New System.Windows.Forms.Timer(Me.components)
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.TimerRefreshIcon = New System.Windows.Forms.Timer(Me.components)
        Me.PostWorker = New System.ComponentModel.BackgroundWorker
        Me.ClearTabMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator11 = New System.Windows.Forms.ToolStripSeparator
        CType(Me.UserPicture, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.ListTab.SuspendLayout()
        Me.ContextMenuTabProperty.SuspendLayout()
        Me.TabRecent.SuspendLayout()
        Me.ContextMenuStrip2.SuspendLayout()
        Me.TabReply.SuspendLayout()
        Me.TabDirect.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'PostButton
        '
        Me.PostButton.Location = New System.Drawing.Point(408, 63)
        Me.PostButton.Name = "PostButton"
        Me.PostButton.Size = New System.Drawing.Size(50, 20)
        Me.PostButton.TabIndex = 1
        Me.PostButton.TabStop = False
        Me.PostButton.Text = "Post"
        Me.PostButton.UseVisualStyleBackColor = True
        '
        'StatusText
        '
        Me.StatusText.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.SetColumnSpan(Me.StatusText, 3)
        Me.StatusText.Location = New System.Drawing.Point(3, 63)
        Me.StatusText.Name = "StatusText"
        Me.StatusText.Size = New System.Drawing.Size(349, 19)
        Me.StatusText.TabIndex = 0
        '
        'TimerTimeline
        '
        Me.TimerTimeline.Interval = 60000
        '
        'UserPicture
        '
        Me.UserPicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.UserPicture.Location = New System.Drawing.Point(3, 3)
        Me.UserPicture.Name = "UserPicture"
        Me.TableLayoutPanel1.SetRowSpan(Me.UserPicture, 2)
        Me.UserPicture.Size = New System.Drawing.Size(48, 48)
        Me.UserPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.UserPicture.TabIndex = 5
        Me.UserPicture.TabStop = False
        '
        'NameLabel
        '
        Me.NameLabel.AutoSize = True
        Me.NameLabel.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.NameLabel.Location = New System.Drawing.Point(58, 0)
        Me.NameLabel.Name = "NameLabel"
        Me.NameLabel.Size = New System.Drawing.Size(53, 12)
        Me.NameLabel.TabIndex = 6
        Me.NameLabel.Text = "lblName"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.ListTab)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.TableLayoutPanel1)
        Me.SplitContainer1.Panel2MinSize = 50
        Me.SplitContainer1.Size = New System.Drawing.Size(468, 316)
        Me.SplitContainer1.SplitterDistance = 197
        Me.SplitContainer1.TabIndex = 8
        Me.SplitContainer1.TabStop = False
        '
        'ListTab
        '
        Me.ListTab.Alignment = System.Windows.Forms.TabAlignment.Bottom
        Me.ListTab.AllowDrop = True
        Me.ListTab.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListTab.ContextMenuStrip = Me.ContextMenuTabProperty
        Me.ListTab.Controls.Add(Me.TabRecent)
        Me.ListTab.Controls.Add(Me.TabReply)
        Me.ListTab.Controls.Add(Me.TabDirect)
        Me.ListTab.ImageList = Me.TabImage
        Me.ListTab.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.ListTab.Location = New System.Drawing.Point(0, 25)
        Me.ListTab.Margin = New System.Windows.Forms.Padding(0)
        Me.ListTab.Multiline = True
        Me.ListTab.Name = "ListTab"
        Me.ListTab.SelectedIndex = 0
        Me.ListTab.Size = New System.Drawing.Size(468, 168)
        Me.ListTab.TabIndex = 4
        Me.ListTab.TabStop = False
        '
        'ContextMenuTabProperty
        '
        Me.ContextMenuTabProperty.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddTabMenuItem, Me.ToolStripSeparator20, Me.UreadManageMenuItem, Me.NotifyDispMenuItem, Me.SoundFileComboBox, Me.ToolStripSeparator18, Me.FilterEditMenuItem, Me.ToolStripSeparator19, Me.ClearTabMenuItem, Me.ToolStripSeparator11, Me.DeleteTabMenuItem})
        Me.ContextMenuTabProperty.Name = "ContextMenuStrip3"
        Me.ContextMenuTabProperty.OwnerItem = Me.MenuItemTab
        Me.ContextMenuTabProperty.Size = New System.Drawing.Size(186, 208)
        '
        'AddTabMenuItem
        '
        Me.AddTabMenuItem.Name = "AddTabMenuItem"
        Me.AddTabMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.AddTabMenuItem.Text = "タブ作成(&N)"
        '
        'ToolStripSeparator20
        '
        Me.ToolStripSeparator20.Name = "ToolStripSeparator20"
        Me.ToolStripSeparator20.Size = New System.Drawing.Size(182, 6)
        '
        'UreadManageMenuItem
        '
        Me.UreadManageMenuItem.CheckOnClick = True
        Me.UreadManageMenuItem.Name = "UreadManageMenuItem"
        Me.UreadManageMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.UreadManageMenuItem.Text = "未読管理(&U)"
        '
        'NotifyDispMenuItem
        '
        Me.NotifyDispMenuItem.CheckOnClick = True
        Me.NotifyDispMenuItem.Name = "NotifyDispMenuItem"
        Me.NotifyDispMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.NotifyDispMenuItem.Text = "新着通知表示(&Q)"
        '
        'SoundFileComboBox
        '
        Me.SoundFileComboBox.AutoToolTip = True
        Me.SoundFileComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.SoundFileComboBox.Name = "SoundFileComboBox"
        Me.SoundFileComboBox.Size = New System.Drawing.Size(121, 22)
        Me.SoundFileComboBox.ToolTipText = "再生するwavファイルを指定してください"
        '
        'ToolStripSeparator18
        '
        Me.ToolStripSeparator18.Name = "ToolStripSeparator18"
        Me.ToolStripSeparator18.Size = New System.Drawing.Size(182, 6)
        '
        'FilterEditMenuItem
        '
        Me.FilterEditMenuItem.Name = "FilterEditMenuItem"
        Me.FilterEditMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.FilterEditMenuItem.Text = "振り分けルール編集(&F)"
        '
        'ToolStripSeparator19
        '
        Me.ToolStripSeparator19.Name = "ToolStripSeparator19"
        Me.ToolStripSeparator19.Size = New System.Drawing.Size(182, 6)
        '
        'DeleteTabMenuItem
        '
        Me.DeleteTabMenuItem.Name = "DeleteTabMenuItem"
        Me.DeleteTabMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.DeleteTabMenuItem.Text = "タブ削除(&D)"
        '
        'TabRecent
        '
        Me.TabRecent.Controls.Add(Me.Timeline)
        Me.TabRecent.Location = New System.Drawing.Point(4, 4)
        Me.TabRecent.Name = "TabRecent"
        Me.TabRecent.Size = New System.Drawing.Size(460, 141)
        Me.TabRecent.TabIndex = 0
        Me.TabRecent.Text = "Recent"
        Me.TabRecent.UseVisualStyleBackColor = True
        '
        'Timeline
        '
        Me.Timeline.AllowColumnReorder = True
        Me.Timeline.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5})
        Me.Timeline.ContextMenuStrip = Me.ContextMenuStrip2
        Me.Timeline.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Timeline.FullRowSelect = True
        Me.Timeline.HideSelection = False
        Me.Timeline.Location = New System.Drawing.Point(0, 0)
        Me.Timeline.Name = "Timeline"
        Me.Timeline.OwnerDraw = True
        Me.Timeline.ShowItemToolTips = True
        Me.Timeline.Size = New System.Drawing.Size(460, 141)
        Me.Timeline.TabIndex = 0
        Me.Timeline.UseCompatibleStateImageBehavior = False
        Me.Timeline.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = ""
        Me.ColumnHeader1.Width = 26
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "名前"
        Me.ColumnHeader2.Width = 80
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "投稿"
        Me.ColumnHeader3.Width = 300
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "日時"
        Me.ColumnHeader4.Width = 50
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "ユーザ名"
        Me.ColumnHeader5.Width = 50
        '
        'ContextMenuStrip2
        '
        Me.ContextMenuStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ReplyStripMenuItem, Me.ReplyAllStripMenuItem, Me.DMStripMenuItem, Me.ToolStripSeparator2, Me.FavAddToolStripMenuItem, Me.FavRemoveToolStripMenuItem, Me.ToolStripSeparator1, Me.MoveToHomeToolStripMenuItem, Me.MoveToFavToolStripMenuItem, Me.StatusOpenMenuItem, Me.FavorareMenuItem, Me.OpenURLMenuItem, Me.ToolStripSeparator3, Me.TabMenuItem, Me.IDRuleMenuItem, Me.ToolStripSeparator4, Me.ReadedStripMenuItem, Me.UnreadStripMenuItem, Me.JumpUnreadMenuItem, Me.ToolStripSeparator10, Me.SelectAllMenuItem, Me.ToolStripSeparator5, Me.DeleteStripMenuItem, Me.ToolStripSeparator8, Me.RefreshStripMenuItem})
        Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
        Me.ContextMenuStrip2.ShowImageMargin = False
        Me.ContextMenuStrip2.Size = New System.Drawing.Size(219, 442)
        '
        'ReplyStripMenuItem
        '
        Me.ReplyStripMenuItem.Name = "ReplyStripMenuItem"
        Me.ReplyStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.R), System.Windows.Forms.Keys)
        Me.ReplyStripMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.ReplyStripMenuItem.Text = "@返信(&R)"
        '
        'ReplyAllStripMenuItem
        '
        Me.ReplyAllStripMenuItem.Name = "ReplyAllStripMenuItem"
        Me.ReplyAllStripMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
                    Or System.Windows.Forms.Keys.R), System.Windows.Forms.Keys)
        Me.ReplyAllStripMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.ReplyAllStripMenuItem.Text = "@返信ALL"
        '
        'DMStripMenuItem
        '
        Me.DMStripMenuItem.Name = "DMStripMenuItem"
        Me.DMStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.M), System.Windows.Forms.Keys)
        Me.DMStripMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.DMStripMenuItem.Text = "DM送信(&M)"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(215, 6)
        '
        'FavAddToolStripMenuItem
        '
        Me.FavAddToolStripMenuItem.Name = "FavAddToolStripMenuItem"
        Me.FavAddToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.FavAddToolStripMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.FavAddToolStripMenuItem.Text = "Fav追加(&F)"
        '
        'FavRemoveToolStripMenuItem
        '
        Me.FavRemoveToolStripMenuItem.Name = "FavRemoveToolStripMenuItem"
        Me.FavRemoveToolStripMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
                    Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.FavRemoveToolStripMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.FavRemoveToolStripMenuItem.Text = "Fav削除"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(215, 6)
        '
        'MoveToHomeToolStripMenuItem
        '
        Me.MoveToHomeToolStripMenuItem.Name = "MoveToHomeToolStripMenuItem"
        Me.MoveToHomeToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.H), System.Windows.Forms.Keys)
        Me.MoveToHomeToolStripMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.MoveToHomeToolStripMenuItem.Text = "ホームを開く(&H)"
        '
        'MoveToFavToolStripMenuItem
        '
        Me.MoveToFavToolStripMenuItem.Name = "MoveToFavToolStripMenuItem"
        Me.MoveToFavToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.G), System.Windows.Forms.Keys)
        Me.MoveToFavToolStripMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.MoveToFavToolStripMenuItem.Text = "Favを開く(&G)"
        '
        'StatusOpenMenuItem
        '
        Me.StatusOpenMenuItem.Name = "StatusOpenMenuItem"
        Me.StatusOpenMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.StatusOpenMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.StatusOpenMenuItem.Text = "ステータスを開く(&O)"
        '
        'FavorareMenuItem
        '
        Me.FavorareMenuItem.Name = "FavorareMenuItem"
        Me.FavorareMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
                    Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.FavorareMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.FavorareMenuItem.Text = "ふぁぼられを開く(&P)"
        '
        'OpenURLMenuItem
        '
        Me.OpenURLMenuItem.Name = "OpenURLMenuItem"
        Me.OpenURLMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.OpenURLMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.OpenURLMenuItem.Text = "発言内URLを開く(&U)"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(215, 6)
        '
        'TabMenuItem
        '
        Me.TabMenuItem.Name = "TabMenuItem"
        Me.TabMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.TabMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.TabMenuItem.Text = "タブ振り分けルール作成(&N)"
        '
        'IDRuleMenuItem
        '
        Me.IDRuleMenuItem.Name = "IDRuleMenuItem"
        Me.IDRuleMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.IDRuleMenuItem.Text = "ID振り分けルール作成"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(215, 6)
        '
        'ReadedStripMenuItem
        '
        Me.ReadedStripMenuItem.Name = "ReadedStripMenuItem"
        Me.ReadedStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.B), System.Windows.Forms.Keys)
        Me.ReadedStripMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.ReadedStripMenuItem.Text = "既読にする(&B)"
        '
        'UnreadStripMenuItem
        '
        Me.UnreadStripMenuItem.Name = "UnreadStripMenuItem"
        Me.UnreadStripMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Shift) _
                    Or System.Windows.Forms.Keys.B), System.Windows.Forms.Keys)
        Me.UnreadStripMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.UnreadStripMenuItem.Text = "未読にする"
        '
        'JumpUnreadMenuItem
        '
        Me.JumpUnreadMenuItem.Name = "JumpUnreadMenuItem"
        Me.JumpUnreadMenuItem.ShortcutKeyDisplayString = ""
        Me.JumpUnreadMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.JumpUnreadMenuItem.Text = "未読へジャンプ"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(215, 6)
        '
        'SelectAllMenuItem
        '
        Me.SelectAllMenuItem.Name = "SelectAllMenuItem"
        Me.SelectAllMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.SelectAllMenuItem.Text = "全て選択(&A)"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(215, 6)
        '
        'DeleteStripMenuItem
        '
        Me.DeleteStripMenuItem.Name = "DeleteStripMenuItem"
        Me.DeleteStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D), System.Windows.Forms.Keys)
        Me.DeleteStripMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.DeleteStripMenuItem.Text = "削除(&D)"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(215, 6)
        '
        'RefreshStripMenuItem
        '
        Me.RefreshStripMenuItem.Name = "RefreshStripMenuItem"
        Me.RefreshStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5
        Me.RefreshStripMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.RefreshStripMenuItem.Text = "更新(&U)"
        '
        'TabReply
        '
        Me.TabReply.Controls.Add(Me.Reply)
        Me.TabReply.Location = New System.Drawing.Point(4, 4)
        Me.TabReply.Name = "TabReply"
        Me.TabReply.Size = New System.Drawing.Size(460, 141)
        Me.TabReply.TabIndex = 1
        Me.TabReply.Text = "Reply"
        Me.TabReply.UseVisualStyleBackColor = True
        '
        'Reply
        '
        Me.Reply.AllowColumnReorder = True
        Me.Reply.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader6, Me.ColumnHeader7, Me.ColumnHeader8, Me.ColumnHeader9, Me.ColumnHeader10})
        Me.Reply.ContextMenuStrip = Me.ContextMenuStrip2
        Me.Reply.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Reply.FullRowSelect = True
        Me.Reply.HideSelection = False
        Me.Reply.Location = New System.Drawing.Point(0, 0)
        Me.Reply.Name = "Reply"
        Me.Reply.OwnerDraw = True
        Me.Reply.ShowItemToolTips = True
        Me.Reply.Size = New System.Drawing.Size(460, 141)
        Me.Reply.TabIndex = 0
        Me.Reply.UseCompatibleStateImageBehavior = False
        Me.Reply.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = ""
        Me.ColumnHeader6.Width = 26
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "名前"
        Me.ColumnHeader7.Width = 80
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "投稿"
        Me.ColumnHeader8.Width = 300
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "日時"
        Me.ColumnHeader9.Width = 50
        '
        'ColumnHeader10
        '
        Me.ColumnHeader10.Text = "ユーザ名"
        Me.ColumnHeader10.Width = 50
        '
        'TabDirect
        '
        Me.TabDirect.Controls.Add(Me.DirectMsg)
        Me.TabDirect.Location = New System.Drawing.Point(4, 4)
        Me.TabDirect.Name = "TabDirect"
        Me.TabDirect.Size = New System.Drawing.Size(460, 141)
        Me.TabDirect.TabIndex = 2
        Me.TabDirect.Text = "Direct"
        Me.TabDirect.UseVisualStyleBackColor = True
        '
        'DirectMsg
        '
        Me.DirectMsg.AllowColumnReorder = True
        Me.DirectMsg.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader11, Me.ColumnHeader12, Me.ColumnHeader13, Me.ColumnHeader14, Me.ColumnHeader15})
        Me.DirectMsg.ContextMenuStrip = Me.ContextMenuStrip2
        Me.DirectMsg.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DirectMsg.FullRowSelect = True
        Me.DirectMsg.HideSelection = False
        Me.DirectMsg.Location = New System.Drawing.Point(0, 0)
        Me.DirectMsg.Name = "DirectMsg"
        Me.DirectMsg.OwnerDraw = True
        Me.DirectMsg.ShowItemToolTips = True
        Me.DirectMsg.Size = New System.Drawing.Size(460, 141)
        Me.DirectMsg.TabIndex = 0
        Me.DirectMsg.UseCompatibleStateImageBehavior = False
        Me.DirectMsg.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader11
        '
        Me.ColumnHeader11.Text = ""
        Me.ColumnHeader11.Width = 26
        '
        'ColumnHeader12
        '
        Me.ColumnHeader12.Text = "名前"
        Me.ColumnHeader12.Width = 80
        '
        'ColumnHeader13
        '
        Me.ColumnHeader13.Text = "投稿"
        Me.ColumnHeader13.Width = 300
        '
        'ColumnHeader14
        '
        Me.ColumnHeader14.Text = "日時"
        Me.ColumnHeader14.Width = 50
        '
        'ColumnHeader15
        '
        Me.ColumnHeader15.Text = "ユーザ名"
        Me.ColumnHeader15.Width = 50
        '
        'TabImage
        '
        Me.TabImage.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
        Me.TabImage.ImageSize = New System.Drawing.Size(16, 16)
        Me.TabImage.TransparentColor = System.Drawing.Color.Transparent
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 5
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 58.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.PostButton, 4, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.lblLen, 3, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.StatusText, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.UserPicture, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.NameLabel, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.PostBrowser, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.DateTimeLabel, 2, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(464, 111)
        Me.TableLayoutPanel1.TabIndex = 16
        '
        'lblLen
        '
        Me.lblLen.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblLen.Location = New System.Drawing.Point(358, 60)
        Me.lblLen.Name = "lblLen"
        Me.lblLen.Size = New System.Drawing.Size(44, 19)
        Me.lblLen.TabIndex = 13
        Me.lblLen.Text = "999"
        Me.lblLen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PostBrowser
        '
        Me.PostBrowser.AllowWebBrowserDrop = False
        Me.TableLayoutPanel1.SetColumnSpan(Me.PostBrowser, 4)
        Me.PostBrowser.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PostBrowser.Location = New System.Drawing.Point(58, 15)
        Me.PostBrowser.Name = "PostBrowser"
        Me.PostBrowser.Size = New System.Drawing.Size(403, 42)
        Me.PostBrowser.TabIndex = 12
        Me.PostBrowser.TabStop = False
        '
        'DateTimeLabel
        '
        Me.DateTimeLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.SetColumnSpan(Me.DateTimeLabel, 3)
        Me.DateTimeLabel.Location = New System.Drawing.Point(331, 0)
        Me.DateTimeLabel.Name = "DateTimeLabel"
        Me.DateTimeLabel.Size = New System.Drawing.Size(130, 12)
        Me.DateTimeLabel.TabIndex = 7
        Me.DateTimeLabel.Text = "Label1"
        Me.DateTimeLabel.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'MenuItemTab
        '
        Me.MenuItemTab.DropDown = Me.ContextMenuTabProperty
        Me.MenuItemTab.Name = "MenuItemTab"
        Me.MenuItemTab.Size = New System.Drawing.Size(51, 20)
        Me.MenuItemTab.Text = "タブ(&T)"
        '
        'MenuItemOperate
        '
        Me.MenuItemOperate.DropDown = Me.ContextMenuStrip2
        Me.MenuItemOperate.Name = "MenuItemOperate"
        Me.MenuItemOperate.Size = New System.Drawing.Size(59, 20)
        Me.MenuItemOperate.Text = "操作(&O)"
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.NotifyIcon1.Text = "Tween"
        Me.NotifyIcon1.Visible = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SettingStripMenuItem, Me.ToolStripSeparator9, Me.SaveLogMenuItem, Me.ToolStripSeparator17, Me.NewPostPopMenuItem, Me.ListLockMenuItem, Me.ToolStripSeparator15, Me.EndToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.ShowCheckMargin = True
        Me.ContextMenuStrip1.ShowImageMargin = False
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(211, 132)
        '
        'SettingStripMenuItem
        '
        Me.SettingStripMenuItem.Name = "SettingStripMenuItem"
        Me.SettingStripMenuItem.Size = New System.Drawing.Size(210, 22)
        Me.SettingStripMenuItem.Text = "設定(&O)"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(207, 6)
        '
        'SaveLogMenuItem
        '
        Me.SaveLogMenuItem.Name = "SaveLogMenuItem"
        Me.SaveLogMenuItem.Size = New System.Drawing.Size(210, 22)
        Me.SaveLogMenuItem.Text = "ファイル保存(&S)"
        '
        'ToolStripSeparator17
        '
        Me.ToolStripSeparator17.Name = "ToolStripSeparator17"
        Me.ToolStripSeparator17.Size = New System.Drawing.Size(207, 6)
        '
        'NewPostPopMenuItem
        '
        Me.NewPostPopMenuItem.CheckOnClick = True
        Me.NewPostPopMenuItem.Name = "NewPostPopMenuItem"
        Me.NewPostPopMenuItem.Size = New System.Drawing.Size(210, 22)
        Me.NewPostPopMenuItem.Text = "新着通知(&Q)"
        '
        'ListLockMenuItem
        '
        Me.ListLockMenuItem.CheckOnClick = True
        Me.ListLockMenuItem.Name = "ListLockMenuItem"
        Me.ListLockMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.L), System.Windows.Forms.Keys)
        Me.ListLockMenuItem.Size = New System.Drawing.Size(210, 22)
        Me.ListLockMenuItem.Text = "新着時リスト固定(&L)"
        '
        'ToolStripSeparator15
        '
        Me.ToolStripSeparator15.Name = "ToolStripSeparator15"
        Me.ToolStripSeparator15.Size = New System.Drawing.Size(207, 6)
        '
        'EndToolStripMenuItem
        '
        Me.EndToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.EndToolStripMenuItem.Name = "EndToolStripMenuItem"
        Me.EndToolStripMenuItem.ShowShortcutKeys = False
        Me.EndToolStripMenuItem.Size = New System.Drawing.Size(210, 22)
        Me.EndToolStripMenuItem.Text = "終了(&X)"
        '
        'MenuItemFile
        '
        Me.MenuItemFile.DropDown = Me.ContextMenuStrip1
        Me.MenuItemFile.Name = "MenuItemFile"
        Me.MenuItemFile.Size = New System.Drawing.Size(68, 20)
        Me.MenuItemFile.Text = "ファイル(&F)"
        '
        'GetTimelineWorker
        '
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuItemFile, Me.MenuItemEdit, Me.MenuItemOperate, Me.MenuItemTab, Me.MenuItemCommand, Me.MenuItemHelp})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(468, 24)
        Me.MenuStrip1.TabIndex = 9
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'MenuItemEdit
        '
        Me.MenuItemEdit.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopySTOTMenuItem, Me.CopyURLMenuItem, Me.ToolStripSeparator6, Me.MenuItemSubSearch, Me.MenuItemSearchNext, Me.MenuItemSearchPrev})
        Me.MenuItemEdit.Name = "MenuItemEdit"
        Me.MenuItemEdit.Size = New System.Drawing.Size(58, 20)
        Me.MenuItemEdit.Text = "編集(&E)"
        '
        'CopySTOTMenuItem
        '
        Me.CopySTOTMenuItem.Name = "CopySTOTMenuItem"
        Me.CopySTOTMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.CopySTOTMenuItem.Text = "コピー（STOT形式テキスト）(&C)"
        '
        'CopyURLMenuItem
        '
        Me.CopyURLMenuItem.Name = "CopyURLMenuItem"
        Me.CopyURLMenuItem.Size = New System.Drawing.Size(218, 22)
        Me.CopyURLMenuItem.Text = "コピー（ステータスURL）(&S)"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(215, 6)
        '
        'MenuItemSubSearch
        '
        Me.MenuItemSubSearch.Name = "MenuItemSubSearch"
        Me.MenuItemSubSearch.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F), System.Windows.Forms.Keys)
        Me.MenuItemSubSearch.Size = New System.Drawing.Size(218, 22)
        Me.MenuItemSubSearch.Text = "検索(&F)"
        '
        'MenuItemSearchNext
        '
        Me.MenuItemSearchNext.Name = "MenuItemSearchNext"
        Me.MenuItemSearchNext.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.MenuItemSearchNext.Size = New System.Drawing.Size(218, 22)
        Me.MenuItemSearchNext.Text = "次を検索(&X)"
        '
        'MenuItemSearchPrev
        '
        Me.MenuItemSearchPrev.Name = "MenuItemSearchPrev"
        Me.MenuItemSearchPrev.ShortcutKeys = CType((System.Windows.Forms.Keys.Shift Or System.Windows.Forms.Keys.F3), System.Windows.Forms.Keys)
        Me.MenuItemSearchPrev.Size = New System.Drawing.Size(218, 22)
        Me.MenuItemSearchPrev.Text = "前を検索(&P)"
        '
        'MenuItemCommand
        '
        Me.MenuItemCommand.Enabled = False
        Me.MenuItemCommand.Name = "MenuItemCommand"
        Me.MenuItemCommand.Size = New System.Drawing.Size(70, 20)
        Me.MenuItemCommand.Text = "コマンド(&C)"
        '
        'MenuItemHelp
        '
        Me.MenuItemHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MatomeMenuItem, Me.ToolStripSeparator12, Me.OfficialMenuItem, Me.DLPageMenuItem, Me.ToolStripSeparator16, Me.VerUpMenuItem, Me.WedataMenuItem, Me.ToolStripSeparator14, Me.InfoTwitterMenuItem, Me.ToolStripSeparator7, Me.AboutMenuItem})
        Me.MenuItemHelp.Name = "MenuItemHelp"
        Me.MenuItemHelp.Size = New System.Drawing.Size(64, 20)
        Me.MenuItemHelp.Text = "ヘルプ(&H)"
        '
        'MatomeMenuItem
        '
        Me.MatomeMenuItem.Name = "MatomeMenuItem"
        Me.MatomeMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.MatomeMenuItem.Text = "Tweenまとめサイト(&H)"
        '
        'ToolStripSeparator12
        '
        Me.ToolStripSeparator12.Name = "ToolStripSeparator12"
        Me.ToolStripSeparator12.Size = New System.Drawing.Size(186, 6)
        '
        'OfficialMenuItem
        '
        Me.OfficialMenuItem.Name = "OfficialMenuItem"
        Me.OfficialMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.OfficialMenuItem.Text = "公式ページ(&O)"
        '
        'DLPageMenuItem
        '
        Me.DLPageMenuItem.Name = "DLPageMenuItem"
        Me.DLPageMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.DLPageMenuItem.Text = "配布ページ(&D)"
        '
        'ToolStripSeparator16
        '
        Me.ToolStripSeparator16.Name = "ToolStripSeparator16"
        Me.ToolStripSeparator16.Size = New System.Drawing.Size(186, 6)
        '
        'VerUpMenuItem
        '
        Me.VerUpMenuItem.Name = "VerUpMenuItem"
        Me.VerUpMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.VerUpMenuItem.Text = "最新版の取得(&G)"
        '
        'WedataMenuItem
        '
        Me.WedataMenuItem.Name = "WedataMenuItem"
        Me.WedataMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.WedataMenuItem.Text = "解析キー情報更新"
        '
        'ToolStripSeparator14
        '
        Me.ToolStripSeparator14.Name = "ToolStripSeparator14"
        Me.ToolStripSeparator14.Size = New System.Drawing.Size(186, 6)
        '
        'InfoTwitterMenuItem
        '
        Me.InfoTwitterMenuItem.Name = "InfoTwitterMenuItem"
        Me.InfoTwitterMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.InfoTwitterMenuItem.Text = "Twitterからのお知らせ(&I)"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(186, 6)
        '
        'AboutMenuItem
        '
        Me.AboutMenuItem.Name = "AboutMenuItem"
        Me.AboutMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.AboutMenuItem.Text = "Tweenについて(&A)"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLabelUrl, Me.StatusLabel})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 293)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(468, 23)
        Me.StatusStrip1.TabIndex = 10
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'StatusLabelUrl
        '
        Me.StatusLabelUrl.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right
        Me.StatusLabelUrl.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.StatusLabelUrl.Name = "StatusLabelUrl"
        Me.StatusLabelUrl.Size = New System.Drawing.Size(334, 18)
        Me.StatusLabelUrl.Spring = True
        Me.StatusLabelUrl.Text = "ToolStripStatusLabel1"
        Me.StatusLabelUrl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'StatusLabel
        '
        Me.StatusLabel.Name = "StatusLabel"
        Me.StatusLabel.Size = New System.Drawing.Size(119, 18)
        Me.StatusLabel.Text = "ToolStripStatusLabel1"
        '
        'TimerDM
        '
        Me.TimerDM.Interval = 600000
        '
        'GetLogWorker
        '
        '
        'ExecWorker
        '
        '
        'TimerColorize
        '
        '
        'TimerRefreshIcon
        '
        Me.TimerRefreshIcon.Interval = 50
        '
        'PostWorker
        '
        '
        'ClearTabMenuItem
        '
        Me.ClearTabMenuItem.Name = "ClearTabMenuItem"
        Me.ClearTabMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.ClearTabMenuItem.Text = "このタブの発言をクリア"
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(182, 6)
        '
        'TweenMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(468, 316)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.SplitContainer1)
        Me.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "TweenMain"
        Me.Text = "Tween"
        CType(Me.UserPicture, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.ListTab.ResumeLayout(False)
        Me.ContextMenuTabProperty.ResumeLayout(False)
        Me.TabRecent.ResumeLayout(False)
        Me.ContextMenuStrip2.ResumeLayout(False)
        Me.TabReply.ResumeLayout(False)
        Me.TabDirect.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PostButton As System.Windows.Forms.Button
    Friend WithEvents StatusText As System.Windows.Forms.TextBox
    Friend WithEvents TimerTimeline As System.Windows.Forms.Timer
    Friend WithEvents UserPicture As System.Windows.Forms.PictureBox
    Friend WithEvents NameLabel As System.Windows.Forms.Label
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents DateTimeLabel As System.Windows.Forms.Label
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents EndToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetTimelineWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents MenuItemFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents StatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ContextMenuStrip2 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents FavAddToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FavRemoveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MoveToHomeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MoveToFavToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ListTab As System.Windows.Forms.TabControl
    Friend WithEvents TabRecent As System.Windows.Forms.TabPage
    Friend WithEvents TabReply As System.Windows.Forms.TabPage
    Friend WithEvents TabDirect As System.Windows.Forms.TabPage
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ReplyStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DMStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents DeleteStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TimerDM As System.Windows.Forms.Timer
    Friend WithEvents ReadedStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UnreadStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents GetLogWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents MenuItemOperate As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents RefreshStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SettingStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator9 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents PostBrowser As System.Windows.Forms.WebBrowser
    Friend WithEvents StatusLabelUrl As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents TabImage As System.Windows.Forms.ImageList
    Friend WithEvents ExecWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents lblLen As System.Windows.Forms.Label
    Friend WithEvents NewPostPopMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemEdit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemSubSearch As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemSearchNext As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemSearchPrev As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ListLockMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents JumpUnreadMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusOpenMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FavorareMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Timeline As Tween.TweenCustomControl.DetailsListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Reply As Tween.TweenCustomControl.DetailsListView
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader10 As System.Windows.Forms.ColumnHeader
    Friend WithEvents DirectMsg As Tween.TweenCustomControl.DetailsListView
    Friend WithEvents ColumnHeader11 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader12 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader13 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader14 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader15 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ToolStripSeparator15 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TimerColorize As System.Windows.Forms.Timer
    Friend WithEvents MenuItemCommand As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TabMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents AboutMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents VerUpMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MatomeMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator12 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents OfficialMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DLPageMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator16 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator14 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SaveLogMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator17 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents TimerRefreshIcon As System.Windows.Forms.Timer
    Friend WithEvents ContextMenuTabProperty As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents UreadManageMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NotifyDispMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SoundFileComboBox As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents ToolStripSeparator18 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents DeleteTabMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FilterEditMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator19 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents AddTabMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator20 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MenuItemTab As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InfoTwitterMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ReplyAllStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PostWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents IDRuleMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CopySTOTMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CopyURLMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator10 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SelectAllMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WedataMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OpenURLMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ClearTabMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator11 As System.Windows.Forms.ToolStripSeparator

End Class
