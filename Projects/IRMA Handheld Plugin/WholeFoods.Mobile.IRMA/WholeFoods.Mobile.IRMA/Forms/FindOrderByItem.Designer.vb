<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class FindOrderByItem
    Inherits HandheldHardware.ScanForm

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FindOrderByItem))
        Me.mainMenu1 = New System.Windows.Forms.MainMenu
        Me.MenuItemExit = New System.Windows.Forms.MenuItem
        Me.MenuItemSelectPO = New System.Windows.Forms.MenuItem
        Me.LabelStore = New System.Windows.Forms.Label
        Me.LabelUPC = New System.Windows.Forms.Label
        Me.DataGridPONumber = New System.Windows.Forms.DataGrid
        Me.DataGridTableStyle = New System.Windows.Forms.DataGridTableStyle
        Me.DataGridTextBoxColumnPO = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumnOrderedCost = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumnExpectedDate = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumnSubteam = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumnEinvoice = New System.Windows.Forms.DataGridTextBoxColumn
        Me.LabelUPCValue = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.MenuItemExit)
        Me.mainMenu1.MenuItems.Add(Me.MenuItemSelectPO)
        '
        'MenuItemExit
        '
        resources.ApplyResources(Me.MenuItemExit, "MenuItemExit")
        '
        'MenuItemSelectPO
        '
        resources.ApplyResources(Me.MenuItemSelectPO, "MenuItemSelectPO")
        '
        'LabelStore
        '
        Me.LabelStore.BackColor = System.Drawing.Color.Silver
        resources.ApplyResources(Me.LabelStore, "LabelStore")
        Me.LabelStore.Name = "LabelStore"
        '
        'LabelUPC
        '
        resources.ApplyResources(Me.LabelUPC, "LabelUPC")
        Me.LabelUPC.Name = "LabelUPC"
        '
        'DataGridPONumber
        '
        Me.DataGridPONumber.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        resources.ApplyResources(Me.DataGridPONumber, "DataGridPONumber")
        Me.DataGridPONumber.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.DataGridPONumber.Name = "DataGridPONumber"
        Me.DataGridPONumber.RowHeadersVisible = False
        Me.DataGridPONumber.TableStyles.Add(Me.DataGridTableStyle)
        '
        'DataGridTableStyle
        '
        Me.DataGridTableStyle.GridColumnStyles.Add(Me.DataGridTextBoxColumnPO)
        Me.DataGridTableStyle.GridColumnStyles.Add(Me.DataGridTextBoxColumnOrderedCost)
        Me.DataGridTableStyle.GridColumnStyles.Add(Me.DataGridTextBoxColumnExpectedDate)
        Me.DataGridTableStyle.GridColumnStyles.Add(Me.DataGridTextBoxColumnSubteam)
        Me.DataGridTableStyle.GridColumnStyles.Add(Me.DataGridTextBoxColumnEinvoice)
        Me.DataGridTableStyle.MappingName = "Orders"
        '
        'DataGridTextBoxColumnPO
        '
        Me.DataGridTextBoxColumnPO.Format = ""
        resources.ApplyResources(Me.DataGridTextBoxColumnPO, "DataGridTextBoxColumnPO")
        '
        'DataGridTextBoxColumnOrderedCost
        '
        Me.DataGridTextBoxColumnOrderedCost.Format = ""
        resources.ApplyResources(Me.DataGridTextBoxColumnOrderedCost, "DataGridTextBoxColumnOrderedCost")
        '
        'DataGridTextBoxColumnExpectedDate
        '
        Me.DataGridTextBoxColumnExpectedDate.Format = ""
        resources.ApplyResources(Me.DataGridTextBoxColumnExpectedDate, "DataGridTextBoxColumnExpectedDate")
        '
        'DataGridTextBoxColumnSubteam
        '
        Me.DataGridTextBoxColumnSubteam.Format = ""
        resources.ApplyResources(Me.DataGridTextBoxColumnSubteam, "DataGridTextBoxColumnSubteam")
        '
        'DataGridTextBoxColumnEinvoice
        '
        Me.DataGridTextBoxColumnEinvoice.Format = ""
        resources.ApplyResources(Me.DataGridTextBoxColumnEinvoice, "DataGridTextBoxColumnEinvoice")
        '
        'LabelUPCValue
        '
        resources.ApplyResources(Me.LabelUPCValue, "LabelUPCValue")
        Me.LabelUPCValue.Name = "LabelUPCValue"
        '
        'FindOrderByItem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        resources.ApplyResources(Me, "$this")
        Me.ControlBox = False
        Me.Controls.Add(Me.LabelUPCValue)
        Me.Controls.Add(Me.DataGridPONumber)
        Me.Controls.Add(Me.LabelUPC)
        Me.Controls.Add(Me.LabelStore)
        Me.Menu = Me.mainMenu1
        Me.Name = "FindOrderByItem"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LabelStore As System.Windows.Forms.Label
    Friend WithEvents LabelUPC As System.Windows.Forms.Label
    Friend WithEvents DataGridPONumber As System.Windows.Forms.DataGrid
    Friend WithEvents MenuItemExit As System.Windows.Forms.MenuItem
    Friend WithEvents DataGridTableStyle As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents DataGridTextBoxColumnPO As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumnOrderedCost As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumnExpectedDate As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumnSubteam As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumnEinvoice As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents LabelUPCValue As System.Windows.Forms.Label
    Friend WithEvents MenuItemSelectPO As System.Windows.Forms.MenuItem
End Class
