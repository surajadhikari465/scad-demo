

Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.DataAccess

    ''' <summary>
    ''' Generated Data Access Object class for the UploadRow db table.
    '''
    ''' This class inherits the following CRUD methods from its regenerable base class:
    '''    1. Find method by PK
    '''    2. Find method for each FK
    '''    3. Insert Method
    '''    4. Update Method
    '''    5. Delete Method
    '''
    ''' MODIFY THIS CLASS, NOT THE BASE CLASS.
    '''
    ''' Created By:	David Marine
    ''' Created   :	Feb 12, 2007
    ''' </summary>
    ''' <remarks></remarks>
    Public Class UploadRowDAO
        Inherits UploadRowDAORegen

#Region "Static Singleton Accessor"

        Private Shared _instance As UploadRowDAO = Nothing

        Public Shared ReadOnly Property Instance() As UploadRowDAO
            Get
                If IsNothing(_instance) Then
                    _instance = New UploadRowDAO()
                End If

                Return _instance
            End Get
        End Property

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Returns the item key for the provided item identifier.
        ''' </summary>
        ''' <returns>Integer</returns>
        ''' <remarks></remarks>
        Public Overridable Function GetItemKeyByIdentifier(ByVal identifier As String) As Integer

            Dim itemKey As Integer = -1
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            If Not IsNothing(identifier) And Not String.IsNullOrEmpty(identifier) Then
                Try
                    ' setup identifier for stored proc
                    currentParam = New DBParam
                    currentParam.Name = "Identifier"
                    currentParam.Type = DBParamType.String
                    currentParam.Value = identifier
                    paramList.Add(currentParam)

                    ' Execute the stored procedure 
                    results = factory.GetStoredProcedureDataReader("GetItemInfoByIdentifier", paramList)

                    If results.Read Then
                        itemKey = results.GetInt32(results.GetOrdinal("Item_Key"))
                    End If

                Finally
                    If results IsNot Nothing Then
                        results.Close()
                    End If
                End Try
            End If

            Return itemKey

        End Function


        ''' <summary>
        ''' Returns the item key for the provided VIN and VendorID.
        ''' </summary>
        ''' <returns>Integer</returns>
        ''' <remarks></remarks>
        Public Overridable Function GetItemByVendorVIN(ByVal VIN As String, ByVal VendorID As Integer) As ArrayList

            Dim itemKey As Integer = -1
            Dim itemIdentifier As String = String.Empty
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim returnList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            If Not IsNothing(VIN) And Not String.IsNullOrEmpty(VIN) Then
                Try
                    currentParam = New DBParam
                    currentParam.Name = "Item_ID"
                    currentParam.Type = DBParamType.String
                    currentParam.Value = VIN
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Vendor_ID"
                    currentParam.Type = DBParamType.Int
                    currentParam.Value = VendorID
                    paramList.Add(currentParam)

                    ' Execute the stored procedure 
                    results = factory.GetStoredProcedureDataReader("GetItemByVIN_VendorID", paramList)

                    While results.Read
                        returnList.Add(results.GetInt32(results.GetOrdinal("Item_Key")))
                        returnList.Add(results.GetString(results.GetOrdinal("Identifier")))
                    End While

                Catch ex As Exception

                    Debug.WriteLine(ex.Message)
                    'Finally
                    '    If results IsNot Nothing Then
                    '        results.Close()
                    '    End If
                End Try
            End If

            Return returnList

        End Function



        ''' <summary>
        ''' Set this in the UploadRowDAO sub class to change the stored
        ''' procedure used by the GetAllUploadRows function.
        ''' </summary>
        Public OptimizedInsertUploadRowStoredProcName As String = "EIM_OptimizedInsertUploadRow"

        Public Overridable Sub OptimizedInsertUploadRow(ByRef businessObject As UploadRow)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim pkValueList As New ArrayList

            ' setup parameters for stored proc

            ' Item_Key
            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Type = DBParamType.Int
            If businessObject.IsItemKeyNull Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = businessObject.ItemKey
            End If

            paramList.Add(currentParam)

            ' UploadSession_ID
            currentParam = New DBParam
            currentParam.Name = "UploadSession_ID"
            currentParam.Type = DBParamType.Int
            currentParam.Value = businessObject.UploadSessionID

            paramList.Add(currentParam)

            ' Identifier
            currentParam = New DBParam
            currentParam.Name = "Identifier"
            currentParam.Type = DBParamType.String
            If businessObject.IsIdentifierNull Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = businessObject.Identifier
            End If

            paramList.Add(currentParam)

            ' IsValidated
            currentParam = New DBParam
            currentParam.Name = "ValidationLevel"
            currentParam.Type = DBParamType.Int
            currentParam.Value = businessObject.ValidationLevel

            paramList.Add(currentParam)

            ' ItemRequestID
            currentParam = New DBParam
            currentParam.Name = "ItemRequest_ID"
            currentParam.Type = DBParamType.Int
            currentParam.Value = businessObject.ItemRequestID

            paramList.Add(currentParam)

            ' ConcatonatedValuesString
            currentParam = New DBParam
            currentParam.Name = "ConcatonatedUploadValuesString"
            currentParam.Type = DBParamType.String
            currentParam.Value = businessObject.ConcatonatedValuesString

            paramList.Add(currentParam)

            ' Get the output values
            currentParam = New DBParam
            currentParam.Name = "UploadValueIdsString"
            currentParam.Type = DBParamType.String

            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UploadRow_ID"
            currentParam.Type = DBParamType.Int

            paramList.Add(currentParam)

            ' get the current transaction if any
            Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction

            ' Execute the stored procedure
            If Not IsNothing(theTransaction) Then
                pkValueList = factory.ExecuteStoredProcedure(OptimizedInsertUploadRowStoredProcName, paramList, theTransaction.SqlTransaction)
            Else
                pkValueList = factory.ExecuteStoredProcedure(OptimizedInsertUploadRowStoredProcName, paramList)
            End If

            ' set the rows UploadValue's IDs
            Dim theUploadValueIdsString As String = CStr(pkValueList(0))
            Dim theUploadValueIdsStringArray As String() = theUploadValueIdsString.Split(New String() {"|"}, StringSplitOptions.None)

            Dim theUploadValueIdsStringArrayIndex As Integer = 0
            For Each theUploadValue As UploadValue In businessObject.UploadValueCollection

                theUploadValue.UploadValueID = CInt(theUploadValueIdsStringArray(theUploadValueIdsStringArrayIndex))

                theUploadValueIdsStringArrayIndex = theUploadValueIdsStringArrayIndex + 1
            Next

            ' set the returned pk value
            businessObject.UploadRowID = CInt(pkValueList(1))

        End Sub

        ''' <summary>
        ''' Set this in the UploadRowDAO sub class to change the stored
        ''' procedure used by the GetAllUploadRows function.
        ''' </summary>
        Public OptimizedUpdateUploadRowStoredProcName As String = "EIM_OptimizedUpdateUploadRow"

        Public Overridable Function OptimizedUpdateUploadRow(ByVal businessObject As UploadRow) As Integer

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim outputList As New ArrayList

            ' setup parameters for stored proc

            ' UploadRow_ID
            currentParam = New DBParam
            currentParam.Name = "UploadRow_ID"
            currentParam.Type = DBParamType.Int
            currentParam.Value = businessObject.UploadRowID

            paramList.Add(currentParam)

            ' Item_Key
            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Type = DBParamType.Int
            If businessObject.IsItemKeyNull Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = businessObject.ItemKey
            End If

            paramList.Add(currentParam)

            ' UploadSession_ID
            currentParam = New DBParam
            currentParam.Name = "UploadSession_ID"
            currentParam.Type = DBParamType.Int
            currentParam.Value = businessObject.UploadSessionID

            paramList.Add(currentParam)

            ' Identifier
            currentParam = New DBParam
            currentParam.Name = "Identifier"
            currentParam.Type = DBParamType.String
            If businessObject.IsIdentifierNull Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = businessObject.Identifier
            End If

            paramList.Add(currentParam)

            ' IsValidated
            currentParam = New DBParam
            currentParam.Name = "ValidationLevel"
            currentParam.Type = DBParamType.Int
            currentParam.Value = businessObject.ValidationLevel

            paramList.Add(currentParam)

            ' ItemRequestID
            currentParam = New DBParam
            currentParam.Name = "ItemRequest_ID"
            currentParam.Type = DBParamType.Int
            currentParam.Value = businessObject.ItemRequestID

            paramList.Add(currentParam)

            ' ConcatonatedValuesString
            currentParam = New DBParam
            currentParam.Name = "ConcatonatedUploadValuesString"
            currentParam.Type = DBParamType.String
            currentParam.Value = businessObject.ConcatonatedValuesString

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
                outputList = factory.ExecuteStoredProcedure(OptimizedUpdateUploadRowStoredProcName, paramList, theTransaction.SqlTransaction)
            Else
                outputList = factory.ExecuteStoredProcedure(OptimizedUpdateUploadRowStoredProcName, paramList)
            End If

            Return CInt(outputList(0))

        End Function

#End Region

    End Class

End Namespace
