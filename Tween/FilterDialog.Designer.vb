Option Strict On
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FilterDialog))
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
        Me.ButtonClose.AccessibleDescription = Nothing
        Me.ButtonClose.AccessibleName = Nothing
        resources.ApplyResources(Me.ButtonClose, "ButtonClose")
        Me.ButtonClose.BackgroundImage = Nothing
        Me.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonClose.Font = Nothing
        Me.ButtonClose.Name = "ButtonClose"
        Me.ButtonClose.UseVisualStyleBackColor = True
        '
        'ComboTabs
        '
        Me.ComboTabs.AccessibleDescription = Nothing
        Me.ComboTabs.AccessibleName = Nothing
        resources.ApplyResources(Me.ComboTabs, "ComboTabs")
        Me.ComboTabs.BackgroundImage = Nothing
        Me.ComboTabs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboTabs.Font = Nothing
        Me.ComboTabs.FormattingEnabled = True
        Me.ComboTabs.Name = "ComboTabs"
        '
        'Label5
        '
        Me.Label5.AccessibleDescription = Nothing
        Me.Label5.AccessibleName = Nothing
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Font = Nothing
        Me.Label5.Name = "Label5"
        '
        'ListFilters
        '
        Me.ListFilters.AccessibleDescription = Nothing
        Me.ListFilters.AccessibleName = Nothing
        resources.ApplyResources(Me.ListFilters, "ListFilters")
        Me.ListFilters.BackgroundImage = Nothing
        Me.ListFilters.Font = Nothing
        Me.ListFilters.FormattingEnabled = True
        Me.ListFilters.Name = "ListFilters"
        '
        'EditFilterGroup
        '
        Me.EditFilterGroup.AccessibleDescription = Nothing
        Me.EditFilterGroup.AccessibleName = Nothing
        resources.ApplyResources(Me.EditFilterGroup, "EditFilterGroup")
        Me.EditFilterGroup.BackgroundImage = Nothing
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
        Me.EditFilterGroup.Font = Nothing
        Me.EditFilterGroup.Name = "EditFilterGroup"
        Me.EditFilterGroup.TabStop = False
        '
        'GroupBox1
        '
        Me.GroupBox1.AccessibleDescription = Nothing
        Me.GroupBox1.AccessibleName = Nothing
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.BackgroundImage = Nothing
        Me.GroupBox1.Controls.Add(Me.OptNone)
        Me.GroupBox1.Controls.Add(Me.OptMark)
        Me.GroupBox1.Controls.Add(Me.OptMove)
        Me.GroupBox1.Font = Nothing
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        '
        'OptNone
        '
        Me.OptNone.AccessibleDescription = Nothing
        Me.OptNone.AccessibleName = Nothing
        resources.ApplyResources(Me.OptNone, "OptNone")
        Me.OptNone.BackgroundImage = Nothing
        Me.OptNone.Font = Nothing
        Me.OptNone.Name = "OptNone"
        Me.OptNone.TabStop = True
        Me.OptNone.UseVisualStyleBackColor = True
        '
        'OptMark
        '
        Me.OptMark.AccessibleDescription = Nothing
        Me.OptMark.AccessibleName = Nothing
        resources.ApplyResources(Me.OptMark, "OptMark")
        Me.OptMark.BackgroundImage = Nothing
        Me.OptMark.Font = Nothing
        Me.OptMark.Name = "OptMark"
        Me.OptMark.TabStop = True
        Me.OptMark.UseVisualStyleBackColor = True
        '
        'OptMove
        '
        Me.OptMove.AccessibleDescription = Nothing
        Me.OptMove.AccessibleName = Nothing
        resources.ApplyResources(Me.OptMove, "OptMove")
        Me.OptMove.BackgroundImage = Nothing
        Me.OptMove.Font = Nothing
        Me.OptMove.Name = "OptMove"
        Me.OptMove.TabStop = True
        Me.OptMove.UseVisualStyleBackColor = True
        '
        'CheckURL
        '
        Me.CheckURL.AccessibleDescription = Nothing
        Me.CheckURL.AccessibleName = Nothing
        resources.ApplyResources(Me.CheckURL, "CheckURL")
        Me.CheckURL.BackgroundImage = Nothing
        Me.CheckURL.Font = Nothing
        Me.CheckURL.Name = "CheckURL"
        Me.CheckURL.UseVisualStyleBackColor = True
        '
        'CheckRegex
        '
        Me.CheckRegex.AccessibleDescription = Nothing
        Me.CheckRegex.AccessibleName = Nothing
        resources.ApplyResources(Me.CheckRegex, "CheckRegex")
        Me.CheckRegex.BackgroundImage = Nothing
        Me.CheckRegex.Font = Nothing
        Me.CheckRegex.Name = "CheckRegex"
        Me.CheckRegex.UseVisualStyleBackColor = True
        '
        'RadioAND
        '
        Me.RadioAND.AccessibleDescription = Nothing
        Me.RadioAND.AccessibleName = Nothing
        resources.ApplyResources(Me.RadioAND, "RadioAND")
        Me.RadioAND.BackgroundImage = Nothing
        Me.RadioAND.Checked = True
        Me.RadioAND.Font = Nothing
        Me.RadioAND.Name = "RadioAND"
        Me.RadioAND.TabStop = True
        Me.RadioAND.UseVisualStyleBackColor = True
        '
        'ButtonCancel
        '
        Me.ButtonCancel.AccessibleDescription = Nothing
        Me.ButtonCancel.AccessibleName = Nothing
        resources.ApplyResources(Me.ButtonCancel, "ButtonCancel")
        Me.ButtonCancel.BackgroundImage = Nothing
        Me.ButtonCancel.Font = Nothing
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.UseVisualStyleBackColor = True
        '
        'UID
        '
        Me.UID.AccessibleDescription = Nothing
        Me.UID.AccessibleName = Nothing
        resources.ApplyResources(Me.UID, "UID")
        Me.UID.BackgroundImage = Nothing
        Me.UID.Font = Nothing
        Me.UID.Name = "UID"
        '
        'MSG2
        '
        Me.MSG2.AccessibleDescription = Nothing
        Me.MSG2.AccessibleName = Nothing
        resources.ApplyResources(Me.MSG2, "MSG2")
        Me.MSG2.BackgroundImage = Nothing
        Me.MSG2.Font = Nothing
        Me.MSG2.Name = "MSG2"
        '
        'MSG1
        '
        Me.MSG1.AccessibleDescription = Nothing
        Me.MSG1.AccessibleName = Nothing
        resources.ApplyResources(Me.MSG1, "MSG1")
        Me.MSG1.BackgroundImage = Nothing
        Me.MSG1.Font = Nothing
        Me.MSG1.Name = "MSG1"
        '
        'Label6
        '
        Me.Label6.AccessibleDescription = Nothing
        Me.Label6.AccessibleName = Nothing
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.Font = Nothing
        Me.Label6.Name = "Label6"
        '
        'Label7
        '
        Me.Label7.AccessibleDescription = Nothing
        Me.Label7.AccessibleName = Nothing
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.Font = Nothing
        Me.Label7.Name = "Label7"
        '
        'Label9
        '
        Me.Label9.AccessibleDescription = Nothing
        Me.Label9.AccessibleName = Nothing
        resources.ApplyResources(Me.Label9, "Label9")
        Me.Label9.Font = Nothing
        Me.Label9.Name = "Label9"
        '
        'RadioPLUS
        '
        Me.RadioPLUS.AccessibleDescription = Nothing
        Me.RadioPLUS.AccessibleName = Nothing
        resources.ApplyResources(Me.RadioPLUS, "RadioPLUS")
        Me.RadioPLUS.BackgroundImage = Nothing
        Me.RadioPLUS.Font = Nothing
        Me.RadioPLUS.Name = "RadioPLUS"
        Me.RadioPLUS.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AccessibleDescription = Nothing
        Me.Label8.AccessibleName = Nothing
        resources.ApplyResources(Me.Label8, "Label8")
        Me.Label8.Font = Nothing
        Me.Label8.Name = "Label8"
        '
        'ButtonOK
        '
        Me.ButtonOK.AccessibleDescription = Nothing
        Me.ButtonOK.AccessibleName = Nothing
        resources.ApplyResources(Me.ButtonOK, "ButtonOK")
        Me.ButtonOK.BackgroundImage = Nothing
        Me.ButtonOK.Font = Nothing
        Me.ButtonOK.Name = "ButtonOK"
        Me.ButtonOK.UseVisualStyleBackColor = True
        '
        'ButtonNew
        '
        Me.ButtonNew.AccessibleDescription = Nothing
        Me.ButtonNew.AccessibleName = Nothing
        resources.ApplyResources(Me.ButtonNew, "ButtonNew")
        Me.ButtonNew.BackgroundImage = Nothing
        Me.ButtonNew.Font = Nothing
        Me.ButtonNew.Name = "ButtonNew"
        Me.ButtonNew.UseVisualStyleBackColor = True
        '
        'ButtonDelete
        '
        Me.ButtonDelete.AccessibleDescription = Nothing
        Me.ButtonDelete.AccessibleName = Nothing
        resources.ApplyResources(Me.ButtonDelete, "ButtonDelete")
        Me.ButtonDelete.BackgroundImage = Nothing
        Me.ButtonDelete.Font = Nothing
        Me.ButtonDelete.Name = "ButtonDelete"
        Me.ButtonDelete.UseVisualStyleBackColor = True
        '
        'ButtonEdit
        '
        Me.ButtonEdit.AccessibleDescription = Nothing
        Me.ButtonEdit.AccessibleName = Nothing
        resources.ApplyResources(Me.ButtonEdit, "ButtonEdit")
        Me.ButtonEdit.BackgroundImage = Nothing
        Me.ButtonEdit.Font = Nothing
        Me.ButtonEdit.Name = "ButtonEdit"
        Me.ButtonEdit.UseVisualStyleBackColor = True
        '
        'FilterDialog
        '
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Nothing
        Me.CancelButton = Me.ButtonClose
        Me.ControlBox = False
        Me.Controls.Add(Me.ComboTabs)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.ListFilters)
        Me.Controls.Add(Me.EditFilterGroup)
        Me.Controls.Add(Me.ButtonNew)
        Me.Controls.Add(Me.ButtonDelete)
        Me.Controls.Add(Me.ButtonEdit)
        Me.Controls.Add(Me.ButtonClose)
        Me.Font = Nothing
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = Nothing
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FilterDialog"
        Me.ShowInTaskbar = False
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
