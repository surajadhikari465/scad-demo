'********************************************************************************************************************************************************************************
'Ripe Import Summary
'Imports all orders available for selected rows

'CREATED_BY         CREATED_DATE        FUNCTION_NAME                       FUNCTION_SUMMARY 
'--------------------------------------------------------------------------------------------

'UPDATED_BY         UPDATED_DATE        UPDATED_FUNCTION_NAME               UPDATION_SUMMARY
'--------------------------------------------------------------------------------------------
' vayals            12/14/09            LoadDataTable                       Replaced DAO recordset with arraylist
'                                                                           Added more Input parameters           
' vayals            12/14/09            cmdImport_Click                     Created a new object for class "clsRipeImport" and all methods are called via the object
'                                                                           Commented the code on crystal reports
' vayals            12/14/09            frmRIPEImportSelection_Load         Created a new object for class "clsRipeImport" and all methods are called via the object
' vayals            12/14/09            RefreshGrid                         Split up the parameters for function LoadDataTable
'********************************************************************************************************************************************************************************

Option Strict Off
Option Explicit On
Imports System.Text
Imports Infragistics.Win
Imports log4net
Imports System.Configuration
Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.IRMA.Common
Imports WholeFoods.IRMA.Common.BusinessLogic
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.Tools.BusinessLogic
Imports WholeFoods.IRMA.Tools.DataAccess
Imports System
Imports System.Web.Services.Protocols


Friend Class frmRIPEImportSelection
    Inherits System.Windows.Forms.Form
    
    Private mdt As DataTable
    Private mdv As DataView
    Private m_oCon As System.Data.SqlClient.SqlConnection
    Private IsInitializing As Boolean

    Private Sub SetupDataTable()
        ' Create a data table
        mdt = New DataTable("RipeCust")
        'Visible on grid.
        '--------------------
        mdt.Columns.Add(New DataColumn("CompanyName", GetType(String)))
        'Hidden.
        '--------------------
        mdt.Columns.Add(New DataColumn("CustomerID", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("ZoneID", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("StoreNo", GetType(Integer)))

    End Sub

    Private Sub LoadDataTable(ByVal sSearchSQL As String, ByVal nLocation As Integer, ByVal sDistDate As String)

        Dim row As DataRow
        Dim MaxLoop As Short = 1000
        Dim listResult As New ArrayList
        Dim objBORipe As New clsRipeImport
        ' Fetches all information to load the Ripe Form controls
        listResult = objBORipe.Ripe_SQLOpenRecordSet(sSearchSQL, nLocation, sDistDate, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        'Load the data set.
        mdt.Rows.Clear()
        'adds the data to the Grid
        For i As Integer = 0 To listResult.Count - 1
            row = mdt.NewRow
            row("CompanyName") = listResult.Item(i).Item(0)
            row("CustomerID") = listResult.Item(i).Item(1)
            row("StoreNo") = listResult.Item(i).Item(2)
            row("ZoneID") = listResult.Item(i).Item(3)
            mdt.Rows.Add(row)
        Next
        'Setup a column that you would like to sort on initially.
        mdt.AcceptChanges()
        mdv = New System.Data.DataView(mdt)
        mdv.Sort = "CompanyName"
        ugrdRecipeCustomer.DataSource = mdv
        ugrdRecipeCustomer.DataBind()
        'This may or may not be required.
        If listResult.Count > 0 Then
            'Set the first item to selected.
            ugrdRecipeCustomer.Rows(0).Selected = True
        Else
            MsgBox("No items found.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
        End If

ExitSub:
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub

    Private Sub cboFromStore_Change()

    End Sub

    Private Sub cmbFromStore_Click()
        If Me.txtDistribution.Text = "" Then
            MsgBox("You must select a distribution date before you select a Facility.")
        End If

    End Sub

    Private Sub cmbFromLocation_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbFromLocation.SelectedIndexChanged
        If IsInitializing = True Then Exit Sub
        Call RefreshGrid()
    End Sub

    Private Sub cmbRipeZone_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbRipeZone.SelectedIndexChanged
        If optSelection(2).Checked <> True Then
            optSelection(2).Checked = True
        Else
            Call OptSelection_CheckedChanged(optSelection.Item(2), New System.EventArgs())
        End If

    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub cmdImport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdImport.Click
        Dim dblTotTime As Double
        Dim lImportOrderCnt As Integer
        'creates a new object fro class "clsRipeImport"
        Dim objBORipe As New clsRipeImport
        On Error GoTo CleanUp
        'Call WriteMasterLog("Begin Import (cmdImport_Click)")
        If Len(Trim(txtDistribution.Text)) <> 10 Then
            MsgBox("Distribution date must be completed.", MsgBoxStyle.Exclamation, "Error!")
            Exit Sub
        End If
        If cmbFromLocation.SelectedIndex = -1 Then
            MsgBox("From Location must be selected.", MsgBoxStyle.Exclamation, "Error!")
            Exit Sub
        End If
        If ugrdRecipeCustomer.Selected.Rows.Count = 0 Then
            MsgBox("You must select the customer(s) thay you want to import orders for.", MsgBoxStyle.Exclamation, "Error!")
            Exit Sub
        End If
        Err.Clear()
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        Dim iCnt As Short
        Dim vBook As Object
        'build selected customer list
        Dim arySelectedCust() As String
        ReDim arySelectedCust(ugrdRecipeCustomer.Selected.Rows.Count)
        For iCnt = 0 To ugrdRecipeCustomer.Selected.Rows.Count - 1
            arySelectedCust(iCnt + 1) = ugrdRecipeCustomer.Selected.Rows(iCnt).Cells("CustomerID").Value
        Next iCnt
        'Check for orders already imported for selected customers
        Dim aryAlreadyExists(0, 0) As String
        dblTotTime = Microsoft.VisualBasic.DateAndTime.Timer
        'Call WriteMasterLog("begin CheckforExistingDistributions")
        'calls the method using the object
        aryAlreadyExists = objBORipe.Ripe_CheckforExistingDistributions(Me.txtDistribution.Text, ComboVal(Me.cmbFromLocation), arySelectedCust)
        'Call WriteMasterLog("end CheckforExistingDistributions (total time: " & Microsoft.VisualBasic.DateAndTime.Timer - dblTotTime & "}")
        If UBound(aryAlreadyExists, 2) > 0 Then
            'display warning dialog
            'Call WriteMasterLog("begin Create Warning Form")
            Dim fImportWarning As New frmRipeImportWarning(aryAlreadyExists, txtDistribution.Text)
            'Call WriteMasterLog("end Create Warning Form")
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
            fImportWarning.ShowDialog()
            If fImportWarning.Cancel = True Then
                fImportWarning.Close()
                fImportWarning.Dispose()
                MsgBox("Import Process was cancelled.  No orders were imported.")
                Exit Sub
            Else
                fImportWarning.Close()
                fImportWarning.Dispose()
            End If
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        End If
        'for each selected Customer, import the orders
        Me.Enabled = False
        Dim sImportDateTime As String
        'calls the method using the object
        sImportDateTime = CStr(objBORipe.Ripe_SystemDateTime())
        Dim dblTotImpTime As Double
        Dim dblImpTime As Double
        dblTotTime = Microsoft.VisualBasic.DateAndTime.Timer
        'Call WriteMasterLog("begin CheckForImportOrders")
        For iCnt = 0 To ugrdRecipeCustomer.Selected.Rows.Count - 1
            'calls the method using the object
            If objBORipe.Ripe_CheckForImportErrors((txtDistribution.Text), ComboVal(cmbFromLocation), ugrdRecipeCustomer.Selected.Rows(iCnt).Cells("CustomerID").Value) Then
                '-- Ask if they want to display a list of items that did not match
                If MsgBox("Cannot import RIPE order for customer " & ugrdRecipeCustomer.Selected.Rows(iCnt).Cells("CompanyName").Value & " due to items that could not be mapped." & vbCrLf & "Display listing?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Error") = MsgBoxResult.Yes Then

                    '-----------------------------------------------
                    'Unmapped Ripe Items report display
                    '-----------------------------------------------
                    Dim sReportnmappedItemsURL As New System.Text.StringBuilder
                    sReportnmappedItemsURL.Append("rptUnmappedItems")
                    ' Add Report Parameters
                    sReportnmappedItemsURL.Append("&rs:Command=Render")
                    sReportnmappedItemsURL.Append("&rc:Parameters=False")
                    sReportnmappedItemsURL.Append("&ToStore_No=" & Str(ugrdRecipeCustomer.Selected.Rows(iCnt).Cells("CustomerID").Value))
                    sReportnmappedItemsURL.Append("&FromStore_No=" & Str(VB6.GetItemData(cmbFromLocation, cmbFromLocation.SelectedIndex)))
                    sReportnmappedItemsURL.Append("&DistributionDate=" & txtDistribution.Text)
                    'System.Diagnostics.Process.Start("http://iad-so/ReportServer?/irma/so/" & sReportnmappedItemsURL.ToString)
                    System.Diagnostics.Process.Start(gsReportingServicesURL & sReportnmappedItemsURL.ToString)

                End If
            Else
                '-- Begin Import
                dblImpTime = Microsoft.VisualBasic.DateAndTime.Timer
                'Call WriteMasterLog("begin ImportOrder")
                'calls the method using the object
                lImportOrderCnt = lImportOrderCnt + objBORipe.Ripe_ImportOrder(txtDistribution.Text, ComboVal(cmbFromLocation), ugrdRecipeCustomer.Selected.Rows(iCnt).Cells("CustomerID").Value, giUserID, sImportDateTime)
                dblTotImpTime = dblTotImpTime + (Microsoft.VisualBasic.DateAndTime.Timer - dblImpTime)
                'Call WriteMasterLog("End ImportOrder (Time To Import: " & Microsoft.VisualBasic.DateAndTime.Timer - dblImpTime & ")")
            End If
        Next iCnt
        'Call WriteMasterLog("End CheckForImportOrders (total time: " & Microsoft.VisualBasic.DateAndTime.Timer - dblTotTime - dblTotImpTime & ")")

        If lImportOrderCnt = 0 Then
            Err.Raise(ERR_RIPE_IMPORT_NO_NEW_ORDERS, "RIPE Import", ERR_RIPE_IMPORT_NO_NEW_ORDERS_DESC)
        Else
            MsgBox("Given orders have been imported successfully", MsgBoxStyle.Information)

            '-----------------------------------------------
            'Import Summary Report Display
            '-----------------------------------------------
            Dim sReportImportSummary As New System.Text.StringBuilder
            sReportImportSummary.Append("rptImportSummary")
            ' Add Report Parameters
            sReportImportSummary.Append("&rs:Command=Render")
            sReportImportSummary.Append("&rc:Parameters=False")
            sReportImportSummary.Append("&ImportDate=" & sImportDateTime)
            Dim pImportSummary As New ProcessStartInfo("IExplore.exe")
            pImportSummary.WindowStyle = ProcessWindowStyle.Minimized
            'pImportSummary.Arguments = "http://iad-so/ReportServer?/irma/so/" & sReportImportSummary.ToString
            pImportSummary.Arguments = gsReportingServicesURL & sReportImportSummary.ToString
            Process.Start(pImportSummary)

            '-----------------------------------------------
            'Ripe Invoice Report Display
            '-----------------------------------------------
            Dim stringIds As String
            stringIds = objBORipe.Ripe_GetImportedOrders(sImportDateTime)
            Dim sReportRipeInvoice As New System.Text.StringBuilder
            sReportRipeInvoice.Append("RipeInvoice")
            ' Add Report Parameters
            sReportRipeInvoice.Append("&rs:Command=Render")
            sReportRipeInvoice.Append("&rc:Parameters=False")
            sReportRipeInvoice.Append("&StringIds=" & stringIds)
            sReportRipeInvoice.Append("&Item_ID=True")
            sReportRipeInvoice.Append("&SortType=" & 1)
            Dim pRipeInvoice As New ProcessStartInfo("IExplore.exe")
            pRipeInvoice.WindowStyle = ProcessWindowStyle.Minimized
            'pRipeInvoice.Arguments = "http://iad-so/ReportServer?/irma/so/" & sReportRipeInvoice.ToString
            pRipeInvoice.Arguments = gsReportingServicesURL & sReportRipeInvoice.ToString
            Process.Start(pRipeInvoice)

        End If
CleanUp:
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        Me.Enabled = True
        If Err.Number <> 0 Then
            Select Case Err.Number
                Case ERR_RIPE_IMPORT_NO_NEW_ORDERS, ERR_RIPE_IMPORT_NO_TRANSFER_SUBTEAM, ERR_RIPE_IMPORT_NO_TRANSFER_TO_SUBTEAM
                    MsgBox(Err.Description, MsgBoxStyle.Critical, Err.Source)
                Case Else
                    MsgBox(Err.Number & ": " & Err.Description, MsgBoxStyle.Critical, Err.Source)
            End Select
        End If
        'clears the object
        objBORipe = Nothing
    End Sub


   


    Private Sub frmRIPEImportSelection_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        'created a new object in every method to avoid object reference error
        Dim objBORipe As New clsRipeImport
        CenterForm(Me)
        txtDistribution.Text = VB6.Format(System.DateTime.FromOADate(SystemDateTime.ToOADate - 1), "MM/DD/YYYY")
        Call SetupDataTable()
        '-- Set up that damn buggy sheridan grid
        'calls the method using the object
        objBORipe.Ripe_LoadRipeLocations(cmbFromLocation, gsUserName)
        If cmbFromLocation.Items.Count = 0 Then
            Call MsgBox("Your user ID is not associated with any Locations in Ripe.  Contact your help desk.")
            Me.Close()
        Else
            cmbFromLocation.SelectedIndex = -1
            'calls the method using the object
            Call objBORipe.Ripe_LoadRipeZones((Me.cmbRipeZone))
        End If
    End Sub

    Private Sub lblNowPrint_Click()

    End Sub

    Private Sub frmRIPEImportSelection_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        'objBORipe = Nothing
        'm_oCon.Close()
        'm_oCon.Dispose()
    End Sub

    'Private Sub objBORipe_PrintingInvoice(ByRef iInvoiceCnt As Short) Handles objBORipe.PrintingInvoice
    '    Me.lblCurInv.Text = "Now Printing Invoice " & iInvoiceCnt & " "
    'End Sub

    Private Sub OptSelection_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles optSelection.CheckedChanged
        If Me.IsInitializing = True Then Exit Sub
        If eventSender.Checked Then
            Dim Index As Short = optSelection.GetIndex(eventSender)
            Dim iLoop As Short
            Dim iFirstStore As Short
            If Index = 0 Then Exit Sub 'manual selection
            iFirstStore = -1
            Dim iCnt As Integer
            For iCnt = 0 To ugrdRecipeCustomer.Rows.Count - 1
                ugrdRecipeCustomer.Rows(iCnt).Selected = False
            Next
            Select Case Index
                Case 1
                    '-- Select All customers in grid
                    iFirstStore = 0
                    For iCnt = 0 To ugrdRecipeCustomer.Rows.Count - 1
                        ugrdRecipeCustomer.Rows(iCnt).Selected = True
                    Next
                Case 2
                    '--Select only customers in the chosen Ripe zone
                    If cmbRipeZone.SelectedIndex = 0 Then
                        '-- Select All customers in grid
                        iFirstStore = 0
                        For iCnt = 0 To ugrdRecipeCustomer.Rows.Count - 1
                            ugrdRecipeCustomer.Rows(iCnt).Selected = True
                        Next
                    ElseIf cmbRipeZone.SelectedIndex = -1 Then
                        Exit Sub
                    Else
                        For iLoop = 0 To ugrdRecipeCustomer.Rows.Count - 1
                            If ugrdRecipeCustomer.Rows(iLoop).Cells("ZoneID").Value = VB6.GetItemData(cmbRipeZone, cmbRipeZone.SelectedIndex) Then
                                If iFirstStore = -1 Then
                                    iFirstStore = iLoop
                                End If
                                ugrdRecipeCustomer.Rows(iLoop).Selected = True
                            End If
                        Next iLoop
                    End If
            End Select
            If iFirstStore <> -1 And ugrdRecipeCustomer.Rows.Count > 0 Then
                ugrdRecipeCustomer.ActiveRow = ugrdRecipeCustomer.Rows(iFirstStore)
            ElseIf ugrdRecipeCustomer.Rows.Count > 0 Then
                ugrdRecipeCustomer.ActiveRow = ugrdRecipeCustomer.Rows(0)
            End If
        End If
    End Sub

    Private Sub txtDistribution_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtDistribution.Enter
        HighlightText(txtDistribution)
    End Sub

    Private Sub txtDistribution_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtDistribution.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        If KeyAscii = 13 Then
            Call RefreshGrid()
        Else
            '-- Restrict key presses to that type of field
            KeyAscii = ValidateKeyPressEvent(KeyAscii, "Date", txtDistribution, 0, 0, 0)
        End If
        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Private Sub txtDistribution_Leave(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtDistribution.Leave
        Call RefreshGrid()
    End Sub

    Private Sub RefreshGrid()

        If Me.cmbFromLocation.SelectedIndex <> -1 Then
            'Get a list of all customers for the given distribution day and recipe location(Facility)
            Dim sSQL As String
            sSQL = "GetRipeCustomerByRipeZoneLocationDistDate"
            Call LoadDataTable(sSQL, VB6.GetItemData(Me.cmbFromLocation, Me.cmbFromLocation.SelectedIndex), Me.txtDistribution.Text)
        End If
    End Sub


    Private Sub ugrdRecipeCustomer_InitializeLayout(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles ugrdRecipeCustomer.InitializeLayout

    End Sub
End Class