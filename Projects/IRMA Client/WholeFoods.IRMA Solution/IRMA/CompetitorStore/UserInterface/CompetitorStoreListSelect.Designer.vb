Namespace WholeFoods.IRMA.CompetitorStore.UserInterface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class CompetitorStoreListSelect
        Inherits WholeFoods.IRMA.ItemChaining.UserInterface.ListSelect

        'UserControl overrides dispose to clean up the component list.
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
            Me.lstSelected = New System.Windows.Forms.ListView
            Me.colPriority = New System.Windows.Forms.ColumnHeader
            Me.colCompetitor2 = New System.Windows.Forms.ColumnHeader
            Me.colStore2 = New System.Windows.Forms.ColumnHeader
            Me.colLocation2 = New System.Windows.Forms.ColumnHeader
            Me.lstAvailable = New System.Windows.Forms.ListView
            Me.colCompetitor = New System.Windows.Forms.ColumnHeader
            Me.colStore = New System.Windows.Forms.ColumnHeader
            Me.colLocation = New System.Windows.Forms.ColumnHeader
            Me.btnRemoveAll = New System.Windows.Forms.Button
            Me.btnAddAll = New System.Windows.Forms.Button
            Me.btnRemove = New System.Windows.Forms.Button
            Me.btnAdd = New System.Windows.Forms.Button
            Me.lblSelectedStores = New System.Windows.Forms.Label
            Me.lblAvailableStores = New System.Windows.Forms.Label
            Me.lblFilterBy = New System.Windows.Forms.Label
            Me.lblCompetitor = New System.Windows.Forms.Label
            Me.lblLocation = New System.Windows.Forms.Label
            Me.cmbCompetitor = New System.Windows.Forms.ComboBox
            Me.cmbLocation = New System.Windows.Forms.ComboBox
            Me.pnlFilter = New System.Windows.Forms.Panel
            Me.btnIncreasePriority = New System.Windows.Forms.Button
            Me.btnDecreasePriority = New System.Windows.Forms.Button
            Me.pnlFilter.SuspendLayout()
            Me.SuspendLayout()
            '
            'lstSelected
            '
            Me.lstSelected.AllowDrop = True
            Me.lstSelected.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lstSelected.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colPriority, Me.colCompetitor2, Me.colStore2, Me.colLocation2})
            Me.lstSelected.FullRowSelect = True
            Me.lstSelected.GridLines = True
            Me.lstSelected.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            Me.lstSelected.HideSelection = False
            Me.lstSelected.Location = New System.Drawing.Point(340, 20)
            Me.lstSelected.Name = "lstSelected"
            Me.lstSelected.Size = New System.Drawing.Size(357, 150)
            Me.lstSelected.TabIndex = 30
            Me.lstSelected.UseCompatibleStateImageBehavior = False
            Me.lstSelected.View = System.Windows.Forms.View.Details
            '
            'colPriority
            '
            Me.colPriority.Text = "Priority"
            Me.colPriority.Width = 50
            '
            'colCompetitor2
            '
            Me.colCompetitor2.Text = "Competitor"
            Me.colCompetitor2.Width = 100
            '
            'colStore2
            '
            Me.colStore2.Text = "Store"
            Me.colStore2.Width = 100
            '
            'colLocation2
            '
            Me.colLocation2.Text = "Location"
            Me.colLocation2.Width = 100
            '
            'lstAvailable
            '
            Me.lstAvailable.AllowDrop = True
            Me.lstAvailable.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.lstAvailable.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colCompetitor, Me.colStore, Me.colLocation})
            Me.lstAvailable.FullRowSelect = True
            Me.lstAvailable.GridLines = True
            Me.lstAvailable.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            Me.lstAvailable.HideSelection = False
            Me.lstAvailable.Location = New System.Drawing.Point(3, 20)
            Me.lstAvailable.Name = "lstAvailable"
            Me.lstAvailable.Size = New System.Drawing.Size(305, 150)
            Me.lstAvailable.TabIndex = 29
            Me.lstAvailable.UseCompatibleStateImageBehavior = False
            Me.lstAvailable.View = System.Windows.Forms.View.Details
            '
            'colCompetitor
            '
            Me.colCompetitor.Text = "Competitor"
            Me.colCompetitor.Width = 100
            '
            'colStore
            '
            Me.colStore.Text = "Store"
            Me.colStore.Width = 100
            '
            'colLocation
            '
            Me.colLocation.Text = "Location"
            Me.colLocation.Width = 100
            '
            'btnRemoveAll
            '
            Me.btnRemoveAll.Enabled = False
            Me.btnRemoveAll.Location = New System.Drawing.Point(311, 107)
            Me.btnRemoveAll.Name = "btnRemoveAll"
            Me.btnRemoveAll.Size = New System.Drawing.Size(28, 23)
            Me.btnRemoveAll.TabIndex = 26
            Me.btnRemoveAll.Text = "<<"
            Me.btnRemoveAll.UseVisualStyleBackColor = True
            '
            'btnAddAll
            '
            Me.btnAddAll.Enabled = False
            Me.btnAddAll.Location = New System.Drawing.Point(311, 78)
            Me.btnAddAll.Name = "btnAddAll"
            Me.btnAddAll.Size = New System.Drawing.Size(28, 23)
            Me.btnAddAll.TabIndex = 25
            Me.btnAddAll.Text = ">>"
            Me.btnAddAll.UseVisualStyleBackColor = True
            '
            'btnRemove
            '
            Me.btnRemove.Enabled = False
            Me.btnRemove.Location = New System.Drawing.Point(311, 49)
            Me.btnRemove.Name = "btnRemove"
            Me.btnRemove.Size = New System.Drawing.Size(28, 23)
            Me.btnRemove.TabIndex = 24
            Me.btnRemove.Text = "<"
            Me.btnRemove.UseVisualStyleBackColor = True
            '
            'btnAdd
            '
            Me.btnAdd.Enabled = False
            Me.btnAdd.Location = New System.Drawing.Point(311, 20)
            Me.btnAdd.Name = "btnAdd"
            Me.btnAdd.Size = New System.Drawing.Size(28, 23)
            Me.btnAdd.TabIndex = 23
            Me.btnAdd.Text = ">"
            Me.btnAdd.UseVisualStyleBackColor = True
            '
            'lblSelectedStores
            '
            Me.lblSelectedStores.AutoSize = True
            Me.lblSelectedStores.Location = New System.Drawing.Point(342, 4)
            Me.lblSelectedStores.Name = "lblSelectedStores"
            Me.lblSelectedStores.Size = New System.Drawing.Size(138, 13)
            Me.lblSelectedStores.TabIndex = 28
            Me.lblSelectedStores.Text = "Selected Competitor Stores:"
            '
            'lblAvailableStores
            '
            Me.lblAvailableStores.AutoSize = True
            Me.lblAvailableStores.Location = New System.Drawing.Point(0, 4)
            Me.lblAvailableStores.Name = "lblAvailableStores"
            Me.lblAvailableStores.Size = New System.Drawing.Size(139, 13)
            Me.lblAvailableStores.TabIndex = 27
            Me.lblAvailableStores.Text = "Available Competitor Stores:"
            '
            'lblFilterBy
            '
            Me.lblFilterBy.AutoSize = True
            Me.lblFilterBy.Location = New System.Drawing.Point(3, 0)
            Me.lblFilterBy.Name = "lblFilterBy"
            Me.lblFilterBy.Size = New System.Drawing.Size(53, 13)
            Me.lblFilterBy.TabIndex = 31
            Me.lblFilterBy.Text = "Filter By..."
            '
            'lblCompetitor
            '
            Me.lblCompetitor.AutoSize = True
            Me.lblCompetitor.Location = New System.Drawing.Point(16, 20)
            Me.lblCompetitor.Name = "lblCompetitor"
            Me.lblCompetitor.Size = New System.Drawing.Size(63, 13)
            Me.lblCompetitor.TabIndex = 32
            Me.lblCompetitor.Text = "Competitor :"
            '
            'lblLocation
            '
            Me.lblLocation.AutoSize = True
            Me.lblLocation.Location = New System.Drawing.Point(25, 42)
            Me.lblLocation.Name = "lblLocation"
            Me.lblLocation.Size = New System.Drawing.Size(54, 13)
            Me.lblLocation.TabIndex = 33
            Me.lblLocation.Text = "Location :"
            Me.lblLocation.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'cmbCompetitor
            '
            Me.cmbCompetitor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbCompetitor.FormattingEnabled = True
            Me.cmbCompetitor.Location = New System.Drawing.Point(86, 17)
            Me.cmbCompetitor.Name = "cmbCompetitor"
            Me.cmbCompetitor.Size = New System.Drawing.Size(150, 21)
            Me.cmbCompetitor.TabIndex = 34
            '
            'cmbLocation
            '
            Me.cmbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbLocation.FormattingEnabled = True
            Me.cmbLocation.Location = New System.Drawing.Point(86, 39)
            Me.cmbLocation.Name = "cmbLocation"
            Me.cmbLocation.Size = New System.Drawing.Size(150, 21)
            Me.cmbLocation.TabIndex = 35
            '
            'pnlFilter
            '
            Me.pnlFilter.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.pnlFilter.Controls.Add(Me.lblFilterBy)
            Me.pnlFilter.Controls.Add(Me.cmbLocation)
            Me.pnlFilter.Controls.Add(Me.lblCompetitor)
            Me.pnlFilter.Controls.Add(Me.cmbCompetitor)
            Me.pnlFilter.Controls.Add(Me.lblLocation)
            Me.pnlFilter.Location = New System.Drawing.Point(3, 176)
            Me.pnlFilter.Name = "pnlFilter"
            Me.pnlFilter.Size = New System.Drawing.Size(305, 63)
            Me.pnlFilter.TabIndex = 36
            '
            'btnIncreasePriority
            '
            Me.btnIncreasePriority.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnIncreasePriority.Location = New System.Drawing.Point(443, 176)
            Me.btnIncreasePriority.Name = "btnIncreasePriority"
            Me.btnIncreasePriority.Size = New System.Drawing.Size(75, 23)
            Me.btnIncreasePriority.TabIndex = 37
            Me.btnIncreasePriority.Text = "/\"
            Me.btnIncreasePriority.UseVisualStyleBackColor = True
            '
            'btnDecreasePriority
            '
            Me.btnDecreasePriority.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnDecreasePriority.Location = New System.Drawing.Point(524, 176)
            Me.btnDecreasePriority.Name = "btnDecreasePriority"
            Me.btnDecreasePriority.Size = New System.Drawing.Size(75, 23)
            Me.btnDecreasePriority.TabIndex = 38
            Me.btnDecreasePriority.Text = "\/"
            Me.btnDecreasePriority.UseVisualStyleBackColor = True
            '
            'CompetitorStoreListSelect
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.btnDecreasePriority)
            Me.Controls.Add(Me.btnIncreasePriority)
            Me.Controls.Add(Me.pnlFilter)
            Me.Controls.Add(Me.lstSelected)
            Me.Controls.Add(Me.lstAvailable)
            Me.Controls.Add(Me.btnRemoveAll)
            Me.Controls.Add(Me.btnAddAll)
            Me.Controls.Add(Me.btnRemove)
            Me.Controls.Add(Me.btnAdd)
            Me.Controls.Add(Me.lblSelectedStores)
            Me.Controls.Add(Me.lblAvailableStores)
            Me.MinimumSize = New System.Drawing.Size(700, 175)
            Me.Name = "CompetitorStoreListSelect"
            Me.Size = New System.Drawing.Size(700, 250)
            Me.pnlFilter.ResumeLayout(False)
            Me.pnlFilter.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents lstSelected As System.Windows.Forms.ListView
        Friend WithEvents colPriority As System.Windows.Forms.ColumnHeader
        Friend WithEvents colCompetitor2 As System.Windows.Forms.ColumnHeader
        Friend WithEvents lstAvailable As System.Windows.Forms.ListView
        Friend WithEvents colCompetitor As System.Windows.Forms.ColumnHeader
        Friend WithEvents colStore As System.Windows.Forms.ColumnHeader
        Friend WithEvents colLocation As System.Windows.Forms.ColumnHeader
        Friend WithEvents btnRemoveAll As System.Windows.Forms.Button
        Friend WithEvents btnAddAll As System.Windows.Forms.Button
        Friend WithEvents btnRemove As System.Windows.Forms.Button
        Friend WithEvents btnAdd As System.Windows.Forms.Button
        Friend WithEvents lblSelectedStores As System.Windows.Forms.Label
        Friend WithEvents lblAvailableStores As System.Windows.Forms.Label
        Friend WithEvents colStore2 As System.Windows.Forms.ColumnHeader
        Friend WithEvents colLocation2 As System.Windows.Forms.ColumnHeader
        Friend WithEvents lblFilterBy As System.Windows.Forms.Label
        Friend WithEvents lblCompetitor As System.Windows.Forms.Label
        Friend WithEvents lblLocation As System.Windows.Forms.Label
        Friend WithEvents cmbCompetitor As System.Windows.Forms.ComboBox
        Friend WithEvents cmbLocation As System.Windows.Forms.ComboBox
        Friend WithEvents pnlFilter As System.Windows.Forms.Panel
        Friend WithEvents btnIncreasePriority As System.Windows.Forms.Button
        Friend WithEvents btnDecreasePriority As System.Windows.Forms.Button

    End Class
End Namespace