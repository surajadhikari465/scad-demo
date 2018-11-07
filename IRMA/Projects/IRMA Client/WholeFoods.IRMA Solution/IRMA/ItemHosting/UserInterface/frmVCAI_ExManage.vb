Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports log4net
Friend Class frmVCAI_ExManage
    Inherits System.Windows.Forms.Form
    Private IsInitializing As Boolean
    'datagrid
    Private mdt As DataTable
    Private mdv As DataView

	Private m_colChanges As New Collection
    Private m_sStoreList As String
    Private m_sUniversalDateFormat As String = ResourcesIRMA.GetString("YearDateFormat")
    'Private m_bGoodCreate As Boolean
	Private m_lVendorID As Integer
	Private m_sTeamList As String
	Private m_iChangedCnt As Short
    Private Const APP_ID As Short = 4

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

	'column const
    'Private Const VCAI_EX_ID As Short = 0
    'Private Const PRIORITY As Short = 1
    'Private Const Item_Key As Short = 2
    'Private Const MSRP As Short = 3
    'Private Const Identifier As Short = 4
    'Private Const Item_Description As Short = 5
    'Private Const CHANGE_TYPE As Short = 6
    'Private Const LAST_PACK As Short = 7
    'Private Const NEW_PACK As Short = 8
    'Private Const LAST_COST As Short = 9
    'Private Const NEW_COST As Short = 10
    'Private Const START_DATE As Short = 11
    'Private Const END_DATE As Short = 12
    Public Sub New(ByRef sTeamList As String, ByRef Vendor_ID As Integer)

        logger.Debug("Entry(New)")

        ' This call is required by the Windows Form Designer.
        IsInitializing = True
        InitializeComponent()
        IsInitializing = False

        ' Add any initialization after the InitializeComponent() call.
        m_lVendorID = Vendor_ID
        m_sTeamList = sTeamList
        Call LoadSubTeam((Me.cmbSubTeam), sTeamList)
        Me.cmbSubTeam.SelectedIndex = -1

        Call LoadExType((Me.cmbExType), APP_ID)
        Me.cmbExType.SelectedIndex = -1
        Me.cmbSeverity.SelectedIndex = -1
        'format grid
        Call SetupDataTable()
        logger.Debug("Exit")

    End Sub
    Private Sub SetupDataTable()

        logger.Debug("SetupDataTable Entry")


        ' Create a data table
        mdt = New DataTable("Exceptions")

        'Visible on grid.
        '--------------------
        mdt.Columns.Add(New DataColumn("Status", GetType(String)))
        mdt.Columns.Add(New DataColumn("Identifier", GetType(String)))
        mdt.Columns.Add(New DataColumn("Item_Description", GetType(String)))
        mdt.Columns.Add(New DataColumn("ChangeType", GetType(String)))
        mdt.Columns.Add(New DataColumn("LastPackSize", GetType(Double)))
        mdt.Columns.Add(New DataColumn("NewPackSize", GetType(Double)))
        mdt.Columns.Add(New DataColumn("LastUnitCost", GetType(Double)))
        mdt.Columns.Add(New DataColumn("NewUnitCost", GetType(Double)))
        mdt.Columns.Add(New DataColumn("Start_Date", GetType(Date)))
        mdt.Columns.Add(New DataColumn("End_Date", GetType(Date)))

        'Hidden.
        '--------------------
        mdt.Columns.Add(New DataColumn("Item_Key", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("MSRP", GetType(String)))
        mdt.Columns.Add(New DataColumn("VCAI_ExID", GetType(Integer)))

        logger.Debug("SetupDataTable Exit")


    End Sub


    '   Public Property TeamList() As String
    '       Get
    '           TeamList = m_sTeamList
    '       End Get
    '       Set(ByVal Value As String)
    '           m_sTeamList = Value
    '       End Set
    '   End Property
    'Public Property VendorID() As Integer
    '	Get
    '		VendorID = m_lVendorID
    '	End Get
    '	Set(ByVal Value As Integer)
    '		m_lVendorID = Value
    '	End Set
    'End Property
	

    Private Sub cmbExType_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbExType.SelectedIndexChanged
        If Me.IsInitializing = True Then Exit Sub

        logger.Debug("cmbExType_SelectedIndexChanged Entry")
        If cmbExType.SelectedIndex <> -1 Then
            Call LoadExSeverity(cmbSeverity, APP_ID, VB6.GetItemData(cmbExType, cmbExType.SelectedIndex))
            If cmbSeverity.Items.Count > 0 Then cmbSeverity.SelectedIndex = 0
        End If
        logger.Debug("cmbExType_SelectedIndexChanged Exit")

    End Sub
	
	Private Sub cmbSubTeam_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbSubTeam.SelectedIndexChanged
        If Me.IsInitializing = True Then Exit Sub

        logger.Debug("cmbSubTeam_SelectedIndexChanged Entry")
        If cmbExType.Items.Count > 0 Then cmbExType.SelectedIndex = 0
        logger.Debug("cmbSubTeam_SelectedIndexChanged Exit")
    End Sub
	
	
	
	Private Sub cmdAplyGridValues_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAplyGridValues.Click

        logger.Debug("cmdAplyGridValues_Click Entry")
        Dim iCnt As Short
		Dim bHasChanged As Boolean
        Dim sParams As String

        If ugrdVCAI_Exceptions.Selected.Rows.Count > 0 Then
            Dim sStartDate As String = String.Empty
            Dim sEndDate As String = String.Empty

            If dtpStartDate.Value IsNot Nothing Then
                sStartDate = CDate(dtpStartDate.Value).ToString(m_sUniversalDateFormat)
            End If

            If dtpEndDate.Value IsNot Nothing Then
                sEndDate = CDate(dtpEndDate.Value).ToString(m_sUniversalDateFormat)
            End If

            For iCnt = 0 To ugrdVCAI_Exceptions.Selected.Rows.Count - 1
                bHasChanged = False

                sParams = ugrdVCAI_Exceptions.Selected.Rows(iCnt).Cells("VCAI_ExID").Value & ", "
                If IsNumeric(txtNewUnitCost.Text) AndAlso txtNewUnitCost.Text <> ugrdVCAI_Exceptions.Selected.Rows(iCnt).Cells("NewUnitCost").Value Then
                    bHasChanged = True
                    sParams = sParams & txtNewUnitCost.Text & ", "
                Else
                    sParams = sParams & "null, "
                End If
                If IsNumeric(txtNewPackSize.Text) AndAlso txtNewPackSize.Text <> ugrdVCAI_Exceptions.Selected.Rows(iCnt).Cells("NewPackSize").Value Then
                    bHasChanged = True
                    sParams = sParams & txtNewPackSize.Text & ", "
                Else
                    sParams = sParams & "null, "
                End If

                If IsDate(sStartDate) AndAlso sStartDate <> CDate(ugrdVCAI_Exceptions.Selected.Rows(iCnt).Cells("Start_Date").Value).ToString(m_sUniversalDateFormat) Then
                    bHasChanged = True
                    sParams = sParams & "'" & sStartDate & "', "
                Else
                    sParams = sParams & "null, "
                End If
                If IsDate(sEndDate) AndAlso sEndDate <> CDate(ugrdVCAI_Exceptions.Selected.Rows(iCnt).Cells("End_Date").Value).ToString(m_sUniversalDateFormat) Then
                    bHasChanged = True
                    sParams = sParams & "'" & sEndDate & "', "
                Else
                    sParams = sParams & "null, "
                End If

                sParams = sParams & "null, "
                sParams = sParams & "0, "
                sParams = sParams & giUserID
                If bHasChanged Then
                    Call SQLExecute("Exec UpdateVCAI_Exception " & sParams, DAO.RecordsetOptionEnum.dbSQLPassThrough, True)
                End If
            Next iCnt
            Call cmdSearch_Click(cmdSearch, New System.EventArgs())
        End If
        logger.Debug("cmdAplyGridValues_Click Exit")
		
	End Sub
	
	Private Sub cmdApply_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdApply.Click

        logger.Debug("cmdApply_Click Entry")

		Dim iCnt As Short
		
		
        If ugrdVCAI_Exceptions.Selected.Rows.Count > 0 Then
            For iCnt = 0 To ugrdVCAI_Exceptions.Selected.Rows.Count - 1
                Call ApplyChanges(ugrdVCAI_Exceptions.Selected.Rows(iCnt))
            Next iCnt
            Call cmdSearch_Click(cmdSearch, New System.EventArgs())
        Else
            MsgBox(ResourcesItemHosting.GetString("SelectVendorCost"))
            logger.Info(ResourcesItemHosting.GetString("SelectVendorCost"))
        End If

        logger.Debug("cmdApply_Click Exit")
	End Sub
	
    Private Sub ApplyChanges(ByVal oRow As Infragistics.Win.UltraWinGrid.UltraGridRow)

        logger.Debug("ApplyChanges Entry")

        Dim sParams As String

        Call GetStoreList(oRow.Cells("Item_Key").Value)
        sParams = "'" & m_sStoreList & "', "
        sParams = sParams & "'|'" & ", "
        sParams = sParams & oRow.Cells("Item_Key").Value & ", "
        sParams = sParams & m_lVendorID & ", "
        sParams = sParams & oRow.Cells("NewUnitCost").Value & ", "
        sParams = sParams & "null, " 'freight
        sParams = sParams & oRow.Cells("NewPackSize").Value & ", "
        sParams = sParams & "'" & CDate(oRow.Cells("Start_Date").Value).ToString(m_sUniversalDateFormat) & "', "
        sParams = sParams & "'" & CDate(oRow.Cells("End_Date").Value).ToString(m_sUniversalDateFormat) & "', "
        sParams = sParams & IIf(oRow.Cells("ChangeType").Value = "P", "1", "0") & ", "
        If IsDBNull(oRow.Cells("MSRP").Value) Then
            sParams = sParams & "null, "
        Else
            sParams = sParams & oRow.Cells("MSRP").Value & ", "
        End If

        'sParams = sParams & IIf(oRow.Cells("MSRP").Value = "", "null", oRow.Cells("MSRP").Value) & ", "
        sParams = sParams & 1 & ", " 'From Vendor
        sParams = sParams & IIf(InStr(1, oRow.Cells("Status").Value, "+") > 0, giUserID, "null")

        gRSRecordset = SQLOpenRecordSet("EXEC InsertVendorCostHistory3 " & sParams, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        'set the state to "handled"
        sParams = oRow.Cells("VCAI_ExID").Value & ", "
        sParams = sParams & "null, null, null, null, "
        sParams = sParams & gRSRecordset.Fields("VCH_ID").Value & ", "
        sParams = sParams & "1, "
        sParams = sParams & giUserID
        Call SQLExecute("EXEC UpdateVCAI_Exception " & sParams, DAO.RecordsetOptionEnum.dbSQLPassThrough, True)
        gRSRecordset.Close()
        gRSRecordset = Nothing

        On Error Resume Next
        Call m_colChanges.Remove(CStr(oRow.Cells("VCAI_ExID").Value))
        On Error GoTo 0

        logger.Debug("ApplyChanges Exit")

    End Sub
	Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click

        logger.Debug("cmdDelete_Click Entry")

        Dim sParams As Object
        Dim iCnt As Short
        Dim iselCount As Short

        iselCount = ugrdVCAI_Exceptions.Selected.Rows.Count - 1
        If iselCount > -1 Then
            For iCnt = 0 To iselCount
                sParams = ugrdVCAI_Exceptions.Selected.Rows(iCnt).Cells("VCAI_ExID").Value & ", "
                sParams = sParams & "null, null, null, null, null, "
                sParams = sParams & "-1, "
                sParams = sParams & giUserID
                Call SQLExecute("EXEC UpdateVCAI_Exception " & sParams, DAO.RecordsetOptionEnum.dbSQLPassThrough, True)
                On Error Resume Next
                Call m_colChanges.Remove(CStr(ugrdVCAI_Exceptions.Selected.Rows(iCnt).Cells("VCAI_ExID").Value))
                On Error GoTo 0
            Next iCnt
            Call cmdSearch_Click(cmdSearch, New System.EventArgs())
        Else
            MsgBox(ResourcesItemHosting.GetString("SelectExceptionDelete"))

            logger.Info(ResourcesItemHosting.GetString("SelectExceptionDelete"))
        End If

        logger.Debug("cmdDelete_Click Exit")
	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		Me.Close()
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
        logger.Debug("cmdReport_Click Entry")

        Dim fExReport As New frmVCAI_ExReport(m_sTeamList, m_lVendorID)
		fExReport.ShowDialog()
        fExReport.Dispose()

        logger.Debug("cmdReport_Click Exit")
	End Sub
	
	Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch.Click

        logger.Debug("cmdSearch_Click Entry")

        'get exceptions matching the filter selections
        Dim rsSearch As DAO.Recordset
        Dim row As DataRow
        Dim iLoop As Integer

        Dim sParams As String
		Dim iCnt As Short
		Dim sAddString As String
		Dim bIsUserChange, bHasPriority As Boolean
		
		If cmbSubTeam.SelectedIndex = -1 Then
            MsgBox(ResourcesIRMA.GetString("EnterSubTeam"))
			Exit Sub
		ElseIf cmbExType.SelectedIndex = -1 Then 
            MsgBox(ResourcesItemHosting.GetString("EnterExceptionType"))
            logger.Info(ResourcesItemHosting.GetString("EnterExceptionType"))
            logger.Debug("cmdSearch_Click Exit")
			Exit Sub
			'ElseIf cmbSeverity.ListIndex = -1 Then
			'    MsgBox "You must enter a Severity Level."
			'    Exit Sub
		End If
		
        dtpStartDate.Value = Nothing
        dtpEndDate.Value = Nothing

        txtNewPackSize.Text = ""
        txtNewUnitCost.Text = ""
		
		'SubTeam
		sParams = CStr(VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
		
		'ExType
		sParams = sParams & ", " & VB6.GetItemData(cmbExType, cmbExType.SelectedIndex)
		
		'ExSeverity
        If cmbSeverity.SelectedIndex = -1 Then
            sParams = sParams & ", Null"
        Else
            sParams = sParams & ", " & cmbSeverity.SelectedItem
        End If
		
		'VendorID
        sParams = sParams & ", " & m_lVendorID

		Dim dtCurDate As Date
        dtCurDate = SystemDateTime()

        'gRSRecordset = SQLOpenRecordSet("EXEC GetVCAI_Exceptions " & sParams, dao.RecordsetTypeEnum.dbOpenSnapshot, dao.RecordsetOptionEnum.dbSQLPassThrough)
        rsSearch = SQLOpenRecordSet("EXEC GetVCAI_Exceptions " & sParams, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        bIsUserChange = False

        'Load the data set.
        mdt.Rows.Clear()

        While (Not rsSearch.EOF)
            iLoop = iLoop + 1

            bHasPriority = False
            bIsUserChange = False
            If IsDBNull(rsSearch.Fields("UserSTART_DATE").Value) Then
                If rsSearch.Fields("START_DATE").Value <= DateAdd(Microsoft.VisualBasic.DateInterval.Day, 1, dtCurDate) Then
                    bHasPriority = True
                Else
                    bHasPriority = False
                End If
            Else
                If rsSearch.Fields("UserSTART_DATE").Value <= DateAdd(Microsoft.VisualBasic.DateInterval.Day, 1, dtCurDate) Then
                    bHasPriority = True
                Else
                    bHasPriority = False
                End If
            End If

            row = mdt.NewRow
            row("Item_Key") = rsSearch.Fields("Item_Key").Value
            row("MSRP") = rsSearch.Fields("MSRP").Value
            row("Identifier") = rsSearch.Fields("Identifier").Value
            row("Item_Description") = rsSearch.Fields("Item_Description").Value
            row("ChangeType") = rsSearch.Fields("ChangeType").Value
            row("LastPackSize") = rsSearch.Fields("LastPackSize").Value
            If IsDBNull(rsSearch.Fields("UserNewPackSize").Value) Then
                row("NewPackSize") = rsSearch.Fields("newpacksize").Value
            Else
                row("NewPackSize") = rsSearch.Fields("UserNewPackSize").Value
                bIsUserChange = True
            End If

            row("LastUnitCost") = rsSearch.Fields("LastUnitCost").Value
            If IsDBNull(rsSearch.Fields("UserNewUnitCost").Value) Then
                row("NewUnitCost") = rsSearch.Fields("newunitcost").Value
            Else
                row("NewUnitCost") = rsSearch.Fields("UserNewUnitCost").Value
                bIsUserChange = True
            End If

            If IsDBNull(rsSearch.Fields("UserSTART_DATE").Value) Then
                row("Start_Date") = rsSearch.Fields("START_DATE").Value
            Else
                row("Start_Date") = rsSearch.Fields("UserSTART_DATE").Value
                bIsUserChange = True
            End If

            If IsDBNull(rsSearch.Fields("UserEND_DATE").Value) Then
                row("End_Date") = rsSearch.Fields("END_DATE").Value
            Else
                row("End_Date") = rsSearch.Fields("UserEND_DATE").Value
                bIsUserChange = True
            End If

            row("VCAI_ExID") = rsSearch.Fields("VCAI_ExID").Value
            If bHasPriority Then
                If bIsUserChange Then
                    row("Status") = "*+"
                Else
                    row("Status") = "*"
                End If
            Else
                If bIsUserChange Then
                    row("Status") = "+"
                Else
                    row("Status") = ""
                End If
            End If

            mdt.Rows.Add(row)

            rsSearch.MoveNext()
        End While

        'Setup a column that you would like to sort on initially.
        mdt.AcceptChanges()
        mdv = New System.Data.DataView(mdt)
        'mdv.Sort = "Description"
        ugrdVCAI_Exceptions.DataSource = mdv

        For iLoop = 0 To ugrdVCAI_Exceptions.Rows.Count - 1
            'if the status contains a '+' then the user has modified the record at some point so it needs to be in the changes collection
            'this is legacey code from the old grid and can be handled better now...when we have the time.
            If InStr(ugrdVCAI_Exceptions.Rows(iLoop).Cells("Status").Value, "+") > 0 Then
                On Error Resume Next
                Call m_colChanges.Add(ugrdVCAI_Exceptions.Rows(iLoop), CStr(ugrdVCAI_Exceptions.Rows(iLoop).Cells("VCAI_ExID").Value))
                On Error GoTo 0
            End If
        Next


        'This may or may not be required.
        If rsSearch.RecordCount = 0 Then
            MsgBox("No items found.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
            logger.Info("No items found.")
        End If

        rsSearch.Close()
        rsSearch = Nothing

        logger.Debug("cmdSearch_Click Exit")

ExitSub:
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        If rsSearch IsNot Nothing Then
            rsSearch.Close()
            rsSearch = Nothing
        End If

        logger.Debug("cmdSearch_Click Exit from ExitSub:")
    End Sub

    Private Sub frmVCAI_ExManage_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        logger.Debug("frmVCAI_ExManage_Load Entry")
        CenterForm(Me)
        m_iChangedCnt = 0
        logger.Debug("frmVCAI_ExManage_Load Exit")

    End Sub
    Private Sub GetStoreList(ByRef lItem_Key As Integer)

        logger.Debug("GetStoreList Entry with lItem_Key= " + lItem_Key.ToString)

        Try
            gRSRecordset = SQLOpenRecordSet("EXEC GetCurrentVendorStores " & m_lVendorID & ", " & lItem_Key, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            If gRSRecordset.EOF <> True And gRSRecordset.BOF <> True Then
                m_sStoreList = ""
                Do Until gRSRecordset.EOF
                    m_sStoreList = m_sStoreList & gRSRecordset.Fields("Store_No").Value & "|"
                    gRSRecordset.MoveNext()
                Loop
                m_sStoreList = VB.Left(m_sStoreList, Len(m_sStoreList) - 1)
            End If
        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
        End Try
        logger.Debug("GetStoreList Exit")

    End Sub

	
	Private Sub frmVCAI_ExManage_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        logger.Debug("frmVCAI_ExManage_FormClosing Entry")

        Dim Cancel As Boolean = eventArgs.Cancel
		Dim UnloadMode As System.Windows.Forms.CloseReason = eventArgs.CloseReason
		Dim iAns As Short
        Dim oRow As Infragistics.Win.UltraWinGrid.UltraGridRow
		
		If m_colChanges.Count() > 0 Then
            iAns = MsgBox(ResourcesIRMA.GetString("SaveChanges"), MsgBoxStyle.Information + MsgBoxStyle.YesNoCancel, Me.Text)
			If iAns = MsgBoxResult.Yes Then
                For Each oRow In Me.ugrdVCAI_Exceptions.Rows 'm_colChanges
                    If InStr(oRow.Cells("Status").Value, "+") > 0 Then
                        Call ApplyChanges(oRow)
                    End If
                Next oRow
			ElseIf iAns = MsgBoxResult.No Then 
                'For Each oRow In m_colChanges
                'Call SQLExecute("EXEC UndoVCAI_Exception " & oRow.Cells("VCAI_ExID").Value, DAO.RecordsetOptionEnum.dbSQLPassThrough, True)
                'Next oRow
			Else
				Cancel = True
			End If
		End If
        eventArgs.Cancel = Cancel

        logger.Debug("frmVCAI_ExManage_FormClosing Exit")
	End Sub
	
    Private Sub ugrdVCAI_Exceptions_AfterSelectChange(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs) Handles ugrdVCAI_Exceptions.AfterSelectChange
        logger.Debug("ugrdVCAI_Exceptions_AfterSelectChange Entry")

        If ugrdVCAI_Exceptions.Selected.Rows.Count = 1 Then
            txtNewUnitCost.Text = ugrdVCAI_Exceptions.Selected.Rows(0).Cells("NewUnitCost").Value
            txtNewPackSize.Text = ugrdVCAI_Exceptions.Selected.Rows(0).Cells("NewPackSize").Value
            dtpStartDate.Value = ugrdVCAI_Exceptions.Selected.Rows(0).Cells("Start_Date").Value
            dtpEndDate.Value = ugrdVCAI_Exceptions.Selected.Rows(0).Cells("End_Date").Value

            'txtDate(0).Text = ugrdVCAI_Exceptions.Selected.Rows(0).Cells("Start_Date").Value
            'txtDate(1).Text = ugrdVCAI_Exceptions.Selected.Rows(0).Cells("End_Date").Value
        Else
            txtNewUnitCost.Text = ""
            txtNewPackSize.Text = ""
            'txtDate(0).Text = ""
            'txtDate(1).Text = ""
            dtpStartDate.Value = Nothing
            dtpEndDate.Value = Nothing
        End If

        logger.Debug("ugrdVCAI_Exceptions_AfterSelectChange Exit")

    End Sub
End Class