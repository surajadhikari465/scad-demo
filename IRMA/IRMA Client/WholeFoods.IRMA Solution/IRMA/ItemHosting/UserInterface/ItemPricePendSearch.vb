Option Strict Off
Option Explicit On

Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic

Friend Class frmItemPricePendSearch
	Inherits System.Windows.Forms.Form
	
	Dim mrsStore As ADODB.Recordset
	Dim mrsPendPrice As ADODB.Recordset
	
    Dim msUser_ID_Date As String

    Dim mdt As DataTable
    Dim mdv As DataView

    Dim IsInitializing As Boolean

    Private Sub frmItemPricePendSearch_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        Call LoadData()

        SetActive(cmbStore, False)
        SetActive(cmbZones, False)
        SetActive(cmbState, False)

        _optSelection_4.Checked = True
        _optSelection_5.Checked = False
        _optSelection_5.Visible = False

        Call SetupDataTable()

        RefreshData()

    End Sub

    Private Sub SetupDataTable()

        ' Create a data table
        mdt = New DataTable("ItemHistory")

        'Visible on grid.
        '--------------------
        mdt.Columns.Add(New DataColumn("Store Name", GetType(String)))
        mdt.Columns.Add(New DataColumn("Start Date", GetType(String)))
        mdt.Columns.Add(New DataColumn("Type", GetType(String)))
        mdt.Columns.Add(New DataColumn("POS Price", GetType(String)))
        mdt.Columns.Add(New DataColumn("Price", GetType(String)))
        mdt.Columns.Add(New DataColumn("Priority", GetType(String)))
        mdt.Columns.Add(New DataColumn("Sale End", GetType(String)))
        mdt.Columns.Add(New DataColumn("Batch Status", GetType(String)))

        'Hidden on grid.
        '--------------------
        mdt.Columns.Add(New DataColumn("ID", GetType(String)))

    End Sub

    Private Sub LoadData()

        On Error GoTo ExitSub

        Dim sPrevZone As String
        Dim sPrevState As String
        sPrevState = String.Empty
        sPrevZone = String.Empty

        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        gRSRecordset = SQLOpenRecordSet("EXEC GetRetailStores", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        CreateEmptyADORS_FromDAO(gRSRecordset, mrsStore, bAllowNulls:=False)
        mrsStore.Open()

        While Not gRSRecordset.EOF

            CopyDAORecordToADORecord(gRSRecordset, mrsStore, bAllowNulls:=False)

            cmbStore.Items.Add(New VB6.ListBoxItem(gRSRecordset.Fields("Store_Name").Value, gRSRecordset.Fields("Store_No").Value))

            gRSRecordset.MoveNext()
        End While

        If mrsStore.RecordCount > 0 Then

            mrsStore.Sort = "Zone_Name"
            Do
                If mrsStore.Fields("Zone_Name").Value <> sPrevZone Then
                    sPrevZone = mrsStore.Fields("Zone_Name").Value
                    cmbZones.Items.Add(New VB6.ListBoxItem(mrsStore.Fields("Zone_Name").Value, mrsStore.Fields("Zone_ID").Value))
                End If

                mrsStore.MoveNext()
            Loop Until mrsStore.EOF

            mrsStore.Sort = "State"
            Do
                If (mrsStore.Fields("State").Value <> sPrevState) And (mrsStore.Fields("State").Value <> "") Then
                    sPrevState = mrsStore.Fields("State").Value
                    cmbState.Items.Add(mrsStore.Fields("State").Value)
                End If

                mrsStore.MoveNext()
            Loop Until mrsStore.EOF

            mrsStore.Sort = ""
        End If

ExitSub:
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        If gRSRecordset IsNot Nothing Then
            gRSRecordset.Close()
            gRSRecordset = Nothing
        End If

    End Sub

    Private Sub PopGrid()
        Dim cPrice As Decimal
        Dim cPOSPrice As Decimal
        Dim iMultiple As Short
        Dim row As DataRow
        Dim sDateFormat As String = ResourcesIRMA.GetString("DateStringFormat")
        Dim dtDate As Date

        'Load the data set.
        mdt.Rows.Clear()

        If mrsPendPrice.RecordCount > 0 Then
            mrsPendPrice.MoveFirst()

            While (Not mrsPendPrice.EOF)
                If mrsPendPrice.Fields("On_Sale").Value = True Then
                    cPrice = mrsPendPrice.Fields("Sale_Price").Value
                    cPOSPrice = mrsPendPrice.Fields("POSSale_Price").Value
                    iMultiple = IIf(mrsPendPrice.Fields("Sale_Multiple").Value > 0, mrsPendPrice.Fields("Sale_Multiple").Value, 1)
                Else
                    cPrice = mrsPendPrice.Fields("Price").Value
                    cPOSPrice = mrsPendPrice.Fields("POSPrice").Value
                    iMultiple = IIf(mrsPendPrice.Fields("Multiple").Value > 0, mrsPendPrice.Fields("Multiple").Value, 1)
                End If

                row = mdt.NewRow

                row("ID") = mrsPendPrice.Fields("PriceBatchDetailID").Value
                row("Store Name") = mrsPendPrice.Fields("Store_Name").Value
                dtDate = mrsPendPrice.Fields("StartDate").Value
                'SV 08/30/2006: Change the formatting so that the correct date format is displayed based on user's locale
                row("Start Date") = IIf(dtDate > DateTime.MinValue, dtDate.ToString("d"), String.Empty)
                row("Type") = mrsPendPrice.Fields("PriceChgTypeDesc").Value
                If (mrsPendPrice.Fields("On_Sale").Value = True AndAlso InstanceDataDAO.IsFlagActive("AllowZeroSalePrice")) Or _
                   (mrsPendPrice.Fields("On_Sale").Value = False AndAlso InstanceDataDAO.IsFlagActive("AllowZeroRegPrice")) Then
                    row("POS Price") = IIf(cPOSPrice >= 0, iMultiple & " @ " & VB6.Format(cPOSPrice, "####0.00"), "")
                    row("Price") = IIf(cPrice >= 0, iMultiple & " @ " & VB6.Format(cPrice, "####0.00"), "")
                Else
                    row("POS Price") = IIf(cPOSPrice > 0, iMultiple & " @ " & VB6.Format(cPOSPrice, "####0.00"), "")
                    row("Price") = IIf(cPrice > 0, iMultiple & " @ " & VB6.Format(cPrice, "####0.00"), "")
                End If
                row("Priority") = mrsPendPrice.Fields("Priority").Value
                'TSP 08/09/2006: changed the way NULL dates were handled; adding a "" after the date caused the format string to be ignored
                dtDate = mrsPendPrice.Fields("Sale_End_Date").Value
                'SV 08/30/2006: Change the formatting so that the correct date format is displayed based on user's locale
                row("Sale End") = IIf(dtDate > DateTime.MinValue, dtDate.ToString("d"), String.Empty)
                row("Batch Status") = mrsPendPrice.Fields("PriceBatchStatusDesc").Value

                mdt.Rows.Add(row)

                mrsPendPrice.MoveNext()
            End While

            mdt.AcceptChanges()
            mdv = New System.Data.DataView(mdt)
            mdv.Sort = "Store Name"
            ugrdList.DataSource = mdv

        End If

        If ugrdList.Rows.Count > 0 Then
            ugrdList.Rows(0).Selected = True
            cmdFunction(2).Enabled = True
            cmdFunction(3).Enabled = True
        Else
            cmdFunction(2).Enabled = False
            cmdFunction(3).Enabled = False
        End If

        ugrdList.Selected.Rows.Clear()
        ugrdList.ActiveRow = Nothing

    End Sub

    Public ReadOnly Property LockDate() As String
        Get
            LockDate = msUser_ID_Date
        End Get
    End Property
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	Private Sub cmdFilter_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFilter.Click
		
		Dim sStores As String
		Dim sState As String
		Dim lZone_ID As Integer
		Dim sFilter As String
        sStores = String.Empty
        sState = String.Empty
        sFilter = String.Empty

		Select Case True
			Case optSelection(0).Checked
				sStores = ComboValue(cmbStore)
				
			Case optSelection(1).Checked
				If cmbZones.SelectedIndex = -1 Then
                    MsgBox(ResourcesItemHosting.GetString("SelectZone"), MsgBoxStyle.Critical, Me.Text)
					Exit Sub
				Else
					lZone_ID = CInt(ComboValue(cmbZones))
				End If
				
			Case optSelection(2).Checked
				If cmbState.SelectedIndex = -1 Then
                    MsgBox(ResourcesItemHosting.GetString("SelectState"), MsgBoxStyle.Critical, Me.Text)
					Exit Sub
				Else
					sState = cmbState.Text
				End If
		End Select
		
		If Len(sStores) = 0 Then
			sStores = GetStoreListString(lZone_ID, sState, optSelection(3).Checked, optSelection(4).Checked, optSelection(5).Checked)
            sFilter = "Store_No = " & Replace(sStores, "|", " OR Store_No = ")
        Else
            sFilter = "Store_No = " & sStores
        End If

        If sFilter = "Store_No = " Then sFilter = "Store_No = 0"

        mrsPendPrice.Filter = ADODB.FilterGroupEnum.adFilterNone
        mrsPendPrice.Filter = sFilter
        
        PopGrid()

    End Sub

    Private Sub cmdFunction_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFunction.Click
        Dim Index As Short = cmdFunction.GetIndex(eventSender)

        Dim lPriceBatchDetailID As Integer
        Dim lPriceChgTypeID As Integer
        Dim bIsOnSale As Boolean
        Dim iLoop As Short
        Dim bCouldNotDelete As Boolean
        Dim lStore_No As Integer
        Dim iMultiple As Short
        Dim cPrice As Decimal
        Dim cMSRPPrice As Decimal
        Dim cPOSPrice As Decimal
        Dim cAvgCost As Decimal

        Dim itemInfo As New ItemBO
        itemInfo.Item_Key = glItemID
        itemInfo.ItemDescription = gsItemDescription

        'On Error GoTo me_err

        Select Case Index
            Case 0
                'Add price change.
                '.StartDate = DateAdd(Microsoft.VisualBasic.DateInterval.Day, 1, SystemDateTime)
                frmItemPrice.GetRegPriceInfo(lStore_No, iMultiple, cPrice, cMSRPPrice, cPOSPrice, cAvgCost)
                Dim fPriceChange As New frmPriceChange
                With fPriceChange
                    .StoreNo = lStore_No
                    .Multiple = iMultiple
                    .POSPrice = cPOSPrice
                    .AvgCost = cAvgCost
                    .ItemBO = itemInfo
                    'this was removed: .StartDate = DateAdd(Microsoft.VisualBasic.DateInterval.Day, 1, SystemDateTime)
                    .ShowDialog()
                    .Dispose()
                    GC.Collect()
                End With

            Case 1
                'Add promotional (sale) change.
                frmItemPrice.GetRegPriceInfo(lStore_No, iMultiple, cPrice, cMSRPPrice, cPOSPrice, cAvgCost)
                Dim fPromotionChange As New frmPromotionChange
                With fPromotionChange
                    .Text = "Promotion Information - [" & gsItemDescription & "]"
                    .EditPriceBatchDetailID = -1
                    .StoreNo = lStore_No
                    .RegularMultiple = iMultiple
                    .RegularPOSPrice = cPOSPrice

                    ' 11/13/07 reverted back by David Marine as per Frank
                    .AllowRegularPriceChange = True

                    .SaleMultiple = iMultiple
                    .POSSalePrice = cPOSPrice
                    .POSSalePrice1 = cPOSPrice
                    .MSRPPrice = cMSRPPrice
                    .ItemBO = itemInfo
                    .ShowDialog()
                    '.Dispose()
                    GC.Collect()
                End With

            Case 2
                'Edit price (reg or promotional).
                If (ugrdList.Selected.Rows.Count = 1) Then
                    lPriceBatchDetailID = ugrdList.Selected.Rows(0).Cells("ID").Value
                ElseIf (ugrdList.Rows.Count = 1) Then
                    lPriceBatchDetailID = ugrdList.Rows(0).Cells("ID").Value
                Else
                    MsgBox(ResourcesIRMA.GetString("SelectSingleRow"), MsgBoxStyle.Critical, Me.Text)
                    GoTo me_exit
                End If

                lPriceChgTypeID = GetPriceChgType(lPriceBatchDetailID)
                bIsOnSale = GetOnSale(lPriceBatchDetailID)

                ' need to do different things if we're sale or not.  

                If (lPriceChgTypeID = 0) Then
                    MsgBox(ResourcesIRMA.GetString("CannotChangeRow"), MsgBoxStyle.Critical, Me.Text)
                    GoTo me_exit
                End If

                If (bIsOnSale) Then
                    'Promo Price
                    mrsPendPrice.Filter = ADODB.FilterGroupEnum.adFilterNone
                    mrsPendPrice.Filter = "PriceBatchDetailID = " & lPriceBatchDetailID
                    Dim fPromotionChange As New frmPromotionChange
                    With fPromotionChange
                        .EditPriceBatchDetailID = lPriceBatchDetailID
                        .StoreNo = mrsPendPrice.Fields("Store_No").Value
                        .ItemBO = itemInfo
                        frmItemPrice.GetRegPriceInfo(mrsPendPrice.Fields("Store_No").Value, iMultiple, cPrice, cMSRPPrice, cPOSPrice, cAvgCost)
                        If cPrice = 0 Then
                            .RegularMultiple = mrsPendPrice.Fields("Multiple").Value
                            .RegularPOSPrice = mrsPendPrice.Fields("POSPrice").Value
                            '.AllowRegularPriceChange = True
                        Else
                            '.AllowRegularPriceChange = False
                            .RegularMultiple = iMultiple
                            .RegularPOSPrice = cPOSPrice
                        End If

                        'always allow user to edit regular price when creating a promo price (per End Sale Early functionality)
                        .AllowRegularPriceChange = True

                        .SaleMultiple = mrsPendPrice.Fields("Sale_Multiple").Value
                        .POSSalePrice = mrsPendPrice.Fields("POSSale_Price").Value
                        .StartDate = mrsPendPrice.Fields("StartDate").Value
                        .EndDate = mrsPendPrice.Fields("Sale_End_Date").Value
                        .PricingMethodID = mrsPendPrice.Fields("PricingMethod_ID").Value
                        .PriceType = mrsPendPrice.Fields("PriceChgTypeID").Value
                        .MSRPMultiple = IIf(mrsPendPrice.Fields("MSRPMultiple").Value <> Nothing, mrsPendPrice.Fields("MSRPMultiple").Value, 1)
                        .MSRPPrice = mrsPendPrice.Fields("MSRPPrice").Value
                        .EarnedDiscount1 = mrsPendPrice.Fields("Sale_Earned_Disc1").Value
                        .EarnedDiscount2 = mrsPendPrice.Fields("Sale_Earned_Disc2").Value
                        .EarnedDiscount3 = mrsPendPrice.Fields("Sale_Earned_Disc3").Value
                        mrsPendPrice.Filter = ADODB.FilterGroupEnum.adFilterNone
                        .ShowDialog(Me)
                        .Dispose()
                    End With
                Else
                    ' Regular Price
                    mrsPendPrice.Filter = ADODB.FilterGroupEnum.adFilterNone
                    mrsPendPrice.Filter = "PriceBatchDetailID = " & lPriceBatchDetailID

                    Dim fpricechange As New frmPriceChange
                    With fpricechange
                        .StoreNo = mrsPendPrice.Fields("Store_No").Value
                        .Multiple = mrsPendPrice.Fields("Multiple").Value
                        .POSPrice = mrsPendPrice.Fields("POSPrice").Value
                        .StartDate = mrsPendPrice.Fields("StartDate").Value
                        .ItemBO = itemInfo
                        mrsPendPrice.Filter = ADODB.FilterGroupEnum.adFilterNone
                        .ShowDialog(Me)
                        .Dispose()
                    End With
                End If

            Case 3
                If ugrdList.Selected.Rows.Count <= 0 Then
                    MsgBox(ResourcesIRMA.GetString("MustSelect"), MsgBoxStyle.Critical, Me.Text)
                    Exit Sub
                End If

                If MsgBox(ResourcesIRMA.GetString("DeleteHighlightedRows"), MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
                    For iLoop = 0 To ugrdList.Selected.Rows.Count - 1
                        'Make sure the row is editable
                        Select Case CShort(GetPriceChgType(ugrdList.Selected.Rows(iLoop).Cells("ID").Value))
                            Case 0
                                bCouldNotDelete = True
                            Case Else
                                SQLExecute("EXEC DeletePriceBatchDetail " & ugrdList.Selected.Rows(iLoop).Cells("ID").Value, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                        End Select
                    Next iLoop

                    If bCouldNotDelete Then MsgBox(ResourcesIRMA.GetString("CannotDeleteRow"), MsgBoxStyle.Exclamation, Me.Text)
                End If

            Case Else
                GoTo me_exit
        End Select

        RefreshData()

me_exit:

        Exit Sub

me_err:
        If Err.Number = 364 Then
            Resume me_exit
        Else
            MsgBox("Error in cmdFunction_Click(" & CStr(Index) & "): " & Err.Description, MsgBoxStyle.Critical, Me.Text)
        End If

    End Sub

    Private Sub cmdItemSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdItemSearch.Click

        Dim lSaveItem_Key As Integer
        Dim lSaveSubTeam As Integer
        Dim sSaveItem_Desc As String

        lSaveItem_Key = glItemID
        lSaveSubTeam = glItemSubTeam
        sSaveItem_Desc = gsItemDescription
        glItemID = 0
        gsItemDescription = ""
        glItemSubTeam = 0

        frmItemSearch.ShowDialog()

        '-- if its not zero, then something was selected
        If glItemID <> 0 Then

            If frmItemPrice.MSRP_Required Then
                If MsgBox(ResourcesItemHosting.GetString("BrandEDLP"), MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then
                    glItemID = lSaveItem_Key
                    glItemSubTeam = lSaveSubTeam
                    gsItemDescription = sSaveItem_Desc
                    GoTo me_exit
                End If
            End If

            SQLExecute("EXEC UnlockItem " & lSaveItem_Key, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            RefreshData()
        Else
            glItemID = lSaveItem_Key
            glItemSubTeam = lSaveSubTeam
            gsItemDescription = sSaveItem_Desc
        End If

me_exit:
        frmItemSearch.Close()
        frmItemSearch.Dispose()

    End Sub

    Private Sub RefreshData()

        Me.Text = String.Format(ResourcesItemHosting.GetString("PendingPriceChanges"), gsItemDescription)

        Try
            gRSRecordset = SQLOpenRecordSet("EXEC LockItem " & glItemID & "," & giUserID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            If gRSRecordset.Fields("User_ID").Value <> giUserID Then
                MsgBox(String.Format(ResourcesItemHosting.GetString("ItemLocked"), gRSRecordset.Fields("FullName").Value, gRSRecordset.Fields("User_ID_Date").Value), MsgBoxStyle.Critical, Me.Text)
                gRSRecordset.Close()
                gRSRecordset = Nothing
                Me.Close()
                Exit Sub
            Else
                msUser_ID_Date = gRSRecordset.Fields("User_ID_Date").Value
            End If
        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
        End Try

        Try
            gRSRecordset = SQLOpenRecordSet("EXEC GetItemPendPrice " & glItemID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            CreateEmptyADORS_FromDAO(gRSRecordset, mrsPendPrice, bAllowNulls:=False)
            mrsPendPrice.Open()

            While Not gRSRecordset.EOF
                CopyDAORecordToADORecord(gRSRecordset, mrsPendPrice, bAllowNulls:=False)
                gRSRecordset.MoveNext()
            End While

        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
        End Try

        PopGrid()

    End Sub
	
	Private Sub frmItemPricePendSearch_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
		
		msUser_ID_Date = ""
		
		If Not (mrsStore Is Nothing) Then
            If mrsStore.State = ADODB.ObjectStateEnum.adStateOpen Then mrsStore.Close()
			mrsStore = Nothing
		End If
		
		If Not (mrsPendPrice Is Nothing) Then
            If mrsPendPrice.State = ADODB.ObjectStateEnum.adStateOpen Then mrsPendPrice.Close()
			mrsPendPrice = Nothing
		End If
		
    End Sub

    Private Function GetOnSale(ByRef lPriceBatchDetailID As Integer) As Boolean
        Dim bResult As Boolean
        If Not (mrsPendPrice Is Nothing) Then
            If mrsPendPrice.State = ADODB.ObjectStateEnum.adStateOpen Then
                mrsPendPrice.Filter = ADODB.FilterGroupEnum.adFilterNone
                mrsPendPrice.Filter = "PriceBatchDetailID = " & lPriceBatchDetailID & " AND PriceBatchStatusID = 0"
                If mrsPendPrice.RecordCount = 1 Then bResult = mrsPendPrice.Fields("On_Sale").Value
                mrsPendPrice.Filter = ADODB.FilterGroupEnum.adFilterNone
            End If
        End If
        GetOnSale = bResult
    End Function

    Private Function GetPriceChgType(ByRef lPriceBatchDetailID As Integer) As Integer
        Dim iResult As Integer
        If Not (mrsPendPrice Is Nothing) Then
            If mrsPendPrice.State = ADODB.ObjectStateEnum.adStateOpen Then
                mrsPendPrice.Filter = ADODB.FilterGroupEnum.adFilterNone
                mrsPendPrice.Filter = "PriceBatchDetailID = " & lPriceBatchDetailID & " AND PriceBatchStatusID = 0"
                If mrsPendPrice.RecordCount = 1 Then iResult = mrsPendPrice.Fields("PriceChgTypeID").Value
                mrsPendPrice.Filter = ADODB.FilterGroupEnum.adFilterNone
            End If
        End If
        GetPriceChgType = iResult
    End Function
	
	Private Sub OptSelection_CheckedChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles OptSelection.CheckedChanged
		If eventSender.Checked Then
			Dim Index As Short = OptSelection.GetIndex(eventSender)
			
			SetActive(cmbStore, False)
			SetActive(cmbZones, False)
			SetActive(cmbState, False)
			
			Select Case Index
				Case 0 'Store
					SetActive(cmbStore, True)
				Case 1 'Zone
					SetActive(cmbZones, True)
				Case 2 'State
					SetActive(cmbState, True)
			End Select
			
		End If
	End Sub
	Public Sub PopulateRetailStoreDropDown(ByRef cmb As System.Windows.Forms.ComboBox)
		
		ReplicateCombo(Me.cmbStore, cmb)
		
	End Sub
	Public Sub PopulateRetailStoreZoneDropDown(ByRef cmb As System.Windows.Forms.ComboBox)
		
		ReplicateCombo(Me.cmbZones, cmb)
		
	End Sub
	Public Sub PopulateRetailStoreStateDropDown(ByRef cmb As System.Windows.Forms.ComboBox)
		
		ReplicateCombo(Me.cmbState, cmb)
		
	End Sub
	
	Public Function GetStoreListString(ByRef lZone_ID As Integer, ByRef msState As String, ByRef bMega_Stores As Boolean, ByRef bWFM_Stores As Boolean, ByRef bAllStores As Boolean) As String
		Dim msStores As String
        Dim sFilter As String
        sFilter = String.Empty
        msStores = String.Empty

		If lZone_ID > 0 Then
			sFilter = "Zone_ID = " & lZone_ID
		ElseIf Len(msState) > 0 Then 
			sFilter = "State = '" & msState & "'"
		ElseIf bMega_Stores Then 
			sFilter = "Mega_Store = 1"
		ElseIf bWFM_Stores Then 
			sFilter = "WFM_Store = 1"
		End If
		
		mrsStore.Filter = ADODB.FilterGroupEnum.adFilterNone
		mrsStore.Filter = sFilter
		Do While Not mrsStore.EOF
			If Len(msStores) > 0 Then
				msStores = msStores & "|" & mrsStore.Fields("Store_No").Value
			Else
				msStores = mrsStore.Fields("Store_No").Value
			End If
			mrsStore.MoveNext()
		Loop 
		mrsStore.Filter = ADODB.FilterGroupEnum.adFilterNone
		
		GetStoreListString = msStores
	End Function
	
	Public Function GetStoreListRS() As ADODB.Recordset
		Dim rs As ADODB.Recordset
		
		rs = New ADODB.Recordset
		rs.Fields.Append("Store_No", ADODB.DataTypeEnum.adInteger)
		rs.Fields.Append("Store_Name", ADODB.DataTypeEnum.adVarChar, 255)
		rs.Fields.Append("Zone_ID", ADODB.DataTypeEnum.adInteger)
		rs.Fields.Append("State", ADODB.DataTypeEnum.adVarChar, 2)
		rs.Fields.Append("WFM_Store", ADODB.DataTypeEnum.adInteger)
		rs.Fields.Append("Mega_Store", ADODB.DataTypeEnum.adInteger)
		rs.Fields.Append("CustomerType", ADODB.DataTypeEnum.adInteger)
		
		rs.Open()
		
		If Not (mrsStore Is Nothing) Then
			If mrsStore.State = ADODB.ObjectStateEnum.adStateOpen Then
				mrsStore.Filter = ADODB.FilterGroupEnum.adFilterNone
				mrsStore.Sort = "Store_Name"
				Do 
					rs.AddNew()
					rs.Fields("Store_Name").Value = mrsStore.Fields("Store_Name").Value
					rs.Fields("Store_No").Value = mrsStore.Fields("Store_No").Value
					rs.Fields("Zone_ID").Value = mrsStore.Fields("Zone_ID").Value
					rs.Fields("State").Value = mrsStore.Fields("State").Value
					rs.Fields("WFM_Store").Value = mrsStore.Fields("WFM_Store").Value
					rs.Fields("Mega_Store").Value = mrsStore.Fields("Mega_Store").Value
					rs.Fields("CustomerType").Value = mrsStore.Fields("CustomerType").Value
					rs.Update()
					mrsStore.MoveNext()
				Loop Until mrsStore.EOF
				rs.MoveFirst()
			End If
		End If
		
		GetStoreListRS = rs
    End Function

End Class
