Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ItemHosting.DataAccess
    Public Class ScaleLabelTypeDAO

        Public Shared Function GetComboList() As ArrayList
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim scaleLabelType As ScaleLabelTypeBO
            Dim scaleLabelTypeList As New ArrayList
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Scale_GetLabelTypes")

                While results.Read
                    scaleLabelType = New ScaleLabelTypeBO()
                    scaleLabelType.ID = results.GetInt32(results.GetOrdinal("Scale_LabelType_ID"))
                    scaleLabelType.Description = results.GetString(results.GetOrdinal("Description"))
                    If (Not results.IsDBNull(results.GetOrdinal("LinesPerLabel"))) Then
                        scaleLabelType.LinesPerLabel = CInt(results.GetValue(results.GetOrdinal("LinesPerLabel")))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Characters"))) Then
                        scaleLabelType.CharsPerLine = results.GetInt32(results.GetOrdinal("Characters"))
                    End If
                    scaleLabelTypeList.Add(scaleLabelType)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return scaleLabelTypeList
        End Function
        Public Shared Function Save(ByVal scaleLabelType As ScaleLabelTypeBO) As Boolean

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing
            Dim isSuccess As Boolean = True

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ID"
                currentParam.Value = scaleLabelType.ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Description"
                currentParam.Value = scaleLabelType.Description
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                results = factory.GetStoredProcedureDataReader("Scale_CheckForDuplicateLabelType", paramList)
                While results.Read
                    If results.GetInt32(results.GetOrdinal("DuplicateCount")) > 0 Then
                        ' this is a duplicate
                        isSuccess = False
                        Exit While
                    End If
                End While
                results.Close()

                If isSuccess Then
                    currentParam = New DBParam
                    currentParam.Name = "LinesPerLabel"
                    currentParam.Value = scaleLabelType.LinesPerLabel
                    currentParam.Type = DBParamType.Int
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Characters"
                    currentParam.Value = scaleLabelType.CharsPerLine
                    currentParam.Type = DBParamType.Int
                    paramList.Add(currentParam)

                    ' Execute the stored procedure 
                    factory.ExecuteStoredProcedure("Scale_InsertUpdateLabelType", paramList)
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
                factory.ExecuteStoredProcedure("Scale_DeleteLabelType", paramList)

            Catch ex As Exception
                If Err.Number = 5 Then
                    MsgBox("Unable to delete.  Make sure this Label Type is not associated with any items before deleting.", MsgBoxStyle.Critical, "Scale Label Type Delete")
                Else
                    Throw ex
                End If
            End Try

        End Sub

    End Class

End Namespace
