<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class dlgReceivingDocument
    Inherits System.Windows.Forms.Form

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
        Me.lblHeading = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOverwrite = New System.Windows.Forms.Button
        Me.btnADD = New System.Windows.Forms.Button
        Me.btnR = New System.Windows.Forms.Button
        Me.btnRD = New System.Windows.Forms.Button
        Me.btnC = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'lblHeading
        '
        Me.lblHeading.BackColor = System.Drawing.Color.Green
        Me.lblHeading.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblHeading.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblHeading.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblHeading.Location = New System.Drawing.Point(0, 0)
        Me.lblHeading.Name = "lblHeading"
        Me.lblHeading.Size = New System.Drawing.Size(183, 31)
        Me.lblHeading.Text = "Receive PO or Receiving Document"
        Me.lblHeading.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackColor = System.Drawing.Color.YellowGreen
        Me.btnCancel.Location = New System.Drawing.Point(149, -205)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(32, 10)
        Me.btnCancel.TabIndex = 19
        Me.btnCancel.Text = "&Cancel"
        '
        'btnOverwrite
        '
        Me.btnOverwrite.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnOverwrite.BackColor = System.Drawing.Color.YellowGreen
        Me.btnOverwrite.Location = New System.Drawing.Point(97, -205)
        Me.btnOverwrite.Name = "btnOverwrite"
        Me.btnOverwrite.Size = New System.Drawing.Size(37, 10)
        Me.btnOverwrite.TabIndex = 18
        Me.btnOverwrite.Text = "&Overwrite"
        '
        'btnADD
        '
        Me.btnADD.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnADD.BackColor = System.Drawing.Color.YellowGreen
        Me.btnADD.Location = New System.Drawing.Point(44, -205)
        Me.btnADD.Name = "btnADD"
        Me.btnADD.Size = New System.Drawing.Size(34, 10)
        Me.btnADD.TabIndex = 17
        Me.btnADD.Text = "&Add"
        '
        'btnR
        '
        Me.btnR.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnR.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.btnR.Location = New System.Drawing.Point(28, 49)
        Me.btnR.Name = "btnR"
        Me.btnR.Size = New System.Drawing.Size(134, 39)
        Me.btnR.TabIndex = 20
        Me.btnR.Text = "Receive PO"
        '
        'btnRD
        '
        Me.btnRD.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRD.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.btnRD.Location = New System.Drawing.Point(28, 112)
        Me.btnRD.Name = "btnRD"
        Me.btnRD.Size = New System.Drawing.Size(134, 41)
        Me.btnRD.TabIndex = 21
        Me.btnRD.Text = "Receiving Document"
        '
        'btnC
        '
        Me.btnC.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnC.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.btnC.Location = New System.Drawing.Point(28, 182)
        Me.btnC.Name = "btnC"
        Me.btnC.Size = New System.Drawing.Size(134, 39)
        Me.btnC.TabIndex = 22
        Me.btnC.Text = "Cancel"
        '
        'dlgReceivingDocument
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.Color.Silver
        Me.ClientSize = New System.Drawing.Size(183, 239)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnC)
        Me.Controls.Add(Me.btnRD)
        Me.Controls.Add(Me.btnR)
        Me.Controls.Add(Me.lblHeading)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOverwrite)
        Me.Controls.Add(Me.btnADD)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Location = New System.Drawing.Point(10, 26)
        Me.MinimizeBox = False
        Me.Name = "dlgReceivingDocument"
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblHeading As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOverwrite As System.Windows.Forms.Button
    Friend WithEvents btnADD As System.Windows.Forms.Button
    Friend WithEvents btnR As System.Windows.Forms.Button
    Friend WithEvents btnRD As System.Windows.Forms.Button
    Friend WithEvents btnC As System.Windows.Forms.Button
End Class
