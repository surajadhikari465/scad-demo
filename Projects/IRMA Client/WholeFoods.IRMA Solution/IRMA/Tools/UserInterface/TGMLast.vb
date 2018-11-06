Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports Infragistics.Shared
Imports Infragistics.Win
Imports Infragistics.Win.UltraWinDataSource
Imports Infragistics.Win.UltraWinGrid

Friend Class frmTGMLast
	Inherits System.Windows.Forms.Form
    Private IsInitializing As Boolean

    Const ciTGMStoreCorporate As Integer = 0
    Const ciTGMStoreMega As Integer = 1
    Const ciTGMStoreHIAH As Integer = 0
	
    Const ciTGMViewAverage As Integer = 0
    Const ciTGMViewAverage2 As Integer = 1
    Const ciTGMViewAverageMix As Integer = 2
    Const ciTGMViewAverageTGMMix As Integer = 3
    Const ciTGMViewActual As Integer = 4
    Const ciTGMViewActual2 As Integer = 5
    Const ciTGMViewActualMix As Integer = 6
    Const ciTGMViewActualTGMMix As Integer = 7
    Const ciTGMViewCurrent As Integer = 8
    Const ciTGMViewCurrent2 As Integer = 9
    Const ciTGMViewCurrentMix As Integer = 10
    Const ciTGMViewCurrentTGMMix As Integer = 11
    Const ciTGMViewNew As Integer = 12
    Const ciTGMViewNew2 As Integer = 13
    Const ciTGMViewNewMix As Integer = 14
    Const ciTGMViewNewTGMMix As Integer = 15
    Const ciTGMViewInformation As Integer = 16

    Const ciTGMSortByID As Integer = 0
    Const ciTGMSortByTGM As Integer = 1
    Const ciTGMSortBySalesMix As Integer = 2
	
    Const ciTGMSalesMixByCurrentView As Integer = 0
    Const ciTGMSalesMixByCompleteView As Integer = 1
    Const ciTGMSalesMixBySubTeam As Integer = 2

    Private rsReport As ADODB.Recordset
    Private bNewPrices As Boolean
    Private bViewchanged As Boolean


    ' Printing variables
    Private printTitleFont As New Font("MS Sans Serif", 14, System.Drawing.FontStyle.Regular)
    Private printDetailHeaderFont As New Font("MS Sans Serif", 10, System.Drawing.FontStyle.Bold)
    Private printDetailFont As New Font("MS Sans Serif", 10, System.Drawing.FontStyle.Regular)
    Private printHeaderFont As New Font("MS Sans Serif", 8, System.Drawing.FontStyle.Regular)
    Private totalCount As Integer = 0
    Private count As Integer = 0
    Private index As Integer

    Private dtStore As New DataTable("Store")
    Private bClearingCombo As Boolean
    Private bLoading As Boolean


    Private Sub cmbCategory_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbCategory.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)

        Select Case KeyAscii
            Case 8 : cmbCategory.SelectedIndex = -1
            Case 13 : RefreshGrid()
        End Select

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdRefresh_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdRefresh.Click
		
		RefreshGrid()
		
	End Sub
	
   
	
	Private Sub frmTGMLast_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        bLoading = True

		rsReport = New ADODB.Recordset
		
		'-- Center the screen
        Me.Height = cmdExit.Height + cmdExit.Top + 52
        ResizeScreen()
		CenterForm(Me)
		
		'-- Set the caption
		Me.Text = Me.Text & " - " & GetSubTeamName(glTGMTool.SubTeam_No) & " (" & VB6.Format(glTGMTool.StartDate, "MM/DD/YYYY") & " - " & VB6.Format(glTGMTool.EndDate, "MM/DD/YYYY") & ")"
		lblTarget.Text = VB6.Format(GetSubTeamMargin(glTGMTool.SubTeam_No), "#0.00") & "%"
		
        LoadStores()
        cmbMultiStores.SelectedIndex = 0
        cmbStore.SelectedIndex = -1

		'-- Load the drop downs
		LoadCategory(cmbCategory)
		
		'-- Choose the information grid
        'mnuStore.Tag = mnuMultipleStoreList(0).Text
        mnuViews_Click(mnuViews.Item(ciTGMViewInformation), New System.EventArgs())

        bLoading = False
	End Sub
	

    Public Sub mnuCalculator_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuCalculator.Click
        If Me.UltraGrid1.Selected.Rows.Count = 0 Then

            MsgBox("You must select a line item to use the calculator.", MsgBoxStyle.Exclamation, "Notice!")

            Exit Sub
        End If

        '-- Go to edit this item in the list
        If UltraGrid1.Selected.Rows(0).Cells(0).Value >= 1 Then

            gDBReport.BeginTrans()

            '-- Set the detail information for the view
            rsReport.Open("SELECT Identifier, Item_Description, AVG(IIF(ISNULL(NewRetail),CurrentRetail,NewRetail)) AS Retail, AVG(CurrentExtCost) AS Cost " & "FROM TGMStore INNER JOIN TGMTool ON (TGMStore.Store_No = TGMTool.Store_No AND TGMStore.Instance = TGMTool.Instance) " & "WHERE TGMStore.Instance = " & glInstance & " AND TGMStore." & ReturnStore() & " AND Item_Key = " & UltraGrid1.Selected.Rows(0).Cells(0).Value & " " & "GROUP BY Identifier, Item_Description", gDBReport, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

            If Not rsReport.EOF Then

                gtTGMCalculator.Identifier = rsReport.Fields("Identifier").Value
                gtTGMCalculator.Item_Description = rsReport.Fields("Item_Description").Value
                gtTGMCalculator.Retail = rsReport.Fields("Retail").Value
                gtTGMCalculator.Cost = rsReport.Fields("Cost").Value
                If gtTGMCalculator.Retail = 0 Then
                    gtTGMCalculator.Margin = 0
                Else
                    gtTGMCalculator.Margin = (rsReport.Fields("Retail").Value - rsReport.Fields("Cost").Value) / rsReport.Fields("Retail").Value * 100
                End If

            End If

            rsReport.Close()

            gDBReport.CommitTrans()
            If gJetFlush IsNot Nothing Then
                gJetFlush.RefreshCache(gDBReport)
            End If

            frmTGMCalculator.ShowDialog()
            frmTGMCalculator.Dispose()

            If Trim(gtTGMCalculator.Identifier) <> "" Then
                UltraGrid1.Selected.Rows(0).Cells(4).Value = VB6.Format(gtTGMCalculator.Retail, "##0.00")
            End If

        End If

    End Sub

    Public Sub mnuCommitPrices_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuCommitPrices.Click

        If gbSuperUser Then
            If MsgBox("Really update all new retails into the system?", MsgBoxStyle.YesNo + MsgBoxStyle.Question + MsgBoxStyle.DefaultButton2, "Notice") = MsgBoxResult.Yes Then
                PushPrices()
            End If
        Else
            MsgBox("You must have super user access to update prices", MsgBoxStyle.Exclamation, "Invalid Access!")
        End If

    End Sub

    Public Sub mnuExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuExit.Click

        Me.Close()

    End Sub

    Public Sub mnuExportView_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuExportView.Click

        Dim lX, lY As Integer
        Dim sLine As String

        '-- See if there is anything to print
        If UltraGrid1.Rows.Count = 0 Then
            MsgBox("There is no data to export.", MsgBoxStyle.Exclamation, "No Data")
            Exit Sub
        End If

        cdbFileOpen.InitialDirectory = My.Application.Info.DirectoryPath
        cdbFileSave.InitialDirectory = My.Application.Info.DirectoryPath
        cdbFileOpen.CheckFileExists = True
        cdbFileOpen.CheckPathExists = True
        cdbFileSave.CheckPathExists = True
        cdbFileOpen.ShowReadOnly = False
        cdbFileOpen.FileName = ""
        cdbFileSave.FileName = ""
        cdbFileOpen.Filter = "comma seperated values (*.csv)|*.csv"
        cdbFileSave.Filter = "comma seperated values (*.csv)|*.csv"
        cdbFileSave.ShowDialog()
        cdbFileOpen.FileName = cdbFileSave.FileName

        If Trim(cdbFileOpen.FileName) = "" Then Exit Sub
        If Dir(cdbFileOpen.FileName) <> "" Then
            If MsgBox("File already exists. Overwrite it?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Warning!") = MsgBoxResult.No Then
                MsgBox("Export Aborted.", MsgBoxStyle.Exclamation, "Notice")
                Exit Sub
            End If
        End If

        Err.Clear()
        On Error Resume Next
        FileOpen(1, cdbFileOpen.FileName, OpenMode.Output)
        On Error GoTo 0
        If Err.Number <> 0 Then
            FileClose(1)
            MsgBox("Error opening file.  Export Aborted.", MsgBoxStyle.Exclamation, "Notice")
            Exit Sub
        End If

        '-- Print column headers
        sLine = ""
        For lX = 0 To 8
            If UltraGrid1.DisplayLayout.Bands(0).Columns(lX).Hidden = False Then
                If sLine <> "" Then sLine = sLine & ","
                sLine = sLine & UltraGrid1.DisplayLayout.Bands(0).Columns(lX).Header.Caption
            End If
        Next lX
        PrintLine(1, sLine)

        '-- Continue and print the rest of the lines
        For lY = 0 To UltraGrid1.Rows.Count - 1
            sLine = ""
            For lX = 0 To 8
                If UltraGrid1.Rows(lY).Cells(lX).Hidden = False Then
                    If sLine <> "" Then sLine = sLine & ","
                    sLine = sLine & UltraGrid1.Rows(lY).Cells(lX).Value
                End If
            Next lX
            PrintLine(1, sLine)

        Next lY
        FileClose(1)

    End Sub

    Public Sub mnuItemHistory_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuItemHistory.Click
        If Me.UltraGrid1.Selected.Rows.Count = 0 Then

            MsgBox("You must select a line item to edit.", MsgBoxStyle.Exclamation, "Notice!")

            Exit Sub
        End If

        If UltraGrid1.Selected.Rows(0).Cells(0).Value >= 1 Then
            glItemID = CInt(UltraGrid1.Selected.Rows(0).Cells(0).Value)
            frmItemHistory.Identifier = UltraGrid1.Selected.Rows(0).Cells(1).Value.ToString()
            frmItemHistory.Text = "Order History For " & UltraGrid1.Selected.Rows(0).Cells(2).Value.ToString()
            frmItemHistory.ShowDialog()
            frmItemHistory.Dispose()
        End If
    End Sub

    Public Sub mnuItemInformation_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuItemInformation.Click
        If Me.UltraGrid1.Selected.Rows.Count = 0 Then

            MsgBox("You must select a line item to edit.", MsgBoxStyle.Exclamation, "Notice!")

            Exit Sub
        End If

        If Val(UltraGrid1.Selected.Rows(0).Cells(0).Value) >= 1 Then
            glItemID = CInt(UltraGrid1.Selected.Rows(0).Cells(0).Value)
            frmItem.ShowDialog()
            frmItem.Dispose()
        End If
    End Sub


    Public Sub mnuPriceChanges_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuPriceChanges.Click

        If MsgBox("Price changes will only show retails as of the" & vbCrLf & "last time information for the view was updated." & vbCrLf & "Continue?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Notice!") = MsgBoxResult.No Then
            Exit Sub
        End If

        ' ###########################################################################
        ' 10/8/2007 (Robin Eudy) Crystal Report Dependency has been commented out.
        ' ###########################################################################
        MsgBox("TGMLast.vb  mnuPriceChanges_Click(): The Crystal Report TGMPriceChange*.rpt is normally shown here. It has been disabled until the Crystal dependencies can be removed or replaced", MsgBoxStyle.Exclamation)

        'crwReport.SelectionFormula = ""
        'crwReport.Destination = Crystal.DestinationConstants.crptToWindow

        ''This will have to be fixed here for this to work... currently only "TGMPriceChange.rpt" exists. This .rpt would have to be copied to
        ''TGMPriceChange0.rpt and TGMPriceChange1.rpt. The menu option for this function was commented out per Gary. -- Patrick 01/31/2006.
        'crwReport.ReportFileName = My.Application.Info.DirectoryPath & gsReportDirectory & "TGMPriceChange" & glInstance & ".rpt"
        'crwReport.ReportTitle = "TGM Proposed Price Changes"
        'PrintReport(crwReport)

    End Sub

    Public Sub mnuPrintRows_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuPrintRows.Click
        'MsgBox("Until 'Print Top Rows' is re-implemented, use 'Print View'.", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "Temporarily Disabled")
        frmTGMPrintByRow.ShowDialog()
        frmTGMPrintByRow.Dispose()

    End Sub

    Public Sub mnuPrintView_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuPrintView.Click

        glMaxRows = 0
        Call Print_TGM()

    End Sub

    Public Sub mnuSalesMixList_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuSalesMixList.Click
        Dim Index As Integer = mnuSalesMixList.GetIndex(eventSender)

        Dim iLoop As Integer

        For iLoop = mnuSalesMixList.LBound To mnuSalesMixList.UBound
            mnuSalesMixList(iLoop).Checked = (Index = iLoop)
        Next iLoop

        RefreshGrid()

    End Sub

    Public Sub mnuSave_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuSave.Click

        If Trim(glTGMTool.FileName) = "" Then
            Call mnuSaveAs_Click(mnuSaveAs, New System.EventArgs())
        Else
            SaveTGMData()
        End If

    End Sub

    Public Sub mnuSaveAs_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuSaveAs.Click

        cdbFileOpen.InitialDirectory = My.Application.Info.DirectoryPath
        cdbFileSave.InitialDirectory = My.Application.Info.DirectoryPath
        cdbFileOpen.CheckFileExists = True
        cdbFileOpen.CheckPathExists = True
        cdbFileSave.CheckPathExists = True
        cdbFileOpen.ShowReadOnly = False
        cdbFileOpen.Filter = "TGM Tool Files (*.tgm)|*.tgm"
        cdbFileSave.Filter = "TGM Tool Files (*.tgm)|*.tgm"
        cdbFileSave.ShowDialog()
        cdbFileOpen.FileName = cdbFileSave.FileName

        If Dir(cdbFileOpen.FileName) <> "" Then
            If MsgBox("File already exists. Overwrite it?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Warning!") = MsgBoxResult.No Then
                MsgBox("Save Aborted.", MsgBoxStyle.Exclamation, "Notice")
                Exit Sub
            End If
        End If

        Err.Clear()
        On Error Resume Next
        FileOpen(1, cdbFileOpen.FileName, OpenMode.Append)
        FileClose(1)
        On Error GoTo 0
        If Err.Number <> 0 Then
            MsgBox("Error opening file.  Save Aborted.", MsgBoxStyle.Exclamation, "Notice")
            Exit Sub
        End If

        glTGMTool.FileName = cdbFileOpen.FileName
        SaveTGMData()

    End Sub

    Public Sub mnuSortBy_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuSortBy.Click
        Dim Index As Integer = mnuSortBy.GetIndex(eventSender)

        Dim iLoop As Integer

        For iLoop = mnuSortBy.LBound To mnuSortBy.UBound
            mnuSortBy(iLoop).Checked = (Index = iLoop)
        Next iLoop

        RefreshGrid()

    End Sub

    Public Sub mnuViews_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuViews.Click
        Dim Index As Integer = mnuViews.GetIndex(eventSender)

        Dim iLoop As Integer

        For iLoop = mnuViews.LBound To mnuViews.UBound
            mnuViews(iLoop).Checked = (Index = iLoop)

            If Index = iLoop Then mnuView.Tag = mnuViews(iLoop).Text & " View"
        Next iLoop

        RefreshGrid()

    End Sub
    Private Sub UpdateGridTitle()
        Dim sStore As String = String.Empty
        Dim sView As String = mnuView.Tag

        While InStr(1, sView, "&")
            sView = Mid(sView, 1, InStr(1, sView, "&") - 1) & Mid(sView, InStr(1, sView, "&") + 1)
        End While
        If cmbMultiStores.SelectedIndex > -1 Then
            sStore = cmbMultiStores.Text
        ElseIf cmbStore.SelectedIndex > -1 Then
            sStore = cmbStore.Text
        End If
        UltraGrid1.Text = String.Format("{0} {1}", sStore, sView)

    End Sub
    Private Sub RefreshGrid()
        Dim usedWidth As Integer
        Dim newWidth As Integer
        Dim i As Integer
        Dim rowIndex As Integer
        Dim sSQL As String
        Dim sFrom As String
        Dim sWhere As String
        Dim sOrder As String
        Dim sGroup As String
        Dim cSubTeamAvg As Decimal
        Dim cCurrentCur, cCurrentNew, cCurrentAvg, cCurrentAct As Decimal
        Dim cCompleteCur, cCompleteNew, cCompleteAvg, cCompleteAct As Decimal
        Dim cTotalSales As Decimal
        Dim iRowCount As Integer
        sSQL = String.Empty
        sOrder = String.Empty


        '-- Clear the grid
        Me.UltraDataSource1.Rows.Clear()


        '-- Set up the grid
        UltraGrid1.DisplayLayout.Bands(0).Columns(6).Hidden = False
        UltraDataSource1.Band.Columns(6).DataType = GetType(Decimal)
        UltraGrid1.DisplayLayout.Bands(0).Columns(6).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right
        UltraGrid1.DisplayLayout.Bands(0).Columns(6).Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Right
        UltraGrid1.DisplayLayout.Bands(0).Columns(7).Hidden = False
        UltraGrid1.DisplayLayout.Bands(0).Columns(8).Hidden = False

        UpdateGridTitle()

        '-- Set up the from statement
        sFrom = "FROM TGMStore INNER JOIN TGMTool ON (TGMStore.Store_No = TGMTool.Store_No AND TGMStore.Instance = TGMTool.Instance)"

        '-- Set up the where statement
        sWhere = "WHERE TGMStore.Instance = " & glInstance & " AND TGMStore." & ReturnStore()

        '-- get complete totals here
        If mnuSalesMixList(ciTGMSalesMixByCompleteView).Checked Or mnuSalesMixList(ciTGMSalesMixBySubTeam).Checked Then
            gDBReport.BeginTrans()
            rsReport.Open("SELECT SUM(TotalRetail) AS CompleteAvg, " & " SUM(CurrentRetail*TotalQuantity) AS CompleteCur, " & " SUM(IIF(ISNULL(NewRetail),CurrentRetail,NewRetail)*TotalQuantity) AS CompleteNew, " & " SUM(TotalActualRetail) AS CompleteAct " & sFrom & " " & sWhere, gDBReport, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)
            If Not rsReport.EOF Then
                If IsDBNull(rsReport.Fields("CompleteNew").Value) Then
                    cCompleteNew = 0
                Else
                    cCompleteNew = rsReport.Fields("CompleteNew").Value
                End If
                If IsDBNull(rsReport.Fields("CompleteCur").Value) Then
                    cCompleteCur = 0
                Else
                    cCompleteCur = rsReport.Fields("CompleteCur").Value
                End If
                If IsDBNull(rsReport.Fields("CompleteAvg").Value) Then
                    cCompleteAvg = 0
                Else
                    cCompleteAvg = rsReport.Fields("CompleteAvg").Value
                End If
                If IsDBNull(rsReport.Fields("CompleteAct").Value) Then
                    cCompleteAct = 0
                Else
                    cCompleteAct = rsReport.Fields("CompleteAct").Value
                End If
            End If
            rsReport.Close()
            gDBReport.CommitTrans()
            If gJetFlush IsNot Nothing Then
                gJetFlush.RefreshCache(gDBReport)
            End If
        End If

        '-- get subteam totals here
        If mnuSalesMixList(ciTGMSalesMixBySubTeam).Checked Then
            gDBReport.BeginTrans()
            rsReport.Open("SELECT SUM(TotalRetail) AS SubTeamAvg FROM TGMStore INNER JOIN TGMToolHeader ON (TGMStore.Store_No = TGMToolHeader.Store_No) " & sWhere, gDBReport, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)
            If Not rsReport.EOF Then
                If IsDBNull(rsReport.Fields("SubTeamAvg").Value) Then
                    cSubTeamAvg = 0
                Else
                    cSubTeamAvg = rsReport.Fields("SubTeamAvg").Value
                End If
            End If
            rsReport.Close()
            gDBReport.CommitTrans()
            If gJetFlush IsNot Nothing Then
                gJetFlush.RefreshCache(gDBReport)
            End If
        End If

        If cmbCategory.SelectedIndex > -1 Then
            sWhere = sWhere & " AND Category_ID = " & VB6.GetItemData(cmbCategory, cmbCategory.SelectedIndex)
        End If

        If txtField(0).Text > "" Then sWhere = sWhere & " AND Item_Description LIKE '%" & txtField(0).Text & "%'"
        If txtField(1).Text > "" Then sWhere = sWhere & " AND Identifier LIKE '%" & txtField(1).Text & "%'"

        '-- get current totals here
        If mnuSalesMixList(ciTGMSalesMixByCurrentView).Checked Then
            gDBReport.BeginTrans()
            Dim totalsSQL As String
            totalsSQL = "SELECT SUM(TotalRetail) AS CurrentAvg, " & " SUM(CurrentRetail*TotalQuantity) AS CurrentCur, " & " SUM(IIF(ISNULL(NewRetail),CurrentRetail,NewRetail) * TotalQuantity) AS CurrentNew, " & " SUM(TotalActualRetail) AS CurrentAct " & sFrom & " " & sWhere
            rsReport.Open(totalsSQL, gDBReport, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)
            If Not rsReport.EOF Then

                If IsDBNull(rsReport.Fields("CurrentNew").Value) Then
                    cCurrentNew = 0
                Else
                    cCurrentNew = rsReport.Fields("CurrentNew").Value
                End If
                If IsDBNull(rsReport.Fields("CurrentCur").Value) Then
                    cCurrentCur = 0
                Else
                    cCurrentCur = rsReport.Fields("CurrentCur").Value
                End If
                If IsDBNull(rsReport.Fields("CurrentAvg").Value) Then
                    cCurrentAvg = 0
                Else
                    cCurrentAvg = rsReport.Fields("CurrentAvg").Value
                End If
                If IsDBNull(rsReport.Fields("CurrentAct").Value) Then
                    cCurrentAct = 0
                Else
                    cCurrentAct = rsReport.Fields("CurrentAct").Value
                End If
            End If
            rsReport.Close()
            gDBReport.CommitTrans()
            If gJetFlush IsNot Nothing Then
                gJetFlush.RefreshCache(gDBReport)
            End If
        End If

        '-- Set up the specific view
        Select Case True
            Case mnuViews(ciTGMViewAverage).Checked
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Header.Caption = "Avg Price"
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).MinWidth = 70
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Format = "$##0.00#"

                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Header.Caption = "Avg Cost"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).MinWidth = 70
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Format = "$##0.00#"

                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Header.Caption = "Avg ExtCost"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).MinWidth = 80
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Format = "$##0.00#"

                UltraGrid1.DisplayLayout.Bands(0).Columns(8).Header.Caption = "TGM"
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).MinWidth = 70
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).Format = "##0.00%"
                sSQL = "SELECT Item_Key, Identifier, Item_Description, IIF(Sold_By_Weight = -1,'X',' '), AVG(NewRetail), IIF(SUM(TotalQuantity)=0,NULL,SUM(TotalRetail)/SUM(TotalQuantity)), IIF(SUM(TotalQuantity)=0,NULL,SUM(TotalCost)/SUM(TotalQuantity)), IIF(SUM(TotalQuantity)=0,NULL,SUM(TotalExtCost)/SUM(TotalQuantity)), IIf(SUM(TotalRetail)=0,0,(SUM(TotalRetail)-SUM(TotalExtCost))/SUM(TotalRetail))"
            Case mnuViews(ciTGMViewAverage2).Checked
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Header.Caption = "Units"
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).MinWidth = 60
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Format = "#####0.0#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Header.Caption = "Avg Price"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).MinWidth = 70
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Format = "$##0.00#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Header.Caption = "Avg ExtCost"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).MinWidth = 80
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Format = "$##0.00#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).Header.Caption = "TGM"
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).MinWidth = 70
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).Format = "##0.00%"
                sSQL = "SELECT Item_Key, Identifier, Item_Description, IIF(Sold_By_Weight = -1,'X',' '), AVG(NewRetail), SUM(TotalQuantity), IIF(SUM(TotalQuantity)=0,NULL,SUM(TotalRetail)/SUM(TotalQuantity)), IIF(SUM(TotalQuantity)=0,NULL,SUM(TotalExtCost)/SUM(TotalQuantity)), IIf(SUM(TotalRetail)=0,0,(SUM(TotalRetail)-SUM(TotalExtCost))/SUM(TotalRetail))"
            Case mnuViews(ciTGMViewAverageMix).Checked
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Header.Caption = "Units"
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).MinWidth = 60
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Format = "#####0.0#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Header.Caption = "Avg Total"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).MinWidth = 80
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Format = "$####0.00"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Header.Caption = "Avg Sale Mix"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).MinWidth = 90
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Format = "##0.000%"
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).Hidden = True
                Select Case True
                    Case mnuSalesMixList(ciTGMSalesMixByCurrentView).Checked : cTotalSales = cCurrentAvg
                    Case mnuSalesMixList(ciTGMSalesMixByCompleteView).Checked : cTotalSales = cCompleteAvg
                    Case mnuSalesMixList(ciTGMSalesMixBySubTeam).Checked : cTotalSales = cSubTeamAvg
                End Select
                If cTotalSales <> 0 Then
                    sSQL = "SELECT Item_Key, Identifier, Item_Description, IIF(Sold_By_Weight = -1,'X',' '), AVG(NewRetail), SUM(TotalQuantity), SUM(TotalRetail), SUM(TotalRetail)/" & cTotalSales
                Else
                    sSQL = "SELECT Item_Key, Identifier, Item_Description, IIF(Sold_By_Weight = -1,'X',' '), AVG(NewRetail), SUM(TotalQuantity), SUM(TotalRetail), 0"
                End If
            Case mnuViews(ciTGMViewAverageTGMMix).Checked
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Header.Caption = "Units"
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).MinWidth = 60
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Format = "#####0.0#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Header.Caption = "Avg TGM"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).MinWidth = 80
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Format = "##0.000%"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Header.Caption = "Avg Sale Mix"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).MinWidth = 90
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Format = "##0.000%"
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).Hidden = True
                Select Case True
                    Case mnuSalesMixList(ciTGMSalesMixByCurrentView).Checked : cTotalSales = cCurrentAvg
                    Case mnuSalesMixList(ciTGMSalesMixByCompleteView).Checked : cTotalSales = cCompleteAvg
                    Case mnuSalesMixList(ciTGMSalesMixBySubTeam).Checked : cTotalSales = cSubTeamAvg
                End Select
                If cTotalSales <> 0 Then
                    sSQL = "SELECT Item_Key, Identifier, Item_Description, IIF(Sold_By_Weight = -1,'X',' '), AVG(NewRetail), SUM(TotalQuantity), IIf(SUM(TotalRetail)=0,0,(SUM(TotalRetail)-SUM(TotalExtCost))/SUM(TotalRetail)), SUM(TotalRetail)/" & cTotalSales
                Else
                    sSQL = "SELECT Item_Key, Identifier, Item_Description, IIF(Sold_By_Weight = -1,'X',' '), AVG(NewRetail), SUM(TotalQuantity), IIf(SUM(TotalRetail)=0,0,(SUM(TotalRetail)-SUM(TotalExtCost))/SUM(TotalRetail)), 0"
                End If
            Case mnuViews(ciTGMViewActual).Checked
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Header.Caption = "Act Price"
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).MinWidth = 70
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Format = "$##0.00#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Header.Caption = "Act Cost"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).MinWidth = 70
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Format = "$##0.00#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Header.Caption = "Act ExtCost"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).MinWidth = 80
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Format = "$##0.00#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).Header.Caption = "TGM"
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).MinWidth = 70
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).Format = "##0.00%"
                sSQL = "SELECT Item_Key, Identifier, Item_Description, IIF(Sold_By_Weight = -1,'X',' '), AVG(NewRetail), IIF(SUM(TotalQuantity)=0,NULL,SUM(TotalActualRetail)/SUM(TotalQuantity)), IIF(SUM(TotalQuantity)=0,NULL,SUM(TotalCost)/SUM(TotalQuantity)), IIF(SUM(TotalQuantity)=0,NULL,SUM(TotalExtCost)/SUM(TotalQuantity)), IIf(SUM(TotalActualRetail)=0,0,(SUM(TotalActualRetail)-SUM(TotalExtCost))/SUM(TotalActualRetail))"
            Case mnuViews(ciTGMViewActual2).Checked
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Header.Caption = "Units"
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).MinWidth = 60
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Format = "#####0.0#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Header.Caption = "Act Price"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).MinWidth = 70
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Format = "$##0.00#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Header.Caption = "Act ExtCost"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).MinWidth = 80
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Format = "$##0.00#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).Header.Caption = "TGM"
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).MinWidth = 70
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).Format = "##0.00%"
                sSQL = "SELECT Item_Key, Identifier, Item_Description, IIF(Sold_By_Weight = -1,'X',' '), AVG(NewRetail), SUM(TotalQuantity), IIF(SUM(TotalQuantity)=0,NULL,SUM(TotalActualRetail)/SUM(TotalQuantity)), IIF(SUM(TotalQuantity)=0,NULL,SUM(TotalExtCost)/SUM(TotalQuantity)), IIf(SUM(TotalActualRetail)=0,0,(SUM(TotalActualRetail)-SUM(TotalExtCost))/SUM(TotalActualRetail))"
            Case mnuViews(ciTGMViewActualMix).Checked
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Header.Caption = "Units"
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).MinWidth = 60
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Format = "#####0.0#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Header.Caption = "Act Total"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).MinWidth = 80
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Format = "$####0.00"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Header.Caption = "Act Sale Mix"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).MinWidth = 90
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Format = "##0.000%"
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).Hidden = True
                Select Case True
                    Case mnuSalesMixList(ciTGMSalesMixByCurrentView).Checked : cTotalSales = cCurrentAct
                    Case mnuSalesMixList(ciTGMSalesMixByCompleteView).Checked : cTotalSales = cCompleteAct
                    Case mnuSalesMixList(ciTGMSalesMixBySubTeam).Checked : cTotalSales = cSubTeamAvg
                End Select
                If cTotalSales <> 0 Then
                    sSQL = "SELECT Item_Key, Identifier, Item_Description, IIF(Sold_By_Weight = -1,'X',' '), AVG(NewRetail), SUM(TotalQuantity), SUM(TotalActualRetail), SUM(TotalActualRetail)/" & cTotalSales
                Else
                    sSQL = "SELECT Item_Key, Identifier, Item_Description, IIF(Sold_By_Weight = -1,'X',' '), AVG(NewRetail), SUM(TotalQuantity), SUM(TotalActualRetail), 0"
                End If
            Case mnuViews(ciTGMViewActualTGMMix).Checked
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Header.Caption = "Units"
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).MinWidth = 60
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Format = "#####0.0#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Header.Caption = "Act TGM"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).MinWidth = 80
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Format = "##0.000%"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Header.Caption = "Act Sale Mix"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).MinWidth = 90
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Format = "##0.000%"
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).Hidden = True
                Select Case True
                    Case mnuSalesMixList(ciTGMSalesMixByCurrentView).Checked : cTotalSales = cCurrentAct
                    Case mnuSalesMixList(ciTGMSalesMixByCompleteView).Checked : cTotalSales = cCompleteAct
                    Case mnuSalesMixList(ciTGMSalesMixBySubTeam).Checked : cTotalSales = cSubTeamAvg
                End Select
                If cTotalSales <> 0 Then
                    sSQL = "SELECT Item_Key, Identifier, Item_Description, IIF(Sold_By_Weight = -1,'X',' '), AVG(NewRetail), SUM(TotalQuantity), IIf(SUM(TotalActualRetail)=0,0,(SUM(TotalActualRetail)-SUM(TotalExtCost))/SUM(TotalActualRetail)), SUM(TotalActualRetail)/" & cTotalSales
                Else
                    sSQL = "SELECT Item_Key, Identifier, Item_Description, IIF(Sold_By_Weight = -1,'X',' '), AVG(NewRetail), SUM(TotalQuantity), IIf(SUM(TotalActualRetail)=0,0,(SUM(TotalActualRetail)-SUM(TotalExtCost))/SUM(TotalActualRetail)), 0"
                End If
            Case mnuViews(ciTGMViewCurrent).Checked
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Header.Caption = "Cur Price"
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).MinWidth = 70
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Format = "$##0.00#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Header.Caption = "Cur Cost"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).MinWidth = 70
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Format = "$##0.00#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Header.Caption = "Cur ExtCost"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).MinWidth = 80
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Format = "$##0.00#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).Header.Caption = "Cur GM"
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).MinWidth = 70
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).Format = "##0.00%"
                sSQL = "SELECT Item_Key, Identifier, Item_Description, IIF(Sold_By_Weight = -1,'X',' '), AVG(NewRetail), AVG(CurrentRetail), AVG(CurrentCost), AVG(CurrentExtCost), IIf(SUM(CurrentRetail)=0,0,(SUM(CurrentRetail)-SUM(CurrentExtCost))/SUM(CurrentRetail))"
            Case mnuViews(ciTGMViewCurrent2).Checked
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Header.Caption = "Units"
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).MinWidth = 60
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Format = "#####0.0#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Header.Caption = "Cur Price"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).MinWidth = 70
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Format = "$##0.00#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Header.Caption = "Cur ExtCost"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).MinWidth = 80
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Format = "$##0.00#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).Header.Caption = "Cur GM"
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).MinWidth = 70
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).Format = "##0.00%"
                sSQL = "SELECT Item_Key, Identifier, Item_Description, IIF(Sold_By_Weight = -1,'X',' '), AVG(NewRetail), SUM(TotalQuantity), AVG(CurrentRetail), AVG(CurrentExtCost), IIf(SUM(CurrentRetail)=0,0,(SUM(CurrentRetail)-SUM(CurrentExtCost))/SUM(CurrentRetail))"
            Case mnuViews(ciTGMViewCurrentMix).Checked
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Header.Caption = "Units"
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).MinWidth = 60
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Format = "#####0.0#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Header.Caption = "Cur Total"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).MinWidth = 90
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Format = "$####0.00"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Header.Caption = "Cur Sale Mix"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).MinWidth = 90
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Format = "##0.000%"
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).Hidden = True
                Select Case True
                    Case mnuSalesMixList(ciTGMSalesMixByCurrentView).Checked : cTotalSales = cCurrentCur
                    Case mnuSalesMixList(ciTGMSalesMixByCompleteView).Checked : cTotalSales = cCompleteCur
                    Case mnuSalesMixList(ciTGMSalesMixBySubTeam).Checked : cTotalSales = cSubTeamAvg - cCompleteAvg + cCompleteCur
                End Select
                If cTotalSales <> 0 Then
                    sSQL = "SELECT Item_Key, Identifier, Item_Description, IIF(Sold_By_Weight = -1,'X',' '), AVG(NewRetail), SUM(TotalQuantity), SUM(CurrentRetail*TotalQuantity), SUM(CurrentRetail*TotalQuantity)/" & cTotalSales
                Else
                    sSQL = "SELECT Item_Key, Identifier, Item_Description, IIF(Sold_By_Weight = -1,'X',' '), AVG(NewRetail), SUM(TotalQuantity), SUM(CurrentRetail*TotalQuantity), 0"
                End If
            Case mnuViews(ciTGMViewCurrentTGMMix).Checked
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Header.Caption = "Units"
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).MinWidth = 60
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Format = "#####0.0#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Header.Caption = "Cur GM"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).MinWidth = 80
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Format = "##0.000%"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Header.Caption = "Cur Sale Mix"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).MinWidth = 90
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Format = "##0.000%"
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).Hidden = True
                Select Case True
                    Case mnuSalesMixList(ciTGMSalesMixByCurrentView).Checked : cTotalSales = cCurrentCur
                    Case mnuSalesMixList(ciTGMSalesMixByCompleteView).Checked : cTotalSales = cCompleteCur
                    Case mnuSalesMixList(ciTGMSalesMixBySubTeam).Checked : cTotalSales = cSubTeamAvg - cCompleteAvg + cCompleteCur
                End Select
                If cTotalSales <> 0 Then
                    sSQL = "SELECT Item_Key, Identifier, Item_Description, IIF(Sold_By_Weight = -1,'X',' '), AVG(NewRetail), SUM(TotalQuantity), IIf(SUM(CurrentRetail)=0,0,(SUM(CurrentRetail)-SUM(CurrentExtCost))/SUM(CurrentRetail)), SUM(CurrentRetail*TotalQuantity)/" & cTotalSales
                Else
                    sSQL = "SELECT Item_Key, Identifier, Item_Description, IIF(Sold_By_Weight = -1,'X',' '), AVG(NewRetail), SUM(TotalQuantity), IIf(SUM(CurrentRetail)=0,0,(SUM(CurrentRetail)-SUM(CurrentExtCost))/SUM(CurrentRetail)), 0"
                End If
            Case mnuViews(ciTGMViewNew).Checked
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Header.Caption = "Cur Price"
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).MinWidth = 70
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Format = "$##0.00#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Header.Caption = "Cur Cost"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).MinWidth = 70
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Format = "$##0.00#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Header.Caption = "Cur ExtCost"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).MinWidth = 80
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Format = "$##0.00#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).Header.Caption = "New GM"
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).MinWidth = 80
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).Format = "##0.00%"
                sSQL = "SELECT Item_Key, Identifier, Item_Description, IIF(Sold_By_Weight = -1,'X',' '), AVG(NewRetail), AVG(CurrentRetail), AVG(CurrentCost), AVG(CurrentExtCost), IIf(SUM(IIF(ISNULL(NewRetail),CurrentRetail,NewRetail))=0,0,(SUM(IIF(ISNULL(NewRetail),CurrentRetail,NewRetail))-SUM(CurrentExtCost))/SUM(IIF(ISNULL(NewRetail),CurrentRetail,NewRetail)))"
            Case mnuViews(ciTGMViewNew2).Checked
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Header.Caption = "Units"
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).MinWidth = 60
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Format = "#####0.0#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Header.Caption = "Cur Price"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).MinWidth = 70
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Format = "$##0.00#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Header.Caption = "Cur ExtCost"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).MinWidth = 80
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Format = "$##0.00#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).Header.Caption = "New GM"
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).MinWidth = 80
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).Format = "##0.00%"
                sSQL = "SELECT Item_Key, Identifier, Item_Description, IIF(Sold_By_Weight = -1,'X',' '), AVG(NewRetail), SUM(TotalQuantity), AVG(CurrentRetail), AVG(CurrentExtCost), IIf(SUM(IIF(ISNULL(NewRetail),CurrentRetail,NewRetail))=0,0,(SUM(IIF(ISNULL(NewRetail),CurrentRetail,NewRetail))-SUM(CurrentExtCost))/SUM(IIF(ISNULL(NewRetail),CurrentRetail,NewRetail)))"
            Case mnuViews(ciTGMViewNewMix).Checked
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Header.Caption = "Units"
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).MinWidth = 60
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Format = "#####0.0#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Header.Caption = "New Total"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).MinWidth = 68
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Format = "$####0.00"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Header.Caption = "New Sale Mix"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).MinWidth = 90
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Format = "##0.000%"
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).Hidden = True
                Select Case True
                    Case mnuSalesMixList(ciTGMSalesMixByCurrentView).Checked : cTotalSales = cCurrentNew
                    Case mnuSalesMixList(ciTGMSalesMixByCompleteView).Checked : cTotalSales = cCompleteNew
                    Case mnuSalesMixList(ciTGMSalesMixBySubTeam).Checked : cTotalSales = cSubTeamAvg - cCompleteAvg + cCompleteNew
                End Select
                If cTotalSales <> 0 Then
                    sSQL = "SELECT Item_Key, Identifier, Item_Description, IIF(Sold_By_Weight = -1,'X',' '), AVG(NewRetail), SUM(TotalQuantity), SUM(IIF(ISNULL(NewRetail),CurrentRetail,NewRetail)*TotalQuantity), SUM(IIF(ISNULL(NewRetail),CurrentRetail,NewRetail)*TotalQuantity)/" & cTotalSales
                Else
                    sSQL = "SELECT Item_Key, Identifier, Item_Description, IIF(Sold_By_Weight = -1,'X',' '), AVG(NewRetail), SUM(TotalQuantity), SUM(IIF(ISNULL(NewRetail),CurrentRetail,NewRetail)*TotalQuantity), 0"
                End If
            Case mnuViews(ciTGMViewNewTGMMix).Checked
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Header.Caption = "Units"
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).MinWidth = 60
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Format = "#####0.0#"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Header.Caption = "New GM"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).MinWidth = 80
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Format = "##0.000%"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Header.Caption = "New Sale Mix"
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).MinWidth = 90
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Format = "##0.000%"
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).Hidden = True
                Select Case True
                    Case mnuSalesMixList(ciTGMSalesMixByCurrentView).Checked : cTotalSales = cCurrentNew
                    Case mnuSalesMixList(ciTGMSalesMixByCompleteView).Checked : cTotalSales = cCompleteNew
                    Case mnuSalesMixList(ciTGMSalesMixBySubTeam).Checked : cTotalSales = cSubTeamAvg - cCompleteAvg + cCompleteNew
                End Select
                If cTotalSales <> 0 Then
                    sSQL = "SELECT Item_Key, Identifier, Item_Description, IIF(Sold_By_Weight = -1,'X',' '), AVG(NewRetail), SUM(TotalQuantity), IIf(SUM(IIF(ISNULL(NewRetail),CurrentRetail,NewRetail))=0,0,(SUM(IIF(ISNULL(NewRetail),CurrentRetail,NewRetail))-SUM(CurrentExtCost))/SUM(IIF(ISNULL(NewRetail),CurrentRetail,NewRetail))), SUM(IIF(ISNULL(NewRetail),CurrentRetail,NewRetail)*TotalQuantity)/" & cTotalSales
                Else
                    sSQL = "SELECT Item_Key, Identifier, Item_Description, IIF(Sold_By_Weight = -1,'X',' '), AVG(NewRetail), SUM(TotalQuantity), IIf(SUM(IIF(ISNULL(NewRetail),CurrentRetail,NewRetail))=0,0,(SUM(IIF(ISNULL(NewRetail),CurrentRetail,NewRetail))-SUM(CurrentExtCost))/SUM(IIF(ISNULL(NewRetail),CurrentRetail,NewRetail))), 0"
                End If
            Case mnuViews(ciTGMViewInformation).Checked
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Header.Caption = "Cur Price"
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).MinWidth = 70
                UltraGrid1.DisplayLayout.Bands(0).Columns(5).Format = "$##0.00#"
                UltraDataSource1.Band.Columns(6).DataType = GetType(String)
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Header.Caption = "Pkg Desc"
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).MinWidth = 100
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).CellAppearance.TextHAlign = Infragistics.Win.HAlign.Left
                UltraGrid1.DisplayLayout.Bands(0).Columns(6).Header.Appearance.TextHAlign = Infragistics.Win.HAlign.Left
                UltraGrid1.DisplayLayout.Bands(0).Columns(7).Hidden = True
                UltraGrid1.DisplayLayout.Bands(0).Columns(8).Hidden = True
                sSQL = "SELECT Item_Key, Identifier, Item_Description, IIF(Sold_By_Weight = -1,'X',' '), AVG(NewRetail), AVG(CurrentRetail), MAX(Package_Desc)"
        End Select
        For i = 0 To UltraGrid1.DisplayLayout.Bands(0).Columns.Count - 1
            If i <> 2 Then
                If UltraGrid1.DisplayLayout.Bands(0).Columns(i).Hidden = False Then
                    usedWidth = usedWidth + UltraGrid1.DisplayLayout.Bands(0).Columns(i).Width
                End If
            End If
        Next i
        newWidth = UltraGrid1.Size.Width - (usedWidth + 40)
        UltraGrid1.DisplayLayout.Bands(0).Columns(2).MinWidth = newWidth
        UltraGrid1.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns

        '-- Set up the order statement for the grid
        If mnuSortBy(ciTGMSortByID).Checked Then sOrder = "ORDER BY Identifier, Item_Description"
        Select Case True
            Case mnuViews(ciTGMViewAverage).Checked, mnuViews(ciTGMViewAverage2).Checked, mnuViews(ciTGMViewAverageMix).Checked, mnuViews(ciTGMViewAverageTGMMix).Checked
                Select Case True
                    Case mnuSortBy(ciTGMSortByTGM).Checked : sOrder = "ORDER BY IIf(SUM(TotalRetail)=0,0,(SUM(TotalRetail)-SUM(TotalExtCost))/SUM(TotalRetail)) DESC, Identifier, Item_Description"
                    Case mnuSortBy(ciTGMSortBySalesMix).Checked : sOrder = "ORDER BY Avg(TotalRetail) DESC, Identifier, Item_Description"
                End Select
            Case mnuViews(ciTGMViewActual).Checked, mnuViews(ciTGMViewActual2).Checked, mnuViews(ciTGMViewActualMix).Checked, mnuViews(ciTGMViewActualTGMMix).Checked
                Select Case True
                    Case mnuSortBy(ciTGMSortByTGM).Checked : sOrder = "ORDER BY IIf(SUM(TotalActualRetail)=0,0,(SUM(TotalActualRetail)-SUM(TotalExtCost))/SUM(TotalActualRetail)) DESC, Identifier, Item_Description"
                    Case mnuSortBy(ciTGMSortBySalesMix).Checked : sOrder = "ORDER BY Avg(TotalActualRetail) DESC, Identifier, Item_Description"
                End Select
            Case mnuViews(ciTGMViewCurrent).Checked, mnuViews(ciTGMViewCurrent2).Checked, mnuViews(ciTGMViewCurrentMix).Checked, mnuViews(ciTGMViewCurrentTGMMix).Checked
                Select Case True
                    Case mnuSortBy(ciTGMSortByTGM).Checked : sOrder = "ORDER BY IIf(SUM(CurrentRetail)=0,0,(SUM(CurrentRetail)-SUM(CurrentExtCost))/SUM(CurrentRetail)) DESC, Identifier, Item_Description"
                    Case mnuSortBy(ciTGMSortBySalesMix).Checked : sOrder = "ORDER BY Avg(CurrentRetail*TotalQuantity) DESC, Identifier, Item_Description"
                        '                    Case mnuSortBy(ciTGMSortBySalesMix).Checked: sOrder = "ORDER BY format((Sum(CurrentRetail*TotalQuantity)/227531.0892) * 100, ""#0.000"") DESC, Identifier, Item_Description"
                End Select
            Case Else
                Select Case True
                    Case mnuSortBy(ciTGMSortByTGM).Checked : sOrder = "ORDER BY IIf(SUM(IIF(ISNULL(NewRetail),CurrentRetail,NewRetail))=0,0,(SUM(IIF(ISNULL(NewRetail),CurrentRetail,NewRetail))-SUM(CurrentExtCost))/SUM(IIF(ISNULL(NewRetail),CurrentRetail,NewRetail))) DESC, Identifier, Item_Description"
                    Case mnuSortBy(ciTGMSortBySalesMix).Checked : sOrder = "ORDER BY Avg(IIF(ISNULL(NewRetail),CurrentRetail,NewRetail)*TotalQuantity) DESC, Identifier, Item_Description"
                        '                    Case mnuSortBy(ciTGMSortBySalesMix).Checked: sOrder = "ORDER BY  format((Sum(CurrentRetail*TotalQuantity)/227531.0892) * 100, ""#0.000"") DESC, Identifier, Item_Description"
                End Select
        End Select

        '-- Set group for selection formula
        sGroup = "GROUP BY Item_Key, Identifier, Item_Description, Sold_By_Weight"

        '-- Fill it up with the results
        gDBReport.BeginTrans()
        sSQL = sSQL & " " & sFrom & " " & sWhere & " " & sGroup & " " & sOrder
        rsReport.Open(sSQL, gDBReport, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic)

        If Not (rsReport.EOF And rsReport.BOF) Then
            rsReport.MoveLast()
            iRowCount = rsReport.RecordCount
            rsReport.MoveFirst()
        End If

        UltraDataSource1.Rows.SetCount(iRowCount)
        For rowIndex = 0 To iRowCount - 1
            PopGridRow(rsReport.Fields, rowIndex)
            rsReport.MoveNext()
        Next rowIndex

        rsReport.Close()
        gDBReport.CommitTrans()
        If gJetFlush IsNot Nothing Then
            gJetFlush.RefreshCache(gDBReport)
        End If

        '-- Change totals at the bottom
        RefreshTGM()

    End Sub

    Private Sub PopGridRow(ByVal rsFields As ADODB.Fields, ByVal index As Integer)
        Dim row As UltraDataRow
        Dim colIndex As Integer

        row = Me.UltraDataSource1.Rows(index)
        For colIndex = 0 To rsFields.Count - 1
            row(colIndex) = rsFields(colIndex).Value
        Next colIndex
    End Sub

    Private Sub CopyRow(ByVal row As UltraDataRow)
        Dim newRow As UltraDataRow
        Dim colIndex As Integer

        newRow = Me.UltraDataSource2.Rows(row.Index)
        For colIndex = 0 To 8
            newRow(colIndex) = row(colIndex)
        Next colIndex

    End Sub

    Private Sub RefreshTGM()

        gDBReport.BeginTrans()

        rsReport.Open("SELECT SUM(CurrentRetail*TotalQuantity) AS CurRetail, SUM(CurrentExtCost*TotalQuantity) AS CurCost, SUM(TotalActualRetail) AS AvgActualRetail, SUM(TotalRetail) AS AvgRetail, SUM(TotalExtCost) AS AvgCost, SUM(IIF(ISNULL(NewRetail),CurrentRetail,NewRetail)*TotalQuantity) AS NewRetail " & "FROM TGMStore INNER JOIN TGMTool ON (TGMStore.Store_No = TGMTool.Store_No AND TGMStore.Instance = TGMTool.Instance) " & "WHERE TGMStore.Instance = " & glInstance & " AND TGMStore." & ReturnStore(), gDBReport, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

        '-- Change totals at the bottom
        If Not rsReport.EOF Then

            lblAverage(0).Text = VB6.Format(rsReport.Fields("AvgRetail").Value, "######0.00")
            lblAverage(1).Text = VB6.Format(rsReport.Fields("AvgCost").Value, "######0.00")
            If Not IsDBNull(rsReport.Fields("AvgRetail").Value) Then
                If rsReport.Fields("AvgRetail").Value <> 0 Then
                    lblAverage(2).Text = VB6.Format((rsReport.Fields("AvgRetail").Value - rsReport.Fields("AvgCost").Value) / rsReport.Fields("AvgRetail").Value, "##0.000%")
                Else
                    lblAverage(2).Text = "0.000%"
                End If
            Else
                lblAverage(2).Text = "0.000%"
            End If

            lblActual(0).Text = VB6.Format(rsReport.Fields("AvgActualRetail").Value, "######0.00")
            lblActual(1).Text = VB6.Format(rsReport.Fields("AvgCost").Value, "######0.00")
            If Not IsDBNull(rsReport.Fields("AvgActualRetail").Value) Then
                If rsReport.Fields("AvgActualRetail").Value <> 0 Then
                    lblActual(2).Text = VB6.Format((rsReport.Fields("AvgActualRetail").Value - rsReport.Fields("AvgCost").Value) / rsReport.Fields("AvgActualRetail").Value, "##0.000%")
                Else
                    lblActual(2).Text = "0.000%"
                End If
            Else
                lblActual(2).Text = "0.000%"
            End If

            lblCurrent(0).Text = VB6.Format(rsReport.Fields("CurRetail").Value, "######0.00")
            lblCurrent(1).Text = VB6.Format(rsReport.Fields("CurCost").Value, "######0.00")
            If Not IsDBNull(rsReport.Fields("CurRetail").Value) Then
                If rsReport.Fields("CurRetail").Value <> 0 Then
                    lblCurrent(2).Text = VB6.Format((rsReport.Fields("CurRetail").Value - rsReport.Fields("CurCost").Value) / rsReport.Fields("CurRetail").Value, "##0.000%")
                Else
                    lblCurrent(2).Text = "0.000%"
                End If
            Else
                lblCurrent(2).Text = "0.000%"
            End If

            lblNew(0).Text = VB6.Format(rsReport.Fields("NewRetail").Value, "######0.00")
            lblNew(1).Text = VB6.Format(rsReport.Fields("CurCost").Value, "######0.00")
            If Not IsDBNull(rsReport.Fields("NewRetail").Value) Then
                If rsReport.Fields("NewRetail").Value <> 0 Then
                    lblNew(2).Text = VB6.Format((rsReport.Fields("NewRetail").Value - rsReport.Fields("CurCost").Value) / rsReport.Fields("NewRetail").Value, "##0.000%")
                Else
                    lblNew(2).Text = "0.000%"
                End If
            Else
                lblNew(2).Text = "0.000%"
            End If

        End If
        rsReport.Close()

        gDBReport.CommitTrans()
        If gJetFlush IsNot Nothing Then
            gJetFlush.RefreshCache(gDBReport)
        End If

    End Sub

    Private Function ReturnStore() As String
        Dim sReturn As String = "Store_No = TGMStore.Store_No"
        If cmbMultiStores.SelectedIndex > -1 Then
            Select Case cmbMultiStores.Text
                Case "Corporate"
                    sReturn = "Store_No > 0"
                Case "HFM-GA"
                    sReturn = "Mega_Store = -1"
                Case "WFM Stores"
                    sReturn = "WFM_Store = -1"
                Case "WFM-GA"
                    sReturn = "Zone_ID = 6"
                Case "WFM-NC"
                    sReturn = "Zone_ID = 7"

            End Select
        ElseIf cmbStore.SelectedIndex > -1 Then
            Dim strStoreNo As String = cmbStore.SelectedValue.ToString()
            sReturn = IIf(strStoreNo.Length > 0, String.Format("Store_No = {0}", strStoreNo), sReturn)

        End If
        ReturnStore = sReturn

    End Function


    Private Sub txtField_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtField.Enter
        Dim Index As Integer = txtField.GetIndex(eventSender)

        HighlightText(txtField(Index))

    End Sub

    Private Sub txtField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtField.KeyPress
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
        Dim Index As Integer = txtField.GetIndex(eventSender)

        If KeyAscii = 13 Then
            RefreshGrid()
            KeyAscii = 0
        Else
            KeyAscii = ValidateKeyPressEvent(KeyAscii, txtField(Index).Tag, txtField(Index), 0, 0, 0)
        End If

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
    End Sub

    Public Sub SaveTGMData()

        FileOpen(1, Trim(glTGMTool.FileName), OpenMode.Output)

        gDBReport.BeginTrans()

        '-- Set the query base info
        PrintLine(1, "1|" & glTGMTool.SubTeam_No & "|" & VB6.Format(glTGMTool.StartDate, "MM/DD/YYYY") & "|" & VB6.Format(glTGMTool.EndDate, "MM/DD/YYYY") & "|" & glTGMTool.Discontinued & "|" & glTGMTool.HIAH & "|" & Trim(glTGMTool.Query) & "|" & glTGMTool.value)

        '-- Set the stores used at the time
        rsReport.Open("SELECT * FROM TGMStore WHERE Instance = " & glInstance, gDBReport, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)
        While Not rsReport.EOF
            PrintLine(1, "2|" & rsReport.Fields("Store_Name").Value & "|" & rsReport.Fields("Store_No").Value & "|" & IIf(rsReport.Fields("Mega_Store").Value, -1, 0) & "|" & IIf(rsReport.Fields("WFM_Store").Value, -1, 0) & "|" & rsReport.Fields("Zone_ID").Value)
            rsReport.MoveNext()
        End While
        rsReport.Close()

        '-- Set the Total dollars for that time range
        rsReport.Open("SELECT * FROM TGMToolHeader WHERE Instance = " & glInstance, gDBReport, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)
        While Not rsReport.EOF
            PrintLine(1, "3|" & rsReport.Fields("Store_No").Value & "|" & rsReport.Fields("TotalActualRetail").Value & "|" & rsReport.Fields("TotalRetail").Value & "|" & rsReport.Fields("TotalCost").Value & "|" & rsReport.Fields("TotalExtCost").Value)
            rsReport.MoveNext()
        End While
        rsReport.Close()

        '-- Set the detail information for the view
        rsReport.Open("SELECT * FROM TGMTool WHERE Instance = " & glInstance, gDBReport, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)
        While Not rsReport.EOF
            PrintLine(1, "4|" & rsReport.Fields("Item_Key").Value & "|" & rsReport.Fields("Store_No").Value & "|" & rsReport.Fields("Identifier").Value & "|" & rsReport.Fields("Item_Description").Value & "|" & rsReport.Fields("Package_Desc").Value & "|" & rsReport.Fields("Category_ID").Value & "|" & rsReport.Fields("CurrentCost").Value & "|" & rsReport.Fields("CurrentExtCost").Value & "|" & rsReport.Fields("CurrentRetail").Value & "|" & rsReport.Fields("Sold_By_weight").Value & "|" & rsReport.Fields("TotalQuantity").Value & "|" & rsReport.Fields("TotalActualRetail").Value & "|" & rsReport.Fields("TotalRetail").Value & "|" & rsReport.Fields("TotalCost").Value & "|" & rsReport.Fields("TotalExtCost").Value & "|" & rsReport.Fields("NewRetail").Value)
            rsReport.MoveNext()
        End While

        rsReport.Close()

        gDBReport.CommitTrans()
        If gJetFlush IsNot Nothing Then
            gJetFlush.RefreshCache(gDBReport)
        End If

        FileClose(1)

        bViewchanged = False

    End Sub

    Private Sub PushPrices()
        'This must be changed to work with batch pricing structure or removed - this  produces a divide by zero error if run now
        gDBReport.BeginTrans()

        '-- Set the detail information for the view
        rsReport.Open("SELECT Item_Key, Store_No, NewRetail FROM TGMTool WHERE Instance = " & glInstance & " AND (Not ISNull(NewRetail)) AND (NewRetail <> CurrentRetail)", gDBReport, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)
        While Not rsReport.EOF
            ' replace old automatic price update procedure (which was commented out) with a new update, based on new rules

            rsReport.MoveNext()
        End While

        rsReport.Close()

        gDBReport.CommitTrans()
        If gJetFlush IsNot Nothing Then
            gJetFlush.RefreshCache(gDBReport)
        End If

        bNewPrices = False

    End Sub

    Private Sub ResizeScreen()

        Dim lScreenRight, lGridRight, lMovement As Integer
        Dim lLoop As Integer

        lScreenRight = Me.Width
        lGridRight = UltraGrid1.Size.Width

        If Me.Width * 1.1 < System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width Then
            If System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width >= Me.Width * 1.4 Then
                Me.Width = Me.Width * 1.3
            Else
                Me.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width * 0.91
            End If
            Me.Width = (Me.Width \ 15) * 15
        End If

        lMovement = Me.Width - lScreenRight

        '-- widen the grid
        Dim gridSize As Size = New Size(UltraGrid1.Size.Width + lMovement, UltraGrid1.Size.Height)
        UltraGrid1.Size = gridSize
        '-- Move the controls
        For lLoop = 0 To 3
            lblLine(lLoop + 1).Left = lblLine(lLoop + 1).Left + lMovement
        Next lLoop

        For lLoop = 0 To 2
            lblTitle(lLoop).Left = lblTitle(lLoop).Left + lMovement
            lblAverage(lLoop).Left = lblAverage(lLoop).Left + lMovement
            lblActual(lLoop).Left = lblActual(lLoop).Left + lMovement
            lblCurrent(lLoop).Left = lblCurrent(lLoop).Left + lMovement
            lblNew(lLoop).Left = lblNew(lLoop).Left + lMovement
        Next lLoop

        cmdRefresh.Left = cmdRefresh.Left + lMovement
        cmdExit.Left = cmdExit.Left + lMovement


    End Sub
    Public Sub PopGridTopRows()
        Dim i As Integer

        If glMaxRows > 0 And UltraDataSource1.Rows.Count > glMaxRows Then
            UltraDataSource2.Rows.Clear()
            UltraDataSource2.Rows.SetCount(glMaxRows)

            For i = 0 To UltraDataSource1.Rows.Count - 1
                If i < glMaxRows Then
                    Try
                        CopyRow(UltraDataSource1.Rows(i))
                    Catch
                    End Try
                End If
            Next i
        End If
        UltraGrid1.DataSource = UltraDataSource2
        UltraGrid1.Refresh()

    End Sub
    Public Sub ReBindGrid()
        UltraGrid1.DataSource = UltraDataSource1
        UltraGrid1.Refresh()

    End Sub
    Public Sub Print_TGM()

        UltraGridPrintDocument1.Header.TextCenter = UltraGrid1.Text + vbCrLf + Me.Text
        UltraGridPrintDocument1.Footer.TextLeft = Now

        UltraPrintPreviewDialog1.ShowDialog(Me)

    End Sub


    Private Sub frmTGMLast_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If bViewchanged Or Trim(glTGMTool.FileName) = "" Then
            Select Case MsgBox("Save this TGM view for later use?", MsgBoxStyle.YesNoCancel + MsgBoxStyle.Question + MsgBoxStyle.DefaultButton1, "Save View")
                Case MsgBoxResult.Yes : mnuSave_Click(mnuSave, New System.EventArgs())
                Case MsgBoxResult.Cancel
                    e.Cancel = 1 : Exit Sub
            End Select
        End If

        rsReport = Nothing

    End Sub

    Private Sub UltraGrid1_AfterCellActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles UltraGrid1.AfterCellActivate

        If UltraGrid1.ActiveCell.Column.CellActivation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit Then
            UltraGrid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
        End If

    End Sub

    Private Sub UltraGrid1_AfterCellUpdate(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles UltraGrid1.AfterCellUpdate
        If IsInitializing Then Exit Sub

        Dim curRow As UltraGridRow = CType(sender, UltraGrid).ActiveRow

        If e.Cell.Column.Index <> 4 Then Exit Sub

        If e.Cell.DataChanged Then
            Dim sSQL As String
            Dim iItemKey As Integer
            Dim dNewPrice As Decimal

            gDBReport.BeginTrans()

            bNewPrices = True
            bViewchanged = True
            dNewPrice = CDec(e.Cell.Text)
            iItemKey = curRow.Cells(0).Value

            '-- Update to the new price
            sSQL = "UPDATE TGMTool " & "SET NewRetail = " & _
                IIf(dNewPrice > 0, dNewPrice, "NULL") & " " & _
                "WHERE Instance = " & glInstance & " AND Item_Key = " & iItemKey & _
                " AND Store_No IN (SELECT Store_No FROM TGMStore WHERE Instance = " & glInstance & _
                " AND " & ReturnStore() & ")"

            gDBReport.Execute(sSQL, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            If mnuViews(ciTGMViewNew).Checked Then

                '-- Update margin on the right when applicable
                rsReport.Open("SELECT IIf(Avg(IIF(ISNULL(NewRetail),CurrentRetail,NewRetail))=0,0,(Avg(IIF(ISNULL(NewRetail),CurrentRetail,NewRetail))-Avg(CurrentExtCost))/Avg(IIF(ISNULL(NewRetail),CurrentRetail,NewRetail))) AS TGM " & "FROM TGMStore INNER JOIN TGMTool ON (TGMStore.Store_No = TGMTool.Store_No AND TGMStore.Instance = TGMTool.Instance) " & "WHERE TGMStore.Instance = " & glInstance & " AND Item_Key = " & iItemKey & " AND TGMStore." & ReturnStore(), gDBReport, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

                If Not rsReport.EOF Then
                    curRow.Cells(8).Value = rsReport.Fields("tgm").Value
                End If
                rsReport.Close()

            End If

            gDBReport.CommitTrans()
            If gJetFlush IsNot Nothing Then
                gJetFlush.RefreshCache(gDBReport)
            End If

        End If

    End Sub


    Private Sub UltraGrid1_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles UltraGrid1.DoubleClickRow
        If Me.UltraGrid1.Selected.Rows.Count = 0 Then

            MsgBox("You must select a line item to edit.", MsgBoxStyle.Exclamation, "Notice!")

            Exit Sub
        End If
        '-- Go to edit this item in the list

        If e.Row.Cells(0).Text >= 1 Then

            glItemID = e.Row.Cells(0).Text
            frmItem.ShowDialog()
            frmItem.Dispose()

        End If

    End Sub

    Private Sub UltraGrid1_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles UltraGrid1.InitializeLayout
        e.Layout.Bands(0).Columns("ID").Hidden = True

        e.Layout.Bands(0).Columns("Identifier").MinWidth = 80
        e.Layout.Bands(0).Columns("Wt").MinWidth = 30
        e.Layout.Bands(0).Columns("Wt").MaxWidth = 30
        'e.Layout.Bands(0).Columns("New Price").EditorControl = Me.UltraCurrencyEditor1
        e.Layout.Bands(0).Columns("New Price").MinWidth = 70
        e.Layout.Bands(0).Columns("New Price").MaxWidth = 70
        e.Layout.Bands(0).Columns("New Price").Format = "#,##0.0#"
        e.Layout.Bands(0).Columns("New Price").CellAppearance.TextHAlign _
          = Infragistics.Win.HAlign.Right
        e.Layout.Bands(0).Columns("5").CellAppearance.TextHAlign _
          = Infragistics.Win.HAlign.Right
        e.Layout.Bands(0).Columns("5").Header.Appearance.TextHAlign _
          = Infragistics.Win.HAlign.Right
        e.Layout.Bands(0).Columns("6").CellAppearance.TextHAlign _
          = Infragistics.Win.HAlign.Right
        e.Layout.Bands(0).Columns("6").Header.Appearance.TextHAlign _
          = Infragistics.Win.HAlign.Right
        e.Layout.Bands(0).Columns("7").CellAppearance.TextHAlign _
          = Infragistics.Win.HAlign.Right
        e.Layout.Bands(0).Columns("7").Header.Appearance.TextHAlign _
          = Infragistics.Win.HAlign.Right
        e.Layout.Bands(0).Columns("8").CellAppearance.TextHAlign _
          = Infragistics.Win.HAlign.Right
        e.Layout.Bands(0).Columns("8").Header.Appearance.TextHAlign _
          = Infragistics.Win.HAlign.Right
        e.Layout.Bands(0).Columns(1).CellActivation = Activation.NoEdit
        e.Layout.Bands(0).Columns(2).CellActivation = Activation.NoEdit
        e.Layout.Bands(0).Columns(3).CellActivation = Activation.NoEdit
        e.Layout.Bands(0).Columns(5).CellActivation = Activation.NoEdit
        e.Layout.Bands(0).Columns(6).CellActivation = Activation.NoEdit
        e.Layout.Bands(0).Columns(7).CellActivation = Activation.NoEdit
        e.Layout.Bands(0).Columns(8).CellActivation = Activation.NoEdit
    End Sub

    Private Sub UltraGridPrintDocument1_PagePrinting(ByVal sender As Object, ByVal e As Infragistics.Win.Printing.PagePrintingEventArgs) Handles UltraGridPrintDocument1.PagePrinting

        Me.UltraGridPrintDocument1.Footer.TextRight = String.Format("Page {0}", e.Document.PageNumber)

    End Sub
    'Use this for grid navigation capability.
    Private Sub UltraGrid1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles UltraGrid1.KeyDown
        Select Case e.KeyValue
            Case Keys.Up
                UltraGrid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, False, False)
                UltraGrid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.AboveCell, False, False)
                e.Handled = True
                UltraGrid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
            Case Keys.Down
                UltraGrid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, False, False)
                UltraGrid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.BelowCell, False, False)
                e.Handled = True
                UltraGrid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
            Case Keys.Right
                UltraGrid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, False, False)
                UltraGrid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.NextCellByTab, False, False)
                e.Handled = True
                UltraGrid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
            Case Keys.Left
                UltraGrid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode, False, False)
                UltraGrid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.PrevCellByTab, False, False)
                e.Handled = True
                UltraGrid1.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, False, False)
        End Select
    End Sub

    Private Sub LoadStores()
        Dim row As DataRow
        Dim Keys(0) As DataColumn
        Dim dc As DataColumn

        dtStore.PrimaryKey = Keys
        dc = New DataColumn("Store_No", GetType(Integer))
        dtStore.Columns.Add(dc)
        Keys(0) = dc

        'Visible in the combo.
        '--------------------
        dtStore.Columns.Add(New DataColumn("Store_Name", GetType(String)))

        '-- Fill the specific store list
        gDBReport.BeginTrans()

        rsReport.Open("SELECT * FROM TGMStore WHERE Instance = " & glInstance & " ORDER BY Mega_Store, WFM_Store, Store_Name", gDBReport, ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)
        While Not rsReport.EOF
            row = dtStore.NewRow
            row("Store_No") = rsReport.Fields("Store_No").Value
            row("Store_Name") = rsReport.Fields("Store_Name").Value
            dtStore.Rows.Add(row)
            rsReport.MoveNext()
        End While

        rsReport.Close()
        gDBReport.CommitTrans()
        If gJetFlush IsNot Nothing Then
            gJetFlush.RefreshCache(gDBReport)
        End If

        cmbStore.DataSource = dtStore
        cmbStore.DisplayMember = "Store_Name"
        cmbStore.ValueMember = "Store_No"

    End Sub

    Private Sub cmbMultiStores_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbMultiStores.KeyPress
        Dim KeyAscii As Integer = Asc(e.KeyChar)

        Select Case KeyAscii
            Case 8 : cmbMultiStores.SelectedIndex = -1
            Case 13 : RefreshGrid()
        End Select

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If

    End Sub

    Private Sub cmbStore_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbStore.KeyPress
        Dim KeyAscii As Integer = Asc(e.KeyChar)

        Select Case KeyAscii
            Case 8 : cmbStore.SelectedIndex = -1
            Case 13 : RefreshGrid()
        End Select

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If

    End Sub

    Private Sub cmbMultiStores_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMultiStores.SelectedIndexChanged
        If IsInitializing = True Then Exit Sub
        If bLoading = True Then Exit Sub

        If Not bClearingCombo Then
            bClearingCombo = True

            If cmbMultiStores.SelectedIndex > -1 Then
                RefreshGrid()
                cmbStore.SelectedIndex = -1
                ' needed to do this twice - appears to be a timing issue
                cmbStore.SelectedIndex = -1
            End If
            bClearingCombo = False
        End If
    End Sub

    Private Sub cmbStore_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore.SelectedIndexChanged
        If IsInitializing = True Then Exit Sub
        If bLoading = True Then Exit Sub

        If Not bClearingCombo Then
            bClearingCombo = True

            If cmbStore.SelectedIndex > -1 Then
                RefreshGrid()
                cmbMultiStores.SelectedIndex = -1
            End If
            bClearingCombo = False
        End If
    End Sub
End Class