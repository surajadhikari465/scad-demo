<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ZoneAdd
    Inherits Form_IRMABase

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.grpSelectZone = New System.Windows.Forms.GroupBox()
        Me.cboZoneRegion = New System.Windows.Forms.ComboBox()
        Me.RegionListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.lblSelectZoneRegion = New System.Windows.Forms.Label()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.GroupBox_UserProperties = New System.Windows.Forms.GroupBox()
        Me.txtZoneGLMarketingExpenseAcct = New System.Windows.Forms.TextBox()
        Me.lblZoneGLMarketingExpenseAcct = New System.Windows.Forms.Label()
        Me.txtZoneDesc = New System.Windows.Forms.TextBox()
        Me.lblZoneDesc = New System.Windows.Forms.Label()
        Me.grpSelectZone.SuspendLayout()
        CType(Me.RegionListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_UserProperties.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpSelectZone
        '
        Me.grpSelectZone.Controls.Add(Me.cboZoneRegion)
        Me.grpSelectZone.Controls.Add(Me.lblSelectZoneRegion)
        Me.grpSelectZone.Location = New System.Drawing.Point(12, 113)
        Me.grpSelectZone.Name = "grpSelectZone"
        Me.grpSelectZone.Size = New System.Drawing.Size(374, 57)
        Me.grpSelectZone.TabIndex = 22
        Me.grpSelectZone.TabStop = False
        Me.grpSelectZone.Text = "Zone Region"
        '
        'cboZoneRegion
        '
        Me.cboZoneRegion.DataSource = Me.RegionListBindingSource
        Me.cboZoneRegion.DisplayMember = "RegionName"
        Me.cboZoneRegion.FormattingEnabled = True
        Me.cboZoneRegion.Location = New System.Drawing.Point(89, 22)
        Me.cboZoneRegion.Name = "cboZoneRegion"
        Me.cboZoneRegion.Size = New System.Drawing.Size(277, 21)
        Me.cboZoneRegion.TabIndex = 3
        Me.cboZoneRegion.ValueMember = "RegionID"
        '
        'lblSelectZoneRegion
        '
        Me.lblSelectZoneRegion.AutoSize = True
        Me.lblSelectZoneRegion.Location = New System.Drawing.Point(6, 25)
        Me.lblSelectZoneRegion.Name = "lblSelectZoneRegion"
        Me.lblSelectZoneRegion.Size = New System.Drawing.Size(80, 13)
        Me.lblSelectZoneRegion.TabIndex = 1
        Me.lblSelectZoneRegion.Text = "Select Region:"
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(311, 176)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 21
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(221, 176)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 20
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'GroupBox_UserProperties
        '
        Me.GroupBox_UserProperties.Controls.Add(Me.txtZoneGLMarketingExpenseAcct)
        Me.GroupBox_UserProperties.Controls.Add(Me.lblZoneGLMarketingExpenseAcct)
        Me.GroupBox_UserProperties.Controls.Add(Me.txtZoneDesc)
        Me.GroupBox_UserProperties.Controls.Add(Me.lblZoneDesc)
        Me.GroupBox_UserProperties.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox_UserProperties.Name = "GroupBox_UserProperties"
        Me.GroupBox_UserProperties.Size = New System.Drawing.Size(377, 95)
        Me.GroupBox_UserProperties.TabIndex = 19
        Me.GroupBox_UserProperties.TabStop = False
        Me.GroupBox_UserProperties.Text = "Zone Properties"
        '
        'txtZoneGLMarketingExpenseAcct
        '
        Me.txtZoneGLMarketingExpenseAcct.Location = New System.Drawing.Point(164, 57)
        Me.txtZoneGLMarketingExpenseAcct.MaxLength = 60
        Me.txtZoneGLMarketingExpenseAcct.Name = "txtZoneGLMarketingExpenseAcct"
        Me.txtZoneGLMarketingExpenseAcct.Size = New System.Drawing.Size(127, 22)
        Me.txtZoneGLMarketingExpenseAcct.TabIndex = 16
        '
        'lblZoneGLMarketingExpenseAcct
        '
        Me.lblZoneGLMarketingExpenseAcct.AutoSize = True
        Me.lblZoneGLMarketingExpenseAcct.Location = New System.Drawing.Point(12, 60)
        Me.lblZoneGLMarketingExpenseAcct.Name = "lblZoneGLMarketingExpenseAcct"
        Me.lblZoneGLMarketingExpenseAcct.Size = New System.Drawing.Size(148, 13)
        Me.lblZoneGLMarketingExpenseAcct.TabIndex = 13
        Me.lblZoneGLMarketingExpenseAcct.Text = "GL Marketing Expense Acct:"
        '
        'txtZoneDesc
        '
        Me.txtZoneDesc.Location = New System.Drawing.Point(164, 22)
        Me.txtZoneDesc.MaxLength = 25
        Me.txtZoneDesc.Name = "txtZoneDesc"
        Me.txtZoneDesc.Size = New System.Drawing.Size(202, 22)
        Me.txtZoneDesc.TabIndex = 3
        '
        'lblZoneDesc
        '
        Me.lblZoneDesc.AutoSize = True
        Me.lblZoneDesc.Location = New System.Drawing.Point(12, 25)
        Me.lblZoneDesc.Name = "lblZoneDesc"
        Me.lblZoneDesc.Size = New System.Drawing.Size(98, 13)
        Me.lblZoneDesc.TabIndex = 1
        Me.lblZoneDesc.Text = "Zone Description:"
        '
        'ZoneAdd
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(398, 210)
        Me.Controls.Add(Me.grpSelectZone)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.GroupBox_UserProperties)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ZoneAdd"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Add Zone"
        Me.grpSelectZone.ResumeLayout(False)
        Me.grpSelectZone.PerformLayout()
        CType(Me.RegionListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_UserProperties.ResumeLayout(False)
        Me.GroupBox_UserProperties.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpSelectZone As System.Windows.Forms.GroupBox
    Friend WithEvents cboZoneRegion As System.Windows.Forms.ComboBox
    Friend WithEvents lblSelectZoneRegion As System.Windows.Forms.Label
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents GroupBox_UserProperties As System.Windows.Forms.GroupBox
    Friend WithEvents txtZoneGLMarketingExpenseAcct As System.Windows.Forms.TextBox
    Friend WithEvents lblZoneGLMarketingExpenseAcct As System.Windows.Forms.Label
    Friend WithEvents txtZoneDesc As System.Windows.Forms.TextBox
    Friend WithEvents lblZoneDesc As System.Windows.Forms.Label
    Friend WithEvents RegionListBindingSource As System.Windows.Forms.BindingSource

End Class
