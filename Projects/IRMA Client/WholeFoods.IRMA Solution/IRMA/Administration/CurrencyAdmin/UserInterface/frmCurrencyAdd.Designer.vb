<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CurrencyAdd
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
        Me.RegionListBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.GroupBox_UserProperties = New System.Windows.Forms.GroupBox
        Me.txtCurrencyCode = New System.Windows.Forms.TextBox
        Me.lblZoneGLMarketingExpenseAcct = New System.Windows.Forms.Label
        Me.txtCurrencyDesc = New System.Windows.Forms.TextBox
        Me.lblZoneDesc = New System.Windows.Forms.Label
        CType(Me.RegionListBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_UserProperties.SuspendLayout()
        Me.SuspendLayout()
        '
        'RegionListBindingSource
        '
        '  Me.RegionListBindingSource.DataSource = GetType(IRMA.Library.Administration.RegionList)
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(318, 126)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 21
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(228, 126)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 20
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'GroupBox_UserProperties
        '
        Me.GroupBox_UserProperties.Controls.Add(Me.txtCurrencyCode)
        Me.GroupBox_UserProperties.Controls.Add(Me.lblZoneGLMarketingExpenseAcct)
        Me.GroupBox_UserProperties.Controls.Add(Me.txtCurrencyDesc)
        Me.GroupBox_UserProperties.Controls.Add(Me.lblZoneDesc)
        Me.GroupBox_UserProperties.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox_UserProperties.Name = "GroupBox_UserProperties"
        Me.GroupBox_UserProperties.Size = New System.Drawing.Size(377, 95)
        Me.GroupBox_UserProperties.TabIndex = 19
        Me.GroupBox_UserProperties.TabStop = False
        Me.GroupBox_UserProperties.Text = "Currency Properties"
        '
        'txtCurrencyCode
        '
        Me.txtCurrencyCode.Location = New System.Drawing.Point(164, 57)
        Me.txtCurrencyCode.MaxLength = 60
        Me.txtCurrencyCode.Name = "txtCurrencyCode"
        Me.txtCurrencyCode.Size = New System.Drawing.Size(127, 20)
        Me.txtCurrencyCode.TabIndex = 16
        '
        'lblZoneGLMarketingExpenseAcct
        '
        Me.lblZoneGLMarketingExpenseAcct.AutoSize = True
        Me.lblZoneGLMarketingExpenseAcct.Location = New System.Drawing.Point(12, 60)
        Me.lblZoneGLMarketingExpenseAcct.Name = "lblZoneGLMarketingExpenseAcct"
        Me.lblZoneGLMarketingExpenseAcct.Size = New System.Drawing.Size(77, 13)
        Me.lblZoneGLMarketingExpenseAcct.TabIndex = 13
        Me.lblZoneGLMarketingExpenseAcct.Text = "Currency Code"
        '
        'txtCurrencyDesc
        '
        Me.txtCurrencyDesc.Location = New System.Drawing.Point(164, 22)
        Me.txtCurrencyDesc.MaxLength = 25
        Me.txtCurrencyDesc.Name = "txtCurrencyDesc"
        Me.txtCurrencyDesc.Size = New System.Drawing.Size(202, 20)
        Me.txtCurrencyDesc.TabIndex = 3
        '
        'lblZoneDesc
        '
        Me.lblZoneDesc.AutoSize = True
        Me.lblZoneDesc.Location = New System.Drawing.Point(12, 25)
        Me.lblZoneDesc.Name = "lblZoneDesc"
        Me.lblZoneDesc.Size = New System.Drawing.Size(111, 13)
        Me.lblZoneDesc.TabIndex = 1
        Me.lblZoneDesc.Text = "Currency Description: "
        '
        'CurrencyAdd
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(398, 164)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.GroupBox_UserProperties)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "CurrencyAdd"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Add Zone"
        CType(Me.RegionListBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_UserProperties.ResumeLayout(False)
        Me.GroupBox_UserProperties.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents GroupBox_UserProperties As System.Windows.Forms.GroupBox
    Friend WithEvents txtCurrencyCode As System.Windows.Forms.TextBox
    Friend WithEvents lblZoneGLMarketingExpenseAcct As System.Windows.Forms.Label
    Friend WithEvents txtCurrencyDesc As System.Windows.Forms.TextBox
    Friend WithEvents lblZoneDesc As System.Windows.Forms.Label
    Friend WithEvents RegionListBindingSource As System.Windows.Forms.BindingSource

End Class
