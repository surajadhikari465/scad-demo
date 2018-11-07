Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Imports log4net

Namespace WholeFoods.IRMA.ItemHosting.DataAccess
    Public Class ScaleTareDAO
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


        Public Shared Function GetComboList() As ArrayList

            logger.Debug("GetComboList Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim scaleTare As ScaleTareBO
            Dim scaleTareList As New ArrayList
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Scale_GetTares")

                While results.Read
                    scaleTare = New ScaleTareBO()
                    scaleTare.ID = results.GetInt32(results.GetOrdinal("Scale_Tare_ID"))
                    scaleTare.Description = results.GetString(results.GetOrdinal("Description"))
                    If (Not results.IsDBNull(results.GetOrdinal("Zone1"))) Then
                        scaleTare.Zone1 = results.GetDecimal(results.GetOrdinal("Zone1"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Zone2"))) Then
                        scaleTare.Zone2 = results.GetDecimal(results.GetOrdinal("Zone2"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Zone3"))) Then
                        scaleTare.Zone3 = results.GetDecimal(results.GetOrdinal("Zone3"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Zone4"))) Then
                        scaleTare.Zone4 = results.GetDecimal(results.GetOrdinal("Zone4"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Zone5"))) Then
                        scaleTare.Zone5 = results.GetDecimal(results.GetOrdinal("Zone5"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Zone6"))) Then
                        scaleTare.Zone6 = results.GetDecimal(results.GetOrdinal("Zone6"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Zone7"))) Then
                        scaleTare.Zone7 = results.GetDecimal(results.GetOrdinal("Zone7"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Zone8"))) Then
                        scaleTare.Zone8 = results.GetDecimal(results.GetOrdinal("Zone8"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Zone9"))) Then
                        scaleTare.Zone9 = results.GetDecimal(results.GetOrdinal("Zone9"))
                    End If
                    If (Not results.IsDBNull(results.GetOrdinal("Zone10"))) Then
                        scaleTare.Zone10 = results.GetDecimal(results.GetOrdinal("Zone10"))
                    End If

                    scaleTareList.Add(scaleTare)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetComboList Exit")

            Return scaleTareList
        End Function
        Public Shared Function Save(ByVal scaleTare As ScaleTareBO) As Boolean

            logger.Debug("Save Entry")


            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing
            Dim isSuccess As Boolean = True

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ID"
                currentParam.Value = scaleTare.ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Description"
                currentParam.Value = scaleTare.Description
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                results = factory.GetStoredProcedureDataReader("Scale_CheckForDuplicateTare", paramList)
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
                    currentParam.Value = scaleTare.Zone1
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Zone2"
                    currentParam.Value = scaleTare.Zone2
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Zone3"
                    currentParam.Value = scaleTare.Zone3
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Zone4"
                    currentParam.Value = scaleTare.Zone4
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Zone5"
                    currentParam.Value = scaleTare.Zone5
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Zone6"
                    currentParam.Value = scaleTare.Zone6
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Zone7"
                    currentParam.Value = scaleTare.Zone7
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Zone8"
                    currentParam.Value = scaleTare.Zone8
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Zone9"
                    currentParam.Value = scaleTare.Zone9
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "Zone10"
                    currentParam.Value = scaleTare.Zone10
                    currentParam.Type = DBParamType.Decimal
                    paramList.Add(currentParam)

                    ' Execute the stored procedure 
                    factory.ExecuteStoredProcedure("Scale_InsertUpdateTare", paramList)
                End If

            Catch ex As Exception
                Throw ex
            End Try

            logger.Debug("Save Exit")
            Return isSuccess
        End Function

        Public Shared Sub Delete(ByVal id As Integer)

            logger.Debug("Delete Entry")

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
                factory.ExecuteStoredProcedure("Scale_DeleteTare", paramList)

            Catch ex As Exception
                If Err.Number = 5 Then
                    MsgBox("Unable to delete.  Make sure this Tare is not associated with any items before deleting.", MsgBoxStyle.Critical, "Scale Tare Delete")
                Else
                    Throw ex
                End If
            End Try

            logger.Debug("Delete Exit")

        End Sub

    End Class

End Namespace
