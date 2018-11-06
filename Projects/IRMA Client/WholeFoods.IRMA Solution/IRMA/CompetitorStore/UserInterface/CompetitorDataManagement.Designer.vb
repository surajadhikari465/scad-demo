Namespace WholeFoods.IRMA.CompetitorStore.UserInterface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class CompetitorDataManagement
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
            Me.components = New System.ComponentModel.Container
            Me.gbSearchCriteria = New System.Windows.Forms.GroupBox
            Me.btnSearch = New System.Windows.Forms.Button
            Me.txtWFMIdentifier = New System.Windows.Forms.TextBox
            Me.lblWFMIdentifier = New System.Windows.Forms.Label
            Me.cmbFiscalWeek = New System.Windows.Forms.ComboBox
            Me.FiscalWeekBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.cmbStore = New System.Windows.Forms.ComboBox
            Me.CompetitorStoreBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.lblFiscalWeek = New System.Windows.Forms.Label
            Me.lblStore = New System.Windows.Forms.Label
            Me.cmbLocation = New System.Windows.Forms.ComboBox
            Me.CompetitorLocationBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.cmbCompetitor = New System.Windows.Forms.ComboBox
            Me.CompetitorBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.lblLocation = New System.Windows.Forms.Label
            Me.lblCompetitor = New System.Windows.Forms.Label
            Me.btnSave = New System.Windows.Forms.Button
            Me.btnExit = New System.Windows.Forms.Button
            Me.cpgGrid = New WholeFoods.IRMA.CompetitorStore.UserInterface.CompetitorPriceGrid
            Me.btnItemSearch = New System.Windows.Forms.Button
            Me.btnStoreSearch = New System.Windows.Forms.Button
            Me.gbSearchCriteria.SuspendLayout()
            CType(Me.FiscalWeekBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.CompetitorStoreBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.CompetitorLocationBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.CompetitorBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'gbSearchCriteria
            '
            Me.gbSearchCriteria.BackColor = System.Drawing.Color.Transparent
            Me.gbSearchCriteria.Controls.Add(Me.btnSearch)
            Me.gbSearchCriteria.Controls.Add(Me.txtWFMIdentifier)
            Me.gbSearchCriteria.Controls.Add(Me.lblWFMIdentifier)
            Me.gbSearchCriteria.Controls.Add(Me.cmbFiscalWeek)
            Me.gbSearchCriteria.Controls.Add(Me.cmbStore)
            Me.gbSearchCriteria.Controls.Add(Me.lblFiscalWeek)
            Me.gbSearchCriteria.Controls.Add(Me.lblStore)
            Me.gbSearchCriteria.Controls.Add(Me.cmbLocation)
            Me.gbSearchCriteria.Controls.Add(Me.cmbCompetitor)
            Me.gbSearchCriteria.Controls.Add(Me.lblLocation)
            Me.gbSearchCriteria.Controls.Add(Me.lblCompetitor)
            Me.gbSearchCriteria.Location = New System.Drawing.Point(12, 12)
            Me.gbSearchCriteria.Name = "gbSearchCriteria"
            Me.gbSearchCriteria.Size = New System.Drawing.Size(718, 64)
            Me.gbSearchCriteria.TabIndex = 0
            Me.gbSearchCriteria.TabStop = False
            Me.gbSearchCriteria.Text = "Search Criteria"
            '
            'btnSearch
            '
            Me.btnSearch.Location = New System.Drawing.Point(615, 33)
            Me.btnSearch.Name = "btnSearch"
            Me.btnSearch.Size = New System.Drawing.Size(100, 23)
            Me.btnSearch.TabIndex = 10
            Me.btnSearch.Text = "Search"
            Me.btnSearch.UseVisualStyleBackColor = True
            '
            'txtWFMIdentifier
            '
            Me.txtWFMIdentifier.Location = New System.Drawing.Point(615, 12)
            Me.txtWFMIdentifier.Name = "txtWFMIdentifier"
            Me.txtWFMIdentifier.Size = New System.Drawing.Size(100, 20)
            Me.txtWFMIdentifier.TabIndex = 9
            '
            'lblWFMIdentifier
            '
            Me.lblWFMIdentifier.AutoSize = True
            Me.lblWFMIdentifier.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblWFMIdentifier.Location = New System.Drawing.Point(546, 16)
            Me.lblWFMIdentifier.Name = "lblWFMIdentifier"
            Me.lblWFMIdentifier.Size = New System.Drawing.Size(65, 13)
            Me.lblWFMIdentifier.TabIndex = 8
            Me.lblWFMIdentifier.Text = "Identifier :"
            Me.lblWFMIdentifier.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'cmbFiscalWeek
            '
            Me.cmbFiscalWeek.DataSource = Me.FiscalWeekBindingSource
            Me.cmbFiscalWeek.DisplayMember = "FiscalWeekDescription"
            Me.cmbFiscalWeek.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbFiscalWeek.FormattingEnabled = True
            Me.cmbFiscalWeek.Location = New System.Drawing.Point(349, 35)
            Me.cmbFiscalWeek.Name = "cmbFiscalWeek"
            Me.cmbFiscalWeek.Size = New System.Drawing.Size(150, 21)
            Me.cmbFiscalWeek.TabIndex = 7
            '
            'FiscalWeekBindingSource
            '
            Me.FiscalWeekBindingSource.DataMember = "FiscalWeek"
            '
            'cmbStore
            '
            Me.cmbStore.DataSource = Me.CompetitorStoreBindingSource
            Me.cmbStore.DisplayMember = "Name"
            Me.cmbStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbStore.Enabled = False
            Me.cmbStore.FormattingEnabled = True
            Me.cmbStore.Location = New System.Drawing.Point(349, 12)
            Me.cmbStore.Name = "cmbStore"
            Me.cmbStore.Size = New System.Drawing.Size(150, 21)
            Me.cmbStore.TabIndex = 6
            Me.cmbStore.ValueMember = "CompetitorStoreID"
            '
            'CompetitorStoreBindingSource
            '
            Me.CompetitorStoreBindingSource.DataMember = "CompetitorStore"
            '
            'lblFiscalWeek
            '
            Me.lblFiscalWeek.AutoSize = True
            Me.lblFiscalWeek.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblFiscalWeek.Location = New System.Drawing.Point(260, 38)
            Me.lblFiscalWeek.Name = "lblFiscalWeek"
            Me.lblFiscalWeek.Size = New System.Drawing.Size(85, 13)
            Me.lblFiscalWeek.TabIndex = 5
            Me.lblFiscalWeek.Text = "Fiscal Week :"
            Me.lblFiscalWeek.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'lblStore
            '
            Me.lblStore.AutoSize = True
            Me.lblStore.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblStore.Location = New System.Drawing.Point(265, 16)
            Me.lblStore.Name = "lblStore"
            Me.lblStore.Size = New System.Drawing.Size(80, 13)
            Me.lblStore.TabIndex = 4
            Me.lblStore.Text = "Comp Store :"
            Me.lblStore.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'cmbLocation
            '
            Me.cmbLocation.DataSource = Me.CompetitorLocationBindingSource
            Me.cmbLocation.DisplayMember = "Name"
            Me.cmbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbLocation.FormattingEnabled = True
            Me.cmbLocation.Location = New System.Drawing.Point(108, 35)
            Me.cmbLocation.Name = "cmbLocation"
            Me.cmbLocation.Size = New System.Drawing.Size(150, 21)
            Me.cmbLocation.TabIndex = 3
            Me.cmbLocation.ValueMember = "CompetitorLocationID"
            '
            'CompetitorLocationBindingSource
            '
            Me.CompetitorLocationBindingSource.DataMember = "CompetitorLocation"
            '
            'cmbCompetitor
            '
            Me.cmbCompetitor.DataSource = Me.CompetitorBindingSource
            Me.cmbCompetitor.DisplayMember = "Name"
            Me.cmbCompetitor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbCompetitor.FormattingEnabled = True
            Me.cmbCompetitor.Location = New System.Drawing.Point(108, 13)
            Me.cmbCompetitor.Name = "cmbCompetitor"
            Me.cmbCompetitor.Size = New System.Drawing.Size(150, 21)
            Me.cmbCompetitor.TabIndex = 2
            Me.cmbCompetitor.ValueMember = "CompetitorID"
            '
            'CompetitorBindingSource
            '
            Me.CompetitorBindingSource.DataMember = "Competitor"
            '
            'lblLocation
            '
            Me.lblLocation.AutoSize = True
            Me.lblLocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblLocation.Location = New System.Drawing.Point(2, 38)
            Me.lblLocation.Name = "lblLocation"
            Me.lblLocation.Size = New System.Drawing.Size(99, 13)
            Me.lblLocation.TabIndex = 1
            Me.lblLocation.Text = "Comp Location :"
            Me.lblLocation.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'lblCompetitor
            '
            Me.lblCompetitor.AutoSize = True
            Me.lblCompetitor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblCompetitor.Location = New System.Drawing.Point(27, 16)
            Me.lblCompetitor.Name = "lblCompetitor"
            Me.lblCompetitor.Size = New System.Drawing.Size(75, 13)
            Me.lblCompetitor.TabIndex = 0
            Me.lblCompetitor.Text = "Competitor :"
            Me.lblCompetitor.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'btnSave
            '
            Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnSave.BackColor = System.Drawing.Color.Transparent
            Me.btnSave.Location = New System.Drawing.Point(630, 381)
            Me.btnSave.Name = "btnSave"
            Me.btnSave.Size = New System.Drawing.Size(100, 23)
            Me.btnSave.TabIndex = 1
            Me.btnSave.Text = "Save"
            Me.btnSave.UseVisualStyleBackColor = False
            '
            'btnExit
            '
            Me.btnExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnExit.Location = New System.Drawing.Point(13, 381)
            Me.btnExit.Name = "btnExit"
            Me.btnExit.Size = New System.Drawing.Size(100, 23)
            Me.btnExit.TabIndex = 2
            Me.btnExit.Text = "Exit"
            Me.btnExit.UseVisualStyleBackColor = True
            '
            'cpgGrid
            '
            Me.cpgGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cpgGrid.Location = New System.Drawing.Point(12, 82)
            Me.cpgGrid.MinimumSize = New System.Drawing.Size(600, 250)
            Me.cpgGrid.Name = "cpgGrid"
            Me.cpgGrid.Size = New System.Drawing.Size(718, 293)
            Me.cpgGrid.TabIndex = 3
            '
            'btnItemSearch
            '
            Me.btnItemSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnItemSearch.CausesValidation = False
            Me.btnItemSearch.Location = New System.Drawing.Point(524, 381)
            Me.btnItemSearch.Name = "btnItemSearch"
            Me.btnItemSearch.Size = New System.Drawing.Size(100, 23)
            Me.btnItemSearch.TabIndex = 4
            Me.btnItemSearch.Text = "Item Search"
            Me.btnItemSearch.UseVisualStyleBackColor = True
            '
            'btnStoreSearch
            '
            Me.btnStoreSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnStoreSearch.CausesValidation = False
            Me.btnStoreSearch.Location = New System.Drawing.Point(418, 381)
            Me.btnStoreSearch.Name = "btnStoreSearch"
            Me.btnStoreSearch.Size = New System.Drawing.Size(100, 23)
            Me.btnStoreSearch.TabIndex = 5
            Me.btnStoreSearch.Text = "Store Search"
            Me.btnStoreSearch.UseVisualStyleBackColor = True
            '
            'CompetitorDataManagement
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.ClientSize = New System.Drawing.Size(742, 416)
            Me.Controls.Add(Me.btnStoreSearch)
            Me.Controls.Add(Me.btnItemSearch)
            Me.Controls.Add(Me.cpgGrid)
            Me.Controls.Add(Me.btnExit)
            Me.Controls.Add(Me.btnSave)
            Me.Controls.Add(Me.gbSearchCriteria)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(750, 450)
            Me.Name = "CompetitorDataManagement"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.Text = "Competitor Data Management"
            Me.gbSearchCriteria.ResumeLayout(False)
            Me.gbSearchCriteria.PerformLayout()
            CType(Me.FiscalWeekBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.CompetitorStoreBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.CompetitorLocationBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.CompetitorBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents gbSearchCriteria As System.Windows.Forms.GroupBox
        Friend WithEvents lblCompetitor As System.Windows.Forms.Label
        Friend WithEvents cmbLocation As System.Windows.Forms.ComboBox
        Friend WithEvents cmbCompetitor As System.Windows.Forms.ComboBox
        Friend WithEvents lblLocation As System.Windows.Forms.Label
        Friend WithEvents CompetitorLocationBindingSource As System.Windows.Forms.BindingSource
        Friend WithEvents CompetitorBindingSource As System.Windows.Forms.BindingSource
        Friend WithEvents lblFiscalWeek As System.Windows.Forms.Label
        Friend WithEvents lblStore As System.Windows.Forms.Label
        Friend WithEvents cmbStore As System.Windows.Forms.ComboBox
        Friend WithEvents CompetitorStoreBindingSource As System.Windows.Forms.BindingSource
        Friend WithEvents cmbFiscalWeek As System.Windows.Forms.ComboBox
        Friend WithEvents FiscalWeekBindingSource As System.Windows.Forms.BindingSource
        Friend WithEvents lblWFMIdentifier As System.Windows.Forms.Label
        Friend WithEvents btnSearch As System.Windows.Forms.Button
        Friend WithEvents txtWFMIdentifier As System.Windows.Forms.TextBox
        Friend WithEvents btnSave As System.Windows.Forms.Button
        Friend WithEvents btnExit As System.Windows.Forms.Button
        Friend WithEvents cpgGrid As WholeFoods.IRMA.CompetitorStore.UserInterface.CompetitorPriceGrid
        Friend WithEvents btnItemSearch As System.Windows.Forms.Button
        Friend WithEvents btnStoreSearch As System.Windows.Forms.Button
    End Class
End Namespace