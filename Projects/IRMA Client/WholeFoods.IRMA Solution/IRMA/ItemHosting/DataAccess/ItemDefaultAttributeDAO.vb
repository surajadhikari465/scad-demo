Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess
Imports log4net

Namespace WholeFoods.IRMA.ItemHosting.DataAccess

    Public Class ItemDefaultAttributeDAO

        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Shared Sub UpdateItemDefaultAttribute(ByVal itemDefaultAttribute_ID As Integer, ByVal attributeName As String, ByVal active As Boolean, ByVal controlOrder As Integer)
            logger.Debug("UpdateItemDefaultAttribute Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As New DBParam

            Try
                currentParam = New DBParam
                currentParam.Name = "itemDefaultAttribute_ID"
                currentParam.Value = itemDefaultAttribute_ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "attributeName"
                currentParam.Value = attributeName
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "active"
                currentParam.Value = active
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "controlOrder"
                currentParam.Value = controlOrder
                currentParam.Type = DBParamType.SmallInt
                paramList.Add(currentParam)

                factory.ExecuteStoredProcedure("ItemDefaultAttributes_UpdateItemDefaultAttribute", paramList)
            Catch ex As Exception
                Dim message As String = ex.Message

                If Not IsNothing(ex.InnerException) Then
                    message = ex.InnerException.Message
                End If

                MessageBox.Show("The following error has occured in UpdateItemDefaultAttribute. Please report this to the IRMA support team." +
                    ControlChars.NewLine + ControlChars.NewLine + message)
            End Try

            logger.Debug("UpdateItemDefaultAttribute Exit")
        End Sub

        ''' <summary>
        ''' returns a list of all defaultable attributes, whether enabled or not
        ''' </summary>
        ''' <returns>ArrayList of store names</returns>
        ''' <remarks></remarks>
        Public Shared Function GetAllItemDefaultAttributes() As DataTable
            logger.Debug("GetAllItemDefaultAttributes Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataSet = Nothing

            Try
                results = New DataSet
                results = factory.GetStoredProcedureDataSet("ItemDefaultAttributes_GetAll")
            Catch ex As Exception
                Dim message As String = ex.Message

                If Not IsNothing(ex.InnerException) Then
                    message = ex.InnerException.Message
                End If

                MessageBox.Show("The following error has occured in GetAllItemDefaultAttributes. Please report this to the IRMA support team." +
                    ControlChars.NewLine + ControlChars.NewLine + message)
            Finally
                If results IsNot Nothing Then
                    results.Dispose()
                End If
            End Try

            logger.Debug("GetAllItemDefaultAttributes Exit")

            Return results.Tables(0)
        End Function

        ''' <summary>
        ''' returns a list of defaultable attrbutes
        ''' </summary>
        ''' <returns>ArrayList of store names</returns>
        ''' <remarks></remarks>
        Public Shared Function GetItemDefaultAttributes(ByVal prodHierarchyLevel4ID As Integer, ByVal categoryID As Integer) As ArrayList

            logger.Debug("GetItemDefaultAttributes Entry with prodHierarchyLevel4ID=" + prodHierarchyLevel4ID.ToString() + ", categoryID= " & categoryID.ToString())

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As New DBParam
            Dim itemAttributeList As New ArrayList
            Dim itemDefaultAttribute As ItemDefaultAttributeBO

            Try
                ' setup parameters for stored proc
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
                currentParam.Value = categoryID

                ' set the categoryID to nothing if there is a prodHierarchyLevel4ID
                ' because the region uses four levels not two and we need to use the ID of the bottom most level
                If prodHierarchyLevel4ID < 1 And categoryID >= 1 Then
                    currentParam.Value = categoryID
                Else
                    currentParam.Value = DBNull.Value
                End If

                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetItemDefaultAttributes", paramList)

                While results.Read

                    itemDefaultAttribute = New ItemDefaultAttributeBO

                    itemDefaultAttribute.ID = results.GetInt32(results.GetOrdinal("ItemDefaultAttribute_ID"))
                    itemDefaultAttribute.AttributeName = results.GetString(results.GetOrdinal("AttributeName"))
                    itemDefaultAttribute.AttributeField = results.GetString(results.GetOrdinal("AttributeField"))
                    itemDefaultAttribute.Type = CType(results.GetByte(results.GetOrdinal("Type")), Integer)
                    itemDefaultAttribute.Active = results.GetBoolean(results.GetOrdinal("Active"))
                    itemDefaultAttribute.ControlType = CType(results.GetByte(results.GetOrdinal("ControlType")), Integer)

                    If Not results.IsDBNull(results.GetOrdinal("PopulateProcedure")) Then
                        itemDefaultAttribute.PopulateProcedure = results.GetString(results.GetOrdinal("PopulateProcedure"))
                    End If

                    If Not results.IsDBNull(results.GetOrdinal("IndexField")) Then
                        itemDefaultAttribute.IndexField = results.GetString(results.GetOrdinal("IndexField"))
                    End If

                    If Not results.IsDBNull(results.GetOrdinal("DescriptionField")) Then
                        itemDefaultAttribute.DescriptionField = results.GetString(results.GetOrdinal("DescriptionField"))
                    End If

                    If Not results.IsDBNull(results.GetOrdinal("Value")) Then
                        itemDefaultAttribute.ItemDefaultValueID = results.GetInt32(results.GetOrdinal("ItemDefaultValue_ID"))
                        itemDefaultAttribute.Value = results.GetString(results.GetOrdinal("Value"))
                    End If

                    itemAttributeList.Add(itemDefaultAttribute)

                End While

            Catch ex As Exception
                Dim message As String = ex.Message

                If Not IsNothing(ex.InnerException) Then
                    message = ex.InnerException.Message
                End If
                MessageBox.Show("The following error has occured in GetItemDefaultAttributes. Please report this to the IRMA support team." +
                    ControlChars.NewLine + ControlChars.NewLine + message)

            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return itemAttributeList

        End Function

        ''' <summary>
        ''' Runs the populate procedure specified and fills the given comboBox.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub RunPopulateProcedure(ByVal populateProcedureName As String,
            ByVal indexField As String, ByVal descriptionField As String, ByRef combobox As ComboBox)


            logger.Debug("RunPopulateProcedure Entry with populateProcedureName= " + populateProcedureName + ", indexField=" + indexField + ",descriptionField= " + descriptionField)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList

            Dim NewIndex As Integer

            Try

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader(populateProcedureName, paramList)

                While results.Read

                    NewIndex = combobox.Items.Add(results.GetString(results.GetOrdinal(descriptionField)))
                    VB6.SetItemData(combobox, NewIndex, CInt(results.GetValue(results.GetOrdinal(indexField))))

                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

        End Sub

    End Class

End Namespace