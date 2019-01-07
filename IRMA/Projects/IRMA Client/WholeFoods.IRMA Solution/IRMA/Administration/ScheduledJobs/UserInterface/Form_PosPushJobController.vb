Imports log4net
Imports WholeFoods.IRMA.Replenishment.Jobs
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports IRMAService
Imports System.ServiceModel
Imports WholeFoods.IRMA.ProcessMonitor.DataAccess
Imports Infragistics.Win.UltraWinGrid
Imports WholeFoods.Utility
Imports WholeFoods.Utility.Encryption
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic

Public Class Form_POSPushJobController
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim WithEvents pushJob As POSPushJob

    Private Sub Form_PosPushJobController_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetJobStatusList()
        Me.ugrdJobStatus.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False
    End Sub

  Private Sub Button_POSPush_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_StartJob.Click
    Dim myBinding As New WSHttpBinding
    Dim myEndpoint As New EndpointAddress(ConfigurationServices.AppSettings("IRMAWebServiceAddress").ToString)
    Dim myChannelFactory As ChannelFactory(Of IGatewayChannel) = New ChannelFactory(Of IGatewayChannel)(myBinding, myEndpoint)
    Dim wcfClient1 As IGatewayChannel = myChannelFactory.CreateChannel()
    Dim encrypted As Boolean = CType(ConfigurationManager.AppSettings("encryptedConnectionStrings"), Boolean)
    Dim sConnectionString As String = ""
    Dim user As UserBO = New UserBO(giUserID)

    If (String.IsNullOrWhiteSpace(user.Email)) Then
      MessageBox.Show("User's eMail is not specified but required by this operation. Please contact your System Administrator.", "System Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
      Exit Sub
    End If

    Windows.Forms.Cursor.Current = Cursors.WaitCursor
    Application.DoEvents()

    Try
      sConnectionString = ConfigurationManager.ConnectionStrings("ItemCatalog").ConnectionString()

      If encrypted Then
        Dim encryptor As New Encryptor()
        sConnectionString = encryptor.Decrypt(sConnectionString)
      End If

      Dim blnPush = wcfClient1.RunPOSPush(ConfigurationServices.AppSettings("POSPushApplicationPath").ToString, gsRegionCode, sConnectionString, user.Email)

      'have the client sleep for 3 seconds so that the job has a chance to kick off and update the JobStatus table and so when the UI
      'refreshes, the grid will show the job as running
      Sleep(3000)
      GetJobStatusList()
    Catch ex As Exception
      MsgBox(ex.Message, MsgBoxStyle.Critical, "POS Push Error")
      Button_StartJob.Enabled = True

      If myChannelFactory.State = CommunicationState.Opened Then myChannelFactory.Close()

      logger.Error("Error during processing of the Scale/POS Push job", ex)
    Finally
      Windows.Forms.Cursor.Current = Cursors.Default
      Me.Refresh()
    End Try
  End Sub

  Private Sub GetJobStatusList()
        logger.Debug("GetJobStatusList Entry")
        Dim DAO As ProcessMDAO = New ProcessMDAO
        Dim ds = DAO.GetJobStatusList()

        Me.ugrdJobStatus.DataSource = ds.Tables(0)
        Me.ugrdJobStatus.DisplayLayout.Bands(0).Columns("Job Name").SortIndicator = SortIndicator.Descending

        If IsPushRunning() Then
            Button_StartJob.Enabled = False
            lblPushRunning.Visible = True
        Else
            Button_StartJob.Enabled = True
            lblPushRunning.Visible = False
        End If

        logger.Debug("GetJobStatusList Exit")

    End Sub

    Private Sub ugrdJobStatus_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles ugrdJobStatus.InitializeLayout
        e.Layout.Bands(0).Columns("LastRun").Format = gsUG_DateMask + " h:mm tt"

        'set column widths
        ugrdJobStatus.DisplayLayout.Bands(0).Columns(0).Width = 111
        ugrdJobStatus.DisplayLayout.Bands(0).Columns(1).Width = 87
        ugrdJobStatus.DisplayLayout.Bands(0).Columns(2).Width = 124
        ugrdJobStatus.DisplayLayout.Bands(0).Columns(3).Width = 72
        ugrdJobStatus.DisplayLayout.Bands(0).Columns(4).Width = 257
        ugrdJobStatus.DisplayLayout.Bands(0).Columns(5).Width = 231
    End Sub

    Private Sub ugrdJobStatus_InitializeRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles ugrdJobStatus.InitializeRow
        If e.Row.Cells(0).Value = "POSPushJob" Or e.Row.Cells(0).Value = "ScalePushJob" Then
            e.Row.Hidden = False
        Else
            e.Row.Hidden = True
        End If
    End Sub

    Private Sub Button_Refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Refresh.Click
        Cursor = Cursors.WaitCursor
        GetJobStatusList()
        Cursor = Cursors.Default
    End Sub

    Private Function IsPushRunning() As Boolean
        For Each dr As UltraGridRow In ugrdJobStatus.Rows
            If dr.Cells("Status").Value = "RUNNING" And dr.Hidden = False Then
                Return True
            End If
        Next

        Return False
    End Function

End Class