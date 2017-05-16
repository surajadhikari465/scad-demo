<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class UpdateSplash
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UpdateSplash))
        Me.pbrUpdate = New System.Windows.Forms.ProgressBar
        Me.imgUpdate = New System.Windows.Forms.PictureBox
        Me.lblSubtitle = New System.Windows.Forms.Label
        Me.lblStep = New System.Windows.Forms.Label
        Me.lblStepMessage = New System.Windows.Forms.Label
        Me.lblTitle = New System.Windows.Forms.Label
        Me.MainMenu1 = New System.Windows.Forms.MainMenu
        Me.SuspendLayout()
        '
        'pbrUpdate
        '
        Me.pbrUpdate.Location = New System.Drawing.Point(3, 212)
        Me.pbrUpdate.Maximum = 0
        Me.pbrUpdate.Name = "pbrUpdate"
        Me.pbrUpdate.Size = New System.Drawing.Size(234, 15)
        '
        'imgUpdate
        '
        Me.imgUpdate.Image = CType(resources.GetObject("imgUpdate.Image"), System.Drawing.Image)
        Me.imgUpdate.Location = New System.Drawing.Point(88, 61)
        Me.imgUpdate.Name = "imgUpdate"
        Me.imgUpdate.Size = New System.Drawing.Size(64, 64)
        Me.imgUpdate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        '
        'lblSubtitle
        '
        Me.lblSubtitle.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblSubtitle.Location = New System.Drawing.Point(0, 128)
        Me.lblSubtitle.Name = "lblSubtitle"
        Me.lblSubtitle.Size = New System.Drawing.Size(240, 20)
        Me.lblSubtitle.Text = "Update in progress..."
        Me.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblStep
        '
        Me.lblStep.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.lblStep.Location = New System.Drawing.Point(4, 183)
        Me.lblStep.Name = "lblStep"
        Me.lblStep.Size = New System.Drawing.Size(233, 26)
        Me.lblStep.Text = "Gathering updates"
        '
        'lblStepMessage
        '
        Me.lblStepMessage.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.lblStepMessage.Location = New System.Drawing.Point(4, 230)
        Me.lblStepMessage.Name = "lblStepMessage"
        Me.lblStepMessage.Size = New System.Drawing.Size(233, 34)
        Me.lblStepMessage.Text = "Please wait..."
        '
        'lblTitle
        '
        Me.lblTitle.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitle.Location = New System.Drawing.Point(0, 36)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(240, 22)
        Me.lblTitle.Text = "Whole Foods Mobility Tools"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'UpdateSplash
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.lblStepMessage)
        Me.Controls.Add(Me.lblStep)
        Me.Controls.Add(Me.lblSubtitle)
        Me.Controls.Add(Me.imgUpdate)
        Me.Controls.Add(Me.pbrUpdate)
        Me.Menu = Me.MainMenu1
        Me.MinimizeBox = False
        Me.Name = "UpdateSplash"
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pbrUpdate As System.Windows.Forms.ProgressBar
    Friend WithEvents imgUpdate As System.Windows.Forms.PictureBox
    Friend WithEvents lblSubtitle As System.Windows.Forms.Label
    Friend WithEvents lblStep As System.Windows.Forms.Label
    Friend WithEvents lblStepMessage As System.Windows.Forms.Label
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu

End Class
