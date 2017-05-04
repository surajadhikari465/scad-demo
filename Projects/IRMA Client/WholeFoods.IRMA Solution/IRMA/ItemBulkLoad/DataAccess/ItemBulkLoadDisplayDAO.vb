Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ItemBulkLoad.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ItemBulkLoad.DataAccess

    Public Class ItemBulkLoadDisplayDAO

        Public Function GetValidItems(ByVal itemUploadHeaderID As Integer) As Collection
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim reader As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim validItems As Collection = Nothing

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ItemUploadHeader_ID"
                If itemUploadHeaderID <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = itemUploadHeaderID
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                reader = factory.GetStoredProcedureDataReader("GetItemUploadDetails", paramList)

                ' Get the collection of valid items
                If reader.HasRows Then
                    validItems = GetCollection(reader)
                End If

                '' Get a collection of invalid items that are invalid as a result of invalid identifiers
                'reader.NextResult()
                'If reader.HasRows = True Then
                '    IdentifierInvalidItems = GetCollection(reader)
                'End If

                '' Get the collection of invalid items that are invalid as a result of check to see if the user
                '' has access to those sub teams.
                'reader.NextResult()
                'If reader.HasRows = True Then
                '    InvalidSubTeamItems = GetCollection(reader)
                'End If
            Catch ex As Exception
                'TODO handle exception
                Throw ex
            End Try

            Return validItems
        End Function

        Private Function GetCollection(ByVal reader As SqlDataReader) As Collection
            Dim obj As ItemUploadDetailBO
            Dim coll As New Collection

            While reader.Read
                obj = New ItemUploadDetailBO

                obj.ItemUploadDetail_ID = reader.GetInt32(reader.GetOrdinal("ItemUploadDetail_ID"))

                If (reader.IsDBNull(reader.GetOrdinal("ItemIdentifier"))) Then
                    obj.ItemIdentifier = Nothing
                Else
                    obj.ItemIdentifier = reader.GetString(reader.GetOrdinal("ItemIdentifier"))
                End If

                If (reader.IsDBNull(reader.GetOrdinal("POSDescription"))) Then
                    obj.PosDescription = Nothing
                Else
                    obj.PosDescription = reader.GetString(reader.GetOrdinal("POSDescription"))
                End If

                If (reader.IsDBNull(reader.GetOrdinal("Description"))) Then
                    obj.Description = Nothing
                Else
                    obj.Description = reader.GetString(reader.GetOrdinal("Description"))
                End If

                If (reader.IsDBNull(reader.GetOrdinal("TaxClassID"))) Then
                    obj.TaxClassID = Nothing
                Else
                    obj.TaxClassID = reader.GetString(reader.GetOrdinal("TaxClassID"))
                End If

                If (reader.IsDBNull(reader.GetOrdinal("FoodStamps"))) Then
                    obj.FoodStamps = Nothing
                Else
                    obj.FoodStamps = reader.GetString(reader.GetOrdinal("FoodStamps"))
                End If

                If (reader.IsDBNull(reader.GetOrdinal("RestrictedHours"))) Then
                    obj.RestrictedHours = Nothing
                Else
                    obj.RestrictedHours = reader.GetString(reader.GetOrdinal("RestrictedHours"))
                End If

                If (reader.IsDBNull(reader.GetOrdinal("EmployeeDiscountable"))) Then
                    obj.EmployeeDiscountable = Nothing
                Else
                    obj.EmployeeDiscountable = reader.GetString(reader.GetOrdinal("EmployeeDiscountable"))
                End If

                If (reader.IsDBNull(reader.GetOrdinal("Discontinued"))) Then
                    obj.Discontinued = Nothing
                Else
                    obj.Discontinued = reader.GetString(reader.GetOrdinal("Discontinued"))
                End If

                If (reader.IsDBNull(reader.GetOrdinal("NationalClassID"))) Then
                    obj.NationalClassID = Nothing
                Else
                    obj.NationalClassID = reader.GetString(reader.GetOrdinal("NationalClassID"))
                End If

                If (reader.IsDBNull(reader.GetOrdinal("SubTeam_No"))) Then
                    obj.SubTeamNo = Nothing
                Else
                    obj.SubTeamNo = reader.GetInt32(reader.GetOrdinal("SubTeam_No"))
                End If

                If (reader.IsDBNull(reader.GetOrdinal("ItemIdentifierValid"))) Then
                    obj.ItemIdentifierValid = Nothing
                Else
                    obj.ItemIdentifierValid = CInt(IIf(CInt(reader.GetBoolean(reader.GetOrdinal("ItemIdentifierValid"))) < 0, 1, 0))
                End If

                If (reader.IsDBNull(reader.GetOrdinal("SubTeamAllowed"))) Then
                    obj.SubTeamAllowed = Nothing
                Else
                    obj.SubTeamAllowed = CInt(IIf(CInt(reader.GetBoolean(reader.GetOrdinal("SubTeamAllowed"))) < 0, 1, 0))
                End If

                If (reader.IsDBNull(reader.GetOrdinal("SubTeam_Name"))) Then
                    obj.SubTeamName = Nothing
                Else
                    obj.SubTeamName = reader.GetString(reader.GetOrdinal("SubTeam_Name"))
                End If

                If (reader.IsDBNull(reader.GetOrdinal("Uploaded"))) Then
                    obj.Uploaded = Nothing
                Else
                    obj.Uploaded = reader.GetBoolean(reader.GetOrdinal("Uploaded"))
                End If

                If (reader.IsDBNull(reader.GetOrdinal("Item_Key"))) Then
                    obj.Item_Key = Nothing
                Else
                    obj.Item_Key = reader.GetInt32(reader.GetOrdinal("Item_Key"))
                End If

                coll.Add(obj)
            End While

            Return coll
        End Function

    End Class

End Namespace
