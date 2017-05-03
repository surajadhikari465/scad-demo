Imports Microsoft.VisualBasic

Public Class IdentifierCheck

    Public Shared Function IdentifierExists(ByVal iid As String) As Integer
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim currentParam As DBParam
        Dim paramList As New ArrayList
        Dim returnList As Boolean
        Dim sql As String
        currentParam = New DBParam
        currentParam.Name = "Identifier"
        currentParam.Value = iid
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)
        sql = "CheckForDuplicateIdentifier '" & iid & "'"
        ' Execute the stored procedure 
        returnList = CInt(factory.ExecuteScalar(sql))
        Return returnList
    End Function
    
End Class
