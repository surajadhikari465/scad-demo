Option Strict Off
Option Explicit On
Friend Class frmFulfillmentReport
	Inherits System.Windows.Forms.Form
	
	Private Sub cmdVendorSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdVendorSearch.Click
		
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
			txtVendor.Tag = glVendorID
            txtVendor.Text = ReturnVendorName(glVendorID)
		End If
		
	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
		
        Dim sTitle As String
        Dim sSelection As String
        sTitle = String.Empty
        sSelection = String.Empty

        If txtVendor.Text = String.Empty Then
            MsgBox("Vendor must be selected", MsgBoxStyle.Exclamation, "Error!")
            Exit Sub
        End If
		
        If dtpEndDate.Value < dtpStartDate.Value Then
            MsgBox(ResourcesIRMA.GetString("EndDateGreaterEqual"), MsgBoxStyle.Exclamation, Me.Text)
            dtpEndDate.Focus()
            Exit Sub
        End If

		If CInt(txtVendor.Tag) > 0 Then
			sSelection = sSelection & " AND {Vendor.Vendor_ID} = " & txtVendor.Tag
			sTitle = sTitle & "FROM " & txtVendor.Text & " "
		End If
		
        sTitle = sTitle & "For " & cmbCustomer.Text & " "

        ' ###########################################################################
        ' 10/8/2007 (Robin Eudy) Crystal Report Dependency has been commented out.
        ' ###########################################################################
        MsgBox("FulFillmentReport.vb  cmdReport_Click(): The Crystal Report OrderFulfillment.rpt or OrderFulfillmentSummary.rpt is normally shown here. It has been disabled until the Crystal dependencies can be removed or replaced", MsgBoxStyle.Exclamation)

		
        '      crwReport.SelectionFormula = "{OrderHeader.CloseDate} >= Date (" & VB6.Format(dtpStartDate.Value, "YYYY, MM, DD") & ") AND {OrderHeader.CloseDate} < Date (" & VB6.Format(DateAdd(DateInterval.Day, 1, dtpEndDate.Value), "YYYY, MM, DD") & ") AND {Vendor2.Vendor_ID} = " & VB6.GetItemData(cmbCustomer, cmbCustomer.SelectedIndex) & " " & sSelection
        '      crwReport.ReportFileName = My.Application.Info.DirectoryPath & gsReportDirectory & "OrderFulfillment" & IIf(chkSummary.CheckState = 1, "Summary", "") & ".rpt"
        'crwReport.Destination = IIf(chkPrintOnly.CheckState = 0, Crystal.DestinationConstants.crptToWindow, Crystal.DestinationConstants.crptToPrinter)

        '      crwReport.ReportTitle = "Order Fulfillment " & dtpStartDate.Text & " - " & dtpEndDate.Text & vbCrLf & IIf(chkSummary.CheckState = 1, "Summary ", "") & sTitle

        'crwReport.Connect = gsCrystal_Connect
        'PrintReport(crwReport)
		
	End Sub
	
	Private Sub frmFulfillmentReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		CenterForm(Me)
		
		LoadInternalCustomer(cmbCustomer)
		cmbCustomer.SelectedIndex = 0

        dtpStartDate.Value = DateAdd(DateInterval.Day, -7, SystemDateTime)
        dtpEndDate.Value = DateAdd(DateInterval.Day, -1, SystemDateTime)

	End Sub
	
End Class