Namespace WholeFoods.IRMA.ItemChaining.UserInterface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class ItemSearchItemOptions
        Inherits System.Windows.Forms.UserControl

        'UserControl overrides dispose to clean up the component list.
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
            Me.chkIncludeDeletedItems = New System.Windows.Forms.CheckBox
            Me.chkHFM = New System.Windows.Forms.CheckBox
            Me.chkNotAvailable = New System.Windows.Forms.CheckBox
            Me.chkWFMItems = New System.Windows.Forms.CheckBox
            Me.chkDiscontinued = New System.Windows.Forms.CheckBox
            Me.SuspendLayout()
            '
            'chkIncludeDeletedItems
            '
            Me.chkIncludeDeletedItems.AutoSize = True
            Me.chkIncludeDeletedItems.BackColor = System.Drawing.Color.Transparent
            Me.chkIncludeDeletedItems.CheckAlign = System.Drawing.ContentAlignment.TopLeft
            Me.chkIncludeDeletedItems.Cursor = System.Windows.Forms.Cursors.Default
            Me.chkIncludeDeletedItems.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
            Me.chkIncludeDeletedItems.ForeColor = System.Drawing.SystemColors.ControlText
            Me.chkIncludeDeletedItems.ImeMode = System.Windows.Forms.ImeMode.NoControl
            Me.chkIncludeDeletedItems.Location = New System.Drawing.Point(3, 0)
            Me.chkIncludeDeletedItems.Name = "chkIncludeDeletedItems"
            Me.chkIncludeDeletedItems.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me.chkIncludeDeletedItems.Size = New System.Drawing.Size(146, 18)
            Me.chkIncludeDeletedItems.TabIndex = 17
            Me.chkIncludeDeletedItems.Text = "Include Deleted Items"
            Me.chkIncludeDeletedItems.UseVisualStyleBackColor = False
            '
            'chkHFM
            '
            Me.chkHFM.AutoSize = True
            Me.chkHFM.BackColor = System.Drawing.Color.Transparent
            Me.chkHFM.CheckAlign = System.Drawing.ContentAlignment.TopLeft
            Me.chkHFM.Cursor = System.Windows.Forms.Cursors.Default
            Me.chkHFM.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
            Me.chkHFM.ForeColor = System.Drawing.SystemColors.ControlText
            Me.chkHFM.ImeMode = System.Windows.Forms.ImeMode.NoControl
            Me.chkHFM.Location = New System.Drawing.Point(203, 39)
            Me.chkHFM.Name = "chkHFM"
            Me.chkHFM.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me.chkHFM.Size = New System.Drawing.Size(94, 18)
            Me.chkHFM.TabIndex = 21
            Me.chkHFM.Text = "Sold At HFM "
            Me.chkHFM.UseVisualStyleBackColor = False
            Me.chkHFM.Visible = False
            '
            'chkNotAvailable
            '
            Me.chkNotAvailable.AutoSize = True
            Me.chkNotAvailable.BackColor = System.Drawing.Color.Transparent
            Me.chkNotAvailable.CheckAlign = System.Drawing.ContentAlignment.TopLeft
            Me.chkNotAvailable.Cursor = System.Windows.Forms.Cursors.Default
            Me.chkNotAvailable.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
            Me.chkNotAvailable.ForeColor = System.Drawing.SystemColors.ControlText
            Me.chkNotAvailable.ImeMode = System.Windows.Forms.ImeMode.NoControl
            Me.chkNotAvailable.Location = New System.Drawing.Point(3, 37)
            Me.chkNotAvailable.Name = "chkNotAvailable"
            Me.chkNotAvailable.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me.chkNotAvailable.Size = New System.Drawing.Size(144, 18)
            Me.chkNotAvailable.TabIndex = 19
            Me.chkNotAvailable.Text = "Exclude Not Available "
            Me.chkNotAvailable.UseVisualStyleBackColor = False
            '
            'chkWFMItems
            '
            Me.chkWFMItems.AutoSize = True
            Me.chkWFMItems.BackColor = System.Drawing.Color.Transparent
            Me.chkWFMItems.CheckAlign = System.Drawing.ContentAlignment.TopLeft
            Me.chkWFMItems.Cursor = System.Windows.Forms.Cursors.Default
            Me.chkWFMItems.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
            Me.chkWFMItems.ForeColor = System.Drawing.SystemColors.ControlText
            Me.chkWFMItems.ImeMode = System.Windows.Forms.ImeMode.NoControl
            Me.chkWFMItems.Location = New System.Drawing.Point(203, 19)
            Me.chkWFMItems.Name = "chkWFMItems"
            Me.chkWFMItems.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me.chkWFMItems.Size = New System.Drawing.Size(97, 18)
            Me.chkWFMItems.TabIndex = 20
            Me.chkWFMItems.Text = "Sold At WFM "
            Me.chkWFMItems.UseVisualStyleBackColor = False
            Me.chkWFMItems.Visible = False
            '
            'chkDiscontinued
            '
            Me.chkDiscontinued.AutoSize = True
            Me.chkDiscontinued.BackColor = System.Drawing.Color.Transparent
            Me.chkDiscontinued.CheckAlign = System.Drawing.ContentAlignment.TopLeft
            Me.chkDiscontinued.Cursor = System.Windows.Forms.Cursors.Default
            Me.chkDiscontinued.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
            Me.chkDiscontinued.ForeColor = System.Drawing.SystemColors.ControlText
            Me.chkDiscontinued.ImeMode = System.Windows.Forms.ImeMode.NoControl
            Me.chkDiscontinued.Location = New System.Drawing.Point(3, 19)
            Me.chkDiscontinued.Name = "chkDiscontinued"
            Me.chkDiscontinued.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me.chkDiscontinued.Size = New System.Drawing.Size(144, 18)
            Me.chkDiscontinued.TabIndex = 18
            Me.chkDiscontinued.Text = "Include Discontinued "
            Me.chkDiscontinued.UseVisualStyleBackColor = False
            '
            'ItemSearchItemOptions
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.chkIncludeDeletedItems)
            Me.Controls.Add(Me.chkHFM)
            Me.Controls.Add(Me.chkNotAvailable)
            Me.Controls.Add(Me.chkWFMItems)
            Me.Controls.Add(Me.chkDiscontinued)
            Me.Name = "ItemSearchItemOptions"
            Me.Size = New System.Drawing.Size(305, 57)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Public WithEvents chkIncludeDeletedItems As System.Windows.Forms.CheckBox
        Public WithEvents chkHFM As System.Windows.Forms.CheckBox
        Public WithEvents chkNotAvailable As System.Windows.Forms.CheckBox
        Public WithEvents chkWFMItems As System.Windows.Forms.CheckBox
        Public WithEvents chkDiscontinued As System.Windows.Forms.CheckBox

    End Class
End Namespace