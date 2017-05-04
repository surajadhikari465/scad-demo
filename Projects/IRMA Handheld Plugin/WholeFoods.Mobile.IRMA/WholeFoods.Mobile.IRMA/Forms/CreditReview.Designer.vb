<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class CreditReview
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
        Me.mnuCancel = New System.Windows.Forms.MenuItem
        Me.mnuNext = New System.Windows.Forms.MenuItem
        Me.lblIdentifier = New System.Windows.Forms.Label
        Me.lblIdentifierVal = New System.Windows.Forms.Label
        Me.lblPriceTypeVal = New System.Windows.Forms.Label
        Me.lblPriceType = New System.Windows.Forms.Label
        Me.lblPrimaryVendorVal = New System.Windows.Forms.Label
        Me.lblPrimaryVendor = New System.Windows.Forms.Label
        Me.lblQueuedVal = New System.Windows.Forms.Label
        Me.lblQueued = New System.Windows.Forms.Label
        Me.lblQty = New System.Windows.Forms.Label
        Me.lblSubTeamVal = New System.Windows.Forms.Label
        Me.lblStoreVal = New System.Windows.Forms.Label
        Me.lblDescriptionVal = New System.Windows.Forms.Label
        Me.lblPkg = New System.Windows.Forms.Label
        Me.lblPkgVal = New System.Windows.Forms.Label
        Me.txtQty = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.mnuCancel)
        Me.mainMenu1.MenuItems.Add(Me.mnuNext)
        '
        'mnuCancel
        '
        Me.mnuCancel.Text = "Cancel"
        '
        'mnuNext
        '
        Me.mnuNext.Text = "Next"
        '
        'lblIdentifier
        '
        Me.lblIdentifier.Location = New System.Drawing.Point(3, 53)
        Me.lblIdentifier.Name = "lblIdentifier"
        Me.lblIdentifier.Size = New System.Drawing.Size(75, 20)
        Me.lblIdentifier.Text = "Identifier:"
        '
        'lblIdentifierVal
        '
        Me.lblIdentifierVal.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblIdentifierVal.Location = New System.Drawing.Point(84, 53)
        Me.lblIdentifierVal.Name = "lblIdentifierVal"
        Me.lblIdentifierVal.Size = New System.Drawing.Size(149, 20)
        Me.lblIdentifierVal.Text = "4060"
        '
        'lblPriceTypeVal
        '
        Me.lblPriceTypeVal.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblPriceTypeVal.Location = New System.Drawing.Point(84, 73)
        Me.lblPriceTypeVal.Name = "lblPriceTypeVal"
        Me.lblPriceTypeVal.Size = New System.Drawing.Size(149, 20)
        Me.lblPriceTypeVal.Text = "REG"
        '
        'lblPriceType
        '
        Me.lblPriceType.Location = New System.Drawing.Point(3, 73)
        Me.lblPriceType.Name = "lblPriceType"
        Me.lblPriceType.Size = New System.Drawing.Size(75, 20)
        Me.lblPriceType.Text = "Price Type:"
        '
        'lblPrimaryVendorVal
        '
        Me.lblPrimaryVendorVal.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblPrimaryVendorVal.Location = New System.Drawing.Point(84, 113)
        Me.lblPrimaryVendorVal.Name = "lblPrimaryVendorVal"
        Me.lblPrimaryVendorVal.Size = New System.Drawing.Size(149, 61)
        Me.lblPrimaryVendorVal.Text = "PACIFIC NORTHWEST"
        '
        'lblPrimaryVendor
        '
        Me.lblPrimaryVendor.Location = New System.Drawing.Point(3, 113)
        Me.lblPrimaryVendor.Name = "lblPrimaryVendor"
        Me.lblPrimaryVendor.Size = New System.Drawing.Size(75, 20)
        Me.lblPrimaryVendor.Text = "PV:"
        '
        'lblQueuedVal
        '
        Me.lblQueuedVal.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblQueuedVal.Location = New System.Drawing.Point(84, 174)
        Me.lblQueuedVal.Name = "lblQueuedVal"
        Me.lblQueuedVal.Size = New System.Drawing.Size(149, 20)
        Me.lblQueuedVal.Text = "0"
        '
        'lblQueued
        '
        Me.lblQueued.Location = New System.Drawing.Point(3, 174)
        Me.lblQueued.Name = "lblQueued"
        Me.lblQueued.Size = New System.Drawing.Size(75, 20)
        Me.lblQueued.Text = "Queued:"
        '
        'lblQty
        '
        Me.lblQty.Location = New System.Drawing.Point(3, 194)
        Me.lblQty.Name = "lblQty"
        Me.lblQty.Size = New System.Drawing.Size(75, 20)
        Me.lblQty.Text = "QTY:"
        '
        'lblSubTeamVal
        '
        Me.lblSubTeamVal.BackColor = System.Drawing.Color.Silver
        Me.lblSubTeamVal.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblSubTeamVal.Location = New System.Drawing.Point(122, 4)
        Me.lblSubTeamVal.Name = "lblSubTeamVal"
        Me.lblSubTeamVal.Size = New System.Drawing.Size(118, 20)
        Me.lblSubTeamVal.Text = "SubTeam"
        '
        'lblStoreVal
        '
        Me.lblStoreVal.BackColor = System.Drawing.Color.Silver
        Me.lblStoreVal.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblStoreVal.Location = New System.Drawing.Point(0, 4)
        Me.lblStoreVal.Name = "lblStoreVal"
        Me.lblStoreVal.Size = New System.Drawing.Size(118, 20)
        Me.lblStoreVal.Text = "Store"
        '
        'lblDescriptionVal
        '
        Me.lblDescriptionVal.Location = New System.Drawing.Point(3, 33)
        Me.lblDescriptionVal.Name = "lblDescriptionVal"
        Me.lblDescriptionVal.Size = New System.Drawing.Size(234, 20)
        Me.lblDescriptionVal.Text = "BIG BOWL MANDARIN BROCCOLI"
        '
        'lblPkg
        '
        Me.lblPkg.Location = New System.Drawing.Point(3, 93)
        Me.lblPkg.Name = "lblPkg"
        Me.lblPkg.Size = New System.Drawing.Size(75, 20)
        Me.lblPkg.Text = "Pkg:"
        '
        'lblPkgVal
        '
        Me.lblPkgVal.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblPkgVal.Location = New System.Drawing.Point(84, 93)
        Me.lblPkgVal.Name = "lblPkgVal"
        Me.lblPkgVal.Size = New System.Drawing.Size(149, 20)
        Me.lblPkgVal.Text = "1 / 1 CASE"
        '
        'txtQty
        '
        Me.txtQty.Location = New System.Drawing.Point(84, 194)
        Me.txtQty.Name = "txtQty"
        Me.txtQty.Size = New System.Drawing.Size(100, 21)
        Me.txtQty.TabIndex = 70
        '
        'CreditReview
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.txtQty)
        Me.Controls.Add(Me.lblPkgVal)
        Me.Controls.Add(Me.lblPkg)
        Me.Controls.Add(Me.lblDescriptionVal)
        Me.Controls.Add(Me.lblSubTeamVal)
        Me.Controls.Add(Me.lblStoreVal)
        Me.Controls.Add(Me.lblQty)
        Me.Controls.Add(Me.lblQueuedVal)
        Me.Controls.Add(Me.lblQueued)
        Me.Controls.Add(Me.lblPrimaryVendorVal)
        Me.Controls.Add(Me.lblPrimaryVendor)
        Me.Controls.Add(Me.lblPriceTypeVal)
        Me.Controls.Add(Me.lblPriceType)
        Me.Controls.Add(Me.lblIdentifierVal)
        Me.Controls.Add(Me.lblIdentifier)
        Me.Menu = Me.mainMenu1
        Me.Name = "CreditReview"
        Me.Text = "Credit"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents mnuCancel As System.Windows.Forms.MenuItem
    Friend WithEvents mnuNext As System.Windows.Forms.MenuItem
    Friend WithEvents lblIdentifier As System.Windows.Forms.Label
    Friend WithEvents lblIdentifierVal As System.Windows.Forms.Label
    Friend WithEvents lblPriceTypeVal As System.Windows.Forms.Label
    Friend WithEvents lblPriceType As System.Windows.Forms.Label
    Friend WithEvents lblPrimaryVendorVal As System.Windows.Forms.Label
    Friend WithEvents lblPrimaryVendor As System.Windows.Forms.Label
    Friend WithEvents lblQueuedVal As System.Windows.Forms.Label
    Friend WithEvents lblQueued As System.Windows.Forms.Label
    Friend WithEvents lblQty As System.Windows.Forms.Label
    Friend WithEvents lblSubTeamVal As System.Windows.Forms.Label
    Friend WithEvents lblStoreVal As System.Windows.Forms.Label
    Friend WithEvents lblDescriptionVal As System.Windows.Forms.Label
    Friend WithEvents lblPkg As System.Windows.Forms.Label
    Friend WithEvents lblPkgVal As System.Windows.Forms.Label
    Friend WithEvents txtQty As System.Windows.Forms.TextBox
End Class
