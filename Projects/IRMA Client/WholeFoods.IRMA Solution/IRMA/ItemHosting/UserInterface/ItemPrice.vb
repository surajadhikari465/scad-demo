
Option Strict Off
Option Explicit On

Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess


Friend Class frmItemPrice
    Inherits System.Windows.Forms.Form

    Private mbMSRP_Required As Boolean
	Private mrsPrice As ADODB.Recordset
    Private mlItemID As Integer
    Private poItemInfo As ItemBO

    Private Const miStore_No_Col As Short = 6
    Private Const miStore_Name_Col As Short = 0

    Private gRSRecordset As DAO.Recordset

    Private UseAverageCostforCostandMargin As Boolean = False

    Private mdt As DataTable
    Private mdv As DataView

    Private Sub frmItemPrice_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        On Error GoTo me_err

        CheckConfigurationSettings()

        'hide buttons appropriately
        ShowOrHideButtons()

        SetupFormForNewItem()

        Exit Sub

me_err:
        MsgBox("Error loading " & Me.Name & ".Form_Load: " & Err.Description, MsgBoxStyle.Critical, Me.Text)

    End Sub
    Private Sub CheckConfigurationSettings()

        Try
            UseAverageCostforCostandMargin = ConfigurationServices.AppSettings("UseAvgCostforCostandMargin")

        Catch ex As Exception
            ' UseAverageCostforCostandMargin was not found. This key needs to be created. Default to false and display a warning.
            UseAverageCostforCostandMargin = False
            Label_UseAverageCostforCostandMargin.Visible = True
        End Try


    End Sub

    Private Sub SetupFormForNewItem()
        Dim rs As DAO.Recordset = Nothing
        Dim bUserSubTeam As Boolean

        SetupItemData()
        Call RefreshGrid()

        Try
            'Determine if user has access to this item's subteam
            rs = SQLOpenRecordSet("EXEC GetUsersSubTeam " & giUserID & ",NULL," & glItemID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            If Not rs.EOF Then bUserSubTeam = True
        Finally
            If rs IsNot Nothing Then
                rs.Close()
                rs = Nothing
            End If
        End Try


        cmdFunction(0).Enabled = ((gbItemAdministrator And bUserSubTeam) Or gbSuperUser Or gbDataAdministrator)
        cmdFunction(1).Enabled = cmdFunction(0).Enabled Or gbTaxAdministrator
        Button_MarginInfo.Enabled = ((gbItemAdministrator And bUserSubTeam) Or gbSuperUser Or gbDataAdministrator)

        Button_EndSaleEarly.Enabled = cmdFunction(0).Enabled
        Button_CancelAllSales.Enabled = cmdFunction(0).Enabled
    End Sub

    ''' <summary>
    ''' buttons are controlled by InstanceDataFlag values; if active then show button
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ShowOrHideButtons()
        'hide buttons by default
        Me.Button_EndSaleEarly.Visible = False
        Me.Button_CancelAllSales.Visible = False

        'End Sale Early button
        If InstanceDataDAO.IsFlagActive("ShowEndSaleEarlyButton") Then
            Me.Button_EndSaleEarly.Visible = True
        End If

        'Cancel All Sales button
        If InstanceDataDAO.IsFlagActive("ShowCancelAllSalesButton") Then
            Me.Button_CancelAllSales.Visible = True
        End If
    End Sub

    Private Sub SetupItemData()
        'setup item business object to be passed to child forms
        poItemInfo = New ItemBO
        poItemInfo.Item_Key = glItemID
        poItemInfo.ItemDescription = gsItemDescription
        'populate package desc info
        ItemUnitDAO.GetItemUnitInfo(poItemInfo)
    End Sub

    Private Sub SetupDataTable()

        ' Create a data table
        mdt = New DataTable("ItemPrices")

        'Visible on grid.
        '--------------------
        mdt.Columns.Add(New DataColumn("Store Name", GetType(String)))
        mdt.Columns.Add(New DataColumn("POS Price", GetType(String)))
        mdt.Columns.Add(New DataColumn("Case", GetType(String)))
        mdt.Columns.Add(New DataColumn("Sale Start", GetType(String)))
        mdt.Columns.Add(New DataColumn("Sale End", GetType(String)))
        mdt.Columns.Add(New DataColumn("Sale Price", GetType(String)))
        mdt.Columns.Add(New DataColumn("Tax", GetType(String)))
        mdt.Columns.Add(New DataColumn("MSRP Required", GetType(String)))
        mdt.Columns.Add(New DataColumn("Restricted", GetType(String)))
        mdt.Columns.Add(New DataColumn("Stop Sale", GetType(String)))
        mdt.Columns.Add(New DataColumn("Line Discount", GetType(String)))
        mdt.Columns.Add(New DataColumn("Competitive Item", GetType(String)))
        mdt.Columns.Add(New DataColumn("SubTeam", GetType(String)))
        mdt.Columns.Add(New DataColumn("PriceChgTypeDesc", GetType(String)))

        ' new columns
        mdt.Columns.Add(New DataColumn("AvgCost", GetType(String)))
        mdt.Columns.Add(New DataColumn("MarginPercentAvgCost", GetType(String)))
        mdt.Columns.Add(New DataColumn("RegUnitCost", GetType(String)))
        mdt.Columns.Add(New DataColumn("NetUnitCost", GetType(String)))
        mdt.Columns.Add(New DataColumn("MarginPercentCurrCost", GetType(String)))

        mdt.Columns.Add(New DataColumn("Currency", GetType(String)))


        'Hidden.
        '--------------------
        mdt.Columns.Add(New DataColumn("SubTeam No", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("StoreNo", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("Price", GetType(String)))
        mdt.Columns.Add(New DataColumn("IsRetailStore", GetType(String)))
        mdt.Columns.Add(New DataColumn("POS Sale Price", GetType(String)))
    End Sub

    Private Sub LoadDataTable()
        Dim cPrice As Decimal
        Dim sStartDate As String = String.Empty
        Dim sEndDate As String = String.Empty
        Dim row As DataRow

        Me.Text = String.Format(ResourcesItemHosting.GetString("CurrentPrices"), gsItemDescription)

        mbMSRP_Required = False

        Try
            gRSRecordset = SQLOpenRecordSet("EXEC GetCurrentPrices " & glItemID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            CreateEmptyADORS_FromDAO(gRSRecordset, mrsPrice, bAllowNulls:=False)
            mrsPrice.Open()

            While Not gRSRecordset.EOF

                row = mdt.NewRow

                If Not IsDBNull(gRSRecordset.Fields("Sale_Start_Date").Value) Then
                    sStartDate = CDate(gRSRecordset.Fields("Sale_Start_Date").Value).ToString("d")
                Else
                    sStartDate = String.Empty
                End If
                If Not IsDBNull(gRSRecordset.Fields("Sale_End_Date").Value) Then
                    sEndDate = CDate(gRSRecordset.Fields("Sale_End_Date").Value).ToString("d")
                Else
                    sEndDate = String.Empty
                End If

                CopyDAORecordToADORecord(gRSRecordset, mrsPrice, bAllowNulls:=False)

                If gRSRecordset.Fields("Multiple").Value > 0 Then
                    cPrice = gRSRecordset.Fields("Price").Value / gRSRecordset.Fields("Multiple").Value
                Else
                    cPrice = gRSRecordset.Fields("Price").Value
                End If

                row("Store Name") = gRSRecordset.Fields("Store_Name").Value.ToString

                row("Price") = String.Format("{0} @ {1}", gRSRecordset.Fields("Multiple").Value.ToString, CDbl(gRSRecordset.Fields("Price").Value).ToString("####0.00"))
                'row("MSRPPrice") = String.Format("{0} @ {1}", gRSRecordset.Fields("Multiple").Value.ToString, CDbl(gRSRecordset.Fields("MSRPPrice").Value).ToString("####0.00"))
                row("Case") = CDbl(gRSRecordset.Fields("Case_Price").Value).ToString("####0.00")
                row("Sale Start") = sStartDate
                row("Sale End") = sEndDate
                row("Sale Price") = String.Format("{0} @ {1}", gRSRecordset.Fields("Sale_Multiple").Value.ToString, CDbl(gRSRecordset.Fields("Sale_Price").Value).ToString("####0.00"))
                row("Tax") = gRSRecordset.Fields("TaxTable").Value.ToString
                row("Restricted") = IIf(System.Math.Abs(CInt(gRSRecordset.Fields("Restricted_Hours").Value)) = 1, "*", "")
                row("Stop Sale") = IIf(System.Math.Abs(CInt(gRSRecordset.Fields("NotAuthorizedForSale").Value)) = 1, "*", "")
                row("Competitive Item") = IIf(System.Math.Abs(CInt(gRSRecordset.Fields("CompFlag").Value)) = 1, "*", "")
                row("Line Discount") = IIf(System.Math.Abs(CInt(gRSRecordset.Fields("IBM_Discount").Value)) = 1, "*", "")
                row("StoreNo") = gRSRecordset.Fields("Store_No").Value.ToString
                row("IsRetailStore") = gRSRecordset.Fields("IsRetailStore").Value.ToString
                row("POS Price") = String.Format("{0} @ {1}", gRSRecordset.Fields("Multiple").Value.ToString, CDbl(gRSRecordset.Fields("POSPrice").Value).ToString("####0.00"))
                row("POS Sale Price") = String.Format("{0} @ {1}", gRSRecordset.Fields("Sale_Multiple").Value.ToString, CDbl(gRSRecordset.Fields("POSSale_Price").Value).ToString("####0.00"))
                row("MSRP Required") = IIf(System.Math.Abs(CInt(gRSRecordset.Fields("MSRP_Required").Value)) = 1, "*", "")
                row("SubTeam") = gRSRecordset.Fields("SubTeam_Name").Value.ToString.Trim
                row("SubTeam No") = CInt(gRSRecordset.Fields("SubTeam_No").Value)
                row("PriceChgTypeDesc") = gRSRecordset.Fields("PriceChgTypeDesc").Value

                ' new columns
                If IsDBNull(gRSRecordset.Fields("AvgCost").Value) Then
                    row("AvgCost") = 0.0
                Else
                    row("AvgCost") = CDbl(gRSRecordset.Fields("AvgCost").Value).ToString("#####0.0000")
                End If

                If IsDBNull(gRSRecordset.Fields("MarginPercentAvgCost").Value) Then
                    row("MarginPercentAvgCost") = 0.0
                Else
                    row("MarginPercentAvgCost") = CDbl(gRSRecordset.Fields("MarginPercentAvgCost").Value).ToString("#####0.0000")
                End If

                If IsDBNull(gRSRecordset.Fields("CurrentRegCost").Value) Then
                    row("RegUnitCost") = 0.0
                Else
                    row("RegUnitCost") = CDbl(gRSRecordset.Fields("CurrentRegCost").Value).ToString("#####0.0000")
                End If

                If IsDBNull(gRSRecordset.Fields("NetUnitCost").Value) Then
                    row("NetUnitCost") = 0.0
                Else
                    row("NetUnitCost") = CDbl(gRSRecordset.Fields("NetUnitCost").Value).ToString("#####0.0000")
                End If

                If IsDBNull(gRSRecordset.Fields("MarginPercentCurrCost").Value) Then
                    row("MarginPercentCurrCost") = 0.0
                Else
                    row("MarginPercentCurrCost") = CDbl(gRSRecordset.Fields("MarginPercentCurrCost").Value).ToString("#####0.0000")
                End If

                row("Currency") = gRSRecordset.Fields("CurrencyCode").Value.ToString.Trim

                mdt.Rows.Add(row)

                If gRSRecordset.Fields("EDLP_365").Value Then mbMSRP_Required = True

                gRSRecordset.MoveNext()

            End While

            mdt.AcceptChanges()
            mdv = New System.Data.DataView(mdt)
            ugrdPriceList.DataSource = mdv


            ' changes made to make un-authorized stores greyed out.. task #2179... AM
            Dim iLoop As Short
            For iLoop = 0 To ugrdPriceList.Rows.Count - 1
                If StoreDAO.IsItemAuthorized(CInt(ugrdPriceList.Rows(iLoop).Cells("StoreNo").Value), glItemID) Then
                    ugrdPriceList.Rows(iLoop).Appearance.ForeColor = Color.Black
                Else
                    ugrdPriceList.Rows(iLoop).Appearance.ForeColor = Color.Gray
                End If
            Next iLoop
            'end AM

            'conditionally hide the Case Price column if InstanceDataFlag says to
            ugrdPriceList.DisplayLayout.Bands(0).Columns("Case").Hidden = InstanceDataDAO.IsFlagActive("HideCaseColumnOnCurrentPricesScreen")


            ugrdPriceList.DisplayLayout.Bands(0).Columns("AvgCost").Hidden = Not UseAverageCostforCostandMargin
            ugrdPriceList.DisplayLayout.Bands(0).Columns("MarginPercentAvgCost").Hidden = Not UseAverageCostforCostandMargin

            ugrdPriceList.DisplayLayout.Bands(0).Columns("RegUnitCost").Hidden = UseAverageCostforCostandMargin
            ugrdPriceList.DisplayLayout.Bands(0).Columns("NetUnitCost").Hidden = UseAverageCostforCostandMargin
            ugrdPriceList.DisplayLayout.Bands(0).Columns("MarginPercentCurrCost").Hidden = UseAverageCostforCostandMargin

            If ugrdPriceList.Rows.Count > 0 Then
                ugrdPriceList.Rows(0).Selected = True
            End If

            mlItemID = glItemID
        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset.Close()
                gRSRecordset = Nothing
            End If
        End Try

ExitSub:
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default

    End Sub

    Private Sub RefreshGrid()

        Call SetupDataTable()
        Call LoadDataTable()

    End Sub

    Public ReadOnly Property MSRP_Required() As Boolean
        Get
            RefreshGrid()
            MSRP_Required = mbMSRP_Required
        End Get
    End Property
	
	
	Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
		
		'-- Close the form
		Me.Close()
		
	End Sub

	Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch.Click
		
		Dim lOldItemID As Integer
		Dim sOldItemDescription As String
		Dim lOldSubTeam As Integer
		
		'Search for an item
		lOldItemID = glItemID
		glItemID = 0
		sOldItemDescription = gsItemDescription
		gsItemDescription = ""
		lOldSubTeam = glItemSubTeam
		glItemSubTeam = 0
		
        frmItemSearch.ShowDialog()
		
		If glItemID > 0 Then
			SQLExecute("EXEC UnlockItem " & lOldItemID, dao.RecordsetOptionEnum.dbSQLPassThrough)
            SetupFormForNewItem()
		Else
			'Restore globals to old values
			glItemID = lOldItemID
			gsItemDescription = sOldItemDescription
			glItemSubTeam = lOldSubTeam
		End If
		
        frmItemSearch.Close()
        frmItemSearch.Dispose()

    End Sub

	Private Sub frmItemPrice_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
		
		mlItemID = 0
		
		If Not (mrsPrice Is Nothing) Then
			If mrsPrice.State = ADODB.ObjectStateEnum.adStateOpen Then mrsPrice.Close()
            mrsPrice = Nothing
        End If
		
	End Sub

    Public Sub GetRegPriceInfo(ByRef lStore_No As Integer, ByRef iMultiple As Short, ByRef cPrice As Decimal, ByRef cMSRPPrice As Decimal, ByRef cPOSPrice As Decimal, ByRef cAvgCost As Decimal)

        RefreshGrid() 'Because glItemID could have been changed on a called form

        If Not (mrsPrice Is Nothing) Then
            If mrsPrice.State = ADODB.ObjectStateEnum.adStateOpen Then
                mrsPrice.Filter = ADODB.FilterGroupEnum.adFilterNone
                If lStore_No = 0 Then
                    If ugrdPriceList.Rows.Count > 0 Then lStore_No = ugrdPriceList.Selected.Rows(0).Cells("StoreNo").Value
                End If
                mrsPrice.Filter = "Store_No = " & lStore_No
                If mrsPrice.RecordCount = 1 Then
                    iMultiple = mrsPrice.Fields("Multiple").Value
                    cPrice = mrsPrice.Fields("Price").Value
                    cMSRPPrice = mrsPrice.Fields("MSRPPrice").Value
                    cAvgCost = mrsPrice.Fields("AvgCost").Value
                    cPOSPrice = mrsPrice.Fields("POSPrice").Value
                End If
                mrsPrice.Filter = ADODB.FilterGroupEnum.adFilterNone
            End If
        End If

    End Sub

    Private Sub cmdPrice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrice.Click
        Dim lOldStoreId As Integer

        On Error GoTo me_err

        If Not CBool(ugrdPriceList.Selected.Rows(0).Cells("IsRetailStore").Value) Then
            MsgBox(ResourcesItemHosting.GetString("SelectRetailStore"), MsgBoxStyle.Critical, Me.Text)
            GoTo me_exit
        End If

        If MSRP_Required Then
            If MsgBox(ResourcesItemHosting.GetString("BrandEDLP"), MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then
                Exit Sub
            End If
        End If
        frmItemPricePendSearch.ShowDialog()
        frmItemPricePendSearch.Close()
        frmItemPricePendSearch.Dispose()

        RefreshGrid()

me_exit:

        Exit Sub

me_err:
        If Err.Number = 364 Then
            Resume me_exit
        Else
            MsgBox("Error in cmdPrice: " & Err.Description, MsgBoxStyle.Critical, Me.Text)
        End If

    End Sub

    Private Sub cmdStore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdStore.Click

        Dim lOldStoreId As Integer

        On Error GoTo me_err

        If Not CBool(ugrdPriceList.Selected.Rows(0).Cells("IsRetailStore").Value) Then
            MsgBox(ResourcesItemHosting.GetString("SelectRetailStore"), MsgBoxStyle.Critical, Me.Text)
            GoTo me_exit
        End If

        If ugrdPriceList.Selected.Rows.Count = 1 Or ugrdPriceList.Rows.Count = 1 Then
            lOldStoreId = glStoreID
            glStoreID = ugrdPriceList.Selected.Rows(0).Cells("StoreNo").Value

            Dim fItemStore As New frmItemStore
            fItemStore.ShowDialog()
            fItemStore.Close()
            fItemStore.Dispose()

            glStoreID = lOldStoreId
        Else
            MsgBox(ResourcesItemHosting.GetString("SelectPrice"), MsgBoxStyle.Exclamation, Me.Text)
        End If

        RefreshGrid()

me_exit:

        Exit Sub

me_err:
        If Err.Number = 364 Then
            Resume me_exit
        Else
            MsgBox("Error in cmdStore_Click: " & Err.Description, MsgBoxStyle.Critical, Me.Text)
        End If

    End Sub

    Private Sub ugrdPriceList_AfterSelectChange(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs) Handles ugrdPriceList.AfterSelectChange
        Dim lOldStoreId As Integer

        If e.Type Is GetType(Infragistics.Win.UltraWinGrid.UltraGridRow) Then
            If Me.ugrdPriceList.Selected.Rows.Count > 0 Then
                If CBool(ugrdPriceList.Selected.Rows(0).Cells("IsRetailStore").Value) Then
                    lOldStoreId = glStoreID
                    glStoreID = ugrdPriceList.Selected.Rows(0).Cells("StoreNo").Value
                    If PromotionCheck(glItemID, glStoreID) Then
                        Label_PromotionExistence.Visible = True
                    Else
                        Label_PromotionExistence.Visible = False
                    End If
                    glStoreID = lOldStoreId
                End If
            End If
        End If

    End Sub

    Private Function PromotionCheck(ByVal ItemKey As Integer, ByVal StoreId As Integer) As Boolean
        Dim factory As DataFactory = New DataFactory(DataFactory.ItemCatalog)
        Dim ds As DataSet = Nothing
        Dim paramList As ArrayList = New ArrayList()
        Dim currentParam As DBParam
        Dim retval As Boolean = False

        currentParam = New DBParam
        currentParam.Name = "@ItemKey"
        currentParam.Value = ItemKey
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@StoreId"
        currentParam.Value = StoreId
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        ds = factory.GetStoredProcedureDataSet("EPromotions_PromotionExistence", paramList)
        If Not ds.Tables(0).Rows(0)("PromotionExistence") Is DBNull.Value Then
            If ds.Tables(0).Rows(0)("PromotionExistence").ToString().Equals("1") Then
                retval = True
            Else
                retval = False
            End If
        Else
            retval = False
        End If
        ds.Dispose()
        Return retval
    End Function

    Private Sub ugrdPriceList_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles ugrdPriceList.InitializeLayout
        ugrdPriceList.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns

    End Sub

    Private Sub Button_MarginInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_MarginInfo.Click
        Dim marginInfoForm As New MarginInfo

        marginInfoForm.ItemBO = poItemInfo
        marginInfoForm.ShowDialog()
        marginInfoForm.Dispose()
    End Sub

    Private Sub Button_CancelAllSales_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_CancelAllSales.Click
        Dim cancelAllSalesForm As New CancelAllSales

        cancelAllSalesForm.ItemBO = poItemInfo
        cancelAllSalesForm.ShowDialog()
        cancelAllSalesForm.Dispose()
    End Sub

    Private Sub Button_EndSaleEarly_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_EndSaleEarly.Click
        Dim endSaleEarlyForm As New EndSaleEarly
        Dim storeInfo As New StoreBO

        Dim iMultiple As Short
        Dim cPrice As Decimal
        Dim cMSRPPrice As Decimal
        Dim cPOSPrice As Decimal
        Dim cAvgCost As Decimal

        'user must select a store to end sale early for
        If ugrdPriceList.Selected.Rows.Count = 1 Or ugrdPriceList.Rows.Count = 1 Then
            'setup store info
            storeInfo.StoreNo = ugrdPriceList.Selected.Rows(0).Cells("StoreNo").Value
            storeInfo.StoreName = ugrdPriceList.Selected.Rows(0).Cells("Store Name").Value

            'get current reg price and multiple for selected store/item
            Me.GetRegPriceInfo(storeInfo.StoreNo, iMultiple, cPrice, cMSRPPrice, cPOSPrice, cAvgCost)
            endSaleEarlyForm.RegPrice = cPOSPrice
            endSaleEarlyForm.RegMultiple = iMultiple

            'setup form data
            endSaleEarlyForm.ItemBO = poItemInfo
            endSaleEarlyForm.StoreBO = storeInfo

            endSaleEarlyForm.ShowDialog()
            endSaleEarlyForm.Dispose()
        Else
            MsgBox(ResourcesItemHosting.GetString("SelectStoreEndSaleEarly"), MsgBoxStyle.Exclamation, Me.Text)
        End If
    End Sub


End Class
