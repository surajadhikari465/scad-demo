Option Strict Off
Option Explicit On
Imports Infragistics.Win.UltraWinGrid
Imports Infragistics.Win.UltraWinDataSource
Imports log4net
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Friend Class frmReceivingDocumentSetting

    Inherits System.Windows.Forms.Form

    Private changedList As New Dictionary(Of Int32, Boolean)
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private Sub frmReceivingDocumentSetting_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmReceivingDocumentSetting Entry")
        Me.MinimumSize = Me.Size

        Dim blnEnableEdit As Boolean = False
        If gbSuperUser Or gbVendorAdministrator Then blnEnableEdit = True

        Me.btnSetAllDocumentSetting.Enabled = blnEnableEdit
        Me.btnSaveAndExit.Enabled = blnEnableEdit


        Dim dt As New DataTable
        dt = DSDVendorDAO.GetDSDVendorAllStore(glVendorID)
        DataGridView1.DataSource = dt
        
        CenterForm(Me)



        Me.Show()

        logger.Debug("frmReceivingDocumentSetting Exit")
    End Sub

    Private Sub btnSaveAndExit_Click(sender As System.Object, e As System.EventArgs) Handles btnSaveAndExit.Click
        updateDSDVendorSetup()
    End Sub
    Private Sub updateDSDVendorSetup()
        Dim pair As KeyValuePair(Of Int32, Boolean)
        For Each pair In changedList
            DSDVendorDAO.updateDSDVendorSetup(pair.Key, glVendorID, DateTime.Now, pair.Value)
        Next
        logger.Info("save DSD vendor store set up")
        changedList.Clear()
        Me.Close()
    End Sub

    Private Sub btnSetAllDocumentSetting_Click(sender As System.Object, e As System.EventArgs) Handles btnSetAllDocumentSetting.Click
        For Each row As DataGridViewRow In DataGridView1.Rows
            row.Cells("IsReceivingDocument").Value = True
            If changedList.ContainsKey(CInt(row.Cells("Store_Number").Value)) Then
                changedList(CInt(row.Cells("Store_Number").Value)) = True
            Else
                changedList.Add(CInt(row.Cells("Store_Number").Value), True)
            End If
        Next
    End Sub

    Private Sub DataGridView1_CurrentCellDirtyStateChanged( _
    ByVal sender As Object, ByVal e As EventArgs) _
    Handles DataGridView1.CurrentCellDirtyStateChanged

        If DataGridView1.IsCurrentCellDirty Then
            DataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub

    Public Sub DataGridView1_CellContentClick(ByVal sender As Object, _
    ByVal e As DataGridViewCellEventArgs) _
        Handles DataGridView1.CellContentClick
        If e.RowIndex = -1 Then Exit Sub

        If changedList.ContainsKey(CInt(DataGridView1.Rows(e.RowIndex).Cells("Store_Number").Value)) Then
            changedList(CInt(DataGridView1.Rows(e.RowIndex).Cells("Store_Number").Value)) = DataGridView1.Rows(e.RowIndex).Cells("IsReceivingDocument").Value
        Else
            changedList.Add(CInt(DataGridView1.Rows(e.RowIndex).Cells("Store_Number").Value), DataGridView1.Rows(e.RowIndex).Cells("IsReceivingDocument").Value)
        End If

        logger.Info("update each receiving document checkbox change")

    End Sub

    Private Sub btnExitWithoutSaving_Click(sender As System.Object, e As System.EventArgs) Handles btnExitWithoutSaving.Click
        If changedList.Count > 0 Then
            If MessageBox.Show("Are you sure you want to exit and lose your changes?", _
                               "Exit and lose Changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = MsgBoxResult.Yes Then
                Me.Close()
            Else
            End If
        Else
            Me.Close()
        End If
        logger.Info("exit vendor set up form")
    End Sub

    Private Sub deSelectAllStores_Click(sender As System.Object, e As System.EventArgs) Handles deSelectAllStores.Click
        For Each row As DataGridViewRow In DataGridView1.Rows
            row.Cells("IsReceivingDocument").Value = False
            If changedList.ContainsKey(CInt(row.Cells("Store_Number").Value)) Then
                changedList(CInt(row.Cells("Store_Number").Value)) = False
            Else
                changedList.Add(CInt(row.Cells("Store_Number").Value), False)
            End If
        Next
        logger.Info("DeSelect all stores for vendor set up")
    End Sub

    Private Sub frmReceivingDocumenteSettings_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
         logger.Info("close DSD vendor set up form")
    End Sub


End Class