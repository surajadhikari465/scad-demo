Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Imports log4net

Namespace WholeFoods.IRMA.ItemHosting.DataAccess
    Public Class ScaleRandomWeightTypeDAO

        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


        Public Shared Function GetComboList() As ArrayList
            logger.Debug("GetComboList Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim scaleRandomWeightType As ScaleRandomWeightTypeBO
            Dim scaleRandomWeightTypeList As New ArrayList
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("Scale_GetRandomWeightTypes")

                While results.Read
                    scaleRandomWeightType = New ScaleRandomWeightTypeBO()
                    scaleRandomWeightType.ID = results.GetInt32(results.GetOrdinal("Scale_RandomWeightType_ID"))
                    scaleRandomWeightType.Description = results.GetString(results.GetOrdinal("Description"))

                    scaleRandomWeightTypeList.Add(scaleRandomWeightType)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetComboList Exit")

            Return scaleRandomWeightTypeList
        End Function
        Public Shared Function Save(ByVal scaleRandomWeightType As ScaleRandomWeightTypeBO) As Boolean

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
                currentParam.Value = scaleRandomWeightType.ID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Description"
                currentParam.Value = scaleRandomWeightType.Description
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                results = factory.GetStoredProcedureDataReader("Scale_CheckForDuplicateRandomWeightType", paramList)
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
                    factory.ExecuteStoredProcedure("Scale_InsertUpdateRandomWeightType", paramList)
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
                factory.ExecuteStoredProcedure("Scale_DeleteRandomWeightType", paramList)

            Catch ex As Exception
                If Err.Number = 5 Then
                    MsgBox("Unable to delete.  Make sure this Random Weight Type is not associated with any items before deleting.", MsgBoxStyle.Critical, "Scale Random Weight Type Delete")
                Else
                    Throw ex
                End If
            End Try

            logger.Debug("Delete Exit")

        End Sub
    End Class

End Namespace
