<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_WeeklySalesRollup
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.DateTime_StartDate = New System.Windows.Forms.DateTimePicker
        Me.DateTime_EndDate = New System.Windows.Forms.DateTimePicker
        Me.RunButton = New System.Windows.Forms.Button
        Me.CancelButton = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(41, 26)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(139, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Enter Start and End Dates"
        '
        'DateTime_StartDate
        '
        Me.DateTime_StartDate.Location = New System.Drawing.Point(44, 68)
        Me.DateTime_StartDate.Name = "DateTime_StartDate"
        Me.DateTime_StartDate.Size = New System.Drawing.Size(200, 22)
        Me.DateTime_StartDate.TabIndex = 1
        '
        'DateTime_EndDate
        '
        Me.DateTime_EndDate.Location = New System.Drawing.Point(44, 120)
        Me.DateTime_EndDate.Name = "DateTime_EndDate"
        Me.DateTime_EndDate.Size = New System.Drawing.Size(200, 22)
        Me.DateTime_EndDate.TabIndex = 2
        '
        'RunButton
        '
        Me.RunButton.Location = New System.Drawing.Point(44, 166)
        Me.RunButton.Name = "RunButton"
        Me.RunButton.Size = New System.Drawing.Size(170, 23)
        Me.RunButton.TabIndex = 3
        Me.RunButton.Text = "Run Weekly Sales Rollup"
        Me.RunButton.UseVisualStyleBackColor = True
        '
        'CancelButton
        '
        Me.CancelButton.Location = New System.Drawing.Point(44, 213)
        Me.CancelButton.Name = "CancelButton"
        Me.CancelButton.Size = New System.Drawing.Size(75, 23)
        Me.CancelButton.TabIndex = 4
        Me.CancelButton.Text = "Cancel"
        Me.CancelButton.UseVisualStyleBackColor = True
        '
        'Form_WeeklySalesRollup
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(292, 266)
        Me.Controls.Add(Me.CancelButton)
        Me.Controls.Add(Me.RunButton)
        Me.Controls.Add(Me.DateTime_EndDate)
        Me.Controls.Add(Me.DateTime_StartDate)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_WeeklySalesRollup"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Weekly Sales Rollup"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DateTime_StartDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTime_EndDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents RunButton As System.Windows.Forms.Button
    Friend Shadows WithEvents CancelButton As System.Windows.Forms.Button
End Class
