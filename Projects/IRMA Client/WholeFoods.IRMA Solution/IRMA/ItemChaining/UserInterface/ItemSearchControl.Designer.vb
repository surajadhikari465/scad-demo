Namespace WholeFoods.IRMA.ItemChaining.UserInterface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class ItemSearchControl
        Inherits System.Windows.Forms.UserControl

        'UserControl1 overrides dispose to clean up the component list.
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
            Me.components = New System.ComponentModel.Container
            Me.txtChain = New System.Windows.Forms.TextBox
            Me.btnSearch = New System.Windows.Forms.Button
            Me.txtDistSubTeam = New System.Windows.Forms.TextBox
            Me.txtBrand = New System.Windows.Forms.TextBox
            Me.Label7 = New System.Windows.Forms.Label
            Me.txtVendorItemID = New System.Windows.Forms.TextBox
            Me.txtVendor = New System.Windows.Forms.TextBox
            Me.txtDescription = New System.Windows.Forms.TextBox
            Me.txtIdentifier = New System.Windows.Forms.TextBox
            Me._lblLabel_5 = New System.Windows.Forms.Label
            Me._lblLabel_3 = New System.Windows.Forms.Label
            Me._lblLabel_0 = New System.Windows.Forms.Label
            Me._lblLabel_7 = New System.Windows.Forms.Label
            Me._lblLabel_2 = New System.Windows.Forms.Label
            Me._lblLabel_1 = New System.Windows.Forms.Label
            Me.FormManager1 = New LaMarvin.Windows.Forms.AutoComplete.FormManager(Me.components)
            Me.txtVendorPS = New System.Windows.Forms.TextBox
            Me.txtLevel4 = New System.Windows.Forms.TextBox
            Me.txtLevel3 = New System.Windows.Forms.TextBox
            Me.txtClass = New System.Windows.Forms.TextBox
            Me.txtSubTeam = New System.Windows.Forms.TextBox
            Me.Label1 = New System.Windows.Forms.Label
            Me.GroupBox1 = New System.Windows.Forms.GroupBox
            Me.lblLevel4 = New System.Windows.Forms.Label
            Me.Label2 = New System.Windows.Forms.Label
            Me.lblCategory = New System.Windows.Forms.Label
            Me.lblSubTeam = New System.Windows.Forms.Label
            Me.btnClear = New System.Windows.Forms.Button
            Me.isoItemOptions = New WholeFoods.IRMA.ItemChaining.UserInterface.ItemSearchItemOptions
            CType(Me.FormManager1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.GroupBox1.SuspendLayout()
            Me.SuspendLayout()
            '
            'txtChain
            '
            Me.txtChain.AcceptsReturn = True
            Me.txtChain.Location = New System.Drawing.Point(319, 0)
            Me.txtChain.Name = "txtChain"
            Me.FormManager1.SetNamespace(Me.txtChain, "txtChain")
            Me.FormManager1.SetOptions(Me.txtChain, CType((LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions.AutoSuggest Or LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions.AutoAppend), LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions))
            Me.txtChain.Size = New System.Drawing.Size(159, 20)
            Me.txtChain.TabIndex = 2
            Me.txtChain.Tag = "GetItemChains_ByDescriptionStartsWith"
            '
            'btnSearch
            '
            Me.btnSearch.Location = New System.Drawing.Point(403, 172)
            Me.btnSearch.Name = "btnSearch"
            Me.btnSearch.Size = New System.Drawing.Size(75, 23)
            Me.btnSearch.TabIndex = 17
            Me.btnSearch.Text = "Search"
            Me.btnSearch.UseVisualStyleBackColor = True
            '
            'txtDistSubTeam
            '
            Me.txtDistSubTeam.Location = New System.Drawing.Point(319, 112)
            Me.txtDistSubTeam.Name = "txtDistSubTeam"
            Me.FormManager1.SetNamespace(Me.txtDistSubTeam, "TextBox5")
            Me.FormManager1.SetOptions(Me.txtDistSubTeam, CType((LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions.AutoSuggest Or LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions.AutoAppend), LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions))
            Me.txtDistSubTeam.Size = New System.Drawing.Size(159, 20)
            Me.txtDistSubTeam.TabIndex = 7
            '
            'txtBrand
            '
            Me.txtBrand.Location = New System.Drawing.Point(73, 23)
            Me.txtBrand.Name = "txtBrand"
            Me.FormManager1.SetNamespace(Me.txtBrand, "txtBrand")
            Me.FormManager1.SetOptions(Me.txtBrand, CType((LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions.AutoSuggest Or LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions.AutoAppend), LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions))
            Me.txtBrand.Size = New System.Drawing.Size(159, 20)
            Me.txtBrand.TabIndex = 1
            Me.txtBrand.Tag = "GetBrands_ByNameStartsWith"
            '
            'Label7
            '
            Me.Label7.AutoSize = True
            Me.Label7.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
            Me.Label7.ImeMode = System.Windows.Forms.ImeMode.NoControl
            Me.Label7.Location = New System.Drawing.Point(273, 8)
            Me.Label7.Name = "Label7"
            Me.Label7.Size = New System.Drawing.Size(44, 14)
            Me.Label7.TabIndex = 116
            Me.Label7.Text = "Chain :"
            '
            'txtVendorItemID
            '
            Me.txtVendorItemID.AcceptsReturn = True
            Me.txtVendorItemID.BackColor = System.Drawing.SystemColors.Window
            Me.txtVendorItemID.Cursor = System.Windows.Forms.Cursors.IBeam
            Me.txtVendorItemID.ForeColor = System.Drawing.SystemColors.WindowText
            Me.txtVendorItemID.Location = New System.Drawing.Point(319, 90)
            Me.txtVendorItemID.MaxLength = 20
            Me.txtVendorItemID.Name = "txtVendorItemID"
            Me.FormManager1.SetNamespace(Me.txtVendorItemID, "_txtField_5")
            Me.FormManager1.SetOptions(Me.txtVendorItemID, CType((LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions.AutoSuggest Or LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions.AutoAppend), LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions))
            Me.txtVendorItemID.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me.txtVendorItemID.Size = New System.Drawing.Size(159, 20)
            Me.txtVendorItemID.TabIndex = 6
            Me.txtVendorItemID.Tag = "Integer"
            '
            'txtVendor
            '
            Me.txtVendor.AcceptsReturn = True
            Me.txtVendor.BackColor = System.Drawing.SystemColors.Window
            Me.txtVendor.Cursor = System.Windows.Forms.Cursors.IBeam
            Me.txtVendor.ForeColor = System.Drawing.SystemColors.WindowText
            Me.txtVendor.Location = New System.Drawing.Point(319, 46)
            Me.txtVendor.MaxLength = 50
            Me.txtVendor.Name = "txtVendor"
            Me.FormManager1.SetNamespace(Me.txtVendor, "txtVendor")
            Me.FormManager1.SetOptions(Me.txtVendor, CType((LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions.AutoSuggest Or LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions.AutoAppend), LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions))
            Me.txtVendor.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me.txtVendor.Size = New System.Drawing.Size(159, 20)
            Me.txtVendor.TabIndex = 4
            Me.txtVendor.Tag = "GetVendor_ByCompanyNameStartsWith"
            '
            'txtDescription
            '
            Me.txtDescription.AcceptsReturn = True
            Me.txtDescription.BackColor = System.Drawing.SystemColors.Window
            Me.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam
            Me.txtDescription.ForeColor = System.Drawing.SystemColors.WindowText
            Me.txtDescription.Location = New System.Drawing.Point(319, 23)
            Me.txtDescription.MaxLength = 60
            Me.txtDescription.Name = "txtDescription"
            Me.FormManager1.SetNamespace(Me.txtDescription, "_txtField_0")
            Me.FormManager1.SetOptions(Me.txtDescription, CType((LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions.AutoSuggest Or LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions.AutoAppend), LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions))
            Me.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me.txtDescription.Size = New System.Drawing.Size(159, 20)
            Me.txtDescription.TabIndex = 3
            Me.txtDescription.Tag = "String"
            '
            'txtIdentifier
            '
            Me.txtIdentifier.AcceptsReturn = True
            Me.txtIdentifier.BackColor = System.Drawing.SystemColors.Window
            Me.txtIdentifier.Cursor = System.Windows.Forms.Cursors.IBeam
            Me.txtIdentifier.ForeColor = System.Drawing.SystemColors.WindowText
            Me.txtIdentifier.Location = New System.Drawing.Point(73, 0)
            Me.txtIdentifier.MaxLength = 13
            Me.txtIdentifier.Name = "txtIdentifier"
            Me.FormManager1.SetNamespace(Me.txtIdentifier, "_txtField_1")
            Me.FormManager1.SetOptions(Me.txtIdentifier, CType((LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions.AutoSuggest Or LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions.AutoAppend), LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions))
            Me.txtIdentifier.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me.txtIdentifier.Size = New System.Drawing.Size(159, 20)
            Me.txtIdentifier.TabIndex = 0
            Me.txtIdentifier.Tag = "String"
            '
            '_lblLabel_5
            '
            Me._lblLabel_5.AutoSize = True
            Me._lblLabel_5.BackColor = System.Drawing.Color.Transparent
            Me._lblLabel_5.Cursor = System.Windows.Forms.Cursors.Default
            Me._lblLabel_5.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
            Me._lblLabel_5.ForeColor = System.Drawing.SystemColors.ControlText
            Me._lblLabel_5.ImeMode = System.Windows.Forms.ImeMode.NoControl
            Me._lblLabel_5.Location = New System.Drawing.Point(220, 115)
            Me._lblLabel_5.Name = "_lblLabel_5"
            Me._lblLabel_5.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me._lblLabel_5.Size = New System.Drawing.Size(96, 14)
            Me._lblLabel_5.TabIndex = 110
            Me._lblLabel_5.Text = "Dist. Sub-Team :"
            Me._lblLabel_5.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            '_lblLabel_3
            '
            Me._lblLabel_3.AutoSize = True
            Me._lblLabel_3.BackColor = System.Drawing.Color.Transparent
            Me._lblLabel_3.Cursor = System.Windows.Forms.Cursors.Default
            Me._lblLabel_3.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
            Me._lblLabel_3.ForeColor = System.Drawing.SystemColors.ControlText
            Me._lblLabel_3.ImeMode = System.Windows.Forms.ImeMode.NoControl
            Me._lblLabel_3.Location = New System.Drawing.Point(27, 29)
            Me._lblLabel_3.Name = "_lblLabel_3"
            Me._lblLabel_3.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me._lblLabel_3.Size = New System.Drawing.Size(45, 14)
            Me._lblLabel_3.TabIndex = 109
            Me._lblLabel_3.Text = "Brand :"
            Me._lblLabel_3.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            '_lblLabel_0
            '
            Me._lblLabel_0.BackColor = System.Drawing.Color.Transparent
            Me._lblLabel_0.Cursor = System.Windows.Forms.Cursors.Default
            Me._lblLabel_0.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
            Me._lblLabel_0.ForeColor = System.Drawing.SystemColors.ControlText
            Me._lblLabel_0.ImeMode = System.Windows.Forms.ImeMode.NoControl
            Me._lblLabel_0.Location = New System.Drawing.Point(251, 49)
            Me._lblLabel_0.Name = "_lblLabel_0"
            Me._lblLabel_0.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me._lblLabel_0.Size = New System.Drawing.Size(65, 17)
            Me._lblLabel_0.TabIndex = 99
            Me._lblLabel_0.Text = "Vendor :"
            Me._lblLabel_0.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            '_lblLabel_7
            '
            Me._lblLabel_7.BackColor = System.Drawing.Color.Transparent
            Me._lblLabel_7.Cursor = System.Windows.Forms.Cursors.Default
            Me._lblLabel_7.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
            Me._lblLabel_7.ForeColor = System.Drawing.SystemColors.ControlText
            Me._lblLabel_7.ImeMode = System.Windows.Forms.ImeMode.NoControl
            Me._lblLabel_7.Location = New System.Drawing.Point(219, 93)
            Me._lblLabel_7.Name = "_lblLabel_7"
            Me._lblLabel_7.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me._lblLabel_7.Size = New System.Drawing.Size(97, 17)
            Me._lblLabel_7.TabIndex = 105
            Me._lblLabel_7.Text = "Vendor Item ID :"
            Me._lblLabel_7.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            '_lblLabel_2
            '
            Me._lblLabel_2.AutoSize = True
            Me._lblLabel_2.BackColor = System.Drawing.Color.Transparent
            Me._lblLabel_2.Cursor = System.Windows.Forms.Cursors.Default
            Me._lblLabel_2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
            Me._lblLabel_2.ForeColor = System.Drawing.SystemColors.ControlText
            Me._lblLabel_2.ImeMode = System.Windows.Forms.ImeMode.NoControl
            Me._lblLabel_2.Location = New System.Drawing.Point(278, 26)
            Me._lblLabel_2.Name = "_lblLabel_2"
            Me._lblLabel_2.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me._lblLabel_2.Size = New System.Drawing.Size(40, 14)
            Me._lblLabel_2.TabIndex = 101
            Me._lblLabel_2.Text = "Desc :"
            Me._lblLabel_2.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            '_lblLabel_1
            '
            Me._lblLabel_1.BackColor = System.Drawing.Color.Transparent
            Me._lblLabel_1.Cursor = System.Windows.Forms.Cursors.Default
            Me._lblLabel_1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
            Me._lblLabel_1.ForeColor = System.Drawing.SystemColors.ControlText
            Me._lblLabel_1.ImeMode = System.Windows.Forms.ImeMode.NoControl
            Me._lblLabel_1.Location = New System.Drawing.Point(5, 6)
            Me._lblLabel_1.Name = "_lblLabel_1"
            Me._lblLabel_1.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me._lblLabel_1.Size = New System.Drawing.Size(65, 17)
            Me._lblLabel_1.TabIndex = 103
            Me._lblLabel_1.Text = "Identifier :"
            Me._lblLabel_1.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'FormManager1
            '
            Me.FormManager1.AutoSaveStrings = False
            '
            'txtVendorPS
            '
            Me.txtVendorPS.AcceptsReturn = True
            Me.txtVendorPS.BackColor = System.Drawing.SystemColors.Window
            Me.txtVendorPS.Cursor = System.Windows.Forms.Cursors.IBeam
            Me.txtVendorPS.ForeColor = System.Drawing.SystemColors.WindowText
            Me.txtVendorPS.Location = New System.Drawing.Point(319, 68)
            Me.txtVendorPS.MaxLength = 50
            Me.txtVendorPS.Name = "txtVendorPS"
            Me.FormManager1.SetNamespace(Me.txtVendorPS, "txtVendor")
            Me.FormManager1.SetOptions(Me.txtVendorPS, CType((LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions.AutoSuggest Or LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions.AutoAppend), LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions))
            Me.txtVendorPS.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me.txtVendorPS.Size = New System.Drawing.Size(159, 20)
            Me.txtVendorPS.TabIndex = 5
            Me.txtVendorPS.Tag = "GetVendor_ByCompanyNameStartsWith"
            '
            'txtLevel4
            '
            Me.txtLevel4.Location = New System.Drawing.Point(71, 77)
            Me.txtLevel4.Name = "txtLevel4"
            Me.FormManager1.SetNamespace(Me.txtLevel4, "txtLevel4")
            Me.FormManager1.SetOptions(Me.txtLevel4, CType((LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions.AutoSuggest Or LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions.AutoAppend), LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions))
            Me.txtLevel4.Size = New System.Drawing.Size(139, 20)
            Me.txtLevel4.TabIndex = 11
            Me.txtLevel4.Tag = "GetProdHierarchyLevel4s_ByDescriptionStartsWith"
            Me.txtLevel4.Text = "Select Level 3 First"
            '
            'txtLevel3
            '
            Me.txtLevel3.Location = New System.Drawing.Point(71, 54)
            Me.txtLevel3.Name = "txtLevel3"
            Me.FormManager1.SetNamespace(Me.txtLevel3, "txtLevel3")
            Me.FormManager1.SetOptions(Me.txtLevel3, CType((LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions.AutoSuggest Or LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions.AutoAppend), LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions))
            Me.txtLevel3.Size = New System.Drawing.Size(139, 20)
            Me.txtLevel3.TabIndex = 10
            Me.txtLevel3.Tag = "GetProdHierarchyLevel3s_ByDescriptionStartsWith"
            Me.txtLevel3.Text = "Select Class First"
            '
            'txtClass
            '
            Me.txtClass.Location = New System.Drawing.Point(71, 32)
            Me.txtClass.Name = "txtClass"
            Me.FormManager1.SetNamespace(Me.txtClass, "txtClass")
            Me.FormManager1.SetOptions(Me.txtClass, CType((LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions.AutoSuggest Or LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions.AutoAppend), LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions))
            Me.txtClass.Size = New System.Drawing.Size(139, 20)
            Me.txtClass.TabIndex = 9
            Me.txtClass.Tag = "GetCategory_ByNameStartsWith"
            Me.txtClass.Text = "Select Sub-Team First"
            '
            'txtSubTeam
            '
            Me.txtSubTeam.Location = New System.Drawing.Point(71, 10)
            Me.txtSubTeam.Name = "txtSubTeam"
            Me.FormManager1.SetNamespace(Me.txtSubTeam, "txtSubTeam")
            Me.FormManager1.SetOptions(Me.txtSubTeam, CType((LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions.AutoSuggest Or LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions.AutoAppend), LaMarvin.Windows.Forms.AutoComplete.AutoCompleteOptions))
            Me.txtSubTeam.Size = New System.Drawing.Size(139, 20)
            Me.txtSubTeam.TabIndex = 8
            Me.txtSubTeam.Tag = "GetSubTeams_BySubTeam_NameStartsWith"
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.BackColor = System.Drawing.Color.Transparent
            Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
            Me.Label1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
            Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
            Me.Label1.ImeMode = System.Windows.Forms.ImeMode.NoControl
            Me.Label1.Location = New System.Drawing.Point(239, 71)
            Me.Label1.Name = "Label1"
            Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
            Me.Label1.Size = New System.Drawing.Size(77, 14)
            Me.Label1.TabIndex = 118
            Me.Label1.Text = "Vendor PS# :"
            Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'GroupBox1
            '
            Me.GroupBox1.BackColor = System.Drawing.Color.Transparent
            Me.GroupBox1.Controls.Add(Me.txtLevel4)
            Me.GroupBox1.Controls.Add(Me.lblLevel4)
            Me.GroupBox1.Controls.Add(Me.txtLevel3)
            Me.GroupBox1.Controls.Add(Me.Label2)
            Me.GroupBox1.Controls.Add(Me.txtClass)
            Me.GroupBox1.Controls.Add(Me.txtSubTeam)
            Me.GroupBox1.Controls.Add(Me.lblCategory)
            Me.GroupBox1.Controls.Add(Me.lblSubTeam)
            Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.GroupBox1.Location = New System.Drawing.Point(3, 46)
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.Size = New System.Drawing.Size(215, 100)
            Me.GroupBox1.TabIndex = 123
            Me.GroupBox1.TabStop = False
            Me.GroupBox1.Text = "Hierarchy"
            '
            'lblLevel4
            '
            Me.lblLevel4.AutoSize = True
            Me.lblLevel4.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
            Me.lblLevel4.ImeMode = System.Windows.Forms.ImeMode.NoControl
            Me.lblLevel4.Location = New System.Drawing.Point(18, 80)
            Me.lblLevel4.Name = "lblLevel4"
            Me.lblLevel4.Size = New System.Drawing.Size(52, 14)
            Me.lblLevel4.TabIndex = 130
            Me.lblLevel4.Text = "Level 4 :"
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
            Me.Label2.ImeMode = System.Windows.Forms.ImeMode.NoControl
            Me.Label2.Location = New System.Drawing.Point(18, 57)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(52, 14)
            Me.Label2.TabIndex = 129
            Me.Label2.Text = "Level 3 :"
            '
            'lblCategory
            '
            Me.lblCategory.AutoSize = True
            Me.lblCategory.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
            Me.lblCategory.ImeMode = System.Windows.Forms.ImeMode.NoControl
            Me.lblCategory.Location = New System.Drawing.Point(26, 35)
            Me.lblCategory.Name = "lblCategory"
            Me.lblCategory.Size = New System.Drawing.Size(44, 14)
            Me.lblCategory.TabIndex = 128
            Me.lblCategory.Text = "Class :"
            '
            'lblSubTeam
            '
            Me.lblSubTeam.AutoSize = True
            Me.lblSubTeam.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
            Me.lblSubTeam.ImeMode = System.Windows.Forms.ImeMode.NoControl
            Me.lblSubTeam.Location = New System.Drawing.Point(1, 13)
            Me.lblSubTeam.Name = "lblSubTeam"
            Me.lblSubTeam.Size = New System.Drawing.Size(69, 14)
            Me.lblSubTeam.TabIndex = 127
            Me.lblSubTeam.Text = "Sub-Team :"
            '
            'btnClear
            '
            Me.btnClear.Location = New System.Drawing.Point(319, 172)
            Me.btnClear.Name = "btnClear"
            Me.btnClear.Size = New System.Drawing.Size(75, 23)
            Me.btnClear.TabIndex = 124
            Me.btnClear.Text = "Clear"
            Me.btnClear.UseVisualStyleBackColor = True
            '
            'isoItemOptions
            '
            Me.isoItemOptions.BackColor = System.Drawing.Color.Transparent
            Me.isoItemOptions.Location = New System.Drawing.Point(6, 150)
            Me.isoItemOptions.Name = "isoItemOptions"
            Me.isoItemOptions.ShowHFM = False
            Me.isoItemOptions.ShowWFM = False
            Me.isoItemOptions.Size = New System.Drawing.Size(305, 62)
            Me.isoItemOptions.TabIndex = 125
            '
            'ItemSearchControl
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.Transparent
            Me.Controls.Add(Me.isoItemOptions)
            Me.Controls.Add(Me.btnClear)
            Me.Controls.Add(Me.txtVendorPS)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.txtChain)
            Me.Controls.Add(Me.btnSearch)
            Me.Controls.Add(Me.txtDistSubTeam)
            Me.Controls.Add(Me.txtBrand)
            Me.Controls.Add(Me.Label7)
            Me.Controls.Add(Me.txtVendorItemID)
            Me.Controls.Add(Me.txtVendor)
            Me.Controls.Add(Me.txtDescription)
            Me.Controls.Add(Me.txtIdentifier)
            Me.Controls.Add(Me._lblLabel_5)
            Me.Controls.Add(Me._lblLabel_3)
            Me.Controls.Add(Me._lblLabel_0)
            Me.Controls.Add(Me._lblLabel_7)
            Me.Controls.Add(Me._lblLabel_2)
            Me.Controls.Add(Me._lblLabel_1)
            Me.Controls.Add(Me.GroupBox1)
            Me.Name = "ItemSearchControl"
            Me.Size = New System.Drawing.Size(485, 204)
            CType(Me.FormManager1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.GroupBox1.ResumeLayout(False)
            Me.GroupBox1.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents txtChain As System.Windows.Forms.TextBox
        Friend WithEvents btnSearch As System.Windows.Forms.Button
        Friend WithEvents txtDistSubTeam As System.Windows.Forms.TextBox
        Friend WithEvents txtBrand As System.Windows.Forms.TextBox
        Friend WithEvents Label7 As System.Windows.Forms.Label
        Public WithEvents txtVendorItemID As System.Windows.Forms.TextBox
        Friend WithEvents txtVendor As System.Windows.Forms.TextBox
        Public WithEvents txtDescription As System.Windows.Forms.TextBox
        Public WithEvents txtIdentifier As System.Windows.Forms.TextBox
        Public WithEvents _lblLabel_5 As System.Windows.Forms.Label
        Public WithEvents _lblLabel_3 As System.Windows.Forms.Label
        Public WithEvents _lblLabel_0 As System.Windows.Forms.Label
        Public WithEvents _lblLabel_7 As System.Windows.Forms.Label
        Public WithEvents _lblLabel_2 As System.Windows.Forms.Label
        Public WithEvents _lblLabel_1 As System.Windows.Forms.Label
        Friend WithEvents FormManager1 As LaMarvin.Windows.Forms.AutoComplete.FormManager
        Friend WithEvents txtVendorPS As System.Windows.Forms.TextBox
        Public WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
        Friend WithEvents txtLevel4 As System.Windows.Forms.TextBox
        Friend WithEvents lblLevel4 As System.Windows.Forms.Label
        Friend WithEvents txtLevel3 As System.Windows.Forms.TextBox
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents txtClass As System.Windows.Forms.TextBox
        Friend WithEvents txtSubTeam As System.Windows.Forms.TextBox
        Friend WithEvents lblCategory As System.Windows.Forms.Label
        Friend WithEvents lblSubTeam As System.Windows.Forms.Label
        Friend WithEvents btnClear As System.Windows.Forms.Button
        Friend WithEvents isoItemOptions As WholeFoods.IRMA.ItemChaining.UserInterface.ItemSearchItemOptions
    End Class
End Namespace