Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Common.BusinessLogic
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.ItemChaining.DataAccess
Imports WholeFoods.IRMA.Pricing.BusinessLogic
Imports WholeFoods.IRMA.Pricing.DataAccess
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ItemChaining.UserInterface
    Public Class frmPriceChange2

#Region "Member Variables"
        Public Connection As Object
        Private _validating As Boolean
        Private _warnings As New SortedList(Of PriceChangeStatus, String)
        Private _store As DataAccess.Store = New DataAccess.Store()
        Private _settings As DataAccess.Settings = New DataAccess.Settings()
        Private _chain As DataAccess.ItemChain = New DataAccess.ItemChain()
        Private _priceBatchDetail As DataAccess.PriceBatchDetail = New DataAccess.PriceBatchDetail()
        Private _itemIDs As String
        Private _storeIDs As String
        Private _priceChangeList As New List(Of PriceChangeBO)()
        Private _itemPriceHistory As New DataTable()
#End Region

#Region "Enumerations"
        Private Enum PriceChangeSubItemIndex
            DisplayText = 0
            OnSale = 1
            MSRPRequired = 2
            LineDrive = 3
            PriceChangeTypeID = 4
        End Enum
#End Region

#Region "Helper Methods"

#Region "Validation"
        Private Function wpRegValidate() As Boolean
            'colors
            If radChangePOSpriceto.Checked Then
                txtChangePOSpricetoQty.BackColor = Color.MistyRose
                txtChangePOSpricetoPrice.BackColor = Color.MistyRose
            Else
                txtChangePOSpricetoQty.BackColor = Color.White
                txtChangePOSpricetoPrice.BackColor = Color.White
            End If

            If radChangePOSpriceby.Checked Then
                txtChangePOSpricebyPercent.BackColor = Color.MistyRose
            Else
                txtChangePOSpricebyPercent.BackColor = Color.White
            End If

            'Values
            If radChangePOSpriceto.Checked And ValidateQuantity(txtChangePOSpricetoQty) And isNumeric(txtChangePOSpricetoPrice, CDec(0.01), 100000) Then
                Return True
            End If

            If radChangePOSpriceby.Checked And isNumeric(txtChangePOSpricebyPercent, -100, 100) Then
                Return True
            End If

            Return False
        End Function

        Private Function wpPromoValidate() As Boolean
            If lstPriceType.SelectedItems.Count = 0 Then Return False

            If IsRegularPriceChange() Then
                radChangePOSpromopriceto.Enabled = False
                txtChangePOSpromopricetoQty.Enabled = False
                txtChangePOSpromopricetoPrice.Enabled = False
                txtChangePOSpromopricetoQtyMSRP.Enabled = False
                txtChangePOSpromopricetoPriceMSRP.Enabled = False
            Else
                radChangePOSpromopriceto.Enabled = True
                txtChangePOSpromopricetoQty.Enabled = True
                txtChangePOSpromopricetoPrice.Enabled = True
                txtChangePOSpromopricetoQtyMSRP.Enabled = True
                txtChangePOSpromopricetoPriceMSRP.Enabled = True
            End If

            'colors
            If radChangePOSpromopriceto.Checked Then
                txtChangePOSpromopricetoQty.BackColor = Color.MistyRose
                txtChangePOSpromopricetoPrice.BackColor = Color.MistyRose

                If lstPriceType.SelectedItems(0).SubItems(2).Text = "True" Then
                    txtChangePOSpromopricetoQtyMSRP.BackColor = Color.MistyRose
                    txtChangePOSpromopricetoPriceMSRP.BackColor = Color.MistyRose
                Else
                    txtChangePOSpromopricetoQtyMSRP.BackColor = EnabledColor(txtChangePOSpromopricetoQtyMSRP)
                    txtChangePOSpromopricetoPriceMSRP.BackColor = EnabledColor(txtChangePOSpromopricetoPriceMSRP)
                End If
            Else
                txtChangePOSpromopricetoQty.BackColor = EnabledColor(txtChangePOSpromopricetoQty)
                txtChangePOSpromopricetoPrice.BackColor = EnabledColor(txtChangePOSpromopricetoPrice)
                txtChangePOSpromopricetoQtyMSRP.BackColor = EnabledColor(txtChangePOSpromopricetoQtyMSRP)
                txtChangePOSpromopricetoPriceMSRP.BackColor = EnabledColor(txtChangePOSpromopricetoPriceMSRP)
            End If

            If radChangePOSpromopricebyMSRP.Checked Then
                txtChangePOSpromopricebyMSRP.BackColor = Color.MistyRose
            Else
                txtChangePOSpromopricebyMSRP.BackColor = EnabledColor(txtChangePOSpromopricebyMSRP)
            End If

            If radChangePOSpromopricebyREG.Checked Then
                txtChangePOSpromopricebyREG.BackColor = Color.MistyRose
            Else
                txtChangePOSpromopricebyREG.BackColor = EnabledColor(txtChangePOSpromopricebyREG)
            End If

            'values 
            If radChangePOSpromopriceto.Checked And ValidateQuantity(txtChangePOSpromopricetoQty) And isNumeric(txtChangePOSpromopricetoPrice, CDec(0.01), 100000) Then
                'force msrp
                If lstPriceType.SelectedItems(0).SubItems(2).Text = "True" Then
                    If ValidateQuantity(txtChangePOSpromopricetoQtyMSRP) And isNumeric(txtChangePOSpromopricetoPriceMSRP, CDec(0.01), 100000) Then
                        Return True
                    Else
                        Return False
                    End If
                End If

                Return True
            End If

            If radChangePOSpromopricebyMSRP.Checked And isNumeric(txtChangePOSpromopricebyMSRP, -100, 100) Then
                Return True
            End If

            If radChangePOSpromopricebyREG.Checked And isNumeric(txtChangePOSpromopricebyREG, -100, 100) Then
                Return True
            End If

            Return False
        End Function

        Private Function ValidateQuantity(ByVal textBox As TextBox) As Boolean
            Dim output As Integer

            If (Int32.TryParse(textBox.Text, output) AndAlso output > 0) Then
                textBox.ForeColor = Color.Black
                Return True
            Else
                textBox.ForeColor = Color.Red
                Return False
            End If
        End Function

        Private Function isNumeric(ByVal textBox As TextBox, ByRef value As Decimal) As Boolean
            Dim pointIndex As Integer = textBox.Text.LastIndexOf(".")

            If Decimal.TryParse(textBox.Text, value) AndAlso (pointIndex = -1 OrElse pointIndex + 3 >= textBox.Text.Trim().Length) Then
                textBox.ForeColor = Color.Black
                Return True
            Else
                textBox.ForeColor = Color.Red
                Return False
            End If
        End Function

        Private Function isNumeric(ByVal textBox As TextBox, ByVal min As Decimal, ByVal max As Decimal) As Boolean
            If textBox.Text.Length = 0 Then Return False

            Dim value As Decimal
            If isNumeric(textBox, value) AndAlso value >= min And value <= max Then
                textBox.ForeColor = Color.Black
                Return True
            Else
                textBox.ForeColor = Color.Red
                Return False
            End If
        End Function

        Private Sub DisplayErrorMessage(ByVal message As String)
            MsgBox(message, MsgBoxStyle.Critical, My.Resources.ItemChaining.PriceChangeError)
        End Sub

        Private Function ProcessRegularStatus(ByVal priceChange As PriceChangeBO, ByVal status As PriceChangeStatus) As Boolean
            Dim warning As String = Nothing
            Dim foundError As Boolean = False

            Select Case status
                Case PriceChangeStatus.Valid
                    'Valid data - do nothing
                Case PriceChangeStatus.Error_RegMultipleGreaterZero
                    foundError = True
                    DisplayErrorMessage(My.Resources.ItemChaining.RegularMultipleGreaterThanZero)
                Case PriceChangeStatus.Error_RegPriceGreaterEqualZero, PriceChangeStatus.Error_RegPriceGreaterZero
                    foundError = True
                    DisplayErrorMessage(My.Resources.ItemChaining.RegularPriceGreaterThanZero)
                Case PriceChangeStatus.Error_RegStartDateInPast
                    foundError = True
                    DisplayErrorMessage(My.Resources.ItemChaining.StartDateInPast)
                Case PriceChangeStatus.Warning_RegConflictsWithRegPriceChange
                    warning = My.Resources.ItemChaining.PendingRegularPriceChanges
                Case PriceChangeStatus.Warning_RegConflictsWithSalePriceChange
                    warning = My.Resources.ItemChaining.PendingPromoChanges
                Case PriceChangeStatus.Warning_RegWithSaleCurrentlyOngoing
                    warning = My.Resources.ItemChaining.OngoingSale
                Case PriceChangeStatus.Warning_RegWithPriceChangeInBatch
                    warning = My.Resources.ItemChaining.UnprocessedPriceChanges
                Case Else
                    ' Unknown Error
                    Dim validationCode As ValidationBO = ValidationDAO.GetValidationCodeDetails(status)
                    MsgBox(String.Format(My.Resources.ItemChaining.UnknownValidationError, vbCrLf, status, validationCode.ValidationCodeDesc), MsgBoxStyle.Critical)
            End Select

            If warning IsNot Nothing And Not _warnings.ContainsKey(status) Then
                _warnings.Add(status, warning)
            End If

            Return foundError
        End Function

        Private Function ProcessPromoStatus(ByVal priceChange As PriceChangeBO, ByVal status As PriceChangeStatus) As Boolean
            Dim warning As String = Nothing
            Dim foundError As Boolean = False

            Select Case status
                Case PriceChangeStatus.Valid
                    'Valid data - do nothing
                Case PriceChangeStatus.Error_RegMultipleGreaterZero
                    foundError = True
                    DisplayErrorMessage(My.Resources.ItemChaining.RegularMultipleGreaterThanZero)
                Case PriceChangeStatus.Error_RegPriceGreaterEqualZero, PriceChangeStatus.Error_RegPriceGreaterZero
                    foundError = True
                    DisplayErrorMessage(My.Resources.ItemChaining.RegularPriceGreaterThanZero)
                Case PriceChangeStatus.Error_SaleMultipleGreaterZero
                    foundError = True
                    DisplayErrorMessage(My.Resources.ItemChaining.SaleMultipleGreaterThanZero)
                Case PriceChangeStatus.Error_SalePriceGreaterEqualZero, PriceChangeStatus.Error_SalePriceGreaterZero
                    foundError = True
                    DisplayErrorMessage(My.Resources.ItemChaining.SalePriceGreaterThanZero)
                    'Case PriceChangeStatus.Error_SalePriceMustEqualZero
                Case PriceChangeStatus.Error_SaleStartAndEndDatesRequired
                    foundError = True
                    DisplayErrorMessage(My.Resources.ItemChaining.SaleStartAndEndDatesRequired)
                Case PriceChangeStatus.Error_SaleStartDateInPast
                    foundError = True
                    DisplayErrorMessage(My.Resources.ItemChaining.SaleStartDateInPast)
                Case PriceChangeStatus.Error_SaleEndDateAfterSaleStartDate
                    foundError = True
                    DisplayErrorMessage(My.Resources.ItemChaining.SaleEndDateBeforeStartDate)
                Case PriceChangeStatus.Error_SaleEndDateGreaterMaxDBDate
                    foundError = True
                    DisplayErrorMessage(My.Resources.ItemChaining.SaleEndDateTooLarge)
                Case PriceChangeStatus.Error_MSRPMultipleGreaterZero
                    foundError = True
                    DisplayErrorMessage(My.Resources.ItemChaining.MSRPMultipleGreaterThanZero)
                Case PriceChangeStatus.Error_MSRPPriceGreaterZero
                    foundError = True
                    DisplayErrorMessage(My.Resources.ItemChaining.MSRPPriceGreaterThanZero)
                Case PriceChangeStatus.Error_SalePricingMethodRequired
                    foundError = True
                    DisplayErrorMessage(My.Resources.ItemChaining.SalePricingMethodRequired)
                Case PriceChangeStatus.Error_SalePriceChgTypeIDRequired
                    foundError = True
                    DisplayErrorMessage(My.Resources.ItemChaining.SalePriceChangeTypeRequired)
                Case PriceChangeStatus.Error_PriceQuantityGreaterZero
                    foundError = True
                    DisplayErrorMessage(My.Resources.ItemChaining.RegularPriceQuantityGreaterThanZero)
                Case PriceChangeStatus.Error_SalePriceLimitGreaterZero
                    foundError = True
                    DisplayErrorMessage(My.Resources.ItemChaining.LimitSalePriceGreaterThanZero)
                Case PriceChangeStatus.Error_SaleWithPriceChangeInBatch
                    foundError = True
                    DisplayErrorMessage(My.Resources.ItemChaining.SaleWithPriceChangeInBatch)
                Case PriceChangeStatus.Error_ExistingPromoWithSameStartDate
                    foundError = True
                    DisplayErrorMessage(My.Resources.ItemChaining.ExistingPromoWithSameStartDate)
                Case PriceChangeStatus.Warning_SaleConflictsWithRegPriceChange
                    warning = My.Resources.ItemChaining.SaleConflictsWithRegPriceChange
                Case PriceChangeStatus.Warning_SaleConflictsWithSalePriceChange
                    warning = My.Resources.ItemChaining.SaleConflictsWithSalePriceChange
                Case PriceChangeStatus.Warning_SaleCurrentlyOngoing
                    warning = My.Resources.ItemChaining.OngoingSale
                Case Else
                    ' Unknown Error
                    Dim validationCode As ValidationBO = ValidationDAO.GetValidationCodeDetails(status)
                    MsgBox(String.Format(My.Resources.ItemChaining.UnknownValidationError, vbCrLf, status, validationCode.ValidationCodeDesc), MsgBoxStyle.Critical)
            End Select

            If warning IsNot Nothing And Not _warnings.ContainsKey(status) Then
                _warnings.Add(status, warning)
            End If

            Return foundError
        End Function

        Private Sub ValidateFinal(ByVal state As Object)
            Me.Invoke(New ThreadStart(AddressOf ValidateFinal_Invoked))
        End Sub

        Private Sub ValidateFinal_Invoked()
            Dim regularPriceChange As Boolean = IsRegularPriceChange()
            Dim foundError As Boolean

            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            lblValidating.Visible = True

            _warnings.Clear()
            _itemPriceHistory = GetItemPriceHistory(regularPriceChange)

            GetPriceChanges()

            For Each priceChange As PriceChangeBO In _priceChangeList
                If regularPriceChange Then
                    foundError = ProcessRegularStatus(priceChange, priceChange.ValidateRegularPriceChangeData())
                Else
                    foundError = ProcessPromoStatus(priceChange, priceChange.ValidatePromoPriceChangeData())
                End If

                If foundError Then
                    Exit For
                End If
            Next

            lblValidating.Visible = False

            If foundError Then
                TabControl1.SelectedTab = wpSearchItems
            Else
                ShowNextWarning()
                chkIAggree.Visible = True
            End If

            Windows.Forms.Cursor.Current = Cursors.Default
        End Sub
#End Region

#Region "Preview and final step"

        Private Function GetCurrentPriceChgType() As ItemHosting.BusinessLogic.PriceChgTypeBO
            Dim priceChangeType As ItemHosting.BusinessLogic.PriceChgTypeBO = Nothing
            Dim types As ArrayList = ItemHosting.DataAccess.PriceChgTypeDAO.GetPriceChgTypeList(True, False)
            Dim typeID As Integer = Convert.ToInt32(lstPriceType.SelectedItems(0).SubItems(4).Text)

            For Each type As ItemHosting.BusinessLogic.PriceChgTypeBO In types
                If type.PriceChgTypeID = typeID Then
                    priceChangeType = type
                    Exit For
                End If
            Next

            Return priceChangeType
        End Function

        Private Sub GetPriceChanges()
            Dim priceChange As PriceChangeBO
            Dim priceChangeType As ItemHosting.BusinessLogic.PriceChgTypeBO = GetCurrentPriceChgType()
            Dim regularPriceChange As Boolean = IsRegularPriceChange()

            _priceChangeList.Clear()

            For Each row As DataRow In _itemPriceHistory.Rows
                priceChange = New PriceChangeBO()

                priceChange.ItemKey = CInt(row("Item_Key"))
                priceChange.StoreNo = CInt(row("Store_No"))
                priceChange.InsertApplication = "Pricing Wizard"
                priceChange.PriceChgType = priceChangeType
                priceChange.PriceBatchDetailId = -1

                If (regularPriceChange) Then
                    priceChange.StartDate = mclReg.SelectionStart.Date
                    priceChange.RegMultiple = CInt(row("NewMultiple"))
                    priceChange.RegPrice = CDec(row("NewPrice"))
                    priceChange.RegPOSPrice = CDec(row("NewPrice"))
                Else
                    priceChange.StartDate = mclPromoStart.SelectionStart.Date
                    priceChange.RegMultiple = CInt(row("Multiple"))
                    priceChange.RegPrice = CDec(row("Price"))
                    priceChange.RegPOSPrice = CDec(row("Price"))
                    priceChange.SaleEndDate = mclPromoEnd.SelectionStart.Date
                    priceChange.SaleMultiple = CInt(row("NewMultiple"))
                    priceChange.SalePrice = CDec(row("NewPrice"))
                    priceChange.SalePOSPrice = CDec(row("NewPrice"))
                    priceChange.MSRPMultiple = CInt(row("NewMSRPMultiple"))
                    priceChange.MSRPPrice = CDec(row("NewMSRPPrice"))
                    priceChange.LineDrive = priceChangeType.IsLineDrive
                End If

                _priceChangeList.Add(priceChange)
            Next
        End Sub

        Private Sub GetItemIDs()
            Dim builder As New StringBuilder

            For i As Integer = 0 To lstSelectedItems.Items.Count - 1
                builder.AppendFormat("{0},", lstSelectedItems.Items(i).SubItems(2).Text)
            Next

            _itemIDs = (builder.ToString().TrimEnd(CChar(",")))
        End Sub

        Private Sub GetStoreIDs()
            Dim builder As New StringBuilder

            For i As Integer = 0 To lstSelectedStores.Items.Count - 1
                builder.AppendFormat("{0},", lstSelectedStores.Items(i).SubItems(2).Text)
            Next

            _storeIDs = builder.ToString().TrimEnd(CChar(","))
        End Sub

        Private Sub ProcessPreviewRow(ByVal row As DataRow, ByVal grp As ListViewGroup)
            Dim item As ListViewItem = New ListViewItem
            Dim oldPrice As Decimal = CDec(row("price"))
            Dim newPrice As Decimal = CDec(row("NewPrice"))
            Dim oldMultiple As Integer = CInt(row("multiple"))
            Dim newMultiple As Integer = CInt(row("NewMultiple"))

            item.Group = grp
            item.Text = CStr(row("store_name"))
            item.SubItems.Add(String.Format("{0} @ {1:c}", oldMultiple, oldPrice))
            item.SubItems.Add(String.Format("{0} @ {1:c}", newMultiple, newPrice))

            If row.IsNull("Margin") Then
                item.SubItems.Add("N/A")
            Else
                item.SubItems.Add(CDec(row("Margin")).ToString("N2") + "%")
            End If

            If row.IsNull("NewMargin") Then
                item.SubItems.Add("N/A")
            Else
                item.SubItems.Add(CDec(row("NewMargin")).ToString("N2") + "%")
            End If

            lstPreview.Items.Add(item)
        End Sub

        Private Sub GetPriceChangeTypeExpressions(ByVal regularPriceChange As Boolean, _
            ByRef priceExpression As String, ByRef quantityExpression As String, ByRef msrpPriceExpression As String, _
            ByRef msrpMultipleExpression As String)

            Dim percentageFormat As String = "{0} - (({0} * {1}) / 100)"
            msrpPriceExpression = "msrpPrice"
            msrpMultipleExpression = "msrpMultiple"

            If regularPriceChange Then
                If radChangePOSpriceto.Checked Then
                    'fixed
                    priceExpression = txtChangePOSpricetoPrice.Text
                    quantityExpression = txtChangePOSpricetoQty.Text
                ElseIf radChangePOSpriceby.Checked Then
                    'percent
                    priceExpression = String.Format(percentageFormat, "price", txtChangePOSpricebyPercent.Text)
                    quantityExpression = "multiple"
                End If
            Else
                'PROMO
                If radChangePOSpromopriceto.Checked Then
                    'fixed
                    priceExpression = txtChangePOSpromopricetoPrice.Text
                    quantityExpression = txtChangePOSpromopricetoQty.Text

                    If (txtChangePOSpromopricetoQtyMSRP.Text.Trim().Length > 0) Then
                        msrpPriceExpression = txtChangePOSpromopricetoPriceMSRP.Text
                        msrpMultipleExpression = txtChangePOSpromopricetoQtyMSRP.Text
                    End If
                ElseIf radChangePOSpromopricebyMSRP.Checked Then
                    'percent of msrp
                    priceExpression = String.Format(percentageFormat, "msrpprice", txtChangePOSpromopricebyMSRP.Text)
                    quantityExpression = "multiple"
                ElseIf radChangePOSpromopricebyREG.Checked Then
                    'percent of price
                    priceExpression = String.Format(percentageFormat, "price", txtChangePOSpromopricebyREG.Text)
                    quantityExpression = "multiple"
                End If
            End If
        End Sub

        Private Function GetItemPriceHistory(ByVal regularPriceChange As Boolean) As DataTable
            Dim resultsTable As DataTable = Nothing
            Dim priceExpression As String = Nothing
            Dim multipleExpression As String = Nothing
            Dim msrpPriceExpression As String = Nothing
            Dim msrpMultipleExpression As String = Nothing
            Dim results As DataSet

            If (Me.InvokeRequired) Then
                Me.Invoke(New ThreadStart(AddressOf GetItemIDs))
                Me.Invoke(New ThreadStart(AddressOf GetStoreIDs))
            Else
                GetItemIDs()
                GetStoreIDs()
            End If

            results = _chain.ItemPriceListByItemAndStore(_itemIDs, _storeIDs)

            If (results.Tables.Count > 0) Then
                resultsTable = results.Tables(0)

                GetPriceChangeTypeExpressions(regularPriceChange, priceExpression, multipleExpression, msrpPriceExpression, msrpMultipleExpression)

                resultsTable.Columns.Add("NewPrice", GetType(Decimal), priceExpression)
                resultsTable.Columns.Add("NewMultiple", GetType(Integer), multipleExpression)
                resultsTable.Columns.Add("NewMSRPPrice", GetType(Decimal), msrpPriceExpression)
                resultsTable.Columns.Add("NewMSRPMultiple", GetType(Integer), msrpMultipleExpression)
                resultsTable.Columns.Add("NewMargin", GetType(Decimal), "iif(NewPrice = 0 or NewMultiple = 0, 0, (1 - (UnitCost / (NewPrice / NewMultiple))) * 100)")
            End If

            Return resultsTable
        End Function

        Private Sub CalcPreview()
            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            Dim lastGroup As String = String.Empty
            Dim grp As ListViewGroup = Nothing

            lstPreview.Items.Clear()
            lstPreview.Groups.Clear()

            If (_itemPriceHistory IsNot Nothing) Then
                For Each row As Data.DataRow In _itemPriceHistory.Rows
                    If lastGroup <> row("item_key").ToString Then
                        lastGroup = row("item_key").ToString
                        grp = New ListViewGroup(row("item_description").ToString + " - " + row("Identifier").ToString)
                        lstPreview.Groups.Add(grp)
                    End If

                    ProcessPreviewRow(row, grp)
                Next
            Else
                ' No history found
            End If

            Windows.Forms.Cursor.Current = Cursors.Default
        End Sub

        Function FinalText() As String
            Dim builder As New StringBuilder

            If IsRegularPriceChange() Then
                builder.Append(My.Resources.ItemChaining.RegularPriceChangeDescription)

                If radChangePOSpriceto.Checked Then
                    builder.AppendFormat(My.Resources.ItemChaining.RegularPriceChangeMethodAbsolute, txtChangePOSpricetoQty.Text, Convert.ToDecimal(txtChangePOSpricetoPrice.Text))
                Else
                    builder.AppendFormat(My.Resources.ItemChaining.RegularPriceChangeMethodPercentage, CDec(txtChangePOSpricebyPercent.Text) / 100.0)
                End If

                builder.AppendFormat(My.Resources.ItemChaining.RegularPriceChangeDate, mclReg.SelectionStart)
            Else
                builder.Append(My.Resources.ItemChaining.PromoPriceChangeDescription)

                If radChangePOSpromopriceto.Checked Then
                    builder.AppendFormat(My.Resources.ItemChaining.PromoPriceChangeMethodAbsolute, txtChangePOSpromopricetoQty.Text, CDec(txtChangePOSpromopricetoPrice.Text))

                    If txtChangePOSpromopricetoQtyMSRP.Text <> "" And txtChangePOSpromopricetoPriceMSRP.Text <> "" Then
                        builder.AppendFormat(My.Resources.ItemChaining.PromoPriceChangeMethodMSRP, txtChangePOSpromopricetoQtyMSRP.Text, CDec(txtChangePOSpromopricetoPriceMSRP.Text))
                    End If
                End If

                If radChangePOSpromopricebyMSRP.Checked Then
                    builder.AppendFormat(My.Resources.ItemChaining.PromoPriceChangeMethodPercentageMSRP, CDec(txtChangePOSpromopricebyMSRP.Text) / 100.0)
                End If

                If radChangePOSpromopricebyREG.Checked Then
                    builder.AppendFormat(My.Resources.ItemChaining.PromoPriceChangeMethodPercentageRegular, CDec(txtChangePOSpromopricebyREG.Text) / 100.0)
                End If

                builder.AppendFormat(My.Resources.ItemChaining.PromoPriceChangeDate, mclPromoStart.SelectionStart, mclPromoEnd.SelectionStart)
            End If

            Return builder.ToString()
        End Function

        Private Sub ShowNextWarning()
            If _warnings.Count = 0 Then
                grpWarning.Visible = False
            Else
                lblWarning.Text = _warnings.Values(0)
                _warnings.RemoveAt(0)
                grpWarning.Visible = True
            End If
        End Sub
#End Region

#Region "UI"
        Private Function EnabledColor(ByVal textBox As TextBox) As Color
            If textBox.Enabled Then Return Color.White Else Return Color.Silver
        End Function

        Public Sub PopulateDropDown(ByVal dataSet As DataSet, ByVal comboBox As ComboBox, ByVal valueField As String)
            comboBox.Items.Clear()

            For Each row As Data.DataRow In dataSet.Tables(0).Rows
                comboBox.Items.Add(row(valueField))
            Next
        End Sub

        Private Sub SetDecimal(ByVal sender As System.Object)
            Dim TextBox As TextBox = CType(sender, TextBox)

            If TextBox.Text.Length = 0 Then Return

            Try
                Dim v As Decimal = Convert.ToDecimal(TextBox.Text)
                TextBox.Text = v.ToString("N2")
            Catch ex As Exception
                TextBox.Text = String.Empty
            End Try
        End Sub
#End Region

        Private Function IsRegularPriceChange() As Boolean
            Return lstPriceType.SelectedItems(0).SubItems(1).Text = "False"
        End Function

        Private Sub ResetButtons()
            btnNext.Visible = True
            btnFinish.Visible = False
            btnNext.Enabled = True
            btnFinish.Enabled = False
            btnBack.Enabled = False
        End Sub

        Private Sub CreatePriceChanges(ByVal regularPriceChange As Boolean)
            Dim success As Boolean = True

            Try
                Dim validationCode As Integer

                For Each priceChange As PriceChangeBO In _priceChangeList
                    If regularPriceChange Then
                        validationCode = priceChange.SaveRegularPriceChange()
                    Else
                        validationCode = priceChange.SavePromoPriceChange()
                    End If

                    If validationCode <> 0 AndAlso ValidationDAO.IsErrorCode(validationCode) Then
                        success = False

                        Dim message As ValidationBO = ValidationDAO.GetValidationCodeDetails(validationCode)
                        DisplayErrorMessage(message.ValidationCodeTypeDesc)
                        Exit For
                    End If
                Next
            Catch ex As Exception
                success = False
            End Try

            Windows.Forms.Cursor.Current = Cursors.Default

            If (success) Then
                MessageBox.Show(My.Resources.ItemChaining.PriceChangesSuccessful, My.Resources.ItemChaining.PriceChangeWizardTitle)
            End If

            TabControl1.SelectedTab = wpSearchItems
            btnNext.Enabled = True
        End Sub
#End Region

#Region "Form Event Handlers"
        Private Sub frmChaining2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            pnlWorkArea.SendToBack()
            lblTitle.Text = TabControl1.SelectedTab.Text
            lblHeader.Text = TabControl1.SelectedTab.Tag.ToString()
            btnBack.Enabled = False

            isSelectStores.FoundItemsListView.StateImageList = Nothing
            isSelectStores.SelectedItemsListView.StateImageList = Nothing
            isSelectStores.SelectFromDataSet = _store.ListStores()
            'populate dropdowns
            PopulateDropDown(_settings.ListZones, cmbZone, "Zone_Name")
            cmbZone.Items.Insert(0, My.Resources.CompetitorStore.AllItems)
            cmbZone.SelectedIndex = 0

            PopulateDropDown(_settings.ListStates, cmbState, "State")
            cmbState.Items.Insert(0, My.Resources.CompetitorStore.AllItems)
            cmbState.SelectedIndex = 0

            AddHandler cmbZone.SelectedIndexChanged, New EventHandler(AddressOf cmbZone_SelectedIndexChanged)
            AddHandler cmbState.SelectedIndexChanged, New EventHandler(AddressOf cmbState_SelectedIndexChanged)

            'Load items
            lstPriceType.Items.Clear()

            For Each row As Data.DataRow In _settings.ListPriceTypes(True).Tables(0).Rows
                Dim item As ListViewItem = New ListViewItem()

                item.Text = row("PriceChgTypeId").ToString + ". " + row("PriceChgTypeDesc").ToString()

                item.SubItems.Add(row("On_Sale").ToString())
                item.SubItems.Add(row("MSRP_Required").ToString())
                item.SubItems.Add(row("LineDrive").ToString())
                item.SubItems.Add(row("PriceChgTypeId").ToString())

                If row("On_Sale").ToString().Equals("True") Then
                    item.StateImageIndex = 1
                Else
                    item.StateImageIndex = 0
                End If

                lstPriceType.Items.Add(item)
            Next

            ResetButtons()

            Windows.Forms.Cursor.Current = Cursors.Default
        End Sub
#End Region

#Region "Button Event Handlers"
        Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            If MsgBox(My.Resources.ItemChaining.ConfirmPriceChangeWizardExit, MsgBoxStyle.OkCancel, My.Resources.ItemChaining.ConfirmPriceChangeWizardExitTitle) = MsgBoxResult.Ok Then
                Me.Close()
            End If
        End Sub

        Private Sub btnFinish_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFinish.Click
            Windows.Forms.Cursor.Current = Cursors.WaitCursor

            btnBack.Enabled = False
            btnFinish.Enabled = False

            CreatePriceChanges(IsRegularPriceChange())
        End Sub

        Private Sub btnPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPreview.Click
            TabControl1.SelectedTab = wpPreview
        End Sub

        Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
            If TabControl1.SelectedTab Is wpSearchItems Then
                Windows.Forms.Cursor.Current = Cursors.WaitCursor
                lblWait.Visible = True
                Update()
                ItemSearchControl1.Search()
                ItemSelectingControl1.SelectFromDataSet = ItemSearchControl1.DataSet

                lblWait.Visible = False
                TabControl1.SelectedTab = wpSelectItems
                Windows.Forms.Cursor.Current = Cursors.Default
            ElseIf TabControl1.SelectedTab Is wpPriceType Then
                If IsRegularPriceChange() Then
                    TabControl1.SelectedTab = wpReg
                Else
                    TabControl1.SelectedTab = wpPromo
                End If
            ElseIf TabControl1.SelectedTab Is wpReg Then
                TabControl1.SelectedTab = wpFinal
            ElseIf TabControl1.SelectedTab Is wpPromo Then
                If cmbPromoType.Text = "EARNED DISCOUNT" Then
                    TabControl1.SelectedTab = wpEarnedDiscount
                Else
                    TabControl1.SelectedTab = wpFinal
                End If
            ElseIf TabControl1.SelectedTab Is wpEarnedDiscount Then
                TabControl1.SelectedTab = wpFinal
            Else
                TabControl1.SelectedIndex = TabControl1.SelectedIndex + 1

                If TabControl1.SelectedIndex = TabControl1.TabCount - 1 Then
                    btnNext.Visible = False
                    btnFinish.Visible = True
                End If
            End If

            btnBack.Enabled = True
        End Sub

        Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
            btnNext.Visible = True
            btnFinish.Visible = False

            If TabControl1.SelectedTab Is wpPreview Then
                TabControl1.SelectedTab = wpFinal
                btnNext.Visible = False
                btnFinish.Visible = True
            ElseIf TabControl1.SelectedTab Is wpPromo Then
                TabControl1.SelectedTab = wpPriceType
            ElseIf TabControl1.SelectedTab Is wpFinal Then
                If IsRegularPriceChange() Then
                    TabControl1.SelectedTab = wpReg
                Else
                    TabControl1.SelectedTab = wpPromo
                End If
            Else
                TabControl1.SelectedIndex = TabControl1.SelectedIndex - 1
            End If

            btnBack.Enabled = TabControl1.SelectedIndex > 0
        End Sub
#End Region

#Region "CheckBox Event Handlers"
        Private Sub chkIAggree_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIAggree.CheckedChanged
            btnFinish.Enabled = chkIAggree.Checked
        End Sub
#End Region

#Region "Radio Button Event Handlers"
        Private Sub radChangePOSpriceto_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radChangePOSpriceto.CheckedChanged
            If radChangePOSpriceto.Checked Then
                btnNext.Enabled = wpRegValidate()
                If radChangePOSpriceto.Checked Then
                    txtChangePOSpricetoQty.Focus()
                    txtChangePOSpricebyPercent.Text = String.Empty
                End If
            End If
        End Sub

        Private Sub radChangePOSpriceby_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radChangePOSpriceby.CheckedChanged
            If radChangePOSpriceby.Checked Then
                btnNext.Enabled = wpRegValidate()
                If radChangePOSpriceby.Checked Then
                    txtChangePOSpricebyPercent.Focus()
                    txtChangePOSpricetoQty.Text = String.Empty
                    txtChangePOSpricetoPrice.Text = String.Empty
                End If
            End If
        End Sub

        Private Sub radChangePOSpromopriceto_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radChangePOSpromopriceto.CheckedChanged
            If radChangePOSpromopriceto.Checked Then
                btnNext.Enabled = wpPromoValidate()
                txtChangePOSpromopricetoQty.Focus()
                txtChangePOSpromopricebyMSRP.Text = String.Empty
                txtChangePOSpromopricebyREG.Text = String.Empty
            End If
        End Sub

        Private Sub radChangePOSpromopricebyMSRP_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radChangePOSpromopricebyMSRP.CheckedChanged
            If radChangePOSpromopricebyMSRP.Checked Then
                btnNext.Enabled = wpPromoValidate()
                txtChangePOSpromopricebyMSRP.Focus()
                txtChangePOSpromopricebyREG.Text = String.Empty
                txtChangePOSpromopricetoQty.Text = String.Empty
                txtChangePOSpromopricetoPrice.Text = String.Empty
                txtChangePOSpromopricetoQtyMSRP.Text = String.Empty
                txtChangePOSpromopricetoPriceMSRP.Text = String.Empty
            End If
        End Sub

        Private Sub radChangePOSpromopricebyREG_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radChangePOSpromopricebyREG.CheckedChanged
            If radChangePOSpromopricebyREG.Checked Then
                btnNext.Enabled = wpPromoValidate()
                txtChangePOSpromopricebyREG.Focus()
                txtChangePOSpromopricebyMSRP.Text = String.Empty
                txtChangePOSpromopricetoQty.Text = String.Empty
                txtChangePOSpromopricetoPrice.Text = String.Empty
                txtChangePOSpromopricetoQtyMSRP.Text = String.Empty
                txtChangePOSpromopricetoPriceMSRP.Text = String.Empty
            End If
        End Sub
#End Region

#Region "TextBox Event Handlers"
        Private Sub txtChangePOSpromopricetoQty_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtChangePOSpromopricetoQtyMSRP.TextChanged, txtChangePOSpromopricetoQty.TextChanged, txtChangePOSpromopricetoPriceMSRP.TextChanged, txtChangePOSpromopricetoPrice.TextChanged, txtChangePOSpromopricebyREG.TextChanged, txtChangePOSpromopricebyMSRP.TextChanged
            btnNext.Enabled = wpPromoValidate()
        End Sub

        Private Sub txtChangePOSpricetoQty_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtChangePOSpricetoQty.TextChanged, txtChangePOSpricetoPrice.TextChanged, txtChangePOSpricebyPercent.TextChanged
            btnNext.Enabled = wpRegValidate()
        End Sub

        Private Sub txtChangePOSpricetoQty_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtChangePOSpricetoQty.Enter
            radChangePOSpriceto.Checked = True
        End Sub

        Private Sub txtChangePOSpricetoPrice_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtChangePOSpricetoPrice.Enter
            radChangePOSpriceto.Checked = True
            txtChangePOSpricetoPrice.Focus()
        End Sub

        Private Sub txtChangePOSpricebyPercent_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtChangePOSpricebyPercent.Enter
            radChangePOSpriceby.Checked = True
        End Sub

        Private Sub txtChangePOSpromopricetoQty_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtChangePOSpromopricetoQty.Enter
            radChangePOSpromopriceto.Checked = True
        End Sub

        Private Sub txtChangePOSpromopricetoPrice_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtChangePOSpromopricetoPrice.Enter
            radChangePOSpromopriceto.Checked = True
            txtChangePOSpromopricetoPrice.Focus()
        End Sub

        Private Sub txtChangePOSpromopricetoQtyMSRP_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtChangePOSpromopricetoQtyMSRP.Enter
            radChangePOSpromopriceto.Checked = True
            txtChangePOSpromopricetoQtyMSRP.Focus()
        End Sub

        Private Sub txtChangePOSpromopricetoPriceMSRP_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtChangePOSpromopricetoPriceMSRP.Enter
            radChangePOSpromopriceto.Checked = True
            txtChangePOSpromopricetoPriceMSRP.Focus()
        End Sub

        Private Sub txtChangePOSpromopricebyMSRP_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtChangePOSpromopricebyMSRP.Enter
            radChangePOSpromopricebyMSRP.Checked = True
        End Sub

        Private Sub txtChangePOSpromopricebyREG_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtChangePOSpromopricebyREG.Enter
            radChangePOSpromopricebyREG.Checked = True
        End Sub

        Private Sub DecimalTextBox_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtChangePOSpricetoPrice.Leave, txtChangePOSpricebyPercent.Leave, txtChangePOSpromopricetoPriceMSRP.Leave, txtChangePOSpromopricetoPrice.Leave, txtChangePOSpromopricebyREG.Leave, txtChangePOSpromopricebyMSRP.Leave
            SetDecimal(sender)
        End Sub
#End Region

#Region "ListView Event Handlers"
        Private Sub lstPriceType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstPriceType.SelectedIndexChanged
            btnNext.Enabled = lstPriceType.SelectedItems.Count > 0
        End Sub

        Private Sub lstPriceType_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstPriceType.DoubleClick
            If IsRegularPriceChange() Then
                TabControl1.SelectedTab = wpReg
            Else
                TabControl1.SelectedTab = wpPromo
            End If
        End Sub
#End Region

#Region "TabControl Event Handlers"
        Private Sub TabControl1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
            lblTitle.Text = TabControl1.SelectedTab.Text
            lblHeader.Text = TabControl1.SelectedTab.Tag.ToString()
            btnBack.Enabled = True
            btnFinish.Visible = False
            btnNext.Visible = True

            If TabControl1.SelectedTab Is wpSearchItems Then
                ResetButtons()
            ElseIf TabControl1.SelectedTab Is wpSelectStores Then
                Windows.Forms.Cursor.Current = Cursors.WaitCursor
                Update()
                btnNext.Enabled = isSelectStores.SelectedItemsListView.Items.Count > 0
                Windows.Forms.Cursor.Current = Cursors.Default
            ElseIf TabControl1.SelectedTab Is wpPriceType Then
                btnNext.Enabled = lstPriceType.SelectedItems.Count > 0

            ElseIf TabControl1.SelectedTab Is wpReg Then
                mclReg.MinDate = Now
                btnNext.Enabled = wpRegValidate()

            ElseIf TabControl1.SelectedTab Is wpPreview Then
                CalcPreview()
                btnNext.Enabled = False
            ElseIf TabControl1.SelectedTab Is wpPromo Then
                btnNext.Enabled = wpPromoValidate()
                lblTitle.Text = String.Format(My.Resources.ItemChaining.PromoPriceChangeTitle, lstPriceType.SelectedItems(0).Text)
                mclPromoStart.MinDate = Now
                mclPromoEnd.MinDate = Now.AddDays(1)
                If lstPriceType.SelectedItems(0).SubItems(3).Text = "True" Then
                    If radChangePOSpromopriceto.Checked Then
                        radChangePOSpromopricebyMSRP.Checked = True
                    End If
                End If
            ElseIf TabControl1.SelectedTab Is wpFinal Then
                btnFinish.Visible = True
                btnNext.Visible = False

                Update()

                lstSelectedStores.BeginUpdate()
                lstSelectedItems.BeginUpdate()

                lstSelectedStores.Items.Clear()
                lstSelectedItems.Items.Clear()

                For n As Integer = 0 To ItemSelectingControl1.SelectedItems.Count - 1
                    Dim item As ListViewItem = CType(ItemSelectingControl1.SelectedItems(n).Clone(), ListViewItem)
                    lstSelectedItems.Items.Add(item)
                Next

                For n As Integer = 0 To isSelectStores.SelectedItems.Count - 1
                    Dim item As ListViewItem = CType(isSelectStores.SelectedItems(n).Clone(), ListViewItem)
                    lstSelectedStores.Items.Add(item)
                Next

                lstSelectedStores.EndUpdate()
                lstSelectedItems.EndUpdate()

                lblFinal.Text = FinalText()
                btnFinish.Enabled = chkIAggree.Checked

                ' Thread hopping so we can fire this event handler again when the tab page is supposed to change
                ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf ValidateFinal))
            End If
        End Sub
#End Region

#Region "ItemSelectingControl Event Handlers"
        Private Sub ItemSelectingControl1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemSelectingControl1.SelectedIndexChanged
            lblMessage.Text = String.Empty

            If ItemSelectingControl1.FoundItemsListView.SelectedItems.Count > 0 AndAlso ItemSelectingControl1.FoundItemsListView.SelectedItems(0).StateImageIndex = 1 Then
                Dim chains As String = String.Empty

                Windows.Forms.Cursor.Current = Cursors.WaitCursor

                _chain.ID = ItemSelectingControl1.FoundItemsListView.SelectedItems(0).SubItems(3).Text

                For Each row As Data.DataRow In _chain.ListItemChains(ItemSelectingControl1.FoundItemsListView.SelectedItems(0).SubItems(2).Text).Tables(0).Rows
                    If chains.Length > 0 Then chains = chains + ", "

                    chains = chains + row("ItemChainDesc").ToString()
                Next

                lblMessage.Text = String.Format(My.Resources.ItemChaining.ItemInChainWarning, chains)

                If chains.IndexOf(",") > -1 Then lblMessage.Text = lblMessage.Text + "s"

                Windows.Forms.Cursor.Current = Cursors.Default
            End If
        End Sub

        Private Sub isSelectStores_ButtonStateChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles isSelectStores.ButtonStateChanged
            btnNext.Enabled = isSelectStores.SelectedListCount > 0
        End Sub

        Private Sub ItemSelectingControl1_ButtonStateChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ItemSelectingControl1.ButtonStateChanged
            btnNext.Enabled = ItemSelectingControl1.SelectedListCount > 0
        End Sub
#End Region

#Region "ComboBox Event Handlers"
        Private Sub cmbZone_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
            cmbState.SelectedIndex = 0

            If cmbZone.Text = My.Resources.CompetitorStore.AllItems Then
                isSelectStores.SelectFromDataSet = _store.ListStores
            Else
                isSelectStores.SelectFromDataSet = _store.ListStores(cmbZone.Text)
            End If
        End Sub

        Private Sub cmbState_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
            cmbZone.SelectedIndex = 0

            If cmbState.Text = My.Resources.CompetitorStore.AllItems Then
                isSelectStores.SelectFromDataSet = _store.ListStores
            Else
                isSelectStores.SelectFromDataSet = _store.ListStoresByState(cmbState.Text)
            End If
        End Sub
#End Region

        Private Sub mclPromoStart_DateChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DateRangeEventArgs) Handles mclPromoStart.DateChanged
            mclPromoStart.MinDate = Now
            mclPromoEnd.MinDate = mclPromoStart.SelectionStart.AddDays(1)
        End Sub

        Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox2.Click, lblWarning.Click
            ShowNextWarning()
        End Sub
    End Class
End Namespace