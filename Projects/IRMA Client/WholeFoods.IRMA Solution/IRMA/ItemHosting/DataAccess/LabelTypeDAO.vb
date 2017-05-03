Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess
Imports log4net

Namespace WholeFoods.IRMA.ItemHosting.DataAccess
    Public Class LabelTypeDAO
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


        Public Shared Function GetLabelTypeList() As ArrayList
            logger.Debug("GetLabelTypeList Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim labelType As LabelTypeBO
            Dim labelTypeList As New ArrayList
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("ItemHosting_GetLabelType")

                While results.Read
                    labelType = New LabelTypeBO()
                    labelType.LabelTypeID = results.GetInt32(results.GetOrdinal("LabelType_ID"))
                    labelType.LabelTypeDesc = results.GetString(results.GetOrdinal("LabelTypeDesc"))

                    labelTypeList.Add(labelType)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetLabelTypeList Exit")

            Return labelTypeList
        End Function

        Protected Shared LabelType_Add_StoredProcName As String = "LabelType_Add"

        Public Shared Sub AddLabelType(ByVal labelType As LabelTypeBO)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim pkValueList As New ArrayList

            ' setup parameters for stored proc

            currentParam = New DBParam
            currentParam.Name = "LabelTypeDesc"
            currentParam.Type = DBParamType.String
            currentParam.Value = labelType.LabelTypeDesc

            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "LabelTypeID"
            currentParam.Type = DBParamType.Int

            paramList.Add(currentParam)

            pkValueList = factory.ExecuteStoredProcedure(LabelType_Add_StoredProcName, paramList)
        End Sub

        Protected Shared LabelType_Update_StoredProcName As String = "LabelType_Update"

        Public Shared Sub UpdateLabelType(ByVal labelType As LabelTypeBO)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim pkValueList As New ArrayList

            ' setup parameters for stored proc

            currentParam = New DBParam
            currentParam.Name = "LabelTypeID"
            currentParam.Type = DBParamType.Int
            currentParam.Value = labelType.LabelTypeID

            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "LabelTypeDesc"
            currentParam.Type = DBParamType.String
            currentParam.Value = labelType.LabelTypeDesc

            paramList.Add(currentParam)

            pkValueList = factory.ExecuteStoredProcedure(LabelType_Update_StoredProcName, paramList)
        End Sub

        Protected Shared LabelType_Delete_StoredProcName As String = "LabelType_Delete"

        Public Shared Sub DeleteLabelType(ByVal labelType As LabelTypeBO)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim pkValueList As New ArrayList

            ' setup parameters for stored proc

            currentParam = New DBParam
            currentParam.Name = "LabelTypeID"
            currentParam.Type = DBParamType.Int
            currentParam.Value = labelType.LabelTypeID

            paramList.Add(currentParam)

            pkValueList = factory.ExecuteStoredProcedure(LabelType_Delete_StoredProcName, paramList)
        End Sub
    End Class
End Namespace
