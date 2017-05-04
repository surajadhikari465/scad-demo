<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ClosedOrdersMissingInvoiceDataReport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ClosedOrdersMissingInvoiceDataReport))
        Me.RadioButton_Other = New System.Windows.Forms.RadioButton
        Me.RadioButton_None = New System.Windows.Forms.RadioButton
        Me.RadioButton_Both = New System.Windows.Forms.RadioButton
        Me.cmdReport = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'RadioButton_Other
        '
        Me.RadioButton_Other.AutoSize = True
        Me.RadioButton_Other.Location = New System.Drawing.Point(55, 38)
        Me.RadioButton_Other.Name = "RadioButton_Other"
        Me.RadioButton_Other.Size = New System.Drawing.Size(51, 17)
        Me.RadioButton_Other.TabIndex = 1
        Me.RadioButton_Other.TabStop = True
        Me.RadioButton_Other.Text = "Other"
        Me.RadioButton_Other.UseVisualStyleBackColor = True
        '
        'RadioButton_None
        '
        Me.RadioButton_None.AutoSize = True
        Me.RadioButton_None.Location = New System.Drawing.Point(112, 38)
        Me.RadioButton_None.Name = "RadioButton_None"
        Me.RadioButton_None.Size = New System.Drawing.Size(51, 17)
        Me.RadioButton_None.TabIndex = 2
        Me.RadioButton_None.TabStop = True
        Me.RadioButton_None.Text = "None"
        Me.RadioButton_None.UseVisualStyleBackColor = True
        '
        'RadioButton_Both
        '
        Me.RadioButton_Both.AutoSize = True
        Me.RadioButton_Both.Location = New System.Drawing.Point(169, 38)
        Me.RadioButton_Both.Name = "RadioButton_Both"
        Me.RadioButton_Both.Size = New System.Drawing.Size(47, 17)
        Me.RadioButton_Both.TabIndex = 3
        Me.RadioButton_Both.TabStop = True
        Me.RadioButton_Both.Text = "Both"
        Me.RadioButton_Both.UseVisualStyleBackColor = True
        '
        'cmdReport
        '
        Me.cmdReport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdReport.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdReport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReport.Image = CType(resources.GetObject("cmdReport.Image"), System.Drawing.Image)
        Me.cmdReport.Location = New System.Drawing.Point(189, 78)
        Me.cmdReport.Name = "cmdReport"
        Me.cmdReport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdReport.Size = New System.Drawing.Size(41, 41)
        Me.cmdReport.TabIndex = 8
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
        Me.cmdExit.Location = New System.Drawing.Point(236, 78)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 9
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(52, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(102, 16)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Invoice Type:"
        '
        'ClosedOrdersMissingInvoiceDataReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(280, 121)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmdReport)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.RadioButton_Both)
        Me.Controls.Add(Me.RadioButton_None)
        Me.Controls.Add(Me.RadioButton_Other)
        Me.Name = "ClosedOrdersMissingInvoiceDataReport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Closed Orders Missing Invoice Data"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents RadioButton_Other As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton_None As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton_Both As System.Windows.Forms.RadioButton
    Public WithEvents cmdReport As System.Windows.Forms.Button
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
