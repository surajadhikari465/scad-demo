Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ItemHosting.DataAccess
    Public Class ScaleGradeDAO

        Public Shared Function GetComboList() As ArrayList
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim scaleGrade As ScaleGradeBO
            Dim scaleGradeList As New ArrayList
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Scale_GetGrades")

                While results.Read
                    scaleGrade = New ScaleGradeBO()
                    scaleGrade.ID = results.GetInt32(results.GetOrdinal("Scale_Grade_ID"))
                    scaleGrade.Description = results.GetString(results.GetOrdinal("Description"))
                    If (Not results.IsDBNull(results.GetOrdinal("Zone1"))) Then
                        scaleGrade.Zone1 = CInt(results.GetValue(results.GetOrdinal("Zone1")))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Zone2"))) Then
                        scaleGrade.Zone2 = CInt(results.GetValue(results.GetOrdinal("Zone2")))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Zone3"))) Then
                        scaleGrade.Zone3 = CInt(results.GetValue(results.GetOrdinal("Zone3")))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Zone4"))) Then
                        scaleGrade.Zone4 = CInt(results.GetValue(results.GetOrdinal("Zone4")))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Zone5"))) Then
                        scaleGrade.Zone5 = CInt(results.GetValue(results.GetOrdinal("Zone5")))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Zone6"))) Then
                        scaleGrade.Zone6 = CInt(results.GetValue(results.GetOrdinal("Zone6")))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Zone7"))) Then
                        scaleGrade.Zone7 = CInt(results.GetValue(results.GetOrdinal("Zone7")))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Zone8"))) Then
                        scaleGrade.Zone8 = CInt(results.GetValue(results.GetOrdinal("Zone8")))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Zone9"))) Then
                        scaleGrade.Zone9 = CInt(results.GetValue(results.GetOrdinal("Zone9")))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Zone10"))) Then
                        scaleGrade.Zone10 = CInt(results.GetValue(results.GetOrdinal("Zone10")))
                    End If
                    scaleGradeList.Add(scaleGrade)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return scaleGradeList
        End Function
        Public Shared Function Save(ByVal scaleGrade As ScaleGradeBO) As Boolean

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing
            Dim isSuccess As Boolean = True

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ID"
                currentParam.Value = scaleGrade.ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Description"
                currentParam.Value = scaleGrade.Description
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                results = factory.GetStoredProcedureDataReader("Scale_CheckForDuplicateGrade", paramList)
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
                    currentParam.Name = "Zone1"
                    currentParam.Value = scaleGrade.Zone1
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Zone2"
                    currentParam.Value = scaleGrade.Zone2
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Zone3"
                    currentParam.Value = scaleGrade.Zone3
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Zone4"
                    currentParam.Value = scaleGrade.Zone4
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Zone5"
                    currentParam.Value = scaleGrade.Zone5
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Zone6"
                    currentParam.Value = scaleGrade.Zone6
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Zone7"
                    currentParam.Value = scaleGrade.Zone7
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Zone8"
                    currentParam.Value = scaleGrade.Zone8
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Zone9"
                    currentParam.Value = scaleGrade.Zone9
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Zone10"
                    currentParam.Value = scaleGrade.Zone10
                    currentParam.Type = DBParamType.SmallInt
                    paramList.Add(currentParam)

                    ' Execute the stored procedure 
                    factory.ExecuteStoredProcedure("Scale_InsertUpdateGrade", paramList)
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
                factory.ExecuteStoredProcedure("Scale_DeleteGrade", paramList)

            Catch ex As Exception
                If Err.Number = 5 Then
                    MsgBox("Unable to delete.  Make sure this Grade is not associated with any items before deleting.", MsgBoxStyle.Critical, "Scale Grade Delete")
                Else
                    Throw ex
                End If
            End Try

        End Sub
    End Class

End Namespace
