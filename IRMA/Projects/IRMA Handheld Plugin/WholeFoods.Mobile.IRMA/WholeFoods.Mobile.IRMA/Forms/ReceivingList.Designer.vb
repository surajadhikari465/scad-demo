<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ReceivingList
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
        Me.mnuMain = New System.Windows.Forms.MenuItem
        Me.mnuPartialShipping = New System.Windows.Forms.MenuItem
        Me.mnuExit = New System.Windows.Forms.MenuItem
        Me.mnuCloseOrder = New System.Windows.Forms.MenuItem
        Me.tabReceivingList = New System.Windows.Forms.TabControl
        Me.tabEInvMismatch = New System.Windows.Forms.TabPage
        Me.tabEInvoiceException = New System.Windows.Forms.TabPage
        Me.tabNotReceived = New System.Windows.Forms.TabPage
        Me.lblTotalOrdered = New System.Windows.Forms.Label
        Me.lblTotalReceived = New System.Windows.Forms.Label
        Me.lblPONumber = New System.Windows.Forms.Label
        Me.lblOrdered = New System.Windows.Forms.Label
        Me.lblReceived = New System.Windows.Forms.Label
        Me.lblEinvoiced = New System.Windows.Forms.Label
        Me.lblTotaleInvoiced = New System.Windows.Forms.Label
        Me.lblPONumberValue = New System.Windows.Forms.Label
        Me.btnPrev = New System.Windows.Forms.Button
        Me.btnNext = New System.Windows.Forms.Button
        Me.tabReceivingList.SuspendLayout()
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.mnuMain)
        Me.mainMenu1.MenuItems.Add(Me.mnuCloseOrder)
        '
        'mnuMain
        '
        Me.mnuMain.MenuItems.Add(Me.mnuPartialShipping)
        Me.mnuMain.MenuItems.Add(Me.mnuExit)
        Me.mnuMain.Text = "Menu"
        '
        'mnuPartialShipping
        '
        Me.mnuPartialShipping.Text = "Partial Shipment"
        '
        'mnuExit
        '
        Me.mnuExit.Text = "Back"
        '
        'mnuCloseOrder
        '
        Me.mnuCloseOrder.Text = "Invoice Data"
        '
        'tabReceivingList
        '
        Me.tabReceivingList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.tabReceivingList.Controls.Add(Me.tabEInvMismatch)
        Me.tabReceivingList.Controls.Add(Me.tabEInvoiceException)
        Me.tabReceivingList.Controls.Add(Me.tabNotReceived)
        Me.tabReceivingList.Dock = System.Windows.Forms.DockStyle.None
        Me.tabReceivingList.Location = New System.Drawing.Point(0, 39)
        Me.tabReceivingList.Name = "tabReceivingList"
        Me.tabReceivingList.SelectedIndex = 0
        Me.tabReceivingList.Size = New System.Drawing.Size(240, 228)
        Me.tabReceivingList.TabIndex = 0
        '
        'tabEInvMismatch
        '
        Me.tabEInvMismatch.AutoScroll = True
        Me.tabEInvMismatch.Location = New System.Drawing.Point(0, 0)
        Me.tabEInvMismatch.Name = "tabEInvMismatch"
        Me.tabEInvMismatch.Size = New System.Drawing.Size(240, 205)
        Me.tabEInvMismatch.Text = "Inv. Mismatch"
        '
        'tabEInvoiceException
        '
        Me.tabEInvoiceException.AutoScroll = True
        Me.tabEInvoiceException.Location = New System.Drawing.Point(0, 0)
        Me.tabEInvoiceException.Name = "tabEInvoiceException"
        Me.tabEInvoiceException.Size = New System.Drawing.Size(232, 202)
        Me.tabEInvoiceException.Text = "eInv. Except."
        '
        'tabNotReceived
        '
        Me.tabNotReceived.AutoScroll = True
        Me.tabNotReceived.Location = New System.Drawing.Point(0, 0)
        Me.tabNotReceived.Name = "tabNotReceived"
        Me.tabNotReceived.Size = New System.Drawing.Size(232, 202)
        Me.tabNotReceived.Text = "Not Recvd."
        '
        'lblTotalOrdered
        '
        Me.lblTotalOrdered.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblTotalOrdered.Location = New System.Drawing.Point(3, 20)
        Me.lblTotalOrdered.Name = "lblTotalOrdered"
        Me.lblTotalOrdered.Size = New System.Drawing.Size(53, 12)
        Me.lblTotalOrdered.Text = "Ordered:"
        '
        'lblTotalReceived
        '
        Me.lblTotalReceived.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblTotalReceived.Location = New System.Drawing.Point(90, 20)
        Me.lblTotalReceived.Name = "lblTotalReceived"
        Me.lblTotalReceived.Size = New System.Drawing.Size(37, 12)
        Me.lblTotalReceived.Text = "Rcvd:"
        '
        'lblPONumber
        '
        Me.lblPONumber.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblPONumber.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.lblPONumber.Location = New System.Drawing.Point(4, 5)
        Me.lblPONumber.Name = "lblPONumber"
        Me.lblPONumber.Size = New System.Drawing.Size(35, 12)
        Me.lblPONumber.Text = "PO#"
        '
        'lblOrdered
        '
        Me.lblOrdered.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblOrdered.Location = New System.Drawing.Point(55, 20)
        Me.lblOrdered.Name = "lblOrdered"
        Me.lblOrdered.Size = New System.Drawing.Size(33, 12)
        '
        'lblReceived
        '
        Me.lblReceived.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblReceived.Location = New System.Drawing.Point(127, 20)
        Me.lblReceived.Name = "lblReceived"
        Me.lblReceived.Size = New System.Drawing.Size(35, 12)
        '
        'lblEinvoiced
        '
        Me.lblEinvoiced.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblEinvoiced.Location = New System.Drawing.Point(202, 20)
        Me.lblEinvoiced.Name = "lblEinvoiced"
        Me.lblEinvoiced.Size = New System.Drawing.Size(35, 12)
        '
        'lblTotaleInvoiced
        '
        Me.lblTotaleInvoiced.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblTotaleInvoiced.Location = New System.Drawing.Point(162, 20)
        Me.lblTotaleInvoiced.Name = "lblTotaleInvoiced"
        Me.lblTotaleInvoiced.Size = New System.Drawing.Size(41, 12)
        Me.lblTotaleInvoiced.Text = "eInvd:"
        '
        'lblPONumberValue
        '
        Me.lblPONumberValue.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblPONumberValue.Location = New System.Drawing.Point(39, 5)
        Me.lblPONumberValue.Name = "lblPONumberValue"
        Me.lblPONumberValue.Size = New System.Drawing.Size(81, 12)
        '
        'btnPrev
        '
        Me.btnPrev.Location = New System.Drawing.Point(117, 5)
        Me.btnPrev.Name = "btnPrev"
        Me.btnPrev.Size = New System.Drawing.Size(54, 14)
        Me.btnPrev.TabIndex = 11
        Me.btnPrev.Text = "< PREV"
        '
        'btnNext
        '
        Me.btnNext.Location = New System.Drawing.Point(183, 5)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(54, 14)
        Me.btnNext.TabIndex = 12
        Me.btnNext.Text = "NEXT >"
        '
        'ReceivingList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnNext)
        Me.Controls.Add(Me.btnPrev)
        Me.Controls.Add(Me.lblPONumberValue)
        Me.Controls.Add(Me.lblEinvoiced)
        Me.Controls.Add(Me.lblTotaleInvoiced)
        Me.Controls.Add(Me.lblReceived)
        Me.Controls.Add(Me.lblOrdered)
        Me.Controls.Add(Me.lblPONumber)
        Me.Controls.Add(Me.lblTotalReceived)
        Me.Controls.Add(Me.lblTotalOrdered)
        Me.Controls.Add(Me.tabReceivingList)
        Me.Menu = Me.mainMenu1
        Me.Name = "ReceivingList"
        Me.Text = "Receiving List"
        Me.tabReceivingList.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents mnuMain As System.Windows.Forms.MenuItem
    Friend WithEvents mnuCloseOrder As System.Windows.Forms.MenuItem
    Friend WithEvents tabReceivingList As System.Windows.Forms.TabControl
    Friend WithEvents tabEInvMismatch As System.Windows.Forms.TabPage
    Friend WithEvents tabEInvoiceException As System.Windows.Forms.TabPage
    Friend WithEvents tabNotReceived As System.Windows.Forms.TabPage
    Friend WithEvents lblTotalOrdered As System.Windows.Forms.Label
    Friend WithEvents lblTotalReceived As System.Windows.Forms.Label
    Friend WithEvents lblPONumber As System.Windows.Forms.Label
    Friend WithEvents mnuExit As System.Windows.Forms.MenuItem
    Friend WithEvents mnuPartialShipping As System.Windows.Forms.MenuItem
    Friend WithEvents lblOrdered As System.Windows.Forms.Label
    Friend WithEvents lblReceived As System.Windows.Forms.Label
    Friend WithEvents lblEinvoiced As System.Windows.Forms.Label
    Friend WithEvents lblTotaleInvoiced As System.Windows.Forms.Label
    Friend WithEvents lblPONumberValue As System.Windows.Forms.Label
    Friend WithEvents btnPrev As System.Windows.Forms.Button
    Friend WithEvents btnNext As System.Windows.Forms.Button
End Class
