﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ShowUserInfo
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

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ShowUserInfo))
        Me.ButtonClose = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.LinkLabelWeb = New System.Windows.Forms.LinkLabel()
        Me.LabelLocation = New System.Windows.Forms.Label()
        Me.LabelName = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.LinkLabelFollowing = New System.Windows.Forms.LinkLabel()
        Me.LinkLabelFollowers = New System.Windows.Forms.LinkLabel()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.LabelCreatedAt = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.LinkLabelTweet = New System.Windows.Forms.LinkLabel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.LinkLabelFav = New System.Windows.Forms.LinkLabel()
        Me.ButtonFollow = New System.Windows.Forms.Button()
        Me.ButtonUnFollow = New System.Windows.Forms.Button()
        Me.LabelIsProtected = New System.Windows.Forms.Label()
        Me.LabelIsFollowing = New System.Windows.Forms.Label()
        Me.LabelIsFollowed = New System.Windows.Forms.Label()
        Me.UserPicture = New System.Windows.Forms.PictureBox()
        Me.ContextMenuUserPicture = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ChangeIconToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BackgroundWorkerImageLoader = New System.ComponentModel.BackgroundWorker()
        Me.LabelScreenName = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.ContextMenuRecentPostBrowser = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SelectionCopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelectAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LabelRecentPost = New System.Windows.Forms.Label()
        Me.LabelIsVerified = New System.Windows.Forms.Label()
        Me.ButtonSearchPosts = New System.Windows.Forms.Button()
        Me.LabelId = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.ButtonEdit = New System.Windows.Forms.Button()
        Me.RecentPostBrowser = New System.Windows.Forms.WebBrowser()
        Me.DescriptionBrowser = New System.Windows.Forms.WebBrowser()
        Me.OpenFileDialogIcon = New System.Windows.Forms.OpenFileDialog()
        Me.TextBoxName = New System.Windows.Forms.TextBox()
        Me.TextBoxLocation = New System.Windows.Forms.TextBox()
        Me.TextBoxWeb = New System.Windows.Forms.TextBox()
        Me.TextBoxDescription = New System.Windows.Forms.TextBox()
        Me.ButtonBlock = New System.Windows.Forms.Button()
        Me.ButtonReportSpam = New System.Windows.Forms.Button()
        Me.ButtonBlockDestroy = New System.Windows.Forms.Button()
        Me.LinkLabel2 = New System.Windows.Forms.LinkLabel()
        CType(Me.UserPicture, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuUserPicture.SuspendLayout()
        Me.ContextMenuRecentPostBrowser.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonClose
        '
        resources.ApplyResources(Me.ButtonClose, "ButtonClose")
        Me.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonClose.Name = "ButtonClose"
        Me.ToolTip1.SetToolTip(Me.ButtonClose, resources.GetString("ButtonClose.ToolTip"))
        Me.ButtonClose.UseVisualStyleBackColor = True
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        Me.ToolTip1.SetToolTip(Me.Label1, resources.GetString("Label1.ToolTip"))
        Me.Label1.UseMnemonic = False
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        Me.ToolTip1.SetToolTip(Me.Label2, resources.GetString("Label2.ToolTip"))
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        Me.ToolTip1.SetToolTip(Me.Label3, resources.GetString("Label3.ToolTip"))
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        Me.ToolTip1.SetToolTip(Me.Label4, resources.GetString("Label4.ToolTip"))
        '
        'LinkLabelWeb
        '
        resources.ApplyResources(Me.LinkLabelWeb, "LinkLabelWeb")
        Me.LinkLabelWeb.AutoEllipsis = True
        Me.LinkLabelWeb.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LinkLabelWeb.Name = "LinkLabelWeb"
        Me.LinkLabelWeb.TabStop = True
        Me.ToolTip1.SetToolTip(Me.LinkLabelWeb, resources.GetString("LinkLabelWeb.ToolTip"))
        Me.LinkLabelWeb.UseMnemonic = False
        '
        'LabelLocation
        '
        resources.ApplyResources(Me.LabelLocation, "LabelLocation")
        Me.LabelLocation.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelLocation.Name = "LabelLocation"
        Me.ToolTip1.SetToolTip(Me.LabelLocation, resources.GetString("LabelLocation.ToolTip"))
        Me.LabelLocation.UseMnemonic = False
        '
        'LabelName
        '
        resources.ApplyResources(Me.LabelName, "LabelName")
        Me.LabelName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelName.Name = "LabelName"
        Me.ToolTip1.SetToolTip(Me.LabelName, resources.GetString("LabelName.ToolTip"))
        Me.LabelName.UseMnemonic = False
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        Me.ToolTip1.SetToolTip(Me.Label5, resources.GetString("Label5.ToolTip"))
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.Name = "Label6"
        Me.ToolTip1.SetToolTip(Me.Label6, resources.GetString("Label6.ToolTip"))
        '
        'LinkLabelFollowing
        '
        resources.ApplyResources(Me.LinkLabelFollowing, "LinkLabelFollowing")
        Me.LinkLabelFollowing.Name = "LinkLabelFollowing"
        Me.LinkLabelFollowing.TabStop = True
        Me.ToolTip1.SetToolTip(Me.LinkLabelFollowing, resources.GetString("LinkLabelFollowing.ToolTip"))
        '
        'LinkLabelFollowers
        '
        resources.ApplyResources(Me.LinkLabelFollowers, "LinkLabelFollowers")
        Me.LinkLabelFollowers.Name = "LinkLabelFollowers"
        Me.LinkLabelFollowers.TabStop = True
        Me.ToolTip1.SetToolTip(Me.LinkLabelFollowers, resources.GetString("LinkLabelFollowers.ToolTip"))
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.Name = "Label7"
        Me.ToolTip1.SetToolTip(Me.Label7, resources.GetString("Label7.ToolTip"))
        '
        'LabelCreatedAt
        '
        resources.ApplyResources(Me.LabelCreatedAt, "LabelCreatedAt")
        Me.LabelCreatedAt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelCreatedAt.Name = "LabelCreatedAt"
        Me.ToolTip1.SetToolTip(Me.LabelCreatedAt, resources.GetString("LabelCreatedAt.ToolTip"))
        '
        'Label8
        '
        resources.ApplyResources(Me.Label8, "Label8")
        Me.Label8.Name = "Label8"
        Me.ToolTip1.SetToolTip(Me.Label8, resources.GetString("Label8.ToolTip"))
        '
        'LinkLabelTweet
        '
        resources.ApplyResources(Me.LinkLabelTweet, "LinkLabelTweet")
        Me.LinkLabelTweet.Name = "LinkLabelTweet"
        Me.LinkLabelTweet.TabStop = True
        Me.ToolTip1.SetToolTip(Me.LinkLabelTweet, resources.GetString("LinkLabelTweet.ToolTip"))
        '
        'Label9
        '
        resources.ApplyResources(Me.Label9, "Label9")
        Me.Label9.Name = "Label9"
        Me.ToolTip1.SetToolTip(Me.Label9, resources.GetString("Label9.ToolTip"))
        '
        'LinkLabelFav
        '
        resources.ApplyResources(Me.LinkLabelFav, "LinkLabelFav")
        Me.LinkLabelFav.Name = "LinkLabelFav"
        Me.LinkLabelFav.TabStop = True
        Me.ToolTip1.SetToolTip(Me.LinkLabelFav, resources.GetString("LinkLabelFav.ToolTip"))
        '
        'ButtonFollow
        '
        resources.ApplyResources(Me.ButtonFollow, "ButtonFollow")
        Me.ButtonFollow.Name = "ButtonFollow"
        Me.ToolTip1.SetToolTip(Me.ButtonFollow, resources.GetString("ButtonFollow.ToolTip"))
        Me.ButtonFollow.UseVisualStyleBackColor = True
        '
        'ButtonUnFollow
        '
        resources.ApplyResources(Me.ButtonUnFollow, "ButtonUnFollow")
        Me.ButtonUnFollow.Name = "ButtonUnFollow"
        Me.ToolTip1.SetToolTip(Me.ButtonUnFollow, resources.GetString("ButtonUnFollow.ToolTip"))
        Me.ButtonUnFollow.UseVisualStyleBackColor = True
        '
        'LabelIsProtected
        '
        resources.ApplyResources(Me.LabelIsProtected, "LabelIsProtected")
        Me.LabelIsProtected.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelIsProtected.Name = "LabelIsProtected"
        Me.ToolTip1.SetToolTip(Me.LabelIsProtected, resources.GetString("LabelIsProtected.ToolTip"))
        '
        'LabelIsFollowing
        '
        resources.ApplyResources(Me.LabelIsFollowing, "LabelIsFollowing")
        Me.LabelIsFollowing.Name = "LabelIsFollowing"
        Me.ToolTip1.SetToolTip(Me.LabelIsFollowing, resources.GetString("LabelIsFollowing.ToolTip"))
        '
        'LabelIsFollowed
        '
        resources.ApplyResources(Me.LabelIsFollowed, "LabelIsFollowed")
        Me.LabelIsFollowed.Name = "LabelIsFollowed"
        Me.ToolTip1.SetToolTip(Me.LabelIsFollowed, resources.GetString("LabelIsFollowed.ToolTip"))
        '
        'UserPicture
        '
        resources.ApplyResources(Me.UserPicture, "UserPicture")
        Me.UserPicture.BackColor = System.Drawing.Color.White
        Me.UserPicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.UserPicture.ContextMenuStrip = Me.ContextMenuUserPicture
        Me.UserPicture.Name = "UserPicture"
        Me.UserPicture.TabStop = False
        Me.ToolTip1.SetToolTip(Me.UserPicture, resources.GetString("UserPicture.ToolTip"))
        '
        'ContextMenuUserPicture
        '
        resources.ApplyResources(Me.ContextMenuUserPicture, "ContextMenuUserPicture")
        Me.ContextMenuUserPicture.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ChangeIconToolStripMenuItem})
        Me.ContextMenuUserPicture.Name = "ContextMenuStrip2"
        Me.ToolTip1.SetToolTip(Me.ContextMenuUserPicture, resources.GetString("ContextMenuUserPicture.ToolTip"))
        '
        'ChangeIconToolStripMenuItem
        '
        resources.ApplyResources(Me.ChangeIconToolStripMenuItem, "ChangeIconToolStripMenuItem")
        Me.ChangeIconToolStripMenuItem.Name = "ChangeIconToolStripMenuItem"
        '
        'BackgroundWorkerImageLoader
        '
        '
        'LabelScreenName
        '
        resources.ApplyResources(Me.LabelScreenName, "LabelScreenName")
        Me.LabelScreenName.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.LabelScreenName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LabelScreenName.Name = "LabelScreenName"
        Me.ToolTip1.SetToolTip(Me.LabelScreenName, resources.GetString("LabelScreenName.ToolTip"))
        '
        'ToolTip1
        '
        Me.ToolTip1.ShowAlways = True
        '
        'LinkLabel1
        '
        resources.ApplyResources(Me.LinkLabel1, "LinkLabel1")
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.TabStop = True
        Me.ToolTip1.SetToolTip(Me.LinkLabel1, resources.GetString("LinkLabel1.ToolTip"))
        '
        'ContextMenuRecentPostBrowser
        '
        resources.ApplyResources(Me.ContextMenuRecentPostBrowser, "ContextMenuRecentPostBrowser")
        Me.ContextMenuRecentPostBrowser.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SelectionCopyToolStripMenuItem, Me.SelectAllToolStripMenuItem})
        Me.ContextMenuRecentPostBrowser.Name = "ContextMenuStrip1"
        Me.ToolTip1.SetToolTip(Me.ContextMenuRecentPostBrowser, resources.GetString("ContextMenuRecentPostBrowser.ToolTip"))
        '
        'SelectionCopyToolStripMenuItem
        '
        resources.ApplyResources(Me.SelectionCopyToolStripMenuItem, "SelectionCopyToolStripMenuItem")
        Me.SelectionCopyToolStripMenuItem.Name = "SelectionCopyToolStripMenuItem"
        '
        'SelectAllToolStripMenuItem
        '
        resources.ApplyResources(Me.SelectAllToolStripMenuItem, "SelectAllToolStripMenuItem")
        Me.SelectAllToolStripMenuItem.Name = "SelectAllToolStripMenuItem"
        '
        'LabelRecentPost
        '
        resources.ApplyResources(Me.LabelRecentPost, "LabelRecentPost")
        Me.LabelRecentPost.Name = "LabelRecentPost"
        Me.ToolTip1.SetToolTip(Me.LabelRecentPost, resources.GetString("LabelRecentPost.ToolTip"))
        '
        'LabelIsVerified
        '
        resources.ApplyResources(Me.LabelIsVerified, "LabelIsVerified")
        Me.LabelIsVerified.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelIsVerified.Name = "LabelIsVerified"
        Me.ToolTip1.SetToolTip(Me.LabelIsVerified, resources.GetString("LabelIsVerified.ToolTip"))
        '
        'ButtonSearchPosts
        '
        resources.ApplyResources(Me.ButtonSearchPosts, "ButtonSearchPosts")
        Me.ButtonSearchPosts.Name = "ButtonSearchPosts"
        Me.ToolTip1.SetToolTip(Me.ButtonSearchPosts, resources.GetString("ButtonSearchPosts.ToolTip"))
        Me.ButtonSearchPosts.UseVisualStyleBackColor = True
        '
        'LabelId
        '
        resources.ApplyResources(Me.LabelId, "LabelId")
        Me.LabelId.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelId.Name = "LabelId"
        Me.ToolTip1.SetToolTip(Me.LabelId, resources.GetString("LabelId.ToolTip"))
        '
        'Label12
        '
        resources.ApplyResources(Me.Label12, "Label12")
        Me.Label12.Name = "Label12"
        Me.ToolTip1.SetToolTip(Me.Label12, resources.GetString("Label12.ToolTip"))
        '
        'ButtonEdit
        '
        resources.ApplyResources(Me.ButtonEdit, "ButtonEdit")
        Me.ButtonEdit.Name = "ButtonEdit"
        Me.ToolTip1.SetToolTip(Me.ButtonEdit, resources.GetString("ButtonEdit.ToolTip"))
        Me.ButtonEdit.UseVisualStyleBackColor = True
        '
        'RecentPostBrowser
        '
        resources.ApplyResources(Me.RecentPostBrowser, "RecentPostBrowser")
        Me.RecentPostBrowser.AllowWebBrowserDrop = False
        Me.RecentPostBrowser.ContextMenuStrip = Me.ContextMenuRecentPostBrowser
        Me.RecentPostBrowser.IsWebBrowserContextMenuEnabled = False
        Me.RecentPostBrowser.MinimumSize = New System.Drawing.Size(20, 20)
        Me.RecentPostBrowser.Name = "RecentPostBrowser"
        Me.RecentPostBrowser.TabStop = False
        Me.ToolTip1.SetToolTip(Me.RecentPostBrowser, resources.GetString("RecentPostBrowser.ToolTip"))
        Me.RecentPostBrowser.Url = New System.Uri("about:blank", System.UriKind.Absolute)
        Me.RecentPostBrowser.WebBrowserShortcutsEnabled = False
        '
        'DescriptionBrowser
        '
        resources.ApplyResources(Me.DescriptionBrowser, "DescriptionBrowser")
        Me.DescriptionBrowser.AllowWebBrowserDrop = False
        Me.DescriptionBrowser.ContextMenuStrip = Me.ContextMenuRecentPostBrowser
        Me.DescriptionBrowser.IsWebBrowserContextMenuEnabled = False
        Me.DescriptionBrowser.MinimumSize = New System.Drawing.Size(20, 20)
        Me.DescriptionBrowser.Name = "DescriptionBrowser"
        Me.DescriptionBrowser.TabStop = False
        Me.ToolTip1.SetToolTip(Me.DescriptionBrowser, resources.GetString("DescriptionBrowser.ToolTip"))
        Me.DescriptionBrowser.Url = New System.Uri("about:blank", System.UriKind.Absolute)
        Me.DescriptionBrowser.WebBrowserShortcutsEnabled = False
        '
        'OpenFileDialogIcon
        '
        Me.OpenFileDialogIcon.FileName = "OpenFileDialog1"
        resources.ApplyResources(Me.OpenFileDialogIcon, "OpenFileDialogIcon")
        '
        'TextBoxName
        '
        resources.ApplyResources(Me.TextBoxName, "TextBoxName")
        Me.TextBoxName.Name = "TextBoxName"
        Me.TextBoxName.TabStop = False
        Me.ToolTip1.SetToolTip(Me.TextBoxName, resources.GetString("TextBoxName.ToolTip"))
        '
        'TextBoxLocation
        '
        resources.ApplyResources(Me.TextBoxLocation, "TextBoxLocation")
        Me.TextBoxLocation.Name = "TextBoxLocation"
        Me.TextBoxLocation.TabStop = False
        Me.ToolTip1.SetToolTip(Me.TextBoxLocation, resources.GetString("TextBoxLocation.ToolTip"))
        '
        'TextBoxWeb
        '
        resources.ApplyResources(Me.TextBoxWeb, "TextBoxWeb")
        Me.TextBoxWeb.Name = "TextBoxWeb"
        Me.TextBoxWeb.TabStop = False
        Me.ToolTip1.SetToolTip(Me.TextBoxWeb, resources.GetString("TextBoxWeb.ToolTip"))
        '
        'TextBoxDescription
        '
        resources.ApplyResources(Me.TextBoxDescription, "TextBoxDescription")
        Me.TextBoxDescription.Name = "TextBoxDescription"
        Me.TextBoxDescription.TabStop = False
        Me.ToolTip1.SetToolTip(Me.TextBoxDescription, resources.GetString("TextBoxDescription.ToolTip"))
        '
        'ButtonBlock
        '
        resources.ApplyResources(Me.ButtonBlock, "ButtonBlock")
        Me.ButtonBlock.Name = "ButtonBlock"
        Me.ToolTip1.SetToolTip(Me.ButtonBlock, resources.GetString("ButtonBlock.ToolTip"))
        Me.ButtonBlock.UseVisualStyleBackColor = True
        '
        'ButtonReportSpam
        '
        resources.ApplyResources(Me.ButtonReportSpam, "ButtonReportSpam")
        Me.ButtonReportSpam.Name = "ButtonReportSpam"
        Me.ToolTip1.SetToolTip(Me.ButtonReportSpam, resources.GetString("ButtonReportSpam.ToolTip"))
        Me.ButtonReportSpam.UseVisualStyleBackColor = True
        '
        'ButtonBlockDestroy
        '
        resources.ApplyResources(Me.ButtonBlockDestroy, "ButtonBlockDestroy")
        Me.ButtonBlockDestroy.Name = "ButtonBlockDestroy"
        Me.ToolTip1.SetToolTip(Me.ButtonBlockDestroy, resources.GetString("ButtonBlockDestroy.ToolTip"))
        Me.ButtonBlockDestroy.UseVisualStyleBackColor = True
        '
        'LinkLabel2
        '
        resources.ApplyResources(Me.LinkLabel2, "LinkLabel2")
        Me.LinkLabel2.Name = "LinkLabel2"
        Me.LinkLabel2.TabStop = True
        Me.ToolTip1.SetToolTip(Me.LinkLabel2, resources.GetString("LinkLabel2.ToolTip"))
        '
        'ShowUserInfo
        '
        resources.ApplyResources(Me, "$this")
        Me.AllowDrop = True
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.ButtonClose
        Me.Controls.Add(Me.LinkLabel2)
        Me.Controls.Add(Me.ButtonBlockDestroy)
        Me.Controls.Add(Me.ButtonReportSpam)
        Me.Controls.Add(Me.ButtonBlock)
        Me.Controls.Add(Me.TextBoxDescription)
        Me.Controls.Add(Me.TextBoxWeb)
        Me.Controls.Add(Me.ButtonEdit)
        Me.Controls.Add(Me.LabelId)
        Me.Controls.Add(Me.TextBoxLocation)
        Me.Controls.Add(Me.TextBoxName)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.ButtonSearchPosts)
        Me.Controls.Add(Me.LinkLabel1)
        Me.Controls.Add(Me.RecentPostBrowser)
        Me.Controls.Add(Me.UserPicture)
        Me.Controls.Add(Me.LabelIsVerified)
        Me.Controls.Add(Me.DescriptionBrowser)
        Me.Controls.Add(Me.LabelScreenName)
        Me.Controls.Add(Me.LabelRecentPost)
        Me.Controls.Add(Me.LinkLabelFav)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.LabelIsProtected)
        Me.Controls.Add(Me.LabelCreatedAt)
        Me.Controls.Add(Me.LinkLabelTweet)
        Me.Controls.Add(Me.LabelIsFollowed)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.LabelIsFollowing)
        Me.Controls.Add(Me.LinkLabelFollowers)
        Me.Controls.Add(Me.ButtonUnFollow)
        Me.Controls.Add(Me.LinkLabelFollowing)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.LabelName)
        Me.Controls.Add(Me.ButtonFollow)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.LabelLocation)
        Me.Controls.Add(Me.LinkLabelWeb)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ButtonClose)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ShowUserInfo"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.ToolTip1.SetToolTip(Me, resources.GetString("$this.ToolTip"))
        Me.TopMost = True
        CType(Me.UserPicture, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuUserPicture.ResumeLayout(False)
        Me.ContextMenuRecentPostBrowser.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ButtonClose As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents LinkLabelWeb As System.Windows.Forms.LinkLabel
    Friend WithEvents LabelLocation As System.Windows.Forms.Label
    Friend WithEvents LabelName As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents LinkLabelFollowing As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabelFollowers As System.Windows.Forms.LinkLabel
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents LabelCreatedAt As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents LinkLabelTweet As System.Windows.Forms.LinkLabel
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents LinkLabelFav As System.Windows.Forms.LinkLabel
    Friend WithEvents ButtonFollow As System.Windows.Forms.Button
    Friend WithEvents ButtonUnFollow As System.Windows.Forms.Button
    Friend WithEvents LabelIsProtected As System.Windows.Forms.Label
    Friend WithEvents LabelIsFollowing As System.Windows.Forms.Label
    Friend WithEvents LabelIsFollowed As System.Windows.Forms.Label
    Friend WithEvents UserPicture As System.Windows.Forms.PictureBox
    Friend WithEvents BackgroundWorkerImageLoader As System.ComponentModel.BackgroundWorker
    Friend WithEvents LabelScreenName As System.Windows.Forms.Label
    Friend WithEvents DescriptionBrowser As System.Windows.Forms.WebBrowser
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents ContextMenuRecentPostBrowser As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SelectionCopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SelectAllToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LabelRecentPost As System.Windows.Forms.Label
    Friend WithEvents RecentPostBrowser As System.Windows.Forms.WebBrowser
    Friend WithEvents LabelIsVerified As System.Windows.Forms.Label
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents ButtonSearchPosts As System.Windows.Forms.Button
    Friend WithEvents LabelId As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents ButtonEdit As System.Windows.Forms.Button
    Friend WithEvents ContextMenuUserPicture As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ChangeIconToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenFileDialogIcon As System.Windows.Forms.OpenFileDialog
    Friend WithEvents TextBoxName As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxLocation As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxWeb As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxDescription As System.Windows.Forms.TextBox
    Friend WithEvents ButtonBlock As System.Windows.Forms.Button
    Friend WithEvents ButtonReportSpam As System.Windows.Forms.Button
    Friend WithEvents ButtonBlockDestroy As System.Windows.Forms.Button
    Friend WithEvents LinkLabel2 As System.Windows.Forms.LinkLabel
End Class
