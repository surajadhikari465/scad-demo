Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Text
Imports System.Web
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ItemChaining.DataAccess
    Public Class Item
        Public SearchHelper As New ItemSearchHelper

        Public Function Search() As ItemSearchResults
            Dim results As New ItemSearchResults()
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim cmd As SqlCommand = factory.GetDataCommand("GetItemSearch", Nothing, True)
            
            Me.SearchHelper.AddParameters(cmd.Parameters)

            Using adapter As SqlDataAdapter = factory.GetDataAdapter(cmd, Nothing)
                adapter.Fill(results.Results)
            End Using

            Return results
        End Function

#Region "Helper Class"

        Public Class ItemSearchHelper

#Region "Member variables"

            Public SubTeam_No As New Nullable(Of Integer)
            Public Category_ID As New Nullable(Of Integer)
            Public Level3_ID As New Nullable(Of Integer)
            Public Level4_ID As New Nullable(Of Integer)
            Public Chain_ID As New Nullable(Of Integer)
            Public Vendor As String
            Public Vendor_ID As String
            Public Brand_ID As String
            Public DistSubTeam_No As String
            Public Item_Description As String
            Public Identifier As String
            Public Discontinue_Item As New Nullable(Of Boolean)
            Public WFMItems As New Nullable(Of Boolean)
            Public HFM As New Nullable(Of Boolean)
            Public IncludeDeletedItems As New Nullable(Of Boolean)
            Public NotAvailable As New Nullable(Of Boolean)
            Public Vendor_PS As String

#End Region

#Region "Helper Methods"

            Private Function CreateValueParameter(ByVal name As String, ByVal type As SqlDbType, ByVal value As Object) As SqlParameter
                Dim param As New SqlParameter(name, type)

                param.Value = value

                Return param
            End Function

            Private Sub NonDefaultIntegerParameter(ByVal value As Nullable(Of Integer), ByVal name As String, ByVal parameters As SqlParameterCollection)
                If value.HasValue Then
                    parameters.Add(CreateValueParameter(name, SqlDbType.Int, value.Value))
                End If
            End Sub

            Private Sub NonDefaultStringParameter(ByVal value As String, ByVal name As String, ByVal parameters As SqlParameterCollection)
                If Not String.IsNullOrEmpty(value) Then
                    parameters.Add(CreateValueParameter(name, SqlDbType.VarChar, value))
                End If
            End Sub

            Private Sub NonDefaultBooleanParameter(ByVal value As Nullable(Of Boolean), ByVal name As String, ByVal queryStringBuilder As StringBuilder)
                If value.HasValue Then
                    queryStringBuilder.AppendFormat("&{0}={1}", name, IIf(value.Value, "1", "0"))
                End If
            End Sub

            Private Sub NonDefaultIntegerParameter(ByVal value As Nullable(Of Integer), ByVal name As String, ByVal queryStringBuilder As StringBuilder)
                If value.HasValue Then
                    queryStringBuilder.AppendFormat("&{0}={1}", name, value.Value)
                End If
            End Sub

            Private Sub NonDefaultStringParameter(ByVal value As String, ByVal name As String, ByVal queryStringBuilder As StringBuilder)
                If Not String.IsNullOrEmpty(value) Then
                    queryStringBuilder.AppendFormat("&{0}={1}", name, HttpUtility.UrlEncode(value))
                End If
            End Sub

#End Region

            Private Sub PrepareValues()
                If Identifier IsNot Nothing Then
                    Identifier = Trim(Identifier)
                End If

                If Vendor_ID IsNot Nothing Then
                    Vendor_ID = Trim(Vendor_ID)
                End If
            End Sub

            Public Sub AddParameters(ByVal parameters As SqlParameterCollection)
                PrepareValues()

                With parameters
                    .Add(CreateValueParameter("SubTeam_No", SqlDbType.Int, SubTeam_No.Value))
                    .Add(CreateValueParameter("Category_ID", SqlDbType.Int, Category_ID.Value))
                    .Add(CreateValueParameter("Vendor", SqlDbType.VarChar, Vendor))
                    .Add(CreateValueParameter("Vendor_ID", SqlDbType.VarChar, Vendor_ID))
                    .Add(CreateValueParameter("Item_Description", SqlDbType.VarChar, Item_Description))
                    .Add(CreateValueParameter("Identifier", SqlDbType.VarChar, Identifier))
                    .Add(CreateValueParameter("Discontinue_Item", SqlDbType.Int, Discontinue_Item.Value))
                    .Add(CreateValueParameter("WFM_Item", SqlDbType.Int, WFMItems.Value))
                    .Add(CreateValueParameter("Not_Available", SqlDbType.Int, NotAvailable.Value))
                    .Add(CreateValueParameter("HFM_Item", SqlDbType.SmallInt, HFM.Value))
                    .Add(CreateValueParameter("IncludeDeletedItems", SqlDbType.SmallInt, IncludeDeletedItems.Value))
                    .Add(CreateValueParameter("Brand_ID", SqlDbType.Int, IIf(Brand_ID = "", DBNull.Value, Brand_ID)))
                    .Add(CreateValueParameter("DistSubTeam_No", SqlDbType.Int, IIf(DistSubTeam_No = "", DBNull.Value, DistSubTeam_No)))
                    .Add(CreateValueParameter("LinkCodeID", SqlDbType.Int, DBNull.Value))
                    .Add(CreateValueParameter("ProdHierarchyLevel3_ID", SqlDbType.Int, Level3_ID.Value))
                    .Add(CreateValueParameter("ProdHierarchyLevel4_ID", SqlDbType.Int, Level4_ID.Value))
                    .Add(CreateValueParameter("Chain_ID", SqlDbType.Int, Chain_ID.Value))
                    .Add(CreateValueParameter("Vendor_PS", SqlDbType.VarChar, Vendor_PS))
                End With
            End Sub

            Public Sub AddOnlyNonDefaultParameters(ByVal parameters As SqlParameterCollection)
                PrepareValues()

                NonDefaultIntegerParameter(SubTeam_No, "SubTeam_No", parameters)
                NonDefaultIntegerParameter(Category_ID, "Category_ID", parameters)
                NonDefaultIntegerParameter(Level3_ID, "ProdHierarchyLevel3_ID", parameters)
                NonDefaultIntegerParameter(Level4_ID, "ProdHierarchyLevel4_ID", parameters)
                NonDefaultIntegerParameter(Chain_ID, "Chain_ID", parameters)

                NonDefaultStringParameter(Vendor, "Vendor", parameters)
                NonDefaultStringParameter(Vendor_ID, "Vendor_ID", parameters)
                NonDefaultStringParameter(Item_Description, "Item_Description", parameters)
                NonDefaultStringParameter(Identifier, "Identifier", parameters)
                NonDefaultStringParameter(Vendor_PS, "Vendor_PS", parameters)

                With parameters
                    If Not String.IsNullOrEmpty(Brand_ID) Then
                        .Add(CreateValueParameter("Brand_ID", SqlDbType.Int, Brand_ID))
                    End If

                    If Not String.IsNullOrEmpty(DistSubTeam_No) Then
                        .Add(CreateValueParameter("DistSubTeam_No", SqlDbType.Int, DistSubTeam_No))
                    End If

                    If Discontinue_Item.HasValue Then
                        .Add(CreateValueParameter("Discontinue_Item", SqlDbType.Int, Discontinue_Item))
                    End If

                    If WFMItems.HasValue Then
                        .Add(CreateValueParameter("WFM_Item", SqlDbType.Int, WFMItems))
                    End If

                    If NotAvailable.HasValue Then
                        .Add(CreateValueParameter("Not_Available", SqlDbType.Int, NotAvailable))
                    End If

                    If HFM.HasValue Then
                        .Add(CreateValueParameter("HFM_Item", SqlDbType.SmallInt, HFM))
                    End If

                    If IncludeDeletedItems.HasValue Then
                        .Add(CreateValueParameter("IncludeDeletedItems", SqlDbType.SmallInt, IncludeDeletedItems))
                    End If
                End With
            End Sub

            Public Sub AddOnlyNonDefaultParameters(ByVal queryStringBuilder As StringBuilder)
                PrepareValues()

                NonDefaultIntegerParameter(SubTeam_No, "SubTeam_No", queryStringBuilder)
                NonDefaultIntegerParameter(Category_ID, "Category_ID", queryStringBuilder)
                NonDefaultIntegerParameter(Level3_ID, "ProdHierarchyLevel3_ID", queryStringBuilder)
                NonDefaultIntegerParameter(Level4_ID, "ProdHierarchyLevel4_ID", queryStringBuilder)
                NonDefaultIntegerParameter(Chain_ID, "Chain_ID", queryStringBuilder)

                NonDefaultStringParameter(Vendor, "Vendor", queryStringBuilder)
                NonDefaultStringParameter(Vendor_ID, "Vendor_ID", queryStringBuilder)
                NonDefaultStringParameter(Item_Description, "Item_Description", queryStringBuilder)
                NonDefaultStringParameter(Identifier, "Identifier", queryStringBuilder)
                NonDefaultStringParameter(Vendor_PS, "Vendor_PS", queryStringBuilder)
                NonDefaultStringParameter(Brand_ID, "Brand_ID", queryStringBuilder)
                NonDefaultStringParameter(DistSubTeam_No, "DistSubTeam_No", queryStringBuilder)

                NonDefaultBooleanParameter(Discontinue_Item, "Discontinue_Item", queryStringBuilder)
                NonDefaultBooleanParameter(WFMItems, "WFM_Item", queryStringBuilder)
                NonDefaultBooleanParameter(NotAvailable, "Not_Available", queryStringBuilder)
                NonDefaultBooleanParameter(HFM, "HFM_Item", queryStringBuilder)
                NonDefaultBooleanParameter(IncludeDeletedItems, "IncludeDeletedItems", queryStringBuilder)
            End Sub
        End Class

#End Region

    End Class
End Namespace