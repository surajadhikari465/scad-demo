<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_AddFileWriter
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

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance10 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance11 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance12 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Me.Properties_Button_OK = New System.Windows.Forms.Button
        Me.Properties_Button_Cancel = New System.Windows.Forms.Button
        Me.Properties_GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Properties_GroupBox_BatchId = New System.Windows.Forms.GroupBox
        Me.Properties_TextBox_MaxBatchId = New System.Windows.Forms.TextBox
        Me.Properties_TextBox_MinBatchId = New System.Windows.Forms.TextBox
        Me.Properties_Label_MaxBatchId = New System.Windows.Forms.Label
        Me.Properties_Label_MinBatchId = New System.Windows.Forms.Label
        Me.Properties_CheckBox_POSSectionHeader = New System.Windows.Forms.CheckBox
        Me.Label_ScaleType = New System.Windows.Forms.Label
        Me.ComboBox_ScaleWriterType = New System.Windows.Forms.ComboBox
        Me.ComboBox_FileWriterType = New System.Windows.Forms.ComboBox
        Me.Label_FileWriterType = New System.Windows.Forms.Label
        Me.ComboBox_FileWriterClass = New System.Windows.Forms.ComboBox
        Me.Properties_CheckBox_AppendToFile = New System.Windows.Forms.CheckBox
        Me.Label_FileWriterCode = New System.Windows.Forms.Label
        Me.GroupBox_EscapeChars = New System.Windows.Forms.GroupBox
        Me.UltraGrid_EscapeChars = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me.TextBox_FileWriterCodeVal = New System.Windows.Forms.TextBox
        Me.Properties_CheckBox_FixedWidthVal = New System.Windows.Forms.CheckBox
        Me.GroupBox_Delimiter = New System.Windows.Forms.GroupBox
        Me.Properties_CheckBox_FieldIdDelimiter = New System.Windows.Forms.CheckBox
        Me.Properties_CheckBox_TrailingDelimiter = New System.Windows.Forms.CheckBox
        Me.Properties_CheckBox_LeadingDelimiter = New System.Windows.Forms.CheckBox
        Me.Properties_Label_DelimChar = New System.Windows.Forms.Label
        Me.Properties_TextBox_DelimCharVal = New System.Windows.Forms.TextBox
        Me.Properties_CheckBox_EnforceDictionary = New System.Windows.Forms.CheckBox
        Me.GroupBox_TaxFlag = New System.Windows.Forms.GroupBox
        Me.Properties_TextBox_TaxFlagFalseVal = New System.Windows.Forms.TextBox
        Me.Properties_TextBox_TaxFlagTrueVal = New System.Windows.Forms.TextBox
        Me.Properties_Label_TaxFlagFalse = New System.Windows.Forms.Label
        Me.Properties_Label_TaxFlagTrue = New System.Windows.Forms.Label
        Me.Properties_Label_WriterClass = New System.Windows.Forms.Label
        Me.Properties_GroupBox1.SuspendLayout()
        Me.Properties_GroupBox_BatchId.SuspendLayout()
        Me.GroupBox_EscapeChars.SuspendLayout()
        CType(Me.UltraGrid_EscapeChars, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_Delimiter.SuspendLayout()
        Me.GroupBox_TaxFlag.SuspendLayout()
        Me.SuspendLayout()
        '
        'Properties_Button_OK
        '
        Me.Properties_Button_OK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Properties_Button_OK.Location = New System.Drawing.Point(468, 470)
        Me.Properties_Button_OK.Name = "Properties_Button_OK"
        Me.Properties_Button_OK.Size = New System.Drawing.Size(75, 23)
        Me.Properties_Button_OK.TabIndex = 18
        Me.Properties_Button_OK.Text = "OK"
        Me.Properties_Button_OK.UseVisualStyleBackColor = True
        '
        'Properties_Button_Cancel
        '
        Me.Properties_Button_Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Properties_Button_Cancel.Location = New System.Drawing.Point(550, 470)
        Me.Properties_Button_Cancel.Name = "Properties_Button_Cancel"
        Me.Properties_Button_Cancel.Size = New System.Drawing.Size(75, 23)
        Me.Properties_Button_Cancel.TabIndex = 19
        Me.Properties_Button_Cancel.Text = "Cancel"
        Me.Properties_Button_Cancel.UseVisualStyleBackColor = True
        '
        'Properties_GroupBox1
        '
        Me.Properties_GroupBox1.Controls.Add(Me.Properties_GroupBox_BatchId)
        Me.Properties_GroupBox1.Controls.Add(Me.Properties_CheckBox_POSSectionHeader)
        Me.Properties_GroupBox1.Controls.Add(Me.Label_ScaleType)
        Me.Properties_GroupBox1.Controls.Add(Me.ComboBox_ScaleWriterType)
        Me.Properties_GroupBox1.Controls.Add(Me.ComboBox_FileWriterType)
        Me.Properties_GroupBox1.Controls.Add(Me.Label_FileWriterType)
        Me.Properties_GroupBox1.Controls.Add(Me.ComboBox_FileWriterClass)
        Me.Properties_GroupBox1.Controls.Add(Me.Properties_CheckBox_AppendToFile)
        Me.Properties_GroupBox1.Controls.Add(Me.Label_FileWriterCode)
        Me.Properties_GroupBox1.Controls.Add(Me.GroupBox_EscapeChars)
        Me.Properties_GroupBox1.Controls.Add(Me.TextBox_FileWriterCodeVal)
        Me.Properties_GroupBox1.Controls.Add(Me.Properties_CheckBox_FixedWidthVal)
        Me.Properties_GroupBox1.Controls.Add(Me.GroupBox_Delimiter)
        Me.Properties_GroupBox1.Controls.Add(Me.Properties_CheckBox_EnforceDictionary)
        Me.Properties_GroupBox1.Controls.Add(Me.GroupBox_TaxFlag)
        Me.Properties_GroupBox1.Controls.Add(Me.Properties_Label_WriterClass)
        Me.Properties_GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.Properties_GroupBox1.Name = "Properties_GroupBox1"
        Me.Properties_GroupBox1.Size = New System.Drawing.Size(611, 449)
        Me.Properties_GroupBox1.TabIndex = 0
        Me.Properties_GroupBox1.TabStop = False
        '
        'Properties_GroupBox_BatchId
        '
        Me.Properties_GroupBox_BatchId.Controls.Add(Me.Properties_TextBox_MaxBatchId)
        Me.Properties_GroupBox_BatchId.Controls.Add(Me.Properties_TextBox_MinBatchId)
        Me.Properties_GroupBox_BatchId.Controls.Add(Me.Properties_Label_MaxBatchId)
        Me.Properties_GroupBox_BatchId.Controls.Add(Me.Properties_Label_MinBatchId)
        Me.Properties_GroupBox_BatchId.Location = New System.Drawing.Point(15, 366)
        Me.Properties_GroupBox_BatchId.Name = "Properties_GroupBox_BatchId"
        Me.Properties_GroupBox_BatchId.Size = New System.Drawing.Size(250, 70)
        Me.Properties_GroupBox_BatchId.TabIndex = 32
        Me.Properties_GroupBox_BatchId.TabStop = False
        Me.Properties_GroupBox_BatchId.Text = "Batch ID Settings"
        '
        'Properties_TextBox_MaxBatchId
        '
        Me.Properties_TextBox_MaxBatchId.Location = New System.Drawing.Point(105, 42)
        Me.Properties_TextBox_MaxBatchId.MaxLength = 5
        Me.Properties_TextBox_MaxBatchId.Name = "Properties_TextBox_MaxBatchId"
        Me.Properties_TextBox_MaxBatchId.Size = New System.Drawing.Size(56, 22)
        Me.Properties_TextBox_MaxBatchId.TabIndex = 17
        '
        'Properties_TextBox_MinBatchId
        '
        Me.Properties_TextBox_MinBatchId.Location = New System.Drawing.Point(105, 16)
        Me.Properties_TextBox_MinBatchId.MaxLength = 5
        Me.Properties_TextBox_MinBatchId.Name = "Properties_TextBox_MinBatchId"
        Me.Properties_TextBox_MinBatchId.Size = New System.Drawing.Size(56, 22)
        Me.Properties_TextBox_MinBatchId.TabIndex = 16
        '
        'Properties_Label_MaxBatchId
        '
        Me.Properties_Label_MaxBatchId.AutoSize = True
        Me.Properties_Label_MaxBatchId.Location = New System.Drawing.Point(6, 45)
        Me.Properties_Label_MaxBatchId.Name = "Properties_Label_MaxBatchId"
        Me.Properties_Label_MaxBatchId.Size = New System.Drawing.Size(74, 13)
        Me.Properties_Label_MaxBatchId.TabIndex = 2
        Me.Properties_Label_MaxBatchId.Text = "Max Batch ID"
        '
        'Properties_Label_MinBatchId
        '
        Me.Properties_Label_MinBatchId.AutoSize = True
        Me.Properties_Label_MinBatchId.Location = New System.Drawing.Point(6, 19)
        Me.Properties_Label_MinBatchId.Name = "Properties_Label_MinBatchId"
        Me.Properties_Label_MinBatchId.Size = New System.Drawing.Size(73, 13)
        Me.Properties_Label_MinBatchId.TabIndex = 0
        Me.Properties_Label_MinBatchId.Text = "Min Batch ID"
        '
        'Properties_CheckBox_POSSectionHeader
        '
        Me.Properties_CheckBox_POSSectionHeader.AutoSize = True
        Me.Properties_CheckBox_POSSectionHeader.Location = New System.Drawing.Point(24, 74)
        Me.Properties_CheckBox_POSSectionHeader.Name = "Properties_CheckBox_POSSectionHeader"
        Me.Properties_CheckBox_POSSectionHeader.Size = New System.Drawing.Size(288, 17)
        Me.Properties_CheckBox_POSSectionHeader.TabIndex = 5
        Me.Properties_CheckBox_POSSectionHeader.Text = "Generate POS section headers for each IRMA batch"
        Me.Properties_CheckBox_POSSectionHeader.UseVisualStyleBackColor = True
        '
        'Label_ScaleType
        '
        Me.Label_ScaleType.AutoSize = True
        Me.Label_ScaleType.Location = New System.Drawing.Point(325, 22)
        Me.Label_ScaleType.Name = "Label_ScaleType"
        Me.Label_ScaleType.Size = New System.Drawing.Size(94, 13)
        Me.Label_ScaleType.TabIndex = 30
        Me.Label_ScaleType.Text = "Scale Writer Type"
        '
        'ComboBox_ScaleWriterType
        '
        Me.ComboBox_ScaleWriterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_ScaleWriterType.FormattingEnabled = True
        Me.ComboBox_ScaleWriterType.Location = New System.Drawing.Point(455, 19)
        Me.ComboBox_ScaleWriterType.Name = "ComboBox_ScaleWriterType"
        Me.ComboBox_ScaleWriterType.Size = New System.Drawing.Size(143, 21)
        Me.ComboBox_ScaleWriterType.TabIndex = 3
        '
        'ComboBox_FileWriterType
        '
        Me.ComboBox_FileWriterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_FileWriterType.FormattingEnabled = True
        Me.ComboBox_FileWriterType.Location = New System.Drawing.Point(147, 19)
        Me.ComboBox_FileWriterType.Name = "ComboBox_FileWriterType"
        Me.ComboBox_FileWriterType.Size = New System.Drawing.Size(143, 21)
        Me.ComboBox_FileWriterType.TabIndex = 1
        '
        'Label_FileWriterType
        '
        Me.Label_FileWriterType.AutoSize = True
        Me.Label_FileWriterType.Location = New System.Drawing.Point(17, 22)
        Me.Label_FileWriterType.Name = "Label_FileWriterType"
        Me.Label_FileWriterType.Size = New System.Drawing.Size(86, 13)
        Me.Label_FileWriterType.TabIndex = 27
        Me.Label_FileWriterType.Text = "File Writer Type"
        '
        'ComboBox_FileWriterClass
        '
        Me.ComboBox_FileWriterClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_FileWriterClass.FormattingEnabled = True
        Me.ComboBox_FileWriterClass.Location = New System.Drawing.Point(455, 46)
        Me.ComboBox_FileWriterClass.Name = "ComboBox_FileWriterClass"
        Me.ComboBox_FileWriterClass.Size = New System.Drawing.Size(143, 21)
        Me.ComboBox_FileWriterClass.TabIndex = 4
        '
        'Properties_CheckBox_AppendToFile
        '
        Me.Properties_CheckBox_AppendToFile.AutoSize = True
        Me.Properties_CheckBox_AppendToFile.Location = New System.Drawing.Point(24, 143)
        Me.Properties_CheckBox_AppendToFile.Name = "Properties_CheckBox_AppendToFile"
        Me.Properties_CheckBox_AppendToFile.Size = New System.Drawing.Size(201, 17)
        Me.Properties_CheckBox_AppendToFile.TabIndex = 8
        Me.Properties_CheckBox_AppendToFile.Text = "Append To File? (no = replace file)"
        Me.Properties_CheckBox_AppendToFile.UseVisualStyleBackColor = True
        '
        'Label_FileWriterCode
        '
        Me.Label_FileWriterCode.AutoSize = True
        Me.Label_FileWriterCode.Location = New System.Drawing.Point(17, 49)
        Me.Label_FileWriterCode.Name = "Label_FileWriterCode"
        Me.Label_FileWriterCode.Size = New System.Drawing.Size(90, 13)
        Me.Label_FileWriterCode.TabIndex = 25
        Me.Label_FileWriterCode.Text = "File Writer Code"
        '
        'GroupBox_EscapeChars
        '
        Me.GroupBox_EscapeChars.Controls.Add(Me.UltraGrid_EscapeChars)
        Me.GroupBox_EscapeChars.Location = New System.Drawing.Point(294, 166)
        Me.GroupBox_EscapeChars.Name = "GroupBox_EscapeChars"
        Me.GroupBox_EscapeChars.Size = New System.Drawing.Size(246, 194)
        Me.GroupBox_EscapeChars.TabIndex = 16
        Me.GroupBox_EscapeChars.TabStop = False
        Me.GroupBox_EscapeChars.Text = "Escape Characters"
        '
        'UltraGrid_EscapeChars
        '
        Appearance1.BackColor = System.Drawing.SystemColors.Window
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.UltraGrid_EscapeChars.DisplayLayout.Appearance = Appearance1
        Me.UltraGrid_EscapeChars.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        Me.UltraGrid_EscapeChars.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_EscapeChars.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance2.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_EscapeChars.DisplayLayout.GroupByBox.Appearance = Appearance2
        Appearance3.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_EscapeChars.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance3
        Me.UltraGrid_EscapeChars.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UltraGrid_EscapeChars.DisplayLayout.GroupByBox.Hidden = True
        Appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance4.BackColor2 = System.Drawing.SystemColors.Control
        Appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.UltraGrid_EscapeChars.DisplayLayout.GroupByBox.PromptAppearance = Appearance4
        Me.UltraGrid_EscapeChars.DisplayLayout.MaxColScrollRegions = 1
        Me.UltraGrid_EscapeChars.DisplayLayout.MaxRowScrollRegions = 1
        Appearance5.BackColor = System.Drawing.SystemColors.Window
        Appearance5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UltraGrid_EscapeChars.DisplayLayout.Override.ActiveCellAppearance = Appearance5
        Appearance6.BackColor = System.Drawing.SystemColors.Highlight
        Appearance6.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.UltraGrid_EscapeChars.DisplayLayout.Override.ActiveRowAppearance = Appearance6
        Me.UltraGrid_EscapeChars.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.TemplateOnTop
        Me.UltraGrid_EscapeChars.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[True]
        Me.UltraGrid_EscapeChars.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[True]
        Me.UltraGrid_EscapeChars.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.UltraGrid_EscapeChars.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_EscapeChars.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.UltraGrid_EscapeChars.DisplayLayout.Override.CellAppearance = Appearance8
        Me.UltraGrid_EscapeChars.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.UltraGrid_EscapeChars.DisplayLayout.Override.CellPadding = 0
        Appearance9.BackColor = System.Drawing.SystemColors.Control
        Appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance9.BorderColor = System.Drawing.SystemColors.Window
        Me.UltraGrid_EscapeChars.DisplayLayout.Override.GroupByRowAppearance = Appearance9
        Appearance10.TextHAlign = Infragistics.Win.HAlign.Left
        Me.UltraGrid_EscapeChars.DisplayLayout.Override.HeaderAppearance = Appearance10
        Me.UltraGrid_EscapeChars.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.UltraGrid_EscapeChars.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance11.BackColor = System.Drawing.SystemColors.Window
        Appearance11.BorderColor = System.Drawing.Color.Silver
        Me.UltraGrid_EscapeChars.DisplayLayout.Override.RowAppearance = Appearance11
        Me.UltraGrid_EscapeChars.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Appearance12.BackColor = System.Drawing.SystemColors.ControlLight
        Me.UltraGrid_EscapeChars.DisplayLayout.Override.TemplateAddRowAppearance = Appearance12
        Me.UltraGrid_EscapeChars.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UltraGrid_EscapeChars.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UltraGrid_EscapeChars.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.UltraGrid_EscapeChars.Location = New System.Drawing.Point(7, 20)
        Me.UltraGrid_EscapeChars.Name = "UltraGrid_EscapeChars"
        Me.UltraGrid_EscapeChars.Size = New System.Drawing.Size(233, 167)
        Me.UltraGrid_EscapeChars.TabIndex = 15
        Me.UltraGrid_EscapeChars.Text = "UltraGrid1"
        '
        'TextBox_FileWriterCodeVal
        '
        Me.TextBox_FileWriterCodeVal.Location = New System.Drawing.Point(147, 46)
        Me.TextBox_FileWriterCodeVal.MaxLength = 20
        Me.TextBox_FileWriterCodeVal.Name = "TextBox_FileWriterCodeVal"
        Me.TextBox_FileWriterCodeVal.Size = New System.Drawing.Size(143, 22)
        Me.TextBox_FileWriterCodeVal.TabIndex = 2
        '
        'Properties_CheckBox_FixedWidthVal
        '
        Me.Properties_CheckBox_FixedWidthVal.AutoSize = True
        Me.Properties_CheckBox_FixedWidthVal.Location = New System.Drawing.Point(24, 97)
        Me.Properties_CheckBox_FixedWidthVal.Name = "Properties_CheckBox_FixedWidthVal"
        Me.Properties_CheckBox_FixedWidthVal.Size = New System.Drawing.Size(88, 17)
        Me.Properties_CheckBox_FixedWidthVal.TabIndex = 6
        Me.Properties_CheckBox_FixedWidthVal.Text = "Fixed Width"
        Me.Properties_CheckBox_FixedWidthVal.UseVisualStyleBackColor = True
        '
        'GroupBox_Delimiter
        '
        Me.GroupBox_Delimiter.Controls.Add(Me.Properties_CheckBox_FieldIdDelimiter)
        Me.GroupBox_Delimiter.Controls.Add(Me.Properties_CheckBox_TrailingDelimiter)
        Me.GroupBox_Delimiter.Controls.Add(Me.Properties_CheckBox_LeadingDelimiter)
        Me.GroupBox_Delimiter.Controls.Add(Me.Properties_Label_DelimChar)
        Me.GroupBox_Delimiter.Controls.Add(Me.Properties_TextBox_DelimCharVal)
        Me.GroupBox_Delimiter.Location = New System.Drawing.Point(15, 165)
        Me.GroupBox_Delimiter.Name = "GroupBox_Delimiter"
        Me.GroupBox_Delimiter.Size = New System.Drawing.Size(250, 117)
        Me.GroupBox_Delimiter.TabIndex = 8
        Me.GroupBox_Delimiter.TabStop = False
        Me.GroupBox_Delimiter.Text = "Delimiter Settings"
        '
        'Properties_CheckBox_FieldIdDelimiter
        '
        Me.Properties_CheckBox_FieldIdDelimiter.AutoSize = True
        Me.Properties_CheckBox_FieldIdDelimiter.Location = New System.Drawing.Point(9, 92)
        Me.Properties_CheckBox_FieldIdDelimiter.Name = "Properties_CheckBox_FieldIdDelimiter"
        Me.Properties_CheckBox_FieldIdDelimiter.Size = New System.Drawing.Size(256, 17)
        Me.Properties_CheckBox_FieldIdDelimiter.TabIndex = 12
        Me.Properties_CheckBox_FieldIdDelimiter.Text = "Delimiter between Field ID and Data Element"
        Me.Properties_CheckBox_FieldIdDelimiter.UseVisualStyleBackColor = True
        '
        'Properties_CheckBox_TrailingDelimiter
        '
        Me.Properties_CheckBox_TrailingDelimiter.AutoSize = True
        Me.Properties_CheckBox_TrailingDelimiter.Location = New System.Drawing.Point(9, 69)
        Me.Properties_CheckBox_TrailingDelimiter.Name = "Properties_CheckBox_TrailingDelimiter"
        Me.Properties_CheckBox_TrailingDelimiter.Size = New System.Drawing.Size(113, 17)
        Me.Properties_CheckBox_TrailingDelimiter.TabIndex = 11
        Me.Properties_CheckBox_TrailingDelimiter.Text = "Trailing Delimiter"
        Me.Properties_CheckBox_TrailingDelimiter.UseVisualStyleBackColor = True
        '
        'Properties_CheckBox_LeadingDelimiter
        '
        Me.Properties_CheckBox_LeadingDelimiter.AutoSize = True
        Me.Properties_CheckBox_LeadingDelimiter.Location = New System.Drawing.Point(9, 46)
        Me.Properties_CheckBox_LeadingDelimiter.Name = "Properties_CheckBox_LeadingDelimiter"
        Me.Properties_CheckBox_LeadingDelimiter.Size = New System.Drawing.Size(116, 17)
        Me.Properties_CheckBox_LeadingDelimiter.TabIndex = 10
        Me.Properties_CheckBox_LeadingDelimiter.Text = "Leading Delimiter"
        Me.Properties_CheckBox_LeadingDelimiter.UseVisualStyleBackColor = True
        '
        'Properties_Label_DelimChar
        '
        Me.Properties_Label_DelimChar.AutoSize = True
        Me.Properties_Label_DelimChar.Location = New System.Drawing.Point(6, 23)
        Me.Properties_Label_DelimChar.Name = "Properties_Label_DelimChar"
        Me.Properties_Label_DelimChar.Size = New System.Drawing.Size(105, 13)
        Me.Properties_Label_DelimChar.TabIndex = 16
        Me.Properties_Label_DelimChar.Text = "Delimiter Character"
        '
        'Properties_TextBox_DelimCharVal
        '
        Me.Properties_TextBox_DelimCharVal.Location = New System.Drawing.Point(117, 20)
        Me.Properties_TextBox_DelimCharVal.MaxLength = 1
        Me.Properties_TextBox_DelimCharVal.Name = "Properties_TextBox_DelimCharVal"
        Me.Properties_TextBox_DelimCharVal.Size = New System.Drawing.Size(32, 22)
        Me.Properties_TextBox_DelimCharVal.TabIndex = 9
        '
        'Properties_CheckBox_EnforceDictionary
        '
        Me.Properties_CheckBox_EnforceDictionary.AutoSize = True
        Me.Properties_CheckBox_EnforceDictionary.Location = New System.Drawing.Point(24, 120)
        Me.Properties_CheckBox_EnforceDictionary.Name = "Properties_CheckBox_EnforceDictionary"
        Me.Properties_CheckBox_EnforceDictionary.Size = New System.Drawing.Size(147, 17)
        Me.Properties_CheckBox_EnforceDictionary.TabIndex = 7
        Me.Properties_CheckBox_EnforceDictionary.Text = "Enforce Data Dictionary"
        Me.Properties_CheckBox_EnforceDictionary.UseVisualStyleBackColor = True
        '
        'GroupBox_TaxFlag
        '
        Me.GroupBox_TaxFlag.Controls.Add(Me.Properties_TextBox_TaxFlagFalseVal)
        Me.GroupBox_TaxFlag.Controls.Add(Me.Properties_TextBox_TaxFlagTrueVal)
        Me.GroupBox_TaxFlag.Controls.Add(Me.Properties_Label_TaxFlagFalse)
        Me.GroupBox_TaxFlag.Controls.Add(Me.Properties_Label_TaxFlagTrue)
        Me.GroupBox_TaxFlag.Location = New System.Drawing.Point(15, 288)
        Me.GroupBox_TaxFlag.Name = "GroupBox_TaxFlag"
        Me.GroupBox_TaxFlag.Size = New System.Drawing.Size(250, 72)
        Me.GroupBox_TaxFlag.TabIndex = 13
        Me.GroupBox_TaxFlag.TabStop = False
        Me.GroupBox_TaxFlag.Text = "Tax Flag Settings"
        '
        'Properties_TextBox_TaxFlagFalseVal
        '
        Me.Properties_TextBox_TaxFlagFalseVal.Location = New System.Drawing.Point(105, 45)
        Me.Properties_TextBox_TaxFlagFalseVal.MaxLength = 1
        Me.Properties_TextBox_TaxFlagFalseVal.Name = "Properties_TextBox_TaxFlagFalseVal"
        Me.Properties_TextBox_TaxFlagFalseVal.Size = New System.Drawing.Size(32, 22)
        Me.Properties_TextBox_TaxFlagFalseVal.TabIndex = 14
        '
        'Properties_TextBox_TaxFlagTrueVal
        '
        Me.Properties_TextBox_TaxFlagTrueVal.Location = New System.Drawing.Point(105, 19)
        Me.Properties_TextBox_TaxFlagTrueVal.MaxLength = 1
        Me.Properties_TextBox_TaxFlagTrueVal.Name = "Properties_TextBox_TaxFlagTrueVal"
        Me.Properties_TextBox_TaxFlagTrueVal.Size = New System.Drawing.Size(32, 22)
        Me.Properties_TextBox_TaxFlagTrueVal.TabIndex = 13
        '
        'Properties_Label_TaxFlagFalse
        '
        Me.Properties_Label_TaxFlagFalse.AutoSize = True
        Me.Properties_Label_TaxFlagFalse.Location = New System.Drawing.Point(6, 48)
        Me.Properties_Label_TaxFlagFalse.Name = "Properties_Label_TaxFlagFalse"
        Me.Properties_Label_TaxFlagFalse.Size = New System.Drawing.Size(85, 13)
        Me.Properties_Label_TaxFlagFalse.TabIndex = 1
        Me.Properties_Label_TaxFlagFalse.Text = "False Character"
        '
        'Properties_Label_TaxFlagTrue
        '
        Me.Properties_Label_TaxFlagTrue.AutoSize = True
        Me.Properties_Label_TaxFlagTrue.Location = New System.Drawing.Point(6, 22)
        Me.Properties_Label_TaxFlagTrue.Name = "Properties_Label_TaxFlagTrue"
        Me.Properties_Label_TaxFlagTrue.Size = New System.Drawing.Size(81, 13)
        Me.Properties_Label_TaxFlagTrue.TabIndex = 0
        Me.Properties_Label_TaxFlagTrue.Text = "True Character"
        '
        'Properties_Label_WriterClass
        '
        Me.Properties_Label_WriterClass.AutoSize = True
        Me.Properties_Label_WriterClass.Location = New System.Drawing.Point(325, 49)
        Me.Properties_Label_WriterClass.Name = "Properties_Label_WriterClass"
        Me.Properties_Label_WriterClass.Size = New System.Drawing.Size(106, 13)
        Me.Properties_Label_WriterClass.TabIndex = 14
        Me.Properties_Label_WriterClass.Text = "VB.Net Writer Class"
        '
        'Form_AddFileWriter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(637, 499)
        Me.Controls.Add(Me.Properties_Button_OK)
        Me.Controls.Add(Me.Properties_Button_Cancel)
        Me.Controls.Add(Me.Properties_GroupBox1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_AddFileWriter"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Add File Writer"
        Me.Properties_GroupBox1.ResumeLayout(False)
        Me.Properties_GroupBox1.PerformLayout()
        Me.Properties_GroupBox_BatchId.ResumeLayout(False)
        Me.Properties_GroupBox_BatchId.PerformLayout()
        Me.GroupBox_EscapeChars.ResumeLayout(False)
        CType(Me.UltraGrid_EscapeChars, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_Delimiter.ResumeLayout(False)
        Me.GroupBox_Delimiter.PerformLayout()
        Me.GroupBox_TaxFlag.ResumeLayout(False)
        Me.GroupBox_TaxFlag.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Properties_Button_OK As System.Windows.Forms.Button
    Friend WithEvents Properties_Button_Cancel As System.Windows.Forms.Button
    Friend WithEvents Properties_GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox_EscapeChars As System.Windows.Forms.GroupBox
    Friend WithEvents UltraGrid_EscapeChars As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents Properties_CheckBox_FixedWidthVal As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox_Delimiter As System.Windows.Forms.GroupBox
    Friend WithEvents Properties_CheckBox_TrailingDelimiter As System.Windows.Forms.CheckBox
    Friend WithEvents Properties_CheckBox_LeadingDelimiter As System.Windows.Forms.CheckBox
    Friend WithEvents Properties_Label_DelimChar As System.Windows.Forms.Label
    Friend WithEvents Properties_TextBox_DelimCharVal As System.Windows.Forms.TextBox
    Friend WithEvents Properties_CheckBox_EnforceDictionary As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox_TaxFlag As System.Windows.Forms.GroupBox
    Friend WithEvents Properties_TextBox_TaxFlagFalseVal As System.Windows.Forms.TextBox
    Friend WithEvents Properties_TextBox_TaxFlagTrueVal As System.Windows.Forms.TextBox
    Friend WithEvents Properties_Label_TaxFlagFalse As System.Windows.Forms.Label
    Friend WithEvents Properties_Label_TaxFlagTrue As System.Windows.Forms.Label
    Friend WithEvents Properties_Label_WriterClass As System.Windows.Forms.Label
    Friend WithEvents Label_FileWriterCode As System.Windows.Forms.Label
    Friend WithEvents TextBox_FileWriterCodeVal As System.Windows.Forms.TextBox
    Friend WithEvents Properties_CheckBox_AppendToFile As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBox_FileWriterClass As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBox_FileWriterType As System.Windows.Forms.ComboBox
    Friend WithEvents Label_FileWriterType As System.Windows.Forms.Label
    Friend WithEvents Label_ScaleType As System.Windows.Forms.Label
    Friend WithEvents ComboBox_ScaleWriterType As System.Windows.Forms.ComboBox
    Friend WithEvents Properties_CheckBox_FieldIdDelimiter As System.Windows.Forms.CheckBox
    Friend WithEvents Properties_CheckBox_POSSectionHeader As System.Windows.Forms.CheckBox
    Friend WithEvents Properties_GroupBox_BatchId As System.Windows.Forms.GroupBox
    Friend WithEvents Properties_Label_MaxBatchId As System.Windows.Forms.Label
    Friend WithEvents Properties_Label_MinBatchId As System.Windows.Forms.Label
    Friend WithEvents Properties_TextBox_MinBatchId As System.Windows.Forms.TextBox
    Friend WithEvents Properties_TextBox_MaxBatchId As System.Windows.Forms.TextBox
End Class
