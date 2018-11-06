Option Strict Off
Option Explicit On

Imports log4net
Friend Class frmStoreItemVendors
	Inherits System.Windows.Forms.Form
	Private m_lItemKey As Integer
	Private m_lStoreNo As Integer
	Private m_sStoreName As String
	Private m_sItemIdentifier As String
	Private m_sItemDescription As String
    Private mdt As DataTable
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Sub New(ByRef lItemKey As Integer, ByRef lStoreNo As Integer, Optional ByRef sStoreName As String = "", Optional ByRef sItemDescription As String = "", Optional ByRef sItemIdentifier As String = "")

        logger.Debug(" New Entry")

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_lItemKey = lItemKey
        m_lStoreNo = lStoreNo
        If sStoreName <> "" Then Me.StoreName = sStoreName
        If sItemDescription <> "" Then Me.ItemDescription = sItemDescription
        If sItemIdentifier <> "" Then Me.ItemIdentifier = sItemIdentifier

        logger.Debug("New Exit")
    End Sub
	Public ReadOnly Property Item_Key() As Integer
		Get
			Item_Key = m_lItemKey
		End Get
	End Property
	Public Property StoreName() As String
		Get
			StoreName = m_sStoreName
		End Get
		Set(ByVal Value As String)
			Me.txtStore.Text = Value
			m_sStoreName = Value
		End Set
	End Property
	Public Property ItemIdentifier() As String
		Get
			ItemIdentifier = m_sItemIdentifier
		End Get
		Set(ByVal Value As String)
			Me.txtIdentifier.Text = Value
			m_sItemIdentifier = Value
		End Set
	End Property
	Public Property ItemDescription() As String
		Get
			ItemDescription = m_sItemDescription
		End Get
		Set(ByVal Value As String)
			Me.txtItem_Description.Text = Value
			m_sItemDescription = Value
		End Set
	End Property
	

    Private Sub RefreshGrid()

        logger.Debug("RefreshGrid Entry")
        Dim rsVendorList As DAO.Recordset = Nothing
        Dim row As DataRow

        mdt.Clear()

        Try
            '-- Set up the databound stuff
            rsVendorList = SQLOpenRecordSet("EXEC GetItemStoreVendorsCost " & m_lStoreNo & ", " & m_lItemKey, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough + DAO.RecordsetOptionEnum.dbForwardOnly)

            While Not rsVendorList.EOF
                row = mdt.NewRow
                row("CompanyName") = rsVendorList.Fields("CompanyName").Value
                row("UnitCost") = rsVendorList.Fields("UnitCost").Value
                If rsVendorList.Fields("PrimaryVendor").Value Then
                    row("PrimaryVendor") = "*"
                Else
                    row("PrimaryVendor") = ""
                End If

                mdt.Rows.Add(row)
                rsVendorList.MoveNext()
            End While

        Finally
            If rsVendorList IsNot Nothing Then
                rsVendorList.Close()
                rsVendorList = Nothing
            End If
        End Try

        mdt.AcceptChanges()
        ugrdVendors.DataSource = mdt
        logger.Debug("RefreshGrid Exit")
    End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click Entry")
        Me.Close()
        logger.Debug("cmdExit_Click Exit")
	End Sub
	
	Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch.Click

        logger.Debug("cmdSearch_Click Entry")

        frmItemSearch.ShowDialog()
		
        If frmItemSearch.SelectedItems.Count > 0 Then
            m_lItemKey = frmItemSearch.SelectedItems.Item(0).Item_Key
            Me.ItemDescription = frmItemSearch.SelectedItems.Item(0).ItemDescription
            Me.ItemIdentifier = frmItemSearch.SelectedItems.Item(0).ItemIdentifier

            Call RefreshGrid()
        End If
		
        frmItemSearch.Close()

        logger.Debug("cmdSearch_Click Exit")

	End Sub
	
	Private Sub frmStoreItemVendors_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmStoreItemVendors_Load Entry")

        CenterForm(Me)

        SetupDataTable()
        RefreshGrid()

        logger.Debug("frmStoreItemVendors_Load Exit")
    End Sub
    Private Sub SetupDataTable()

        logger.Debug("SetupDataTable Entry")

        ' Create a data table
        mdt = New DataTable("Vendors")

        'Visible on grid.
        '--------------------
        mdt.Columns.Add(New DataColumn("CompanyName", GetType(String)))
        mdt.Columns.Add(New DataColumn("UnitCost", GetType(Double)))
        mdt.Columns.Add(New DataColumn("PrimaryVendor", GetType(String)))

        logger.Debug("SetupDataTable Exit")

    End Sub
End Class