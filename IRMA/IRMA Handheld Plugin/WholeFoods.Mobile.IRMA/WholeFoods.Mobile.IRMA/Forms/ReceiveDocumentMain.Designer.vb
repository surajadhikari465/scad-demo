<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ReceiveDocumentMain
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
        Me.MenuItemMenu = New System.Windows.Forms.MenuItem
        Me.MenuItemNewSession = New System.Windows.Forms.MenuItem
        Me.MenuItemClearSession = New System.Windows.Forms.MenuItem
        Me.MenuItemExitReceiveDocument = New System.Windows.Forms.MenuItem
        Me.MenuItemExitIrmaMobile = New System.Windows.Forms.MenuItem
        Me.MenuItemViewSessions = New System.Windows.Forms.MenuItem
        Me.LabelStore = New System.Windows.Forms.Label
        Me.LabelSelectedStore = New System.Windows.Forms.Label
        Me.LabelSubteam = New System.Windows.Forms.Label
        Me.LabelSelectedSubteam = New System.Windows.Forms.Label
        Me.dsdVendors = New System.Windows.Forms.Label
        Me.vendorCombo = New System.Windows.Forms.ComboBox
        Me.LabelInvoiceNumber = New System.Windows.Forms.Label
        Me.invoiceNum = New System.Windows.Forms.TextBox
        Me.ButtonReceiveItem = New System.Windows.Forms.Button
        Me.LabelInvoiceCredit = New System.Windows.Forms.Label
        Me.returnCombo = New System.Windows.Forms.ComboBox
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.MenuItemMenu)
        Me.mainMenu1.MenuItems.Add(Me.MenuItemViewSessions)
        '
        'MenuItemMenu
        '
        Me.MenuItemMenu.MenuItems.Add(Me.MenuItemNewSession)
        Me.MenuItemMenu.MenuItems.Add(Me.MenuItemClearSession)
        Me.MenuItemMenu.MenuItems.Add(Me.MenuItemExitReceiveDocument)
        Me.MenuItemMenu.MenuItems.Add(Me.MenuItemExitIrmaMobile)
        Me.MenuItemMenu.Text = "Menu"
        '
        'MenuItemNewSession
        '
        Me.MenuItemNewSession.Text = "New Session"
        '
        'MenuItemClearSession
        '
        Me.MenuItemClearSession.Text = "Clear Session"
        '
        'MenuItemExitReceiveDocument
        '
        Me.MenuItemExitReceiveDocument.Text = "Exit Receive Document"
        '
        'MenuItemExitIrmaMobile
        '
        Me.MenuItemExitIrmaMobile.Text = "Exit IRMA Mobile"
        '
        'MenuItemViewSessions
        '
        Me.MenuItemViewSessions.Text = "View Sessions"
        '
        'LabelStore
        '
        Me.LabelStore.Location = New System.Drawing.Point(6, 16)
        Me.LabelStore.Name = "LabelStore"
        Me.LabelStore.Size = New System.Drawing.Size(43, 20)
        Me.LabelStore.Text = "Store:"
        '
        'LabelSelectedStore
        '
        Me.LabelSelectedStore.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.LabelSelectedStore.Location = New System.Drawing.Point(6, 36)
        Me.LabelSelectedStore.Name = "LabelSelectedStore"
        Me.LabelSelectedStore.Size = New System.Drawing.Size(216, 20)
        '
        'LabelSubteam
        '
        Me.LabelSubteam.Location = New System.Drawing.Point(6, 62)
        Me.LabelSubteam.Name = "LabelSubteam"
        Me.LabelSubteam.Size = New System.Drawing.Size(92, 20)
        Me.LabelSubteam.Text = "Subteam:"
        '
        'LabelSelectedSubteam
        '
        Me.LabelSelectedSubteam.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.LabelSelectedSubteam.Location = New System.Drawing.Point(6, 82)
        Me.LabelSelectedSubteam.Name = "LabelSelectedSubteam"
        Me.LabelSelectedSubteam.Size = New System.Drawing.Size(216, 20)
        '
        'dsdVendors
        '
        Me.dsdVendors.Location = New System.Drawing.Point(6, 110)
        Me.dsdVendors.Name = "dsdVendors"
        Me.dsdVendors.Size = New System.Drawing.Size(92, 20)
        Me.dsdVendors.Text = "DSD Vendors:"
        '
        'vendorCombo
        '
        Me.vendorCombo.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.vendorCombo.Location = New System.Drawing.Point(6, 131)
        Me.vendorCombo.Name = "vendorCombo"
        Me.vendorCombo.Size = New System.Drawing.Size(216, 22)
        Me.vendorCombo.TabIndex = 10
        '
        'LabelInvoiceNumber
        '
        Me.LabelInvoiceNumber.Location = New System.Drawing.Point(6, 168)
        Me.LabelInvoiceNumber.Name = "LabelInvoiceNumber"
        Me.LabelInvoiceNumber.Size = New System.Drawing.Size(92, 20)
        Me.LabelInvoiceNumber.Text = "Invoice#:"
        '
        'invoiceNum
        '
        Me.invoiceNum.Location = New System.Drawing.Point(104, 168)
        Me.invoiceNum.Name = "invoiceNum"
        Me.invoiceNum.Size = New System.Drawing.Size(118, 21)
        Me.invoiceNum.TabIndex = 133
        '
        'ButtonReceiveItem
        '
        Me.ButtonReceiveItem.Location = New System.Drawing.Point(30, 233)
        Me.ButtonReceiveItem.Name = "ButtonReceiveItem"
        Me.ButtonReceiveItem.Size = New System.Drawing.Size(167, 25)
        Me.ButtonReceiveItem.TabIndex = 140
        Me.ButtonReceiveItem.Text = "Receive Item"
        '
        'LabelInvoiceCredit
        '
        Me.LabelInvoiceCredit.Location = New System.Drawing.Point(6, 197)
        Me.LabelInvoiceCredit.Name = "LabelInvoiceCredit"
        Me.LabelInvoiceCredit.Size = New System.Drawing.Size(92, 20)
        Me.LabelInvoiceCredit.Text = "Invoice/Credit:"
        '
        'returnCombo
        '
        Me.returnCombo.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.returnCombo.Location = New System.Drawing.Point(104, 195)
        Me.returnCombo.Name = "returnCombo"
        Me.returnCombo.Size = New System.Drawing.Size(118, 22)
        Me.returnCombo.TabIndex = 148
        '
        'ReceiveDocumentMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.returnCombo)
        Me.Controls.Add(Me.LabelInvoiceCredit)
        Me.Controls.Add(Me.ButtonReceiveItem)
        Me.Controls.Add(Me.invoiceNum)
        Me.Controls.Add(Me.LabelInvoiceNumber)
        Me.Controls.Add(Me.vendorCombo)
        Me.Controls.Add(Me.dsdVendors)
        Me.Controls.Add(Me.LabelSelectedSubteam)
        Me.Controls.Add(Me.LabelSubteam)
        Me.Controls.Add(Me.LabelSelectedStore)
        Me.Controls.Add(Me.LabelStore)
        Me.Menu = Me.mainMenu1
        Me.Name = "ReceiveDocumentMain"
        Me.Text = "RD Main Menu"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LabelStore As System.Windows.Forms.Label
    Friend WithEvents LabelSelectedStore As System.Windows.Forms.Label
    Friend WithEvents LabelSubteam As System.Windows.Forms.Label
    Friend WithEvents LabelSelectedSubteam As System.Windows.Forms.Label
    Friend WithEvents dsdVendors As System.Windows.Forms.Label
    Friend WithEvents vendorCombo As System.Windows.Forms.ComboBox
    Friend WithEvents LabelInvoiceNumber As System.Windows.Forms.Label
    Friend WithEvents MenuItemMenu As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemViewSessions As System.Windows.Forms.MenuItem
    Friend WithEvents invoiceNum As System.Windows.Forms.TextBox
    Friend WithEvents ButtonReceiveItem As System.Windows.Forms.Button
    Friend WithEvents LabelInvoiceCredit As System.Windows.Forms.Label
    Friend WithEvents returnCombo As System.Windows.Forms.ComboBox
    Friend WithEvents MenuItemNewSession As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemClearSession As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemExitReceiveDocument As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemExitIrmaMobile As System.Windows.Forms.MenuItem
End Class
