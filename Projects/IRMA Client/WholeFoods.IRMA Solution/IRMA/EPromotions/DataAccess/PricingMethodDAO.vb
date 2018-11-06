
Option Explicit On
Option Strict On

Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports WholeFoods.IRMA.EPromotions.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.EPromotions.DataAccess
    Public Class PricingMethodDAO
        ''' <summary>
        ''' Read complete list of Pricing Method data and return ArrayList of PricingMethodBO objects
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPricingMethodList(Optional ByVal PricingMethodID As Integer = -1, Optional ByVal OfferEditorOnly As Boolean = False) As BindingList(Of PricingMethodBO)
            Dim pricingmethodList As New BindingList(Of PricingMethodBO)
            Dim pricingmethodBO As PricingMethodBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing

            Try
                ' setup parameters for stored proc;  0 returns all values
                Dim paramList As New ArrayList
                Dim currentParam As DBParam

                currentParam = New DBParam
                currentParam.Name = "PricingMethodID"
                currentParam.Value = PricingMethodID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "OfferEditorOnly"
                currentParam.Value = OfferEditorOnly
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("EPromotions_GetPricingMethods", paramList)

                While results.Read

                    pricingmethodBO = New PricingMethodBO()
                    With pricingmethodBO

                        If (Not results.IsDBNull(results.GetOrdinal("PricingMethod_ID"))) Then
                            .PricingMethodID = results.GetByte(results.GetOrdinal("PricingMethod_ID"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("PricingMethod_Name"))) Then
                            .Name = results.GetString(results.GetOrdinal("PricingMethod_Name"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("EnableOfferEditor"))) Then
                            .EnableOfferEditor = results.GetBoolean(results.GetOrdinal("EnableOfferEditor"))
                        End If
                    End With

                    pricingmethodList.Add(pricingmethodBO)

                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return pricingmethodList
        End Function


        Public Function GetPromoScreenPricingMethodList(Optional ByVal PricingMethodID As Integer = -1, Optional ByVal OfferEditorOnly As Boolean = False) As BindingList(Of PricingMethodBO)
            ' This function is only used on the PriceChanges screen. Returns all pricingmethods that do not belong to Epromotions.

            Dim pricingmethodList As New BindingList(Of PricingMethodBO)
            Dim pricingmethodBO As PricingMethodBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            'Dim include As Boolean  
            '  The hard code to exclude BOGO from UK is removed
            '  This is table driven and can be set correctly for each region  

            Try

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetPricingMethodInfo")

                While results.Read

                    pricingmethodBO = New PricingMethodBO()
                    With pricingmethodBO

                        If (Not results.IsDBNull(results.GetOrdinal("PricingMethod_ID"))) Then
                            .PricingMethodID = results.GetByte(results.GetOrdinal("PricingMethod_ID"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("PricingMethod_Name"))) Then
                            .Name = results.GetString(results.GetOrdinal("PricingMethod_Name"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("EnableOfferEditor"))) Then
                            .EnableOfferEditor = results.GetBoolean(results.GetOrdinal("EnableOfferEditor"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("EnablePromoScreen"))) Then
                            .EnablePromoScreen = results.GetBoolean(results.GetOrdinal("EnablePromoScreen"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("EnableSaleMultiple"))) Then
                            .EnableSaleMultiple = results.GetBoolean(results.GetOrdinal("EnableSaleMultiple"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("EnableEarnedRegMultiple"))) Then
                            .EnableEarnedRegMultiple = results.GetBoolean(results.GetOrdinal("EnableEarnedRegMultiple"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("EarnedRegMultipleDefault"))) Then
                            .EarnedRegMultipleDefault = results.GetInt32(results.GetOrdinal("EarnedRegMultipleDefault"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("EnableEarnedSaleMultiple"))) Then
                            .EnableEarnedSaleMultiple = results.GetBoolean(results.GetOrdinal("EnableEarnedSaleMultiple"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("EarnedSaleMultipleDefault"))) Then
                            .EarnedSaleMultipleDefault = results.GetInt32(results.GetOrdinal("EarnedSaleMultipleDefault"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("EnableEarnedLimit"))) Then
                            .EnableEarnedLimit = results.GetBoolean(results.GetOrdinal("EnableEarnedLimit"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("EarnedLimitDefault"))) Then
                            .EarnedLimitDefault = results.GetInt32(results.GetOrdinal("EarnedLimitDefault"))
                        End If

                        pricingmethodList.Add(pricingmethodBO)

                    End With

                End While
            Catch e As Exception
                Debug.WriteLine(e.InnerException.Message)
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If

            End Try

            Return pricingmethodList
        End Function


    End Class
End Namespace

