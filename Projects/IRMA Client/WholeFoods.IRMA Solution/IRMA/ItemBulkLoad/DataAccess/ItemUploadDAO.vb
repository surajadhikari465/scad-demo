Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ItemBulkLoad.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Public Class ItemUploadDAO

    Public Function ItemUploadSearch(ByVal itemUploadSearchBO As ItemUploadSearchBO) As SqlDataReader
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim results As SqlDataReader = Nothing
        Dim currentParam As DBParam
        Dim paramList As New ArrayList

        Try
            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "ItemUploadHeader_ID"
            If itemUploadSearchBO.ItemUploadHeaderID <= 0 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = itemUploadSearchBO.ItemUploadHeaderID
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ItemUploadType_ID"
            If itemUploadSearchBO.ItemUploadTypeID <= 0 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = itemUploadSearchBO.ItemUploadTypeID
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "User_ID"
            If itemUploadSearchBO.UserID <= 0 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = itemUploadSearchBO.UserID
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UploadDatetime"
            If itemUploadSearchBO.CreateDate = Nothing Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = itemUploadSearchBO.CreateDate
            End If
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            results = factory.GetStoredProcedureDataReader("[GetItemUploadSearch]", paramList)

        Catch e As Exception
            'TODO handle exception
            Throw e
        End Try

        Return results

    End Function
End Class
