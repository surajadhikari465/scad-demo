Imports System.Data.SqlClient
Imports System.Text
Imports SLIM.WholeFoods.IRMA.Common.BusinessLogic
Imports SLIM.WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Common.DataAccess

    Public Class ValidationDAO
        ''' <summary>
        ''' This function checks to see if a validation code is associated with an error message.
        ''' * ERROR type returns TRUE
        ''' * WARNING or VALID type returns FALSE
        ''' </summary>
        ''' <param name="valCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function IsErrorCode(ByVal valCode As Integer) As Boolean
            Dim isError As Boolean = False
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New StringBuilder

            If valCode <> 0 Then
                ' This is not the success code.  Check to see if it is a warning.
                ' setup parameters for function
                paramList.Append(valCode)

                ' Execute the function to see if this is a warning message.
                If Not CBool(factory.ExecuteScalar("SELECT dbo.fn_IsWarningValidationCode(" & paramList.ToString & ")")) Then
                    isError = True
                End If
            End If

            Return isError
        End Function

        ''' <summary>
        ''' Reads the details from the database for a particular ValidationCode.
        ''' </summary>
        ''' <returns>InstanceDataBO</returns>
        ''' <remarks></remarks>
        Public Shared Function GetValidationCodeDetails(ByVal valCode As Integer) As ValidationBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim validation As New ValidationBO
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ValidationCode"
                currentParam.Value = valCode
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetValidationCodeDetails", paramList)

                While results.Read
                    validation.ValidationCode = valCode

                    If results.GetValue(results.GetOrdinal("ValidationCodeType")).GetType IsNot GetType(DBNull) Then
                        validation.ValidationCodeType = results.GetInt32(results.GetOrdinal("ValidationCodeType"))
                    End If

                    If results.GetValue(results.GetOrdinal("ValidationCodeType_Description")).GetType IsNot GetType(DBNull) Then
                        validation.ValidationCodeDesc = results.GetString(results.GetOrdinal("ValidationCodeType_Description"))
                    End If

                    If results.GetValue(results.GetOrdinal("ValidationCode_Description")).GetType IsNot GetType(DBNull) Then
                        validation.ValidationCodeTypeDesc = results.GetString(results.GetOrdinal("ValidationCode_Description"))
                    End If
                End While
            Catch ex As Exception
                Throw ex
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try
            Return validation
        End Function
    End Class
End Namespace

