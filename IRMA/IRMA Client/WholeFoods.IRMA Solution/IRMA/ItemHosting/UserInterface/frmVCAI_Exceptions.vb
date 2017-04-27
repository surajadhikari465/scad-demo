Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic

Imports log4net

Friend Class frmVCAI_Exceptions
	Inherits System.Windows.Forms.Form
	Private m_sTeamList As String
    Private m_iUserTeam_No As Short

    Private mdt As DataTable
    Private mdv As DataView
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		Me.Close()
    End Sub
    Private Sub SetupDataTable()

        logger.Debug("SetupDataTable Entry")
        ' Create a data table
        mdt = New DataTable("VendExceptions")

        'Visible on grid.
        '--------------------
        mdt.Columns.Add(New DataColumn("CompanyName", GetType(String)))
        mdt.Columns.Add(New DataColumn("ExceptionCount", GetType(Integer)))

        'Hidden.
        '--------------------
        mdt.Columns.Add(New DataColumn("Vendor_ID", GetType(Integer)))

        logger.Debug("SetupDataTable Exit")

    End Sub
    Private Sub LoadDataTable(ByVal sSearchSQL As String)

        logger.Debug("LoadDataTable Entry")

        Dim rsSearch As DAO.Recordset = Nothing
        Dim row As DataRow
        Dim iLoop As Integer

        Try
            rsSearch = SQLOpenRecordSet(sSearchSQL, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            'Load the data set.
            mdt.Rows.Clear()

            While (Not rsSearch.EOF)
                iLoop = iLoop + 1

                row = mdt.NewRow
                row("Vendor_ID") = rsSearch.Fields("Vendor_ID").Value
                row("CompanyName") = rsSearch.Fields("CompanyName").Value
                row("ExceptionCount") = rsSearch.Fields("ExceptionCount").Value
                mdt.Rows.Add(row)

                rsSearch.MoveNext()
            End While

            'Setup a column that you would like to sort on initially.
            mdt.AcceptChanges()
            mdv = New System.Data.DataView(mdt)
            mdv.Sort = "CompanyName"
            ugrdVCAI_Exceptions.DataSource = mdv


            'This may or may not be required.
            If rsSearch.RecordCount > 0 Then
                'Set the first item to selected.
                ugrdVCAI_Exceptions.Rows(0).Selected = True
                ugrdVCAI_Exceptions.ActiveRow = ugrdVCAI_Exceptions.Rows(0)
            Else
                MsgBox(ResourcesIRMA.GetString("NoneFound"), MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)

                logger.Debug(ResourcesIRMA.GetString("NoneFound"))
            End If

        Finally
            If rsSearch IsNot Nothing Then
                rsSearch.Close()
                rsSearch = Nothing
            End If
        End Try

        logger.Debug("LoadDataTable Exit")

ExitSub:
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        logger.Debug("LoadDataTable Exit from ExitSub:")

    End Sub
	
    Private Sub cmdSelect_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSelect.Click


        logger.Debug("cmdSelect_Click Entry")

        Dim iRowTot As Short

        iRowTot = Me.ugrdVCAI_Exceptions.Selected.Rows.Count
        If iRowTot <= 0 Then
            MsgBox(ResourcesItemHosting.GetString("SelectVendorManage"), MsgBoxStyle.Exclamation, Me.Text)
            Exit Sub
        End If

        Dim fVCAI_ManageEx As New frmVCAI_ExManage(m_sTeamList, ugrdVCAI_Exceptions.Selected.Rows(0).Cells("Vendor_ID").Value)
        fVCAI_ManageEx.ShowDialog()
        fVCAI_ManageEx.Dispose()

        Call LoadDataTable("GetVCAI_ExceptionsCount " & "'" & m_sTeamList & "'")

        logger.Debug("cmdSelect_Click Exit")


    End Sub
	
	Private Sub frmVCAI_Exceptions_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        logger.Debug("frmVCAI_Exceptions_Load Entry")

        Dim rsTeams As DAO.Recordset = Nothing
        Call CenterForm(Me)

        Call SetupDataTable()

        Try
            'get all teams user is associated with
            rsTeams = SQLOpenRecordSet("GetUserStoreTeam_ByUserTitle " & giUserID & ", 12", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            If rsTeams.EOF <> True And rsTeams.BOF <> True Then
                m_sTeamList = ""
                Do Until rsTeams.EOF
                    m_sTeamList = m_sTeamList & rsTeams.Fields("Team_No").Value & "|"
                    rsTeams.MoveNext()
                Loop
                If Len(m_sTeamList) > 0 Then
                    m_sTeamList = VB.Left(m_sTeamList, Len(m_sTeamList) - 1)
                End If
                Call LoadDataTable("GetVCAI_ExceptionsCount " & "'" & m_sTeamList & "'")

            Else
                MsgBox(ResourcesItemHosting.GetString("UserNoTeams"))
            End If
            Me.Text = Replace(Me.Text, "[" + ResourcesItemHosting.GetString("Team") + "]", ResourcesItemHosting.GetString("Team") + ":  " & Replace(m_sTeamList, "|", ", "))
        Finally
            If rsTeams IsNot Nothing Then
                rsTeams.Close()
                rsTeams = Nothing
            End If
        End Try

        logger.Debug("frmVCAI_Exceptions_Load Exit")

    End Sub

    Private Sub ugrdVCAI_Exceptions_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles ugrdVCAI_Exceptions.DoubleClickRow

        logger.Debug("ugrdVCAI_Exceptions_DoubleClickRow Entry")
        Call cmdSelect_Click(cmdSelect, New System.EventArgs())
        logger.Debug("ugrdVCAI_Exceptions_DoubleClickRow Exit")

    End Sub
End Class