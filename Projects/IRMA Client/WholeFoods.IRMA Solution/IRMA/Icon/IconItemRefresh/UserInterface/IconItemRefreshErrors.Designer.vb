﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class IconItemRefreshErrors
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
        Me.ErrorsDataGridView = New System.Windows.Forms.DataGridView()
        CType(Me.ErrorsDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ErrorsDataGridView
        '
        Me.ErrorsDataGridView.AllowUserToAddRows = False
        Me.ErrorsDataGridView.AllowUserToDeleteRows = False
        Me.ErrorsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.ErrorsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ErrorsDataGridView.Location = New System.Drawing.Point(0, 0)
        Me.ErrorsDataGridView.Name = "ErrorsDataGridView"
        Me.ErrorsDataGridView.ReadOnly = True
        Me.ErrorsDataGridView.Size = New System.Drawing.Size(739, 310)
        Me.ErrorsDataGridView.TabIndex = 0
        '
        'IconItemRefreshErrors
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(739, 310)
        Me.Controls.Add(Me.ErrorsDataGridView)
        Me.Name = "IconItemRefreshErrors"
        Me.Text = "Infor Item Refresh Errors"
        CType(Me.ErrorsDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ErrorsDataGridView As System.Windows.Forms.DataGridView
End Class
