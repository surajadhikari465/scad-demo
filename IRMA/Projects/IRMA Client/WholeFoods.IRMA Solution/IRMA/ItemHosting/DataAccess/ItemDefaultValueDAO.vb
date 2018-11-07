Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Common.DataAccess
Imports log4net

Namespace WholeFoods.IRMA.ItemHosting.DataAccess

    Public Class ItemDefaultValueDAO

        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


        ''' <summary>
        ''' returns a list of defaulted attrbutes values by hierarchy position
        ''' </summary>
        ''' <returns>ArrayList of defaulted attrbutes values</returns>
        ''' <remarks></remarks>
        Public Shared Function GetItemDefaultValues(ByVal categoryId As Integer, ByVal prodHierarchyLevel4Id As Integer) As ArrayList

            logger.Debug("GetItemDefaultValues Entry  with categoryId = " + categoryId.ToString() & " , prodHierarchyLevel4Id = " + prodHierarchyLevel4Id.ToString())

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As New DBParam
            Dim itemValueList As New ArrayList
            Dim itemDefaultValue As ItemDefaultValueBO

            Try

                Dim usesFourLevelHierarchy As Boolean = InstanceDataDAO.IsFlagActive("FourLevelHierarchy")

                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Category_ID"

                If Not usesFourLevelHierarchy And categoryId >= 1 Then
                    currentParam.Value = categoryId
                Else
                    currentParam.Value = DBNull.Value
                End If

                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ProdHierarchyLevel4_ID"

                If usesFourLevelHierarchy And prodHierarchyLevel4Id >= 1 Then
                    currentParam.Value = prodHierarchyLevel4Id
                Else
                    currentParam.Value = DBNull.Value
                End If

                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetItemDefaultValues", paramList)

                ' Retreive the results into an arrayList of ItemDefaultValueBOs.
                While results.Read

                    itemDefaultValue = New ItemDefaultValueBO

                    itemDefaultValue.ID = results.GetInt32(results.GetOrdinal("ItemDefaultAttribute_ID"))
                    itemDefaultValue.ItemDefaultAttributeID = results.GetInt32(results.GetOrdinal("ItemDefaultAttribute_ID"))

                    If Not results.IsDBNull(results.GetOrdinal("ProdHierarchyLevel4_ID")) Then
                        itemDefaultValue.ProdHierarchyLevel4ID = results.GetInt32(results.GetOrdinal("ProdHierarchyLevel4_ID"))
                    Else
                        itemDefaultValue.ProdHierarchyLevel4ID = Nothing
                    End If

                    If Not results.IsDBNull(results.GetOrdinal("Category_ID")) Then
                        itemDefaultValue.CategoryID = results.GetInt32(results.GetOrdinal("Category_ID"))
                    Else
                        itemDefaultValue.CategoryID = Nothing
                    End If

                    itemDefaultValue.Value = results.GetString(results.GetOrdinal("Value"))
                    itemDefaultValue.FieldName = results.GetString(results.GetOrdinal("AttributeField"))
                    itemDefaultValue.DbDataType = CInt(results.GetByte(results.GetOrdinal("Type")))

                    itemValueList.Add(itemDefaultValue)

                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetItemDefaultValues Exit")

            Return itemValueList
        End Function

        ''' <summary>
        ''' saves a defaulted attrbutes value
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub SaveItemDefaultValues(ByVal itemDefaultAttributeID As Integer, _
            ByVal prodHierarchyLevel4ID As Integer, ByVal categoryID As Integer, ByVal value As String)


            logger.Debug("SaveItemDefaultValues Entry with itemDefaultAttributeID = " + itemDefaultAttributeID.ToString() + ", prodHierarchyLevel4ID = " + prodHierarchyLevel4ID.ToString() + " , categoryID= " + categoryID.ToString())

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As New DBParam

            Try

                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ItemDefaultAttribute_ID"
                currentParam.Value = itemDefaultAttributeID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ProdHierarchyLevel4_ID"

                If prodHierarchyLevel4ID >= 1 Then
                    currentParam.Value = prodHierarchyLevel4ID
                Else
                    currentParam.Value = DBNull.Value
                End If

                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Category_ID"

                If categoryID >= 1 Then
                    currentParam.Value = categoryID
                Else
                    currentParam.Value = DBNull.Value
                End If

                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "value"
                currentParam.Value = value
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.GetStoredProcedureDataReader("SaveItemDefaultValue", paramList)

            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If

                logger.Debug("SaveItemDefaultValues Exit")

            End Try

        End Sub

        ''' <summary>
        ''' deletes a defaulted attrbutes value
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub DeleteItemDefaultValues(ByVal itemDefaultAttributeID As Integer, _
            ByVal prodHierarchyLevel4ID As Integer, ByVal categoryID As Integer, ByVal value As String)


            logger.Debug(" DeleteItemDefaultValues Entry with itemDefaultAttributeID = " + itemDefaultAttributeID.ToString + " , prodHierarchyLevel4ID = " + prodHierarchyLevel4ID.ToString & ", categoryID =" + categoryID.ToString)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As New DBParam

            Try

                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ItemDefaultAttribute_ID"
                currentParam.Value = itemDefaultAttributeID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ProdHierarchyLevel4_ID"

                If prodHierarchyLevel4ID >= 1 Then
                    currentParam.Value = prodHierarchyLevel4ID
                Else
                    currentParam.Value = DBNull.Value
                End If

                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Category_ID"

                If categoryID >= 1 Then
                    currentParam.Value = categoryID
                Else
                    currentParam.Value = DBNull.Value
                End If

                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "value"

                If Not value Is Nothing Then
                    currentParam.Value = value
                Else
                    currentParam.Value = DBNull.Value
                End If

                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("DeleteItemDefaultValue", paramList)

            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("DeleteItemDefaultValues Exit")

        End Sub

    End Class

End Namespace