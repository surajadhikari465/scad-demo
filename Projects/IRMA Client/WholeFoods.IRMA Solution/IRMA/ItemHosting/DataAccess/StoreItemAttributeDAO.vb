Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Imports log4net


Public Class StoreItemAttributeDAO
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Public Shared Sub Save(ByVal storeItemAttributeBO As StoreItemAttributeBO)

        logger.Debug("Save Entry")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "ID"
            currentParam.Value = storeItemAttributeBO.ID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = storeItemAttributeBO.StoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = storeItemAttributeBO.ItemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Exempt"
            currentParam.Value = IIf(storeItemAttributeBO.Exempt, 1, 0)
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "User_ID"
            currentParam.Value = giUserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            factory.ExecuteStoredProcedure("StoreItemAttribute_InsertUpdateAttribute", paramList)

        Catch ex As Exception
            Throw ex
        End Try

        logger.Debug("Save Exit")

    End Sub

    Public Shared Sub GetAttribute(ByRef storeItemAttributeBO As StoreItemAttributeBO)

        logger.Debug("GetAttribute Entry")


        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As SqlDataReader = Nothing
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = storeItemAttributeBO.StoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = storeItemAttributeBO.ItemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            results = factory.GetStoredProcedureDataReader("StoreItemAttribute_GetAttribute", paramList)

            While results.Read
                If (Not results.IsDBNull(results.GetOrdinal("StoreItemAttribute_ID"))) Then
                    storeItemAttributeBO.ID = results.GetInt32(results.GetOrdinal("StoreItemAttribute_ID"))
                End If
                If (Not results.IsDBNull(results.GetOrdinal("Exempt"))) Then
                    storeItemAttributeBO.Exempt = results.GetBoolean(results.GetOrdinal("Exempt"))
                End If

            End While
        Finally
            If results IsNot Nothing Then
                results.Close()
            End If
        End Try

        logger.Debug("GetAttribute Exit")

    End Sub

    Public Shared Function IsItemValidated(ByVal LinkedIdentifier As String) As Boolean

        logger.Debug("IsItemValidated Entry")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim validated As Boolean = False

        ' Execute the function
        validated = CType(factory.ExecuteScalar("SELECT dbo.fn_IsItemValidated(" & "'" & LinkedIdentifier & "'" & ")"), Boolean)

        logger.Debug("IsItemAuthorized Exit")

        Return validated
    End Function
End Class
