Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient
Imports log4net

Namespace WholeFoods.IRMA.ItemHosting.DataAccess

    Public Class CostPromotionDAO

        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


#Region "read methods"

        ''' <summary>
        ''' returns list of CostPromoCodeTypeBO objects for all records in the CostPromoCodeType table
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetCostPromoCodeTypes() As ArrayList

            logger.Debug("GetCostPromoCodeTypes Entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim codeInfo As CostPromoCodeTypeBO
            Dim promoCodeList As New ArrayList
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetCostPromoCodeTypes")

                While results.Read
                    codeInfo = New CostPromoCodeTypeBO()
                    codeInfo.CostPromoCodeTypeID = results.GetInt32(results.GetOrdinal("CostPromoCodeTypeID"))
                    codeInfo.CostPromoCode = results.GetInt32(results.GetOrdinal("CostPromoCode"))
                    codeInfo.CostPromoDesc = results.GetString(results.GetOrdinal("CostPromoDesc"))

                    promoCodeList.Add(codeInfo)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try
            logger.Debug("GetCostPromoCodeTypes Exit")

            Return promoCodeList
        End Function

        ''' <summary>
        ''' returns list of VendorDealTypeBO objects for all records in the VendorDealType table
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetVendorDealTypes() As ArrayList

            logger.Debug("GetVendorDealTypes Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim dealInfo As VendorDealTypeBO
            Dim dealList As New ArrayList
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetVendorDealTypes")

                While results.Read
                    dealInfo = New VendorDealTypeBO()
                    dealInfo.VendorDealTypeID = results.GetInt32(results.GetOrdinal("VendorDealTypeID"))
                    dealInfo.VendorDealTypeCode = results.GetString(results.GetOrdinal("Code"))
                    dealInfo.VendorDealTypeDesc = results.GetString(results.GetOrdinal("Description"))
                    If results.IsDBNull(3) Then
                        dealInfo.CaseAmtType = Nothing
                    Else
                        dealInfo.CaseAmtType = results.GetString(results.GetOrdinal("CaseAmtType"))
                    End If

                    dealList.Add(dealInfo)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetVendorDealTypes Exit")


            Return dealList
        End Function

        ''' <summary>
        ''' gets list of all data in VendorDealHistory for the current item/vendor passed in
        ''' building data table because infragistics does not play nice with objects that have child object properties
        ''' </summary>
        ''' <param name="itemKey"></param>
        ''' <param name="vendorID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetVendorDeals(ByVal itemKey As Integer, ByVal vendorID As Integer, ByVal storeNo As Integer) As DataTable

            logger.Debug("GetVendorDeals Entry with itemKey = " + itemKey.ToString + ", vendorID =" + vendorID.ToString + "storeNo=" + storeNo.ToString)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim dealList As New ArrayList
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Dim table As New DataTable("VendorDealHistory")
            Dim row As DataRow

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = itemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Vendor_ID"
                currentParam.Value = vendorID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = storeNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetVendorDealHistory", paramList)

                'add columns to table
                table.Columns.Add(New DataColumn("VendorDealHistoryID", GetType(Integer)))
                table.Columns.Add(New DataColumn("ItemKey", GetType(Integer)))
                table.Columns.Add(New DataColumn("VendorID", GetType(Integer)))
                table.Columns.Add(New DataColumn("CaseQty", GetType(Integer)))
                table.Columns.Add(New DataColumn("PackageDesc1", GetType(Decimal)))
                table.Columns.Add(New DataColumn("CaseAmt", GetType(Decimal)))
                table.Columns.Add(New DataColumn("CaseAmtType", GetType(String)))
                table.Columns.Add(New DataColumn("StartDate", GetType(Date)))
                table.Columns.Add(New DataColumn("EndDate", GetType(Date)))
                table.Columns.Add(New DataColumn("VendorDealTypeID", GetType(Integer)))
                table.Columns.Add(New DataColumn("VendorDealTypeCode", GetType(String)))
                table.Columns.Add(New DataColumn("VendorDealTypeDesc", GetType(String)))
                table.Columns.Add(New DataColumn("CostPromoCodeTypeID", GetType(Integer)))
                table.Columns.Add(New DataColumn("CostPromoCode", GetType(Integer)))
                table.Columns.Add(New DataColumn("CostPromoDesc", GetType(String)))
                table.Columns.Add(New DataColumn("InsertDate", GetType(Date)))
                table.Columns.Add(New DataColumn("NotStackable", GetType(Boolean)))

                While results.Read
                    row = table.NewRow

                    If results.GetValue(results.GetOrdinal("VendorDealHistoryID")).GetType IsNot GetType(DBNull) Then
                        row("VendorDealHistoryID") = results.GetInt32(results.GetOrdinal("VendorDealHistoryID"))
                    End If

                    row("ItemKey") = itemKey
                    row("VendorID") = vendorID

                    If results.GetValue(results.GetOrdinal("CaseQty")).GetType IsNot GetType(DBNull) Then
                        row("CaseQty") = results.GetInt32(results.GetOrdinal("CaseQty"))
                    End If

                    If results.GetValue(results.GetOrdinal("Package_Desc1")).GetType IsNot GetType(DBNull) Then
                        row("PackageDesc1") = results.GetDecimal(results.GetOrdinal("Package_Desc1"))
                    End If

                    If results.GetValue(results.GetOrdinal("CaseAmt")).GetType IsNot GetType(DBNull) Then
                        row("CaseAmt") = results.GetDecimal(results.GetOrdinal("CaseAmt"))
                    End If

                    If results.GetValue(results.GetOrdinal("CaseAmtType")).GetType IsNot GetType(DBNull) Then
                        row("CaseAmtType") = results.GetString(results.GetOrdinal("CaseAmtType"))
                    End If

                    If results.GetValue(results.GetOrdinal("StartDate")).GetType IsNot GetType(DBNull) Then
                        row("StartDate") = results.GetDateTime(results.GetOrdinal("StartDate"))
                    End If

                    If results.GetValue(results.GetOrdinal("EndDate")).GetType IsNot GetType(DBNull) Then
                        row("EndDate") = results.GetDateTime(results.GetOrdinal("EndDate"))
                    End If

                    If results.GetValue(results.GetOrdinal("VendorDealTypeID")).GetType IsNot GetType(DBNull) Then
                        row("VendorDealTypeID") = results.GetInt32(results.GetOrdinal("VendorDealTypeID"))
                    End If

                    If results.GetValue(results.GetOrdinal("VendorDealTypeCode")).GetType IsNot GetType(DBNull) Then
                        row("VendorDealTypeCode") = results.GetString(results.GetOrdinal("VendorDealTypeCode"))
                    End If

                    If results.GetValue(results.GetOrdinal("VendorDealTypeDesc")).GetType IsNot GetType(DBNull) Then
                        row("VendorDealTypeDesc") = results.GetString(results.GetOrdinal("VendorDealTypeDesc"))
                    End If

                    If results.GetValue(results.GetOrdinal("CostPromoCodeTypeID")).GetType IsNot GetType(DBNull) Then
                        row("CostPromoCodeTypeID") = results.GetInt32(results.GetOrdinal("CostPromoCodeTypeID"))
                    End If

                    If results.GetValue(results.GetOrdinal("CostPromoCode")).GetType IsNot GetType(DBNull) Then
                        row("CostPromoCode") = results.GetInt32(results.GetOrdinal("CostPromoCode"))
                    End If

                    If results.GetValue(results.GetOrdinal("CostPromoDesc")).GetType IsNot GetType(DBNull) Then
                        row("CostPromoDesc") = results.GetString(results.GetOrdinal("CostPromoDesc"))
                    End If

                    If results.GetValue(results.GetOrdinal("InsertDate")).GetType IsNot GetType(DBNull) Then
                        row("InsertDate") = results.GetDateTime(results.GetOrdinal("InsertDate"))
                    End If


                    If results.GetValue(results.GetOrdinal("NotStackable")).GetType IsNot GetType(DBNull) Then
                        row("NotStackable") = results.GetBoolean(results.GetOrdinal("NotStackable"))
                    End If

                    table.Rows.Add(row)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetVendorDeals Exit")

            Return table
        End Function

        ''' <summary>
        ''' user is creating new discounts/allowances --> returns potential cost conflicts with the vendorDeal data that is passed in; list of stores where
        ''' a resulting net cost will be less than or equal to zero.
        ''' </summary>
        ''' <param name="vendorDeal"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetNetCostConflicts(ByVal vendorDeal As VendorDealBO) As ArrayList

            logger.Debug("GetNetCostConflicts Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim storeInfo As StoreBO
            Dim storeList As New ArrayList
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = vendorDeal.ItemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Vendor_ID"
                currentParam.Value = vendorDeal.VendorID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreList"
                currentParam.Value = vendorDeal.StoreList
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreListSeparator"
                currentParam.Value = vendorDeal.StoreListSeparator
                currentParam.Type = DBParamType.Char
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "CaseAmt"
                currentParam.Value = vendorDeal.CaseAmt
                currentParam.Type = DBParamType.Money
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "CaseAmtType"
                currentParam.Value = vendorDeal.DealTypeBO.CaseAmtType
                currentParam.Type = DBParamType.Char
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StartDate"
                currentParam.Value = vendorDeal.StartDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "EndDate"
                currentParam.Value = vendorDeal.EndDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "VendorDealHistory_ID"
                currentParam.Value = vendorDeal.VendorDealHistoryID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "NotStackable"
                currentParam.Value = vendorDeal.NotStackable
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetProposedNetCostValues", paramList)

                While results.Read
                    storeInfo = New StoreBO()
                    storeInfo.StoreNo = results.GetInt32(results.GetOrdinal("Store_No"))
                    storeInfo.StoreName = results.GetString(results.GetOrdinal("Store_Name"))

                    storeList.Add(storeInfo)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetNetCostConflicts Exit")

            Return storeList
        End Function

        ''' <summary>
        ''' user is making a cost change --> returns potential cost conflicts with the vendorDeal data that is passed in; list of stores where
        ''' a resulting net cost will be less than or equal to zero.
        ''' </summary>
        ''' <param name="vendorDeal"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetNetCostConflicts_CostChange(ByVal vendorDeal As VendorDealBO) As ArrayList

            logger.Debug("GetNetCostConflicts_CostChange Entry")


            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim storeInfo As StoreBO
            Dim storeList As New ArrayList
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = vendorDeal.ItemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Vendor_ID"
                currentParam.Value = vendorDeal.VendorID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreList"
                currentParam.Value = vendorDeal.StoreList
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreListSeparator"
                currentParam.Value = vendorDeal.StoreListSeparator
                currentParam.Type = DBParamType.Char
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "NewCost"
                currentParam.Value = vendorDeal.CaseAmt
                currentParam.Type = DBParamType.Money
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StartDate"
                currentParam.Value = vendorDeal.StartDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetProposedNetCost_CostChange", paramList)

                While results.Read
                    storeInfo = New StoreBO()
                    storeInfo.StoreNo = results.GetInt32(results.GetOrdinal("Store_No"))
                    storeInfo.StoreName = results.GetString(results.GetOrdinal("Store_Name"))

                    storeList.Add(storeInfo)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetNetCostConflicts_CostChange Exit")

            Return storeList
        End Function

        ''' <summary>
        ''' returns the number of stackable conflicts with the vendorDeal data that is passed in;
        ''' only one deal marked "Not Stackable" can be created for any start/end date range
        ''' </summary>
        ''' <param name="vendorDeal"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetStackableConflicts(ByVal vendorDeal As VendorDealBO) As Boolean

            logger.Debug("GetStackableConflicts Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim isConflict As Boolean = False
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "VendorDealHistoryID"
                currentParam.Value = vendorDeal.VendorDealHistoryID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = vendorDeal.ItemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Vendor_ID"
                currentParam.Value = vendorDeal.VendorID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreList"
                currentParam.Value = vendorDeal.StoreList
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreListSeparator"
                currentParam.Value = vendorDeal.StoreListSeparator
                currentParam.Type = DBParamType.Char
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StartDate"
                currentParam.Value = vendorDeal.StartDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "EndDate"
                currentParam.Value = vendorDeal.EndDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetVendorDealHistoryStackableConflicts", paramList)

                If results.Read Then
                    If results.GetInt32(results.GetOrdinal("NumConflicts")) > 0 Then
                        isConflict = True
                    End If
                End If
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetStackableConflicts Exit")


            Return isConflict
        End Function

#End Region

#Region "write methods"

        ''' <summary>
        ''' Insert VendorDealHistory data
        ''' </summary>
        ''' <param name="vendorDeal"></param>
        ''' <remarks></remarks>
        Public Sub InsertVendorDeal(ByVal vendorDeal As VendorDealBO)

            logger.Debug("InsertVendorDeal Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = vendorDeal.ItemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Vendor_ID"
                currentParam.Value = vendorDeal.VendorID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreList"
                currentParam.Value = vendorDeal.StoreList
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreListSeparator"
                currentParam.Value = vendorDeal.StoreListSeparator
                currentParam.Type = DBParamType.Char
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "CaseQty"
                currentParam.Value = vendorDeal.CaseQty
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Package_Desc1"
                currentParam.Value = vendorDeal.PackageDesc1
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "CaseAmt"
                currentParam.Value = vendorDeal.CaseAmt
                currentParam.Type = DBParamType.Money
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StartDate"
                currentParam.Value = vendorDeal.StartDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "EndDate"
                currentParam.Value = vendorDeal.EndDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "TypeCode"
                currentParam.Value = vendorDeal.DealTypeBO.VendorDealTypeCode
                currentParam.Type = DBParamType.Char
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "FromVendor"
                currentParam.Value = vendorDeal.IsFromVendor
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "CostPromoCode"
                currentParam.Value = vendorDeal.CostPromoBO.CostPromoCode
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "NotStackable"
                currentParam.Value = vendorDeal.NotStackable
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("InsertVendorDealHistory", paramList)
            Catch ex As Exception
                Throw ex
            End Try

            logger.Debug("InsertVendorDeal Exit")
        End Sub

        ''' <summary>
        ''' Update VendorDealHistory data
        ''' </summary>
        ''' <param name="vendorDeal"></param>
        ''' <remarks></remarks>
        Public Sub UpdateVendorDeal(ByVal vendorDeal As VendorDealBO)

            logger.Debug("UpdateVendorDeal Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "VendorDealHistoryID"
                currentParam.Value = vendorDeal.VendorDealHistoryID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "CaseQty"
                currentParam.Value = vendorDeal.CaseQty
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "CaseAmt"
                currentParam.Value = vendorDeal.CaseAmt
                currentParam.Type = DBParamType.Money
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StartDate"
                currentParam.Value = vendorDeal.StartDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "EndDate"
                currentParam.Value = vendorDeal.EndDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "NotStackable"
                currentParam.Value = vendorDeal.NotStackable
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("UpdateVendorDealHistory", paramList)
            Catch ex As Exception
                Throw ex
            End Try

            logger.Debug("UpdateVendorDeal Exit")
        End Sub

        ''' <summary>
        ''' Delete VendorDealHistory data
        ''' </summary>
        ''' <param name="vendorDeal"></param>
        ''' <remarks></remarks>
        Public Sub DeleteVendorDeal(ByVal vendorDeal As VendorDealBO)

            logger.Debug("DeleteVendorDeal Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "VendorDealHistoryID"
                currentParam.Value = vendorDeal.VendorDealHistoryID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("DeleteVendorDealHistory", paramList)
            Catch ex As Exception
                Throw ex
            End Try
            logger.Debug("DeleteVendorDeal Exit")
        End Sub

#End Region

    End Class

End Namespace
