Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic

Friend Class frmAvgCostReport
    Inherits System.Windows.Forms.Form

    Private IsInitializing As Boolean

    Private Sub frmAvgCostReport_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        SetActive(cmbZone, False) 'Must pick a store first

        '-- Load Stores
        LoadStores(cmbStore)

        If glStore_Limit > 0 Then
            SetActive(cmbStore, False)
            SetCombo(cmbStore, glStore_Limit)
        Else
            cmbStore.SelectedIndex = -1
            SetActive(cmbStore, True)
        End If
    End Sub

    Private Sub cmbStore_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbStore.SelectedIndexChanged

        If IsInitializing Then Exit Sub

        If cmbStore.SelectedIndex > -1 Then
            'Patrick Test...
            Call LoadSubTeamByType(Global_Renamed.enumSubTeamType.Store, cmbSubTeam, (VB6.GetItemData(cmbStore, cmbStore.SelectedIndex)))

            SetActive((cmbSubTeam), True)

            LoadStoreZones(cmbZone, VB6.GetItemData(cmbStore, cmbStore.SelectedIndex))
            SetActive((cmbZone), True)
        Else
            cmbSubTeam.Items.Clear()
            SetActive((cmbSubTeam), False)
            cmbZone.Items.Clear()
            SetActive(cmbZone, False)
        End If

    End Sub

    Private Sub cmbZone_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbZone.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        If KeyAscii = 8 Then cmbZone.SelectedIndex = -1

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub cmdReport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReport.Click
        On Error GoTo me_err

        Dim rsReport As ADODB.Recordset
        Dim fld As ADODB.Field
        Dim sStore_No As String
        Dim sSubTeam_No As String
        Dim sCategory_ID As String
        Dim sZone_ID As String
        Dim sCurrIdentifier As String
        Dim dblPackage_Desc1 As Double
        Dim sOldTitle As String

        sCurrIdentifier = String.Empty

        sOldTitle = Me.Text
        Me.Text = "Running the Average Cost Report..."

        If cmbStore.SelectedIndex = -1 Then
            MsgBox("You must select a store", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Select a Store")
            Exit Sub
        End If

        sStore_No = CStr(VB6.GetItemData(cmbStore, cmbStore.SelectedIndex))
        sSubTeam_No = ComboValue((cmbSubTeam))
        sCategory_ID = ComboValue((cmbCategory))

        If cmbZone.Enabled Then
            sZone_ID = ComboValue(cmbZone)
        Else
            sZone_ID = "NULL"
        End If

        '--------------------------
        ' Setup Report URL
        ' for Reporting Services
        '--------------------------

        Dim sReportURL As New System.Text.StringBuilder

        sReportURL.Append("AvgCost")

        'report display
        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        ' Add Report Parameters

        If sStore_No = "" Then
            sReportURL.Append("&Store_No:isnull=true")
        Else
            sReportURL.Append("&Store_No=" & sStore_No.Trim)
        End If

        sReportURL.Append("&SubTeam_Name=" & cmbSubTeam.Text.Trim)

        If sSubTeam_No = "NULL" Then
            sReportURL.Append("&SubTeam_No:isnull=true")
        Else
            sReportURL.Append("&SubTeam_No=" & sSubTeam_No.Trim)
        End If

        If sCategory_ID = "NULL" Then
            sReportURL.Append("&Category_ID:isnull=true")
        Else
            sReportURL.Append("&Category_ID=" & sCategory_ID.Trim)
        End If

        If sZone_ID = "NULL" Then
            sReportURL.Append("&Zone_ID:isnull=true")
        Else
            sReportURL.Append("&Zone_ID=" & sZone_ID.Trim)
        End If

        If chkPendOnly.CheckState Then
            sReportURL.Append("&PendOnly=true")
        Else
            sReportURL.Append("&PendOnly=false")
        End If

        '--------------------------
        ' Display Report
        '--------------------------
        Dim s As String = sReportURL.ToString()
        Call ReportingServicesReport(s)

        Me.Text = sOldTitle

        ' Code for old Crystal Report version
        ' Commented out when replaced by Reporting Services report
        '-- Clear Access Database Tables and open recordset
        'gDBReport.BeginTrans()
        'On Error GoTo me_db_err
        'rsReport = New ADODB.Recordset
        'gDBReport.Execute("DELETE * FROM AvgCost")
        ''-- Main report
        'rsReport.Open("AvgCost", gDBReport, ADODB.CursorTypeEnum.adOpenKeyset, ADODB.LockTypeEnum.adLockOptimistic, ADODB.CommandTypeEnum.adCmdTableDirect)
        ''-- Execute main report stored procedure
        'gRSRecordset = SQLOpenRecordSet("EXEC GetAvgCost " & sStore_No & "," & sSubTeam_No & "," & sCategory_ID & "," & sZone_ID & "," & chkPendOnly.CheckState, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough + DAO.RecordsetOptionEnum.dbForwardOnly)
        'While Not gRSRecordset.EOF
        '    If Not (gRSRecordset.Fields("Identifier").Value = sCurrIdentifier And gRSRecordset.Fields("Package_Desc1").Value = dblPackage_Desc1) Then
        '        sCurrIdentifier = gRSRecordset.Fields("Identifier").Value
        '        dblPackage_Desc1 = gRSRecordset.Fields("Package_Desc1").Value
        '        rsReport.AddNew()
        '        For Each fld In rsReport.Fields
        '            If VB.Left(fld.Name, 1) <> "_" Then 'Fill in the item specific info
        '                fld.Value = gRSRecordset.Fields(fld.Name).Value
        '            End If
        '        Next fld
        '    End If
        '    'Fill in the zone specific stuff
        '    rsReport.Fields("_" & gRSRecordset.Fields("Zone_Name").Value & "Store_ExtCost").Value = gRSRecordset.Fields("Store_ExtCost").Value
        '    rsReport.Fields("_" & gRSRecordset.Fields("Zone_Name").Value & "Price").Value = gRSRecordset.Fields("Price").Value
        '    rsReport.Fields("_" & gRSRecordset.Fields("Zone_Name").Value & "BuyerPrice").Value = gRSRecordset.Fields("BuyerPrice").Value
        '    gRSRecordset.MoveNext()
        'End While
        'Update the last AddNew before closing
        'If Not (gRSRecordset.EOF And gRSRecordset.BOF) Then rsReport.Update()
        'fld = Nothing
        'rsReport.Close()
        'gRSRecordset.Close()
        'rsReport = Nothing
        'gDBReport.CommitTrans()
        'On Error GoTo me_err
        'gJetFlush.RefreshCache(gDBReport)

        ' ###########################################################################
        ' 10/8/2007 (Robin Eudy) Crystal Report Dependency has been commented out.
        ' ###########################################################################
        'MsgBox("AvgCostReport.vb  cmdReport_Click(): The Crystal Report AvgCost.rpt is normally shown here. It has been disabled until the Crystal dependencies can be removed or replaced", MsgBoxStyle.Exclamation)

        ''-- Print Crystal Report
        'crwReport.Destination = IIf(chkPrintOnly.CheckState = 0, Crystal.DestinationConstants.crptToWindow, Crystal.DestinationConstants.crptToPrinter)
        'crwReport.ReportFileName = My.Application.Info.DirectoryPath & gsReportDirectory & "AvgCost.rpt"
        'crwReport.ReportTitle = "Average Cost - " & cmbStore.Text
        'If cmbZone.SelectedIndex > -1 Then
        '    crwReport.ReportTitle = crwReport.ReportTitle & ", Zone = " & cmbZone.Text
        'Else
        '    crwReport.ReportTitle = crwReport.ReportTitle & ", Regional"
        'End If

        'PrintReport(crwReport)

        Exit Sub

me_err:
        MsgBox("Report error: " & Err.Description, MsgBoxStyle.Critical, Me.Text)

        Exit Sub

me_db_err:
        MsgBox("Report error: " & Err.Description, MsgBoxStyle.Critical, Me.Text)
        gDBReport.RollbackTrans()

    End Sub

    '********************************
    'SubTeam / Category Code
    '********************************
    Private Sub cmbSubTeam_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.SelectedIndexChanged

        If IsInitializing Then Exit Sub

        If cmbSubTeam.SelectedIndex = -1 Then
            cmbCategory.Items.Clear()
        Else
            Call LoadCategory(cmbCategory, VB6.GetItemData(cmbSubTeam, cmbSubTeam.SelectedIndex))
            SetActive(cmbCategory, True)
        End If

    End Sub

    Private Sub cmbSubTeam_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.TextChanged
        If cmbSubTeam.Text = "" Then cmbSubTeam.SelectedIndex = -1
    End Sub

    Private Sub cmbSubTeam_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSubTeam.KeyPress
        If Asc(e.KeyChar) = 8 Then cmbSubTeam.SelectedIndex = -1
    End Sub

    Private Sub cmbCategory_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbCategory.KeyPress
        If Asc(e.KeyChar) = 8 Then Me.cmbCategory.SelectedIndex = -1
    End Sub

    '*************************************
    'End SubTeam / Category Code
    '*************************************

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

End Class