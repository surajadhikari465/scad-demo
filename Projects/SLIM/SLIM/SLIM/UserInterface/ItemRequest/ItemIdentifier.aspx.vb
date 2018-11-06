Imports SLIM.InputValidation
Imports Microsoft.VisualBasic.FileIO
Imports SLIM.WholeFoods.IRMA.Common.BusinessLogic
Imports SLIM.WholeFoods.IRMA.Common.DataAccess
Imports SLIM.upcFormat


Partial Class UserInterface_ItemIdentifier
    Inherits System.Web.UI.Page

    Dim _dateErrorString As String
    Dim _simpleItemDescriptionView As Boolean = True


    Protected Sub ImageButton4_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton4.Click
        Dim idExists As Boolean
        Dim idRequestedExists As Boolean
        Dim da As New NewItemRequestTableAdapters.ItemRequestTableAdapter

        If upcTxBx.Text IsNot "" Then
            Try
                ' ***** Check for existing UPC in ItemCatalog and ItemRequest table ****
                idExists = InputValidation.IdentifierCheck(upcTxBx.Text.ToString)
                ' ***** Check for existing UPC in ItemRequest *******
                idRequestedExists = da.IdentifierRequestedExists(upcTxBx.Text.ToString)
            Catch ex As Exception
                upcErrorLbl.Text = ex.Message
            End Try
            If idExists Then
                upcErrorLbl.ForeColor = Drawing.Color.Red
                upcErrorLbl.Text = "This Identifier already " & _
                "exists - Please use WebQuery to get Item Info"

            ElseIf idRequestedExists Then
                upcErrorLbl.ForeColor = Drawing.Color.Red
                upcErrorLbl.Text = "This Identifier has already " & _
                "been requested - Please use WebQuery to get Item Info"

            Else
                upcErrorLbl.ForeColor = Drawing.Color.Green
                upcErrorLbl.Text = "Identifier is available to use"
            End If
        End If
    End Sub

    Protected Sub RadioButtonList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles IdentifierRadio.SelectedIndexChanged
        ScaleInfoLabel.Text = ""
        upcTxBx.Text = ""
        Dim webAl As New ArrayList
        Dim wc As WebControl
        webAl.Add(ScaleDesc1)
        webAl.Add(ScaleDesc2)
        webAl.Add(ScaleDesc3)
        webAl.Add(ScaleDesc4)
        webAl.Add(Ingredients)
        webAl.Add(ShelfLife)
        webAl.Add(TareDropDown)
        webAl.Add(ScaleUOMDropDown)
        webAl.Add(RandomWeightDropDown)
        webAl.Add(FixedWeightTxBx)
        webAl.Add(ByCountTxBx)
        webAl.Add(ScaleLabelTypeDropDown)
        webAl.Add(LabelStyleDropDown)
        If IdentifierRadio.SelectedValue = 2 Then
            upcTxBx.Text = "20000000000"
            For Each wc In webAl
                wc.Enabled = True
            Next
            IdentifierInfoLabel.Text = "Temporary Scale PLU created"
            ScaleInfoLabel.Text = "Please fill out Scale Informnation"
        ElseIf IdentifierRadio.SelectedValue = 3 Then
            For Each wc In webAl
                wc.Enabled = False
            Next
            upcTxBx.Text = GetPLUstring()
            ScaleInfoLabel.Text = "Not a Scale Item - Please skip this Page"
            IdentifierInfoLabel.Text = "Temporary PLU has been created"
        Else
            IdentifierInfoLabel.Text = ""
            For Each wc In webAl
                wc.Enabled = False
            Next
            ScaleInfoLabel.Text = "Not a Scale Item - Please skip this Page"
            upcTxBx.Focus()
        End If
        webAl = Nothing
    End Sub

    Protected Sub PopulateLocalFlexItemAttribs()

        Dim dv As DataView = DataSourceLocal.Select(DataSourceSelectArguments.Empty)
        Dim dt As DataTable = dv.ToTable()
        Dim valueList As String

        If dt.Rows.Count > 0 Then
            TextBoxTempHidden.Text = dt.Rows(0)("field_values")
            valueList = TextBoxTempHidden.Text

            Dim ItemAttribValues As String() = valueList.Split(New String() {","}, StringSplitOptions.RemoveEmptyEntries)
            Dim i As Integer = 0

            While i <= ItemAttribValues.Length - 1
                GoLocalDropDown.Items.Add(New ListItem(ItemAttribValues(i), (i + 1).ToString()))
                i += 1
            End While
        End If
    End Sub

    Protected Sub PopulateCommCodeFlexItemAttribs()

        Dim dv As DataView = DataSourceCommCode.Select(DataSourceSelectArguments.Empty)
        Dim dt As DataTable = dv.ToTable()
        Dim valueList As String

        If dt.Rows.Count > 0 Then
            TextBoxTempHidden.Text = dt.Rows(0)("field_values")
            valueList = TextBoxTempHidden.Text

            Dim ItemAttribValues As String() = valueList.Split(New String() {","}, StringSplitOptions.RemoveEmptyEntries)
            Dim i As Integer = 0

            While i <= ItemAttribValues.Length - 1
                CommodityDropDown.Items.Add(New ListItem(ItemAttribValues(i), (i + 1).ToString()))
                i += 1
            End While
        End If
    End Sub

    Protected Sub SetSimpleItemDescriptionView()
        lblDistributionUnits.Visible = False
        DistUnitsDropDown.Visible = False
        lblAgeCode.Visible = False
        Textbox_AgeCode.Visible = False
        regAgeCode.Enabled = False
        trShelfLabelType.Visible = False
        trHasIngredients.Visible = False
        trFixedWeight.Visible = False
        trKeepFrozen.Visible = False
        trQuantityProhibit.Visible = False
        tdRefrigerated.Visible = False
        trPriceRequired.Visible = False
        trDiscountTerms.Visible = False
        trMisc.Visible = False
        trNotAvailable.Visible = False
        trMixMatch.Visible = False
        trPOSLinkCode.Visible = False
        trVenue.Visible = False
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim WizIndex As Integer = 8

        ' ****** Check Permission **************
        If Not IsPostBack Then
            If Not Session("ItemRequest") = True And Not Session("AccessLevel") = 3 Then
                Response.Redirect("~/AccessDenied.aspx", True)
            End If
            Wizard1.ActiveStepIndex = 0
            upcTxBx.Focus()
            If Session("AccessLevel") = 1 Then
                SubTeamDropDown.DataSourceID = "SqlDataSource5"
                SubTeamDropDown.DataBind()
            End If

            'This populates the flexible item attrib dropdowns
            PopulateLocalFlexItemAttribs()
            PopulateCommCodeFlexItemAttribs()
            PromoHider(False)

            Label_NotAvail.Visible = False
            NotAvailTxBx.Visible = False

            WebDateChooser5.Value = Date.Today
            WebDateChooser6.Value = CDate("06/06/2079")
        End If

        ' Set the simple item description view flag
        _simpleItemDescriptionView = CInt(Application.Get("Simple_New_Item_View"))

        If Not IsPostBack AndAlso _simpleItemDescriptionView Then
            SetSimpleItemDescriptionView()
        End If

        'Removed scale page if scale flag is true
        If Session("Slim_ScaleInfo") = False Then
            'removes the scale link from the wizard
            WizIndex = 7
            'removes the scale and PLU buttons from the main screen
            IdentifierRadio.Items(2).Enabled = False
            IdentifierRadio.Items(1).Enabled = False

        End If

        ' ********* Get CRV - Bottle Deposit Subteams *******
        If Application.Get("CRV-BottleDepositSubteams").ToString <> "" Then
            If InStr(Application.Get("CRV-BottleDepositSubteams").ToString, "(", CompareMethod.Text) > 0 Then
                SqlDataSource33.SelectCommand = "SELECT Item_Description, Item_Key, subteam_no FROM Item I " & _
                "WHERE subteam_No in " & Application.Get("CRV-BottleDepositSubteams") & _
                " ORDER BY Item_Description "
            Else
                SqlDataSource33.SelectCommand = "SELECT Item_Description, Item_Key, subteam_no FROM Item I " & _
                "WHERE subteam_No in (" & Application.Get("CRV-BottleDepositSubteams") & ")" & _
                " ORDER BY Item_Description "
            End If
        Else
            SqlDataSource33.SelectCommand = ""
        End If

        Label21.Text = Application.Get("CRV-BottleDepositLabel")
        ' *****************************

        ' *****************************
        ' ****** Scale Stuff *********
        'If IdentifierRadio.SelectedValue = 2 Then
        '    Wizard1.WizardSteps(4).Visible = True
        'End If
        ' ****************************
        posDescTxBx.Text = posDescTxBx.Text.ToUpper
        itemDescTxBx.Text = itemDescTxBx.Text.ToUpper
        caseCostTxBx.Text = IIf(InStr(caseCostTxBx.Text.ToString, ".", CompareMethod.Text), _
        caseCostTxBx.Text.ToString, caseCostTxBx.Text.ToString & ".00")
        priceTxBx.Text = IIf(InStr(priceTxBx.Text.ToString, ".", CompareMethod.Text), _
        priceTxBx.Text.ToString, priceTxBx.Text.ToString & ".00")

        ' ******************* Populate the Summary Table ***************************

        If IsPostBack And Wizard1.WizardSteps(WizIndex).Title = "Summary" Then
            FinishLabel.Text = ""
            SLabelUPC.Text = upcTxBx.Text
            If SubTeamDropDown.SelectedValue <> 0 Then
                SLabelSub.Text = SubTeamDropDown.SelectedItem.ToString
            End If
            SLabelDesc.Text = itemDescTxBx.Text.ToUpper
            SLabelPOSDesc.Text = posDescTxBx.Text.ToUpper
            If CheckBox2.Checked = False Then
                SLabelBrand.Text = txtBrand.Text
                SLabelNBrand.Text = Nothing
            Else
                SLabelBrand.Text = Nothing
                SLabelNBrand.Text = brandTxBx.Text.ToUpper
            End If
            SLabelItemSize.Text = itemSizeTxBx.Text
            If UOMDropDown.SelectedValue <> 0 Then
                SLabelUOM.Text = UOMDropDown.SelectedItem.ToString
            End If
            SLabelPack.Text = packTxBx.Text
            If UOMDropDown.SelectedValue <> 0 Then
                SLabelUOM.Text = UOMDropDown.SelectedItem.ToString
            End If
            If CatDropDown.SelectedValue <> 0 Then
                SLabelItemCat.Text = CatDropDown.SelectedItem.ToString
            End If
            If ClassDropDown.SelectedValue <> 0 Then
                SLabelClass.Text = ClassDropDown.SelectedItem.ToString
            End If
            If CkBx3.Checked = True Then
                SLabelNVend.Text = NewVendorDropDown.SelectedItem.ToString
                SLabelVend.Text = Nothing
            Else
                SLabelNVend.Text = Nothing
                SLabelVend.Text = txtVendor.Text
            End If
            'SLabelCost.Text = caseCostTxBx.Text
            SLabelCost.Text = "$" & caseCostTxBx.Text
            SLabelCaseSize.Text = caseSizeTxBx.Text
            SLabelVendorOrd.Text = vendorOrderTxBx.Text.ToUpper
            If Not priceTxBx.Text = "" Then
                SLabelPrice.Text = priceMultiTxBx.Text & "@$" & priceTxBx.Text.ToString
            End If
            SLabelACode.Text = CStr(IIf(SubTeamDropDown.SelectedValue = 25 Or SubTeamDropDown.SelectedValue = 15, 2, 0))
            SLabelFS.Text = CStr(IIf(FoodStampsCK.Checked = True, "Yes", "No"))
            SLabelCRV.Text = CRVDropDown.SelectedItem.ToString
            SLabelTax.Text = txtTaxClass.Text
            SLabelStore.Text = Session("Store_name")
        End If
        ' ***************************************************************************
    End Sub

    Protected Sub Wizard1_FinishButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles Wizard1.FinishButtonClick
        Dim da As New NewItemRequestTableAdapters.ItemRequestTableAdapter
        Dim dt As New NewItemRequest.ItemRequestDataTable
        Dim dr As NewItemRequest.ItemRequestRow = dt.NewItemRequestRow()
        Dim upcExistsIrma As Boolean
        Dim upcExistsSlim As Boolean
        Dim al As New ArrayList
        Dim TxBx As New TextBox()
        Dim DropDown As New DropDownList()
        Dim isValidated As Boolean = True
        Dim errorString As String = ""
        Dim ItemRequest_ID As Integer
        Dim returnDate As DateTime
        Dim vendorID As Integer = 0
        Dim brandID As Integer = -1
        Dim taxClassID As Nullable(Of Integer)


        ' ******************     *********************
        ' ***** Get all the TextBox and DropDown data ******
        ' ******************     *********************
        ' ************** Validate the Form *****************************
        ' ***** Walk the TextBoxes ******* Look for Empties ******
        al.Add(upcTxBx)
        al.Add(itemDescTxBx)
        al.Add(posDescTxBx)
        al.Add(itemSizeTxBx)
        al.Add(packTxBx)
        al.Add(priceTxBx)
        al.Add(priceMultiTxBx)
        al.Add(caseCostTxBx)
        al.Add(caseSizeTxBx)
        'removed due to no being required.
        'al.Add(TextBox_MixMatch)
        'al.Add(FreightTxBx)
        'al.Add(MSRPPriceMultTxBx)
        'al.Add(MSRPPriceTxBx)
        If CheckBox2.Checked = False Then
            al.Add(txtBrand)
        End If
        If CkBx3.Checked = False Then
            al.Add(txtVendor)
        End If
        al.Add(txtTaxClass)
        If CheckBox2.Checked Then
            al.Add(brandTxBx)
        End If
        If PromotionalCK.Checked = True Then
            al.Add(AllowanceTxBx)
            al.Add(DiscountsTxBx)
        End If
        If NotAvailableCK.Checked = True Then
            al.Add(NotAvailTxBx)
        End If
        If IdentifierRadio.SelectedValue = 2 Then
            al.Add(ShelfLife)
            al.Add(Ingredients)
        End If
        For Each TxBx In al
            If TxBx.Text.ToString = Nothing Then
                isValidated = False
                errorString = TxBx.CssClass & " is empty or valued Zero!"
            End If
            'TODO: Check for apostrophe and other illegal Characters
        Next
        al.Clear()

        ' Auto-complete text boxes
        If Not CkBx3.Checked Then
            If txtVendor.Text.Trim().Length > 0 Then
                vendorID = ItemSearch.GetVendorIDByName(txtVendor.Text.Trim)
            End If

            If vendorID = 0 Then
                isValidated = False
                errorString = "No valid vendor selected!"
            End If
        End If

        If Not CheckBox2.Checked Then
            If txtBrand.Text.Trim().Length > 0 Then
                brandID = ItemSearch.GetBrandIDByName(txtBrand.Text.Trim())
            End If

            If brandID = 0 Then
                isValidated = False
                errorString = "No valid item brand selected!"
            End If
        End If

        If txtTaxClass.Text.Trim().Length > 0 Then
            taxClassID = ItemSearch.GetTaxClassIDByName(txtTaxClass.Text.Trim())
        End If

        If Not taxClassID.HasValue Then
            isValidated = False
            errorString = "No valid tax class selected!"
        End If

        ' *************** Check Retail/Cost for Zero  *******************
        If CSng(caseCostTxBx.Text) = 0 Then
            isValidated = False
            errorString = caseCostTxBx.CssClass & " is Zero!"
        ElseIf CSng(priceTxBx.Text) = 0 Then
            isValidated = False
            errorString = priceTxBx.CssClass & " is Zero!"
        End If
        ' *************** Check if Retail and Cost UOM match for  *******************
        ' *************** Costed by Weight Items (unless Box) *******************
        If CostByWeightCK.Checked = True And Not RetailUnitsDropDown.SelectedValue = _
        CostUnitDropDown.SelectedValue And Not CostUnitDropDown.SelectedItem.Text = "BOX" Then
            isValidated = False
            errorString = "Cost Unit UOM not BOX/doesn't match Retail Unit!"
        End If
        ' ***** Walk the DropDown for 0 *******************************
        al.Add(UOMDropDown)
        If CkBx3.Checked = True Then
            al.Add(NewVendorDropDown)
        End If
        al.Add(CatDropDown)
        al.Add(SubTeamDropDown)
        'al.Add(TaxDropDown)
        'al.Add(CommodityDropDown)
        al.Add(CostUnitDropDown)
        al.Add(VendorUnitsDropDown)
        'al.Add(FreightUnitDropDown)
        al.Add(RetailUnitsDropDown)

        If Not _simpleItemDescriptionView Then
            If ShelfLabelDropDown.SelectedValue = -1 Then
                al.Add(ShelfLabelDropDown)
            End If

            al.Add(DistSubDropDown)
            al.Add(DistUnitsDropDown)
        End If

        al.Add(ManufUnitsDropDown)

        If IdentifierRadio.SelectedValue = 2 Then
            If ScaleLabelTypeDropDown.SelectedValue = -1 Then
                al.Add(ScaleLabelTypeDropDown)
            End If

        End If
        For Each DropDown In al
            If DropDown.SelectedValue = 0 Then
                isValidated = False
                errorString = DropDown.CssClass & " has no selected Value!"
            End If
        Next
        al = Nothing
        ' ******************     *********************

        ' ****** Check if UPC exists - in Pending and ItemCatalog *******
        Try
            ' **********************************************************************
            If Not Trim(upcTxBx.Text.ToString) = "20000000000" Then
                ' ***** Check for existing UPC in ItemCatalog and ItemRequest table ****
                upcExistsIrma = InputValidation.IdentifierCheck(Trim(upcTxBx.Text.ToString))
                upcExistsSlim = da.IdentifierRequestedExists(Trim(upcTxBx.Text.ToString))
                ' **********************************************************************
            End If

        Catch ex As Exception
            ErrorLabel1.Text = ex.Message
            ErrorLabel1.ForeColor = Drawing.Color.Red
        End Try
        '  **************************************************
        Dim ready2Apply As Boolean
        If vendorID = 0 And NewVendorDropDown.SelectedValue = 0 Then
            ready2Apply = False
        ElseIf upcTxBx.Text.StartsWith("P") = True Then
            ready2Apply = False
        Else
            ready2Apply = True
        End If
        '  **************************************************
        FinishLabel.ForeColor = Drawing.Color.Red

        If upcExistsIrma Then
            FinishLabel.Text = "This Identifier already " & _
            "exists - Please use WebQuery to get Item Info!"
        ElseIf upcExistsSlim Then
            FinishLabel.Text = "This Identifier has already " & _
            "been requested - Please use Items Pending to get Item Info!"
            ' Case 3941
            'ElseIf Len(upcTxBx.Text.ToString) > 11 And (upcTxBx.Text.EndsWith(0) = False _
            'Or upcTxBx.Text.StartsWith(0) = False) Then
            '    isValidated = False
            '    errorString = "Identifier Invalid - CheckDigit Entered"
            '    FinishLabel.Text = errorString
        ElseIf isValidated = False Then
            FinishLabel.Text = errorString
        ElseIf DateValidator(WebDateChooser5.Text) = False Then
            FinishLabel.Text = _dateErrorString
        ElseIf DateValidator(WebDateChooser6.Text) = False Then
            FinishLabel.Text = _dateErrorString
        ElseIf DateValidator(WebDateChooser1.Text) = False Then
            FinishLabel.Text = _dateErrorString
        ElseIf DateValidator(WebDateChooser2.Text) = False Then
            FinishLabel.Text = _dateErrorString
        ElseIf DateValidator(WebDateChooser3.Text) = False Then
            FinishLabel.Text = _dateErrorString
        ElseIf DateValidator(WebDateChooser4.Text) = False Then
            FinishLabel.Text = _dateErrorString


        Else
            Try
                ' ************************** Populate the DataRow with Form Values **************************
                dr.Identifier = Trim(upcTxBx.Text.ToString)
                dr.ItemStatus_ID = 1
                Select Case IdentifierRadio.SelectedValue
                    Case 1
                        dr.IdentifierType = "B"
                    Case 2
                        dr.IdentifierType = "O"
                    Case 3
                        dr.IdentifierType = "P"
                End Select
                dr.ItemType_ID = IdentifierRadio.SelectedValue
                'dr.IdentifierType = IdentifierTypeDropDown.SelectedValue
                dr.ItemTemplate = False
                dr.User_ID = Session("UserID")
                dr.User_Store = Session("Store_No")
                dr.UserAccessLevel_ID = Session("AccessLevel")
                If CkBx3.Checked = True Then
                    dr.VendorRequest_ID = NewVendorDropDown.SelectedValue
                Else
                    dr.VendorRequest_ID = 1
                End If
                If vendorOrderTxBx.Text = "" Then
                    vendorOrderTxBx.Text = warehouse.FormatToUPC(dr.Identifier)
                End If
                dr.Item_Description = Trim(itemDescTxBx.Text.ToString)
                dr.POS_Description = Trim(posDescTxBx.Text.ToString)
                dr.ItemUnit = UOMDropDown.SelectedValue
                dr.ItemSize = Trim(itemSizeTxBx.Text)
                dr.PackSize = Trim(packTxBx.Text)
                dr.VendorNumber = vendorID.ToString()
                dr.SubTeam_No = SubTeamDropDown.SelectedValue
                dr.Price = Trim(priceTxBx.Text)
                dr.PriceMultiple = Trim(priceMultiTxBx.Text)
                dr.CaseCost = Trim(caseCostTxBx.Text)
                dr.CaseSize = Trim(caseSizeTxBx.Text)
                dr.Warehouse = Trim(vendorOrderTxBx.Text.ToUpper)
                dr.Brand_ID = brandID.ToString
                If CheckBox2.Checked Then
                    dr.BrandName = Trim(brandTxBx.Text)
                Else
                    dr.BrandName = Trim(txtBrand.Text)
                End If

                dr.Category_ID = CatDropDown.SelectedValue
                dr.ClassID = ClassDropDown.SelectedValue
                dr.TaxClass_ID = taxClassID.Value
                Try
                    If CRVDropDown.SelectedItem.Text = "-- Select --" Then
                        dr.CRV = Nothing
                    Else
                        dr.CRV = CRVDropDown.SelectedValue
                    End If
                Catch ex As Exception
                    Debug.Print(ex.ToString)
                End Try
                'dr.AgeCode = CInt(IIf(SubTeamDropDown.SelectedValue = 330 Or SubTeamDropDown.SelectedValue = 340, 1, 0))
                dr.Insert_Date = Date.Now
                dr.Ready_To_Apply = ready2Apply
                dr.CostedByWeight = CBool(IIf(CostByWeightCK.Checked = True, True, False))
                dr.Organic = CBool(IIf(OrganicCK.Checked = True, True, False))
                dr.RetailUnits = RetailUnitsDropDown.SelectedValue
                dr.ManufacturingUnits = ManufUnitsDropDown.SelectedValue

                If Not _simpleItemDescriptionView Then
                    dr.ShelfLabelType = ShelfLabelDropDown.SelectedValue
                    dr.DistributionSubteam = DistSubDropDown.SelectedValue
                    dr.DistributionUnits = DistUnitsDropDown.SelectedValue
                    dr.HasIngredients = CBool(IIf(IngredientsCK.Checked = True, True, False))
                    dr.LineDiscount = CBool(IIf(LineDiscountCK.Checked = True, True, False))
                    dr.EmpDiscount = CBool(IIf(EmpDiscountCK.Checked = True, True, False))
                    ' Fixed Weight in scale section
                    dr.FoodStamp = CBool(IIf(FoodStampsCK.Checked = True, True, False))
                    dr.KeepFrozen = CBool(IIf(KeepFrozenCK.Checked = True, True, False))
                    dr.QuantityProhibit = CBool(IIf(QuantityProhibitCK.Checked = True, True, False))
                    dr.QuantityRequired = CBool(IIf(QuantityReqCK.Checked = True, True, False))
                    dr.Refrigerated = CBool(IIf(RefrigeratedCK.Checked = True, True, False))
                    dr.Restricted = CBool(IIf(RestrictedCK.Checked = True, True, False))
                    dr.PriceRequired = CBool(IIf(PriceRequiredCK.Checked = True, True, False))
                    dr.VisualVerify = CBool(IIf(VisualVerifyCK.Checked = True, True, False))
                    dr.DiscountTerms = Trim(DiscTermTxBx.Text)
                    dr.GoLocal = IIf(GoLocalDropDown.SelectedValue = "0", Nothing, GoLocalDropDown.SelectedItem.Text)
                    dr.Misc = Trim(MiscTxBx.Text)
                    dr.CommodityCode = IIf(CommodityDropDown.SelectedValue = "0", Nothing, CommodityDropDown.SelectedItem.Text)
                    dr.NotAvailable = CBool(IIf(NotAvailableCK.Checked = True, True, False))
                    dr.NotAvailableNote = Trim(NotAvailTxBx.Text)
                    dr.MixMatch = Trim(TextBox_MixMatch.Text)
                    dr.ESRSCKI = Trim(ESRSCKTxBx.Text) ' == Cooking instructions
                    dr.POSLinkCode = Trim(POSLinkCodeTxBx.Text)
                    dr.POSTare = IIf(Trim(POSTareTxBx.Text) = "", 0, CInt(Trim(POSTareTxBx.Text)))
                    dr.AgeCode = Textbox_AgeCode.Text
                    dr.Venue = Trim(VenueTxBx.Text)
                End If

                Try
                    If CountryDropDown.SelectedItem.Text = "-- Select --" Then
                        dr.CountryOfProc = Nothing
                    Else
                        dr.CountryOfProc = CountryDropDown.SelectedValue
                    End If
                Catch ex As Exception
                    Debug.Print(ex.ToString)
                End Try
                Try
                    If OriginDropDown.SelectedItem.Text = "-- Select --" Then
                        dr.Origin = Nothing
                    Else
                        dr.Origin = OriginDropDown.SelectedValue
                    End If
                Catch ex As Exception
                    Debug.Print(ex.ToString)
                End Try
                Try
                    If DateTime.TryParse(WebDateChooser5.Text, returnDate) Then
                        dr.CostStart = returnDate.ToShortDateString
                    Else
                        dr.CostStart = ""
                    End If
                Catch ex As Exception
                    Debug.Print(ex.ToString)
                End Try

                Try
                    If DateTime.TryParse(WebDateChooser6.Text, returnDate) Then
                        dr.CostEnd = returnDate.ToShortDateString
                    Else
                        dr.CostEnd = ""

                    End If
                Catch ex As Exception
                    Debug.Print(ex.ToString)
                End Try

                dr.Allowances = Trim(AllowanceTxBx.Text)
                dr.Discounts = Trim(DiscountsTxBx.Text)
                dr.VendorFreightUnit = FreightUnitDropDown.SelectedValue
                Try
                    If DateTime.TryParse(WebDateChooser1.Text, returnDate) Then
                        dr.AllowanceStartDate = returnDate.ToShortDateString
                    Else
                        dr.AllowanceStartDate = ""
                    End If
                Catch ex As Exception
                    Debug.Print(ex.ToString)
                End Try

                Try
                    If DateTime.TryParse(WebDateChooser2.Text, returnDate) Then
                        dr.AllowanceEndDate = returnDate.ToShortDateString
                    Else
                        dr.AllowanceEndDate = ""
                    End If
                Catch ex As Exception
                    Debug.Print(ex.ToString)
                End Try

                Try
                    If DateTime.TryParse(WebDateChooser3.Text, returnDate) Then
                        dr.DiscountStartDate = returnDate.ToShortDateString
                    Else
                        dr.DiscountStartDate = ""
                    End If
                Catch ex As Exception
                    Debug.Print(ex.ToString)
                End Try

                Try
                    If DateTime.TryParse(WebDateChooser4.Text, returnDate) Then
                        dr.DiscountEndDate = returnDate.ToShortDateString
                    Else
                        dr.DiscountEndDate = ""
                    End If
                Catch ex As Exception
                    Debug.Print(ex.ToString)
                End Try

                dr.Promotional = CBool(IIf(PromotionalCK.Checked = True, True, False))
                dr.UnitFreight = Trim(FreightTxBx.Text)
                dr.VendorUnits = VendorUnitsDropDown.SelectedValue
                dr.CostUnit = CostUnitDropDown.SelectedValue
                dr.MSRPPrice = IIf(MSRPPriceTxBx.Text.Length = 0, 0, Trim(MSRPPriceTxBx.Text))
                dr.MSRPMultiple = Trim(priceMultiTxBx.Text)
                dr.RequestedBy = Session("UserName")

                dt.AddItemRequestRow(dr)

                'Insert record into ItemRequest Table
                ItemRequest_ID = da.Update(dr)
                'ItemRequest_ID = da.InsertItemRequest(dr.Identifier, dr.ItemStatus_ID, dr.ItemType_ID, dr.ItemTemplate, _
                'dr.User_ID, dr.User_Store, dr.UserAccessLevel_ID, dr.VendorRequest_ID, dr.Item_Description, _
                'dr.POS_Description, dr.ItemUnit, dr.ItemSize, dr.PackSize, dr.VendorNumber, dr.SubTeam_No, _
                'dr.Price, dr.PriceMultiple, dr.CaseCost, dr.CaseSize, dr.Warehouse, dr.Brand_ID, _
                'dr.BrandName, dr.Category_ID, dr.Insert_Date, dr.ClassID, dr.TaxClass_ID, dr.CRV, _
                'dr.AgeCode, dr.FoodStamp, dr.Ready_To_Apply, dr.HasIngredients, dr.Promotional, _
                'dr.CostEnd, dr.CostStart, dr.CostUnit, dr.VendorFreightUnit, dr.MSRPPrice, _
                'dr.MSRPMultiple, dr.POSLinkCode, dr.LineDiscount, dr.CommodityCode, dr.DiscountTerms, _
                'dr.GoLocal, dr.Misc, dr.ESRSCKI, dr.CostedByWeight, dr.CountryOfProc, dr.DistributionSubteam, _
                'dr.DistributionUnits, dr.IdentifierType, dr.KeepFrozen, dr.ShelfLabelType, _
                'dr.ManufacturingUnits, dr.Organic, dr.Origin, dr.POSTare, dr.PriceRequired, _
                'dr.QuantityProhibit, dr.QuantityRequired, dr.Refrigerated, dr.Restricted, _
                'dr.RetailUnits, dr.NotAvailable, dr.NotAvailableNote, dr.UnitFreight, _
                'dr.Allowances, dr.Discounts, dr.AllowanceStartDate, dr.AllowanceEndDate, _
                'dr.DiscountStartDate, dr.DiscountEndDate, dr.MixMatch, dr.Venue, _
                'dr.VisualVerify, dr.VendorUnits, dr.RequestedBy, dr.EmpDiscount)
                da = Nothing
                dt = Nothing
                ' ****************************************
                ' *********** Enter Scale Date ***********
                If dr.ItemType_ID = 2 Then
                    Dim da1 As New ItemScaleRequestTableAdapters.ItemScaleRequestTableAdapter
                    Dim dt1 As New ItemScaleRequest.ItemScaleRequestDataTable
                    Dim dr1 As ItemScaleRequest.ItemScaleRequestRow = dt1.NewItemScaleRequestRow
                    dr1.ItemRequest_ID = ItemRequest_ID
                    dr1.ScaleDescription1 = Trim(ScaleDesc1.Text.ToString)
                    dr1.ScaleDescription2 = Trim(ScaleDesc2.Text.ToString)
                    dr1.ScaleDescription3 = Trim(ScaleDesc3.Text.ToString)
                    dr1.ScaleDescription4 = Trim(ScaleDesc4.Text.ToString)
                    dr1.ShelfLife = CInt(Trim(IIf(ShelfLife.Text = "", 0, ShelfLife.Text)))
                    dr1.ScaleUomUnit_ID = ScaleUOMDropDown.SelectedValue
                    dr1.ScaleRandomWeightType_ID = RandomWeightDropDown.SelectedValue
                    dr1.Scale_Tare_ID = TareDropDown.SelectedValue
                    dr1.Ingredients = Trim(Ingredients.Text.ToString)
                    dr1.ForceTare = CBool(IIf(ForceTareCK.Checked = True, True, False))
                    dr1.LabelStyle = LabelStyleDropDown.SelectedValue
                    dr1.ExtraTextLabelType = ScaleLabelTypeDropDown.SelectedValue
                    dr1.ExtraText = 0

                    If Not _simpleItemDescriptionView Then
                        If FixedWeightTxBx.Text = "" Or FixedWeightTxBx.Enabled = False Then
                            dr1.FixedWeight = 0
                        Else
                            dr1.FixedWeight = Trim(FixedWeightTxBx.Text.ToString)
                        End If
                    End If

                    If ByCountTxBx.Text = "" Or ByCountTxBx.Enabled = False Then
                        dr1.ByCount = 0
                    Else
                        dr1.ByCount = ByCountTxBx.Text
                    End If
                    da1.Insert(ItemRequest_ID, dr1.ScaleDescription1, dr1.ScaleDescription2, _
                    dr1.ScaleDescription3, dr1.ScaleDescription4, dr1.ShelfLife, _
                dr1.ScaleUomUnit_ID, dr1.ScaleRandomWeightType_ID, _
                     dr1.Scale_Tare_ID, dr1.Ingredients, dr1.FixedWeight, dr1.ByCount, _
                     dr1.ForceTare, dr1.LabelStyle, dr1.ExtraTextLabelType, dr1.ExtraText)
                    IIf(Trim(ByCountTxBx.Text) = "", 0, CInt(ByCountTxBx.Text))
                    dt1.AddItemScaleRequestRow(dr1)
                    da1.Update(dr1)
                End If
                ' ****************************************
                FinishLabel.Text = "Item Successfully Requested!"
                FinishLabel.ForeColor = Drawing.Color.Green
                ' *********************************************
                'TODO: Send E-mail Notes after Item requested
                Try
                    ' ***************** Sent the Notification E-Mail ************
                    'TODO: Send E-mail Notes after Item requested
                    If Application.Get("ItemRequestEmail") = "1" Then
                        Dim em As New EmailNotifications
                        em.EmailType = "ItemRequest"
                        em.Identifier = dr.Identifier
                        em.ItemDescription = dr.Item_Description
                        em.Store_Name = Session("Store_Name")
                        em.Store_No = Session("Store_No")
                        em.SubTeam_No = dr.SubTeam_No
                        em.SubTeam_Name = Session("SubTeam_Name")
                        em.New_Price = dr.Price
                        em.New_Cost = dr.CaseCost
                        em.User = Session("UserName")
                        em.User_ID = Session("UserID")
                        em.SentEmail()
                    End If
                    ' ***********************************************************
                Catch ex As Exception
                    Debug.WriteLine(ex.Message)
                    'Error_Log.throwException(ex.Message, ex)
                End Try
                ' *********************************************
            Catch ex As Exception
                Debug.WriteLine("Did not WORK!" & ex.Message)
                FinishLabel.Text = "Item Not Submitted - Error encountered " & " (" & ex.Message & ") "
                Error_Log.throwException(ex.Message, ex)
            End Try
        End If
        ' **************************************************
    End Sub

    Protected Sub SubTeamDropDown_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SubTeamDropDown.SelectedIndexChanged
        Session("SubTeam_No") = SubTeamDropDown.SelectedValue
        Session("SubTeam_Name") = SubTeamDropDown.SelectedItem.ToString
        CatDropDown.Items.Clear()
        ClassDropDown.Items.Clear()
        CatDropDown.Items.Insert(0, "--Select--")
        CatDropDown.Items.Item(0).Value = 0
        ClassDropDown.Items.Insert(0, "--Select--")
        ClassDropDown.Items.Item(0).Value = 0


        If Not SubTeamDropDown.SelectedValue = 0 Then
            CatDropDown.Enabled = True
            CatDropDown.Items.Clear()
            CatDropDown.DataSourceID = "DataSource_Category"
            CatDropDown.Items.Insert(0, New ListItem("----Select----", 0))
            CatDropDown.DataBind()
        End If

        If Not SubTeamDropDown.SelectedValue = 0 Then
            ClassDropDown.Enabled = True
            ClassDropDown.Items.Clear()
            ClassDropDown.DataSourceID = "DataSource_NatClass"
            ClassDropDown.Items.Insert(0, New ListItem("----Select----", 0))
            ClassDropDown.DataBind()
        End If

        If Not _simpleItemDescriptionView Then
            DistSubDropDown.SelectedValue = SubTeamDropDown.SelectedValue
        End If
    End Sub

    Protected Sub upcTxBx_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles upcTxBx.TextChanged
        Session("UPC") = upcTxBx.Text.ToString
    End Sub

    Protected Sub caseCostTxBx_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles caseCostTxBx.TextChanged
        Session("Cost") = caseCostTxBx.Text.ToString
    End Sub

    Protected Sub ImageButton3_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton3.Click
        If Not caseCostTxBx.Text = "" And Not MarginTxBx.Text = "" Then
            Dim cost As Single
            MarginErrorLbl.Text = Nothing
            cost = (CSng(caseCostTxBx.Text) / CInt(caseSizeTxBx.Text))
            If cost > 0.0 Then
                priceTxBx.Text = String.Format("{0:N}", Margin.GetPrice(cost, CSng(MarginTxBx.Text), CInt(priceMultiTxBx.Text)))
            Else
                MarginErrorLbl.Text = "Price/Cost has an invalid value"
            End If
        Else
            MarginErrorLbl.Text = "Margin has an invalid value"
        End If
    End Sub

    Protected Sub caseSizeTxBx_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles caseSizeTxBx.TextChanged
        Session("Size") = caseSizeTxBx.Text.ToString
    End Sub


    Protected Sub ImageButton5_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton5.Click
        Dim price As Single
        Dim cost As Single
        MarginErrorLbl.Text = Nothing
        If Not caseSizeTxBx.Text = "" Then
            price = (CSng(priceTxBx.Text) / CInt(priceMultiTxBx.Text))
            cost = (CSng(caseCostTxBx.Text) / CInt(caseSizeTxBx.Text))
            If price > 0.0 And cost > 0.0 Then
                MarginTxBx.Text = Margin.GetMargin(price, cost)
            Else
                MarginErrorLbl.Text = "Price/Cost has Invalid Value"
            End If
        Else
            MarginErrorLbl.Text = "Vendor Pack has an invalid value"
        End If
    End Sub

    Protected Sub CheckBox2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked = True Then
            brandTxBx.Visible = True
            txtBrand.Text = String.Empty
            txtBrand.Enabled = False
        Else
            brandTxBx.Text = ""
            brandTxBx.Visible = False
            txtBrand.Enabled = True
        End If
    End Sub

    Protected Sub CheckBox3_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CkBx3.CheckedChanged
        If CkBx3.Checked = True Then
            NewVendorDropDown.Visible = True
            txtVendor.Text = String.Empty
            txtVendor.Enabled = False
        Else
            NewVendorDropDown.SelectedIndex = -1
            NewVendorDropDown.Visible = False
            txtVendor.Enabled = True
        End If
    End Sub

    Protected Sub ScaleUOMDropDown_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ScaleUOMDropDown.SelectedIndexChanged
        If ScaleUOMDropDown.SelectedValue = 39 Then
            ByCountTxBx.Enabled = True
            ByCountTxBx.Focus()
            FixedWeightTxBx.Enabled = False
        ElseIf ScaleUOMDropDown.SelectedValue = 40 Then
            ByCountTxBx.Enabled = False
            FixedWeightTxBx.Enabled = True
            FixedWeightTxBx.Focus()
        Else
            ByCountTxBx.Enabled = False
            FixedWeightTxBx.Enabled = False
        End If
    End Sub

    Protected Sub Wizard1_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Wizard1.Init
        If Session("Slim_ScaleInfo") = False Then
            Wizard1.WizardSteps.Remove(Wizard1.WizardSteps(4))
        End If
    End Sub

    Protected Sub WebDateChooser5_ValueChanged(ByVal sender As Object, ByVal e As Infragistics.WebUI.WebSchedule.WebDateChooser.WebDateChooserEventArgs) Handles WebDateChooser5.ValueChanged
        If DateValidator(WebDateChooser5.Text) Then
            Label_WebDateChooser5.Visible = False
        Else
            Label_WebDateChooser5.Visible = True
        End If
    End Sub
    Protected Sub WebDateChooser6_ValueChanged(ByVal sender As Object, ByVal e As Infragistics.WebUI.WebSchedule.WebDateChooser.WebDateChooserEventArgs) Handles WebDateChooser6.ValueChanged
        If DateValidator(WebDateChooser6.Text) Then
            Label_WebDateChooser6.Visible = False
        Else
            Label_WebDateChooser6.Visible = True
        End If
    End Sub
    Protected Sub WebDateChooser1_ValueChanged(ByVal sender As Object, ByVal e As Infragistics.WebUI.WebSchedule.WebDateChooser.WebDateChooserEventArgs) Handles WebDateChooser1.ValueChanged
        If DateValidator(WebDateChooser1.Text) Then
            Label_WebDateChooser1.Visible = False
        Else
            Label_WebDateChooser1.Visible = True
        End If
    End Sub
    Protected Sub WebDateChooser2_ValueChanged(ByVal sender As Object, ByVal e As Infragistics.WebUI.WebSchedule.WebDateChooser.WebDateChooserEventArgs) Handles WebDateChooser2.ValueChanged
        If DateValidator(WebDateChooser2.Text) Then
            Label_WebDateChooser2.Visible = False
        Else
            Label_WebDateChooser2.Visible = True
        End If
    End Sub
    Protected Sub WebDateChooser3_ValueChanged(ByVal sender As Object, ByVal e As Infragistics.WebUI.WebSchedule.WebDateChooser.WebDateChooserEventArgs) Handles WebDateChooser3.ValueChanged
        If DateValidator(WebDateChooser3.Text) Then
            Label_WebDateChooser3.Visible = False
        Else
            Label_WebDateChooser3.Visible = True
        End If
    End Sub
    Protected Sub WebDateChooser4_ValueChanged(ByVal sender As Object, ByVal e As Infragistics.WebUI.WebSchedule.WebDateChooser.WebDateChooserEventArgs) Handles WebDateChooser4.ValueChanged
        If DateValidator(WebDateChooser4.Text) Then
            Label_WebDateChooser4.Visible = False
        Else
            Label_WebDateChooser4.Visible = True
        End If
    End Sub

    Private Function DateValidator(ByVal webDateText As String)
        Dim dt As Date

        If DateTime.TryParse(webDateText, dt) Then
            If dt >= Today() Then
                Return True
            Else
                _dateErrorString = "An Invalid Date has been entered."
                Return False
            End If
        Else
            If webDateText.Length > 0 Then
                _dateErrorString = "An Invalid Date has been entered."
                Return False
            Else
                Return True
            End If
        End If
    End Function

    Protected Sub PromotionalCK_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles PromotionalCK.CheckedChanged
        'Show and hide the promo controls
        If PromotionalCK.Checked = True Then
            PromoHider(True)
        Else
            PromoHider(False)
        End If

    End Sub

    Protected Sub PromoHider(ByVal isHidden As Boolean)
        Label_Allow.Visible = isHidden
        AllowanceTxBx.Visible = isHidden
        Label_Disc.Visible = isHidden
        DiscountsTxBx.Visible = isHidden

        WebDateChooser1.Visible = isHidden
        WebDateChooser2.Visible = isHidden
        WebDateChooser3.Visible = isHidden
        WebDateChooser4.Visible = isHidden

        Label81.Visible = isHidden
        Label82.Visible = isHidden
        Label83.Visible = isHidden
        Label84.Visible = isHidden
        Label85.Visible = isHidden
        Label86.Visible = isHidden

    End Sub


    Protected Sub NotAvailableCK_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles NotAvailableCK.CheckedChanged
        'Show and hide the promo controls
        If NotAvailableCK.Checked = True Then
            Label_NotAvail.Visible = True
            NotAvailTxBx.Visible = True

        Else
            Label_NotAvail.Visible = False
            NotAvailTxBx.Visible = False
        End If
  
    End Sub

    Protected Sub CostUnitDropDown_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CostUnitDropDown.SelectedIndexChanged
        Dim unitValue As String
        'autopopulate the cost units
        unitValue = CostUnitDropDown.SelectedValue
        VendorUnitsDropDown.SelectedValue = unitValue
        FreightUnitDropDown.SelectedValue = unitValue

    End Sub

    'TODO: EACH REGION SHOULD ADD THEIR OWN CASE STATEMENT HERE ACCORDING TO HOW THEY HAVE POSITIONED THEIR NATL CLASS ID IN THE SUBTEAM CLASS
    'DESCRIPTION
    Protected Sub CatDropDown_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CatDropDown.SelectedIndexChanged
        Try
            Dim _parsedCatID As Integer
            Dim _split As String()
            Dim _selectedCat As String

            Dim instance As InstanceDataBO = InstanceDataDAO.GetInstanceData

            Select Case instance.RegionCode.ToString
                Case "NC", "PN", "MW"
                    _selectedCat = CatDropDown.SelectedItem.Text
                    _parsedCatID = CInt(Right(_selectedCat, 4))
                Case "SW", "RM"
                    _selectedCat = CatDropDown.SelectedItem.Text
                    _split = _selectedCat.Split(" ")
                    _parsedCatID = CInt(Trim(_split.GetValue(0)))
            End Select

            ClassDropDown.SelectedValue = _parsedCatID

        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try
    End Sub

    Protected Sub RetailUnitsDropDown_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RetailUnitsDropDown.SelectedIndexChanged
        Dim unitValue As String
        'autopopulate the cost units
        unitValue = RetailUnitsDropDown.SelectedValue
        ManufUnitsDropDown.SelectedValue = unitValue

        If Not _simpleItemDescriptionView Then
            DistUnitsDropDown.SelectedValue = unitValue
        End If
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Protected Sub CostByWeightCK_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CostByWeightCK.CheckedChanged
        Dim ddcol As New ArrayList
        ddcol.Add(RetailUnitsDropDown)
        ddcol.Add(ManufUnitsDropDown)
        ddcol.Add(DistUnitsDropDown)
        ddcol.Add(UOMDropDown)
        ddcol.Add(CostUnitDropDown)
        ddcol.Add(VendorUnitsDropDown)
        ddcol.Add(FreightUnitDropDown)
        If CostByWeightCK.Checked = True Then
            SqlDataSource10.SelectParameters.Clear()
            SqlDataSource10.SelectParameters.Add("WeightUnits", True)
            'SqlDataSource10.SelectParameters.Add("AllUnits", False)
            For Each ddl As DropDownList In ddcol
                ddl.Items.Clear()
                ddl.Items.Insert(0, "--Select--")
                ddl.Items.Item(0).Value = 0
                ddl.DataBind()
            Next
        Else
            SqlDataSource10.SelectParameters.Clear()
            SqlDataSource10.SelectParameters.Add("WeightUnits", False)
            'SqlDataSource10.SelectParameters.Add("AllUnits", True)
            For Each ddl As DropDownList In ddcol
                ddl.Items.Clear()
                ddl.Items.Insert(0, "--Select--")
                ddl.Items.Item(0).Value = 0
                ddl.DataBind()
            Next
        End If
    End Sub

    Protected Sub NewVendorDropDown_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles NewVendorDropDown.SelectedIndexChanged



    End Sub


    Protected Sub Wizard1_NextButtonClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.WizardNavigationEventArgs) Handles Wizard1.NextButtonClick
        Dim idExists As Boolean
        Dim idRequestedExists As Boolean
        Dim da As New NewItemRequestTableAdapters.ItemRequestTableAdapter

        If e.CurrentStepIndex = 0 Then

            ' ***** Check for existing UPC in ItemCatalog and ItemRequest table ****
            idExists = InputValidation.IdentifierCheck(upcTxBx.Text.ToString)
            ' ***** Check for existing UPC in ItemRequest *******
            idRequestedExists = da.IdentifierRequestedExists(upcTxBx.Text.ToString)

            If idExists Or idRequestedExists Then
                e.Cancel = True
            End If

        End If
    End Sub
End Class
