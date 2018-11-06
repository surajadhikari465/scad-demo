Option Strict Off
Option Explicit On

Public Class selInventoryAdjustmentCode

    Private Sub selInventoryAdjustmentCode_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        CenterForm(Me)
        LoadInventoryCodes()

    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        ' Pass False if DataChange requested by user is "Add"
        ManageInventoryAdjustmentCode(0)
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        'Pass True if DataChange requested by user is "Edit"
        ManageInventoryAdjustmentCode(1)
    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub LoadInventoryCodes()
        cmbInvAdjCode.Items.Clear()

        Try
            ' Load all Inventory Adjustment Codes into combo box
            gRSRecordset = SQLOpenRecordSet("EXEC GetInventoryAdjustmentCodeList NULL, NULL", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            Do While Not gRSRecordset.EOF
                cmbInvAdjCode.Items.Add(New VB6.ListBoxItem(gRSRecordset.Fields("LongDescription").Value, gRSRecordset.Fields("InventoryAdjustmentCode_ID").Value))
                gRSRecordset.MoveNext()
            Loop

            cmbInvAdjCode.SelectedIndex = -1

        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
        End Try
    End Sub

    Private Sub ManageInventoryAdjustmentCode(ByRef blnIsEdit As Boolean)

        Dim iInventoryAdjustmentCodeID As Integer

        ' Check if an Inventory Adjustment code/desc was selected for Edit Mode only
        If cmbInvAdjCode.SelectedIndex = -1 And blnIsEdit Then
            MsgBox("No Inventory Adjustment Description was selected!")
            Exit Sub
        End If

        ' Get Inventory Adjustment Code ID
        If cmbInvAdjCode.SelectedIndex = -1 Then
            iInventoryAdjustmentCodeID = 0
        Else
            iInventoryAdjustmentCodeID = VB6.GetItemData(cmbInvAdjCode, cmbInvAdjCode.SelectedIndex).ToString
        End If

        ' Load Child Form
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim fInventoryAdjustmentCode As New InventoryAdjustmentCode(iInventoryAdjustmentCodeID, blnIsEdit)

        If blnIsEdit Then
            fInventoryAdjustmentCode.Text = "Edit Inventory Adjustment Information"
        Else
            fInventoryAdjustmentCode.Text = "Create a new Inventory Adjustment Code"
        End If
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        fInventoryAdjustmentCode.ShowDialog()
        fInventoryAdjustmentCode.Close()
        fInventoryAdjustmentCode.Dispose()
        fInventoryAdjustmentCode = Nothing
        LoadInventoryCodes()
    End Sub

End Class