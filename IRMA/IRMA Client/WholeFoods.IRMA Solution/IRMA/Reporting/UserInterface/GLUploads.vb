Option Strict Off
Option Explicit On

Imports System.IO
Imports System.Text
Imports WholeFoods.IRMA.Ordering.DataAccess

Friend Class frmGLUploads
    Inherits System.Windows.Forms.Form

    Private IsInitializing As Boolean
    Private HasValidationErrors As Boolean

    Private ReadOnly Property ExportFileName(ByVal Commit As Boolean) As String
        Get

            Dim fName As String = textOutputLocation.Text & "\"
            Dim prOnly As String = IIf(Commit, String.Empty, "PRINT_ONLY.")
            Dim typeName As String = IIf(optUpload(0).Checked, "GL-DISTRIBUTION.", "GL-TRANSFERS.")
            Dim facility As String = VB6.GetItemData(cmbStore, cmbStore.SelectedIndex).ToString & "."
            Dim dttmStr As String = Date.Today.Month.ToString & Date.Today.Day.ToString & Date.Today.Year.ToString
            Dim fExt As String = ".csv"

            Return fName & prOnly & typeName & facility & dttmStr & fExt

        End Get
    End Property

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub cmdExport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExport.Click

        If Not ValidateForm(False, False) Then
            Exit Sub
        End If

        If MessageBox.Show("Exporting GL transactions will mark them as exported." & vbCrLf & "This action cannot be undone." & vbCrLf & vbCrLf & "Are you sure you want to do this?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then

            DoExport(True)

        End If

    End Sub

    Private Sub frmGLUploads_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        CenterForm(Me)

        dtpStartDate.Value = SystemDateTime()
        dtpEndDate.Value = SystemDateTime()

        LoadDistMfg(cmbStore)

        'If cmbStore.Items.Count > 0 Then
        '    cmbStore.SelectedIndex = 0 'Make sure a store is picked - cheap way to avoid edit to require it
        'End If

        cmbStore.Items.Insert(0, "--SELECT--")
        cmbStore.SelectedIndex = 0

        SetActive(lblDates, False)
        SetActive(lblDash, False)
        SetActive(dtpStartDate, False)
        SetActive(dtpEndDate, False)

    End Sub

    Private Sub optDateRange_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optDateRange.CheckedChanged
        If Me.IsInitializing = True Then Exit Sub

        If sender.Checked Then
            Dim Index As Short = optDateRange.GetIndex(sender)

            If Index = 0 Then
                SetActive(lblDates, False)
                SetActive(lblDash, False)
                SetActive(dtpStartDate, False)
                SetActive(dtpEndDate, False)
            Else
                SetActive(lblDates, True)
                SetActive(lblDash, True)
                SetActive(dtpStartDate, True)
                SetActive(dtpEndDate, True)
            End If

        End If

    End Sub

    Private Sub optUpload_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optUpload.CheckedChanged
        If Me.IsInitializing = True Then Exit Sub

        If sender.Checked Then
            Dim Index As Short = optUpload.GetIndex(sender)
            If Index = 0 Then
                LoadDistMfg(cmbStore)
                cmbStore.Items.Insert(0, "--SELECT--")
                cmbStore.SelectedIndex = 0
                'SetActive(lblLabel(1), True)
                'SetActive(cmbStore, True)
                SetActive(optDateRange(0), True)
                SetActive(optDateRange(1), True)
                optDateRange(0).Checked = True
            Else
                LoadStore(cmbStore, True)
                cmbStore.Items.Insert(0, "--ALL--")
                cmbStore.SelectedIndex = 0
                'SetActive(lblLabel(1), False)
                'SetActive(cmbStore, False)
                'SetActive(optDateRange(0), False)
                'SetActive(optDateRange(1), False)
                optDateRange(1).Checked = True
            End If
        End If
    End Sub

    Private Sub cmbStore_KeyPress(ByVal eventSender As Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbStore.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        If KeyAscii = 8 Then
            cmbStore.SelectedIndex = 0
        End If

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub cmbStore_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore.SelectedIndexChanged

        formErrorProvider.SetError(cmbStore, "")

    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click

        If ValidateForm(True, True) Then

            RefreshDataSource()

        End If

    End Sub

    Private Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrint.Click

        If Not ValidateForm(False, True) Then
            Exit Sub
        End If

        DoExport(False)

    End Sub

    Private Sub cmdSelectDirectory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelectDirectory.Click

        textOutputLocation.Text = String.Empty

        cbdDirectoryChoose.RootFolder = Environment.SpecialFolder.Desktop

        cbdDirectoryChoose.ShowDialog()

        textOutputLocation.Text = cbdDirectoryChoose.SelectedPath

        If textOutputLocation.Text.Length > 0 And Directory.Exists(textOutputLocation.Text) Then
            formErrorProvider.SetError(cmdSelectDirectory, "")
        End If

    End Sub

    Private Function ValidateForm(ByVal SearchOnly As Boolean, ByVal Print As Boolean) As Boolean

        Dim valid As Boolean = True

        formErrorProvider.SetError(cmbStore, "")
        formErrorProvider.SetError(cmdSelectDirectory, "")

        If optUpload(0).Checked And cmbStore.SelectedIndex = 0 Then
            formErrorProvider.SetError(cmbStore, "You must make a facility selection.")
            valid = False
        End If

        If Not SearchOnly And (textOutputLocation.Text.Length = 0 Or Not Directory.Exists(textOutputLocation.Text)) Then
            formErrorProvider.SetError(cmdSelectDirectory, "Use the Browse button to choose an output location.")
            valid = False
        End If

        If optDateRange(1).Checked Then
            If dtpEndDate.Value < dtpStartDate.Value Then
                MsgBox(ResourcesIRMA.GetString("EndDateGreaterEqual"), MsgBoxStyle.Exclamation, Me.Text)
                dtpEndDate.Focus()
                valid = False
            End If
        End If

        If Not Print And HasValidationErrors Then
            valid = False
        End If

        Return valid

    End Function

    Private Sub DoExport(ByVal Commit As Boolean)

        ' Call ValidateForm BEFORE calling DoExport

        Me.Cursor = Cursors.WaitCursor

        Dim sOut As String
        Dim dt As DataTable = Nothing
        Dim dr As DataRow

        Try

            dt = GLDAO.GetGLTransactions(GetExportParameters)

            If dt.Rows.Count > 0 Then

                FileOpen(1, Trim(ExportFileName(Commit)), OpenMode.Output)

                'Output headers as the first row
                PrintLine(1, "Unit,Ledger,Account,Dept ID,Product,Project,Affiliate,Currency,Amount,RateType,Description,Alt Account,Entry Event,N/R,Rate,Base Amount")

                For Each dr In dt.Rows()

                    sOut = """" & IIf(dr.Item("TransferUnit") > 0, dr.Item("TransferUnit"), "") & """," & """ACTUALS""," & """" & IIf(dr.Item("Account") > 0, dr.Item("Account"), "") & """," & """" & IIf(dr.Item("DeptID") > 0, dr.Item("DeptID"), "") & """," & """" & IIf(dr.Item("Product") > 0, dr.Item("Product"), "") & """," & """""," & """""," & """USD""," & dr.Item("Amount") & "," & """CRRNT""," & """PO#" & dr.Item("Description") & """," & """""," & """""," & """""," & """" & ""","

                    PrintLine(1, sOut)

                Next

                FileClose(1)

                ' Only Distribution orders will be marked as uploaded after the Commit button is clicked. 
                ' Transfer orders remain unmarked even afer the Commit button is clicked.
                If Commit And Me._optUpload_0.Checked Then
                    GLDAO.CommitGLTransactions(GetExportParameters())
                    dgTransactions.DataSource = Nothing
                    dgTransactions.Rows.Clear()
                End If

                MsgBox("Export to " & ExportFileName(Commit) & " completed", MsgBoxStyle.Information, Me.Text)

            Else

                MsgBox("No data to export.", MsgBoxStyle.Critical, Me.Text)

            End If

        Catch ex As Exception

            MsgBox("File processing failed: " & Err.Number & " - " & Err.Description, MsgBoxStyle.Critical, Me.Text)

        Finally

            FileClose(1)

            If Not dt Is Nothing Then dt.Dispose()

            Me.Cursor = Cursors.Default

        End Try

    End Sub

    Private Function GetExportParameters() As GLBO

        Dim _glBO As New GLBO

        _glBO.TransactionType = IIf(optUpload(0).Checked, enumOrderType.Distribution, enumOrderType.Transfer)

        If optUpload(0).Checked Then
            _glBO.StoreNo = VB6.GetItemData(cmbStore, cmbStore.SelectedIndex)
        Else
            _glBO.StoreNo = -1
        End If

        If optDateRange(1).Checked Then
            If cmbStore.SelectedIndex = 0 Then
                _glBO.StoreNo = -1
            Else
                _glBO.StoreNo = VB6.GetItemData(cmbStore, cmbStore.SelectedIndex)
            End If
            _glBO.StartDate = dtpStartDate.Value.ToShortDateString & " 00:00:00"
            _glBO.EndDate = dtpEndDate.Value.ToShortDateString & " 23:59:59"
        Else
            _glBO.StartDate = String.Empty
            _glBO.EndDate = String.Empty
        End If

        _glBO.CurrentDate = String.Empty

        Return _glBO

    End Function
   
    Private Sub RefreshDataSource()

        Try

            Me.Cursor = Cursors.WaitCursor

            dgTransactions.DataSource = GLDAO.GetGLTransactions(GetExportParameters)

            If dgTransactions.RowCount > 0 Then
                ValidateExportData()
                cmdPrint.Enabled = True
                grpExport.Enabled = True
            Else
                cmdPrint.Enabled = False
                grpExport.Enabled = False
            End If

        Finally

            Me.Cursor = Cursors.Default

        End Try

    End Sub

    Private Sub ValidateExportData()

        Dim sb As StringBuilder
        Dim errCount As Integer = 0

        formErrorProvider.SetError(dgTransactions, "")
        dgTransactions.DefaultCellStyle.BackColor = Color.White

        For Each row As DataGridViewRow In dgTransactions.Rows

            sb = New StringBuilder

            If row.Cells("Account").Value.ToString.Equals("0") Then
                sb.AppendLine("SubTeam " & row.Cells("SubTeam").Value.ToString & " is missing Account information.")
                row.DefaultCellStyle.BackColor = Color.MistyRose
                errCount = errCount + 1
            End If

            ' not entirely sure we're doing this - some instances are valid where the DeptID and Product are missing so until rules
            ' have been established, this is commented out.

            'If row.Cells("DeptID").Value.ToString.Equals("0") Then
            '    sb.AppendLine("Subteam " & row.Cells("SubTeam_No").Value.ToString & " is missing Store SubTeam relationship information.")
            '    errCount = errCount + 1
            'End If

            'If row.Cells("Product").Value.ToString.Equals("0") Then
            '    sb.AppendLine("Subteam " & row.Cells("SubTeam_No").Value.ToString & " is missing Store SubTeam relationship information.")
            '    errCount = errCount + 1
            'End If

            row.ErrorText = sb.ToString

        Next

        If errCount > 0 Then
            cmdExport.Enabled = False
            HasValidationErrors = True
            formErrorProvider.SetError(dgTransactions, errCount & " errors exist that must be corrected before the export && commit can take place.")
        Else
            cmdExport.Enabled = True
            HasValidationErrors = False
            formErrorProvider.SetError(dgTransactions, "")
        End If

    End Sub

    Private Sub dgTransactions_Sorted(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgTransactions.Sorted
        ValidateExportData()
    End Sub

End Class