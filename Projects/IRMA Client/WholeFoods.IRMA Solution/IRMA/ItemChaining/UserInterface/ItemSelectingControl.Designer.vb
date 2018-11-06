Namespace WholeFoods.IRMA.ItemChaining.UserInterface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class ItemSelectingControl
        Inherits ListSelect

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
            Me.components = New System.ComponentModel.Container
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ItemSelectingControl))
            Me.btnExport = New System.Windows.Forms.Button
            Me.btnClear = New System.Windows.Forms.Button
            Me.lblSelectedItems = New System.Windows.Forms.Label
            Me.lblFoundItems = New System.Windows.Forms.Label
            Me.btnRemoveAll = New System.Windows.Forms.Button
            Me.btnAddAll = New System.Windows.Forms.Button
            Me.btnRemove = New System.Windows.Forms.Button
            Me.btnAdd = New System.Windows.Forms.Button
            Me.lstFound = New System.Windows.Forms.ListView
            Me.FoundItem = New System.Windows.Forms.ColumnHeader
            Me.FoundIdentifier = New System.Windows.Forms.ColumnHeader
            Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
            Me.lstSelected = New System.Windows.Forms.ListView
            Me.SelectedItem = New System.Windows.Forms.ColumnHeader
            Me.SelectIdentifier = New System.Windows.Forms.ColumnHeader
            Me.SuspendLayout()
            '
            'btnExport
            '
            Me.btnExport.Enabled = False
            Me.btnExport.Location = New System.Drawing.Point(258, 235)
            Me.btnExport.Name = "btnExport"
            Me.btnExport.Size = New System.Drawing.Size(85, 23)
            Me.btnExport.TabIndex = 19
            Me.btnExport.Text = "Export..."
            Me.btnExport.UseVisualStyleBackColor = True
            Me.btnExport.Visible = False
            '
            'btnClear
            '
            Me.btnClear.Enabled = False
            Me.btnClear.Location = New System.Drawing.Point(396, 235)
            Me.btnClear.Name = "btnClear"
            Me.btnClear.Size = New System.Drawing.Size(85, 23)
            Me.btnClear.TabIndex = 18
            Me.btnClear.Text = "Clear"
            Me.btnClear.UseVisualStyleBackColor = True
            '
            'lblSelectedItems
            '
            Me.lblSelectedItems.AutoSize = True
            Me.lblSelectedItems.Location = New System.Drawing.Point(259, 6)
            Me.lblSelectedItems.Name = "lblSelectedItems"
            Me.lblSelectedItems.Size = New System.Drawing.Size(80, 13)
            Me.lblSelectedItems.TabIndex = 17
            Me.lblSelectedItems.Text = "Selected Items:"
            '
            'lblFoundItems
            '
            Me.lblFoundItems.AutoSize = True
            Me.lblFoundItems.Location = New System.Drawing.Point(0, 6)
            Me.lblFoundItems.Name = "lblFoundItems"
            Me.lblFoundItems.Size = New System.Drawing.Size(68, 13)
            Me.lblFoundItems.TabIndex = 16
            Me.lblFoundItems.Text = "Found Items:"
            '
            'btnRemoveAll
            '
            Me.btnRemoveAll.Enabled = False
            Me.btnRemoveAll.Location = New System.Drawing.Point(228, 109)
            Me.btnRemoveAll.Name = "btnRemoveAll"
            Me.btnRemoveAll.Size = New System.Drawing.Size(28, 23)
            Me.btnRemoveAll.TabIndex = 15
            Me.btnRemoveAll.Text = "<<"
            Me.btnRemoveAll.UseVisualStyleBackColor = True
            '
            'btnAddAll
            '
            Me.btnAddAll.Enabled = False
            Me.btnAddAll.Location = New System.Drawing.Point(228, 80)
            Me.btnAddAll.Name = "btnAddAll"
            Me.btnAddAll.Size = New System.Drawing.Size(28, 23)
            Me.btnAddAll.TabIndex = 14
            Me.btnAddAll.Text = ">>"
            Me.btnAddAll.UseVisualStyleBackColor = True
            '
            'btnRemove
            '
            Me.btnRemove.Enabled = False
            Me.btnRemove.Location = New System.Drawing.Point(228, 51)
            Me.btnRemove.Name = "btnRemove"
            Me.btnRemove.Size = New System.Drawing.Size(28, 23)
            Me.btnRemove.TabIndex = 13
            Me.btnRemove.Text = "<"
            Me.btnRemove.UseVisualStyleBackColor = True
            '
            'btnAdd
            '
            Me.btnAdd.Enabled = False
            Me.btnAdd.Location = New System.Drawing.Point(228, 22)
            Me.btnAdd.Name = "btnAdd"
            Me.btnAdd.Size = New System.Drawing.Size(28, 23)
            Me.btnAdd.TabIndex = 12
            Me.btnAdd.Text = ">"
            Me.btnAdd.UseVisualStyleBackColor = True
            '
            'lstFound
            '
            Me.lstFound.AllowDrop = True
            Me.lstFound.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.FoundItem, Me.FoundIdentifier})
            Me.lstFound.FullRowSelect = True
            Me.lstFound.GridLines = True
            Me.lstFound.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            Me.lstFound.HideSelection = False
            Me.lstFound.Location = New System.Drawing.Point(3, 22)
            Me.lstFound.Name = "lstFound"
            Me.lstFound.Size = New System.Drawing.Size(224, 210)
            Me.lstFound.StateImageList = Me.ImageList1
            Me.lstFound.TabIndex = 21
            Me.lstFound.UseCompatibleStateImageBehavior = False
            Me.lstFound.View = System.Windows.Forms.View.Details
            '
            'FoundItem
            '
            Me.FoundItem.Text = "Item"
            Me.FoundItem.Width = 156
            '
            'FoundIdentifier
            '
            Me.FoundIdentifier.Text = "Identifier"
            '
            'ImageList1
            '
            Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
            Me.ImageList1.Images.SetKeyName(0, "blank.bmp")
            Me.ImageList1.Images.SetKeyName(1, "chain.bmp")
            '
            'lstSelected
            '
            Me.lstSelected.AllowDrop = True
            Me.lstSelected.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.SelectedItem, Me.SelectIdentifier})
            Me.lstSelected.FullRowSelect = True
            Me.lstSelected.GridLines = True
            Me.lstSelected.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            Me.lstSelected.HideSelection = False
            Me.lstSelected.Location = New System.Drawing.Point(257, 22)
            Me.lstSelected.Name = "lstSelected"
            Me.lstSelected.Size = New System.Drawing.Size(224, 210)
            Me.lstSelected.StateImageList = Me.ImageList1
            Me.lstSelected.TabIndex = 22
            Me.lstSelected.UseCompatibleStateImageBehavior = False
            Me.lstSelected.View = System.Windows.Forms.View.Details
            '
            'SelectedItem
            '
            Me.SelectedItem.Text = "Item"
            Me.SelectedItem.Width = 159
            '
            'SelectIdentifier
            '
            Me.SelectIdentifier.Text = "Identifier"
            '
            'ItemSelectingControl
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.lstSelected)
            Me.Controls.Add(Me.lstFound)
            Me.Controls.Add(Me.btnExport)
            Me.Controls.Add(Me.btnClear)
            Me.Controls.Add(Me.btnRemoveAll)
            Me.Controls.Add(Me.btnAddAll)
            Me.Controls.Add(Me.btnRemove)
            Me.Controls.Add(Me.btnAdd)
            Me.Controls.Add(Me.lblSelectedItems)
            Me.Controls.Add(Me.lblFoundItems)
            Me.Name = "ItemSelectingControl"
            Me.Size = New System.Drawing.Size(485, 261)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents btnExport As System.Windows.Forms.Button
        Friend WithEvents btnClear As System.Windows.Forms.Button
        Friend WithEvents lblSelectedItems As System.Windows.Forms.Label
        Friend WithEvents lblFoundItems As System.Windows.Forms.Label
        Friend WithEvents btnRemoveAll As System.Windows.Forms.Button
        Friend WithEvents btnAddAll As System.Windows.Forms.Button
        Friend WithEvents btnRemove As System.Windows.Forms.Button
        Friend WithEvents btnAdd As System.Windows.Forms.Button
        Friend WithEvents lstFound As System.Windows.Forms.ListView
        Friend WithEvents FoundItem As System.Windows.Forms.ColumnHeader
        Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
        Friend WithEvents lstSelected As System.Windows.Forms.ListView
        Friend WithEvents SelectedItem As System.Windows.Forms.ColumnHeader
        Friend WithEvents FoundIdentifier As System.Windows.Forms.ColumnHeader
        Friend WithEvents SelectIdentifier As System.Windows.Forms.ColumnHeader

    End Class
End Namespace