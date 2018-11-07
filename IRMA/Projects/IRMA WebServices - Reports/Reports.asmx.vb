Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class Reports
    Inherits System.Web.Services.WebService

    ' function to get a list of items
    <WebMethod()> _
    Public Function GetReceivingCheckList(ByVal OrderHeader_ID As Integer, ByVal optIdentifier As Integer) As DataSet
        Try

            ' Handling reporting services refresh option which takes input parameter as 0.
            Dim val As Integer = Integer.Parse(OrderHeader_ID)
            If (val < 1) Then
                val = 1
            End If

            ' Executing and fetching the stored procedure output into Dataset.
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam1 As New DBParam
            Dim currentParam2 As New DBParam
            Dim lLineNo As Integer

            Dim dataRow As DataRow  ' To read all rows returned by the DataTable.
            ' Dataset to fetch the output of the stored procedure.
            Dim dsReceiveCheckList As DataSet = New DataSet("NewDataSet")

            ' Passing OrderHeader_ID parameter for stored procedure.
            currentParam1 = New DBParam
            currentParam1.Name = "OrderHeader_ID"
            currentParam1.Value = val
            currentParam1.Type = DBParamType.Int
            paramList.Add(currentParam1)

            ' Passing optIdentifier parameter for stored procedure.
            currentParam2 = New DBParam
            currentParam2.Name = "Item_ID"
            currentParam2.Value = optIdentifier
            currentParam2.Type = DBParamType.Bit
            paramList.Add(currentParam2)

            ' Executing the ReceivingCheckList Stored Procedure.
            dsReceiveCheckList = factory.GetStoredProcedureDataSet("dbo.ReceivingCheckList", paramList)

            ' Creating a DataTable named ReceivingCheckList to send as an output to report through Dataset.
            Dim dtReceiveCheckListDetails As DataTable
            dtReceiveCheckListDetails = New DataTable("ReceivingCheckList")

            ' Creating a Dattable schema for the report output DataTable with all columns.
            Dim LineItem As DataColumn = New DataColumn("LineItem")
            LineItem.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtReceiveCheckListDetails.Columns.Add(LineItem)

            Dim Identifier As DataColumn = New DataColumn("Identifier")
            Identifier.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtReceiveCheckListDetails.Columns.Add(Identifier)

            Dim Item_Description As DataColumn = New DataColumn("Item_Description")
            Item_Description.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtReceiveCheckListDetails.Columns.Add(Item_Description)

            Dim Package_Desc As DataColumn = New DataColumn("Package_Desc")
            Package_Desc.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtReceiveCheckListDetails.Columns.Add(Package_Desc)

            Dim QuantityOrdered As DataColumn = New DataColumn("QuantityOrdered")
            QuantityOrdered.DataType = System.Type.GetType("System.Decimal")
            ' Setting the datatype for the column
            dtReceiveCheckListDetails.Columns.Add(QuantityOrdered)

            ' Creating a table schema Columns
            Dim QuantityUnit As DataColumn = New DataColumn("QuantityUnit")
            QuantityUnit.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtReceiveCheckListDetails.Columns.Add(QuantityUnit)

            ' Creating a table schema Columns
            Dim Cost As DataColumn = New DataColumn("Cost")
            Cost.DataType = System.Type.GetType("System.String")
            dtReceiveCheckListDetails.Columns.Add(Cost)

            ' Creating a table schema Columns
            Dim UnitCost As DataColumn = New DataColumn("UnitCost")
            UnitCost.DataType = System.Type.GetType("System.String")
            dtReceiveCheckListDetails.Columns.Add(UnitCost)

            ' Read and write the dataset output to a new DataTable.
            Dim drReceiveCheckDetails As DataRow

            ' Dataset to send as an output from the webMethod, as an input to the Report.
            Dim dsCheckListDetailsOut As DataSet = New DataSet("ReceivingCheckList")
            If dsReceiveCheckList.Tables(0).Rows.Count > 0 Then
                For Each dataRow In dsReceiveCheckList.Tables(0).Rows
                    drReceiveCheckDetails = dtReceiveCheckListDetails.NewRow()
                    ' Filling the Data Row with Data.
                    lLineNo = lLineNo + 1
                    drReceiveCheckDetails.Item("LineItem") = lLineNo
                    drReceiveCheckDetails.Item("Identifier") = dataRow("Identifier")
                    drReceiveCheckDetails.Item("Item_Description") = dataRow("Item_Description")
                    drReceiveCheckDetails.Item("Package_Desc") = dataRow("Package_Desc1") & "/" & dataRow("Package_Desc2") & " " & dataRow("Package_Unit")
                    drReceiveCheckDetails.Item("QuantityOrdered") = CDbl(dataRow("QuantityOrdered")) + CDbl(IIf(dataRow("DiscountType") = 3, dataRow("QuantityDiscount"), 0))
                    drReceiveCheckDetails.Item("QuantityUnit") = dataRow("Unit_Name")
                    drReceiveCheckDetails.Item("Cost") = dataRow("Cost")
                    drReceiveCheckDetails.Item("UnitCost") = dataRow("UnitCost")
                    dtReceiveCheckListDetails.Rows.Add(drReceiveCheckDetails)
                Next
            End If

            dsCheckListDetailsOut.Tables.Add(dtReceiveCheckListDetails)
            Return dsCheckListDetailsOut ' Passing dataset as output to the report.
        Catch e As Exception
            Throw
        End Try
    End Function

    ' function to get a list of items
    <WebMethod()> _
    Public Function GetPurchaseOrderHeader(ByVal OrderHeader_ID As Integer) As DataSet
        Try

            ' Handling reporting services refresh option which takes input parameter as 0.
            Dim val As Integer = Integer.Parse(OrderHeader_ID)
            If (val < 1) Then
                val = 1
            End If

            ' To execute the stored Procedure.
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As New DBParam

            ' Concatinating Vendor complete Information into one column.
            Dim strVendorInformation As New System.Text.StringBuilder

            ' Concatinating Purchaser complete information into one column.
            Dim strPurchaserInformation As New System.Text.StringBuilder

            ' Concatinating Receiver complete information into one column.
            Dim strReceiverInformation As New System.Text.StringBuilder

            ' Dataset to fetch the output of the StoredProcedure.
            Dim dsOrderHeader As DataSet = New DataSet("NewDataSet")

            ' Setup parameters for stored procedure.
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = val
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Executing GetPOHeader stored procedure.
            dsOrderHeader = factory.GetStoredProcedureDataSet("dbo.GetPOHeader", paramList)

            ' Creating a DataTable named PurchaseOrderHeader to send as an output to the report.
            Dim dtPurchaseOrderHeader As DataTable
            dtPurchaseOrderHeader = New DataTable("PurchaseOrderHeader")

            ' Creating a table schema for the output DataTable of the report.
            Dim PONumber As DataColumn = New DataColumn("PONumber")
            PONumber.DataType = System.Type.GetType("System.String")
            'setting the datatype for the column
            dtPurchaseOrderHeader.Columns.Add(PONumber)

            Dim OrderDate As DataColumn = New DataColumn("OrderDate")
            OrderDate.DataType = System.Type.GetType("System.String")
            dtPurchaseOrderHeader.Columns.Add(OrderDate)

            ' Concatinating Vendor Information based on data availability.
            Dim VendorInfo As DataColumn = New DataColumn("VendorInfo")
            VendorInfo.DataType = System.Type.GetType("System.String")
            dtPurchaseOrderHeader.Columns.Add(VendorInfo)

            Dim VendorVendor_Key As DataColumn = New DataColumn("VendorVendor_Key")
            VendorVendor_Key.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPurchaseOrderHeader.Columns.Add(VendorVendor_Key)

            Dim OrderHeaderDesc As DataColumn = New DataColumn("OrderHeaderDesc")
            OrderHeaderDesc.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPurchaseOrderHeader.Columns.Add(OrderHeaderDesc)

            Dim QuantityDiscount As DataColumn = New DataColumn("QuantityDiscount")
            QuantityDiscount.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPurchaseOrderHeader.Columns.Add(QuantityDiscount)

            Dim DiscountType As DataColumn = New DataColumn("DiscountType")
            DiscountType.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPurchaseOrderHeader.Columns.Add(DiscountType)

            Dim UserName As DataColumn = New DataColumn("UserName")
            UserName.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPurchaseOrderHeader.Columns.Add(UserName)

            Dim SubTeam As DataColumn = New DataColumn("SubTeam")
            SubTeam.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPurchaseOrderHeader.Columns.Add(SubTeam)

            Dim Return_Order As DataColumn = New DataColumn("Return_Order")
            Return_Order.DataType = System.Type.GetType("System.Boolean")
            ' Setting the datatype for the column
            dtPurchaseOrderHeader.Columns.Add(Return_Order)

            Dim Expected_Date As DataColumn = New DataColumn("Expected_Date")
            Expected_Date.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPurchaseOrderHeader.Columns.Add(Expected_Date)

            Dim ReceiverInfo As DataColumn = New DataColumn("ReceiverInfo")
            ReceiverInfo.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPurchaseOrderHeader.Columns.Add(ReceiverInfo)

            ' Creating a table schema Column
            Dim PurchaseInfo As DataColumn = New DataColumn("PurchaseInfo")
            PurchaseInfo.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPurchaseOrderHeader.Columns.Add(PurchaseInfo)

            Dim OrderType_ID As DataColumn = New DataColumn("OrderType_ID")
            'declaring a column named Name
            OrderType_ID.DataType = System.Type.GetType("System.String")
            'setting the datatype for the column
            dtPurchaseOrderHeader.Columns.Add(OrderType_ID)

            Dim ProductType_ID As DataColumn = New DataColumn("ProductType_ID")
            ProductType_ID.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPurchaseOrderHeader.Columns.Add(ProductType_ID)

            Dim Title As DataColumn = New DataColumn("Title")
            Title.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPurchaseOrderHeader.Columns.Add(Title)

            Dim ShowDiscount As DataColumn = New DataColumn("ShowDiscount")
            ShowDiscount.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPurchaseOrderHeader.Columns.Add(ShowDiscount)

            ' Filling the data as a row.
            Dim dtRowHeader As DataRow

            ' Dataset to send as an output to the Report.
            Dim dsOrderHeaderDetails As DataSet = New DataSet("PurchaseOrderHeader")

            If dsOrderHeader.Tables(0).Rows.Count > 0 Then

                dtRowHeader = dtPurchaseOrderHeader.NewRow()

                ' Filling the Data Row with Data.
                dtRowHeader.Item("PONumber") = val
                dtRowHeader.Item("OrderDate") = FormatDateTime(dsOrderHeader.Tables(0).Rows(0)("OrderDate"), DateFormat.ShortDate)

                ' Concatinating Vendor information based on data availability.
                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("CompanyName")) Then
                    If Len(Trim(dsOrderHeader.Tables(0).Rows(0)("CompanyName"))) > 0 Then
                        strVendorInformation.Append(Trim(dsOrderHeader.Tables(0).Rows(0)("CompanyName")) + Chr(13))
                    End If
                End If

                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("Address_Line_1")) Then
                    If Len(Trim(dsOrderHeader.Tables(0).Rows(0)("Address_Line_1"))) > 0 Then
                        strVendorInformation.Append(Trim(dsOrderHeader.Tables(0).Rows(0)("Address_Line_1")) + Chr(13))
                    End If
                End If

                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("Address_Line_2")) Then
                    If Len(Trim(dsOrderHeader.Tables(0).Rows(0)("Address_Line_2"))) > 0 Then
                        strVendorInformation.Append(Trim(dsOrderHeader.Tables(0).Rows(0)("Address_Line_2")) + Chr(13))
                    End If
                End If
                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("City")) Then
                    If Len(Trim(dsOrderHeader.Tables(0).Rows(0)("City"))) > 0 Then
                        strVendorInformation.Append(Trim(dsOrderHeader.Tables(0).Rows(0)("City")) + ",")
                    End If
                End If
                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("State")) Then
                    If Len(Trim(dsOrderHeader.Tables(0).Rows(0)("State"))) > 0 Then
                        strVendorInformation.Append(Trim(dsOrderHeader.Tables(0).Rows(0)("State")) + " ")
                    End If
                End If

                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("Zip_Code")) Then
                    If Len(Trim(dsOrderHeader.Tables(0).Rows(0)("Zip_Code"))) > 0 Then
                        strVendorInformation.Append(Trim(dsOrderHeader.Tables(0).Rows(0)("Zip_Code")) + Chr(13))
                    End If
                End If

                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("Country")) Then
                    If Len(Trim(dsOrderHeader.Tables(0).Rows(0)("Country"))) > 0 Then
                        strVendorInformation.Append(Trim(dsOrderHeader.Tables(0).Rows(0)("Country")) + Chr(13))
                    End If
                End If

                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("Phone")) Then
                    If Len(Trim(dsOrderHeader.Tables(0).Rows(0)("Phone"))) > 0 Then
                        strVendorInformation.Append("Phone: " + Trim(dsOrderHeader.Tables(0).Rows(0)("Phone")) + Chr(13))
                    End If
                End If

                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("Fax")) Then
                    If Len(Trim(dsOrderHeader.Tables(0).Rows(0)("Fax"))) > 0 Then
                        strVendorInformation.Append("Fax: " + Trim(dsOrderHeader.Tables(0).Rows(0)("Fax")) + Chr(13))
                    End If
                End If

                If Not strVendorInformation.ToString.Length = 0 Then
                    dtRowHeader.Item("VendorInfo") = strVendorInformation.ToString
                End If

                dtRowHeader.Item("VendorVendor_Key") = dsOrderHeader.Tables(0).Rows(0)("Vendor_Key")

                ' Convatenating OrderHeaderDesc with creditorderDesc. creditorderDesc function will return the same.
                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("OriginalOrderHeader_ID").Value) Then
                    dtRowHeader.Item("OrderHeaderDesc") = CreditOrderDesc(OrderHeader_ID) & vbCrLf & "CM OF PO " & dsOrderHeader.Tables(0).Rows(0)("OriginalOrderHeader_ID").Value & vbCrLf & dsOrderHeader.Tables(0).Rows(0)("OrderHeaderDesc").Value
                Else
                    dtRowHeader.Item("OrderHeaderDesc") = CreditOrderDesc(OrderHeader_ID) & vbCrLf & dsOrderHeader.Tables(0).Rows(0)("OrderHeaderDesc")
                End If

                dtRowHeader.Item("QuantityDiscount") = dsOrderHeader.Tables(0).Rows(0)("QuantityDiscount")
                dtRowHeader.Item("DiscountType") = dsOrderHeader.Tables(0).Rows(0)("DiscountType")
                dtRowHeader.Item("UserName") = dsOrderHeader.Tables(0).Rows(0)("UserName")

                ' Pouplating SubTeam value based on TransferSubTeamName and TransferToSubTeamName.
                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("TransferToSubTeamName")) Or Trim(dsOrderHeader.Tables(0).Rows(0)("TransferToSubTeamName")) <> "" Then
                    dtRowHeader.Item("SubTeam") = Trim(dsOrderHeader.Tables(0).Rows(0)("TransferToSubTeamName"))
                Else
                    dtRowHeader.Item("SubTeam") = Trim(dsOrderHeader.Tables(0).Rows(0)("TransferSubTeamName"))
                End If

                dtRowHeader.Item("Return_Order") = dsOrderHeader.Tables(0).Rows(0)("Return_Order")

                If IsDBNull(dsOrderHeader.Tables(0).Rows(0)("Expected_Date")) Then
                    dtRowHeader.Item("Expected_Date") = ""
                Else
                    dtRowHeader.Item("Expected_Date") = FormatDateTime(dsOrderHeader.Tables(0).Rows(0)("Expected_Date"), DateFormat.ShortDate)
                End If

                ' Concatinating Receiver information based on data availability.
                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("RLCompanyName")) Then
                    If Len(Trim(dsOrderHeader.Tables(0).Rows(0)("RLCompanyName"))) > 0 Then
                        strReceiverInformation.Append(Trim(dsOrderHeader.Tables(0).Rows(0)("RLCompanyName")) + Chr(13))
                    End If
                End If

                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("RLAddress_Line_1")) Then
                    If Len(Trim(dsOrderHeader.Tables(0).Rows(0)("RLAddress_Line_1"))) > 0 Then
                        strReceiverInformation.Append(Trim(dsOrderHeader.Tables(0).Rows(0)("RLAddress_Line_1")) + Chr(13))
                    End If
                End If

                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("RLAddress_Line_2")) Then
                    If Len(Trim(dsOrderHeader.Tables(0).Rows(0)("RLAddress_Line_2"))) > 0 Then
                        strReceiverInformation.Append(Trim(dsOrderHeader.Tables(0).Rows(0)("RLAddress_Line_2")) + Chr(13))
                    End If
                End If

                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("RLCity")) Then
                    If Len(Trim(dsOrderHeader.Tables(0).Rows(0)("RLCity"))) > 0 Then
                        strReceiverInformation.Append(Trim(dsOrderHeader.Tables(0).Rows(0)("RLCity")) + ",")
                    End If
                End If

                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("RLState")) Then
                    If Len(Trim(dsOrderHeader.Tables(0).Rows(0)("RLState"))) > 0 Then
                        strReceiverInformation.Append(Trim(dsOrderHeader.Tables(0).Rows(0)("RLState")) + " ")
                    End If
                End If


                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("RLZip_Code")) Then
                    If Len(Trim(dsOrderHeader.Tables(0).Rows(0)("RLZip_Code"))) > 0 Then
                        strReceiverInformation.Append(Trim(dsOrderHeader.Tables(0).Rows(0)("RLZip_Code")) + Chr(13))
                    End If
                End If

                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("RLCountry")) Then
                    If Len(Trim(dsOrderHeader.Tables(0).Rows(0)("RLCountry"))) > 0 Then
                        strReceiverInformation.Append(Trim(dsOrderHeader.Tables(0).Rows(0)("RLCountry")) + Chr(13))
                    End If
                End If

                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("RLPhone")) Then
                    If Len(Trim(dsOrderHeader.Tables(0).Rows(0)("RLPhone"))) > 0 Then
                        strReceiverInformation.Append("Phone:" + Trim(dsOrderHeader.Tables(0).Rows(0)("RLPhone")) + Chr(13))
                    End If
                End If

                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("RLFax")) Then
                    If Len(Trim(dsOrderHeader.Tables(0).Rows(0)("RLFax"))) > 0 Then
                        strReceiverInformation.Append("Fax:" + Trim(dsOrderHeader.Tables(0).Rows(0)("RLFax")) + Chr(13))
                    End If
                End If

                If Not strReceiverInformation.ToString.Length = 0 Then
                    dtRowHeader.Item("ReceiverInfo") = strReceiverInformation.ToString
                End If

                ' Concatinating Purchaser information based on data availability.
                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("PLCompanyName")) Then
                    If Len(Trim(dsOrderHeader.Tables(0).Rows(0)("PLCompanyName"))) > 0 Then
                        strPurchaserInformation.Append(Trim(dsOrderHeader.Tables(0).Rows(0)("PLCompanyName")) + Chr(13))
                    End If
                End If

                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("PLAddress_Line_1")) Then
                    If Len(Trim(dsOrderHeader.Tables(0).Rows(0)("PLAddress_Line_1"))) > 0 Then
                        strPurchaserInformation.Append(Trim(dsOrderHeader.Tables(0).Rows(0)("PLAddress_Line_1")) + Chr(13))
                    End If
                End If

                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("PLAddress_Line_2")) Then
                    If Len(Trim(dsOrderHeader.Tables(0).Rows(0)("PLAddress_Line_2"))) > 0 Then
                        strPurchaserInformation.Append(Trim(dsOrderHeader.Tables(0).Rows(0)("PLAddress_Line_2")) + Chr(13))
                    End If
                End If

                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("PLCity")) Then
                    If Len(Trim(dsOrderHeader.Tables(0).Rows(0)("PLCity"))) > 0 Then
                        strPurchaserInformation.Append(Trim(dsOrderHeader.Tables(0).Rows(0)("PLCity")) + ",")
                    End If
                End If

                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("PLState")) Then
                    If Len(Trim(dsOrderHeader.Tables(0).Rows(0)("PLState"))) > 0 Then
                        strPurchaserInformation.Append(Trim(dsOrderHeader.Tables(0).Rows(0)("PLState")) + " ")
                    End If
                End If

                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("PLZip_Code")) Then
                    If Len(Trim(dsOrderHeader.Tables(0).Rows(0)("PLZip_Code"))) > 0 Then
                        strPurchaserInformation.Append(Trim(dsOrderHeader.Tables(0).Rows(0)("PLZip_Code")) + Chr(13))
                    End If
                End If

                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("PLCountry")) Then
                    If Len(Trim(dsOrderHeader.Tables(0).Rows(0)("PLCountry"))) > 0 Then
                        strPurchaserInformation.Append(Trim(dsOrderHeader.Tables(0).Rows(0)("PLCountry")) + Chr(13))
                    End If
                End If

                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("PLPhone")) Then
                    If Len(Trim(dsOrderHeader.Tables(0).Rows(0)("PLPhone"))) > 0 Then
                        strPurchaserInformation.Append("Phone: " + Trim(dsOrderHeader.Tables(0).Rows(0)("PLPhone")) + Chr(13))
                    End If
                End If


                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("PLFax")) Then
                    If Len(Trim(dsOrderHeader.Tables(0).Rows(0)("PLFax"))) > 0 Then
                        strPurchaserInformation.Append("Fax: " + Trim(dsOrderHeader.Tables(0).Rows(0)("PLFax")) + Chr(13))
                    End If
                End If

                ' Populating OrderInvoice Title Information into dataTable column.
                Dim strTitle As String
                strTitle = InvoiceTitle(dsOrderHeader.Tables(0).Rows(0)("OrderType_ID"), dsOrderHeader.Tables(0).Rows(0)("TransferToSubTeamName"), IIf(IsDBNull(dsOrderHeader.Tables(0).Rows(0)("TransferSubTeamName")), "", dsOrderHeader.Tables(0).Rows(0)("TransferSubTeamName")))
                If Not IsDBNull(strTitle) Then
                    dtRowHeader.Item("Title") = strTitle
                Else
                    dtRowHeader.Item("Title") = ""
                End If

                ' Populating OrderInvoice Show Discount Information into dataTable column.
                Dim strShowDiscount As String
                strShowDiscount = InvoiceShowDiscount(dsOrderHeader.Tables(0).Rows(0)("DiscountType"), dsOrderHeader.Tables(0).Rows(0)("QuantityDiscount"))
                If Not IsDBNull(strShowDiscount) Then
                    dtRowHeader.Item("ShowDiscount") = strShowDiscount
                Else
                    dtRowHeader.Item("ShowDiscount") = ""
                End If
                If Not strPurchaserInformation.ToString.Length = 0 Then
                    dtRowHeader.Item("PurchaseInfo") = strPurchaserInformation.ToString
                End If

                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("OrderType_ID")) Then
                    dtRowHeader.Item("OrderType_ID") = dsOrderHeader.Tables(0).Rows(0)("OrderType_ID")
                End If

                If Not IsDBNull(dsOrderHeader.Tables(0).Rows(0)("ProductType_ID")) Then
                    dtRowHeader.Item("ProductType_ID") = dsOrderHeader.Tables(0).Rows(0)("ProductType_ID")
                End If

                ' Adding DataRow to DataTable.
                dtPurchaseOrderHeader.Rows.Add(dtRowHeader)

                ' Adding DataTable to the Dataset.
                dsOrderHeaderDetails.Tables.Add(dtPurchaseOrderHeader)

                ' Returning the dataset as input to the Report..
                Return dsOrderHeaderDetails
            Else
                ' In case of no POOrder exists in datbase.
                dsOrderHeaderDetails.Tables.Add(dtPurchaseOrderHeader)
                Return dsOrderHeaderDetails

            End If
        Catch e As Exception
            Throw
        End Try

    End Function

    <WebMethod()> _
        Public Function GetOrderItemReceivedList(ByVal OrderHeader_ID As Integer, ByVal optIdentifier As Integer) As DataSet
        Try

            ' Handling reporting services refresh option which takes input parameter as 0.
            Dim val As Integer = Integer.Parse(OrderHeader_ID)
            If (val < 1) Then
                val = 1
            End If

            ' Executing and fetching the stored procedure output into Dataset.
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam1 As New DBParam
            Dim currentParam2 As New DBParam

            Dim dataRow As DataRow  ' To read all rows returned by the DataTable.
            ' Dataset to fetch the output of the stored procedure.
            Dim dsReceiveCheckList As DataSet = New DataSet("NewDataSet")

            ' Passing OrderHeader_ID parameter for stored procedure.
            currentParam1 = New DBParam
            currentParam1.Name = "OrderHeader_ID"
            currentParam1.Value = val
            currentParam1.Type = DBParamType.Int
            paramList.Add(currentParam1)

            ' Passing optIdentifier parameter for stored procedure.
            currentParam2 = New DBParam
            currentParam2.Name = "Item_ID"
            currentParam2.Value = optIdentifier
            currentParam2.Type = DBParamType.Bit
            paramList.Add(currentParam2)

            ' Executing the ReceivingCheckList Stored Procedure.
            dsReceiveCheckList = factory.GetStoredProcedureDataSet("dbo.GetOrderItemReceivedList", paramList)

            ' Creating a DataTable named ReceivingCheckList to send as an output to report through Dataset.
            Dim dtPurchaseOrderLine As DataTable
            dtPurchaseOrderLine = New DataTable("PurchaseOrderLine")

            Dim LineItem As DataColumn = New DataColumn("LineItem")
            LineItem.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPurchaseOrderLine.Columns.Add(LineItem)

            Dim Identifier As DataColumn = New DataColumn("Identifier")
            Identifier.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPurchaseOrderLine.Columns.Add(Identifier)

            Dim Item_Description As DataColumn = New DataColumn("Item_Description")
            Item_Description.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPurchaseOrderLine.Columns.Add(Item_Description)

            Dim Package_Desc As DataColumn = New DataColumn("Package_Desc")
            Package_Desc.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPurchaseOrderLine.Columns.Add(Package_Desc)

            Dim Package_Desc1 As DataColumn = New DataColumn("Package_Desc1")
            Package_Desc1.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPurchaseOrderLine.Columns.Add(Package_Desc1)

            Dim QuantityOrdered As DataColumn = New DataColumn("QuantityOrdered")
            QuantityOrdered.DataType = System.Type.GetType("System.Decimal")
            ' Setting the datatype for the column
            dtPurchaseOrderLine.Columns.Add(QuantityOrdered)

            ' Creating a table schema Columns
            Dim QuantityUnit As DataColumn = New DataColumn("QuantityUnit")
            QuantityUnit.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPurchaseOrderLine.Columns.Add(QuantityUnit)

            ' Creating a table schema Columns
            Dim CasePrice As DataColumn = New DataColumn("CasePrice")
            CasePrice.DataType = System.Type.GetType("System.String")
            dtPurchaseOrderLine.Columns.Add(CasePrice)

            ' Creating a table schema Columns
            Dim TotalPrice As DataColumn = New DataColumn("TotalPrice")
            TotalPrice.DataType = System.Type.GetType("System.String")
            dtPurchaseOrderLine.Columns.Add(TotalPrice)

            ' Creating a table schema Columns
            Dim TotalHandling As DataColumn = New DataColumn("TotalHandling")
            TotalHandling.DataType = System.Type.GetType("System.String")
            dtPurchaseOrderLine.Columns.Add(TotalHandling)

            ' Creating a table schema Columns
            Dim TotalFreight As DataColumn = New DataColumn("TotalFreight")
            TotalPrice.DataType = System.Type.GetType("System.String")
            dtPurchaseOrderLine.Columns.Add(TotalFreight)

            ' Creating a table schema Columns
            Dim SubTeam_Name As DataColumn = New DataColumn("SubTeam_Name")
            SubTeam_Name.DataType = System.Type.GetType("System.String")
            dtPurchaseOrderLine.Columns.Add(SubTeam_Name)

            ' Creating a table schema Columns
            Dim UnitFreight As DataColumn = New DataColumn("UnitFreight")
            TotalPrice.DataType = System.Type.GetType("System.String")
            dtPurchaseOrderLine.Columns.Add(UnitFreight)

            ' Creating a table schema Columns
            Dim UnitsReceived As DataColumn = New DataColumn("UnitsReceived")
            CasePrice.DataType = System.Type.GetType("System.String")
            dtPurchaseOrderLine.Columns.Add(UnitsReceived)

            ' Creating a table schema Columns
            Dim SubTeam_No As DataColumn = New DataColumn("SubTeam_No")
            TotalPrice.DataType = System.Type.GetType("System.String")
            dtPurchaseOrderLine.Columns.Add(SubTeam_No)

            ' Creating a table schema Columns
            Dim Team_No As DataColumn = New DataColumn("Team_No")
            Team_No.DataType = System.Type.GetType("System.String")
            dtPurchaseOrderLine.Columns.Add(Team_No)

            ' Creating a table schema Columns
            Dim Category_Name As DataColumn = New DataColumn("Category_Name")
            Category_Name.DataType = System.Type.GetType("System.String")
            dtPurchaseOrderLine.Columns.Add(Category_Name)

            ' Creating a table schema Columns
            Dim Brand_Name As DataColumn = New DataColumn("Brand_Name")
            Brand_Name.DataType = System.Type.GetType("System.String")
            dtPurchaseOrderLine.Columns.Add(Brand_Name)

            ' Creating a table schema Columns
            Dim Origin_Name As DataColumn = New DataColumn("Origin_Name")
            Origin_Name.DataType = System.Type.GetType("System.String")
            dtPurchaseOrderLine.Columns.Add(Origin_Name)

            ' Creating a table schema Columns
            Dim Proc_Name As DataColumn = New DataColumn("Proc_Name")
            Proc_Name.DataType = System.Type.GetType("System.String")
            dtPurchaseOrderLine.Columns.Add(Proc_Name)

            ' Creating a table schema Columns
            Dim Lot_no As DataColumn = New DataColumn("Lot_no")
            Lot_no.DataType = System.Type.GetType("System.String")
            dtPurchaseOrderLine.Columns.Add(Lot_no)

            ' Read and write the dataset output to a new DataTable.
            Dim drReceiveCheckDetails As DataRow

            ' Dataset to send as an output from the webMethod, as an input to the Report.
            Dim dsPurchaseOrderLineOut As DataSet = New DataSet("PurchaseOrderLine")
            If dsReceiveCheckList.Tables(0).Rows.Count > 0 Then
                For Each dataRow In dsReceiveCheckList.Tables(0).Rows
                    drReceiveCheckDetails = dtPurchaseOrderLine.NewRow()
                    ' Filling the Data Row with Data.

                    If Not IsDBNull(dataRow("OrderItem_ID")) Then
                        If Len(Trim(dataRow("OrderItem_ID"))) > 0 Then
                            drReceiveCheckDetails.Item("LineItem") = dataRow("OrderItem_ID")
                        End If
                    End If
                    If Not IsDBNull(dataRow("Identifier")) Then
                        If Len(Trim(dataRow("Identifier"))) > 0 Then
                            drReceiveCheckDetails.Item("Identifier") = dataRow("Identifier")
                        End If
                    End If

                    If Not IsDBNull(dataRow("Item_Description")) Then
                        If Len(Trim(dataRow("Item_Description"))) > 0 Then
                            drReceiveCheckDetails.Item("Item_Description") = dataRow("Item_Description")
                        End If
                    End If
                    drReceiveCheckDetails.Item("Package_Desc") = dataRow("Package_Desc1") & "/" & dataRow("Package_Desc2") & " " & dataRow("Package_Unit")
                    drReceiveCheckDetails.Item("Package_Desc1") = dataRow("Package_Desc1")
                    drReceiveCheckDetails.Item("QuantityOrdered") = dataRow("QuantityReceived")
                    drReceiveCheckDetails.Item("QuantityUnit") = dataRow("Unit_Name")
                    If dataRow("QuantityReceived") <> 0 Then drReceiveCheckDetails.Item("CasePrice") = dataRow("ReceivedItemCost") / dataRow("QuantityReceived")
                    drReceiveCheckDetails.Item("TotalPrice") = dataRow("ReceivedItemCost")
                    drReceiveCheckDetails.Item("TotalHandling") = 0
                    drReceiveCheckDetails.Item("TotalFreight") = dataRow("EstItemFreight")
                    drReceiveCheckDetails.Item("SubTeam_Name") = dataRow("SubTeam_Name")
                    drReceiveCheckDetails.Item("UnitFreight") = dataRow("EstUnitFreight")
                    drReceiveCheckDetails.Item("UnitsReceived") = dataRow("UnitsReceived")
                    drReceiveCheckDetails.Item("SubTeam_No") = dataRow("SubTeam_No")
                    drReceiveCheckDetails.Item("Team_No") = dataRow("Team_No")
                    drReceiveCheckDetails.Item("Category_Name") = dataRow("Category_Name")
                    drReceiveCheckDetails.Item("Brand_Name") = dataRow("Brand_Name")
                    drReceiveCheckDetails.Item("Origin_Name") = dataRow("Origin_Name")
                    drReceiveCheckDetails.Item("Proc_Name") = dataRow("Proc_Name")
                    drReceiveCheckDetails.Item("Lot_no") = dataRow("Lot_no")
                    dtPurchaseOrderLine.Rows.Add(drReceiveCheckDetails)

                    If dataRow("DiscountType") = 3 And dataRow("QuantityDiscount") > 0 Then
                        drReceiveCheckDetails = dtPurchaseOrderLine.NewRow()
                        drReceiveCheckDetails.Item("LineItem") = dataRow("OrderItem_ID")
                        drReceiveCheckDetails.Item("Identifier") = dataRow("Identifier")
                        drReceiveCheckDetails.Item("Item_Description") = dataRow("Item_Description")
                        drReceiveCheckDetails.Item("Package_Desc") = dataRow("Package_Desc1") & "/" & dataRow("Package_Desc2") & " " & dataRow("Package_Unit")
                        drReceiveCheckDetails.Item("Package_Desc1") = dataRow("Package_Desc1")
                        ' Here quantityDiscount is populating into the field.
                        drReceiveCheckDetails.Item("QuantityOrdered") = CDbl(dataRow("QuantityDiscount"))
                        drReceiveCheckDetails.Item("QuantityUnit") = dataRow("Unit_Name")
                        drReceiveCheckDetails.Item("CasePrice") = 0
                        drReceiveCheckDetails.Item("TotalPrice") = 0
                        drReceiveCheckDetails.Item("TotalHandling") = 0
                        drReceiveCheckDetails.Item("TotalFreight") = 0
                        drReceiveCheckDetails.Item("SubTeam_Name") = dataRow("SubTeam_Name")
                        drReceiveCheckDetails.Item("UnitFreight") = 0
                        drReceiveCheckDetails.Item("UnitsReceived") = 0
                        drReceiveCheckDetails.Item("SubTeam_No") = dataRow("SubTeam_No")
                        drReceiveCheckDetails.Item("Team_No") = dataRow("Team_No")
                        drReceiveCheckDetails.Item("Category_Name") = dataRow("Category_Name")
                        drReceiveCheckDetails.Item("Brand_Name") = dataRow("Brand_Name")
                        drReceiveCheckDetails.Item("Origin_Name") = dataRow("Origin_Name")
                        drReceiveCheckDetails.Item("Proc_Name") = dataRow("Proc_Name")
                        dtPurchaseOrderLine.Rows.Add(drReceiveCheckDetails)
                    End If
                Next
            End If

            ' Adding DataTable to DataSet
            dsPurchaseOrderLineOut.Tables.Add(dtPurchaseOrderLine)
            Return dsPurchaseOrderLineOut ' Returning dataset as output to the report.

        Catch e As Exception
            Throw
        End Try
    End Function


    ' This webMethod is called from PreAllocation Inventory Weekly History Report.
    <WebMethod()> _
            Public Function GetPreAllocWeeklyHistory(ByVal Vendor_ID As Integer, ByVal SubTeam_ID As Integer) As DataSet
        Try
            ' Handling reporting services refresh option which takes input parameter as 0.
            Dim val As Integer = Integer.Parse(Vendor_ID)
            If (val < 1) Then
                val = 1
            End If

            ' Handling reporting services refresh option which takes input parameter as 0.
            Dim val1 As Integer = Integer.Parse(SubTeam_ID)
            If (val1 < 1) Then
                val1 = 1
            End If

            ' Executing and fetching the stored procedure output into Dataset.
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam1 As New DBParam
            Dim currentParam2 As New DBParam
            
            Dim dataRow As DataRow  ' To read all rows returned by the DataTable.
            ' Dataset to fetch the output of the stored procedure.
            Dim dsWeeklyOrderedQtyHistory As DataSet = New DataSet("NewDataSet")

            ' Passing Vendor_ID parameter for stored procedure.
            currentParam1 = New DBParam
            currentParam1.Name = "Vendor_ID"
            currentParam1.Value = val
            currentParam1.Type = DBParamType.Int
            paramList.Add(currentParam1)

            ' Passing SubTeam_ID parameter for stored procedure.
            currentParam2 = New DBParam
            currentParam2.Name = "SubTeam_ID"
            currentParam2.Value = val1
            currentParam2.Type = DBParamType.Int
            paramList.Add(currentParam2)


            ' Executing the WareHouseWeeklyOrderedQuantities Stored Procedure.
            dsWeeklyOrderedQtyHistory = factory.GetStoredProcedureDataSet("dbo.WareHouseWeeklyOrderedQuantities", paramList)


            ' Dataset to send as an output from the webMethod, as an input to the Report.
            Dim dsPreAllocInventoryWeklyHistory As DataSet = New DataSet("PreInventoryWeeklyHistory")


            ' Creating a DataTable named WeeklyOrderedQtyHistory to send  an output to report through Dataset.
            Dim dtPreAllocInventoryWeklyHistory As DataTable
            dtPreAllocInventoryWeklyHistory = New DataTable("PreInventoryWeeklyHistory")


            ' Creating output DataTable Schema.

            Dim ItemDescription As DataColumn = New DataColumn("ItemDescription")
            ItemDescription.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPreAllocInventoryWeklyHistory.Columns.Add(ItemDescription)

            Dim ItemIdentifier As DataColumn = New DataColumn("ItemIdentifier")
            ItemIdentifier.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPreAllocInventoryWeklyHistory.Columns.Add(ItemIdentifier)


            Dim monOnHandQty As DataColumn = New DataColumn("MonOnHandQty")
            monOnHandQty.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPreAllocInventoryWeklyHistory.Columns.Add(monOnHandQty)

            Dim monQtyOrdered As DataColumn = New DataColumn("MonQtyOrdered")
            monQtyOrdered.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPreAllocInventoryWeklyHistory.Columns.Add(monQtyOrdered)

            Dim TueOnHandQty As DataColumn = New DataColumn("TueOnHandQty")
            TueOnHandQty.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPreAllocInventoryWeklyHistory.Columns.Add(TueOnHandQty)

            Dim TueQtyOrdered As DataColumn = New DataColumn("TueQtyOrdered")
            TueQtyOrdered.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPreAllocInventoryWeklyHistory.Columns.Add(TueQtyOrdered)

            Dim WedOnHandQty As DataColumn = New DataColumn("WedOnHandQty")
            WedOnHandQty.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPreAllocInventoryWeklyHistory.Columns.Add(WedOnHandQty)

            Dim WedQtyOrdered As DataColumn = New DataColumn("WedQtyOrdered")
            WedQtyOrdered.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPreAllocInventoryWeklyHistory.Columns.Add(WedQtyOrdered)

            Dim ThursOnHandQty As DataColumn = New DataColumn("ThursOnHandQty")
            ThursOnHandQty.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPreAllocInventoryWeklyHistory.Columns.Add(ThursOnHandQty)

            Dim ThursQtyOrdered As DataColumn = New DataColumn("ThursQtyOrdered")
            ThursQtyOrdered.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPreAllocInventoryWeklyHistory.Columns.Add(ThursQtyOrdered)

            Dim FriOnHandQty As DataColumn = New DataColumn("FriOnHandQty")
            FriOnHandQty.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPreAllocInventoryWeklyHistory.Columns.Add(FriOnHandQty)

            Dim FriQtyOrdered As DataColumn = New DataColumn("FriQtyOrdered")
            FriQtyOrdered.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPreAllocInventoryWeklyHistory.Columns.Add(FriQtyOrdered)

            Dim SatOnHandQty As DataColumn = New DataColumn("SatOnHandQty")
            SatOnHandQty.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPreAllocInventoryWeklyHistory.Columns.Add(SatOnHandQty)

            Dim SatQtyOrdered As DataColumn = New DataColumn("SatQtyOrdered")
            SatQtyOrdered.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPreAllocInventoryWeklyHistory.Columns.Add(SatQtyOrdered)

            Dim SunOnHandQty As DataColumn = New DataColumn("SunOnHandQty")
            SunOnHandQty.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPreAllocInventoryWeklyHistory.Columns.Add(SunOnHandQty)

            Dim SunQtyOrdered As DataColumn = New DataColumn("SunQtyOrdered")
            SunQtyOrdered.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPreAllocInventoryWeklyHistory.Columns.Add(SunQtyOrdered)


            ' Read and write the dataset output to a new DataTable.
            ' DataRow to populate the DataTable.
            Dim drPreAllocInventoryWeklyHistory As DataRow

            ' To check the ItemKey value.
            Dim strItemKey As String
            strItemKey = ""


            If dsWeeklyOrderedQtyHistory.Tables(0).Rows.Count > 0 Then

                ' Reading record by record from the DataTable.
                For Each dataRow In dsWeeklyOrderedQtyHistory.Tables(0).Rows
                    If strItemKey = "" Then

                        strItemKey = dataRow("Item_Key")
                        drPreAllocInventoryWeklyHistory = dtPreAllocInventoryWeklyHistory.NewRow()
                        drPreAllocInventoryWeklyHistory.Item("SunQtyOrdered") = 0
                        drPreAllocInventoryWeklyHistory.Item("MonQtyOrdered") = 0
                        drPreAllocInventoryWeklyHistory.Item("TueQtyOrdered") = 0
                        drPreAllocInventoryWeklyHistory.Item("WedQtyOrdered") = 0
                        drPreAllocInventoryWeklyHistory.Item("ThursQtyOrdered") = 0
                        drPreAllocInventoryWeklyHistory.Item("FriQtyOrdered") = 0
                        drPreAllocInventoryWeklyHistory.Item("SatQtyOrdered") = 0

                        drPreAllocInventoryWeklyHistory.Item("SunOnHandQty") = 0
                        drPreAllocInventoryWeklyHistory.Item("MonOnHandQty") = 0
                        drPreAllocInventoryWeklyHistory.Item("TueOnHandQty") = 0
                        drPreAllocInventoryWeklyHistory.Item("WedOnHandQty") = 0
                        drPreAllocInventoryWeklyHistory.Item("ThursOnHandQty") = 0
                        drPreAllocInventoryWeklyHistory.Item("FriOnHandQty") = 0
                        drPreAllocInventoryWeklyHistory.Item("SatOnHandQty") = 0

                    End If
                    If strItemKey <> dataRow("Item_Key") Then
                        dtPreAllocInventoryWeklyHistory.Rows.Add(drPreAllocInventoryWeklyHistory)
                        drPreAllocInventoryWeklyHistory = dtPreAllocInventoryWeklyHistory.NewRow()
                        strItemKey = dataRow("Item_Key")
                    End If
                    ' Filling the Data Row with Data.
                    If Not IsDBNull(dataRow("Item_Key")) Then
                        drPreAllocInventoryWeklyHistory.Item("ItemIdentifier") = dataRow("Identifier")
                        drPreAllocInventoryWeklyHistory.Item("ItemDescription") = dataRow("Item_description")
                    End If

                    Select Case Format(dataRow("ExpectedDate"), "dddd")

                        Case "Sunday"
                            If Not IsDBNull(dataRow("ToT_BOH")) Then
                                drPreAllocInventoryWeklyHistory.Item("SunOnHandQty") = dataRow("ToT_BOH")
                            End If
                            drPreAllocInventoryWeklyHistory.Item("SunQtyOrdered") = dataRow("QtyOrd")
                        Case "Monday"
                            If Not IsDBNull(dataRow("ToT_BOH")) Then
                                drPreAllocInventoryWeklyHistory.Item("MonOnHandQty") = dataRow("ToT_BOH")
                            End If
                            drPreAllocInventoryWeklyHistory.Item("MonQtyOrdered") = dataRow("QtyOrd")
                        Case "Tuesday"
                            If Not IsDBNull(dataRow("ToT_BOH")) Then
                                drPreAllocInventoryWeklyHistory.Item("TueOnHandQty") = dataRow("ToT_BOH")
                            End If
                            drPreAllocInventoryWeklyHistory.Item("TueQtyOrdered") = dataRow("QtyOrd")
                        Case "Wednesday"
                            If Not IsDBNull(dataRow("ToT_BOH")) Then
                                drPreAllocInventoryWeklyHistory.Item("WedOnHandQty") = dataRow("ToT_BOH")
                            End If
                            drPreAllocInventoryWeklyHistory.Item("WedQtyOrdered") = dataRow("QtyOrd")
                        Case "Thursday"
                            If Not IsDBNull(dataRow("ToT_BOH")) Then
                                drPreAllocInventoryWeklyHistory.Item("ThursOnHandQty") = dataRow("ToT_BOH")
                            End If
                            drPreAllocInventoryWeklyHistory.Item("ThursQtyOrdered") = dataRow("QtyOrd")
                        Case "Friday"
                            If Not IsDBNull(dataRow("ToT_BOH")) Then
                                drPreAllocInventoryWeklyHistory.Item("FriOnHandQty") = dataRow("ToT_BOH")
                            End If
                            drPreAllocInventoryWeklyHistory.Item("FriQtyOrdered") = dataRow("QtyOrd")
                        Case "Saturday"
                            If Not IsDBNull(dataRow("ToT_BOH")) Then
                                drPreAllocInventoryWeklyHistory.Item("SatOnHandQty") = dataRow("ToT_BOH")
                            End If
                            drPreAllocInventoryWeklyHistory.Item("SatQtyOrdered") = dataRow("QtyOrd")
                    End Select

                Next

                ' For the last record.
                dtPreAllocInventoryWeklyHistory.Rows.Add(drPreAllocInventoryWeklyHistory)
            End If

            ' Adding DataTable to DataSet.
            dsPreAllocInventoryWeklyHistory.Tables.Add(dtPreAllocInventoryWeklyHistory)
            Return dsPreAllocInventoryWeklyHistory ' Returning dataset as output to the report.

        Catch e As Exception
            Throw
        End Try
    End Function

    ' This webMethod is called from PreAllocation Inventory Weekly History Report.
    <WebMethod()> _
            Public Function GetPostAllocWeeklyHistory(ByVal Vendor_ID As Integer, ByVal SubTeam_ID As Integer) As DataSet
        Try
            ' Handling reporting services refresh option which takes input parameter as 0.
            Dim val As Integer = Integer.Parse(Vendor_ID)
            If (val < 1) Then
                val = 1
            End If

            ' Handling reporting services refresh option which takes input parameter as 0.
            Dim val1 As Integer = Integer.Parse(SubTeam_ID)
            If (val1 < 1) Then
                val1 = 1
            End If

            ' Executing and fetching the stored procedure output into Dataset.
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam1 As New DBParam
            Dim currentParam2 As New DBParam

            Dim dataRow As DataRow  ' To read all rows returned by the DataTable.

            ' Dataset to fetch the output of the stored procedure.
            Dim dsWeeklyPostAllocatedHistory As DataSet = New DataSet("NewDataSet")

            ' Passing Vendor_ID parameter for stored procedure.
            currentParam1 = New DBParam
            currentParam1.Name = "Vendor_ID"
            currentParam1.Value = val
            currentParam1.Type = DBParamType.Int
            paramList.Add(currentParam1)

            ' Passing SubTeam_ID parameter for stored procedure.
            currentParam2 = New DBParam
            currentParam2.Name = "SubTeam_ID"
            currentParam2.Value = val1
            currentParam2.Type = DBParamType.Int
            paramList.Add(currentParam2)


            ' Executing the WareHouseWeeklyOrderedQuantities Stored Procedure.
            dsWeeklyPostAllocatedHistory = factory.GetStoredProcedureDataSet("dbo.WareHouseWeeklyAllocatedQuantities", paramList)


            ' Dataset to send as an output from the webMethod, as an input to the Report.
            Dim dsPostAllocInventoryWeklyHistory As DataSet = New DataSet("PostInventoryWeeklyHistory")


            ' Creating a DataTable named WeeklyOrderedQtyHistory to send  an output to report through Dataset.
            Dim dtPostAllocInventoryWeklyHistory As DataTable
            dtPostAllocInventoryWeklyHistory = New DataTable("PostInventoryWeeklyHistory")


            ' Creating output DataTable Schema.

            Dim ItemDescription As DataColumn = New DataColumn("ItemDescription")
            ItemDescription.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPostAllocInventoryWeklyHistory.Columns.Add(ItemDescription)

            Dim ItemIdentifier As DataColumn = New DataColumn("ItemIdentifier")
            ItemIdentifier.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPostAllocInventoryWeklyHistory.Columns.Add(ItemIdentifier)


            Dim monAvlQty As DataColumn = New DataColumn("MonAvlQty")
            monAvlQty.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPostAllocInventoryWeklyHistory.Columns.Add(monAvlQty)

            Dim monAllocQty As DataColumn = New DataColumn("MonAllocQty")
            monAllocQty.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPostAllocInventoryWeklyHistory.Columns.Add(monAllocQty)

            Dim TueAvlQty As DataColumn = New DataColumn("TueAvlQty")
            TueAvlQty.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPostAllocInventoryWeklyHistory.Columns.Add(TueAvlQty)

            Dim TueAllocQty As DataColumn = New DataColumn("TueAllocQty")
            TueAllocQty.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPostAllocInventoryWeklyHistory.Columns.Add(TueAllocQty)

            Dim WedAvlQty As DataColumn = New DataColumn("WedAvlQty")
            WedAvlQty.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPostAllocInventoryWeklyHistory.Columns.Add(WedAvlQty)

            Dim WedAllocQty As DataColumn = New DataColumn("WedAllocQty")
            WedAllocQty.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPostAllocInventoryWeklyHistory.Columns.Add(WedAllocQty)

            Dim ThursAvlQty As DataColumn = New DataColumn("ThursAvlQty")
            ThursAvlQty.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPostAllocInventoryWeklyHistory.Columns.Add(ThursAvlQty)

            Dim ThursAllocQty As DataColumn = New DataColumn("ThursAllocQty")
            ThursAllocQty.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPostAllocInventoryWeklyHistory.Columns.Add(ThursAllocQty)

            Dim FriAvlQty As DataColumn = New DataColumn("FriAvlQty")
            FriAvlQty.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPostAllocInventoryWeklyHistory.Columns.Add(FriAvlQty)

            Dim FriAllocQty As DataColumn = New DataColumn("FriAllocQty")
            FriAllocQty.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPostAllocInventoryWeklyHistory.Columns.Add(FriAllocQty)

            Dim SatAvlQty As DataColumn = New DataColumn("SatAvlQty")
            SatAvlQty.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPostAllocInventoryWeklyHistory.Columns.Add(SatAvlQty)

            Dim SatAllocQty As DataColumn = New DataColumn("SatAllocQty")
            SatAllocQty.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPostAllocInventoryWeklyHistory.Columns.Add(SatAllocQty)

            Dim SunAvlQty As DataColumn = New DataColumn("SunAvlQty")
            SunAvlQty.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPostAllocInventoryWeklyHistory.Columns.Add(SunAvlQty)

            Dim SunAllocQty As DataColumn = New DataColumn("SunAllocQty")
            SunAllocQty.DataType = System.Type.GetType("System.String")
            ' Setting the datatype for the column
            dtPostAllocInventoryWeklyHistory.Columns.Add(SunAllocQty)


            ' Read and write the dataset output to a new DataTable.
            ' DataRow to populate the DataTable.
            Dim drPostAllocInventoryWeklyHistory As DataRow

            ' To check the ItemKey value.
            Dim strItemKey As String
            strItemKey = ""


            If dsWeeklyPostAllocatedHistory.Tables(0).Rows.Count > 0 Then

                ' Reading record by record from the DataTable.
                For Each dataRow In dsWeeklyPostAllocatedHistory.Tables(0).Rows
                    strItemKey = dataRow("Item_Key")
                    drPostAllocInventoryWeklyHistory = dtPostAllocInventoryWeklyHistory.NewRow()
                    drPostAllocInventoryWeklyHistory.Item("SunAvlQty") = 0
                    drPostAllocInventoryWeklyHistory.Item("MonAvlQty") = 0
                    drPostAllocInventoryWeklyHistory.Item("TueAvlQty") = 0
                    drPostAllocInventoryWeklyHistory.Item("WedAvlQty") = 0
                    drPostAllocInventoryWeklyHistory.Item("ThursAvlQty") = 0
                    drPostAllocInventoryWeklyHistory.Item("FriAvlQty") = 0
                    drPostAllocInventoryWeklyHistory.Item("SatAvlQty") = 0

                    drPostAllocInventoryWeklyHistory.Item("SunAllocQty") = 0
                    drPostAllocInventoryWeklyHistory.Item("MonAllocQty") = 0
                    drPostAllocInventoryWeklyHistory.Item("TueAllocQty") = 0
                    drPostAllocInventoryWeklyHistory.Item("WedAllocQty") = 0
                    drPostAllocInventoryWeklyHistory.Item("ThursAllocQty") = 0
                    drPostAllocInventoryWeklyHistory.Item("FriAllocQty") = 0
                    drPostAllocInventoryWeklyHistory.Item("SatAllocQty") = 0

                   
                    ' Filling the Data Row with Data.
                    If Not IsDBNull(dataRow("Item_Key")) Then
                        drPostAllocInventoryWeklyHistory.Item("ItemIdentifier") = dataRow("Identifier")
                        drPostAllocInventoryWeklyHistory.Item("ItemDescription") = dataRow("Item_description")
                    End If

                    Select Case Format(Now(), "dddd")

                        Case "Sunday"
                            If Not IsDBNull(dataRow("ToT_BOH")) Then
                                drPostAllocInventoryWeklyHistory.Item("SunAvlQty") = dataRow("ToT_BOH") - dataRow("QtyAllocated")
                            End If
                            drPostAllocInventoryWeklyHistory.Item("SunAllocQty") = dataRow("QtyAllocated")
                        Case "Monday"
                            If Not IsDBNull(dataRow("ToT_BOH")) Then
                                drPostAllocInventoryWeklyHistory.Item("MonAvlQty") = dataRow("ToT_BOH") - dataRow("QtyAllocated")
                            End If
                            drPostAllocInventoryWeklyHistory.Item("MonAllocQty") = dataRow("QtyAllocated")
                        Case "Tuesday"
                            If Not IsDBNull(dataRow("ToT_BOH")) Then
                                drPostAllocInventoryWeklyHistory.Item("TueAvlQty") = dataRow("ToT_BOH") - dataRow("QtyAllocated")
                            End If
                            drPostAllocInventoryWeklyHistory.Item("TueAllocQty") = dataRow("QtyAllocated")
                        Case "Wednesday"
                            If Not IsDBNull(dataRow("ToT_BOH")) Then
                                drPostAllocInventoryWeklyHistory.Item("WedAvlQty") = dataRow("ToT_BOH") - dataRow("QtyAllocated")
                            End If
                            drPostAllocInventoryWeklyHistory.Item("WedAllocQty") = dataRow("QtyAllocated")
                        Case "Thursday"
                            If Not IsDBNull(dataRow("ToT_BOH")) Then
                                drPostAllocInventoryWeklyHistory.Item("ThursAvlQty") = dataRow("ToT_BOH") - dataRow("QtyAllocated")
                            End If
                            drPostAllocInventoryWeklyHistory.Item("ThursAllocQty") = dataRow("QtyAllocated")
                        Case "Friday"
                            If Not IsDBNull(dataRow("ToT_BOH")) Then
                                drPostAllocInventoryWeklyHistory.Item("FriAvlQty") = dataRow("ToT_BOH") - dataRow("QtyAllocated")
                            End If
                            drPostAllocInventoryWeklyHistory.Item("FriAllocQty") = dataRow("QtyAllocated")
                        Case "Saturday"
                            If Not IsDBNull(dataRow("ToT_BOH")) Then
                                drPostAllocInventoryWeklyHistory.Item("SatAvlQty") = dataRow("ToT_BOH") - dataRow("QtyAllocated")
                            End If
                            drPostAllocInventoryWeklyHistory.Item("SatAllocQty") = dataRow("QtyAllocated")
                    End Select
                    ' For the last record.
                    dtPostAllocInventoryWeklyHistory.Rows.Add(drPostAllocInventoryWeklyHistory)

                Next

               
            End If

            ' Adding DataTable to DataSet.
            dsPostAllocInventoryWeklyHistory.Tables.Add(dtPostAllocInventoryWeklyHistory)
            Return dsPostAllocInventoryWeklyHistory ' Returning dataset as output to the report.

        Catch e As Exception
            Throw
        End Try
    End Function

    ' Is this function being used?
    <WebMethod()> _
    Public Function GetOrderItems(ByVal OrderHeader_ID As Integer) As DataSet
        Try

            'Dim val As Integer = Integer.Parse(OrderHeader_ID)
            Dim val As Integer = OrderHeader_ID
            If (val < 1) Then
                val = 1
            ElseIf (val > 290) Then
                val = 290
            End If

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As New DBParam
            Dim ds As DataSet = New DataSet("NewDataSet")

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = val
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            Dim sConn As String

            ds = factory.GetStoredProcedureDataSet("dbo.GetOrderItemLines", paramList)

            sConn = ConfigurationManager.ConnectionStrings(DataFactory.ItemCatalog).ConnectionString

            'sConn = "Data Source=idd-ce; Initial Catalog=itemcatalog_b16; Integrated Security=true"

            Dim da As SqlDataAdapter = New SqlDataAdapter("dbo.GetOrderItemLines " + OrderHeader_ID.ToString(), sConn)
            ds = factory.GetStoredProcedureDataSet("dbo.GetOrderItemLines", paramList)
            ds = factory.GetStoredProcedureDataSet("dbo.GetRegions")

            da.Fill(ds)

            ' adding new table
            ' Creating a DataTable named ReceivingCheckList to send as an output to report through Dataset.
            Dim dtReceiveCheckListDetails As DataTable
            Dim drReceiveCheckDetails As DataRow
            dtReceiveCheckListDetails = New DataTable("ReceivingCheckList2")

            ' Creating a Dattable schema for the report output DataTable with all columns.
            Dim LineItem As DataColumn = New DataColumn("LineItem")
            LineItem.DataType = System.Type.GetType("System.String")

            ' Setting the datatype for the column
            dtReceiveCheckListDetails.Columns.Add(LineItem)
            drReceiveCheckDetails = dtReceiveCheckListDetails.NewRow()
            drReceiveCheckDetails.Item("LineItem") = 23
            dtReceiveCheckListDetails.Rows.Add(drReceiveCheckDetails)
            ds.Tables.Add(dtReceiveCheckListDetails)

            da.Update(ds, "ReceivingCheckList2")

            Using connection As New SqlConnection(sConn)
                connection.Open()
                Dim command As New SqlCommand("select * from ReceivingCheckList2", connection)
                Dim reader As SqlDataReader = command.ExecuteReader()
                While reader.Read()
                    MsgBox(reader(0))
                End While
            End Using

            Return ds
        Catch e As Exception
            Throw
        End Try

    End Function

    <WebMethod()> _
       Public Function GetPurchaseOrderLineItems(ByVal OrderHeader_ID As Integer, ByVal optIdentifier As Integer, ByVal optSort As Integer) As DataSet
        Try
            ' Handling reporting services refresh option which takes input parameter as 0.
            Dim val As Integer = Integer.Parse(OrderHeader_ID)
            If (val < 1) Then
                val = 1
            End If

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As New DBParam
            Dim currentParam2 As New DBParam
            Dim currentParam3 As New DBParam

            Dim ds As DataSet = New DataSet("PurchaseOrderLine")

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = val
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Passing optIdentifier parameter for stored procedure.
            currentParam2 = New DBParam
            currentParam2.Name = "Item_ID"
            currentParam2.Value = optIdentifier
            currentParam2.Type = DBParamType.Int
            paramList.Add(currentParam2)

            ' Passing optSort parameter for stored procedure.
            currentParam3 = New DBParam
            currentParam3.Name = "OptSort"
            currentParam3.Value = optSort
            currentParam3.Type = DBParamType.Int
            paramList.Add(currentParam3)

            ' Executing the storedProcedure.
            ds = factory.GetStoredProcedureDataSet("dbo.GetOrderItemListReport", paramList)

            ' Fetching the dataTable from the Dataset.
            Dim myDataTable As DataTable
            myDataTable = ds.Tables(0)

            ' Updating the data in the dataTable with next rwo values.
            Dim intI As Integer
            For intI = 0 To myDataTable.Rows.Count - 1
                If intI <> (myDataTable.Rows.Count - 1) Then
                    Dim currentRow As DataRow = myDataTable.Rows(intI)
                    Dim NextRow As DataRow = myDataTable.Rows(intI + 1)
                    currentRow("NextCasePrice") = NextRow("CasePrice")
                    currentRow("NextLineItem") = NextRow("LineItem")
                End If
            Next

            ' Updating the DataTable changes.
            myDataTable.AcceptChanges()

            ' Returning the Dataset as an input to the ReportingServices.
            Return ds
        Catch e As Exception
            Throw
        End Try

    End Function

    '' function to fetch receive Checklist details.
    '<WebMethod()> _
    'Public Function GetOrderItems(ByVal OrderHeader_ID As Integer) As DataSet
    '    Try

    '        Dim val As Integer = Integer.Parse(OrderHeader_ID)
    '        If (val < 1) Then
    '            val = 1
    '        ElseIf (val > 290) Then
    '            val = 290
    '        End If


    '        Dim factory As New DataFactory(DataFactory.ItemCatalog)
    '        Dim paramList As New ArrayList
    '        Dim currentParam As New DBParam
    '        Dim ds As DataSet = New DataSet("NewDataSet")
    '        'Dim sConn As String

    '        ' setup parameters for stored proc
    '        currentParam = New DBParam
    '        currentParam.Name = "OrderHeader_ID"
    '        currentParam.Value = val
    '        currentParam.Type = DBParamType.Int
    '        paramList.Add(currentParam)

    '        ds = factory.GetStoredProcedureDataSet("dbo.GetOrderItemLines", paramList)

    '        ' sConn = ConfigurationManager.ConnectionStrings(DataFactory.ItemCatalog).ConnectionString

    '        'sConn = "Data Source=idd-ce; Initial Catalog=itemcatalog_b16; Integrated Security=true"

    '        ' Dim da As SqlDataAdapter = New SqlDataAdapter("dbo.GetOrderItemLines " + OrderHeader_ID.ToString(), sConn)

    '        'Dim da As SqlDataAdapter = New SqlDataAdapter("uspGetEmployeeManagers " + Val.ToString(), _

    '        'da.Fill(ds)

    '        Return ds
    '    Catch e As Exception
    '        Throw
    '    End Try


    'End Function

    <WebMethod()> _
    Public Function GetEmployeeManagers(ByVal EmployeeID As String) As DataSet
        Dim ds As DataSet = New DataSet("Results")
        Try
            Dim val As Integer = Integer.Parse(EmployeeID)
            If (val < 1) Then
                val = 1
            ElseIf (val > 290) Then
                val = 290
            End If
            Dim da As SqlDataAdapter = New SqlDataAdapter("uspGetEmployeeManagers " + val.ToString(), _
               "Data Source=localhost; Initial Catalog=AdventureWorks; Integrated Security=true")
            da.Fill(ds)
        Catch e As Exception
            Throw
        End Try
        Return ds
    End Function

    <WebMethod()> _
        Public Function HelloWorld() As DataSet
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim dc, dcS, dcD As DataColumn
        Dim dr As DataRow
        Dim i As Integer

        dc = New DataColumn("IntegerExample", System.Type.GetType("System.Int32"))
        dc.AutoIncrement = True
        dc.AutoIncrementSeed = 1
        dc.AutoIncrementStep = 1
        dc.ReadOnly = True
        dt.Columns.Add(dc)

        dcS = New DataColumn("StringExample", System.Type.GetType("System.String"))
        dt.Columns.Add(dcS)

        dcD = New DataColumn("DateExample", System.Type.GetType("System.DateTime"))
        dt.Columns.Add(dcD)

        ds.Tables.Add(dt)
        Dim iStart As Integer = 3
        Dim iFinish As Integer = 20

        For i = iStart To iFinish
            dr = dt.NewRow()
            dr.Item(dcS) = "Row " & i.ToString
            dr.Item(dcD) = DateTime.Parse("2/" & i.ToString & "/2007")
            dt.Rows.Add(dr)
        Next

        dt.TableName = "HelloWorld"
        ds.DataSetName = "HelloData"
        Return ds
    End Function

    Private Function CreditOrderDesc(ByVal OrderHeader_ID As Integer) As String
        Try
            Dim val As Integer = Integer.Parse(OrderHeader_ID)
            If (val < 1) Then
                val = 1
            ElseIf (val > 290) Then
                val = 290
            End If
            Dim strCM As String
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As New DBParam
            Dim dsOrderHeader As DataSet = New DataSet("NewDataSet")

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = val
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            dsOrderHeader = factory.GetStoredProcedureDataSet("dbo.GetCreditOrderList", paramList)

            strCM = ""
            Dim row As DataRow
            If dsOrderHeader.Tables(0).Rows.Count > 0 Then
                For Each row In dsOrderHeader.Tables(0).Rows
                    If strCM = "" Then
                        strCM = Convert.ToString(row("ReturnOrderHeader_id"))
                    Else
                        strCM = strCM & "," & Convert.ToString(row("ReturnOrderHeader_id"))
                    End If
                Next
                strCM = "CM ON PO(s) " & strCM
                Return strCM
            Else
                Return ""
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function

    ' This function to display the Title information on OrderInvoice Report.
    Private Function InvoiceTitle(ByVal Ordertype As Integer, ByVal TransfertoDeptName As String, ByVal TransferDeptName As String) As String

        Dim sProductType As String
        Dim sOrderType As String
        Dim sTitle As String

        ' Assigning Empty Values to strings.

        sOrderType = String.Empty
        sProductType = String.Empty
        sTitle = String.Empty

        If Ordertype = 1 Then
            sOrderType = "Purchase "
            sTitle = Trim(TransfertoDeptName)
        ElseIf Ordertype = 2 Then
            sOrderType = "Distribution "
            sTitle = Trim(TransferDeptName) + " - " + Trim(TransfertoDeptName)
        ElseIf Ordertype = 3 Then
            sOrderType = "Transfer "
            sTitle = Trim(TransferDeptName) + " - " + Trim(TransfertoDeptName)
        End If

        If Ordertype = 1 Then
            sProductType = "Product "
        ElseIf Ordertype = 2 Then
            sProductType = "Packaging "
        ElseIf Ordertype = 3 Then
            sProductType = "Supplies "
        End If

        Return sOrderType + sProductType + "Invoice" + Chr(13) + sTitle
    End Function

    ' This function to display the ShowDiscount information on OrderInvoice Report.
    Private Function InvoiceShowDiscount(ByVal DiscountType As Integer, ByVal QuantityDiscount As String) As String
        Dim strShowDiscount As String
        strShowDiscount = String.Empty
        If DiscountType = 1 Then
            strShowDiscount = "- $" + QuantityDiscount + " Cash"
        ElseIf DiscountType = 2 Then
            strShowDiscount = "- " + QuantityDiscount + "% Item Cost"
        ElseIf DiscountType = 4 Then
            strShowDiscount = "- " + QuantityDiscount + "% Landed Cost"
        End If
        Return strShowDiscount
    End Function
End Class
