Namespace WholeFoods.IRMA.CompetitorStore.UserInterface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class ImportPreview
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
            Me.gbFilter = New System.Windows.Forms.GroupBox
            Me.rbMissingData = New System.Windows.Forms.RadioButton
            Me.rbUnmatchedUPCs = New System.Windows.Forms.RadioButton
            Me.rbUnmatchedStores = New System.Windows.Forms.RadioButton
            Me.rbAllItems = New System.Windows.Forms.RadioButton
            Me.btnCancel = New System.Windows.Forms.Button
            Me.btnDone = New System.Windows.Forms.Button
            Me.btnItemSearch = New System.Windows.Forms.Button
            Me.btnStoreSearch = New System.Windows.Forms.Button
            Me.cgGrid = New WholeFoods.IRMA.CompetitorStore.UserInterface.CompetitorImportInfoGrid
            Me.gbFilter.SuspendLayout()
            Me.SuspendLayout()
            '
            'gbFilter
            '
            Me.gbFilter.BackColor = System.Drawing.Color.Transparent
            Me.gbFilter.Controls.Add(Me.rbMissingData)
            Me.gbFilter.Controls.Add(Me.rbUnmatchedUPCs)
            Me.gbFilter.Controls.Add(Me.rbUnmatchedStores)
            Me.gbFilter.Controls.Add(Me.rbAllItems)
            Me.gbFilter.Location = New System.Drawing.Point(12, 12)
            Me.gbFilter.Name = "gbFilter"
            Me.gbFilter.Size = New System.Drawing.Size(718, 42)
            Me.gbFilter.TabIndex = 0
            Me.gbFilter.TabStop = False
            Me.gbFilter.Text = "Filter"
            '
            'rbMissingData
            '
            Me.rbMissingData.AutoSize = True
            Me.rbMissingData.Location = New System.Drawing.Point(557, 20)
            Me.rbMissingData.Name = "rbMissingData"
            Me.rbMissingData.Size = New System.Drawing.Size(86, 17)
            Me.rbMissingData.TabIndex = 3
            Me.rbMissingData.TabStop = True
            Me.rbMissingData.Text = "Missing Data"
            Me.rbMissingData.UseVisualStyleBackColor = True
            '
            'rbUnmatchedUPCs
            '
            Me.rbUnmatchedUPCs.AutoSize = True
            Me.rbUnmatchedUPCs.Location = New System.Drawing.Point(363, 20)
            Me.rbUnmatchedUPCs.Name = "rbUnmatchedUPCs"
            Me.rbUnmatchedUPCs.Size = New System.Drawing.Size(110, 17)
            Me.rbUnmatchedUPCs.TabIndex = 2
            Me.rbUnmatchedUPCs.TabStop = True
            Me.rbUnmatchedUPCs.Text = "Unmatched UPCs"
            Me.rbUnmatchedUPCs.UseVisualStyleBackColor = True
            '
            'rbUnmatchedStores
            '
            Me.rbUnmatchedStores.AutoSize = True
            Me.rbUnmatchedStores.Location = New System.Drawing.Point(166, 20)
            Me.rbUnmatchedStores.Name = "rbUnmatchedStores"
            Me.rbUnmatchedStores.Size = New System.Drawing.Size(113, 17)
            Me.rbUnmatchedStores.TabIndex = 1
            Me.rbUnmatchedStores.TabStop = True
            Me.rbUnmatchedStores.Text = "Unmatched Stores"
            Me.rbUnmatchedStores.UseVisualStyleBackColor = True
            '
            'rbAllItems
            '
            Me.rbAllItems.AutoSize = True
            Me.rbAllItems.Checked = True
            Me.rbAllItems.Location = New System.Drawing.Point(7, 20)
            Me.rbAllItems.Name = "rbAllItems"
            Me.rbAllItems.Size = New System.Drawing.Size(64, 17)
            Me.rbAllItems.TabIndex = 0
            Me.rbAllItems.TabStop = True
            Me.rbAllItems.Text = "All Items"
            Me.rbAllItems.UseVisualStyleBackColor = True
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.Location = New System.Drawing.Point(12, 407)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(90, 23)
            Me.btnCancel.TabIndex = 1
            Me.btnCancel.Text = "Cancel"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'btnDone
            '
            Me.btnDone.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnDone.Location = New System.Drawing.Point(640, 407)
            Me.btnDone.Name = "btnDone"
            Me.btnDone.Size = New System.Drawing.Size(90, 23)
            Me.btnDone.TabIndex = 2
            Me.btnDone.Text = "Done"
            Me.btnDone.UseVisualStyleBackColor = True
            '
            'btnItemSearch
            '
            Me.btnItemSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnItemSearch.Location = New System.Drawing.Point(448, 407)
            Me.btnItemSearch.Name = "btnItemSearch"
            Me.btnItemSearch.Size = New System.Drawing.Size(90, 23)
            Me.btnItemSearch.TabIndex = 4
            Me.btnItemSearch.Text = "Item Search"
            Me.btnItemSearch.UseVisualStyleBackColor = True
            '
            'btnStoreSearch
            '
            Me.btnStoreSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnStoreSearch.Location = New System.Drawing.Point(352, 407)
            Me.btnStoreSearch.Name = "btnStoreSearch"
            Me.btnStoreSearch.Size = New System.Drawing.Size(90, 23)
            Me.btnStoreSearch.TabIndex = 5
            Me.btnStoreSearch.Text = "Store Search"
            Me.btnStoreSearch.UseVisualStyleBackColor = True
            '
            'cgGrid
            '
            Me.cgGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cgGrid.Location = New System.Drawing.Point(12, 60)
            Me.cgGrid.MinimumSize = New System.Drawing.Size(600, 250)
            Me.cgGrid.Name = "cgGrid"
            Me.cgGrid.Size = New System.Drawing.Size(718, 341)
            Me.cgGrid.TabIndex = 3
            '
            'ImportPreview
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.ClientSize = New System.Drawing.Size(742, 442)
            Me.Controls.Add(Me.btnStoreSearch)
            Me.Controls.Add(Me.btnItemSearch)
            Me.Controls.Add(Me.btnDone)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.gbFilter)
            Me.Controls.Add(Me.cgGrid)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(750, 450)
            Me.Name = "ImportPreview"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.Text = "Competitor Data Import Preview"
            Me.gbFilter.ResumeLayout(False)
            Me.gbFilter.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents gbFilter As System.Windows.Forms.GroupBox
        Friend WithEvents rbMissingData As System.Windows.Forms.RadioButton
        Friend WithEvents rbUnmatchedUPCs As System.Windows.Forms.RadioButton
        Friend WithEvents rbUnmatchedStores As System.Windows.Forms.RadioButton
        Friend WithEvents rbAllItems As System.Windows.Forms.RadioButton
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents btnDone As System.Windows.Forms.Button
        Friend WithEvents btnItemSearch As System.Windows.Forms.Button
        Friend WithEvents btnStoreSearch As System.Windows.Forms.Button
        Friend WithEvents cgGrid As WholeFoods.IRMA.CompetitorStore.UserInterface.CompetitorImportInfoGrid
    End Class
End Namespace