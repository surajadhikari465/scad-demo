Namespace WholeFoods.IRMA.CompetitorStore.UserInterface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class CompetitorStoreSearchForm
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
            Me.btnSelectStore = New System.Windows.Forms.Button
            Me.btnCancel = New System.Windows.Forms.Button
            Me.gbSearch = New System.Windows.Forms.GroupBox
            Me.cbLocation = New System.Windows.Forms.ComboBox
            Me.CompetitorLocationBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.ResultsDataSet = New WholeFoods.IRMA.CompetitorStore.BusinessLogic.CompetitorStoreDataSet
            Me.cbCompetitor = New System.Windows.Forms.ComboBox
            Me.CompetitorBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.lblLocation = New System.Windows.Forms.Label
            Me.lblCompetitor = New System.Windows.Forms.Label
            Me.btnSearch = New System.Windows.Forms.Button
            Me.gbResults = New System.Windows.Forms.GroupBox
            Me.lvResults = New System.Windows.Forms.ListView
            Me.colCompetitor = New System.Windows.Forms.ColumnHeader
            Me.colLocation = New System.Windows.Forms.ColumnHeader
            Me.colStore = New System.Windows.Forms.ColumnHeader
            Me.gbSearch.SuspendLayout()
            CType(Me.CompetitorLocationBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.ResultsDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.CompetitorBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.gbResults.SuspendLayout()
            Me.SuspendLayout()
            '
            'btnSelectStore
            '
            Me.btnSelectStore.Location = New System.Drawing.Point(352, 447)
            Me.btnSelectStore.Name = "btnSelectStore"
            Me.btnSelectStore.Size = New System.Drawing.Size(75, 23)
            Me.btnSelectStore.TabIndex = 0
            Me.btnSelectStore.Text = "Select Store"
            Me.btnSelectStore.UseVisualStyleBackColor = True
            '
            'btnCancel
            '
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(12, 447)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 23)
            Me.btnCancel.TabIndex = 1
            Me.btnCancel.Text = "Cancel"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'gbSearch
            '
            Me.gbSearch.BackColor = System.Drawing.Color.Transparent
            Me.gbSearch.Controls.Add(Me.cbLocation)
            Me.gbSearch.Controls.Add(Me.cbCompetitor)
            Me.gbSearch.Controls.Add(Me.lblLocation)
            Me.gbSearch.Controls.Add(Me.lblCompetitor)
            Me.gbSearch.Controls.Add(Me.btnSearch)
            Me.gbSearch.Location = New System.Drawing.Point(12, 12)
            Me.gbSearch.Name = "gbSearch"
            Me.gbSearch.Size = New System.Drawing.Size(415, 79)
            Me.gbSearch.TabIndex = 2
            Me.gbSearch.TabStop = False
            Me.gbSearch.Text = "Search"
            '
            'cbLocation
            '
            Me.cbLocation.DataSource = Me.CompetitorLocationBindingSource
            Me.cbLocation.DisplayMember = "Name"
            Me.cbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cbLocation.Location = New System.Drawing.Point(105, 41)
            Me.cbLocation.Name = "cbLocation"
            Me.cbLocation.Size = New System.Drawing.Size(187, 21)
            Me.cbLocation.TabIndex = 4
            Me.cbLocation.ValueMember = "CompetitorLocationID"
            '
            'CompetitorLocationBindingSource
            '
            Me.CompetitorLocationBindingSource.DataMember = "CompetitorLocation"
            Me.CompetitorLocationBindingSource.DataSource = Me.ResultsDataSet
            '
            'ResultsDataSet
            '
            Me.ResultsDataSet.DataSetName = "CompetitorStoreDataSet"
            Me.ResultsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'cbCompetitor
            '
            Me.cbCompetitor.DataSource = Me.CompetitorBindingSource
            Me.cbCompetitor.DisplayMember = "Name"
            Me.cbCompetitor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cbCompetitor.Location = New System.Drawing.Point(105, 13)
            Me.cbCompetitor.Name = "cbCompetitor"
            Me.cbCompetitor.Size = New System.Drawing.Size(187, 21)
            Me.cbCompetitor.TabIndex = 3
            Me.cbCompetitor.ValueMember = "CompetitorID"
            '
            'CompetitorBindingSource
            '
            Me.CompetitorBindingSource.DataMember = "Competitor"
            Me.CompetitorBindingSource.DataSource = Me.ResultsDataSet
            '
            'lblLocation
            '
            Me.lblLocation.AutoSize = True
            Me.lblLocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblLocation.Location = New System.Drawing.Point(35, 44)
            Me.lblLocation.Name = "lblLocation"
            Me.lblLocation.Size = New System.Drawing.Size(64, 13)
            Me.lblLocation.TabIndex = 2
            Me.lblLocation.Text = "Location :"
            Me.lblLocation.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'lblCompetitor
            '
            Me.lblCompetitor.AutoSize = True
            Me.lblCompetitor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblCompetitor.Location = New System.Drawing.Point(24, 16)
            Me.lblCompetitor.Name = "lblCompetitor"
            Me.lblCompetitor.Size = New System.Drawing.Size(75, 13)
            Me.lblCompetitor.TabIndex = 1
            Me.lblCompetitor.Text = "Competitor :"
            Me.lblCompetitor.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'btnSearch
            '
            Me.btnSearch.Location = New System.Drawing.Point(334, 50)
            Me.btnSearch.Name = "btnSearch"
            Me.btnSearch.Size = New System.Drawing.Size(75, 23)
            Me.btnSearch.TabIndex = 0
            Me.btnSearch.Text = "Search"
            Me.btnSearch.UseVisualStyleBackColor = True
            '
            'gbResults
            '
            Me.gbResults.BackColor = System.Drawing.Color.Transparent
            Me.gbResults.Controls.Add(Me.lvResults)
            Me.gbResults.Location = New System.Drawing.Point(12, 97)
            Me.gbResults.Name = "gbResults"
            Me.gbResults.Size = New System.Drawing.Size(415, 344)
            Me.gbResults.TabIndex = 3
            Me.gbResults.TabStop = False
            Me.gbResults.Text = "Results"
            '
            'lvResults
            '
            Me.lvResults.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colCompetitor, Me.colLocation, Me.colStore})
            Me.lvResults.FullRowSelect = True
            Me.lvResults.GridLines = True
            Me.lvResults.HideSelection = False
            Me.lvResults.Location = New System.Drawing.Point(6, 19)
            Me.lvResults.MultiSelect = False
            Me.lvResults.Name = "lvResults"
            Me.lvResults.ShowGroups = False
            Me.lvResults.Size = New System.Drawing.Size(403, 319)
            Me.lvResults.TabIndex = 0
            Me.lvResults.UseCompatibleStateImageBehavior = False
            Me.lvResults.View = System.Windows.Forms.View.Details
            '
            'colCompetitor
            '
            Me.colCompetitor.Text = "Competitor"
            Me.colCompetitor.Width = 125
            '
            'colLocation
            '
            Me.colLocation.Text = "Location"
            Me.colLocation.Width = 125
            '
            'colStore
            '
            Me.colStore.Text = "Store"
            Me.colStore.Width = 125
            '
            'CompetitorStoreSearchForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.CancelButton = Me.btnCancel
            Me.ClientSize = New System.Drawing.Size(439, 482)
            Me.ControlBox = False
            Me.Controls.Add(Me.gbResults)
            Me.Controls.Add(Me.gbSearch)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnSelectStore)
            Me.Name = "CompetitorStoreSearchForm"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.Text = "Competitor Store Search"
            Me.gbSearch.ResumeLayout(False)
            Me.gbSearch.PerformLayout()
            CType(Me.CompetitorLocationBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.ResultsDataSet, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.CompetitorBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            Me.gbResults.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents btnSelectStore As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents gbSearch As System.Windows.Forms.GroupBox
        Friend WithEvents btnSearch As System.Windows.Forms.Button
        Friend WithEvents gbResults As System.Windows.Forms.GroupBox
        Friend WithEvents lblCompetitor As System.Windows.Forms.Label
        Friend WithEvents lblLocation As System.Windows.Forms.Label
        Friend WithEvents cbCompetitor As System.Windows.Forms.ComboBox
        Friend WithEvents cbLocation As System.Windows.Forms.ComboBox
        Friend WithEvents CompetitorLocationBindingSource As System.Windows.Forms.BindingSource
        Friend WithEvents CompetitorBindingSource As System.Windows.Forms.BindingSource
        Friend WithEvents ResultsDataSet As BusinessLogic.CompetitorStoreDataSet
        Friend WithEvents lvResults As System.Windows.Forms.ListView
        Friend WithEvents colCompetitor As System.Windows.Forms.ColumnHeader
        Friend WithEvents colLocation As System.Windows.Forms.ColumnHeader
        Friend WithEvents colStore As System.Windows.Forms.ColumnHeader
    End Class
End Namespace