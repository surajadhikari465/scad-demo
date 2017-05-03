Option Strict Off
Option Explicit On

Imports WholeFoods.Utility

Imports log4net
Friend Class frmReceivingCheckList
    Inherits System.Windows.Forms.Form
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click Entry")
        Me.Close()
        logger.Debug("cmdExit_Click Exit")
		
	End Sub
	
	Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click


        logger.Debug("cmdReport_Click Entry")
        ' Parameters declaration to call Reporting Services.
        Dim sReportURL As New System.Text.StringBuilder
        Dim filename As String

        '--------------------------
        ' Setup Report URL
        '--------------------------

        ' To pass the parameter for the ReceivingCheckList stored procedure.
        Dim optItemId As Byte
        If optIdentifier(0).Checked Then
            optItemId = 1
        Else
            optItemId = 0
        End If


        filename = ConfigurationServices.AppSettings("Region")
        filename = "Checklist"
        sReportURL.Append(filename)

        ' This chooses the region and based on the results points to the correct report.

        ' Report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")


        ' Passing parameters to report.
        If glOrderHeaderID > 0 Then
            sReportURL.Append("&OrderHeader_ID=" & glOrderHeaderID)
        End If

        If optItemId >= 0 Then
            sReportURL.Append("&optIdentifier=" & optItemId)
        End If

        ' Passing Sorting Parameter as an input to the report.
        Select Case True
            Case optSort(0).Checked
                ' Soring on LineItem(0 as parameter value)
                sReportURL.Append("&optSort=" & 0)
            Case optSort(1).Checked
                ' Sort on Identifier (1 as parameter Value)
                sReportURL.Append("&optSort=" & 1)
            Case optSort(2).Checked
                ' Sort on Item_description(2 as parameter Value)
                sReportURL.Append("&optSort=" & 2)
        End Select

        If optItemId >= 0 Then
            sReportURL.Append("&Item_ID=" & IIf(optItemId = 1, "True", "False"))
        End If

        If Me.GroupByCatCheckBox.Checked Then
            sReportURL.Append("&GroupByCategory=True")
        Else
            sReportURL.Append("&GroupByCategory=False")
        End If

        If Me.GroupByShippedCheckBox.Checked Then
            sReportURL.Append("&GroupByShipped=True")
        Else
            sReportURL.Append("&GroupByShipped=False")
        End If

        Call ReportingServicesReport(sReportURL.ToString)
        logger.Debug("cmdReport_Click Exit")
    End Sub
	
	Private Sub frmReceivingCheckList_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		CenterForm(Me)
		
	End Sub

    Private Sub GroupByCatCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupByCatCheckBox.CheckedChanged
        If Me.GroupByCatCheckBox.Checked Or Me.GroupByShippedCheckBox.Checked Then
            Me.SortByFrame.Enabled = False
        Else
            Me.SortByFrame.Enabled = True
        End If
    End Sub

    Private Sub GroupByShippedCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupByShippedCheckBox.CheckedChanged
        If Me.GroupByCatCheckBox.Checked Or Me.GroupByShippedCheckBox.Checked Then
            Me.SortByFrame.Enabled = False
        Else
            Me.SortByFrame.Enabled = True
        End If

    End Sub

End Class