<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class TransferReview
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
    Private mainMenu1 As System.Windows.Forms.MainMenu

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.mainMenu1 = New System.Windows.Forms.MainMenu
        Me.mnuAdd = New System.Windows.Forms.MenuItem
        Me.mnuSend = New System.Windows.Forms.MenuItem
        Me.btnViewDetails = New System.Windows.Forms.Button
        Me.lblTo = New System.Windows.Forms.Label
        Me.btnUpdate = New System.Windows.Forms.Button
        Me.btnRemove = New System.Windows.Forms.Button
        Me.dtgInProgressOrder = New System.Windows.Forms.DataGrid
        Me.lblFrom = New System.Windows.Forms.Label
        Me.lblExpectedDate = New System.Windows.Forms.Label
        Me.lblFromStoreSubteam = New System.Windows.Forms.Label
        Me.lblToStoreSubteam = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.mnuAdd)
        Me.mainMenu1.MenuItems.Add(Me.mnuSend)
        '
        'mnuAdd
        '
        Me.mnuAdd.Text = "Add LineItem"
        '
        'mnuSend
        '
        Me.mnuSend.Text = "Send"
        '
        'btnViewDetails
        '
        Me.btnViewDetails.Location = New System.Drawing.Point(3, 237)
        Me.btnViewDetails.Name = "btnViewDetails"
        Me.btnViewDetails.Size = New System.Drawing.Size(85, 28)
        Me.btnViewDetails.TabIndex = 41
        Me.btnViewDetails.Text = "View Details"
        '
        'lblTo
        '
        Me.lblTo.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTo.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold)
        Me.lblTo.Location = New System.Drawing.Point(0, 38)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(240, 18)
        Me.lblTo.Text = "To:"
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(97, 237)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(65, 28)
        Me.btnUpdate.TabIndex = 40
        Me.btnUpdate.Text = "Update"
        '
        'btnRemove
        '
        Me.btnRemove.Location = New System.Drawing.Point(168, 237)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(65, 28)
        Me.btnRemove.TabIndex = 39
        Me.btnRemove.Text = "Remove"
        '
        'dtgInProgressOrder
        '
        Me.dtgInProgressOrder.BackgroundColor = System.Drawing.Color.Silver
        Me.dtgInProgressOrder.Location = New System.Drawing.Point(4, 95)
        Me.dtgInProgressOrder.Name = "dtgInProgressOrder"
        Me.dtgInProgressOrder.Size = New System.Drawing.Size(229, 136)
        Me.dtgInProgressOrder.TabIndex = 38
        '
        'lblFrom
        '
        Me.lblFrom.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblFrom.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold)
        Me.lblFrom.Location = New System.Drawing.Point(0, 2)
        Me.lblFrom.Name = "lblFrom"
        Me.lblFrom.Size = New System.Drawing.Size(240, 18)
        Me.lblFrom.Text = "From:"
        '
        'lblExpectedDate
        '
        Me.lblExpectedDate.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblExpectedDate.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblExpectedDate.Location = New System.Drawing.Point(0, 74)
        Me.lblExpectedDate.Name = "lblExpectedDate"
        Me.lblExpectedDate.Size = New System.Drawing.Size(240, 18)
        Me.lblExpectedDate.Text = "Expected On:"
        '
        'lblFromStoreSubteam
        '
        Me.lblFromStoreSubteam.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblFromStoreSubteam.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblFromStoreSubteam.Location = New System.Drawing.Point(0, 20)
        Me.lblFromStoreSubteam.Name = "lblFromStoreSubteam"
        Me.lblFromStoreSubteam.Size = New System.Drawing.Size(240, 18)
        '
        'lblToStoreSubteam
        '
        Me.lblToStoreSubteam.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblToStoreSubteam.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblToStoreSubteam.Location = New System.Drawing.Point(0, 56)
        Me.lblToStoreSubteam.Name = "lblToStoreSubteam"
        Me.lblToStoreSubteam.Size = New System.Drawing.Size(240, 18)
        '
        'TransferReview
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.lblToStoreSubteam)
        Me.Controls.Add(Me.lblFromStoreSubteam)
        Me.Controls.Add(Me.lblExpectedDate)
        Me.Controls.Add(Me.btnViewDetails)
        Me.Controls.Add(Me.lblTo)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.dtgInProgressOrder)
        Me.Controls.Add(Me.lblFrom)
        Me.Menu = Me.mainMenu1
        Me.Name = "TransferReview"
        Me.Text = "Review Transfer"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnViewDetails As System.Windows.Forms.Button
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents dtgInProgressOrder As System.Windows.Forms.DataGrid
    Friend WithEvents lblFrom As System.Windows.Forms.Label
    Friend WithEvents mnuAdd As System.Windows.Forms.MenuItem
    Friend WithEvents mnuSend As System.Windows.Forms.MenuItem
    Friend WithEvents lblExpectedDate As System.Windows.Forms.Label
    Friend WithEvents lblFromStoreSubteam As System.Windows.Forms.Label
    Friend WithEvents lblToStoreSubteam As System.Windows.Forms.Label
End Class
