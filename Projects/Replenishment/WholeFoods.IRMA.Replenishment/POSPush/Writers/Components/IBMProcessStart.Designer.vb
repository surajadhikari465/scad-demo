<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class IBMProcessStart
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(IBMProcessStart))
        Me.wsREXEC = New AxMSWinsockLib.AxWinsock
        Me.tmrTimer = New System.Windows.Forms.Timer(Me.components)
        CType(Me.wsREXEC, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'wsREXEC
        '
        Me.wsREXEC.Enabled = True
        Me.wsREXEC.Location = New System.Drawing.Point(47, 25)
        Me.wsREXEC.Name = "wsREXEC"
        Me.wsREXEC.OcxState = CType(resources.GetObject("wsREXEC.OcxState"), System.Windows.Forms.AxHost.State)
        Me.wsREXEC.Size = New System.Drawing.Size(28, 28)
        Me.wsREXEC.TabIndex = 0
        '
        'tmrTimer
        '
        Me.tmrTimer.Interval = 60000
        '
        'IBMProcessStart
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(115, 78)
        Me.Controls.Add(Me.wsREXEC)
        Me.Name = "IBMProcessStart"
        Me.Text = "IBMProcessStart"
        CType(Me.wsREXEC, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents wsREXEC As AxMSWinsockLib.AxWinsock
    Friend WithEvents tmrTimer As System.Windows.Forms.Timer
End Class
