Imports WholeFoods.IRMA.Administration.Common.DataAccess
Imports WholeFoods.IRMA.FileMonitor
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.POSPush.DataAccess

Public Class Form_UnprocessedPushFiles

    Private Sub Form_UnprocessedPushFiles_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        RefreshForm()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnResend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResend.Click
        'loop through checked and add them to xml doc to be ftp'd
        Dim storeUpdatesPOS As Hashtable = New Hashtable
        Dim storeUpdatesScale As Hashtable = New Hashtable
        Dim stBusinessUnit As String
        Dim stFileName As String
        Dim stWriter As String
        Dim currentStore As StoreUpdatesBO = Nothing
        Dim bSendFile As Boolean = False
        Dim posJobStatus As JobStatusBO = JobStatusDAO.GetJobStatus("POSPushJob")
        Dim scaleJobStatus As JobStatusBO = JobStatusDAO.GetJobStatus("ScalePushJob")

        If (posJobStatus.Status <> WholeFoods.IRMA.Replenishment.Jobs.DBJobStatus.Complete Or scaleJobStatus.Status <> WholeFoods.IRMA.Replenishment.Jobs.DBJobStatus.Complete) Then
            MsgBox("There is a push currently running or in failed status. Please try again.")
        End If

        Me.Cursor = Cursors.WaitCursor

        storeUpdatesPOS = StorePOSConfigDAO.GetStoreConfigurations(Constants.FileWriterType_POS)
        storeUpdatesScale = StorePOSConfigDAO.GetStoreConfigurations(Constants.FileWriterType_SCALE)

        Dim dsXML As DataSet = TransferWriterFiles.CreatePOSPushXMLTable("POSPushEnvelope", "BizXML")

        For iCount As Int16 = 0 To dgvFiles.RowCount - 1
            If dgvFiles.Rows(iCount).Cells(2).Value = True Then
                Dim storePOSEnum As IEnumerator = storeUpdatesPOS.Values.GetEnumerator()
                Dim storeScaleEnum As IEnumerator = storeUpdatesScale.Values.GetEnumerator()

                stFileName = dgvFiles.Rows(iCount).Cells(0).Value
                stBusinessUnit = stFileName.Substring(2, 5)
                stWriter = stFileName.Substring(7, Len(stFileName) - 7)

                If stWriter = Constants.FileWriterType_POS Then
                    'loop POS Hash
                    While (storePOSEnum.MoveNext)
                        currentStore = (CType(storePOSEnum.Current, StoreUpdatesBO))
                        If currentStore.FTPInfo.BusinessUnitID.ToString = stBusinessUnit Then
                            bSendFile = True
                            Exit While
                        End If
                    End While
                Else
                    'loop Scale Hash
                    While (storeScaleEnum.MoveNext)
                        currentStore = (CType(storeScaleEnum.Current, StoreUpdatesBO))
                        If currentStore.FTPInfo.BusinessUnitID.ToString = stBusinessUnit Then
                            bSendFile = True
                            Exit While
                        End If
                    End While
                End If

                ' Create row in data table for this file entry 
                TransferWriterFiles.AddRowPOSPushXMLTable(currentStore, dsXML, stFileName)
            End If
        Next

        If bSendFile Then
            Try
                If TransferWriterFiles.SendXMLtoBizTalk(dsXML, currentStore.BatchFilePath) Then
                    'success message
                    MsgBox("XML File successfully generated and transfered.")
                End If
            Catch ex As Exception
                MsgBox(Err.Description, MsgBoxStyle.Critical, Me.Text)
            End Try
        End If

        Me.Cursor = Cursors.Default
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        RefreshForm()
    End Sub

    Private Sub btnSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectAll.Click
        If dgvFiles.RowCount > 0 Then
            Dim iCount As Int16
            For iCount = 0 To dgvFiles.RowCount - 1
                dgvFiles.Rows(iCount).Cells(2).Value = True
            Next
        End If
    End Sub

    Private Sub RefreshForm()
        Dim stRegion As String
        Dim fmPushFiles As New FileMonitor
        Dim posJobStatus As JobStatusBO = JobStatusDAO.GetJobStatus("POSPushJob")
        Dim scaleJobStatus As JobStatusBO = JobStatusDAO.GetJobStatus("ScalePushJob")

        Try
            stRegion = RegionDAO.GetRegionList.Rows(0).Item("RegionCode")
            dgvFiles.DataSource = FileMonitor.GetUnprocessedPushFiles(stRegion)

            dgvFiles.Columns(0).ReadOnly = True
            dgvFiles.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

            dgvFiles.Columns(1).ReadOnly = True
            dgvFiles.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

            dgvFiles.Sort(dgvFiles.Columns(1), System.ComponentModel.ListSortDirection.Ascending)
        Catch ex As Exception
            MsgBox(Err.Description, MsgBoxStyle.Critical, Me.Text)
        End Try

        If (posJobStatus.Status <> WholeFoods.IRMA.Replenishment.Jobs.DBJobStatus.Complete Or
                scaleJobStatus.Status <> WholeFoods.IRMA.Replenishment.Jobs.DBJobStatus.Complete) Then
            btnResend.Enabled = False
        Else
            btnResend.Enabled = True
        End If
    End Sub
End Class