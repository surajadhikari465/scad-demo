Option Strict Off
Option Explicit On

Imports log4net
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Friend Class frmCopyPO_ExpectedDate
    Inherits System.Windows.Forms.Form

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private m_sExpectedDate As Date
    Private m_iCopyToStoreNo As Integer
    Private m_iCopyFromStoreNo As Integer
    Private m_iLimitStoreNo As Integer

    Public Property ExpectedDate() As Date
        Get
            ExpectedDate = m_sExpectedDate
        End Get
        Set(ByVal Value As Date)
            m_sExpectedDate = Value
        End Set
    End Property

    Public Property CopyToStoreNo() As Integer
        Get
            CopyToStoreNo = m_iCopyToStoreNo
        End Get
        Set(ByVal value As Integer)
            m_iCopyToStoreNo = value
        End Set
    End Property

    Public Property CopyFromStoreNo() As Integer
        Get
            CopyFromStoreNo = m_iCopyFromStoreNo
        End Get
        Set(ByVal value As Integer)
            m_iCopyFromStoreNo = value
        End Set
    End Property

    Public Property LimitStoreNo() As Integer
        Get
            LimitStoreNo = m_iLimitStoreNo
        End Get
        Set(ByVal value As Integer)
            m_iLimitStoreNo = value
        End Set
    End Property

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Me.ExpectedDate = dtpExpectedDate.Value
        Me.CopyToStoreNo = cmbCopyToStore.SelectedValue
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.ExpectedDate = Nothing
    End Sub

    Private Sub frmCopyPO_ExpectedDate_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim storeDAO As New StoreDAO

        dtpExpectedDate.Value = DateAdd(DateInterval.Day, 1, Now.Date)
        dtpExpectedDate.MinDate = Today

        cmbCopyToStore.DataSource = storeDAO.GetStoreAndDCList
        cmbCopyToStore.DisplayMember = "Store_Name"
        cmbCopyToStore.ValueMember = "Store_No"
        cmbCopyToStore.SelectedValue = Me.CopyFromStoreNo

        ' 20100312 - Dave Stacey TFS 12082 - allow superuser w/store limit to still choose any store when copying PO
        If Not gbSuperUser And Me.LimitStoreNo > 0 Then
            cmbCopyToStore.SelectedValue = Me.LimitStoreNo
            cmbCopyToStore.Enabled = False
        End If
    End Sub

End Class