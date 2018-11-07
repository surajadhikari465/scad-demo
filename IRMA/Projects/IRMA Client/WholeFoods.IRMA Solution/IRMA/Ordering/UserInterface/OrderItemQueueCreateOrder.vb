Option Strict Off
Option Explicit On
Imports log4net
Imports VB = Microsoft.VisualBasic
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.Ordering.BusinessLogic.OrderingFunctions
Imports WholeFoods.IRMA.Ordering.DataAccess
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility

Friend Class frmOrderItemQueueCreateOrder
    Inherits System.Windows.Forms.Form

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private m_bCancel As Boolean
    Private m_bOverride As Boolean
    Private m_iVendorId As Integer
    Private m_sDefaultFax As String
    Private m_sDefaultEmail As String
    Private m_sOverrideTarget As String

	Private Sub cmdCreate_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCreate.Click
        m_bCancel = False

        If (Trim(m_sDefaultFax) <> Trim(txtFax.Text)) And optFax.Checked Then
            m_bOverride = True
            m_sOverrideTarget = Trim(txtFax.Text)
        Else
            m_bOverride = False
        End If

        If (Trim(m_sDefaultEmail) <> Trim(txtEmail.Text)) And optEmail.Checked Then
            m_bOverride = True
            m_sOverrideTarget = Trim(txtEmail.Text)
        Else
            m_bOverride = False
        End If

        Me.Hide()
	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click		
        Me.Hide()
    End Sub

    Private Sub GetOrderSendDefaults(ByVal iVendorID As Integer)
        logger.Debug("GetOrderSendDefaults Entry")
        Dim rsOrderSend As DAO.Recordset

        rsOrderSend = SQLOpenRecordSet("GetOrderSendInfo " & iVendorID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        logger.Debug("Manual = " & frmOrderSend.Manual.ToString)

        If Not IsDBNull(rsOrderSend.Fields("Fax").Value) Then
            txtFax.Text = rsOrderSend.Fields("Fax").Value
            m_sDefaultFax = rsOrderSend.Fields("Fax").Value
        End If

        If Not IsDBNull(rsOrderSend.Fields("Email").Value) Then
            txtEmail.Text = rsOrderSend.Fields("Email").Value
            m_sDefaultEmail = rsOrderSend.Fields("Email").Value
        End If

        rsOrderSend.Close()
        logger.Debug("GetOrderSendDefaults Exit")
    End Sub
	
	Private Sub frmOrderItemQueueCreateOrder_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmOrderItemQueueCreateOrder_Load Entry")
        Dim iDefaultPOTransmission As Integer
        Dim blnOverrideTransmissionMethod As Boolean

        iDefaultPOTransmission = ConfigurationServices.AppSettings("DefaultPOTransmission")
        blnOverrideTransmissionMethod = InstanceDataDAO.IsFlagActive("OverrideDefaultPOTransmissionMethod")

        optFax.Enabled = blnOverrideTransmissionMethod
        txtFax.Enabled = blnOverrideTransmissionMethod
        optEmail.Enabled = blnOverrideTransmissionMethod
        txtEmail.Enabled = blnOverrideTransmissionMethod
        optManual.Enabled = blnOverrideTransmissionMethod

        If GetVendorTransmissionType(Me.VendorId) = 4 Then
            optElectronic.Checked = True
            optElectronic.Enabled = True
            optManual.Enabled = True
            optFax.Enabled = False
            optEmail.Enabled = False
        Else
            optElectronic.Enabled = False
        End If

        m_bCancel = True
        m_bOverride = False
        m_sDefaultFax = ""
        m_sDefaultEmail = ""
        m_sOverrideTarget = ""

        CenterForm(Me)

        Select Case iDefaultPOTransmission
            Case 1
                optFax.Checked = True
            Case 2
                optEmail.Checked = True
            Case 4
                optElectronic.Checked = True
            Case Else
                optManual.Checked = True
        End Select

        GetOrderSendDefaults(Me.VendorId)

        logger.Debug("frmOrderItemQueueCreateOrder_Load Exit")
    End Sub
	
	Public ReadOnly Property IsFax() As Boolean
		Get
            IsFax = optFax.Checked
		End Get
    End Property

    Public ReadOnly Property IsEmail() As Boolean
        Get
            IsEmail = optEmail.Checked
        End Get
    End Property

    Public ReadOnly Property IsElectronic() As Boolean
        Get
            IsElectronic = optElectronic.Checked
        End Get
    End Property
	
	Public ReadOnly Property IsSend() As Boolean
		Get
			IsSend = chkSend.CheckState
		End Get
	End Property
	
	Public ReadOnly Property IsCancel() As Boolean
		Get
			IsCancel = m_bCancel
		End Get
    End Property

    Public ReadOnly Property IsOverrideTransmissionMethod() As Boolean
        Get
            IsOverrideTransmissionMethod = m_bOverride
        End Get
    End Property

    Public ReadOnly Property OverrideTarget() As String
        Get
            OverrideTarget = m_sOverrideTarget
        End Get
    End Property

    Public Property VendorId() As Integer
        Get
            VendorId = m_iVendorId
        End Get
        Set(ByVal value As Integer)
            m_iVendorId = value
        End Set
    End Property

    Public Shared Function GetVendorTransmissionType(ByVal sVendorId As String) As Integer
        logger.Debug("GetVendorTransmissionType Entry")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)

        ' Execute the function
        GetVendorTransmissionType = CType(factory.ExecuteScalar("SELECT dbo.fn_GetVendorTransmissionType(" & sVendorId & ")"), Integer)

        If GetVendorTransmissionType = -1 Then GetVendorTransmissionType = ConfigurationServices.AppSettings("DefaultPOTransmission")

        logger.Debug("GetVendorTransmissionType Exit")
    End Function
End Class