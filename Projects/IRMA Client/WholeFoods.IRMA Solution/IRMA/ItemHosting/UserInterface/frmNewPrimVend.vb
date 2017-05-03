Option Strict Off
Option Explicit On

Imports log4net
Friend Class frmNewPrimVend
    Inherits System.Windows.Forms.Form

	Private m_lUnassignedItems As Integer
	Private m_lVendorID As Integer
	Private m_lItemID As Integer
    Private m_lStore_No As Integer
    Private mdtVendList As DataTable

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Public Sub New(ByRef sVendor As String, ByRef lVendorID As Integer, ByRef lItem_Key As Integer, ByRef lStore_No As Integer)
        ' This call is required by the Windows Form Designer.
        MyBase.New()
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        m_lItemID = lItem_Key
        m_lVendorID = lVendorID
        m_lStore_No = lStore_No

        Me.txtCurVend.Text = sVendor
        Call AfterNew()

    End Sub
    Public Sub New(ByRef sVendor As String, ByRef lVendorID As Integer, ByRef lItem_Key As Integer)
        ' This call is required by the Windows Form Designer.
        MyBase.New()
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        m_lItemID = lItem_Key
        m_lVendorID = lVendorID
        m_lStore_No = -1

        Me.txtCurVend.Text = sVendor
        Call AfterNew()
    End Sub
    Public Sub New(ByRef sVendor As String, ByRef lVendorID As Integer)
        ' This call is required by the Windows Form Designer.
        MyBase.New()
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        m_lVendorID = lVendorID
        m_lItemID = -1
        m_lStore_No = -1
        Me.txtCurVend.Text = sVendor
        Call AfterNew()
    End Sub
    Private Sub AfterNew()

        logger.Debug("AfterNew Entry")

        Call SetActive((Me.txtCurVend), False)

        SetupDataTable()
        '-- Set up that damn buggy sheridan grid
        'Me.grdVendList.Columns(0).Visible = False
        'Me.grdVendList.Columns(1).Caption = ResourcesItemHosting.GetString("VendorName")
        'Me.grdVendList.Columns(1).Width = 6900
        'Me.grdVendList.Columns(2).Caption = ResourcesItemHosting.GetString("ItemsToAssign")

        If m_lItemID = -1 Then
            Me.txtCurVend.Visible = True
            Me.lblVendor.Visible = True
            Me.ugrdVendList.Top = 40
            Me.ugrdVendList.Height = 232
        Else
            Me.txtCurVend.Visible = False
            Me.lblVendor.Visible = False
            Me.ugrdVendList.Top = 12
            Me.ugrdVendList.Height = 260
        End If

        Me.RefreshControls()

        Call CenterForm(Me)

        logger.Debug("AfterNew Exit")

    End Sub
    Public ReadOnly Property UnassignedItems() As Integer
        Get
            UnassignedItems = m_lUnassignedItems
        End Get
    End Property

    Private Function RefreshControls() As Boolean

        logger.Debug("RefreshControls Entry")

        Dim rsChkPrimVend As DAO.Recordset = Nothing
        Dim row As DataRow = Nothing

        Dim rsVendors As DAO.Recordset = Nothing
        Try
            If m_lItemID = -1 Then
                rsVendors = SQLOpenRecordSet("EXEC GetPrimVendItmCnt " & m_lVendorID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough + DAO.RecordsetOptionEnum.dbForwardOnly)
                If rsVendors.EOF And rsVendors.BOF Then
                    m_lUnassignedItems = 0
                Else
                    m_lUnassignedItems = rsVendors.Fields("ItmCnt").Value
                End If
                rsVendors.Close()
                rsVendors = Nothing
                Me.StatusStrip1.Items("tsslMessage").Text = String.Format(ResourcesItemHosting.GetString("NewPrimaryVendorInstructions"), m_lUnassignedItems)
            ElseIf m_lStore_No = -1 Then
                rsChkPrimVend = SQLOpenRecordSet("EXEC CheckIfPrimVendCanSwap " & glVendorID & ", " & glItemID & ", null", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbForwardOnly + DAO.RecordsetOptionEnum.dbSQLPassThrough)
                If rsChkPrimVend.Fields("IsPrimVend").Value = 1 Then
                    m_lUnassignedItems = 1
                Else
                    m_lUnassignedItems = 0
                End If
                Me.StatusStrip1.Items("tsslMessage").Text = ResourcesItemHosting.GetString("NewPrimaryVendorInstructions2")
                rsChkPrimVend.Close()

                rsChkPrimVend = Nothing
            Else
                rsChkPrimVend = SQLOpenRecordSet("EXEC CheckIfPrimVendCanSwap " & glVendorID & ", " & glItemID & ", " & m_lStore_No, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbForwardOnly + DAO.RecordsetOptionEnum.dbSQLPassThrough)
                If rsChkPrimVend.Fields("IsPrimVend").Value = 1 Then
                    m_lUnassignedItems = 1
                Else
                    m_lUnassignedItems = 0
                End If

                Me.StatusStrip1.Items("tsslMessage").Text = ResourcesItemHosting.GetString("NewPrimaryVendorInstructions3")

                rsChkPrimVend.Close()

                rsChkPrimVend = Nothing
            End If
        Finally
            If rsVendors IsNot Nothing Then
                rsVendors.Close()
                rsVendors = Nothing
            End If
            If rsChkPrimVend IsNot Nothing Then
                rsChkPrimVend.Close()
                rsChkPrimVend = Nothing
            End If
        End Try

        Try
            rsVendors = SQLOpenRecordSet("EXEC GetAvailPrimVend " & m_lVendorID & ", " & IIf(m_lItemID = -1, "null", m_lItemID) & ", " & IIf(m_lStore_No = -1, "null", m_lStore_No), DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough + DAO.RecordsetOptionEnum.dbForwardOnly)

            mdtVendList.Clear()
            Do Until rsVendors.EOF
                row = mdtVendList.NewRow
                row("Vendor_ID") = rsVendors.Fields("Vendor_ID").Value
                row("CompanyName") = rsVendors.Fields("CompanyName").Value
                row("ItmCnt") = rsVendors.Fields("ItmCnt").Value

                mdtVendList.Rows.Add(row)
                rsVendors.MoveNext()
            Loop

            mdtVendList.AcceptChanges()

            Me.ugrdVendList.DataSource = mdtVendList
            Me.ugrdVendList.DisplayLayout.Bands(0).Columns("ItmCnt").Hidden = (m_lItemID <> -1)
            If Me.ugrdVendList.Rows.Count > 0 Then Me.ugrdVendList.Rows(0).Selected = True
        Finally
            If rsVendors IsNot Nothing Then
                rsVendors.Close()
                rsVendors = Nothing
            End If
        End Try

        logger.Debug("RefreshControls Exit")

    End Function
	
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		Me.Hide()
	End Sub
	
	Private Sub cmdItems_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdItems.Click

        logger.Debug("cmdItems_Click Entry")

        If ugrdVendList.Selected.Rows.Count = 1 Then
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
            Dim newVendorID As Integer = CInt(ugrdVendList.Selected.Rows(0).Cells("Vendor_ID").Value)
            Dim fVendorItems As New frmVendorItems(newVendorID, ugrdVendList.Selected.Rows(0).Cells("CompanyName").Value.ToString(), String.Format("EXEC GetAvailPrimVendDetail {0}, {1}, null", newVendorID, m_lVendorID))
            fVendorItems.Text = ResourcesItemHosting.GetString("ChangePrimaryVendor")
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
            fVendorItems.ShowDialog()
            fVendorItems.Close()
            fVendorItems.Dispose()
        Else
            Call MsgBox(ResourcesItemHosting.GetString("SelectVendor"), MsgBoxStyle.Critical, Me.Text)
            logger.Info(ResourcesItemHosting.GetString("SelectVendor"))
        End If

        logger.Debug("cmdItems_Click Exit")
    End Sub
	
	Private Sub cmdSubmit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSubmit.Click

        logger.Debug("cmdSubmit_Click Entry")

        If ugrdVendList.Selected.Rows.Count <> 1 Then
            Call MsgBox(ResourcesItemHosting.GetString("SelectVendorAsPrimary"), MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If

        If m_lItemID = -1 Then
            Call SQLExecute("EXEC SwitchPrimaryVendor " & m_lVendorID & ", " & ugrdVendList.Selected.Rows(0).Cells("Vendor_ID").Value.ToString & ", null" & ", null", DAO.RecordsetOptionEnum.dbSQLPassThrough)
        ElseIf m_lStore_No = -1 Then
            Call SQLExecute("EXEC SwitchPrimaryVendor " & m_lVendorID & ", " & ugrdVendList.Selected.Rows(0).Cells("Vendor_ID").Value.ToString & ", " & m_lItemID & ", null", DAO.RecordsetOptionEnum.dbSQLPassThrough)
        Else
            Call SQLExecute("EXEC SwitchPrimaryVendor " & m_lVendorID & ", " & ugrdVendList.Selected.Rows(0).Cells("Vendor_ID").Value.ToString & ", " & m_lItemID & ", " & m_lStore_No, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        End If

        Call RefreshControls()

        If m_lUnassignedItems = 0 Then Me.Hide()

        logger.Debug("cmdSubmit_Click Exit")
	End Sub
	
	Private Sub frmNewPrimVend_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
    End Sub

    Private Sub SetupDataTable()

        logger.Debug("SetupDataTable Entry")

        mdtVendList = New DataTable("VendorList")

        mdtVendList.Columns.Add(New DataColumn("Vendor_ID", GetType(Integer)))
        mdtVendList.Columns.Add(New DataColumn("CompanyName", GetType(String)))
        mdtVendList.Columns.Add(New DataColumn("ItmCnt", GetType(Integer)))

        logger.Debug("SetupDataTable Exit")

    End Sub
End Class