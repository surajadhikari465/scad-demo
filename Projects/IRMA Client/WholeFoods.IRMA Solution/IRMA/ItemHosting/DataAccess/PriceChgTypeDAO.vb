Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Imports log4net

Namespace WholeFoods.IRMA.ItemHosting.DataAccess
    Public Class PriceChgTypeDAO
        Private Shared priceChg As PriceChgTypeBO = Nothing
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)



        ''' <summary>
        ''' Returns a PriceChgTypeBO object, populated with the data for the REGULAR price change type.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetRegularPriceChgTypeData() As PriceChgTypeBO

            logger.Debug("GetRegularPriceChgTypeData Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing

            Try
                ' Populate the priceChg variable, if it has not yet been populated.
                If priceChg Is Nothing Then
                    ' Execute the stored procedure 
                    results = factory.GetStoredProcedureDataReader("GetRegularPriceChgTypeData")

                    While results.Read
                        priceChg = New PriceChgTypeBO()
                        priceChg.PriceChgTypeID = CType(results.GetByte(results.GetOrdinal("PriceChgTypeId")), Integer)
                        priceChg.PriceChgTypeDesc = results.GetString(results.GetOrdinal("PriceChgTypeDesc"))
                        priceChg.Priority = results.GetInt16(results.GetOrdinal("Priority"))
                        priceChg.IsOnSale = results.GetBoolean(results.GetOrdinal("On_Sale"))
                        priceChg.IsMSRPRequired = results.GetBoolean(results.GetOrdinal("MSRP_Required"))
                        priceChg.IsLineDrive = results.GetBoolean(results.GetOrdinal("LineDrive"))
                        priceChg.IsCompetitive = results.GetBoolean(results.GetOrdinal("Competitive"))
                        Try 'Check for null value.
                            priceChg.LastUpdateTimestamp = results.GetDateTime(results.GetOrdinal("LastUpdateTimestamp"))
                        Catch ex As Exception
                            priceChg.LastUpdateTimestamp = DateTime.MinValue
                        End Try
                    End While
                End If
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetRegularPriceChgTypeData Exit")

            Return priceChg

        End Function

        ''' <summary>
        ''' returns data from PriceChgType table in form of PriceChgTypeBO objects
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetPriceChgTypeList(ByVal includeRegularPrice As Boolean, ByVal includeALL As Boolean) As ArrayList

            logger.Debug("GetPriceChgTypeList Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim priceChg As PriceChgTypeBO
            Dim priceChgList As New ArrayList
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "IncludeReg"
                currentParam.Value = includeRegularPrice
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetPriceTypes", paramList)
                Dim priceIndex As Integer
                priceIndex = -1
                If includeALL = True Then
                    priceChg = New PriceChgTypeBO()
                    priceChg.PriceChgTypeID = priceIndex
                    priceChg.PriceChgTypeDesc = "ALL"
                    priceChg.Priority = 0
                    priceChg.IsOnSale = 0
                    priceChg.IsMSRPRequired = 0
                    priceChg.IsLineDrive = 0
                    priceChg.IsCompetitive = 0
                    priceChg.LastUpdateTimestamp = DateTime.MinValue
                    priceChgList.Add(priceChg)
                End If

                While results.Read
                    priceChg = New PriceChgTypeBO()
                    priceChg.PriceChgTypeID = CType(results.GetByte(results.GetOrdinal("PriceChgTypeId")), Integer)
                    priceChg.PriceChgTypeDesc = results.GetString(results.GetOrdinal("PriceChgTypeDesc"))
                    priceChg.Priority = results.GetInt16(results.GetOrdinal("Priority"))
                    priceChg.IsOnSale = results.GetBoolean(results.GetOrdinal("On_Sale"))
                    priceChg.IsMSRPRequired = results.GetBoolean(results.GetOrdinal("MSRP_Required"))
                    priceChg.IsLineDrive = results.GetBoolean(results.GetOrdinal("LineDrive"))
                    priceChg.IsCompetitive = results.GetBoolean(results.GetOrdinal("Competitive"))
                    Try 'Check for null value.
                        priceChg.LastUpdateTimestamp = results.GetDateTime(results.GetOrdinal("LastUpdateTimestamp"))
                    Catch ex As Exception
                        priceChg.LastUpdateTimestamp = DateTime.MinValue
                    End Try

                    priceChgList.Add(priceChg)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetPriceChgTypeList Exit")

            Return priceChgList
        End Function

        'Public Function GetPriceChangeList() As Integer()
        '    logger.Debug("GetPriceChangeList Enter")
        '    Dim result() As Integer
        '    Dim i As Integer

        '    mrsStore.Filter = ADODB.FilterGroupEnum.adFilterNone
        '    ReDim result(mrsStore.RecordCount - 1)
        '    For i = 0 To mrsStore.RecordCount - 1
        '        result(i) = mrsStore.Fields("Store_No").Value
        '        mrsStore.MoveNext()
        '    Next

        '    GetStoreListAll = VB6.CopyArray(result)

        '    logger.Debug("GetStoreListAll Exit")
        'End Function
    End Class

End Namespace