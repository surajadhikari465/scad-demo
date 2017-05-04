<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmConvertMeasures
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
        Me.cboUnitsOut = New System.Windows.Forms.ComboBox()
        Me.lblAmount = New System.Windows.Forms.Label()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.cboUnitsIn = New System.Windows.Forms.ComboBox()
        Me.txtAmountIn = New System.Windows.Forms.TextBox()
        Me.txtAmountOut = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'cboUnitsOut
        '
        Me.cboUnitsOut.FormattingEnabled = True
        Me.cboUnitsOut.Location = New System.Drawing.Point(128, 35)
        Me.cboUnitsOut.Name = "cboUnitsOut"
        Me.cboUnitsOut.Size = New System.Drawing.Size(160, 21)
        Me.cboUnitsOut.TabIndex = 1
        '
        'lblFromAmount
        '
        Me.lblAmount.AutoSize = True
        Me.lblAmount.Location = New System.Drawing.Point(4, 38)
        Me.lblAmount.Name = "lblAmount"
        Me.lblAmount.Size = New System.Drawing.Size(14, 13)
        Me.lblAmount.TabIndex = 3
        Me.lblAmount.Text = "is"
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(92, 71)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(115, 23)
        Me.btnClose.TabIndex = 4
        Me.btnClose.Text = "Close Window"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'cboUnitsIn
        '
        Me.cboUnitsIn.FormattingEnabled = True
        Me.cboUnitsIn.Location = New System.Drawing.Point(128, 11)
        Me.cboUnitsIn.Name = "cboUnitsIn"
        Me.cboUnitsIn.Size = New System.Drawing.Size(160, 21)
        Me.cboUnitsIn.TabIndex = 5
        '
        'txtAmountIn
        '
        Me.txtAmountIn.Location = New System.Drawing.Point(29, 12)
        Me.txtAmountIn.Name = "txtAmountIn"
        Me.txtAmountIn.Size = New System.Drawing.Size(92, 20)
        Me.txtAmountIn.TabIndex = 6
        '
        'txtAmountOut
        '
        Me.txtAmountOut.Location = New System.Drawing.Point(30, 35)
        Me.txtAmountOut.Name = "txtAmountOut"
        Me.txtAmountOut.Size = New System.Drawing.Size(92, 20)
        Me.txtAmountOut.TabIndex = 7
        '
        'FrmConvertMeasures
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(308, 106)
        Me.Controls.Add(Me.txtAmountOut)
        Me.Controls.Add(Me.txtAmountIn)
        Me.Controls.Add(Me.cboUnitsIn)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lblAmount)
        Me.Controls.Add(Me.cboUnitsOut)
        Me.Name = "FrmConvertMeasures"
        Me.Text = "Convert Measurements"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cboUnitsOut As System.Windows.Forms.ComboBox
    Friend WithEvents lblAmount As System.Windows.Forms.Label
    Friend WithEvents btnClose As System.Windows.Forms.Button

    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
        logger.Info("User canceled out of conversion")
        Me.Close()
    End Sub

    Private Sub FrmConvertMeasures_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        PopulateInUnits()
    End Sub
    Friend WithEvents cboUnitsIn As System.Windows.Forms.ComboBox
    Friend WithEvents txtAmountIn As System.Windows.Forms.TextBox
    Friend WithEvents txtAmountOut As System.Windows.Forms.TextBox
    Private Sub cboUnitsIn_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboUnitsIn.SelectedIndexChanged
        PopulateOutUnits()
        If txtAmountIn.Text <> "" Then
            Amount = txtAmountIn.Text
            PerformConversion()
        End If
    End Sub

    Private Sub cboUnitsOut_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboUnitsOut.SelectedIndexChanged
        If txtAmountIn.Text <> "" Then
            Amount = txtAmountIn.Text
            PerformConversion()
        End If
    End Sub
    Private Sub txtAmountIn_Leave(sender As System.Object, e As System.EventArgs) Handles txtAmountIn.Leave
        If txtAmountIn.Text <> "" Then
            Amount = txtAmountIn.Text
            PerformConversion()
        End If
    End Sub

    Private Sub FrmConvertMeasures_FormClosed(sender As System.Object, e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        Me.wcfClient1.Close()
        Me.wcfClient1.Dispose()
    End Sub
End Class
