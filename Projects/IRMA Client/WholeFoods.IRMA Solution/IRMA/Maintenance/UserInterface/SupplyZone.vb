Option Strict Off
Option Explicit On

Imports log4net
Friend Class frmSupplyZone
    Inherits System.Windows.Forms.Form

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

	
    Dim mdt As DataTable
	
	Private Sub cmbSubTeam_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbSubTeam.KeyPress

        logger.Debug("cmbSubTeam_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		If KeyAscii = 8 Then cmbSubTeam.SelectedIndex = -1
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
        End If

        logger.Debug("cmbSubTeam_KeyPress Exit")
	End Sub
	
	Private Sub cmbFromZone_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbFromZone.KeyPress

        logger.Debug("cmbFromZone_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		If KeyAscii = 8 Then cmbFromZone.SelectedIndex = -1
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
        End If

        logger.Debug("cmbFromZone_KeyPress Exit")

	End Sub
	
	Private Sub cmbToZone_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbToZone.KeyPress
        logger.Debug("cmbToZone_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		If KeyAscii = 8 Then cmbToZone.SelectedIndex = -1
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
        End If

        logger.Debug("cmbToZone_KeyPress Exit")
	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        logger.Debug("cmdExit_Click Entry")
        Me.Close()
        logger.Debug("cmdExit_Click Exit")
		
    End Sub

    Private Sub frmSupplyZone_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        logger.Debug("frmSupplyZone_FormClosing Entry")
        If Not SaveData() Then e.Cancel = 1

        logger.Debug("frmSupplyZone_FormClosing Exit")

    End Sub
	
	Private Sub frmSupplyZone_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        logger.Debug("frmSupplyZone_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		If KeyAscii = 13 Then
			RunSearch()
			KeyAscii = 0
		End If
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
        End If
        logger.Debug("frmSupplyZone_KeyPress Exit")

	End Sub
	
	Private Sub frmSupplyZone_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        logger.Debug("frmSupplyZone_Load Entry")
		'-- Center the form
		CenterForm(Me)
		
		LoadZone(cmbFromZone)
		ReplicateCombo(cmbFromZone, cmbToZone)
		LoadAllSubTeams(cmbSubTeam)
		
        ' Create a data table
        mdt = New DataTable("ZoneSupply")
        Dim Keys(3) As DataColumn
        Dim dc As DataColumn

        mdt.PrimaryKey = Keys

        dc = New DataColumn("FromZone_ID", GetType(Integer))
        mdt.Columns.Add(dc)
        Keys(0) = dc

        dc = New DataColumn("FromZone_Name", GetType(String))
        mdt.Columns.Add(dc)

        dc = New DataColumn("ToZone_ID", GetType(Integer))
        mdt.Columns.Add(dc)
        Keys(1) = dc

        dc = New DataColumn("ToZone_Name", GetType(String))
        mdt.Columns.Add(dc)

        dc = New DataColumn("SubTeam_No", GetType(Integer))
        mdt.Columns.Add(dc)
        Keys(2) = dc

        dc = New DataColumn("SubTeam_Name", GetType(String))
        mdt.Columns.Add(dc)

        dc = New DataColumn("Distribution_Markup", GetType(Decimal))
        mdt.Columns.Add(dc)

        logger.Debug("frmSupplyZone_Load Exit")

		
	End Sub
	
    Private Sub RunSearch()

        logger.Debug("RunSearch Entry")


        If Not SaveData() Then Exit Sub

        Dim rsStoreList As DAO.Recordset = Nothing
        Dim row As DataRow

        mdt.Rows.Clear()

        Try
            rsStoreList = SQLOpenRecordSet("EXEC GetZoneSupply " & ComboValue(cmbFromZone) & "," & ComboValue(cmbToZone) & "," & ComboValue(cmbSubTeam), DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            While Not rsStoreList.EOF
                row = mdt.NewRow
                row("FromZone_ID") = rsStoreList.Fields("FromZone_ID").Value
                row("FromZone_Name") = rsStoreList.Fields("FromZone_Name").Value
                row("ToZone_ID") = rsStoreList.Fields("ToZone_ID").Value
                row("ToZone_Name") = rsStoreList.Fields("ToZone_Name").Value
                row("SubTeam_No") = rsStoreList.Fields("SubTeam_No").Value
                row("SubTeam_Name") = rsStoreList.Fields("SubTeam_Name").Value
                row("Distribution_Markup") = rsStoreList.Fields("Distribution_Markup").Value
                mdt.Rows.Add(row)
                rsStoreList.MoveNext()
            End While
        Finally
            If rsStoreList IsNot Nothing Then
                rsStoreList.Close()
            End If
        End Try

        mdt.AcceptChanges()

        Me.grdDeptList.DataSource = mdt

        logger.Debug("RunSearch Exit")


    End Sub
	
    Function SaveData() As Boolean

        logger.Debug("SaveData Entry")


        SaveData = True

        If mdt Is Nothing Then Exit Function

        grdDeptList.UpdateData()

        Dim cdt As DataTable = mdt.GetChanges()

        If Not (cdt Is Nothing) Then
            Select Case MsgBox("Save these Changes?", MsgBoxStyle.Question + MsgBoxStyle.YesNoCancel + MsgBoxStyle.DefaultButton1, "Notice")
                Case MsgBoxResult.Cancel : SaveData = False
                Case MsgBoxResult.Yes
                    Dim row As DataRow
                    Dim i As Integer
                    For i = 0 To cdt.Rows.Count - 1
                        row = cdt.Rows(i)
                        SQLExecute("EXEC UpdateZoneSupply " & row("FromZone_ID") & "," & row("ToZone_ID") & ", " & row("SubTeam_No") & ", " & row("Distribution_Markup") & ", 0", DAO.RecordsetOptionEnum.dbSQLPassThrough)
                    Next
                    cdt.AcceptChanges()
            End Select
        End If

        logger.Debug("SaveData Exit")

    End Function

    'Use this for grid navigation capability.
    Private Sub grdDeptList_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles grdDeptList.KeyDown

        logger.Debug("grdDeptList_KeyDown Entry")
        Select Case e.KeyValue
            Case Keys.Up
                grdDeptList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, False, False)
                grdDeptList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.AboveCell, False, False)
                e.Handled = True
                grdDeptList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
            Case Keys.Down
                grdDeptList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, False, False)
                grdDeptList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.BelowCell, False, False)
                e.Handled = True
                grdDeptList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
            Case Keys.Right
                grdDeptList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, False, False)
                grdDeptList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.NextCellByTab, False, False)
                e.Handled = True
                grdDeptList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
            Case Keys.Left
                grdDeptList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, False, False)
                grdDeptList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.PrevCellByTab, False, False)
                e.Handled = True
                grdDeptList.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
        End Select

        logger.Debug("grdDeptList_KeyDown Exit")
    End Sub

    Private Sub UltraGrid1_AfterCellActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles UltraGrid1.AfterCellActivate

        logger.Debug("UltraGrid1_AfterCellActivate Entry")

        If UltraGrid1.ActiveCell.Column.CellActivation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit Then
            UltraGrid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
        End If

        logger.Debug("UltraGrid1_AfterCellActivate Exit")

    End Sub
End Class