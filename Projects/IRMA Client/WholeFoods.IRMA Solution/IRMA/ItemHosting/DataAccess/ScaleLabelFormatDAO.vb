Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ItemHosting.DataAccess
    Public Class ScaleLabelFormatDAO

        Public Shared Function GetComboList() As ArrayList
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim scaleLabelFormat As ScaleLabelFormatBO
            Dim scaleLabelFormatList As New ArrayList
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Scale_GetLabelFormats")

                While results.Read
                    scaleLabelFormat = New ScaleLabelFormatBO()
                    scaleLabelFormat.ID = results.GetInt32(results.GetOrdinal("Scale_LabelFormat_ID"))
                    scaleLabelFormat.Description = results.GetString(results.GetOrdinal("Description"))

                    scaleLabelFormatList.Add(scaleLabelFormat)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return scaleLabelFormatList
        End Function
        Public Shared Function Save(ByVal scaleLabelFormat As ScaleLabelFormatBO) As Boolean

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing
            Dim isSuccess As Boolean = True

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ID"
                currentParam.Value = scaleLabelFormat.ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Description"
                currentParam.Value = scaleLabelFormat.Description
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                results = factory.GetStoredProcedureDataReader("Scale_CheckForDuplicateLabelFormat", paramList)
                While results.Read
                    If results.GetInt32(results.GetOrdinal("DuplicateCount")) > 0 Then
                        ' this is a duplicate
                        isSuccess = False
                        Exit While
                    End If
                End While
                results.Close()

                If isSuccess Then
                    ' Execute the stored procedure 
                    factory.ExecuteStoredProcedure("Scale_InsertUpdateLabelFormat", paramList)
                End If

            Catch ex As Exception
                Throw ex
            End Try

            Return isSuccess
        End Function

        Public Shared Sub Delete(ByVal id As Integer)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ID"
                currentParam.Value = id
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("Scale_DeleteLabelFormat", paramList)

            Catch ex As Exception
                If Err.Number = 5 Then
                    MsgBox("Unable to delete.  Make sure this Label Format is not associated with any items before deleting.", MsgBoxStyle.Critical, "Scale Label Format Delete")
                Else
                    Throw ex
                End If
            End Try

        End Sub
    End Class

End Namespace
