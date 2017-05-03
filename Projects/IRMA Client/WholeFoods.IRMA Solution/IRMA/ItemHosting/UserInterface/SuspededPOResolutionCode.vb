Option Strict Off
Option Explicit On

Public Class SuspededPOResolutionCode

    Private Sub selInventoryAdjustmentCode_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        CenterForm(Me)
        LoadResolutionCodes()

    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        ' Pass False if DataChange requested by user is "Add"
        ManageResolutionCode(0)
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        'Pass True if DataChange requested by user is "Edit"
        ManageResolutionCode(1)
    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub LoadResolutionCodes()
        cmbResolutionCode.Items.Clear()

        Try
            ' Load all Resolution Codes into combo box
            gRSRecordset = SQLOpenRecordSet("EXEC GetResolutionCodeList 0", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            Do While Not gRSRecordset.EOF
                cmbResolutionCode.Items.Add(New VB6.ListBoxItem(gRSRecordset.Fields("Description").Value, gRSRecordset.Fields("ResolutionCodeID").Value))
                gRSRecordset.MoveNext()
            Loop

            cmbResolutionCode.SelectedIndex = -1

        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
        End Try
    End Sub

    Private Sub ManageResolutionCode(ByRef blnIsEdit As Boolean)

        Dim iResolutionCodeID As Integer

        ' Check if an Resolution Code was selected for Edit Mode only
        If cmbResolutionCode.SelectedIndex = -1 And blnIsEdit Then
            MsgBox("No Resolution code was selected!")
            Exit Sub
        End If

        ' Get Inventory Adjustment Code ID
        If cmbResolutionCode.SelectedIndex = -1 Then
            iResolutionCodeID = 0
        Else
            iResolutionCodeID = VB6.GetItemData(cmbResolutionCode, cmbResolutionCode.SelectedIndex).ToString
        End If

        ' Load Child Form
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

        Dim frmResolutionCode As New EditResolutionCode(iResolutionCodeID, blnIsEdit)

        If blnIsEdit Then
            frmResolutionCode.Text = "Edit Resolution Code"
        Else
            frmResolutionCode.Text = "Create a new Resolution Code"
        End If
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default

        frmResolutionCode.ShowDialog()
        frmResolutionCode.Close()

        frmResolutionCode.Dispose()
        frmResolutionCode = Nothing

        LoadResolutionCodes()
    End Sub

End Class