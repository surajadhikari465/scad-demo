Option Strict Off
Option Explicit On
Friend Class frmTGMCreate
	Inherits System.Windows.Forms.Form
    Private IsInitializing As Boolean
	Dim piFrame As Short
	
	Private Sub cmdCreate_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCreate.Click
		
		With glTGMTool
			
			.SubTeam_No = VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex)
            .StartDate = dtpStartDate.Value
            .EndDate = dtpEndDate.Value
			.Discontinued = System.Math.Abs(chkDiscontinued.CheckState)
			.HIAH = System.Math.Abs(chkHIAH.CheckState)
			.FileName = ""
			
			Select Case True
				Case optAll.Checked : .Query = "ALL"
				Case optBrand.Checked : .Query = "BRAND" : .value = CInt(txtSearch.Tag)
				Case optCategory.Checked : .Query = "CATEGORY" : .value = CInt(txtSearch.Tag)
				Case OptVendor.Checked : .Query = "VENDOR" : .value = CInt(txtSearch.Tag)
			End Select
			
			RetrieveTGMData()
			
		End With
		
		Me.Close()
		
		frmTGMLast.ShowDialog()
        frmTGMLast.Dispose()
		
	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		'-- Unload search form
		Me.Close()
		
	End Sub
	
	Private Sub cmdNext_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdNext.Click
		
        Select Case piFrame
            Case 0
                If dtpEndDate.Value < dtpStartDate.Value Then
                    MsgBox("End date is older than begin date.", MsgBoxStyle.Exclamation, "Invalid Range!")
                    dtpEndDate.Focus()
                    Exit Sub
                End If
            Case 1
                Select Case True
                    Case optBrand.Checked And txtSearch.Tag.ToString() = "" : MsgBox("Brand must be selected to use this option.", MsgBoxStyle.Exclamation, "Brand not entered") : Exit Sub
                    Case optCategory.Checked And txtSearch.Tag.ToString() = "" : MsgBox("Category must be selected to use this option.", MsgBoxStyle.Exclamation, "Category not entered") : Exit Sub
                    Case OptVendor.Checked And txtSearch.Tag.ToString() = "" : MsgBox("Vendor must be selected to use this option.", MsgBoxStyle.Exclamation, "Vendor not entered") : Exit Sub
                End Select
        End Select
		
		piFrame = piFrame + 1
		ShowFrame()
		
	End Sub
	
	Private Sub cmdPrevious_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdPrevious.Click
		
		piFrame = piFrame - 1
		ShowFrame()
		
	End Sub
	
	Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch.Click
		
		Select Case True
			
			Case optBrand.Checked
				
				'-- Set glvendorid to none found
				glBrandID = 0
				
				'-- Set the search type
				giSearchType = iSearchBrand
				
				'-- Open the search form
				frmSearch.Text = "Search for Brand by Brand Name"
				frmSearch.ShowDialog()
                frmSearch.Dispose()
				
				'-- if its not zero, then something was found
				If glBrandID <> 0 Then
					txtSearch.Tag = glBrandID
                    txtSearch.Text = ReturnBrandName(glBrandID)
				End If
				
			Case optCategory.Checked
				
				'-- Set glvendorid to none found
				glCategoryID = 0
				
				'-- Set the search type
				giSearchType = iSearchCategory
				
				'-- Open the search form
				frmSearch.Text = "Search for Category by Category Name"
				frmSearch.ShowDialog()
                frmSearch.Dispose()
				
				'-- if its not zero, then something was found
				If glCategoryID <> 0 Then
					txtSearch.Tag = glCategoryID
                    txtSearch.Text = ReturnCategoryName(glCategoryID)
				End If
				
			Case OptVendor.Checked
				
				'-- Set glvendorid to none found
				glVendorID = 0
				
				'-- Set the search type
				giSearchType = iSearchVendorCompany
				
				'-- Open the search form
				frmSearch.Text = "Search for Vendor by Company Name"
				frmSearch.ShowDialog()
                frmSearch.Dispose()
				
				'-- if its not zero, then something was found
				If glVendorID <> 0 Then
					txtSearch.Tag = glVendorID
                    txtSearch.Text = ReturnVendorName(glVendorID)
				End If
				
		End Select
		
	End Sub
	
	Private Sub frmTGMCreate_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		piFrame = 0
		ShowFrame()
		
		'-- Center the form and the buttons on the form
		CenterForm(Me)
		
		LoadAllSubTeams(cmbSubTeam)
		cmbSubTeam.SelectedIndex = 0

        dtpStartDate.Value = DateAdd(DateInterval.Day, -7, System.DateTime.Today)
        dtpEndDate.Value = DateAdd(DateInterval.Day, -1, System.DateTime.Today)

	End Sub
	
	Private Sub ShowFrame()
		
		Dim iLoop As Short
		
		cmdPrevious.Enabled = piFrame > 0
		cmdNext.Visible = piFrame < 2
		cmdCreate.Visible = piFrame >= 2
		
		For iLoop = Frame.LBound To Frame.UBound
			Frame(iLoop).Visible = (piFrame = iLoop)
		Next iLoop
		
		Select Case piFrame
			Case 0 : lblInstructions.Text = "Select Sub-Team and date range for this TGM."
			Case 1 : lblInstructions.Text = "Select limiting criteria for this TGM."
			Case 2 : lblInstructions.Text = "Select special view criteria for this TGM."
		End Select
		
	End Sub
	
    Private Sub optAll_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optAll.CheckedChanged
        If Me.IsInitializing = True Then Exit Sub

        If eventSender.Checked Then

            lblPullType.Text = ""
            txtSearch.Visible = False
            cmdSearch.Visible = False

        End If
    End Sub
	
    Private Sub optBrand_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optBrand.CheckedChanged
        If Me.IsInitializing = True Then Exit Sub

        If eventSender.Checked Then

            lblPullType.Text = "[ Brand ]"
            txtSearch.Visible = True
            cmdSearch.Visible = True
            txtSearch.Text = ""
            txtSearch.Tag = ""

        End If
    End Sub
	
    Private Sub optCategory_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optCategory.CheckedChanged
        If Me.IsInitializing = True Then Exit Sub

        If eventSender.Checked Then

            lblPullType.Text = "[ Category ]"
            txtSearch.Visible = True
            cmdSearch.Visible = True
            txtSearch.Tag = ""
            txtSearch.Text = ""

        End If
    End Sub
	
    Private Sub optVendor_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptVendor.CheckedChanged
        If Me.IsInitializing = True Then Exit Sub

        If eventSender.Checked Then

            lblPullType.Text = "[ Vendor ]"
            txtSearch.Visible = True
            cmdSearch.Visible = True
            txtSearch.Tag = ""
            txtSearch.Text = ""

        End If
    End Sub
	
End Class