Namespace WholeFoods.IRMA.ItemChaining.UserInterface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
        Partial Class ItemChaining
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ItemChaining))
            Me.pnlHeader = New System.Windows.Forms.Panel
            Me.lblHeader = New System.Windows.Forms.Label
            Me.lblTitle = New System.Windows.Forms.Label
            Me.picHEader = New System.Windows.Forms.PictureBox
            Me.pnlWorkArea = New System.Windows.Forms.Panel
            Me.TabControl1 = New System.Windows.Forms.TabControl
            Me.wpWelcome = New System.Windows.Forms.TabPage
            Me.PictureBox1 = New System.Windows.Forms.PictureBox
            Me.radDeleteChain = New System.Windows.Forms.RadioButton
            Me.radEditChain = New System.Windows.Forms.RadioButton
            Me.radNewChain = New System.Windows.Forms.RadioButton
            Me.Label3 = New System.Windows.Forms.Label
            Me.Label2 = New System.Windows.Forms.Label
            Me.Label1 = New System.Windows.Forms.Label
            Me.wpSelectChain = New System.Windows.Forms.TabPage
            Me.lstFoundChain = New System.Windows.Forms.ListView
            Me.FoundChain = New System.Windows.Forms.ColumnHeader
            Me.Label8 = New System.Windows.Forms.Label
            Me.wpSearchItems = New System.Windows.Forms.TabPage
            Me.lblWait = New System.Windows.Forms.Label
            Me.ItemSearchControl1 = New ItemSearchControl
            Me.wpSelectItems = New System.Windows.Forms.TabPage
            Me.lblMessage = New System.Windows.Forms.Label
            Me.ItemSelectingControl1 = New ItemSelectingControl
            Me.wpSaveChain = New System.Windows.Forms.TabPage
            Me.txtChainName = New System.Windows.Forms.TextBox
            Me.Label6 = New System.Windows.Forms.Label
            Me.pnlButtons = New System.Windows.Forms.Panel
            Me.btnCancel = New System.Windows.Forms.Button
            Me.btnBack = New System.Windows.Forms.Button
            Me.btnNext = New System.Windows.Forms.Button
            Me.btnFinish = New System.Windows.Forms.Button
            Me.pnlHeader.SuspendLayout()
            CType(Me.picHEader, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlWorkArea.SuspendLayout()
            Me.TabControl1.SuspendLayout()
            Me.wpWelcome.SuspendLayout()
            CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.wpSelectChain.SuspendLayout()
            Me.wpSearchItems.SuspendLayout()
            Me.wpSelectItems.SuspendLayout()
            Me.wpSaveChain.SuspendLayout()
            Me.pnlButtons.SuspendLayout()
            Me.SuspendLayout()
            '
            'pnlHeader
            '
            Me.pnlHeader.BackColor = System.Drawing.Color.LightSteelBlue
            Me.pnlHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlHeader.Controls.Add(Me.lblHeader)
            Me.pnlHeader.Controls.Add(Me.lblTitle)
            Me.pnlHeader.Controls.Add(Me.picHEader)
            Me.pnlHeader.Location = New System.Drawing.Point(-2, 0)
            Me.pnlHeader.Name = "pnlHeader"
            Me.pnlHeader.Size = New System.Drawing.Size(521, 63)
            Me.pnlHeader.TabIndex = 2
            '
            'lblHeader
            '
            Me.lblHeader.BackColor = System.Drawing.Color.Transparent
            Me.lblHeader.Location = New System.Drawing.Point(79, 25)
            Me.lblHeader.Name = "lblHeader"
            Me.lblHeader.Size = New System.Drawing.Size(439, 38)
            Me.lblHeader.TabIndex = 4
            Me.lblHeader.Text = "Hello"
            '
            'lblTitle
            '
            Me.lblTitle.AutoSize = True
            Me.lblTitle.BackColor = System.Drawing.Color.Transparent
            Me.lblTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblTitle.Location = New System.Drawing.Point(79, 9)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New System.Drawing.Size(36, 13)
            Me.lblTitle.TabIndex = 3
            Me.lblTitle.Text = "Hello"
            '
            'picHEader
            '
            Me.picHEader.BackColor = System.Drawing.Color.LightSteelBlue
            Me.picHEader.Image = CType(resources.GetObject("picHEader.Image"), System.Drawing.Image)
            Me.picHEader.Location = New System.Drawing.Point(0, 0)
            Me.picHEader.Name = "picHEader"
            Me.picHEader.Size = New System.Drawing.Size(521, 63)
            Me.picHEader.TabIndex = 2
            Me.picHEader.TabStop = False
            '
            'pnlWorkArea
            '
            Me.pnlWorkArea.BackColor = System.Drawing.Color.Lavender
            Me.pnlWorkArea.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
            Me.pnlWorkArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlWorkArea.Controls.Add(Me.TabControl1)
            Me.pnlWorkArea.Location = New System.Drawing.Point(-2, 34)
            Me.pnlWorkArea.Name = "pnlWorkArea"
            Me.pnlWorkArea.Size = New System.Drawing.Size(518, 317)
            Me.pnlWorkArea.TabIndex = 3
            '
            'TabControl1
            '
            Me.TabControl1.Controls.Add(Me.wpWelcome)
            Me.TabControl1.Controls.Add(Me.wpSelectChain)
            Me.TabControl1.Controls.Add(Me.wpSearchItems)
            Me.TabControl1.Controls.Add(Me.wpSelectItems)
            Me.TabControl1.Controls.Add(Me.wpSaveChain)
            Me.TabControl1.Location = New System.Drawing.Point(0, 3)
            Me.TabControl1.Name = "TabControl1"
            Me.TabControl1.SelectedIndex = 0
            Me.TabControl1.Size = New System.Drawing.Size(515, 315)
            Me.TabControl1.TabIndex = 0
            '
            'wpWelcome
            '
            Me.wpWelcome.Controls.Add(Me.PictureBox1)
            Me.wpWelcome.Controls.Add(Me.radDeleteChain)
            Me.wpWelcome.Controls.Add(Me.radEditChain)
            Me.wpWelcome.Controls.Add(Me.radNewChain)
            Me.wpWelcome.Controls.Add(Me.Label3)
            Me.wpWelcome.Controls.Add(Me.Label2)
            Me.wpWelcome.Controls.Add(Me.Label1)
            Me.wpWelcome.Location = New System.Drawing.Point(4, 22)
            Me.wpWelcome.Name = "wpWelcome"
            Me.wpWelcome.Padding = New System.Windows.Forms.Padding(3)
            Me.wpWelcome.Size = New System.Drawing.Size(507, 289)
            Me.wpWelcome.TabIndex = 0
            Me.wpWelcome.Tag = ""
            Me.wpWelcome.Text = "Welcome"
            Me.wpWelcome.UseVisualStyleBackColor = True
            '
            'PictureBox1
            '
            Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
            Me.PictureBox1.Location = New System.Drawing.Point(6, 35)
            Me.PictureBox1.Name = "PictureBox1"
            Me.PictureBox1.Size = New System.Drawing.Size(175, 181)
            Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
            Me.PictureBox1.TabIndex = 12
            Me.PictureBox1.TabStop = False
            '
            'radDeleteChain
            '
            Me.radDeleteChain.AutoSize = True
            Me.radDeleteChain.Location = New System.Drawing.Point(221, 199)
            Me.radDeleteChain.Name = "radDeleteChain"
            Me.radDeleteChain.Size = New System.Drawing.Size(125, 17)
            Me.radDeleteChain.TabIndex = 11
            Me.radDeleteChain.Text = "Delete Existing Chain"
            Me.radDeleteChain.UseVisualStyleBackColor = True
            '
            'radEditChain
            '
            Me.radEditChain.AutoSize = True
            Me.radEditChain.Location = New System.Drawing.Point(221, 163)
            Me.radEditChain.Name = "radEditChain"
            Me.radEditChain.Size = New System.Drawing.Size(112, 17)
            Me.radEditChain.TabIndex = 10
            Me.radEditChain.Text = "Edit Existing Chain"
            Me.radEditChain.UseVisualStyleBackColor = True
            '
            'radNewChain
            '
            Me.radNewChain.AutoSize = True
            Me.radNewChain.Checked = True
            Me.radNewChain.Location = New System.Drawing.Point(221, 128)
            Me.radNewChain.Name = "radNewChain"
            Me.radNewChain.Size = New System.Drawing.Size(111, 17)
            Me.radNewChain.TabIndex = 9
            Me.radNewChain.TabStop = True
            Me.radNewChain.Text = "Create New Chain"
            Me.radNewChain.UseVisualStyleBackColor = True
            '
            'Label3
            '
            Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(198, 248)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(120, 13)
            Me.Label3.TabIndex = 8
            Me.Label3.Text = "To continue, click Next."
            '
            'Label2
            '
            Me.Label2.Location = New System.Drawing.Point(198, 66)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(291, 190)
            Me.Label2.TabIndex = 7
            Me.Label2.Text = "This wizard will help you create new item chaining or edit an existing one"
            '
            'Label1
            '
            Me.Label1.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold)
            Me.Label1.Location = New System.Drawing.Point(196, 10)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(309, 55)
            Me.Label1.TabIndex = 6
            Me.Label1.Text = "Welcome to the IRMA Item Chaining Wizard"
            '
            'wpSelectChain
            '
            Me.wpSelectChain.Controls.Add(Me.lstFoundChain)
            Me.wpSelectChain.Controls.Add(Me.Label8)
            Me.wpSelectChain.Location = New System.Drawing.Point(4, 22)
            Me.wpSelectChain.Name = "wpSelectChain"
            Me.wpSelectChain.Padding = New System.Windows.Forms.Padding(3)
            Me.wpSelectChain.Size = New System.Drawing.Size(507, 289)
            Me.wpSelectChain.TabIndex = 1
            Me.wpSelectChain.Tag = "Select Chain to Edit"
            Me.wpSelectChain.Text = "Select Chain"
            Me.wpSelectChain.UseVisualStyleBackColor = True
            '
            'lstFoundChain
            '
            Me.lstFoundChain.AllowDrop = True
            Me.lstFoundChain.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.FoundChain})
            Me.lstFoundChain.FullRowSelect = True
            Me.lstFoundChain.GridLines = True
            Me.lstFoundChain.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            Me.lstFoundChain.HideSelection = False
            Me.lstFoundChain.Location = New System.Drawing.Point(147, 49)
            Me.lstFoundChain.MultiSelect = False
            Me.lstFoundChain.Name = "lstFoundChain"
            Me.lstFoundChain.Size = New System.Drawing.Size(215, 214)
            Me.lstFoundChain.TabIndex = 24
            Me.lstFoundChain.UseCompatibleStateImageBehavior = False
            Me.lstFoundChain.View = System.Windows.Forms.View.Details
            '
            'FoundChain
            '
            Me.FoundChain.Text = "Chain"
            Me.FoundChain.Width = 211
            '
            'Label8
            '
            Me.Label8.AutoSize = True
            Me.Label8.BackColor = System.Drawing.Color.Transparent
            Me.Label8.Location = New System.Drawing.Point(144, 33)
            Me.Label8.Name = "Label8"
            Me.Label8.Size = New System.Drawing.Size(75, 13)
            Me.Label8.TabIndex = 23
            Me.Label8.Text = "Found Chains:"
            '
            'wpSearchItems
            '
            Me.wpSearchItems.Controls.Add(Me.lblWait)
            Me.wpSearchItems.Controls.Add(Me.ItemSearchControl1)
            Me.wpSearchItems.Location = New System.Drawing.Point(4, 22)
            Me.wpSearchItems.Name = "wpSearchItems"
            Me.wpSearchItems.Padding = New System.Windows.Forms.Padding(3)
            Me.wpSearchItems.Size = New System.Drawing.Size(507, 289)
            Me.wpSearchItems.TabIndex = 2
            Me.wpSearchItems.Tag = "This page allows you to search for products that you would like to chain. After y" & _
                "ou search, highlight the items you want to chain and click the arrow button."
            Me.wpSearchItems.Text = "Search Items to Chain"
            Me.wpSearchItems.UseVisualStyleBackColor = True
            '
            'lblWait
            '
            Me.lblWait.AutoSize = True
            Me.lblWait.BackColor = System.Drawing.Color.Transparent
            Me.lblWait.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblWait.ForeColor = System.Drawing.SystemColors.ControlDark
            Me.lblWait.Location = New System.Drawing.Point(160, 234)
            Me.lblWait.Name = "lblWait"
            Me.lblWait.Size = New System.Drawing.Size(200, 37)
            Me.lblWait.TabIndex = 3
            Me.lblWait.Text = "Searching..."
            Me.lblWait.Visible = False
            '
            'ItemSearchControl1
            '
            Me.ItemSearchControl1.BackColor = System.Drawing.Color.Transparent
            Me.ItemSearchControl1.Location = New System.Drawing.Point(11, 26)
            Me.ItemSearchControl1.Name = "ItemSearchControl1"
            Me.ItemSearchControl1.ShowHFM = False
            Me.ItemSearchControl1.ShowSearchButton = False
            Me.ItemSearchControl1.ShowWFM = False
            Me.ItemSearchControl1.Size = New System.Drawing.Size(485, 205)
            Me.ItemSearchControl1.TabIndex = 2
            '
            'wpSelectItems
            '
            Me.wpSelectItems.Controls.Add(Me.lblMessage)
            Me.wpSelectItems.Controls.Add(Me.ItemSelectingControl1)
            Me.wpSelectItems.Location = New System.Drawing.Point(4, 22)
            Me.wpSelectItems.Name = "wpSelectItems"
            Me.wpSelectItems.Padding = New System.Windows.Forms.Padding(3)
            Me.wpSelectItems.Size = New System.Drawing.Size(507, 289)
            Me.wpSelectItems.TabIndex = 3
            Me.wpSelectItems.Tag = "Select from the list on the left and press the arrows to move the selected items " & _
                "to the list on the right. You can press ""Back"" to modify your search."
            Me.wpSelectItems.Text = "Select Items to Chain"
            Me.wpSelectItems.UseVisualStyleBackColor = True
            '
            'lblMessage
            '
            Me.lblMessage.AutoSize = True
            Me.lblMessage.BackColor = System.Drawing.Color.Transparent
            Me.lblMessage.Location = New System.Drawing.Point(15, 259)
            Me.lblMessage.Name = "lblMessage"
            Me.lblMessage.Size = New System.Drawing.Size(0, 13)
            Me.lblMessage.TabIndex = 3
            '
            'ItemSelectingControl1
            '
            Me.ItemSelectingControl1.BackColor = System.Drawing.Color.Transparent
            Me.ItemSelectingControl1.Field_ID = "Item_Key"
            Me.ItemSelectingControl1.Field_Image = "Chain_ID"
            Me.ItemSelectingControl1.Field_Text = "Item_Description"
            Me.ItemSelectingControl1.Field_Text2 = "Identifier"
            Me.ItemSelectingControl1.Icon = ItemSelectingControl.IconType.[Boolean]
            Me.ItemSelectingControl1.ItemText = "Item"
            Me.ItemSelectingControl1.ListHeight = 210
            Me.ItemSelectingControl1.Location = New System.Drawing.Point(11, 18)
            Me.ItemSelectingControl1.Name = "ItemSelectingControl1"
            Me.ItemSelectingControl1.ShowExportButton = False
            Me.ItemSelectingControl1.Size = New System.Drawing.Size(485, 261)
            Me.ItemSelectingControl1.TabIndex = 2
            '
            'wpSaveChain
            '
            Me.wpSaveChain.Controls.Add(Me.txtChainName)
            Me.wpSaveChain.Controls.Add(Me.Label6)
            Me.wpSaveChain.Location = New System.Drawing.Point(4, 22)
            Me.wpSaveChain.Name = "wpSaveChain"
            Me.wpSaveChain.Padding = New System.Windows.Forms.Padding(3)
            Me.wpSaveChain.Size = New System.Drawing.Size(507, 289)
            Me.wpSaveChain.TabIndex = 4
            Me.wpSaveChain.Tag = "Enter the chain name and press ""Finish"" to save chain"
            Me.wpSaveChain.Text = "Save Chain"
            Me.wpSaveChain.UseVisualStyleBackColor = True
            '
            'txtChainName
            '
            Me.txtChainName.Location = New System.Drawing.Point(202, 138)
            Me.txtChainName.Name = "txtChainName"
            Me.txtChainName.Size = New System.Drawing.Size(176, 20)
            Me.txtChainName.TabIndex = 3
            '
            'Label6
            '
            Me.Label6.AutoSize = True
            Me.Label6.BackColor = System.Drawing.Color.Transparent
            Me.Label6.Location = New System.Drawing.Point(128, 141)
            Me.Label6.Name = "Label6"
            Me.Label6.Size = New System.Drawing.Size(68, 13)
            Me.Label6.TabIndex = 2
            Me.Label6.Text = "Chain Name:"
            '
            'pnlButtons
            '
            Me.pnlButtons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlButtons.Controls.Add(Me.btnCancel)
            Me.pnlButtons.Controls.Add(Me.btnBack)
            Me.pnlButtons.Controls.Add(Me.btnNext)
            Me.pnlButtons.Controls.Add(Me.btnFinish)
            Me.pnlButtons.Location = New System.Drawing.Point(-1, 351)
            Me.pnlButtons.Name = "pnlButtons"
            Me.pnlButtons.Size = New System.Drawing.Size(517, 37)
            Me.pnlButtons.TabIndex = 4
            '
            'btnCancel
            '
            Me.btnCancel.Location = New System.Drawing.Point(10, 7)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 23)
            Me.btnCancel.TabIndex = 9
            Me.btnCancel.Text = "Cancel"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'btnBack
            '
            Me.btnBack.Enabled = False
            Me.btnBack.Location = New System.Drawing.Point(352, 7)
            Me.btnBack.Name = "btnBack"
            Me.btnBack.Size = New System.Drawing.Size(75, 23)
            Me.btnBack.TabIndex = 8
            Me.btnBack.Text = "< Back"
            Me.btnBack.UseVisualStyleBackColor = True
            '
            'btnNext
            '
            Me.btnNext.Location = New System.Drawing.Point(433, 7)
            Me.btnNext.Name = "btnNext"
            Me.btnNext.Size = New System.Drawing.Size(75, 23)
            Me.btnNext.TabIndex = 7
            Me.btnNext.Text = "Next >"
            Me.btnNext.UseVisualStyleBackColor = True
            '
            'btnFinish
            '
            Me.btnFinish.Location = New System.Drawing.Point(433, 7)
            Me.btnFinish.Name = "btnFinish"
            Me.btnFinish.Size = New System.Drawing.Size(75, 23)
            Me.btnFinish.TabIndex = 10
            Me.btnFinish.Text = "Finish"
            Me.btnFinish.UseVisualStyleBackColor = True
            Me.btnFinish.Visible = False
            '
            'frmChaining2
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(517, 388)
            Me.Controls.Add(Me.pnlWorkArea)
            Me.Controls.Add(Me.pnlButtons)
            Me.Controls.Add(Me.pnlHeader)
            Me.Name = "frmChaining2"
            Me.Text = "Wizard"
            Me.pnlHeader.ResumeLayout(False)
            Me.pnlHeader.PerformLayout()
            CType(Me.picHEader, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlWorkArea.ResumeLayout(False)
            Me.TabControl1.ResumeLayout(False)
            Me.wpWelcome.ResumeLayout(False)
            Me.wpWelcome.PerformLayout()
            CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.wpSelectChain.ResumeLayout(False)
            Me.wpSelectChain.PerformLayout()
            Me.wpSearchItems.ResumeLayout(False)
            Me.wpSearchItems.PerformLayout()
            Me.wpSelectItems.ResumeLayout(False)
            Me.wpSelectItems.PerformLayout()
            Me.wpSaveChain.ResumeLayout(False)
            Me.wpSaveChain.PerformLayout()
            Me.pnlButtons.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents pnlHeader As System.Windows.Forms.Panel
        Friend WithEvents lblTitle As System.Windows.Forms.Label
        Friend WithEvents picHEader As System.Windows.Forms.PictureBox
        Friend WithEvents lblHeader As System.Windows.Forms.Label
        Friend WithEvents pnlWorkArea As System.Windows.Forms.Panel
        Friend WithEvents pnlButtons As System.Windows.Forms.Panel
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents btnBack As System.Windows.Forms.Button
        Friend WithEvents btnNext As System.Windows.Forms.Button
        Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
        Friend WithEvents wpWelcome As System.Windows.Forms.TabPage
        Friend WithEvents wpSelectChain As System.Windows.Forms.TabPage
        Friend WithEvents radDeleteChain As System.Windows.Forms.RadioButton
        Friend WithEvents radEditChain As System.Windows.Forms.RadioButton
        Friend WithEvents radNewChain As System.Windows.Forms.RadioButton
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
        Friend WithEvents lstFoundChain As System.Windows.Forms.ListView
        Friend WithEvents FoundChain As System.Windows.Forms.ColumnHeader
        Friend WithEvents Label8 As System.Windows.Forms.Label
        Friend WithEvents wpSearchItems As System.Windows.Forms.TabPage
        Friend WithEvents lblWait As System.Windows.Forms.Label
        Friend WithEvents ItemSearchControl1 As ItemSearchControl
        Friend WithEvents wpSelectItems As System.Windows.Forms.TabPage
        Friend WithEvents lblMessage As System.Windows.Forms.Label
        Friend WithEvents ItemSelectingControl1 As ItemSelectingControl
        Friend WithEvents wpSaveChain As System.Windows.Forms.TabPage
        Friend WithEvents txtChainName As System.Windows.Forms.TextBox
        Friend WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents btnFinish As System.Windows.Forms.Button
    End Class
End Namespace