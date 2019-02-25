Imports System.Text
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Friend Class frmReceivingLog
    Inherits System.Windows.Forms.Form

    Private IsInitializing As Boolean

#Region "Helper Methods"

    Public Sub LoadReceivingStores(ByRef cboCombo As System.Windows.Forms.ComboBox)

        cboCombo.Items.Clear()
        Try
            gRSRecordset = SQLOpenRecordSet("EXEC GetReceivingStores", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            Do While Not gRSRecordset.EOF
                cboCombo.Items.Add(New VB6.ListBoxItem(gRSRecordset.Fields("Store_Name").Value.ToString(), CInt(gRSRecordset.Fields("Store_No").Value)))
                gRSRecordset.MoveNext()
            Loop

        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
        End Try

    End Sub

    Private Sub RefreshData()

        'If user entered today
        If DateDiff(DateInterval.Day, Today, CDate(cmbFromDate.Value)) = 0 Then
            Try
                gRSRecordset = SQLOpenRecordSet("EXEC CheckReceiveLog " & VB6.GetItemData(cmbStore, cmbStore.SelectedIndex), DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough + DAO.RecordsetOptionEnum.dbForwardOnly)

                Dim recDate As Date = CDate(Format(Convert.ToDateTime(gRSRecordset.Fields("LastRecvLogDate").Value), "D"))

                'If receiving has not been done today, enable the close button
                SetActive(cmdClose, (DateDiff(DateInterval.Day, recDate, Today) > 0) And (gbDistributor Or gbAccountant))

                If DateDiff(DateInterval.Day, recDate, Today) = 0 Then
                    txtDateTime.Text = VB6.Format(gRSRecordset.Fields("LastRecvLogDate").Value, gsUG_DateMask + " Hh:Nn:Ss")
                Else
                    txtDateTime.Text = ""
                End If

            Finally
                If gRSRecordset IsNot Nothing Then
                    gRSRecordset.Close()
                    gRSRecordset = Nothing
                End If
            End Try

        Else
            Try
                SetActive(cmdClose, False)

                'Show the receiving log date and time for the date entered and for affected orders
                gRSRecordset = SQLOpenRecordSet("EXEC GetReceiveLogDate " & VB6.GetItemData(cmbStore, cmbStore.SelectedIndex) & ", '" & VB6.Format(Me.cmbFromDate.Value.ToString(), "YYYY-MM-DD") & "'", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough + DAO.RecordsetOptionEnum.dbForwardOnly)

                If Not gRSRecordset.EOF Then
                    txtDateTime.Text = VB6.Format(gRSRecordset.Fields("RecvLogDate").Value, gsUG_DateMask + " Hh:Nn:Ss")
                Else
                    txtDateTime.Text = VB6.Format(Me.cmbFromDate.Value.ToString, gsUG_DateMask + " Hh:Nn:Ss")
                End If
            Finally
                If gRSRecordset IsNot Nothing Then
                    gRSRecordset.Close()
                    gRSRecordset = Nothing
                End If
            End Try

        End If

        'Bug 12369: FL 3.5.9 PRD - Daily receiving Log Subteam option is grayed out
        If cmbStore.Text.Trim <> "" Then
            LoadStoreSubteam(cmbSubTeam, VB6.GetItemData(cmbStore, cmbStore.SelectedIndex))
            SetActive(cmbSubTeam, True)
        Else
            cmbSubTeam.Items.Clear()
            SetActive(cmbSubTeam, False)
        End If
    End Sub

#End Region

    Private Sub frmReceivingLog_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        CenterForm(Me)

        SetActive(txtDateTime, False)

        LoadReceivingStores(cmbStore)

        If glStore_Limit > 0 Then
            SetActive(cmbStore, False)
            SetCombo(cmbStore, glStore_Limit)
        Else
            cmbStore.SelectedIndex = -1
            SetActive(cmbStore, True)
        End If

        cmdClose.Enabled = gbDistributor
    End Sub

    Private Sub cmdClose_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdClose.Click

        Dim storeNumber As Integer = VB6.GetItemData(cmbStore, cmbStore.SelectedIndex)
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim isReceivingInProgress As Boolean = False

        ' Execute the SP
        isReceivingInProgress = CType(factory.ExecuteScalar((String.Format("exec dbo.CheckReceivingInProgress {0},{1}", storeNumber, giUserID))), Boolean)

        If (isReceivingInProgress) Then
            If MessageBox.Show("Close receiving is in progress by other user. Do you still want to proceed and close receiving for " & cmbStore.Text.TrimEnd & "?", "Close Receiving", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then Exit Sub
        Else
            If MessageBox.Show("Are you sure you want to close receiving for " & cmbStore.Text & "?", "Close Receiving", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then Exit Sub
        End If

        On Error GoTo me_err


        SQLExecute2("EXEC CloseReceiving " & storeNumber & ", " & CStr(giUserID), DAO.RecordsetOptionEnum.dbSQLPassThrough, True)

        RefreshData()

        cmdReport.PerformClick()

        Exit Sub

me_err:
        MsgBox("Close failed: " & Err.Description, MsgBoxStyle.Critical, Me.Text)

    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        Me.Close()

    End Sub

    Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click

        Windows.Forms.Cursor.Current = Cursors.WaitCursor

        Dim reportUrlBuilder As StringBuilder

        '' Satisfy TFS#7320 and UN-comment-out this to restore functionality to original design
        '-- Data validation
        If cmbFromDate.Value.ToString = String.Empty Then
            MsgBox("Begin date must be completed.", MsgBoxStyle.Exclamation, Me.Text)
            cmbFromDate.Focus()
            Exit Sub
        End If

        '' Satisfy TFS#7320 and UN-comment-out this to restore functionality to original design
        '-- Data validation
        If cmbToDate.Value.ToString = String.Empty Then
            MsgBox("End date must be completed.", MsgBoxStyle.Exclamation, Me.Text)
            cmbToDate.Focus()
            Exit Sub
        End If

        '' Satisfy TFS#7320 - add new validation - the start date is <= the end date
        If CDate(cmbFromDate.Value) > CDate(cmbToDate.Text) Then
            MsgBox("End Date must be greater than or same as Begin Date", MsgBoxStyle.Exclamation, Me.Text)
            cmbFromDate.Focus()
            Exit Sub
        End If

        reportUrlBuilder = New StringBuilder()

        reportUrlBuilder.AppendFormat("ReceivingLog&rs:Command=Render&rc:Parameters=False&Begin={0}&End={1}",
                    VB6.Format(cmbFromDate.Value.ToString, "YYYY-MM-DD"),
                    VB6.Format(cmbToDate.Value.ToString, "YYYY-MM-DD"))

        If (cmbStore.SelectedIndex > -1) Then
            reportUrlBuilder.AppendFormat("&Store_No={0}&Store_Name={1}", VB6.GetItemData(cmbStore, cmbStore.SelectedIndex), cmbStore.Text)
        End If

        If (cmbSubTeam.SelectedIndex > -1) Then
            reportUrlBuilder.AppendFormat("&SubTeam_No={0}&SubTeam_Name={1}", VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex), cmbSubTeam.Text)
        End If

        ReportingServicesReport(reportUrlBuilder.ToString())

        Windows.Forms.Cursor.Current = Cursors.Default

    End Sub

#Region "Combo Box Event Handlers"

    Private Sub cmbStore_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbStore.KeyPress
        Dim KeyAscii As Short = CShort(Asc(e.KeyChar))

        If KeyAscii = 8 Then
            cmbStore.SelectedIndex = -1
        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If
    End Sub

    Private Sub cmbStore_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbStore.SelectedIndexChanged
        If Me.IsInitializing = True Then Exit Sub

        If (cmbStore.SelectedIndex > -1) Then
            RefreshData()
        End If

    End Sub

    Private Sub cmbSubTeam_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbSubTeam.KeyPress
        Dim KeyAscii As Short = CShort(Asc(eventArgs.KeyChar))

        If KeyAscii = 8 Then cmbSubTeam.SelectedIndex = -1

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

#End Region

    Private Sub cmbFromDate_AfterCloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbFromDate.AfterCloseUp
        Me.cmbToDate.Value = cmbFromDate.Value
        Me.cmbToDate.Enabled = True
    End Sub

    Private Sub cmbFromDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbFromDate.ValueChanged
        enableClose()
    End Sub

    Private Sub cmbToDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbToDate.ValueChanged
        enableClose()
    End Sub

    Private Sub enableClose()
        If Me.cmbFromDate.Value = SystemDateTime(True) And Me.cmbFromDate.Value = Me.cmbToDate.Value Then
            Me.cmdClose.Enabled = True
        Else
            Me.cmdClose.Enabled = False
        End If
    End Sub
End Class