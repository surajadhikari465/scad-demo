Namespace WholeFoods.IRMA.CompetitorStore.UserInterface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class CompetitorFileUpload
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CompetitorFileUpload))
            Me.btnCancel = New System.Windows.Forms.Button
            Me.btnUpload = New System.Windows.Forms.Button
            Me.lblWait = New System.Windows.Forms.Label
            Me.lblError = New System.Windows.Forms.Label
            Me.btnBrowse = New System.Windows.Forms.Button
            Me.txtFilePath = New System.Windows.Forms.TextBox
            Me.Label2 = New System.Windows.Forms.Label
            Me.Label1 = New System.Windows.Forms.Label
            Me.PictureBox1 = New System.Windows.Forms.PictureBox
            CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'btnCancel
            '
            Me.btnCancel.Location = New System.Drawing.Point(196, 170)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 23)
            Me.btnCancel.TabIndex = 2
            Me.btnCancel.Text = "Cancel"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'btnUpload
            '
            Me.btnUpload.Location = New System.Drawing.Point(425, 170)
            Me.btnUpload.Name = "btnUpload"
            Me.btnUpload.Size = New System.Drawing.Size(75, 23)
            Me.btnUpload.TabIndex = 0
            Me.btnUpload.Text = "Upload"
            Me.btnUpload.UseVisualStyleBackColor = True
            '
            'lblWait
            '
            Me.lblWait.AutoSize = True
            Me.lblWait.BackColor = System.Drawing.Color.Transparent
            Me.lblWait.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblWait.ForeColor = System.Drawing.SystemColors.ControlDark
            Me.lblWait.Location = New System.Drawing.Point(189, 76)
            Me.lblWait.Name = "lblWait"
            Me.lblWait.Size = New System.Drawing.Size(199, 37)
            Me.lblWait.TabIndex = 4
            Me.lblWait.Text = "Validating..."
            Me.lblWait.Visible = False
            '
            'lblError
            '
            Me.lblError.AutoSize = True
            Me.lblError.Location = New System.Drawing.Point(193, 128)
            Me.lblError.Name = "lblError"
            Me.lblError.Size = New System.Drawing.Size(75, 13)
            Me.lblError.TabIndex = 18
            Me.lblError.Text = "Error Message"
            Me.lblError.Visible = False
            '
            'btnBrowse
            '
            Me.btnBrowse.Location = New System.Drawing.Point(425, 141)
            Me.btnBrowse.Name = "btnBrowse"
            Me.btnBrowse.Size = New System.Drawing.Size(75, 23)
            Me.btnBrowse.TabIndex = 17
            Me.btnBrowse.Text = "Browse"
            Me.btnBrowse.UseVisualStyleBackColor = True
            '
            'txtFilePath
            '
            Me.txtFilePath.Location = New System.Drawing.Point(196, 144)
            Me.txtFilePath.Name = "txtFilePath"
            Me.txtFilePath.Size = New System.Drawing.Size(223, 20)
            Me.txtFilePath.TabIndex = 16
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(193, 43)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(194, 13)
            Me.Label2.TabIndex = 15
            Me.Label2.Text = "Press 'Browse' to open the file to import."
            '
            'Label1
            '
            Me.Label1.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold)
            Me.Label1.Location = New System.Drawing.Point(193, 12)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(341, 31)
            Me.Label1.TabIndex = 14
            Me.Label1.Text = "Competitor Store File Upload"
            '
            'PictureBox1
            '
            Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
            Me.PictureBox1.Location = New System.Drawing.Point(12, 12)
            Me.PictureBox1.Name = "PictureBox1"
            Me.PictureBox1.Size = New System.Drawing.Size(175, 181)
            Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
            Me.PictureBox1.TabIndex = 13
            Me.PictureBox1.TabStop = False
            '
            'CompetitorFileUpload
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.ClientSize = New System.Drawing.Size(503, 198)
            Me.ControlBox = False
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnUpload)
            Me.Controls.Add(Me.lblWait)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.lblError)
            Me.Controls.Add(Me.btnBrowse)
            Me.Controls.Add(Me.txtFilePath)
            Me.Controls.Add(Me.PictureBox1)
            Me.Controls.Add(Me.Label1)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.Name = "CompetitorFileUpload"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.Text = "Competitor Store File Upload"
            CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents btnUpload As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        'Friend WithEvents ugPreview As Infragistics.Win.UltraWinGrid.UltraGrid
        Friend WithEvents lblWait As System.Windows.Forms.Label
        Friend WithEvents lblError As System.Windows.Forms.Label
        Friend WithEvents btnBrowse As System.Windows.Forms.Button
        Friend WithEvents txtFilePath As System.Windows.Forms.TextBox
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox

    End Class
End Namespace