Imports System.Windows.Forms

Public Class ReprintShelfTagsForm

    Private WithEvents _reprintTags As ReprintShelfTags = Nothing

#Region "Event Handlers"

    Private Sub ReprintShelfTagsApplication_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Call OnStart()

        btnStopProcessing.Enabled = False

        Call LoadStores()

        With _reprintTags
            Me.Text = String.Format("{0} [{1}]", .AppVersion, .DBServer)
        End With

    End Sub

    Private Sub ReprintShelfTagsForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Call OnStop()

    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click

        Me.Close()

    End Sub

    Private Sub btnCheckReprintRequests_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCheckReprintRequests.Click

        Call RunProcess(ReprintShelfTags.enumProcessType.Reprint)

    End Sub

    Private Sub btnCheckStoreFTP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheckStoreFTP.Click

        Call RunProcess(ReprintShelfTags.enumProcessType.CheckFTP)

    End Sub

    Private Sub btnStopProcessing_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStopProcessing.Click

        Call _reprintTags.StopProcessing()

    End Sub

    Private Sub btnEmailLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEmailLog.Click

        Call _reprintTags.SendEmail(rtbOutputText.Text)

    End Sub

    Private Sub _reprintTags_UpdateProgressBar(ByVal iteration As Integer, ByVal total As Integer) Handles _reprintTags.UpdateProgressBar

        If Not iteration.Equals(0) Then
            pbrProgress.PerformStep()
        End If

        If Not total.Equals(0) Then
            lblInfo.Text = String.Format("{0} {1} of {2}...", lblInfo.Tag, iteration, total)
        End If

        Me.Refresh()

    End Sub

    Private Sub _reprintTags_UpdateOutputText(ByVal textToAppend As String, ByVal outputTextType As ReprintShelfTags.enumOutputTextType) Handles _reprintTags.UpdateOutputText

        Dim textColor As System.Drawing.Color = Drawing.Color.Black

        If textToAppend.Length.Equals(0) Then
            Exit Sub
        End If

        With rtbOutputText
            .AppendText(textToAppend)

            Select Case outputTextType
                Case ReprintShelfTags.enumOutputTextType.Default
                    'no special format
                    Exit Select

                Case ReprintShelfTags.enumOutputTextType.Information
                    textColor = Drawing.Color.Blue

                Case ReprintShelfTags.enumOutputTextType.Warning
                    textColor = Drawing.Color.Red

                Case ReprintShelfTags.enumOutputTextType.Error
                    textColor = Drawing.Color.Red

            End Select

            If Not textColor.Equals(Drawing.Color.Black) Then
                If textToAppend.Contains("*** <") Then
                    .SelectionStart = .Find(textToAppend, RichTextBoxFinds.Reverse)
                ElseIf .Find("Store: ", RichTextBoxFinds.Reverse) >= 0 Then
                    .SelectionStart = .Find("Store: ", RichTextBoxFinds.Reverse)
                Else
                    .SelectionStart = 0
                End If
                .SelectionLength = .TextLength - .SelectionStart
                .SelectionColor = textColor
                .SelectionLength = 0
            End If
        End With

        Me.Refresh()

    End Sub

    Private Sub btnSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectAll.Click

        With lstStores
            'prevent the control from drawing to eliminate flicker
            .BeginUpdate()

            For i As Integer = .Items.Count To 1 Step -1
                .SetSelected(i - 1, True)
            Next

            'allow the control to draw
            .EndUpdate()
        End With

        Call SetListToolTip()

    End Sub

    Private Sub btnSelectNone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectNone.Click

        lstStores.ClearSelected()

        Call SetListToolTip()

    End Sub

    Private Sub lstStores_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstStores.SelectedIndexChanged

        Call SetListToolTip()

    End Sub

    Private Sub rtbOutputText_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles rtbOutputText.KeyDown

        If e.Control AndAlso e.KeyData = Keys.A Then
            '<Ctrl>+<A> to select all text
            rtbOutputText.SelectAll()
        End If

    End Sub
#End Region

    Private Sub SetListToolTip()

        With lstStores
            ToolTip1.SetToolTip(lstStores, String.Format("{0} of {1} stores selected", .SelectedItems.Count, .Items.Count))
        End With

    End Sub

    Protected Sub OnStart()
        ' Add code here similar to ReprintShelfTagsService.Onstart()
        _reprintTags = New ReprintShelfTags(True)
    End Sub

    Protected Sub OnStop()
        ' Add code here similar to ReprintShelfTagsService.OnStop()
        _reprintTags = Nothing
    End Sub

    Protected Sub LoadStores()

        Try
            With lstStores
                .DataSource = _reprintTags.StoreDataTable()
                .ValueMember = "Store_No"
                .DisplayMember = "Store_Name"
            End With

        Catch ex As Exception
            MsgBox(ex.ToString)

        Finally
            Call btnSelectAll.PerformClick()

        End Try

    End Sub

    Private Sub RunProcess(ByVal processType As ReprintShelfTags.enumProcessType)

        Dim storeList As SortedList

        Select Case processType
            Case ReprintShelfTags.enumProcessType.CheckFTP
                lblInfo.Tag = "Checking connections to remote server"

            Case ReprintShelfTags.enumProcessType.Reprint

                'If Not System.Diagnostics.Debugger.IsAttached Then
                '    MessageBox.Show("Disabled until formal release.", "Reprint Tags", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                '    Exit Sub
                'End If

                If Not MessageBox.Show("Do you really want to manually run a reprint?", "Reprint Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2).Equals(DialogResult.Yes) Then
                    Exit Sub
                End If
                lblInfo.Tag = "Processing shelf tag reprint request"

        End Select

        Try
            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            Call EnableProcessingControls(False)

            lblInfo.Text = String.Concat(lblInfo.Tag, "s...")
            rtbOutputText.Clear()
            Me.Refresh()

            storeList = GetSelectedStores()

            If storeList.Count = 0 Then
                rtbOutputText.Text = "No stores have been selected!"
                Exit Try
            End If

            pbrProgress.Maximum = storeList.Count
            pbrProgress.Step = 1

            Select Case processType
                Case ReprintShelfTags.enumProcessType.CheckFTP
                    Call _reprintTags.CheckRemoteServers(storeList)

                Case ReprintShelfTags.enumProcessType.Reprint
                    Call _reprintTags.ProcessReprintRequests(storeList)

            End Select

        Catch ex As Exception
            Call _reprintTags.HandleError(ex)
            'write to event log 
            Diagnostics.EventLog.WriteEntry(Me.Name, ex.ToString, EventLogEntryType.Error)

        Finally
            If storeList IsNot Nothing AndAlso storeList.Count <> 0 Then
                'update the processing end time
                rtbOutputText.Find(String.Format("End Time: {0}", String.Empty))
                rtbOutputText.SelectedText = String.Format("End Time: {0}", Now.ToString("yyyy-MM-dd HH:mm:ss"))
                lblInfo.Text = String.Concat("Finished ", lblInfo.Tag.ToString.ToLower, "s")
            End If
            pbrProgress.Value = 0
            Call EnableProcessingControls(True)
            Windows.Forms.Cursor.Current = Cursors.Default
            Me.Refresh()

        End Try

        If _reprintTags.ErrorsOccurred Then
            MessageBox.Show("Errors occurred during processing.  See output log for more details.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub

    Private Sub EnableProcessingControls(Optional ByVal ShowEnabled As Boolean = True)

        Try
            btnCheckReprintRequests.Enabled = (ShowEnabled)
            btnCheckStoreFTP.Enabled = (ShowEnabled)
            btnExit.Enabled = (ShowEnabled)
            lstStores.Enabled = (ShowEnabled)
            btnSelectAll.Enabled = (ShowEnabled)
            btnSelectNone.Enabled = (ShowEnabled)

            btnStopProcessing.Enabled = (Not ShowEnabled)

        Catch ex As Exception
            Throw ex

        End Try

    End Sub

    Private Function GetSelectedStores() As SortedList

        Dim storeList As New SortedList

        Try
            For i As Integer = 1 To lstStores.SelectedItems.Count

                With CType(lstStores.SelectedItems(i - 1), DataRowView)
                    storeList.Add(.Item("Store_No"), .Row)
                End With

            Next

        Catch ex As Exception
            Throw ex

        End Try

        Return storeList

    End Function
End Class