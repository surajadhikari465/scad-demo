Option Strict Off
Option Explicit On

Imports log4net

Friend Class frmZoneDistributionEdit
	Inherits System.Windows.Forms.Form
    Private IsInitializing As Boolean
	Private mlToZoneID As Integer
	Private mbChanged As Boolean
	
	Private Const mcClickToAdd As String = "--- Click 'Add' to add this SubTeam / Supplier ---"
	Private Const mcCannotAdd As String = "--- Selected SubTeam / Supplier is already in the list ---"
	
    Private msCurrSelections(0, 0) As String

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

	
	Public Function LoadForm(ByRef sToZone As String, ByRef lToZoneID As Integer, ByRef vSubTeams As Object, ByRef vSuppliers As Object, ByRef vCurrSelections As Object) As Boolean

        logger.Debug("LoadForm Entry")

        Dim i As Short
        Dim iNewIndex As Integer
		
		mbChanged = False
		mlToZoneID = lToZoneID
		txtToZone.Text = sToZone
		lblMsg.Text = ""
		
		'Load SubTeams.
        '---------------
        cmbSubTeam.Items.Clear()
		For i = 0 To UBound(vSubTeams, 2)
			If Not AlreadyInList(vSubTeams(1, i)) Then
				cmbSubTeam.Items.Add(New VB6.ListBoxItem((vSubTeams(1, i)), vSubTeams(0, i)))
			End If
		Next i
		cmbSubTeam.SelectedIndex = -1
		
		'Load Suppliers (Stores).
		'-----------------------
        'iItem = 0
        cmbSupplier.Items.Clear()
		For i = 0 To UBound(vSuppliers, 2)
            iNewIndex = cmbSupplier.Items.Add((vSuppliers(1, i)))
			VB6.SetItemData(cmbSupplier, iNewIndex, vSuppliers(0, i))
		Next i
		cmbSupplier.SelectedIndex = -1
		
		msCurrSelections = vCurrSelections
		
		Me.ShowDialog()
		
        LoadForm = mbChanged

        logger.Debug("LoadForm Exit")
		
	End Function
	
    Private Function AlreadyInList(ByRef vSubTeam As Object) As Boolean

        logger.Debug("AlreadyInList Entry")
        Dim i As Short

        AlreadyInList = False

        For i = 0 To cmbSubTeam.Items.Count - 1
            If VB6.GetItemString(cmbSubTeam, i) = vSubTeam Then
                AlreadyInList = True
                Exit For
            End If
        Next i

        logger.Debug("AlreadyInList Exit")



    End Function
	Private Sub cmbSubTeam_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbSubTeam.SelectedIndexChanged

        logger.Debug("cmbSubTeam_SelectedIndexChanged Entry")
        If IsInitializing = True Then Exit Sub
        Call CheckInList()

        logger.Debug("cmbSubTeam_SelectedIndexChanged Exit")


    End Sub
	
	Private Sub cmbSupplier_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbSupplier.SelectedIndexChanged
        If IsInitializing = True Then Exit Sub
        Call CheckInList()

    End Sub
	
	Private Sub CheckInList()
        logger.Debug("CheckInList Entry")
		Dim i As Short
		Dim lSelectedSubTeamID As Integer
		Dim lSelectedSupplierID As Integer
		
		On Error GoTo NotFound
		
		lblMsg.Text = ""
		cmdAdd.Enabled = True
		
		If cmbSubTeam.SelectedIndex <> -1 And cmbSupplier.SelectedIndex <> -1 Then
			
			lblMsg.Text = mcClickToAdd
			lblMsg.ForeColor = System.Drawing.ColorTranslator.FromOle(&HC00000) 'Blue
			
			lSelectedSubTeamID = VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex)
			lSelectedSupplierID = VB6.GetItemData(cmbSupplier, cmbSupplier.SelectedIndex)
			
			For i = 0 To UBound(msCurrSelections, 2)
				If (CDbl(msCurrSelections(0, i)) = lSelectedSubTeamID) And (CDbl(msCurrSelections(1, i)) = lSelectedSupplierID) Then
					lblMsg.Text = mcCannotAdd
					lblMsg.ForeColor = System.Drawing.ColorTranslator.FromOle(&HC0)
					cmdAdd.Enabled = False
					Exit For
				End If
			Next i
        End If

        logger.Debug("CheckInList Exit")
		
NotFound:
        logger.Debug("CheckInList exit from NotFound:")
		
	End Sub
	
	Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click

        logger.Debug("cmdAdd_Click Entry")

		Dim iCnt As Short
		
		'Add the SubTeam/Supplier to the Zone.
		'--------------------------------------
		SQLExecute("EXEC InsertZoneSubTeam " & mlToZoneID & "," & VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex) & ", " & VB6.GetItemData(Me.cmbSupplier, Me.cmbSupplier.SelectedIndex), dao.RecordsetOptionEnum.dbSQLPassThrough)
		
		lblMsg.Text = "--- Added '" & VB6.GetItemString(cmbSubTeam, cmbSubTeam.SelectedIndex) & " - " & VB6.GetItemString(cmbSupplier, Me.cmbSupplier.SelectedIndex) & "' ---"
		lblMsg.ForeColor = System.Drawing.ColorTranslator.FromOle(&H8000) 'Green
		
		cmdAdd.Enabled = False
		
		'Add the newly added SubTeam / Supplier to the current selection list.
		'--------------------------------------------------------------------
		iCnt = 0
		
		'Yes, I know, goto's suck... but so does trying to determine if an array has been initialized.
		On Error GoTo InitArray
        iCnt = UBound(msCurrSelections, 2)

        logger.Debug("cmdAdd_Click Exit")
		
AddToArray: 
		ReDim Preserve msCurrSelections(1, iCnt + 1)
		GoTo Continue_Renamed
		
InitArray: 
		ReDim msCurrSelections(1, 0)
		
Continue_Renamed: 
		On Error GoTo GetOutaHere
		msCurrSelections(0, UBound(msCurrSelections, 2)) = CStr(VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
		msCurrSelections(1, UBound(msCurrSelections, 2)) = CStr(VB6.GetItemData(cmbSupplier, cmbSupplier.SelectedIndex))
		
		cmbSupplier.Focus()
		
		mbChanged = True
		
GetOutaHere: 
		
	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click Entry")

        Dim intAns As Short
		
		If lblMsg.Text = mcClickToAdd Then
			
			intAns = MsgBox("Add the selected SubTeam / Supplier?", MsgBoxStyle.YesNo, "Add before exiting?")
			
			If intAns = MsgBoxResult.Yes Then Call cmdAdd_Click(cmdAdd, New System.EventArgs())
			
		End If
		
		Erase msCurrSelections
		
        Me.Close()

        logger.Debug("cmdExit_Click Exit")
		
	End Sub
End Class