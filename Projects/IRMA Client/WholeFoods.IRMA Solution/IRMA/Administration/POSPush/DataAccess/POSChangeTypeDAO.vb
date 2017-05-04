Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Administration.POSPush.BusinessLogic
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Administration.POSPush.DataAccess
    Public Class POSChangeTypeDAO
        ''' <summary>
        ''' _changeTypes contains constant configuration data that only needs to be populated once
        ''' </summary>
        ''' <remarks>the key is POSChangeType; the value is POSChangeTypeBO</remarks>
        Private Shared _changeTypes As Hashtable

        ''' <summary>
        ''' Initialize the _changeTypes Hashtable.
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared Sub InitializeChangeTypes()
            If _changeTypes Is Nothing Or (_changeTypes IsNot Nothing AndAlso _changeTypes.Count <= 0) Then
                ' This list is static.  It is only populated the first time it's accessed.
                _changeTypes = New Hashtable
                ' Query the POSChangeType table, using the results to populate the _changeTypes hash.
                Dim factory As New DataFactory(DataFactory.ItemCatalog)
                Dim results As SqlDataReader = Nothing
                Dim currentChange As POSChangeTypeBO

                Try
                    ' Execute the stored procedure 
                    results = factory.GetStoredProcedureDataReader("Administration_POSPush_GetPOSChangeTypes")

                    ' Process each change type in the result set
                    While (results.Read())
                        currentChange = New POSChangeTypeBO(results)
                        Select Case currentChange.POSChangeTypeKey
                            Case POSChangeType.ItemChange
                                _changeTypes.Add(POSChangeType.ItemChange, currentChange)
                            Case POSChangeType.ItemDelete
                                _changeTypes.Add(POSChangeType.ItemDelete, currentChange)
                            Case POSChangeType.ItemIDAdd
                                _changeTypes.Add(POSChangeType.ItemIDAdd, currentChange)
                            Case POSChangeType.ItemIDDelete
                                _changeTypes.Add(POSChangeType.ItemIDDelete, currentChange)
                            Case POSChangeType.PromotionalData
                                _changeTypes.Add(POSChangeType.PromotionalData, currentChange)
                            Case POSChangeType.VendorAdd
                                _changeTypes.Add(POSChangeType.VendorAdd, currentChange)
                            Case POSChangeType.CorpScaleItemChange
                                _changeTypes.Add(POSChangeType.CorpScaleItemChange, currentChange)
                            Case POSChangeType.CorpScaleItemIdDelete
                                _changeTypes.Add(POSChangeType.CorpScaleItemIdDelete, currentChange)
                            Case POSChangeType.CorpScaleItemIdAdd
                                _changeTypes.Add(POSChangeType.CorpScaleItemIdAdd, currentChange)
                            Case POSChangeType.ZoneScaleItemDelete
                                _changeTypes.Add(POSChangeType.ZoneScaleItemDelete, currentChange)
                            Case POSChangeType.ZoneScalePriceChange
                                _changeTypes.Add(POSChangeType.ZoneScalePriceChange, currentChange)
                            Case POSChangeType.ShelfTagFile
                                _changeTypes.Add(POSChangeType.ShelfTagFile, currentChange)
                            Case POSChangeType.NutriFact
                                _changeTypes.Add(POSChangeType.NutriFact, currentChange)
                            Case POSChangeType.ExtraText
                                _changeTypes.Add(POSChangeType.ExtraText, currentChange)
                            Case POSChangeType.ZoneScaleSmartXPriceChange
                                _changeTypes.Add(POSChangeType.ZoneScaleSmartXPriceChange, currentChange)
                            Case POSChangeType.ElectronicShelfTag
                                _changeTypes.Add(POSChangeType.ElectronicShelfTag, currentChange)
                        End Select
                    End While

                Catch e As DataFactoryException
                    Logger.LogError("Exception: ", Nothing, e)
                    'send message about exception
                    ErrorHandler.ProcessError(ErrorType.DataFactoryException, SeverityLevel.Warning, e)
                Finally
                    ' Close the result set and the connection
                    If (results IsNot Nothing) Then
                        results.Close()
                    End If
                End Try
            End If
        End Sub

        ''' <summary>
        ''' This method returns a Hashtable POSChangeTypeBO, keyed by POSChangeType.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetChangeTypes() As Hashtable
            ' Initialize the change type values
            InitializeChangeTypes()
            ' Return the collection
            Return _changeTypes
        End Function

        ''' <summary>
        ''' This method returns the POSChangeTypeBO for the given POSChangeType.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetChangeType(ByVal changeType As POSChangeType) As POSChangeTypeBO
            ' Initialize the change type values
            InitializeChangeTypes()
            ' Return the specified value
            Return CType(_changeTypes.Item(changeType), POSChangeTypeBO)
        End Function


    End Class
End Namespace
