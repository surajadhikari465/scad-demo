Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.TaxHosting.BusinessLogic

Namespace WholeFoods.IRMA.TaxHosting.DataAccess
    Public Class TaxFlagDAO


#Region "read methods"

        ''' <summary>
        ''' Read complete list of TaxFlag data and return ArrayList of TaxFlagBO objects
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetTaxFlagList(ByVal taxFlag As TaxFlagBO, ByVal removeFlagList As ArrayList) As ArrayList
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim taxFlagBO As TaxFlagBO
            Dim taxFlagList As New ArrayList
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing
            Dim currentRemoveFlag As TaxFlagBO
            Dim removeFlagEnum As IEnumerator = Nothing
            Dim isAddCurrentFlag As Boolean

            Try
                If removeFlagList IsNot Nothing Then
                    removeFlagEnum = removeFlagList.GetEnumerator
                End If

                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "TaxClassID"
                currentParam.Value = taxFlag.TaxClassId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "TaxJurisdictionID"
                currentParam.Value = taxFlag.TaxJurisdictionId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("TaxHosting_GetTaxFlag", paramList)

                While results.Read
                    taxFlagBO = New TaxFlagBO()
                    taxFlagBO.TaxClassId = results.GetInt32(results.GetOrdinal("TaxClassID"))
                    taxFlagBO.TaxClassDesc = results.GetString(results.GetOrdinal("TaxClassDesc"))
                    taxFlagBO.TaxJurisdictionId = results.GetInt32(results.GetOrdinal("TaxJurisdictionID"))
                    taxFlagBO.TaxJurisdictionDesc = results.GetString(results.GetOrdinal("TaxJurisdictionDesc"))
                    taxFlagBO.TaxFlagKey = results.GetString(results.GetOrdinal("TaxFlagKey"))
                    taxFlagBO.TaxFlagValue = results.GetBoolean(results.GetOrdinal("TaxFlagValue"))

                    If (Not results.IsDBNull(results.GetOrdinal("TaxPercent"))) Then
                        'format decimal to show 2 digits
                        taxFlagBO.TaxPercent = FormatNumber(results.GetDecimal(results.GetOrdinal("TaxPercent"))).ToString
                    End If

                    If (Not results.IsDBNull(results.GetOrdinal("POSID"))) Then
                        taxFlagBO.POSId = results.GetInt32(results.GetOrdinal("POSID")).ToString
                    End If

                    If (Not results.IsDBNull(results.GetOrdinal("ExternalTaxGroupCode"))) Then
                        taxFlagBO.ExternalTaxGroupCode = results.GetString(results.GetOrdinal("ExternalTaxGroupCode"))
                    End If

                    'do not add this tax flag if it is contained in the removeFlagList
                    If removeFlagList IsNot Nothing AndAlso removeFlagList.Count > 0 Then
                        removeFlagEnum.Reset()
                        isAddCurrentFlag = False

                        While removeFlagEnum.MoveNext
                            currentRemoveFlag = CType(removeFlagEnum.Current, TaxFlagBO)

                            If currentRemoveFlag.TaxClassId = taxFlagBO.TaxClassId AndAlso _
                                currentRemoveFlag.TaxJurisdictionId = taxFlagBO.TaxJurisdictionId AndAlso _
                                currentRemoveFlag.TaxFlagKey.Equals(taxFlagBO.TaxFlagKey) Then
                                'do not add this tax flag
                                isAddCurrentFlag = False
                                Exit While
                            Else
                                isAddCurrentFlag = True
                            End If
                        End While

                        If isAddCurrentFlag Then
                            'add tax flag to list
                            taxFlagList.Add(taxFlagBO)
                        End If
                    Else
                        'add tax flag to list
                        taxFlagList.Add(taxFlagBO)
                    End If
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return taxFlagList
        End Function

        ''' <summary>
        ''' gets existing tax flag values for given item/store combo
        ''' </summary>
        ''' <param name="itemKey"></param>
        ''' <param name="storeNo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAvailableTaxFlagListForItem(ByVal itemKey As Integer, ByVal storeNo As Integer) As ArrayList
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim taxFlagBO As TaxFlagBO
            Dim taxFlagList As New ArrayList
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = itemKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = storeNo
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("TaxHosting_GetAvailableTaxFlagsForItem", paramList)

                While results.Read
                    taxFlagBO = New TaxFlagBO()
                    taxFlagBO.TaxFlagKey = results.GetString(results.GetOrdinal("TaxFlagKey"))

                    'add tax flag to list
                    taxFlagList.Add(taxFlagBO)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return taxFlagList
        End Function

        Public Function GetTaxFlagActiveCount(ByVal taxFlagBO As TaxFlagBO) As Integer
            Dim count As Integer
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "TaxClassID"
                currentParam.Value = taxFlagBO.TaxClassId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "TaxJurisdictionID"
                currentParam.Value = taxFlagBO.TaxJurisdictionId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("TaxHosting_GetTaxFlagActiveCount", paramList)

                While results.Read
                    count = results.GetInt32(results.GetOrdinal("ActiveCount"))
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return count
        End Function

        Public Function IsExistingTaxFlagForJurisdiction(ByVal taxFlagBO As TaxFlagBO) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing
            Dim isExisting As Boolean = False

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "TaxJurisdictionID"
                currentParam.Value = taxFlagBO.TaxJurisdictionId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "TaxFlagKey"
                currentParam.Value = taxFlagBO.TaxFlagKey
                currentParam.Type = DBParamType.Char
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "TaxClassID"
                currentParam.Value = taxFlagBO.TaxClassId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("TaxHosting_IsExistingTaxFlagForJurisdiction", paramList)

                While results.Read
                    If results.GetInt32(results.GetOrdinal("TaxFlagCount")) > 0 Then
                        isExisting = True
                    End If
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return isExisting
        End Function

#End Region

#Region "write methods"

        ''' <summary>
        ''' Insert TaxFlag and TaxDefinition data
        ''' </summary>
        ''' <param name="taxFlagBO"></param>
        ''' <remarks></remarks>
        Public Sub InsertTaxFlag(ByVal taxFlagBO As TaxFlagBO)
            InsertOrUpdateData(True, taxFlagBO)
        End Sub

        ''' <summary>
        ''' Update TaxFlag and TaxDefinition data
        ''' </summary>
        ''' <param name="taxFlagBO"></param>
        ''' <remarks></remarks>
        Public Sub UpdateTaxFlag(ByVal taxFlagBO As TaxFlagBO)
            InsertOrUpdateData(False, taxFlagBO)
        End Sub

        ''' <summary>
        ''' build DBParams and call stored procedure to insert or update data based on flag
        ''' </summary>
        ''' <param name="isInsert"></param>
        ''' <param name="taxFlagBO"></param>
        ''' <remarks></remarks>
        Private Sub InsertOrUpdateData(ByVal isInsert As Boolean, ByVal taxFlagBO As TaxFlagBO)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "TaxJurisdictionID"
                currentParam.Value = taxFlagBO.TaxJurisdictionId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "TaxFlagKey"
                currentParam.Value = taxFlagBO.TaxFlagKey
                currentParam.Type = DBParamType.Char
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "TaxClassID"
                currentParam.Value = taxFlagBO.TaxClassId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "TaxFlagValue"
                If taxFlagBO.TaxFlagValue Then
                    currentParam.Value = 1
                Else
                    currentParam.Value = 0
                End If
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "TaxPercent"
                If taxFlagBO.TaxPercent Is Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = taxFlagBO.TaxPercent
                End If
                currentParam.Type = DBParamType.Decimal
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "POSID"
                If taxFlagBO.POSId Is Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = taxFlagBO.POSId
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ResetActiveTaxFlags"
                If taxFlagBO.ResetActiveFlags Then
                    currentParam.Value = 1
                Else
                    currentParam.Value = 0
                End If
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                ' Disable this parameter per TFS 831/MYounes
                'currentParam = New DBParam
                'currentParam.Name = "ExternalTaxGroupCode"
                'If taxFlagBO.ExternalTaxGroupCode Is Nothing Then
                'currentParam.Value = DBNull.Value
                'Else
                'currentParam.Value = taxFlagBO.ExternalTaxGroupCode
                'End If
                'currentParam.Type = DBParamType.String
                'paramList.Add(currentParam)

                ' Execute the stored procedure
                If isInsert Then
                    factory.ExecuteStoredProcedure("TaxHosting_InsertTaxFlag", paramList)
                Else
                    factory.ExecuteStoredProcedure("TaxHosting_UpdateTaxFlag", paramList)
                End If
            Catch ex As Exception
                ' TODO Handle exception
                'MsgBox("taxflagdao.InsertOrUpdateData exception: " & ex.ToString)
            End Try
        End Sub

        Public Sub DeleteData(ByVal taxFlagBO As TaxFlagBO, ByRef transaction As SqlTransaction)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "TaxClassID"
                currentParam.Value = taxFlagBO.TaxClassId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "TaxJurisdictionID"
                currentParam.Value = taxFlagBO.TaxJurisdictionId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "TaxFlagKey"
                currentParam.Value = taxFlagBO.TaxFlagKey
                currentParam.Type = DBParamType.Char
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("TaxHosting_DeleteTaxFlag", paramList, transaction)
            Catch ex As Exception
                'TODO handle exception
            End Try
        End Sub

#End Region

        Public Function GetTransaction() As SqlTransaction
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim transaction As SqlTransaction = factory.BeginTransaction(IsolationLevel.ReadCommitted)

            Return transaction
        End Function

    End Class
End Namespace