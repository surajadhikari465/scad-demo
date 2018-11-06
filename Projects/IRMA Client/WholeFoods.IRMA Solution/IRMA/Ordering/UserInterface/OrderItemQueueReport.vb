Option Strict Off
Option Explicit On
Imports log4net
Friend Class frmOrderItemQueueReport
	Inherits System.Windows.Forms.Form

    Private IsInitializing As Boolean
    Private mPurchasingVendor_ID As Integer
    Private mTransferToSubTeam_No As Integer
    Private mItem_Description As String
    Private mOrderType As Integer
    Private mIdentifier As String
    Private mVendor_ID As Integer
    Private mUser_ID As Integer

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Property PurchasingVendor_ID() As Integer
        Get
            PurchasingVendor_ID = mPurchasingVendor_ID
        End Get
        Set(ByVal Value As Integer)
            mPurchasingVendor_ID = Value
        End Set
    End Property

    Public Property TransferToSubTeam_No() As Integer
        Get
            TransferToSubTeam_No = mTransferToSubTeam_No
        End Get
        Set(ByVal Value As Integer)
            mTransferToSubTeam_No = Value
        End Set
    End Property

    Public Property Item_Description() As String
        Get
            Item_Description = mItem_Description
        End Get
        Set(ByVal Value As String)
            mItem_Description = Value
        End Set
    End Property

    Public Property OrderType() As Integer
        Get
            OrderType = mOrderType
        End Get
        Set(ByVal Value As Integer)
            mOrderType = Value
        End Set
    End Property

    Public Property Identifier() As String
        Get
            Identifier = mIdentifier
        End Get
        Set(ByVal Value As String)
            mIdentifier = Value
        End Set
    End Property

    Public Property Vendor_ID() As Integer
        Get
            Vendor_ID = mVendor_ID
        End Get
        Set(ByVal Value As Integer)
            mVendor_ID = Value
        End Set
    End Property

    Public Property User_ID() As Integer
        Get
            User_ID = mUser_ID
        End Get
        Set(ByVal Value As Integer)
            mUser_ID = Value
        End Set
    End Property

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Call Me.Close()

    End Sub
	
    Private Sub cmdReports_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReports.Click

        logger.Debug("cmdReports_Click Entry")

        Dim sReportURL As New System.Text.StringBuilder
        Dim iSortBy As Integer

        ' Set Sort By
        Select Case True
            Case _optSortBy_0.Checked
                iSortBy = 0
            Case _optSortBy_1.Checked
                iSortBy = 1
            Case _optSortBy_2.Checked
                iSortBy = 2
            Case Else
                iSortBy = 1                         ' default
        End Select

        ' Setup Report URL
        sReportURL.Append("OrderItemQueueReport")
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        ' Add Report Parameters
        sReportURL.Append("&PurchasingVendor_ID=" & mPurchasingVendor_ID)
        sReportURL.Append("&TransferToSubTeam_No=" & mTransferToSubTeam_No)

        If Len(Trim(mItem_Description)) <> 0 Then
            sReportURL.Append("&Item_Description=" & mItem_Description)
        End If

        sReportURL.Append("&OrderType=" & mOrderType)

        If Len(Trim(mIdentifier)) <> 0 Then
            sReportURL.Append("&Identifier=" & mIdentifier)
        End If

        sReportURL.Append("&SortBy=" & iSortBy)

        If Len(Trim(mVendor_ID)) <> 0 Then
            sReportURL.Append("&Vendor_ID=" & mVendor_ID)
        End If
        If mUser_ID > 0 Then
            sReportURL.Append("&User_ID=" & mUser_ID)
        End If

        ' Display Report    
        Call ReportingServicesReport(sReportURL.ToString)

        logger.Debug("cmdReports_Click Exit")

    End Sub
	
	Private Sub frmOrderItemQueueReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		CenterForm(Me)
		optSortBy(1).Checked = True
    End Sub
    
End Class