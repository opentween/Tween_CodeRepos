Option Strict On
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TweenAboutBox
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

    Friend WithEvents TableLayoutPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents LogoPictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents LabelProductName As System.Windows.Forms.Label
    Friend WithEvents LabelVersion As System.Windows.Forms.Label
    Friend WithEvents LabelCompanyName As System.Windows.Forms.Label
    Friend WithEvents TextBoxDescription As System.Windows.Forms.TextBox
    Friend WithEvents OKButton As System.Windows.Forms.Button
    Friend WithEvents LabelCopyright As System.Windows.Forms.Label

    'Windows フォーム デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使用して変更できます。  
    'コード エディタを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TweenAboutBox))
        Me.TableLayoutPanel = New System.Windows.Forms.TableLayoutPanel
        Me.LogoPictureBox = New System.Windows.Forms.PictureBox
        Me.LabelProductName = New System.Windows.Forms.Label
        Me.LabelVersion = New System.Windows.Forms.Label
        Me.LabelCopyright = New System.Windows.Forms.Label
        Me.LabelCompanyName = New System.Windows.Forms.Label
        Me.TextBoxDescription = New System.Windows.Forms.TextBox
        Me.OKButton = New System.Windows.Forms.Button
        Me.ChangeLog = New System.Windows.Forms.TextBox
        Me.TableLayoutPanel.SuspendLayout()
        CType(Me.LogoPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel
        '
        Me.TableLayoutPanel.AccessibleDescription = Nothing
        Me.TableLayoutPanel.AccessibleName = Nothing
        resources.ApplyResources(Me.TableLayoutPanel, "TableLayoutPanel")
        Me.TableLayoutPanel.BackgroundImage = Nothing
        Me.TableLayoutPanel.Controls.Add(Me.LogoPictureBox, 0, 0)
        Me.TableLayoutPanel.Controls.Add(Me.LabelProductName, 1, 0)
        Me.TableLayoutPanel.Controls.Add(Me.LabelVersion, 1, 1)
        Me.TableLayoutPanel.Controls.Add(Me.LabelCopyright, 1, 2)
        Me.TableLayoutPanel.Controls.Add(Me.LabelCompanyName, 1, 3)
        Me.TableLayoutPanel.Controls.Add(Me.TextBoxDescription, 1, 4)
        Me.TableLayoutPanel.Controls.Add(Me.OKButton, 1, 6)
        Me.TableLayoutPanel.Controls.Add(Me.ChangeLog, 0, 5)
        Me.TableLayoutPanel.Font = Nothing
        Me.TableLayoutPanel.Name = "TableLayoutPanel"
        '
        'LogoPictureBox
        '
        Me.LogoPictureBox.AccessibleDescription = Nothing
        Me.LogoPictureBox.AccessibleName = Nothing
        resources.ApplyResources(Me.LogoPictureBox, "LogoPictureBox")
        Me.LogoPictureBox.BackgroundImage = Nothing
        Me.LogoPictureBox.Font = Nothing
        Me.LogoPictureBox.ImageLocation = Nothing
        Me.LogoPictureBox.Name = "LogoPictureBox"
        Me.TableLayoutPanel.SetRowSpan(Me.LogoPictureBox, 5)
        Me.LogoPictureBox.TabStop = False
        '
        'LabelProductName
        '
        Me.LabelProductName.AccessibleDescription = Nothing
        Me.LabelProductName.AccessibleName = Nothing
        resources.ApplyResources(Me.LabelProductName, "LabelProductName")
        Me.LabelProductName.Font = Nothing
        Me.LabelProductName.MaximumSize = New System.Drawing.Size(0, 16)
        Me.LabelProductName.Name = "LabelProductName"
        '
        'LabelVersion
        '
        Me.LabelVersion.AccessibleDescription = Nothing
        Me.LabelVersion.AccessibleName = Nothing
        resources.ApplyResources(Me.LabelVersion, "LabelVersion")
        Me.LabelVersion.Font = Nothing
        Me.LabelVersion.MaximumSize = New System.Drawing.Size(0, 16)
        Me.LabelVersion.Name = "LabelVersion"
        '
        'LabelCopyright
        '
        Me.LabelCopyright.AccessibleDescription = Nothing
        Me.LabelCopyright.AccessibleName = Nothing
        resources.ApplyResources(Me.LabelCopyright, "LabelCopyright")
        Me.LabelCopyright.Font = Nothing
        Me.LabelCopyright.MaximumSize = New System.Drawing.Size(0, 16)
        Me.LabelCopyright.Name = "LabelCopyright"
        '
        'LabelCompanyName
        '
        Me.LabelCompanyName.AccessibleDescription = Nothing
        Me.LabelCompanyName.AccessibleName = Nothing
        resources.ApplyResources(Me.LabelCompanyName, "LabelCompanyName")
        Me.LabelCompanyName.Font = Nothing
        Me.LabelCompanyName.MaximumSize = New System.Drawing.Size(0, 16)
        Me.LabelCompanyName.Name = "LabelCompanyName"
        '
        'TextBoxDescription
        '
        Me.TextBoxDescription.AccessibleDescription = Nothing
        Me.TextBoxDescription.AccessibleName = Nothing
        resources.ApplyResources(Me.TextBoxDescription, "TextBoxDescription")
        Me.TextBoxDescription.BackgroundImage = Nothing
        Me.TextBoxDescription.Font = Nothing
        Me.TextBoxDescription.Name = "TextBoxDescription"
        Me.TextBoxDescription.ReadOnly = True
        Me.TextBoxDescription.TabStop = False
        '
        'OKButton
        '
        Me.OKButton.AccessibleDescription = Nothing
        Me.OKButton.AccessibleName = Nothing
        resources.ApplyResources(Me.OKButton, "OKButton")
        Me.OKButton.BackgroundImage = Nothing
        Me.OKButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.OKButton.Font = Nothing
        Me.OKButton.Name = "OKButton"
        '
        'ChangeLog
        '
        Me.ChangeLog.AccessibleDescription = Nothing
        Me.ChangeLog.AccessibleName = Nothing
        resources.ApplyResources(Me.ChangeLog, "ChangeLog")
        Me.ChangeLog.BackgroundImage = Nothing
        Me.TableLayoutPanel.SetColumnSpan(Me.ChangeLog, 2)
        Me.ChangeLog.Font = Nothing
        Me.ChangeLog.Name = "ChangeLog"
        Me.ChangeLog.ReadOnly = True
        '
        'TweenAboutBox
        '
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Nothing
        Me.CancelButton = Me.OKButton
        Me.Controls.Add(Me.TableLayoutPanel)
        Me.Font = Nothing
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = Nothing
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "TweenAboutBox"
        Me.ShowInTaskbar = False
        Me.TopMost = True
        Me.TableLayoutPanel.ResumeLayout(False)
        Me.TableLayoutPanel.PerformLayout()
        CType(Me.LogoPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ChangeLog As System.Windows.Forms.TextBox

End Class
