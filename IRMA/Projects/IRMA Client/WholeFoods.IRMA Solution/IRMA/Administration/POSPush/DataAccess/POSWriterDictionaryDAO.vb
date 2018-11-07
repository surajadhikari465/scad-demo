Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Administration.POSPush.DataAccess
    Public Class POSWriterDictionaryDAO
#Region "Read Methods"
        ''' <summary>
        ''' Read complete list of POSWriterDictionary records for the given POSFileWriterKey.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function GetPOSWriterDictionaryValues(ByVal POSFileWriterKey As Integer) As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            ' setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "POSFileWriterKey"
            currentParam.Value = POSFileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            'currentParam = New DBParam
            'currentParam.Name = "POSDataTypeKey"
            'currentParam.Value = POSDataTypeKey
            'currentParam.Type = DBParamType.Int
            'paramList.Add(currentParam)

            ' Execute the stored procedure 
            Return factory.GetStoredProcedureDataSet("Administration_POSPush_GetPOSWriterDictionary", paramList)
        End Function

        ''' <summary>
        ''' Save changes to the DataSet to the database (inserts, updates, deletes)
        ''' </summary>
        ''' <param name="dataSet"></param>
        ''' <remarks></remarks>
        Public Shared Sub SavePOSWriterDictionaryValues(ByRef dataSet As DataSet, ByVal POSFileWriterKey As Integer)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            ' setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "POSFileWriterKey"
            currentParam.Value = POSFileWriterKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            'currentParam = New DBParam
            'currentParam.Name = "POSDataTypeKey"
            'currentParam.Value = POSDataTypeKey
            'currentParam.Type = DBParamType.Int
            'paramList.Add(currentParam)

            ' Execute the updates
            factory.UpdateDataSet(dataSet, "Administration_POSPush_GetPOSWriterDictionary", True, paramList)
        End Sub

#End Region
    End Class
End Namespace
