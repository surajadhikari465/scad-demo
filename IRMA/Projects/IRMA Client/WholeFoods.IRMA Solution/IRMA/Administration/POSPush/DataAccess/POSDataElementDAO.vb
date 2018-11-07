Imports WholeFoods.IRMA.Administration.POSPush.BusinessLogic
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Administration.POSPush.DataAccess
    Public Class POSDataElementDAO
#Region "Read Methods"
        ''' <summary>
        ''' Read complete list of data element objects for the POSDataTypeKey.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function GetPOSDataValues(ByVal POSDataTypeKey As Integer) As DataSet
            Return GetPOSDataValues(POSDataTypeKey, False)
        End Function

        ''' <summary>
        ''' Read complete list of data element objects for the POSDataTypeKey.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function GetPOSDataValues(ByVal POSDataTypeKey As Integer, ByVal BooleanElementsOnly As Boolean) As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            ' setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "POSDataTypeKey"
            currentParam.Value = POSDataTypeKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "BooleanOnly"
            currentParam.Value = IIf(BooleanElementsOnly, 1, 0)
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            Return factory.GetStoredProcedureDataSet("Administration_POSPush_GetPOSDataElement", paramList)
        End Function

        ''' <summary>
        ''' Read the list of distinct TaxFlagKey values from the TaxFlag table.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetTaxFlagKeys() As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            ' Execute the stored procedure
            Return factory.GetStoredProcedureDataSet("Administration_POSPush_GetTaxFlagKeys")
        End Function
#End Region
    End Class
End Namespace

