Imports log4net
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.InvoiceThirdPartyFreight.BusinessLogic
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.InvoiceThirdPartyFreight.DataAccess
    Public Class ThirdPartyFreightDAO
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#Region "Read Functions"
        ''' <summary>
        ''' Read a single record from the OrderInvoice_Freight3Party table, by OrderHeaderID
        ''' </summary>
        ''' <param name="OrderHeaderID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Get3PartyFreightInvoice(ByVal OrderHeaderID As Integer) As ThirdPartyFreightInvoiceBO
            logger.Debug("Get3PartyFreightInvoice entry: OrderHeaderID=" + OrderHeaderID.ToString)
            Dim Invoice As ThirdPartyFreightInvoiceBO = Nothing
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "OrderHeader_ID"
                currentParam.Value = OrderHeaderID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute Stored Procedure to update the data
                results = factory.GetStoredProcedureDataReader("dbo.OrderInvoice_3PartyFreightInvoice_Get", paramList)

                ' Create a new ControlGroupBO object with the current data
                If results.HasRows Then
                    While results.Read
                        Invoice = New ThirdPartyFreightInvoiceBO(results)
                    End While
                End If
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("Get3PartyFreightInvoice exit")
            Return Invoice
        End Function

        ''' <summary>
        ''' Check to see if the vendor id and vendor key input values correspond to a matching record
        ''' in the vendor table.
        ''' </summary>
        ''' <param name="vendorId"></param>
        ''' <param name="vendorKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CheckForVendorIdAndVendorKeyMatch(ByVal vendorId As Integer, ByVal vendorKey As String) As Boolean
            logger.Debug("CheckForVendorIdAndVendorKeyMatch entry: vendorId=" + vendorId.ToString() + ", vendorKey=" + vendorKey)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim outputList As ArrayList
            Dim vendorMatch As Boolean

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Vendor_ID"
            If vendorId <> -1 Then
                currentParam.Value = vendorId
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Vendor_Key"
            currentParam.Value = vendorKey
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            ' -- output --
            currentParam = New DBParam
            currentParam.Name = "VendorMatch"
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            outputList = factory.ExecuteStoredProcedure("dbo.CheckVendorIdAndVendorKey", paramList)
            vendorMatch = CBool(outputList(0))
            logger.Debug("CheckForVendorIdAndVendorKeyMatch exit: vendorMatch=" + vendorMatch.ToString)
            Return vendorMatch
        End Function

        ''' <summary>
        ''' Check to see if the vendor id and vendor key input values correspond to a matching record
        ''' in the vendor table.
        ''' </summary>
        ''' <param name="vendorId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function IsValidVendor(ByVal vendorId As Integer) As Boolean
            logger.Debug("IsValidVendor entry: vendorId=" + vendorId.ToString())

            'Dim Invoice As ThirdPartyFreightInvoiceBO = Nothing
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing
            Dim ValidVendor As Boolean = True

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Vendor_ID"
                currentParam.Value = vendorId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute Stored Procedure to get the vendor
                results = factory.GetStoredProcedureDataReader("dbo.GetVendorInfo", paramList)

                ' Check results for a valid vendor (only zero or one record can be returned)
                If results.HasRows Then
                    While results.Read
                        If results.IsDBNull(results.GetOrdinal("PS_Export_Vendor_ID")) Then
                            ValidVendor = False
                        Else
                            ValidVendor = True
                        End If
                    End While
                Else
                    ValidVendor = False
                End If
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("IsValidVendor exit: vendorId=" + vendorId.ToString())
            Return ValidVendor
        End Function

        ''' <summary>
        ''' Check to see how many records exist in the Vendor table that match the input vendor key.
        ''' If only one record is found, the corresponding vendor id is also returned.
        ''' </summary>
        ''' <param name="vendorKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetVendorCountByVendorKey(ByVal vendorKey As String) As ArrayList
            logger.Debug("GetVendorCountByVendorKey entry: vendorKey=" + vendorKey)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim outputList As ArrayList

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Vendor_Key"
            currentParam.Value = vendorKey
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            ' -- output --
            currentParam = New DBParam
            currentParam.Name = "VendorCount"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "VendorId"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            outputList = factory.ExecuteStoredProcedure("dbo.GetVendorCountByVendorKey", paramList)
            logger.Debug("GetVendorCountByVendorKey exit")
            Return outputList
        End Function

        ''' <summary>
        ''' Check to see how many records exist in the Vendor table that match the input vendor key.
        ''' If only one record is found, the corresponding vendor id is also returned.
        ''' </summary>
        ''' <param name="Invoice"></param>
        ''' <returns>True if there is an existing vendor invoice number, False otherwise</returns>
        ''' <remarks></remarks>
        Public Shared Function ExistingVendorInvoiceNum(ByRef Invoice As ThirdPartyFreightInvoiceBO) As Boolean
            logger.Debug("ExistingVendorInvoiceNum entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim outputList As ArrayList
            Dim RecordCount As Integer

            ' setup parameters for stored proc

            currentParam = New DBParam
            currentParam.Name = "Vendor_ID"
            currentParam.Value = Invoice.VendorID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceNumber"
            currentParam.Value = Invoice.InvoiceNum
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ThisOrderHeader_ID"
            currentParam.Value = Invoice.PONum
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' -- output --
            currentParam = New DBParam
            currentParam.Name = "RecordCount"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute Stored Procedure 
            outputList = factory.ExecuteStoredProcedure("dbo.GetThirdPartyInvoiceNumberUse", paramList)
            RecordCount = CInt(outputList(0))

            logger.Debug("ExistingVendorInvoiceNum exit")

            Return Not (RecordCount = 0)
        End Function
#End Region

#Region "Update Functions"

        ''' <summary>
        ''' Add a new invoice to a control group.
        ''' </summary>
        ''' <param name="ThirdPartyFreightInvoice"></param>
        ''' <remarks></remarks>
        Public Shared Function CreateThirdPartyFreightInvoice(ByRef ThirdPartyFreightInvoice As ThirdPartyFreightInvoiceBO) As Integer
            logger.Debug("CreateThirdPartyFreightInvoice entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim outputList As ArrayList
            Dim validationCode As Integer

            ' setup parameters for stored proc

            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = ThirdPartyFreightInvoice.PONum
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceNumber"
            currentParam.Value = ThirdPartyFreightInvoice.InvoiceNum
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceDate"
            currentParam.Value = ThirdPartyFreightInvoice.InvoiceDate
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceCost"
            currentParam.Value = ThirdPartyFreightInvoice.InvoiceCost
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Vendor_ID"
            currentParam.Value = ThirdPartyFreightInvoice.VendorID
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "MatchingUser_ID"
            currentParam.Value = ThirdPartyFreightInvoice.UpdateUserId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' -- output --
            currentParam = New DBParam
            currentParam.Name = "MatchingValidationCode"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute Stored Procedure to update the data
            outputList = factory.ExecuteStoredProcedure("dbo.InsertThirdPartyFreightInvoice", paramList)
            validationCode = CInt(outputList(0))

            logger.Debug("CreateThirdPartyFreightInvoice exit: ValidationCode=" + validationCode.ToString())
            Return validationCode
        End Function

        ''' <summary>
        ''' Update an existing invoice assigned to a control group.
        ''' </summary>
        ''' <param name="ThirdPartyFreightInvoice"></param>
        ''' <remarks></remarks>
        Public Shared Function UpdateThirdPartyFreightInvoice(ByRef ThirdPartyFreightInvoice As ThirdPartyFreightInvoiceBO) As Integer
            logger.Debug("UpdateThirdPartyFreightInvoice entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim outputList As ArrayList
            Dim validationCode As Integer

            ' setup parameters for stored proc

            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = ThirdPartyFreightInvoice.PONum
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceCost"
            currentParam.Value = ThirdPartyFreightInvoice.InvoiceCost
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceDate"
            currentParam.Value = ThirdPartyFreightInvoice.InvoiceDate
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceNumber"
            currentParam.Value = ThirdPartyFreightInvoice.InvoiceNum
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Vendor_ID"
            currentParam.Value = ThirdPartyFreightInvoice.VendorID
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "MatchingUser_ID"
            currentParam.Value = ThirdPartyFreightInvoice.UpdateUserId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' -- output --
            currentParam = New DBParam
            currentParam.Name = "MatchingValidationCode"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute Stored Procedure to update the data
            outputList = factory.ExecuteStoredProcedure("dbo.OrderInvoice_3PartyFreightInvoice_Update", paramList)
            validationCode = CInt(outputList(0))

            logger.Debug("UpdateThirdPartyFreightInvoice exit: ValidationCode=" + validationCode.ToString())
            Return validationCode
        End Function

 
#End Region
    End Class
End Namespace
