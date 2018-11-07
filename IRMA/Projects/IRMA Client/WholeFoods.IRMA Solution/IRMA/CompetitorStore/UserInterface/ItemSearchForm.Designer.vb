Namespace WholeFoods.IRMA.CompetitorStore.UserInterface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class ItemSearch
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
            Me.gbSearch = New System.Windows.Forms.GroupBox
            Me.ItemSearchControl1 = New WholeFoods.IRMA.ItemChaining.UserInterface.ItemSearchControl
            Me.gbResults = New System.Windows.Forms.GroupBox
            Me.lvResults = New System.Windows.Forms.ListView
            Me.colIdentifier = New System.Windows.Forms.ColumnHeader
            Me.colDescription = New System.Windows.Forms.ColumnHeader
            Me.btnSelectItem = New System.Windows.Forms.Button
            Me.btnCancel = New System.Windows.Forms.Button
            Me.gbSearch.SuspendLayout()
            Me.gbResults.SuspendLayout()
            Me.SuspendLayout()
            '
            'gbSearch
            '
            Me.gbSearch.BackColor = System.Drawing.Color.Transparent
            Me.gbSearch.Controls.Add(Me.ItemSearchControl1)
            Me.gbSearch.Location = New System.Drawing.Point(12, 12)
            Me.gbSearch.Name = "gbSearch"
            Me.gbSearch.Size = New System.Drawing.Size(499, 232)
            Me.gbSearch.TabIndex = 1
            Me.gbSearch.TabStop = False
            Me.gbSearch.Text = "Search"
            '
            'ItemSearchControl1
            '
            Me.ItemSearchControl1.BackColor = System.Drawing.Color.Transparent
            Me.ItemSearchControl1.Location = New System.Drawing.Point(6, 19)
            Me.ItemSearchControl1.Name = "ItemSearchControl1"
            Me.ItemSearchControl1.ShowClearButton = True
            Me.ItemSearchControl1.ShowHFM = False
            Me.ItemSearchControl1.ShowItemCheckBoxes = True
            Me.ItemSearchControl1.ShowSearchButton = True
            Me.ItemSearchControl1.ShowWFM = False
            Me.ItemSearchControl1.Size = New System.Drawing.Size(485, 207)
            Me.ItemSearchControl1.TabIndex = 0
            '
            'gbResults
            '
            Me.gbResults.BackColor = System.Drawing.Color.Transparent
            Me.gbResults.Controls.Add(Me.lvResults)
            Me.gbResults.Location = New System.Drawing.Point(12, 250)
            Me.gbResults.Name = "gbResults"
            Me.gbResults.Size = New System.Drawing.Size(499, 234)
            Me.gbResults.TabIndex = 2
            Me.gbResults.TabStop = False
            Me.gbResults.Text = "Results"
            '
            'lvResults
            '
            Me.lvResults.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colIdentifier, Me.colDescription})
            Me.lvResults.FullRowSelect = True
            Me.lvResults.GridLines = True
            Me.lvResults.Location = New System.Drawing.Point(6, 19)
            Me.lvResults.MultiSelect = False
            Me.lvResults.Name = "lvResults"
            Me.lvResults.ShowGroups = False
            Me.lvResults.Size = New System.Drawing.Size(487, 209)
            Me.lvResults.TabIndex = 0
            Me.lvResults.UseCompatibleStateImageBehavior = False
            Me.lvResults.View = System.Windows.Forms.View.Details
            '
            'colIdentifier
            '
            Me.colIdentifier.Text = "Identifier"
            Me.colIdentifier.Width = 112
            '
            'colDescription
            '
            Me.colDescription.Text = "Description"
            Me.colDescription.Width = 350
            '
            'btnSelectItem
            '
            Me.btnSelectItem.Location = New System.Drawing.Point(436, 490)
            Me.btnSelectItem.Name = "btnSelectItem"
            Me.btnSelectItem.Size = New System.Drawing.Size(75, 23)
            Me.btnSelectItem.TabIndex = 3
            Me.btnSelectItem.Text = "Select Item"
            Me.btnSelectItem.UseVisualStyleBackColor = True
            '
            'btnCancel
            '
            Me.btnCancel.Location = New System.Drawing.Point(12, 490)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 23)
            Me.btnCancel.TabIndex = 4
            Me.btnCancel.Text = "Cancel"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'ItemSearch
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.ClientSize = New System.Drawing.Size(529, 520)
            Me.ControlBox = False
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnSelectItem)
            Me.Controls.Add(Me.gbResults)
            Me.Controls.Add(Me.gbSearch)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.Name = "ItemSearch"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.Text = "Item Search"
            Me.gbSearch.ResumeLayout(False)
            Me.gbResults.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

        Friend WithEvents ItemSearchControl1 As WholeFoods.IRMA.ItemChaining.UserInterface.ItemSearchControl
        Friend WithEvents gbSearch As System.Windows.Forms.GroupBox
        Friend WithEvents gbResults As System.Windows.Forms.GroupBox
        Friend WithEvents btnSelectItem As System.Windows.Forms.Button
        Friend WithEvents lvResults As System.Windows.Forms.ListView
        Friend WithEvents colIdentifier As System.Windows.Forms.ColumnHeader
        Friend WithEvents colDescription As System.Windows.Forms.ColumnHeader
        Friend WithEvents btnCancel As System.Windows.Forms.Button
    End Class
End Namespace