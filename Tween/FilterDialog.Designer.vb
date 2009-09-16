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
        resources.ApplyResources(Me.ButtonClose, "ButtonClose")
        Me.ButtonClose.Name = "ButtonClose"
        Me.ButtonClose.UseVisualStyleBackColor = True
        '
        'ComboTabs
        '
        Me.ComboTabs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboTabs.FormattingEnabled = True
        resources.ApplyResources(Me.ComboTabs, "ComboTabs")
        Me.ComboTabs.Name = "ComboTabs"
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        '
        'ListFilters
        '
        Me.ListFilters.FormattingEnabled = True
        resources.ApplyResources(Me.ListFilters, "ListFilters")
        Me.ListFilters.Name = "ListFilters"
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
        resources.ApplyResources(Me.EditFilterGroup, "EditFilterGroup")
        Me.EditFilterGroup.Name = "EditFilterGroup"
        Me.EditFilterGroup.TabStop = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.OptMark)
        Me.GroupBox1.Controls.Add(Me.OptMove)
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        '
        'OptMark
        '
        resources.ApplyResources(Me.OptMark, "OptMark")
        Me.OptMark.Name = "OptMark"
        Me.OptMark.TabStop = True
        Me.OptMark.UseVisualStyleBackColor = True
        '
        'OptMove
        '
        resources.ApplyResources(Me.OptMove, "OptMove")
        Me.OptMove.Name = "OptMove"
        Me.OptMove.TabStop = True
        Me.OptMove.UseVisualStyleBackColor = True
        '
        'CheckURL
        '
        resources.ApplyResources(Me.CheckURL, "CheckURL")
        Me.CheckURL.Name = "CheckURL"
        Me.CheckURL.UseVisualStyleBackColor = True
        '
        'CheckRegex
        '
        resources.ApplyResources(Me.CheckRegex, "CheckRegex")
        Me.CheckRegex.Name = "CheckRegex"
        Me.CheckRegex.UseVisualStyleBackColor = True
        '
        'RadioAND
        '
        resources.ApplyResources(Me.RadioAND, "RadioAND")
        Me.RadioAND.Checked = True
        Me.RadioAND.Name = "RadioAND"
        Me.RadioAND.TabStop = True
        Me.RadioAND.UseVisualStyleBackColor = True
        '
        'ButtonCancel
        '
        resources.ApplyResources(Me.ButtonCancel, "ButtonCancel")
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.UseVisualStyleBackColor = True
        '
        'UID
        '
        resources.ApplyResources(Me.UID, "UID")
        Me.UID.Name = "UID"
        '
        'MSG2
        '
        resources.ApplyResources(Me.MSG2, "MSG2")
        Me.MSG2.Name = "MSG2"
        '
        'MSG1
        '
        resources.ApplyResources(Me.MSG1, "MSG1")
        Me.MSG1.Name = "MSG1"
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.Name = "Label6"
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.Name = "Label7"
        '
        'Label9
        '
        resources.ApplyResources(Me.Label9, "Label9")
        Me.Label9.Name = "Label9"
        '
        'RadioPLUS
        '
        resources.ApplyResources(Me.RadioPLUS, "RadioPLUS")
        Me.RadioPLUS.Name = "RadioPLUS"
        Me.RadioPLUS.UseVisualStyleBackColor = True
        '
        'Label8
        '
        resources.ApplyResources(Me.Label8, "Label8")
        Me.Label8.Name = "Label8"
        '
        'ButtonOK
        '
        resources.ApplyResources(Me.ButtonOK, "ButtonOK")
        Me.ButtonOK.Name = "ButtonOK"
        Me.ButtonOK.UseVisualStyleBackColor = True
        '
        'ButtonNew
        '
        resources.ApplyResources(Me.ButtonNew, "ButtonNew")
        Me.ButtonNew.Name = "ButtonNew"
        Me.ButtonNew.UseVisualStyleBackColor = True
        '
        'ButtonDelete
        '
        resources.ApplyResources(Me.ButtonDelete, "ButtonDelete")
        Me.ButtonDelete.Name = "ButtonDelete"
        Me.ButtonDelete.UseVisualStyleBackColor = True
        '
        'ButtonEdit
        '
        resources.ApplyResources(Me.ButtonEdit, "ButtonEdit")
        Me.ButtonEdit.Name = "ButtonEdit"
        Me.ButtonEdit.UseVisualStyleBackColor = True
        '
        'FilterDialog
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
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
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
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
    Friend WithEvents OptMark As System.Windows.Forms.RadioButton

End Class
