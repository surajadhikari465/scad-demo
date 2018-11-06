Option Strict Off
Option Explicit On
Friend Class frmCycleCountItemAdd
	Inherits System.Windows.Forms.Form
	
	Private mlCycleCountHeaderID As Integer
	Private mlSubTeamID As Integer
	Private mlInvLocID As Integer
	Private mbManufacturingSubTeam As Boolean
	
    Private mbAdded As Boolean 'Indicates that items were added.

    Private IsInitializing As Boolean
	
	Private Sub frmCycleCountItemAdd_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		CenterForm(Me)
		
		OptSelection_CheckedChanged(OptSelection.Item(0), New System.EventArgs())
		
	End Sub
	
	Public Function LoadForm(ByRef lCycleCountID As Integer, ByRef lSubTeamID As Integer, ByRef bManufacturingSubTeam As Boolean, Optional ByRef lInvLocID As Integer = 0) As Boolean
		
		mlCycleCountHeaderID = lCycleCountID
		mlSubTeamID = lSubTeamID
		mbManufacturingSubTeam = bManufacturingSubTeam
		mlInvLocID = lInvLocID
		mbAdded = False
		
		Me.ShowDialog()
		
		LoadForm = mbAdded
		
	End Function
	
	Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click
		
        Dim sInvLocID As String
		
		If mlInvLocID = 0 Then
			sInvLocID = "null"
		Else
			sInvLocID = CStr(mlInvLocID)
		End If
		
		Select Case True
			
			Case OptSelection(0).Checked
				
				If MsgBox("Add all items into cycle count?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Notice!") = MsgBoxResult.Yes Then
					SQLExecute("EXEC InsertCycleCountItemsAll " & mlCycleCountHeaderID & "," & mlSubTeamID & "," & sInvLocID, dao.RecordsetOptionEnum.dbSQLPassThrough)
				End If
				
			Case OptSelection(1).Checked
				
				If cmbSelection.SelectedIndex = -1 Then
					MsgBox("Brand must be selected.", MsgBoxStyle.Exclamation, "Error!")
				Else
					If MsgBox("Add all items with this brand to cycle count?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Notice!") = MsgBoxResult.Yes Then
						SQLExecute("EXEC InsertCycleCountItemsBrand " & mlCycleCountHeaderID & ", " & mlSubTeamID & ", " & VB6.GetItemData(cmbSelection, cmbSelection.SelectedIndex) & "," & sInvLocID, dao.RecordsetOptionEnum.dbSQLPassThrough)
					End If
				End If
				
			Case OptSelection(2).Checked
				
				If cmbSelection.SelectedIndex = -1 Then
					MsgBox("Category must be selected.", MsgBoxStyle.Exclamation, "Error!")
				Else
                    If MsgBox("Add all items in this class to cycle count?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Notice!") = MsgBoxResult.Yes Then
                        SQLExecute("EXEC InsertCycleCountItemsCategory " & mlCycleCountHeaderID & ", " & mlSubTeamID & ", " & VB6.GetItemData(cmbSelection, cmbSelection.SelectedIndex) & "," & sInvLocID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                    End If
				End If
				
			Case OptSelection(4).Checked
				
				If cmbSelection.SelectedIndex = -1 Then
					MsgBox("Vendor must be selected.", MsgBoxStyle.Exclamation, "Error!")
				Else
					If MsgBox("Add all items supplied by this vendor to cycle count?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Notice!") = MsgBoxResult.Yes Then
						SQLExecute("EXEC InsertCycleCountItemsVendor " & mlCycleCountHeaderID & ", " & mlSubTeamID & ", " & VB6.GetItemData(cmbSelection, cmbSelection.SelectedIndex) & "," & sInvLocID, dao.RecordsetOptionEnum.dbSQLPassThrough)
					End If
				End If
				
			Case OptSelection(5).Checked
				
				Call AddIndividualItems(sInvLocID)
				
			Case OptSelection(6).Checked
				
				If MsgBox("Add all zero count items into cycle count?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Notice!") = MsgBoxResult.Yes Then
					SQLExecute("EXEC InsertCycleCountItemsZeroCount " & mlCycleCountHeaderID & ", " & mlSubTeamID & ", " & glStoreID & "," & sInvLocID, dao.RecordsetOptionEnum.dbSQLPassThrough)
				End If
				
			Case OptSelection(7).Checked
				
				If CDec(txtNumber.Text) <= 0 Then
					MsgBox("Number of Items must be entered.", MsgBoxStyle.Exclamation, "Error!")
				Else
					If MsgBox("Add most expensive items into cycle count?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Notice!") = MsgBoxResult.Yes Then
						SQLExecute("EXEC InsertCycleCountItemsMostExpensive " & mlCycleCountHeaderID & ", " & mlSubTeamID & ", " & txtNumber.Text & "," & sInvLocID, dao.RecordsetOptionEnum.dbSQLPassThrough)
					End If
				End If
				
		End Select
		
		mbAdded = True
		
	End Sub
	
	Private Sub AddIndividualItems(ByRef sInvLocID As String)
		Dim iSelected As Short
		Dim lItemId As Integer
		
		glItemID = 0
		
        '------------------------------------
		'Configure the item search form.
        '------------------------------------
        frmItemSearch.InitForm()
		If Not mbManufacturingSubTeam Then
			'Limit the list of sub-teams to the current subteam of the location.
            frmItemSearch.LimitSubTeam_No = mlSubTeamID
		Else
			'Set the initial sub-team number.  List should only contain retail products.
            frmItemSearch.SubTeam_No = mlSubTeamID
		End If
		
        frmItemSearch.IncludeDeletedItems = Global_Renamed.enumChkBoxValues.UncheckedDisabled
        frmItemSearch.ExcludeNotAvailable = Global_Renamed.enumChkBoxValues.UncheckedDisabled
        frmItemSearch.SoldAtHFM = Global_Renamed.enumChkBoxValues.CheckedDisabled
        frmItemSearch.SoldAtWFM = Global_Renamed.enumChkBoxValues.CheckedDisabled
        frmItemSearch.MultiSelect = True
		
        frmItemSearch.ShowDialog()
		
		'Add Items
        If frmItemSearch.SelectedItems.Count > 0 Then

            For iSelected = 0 To frmItemSearch.SelectedItems.Count - 1

                lItemId = frmItemSearch.SelectedItems.Item(iSelected).Item_Key

                '-- Add the new record
                SQLExecute("EXEC InsertCycleCountItem " & mlCycleCountHeaderID & "," & lItemId & "," & sInvLocID, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            Next iSelected
        End If
		
        frmItemSearch.Close()
        frmItemSearch.Dispose()

	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
    Private Sub OptSelection_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptSelection.CheckedChanged

        If IsInitializing Then Exit Sub

        If eventSender.Checked Then
            Dim Index As Short = OptSelection.GetIndex(eventSender)

            Dim rsRecordset As dao.Recordset = Nothing

            cmbSelection.Items.Clear()

            Select Case Index
                Case 0, 5, 6
                    cmbSelection.Visible = False
                    lblSelection.Visible = False
                    txtNumber.Visible = False
                Case 1
                    cmbSelection.Visible = True
                    lblSelection.Visible = True
                    txtNumber.Visible = False
                    lblSelection.Text = "Brand :"
                    rsRecordset = SQLOpenRecordSet("EXEC GetSubTeamBrand " & mlSubTeamID, dao.RecordsetTypeEnum.dbOpenSnapshot, dao.RecordsetOptionEnum.dbSQLPassThrough)
                    While Not rsRecordset.EOF
                        cmbSelection.Items.Add(New VB6.ListBoxItem(rsRecordset.Fields("Brand_Name").Value & "", rsRecordset.Fields("Brand_ID").Value))
                        rsRecordset.MoveNext()
                    End While
                    rsRecordset.Close()
                Case 2
                    cmbSelection.Visible = True
                    lblSelection.Visible = True
                    txtNumber.Visible = False
                    lblSelection.Text = "Category :"
                    rsRecordset = SQLOpenRecordSet("EXEC GetSubTeamCategory " & mlSubTeamID, dao.RecordsetTypeEnum.dbOpenSnapshot, dao.RecordsetOptionEnum.dbSQLPassThrough)
                    While Not rsRecordset.EOF
                        cmbSelection.Items.Add(New VB6.ListBoxItem(rsRecordset.Fields("Category_Name").Value & "", rsRecordset.Fields("Category_ID").Value))
                        rsRecordset.MoveNext()
                    End While
                    rsRecordset.Close()
                Case 4
                    cmbSelection.Visible = True
                    lblSelection.Visible = True
                    txtNumber.Visible = False
                    lblSelection.Text = "Vendor :"
                    rsRecordset = SQLOpenRecordSet("EXEC GetSubTeamVendors " & mlSubTeamID, dao.RecordsetTypeEnum.dbOpenSnapshot, dao.RecordsetOptionEnum.dbSQLPassThrough)
                    While Not rsRecordset.EOF
                        cmbSelection.Items.Add(New VB6.ListBoxItem(rsRecordset.Fields("CompanyName").Value & "", rsRecordset.Fields("Vendor_ID").Value))
                        rsRecordset.MoveNext()
                    End While
                    rsRecordset.Close()
                Case 7
                    cmbSelection.Visible = False
                    lblSelection.Visible = True
                    txtNumber.Visible = True
                    lblSelection.Text = "Items :"
                    txtNumber.Text = "100"
            End Select

            '-- default to the first selection
            If cmbSelection.Items.Count > 0 Then cmbSelection.SelectedIndex = 0

        End If
    End Sub
	
	Private Sub txtNumber_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtNumber.KeyPress
		Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
		
		KeyAscii = ValidateKeyPressEvent(KeyAscii, "Number", txtNumber, 0, 0, 0)
		
		eventArgs.KeyChar = Chr(KeyAscii)
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
	End Sub
End Class