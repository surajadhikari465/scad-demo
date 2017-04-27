Option Strict Off
Option Explicit On

Imports log4net

Friend Class frmLogin
    Inherits System.Windows.Forms.Form

    Const Domain As String = "WFM"
    Dim mValidated As Boolean
    Dim mUserName As String
    Dim mRetryCount As Short
    Dim mSetUserInfo As Boolean

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Public Property UserName() As String
        Get
            UserName = mUserName
        End Get
        Set(ByVal Value As String)
            mUserName = Value
            txtUserName.Text = mUserName
        End Set
    End Property

    Public ReadOnly Property Validated_Renamed() As Boolean
        Get
            Validated_Renamed = mValidated
        End Get
    End Property

    Public WriteOnly Property SetUserInfo() As Boolean
        Set(ByVal Value As Boolean)
            mSetUserInfo = Value
        End Set
    End Property

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click

        Me.Hide()

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOK.Click
        logger.Debug("cmdOK_Click Entry")

        Dim sPassword As String

        'Validate
        mUserName = Trim(txtUserName.Text)
        sPassword = Trim(txtPassword.Text)

        If Len(mUserName) = 0 Then
            MsgBox("User Name is required", MsgBoxStyle.Critical, Me.Text)
            txtUserName.Focus()
            logger.Info("User Name is required")
            logger.Debug("cmdOK_Click Exit")
            Exit Sub
        End If

        If Len(sPassword) = 0 Then
            MsgBox("Password is required", MsgBoxStyle.Critical, Me.Text)
            txtPassword.Focus()
            logger.Info("Password is required")
            logger.Debug("cmdOK_Click Exit")
            Exit Sub
        End If

        mRetryCount = mRetryCount + 1

        If mRetryCount > 6 Then
            'Cause the app. to end - they are either locked-out or the password is expired
            MsgBox(ResourcesIRMA.GetString("FailedLogon"), MsgBoxStyle.Critical, Me.Text)
            Me.Hide()
            logger.Info(ResourcesIRMA.GetString("FailedLogon"))
            logger.Debug("cmdOK_Click Exit")
            Exit Sub
        End If

        Try
            'Windows authentication
            Dim sLogonErrMsg As String = WFM.UserAuthentication.WindowsAuthentication.ValidUser(mUserName, sPassword)
            If sLogonErrMsg.Length > 0 Then Throw New System.ApplicationException(sLogonErrMsg)

            If mSetUserInfo Then
                SQLOpenRS(gRSRecordset, "EXEC ValidateLogin '" & mUserName & "'", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough, False)
                If gRSRecordset.EOF Then
                    gRSRecordset.Close()
                    MsgBox(String.Format(ResourcesIRMA.GetString("IRMAAccountNotFound"), vbCrLf), MsgBoxStyle.Critical, Me.Text)
                    logger.Info(String.Format(ResourcesIRMA.GetString("IRMAAccountNotFound"), vbCrLf))
                    logger.Debug("cmdOK_Click Exit")
                    Exit Sub
                Else
                    If Not gRSRecordset.Fields("AccountEnabled").Value Then
                        gRSRecordset.Close()
                        MsgBox(String.Format(ResourcesIRMA.GetString("IRMAAccountDisabled"), vbCrLf), MsgBoxStyle.Critical, Me.Text)
                        logger.Info(String.Format(ResourcesIRMA.GetString("IRMAAccountDisabled"), vbCrLf))
                        Me.Hide()
                        logger.Debug("cmdOK_Click Exit")
                        Exit Sub
                    End If
                End If

                'Set global security flags
                giUserID = gRSRecordset.Fields("User_ID").Value
                My.Application.CurrentUserID = giUserID

                'Client Roles
                'Permissions that don't rely on others
                gbSuperUser = CBool(gRSRecordset.Fields("SuperUser").Value)
                gbBatchBuildOnly = gbSuperUser Or CBool(gRSRecordset.Fields("BatchBuildOnly").Value)
                gbCostAdmin = gbSuperUser Or CBool(gRSRecordset.Fields("CostAdmin").Value)
                gbDCAdmin = gbSuperUser Or CBool(gRSRecordset.Fields("DCAdmin").Value)
                gbDeletePO = gbSuperUser Or CBool(gRSRecordset.Fields("DeletePO").Value)
                gbFacilityCreditProcessor = gbSuperUser Or CBool(gRSRecordset.Fields("FacilityCreditProcessor").Value)
                gbEInvoicing = gbSuperUser Or CBool(gRSRecordset.Fields("EInvoicing_Administrator").Value)
                gbInventoryAdministrator = gbSuperUser Or CBool(gRSRecordset.Fields("Inventory_Administrator").Value)
                gbItemAdministrator = gbSuperUser Or CBool(gRSRecordset.Fields("Item_Administrator").Value)
                gbLockAdministrator = gbSuperUser Or CBool(gRSRecordset.Fields("Lock_Administrator").Value)
                gbPOAccountant = gbSuperUser Or CBool(gRSRecordset.Fields("PO_Accountant").Value)
                gbPOApprovalAdmin = gbSuperUser Or CBool(gRSRecordset.Fields("POApprovalAdmin").Value)
                gbPOEditor = gbSuperUser Or CBool(gRSRecordset.Fields("POEditor").Value)
                gbPriceBatchProcessor = gbSuperUser Or CBool(gRSRecordset.Fields("PriceBatchProcessor").Value)
                gbTaxAdministrator = gbSuperUser Or CBool(gRSRecordset.Fields("TaxAdministrator").Value)
                gbUserShrink = gbSuperUser Or CBool(gRSRecordset.Fields("Shrink").Value)
                gbUserShrinkAdmin = gbSuperUser Or CBool(gRSRecordset.Fields("ShrinkAdmin").Value)
                gbVendorAdministrator = gbSuperUser Or CBool(gRSRecordset.Fields("Vendor_Administrator").Value)
                gbVendorCostDiscrepancyAdmin = gbSuperUser Or CBool(gRSRecordset.Fields("VendorCostDiscrepancyAdmin").Value)
                gbWarehouse = gbSuperUser Or CBool(gRSRecordset.Fields("Warehouse").Value)
                gbApplicationConfigurationAdministrator = gbSuperUser Or CBool(gRSRecordset.Fields("ApplicationConfigAdmin").Value)
                gbDataAdministrator = gbSuperUser Or CBool(gRSRecordset.Fields("DataAdministrator").Value)
                gbJobAdministrator = gbSuperUser Or CBool(gRSRecordset.Fields("JobAdministrator").Value)
                gbPOSInterfaceAdministrator = gbSuperUser Or CBool(gRSRecordset.Fields("POSInterfaceAdministrator").Value)
                gbSecurityAdministrator = gbSuperUser Or CBool(gRSRecordset.Fields("SecurityAdministrator").Value)
                gbStoreAdministrator = gbSuperUser Or CBool(gRSRecordset.Fields("StoreAdministrator").Value)
                gbSystemConfigurationAdministrator = gbSuperUser Or CBool(gRSRecordset.Fields("SystemConfigurationAdministrator").Value)
                gbUserMaintenance = gbSuperUser Or CBool(gRSRecordset.Fields("UserMaintenance").Value)

                'Permissions that are based on other permissions
                gbAccountant = gbSuperUser Or gbPOAccountant Or CBool(gRSRecordset.Fields("Accountant").Value)
                gbCoordinator = gbSuperUser Or gbPOApprovalAdmin Or CBool(gRSRecordset.Fields("coordinator").Value)
                gbBuyer = gbSuperUser Or gbCoordinator Or CBool(gRSRecordset.Fields("Buyer").Value)
                gbDistributor = gbSuperUser Or gbPOAccountant Or CBool(gRSRecordset.Fields("Distributor").Value)

                gbReceiver = CBool(gRSRecordset.Fields("Distributor").Value)

                'Store Number fields
                glRecvLog_Store_Limit = IIf(IsDBNull(gRSRecordset.Fields("RecvLog_Store_Limit").Value), 0, gRSRecordset.Fields("RecvLog_Store_Limit").Value)
                glStore_Limit = IIf(IsDBNull(gRSRecordset.Fields("Telxon_Store_Limit").Value), 0, gRSRecordset.Fields("Telxon_Store_Limit").Value)
                glVendor_Limit = IIf(IsDBNull(gRSRecordset.Fields("Vendor_Limit").Value), 0, gRSRecordset.Fields("Vendor_Limit").Value)

                '365
                gsTitleDescription = IIf(IsDBNull(gRSRecordset.Fields("Title_Desc").Value), Nothing, gRSRecordset.Fields("Title_Desc").Value)
                If Not String.IsNullOrEmpty(gsTitleDescription) Then
                    gb365Administrator = gsTitleDescription.Contains("365")
                End If

                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
        Catch ex As Exception
            MsgBox(ex.Message & vbCrLf & "Please try again or contact the helpdesk for assistance.", MsgBoxStyle.Critical, Me.Text)
            logger.Error(ex.Message & vbCrLf & "Please try again or contact the helpdesk for assistance.")
            logger.Debug("cmdOK_Click Exit")
            txtPassword.Clear()
            Exit Sub
        End Try

        mValidated = True
        Me.Hide()

        logger.Debug("cmdOK_Click Exit")
    End Sub

    Private Sub frmLogin_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        logger.Debug("frmLogin_Load Entry")

        Dim sDomain As String
        sDomain = String.Empty

        GetLogonDomainUser(sDomain, mUserName) 'This routine delivers the local PC as the domain in some cases, so don't use below

        'Me.Text = My.Application.Info.Title
        txtUserName.Text = mUserName

        logger.Debug("frmLogin_Load Exit")

    End Sub

    Private Sub frmLogin_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        logger.Debug("frmLogin_FormClosed Entry")

        mValidated = False
        mRetryCount = 0
        mUserName = ""
        mSetUserInfo = False

        logger.Debug("frmLogin_FormClosed Exit")

    End Sub

    Private Sub txtPassword_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtPassword.Enter
        HighlightText(txtPassword)
    End Sub
End Class
