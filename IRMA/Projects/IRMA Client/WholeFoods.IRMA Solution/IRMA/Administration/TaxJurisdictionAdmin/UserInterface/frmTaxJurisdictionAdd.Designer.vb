<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TaxJurisdictionAdd
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
        Me.components = New System.ComponentModel.Container
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.GroupBox_UserProperties = New System.Windows.Forms.GroupBox
        Me.txtTaxJurisdictionDesc = New System.Windows.Forms.TextBox
        Me.Label_UserName = New System.Windows.Forms.Label
        Me.grpCopyValues = New System.Windows.Forms.GroupBox
        Me.cboCloneTaxJurisdiction = New System.Windows.Forms.ComboBox
        Me.TaxJurisdictionListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.lblSelectTaxJurisdiction = New System.Windows.Forms.Label
        Me.GroupBox_UserProperties.SuspendLayout()
        Me.grpCopyValues.SuspendLayout()
        CType(Me.TaxJurisdictionListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(356, 141)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 5
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(266, 141)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'GroupBox_UserProperties
        '
        Me.GroupBox_UserProperties.Controls.Add(Me.txtTaxJurisdictionDesc)
        Me.GroupBox_UserProperties.Controls.Add(Me.Label_UserName)
        Me.GroupBox_UserProperties.Location = New System.Drawing.Point(9, 12)
        Me.GroupBox_UserProperties.Name = "GroupBox_UserProperties"
        Me.GroupBox_UserProperties.Size = New System.Drawing.Size(422, 60)
        Me.GroupBox_UserProperties.TabIndex = 6
        Me.GroupBox_UserProperties.TabStop = False
        Me.GroupBox_UserProperties.Text = "Tax Jurisdiction Properties"
        '
        'txtTaxJurisdictionDesc
        '
        Me.txtTaxJurisdictionDesc.Location = New System.Drawing.Point(164, 22)
        Me.txtTaxJurisdictionDesc.MaxLength = 25
        Me.txtTaxJurisdictionDesc.Name = "txtTaxJurisdictionDesc"
        Me.txtTaxJurisdictionDesc.Size = New System.Drawing.Size(252, 22)
        Me.txtTaxJurisdictionDesc.TabIndex = 1
        '
        'Label_UserName
        '
        Me.Label_UserName.AutoSize = True
        Me.Label_UserName.Location = New System.Drawing.Point(12, 25)
        Me.Label_UserName.Name = "Label_UserName"
        Me.Label_UserName.Size = New System.Drawing.Size(150, 13)
        Me.Label_UserName.TabIndex = 1
        Me.Label_UserName.Text = "Tax Jurisdiction Description:"
        '
        'grpCopyValues
        '
        Me.grpCopyValues.Controls.Add(Me.cboCloneTaxJurisdiction)
        Me.grpCopyValues.Controls.Add(Me.lblSelectTaxJurisdiction)
        Me.grpCopyValues.Location = New System.Drawing.Point(12, 78)
        Me.grpCopyValues.Name = "grpCopyValues"
        Me.grpCopyValues.Size = New System.Drawing.Size(419, 57)
        Me.grpCopyValues.TabIndex = 17
        Me.grpCopyValues.TabStop = False
        Me.grpCopyValues.Text = "Copy Values from Existing Tax Jurisdiction"
        '
        'cboCloneTaxJurisdiction
        '
        Me.cboCloneTaxJurisdiction.DataSource = Me.TaxJurisdictionListBindingSource
        Me.cboCloneTaxJurisdiction.DisplayMember = "TaxJurisdictionName"
        Me.cboCloneTaxJurisdiction.FormattingEnabled = True
        Me.cboCloneTaxJurisdiction.Location = New System.Drawing.Point(161, 22)
        Me.cboCloneTaxJurisdiction.Name = "cboCloneTaxJurisdiction"
        Me.cboCloneTaxJurisdiction.Size = New System.Drawing.Size(252, 21)
        Me.cboCloneTaxJurisdiction.TabIndex = 3
        Me.cboCloneTaxJurisdiction.ValueMember = "TaxJurisdictionID"
        '
        'TaxJurisdictionListBindingSource
        '
        '   Me.TaxJurisdictionListBindingSource.DataSource = GetType(IRMA.Library.Administration.TaxJurisdictionList)
        '
        'lblSelectTaxJurisdiction
        '
        Me.lblSelectTaxJurisdiction.AutoSize = True
        Me.lblSelectTaxJurisdiction.Location = New System.Drawing.Point(12, 25)
        Me.lblSelectTaxJurisdiction.Name = "lblSelectTaxJurisdiction"
        Me.lblSelectTaxJurisdiction.Size = New System.Drawing.Size(121, 13)
        Me.lblSelectTaxJurisdiction.TabIndex = 1
        Me.lblSelectTaxJurisdiction.Text = "Select Tax Jurisdiction:"
        '
        'TaxJurisdictionAdd
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(446, 175)
        Me.Controls.Add(Me.grpCopyValues)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.GroupBox_UserProperties)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "TaxJurisdictionAdd"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Add TaxJurisdiction"
        Me.GroupBox_UserProperties.ResumeLayout(False)
        Me.GroupBox_UserProperties.PerformLayout()
        Me.grpCopyValues.ResumeLayout(False)
        Me.grpCopyValues.PerformLayout()
        CType(Me.TaxJurisdictionListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents GroupBox_UserProperties As System.Windows.Forms.GroupBox
    Friend WithEvents txtTaxJurisdictionDesc As System.Windows.Forms.TextBox
    Friend WithEvents Label_UserName As System.Windows.Forms.Label
    Friend WithEvents grpCopyValues As System.Windows.Forms.GroupBox
    Friend WithEvents cboCloneTaxJurisdiction As System.Windows.Forms.ComboBox
    Friend WithEvents lblSelectTaxJurisdiction As System.Windows.Forms.Label
    Friend WithEvents TaxJurisdictionListBindingSource As System.Windows.Forms.BindingSource

End Class
