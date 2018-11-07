Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ItemHosting.DataAccess
    Public Class ScaleLabelStyleDAO

        Public Shared Function GetComboList() As ArrayList
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim scaleLabelStyle As ScaleLabelStyleBO
            Dim scaleLabelStyleList As New ArrayList
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Scale_GetLabelStyles")

                While results.Read
                    scaleLabelStyle = New ScaleLabelStyleBO()
                    scaleLabelStyle.ID = results.GetInt32(results.GetOrdinal("Scale_LabelStyle_ID"))
                    scaleLabelStyle.Description = results.GetString(results.GetOrdinal("Description"))

                    scaleLabelStyleList.Add(scaleLabelStyle)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return scaleLabelStyleList
        End Function
        Public Shared Function Save(ByVal scaleLabelStyle As ScaleLabelStyleBO) As Boolean

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing
            Dim isSuccess As Boolean = True

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ID"
                currentParam.Value = scaleLabelStyle.ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Description"
                currentParam.Value = scaleLabelStyle.Description
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                results = factory.GetStoredProcedureDataReader("Scale_CheckForDuplicateLabelStyle", paramList)
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
                    factory.ExecuteStoredProcedure("Scale_InsertUpdateLabelStyle", paramList)
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
                factory.ExecuteStoredProcedure("Scale_DeleteLabelStyle", paramList)

            Catch ex As Exception
                If Err.Number = 5 Then
                    MsgBox("Unable to delete.  Make sure this Label Style is not associated with any items before deleting.", MsgBoxStyle.Critical, "Scale Label Style Delete")
                Else
                    Throw ex
                End If
            End Try

        End Sub

    End Class

End Namespace
