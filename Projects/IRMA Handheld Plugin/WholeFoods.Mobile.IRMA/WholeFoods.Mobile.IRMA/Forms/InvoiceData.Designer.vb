<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class InvoiceData
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InvoiceData))
        Me.mainMenu1 = New System.Windows.Forms.MainMenu
        Me.MenuItemMainMenu = New System.Windows.Forms.MenuItem
        Me.MenuItemReceiveOrder = New System.Windows.Forms.MenuItem
        Me.MenuItemCloseOrReOpenOrder = New System.Windows.Forms.MenuItem
        Me.PanelDocumentType = New System.Windows.Forms.Panel
        Me.RadioButtonInvoice = New System.Windows.Forms.RadioButton
        Me.RadioButtonOther = New System.Windows.Forms.RadioButton
        Me.RadioButtonNone = New System.Windows.Forms.RadioButton
        Me.PanelCharges = New System.Windows.Forms.Panel
        Me.LabelCharges = New System.Windows.Forms.Label
        Me.ButtonAddCharge = New System.Windows.Forms.Button
        Me.ButtonRemoveCharge = New System.Windows.Forms.Button
        Me.DataGridCharges = New System.Windows.Forms.DataGrid
        Me.DataGridTableStyle = New System.Windows.Forms.DataGridTableStyle
        Me.DataGridTextBoxColumnType = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumnGLAccount = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumnDescription = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumnValue = New System.Windows.Forms.DataGridTextBoxColumn
        Me.TextBoxCostDifference = New System.Windows.Forms.TextBox
        Me.TextBoxSubteam = New System.Windows.Forms.TextBox
        Me.TextBoxChargesTotal = New System.Windows.Forms.TextBox
        Me.TextBoxInvoiceTotal = New System.Windows.Forms.TextBox
        Me.LabelCostDifference = New System.Windows.Forms.Label
        Me.LabelSubteam = New System.Windows.Forms.Label
        Me.LabelChargesTotal = New System.Windows.Forms.Label
        Me.LabelInvoiceTotal = New System.Windows.Forms.Label
        Me.LabelDate = New System.Windows.Forms.Label
        Me.LabelInvoiceNumber = New System.Windows.Forms.Label
        Me.TextBoxInvoiceNumber = New System.Windows.Forms.TextBox
        Me.ComboBoxCurrency = New System.Windows.Forms.ComboBox
        Me.DateTimePickerInvoiceDate = New System.Windows.Forms.DateTimePicker
        Me.ButtonRefuse = New System.Windows.Forms.Button
        Me.ButtonReparseEinvoice = New System.Windows.Forms.Button
        Me.PanelInvoiceData = New System.Windows.Forms.Panel
        Me.LabelRefusedTotal = New System.Windows.Forms.Label
        Me.TextBoxRefusedTotal = New System.Windows.Forms.TextBox
        Me.PanelReparseRefuse = New System.Windows.Forms.Panel
        Me.PanelDocumentType.SuspendLayout()
        Me.PanelCharges.SuspendLayout()
        Me.PanelInvoiceData.SuspendLayout()
        Me.PanelReparseRefuse.SuspendLayout()
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.MenuItemMainMenu)
        Me.mainMenu1.MenuItems.Add(Me.MenuItemCloseOrReOpenOrder)
        '
        'MenuItemMainMenu
        '
        Me.MenuItemMainMenu.MenuItems.Add(Me.MenuItemReceiveOrder)
        resources.ApplyResources(Me.MenuItemMainMenu, "MenuItemMainMenu")
        '
        'MenuItemReceiveOrder
        '
        resources.ApplyResources(Me.MenuItemReceiveOrder, "MenuItemReceiveOrder")
        '
        'MenuItemCloseOrReOpenOrder
        '
        resources.ApplyResources(Me.MenuItemCloseOrReOpenOrder, "MenuItemCloseOrReOpenOrder")
        '
        'PanelDocumentType
        '
        Me.PanelDocumentType.BackColor = System.Drawing.Color.WhiteSmoke
        Me.PanelDocumentType.Controls.Add(Me.RadioButtonInvoice)
        Me.PanelDocumentType.Controls.Add(Me.RadioButtonOther)
        Me.PanelDocumentType.Controls.Add(Me.RadioButtonNone)
        resources.ApplyResources(Me.PanelDocumentType, "PanelDocumentType")
        Me.PanelDocumentType.Name = "PanelDocumentType"
        '
        'RadioButtonInvoice
        '
        Me.RadioButtonInvoice.ForeColor = System.Drawing.Color.Black
        resources.ApplyResources(Me.RadioButtonInvoice, "RadioButtonInvoice")
        Me.RadioButtonInvoice.Name = "RadioButtonInvoice"
        '
        'RadioButtonOther
        '
        Me.RadioButtonOther.ForeColor = System.Drawing.Color.Black
        resources.ApplyResources(Me.RadioButtonOther, "RadioButtonOther")
        Me.RadioButtonOther.Name = "RadioButtonOther"
        '
        'RadioButtonNone
        '
        Me.RadioButtonNone.ForeColor = System.Drawing.Color.Black
        resources.ApplyResources(Me.RadioButtonNone, "RadioButtonNone")
        Me.RadioButtonNone.Name = "RadioButtonNone"
        '
        'PanelCharges
        '
        Me.PanelCharges.BackColor = System.Drawing.Color.Silver
        Me.PanelCharges.Controls.Add(Me.LabelCharges)
        Me.PanelCharges.Controls.Add(Me.ButtonAddCharge)
        Me.PanelCharges.Controls.Add(Me.ButtonRemoveCharge)
        Me.PanelCharges.Controls.Add(Me.DataGridCharges)
        resources.ApplyResources(Me.PanelCharges, "PanelCharges")
        Me.PanelCharges.Name = "PanelCharges"
        '
        'LabelCharges
        '
        Me.LabelCharges.BackColor = System.Drawing.Color.Silver
        resources.ApplyResources(Me.LabelCharges, "LabelCharges")
        Me.LabelCharges.Name = "LabelCharges"
        '
        'ButtonAddCharge
        '
        resources.ApplyResources(Me.ButtonAddCharge, "ButtonAddCharge")
        Me.ButtonAddCharge.Name = "ButtonAddCharge"
        '
        'ButtonRemoveCharge
        '
        resources.ApplyResources(Me.ButtonRemoveCharge, "ButtonRemoveCharge")
        Me.ButtonRemoveCharge.Name = "ButtonRemoveCharge"
        '
        'DataGridCharges
        '
        Me.DataGridCharges.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        resources.ApplyResources(Me.DataGridCharges, "DataGridCharges")
        Me.DataGridCharges.Name = "DataGridCharges"
        Me.DataGridCharges.RowHeadersVisible = False
        Me.DataGridCharges.TableStyles.Add(Me.DataGridTableStyle)
        Me.DataGridCharges.TabStop = False
        '
        'DataGridTableStyle
        '
        Me.DataGridTableStyle.GridColumnStyles.Add(Me.DataGridTextBoxColumnType)
        Me.DataGridTableStyle.GridColumnStyles.Add(Me.DataGridTextBoxColumnGLAccount)
        Me.DataGridTableStyle.GridColumnStyles.Add(Me.DataGridTextBoxColumnDescription)
        Me.DataGridTableStyle.GridColumnStyles.Add(Me.DataGridTextBoxColumnValue)
        Me.DataGridTableStyle.MappingName = "Charges"
        '
        'DataGridTextBoxColumnType
        '
        Me.DataGridTextBoxColumnType.Format = ""
        Me.DataGridTextBoxColumnType.FormatInfo = Nothing
        resources.ApplyResources(Me.DataGridTextBoxColumnType, "DataGridTextBoxColumnType")
        '
        'DataGridTextBoxColumnGLAccount
        '
        Me.DataGridTextBoxColumnGLAccount.Format = ""
        Me.DataGridTextBoxColumnGLAccount.FormatInfo = Nothing
        resources.ApplyResources(Me.DataGridTextBoxColumnGLAccount, "DataGridTextBoxColumnGLAccount")
        '
        'DataGridTextBoxColumnDescription
        '
        Me.DataGridTextBoxColumnDescription.Format = ""
        Me.DataGridTextBoxColumnDescription.FormatInfo = Nothing
        resources.ApplyResources(Me.DataGridTextBoxColumnDescription, "DataGridTextBoxColumnDescription")
        '
        'DataGridTextBoxColumnValue
        '
        Me.DataGridTextBoxColumnValue.Format = ""
        Me.DataGridTextBoxColumnValue.FormatInfo = Nothing
        resources.ApplyResources(Me.DataGridTextBoxColumnValue, "DataGridTextBoxColumnValue")
        '
        'TextBoxCostDifference
        '
        resources.ApplyResources(Me.TextBoxCostDifference, "TextBoxCostDifference")
        Me.TextBoxCostDifference.Name = "TextBoxCostDifference"
        '
        'TextBoxSubteam
        '
        resources.ApplyResources(Me.TextBoxSubteam, "TextBoxSubteam")
        Me.TextBoxSubteam.Name = "TextBoxSubteam"
        '
        'TextBoxChargesTotal
        '
        resources.ApplyResources(Me.TextBoxChargesTotal, "TextBoxChargesTotal")
        Me.TextBoxChargesTotal.Name = "TextBoxChargesTotal"
        '
        'TextBoxInvoiceTotal
        '
        resources.ApplyResources(Me.TextBoxInvoiceTotal, "TextBoxInvoiceTotal")
        Me.TextBoxInvoiceTotal.Name = "TextBoxInvoiceTotal"
        '
        'LabelCostDifference
        '
        resources.ApplyResources(Me.LabelCostDifference, "LabelCostDifference")
        Me.LabelCostDifference.Name = "LabelCostDifference"
        '
        'LabelSubteam
        '
        resources.ApplyResources(Me.LabelSubteam, "LabelSubteam")
        Me.LabelSubteam.Name = "LabelSubteam"
        '
        'LabelChargesTotal
        '
        resources.ApplyResources(Me.LabelChargesTotal, "LabelChargesTotal")
        Me.LabelChargesTotal.Name = "LabelChargesTotal"
        '
        'LabelInvoiceTotal
        '
        resources.ApplyResources(Me.LabelInvoiceTotal, "LabelInvoiceTotal")
        Me.LabelInvoiceTotal.Name = "LabelInvoiceTotal"
        '
        'LabelDate
        '
        resources.ApplyResources(Me.LabelDate, "LabelDate")
        Me.LabelDate.Name = "LabelDate"
        '
        'LabelInvoiceNumber
        '
        resources.ApplyResources(Me.LabelInvoiceNumber, "LabelInvoiceNumber")
        Me.LabelInvoiceNumber.Name = "LabelInvoiceNumber"
        '
        'TextBoxInvoiceNumber
        '
        resources.ApplyResources(Me.TextBoxInvoiceNumber, "TextBoxInvoiceNumber")
        Me.TextBoxInvoiceNumber.Name = "TextBoxInvoiceNumber"
        '
        'ComboBoxCurrency
        '
        resources.ApplyResources(Me.ComboBoxCurrency, "ComboBoxCurrency")
        Me.ComboBoxCurrency.Name = "ComboBoxCurrency"
        Me.ComboBoxCurrency.TabStop = False
        '
        'DateTimePickerInvoiceDate
        '
        Me.DateTimePickerInvoiceDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        resources.ApplyResources(Me.DateTimePickerInvoiceDate, "DateTimePickerInvoiceDate")
        Me.DateTimePickerInvoiceDate.Name = "DateTimePickerInvoiceDate"
        '
        'ButtonRefuse
        '
        resources.ApplyResources(Me.ButtonRefuse, "ButtonRefuse")
        Me.ButtonRefuse.Name = "ButtonRefuse"
        '
        'ButtonReparseEinvoice
        '
        resources.ApplyResources(Me.ButtonReparseEinvoice, "ButtonReparseEinvoice")
        Me.ButtonReparseEinvoice.Name = "ButtonReparseEinvoice"
        '
        'PanelInvoiceData
        '
        Me.PanelInvoiceData.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.PanelInvoiceData.Controls.Add(Me.LabelRefusedTotal)
        Me.PanelInvoiceData.Controls.Add(Me.TextBoxRefusedTotal)
        Me.PanelInvoiceData.Controls.Add(Me.PanelReparseRefuse)
        Me.PanelInvoiceData.Controls.Add(Me.LabelInvoiceNumber)
        Me.PanelInvoiceData.Controls.Add(Me.TextBoxInvoiceNumber)
        Me.PanelInvoiceData.Controls.Add(Me.LabelDate)
        Me.PanelInvoiceData.Controls.Add(Me.LabelInvoiceTotal)
        Me.PanelInvoiceData.Controls.Add(Me.ComboBoxCurrency)
        Me.PanelInvoiceData.Controls.Add(Me.DateTimePickerInvoiceDate)
        Me.PanelInvoiceData.Controls.Add(Me.LabelChargesTotal)
        Me.PanelInvoiceData.Controls.Add(Me.LabelSubteam)
        Me.PanelInvoiceData.Controls.Add(Me.LabelCostDifference)
        Me.PanelInvoiceData.Controls.Add(Me.TextBoxInvoiceTotal)
        Me.PanelInvoiceData.Controls.Add(Me.TextBoxCostDifference)
        Me.PanelInvoiceData.Controls.Add(Me.TextBoxChargesTotal)
        Me.PanelInvoiceData.Controls.Add(Me.TextBoxSubteam)
        resources.ApplyResources(Me.PanelInvoiceData, "PanelInvoiceData")
        Me.PanelInvoiceData.Name = "PanelInvoiceData"
        '
        'LabelRefusedTotal
        '
        resources.ApplyResources(Me.LabelRefusedTotal, "LabelRefusedTotal")
        Me.LabelRefusedTotal.Name = "LabelRefusedTotal"
        '
        'TextBoxRefusedTotal
        '
        resources.ApplyResources(Me.TextBoxRefusedTotal, "TextBoxRefusedTotal")
        Me.TextBoxRefusedTotal.Name = "TextBoxRefusedTotal"
        Me.TextBoxRefusedTotal.ReadOnly = True
        '
        'PanelReparseRefuse
        '
        Me.PanelReparseRefuse.BackColor = System.Drawing.Color.WhiteSmoke
        Me.PanelReparseRefuse.Controls.Add(Me.ButtonRefuse)
        Me.PanelReparseRefuse.Controls.Add(Me.ButtonReparseEinvoice)
        resources.ApplyResources(Me.PanelReparseRefuse, "PanelReparseRefuse")
        Me.PanelReparseRefuse.Name = "PanelReparseRefuse"
        '
        'InvoiceData
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        resources.ApplyResources(Me, "$this")
        Me.BackColor = System.Drawing.Color.White
        Me.ControlBox = False
        Me.Controls.Add(Me.PanelInvoiceData)
        Me.Controls.Add(Me.PanelDocumentType)
        Me.Controls.Add(Me.PanelCharges)
        Me.Menu = Me.mainMenu1
        Me.Name = "InvoiceData"
        Me.PanelDocumentType.ResumeLayout(False)
        Me.PanelCharges.ResumeLayout(False)
        Me.PanelInvoiceData.ResumeLayout(False)
        Me.PanelReparseRefuse.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents MenuItemMainMenu As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemCloseOrReOpenOrder As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItemReceiveOrder As System.Windows.Forms.MenuItem
    Friend WithEvents PanelDocumentType As System.Windows.Forms.Panel
    Friend WithEvents RadioButtonInvoice As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonOther As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonNone As System.Windows.Forms.RadioButton
    Friend WithEvents PanelCharges As System.Windows.Forms.Panel
    Friend WithEvents LabelCharges As System.Windows.Forms.Label
    Friend WithEvents ButtonAddCharge As System.Windows.Forms.Button
    Friend WithEvents ButtonRemoveCharge As System.Windows.Forms.Button
    Friend WithEvents TextBoxCostDifference As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxSubteam As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxChargesTotal As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxInvoiceTotal As System.Windows.Forms.TextBox
    Friend WithEvents TextBoxInvoiceNumber As System.Windows.Forms.TextBox
    Friend WithEvents DataGridCharges As System.Windows.Forms.DataGrid
    Friend WithEvents DataGridTableStyle As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents DataGridTextBoxColumnType As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumnGLAccount As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumnDescription As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumnValue As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents ComboBoxCurrency As System.Windows.Forms.ComboBox
    Friend WithEvents DateTimePickerInvoiceDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents ButtonRefuse As System.Windows.Forms.Button
    Friend WithEvents ButtonReparseEinvoice As System.Windows.Forms.Button
    Friend WithEvents PanelInvoiceData As System.Windows.Forms.Panel
    Friend WithEvents LabelCostDifference As System.Windows.Forms.Label
    Friend WithEvents LabelSubteam As System.Windows.Forms.Label
    Friend WithEvents LabelChargesTotal As System.Windows.Forms.Label
    Friend WithEvents LabelInvoiceTotal As System.Windows.Forms.Label
    Friend WithEvents LabelDate As System.Windows.Forms.Label
    Friend WithEvents LabelInvoiceNumber As System.Windows.Forms.Label
    Friend WithEvents PanelReparseRefuse As System.Windows.Forms.Panel
    Friend WithEvents LabelRefusedTotal As System.Windows.Forms.Label
    Friend WithEvents TextBoxRefusedTotal As System.Windows.Forms.TextBox
End Class
