Option Strict Off
Imports Microsoft.Office.Interop.Excel
Imports System.IO
Imports System.Data.DataTable
Imports Infragistics.Win
Imports WholeFoods.IRMA.Pricing.BusinessLogic
Imports WholeFoods.IRMA.Pricing.DataAccess
Imports system.Data.SqlClient
Imports System.Text
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility.SMTP
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.RetailBulkLoad.BusinessLogic
Imports WholeFoods.IRMA.RetailBulkLoad.DataAccess.ImportPricingDataDAO

Public Class ImportPricingData

    Dim WithEvents xlBook As Workbook
    Dim excelWorksheet As Worksheet
    Dim excelSheets As Sheets
    Dim selectedFile As String
    Dim dt As System.Data.DataTable
    Dim validRows As Integer
    Dim mdtStores As System.Data.DataTable
    Private mbFilling As Boolean
    Private IsInitializing As Boolean

#Region "Main Events and Subs"

    Private Sub ImportPricingData_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CenterForm(Me)

        LoadZone(cmbZones)

        '-- Fill out the store list
        mdtStores = StoreDAO.GetRetailStoreList()
        UltraGrid1.DataSource = mdtStores

        Call StoreListGridLoadStatesCombo(mdtStores, cmbStates)

        Call SetCombos()
        ToolStripStatusLabel1.Text = "Ready"
    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub ApplyPriceChanges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ApplyPriceChanges.Click
        ' Apply Changes to Price Batch Detail Table
        Dim i As Integer
        Dim al As New ArrayList
        Dim hasErrors As Boolean
        Dim isRegularPrice As Boolean
        Dim isPromoPrice As Boolean
        Label1.Text = Nothing
        ToolStripStatusLabel1.Text = "Applying Changes"
        If UltraGrid1.Selected.Rows.Count = 0 Then
            MsgBox("No Store Selected!", MsgBoxStyle.Critical, "Load Error")
        ElseIf ugrdList.Rows.Count = 0 Then
            MsgBox("No File Loaded!", MsgBoxStyle.Critical, "Load Error")
        Else
            ' ****** Check for data errors *********
            Try
                For i = 0 To ugrdList.Rows.Count - 1
                    isRegularPrice = False
                    isPromoPrice = False

                    ' ****** Check if Identifier exists in ItemCatalog *********
                    'REQUIRED: Identifier
                    If Not (GetItemInfoByIdentifier(ugrdList.Rows(i).Cells(0).Value)) Then
                        ugrdList.Rows(i).Cells(0).Appearance.BackColor = Color.Red
                        ugrdList.Rows(i).Cells(0).ToolTipText = "Identifier was not found!"
                        hasErrors = True
                    End If

                    If ugrdList.Rows(i).Cells("Reg Unit Price").Value IsNot DBNull.Value Then
                        isRegularPrice = True
                    End If

                    If ugrdList.Rows(i).Cells("Promo Unit Price").Value IsNot DBNull.Value Then
                        isPromoPrice = True
                    End If

                    'REQUIRED: Unit Price (either reg or promo)
                    If Not isRegularPrice AndAlso Not isPromoPrice Then
                        ugrdList.Rows(i).Cells("Reg Unit Price").Appearance.BackColor = Color.Red
                        ugrdList.Rows(i).Cells("Reg Unit Price").ToolTipText = "Reg or Promo Unit Price is required"

                        ugrdList.Rows(i).Cells("Promo Unit Price").Appearance.BackColor = Color.Red
                        ugrdList.Rows(i).Cells("Promo Unit Price").ToolTipText = "Reg or Promo Unit Price is required"
                        hasErrors = True
                    End If

                    'REQUIRED: Price Multiple (either reg or promo)
                    If ugrdList.Rows(i).Cells("Reg Price Multi").Value Is DBNull.Value AndAlso ugrdList.Rows(i).Cells("Promo Price Multi").Value Is DBNull.Value Then
                        'if no price is provided prompt user to provide at least 1 type of multiple
                        If Not isRegularPrice AndAlso Not isPromoPrice Then
                            ugrdList.Rows(i).Cells("Reg Price Multi").Appearance.BackColor = Color.Red
                            ugrdList.Rows(i).Cells("Reg Price Multi").ToolTipText = "Reg or Promo Price Multiple is required"

                            ugrdList.Rows(i).Cells("Promo Price Multi").Appearance.BackColor = Color.Red
                            ugrdList.Rows(i).Cells("Promo Price Multi").ToolTipText = "Reg or Promo Price Multiple is required"
                            hasErrors = True
                        Else
                            If isRegularPrice Then
                                'regular multiple is required
                                ugrdList.Rows(i).Cells("Reg Price Multi").Appearance.BackColor = Color.Red
                                ugrdList.Rows(i).Cells("Reg Price Multi").ToolTipText = "Reg Price Multiple is required"
                            End If

                            If isPromoPrice Then
                                'promo multiple is required
                                ugrdList.Rows(i).Cells("Promo Price Multi").Appearance.BackColor = Color.Red
                                ugrdList.Rows(i).Cells("Promo Price Multi").ToolTipText = "Promo Price Multiple is required"
                            End If
                        End If
                    End If

                    'REQUIRED: Start Date (either Reg or Promo)
                    If ugrdList.Rows(i).Cells("Reg Start").Value Is DBNull.Value AndAlso ugrdList.Rows(i).Cells("Promo Start").Value Is DBNull.Value Then
                        If Not isRegularPrice AndAlso Not isPromoPrice Then
                            ugrdList.Rows(i).Cells("Reg Start").Appearance.BackColor = Color.Red
                            ugrdList.Rows(i).Cells("Reg Start").ToolTipText = "Reg or Promo Start Date is required"

                            ugrdList.Rows(i).Cells("Promo Start").Appearance.BackColor = Color.Red
                            ugrdList.Rows(i).Cells("Promo Start").ToolTipText = "Reg or Promo Start Date is required"
                            hasErrors = True
                        Else
                            If isRegularPrice Then
                                'regular start date is required
                                ugrdList.Rows(i).Cells("Reg Start").Appearance.BackColor = Color.Red
                                ugrdList.Rows(i).Cells("Reg Start").ToolTipText = "Reg Start Date is required"
                            End If

                            If isPromoPrice Then
                                'promo start date is required
                                ugrdList.Rows(i).Cells("Promo Start").Appearance.BackColor = Color.Red
                                ugrdList.Rows(i).Cells("Promo Start").ToolTipText = "Promo Start Date is required"
                            End If
                        End If
                    End If

                    'REQUIRED: End Date *ONLY* If the price being entered is a promo price, the End Date should also become a required field
                    If ugrdList.Rows(i).Cells("Promo Unit Price").Value IsNot DBNull.Value _
                            AndAlso ugrdList.Rows(i).Cells("Promo End").Value Is DBNull.Value Then
                        ugrdList.Rows(i).Cells("Promo End").Appearance.BackColor = Color.Red
                        ugrdList.Rows(i).Cells("Promo End").ToolTipText = "Promo End Date is required"
                        hasErrors = True
                    End If
                Next
            Catch ex As Exception
                MsgBox("Empty Identifier Rows!", MsgBoxStyle.Critical, "SpreadSheet Error")
                Exit Sub
            End Try
            If hasErrors Then
                Label1.ForeColor = Color.DarkRed

                Dim errorMsg As New StringBuilder
                errorMsg.Append("Errors found in SpreadSheet")
                errorMsg.Append(Environment.NewLine)
                errorMsg.Append("Hover on the red cells below to see specific errors")

                Label1.Text = errorMsg.ToString
                Exit Sub
            End If
            ' **************** List of Stores *****************************
            For i = 0 To UltraGrid1.Selected.Rows.Count - 1
                al.Add(UltraGrid1.Selected.Rows(i).Cells(0).Value)
            Next
            ' **************** Update Price Batch Detail Table ************
            Try
                Dim bo As New ImportPricingDataBO(al)
                bo.InsertPriceBatchDetails(dt, ToolStripProgressBar1)
                MsgBox("Data Upload Successful", MsgBoxStyle.Information)
                SendMail()
                dt.Clear()
                TextBox1.Text = Nothing
                ugrdList.DataBind()
                ToolStripStatusLabel1.Text = "Ready"
                UltraGrid1.Selected.Rows.Clear()
            Catch ex As Exception
                Debug.WriteLine(ex.Message)
                MsgBox("Data Upload Failed!", MsgBoxStyle.Critical)
            End Try
        End If
    End Sub

    Private Sub SelectFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectFile.Click
        ' Selecting the Spreadsheet
        'OpenFileDialog1.InitialDirectory = "H:\Offline Files\StoredProcedures"
        ToolStripStatusLabel1.Text = "Search File"
        OpenFileDialog1.ShowDialog()
        selectedFile = OpenFileDialog1.FileName()
        TextBox1.Text = StripFile(selectedFile)
        Label1.Text = Nothing
        Me.ImportData.Enabled = True
        ToolStripStatusLabel1.Text = "Ready"
    End Sub

    Private Sub ImportData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImportData.Click
        ' Importing the Spreadsheet
        ToolStripStatusLabel1.Text = "Importing Spreadsheet"
        Dim fi As New FileInfo(selectedFile)
        If TextBox1.Text = Nothing Then
            MsgBox(ResourcesIRMA.GetString("Select File"), MsgBoxStyle.Critical, Me.Text)
            Exit Sub
        End If
        If Not fi.Extension = ".xls" Then
            MsgBox("File Extension is not Excel (.xls) - Please verify!", MsgBoxStyle.Critical, "Load Error")
            Exit Sub
        End If
        Dim xl As Excel = New Excel(selectedFile)
        ' ******* Prep the DataTable ******
        Try
            dt = xl.getDataTable(createTableColumns(), ToolStripProgressBar1)
        Catch ex As Exception
            Dim errorMsg As New StringBuilder
            errorMsg.Append("Spreadsheet Format incorrect - Please verify!")

            If ex.Message.Contains("The string was not recognized as a valid DateTime") Then
                errorMsg.Append(Environment.NewLine)
                errorMsg.Append("- Verify date formats are MM/DD/YYYY")
            End If

            MsgBox(errorMsg.ToString, MsgBoxStyle.Critical, "Load Error")
        End Try
        xl = Nothing
        ' ******** Format the UPC *****************************
        'dt = FormatUPC(dt)
        ' *****************************************************
        Try
            ugrdList.DataSource = dt
            ugrdList.DataBind()
        Catch ex As Exception
            MsgBox("Data cannot be loaded into the Grid - Please verify!", MsgBoxStyle.Critical, "Load Error")
        End Try
        ToolStripStatusLabel1.Text = "Ready"
    End Sub

    Private Sub ugrdList_AfterRowsDeleted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ugrdList.AfterRowsDeleted
        dt.AcceptChanges()
    End Sub
#End Region

#Region "Mail Notifications"
    Private Sub SendMail()
        ' **** Send Mail out to the blokes ****
        ' *** Parse the DataTable to include into MessageBody ***
        Dim dtMessage As String
        dtMessage = ParseTable(dt)
        ' *******************************************************
        Const host As String = "smtp.WholeFoods.com"
        Dim msgBody As String = "New Retail Price Bulk Upload" & vbCrLf & "Date: " & Date.Now & vbCrLf & "User: " & gsUserName
        msgBody &= dtMessage
        Const msgTo As String = "Alex.Zihlavski@wholefoods.com"
        Const msgCC As String = "Martin.Lux@wholefoods.com;Tricia.Zingone@wholefoods.com"
        Const msgFrom As String = "IRMA@wholefoods.com"
        Const msgSubject As String = "IRMA - New Retail Price Bulk Upload"
        Dim smtp As New SMTP(host)
        Try
            smtp.send(msgBody, msgTo, msgCC, msgFrom, msgSubject)
        Catch ex As Exception
            MsgBox("Mail not sent - " & ex.Message, MsgBoxStyle.Critical, "Mail Error")
        End Try
    End Sub
#End Region

#Region "Formatting Functions"

    Private Function FormatUPC(ByVal dt As System.Data.DataTable) As System.Data.DataTable
        Dim dt1 As System.Data.DataTable = Nothing
        Dim row As DataRow
        For Each row In dt.Rows
            If Len(CStr(row.Item(0))) < 13 Then

            End If
        Next
        Return dt1
    End Function

    Private Function StripFile(ByVal FileName As String) As String
        Dim File As String
        Dim fi As FileInfo = New FileInfo(FileName)
        File = fi.Name
        Return File
    End Function

    Private Function ParseTable(ByVal dt As System.Data.DataTable) As String
        Dim i As Integer
        Dim row As System.Data.DataRow
        Dim tb As String = vbCrLf & "Stores: " & vbCrLf
        For i = 0 To UltraGrid1.Selected.Rows.Count - 1
            tb &= Trim(UltraGrid1.Selected.Rows(i).Cells(1).Value) & vbCrLf
        Next
        tb &= "Items: " & vbCrLf
        For Each row In dt.Rows
            For i = 0 To row.ItemArray.GetUpperBound(0)
                tb &= IIf(row.Item(i) Is DBNull.Value, " - | ", row.Item(i).ToString & " | ")
            Next
            tb &= vbCrLf
        Next
        Return tb
    End Function

#End Region

#Region "Populate UltraGrid"

    Private Sub DisplaySpreadsheetData(ByVal execlWorksheet As Worksheet)
        ' **** Display Data in UltraGrid ******
        Dim i As Integer
        Dim row As DataRow
        For i = 2 To excelWorksheet.UsedRange.Rows.Count
            row = dt.NewRow
        Next
        dt.AcceptChanges()
        ' ** Bind the DataTable to the UltraGrid ***
        Try
            ugrdList.DataSource = dt
            ugrdList.DataBind()
        Catch ex As Exception
            MsgBox("Data cannot be loaded into the Grid - Please verify!", MsgBoxStyle.Critical, "Load Error")
        End Try
    End Sub

    Private Function createTableColumns() As System.Data.DataTable
        Dim dt1 As System.Data.DataTable = New System.Data.DataTable("Columns")
        dt1.Columns.Add(New DataColumn("Identifier", GetType(String)))
        dt1.Columns.Add(New DataColumn("Item Description", GetType(String)))
        dt1.Columns.Add(New DataColumn("Reg Unit Price", GetType(Decimal)))
        dt1.Columns.Add(New DataColumn("Reg Price Multi", GetType(Integer)))
        dt1.Columns.Add(New DataColumn("Reg Start", GetType(Date)))
        dt1.Columns.Add(New DataColumn("Promo Unit Price", GetType(Decimal)))
        dt1.Columns.Add(New DataColumn("Promo Price Multi", GetType(Integer)))
        dt1.Columns.Add(New DataColumn("Promo Start", GetType(Date)))
        dt1.Columns.Add(New DataColumn("Promo End", GetType(Date)))
        dt1.Columns.Add(New DataColumn("EDV", GetType(Boolean)))
        Return dt1
    End Function

#End Region

#Region "Set Store/Zone/State Combos"


    Private Sub SetCombos()

        mbFilling = True

        'Zones.
        If ZoneRadioButton.Checked = True Then
            cmbZones.Enabled = True
            cmbZones.BackColor = System.Drawing.SystemColors.Window
        Else
            cmbZones.SelectedIndex = -1
            cmbZones.Enabled = False
            cmbZones.BackColor = System.Drawing.SystemColors.Control
        End If

        'States.
        If StateRadioButton.Checked = True Then
            cmbStates.Enabled = True
            cmbStates.BackColor = System.Drawing.SystemColors.Window
        Else
            cmbStates.SelectedIndex = -1
            cmbStates.Enabled = False
            cmbStates.BackColor = System.Drawing.SystemColors.Control
        End If

        mbFilling = False

    End Sub

    Private Sub cmbStates_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbStates.SelectedIndexChanged
        Dim iFirstStore As Short

        If IsInitializing Or mbFilling Then Exit Sub

        iFirstStore = -1

        mbFilling = True

        Call StoreListGridSelectByState(UltraGrid1, VB6.GetItemString(cmbStates, cmbStates.SelectedIndex))

        mbFilling = False


    End Sub

    Private Sub cmbZones_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbZones.SelectedIndexChanged

        If mbFilling Or IsInitializing Then Exit Sub
        Call SetCombos()
        If cmbZones.SelectedIndex > -1 Then
            Call StoreListGridSelectByZone(UltraGrid1, VB6.GetItemData(cmbZones, cmbZones.SelectedIndex))
        Else
            UltraGrid1.Selected.Rows.Clear()
        End If

        '        mbFilling = True

        'optSelection(2).Checked = True
        'OptSelection_CheckedChanged(optSelection.Item(2), New System.EventArgs())

    End Sub

    Private Sub AllRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllRadioButton.CheckedChanged
        Call StoreListGridSelectAll(UltraGrid1, True)
    End Sub

    Private Sub ManualRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ManualRadioButton.CheckedChanged
        UltraGrid1.Selected.Rows.Clear()
        cmbZones.SelectedIndex = -1
        cmbStates.SelectedIndex = -1
    End Sub

    Private Sub AllWFMRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllWFMRadioButton.CheckedChanged
        Call StoreListGridSelectAllWFM(UltraGrid1)
    End Sub

#End Region
End Class