<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class COOLReceiving
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(COOLReceiving))
        Me.chkPrintOnly = New System.Windows.Forms.CheckBox
        Me.cmdReport = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.lblControlGroup = New System.Windows.Forms.Label
        Me.txtPurchaseOrderID = New System.Windows.Forms.TextBox
        Me.lblPurchaseOrderNo = New System.Windows.Forms.Label
        Me.txtItemIdentifier = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'chkPrintOnly
        '
        Me.chkPrintOnly.BackColor = System.Drawing.SystemColors.Control
        Me.chkPrintOnly.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkPrintOnly.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPrintOnly.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkPrintOnly.Location = New System.Drawing.Point(80, 90)
        Me.chkPrintOnly.Name = "chkPrintOnly"
        Me.chkPrintOnly.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkPrintOnly.Size = New System.Drawing.Size(97, 17)
        Me.chkPrintOnly.TabIndex = 82
        Me.chkPrintOnly.Text = "Print Only"
        Me.chkPrintOnly.UseVisualStyleBackColor = False
        '
        'cmdReport
        '
        Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReport.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReport.Image = CType(resources.GetObject("cmdReport.Image"), System.Drawing.Image)
        Me.cmdReport.Location = New System.Drawing.Point(198, 80)
        Me.cmdReport.Name = "cmdReport"
        Me.cmdReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReport.Size = New System.Drawing.Size(41, 41)
        Me.cmdReport.TabIndex = 83
        Me.cmdReport.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdReport.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(251, 80)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 84
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'lblControlGroup
        '
        Me.lblControlGroup.BackColor = System.Drawing.Color.Transparent
        Me.lblControlGroup.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblControlGroup.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblControlGroup.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblControlGroup.Location = New System.Drawing.Point(7, 12)
        Me.lblControlGroup.Name = "lblControlGroup"
        Me.lblControlGroup.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblControlGroup.Size = New System.Drawing.Size(125, 17)
        Me.lblControlGroup.TabIndex = 80
        Me.lblControlGroup.Text = "Item Identifier :"
        Me.lblControlGroup.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtPurchaseOrderID
        '
        Me.txtPurchaseOrderID.Location = New System.Drawing.Point(138, 43)
        Me.txtPurchaseOrderID.MaxLength = 32
        Me.txtPurchaseOrderID.Name = "txtPurchaseOrderID"
        Me.txtPurchaseOrderID.Size = New System.Drawing.Size(133, 20)
        Me.txtPurchaseOrderID.TabIndex = 92
        '
        'lblPurchaseOrderNo
        '
        Me.lblPurchaseOrderNo.BackColor = System.Drawing.Color.Transparent
        Me.lblPurchaseOrderNo.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPurchaseOrderNo.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPurchaseOrderNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPurchaseOrderNo.Location = New System.Drawing.Point(7, 46)
        Me.lblPurchaseOrderNo.Name = "lblPurchaseOrderNo"
        Me.lblPurchaseOrderNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPurchaseOrderNo.Size = New System.Drawing.Size(125, 17)
        Me.lblPurchaseOrderNo.TabIndex = 91
        Me.lblPurchaseOrderNo.Text = "PO Number:"
        Me.lblPurchaseOrderNo.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtItemIdentifier
        '
        Me.txtItemIdentifier.Location = New System.Drawing.Point(138, 12)
        Me.txtItemIdentifier.Name = "txtItemIdentifier"
        Me.txtItemIdentifier.Size = New System.Drawing.Size(133, 20)
        Me.txtItemIdentifier.TabIndex = 98
        '
        'COOLReceiving
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(327, 130)
        Me.Controls.Add(Me.txtItemIdentifier)
        Me.Controls.Add(Me.txtPurchaseOrderID)
        Me.Controls.Add(Me.lblPurchaseOrderNo)
        Me.Controls.Add(Me.chkPrintOnly)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.lblControlGroup)
        Me.Name = "COOLReceiving"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "COOL Receiving"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents chkPrintOnly As System.Windows.Forms.CheckBox
    Public WithEvents cmdReport As System.Windows.Forms.Button
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents lblControlGroup As System.Windows.Forms.Label
    Friend WithEvents txtPurchaseOrderID As System.Windows.Forms.TextBox
    Public WithEvents lblPurchaseOrderNo As System.Windows.Forms.Label
    Friend WithEvents txtItemIdentifier As System.Windows.Forms.TextBox
End Class
