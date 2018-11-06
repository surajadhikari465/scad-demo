<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ReasonCodeMappings
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ReasonCodeMappings))
        Me.cmbType = New System.Windows.Forms.ComboBox()
        Me.cmbDescription = New System.Windows.Forms.ComboBox()
        Me.cmdAddMapping = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdDisableMapping = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SuspendLayout()
        '
        'cmbType
        '
        Me.cmbType.FormattingEnabled = True
        Me.cmbType.Location = New System.Drawing.Point(101, 56)
        Me.cmbType.Name = "cmbType"
        Me.cmbType.Size = New System.Drawing.Size(228, 21)
        Me.cmbType.TabIndex = 0
        '
        'cmbDescription
        '
        Me.cmbDescription.FormattingEnabled = True
        Me.cmbDescription.Location = New System.Drawing.Point(101, 102)
        Me.cmbDescription.Name = "cmbDescription"
        Me.cmbDescription.Size = New System.Drawing.Size(228, 21)
        Me.cmbDescription.TabIndex = 1
        '
        'cmdAddMapping
        '
        Me.cmdAddMapping.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdAddMapping.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddMapping.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddMapping.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddMapping.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddMapping.Image = CType(resources.GetObject("cmdAddMapping.Image"), System.Drawing.Image)
        Me.cmdAddMapping.Location = New System.Drawing.Point(194, 172)
        Me.cmdAddMapping.Name = "cmdAddMapping"
        Me.cmdAddMapping.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddMapping.Size = New System.Drawing.Size(41, 41)
        Me.cmdAddMapping.TabIndex = 51
        Me.cmdAddMapping.Tag = ""
        Me.cmdAddMapping.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdAddMapping, "Add Mapping")
        Me.cmdAddMapping.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Image = CType(resources.GetObject("cmdExit.Image"), System.Drawing.Image)
        Me.cmdExit.Location = New System.Drawing.Point(288, 172)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExit.Size = New System.Drawing.Size(41, 41)
        Me.cmdExit.TabIndex = 52
        Me.cmdExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdExit, "Exit")
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdDisableMapping
        '
        Me.cmdDisableMapping.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdDisableMapping.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDisableMapping.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDisableMapping.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDisableMapping.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDisableMapping.Image = CType(resources.GetObject("cmdDisableMapping.Image"), System.Drawing.Image)
        Me.cmdDisableMapping.Location = New System.Drawing.Point(241, 172)
        Me.cmdDisableMapping.Name = "cmdDisableMapping"
        Me.cmdDisableMapping.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDisableMapping.Size = New System.Drawing.Size(41, 41)
        Me.cmdDisableMapping.TabIndex = 53
        Me.cmdDisableMapping.Tag = "B"
        Me.cmdDisableMapping.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ToolTip1.SetToolTip(Me.cmdDisableMapping, "Disable Mapping")
        Me.cmdDisableMapping.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(41, 59)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 13)
        Me.Label1.TabIndex = 54
        Me.Label1.Text = "Type:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(44, 110)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(44, 13)
        Me.Label2.TabIndex = 55
        Me.Label2.Text = "Detail:"
        '
        'ReasonCodeMappings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(389, 262)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmdDisableMapping)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdAddMapping)
        Me.Controls.Add(Me.cmbDescription)
        Me.Controls.Add(Me.cmbType)
        Me.Name = "ReasonCodeMappings"
        Me.Text = "Reason Code Mappings"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmbType As System.Windows.Forms.ComboBox
    Friend WithEvents cmbDescription As System.Windows.Forms.ComboBox
    Public WithEvents cmdAddMapping As System.Windows.Forms.Button
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdDisableMapping As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
End Class
