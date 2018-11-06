Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ItemHosting.DataAccess

    Public Class ScaleDAO

#Region "read methods"

        ''' <summary>
        ''' returns TRUE or FALSE if identifier passed in matches criteria for a scale identifier
        ''' </summary>
        ''' <param name="identifier"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' use IsScaleIdentifier rather than IsScaleItem because there's no entry in ItemIdentifier for new ones
        ''' </remarks>
        Public Shared Function IsScaleIdentifier(ByVal identifier As String) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Return CType(factory.ExecuteScalar("SELECT dbo.fn_IsScaleIdentifier('" & identifier & "')"), Boolean)
        End Function

        Public Shared Function GetScalePluItemConflicts(ByVal identifier As String, ByVal pluDigits As Integer, ByVal subTeamNo As Integer) As ArrayList
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim itemInfo As ItemSearchBO
            Dim scalePluConflicts As New ArrayList
            Dim scalePLU As String

            Try
                'TFS 6744
                'Updated the logic to send the correct non-type-2 scale PLU as parameters.

                scalePLU = ""
                If identifier.Substring(0, 1) = "2" And identifier.Length = 11 Then
                    If IIf(Trim(identifier).Length = 11, IIf(identifier.Substring(Trim(identifier).Length - 5, 5) = "00000", True, False), False) Then

                        Select Case pluDigits
                            Case Is = 4
                                scalePLU = identifier.Substring(2, 4) '0 based index in string. Index for position 3 in the string is 2.
                            Case Is = 5
                                scalePLU = identifier.Substring(1, 5)
                            Case Else
                                scalePLU = identifier.Substring(2, 4)
                        End Select
                    End If

                Else
                    If identifier.Length = 4 Then
                        scalePLU = Right(identifier, 4)
                    Else
                        If identifier.Length > 5 Then
                            scalePLU = Right(identifier, 5)
                        Else
                            scalePLU = identifier 'If the length of the non-type-2 identifier is less than 4, send the whole identifier.
                        End If

                    End If

                End If

                    ' setup parameters for stored proc
                    currentParam = New DBParam
                    currentParam.Name = "Identifier"
                    currentParam.Value = identifier
                    currentParam.Type = DBParamType.String
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "PLUDigits"
                    currentParam.Value = pluDigits
                    currentParam.Type = DBParamType.Int
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "ScalePLU"
                    currentParam.Value = scalePLU
                    currentParam.Type = DBParamType.Char
                    paramList.Add(currentParam)

                    currentParam = New DBParam
                    currentParam.Name = "SubTeam_No"
                    currentParam.Value = subTeamNo
                    currentParam.Type = DBParamType.Int
                    paramList.Add(currentParam)

                    ' Execute the stored procedure 
                    results = factory.GetStoredProcedureDataReader("GetScalePLUConflicts", paramList)

                    While results.Read
                        itemInfo = New ItemSearchBO
                        itemInfo.ItemIdentifier = results.GetString(results.GetOrdinal("Identifier"))
                        itemInfo.ItemDesc = results.GetString(results.GetOrdinal("Item_Description"))

                        scalePluConflicts.Add(itemInfo)
                    End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return scalePluConflicts
        End Function

#End Region

#Region "write methods"

#End Region

    End Class

End Namespace
