

Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.DataAccess

    ''' <summary>
    ''' Generated Data Access Object base class for the PriceChgType db table.
    '''
    ''' THIS FILE WAS CODE GENERATED - DO NOT MODIFY!
    '''
    ''' Created By:	David Marine
    ''' Created   :	Mar 27, 2007
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PriceChgTypeDAORegen

        ''' <summary>
        ''' Returns a list of PriceChgTypes from the provided resultset.
        ''' </summary>
        ''' <returns>ArrayList of PriceChgTypes</returns>
        ''' <remarks></remarks>
        Protected Overridable Function GetPriceChgTypesFromResultSet(ByVal results As SqlDataReader) As BusinessObjectCollection

            Dim businessObjectTable As New BusinessObjectCollection
            Dim businessObject As PriceChgType

            While results.Read

                businessObject = New PriceChgType()

                businessObject.PriceChgTypeID = results.GetByte(results.GetOrdinal("PriceChgTypeID"))
                businessObject.PriceChgTypeDesc = results.GetString(results.GetOrdinal("PriceChgTypeDesc"))
                businessObject.Priority = results.GetInt16(results.GetOrdinal("Priority"))
                businessObject.OnSale = results.GetBoolean(results.GetOrdinal("On_Sale"))
                businessObject.MSRPRequired = results.GetBoolean(results.GetOrdinal("MSRP_Required"))
                businessObject.LineDrive = results.GetBoolean(results.GetOrdinal("LineDrive"))

                ' reset all the flags
                businessObject.IsNew = False
                businessObject.IsDirty = False
                businessObject.IsMarkedForDelete = False
                businessObject.IsDeleted = False

                ' add business object to BusinessObjectCollection with abstracted PK key
                businessObjectTable.Add(businessObject.PrimaryKey, businessObject)

            End While

            Trace.WriteLine("Loading " + businessObjectTable.Count.ToString() + " PriceChgTypes from the database.")

            Return businessObjectTable

        End Function

        ''' <summary>
        ''' Set this in the PriceChgTypeDAO sub class to change the stored
        ''' procedure used by the GetAllPriceChgTypes function.
        ''' </summary>
        Public GetAllPriceChgTypesStoredProcName As String = "EIM_Regen_GetAllPriceChgTypes"

        ''' <summary>
        ''' Returns all PriceChgTypes.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function GetAllPriceChgTypes() As BusinessObjectCollection

            Dim businessObjectTable As New BusinessObjectCollection
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim results As SqlDataReader = Nothing

            Try

                ' get the current transaction if any
                Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction

                ' Execute the stored procedure
                If Not IsNothing(theTransaction) Then
                    results = factory.GetStoredProcedureDataReader(Me.GetAllPriceChgTypesStoredProcName, paramList, theTransaction.SqlTransaction)
                Else
                    results = factory.GetStoredProcedureDataReader(Me.GetAllPriceChgTypesStoredProcName, paramList)
                End If

                businessObjectTable = GetPriceChgTypesFromResultSet(results)

            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return businessObjectTable

        End Function


        ''' <summary>
        ''' Set this in the PriceChgTypeDAO sub class to change the stored
        ''' procedure used by the GetPriceChgTypeByPK function.
        ''' </summary>
        Protected GetPriceChgTypeByPKStoredProcName As String = "EIM_Regen_GetPriceChgTypeByPK"

        ''' <summary>
        ''' Returns zero or more PriceChgTypes by PK value.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function GetPriceChgTypeByPK(ByRef priceChgTypeID As System.Byte) As PriceChgType


            Dim businessObjectTable As New BusinessObjectCollection
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            Try
                ' setup identifier for stored proc
                currentParam = New DBParam
                currentParam.Name = "PriceChgTypeID"
                currentParam.Type = DBParamType.SmallInt
                currentParam.Value = priceChgTypeID
                paramList.Add(currentParam)

                ' get the current transaction if any
                Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction

                ' Execute the stored procedure
                If Not IsNothing(theTransaction) Then
                    results = factory.GetStoredProcedureDataReader(Me.GetPriceChgTypeByPKStoredProcName, paramList, theTransaction.SqlTransaction)
                Else
                    results = factory.GetStoredProcedureDataReader(Me.GetPriceChgTypeByPKStoredProcName, paramList)
                End If

                businessObjectTable = GetPriceChgTypesFromResultSet(results)

            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Dim businessObject As PriceChgType = Nothing
            If businessObjectTable.Count > 0 Then
                businessObject = CType(businessObjectTable.Item(0), PriceChgType)
            End If

            Return businessObject

        End Function


        ''' <summary>
        ''' Set this in the PriceChgTypeDAO sub class to change the stored
        ''' procedure used by the GetAllPriceChgTypes function.
        ''' </summary>
        Public InsertPriceChgTypeStoredProcName As String = "EIM_Regen_InsertPriceChgType"

        Public Overridable Sub InsertPriceChgType(ByRef businessObject As PriceChgType)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim pkValueList As New ArrayList

            ' setup parameters for stored proc

            ' PriceChgTypeDesc
            currentParam = New DBParam
            currentParam.Name = "PriceChgTypeDesc"
            currentParam.Type = DBParamType.String
            currentParam.Value = businessObject.PriceChgTypeDesc

            paramList.Add(currentParam)

            ' Priority
            currentParam = New DBParam
            currentParam.Name = "Priority"
            currentParam.Type = DBParamType.SmallInt
            currentParam.Value = businessObject.Priority

            paramList.Add(currentParam)

            ' On_Sale
            currentParam = New DBParam
            currentParam.Name = "On_Sale"
            currentParam.Type = DBParamType.Bit
            currentParam.Value = businessObject.OnSale

            paramList.Add(currentParam)

            ' MSRP_Required
            currentParam = New DBParam
            currentParam.Name = "MSRP_Required"
            currentParam.Type = DBParamType.Bit
            currentParam.Value = businessObject.MSRPRequired

            paramList.Add(currentParam)

            ' LineDrive
            currentParam = New DBParam
            currentParam.Name = "LineDrive"
            currentParam.Type = DBParamType.Bit
            currentParam.Value = businessObject.LineDrive

            paramList.Add(currentParam)

            ' Competetive
            currentParam = New DBParam
            currentParam.Name = "Competetive"
            currentParam.Type = DBParamType.Bit
            currentParam.Value = businessObject.Competetive

            paramList.Add(currentParam)

            'PriceChgTypeID is an output parameter of the stored procedure
            currentParam = New DBParam
            currentParam.Name = "PriceChgTypeID"
            currentParam.Type = DBParamType.Int

            paramList.Add(currentParam)


            ' get the current transaction if any
            Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction

            ' Execute the stored procedure
            If Not IsNothing(theTransaction) Then
                pkValueList = factory.ExecuteStoredProcedure(InsertPriceChgTypeStoredProcName, paramList, theTransaction.SqlTransaction)
            Else
                pkValueList = factory.ExecuteStoredProcedure(InsertPriceChgTypeStoredProcName, paramList)
            End If


        End Sub

        ''' <summary>
        ''' Set this in the PriceChgTypeDAO sub class to change the stored
        ''' procedure used by the GetAllPriceChgTypes function.
        ''' </summary>
        Public UpdatePriceChgTypeStoredProcName As String = "EIM_Regen_UpdatePriceChgType"

        Public Overridable Function UpdatePriceChgType(ByVal businessObject As PriceChgType) As Integer

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim outputList As New ArrayList

            ' setup parameters for stored proc

            ' PriceChgTypeID
            currentParam = New DBParam
            currentParam.Name = "PriceChgTypeID"
            currentParam.Type = DBParamType.SmallInt
            currentParam.Value = businessObject.PriceChgTypeID

            paramList.Add(currentParam)

            ' PriceChgTypeDesc
            currentParam = New DBParam
            currentParam.Name = "PriceChgTypeDesc"
            currentParam.Type = DBParamType.String
            currentParam.Value = businessObject.PriceChgTypeDesc

            paramList.Add(currentParam)

            ' Priority
            currentParam = New DBParam
            currentParam.Name = "Priority"
            currentParam.Type = DBParamType.SmallInt
            currentParam.Value = businessObject.Priority

            paramList.Add(currentParam)

            ' On_Sale
            currentParam = New DBParam
            currentParam.Name = "On_Sale"
            currentParam.Type = DBParamType.Bit
            currentParam.Value = businessObject.OnSale

            paramList.Add(currentParam)

            ' MSRP_Required
            currentParam = New DBParam
            currentParam.Name = "MSRP_Required"
            currentParam.Type = DBParamType.Bit
            currentParam.Value = businessObject.MSRPRequired

            paramList.Add(currentParam)

            ' LineDrive
            currentParam = New DBParam
            currentParam.Name = "LineDrive"
            currentParam.Type = DBParamType.Bit
            currentParam.Value = businessObject.LineDrive

            paramList.Add(currentParam)

            ' Competetive
            currentParam = New DBParam
            currentParam.Name = "Competetive"
            currentParam.Type = DBParamType.Bit
            currentParam.Value = businessObject.Competetive

            paramList.Add(currentParam)

            ' Get the output value
            currentParam = New DBParam
            currentParam.Name = "UpdateCount"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' get the current transaction if any
            Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction

            ' Execute the stored procedure
            If Not IsNothing(theTransaction) Then
                outputList = factory.ExecuteStoredProcedure(UpdatePriceChgTypeStoredProcName, paramList, theTransaction.SqlTransaction)
            Else
                outputList = factory.ExecuteStoredProcedure(UpdatePriceChgTypeStoredProcName, paramList)
            End If

            Return CInt(outputList(0))

        End Function

        ''' <summary>
        ''' Set this in the PriceChgTypeDAO sub class to change the stored
        ''' procedure used by the GetAllPriceChgTypes function.
        ''' </summary>
        Public DeletePriceChgTypeStoredProcName As String = "EIM_Regen_DeletePriceChgType"

        Public Overridable Function DeletePriceChgType(ByVal businessObject As PriceChgType) As Integer

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim outputList As New ArrayList

            ' setup parameters for stored proc

            ' PriceChgTypeID
            currentParam = New DBParam
            currentParam.Name = "PriceChgTypeID"
            currentParam.Type = DBParamType.SmallInt
            currentParam.Value = businessObject.PriceChgTypeID

            paramList.Add(currentParam)

            ' Get the output value
            currentParam = New DBParam
            currentParam.Name = "DeleteCount"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' get the current transaction if any
            Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction

            ' Execute the stored procedure
            If Not IsNothing(theTransaction) Then
                outputList = factory.ExecuteStoredProcedure(DeletePriceChgTypeStoredProcName, paramList, theTransaction.SqlTransaction)
            Else
                outputList = factory.ExecuteStoredProcedure(DeletePriceChgTypeStoredProcName, paramList)
            End If

            Return CInt(outputList(0))

        End Function

    End Class

End Namespace
