Imports log4net
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.InvoiceControlGroup.BusinessLogic
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.InvoiceControlGroup.DataAccess
    Public Class ControlGroupDAO
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#Region "Read Functions"
        ''' <summary>
        ''' Read a single record from the control group table, by control group ID.
        ''' </summary>
        ''' <param name="controlGroupID"></param>
        ''' <param name="populateOrders">TRUE if the orders should also be populated for the control group</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetControlGroupDetails(ByVal controlGroupID As Integer, ByVal populateOrders As Boolean) As ControlGroupBO
            logger.Debug("GetControlGroupDetails entry: controlGroupID=" + controlGroupID.ToString + ", populateOrders=" + populateOrders.ToString())
            Dim controlGroup As ControlGroupBO = Nothing
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "OrderInvoice_ControlGroup_ID"
                currentParam.Value = controlGroupID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute Stored Procedure to update the data
                results = factory.GetStoredProcedureDataReader("dbo.OrderInvoice_GetControlGroup", paramList)

                ' Create a new ControlGroupBO object with the current data
                If results.HasRows Then
                    While results.Read
                        controlGroup = New ControlGroupBO(results)
                    End While
                End If
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            ' Update the orders for the control group
            If populateOrders AndAlso controlGroup IsNot Nothing Then
                GetControlGroupInvoices(controlGroup)
            End If

            logger.Debug("GetControlGroupDetails exit")
            Return controlGroup
        End Function

        ''' <summary>
        ''' Searches for control groups that match the input parameters supplied by the user.
        ''' All input parameters are optional.
        ''' </summary>
        ''' <param name="controlGroupStatus"></param>
        ''' <param name="controlGroupID"></param>
        ''' <returns>ArrayList of ControlGroupBO objects that match the search criteria</returns>
        ''' <remarks></remarks>
        Public Shared Function SearchForControlGroups(ByVal controlGroupStatus As Integer, ByVal controlGroupID As Integer) As ArrayList
            logger.Debug("SearchForControlGroups entry: controlGroupStatus=" + controlGroupStatus.ToString() + ", controlGroupID=" + controlGroupID.ToString())
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing
            Dim returnGroups As New ArrayList

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "OrderInvoice_ControlGroup_ID"
                If controlGroupID <> -1 Then
                    currentParam.Value = controlGroupID
                Else
                    currentParam.Value = DBNull.Value
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "OrderInvoice_ControlGroupStatus_ID"
                If controlGroupStatus <> -1 Then
                    currentParam.Value = controlGroupStatus
                Else
                    currentParam.Value = DBNull.Value
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute Stored Procedure to update the data
                results = factory.GetStoredProcedureDataReader("dbo.OrderInvoice_GetControlGroupSearch", paramList)

                ' Update the return array list with ControlGroupBO objects
                While results.Read
                    returnGroups.Add(New ControlGroupBO(results))
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try
            logger.Debug("SearchForControlGroups exit: returnGroups.Count=" + returnGroups.Count.ToString())
            Return returnGroups
        End Function

        ''' <summary>
        ''' Read all of the control group invoices associated with the given control group,
        ''' and populate the invoice array list in the ControlGroupBO with the invoice data.
        ''' </summary>
        ''' <param name="controlGroup"></param>
        ''' <remarks></remarks>
        Public Shared Sub GetControlGroupInvoices(ByRef controlGroup As ControlGroupBO)
            logger.Debug("GetControlGroupInvoices entry: controlGroupID=" + controlGroup.ControlGroupId.ToString())
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            Try
                If controlGroup IsNot Nothing Then
                    ' setup parameters for stored proc
                    currentParam = New DBParam
                    currentParam.Name = "OrderInvoice_ControlGroup_ID"
                    currentParam.Value = controlGroup.ControlGroupId
                    currentParam.Type = DBParamType.Int
                    paramList.Add(currentParam)

                    ' Execute Stored Procedure to update the data
                    results = factory.GetStoredProcedureDataReader("dbo.OrderInvoice_GetControlGroupInvoices", paramList)

                    ' Update the orders array in the ControlGroupBO with the data being returned
                    While results.Read
                        controlGroup.AddInvoiceToOrder(New ControlGroupInvoiceBO(results))
                    End While
                End If
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try
            logger.Debug("GetControlGroupInvoices exit")
        End Sub

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

        Public Shared Function CheckInvoiceExistsInCurrentControlGroup(ByRef controlGroupInvoice As ControlGroupInvoiceBO, ByVal controlGroupID As Integer) As Boolean
            logger.Debug("CheckInvoiceExistsInCurrentControlGroup entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim outputList As ArrayList
            Dim doesExist As Boolean

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = controlGroupInvoice.PONum
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "OrderInvoice_ControlGroup_ID"
            currentParam.Value = controlGroupID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceType"
            If controlGroupInvoice.InvoiceType = ControlGroupInvoiceType.Vendor Then
                currentParam.Value = 1
            Else
                currentParam.Value = 2
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' -- output --
            currentParam = New DBParam
            currentParam.Name = "DoesExist"
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            outputList = factory.ExecuteStoredProcedure("dbo.OrderInvoice_CheckInvoiceExistsInCurrentControlGroup", paramList)
            doesExist = CBool(outputList(0))
            logger.Debug("CheckInvoiceExistsInCurrentControlGroup exit: doesExist=" + doesExist.ToString)
            Return doesExist
        End Function

        Public Shared Function CheckInvoiceExistsInOpenControlGroup(ByRef controlGroupInvoice As ControlGroupInvoiceBO, ByVal controlGroupID As Integer) As Boolean
            logger.Debug("CheckInvoiceExistsInOpenControlGroup entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim outputList As ArrayList
            Dim doesExist As Boolean

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = controlGroupInvoice.PONum
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "OrderInvoice_ControlGroup_ID"
            currentParam.Value = controlGroupID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceType"
            If controlGroupInvoice.InvoiceType = ControlGroupInvoiceType.Vendor Then
                currentParam.Value = 1
            Else
                currentParam.Value = 2
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' -- output --
            currentParam = New DBParam
            currentParam.Name = "DoesExist"
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            outputList = factory.ExecuteStoredProcedure("dbo.OrderInvoice_CheckInvoiceExistsInOpenControlGroup", paramList)
            doesExist = CBool(outputList(0))
            logger.Debug("CheckInvoiceExistsInOpenControlGroup exit: doesExist=" + doesExist.ToString)
            Return doesExist
        End Function

#End Region

#Region "Update Functions"
        ''' <summary>
        ''' Creates a new control group, updating the ControlGroupId value for the 
        ''' ControlGroupBO that is passed into the stored proc with the value of the
        ''' OrderInvoice_ControlGroup_ID for the newly created record.
        ''' </summary>
        ''' <param name="controlGroup"></param>
        ''' <remarks></remarks>
        Public Shared Sub CreateControlGroup(ByRef controlGroup As ControlGroupBO)
            logger.Debug("CreateControlGroup entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim outputList As ArrayList

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "ExpectedGrossAmt"
            If controlGroup.ExpectedGrossAmt = -1 Then
                currentParam.Value = 0
            Else
                currentParam.Value = controlGroup.ExpectedGrossAmt
            End If
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ExpectedInvoiceCount"
            If controlGroup.ExpectedInvoiceCount = -1 Then
                currentParam.Value = 0
            Else
                currentParam.Value = controlGroup.ExpectedInvoiceCount
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UpdateUser_ID"
            currentParam.Value = controlGroup.UpdateUserId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' -- output --
            currentParam = New DBParam
            currentParam.Name = "OrderInvoice_ControlGroup_ID"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            outputList = factory.ExecuteStoredProcedure("dbo.OrderInvoice_InsertControlGroup", paramList)
            controlGroup.ControlGroupId = CInt(outputList(0))

            logger.Debug("CreateControlGroup exit: OrderInvoice_ControlGroup_ID = " + controlGroup.ControlGroupId.ToString)
        End Sub

        ''' <summary>
        ''' Update the data for an existing control group.
        ''' The control group must be in the OPEN status for the update to be applied.
        ''' </summary>
        ''' <param name="controlGroup"></param>
        ''' <remarks></remarks>
        Public Shared Sub UpdateControlGroup(ByRef controlGroup As ControlGroupBO)
            logger.Debug("UpdateControlGroup entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderInvoice_ControlGroup_ID"
            currentParam.Value = controlGroup.ControlGroupId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ExpectedGrossAmt"
            currentParam.Value = controlGroup.ExpectedGrossAmt
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ExpectedInvoiceCount"
            currentParam.Value = controlGroup.ExpectedInvoiceCount
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UpdateUser_ID"
            currentParam.Value = controlGroup.UpdateUserId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute Stored Procedure to update the data
            factory.ExecuteStoredProcedure("dbo.OrderInvoice_UpdateControlGroup", paramList)

            logger.Debug("UpdateControlGroup exit")
        End Sub

        ''' <summary>
        ''' Change the state of a control group from OPEN to CLOSED.
        ''' </summary>
        ''' <param name="controlGroup"></param>
        ''' <remarks></remarks>
        Public Shared Sub CloseControlGroup(ByRef controlGroup As ControlGroupBO)
            logger.Debug("CloseControlGroup entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim outputList As ArrayList

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderInvoice_ControlGroup_ID"
            currentParam.Value = controlGroup.ControlGroupId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ExpectedGrossAmt"
            currentParam.Value = controlGroup.ExpectedGrossAmt
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ExpectedInvoiceCount"
            currentParam.Value = controlGroup.ExpectedInvoiceCount
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UpdateUser_ID"
            currentParam.Value = controlGroup.UpdateUserId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' -- output --
            currentParam = New DBParam
            currentParam.Name = "Error_No"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "OutputText"
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            ' Execute Stored Procedure to update the data
            outputList = factory.ExecuteStoredProcedure("dbo.OrderInvoice_CloseControlGroup", paramList)
            controlGroup.CloseStatus = CInt(outputList(0))
            controlGroup.CloseErrorMsg = outputList(1).ToString()

            logger.Debug("CloseControlGroup exit")
        End Sub

        ''' <summary>
        ''' Add a new invoice to a control group.
        ''' </summary>
        ''' <param name="controlGroupInvoice"></param>
        ''' <param name="controlGroupID"></param>
        ''' <remarks></remarks>
        Public Shared Function CreateControlGroupInvoice(ByRef controlGroupInvoice As ControlGroupInvoiceBO, ByVal controlGroupID As Integer) As Integer
            logger.Debug("CreateControlGroupInvoice entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim outputList As ArrayList
            Dim validationCode As Integer

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderInvoice_ControlGroup_ID"
            currentParam.Value = controlGroupID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceType"
            If controlGroupInvoice.InvoiceType = ControlGroupInvoiceType.Vendor Then
                currentParam.Value = 1
            Else
                currentParam.Value = 2
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = controlGroupInvoice.PONum
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Return_Order"
            currentParam.Value = controlGroupInvoice.CreditInv
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceCost"
            currentParam.Value = controlGroupInvoice.InvoiceCost
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceFreight"
            currentParam.Value = controlGroupInvoice.InvoiceFreight
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceDate"
            currentParam.Value = controlGroupInvoice.InvoiceDate
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceNumber"
            currentParam.Value = controlGroupInvoice.InvoiceNum
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Vendor_ID"
            currentParam.Value = controlGroupInvoice.VendorID
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UpdateUser_ID"
            If controlGroupInvoice.UpdateUserId <> -1 Then
                currentParam.Value = controlGroupInvoice.UpdateUserId
            Else
                currentParam.Value = giUserID
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' -- output --
            currentParam = New DBParam
            currentParam.Name = "ValidationCode"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute Stored Procedure to update the data
            outputList = factory.ExecuteStoredProcedure("dbo.OrderInvoice_InsertControlGroupInvoice", paramList)
            validationCode = CInt(outputList(0))

            logger.Debug("CreateControlGroupInvoice exit: ValidationCode=" + validationCode.ToString())
            Return validationCode
        End Function

        ''' <summary>
        ''' Update an existing invoice assigned to a control group.
        ''' </summary>
        ''' <param name="controlGroupInvoice"></param>
        ''' <param name="controlGroupID"></param>
        ''' <remarks></remarks>
        Public Shared Function UpdateControlGroupInvoice(ByRef controlGroupInvoice As ControlGroupInvoiceBO, ByVal controlGroupID As Integer) As Integer
            logger.Debug("UpdateControlGroupInvoice entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim outputList As ArrayList
            Dim validationCode As Integer

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderInvoice_ControlGroup_ID"
            currentParam.Value = controlGroupID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceType"
            If controlGroupInvoice.InvoiceType = ControlGroupInvoiceType.Vendor Then
                currentParam.Value = 1
            Else
                currentParam.Value = 2
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = controlGroupInvoice.PONum
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Return_Order"
            currentParam.Value = controlGroupInvoice.CreditInv
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceCost"
            currentParam.Value = controlGroupInvoice.InvoiceCost
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceFreight"
            currentParam.Value = controlGroupInvoice.InvoiceFreight
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceDate"
            currentParam.Value = controlGroupInvoice.InvoiceDate
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceNumber"
            currentParam.Value = controlGroupInvoice.InvoiceNum
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Vendor_ID"
            currentParam.Value = controlGroupInvoice.VendorID
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UpdateUser_ID"
            If controlGroupInvoice.UpdateUserId <> -1 Then
                currentParam.Value = controlGroupInvoice.UpdateUserId
            Else
                currentParam.Value = giUserID
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' -- output --
            currentParam = New DBParam
            currentParam.Name = "ValidationCode"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute Stored Procedure to update the data
            outputList = factory.ExecuteStoredProcedure("dbo.OrderInvoice_UpdateControlGroupInvoice", paramList)
            validationCode = CInt(outputList(0))

            logger.Debug("UpdateControlGroupInvoice exit: ValidationCode=" + validationCode.ToString())
            Return validationCode
        End Function

        ''' <summary>
        ''' Add a new invoice to a control group.
        ''' </summary>
        ''' <param name="controlGroupInvoice"></param>
        ''' <param name="controlGroupID"></param>
        ''' <remarks></remarks>
        Public Shared Sub DeleteControlGroupInvoice(ByRef controlGroupInvoice As ControlGroupInvoiceBO, ByVal controlGroupID As Integer)
            logger.Debug("DeleteControlGroupInvoice entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderInvoice_ControlGroup_ID"
            currentParam.Value = controlGroupID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceType"
            If controlGroupInvoice.InvoiceType = ControlGroupInvoiceType.Vendor Then
                currentParam.Value = 1
            Else
                currentParam.Value = 2
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = controlGroupInvoice.PONum
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute Stored Procedure to update the data
            factory.ExecuteStoredProcedure("dbo.OrderInvoice_DeleteControlGroupInvoice", paramList)

            logger.Debug("DeleteControlGroupInvoice exit")
        End Sub
#End Region
    End Class
End Namespace
