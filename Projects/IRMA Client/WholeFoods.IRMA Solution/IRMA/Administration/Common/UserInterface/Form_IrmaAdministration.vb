Imports log4net
Imports System.Configuration
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.IRMA.Administration.Common.DataAccess
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.Utility

''' <summary>
''' This is the initial form for the IRMA Adminitration application.  It controls the 
''' other forms that appear during the application process.
''' </summary>
''' <remarks></remarks>
Public Class Form_IrmaAdministration
#Region "Class Level Vars and Property Definitions"
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Const ENVIRONMENT_LABEL_PRD As String = "Production"

    ''' <summary>
    ''' represents the current region; found in InstanceData.PrimaryRegionCode
    ''' </summary>
    ''' <remarks></remarks>
    Private regionCode As String

    ''' <summary>
    ''' Keeps track of last time database activity was performed.
    ''' </summary>
    ''' <remarks></remarks>
    Private lastActivityTimestamp As Date

    ''' <summary>
    ''' Application level variable to keep track of the # of login attempts
    ''' </summary>
    ''' <remarks></remarks>
    Private loginRetryCount As Integer = 0

    ''' <summary>
    ''' Keeps track of the current user configuration data, once a user
    ''' has been authenticated to access the application.
    ''' </summary>
    ''' <remarks></remarks>
    Private userConfig As UserBO

    ''' <summary>
    ''' Login Form.
    ''' </summary>
    ''' <remarks></remarks>
    Dim WithEvents loginForm As Form_Login

    ''' <summary>
    ''' Initial form for managing the stores configured for the POS Push process.
    ''' </summary>
    ''' <remarks></remarks>
    Dim WithEvents storeForm As Form_ManageStores

    ''' <summary>
    ''' Initial form for managing the POS Writer configurations for the POS Push process.
    ''' </summary>
    ''' <remarks></remarks>
    Dim WithEvents posWriterFileConfig As Form_ManagePOSWriter

    ''' <summary>
    ''' Initial form for managing the IRMA Client Users.
    ''' </summary>
    ''' <remarks></remarks>
    Dim WithEvents manageUsers As Form_ManageUsers

    ''' <summary>
    ''' Initial form for starting the POS Push and Scale Push processes.
    ''' </summary>
    ''' <remarks></remarks>
    Dim WithEvents posPushJobController As Form_POSPushJobController

    ''' <summary>
    ''' Initial form for starting the Scale Push process.
    ''' </summary>
    ''' <remarks></remarks>
    Dim WithEvents scalePushJobController As Form_ScalePushJobController

    ''' <summary>
    ''' Initial form for starting the Audit Report process.
    ''' </summary>
    ''' <remarks></remarks>
    Dim WithEvents auditReportJobController As Form_AuditReportJobController

    ''' <summary>
    ''' Initial form for starting the build full store scale file process.
    ''' </summary>
    ''' <remarks></remarks>
    Dim WithEvents buildFullScaleFileForStore As Form_BuildFullScaleFileForStore

    ''' <summary>
    ''' Initial form for starting the PLUM Host Jobs.
    ''' </summary>
    ''' <remarks></remarks>
    Dim WithEvents plumHostJobController As Form_PLUMHostJobController

    ''' <summary>
    ''' Initial form for starting the Audit Exceptions Report process.
    ''' </summary>
    ''' <remarks></remarks>
    Dim WithEvents auditExceptionsJobController As Form_AuditExceptionsJobController

#End Region

    ''' <summary>
    ''' Function to locate an application that is running by the window name.
    ''' </summary>
    ''' <param name="lpClassName"></param>
    ''' <param name="lpWindowName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As Integer

    Public _EnvironmentId As String = String.Empty



#Region "Events handled by this form"
#Region "Form Load"

    Private Sub Form_IrmaAdministration_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

        Select Case True
            Case e.KeyCode = Keys.C And e.Modifiers = Keys.Control ' Control - C
                If ToolStripMenuItem_ConfigurationData.Enabled Then
                    OpenConfigurationScreen()
                End If
            Case e.KeyCode = Keys.B And e.Modifiers = Keys.Control ' Control - B
                If ToolStripMenuItem_ConfigurationData.Enabled Then
                    OpenBuildConfigutationScreen()
                End If
        End Select


    End Sub
    ''' <summary>
    ''' Load the main form for the admin application.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Form_IrmaAdministration_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        logger.Debug("Form_IrmaAdministration_Load entry")
        ' Set up the version number - this is stored in the AssemblyInfo.vb
        Dim versionString As String = " v" & My.Application.Info.Version.Major & "." & My.Application.Info.Version.Minor & "." & My.Application.Info.Version.Build
        If My.Application.Info.Version.Revision > 0 Then
            versionString = versionString & "." & My.Application.Info.Version.Revision
        End If

        ' Define the application caption
        Dim appCaption As String = Me.Text & versionString

        'get current region
        regionCode = InstanceDataDAO.GetInstanceData.RegionCode

        ' Set the tool status values
        ToolStripStatusLabel_Region.Text = ToolStripStatusLabel_Region.Text & regionCode
        ToolStripStatusLabel_Environment.Text = ToolStripStatusLabel_Environment.Text & ConfigurationServices.AppSettings("environment")
        ToolStripStatusLabel_Version.Text = ToolStripStatusLabel_Version.Text & "System: " & VersionDAO.GetVersionInfo("System") & "     Application: " & VersionDAO.GetVersionInfo("Admin")

        ' Check to see if an instance of the admin app is already running with this caption
        ' Only allow one instance at a time
        Dim windowCount As Integer = FindWindow(vbNullString, appCaption)
        If windowCount <> 0 Then
            MsgBox(ResourcesCommon.GetString("error_appAlreadyRunning"), MsgBoxStyle.Critical, "Application Already Running")
            Me.Close()
            End
        Else
            Me.Text = "IRMA Administration"
        End If

        ' Show the admin application in the background of the login box
        Me.Show()

        'check that the user is using the most up-to-date version of the admin tool
        Dim clientVersion As String = My.Application.Info.Version.Major & "." & My.Application.Info.Version.Minor & "." & My.Application.Info.Version.Build
        If My.Application.Info.Version.Revision > 0 Then
            clientVersion = clientVersion & "." & My.Application.Info.Version.Revision
        End If

        If VersionDAO.CheckCurrentVersion(clientVersion) = False Then
            'not current - inform user and close application
            logger.Info("Form_IrmaAdministration_Load: versions do not match.  AppVersion=" + clientVersion)
            MessageBox.Show(String.Format(ResourcesCommon.GetString("error_invalidVersion"), clientVersion, vbCrLf), "Invalid Version", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Me.Close()
        Else
            ' Validate the login
            If Not ValidateLogon() Then
                Me.Close()
                End
            End If

            ' Provide the user access to the menu options based on their IRMA role
            SetMenuAccess()
        End If

        ' Display a warning page if this is not production
        'If ConfigurationServices.AppSettings("environment") <> ENVIRONMENT_LABEL_PRD Then
        '    MsgBox(ResourcesCommon.GetString("msg_warning_testSystem"), MsgBoxStyle.Information, Me.Text)
        'End If

        If ConfigurationServices.AppSettings("environment") <> ENVIRONMENT_LABEL_PRD Then
            MsgBox(ResourcesCommon.GetString("msg_warning_testSystem"), MsgBoxStyle.Information, Me.Text)
        End If

        logger.Debug("Form_IrmaAdministration_Load exit")
    End Sub

    ''' <summary>
    ''' Validate the user's logon credentials.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function ValidateLogon() As Boolean
        Dim loginStatus As Boolean = False

        ' The user must login after 30 minutes of inactivity.
        ' Inactivity is tracked based on access to the database.
        If DateDiff(Microsoft.VisualBasic.DateInterval.Minute, lastActivityTimestamp, Now) >= 30 Then
            ' Display the login form
            loginForm = New Form_Login()
            loginForm.LoginData = New LoginBO()

            ' keep showing the login form until the user logs in successfully or they reach the max number of tries
            While Not loginForm.LoginStatus AndAlso Not LoginBO.MaximumRetriesReached(loginRetryCount) AndAlso Not loginForm.LoginCancelled
                If loginForm IsNot Nothing Then
                    ' Close the current instance of the login form & try again
                    loginForm.Close()
                    loginForm.Dispose()
                End If

                ' Display the login form
                loginForm = New Form_Login()
                loginForm.LoginData = New LoginBO()

                ' Pre-fill the user id if they've already logged in successfully and this is a timeout
                If (userConfig IsNot Nothing) AndAlso (Len(userConfig.UserName) > 0) Then
                    loginForm.LoginData.UserName = userConfig.UserName
                    loginForm.ValidateIRMALogin = False
                End If

                loginForm.ShowDialog(Me)
                loginStatus = loginForm.LoginStatus

                ' Increment the retry count
                loginRetryCount = loginRetryCount + 1

                ' Have we exceeded the maximum attempts?
                If LoginBO.MaximumRetriesReached(loginRetryCount) Then
                    MessageBox.Show(ResourcesCommon.GetString("error_maxLoginAttempts"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    ' TODO: Should the IRMA account be disabled when this happens?
                End If
            End While
        End If

        ' store the user configuration
        userConfig = loginForm.LoginData.UserConfig

        ' close the login form
        loginForm.Close()
        loginForm.Dispose()

        ' Update the timestamp for the most recent activity
        lastActivityTimestamp = Now

        ' Reset the login retries in case of a timeout later
        loginRetryCount = 0

        Return loginStatus
    End Function

    ''' <summary>
    ''' Set access to the menu options based on the user's IRMA role.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetMenuAccess()

        Dim tsdi As ToolStripDropDownItem

        ' Stores
        For Each tsdi In ToolStripMenuItem_Stores.DropDownItems
            Select Case tsdi.Name
                Case ToolStripMenuItem_Exit.Name
                    tsdi.Enabled = True
                Case Else
                    tsdi.Enabled = userConfig.SuperUser
            End Select
        Next

        ' File Writers
        For Each tsdi In ToolStripMenuItem_FileWriters.DropDownItems
            tsdi.Enabled = userConfig.SuperUser
        Next

        ' Users
        For Each tsdi In ToolStripMenuItem_Users.DropDownItems
            tsdi.Enabled = userConfig.SuperUser
        Next

        ' Data Tools
        For Each tsdi In BatchMgmtToolStripMenuItem.DropDownItems
            tsdi.Enabled = userConfig.SuperUser
        Next

        ' Data Configuration Menu
        For Each tsdi In DataConfigurationToolStripMenuItem.DropDownItems
            Select Case tsdi.Name
                Case ToolStripMenuItem_ConfigurationData.Name
                    tsdi.Enabled = True
                    ManageToolStripMenuItem.Enabled = (userConfig.ConfigurationAdmin And userConfig.SuperUser)
                    BuildToolStripMenuItem.Enabled = (userConfig.ConfigurationAdmin And userConfig.SuperUser)
                Case Else
                    tsdi.Enabled = userConfig.SuperUser
            End Select
        Next

        ' Scheduled Jobs
        ManageScheduledJobsToolStripMenuItem.Enabled = userConfig.SuperUser
        For Each tsdi In RunScheduleJobsToolStripMenuItem.DropDownItems
            Select Case tsdi.Name
                Case APUploadToolStripMenuItem.Name
                    ' AP Upload is disabled in Production as it is a scheduled job.
                    If ConfigurationServices.AppSettings("environment") = ENVIRONMENT_LABEL_PRD Then
                        tsdi.Enabled = False
                        tsdi.Visible = False
                    Else
                        tsdi.Enabled = userConfig.SuperUser
                    End If
                Case TlogProcessingToolStripMenuItem.Name
                    ' TLOG can be run by an Item Admin
                    tsdi.Enabled = userConfig.SuperUser Or userConfig.ItemAdministrator
                Case EInvoicingIntegrationToolStripMenuItem.Name
                    tsdi.Enabled = userConfig.SuperUser Or userConfig.EInvoicingAdministrator
                Case Else
                    tsdi.Enabled = userConfig.SuperUser
            End Select
        Next

        Dim tss As New ToolStripSeparator

        ToolStripMenuItem_Stores.DropDownItems.Insert(2, tss)


    End Sub

#End Region

#Region "View menu options"
    ''' <summary>
    ''' User selected the Stores -> View Stores menu option.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ViewStoresToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem_ViewStores.Click
        logger.Debug("ViewStoresToolStripMenuItem_Click entry")
        Try
            ' Bring focus to the form
            storeForm = New Form_ManageStores
            storeForm.ShowDialog(Me)
            storeForm.Dispose()
        Catch ex As Exception
            logger.Error("ViewStoresToolStripMenuItem_Click exception when getting type=Form_ManageStores", ex)
            DisplayErrorMessage()
        End Try
        logger.Debug("ViewStoresToolStripMenuItem_Click exit")
    End Sub

    ''' <summary>
    ''' User selected the Writers -> View Writers menu option.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ViewWritersToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem_ViewWriters.Click
        logger.Debug("ViewWritersToolStripMenuItem_Click entry")
        Try
            ' Bring focus to the form
            posWriterFileConfig = New Form_ManagePOSWriter()
            posWriterFileConfig.ShowDialog(Me)
            posWriterFileConfig.Dispose()
        Catch ex As Exception
            logger.Error("ViewWritersToolStripMenuItem_Click exception when getting type=Form_ManagePOSWriter", ex)
            DisplayErrorMessage()
        End Try
        logger.Debug("ViewWritersToolStripMenuItem_Click exit")
    End Sub

    ''' <summary>
    ''' User selected the Users -> View Users menu option.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ViewUsersToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem_ViewUsers.Click
        logger.Debug("ViewWritersToolStripMenuItem_Click entry")
        Try
            ' Bring focus to the form
            Me.Cursor = Cursors.WaitCursor
            manageUsers = New Form_ManageUsers()
            manageUsers.ShowDialog(Me)
            manageUsers.Dispose()
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            logger.Error("ViewWritersToolStripMenuItem_Click exception when getting type=Form_ManagePOSWriter", ex)
            DisplayErrorMessage()
        Finally
            Me.Cursor = Cursors.Default
        End Try
        logger.Debug("ViewWritersToolStripMenuItem_Click exit")
    End Sub

    Private Sub ScalePushToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem_ScalePush.Click
        logger.Debug("ScalePushToolStripMenuItem_Click entry")
        Try
            ' Bring focus to the form
            scalePushJobController = New Form_ScalePushJobController()
            scalePushJobController.ShowDialog(Me)
            scalePushJobController.Dispose()
        Catch ex As Exception
            logger.Error("ScalePushToolStripMenuItem_Click exception", ex)
            DisplayErrorMessage()
        End Try
        logger.Debug("ScalePushToolStripMenuItem_Click exit")
    End Sub

    ''' <summary>
    ''' User selected the Scheduled Jobs -> POS Push menu option.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub POSPushToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem_POSPush.Click
        logger.Debug("POSPushToolStripMenuItem_Click entry")
        Try
            ' Bring focus to the form
            posPushJobController = New Form_POSPushJobController()
            posPushJobController.ShowDialog(Me)
            posPushJobController.Dispose()
        Catch ex As Exception
            logger.Error("POSPushToolStripMenuItem_Click exception", ex)
            DisplayErrorMessage()
        End Try
        logger.Debug("POSPushToolStripMenuItem_Click exit")
    End Sub

    ''' <summary>
    ''' User selected the Scheduled Jobs -> PLUM Host menu option.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PLUMHostToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PLUMHostToolStripMenuItem.Click
        logger.Debug("PLUMHostToolStripMenuItem_Click entry")
        Try
            ' Bring focus to the form
            plumHostJobController = New Form_PLUMHostJobController()
            plumHostJobController.ShowDialog(Me)
            plumHostJobController.Dispose()
        Catch ex As Exception
            logger.Error("PLUMHostToolStripMenuItem_Click exception", ex)
            DisplayErrorMessage()
        End Try
        logger.Debug("PLUMHostToolStripMenuItem_Click exit")
    End Sub
#End Region

#Region "Exit menu options"
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem_Exit.Click
        Me.Close()
    End Sub
#End Region
#End Region

    Private Sub TlogProcessingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TlogProcessingToolStripMenuItem.Click
        Select Case regionCode.ToUpper
            Case "EU" 'changing from UK to EU to match UK's regional code
                Dim frm As Form_IRISTlogProcessing = New Form_IRISTlogProcessing()
                frm.ShowDialog()
                frm.Dispose()
            Case "FL", "SP", "NA", "NE", "MA", "SW", "RM", "PN", "NC", "MW"
                Dim frm As Form_HouseTlogProcessing = New Form_HouseTlogProcessing()
                frm.ShowDialog()
                frm.Dispose()
            Case Else
                MessageBox.Show("Tlog Processing does not support your region at this time. (" & regionCode & ")")
        End Select
    End Sub

    Private Sub POSPullToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles POSPullToolStripMenuItem.Click
        Dim frm As Form_POSPullJobController = New Form_POSPullJobController()
        frm.ShowDialog()
        frm.Dispose()
    End Sub

    Private Sub InstanceDataFlagsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InstanceDataFlagsToolStripMenuItem.Click
        Dim form As Form_InstanceDataFlags = New Form_InstanceDataFlags
        form.ShowDialog()
        form.Dispose()
    End Sub

    Private Sub BuildStorePOSFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BuildStorePOSFileToolStripMenuItem.Click
        logger.Debug("BuildStorePOSFileToolStripMenuItem_Click entry")
        Try
            ' Bring focus to the form
            auditReportJobController = New Form_AuditReportJobController()
            auditReportJobController.ShowDialog(Me)
            auditReportJobController.Dispose()
        Catch ex As Exception
            logger.Error("BuildStorePOSFileToolStripMenuItem_Click exception", ex)
            DisplayErrorMessage()
        End Try
        logger.Debug("BuildStorePOSFileToolStripMenuItem_Click exit")
    End Sub

    Private Sub BuildStoreScaleFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BuildStoreScaleFileToolStripMenuItem.Click
        logger.Debug("BuildStoreScaleFileToolStripMenuItem_Click entry")
        Try
            ' Bring focus to the form
            buildFullScaleFileForStore = New Form_BuildFullScaleFileForStore()
            buildFullScaleFileForStore.ShowDialog(Me)
            buildFullScaleFileForStore.Dispose()
        Catch ex As Exception
            logger.Error("BuildStoreScaleFileToolStripMenuItem_Click exception", ex)
            DisplayErrorMessage()
        End Try
        logger.Debug("BuildStoreScaleFileToolStripMenuItem_Click exit")
    End Sub

    ''' <summary>
    ''' User selected the Scheduled Jobs -> Audit Report (Exceptions) menu option.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ToolStripMenuItem_AuditExceptionsReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem_AuditExceptionsReport.Click
        logger.Debug("ToolStripMenuItem_AuditExceptionsReport_Click entry")
        Try
            ' Bring focus to the form
            auditExceptionsJobController = New Form_AuditExceptionsJobController()
            auditExceptionsJobController.ShowDialog(Me)
            auditExceptionsJobController.Dispose()
        Catch ex As Exception
            logger.Error("ToolStripMenuItem_AuditExceptionsReport_Click exception", ex)
            DisplayErrorMessage()
        End Try
        logger.Debug("ToolStripMenuItem_AuditExceptionsReport_Click exit")

    End Sub

    Private Sub WeeklySalesRollupToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WeeklySalesRollupToolStripMenuItem.Click
        logger.Debug("WeeklySalesRollupToolStripMenuItem_Click entry")
        Try
            Dim form As Form_WeeklySalesRollup = New Form_WeeklySalesRollup
            form.ShowDialog()
            form.Dispose()
        Catch ex As Exception
            logger.Error("WeeklySalesRollupToolStripMenuItem_Click exception", ex)
            DisplayErrorMessage()
        End Try
        logger.Debug("WeeklySalesRollupToolStripMenuItem_Click exit")
    End Sub
    Private Sub RegionalInstanceDataToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RegionalInstanceDataToolStripMenuItem.Click
        logger.Debug("RegionalInstanceDataToolStripMenuItem_Click entry")
        Try
            Dim form As Form_RegionalInstanceData = New Form_RegionalInstanceData
            form.ShowDialog()
            form.Dispose()
        Catch ex As Exception
            logger.Error("RegionalInstanceDataToolStripMenuItem_Click exception", ex)
            DisplayErrorMessage()
        End Try
        logger.Debug("RegionalInstanceDataToolStripMenuItem_Click exit")
    End Sub

    Private Sub ManageItemAttributesToolStrinpMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ManageItemAttributesToolStrinpMenuItem.Click
        logger.Debug("ManageItemAttributesToolStrinpMenuItem_Click entry")
        Try
            Dim form As Form_ManageItemAttributes = New Form_ManageItemAttributes
            form.ShowDialog()
            form.Dispose()
        Catch ex As Exception
            logger.Error("ManageItemAttributesToolStrinpMenuItem_Click exception", ex)
            DisplayErrorMessage()
        End Try
        logger.Debug("ManageItemAttributesToolStrinpMenuItem_Click exit")
    End Sub

    Private Sub ManagePricingMethodsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ManagePricingMethodsToolStripMenuItem.Click
        logger.Debug("ManagePricingMethodsToolStripMenuItem_Click entry")
        Try
            Dim form As Form_ManagePricingMethods = New Form_ManagePricingMethods
            form.ShowDialog()
            form.Dispose()
        Catch ex As Exception
            logger.Error("ManagePricingMethodsToolStripMenuItem_Click exception", ex)
            DisplayErrorMessage()
        End Try
        logger.Debug("ManagePricingMethodsToolStripMenuItem_Click exit")
    End Sub

    Private Sub ManageMenuAccessToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ManageMenuAccessToolStripMenuItem.Click
        logger.Debug("ManageMenuAccessToolStripMenuItem_Click entry")
        Try
            Dim form As Form_ManageMenuAccess = New Form_ManageMenuAccess
            form.ShowDialog()
            form.Dispose()
        Catch ex As Exception
            logger.Error("ManageMenuAccessToolStripMenuItem_Click exception", ex)
            DisplayErrorMessage()
        End Try
        logger.Debug("ManageMenuAccessToolStripMenuItem_Click exit")
    End Sub

    Private Sub CloseReceivingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseReceivingToolStripMenuItem.Click
        logger.Debug("CloseReceivingToolStripMenuItem_Click entry")
        Try
            Dim form As Form_CloseRecievingJobController = New Form_CloseRecievingJobController()
            form.UserId = userConfig.UserId
            form.ShowDialog()
            form.Dispose()
        Catch ex As Exception
            logger.Error("CloseReceivingToolStripMenuItem_Click exception", ex)
            DisplayErrorMessage()
        End Try
        logger.Debug("CloseReceivingToolStripMenuItem_Click exit")
    End Sub

    Private Sub SendOrdersToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SendOrdersToolStripMenuItem.Click
        logger.Debug("SendOrdersToolStripMenuItem_Click entry")
        Try
            Dim form As Form_SendOrders = New Form_SendOrders()
            form.ShowDialog()
            form.Dispose()
        Catch ex As Exception
            logger.Error("SendOrdersToolStripMenuItem_Click exception", ex)
            DisplayErrorMessage()
        End Try
        logger.Debug("SendOrdersToolStripMenuItem_Click exit")
    End Sub

    Private Sub APUploadToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles APUploadToolStripMenuItem.Click
        logger.Debug("APUploadToolStripMenuItem_Click entry")
        Try
            Dim form As Form_PeopleSoftUploadJobController = New Form_PeopleSoftUploadJobController()
            form.ShowDialog(Me)
            form.Dispose()
        Catch ex As Exception
            logger.Error("APUploadToolStripMenuItem_Click exception", ex)
            DisplayErrorMessage()
        End Try
        logger.Debug("APUploadToolStripMenuItem_Click exit")
    End Sub

    Private Sub ManageScheduledJobsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ManageScheduledJobsToolStripMenuItem.Click
        logger.Debug("ManageScheduledJobsToolStripMenuItem_Click entry")
        Try
            Dim form As Form_ManageScheduledJobs = New Form_ManageScheduledJobs()
            form.ShowDialog(Me)
            form.Dispose()
        Catch ex As Exception
            logger.Error("ManageScheduledJobsToolStripMenuItem_Click exception", ex)
            DisplayErrorMessage()
        End Try
        logger.Debug("ManageScheduledJobsToolStripMenuItem_Click exit")
    End Sub

    Private Sub AverageCostUpdateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AverageCostUpdateToolStripMenuItem.Click
        logger.Debug("AverageCostUpdateToolStripMenuItem_Click entry")
        Try
            Dim form As Form_AverageCostUpdateController = New Form_AverageCostUpdateController()
            form.ShowDialog()
            form.Dispose()
        Catch ex As Exception
            logger.Error("AverageCostUpdateToolStripMenuItem_Click exception", ex)
            DisplayErrorMessage()
        End Try
        logger.Debug("AverageCostUpdateToolStripMenuItem_Click exit")
    End Sub

    Private Sub ChangeBatchStateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChangeBatchStateToolStripMenuItem.Click
        logger.Debug("ChangeBatchStateToolStripMenuItem_Click entry")
        Try
            Dim form As Form_ChangeBatchState = New Form_ChangeBatchState()
            form.ShowDialog()
            form.Dispose()
        Catch ex As Exception
            logger.Error("ChangeBatchStateToolStripMenuItem_Click exception", ex)
            DisplayErrorMessage()
        End Try
        logger.Debug("ChangeBatchStateToolStripMenuItem_Click exit")
    End Sub

    Private Sub EditTaxJurisdictionsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TaxJurisdictionsToolStripMenuItem.Click
        logger.Debug("EditTaxJurisdictionsToolStripMenuItem_Click entry")
        Try
            'Dim form As frmTaxJurisdictionAdmin = New frmTaxJurisdictionAdmin()
            'form.ShowDialog()
            'form.Dispose()

            Using dlg As New TaxJurisdictionAdmin()
                dlg.Enabled = True
                dlg.ShowDialog(Me)
            End Using
        Catch ex As Exception
            logger.Error("frmTaxJurisdictionAdmin exception", ex)
            DisplayErrorMessage()
        End Try
        logger.Debug("EditTaxJurisdictionsToolStripMenuItem_Click exit")
    End Sub

    Private Sub EditZonesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ZonesToolStripMenuItem.Click
        logger.Debug("EditZonesToolStripMenuItem_Click entry")
        Try
            Using dlg As New ZoneAdmin()
                dlg.Enabled = True
                dlg.ShowDialog(Me)
            End Using
        Catch ex As Exception
            logger.Error("frmZoneAdmin exception", ex)
            DisplayErrorMessage()
        End Try
        logger.Debug("EditZonesToolStripMenuItem_Click exit")
    End Sub

    Private Sub EditCurrencyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CurrencyToolStripMenuItem.Click
        logger.Debug("EditCurrencyToolStripMenuItem_Click entry")
        Try
            Using dlg As New CurrencyAdmin()
                dlg.Enabled = True
                dlg.ShowDialog(Me)
            End Using
        Catch ex As Exception
            logger.Error("frmCurrencyAdmin exception", ex)
            DisplayErrorMessage()
        End Try
        logger.Debug("EditCurrencyToolStripMenuItem_Click exit")
    End Sub

    Private Sub CreateStoreToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreateStoreToolStripMenuItem.Click
        logger.Debug("CreateStoreToolStripMenuItem_Click entry")
        Try
            Using dlg As New StoreAdd()
                dlg.Enabled = True
                dlg.ShowDialog(Me)
            End Using
        Catch ex As Exception
            logger.Error("frmStoreAdd exception", ex)
            DisplayErrorMessage()
        End Try
        logger.Debug("CreateStoreToolStripMenuItem_Click exit")
    End Sub

    Private Sub RestoreItemToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RestoreDeletedItemToolStripMenuItem.Click
        logger.Debug("RestoreItemToolStripMenuItem_Click entry")
        Try
            Using dlg As New Form_ItemRestore()
                dlg.Enabled = True
                dlg.ShowDialog(Me)
            End Using
        Catch ex As Exception
            logger.Error("Form_ItemRestore exception", ex)
            DisplayErrorMessage()
        End Try
        logger.Debug("RestoreItemToolStripMenuItem_Click exit")
    End Sub

    Private Sub EInvoicingIntegrationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EInvoicingIntegrationToolStripMenuItem.Click
        Dim frm As EInvoicing_ViewInvoices = New EInvoicing_ViewInvoices()
        frm.ShowDialog()
        frm.Dispose()
    End Sub

    Private Sub ManageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ManageToolStripMenuItem.Click
        OpenConfigurationScreen()
    End Sub
    Private Sub OpenConfigurationScreen()
        logger.Debug("   OpenConfigurationScreen() entry")
        Try
            Me.Cursor = Cursors.WaitCursor
            Dim form As Form_ConfigurationData = New Form_ConfigurationData()
            form.ShowDialog()
            form.Dispose()
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            logger.Error("   OpenConfigurationScreen() exception", ex)
            DisplayErrorMessage()
        End Try
        logger.Debug("OpenConfigurationScreen() exit")
    End Sub

    Private Sub BuildToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BuildToolStripMenuItem.Click
      OpenBuildConfigutationScreen
    End Sub

    Private Sub OpenBuildConfigutationScreen()
        logger.Debug("OpenBuildConfigutationScreen entry")
        Try
            Me.Cursor = Cursors.WaitCursor
            Dim form As Form_ConfigurationData_Build = New Form_ConfigurationData_Build()
            form.ShowDialog()
            form.Dispose()
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            Me.Cursor = Cursors.Default
            logger.Error("OpenBuildConfigutationScreen exception", ex)
            DisplayErrorMessage()
        End Try
        logger.Debug("OpenBuildConfigutationScreen exit")
    End Sub

    Private Sub ManageTitlesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ManageTitlesToolStripMenuItem.Click
        Dim frm As Form_ManageTitles = New Form_ManageTitles
        frm.ShowDialog()
        frm.Dispose()
    End Sub
End Class
