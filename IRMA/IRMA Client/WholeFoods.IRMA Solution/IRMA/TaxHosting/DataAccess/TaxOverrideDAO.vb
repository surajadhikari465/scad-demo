Imports System.Data.SqlClient
Imports WholeFoods.IRMA.TaxHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.TaxHosting.DataAccess
    Public Class TaxOverrideDAO

        ''' <summary>
        ''' Read complete list of TaxFlag data and return ArrayList of TaxFlagBO objects
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetTaxOverrideList(ByVal taxOverride As TaxOverrideBO, ByVal removeFlagList As ArrayList) As ArrayList
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim taxOverrideBO As TaxOverrideBO
            Dim taxOverrideList As New ArrayList
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing
            Dim currentRemoveFlag As TaxOverrideBO
            Dim removeFlagEnum As IEnumerator = removeFlagList.GetEnumerator
            Dim isAddCurrentOverride As Boolean

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ItemKey"
                currentParam.Value = taxOverride.ItemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreNo"
                currentParam.Value = taxOverride.StoreNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("TaxHosting_GetTaxOverride", paramList)

                While results.Read
                    taxOverrideBO = New TaxOverrideBO()
                    taxOverrideBO.ItemKey = results.GetInt32(results.GetOrdinal("Item_Key"))
                    taxOverrideBO.StoreNo = results.GetInt32(results.GetOrdinal("Store_No"))
                    taxOverrideBO.TaxFlagKey = results.GetString(results.GetOrdinal("TaxFlagKey"))
                    taxOverrideBO.TaxFlagValue = results.GetBoolean(results.GetOrdinal("TaxFlagValue"))

                    'do not add this tax flag if it is contained in the removeFlagList
                    If removeFlagList.Count > 0 Then
                        removeFlagEnum.Reset()
                        isAddCurrentOverride = False

                        While removeFlagEnum.MoveNext
                            currentRemoveFlag = CType(removeFlagEnum.Current, TaxOverrideBO)

                            If currentRemoveFlag.StoreNo = taxOverrideBO.StoreNo AndAlso _
                                currentRemoveFlag.ItemKey = taxOverrideBO.ItemKey AndAlso _
                                currentRemoveFlag.TaxFlagKey.Equals(taxOverrideBO.TaxFlagKey) Then
                                'do not add this tax flag
                                isAddCurrentOverride = False
                                Exit While
                            Else
                                isAddCurrentOverride = True
                            End If
                        End While

                        If isAddCurrentOverride Then
                            'add tax flag to list
                            taxOverrideList.Add(taxOverrideBO)
                        End If
                    Else
                        'add tax flag to list
                        taxOverrideList.Add(taxOverrideBO)
                    End If
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return taxOverrideList
        End Function

        Public Function GetTaxOverrideList(ByVal itemKey As Integer) As ArrayList
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim taxOverrideBO As TaxOverrideBO
            Dim taxOverrideList As New ArrayList
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ItemKey"
                currentParam.Value = itemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("TaxHosting_GetTaxOverrideForItem", paramList)

                While results.Read
                    taxOverrideBO = New TaxOverrideBO()
                    taxOverrideBO.StoreName = results.GetString(results.GetOrdinal("Store_Name"))
                    taxOverrideBO.TaxFlagKey = results.GetString(results.GetOrdinal("TaxFlagKey"))
                    taxOverrideBO.TaxFlagValue = results.GetBoolean(results.GetOrdinal("TaxFlagValue"))

                    'add tax flag to list
                    taxOverrideList.Add(taxOverrideBO)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return taxOverrideList
        End Function

        ''' <summary>
        ''' returns the number of tax overrides for a given tax class/jurisdiction/flag;
        ''' used when user attempts to delete a tax flag -- they will be informed how many overrides there are that will be affected
        ''' </summary>
        ''' <param name="taxClassID"></param>
        ''' <param name="taxJurisdictionID"></param>
        ''' <param name="taxFlagKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetNumberItemsOverridenForFlag(ByVal taxClassID As Integer, _
                                                                ByVal taxJurisdictionID As Integer, _
                                                                ByVal taxFlagKey As String) As Integer
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing
            Dim overrideCount As Integer

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "TaxClassID"
                currentParam.Value = taxClassID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "TaxJurisdictionID"
                currentParam.Value = taxJurisdictionID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "TaxFlagKey"
                currentParam.Value = taxFlagKey
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("TaxHosting_ConfirmDeleteTaxFlag", paramList)

                If results.Read Then
                    overrideCount = results.GetInt32(results.GetOrdinal("TaxOverrideCount"))
                End If
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return overrideCount
        End Function

        ''' <summary>
        ''' Insert TaxOverride and TaxDefinition data
        ''' </summary>
        ''' <param name="taxOverrideBO"></param>
        ''' <remarks></remarks>
        Public Sub InsertTaxFlag(ByVal taxOverrideBO As TaxOverrideBO)
            InsertOrUpdateData(True, taxOverrideBO)
        End Sub

        ''' <summary>
        ''' Update TaxFlag and TaxDefinition data
        ''' </summary>
        ''' <param name="taxOverrideBO"></param>
        ''' <remarks></remarks>
        Public Sub UpdateTaxFlag(ByVal taxOverrideBO As TaxOverrideBO)
            InsertOrUpdateData(False, taxOverrideBO)
        End Sub

        ''' <summary>
        ''' build DBParams and call stored procedure to insert or update data based on flag
        ''' </summary>
        ''' <param name="isInsert"></param>
        ''' <param name="taxOverrideBO"></param>
        ''' <remarks></remarks>
        Private Sub InsertOrUpdateData(ByVal isInsert As Boolean, ByVal taxOverrideBO As TaxOverrideBO)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ItemKey"
                currentParam.Value = taxOverrideBO.ItemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreNo"
                currentParam.Value = taxOverrideBO.StoreNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "TaxFlagKey"
                currentParam.Value = taxOverrideBO.TaxFlagKey
                currentParam.Type = DBParamType.Char
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "TaxFlagValue"
                If taxOverrideBO.TaxFlagValue Then
                    currentParam.Value = 1
                Else
                    currentParam.Value = 0
                End If
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                If isInsert Then
                    factory.ExecuteStoredProcedure("TaxHosting_InsertTaxOverride", paramList)
                Else
                    factory.ExecuteStoredProcedure("TaxHosting_UpdateTaxOverride", paramList)
                End If
            Catch ex As Exception
                'TODO handle exception
            End Try
        End Sub

        Public Sub DeleteData(ByVal taxOverrideBO As TaxOverrideBO, ByRef transaction As SqlTransaction)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ItemKey"
                currentParam.Value = taxOverrideBO.ItemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "StoreNo"
                currentParam.Value = taxOverrideBO.StoreNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "TaxFlagKey"
                currentParam.Value = taxOverrideBO.TaxFlagKey
                currentParam.Type = DBParamType.Char
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("TaxHosting_DeleteTaxOverride", paramList, transaction)
            Catch ex As Exception
                'TODO handle exception
            End Try
        End Sub

        Public Sub DeleteDataForItemKey(ByVal itemKey As Integer)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ItemKey"
                currentParam.Value = itemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("TaxHosting_DeleteTaxOverrideForItem", paramList)
            Catch ex As Exception
                'TODO handle exception
            End Try
        End Sub

        Public Function GetTransaction() As SqlTransaction
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim transaction As SqlTransaction = factory.BeginTransaction(IsolationLevel.ReadCommitted)

            Return transaction
        End Function

    End Class
End Namespace
