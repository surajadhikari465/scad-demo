<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ChangeBatchState
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
        Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("StoreNo")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("BatchStatusID")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("SubTeamNo")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ItemChgTypeID")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PriceChgTypeID")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("AutoApplyFlag")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("POSBatchID")
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("BypassPrintShelfTags")
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("BypassApplyBatches")
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PriceBatchHeaderID")
        Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("BatchStatusDesc")
        Dim UltraGridColumn12 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("BatchDesc")
        Dim UltraGridColumn13 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("StoreName")
        Dim UltraGridColumn14 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("SubTeamName")
        Dim UltraGridColumn15 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("StartDate")
        Dim UltraGridColumn16 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ApplyDate")
        Dim UltraGridColumn17 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PrintedDate")
        Dim UltraGridColumn18 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("SentDate")
        Dim UltraGridColumn19 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ItemChgTypeDesc")
        Dim UltraGridColumn20 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PriceChgTypeDesc")
        Dim UltraGridColumn21 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("TotalItemCount")
        Dim Appearance16 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance17 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance18 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance19 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance20 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance21 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance22 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance23 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance24 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance25 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance26 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance27 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance28 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.GroupBox_BatchSearch = New System.Windows.Forms.GroupBox()
        Me.TextBox_BatchDescription = New System.Windows.Forms.TextBox()
        Me.Label_BatchDesc = New System.Windows.Forms.Label()
        Me.GroupBox_Stores = New System.Windows.Forms.GroupBox()
        Me.ComboBox_State = New System.Windows.Forms.ComboBox()
        Me.RadioButton_State = New System.Windows.Forms.RadioButton()
        Me.RadioButton_SingleStore = New System.Windows.Forms.RadioButton()
        Me.ComboBox_Store = New System.Windows.Forms.ComboBox()
        Me.RadioButton_AllStores = New System.Windows.Forms.RadioButton()
        Me.RadioButton_Zone = New System.Windows.Forms.RadioButton()
        Me.ComboBox_Zones = New System.Windows.Forms.ComboBox()
        Me.GroupBox_BatchState = New System.Windows.Forms.GroupBox()
        Me.CheckBox_Sent = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Ready = New System.Windows.Forms.CheckBox()
        Me.CheckBox_Printed = New System.Windows.Forms.CheckBox()
        Me.Button_Search = New System.Windows.Forms.Button()
        Me.GroupBox_Batches = New System.Windows.Forms.GroupBox()
        Me.UltraGrid_Batches = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.Button_Package = New System.Windows.Forms.Button()
        Me.GroupBox_BatchSearch.SuspendLayout()
        Me.GroupBox_Stores.SuspendLayout()
        Me.GroupBox_BatchState.SuspendLayout()
        Me.GroupBox_Batches.SuspendLayout()
        CType(Me.UltraGrid_Batches, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox_BatchSearch
        '
        Me.GroupBox_BatchSearch.Controls.Add(Me.TextBox_BatchDescription)
        Me.GroupBox_BatchSearch.Controls.Add(Me.Label_BatchDesc)
        Me.GroupBox_BatchSearch.Controls.Add(Me.GroupBox_Stores)
        Me.GroupBox_BatchSearch.Controls.Add(Me.GroupBox_BatchState)
        Me.GroupBox_BatchSearch.Controls.Add(Me.Button_Search)
        Me.GroupBox_BatchSearch.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox_BatchSearch.Name = "GroupBox_BatchSearch"
        Me.GroupBox_BatchSearch.Size = New System.Drawing.Size(918, 170)
        Me.GroupBox_BatchSearch.TabIndex = 0
        Me.GroupBox_BatchSearch.TabStop = False
        Me.GroupBox_BatchSearch.Text = "Batch Search Criteria"
        '
        'TextBox_BatchDescription
        '
        Me.TextBox_BatchDescription.AcceptsReturn = True
        Me.TextBox_BatchDescription.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox_BatchDescription.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.TextBox_BatchDescription.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.TextBox_BatchDescription.ForeColor = System.Drawing.SystemColors.WindowText
        Me.TextBox_BatchDescription.Location = New System.Drawing.Point(706, 22)
        Me.TextBox_BatchDescription.MaxLength = 30
        Me.TextBox_BatchDescription.Multiline = True
        Me.TextBox_BatchDescription.Name = "TextBox_BatchDescription"
        Me.TextBox_BatchDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.TextBox_BatchDescription.Size = New System.Drawing.Size(185, 53)
        Me.TextBox_BatchDescription.TabIndex = 307
        Me.TextBox_BatchDescription.Tag = "String"
        '
        'Label_BatchDesc
        '
        Me.Label_BatchDesc.BackColor = System.Drawing.Color.Transparent
        Me.Label_BatchDesc.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label_BatchDesc.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_BatchDesc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label_BatchDesc.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label_BatchDesc.Location = New System.Drawing.Point(573, 23)
        Me.Label_BatchDesc.Name = "Label_BatchDesc"
        Me.Label_BatchDesc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_BatchDesc.Size = New System.Drawing.Size(127, 16)
        Me.Label_BatchDesc.TabIndex = 305
        Me.Label_BatchDesc.Text = "Batch  Description :"
        Me.Label_BatchDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'GroupBox_Stores
        '
        Me.GroupBox_Stores.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox_Stores.Controls.Add(Me.ComboBox_State)
        Me.GroupBox_Stores.Controls.Add(Me.RadioButton_State)
        Me.GroupBox_Stores.Controls.Add(Me.RadioButton_SingleStore)
        Me.GroupBox_Stores.Controls.Add(Me.ComboBox_Store)
        Me.GroupBox_Stores.Controls.Add(Me.RadioButton_AllStores)
        Me.GroupBox_Stores.Controls.Add(Me.RadioButton_Zone)
        Me.GroupBox_Stores.Controls.Add(Me.ComboBox_Zones)
        Me.GroupBox_Stores.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox_Stores.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.GroupBox_Stores.Location = New System.Drawing.Point(25, 16)
        Me.GroupBox_Stores.Name = "GroupBox_Stores"
        Me.GroupBox_Stores.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.GroupBox_Stores.Size = New System.Drawing.Size(415, 105)
        Me.GroupBox_Stores.TabIndex = 304
        Me.GroupBox_Stores.TabStop = False
        Me.GroupBox_Stores.Text = "Stores"
        '
        'ComboBox_State
        '
        Me.ComboBox_State.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.ComboBox_State.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ComboBox_State.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox_State.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox_State.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_State.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.ComboBox_State.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBox_State.Location = New System.Drawing.Point(71, 70)
        Me.ComboBox_State.Name = "ComboBox_State"
        Me.ComboBox_State.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBox_State.Size = New System.Drawing.Size(57, 22)
        Me.ComboBox_State.Sorted = True
        Me.ComboBox_State.TabIndex = 25
        '
        'RadioButton_State
        '
        Me.RadioButton_State.BackColor = System.Drawing.SystemColors.Control
        Me.RadioButton_State.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadioButton_State.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton_State.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RadioButton_State.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadioButton_State.Location = New System.Drawing.Point(7, 70)
        Me.RadioButton_State.Name = "RadioButton_State"
        Me.RadioButton_State.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadioButton_State.Size = New System.Drawing.Size(58, 17)
        Me.RadioButton_State.TabIndex = 22
        Me.RadioButton_State.Text = "State"
        Me.RadioButton_State.UseVisualStyleBackColor = False
        '
        'RadioButton_SingleStore
        '
        Me.RadioButton_SingleStore.BackColor = System.Drawing.SystemColors.Control
        Me.RadioButton_SingleStore.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadioButton_SingleStore.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton_SingleStore.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RadioButton_SingleStore.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadioButton_SingleStore.Location = New System.Drawing.Point(7, 22)
        Me.RadioButton_SingleStore.Name = "RadioButton_SingleStore"
        Me.RadioButton_SingleStore.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadioButton_SingleStore.Size = New System.Drawing.Size(58, 17)
        Me.RadioButton_SingleStore.TabIndex = 20
        Me.RadioButton_SingleStore.Text = "Store"
        Me.RadioButton_SingleStore.UseVisualStyleBackColor = False
        '
        'ComboBox_Store
        '
        Me.ComboBox_Store.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.ComboBox_Store.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ComboBox_Store.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox_Store.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox_Store.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Store.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.ComboBox_Store.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBox_Store.Location = New System.Drawing.Point(71, 22)
        Me.ComboBox_Store.Name = "ComboBox_Store"
        Me.ComboBox_Store.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBox_Store.Size = New System.Drawing.Size(233, 22)
        Me.ComboBox_Store.Sorted = True
        Me.ComboBox_Store.TabIndex = 23
        '
        'RadioButton_AllStores
        '
        Me.RadioButton_AllStores.BackColor = System.Drawing.SystemColors.Control
        Me.RadioButton_AllStores.Checked = True
        Me.RadioButton_AllStores.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadioButton_AllStores.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton_AllStores.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RadioButton_AllStores.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadioButton_AllStores.Location = New System.Drawing.Point(320, 24)
        Me.RadioButton_AllStores.Name = "RadioButton_AllStores"
        Me.RadioButton_AllStores.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadioButton_AllStores.Size = New System.Drawing.Size(89, 17)
        Me.RadioButton_AllStores.TabIndex = 1
        Me.RadioButton_AllStores.TabStop = True
        Me.RadioButton_AllStores.Text = "All Stores"
        Me.RadioButton_AllStores.UseVisualStyleBackColor = False
        '
        'RadioButton_Zone
        '
        Me.RadioButton_Zone.BackColor = System.Drawing.SystemColors.Control
        Me.RadioButton_Zone.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadioButton_Zone.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButton_Zone.ForeColor = System.Drawing.SystemColors.ControlText
        Me.RadioButton_Zone.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.RadioButton_Zone.Location = New System.Drawing.Point(7, 46)
        Me.RadioButton_Zone.Name = "RadioButton_Zone"
        Me.RadioButton_Zone.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.RadioButton_Zone.Size = New System.Drawing.Size(58, 17)
        Me.RadioButton_Zone.TabIndex = 21
        Me.RadioButton_Zone.Text = "Zone"
        Me.RadioButton_Zone.UseVisualStyleBackColor = False
        '
        'ComboBox_Zones
        '
        Me.ComboBox_Zones.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.ComboBox_Zones.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ComboBox_Zones.BackColor = System.Drawing.SystemColors.Window
        Me.ComboBox_Zones.Cursor = System.Windows.Forms.Cursors.Default
        Me.ComboBox_Zones.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Zones.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.ComboBox_Zones.ForeColor = System.Drawing.SystemColors.WindowText
        Me.ComboBox_Zones.Location = New System.Drawing.Point(71, 46)
        Me.ComboBox_Zones.Name = "ComboBox_Zones"
        Me.ComboBox_Zones.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBox_Zones.Size = New System.Drawing.Size(233, 22)
        Me.ComboBox_Zones.Sorted = True
        Me.ComboBox_Zones.TabIndex = 2435
        '
        'GroupBox_BatchState
        '
        Me.GroupBox_BatchState.Controls.Add(Me.CheckBox_Sent)
        Me.GroupBox_BatchState.Controls.Add(Me.CheckBox_Ready)
        Me.GroupBox_BatchState.Controls.Add(Me.CheckBox_Printed)
        Me.GroupBox_BatchState.Location = New System.Drawing.Point(457, 16)
        Me.GroupBox_BatchState.Name = "GroupBox_BatchState"
        Me.GroupBox_BatchState.Size = New System.Drawing.Size(90, 105)
        Me.GroupBox_BatchState.TabIndex = 1
        Me.GroupBox_BatchState.TabStop = False
        Me.GroupBox_BatchState.Text = "Status"
        '
        'CheckBox_Sent
        '
        Me.CheckBox_Sent.AutoSize = True
        Me.CheckBox_Sent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox_Sent.Location = New System.Drawing.Point(6, 66)
        Me.CheckBox_Sent.Name = "CheckBox_Sent"
        Me.CheckBox_Sent.Size = New System.Drawing.Size(52, 17)
        Me.CheckBox_Sent.TabIndex = 3
        Me.CheckBox_Sent.Text = "Sent"
        Me.CheckBox_Sent.UseVisualStyleBackColor = True
        '
        'CheckBox_Ready
        '
        Me.CheckBox_Ready.AutoSize = True
        Me.CheckBox_Ready.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox_Ready.Location = New System.Drawing.Point(6, 19)
        Me.CheckBox_Ready.Name = "CheckBox_Ready"
        Me.CheckBox_Ready.Size = New System.Drawing.Size(62, 17)
        Me.CheckBox_Ready.TabIndex = 0
        Me.CheckBox_Ready.Text = "Ready"
        Me.CheckBox_Ready.UseVisualStyleBackColor = True
        '
        'CheckBox_Printed
        '
        Me.CheckBox_Printed.AutoSize = True
        Me.CheckBox_Printed.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox_Printed.Location = New System.Drawing.Point(6, 42)
        Me.CheckBox_Printed.Name = "CheckBox_Printed"
        Me.CheckBox_Printed.Size = New System.Drawing.Size(66, 17)
        Me.CheckBox_Printed.TabIndex = 2
        Me.CheckBox_Printed.Text = "Printed"
        Me.CheckBox_Printed.UseVisualStyleBackColor = True
        '
        'Button_Search
        '
        Me.Button_Search.Location = New System.Drawing.Point(6, 141)
        Me.Button_Search.Name = "Button_Search"
        Me.Button_Search.Size = New System.Drawing.Size(130, 23)
        Me.Button_Search.TabIndex = 0
        Me.Button_Search.Text = "Search for Batches"
        Me.Button_Search.UseVisualStyleBackColor = True
        '
        'GroupBox_Batches
        '
        Me.GroupBox_Batches.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox_Batches.Controls.Add(Me.UltraGrid_Batches)
        Me.GroupBox_Batches.Location = New System.Drawing.Point(12, 197)
        Me.GroupBox_Batches.Name = "GroupBox_Batches"
        Me.GroupBox_Batches.Size = New System.Drawing.Size(918, 359)
        Me.GroupBox_Batches.TabIndex = 1
        Me.GroupBox_Batches.TabStop = False
        Me.GroupBox_Batches.Text = "Batches"
        '
        'UltraGrid_Batches
        '
        Me.UltraGrid_Batches.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Appearance15.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance15.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.UltraGrid_Batches.DisplayLayout.Appearance = Appearance15
        Me.UltraGrid_Batches.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn1.Width = 14
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.Hidden = True
        UltraGridColumn2.Width = 50
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Hidden = True
        UltraGridColumn3.Width = 43
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.Hidden = True
        UltraGridColumn4.Width = 49
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.Hidden = True
        UltraGridColumn5.Width = 56
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.Hidden = True
        UltraGridColumn6.Width = 51
        UltraGridColumn7.Header.VisiblePosition = 6
        UltraGridColumn7.Hidden = True
        UltraGridColumn7.Width = 46
        UltraGridColumn8.Header.VisiblePosition = 7
        UltraGridColumn8.Hidden = True
        UltraGridColumn8.Width = 80
        UltraGridColumn9.Header.VisiblePosition = 8
        UltraGridColumn9.Hidden = True
        UltraGridColumn9.Width = 82
        UltraGridColumn10.Header.Caption = "ID"
        UltraGridColumn10.Header.VisiblePosition = 9
        UltraGridColumn10.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(69, 0)
        UltraGridColumn10.Width = 86
        UltraGridColumn11.Header.Caption = "Status"
        UltraGridColumn11.Header.VisiblePosition = 10
        UltraGridColumn11.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(62, 0)
        UltraGridColumn11.Width = 69
        UltraGridColumn12.Header.Caption = "Batch Desc"
        UltraGridColumn12.Header.VisiblePosition = 11
        UltraGridColumn12.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(81, 0)
        UltraGridColumn12.Width = 62
        UltraGridColumn13.Header.Caption = "Store"
        UltraGridColumn13.Header.VisiblePosition = 12
        UltraGridColumn13.Width = 62
        UltraGridColumn14.Header.Caption = "Subteam"
        UltraGridColumn14.Header.VisiblePosition = 13
        UltraGridColumn14.Width = 64
        UltraGridColumn15.Header.Caption = "Start Date"
        UltraGridColumn15.Header.VisiblePosition = 14
        UltraGridColumn15.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(74, 0)
        UltraGridColumn15.Width = 62
        UltraGridColumn16.Header.Caption = "Apply Date"
        UltraGridColumn16.Header.VisiblePosition = 15
        UltraGridColumn16.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(83, 0)
        UltraGridColumn16.Width = 62
        UltraGridColumn17.Header.Caption = "Printed Date"
        UltraGridColumn17.Header.VisiblePosition = 16
        UltraGridColumn17.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(86, 0)
        UltraGridColumn17.Width = 62
        UltraGridColumn18.Header.Caption = "Sent Date"
        UltraGridColumn18.Header.VisiblePosition = 17
        UltraGridColumn18.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(78, 0)
        UltraGridColumn18.Width = 62
        UltraGridColumn19.Header.Caption = "Chg Type"
        UltraGridColumn19.Header.VisiblePosition = 18
        UltraGridColumn19.Width = 72
        UltraGridColumn20.Header.Caption = "Price Type"
        UltraGridColumn20.Header.VisiblePosition = 19
        UltraGridColumn20.Width = 74
        UltraGridColumn21.Header.Caption = "Items"
        UltraGridColumn21.Header.VisiblePosition = 20
        UltraGridColumn21.Width = 64
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11, UltraGridColumn12, UltraGridColumn13, UltraGridColumn14, UltraGridColumn15, UltraGridColumn16, UltraGridColumn17, UltraGridColumn18, UltraGridColumn19, UltraGridColumn20, UltraGridColumn21})
        UltraGridBand1.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        UltraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.ColumnLayout
        Me.UltraGrid_Batches.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.UltraGrid_Batches.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance16.FontData.BoldAsString = "True"
        Me.UltraGrid_Batches.DisplayLayout.CaptionAppearance = Appearance16
        Me.UltraGrid_Batches.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[True]
        Appearance17.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance17.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance17.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_Batches.DisplayLayout.GroupByBox.Appearance = Appearance17
        Appearance18.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_Batches.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance18
        Me.UltraGrid_Batches.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_Batches.DisplayLayout.GroupByBox.Hidden = True
        Appearance19.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance19.BackColor2 = System.Drawing.SystemColors.Control
        Appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance19.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_Batches.DisplayLayout.GroupByBox.PromptAppearance = Appearance19
        Me.UltraGrid_Batches.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraGrid_Batches.DisplayLayout.MaxRowScrollRegions = 1
        Appearance20.BackColor = System.Drawing.SystemColors.Window
        Appearance20.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UltraGrid_Batches.DisplayLayout.Override.ActiveCellAppearance = Appearance20
        Me.UltraGrid_Batches.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.UltraGrid_Batches.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinGroup
        Me.UltraGrid_Batches.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid_Batches.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[False]
        Me.UltraGrid_Batches.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraGrid_Batches.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance21.BackColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_Batches.DisplayLayout.Override.CardAreaAppearance = Appearance21
        Appearance22.BorderColor = System.Drawing.Color.Silver
        Appearance22.FontData.BoldAsString = "True"
        Appearance22.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraGrid_Batches.DisplayLayout.Override.CellAppearance = Appearance22
        Me.UltraGrid_Batches.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Me.UltraGrid_Batches.DisplayLayout.Override.CellPadding = 0
        Appearance23.FontData.BoldAsString = "True"
        Me.UltraGrid_Batches.DisplayLayout.Override.FixedHeaderAppearance = Appearance23
        Appearance24.BackColor = System.Drawing.SystemColors.Control
        Appearance24.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance24.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance24.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_Batches.DisplayLayout.Override.GroupByRowAppearance = Appearance24
        Appearance25.FontData.BoldAsString = "True"
        Appearance25.TextHAlignAsString = "Left"
        Me.UltraGrid_Batches.DisplayLayout.Override.HeaderAppearance = Appearance25
        Me.UltraGrid_Batches.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance26.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid_Batches.DisplayLayout.Override.RowAlternateAppearance = Appearance26
        Appearance27.BackColor = System.Drawing.SystemColors.Window
        Appearance27.BorderColor = System.Drawing.Color.Silver
        Me.UltraGrid_Batches.DisplayLayout.Override.RowAppearance = Appearance27
        Me.UltraGrid_Batches.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.UltraGrid_Batches.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.UltraGrid_Batches.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.UltraGrid_Batches.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.SingleAutoDrag
        Me.UltraGrid_Batches.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        Appearance28.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid_Batches.DisplayLayout.Override.TemplateAddRowAppearance = Appearance28
        Me.UltraGrid_Batches.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraGrid_Batches.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraGrid_Batches.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.UltraGrid_Batches.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.UltraGrid_Batches.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraGrid_Batches.Location = New System.Drawing.Point(6, 19)
        Me.UltraGrid_Batches.Name = "UltraGrid_Batches"
        Me.UltraGrid_Batches.Size = New System.Drawing.Size(906, 334)
        Me.UltraGrid_Batches.TabIndex = 101
        Me.UltraGrid_Batches.Text = "Search Results"
        '
        'Button_Package
        '
        Me.Button_Package.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Package.Location = New System.Drawing.Point(818, 562)
        Me.Button_Package.Name = "Button_Package"
        Me.Button_Package.Size = New System.Drawing.Size(112, 42)
        Me.Button_Package.TabIndex = 0
        Me.Button_Package.Text = "Re-Package " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Batches"
        Me.Button_Package.UseVisualStyleBackColor = True
        '
        'Form_ChangeBatchState
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(942, 616)
        Me.Controls.Add(Me.GroupBox_Batches)
        Me.Controls.Add(Me.Button_Package)
        Me.Controls.Add(Me.GroupBox_BatchSearch)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_ChangeBatchState"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Change Batch State"
        Me.GroupBox_BatchSearch.ResumeLayout(False)
        Me.GroupBox_BatchSearch.PerformLayout()
        Me.GroupBox_Stores.ResumeLayout(False)
        Me.GroupBox_BatchState.ResumeLayout(False)
        Me.GroupBox_BatchState.PerformLayout()
        Me.GroupBox_Batches.ResumeLayout(False)
        CType(Me.UltraGrid_Batches, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox_BatchSearch As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox_BatchState As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBox_Ready As System.Windows.Forms.CheckBox
    Friend WithEvents Button_Search As System.Windows.Forms.Button
    Friend WithEvents GroupBox_Batches As System.Windows.Forms.GroupBox
    Friend WithEvents Button_Package As System.Windows.Forms.Button
    Friend WithEvents CheckBox_Sent As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox_Printed As System.Windows.Forms.CheckBox
    Public WithEvents GroupBox_Stores As System.Windows.Forms.GroupBox
    Public WithEvents ComboBox_State As System.Windows.Forms.ComboBox
    Public WithEvents RadioButton_State As System.Windows.Forms.RadioButton
    Public WithEvents RadioButton_SingleStore As System.Windows.Forms.RadioButton
    Public WithEvents ComboBox_Store As System.Windows.Forms.ComboBox
    Public WithEvents RadioButton_AllStores As System.Windows.Forms.RadioButton
    Public WithEvents RadioButton_Zone As System.Windows.Forms.RadioButton
    Public WithEvents ComboBox_Zones As System.Windows.Forms.ComboBox
    Public WithEvents TextBox_BatchDescription As System.Windows.Forms.TextBox
    Public WithEvents Label_BatchDesc As System.Windows.Forms.Label
    Friend WithEvents UltraGrid_Batches As Infragistics.Win.UltraWinGrid.UltraGrid
End Class
