<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FilterDialog
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
        Me.ButtonClose = New System.Windows.Forms.Button
        Me.ComboTabs = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.ListFilters = New System.Windows.Forms.ListBox
        Me.EditFilterGroup = New System.Windows.Forms.GroupBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.OptNone = New System.Windows.Forms.RadioButton
        Me.OptMark = New System.Windows.Forms.RadioButton
        Me.OptMove = New System.Windows.Forms.RadioButton
        Me.CheckURL = New System.Windows.Forms.CheckBox
        Me.CheckRegex = New System.Windows.Forms.CheckBox
        Me.RadioAND = New System.Windows.Forms.RadioButton
        Me.ButtonCancel = New System.Windows.Forms.Button
        Me.UID = New System.Windows.Forms.TextBox
        Me.MSG2 = New System.Windows.Forms.TextBox
        Me.MSG1 = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.RadioPLUS = New System.Windows.Forms.RadioButton
        Me.Label8 = New System.Windows.Forms.Label
        Me.ButtonOK = New System.Windows.Forms.Button
        Me.ButtonNew = New System.Windows.Forms.Button
        Me.ButtonDelete = New System.Windows.Forms.Button
        Me.ButtonEdit = New System.Windows.Forms.Button
        Me.EditFilterGroup.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonClose
        '
        Me.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonClose.Location = New System.Drawing.Point(431, 445)
        Me.ButtonClose.Name = "ButtonClose"
        Me.ButtonClose.Size = New System.Drawing.Size(75, 23)
        Me.ButtonClose.TabIndex = 7
        Me.ButtonClose.Text = "閉じる(&C)"
        Me.ButtonClose.UseVisualStyleBackColor = True
        '
        'ComboTabs
        '
        Me.ComboTabs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboTabs.FormattingEnabled = True
        Me.ComboTabs.Location = New System.Drawing.Point(41, 6)
        Me.ComboTabs.Name = "ComboTabs"
        Me.ComboTabs.Size = New System.Drawing.Size(465, 20)
        Me.ComboTabs.TabIndex = 1
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 9)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(22, 12)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "タブ"
        '
        'ListFilters
        '
        Me.ListFilters.FormattingEnabled = True
        Me.ListFilters.ItemHeight = 12
        Me.ListFilters.Location = New System.Drawing.Point(14, 30)
        Me.ListFilters.Name = "ListFilters"
        Me.ListFilters.Size = New System.Drawing.Size(492, 232)
        Me.ListFilters.TabIndex = 2
        '
        'EditFilterGroup
        '
        Me.EditFilterGroup.Controls.Add(Me.GroupBox1)
        Me.EditFilterGroup.Controls.Add(Me.CheckURL)
        Me.EditFilterGroup.Controls.Add(Me.CheckRegex)
        Me.EditFilterGroup.Controls.Add(Me.RadioAND)
        Me.EditFilterGroup.Controls.Add(Me.ButtonCancel)
        Me.EditFilterGroup.Controls.Add(Me.UID)
        Me.EditFilterGroup.Controls.Add(Me.MSG2)
        Me.EditFilterGroup.Controls.Add(Me.MSG1)
        Me.EditFilterGroup.Controls.Add(Me.Label6)
        Me.EditFilterGroup.Controls.Add(Me.Label7)
        Me.EditFilterGroup.Controls.Add(Me.Label9)
        Me.EditFilterGroup.Controls.Add(Me.RadioPLUS)
        Me.EditFilterGroup.Controls.Add(Me.Label8)
        Me.EditFilterGroup.Controls.Add(Me.ButtonOK)
        Me.EditFilterGroup.Location = New System.Drawing.Point(6, 295)
        Me.EditFilterGroup.Name = "EditFilterGroup"
        Me.EditFilterGroup.Size = New System.Drawing.Size(500, 144)
        Me.EditFilterGroup.TabIndex = 6
        Me.EditFilterGroup.TabStop = False
        Me.EditFilterGroup.Text = "ルール編集"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.OptNone)
        Me.GroupBox1.Controls.Add(Me.OptMark)
        Me.GroupBox1.Controls.Add(Me.OptMove)
        Me.GroupBox1.Location = New System.Drawing.Point(343, 14)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(151, 87)
        Me.GroupBox1.TabIndex = 15
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "マッチ時の追加動作(&E)"
        '
        'OptNone
        '
        Me.OptNone.AutoSize = True
        Me.OptNone.Location = New System.Drawing.Point(15, 62)
        Me.OptNone.Name = "OptNone"
        Me.OptNone.Size = New System.Drawing.Size(73, 16)
        Me.OptNone.TabIndex = 17
        Me.OptNone.TabStop = True
        Me.OptNone.Text = "何もしない"
        Me.OptNone.UseVisualStyleBackColor = True
        '
        'OptMark
        '
        Me.OptMark.AutoSize = True
        Me.OptMark.Location = New System.Drawing.Point(15, 40)
        Me.OptMark.Name = "OptMark"
        Me.OptMark.Size = New System.Drawing.Size(119, 16)
        Me.OptMark.TabIndex = 16
        Me.OptMark.TabStop = True
        Me.OptMark.Text = "Recent発言にマーク"
        Me.OptMark.UseVisualStyleBackColor = True
        '
        'OptMove
        '
        Me.OptMove.AutoSize = True
        Me.OptMove.Location = New System.Drawing.Point(15, 18)
        Me.OptMove.Name = "OptMove"
        Me.OptMove.Size = New System.Drawing.Size(120, 16)
        Me.OptMove.TabIndex = 15
        Me.OptMove.TabStop = True
        Me.OptMove.Text = "Recentから移動する"
        Me.OptMove.UseVisualStyleBackColor = True
        '
        'CheckURL
        '
        Me.CheckURL.AutoSize = True
        Me.CheckURL.Location = New System.Drawing.Point(169, 94)
        Me.CheckURL.Name = "CheckURL"
        Me.CheckURL.Size = New System.Drawing.Size(150, 16)
        Me.CheckURL.TabIndex = 13
        Me.CheckURL.Text = "リンク先URLも検索する(&U)"
        Me.CheckURL.UseVisualStyleBackColor = True
        '
        'CheckRegex
        '
        Me.CheckRegex.AutoSize = True
        Me.CheckRegex.Location = New System.Drawing.Point(23, 94)
        Me.CheckRegex.Name = "CheckRegex"
        Me.CheckRegex.Size = New System.Drawing.Size(140, 16)
        Me.CheckRegex.TabIndex = 12
        Me.CheckRegex.Text = "正規表現を使用する(&R)"
        Me.CheckRegex.UseVisualStyleBackColor = True
        '
        'RadioAND
        '
        Me.RadioAND.AutoSize = True
        Me.RadioAND.Checked = True
        Me.RadioAND.Location = New System.Drawing.Point(8, 30)
        Me.RadioAND.Name = "RadioAND"
        Me.RadioAND.Size = New System.Drawing.Size(71, 16)
        Me.RadioAND.TabIndex = 0
        Me.RadioAND.TabStop = True
        Me.RadioAND.Text = "複合条件"
        Me.RadioAND.UseVisualStyleBackColor = True
        '
        'ButtonCancel
        '
        Me.ButtonCancel.Location = New System.Drawing.Point(419, 115)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(75, 23)
        Me.ButtonCancel.TabIndex = 11
        Me.ButtonCancel.Text = "キャンセル"
        Me.ButtonCancel.UseVisualStyleBackColor = True
        '
        'UID
        '
        Me.UID.Location = New System.Drawing.Point(89, 28)
        Me.UID.Name = "UID"
        Me.UID.Size = New System.Drawing.Size(72, 19)
        Me.UID.TabIndex = 2
        '
        'MSG2
        '
        Me.MSG2.Location = New System.Drawing.Point(89, 66)
        Me.MSG2.Name = "MSG2"
        Me.MSG2.Size = New System.Drawing.Size(234, 19)
        Me.MSG2.TabIndex = 8
        '
        'MSG1
        '
        Me.MSG1.Location = New System.Drawing.Point(202, 28)
        Me.MSG1.Name = "MSG1"
        Me.MSG1.Size = New System.Drawing.Size(121, 19)
        Me.MSG1.TabIndex = 5
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(87, 16)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(56, 12)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "ユーザーID"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(167, 31)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(29, 12)
        Me.Label7.TabIndex = 3
        Me.Label7.Text = "AND"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(87, 53)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(114, 12)
        Me.Label9.TabIndex = 7
        Me.Label9.Text = "ユーザーIDか発言内容"
        '
        'RadioPLUS
        '
        Me.RadioPLUS.AutoSize = True
        Me.RadioPLUS.Location = New System.Drawing.Point(8, 67)
        Me.RadioPLUS.Name = "RadioPLUS"
        Me.RadioPLUS.Size = New System.Drawing.Size(71, 16)
        Me.RadioPLUS.TabIndex = 6
        Me.RadioPLUS.Text = "単一条件"
        Me.RadioPLUS.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(200, 16)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(53, 12)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "発言内容"
        '
        'ButtonOK
        '
        Me.ButtonOK.Location = New System.Drawing.Point(339, 115)
        Me.ButtonOK.Name = "ButtonOK"
        Me.ButtonOK.Size = New System.Drawing.Size(75, 23)
        Me.ButtonOK.TabIndex = 10
        Me.ButtonOK.Text = "OK"
        Me.ButtonOK.UseVisualStyleBackColor = True
        '
        'ButtonNew
        '
        Me.ButtonNew.Location = New System.Drawing.Point(14, 266)
        Me.ButtonNew.Name = "ButtonNew"
        Me.ButtonNew.Size = New System.Drawing.Size(75, 23)
        Me.ButtonNew.TabIndex = 3
        Me.ButtonNew.Text = "新規（&N)"
        Me.ButtonNew.UseVisualStyleBackColor = True
        '
        'ButtonDelete
        '
        Me.ButtonDelete.Location = New System.Drawing.Point(430, 266)
        Me.ButtonDelete.Name = "ButtonDelete"
        Me.ButtonDelete.Size = New System.Drawing.Size(75, 23)
        Me.ButtonDelete.TabIndex = 5
        Me.ButtonDelete.Text = "削除(&D)"
        Me.ButtonDelete.UseVisualStyleBackColor = True
        '
        'ButtonEdit
        '
        Me.ButtonEdit.Location = New System.Drawing.Point(222, 266)
        Me.ButtonEdit.Name = "ButtonEdit"
        Me.ButtonEdit.Size = New System.Drawing.Size(75, 23)
        Me.ButtonEdit.TabIndex = 4
        Me.ButtonEdit.Text = "編集(&E)"
        Me.ButtonEdit.UseVisualStyleBackColor = True
        '
        'FilterDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.ButtonClose
        Me.ClientSize = New System.Drawing.Size(518, 472)
        Me.ControlBox = False
        Me.Controls.Add(Me.ComboTabs)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.ListFilters)
        Me.Controls.Add(Me.EditFilterGroup)
        Me.Controls.Add(Me.ButtonNew)
        Me.Controls.Add(Me.ButtonDelete)
        Me.Controls.Add(Me.ButtonEdit)
        Me.Controls.Add(Me.ButtonClose)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FilterDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "振り分けルール"
        Me.TopMost = True
        Me.EditFilterGroup.ResumeLayout(False)
        Me.EditFilterGroup.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ButtonClose As System.Windows.Forms.Button
    Friend WithEvents ComboTabs As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ListFilters As System.Windows.Forms.ListBox
    Friend WithEvents EditFilterGroup As System.Windows.Forms.GroupBox
    Friend WithEvents RadioPLUS As System.Windows.Forms.RadioButton
    Friend WithEvents RadioAND As System.Windows.Forms.RadioButton
    Friend WithEvents MSG2 As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents MSG1 As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents ButtonCancel As System.Windows.Forms.Button
    Friend WithEvents ButtonOK As System.Windows.Forms.Button
    Friend WithEvents UID As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ButtonNew As System.Windows.Forms.Button
    Friend WithEvents ButtonDelete As System.Windows.Forms.Button
    Friend WithEvents ButtonEdit As System.Windows.Forms.Button
    Friend WithEvents CheckURL As System.Windows.Forms.CheckBox
    Friend WithEvents CheckRegex As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents OptMove As System.Windows.Forms.RadioButton
    Friend WithEvents OptNone As System.Windows.Forms.RadioButton
    Friend WithEvents OptMark As System.Windows.Forms.RadioButton

End Class
