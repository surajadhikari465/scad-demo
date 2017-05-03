Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Replenishment.Common.DataAccess

    Public Class PricingMethodsDAO

        ''' <summary>
        ''' gets data in InstanceData table describing the current region's settings
        ''' </summary>
        ''' <returns>InstanceDataBO</returns>
        ''' <remarks></remarks>
        Public Shared Function GetPricingMethods() As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList

            ' Execute the stored procedure 
            Return factory.GetStoredProcedureDataSet("PricingMethodList", paramList)
        End Function

        Public Shared Function GetPricingMethodMapings() As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList

            ' Execute the stored procedure 
            Return factory.GetStoredProcedureDataSet("GetPricingMethodMappings", paramList)
        End Function

        Public Sub InsertPricingMethodMapping(ByVal pricingMethod As PricingMethodsBO)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "POSFileWriterKey"
                currentParam.Value = pricingMethod.PricingMethodFileWriterKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "PricingMethod_Key"
                currentParam.Value = pricingMethod.PricingMethodKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "PricingMethod_ID"
                currentParam.Value = pricingMethod.PricingMethodID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("InsertPricingMethodMapping", paramList)
            Catch ex As Exception
                If InStr(1, ex.InnerException.Message, "Violation of PRIMARY") > 0 Then
                    MsgBox("This File Friter and Pricing Method Type combination already exists.", MsgBoxStyle.Critical, "Manage Pricing Methods")
                    Exit Sub
                Else
                    Throw ex
                End If
            End Try
        End Sub

        Public Sub RemovePricingMethodMapping(ByVal pricingMethod As PricingMethodsBO)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "POSFileWriterKey"
                currentParam.Value = pricingMethod.PricingMethodFileWriterKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "PricingMethod_Key"
                currentParam.Value = pricingMethod.PricingMethodKey
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "PricingMethod_ID"
                currentParam.Value = pricingMethod.PricingMethodID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                factory.ExecuteStoredProcedure("DeletePricingMethodMapping", paramList)
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
    End Class

End Namespace
