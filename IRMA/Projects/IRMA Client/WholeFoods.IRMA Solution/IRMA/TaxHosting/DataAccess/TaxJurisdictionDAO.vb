Imports System.Data.SqlClient
Imports WholeFoods.IRMA.TaxHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.TaxHosting.DataAccess
    Public Class TaxJurisdictionDAO

        ''' <summary>
        ''' Read the complete list of TaxJurisdiction objects.
        ''' </summary>
        ''' <exception cref="DataFactoryException" />
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetJurisdictions() As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            ' Execute the stored procedure 
            Return factory.GetStoredProcedureDataSet("TaxHosting_GetTaxJurisdictions")
        End Function

        ''' <summary>
        ''' Read complete list of TaxJurisdiction data and return ArrayList of TaxJurisdictionBO objects
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetJurisdictionList() As ArrayList
            Dim jurisdictionList As New ArrayList
            Dim jurisdictionBO As TaxJurisdictionBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("TaxHosting_GetTaxJurisdictions")

                While results.Read
                    jurisdictionBO = New TaxJurisdictionBO()
                    jurisdictionBO.TaxJurisdictionId = results.GetInt32(results.GetOrdinal("TaxJurisdictionID"))
                    jurisdictionBO.TaxJurisdictionDesc = results.GetString(results.GetOrdinal("TaxJurisdictionDesc"))
                    If (Not results.IsDBNull(results.GetOrdinal("RegionalJurisdictionID"))) Then
                        jurisdictionBO.RegionalJurisdictionID = results.GetString(results.GetOrdinal("RegionalJurisdictionID"))
                    End If
                    jurisdictionList.Add(jurisdictionBO)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return jurisdictionList
        End Function

        ''' <summary>
        ''' gets list of tax jurisdictions for a given tax class ID.  existing TaxFlag data must be setup to have associated TaxJurisdictions
        ''' </summary>
        ''' <param name="taxClassID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetJurisdictionList(ByVal taxClassID As Integer) As ArrayList
            Dim jurisdictionList As New ArrayList
            Dim jurisdictionBO As TaxJurisdictionBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "TaxClassID"
                currentParam.Value = taxClassID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("TaxHosting_GetTaxJurisdictionsForTaxClass", paramList)

                While results.Read
                    jurisdictionBO = New TaxJurisdictionBO()
                    jurisdictionBO.TaxJurisdictionId = results.GetInt32(results.GetOrdinal("TaxJurisdictionID"))
                    jurisdictionBO.TaxJurisdictionDesc = results.GetString(results.GetOrdinal("TaxJurisdictionDesc"))
                    jurisdictionList.Add(jurisdictionBO)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return jurisdictionList
        End Function

        ''' <summary>
        ''' Save changes to the DataSet to the database (inserts, updates, deletes)
        ''' </summary>
        ''' <param name="dataSet"></param>
        ''' <remarks></remarks>
        Public Sub SaveJurisdictions(ByRef dataSet As DataSet)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            factory.UpdateDataSet(dataSet, "TaxHosting_GetTaxJurisdictions", True)
        End Sub

    End Class
End Namespace
