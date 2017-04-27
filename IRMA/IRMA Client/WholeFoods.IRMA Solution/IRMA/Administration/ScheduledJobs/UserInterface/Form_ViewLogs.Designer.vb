<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ViewLogs
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
        Me.components = New System.ComponentModel.Container()
        Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("AppName")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("EnvShortName")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("LogDate", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Descending, False)
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("AppGUID")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("HostName")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("UserName")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Thread")
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Level")
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Logger")
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Message")
        Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Exception")
        Dim UltraGridColumn12 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("InsertDate")
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
        Dim UltraDataColumn1 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("AppName")
        Dim UltraDataColumn2 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("EnvShortName")
        Dim UltraDataColumn3 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("LogDate")
        Dim UltraDataColumn4 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("AppGUID")
        Dim UltraDataColumn5 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("HostName")
        Dim UltraDataColumn6 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("UserName")
        Dim UltraDataColumn7 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Thread")
        Dim UltraDataColumn8 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Level")
        Dim UltraDataColumn9 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Logger")
        Dim UltraDataColumn10 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Message")
        Dim UltraDataColumn11 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("Exception")
        Dim UltraDataColumn12 As Infragistics.Win.UltraWinDataSource.UltraDataColumn = New Infragistics.Win.UltraWinDataSource.UltraDataColumn("InsertDate")
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_ViewLogs))
        Me.ugridAppLogEntries = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.udsAppLog = New Infragistics.Win.UltraWinDataSource.UltraDataSource(Me.components)
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel2 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel3 = New Infragistics.Win.Misc.UltraLabel()
        Me.txtLogMsgDetail = New System.Windows.Forms.TextBox()
        Me.lblLogMsgDetail = New Infragistics.Win.Misc.UltraLabel()
        Me.btnViewLogEntries = New Infragistics.Win.Misc.UltraButton()
        Me.cmbAppName = New System.Windows.Forms.ComboBox()
        Me.lblLogSearchStatus = New Infragistics.Win.Misc.UltraLabel()
        Me.udteStartDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.udteEndDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.GroupBoxSearchOptions = New System.Windows.Forms.GroupBox()
        Me.rbSearchAppLog = New System.Windows.Forms.RadioButton()
        Me.rbSearchArchive = New System.Windows.Forms.RadioButton()
        Me.rbSearchBoth = New System.Windows.Forms.RadioButton()
        CType(Me.ugridAppLogEntries, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.udsAppLog, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.udteStartDate, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.udteEndDate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBoxSearchOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'ugridAppLogEntries
        '
        Me.ugridAppLogEntries.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ugridAppLogEntries.DataSource = Me.udsAppLog
        Appearance15.BackColor = System.Drawing.SystemColors.Window
        Appearance15.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugridAppLogEntries.DisplayLayout.Appearance = Appearance15
        Me.ugridAppLogEntries.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Width = 60
        UltraGridColumn2.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.Width = 96
        UltraGridColumn3.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly
        UltraGridColumn3.Format = "yyyy-MM-dd hh:mm:ss.FF tt"
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Width = 158
        UltraGridColumn4.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.Width = 65
        UltraGridColumn5.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.Width = 74
        UltraGridColumn6.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.Width = 68
        UltraGridColumn7.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn7.CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly
        UltraGridColumn7.Header.VisiblePosition = 6
        UltraGridColumn7.Width = 54
        UltraGridColumn8.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn8.CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly
        UltraGridColumn8.Header.VisiblePosition = 7
        UltraGridColumn8.Width = 59
        UltraGridColumn9.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn9.CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly
        UltraGridColumn9.Header.VisiblePosition = 8
        UltraGridColumn9.Width = 67
        UltraGridColumn10.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn10.CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly
        UltraGridColumn10.Header.VisiblePosition = 9
        UltraGridColumn10.Width = 137
        UltraGridColumn11.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn11.CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly
        UltraGridColumn11.Header.VisiblePosition = 10
        UltraGridColumn11.Width = 117
        UltraGridColumn12.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.Append
        UltraGridColumn12.CellActivation = Infragistics.Win.UltraWinGrid.Activation.ActivateOnly
        UltraGridColumn12.Header.VisiblePosition = 11
        UltraGridColumn12.Width = 74
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11, UltraGridColumn12})
        Me.ugridAppLogEntries.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugridAppLogEntries.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugridAppLogEntries.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance16.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance16.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance16.BorderColor = System.Drawing.SystemColors.Window
        Me.ugridAppLogEntries.DisplayLayout.GroupByBox.Appearance = Appearance16
        Appearance17.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugridAppLogEntries.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance17
        Me.ugridAppLogEntries.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance18.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance18.BackColor2 = System.Drawing.SystemColors.Control
        Appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance18.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugridAppLogEntries.DisplayLayout.GroupByBox.PromptAppearance = Appearance18
        Me.ugridAppLogEntries.DisplayLayout.MaxColScrollRegions = 1
        Me.ugridAppLogEntries.DisplayLayout.MaxRowScrollRegions = 1
        Appearance19.BackColor = System.Drawing.SystemColors.Window
        Appearance19.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugridAppLogEntries.DisplayLayout.Override.ActiveCellAppearance = Appearance19
        Appearance20.BackColor = System.Drawing.SystemColors.Highlight
        Appearance20.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.ugridAppLogEntries.DisplayLayout.Override.ActiveRowAppearance = Appearance20
        Me.ugridAppLogEntries.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugridAppLogEntries.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance21.BackColor = System.Drawing.SystemColors.Window
        Me.ugridAppLogEntries.DisplayLayout.Override.CardAreaAppearance = Appearance21
        Appearance22.BorderColor = System.Drawing.Color.Silver
        Appearance22.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugridAppLogEntries.DisplayLayout.Override.CellAppearance = Appearance22
        Me.ugridAppLogEntries.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugridAppLogEntries.DisplayLayout.Override.CellPadding = 0
        Appearance23.BackColor = System.Drawing.SystemColors.Control
        Appearance23.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance23.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance23.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance23.BorderColor = System.Drawing.SystemColors.Window
        Me.ugridAppLogEntries.DisplayLayout.Override.GroupByRowAppearance = Appearance23
        Appearance24.TextHAlignAsString = "Left"
        Me.ugridAppLogEntries.DisplayLayout.Override.HeaderAppearance = Appearance24
        Me.ugridAppLogEntries.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.ugridAppLogEntries.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance25.BackColor = System.Drawing.SystemColors.Window
        Appearance25.BorderColor = System.Drawing.Color.Silver
        Me.ugridAppLogEntries.DisplayLayout.Override.RowAppearance = Appearance25
        Me.ugridAppLogEntries.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Appearance26.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugridAppLogEntries.DisplayLayout.Override.TemplateAddRowAppearance = Appearance26
        Me.ugridAppLogEntries.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugridAppLogEntries.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugridAppLogEntries.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ugridAppLogEntries.Location = New System.Drawing.Point(12, 148)
        Me.ugridAppLogEntries.Name = "ugridAppLogEntries"
        Me.ugridAppLogEntries.Size = New System.Drawing.Size(1050, 319)
        Me.ugridAppLogEntries.TabIndex = 0
        Me.ugridAppLogEntries.Text = "UltraGrid1"
        '
        'udsAppLog
        '
        UltraDataColumn3.DataType = GetType(Date)
        UltraDataColumn12.DataType = GetType(Date)
        Me.udsAppLog.Band.Columns.AddRange(New Object() {UltraDataColumn1, UltraDataColumn2, UltraDataColumn3, UltraDataColumn4, UltraDataColumn5, UltraDataColumn6, UltraDataColumn7, UltraDataColumn8, UltraDataColumn9, UltraDataColumn10, UltraDataColumn11, UltraDataColumn12})
        '
        'UltraLabel1
        '
        Me.UltraLabel1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraLabel1.Location = New System.Drawing.Point(12, 20)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(100, 23)
        Me.UltraLabel1.TabIndex = 4
        Me.UltraLabel1.Text = "Application: "
        '
        'UltraLabel2
        '
        Me.UltraLabel2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraLabel2.Location = New System.Drawing.Point(12, 51)
        Me.UltraLabel2.Name = "UltraLabel2"
        Me.UltraLabel2.Size = New System.Drawing.Size(135, 23)
        Me.UltraLabel2.TabIndex = 5
        Me.UltraLabel2.Text = "Log Start Date/Time: "
        '
        'UltraLabel3
        '
        Me.UltraLabel3.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraLabel3.Location = New System.Drawing.Point(12, 81)
        Me.UltraLabel3.Name = "UltraLabel3"
        Me.UltraLabel3.Size = New System.Drawing.Size(135, 23)
        Me.UltraLabel3.TabIndex = 6
        Me.UltraLabel3.Text = "Log End Date/Time: "
        '
        'txtLogMsgDetail
        '
        Me.txtLogMsgDetail.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLogMsgDetail.Location = New System.Drawing.Point(12, 502)
        Me.txtLogMsgDetail.Multiline = True
        Me.txtLogMsgDetail.Name = "txtLogMsgDetail"
        Me.txtLogMsgDetail.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtLogMsgDetail.Size = New System.Drawing.Size(1050, 184)
        Me.txtLogMsgDetail.TabIndex = 7
        '
        'lblLogMsgDetail
        '
        Me.lblLogMsgDetail.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Appearance14.TextVAlignAsString = "Bottom"
        Me.lblLogMsgDetail.Appearance = Appearance14
        Me.lblLogMsgDetail.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLogMsgDetail.Location = New System.Drawing.Point(12, 473)
        Me.lblLogMsgDetail.Name = "lblLogMsgDetail"
        Me.lblLogMsgDetail.Size = New System.Drawing.Size(189, 23)
        Me.lblLogMsgDetail.TabIndex = 8
        Me.lblLogMsgDetail.Text = "Log Message Detail"
        '
        'btnViewLogEntries
        '
        Me.btnViewLogEntries.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnViewLogEntries.Location = New System.Drawing.Point(12, 108)
        Me.btnViewLogEntries.Name = "btnViewLogEntries"
        Me.btnViewLogEntries.Size = New System.Drawing.Size(285, 34)
        Me.btnViewLogEntries.TabIndex = 9
        Me.btnViewLogEntries.Text = "View Log Entries"
        '
        'cmbAppName
        '
        Me.cmbAppName.FormattingEnabled = True
        Me.cmbAppName.Location = New System.Drawing.Point(118, 17)
        Me.cmbAppName.Name = "cmbAppName"
        Me.cmbAppName.Size = New System.Drawing.Size(179, 21)
        Me.cmbAppName.TabIndex = 10
        '
        'lblLogSearchStatus
        '
        Appearance13.ForeColor = System.Drawing.Color.Chocolate
        Me.lblLogSearchStatus.Appearance = Appearance13
        Me.lblLogSearchStatus.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLogSearchStatus.Location = New System.Drawing.Point(303, 113)
        Me.lblLogSearchStatus.Name = "lblLogSearchStatus"
        Me.lblLogSearchStatus.Size = New System.Drawing.Size(204, 18)
        Me.lblLogSearchStatus.TabIndex = 11
        Me.lblLogSearchStatus.Text = "Log Entries Found: ?"
        '
        'udteStartDate
        '
        Me.udteStartDate.FormatProvider = New System.Globalization.CultureInfo("en-US")
        Me.udteStartDate.Location = New System.Drawing.Point(153, 46)
        Me.udteStartDate.MaskInput = "{date} {time}"
        Me.udteStartDate.Name = "udteStartDate"
        Me.udteStartDate.Size = New System.Drawing.Size(144, 24)
        Me.udteStartDate.TabIndex = 16
        '
        'udteEndDate
        '
        Me.udteEndDate.FormatProvider = New System.Globalization.CultureInfo("en-US")
        Me.udteEndDate.Location = New System.Drawing.Point(153, 76)
        Me.udteEndDate.MaskInput = "{date} {time}"
        Me.udteEndDate.Name = "udteEndDate"
        Me.udteEndDate.Size = New System.Drawing.Size(144, 24)
        Me.udteEndDate.TabIndex = 17
        '
        'GroupBoxSearchOptions
        '
        Me.GroupBoxSearchOptions.Controls.Add(Me.rbSearchBoth)
        Me.GroupBoxSearchOptions.Controls.Add(Me.rbSearchArchive)
        Me.GroupBoxSearchOptions.Controls.Add(Me.rbSearchAppLog)
        Me.GroupBoxSearchOptions.Location = New System.Drawing.Point(325, 15)
        Me.GroupBoxSearchOptions.Name = "GroupBoxSearchOptions"
        Me.GroupBoxSearchOptions.Size = New System.Drawing.Size(143, 89)
        Me.GroupBoxSearchOptions.TabIndex = 18
        Me.GroupBoxSearchOptions.TabStop = False
        Me.GroupBoxSearchOptions.Text = "Search  Options"
        '
        'rbSearchAppLog
        '
        Me.rbSearchAppLog.AutoSize = True
        Me.rbSearchAppLog.Checked = True
        Me.rbSearchAppLog.Location = New System.Drawing.Point(10, 17)
        Me.rbSearchAppLog.Name = "rbSearchAppLog"
        Me.rbSearchAppLog.Size = New System.Drawing.Size(131, 17)
        Me.rbSearchAppLog.TabIndex = 0
        Me.rbSearchAppLog.TabStop = True
        Me.rbSearchAppLog.Text = "Search AppLog table"
        Me.rbSearchAppLog.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.rbSearchAppLog.UseVisualStyleBackColor = True
        '
        'rbSearchArchive
        '
        Me.rbSearchArchive.AutoSize = True
        Me.rbSearchArchive.Location = New System.Drawing.Point(10, 41)
        Me.rbSearchArchive.Name = "rbSearchArchive"
        Me.rbSearchArchive.Size = New System.Drawing.Size(128, 17)
        Me.rbSearchArchive.TabIndex = 1
        Me.rbSearchArchive.Text = "Search Archive table"
        Me.rbSearchArchive.UseVisualStyleBackColor = True
        '
        'rbSearchBoth
        '
        Me.rbSearchBoth.AutoSize = True
        Me.rbSearchBoth.Location = New System.Drawing.Point(10, 64)
        Me.rbSearchBoth.Name = "rbSearchBoth"
        Me.rbSearchBoth.Size = New System.Drawing.Size(121, 17)
        Me.rbSearchBoth.TabIndex = 2
        Me.rbSearchBoth.Text = "Search Both tables"
        Me.rbSearchBoth.UseVisualStyleBackColor = True
        '
        'Form_ViewLogs
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1074, 697)
        Me.Controls.Add(Me.GroupBoxSearchOptions)
        Me.Controls.Add(Me.udteEndDate)
        Me.Controls.Add(Me.udteStartDate)
        Me.Controls.Add(Me.lblLogSearchStatus)
        Me.Controls.Add(Me.cmbAppName)
        Me.Controls.Add(Me.btnViewLogEntries)
        Me.Controls.Add(Me.lblLogMsgDetail)
        Me.Controls.Add(Me.txtLogMsgDetail)
        Me.Controls.Add(Me.UltraLabel3)
        Me.Controls.Add(Me.UltraLabel2)
        Me.Controls.Add(Me.UltraLabel1)
        Me.Controls.Add(Me.ugridAppLogEntries)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimizeBox = False
        Me.Name = "Form_ViewLogs"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Log Viewer"
        CType(Me.ugridAppLogEntries, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.udsAppLog, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.udteStartDate, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.udteEndDate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBoxSearchOptions.ResumeLayout(False)
        Me.GroupBoxSearchOptions.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ugridAppLogEntries As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel2 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel3 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents udsAppLog As Infragistics.Win.UltraWinDataSource.UltraDataSource
    Friend WithEvents txtLogMsgDetail As System.Windows.Forms.TextBox
    Friend WithEvents lblLogMsgDetail As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents btnViewLogEntries As Infragistics.Win.Misc.UltraButton
    Friend WithEvents cmbAppName As System.Windows.Forms.ComboBox
    Friend WithEvents lblLogSearchStatus As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents udteStartDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents udteEndDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents GroupBoxSearchOptions As System.Windows.Forms.GroupBox
    Friend WithEvents rbSearchBoth As System.Windows.Forms.RadioButton
    Friend WithEvents rbSearchArchive As System.Windows.Forms.RadioButton
    Friend WithEvents rbSearchAppLog As System.Windows.Forms.RadioButton
End Class
