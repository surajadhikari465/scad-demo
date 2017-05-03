Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Imports log4net

Namespace WholeFoods.IRMA.ItemHosting.DataAccess

    Public Class ItemUnitDAO

        ' TFS 13036, v4.0, 7/21/2010, Tom Lux: Added logger.  Moved LoadItemUnits() from Main.vb and made it shared because it makes sense, needs to be called more than once,
        ' and should be associated with an ItemUnit object.
        ' TFS 13058, v4.0, 8/3/2010, Tom Lux: Merged Administration\ConfigurationData\DataAccess\ItemUnitDAO.vb into this class (to remove dup class).

#Region "Public Constants"

        ''' <summary>
        ''' Unit-of-measure name (ItemUnit table) for "Unit".
        ''' </summary>
        ''' <remarks></remarks>
        Public Const UOM_NAME_UNIT = "Unit"

#End Region

#Region "Private Members"

        ''' <summary>
        ''' Log4Net logger for this class.
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Private factory As DataFactory

#End Region

#Region "Instance Methods"

        Public Sub New()
            CreateDatabaseConnection()
        End Sub

        Private Sub CreateDatabaseConnection()
            If factory Is Nothing Then
                factory = New DataFactory(DataFactory.ItemCatalog)
            End If
        End Sub
        Public Shared Function IsWeightUnit(ByVal UnitID As Integer) As Boolean

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As Boolean = False

            Try
                results = factory.ExecuteScalar("SELECT dbo.fn_IsWeightUnit(" & UnitID & ")")

            Catch ex As Exception
                Throw ex
            End Try

            Return results

        End Function

        Public Sub SaveItemUnit(ByVal itemUnit As ItemUnitBO)


            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "UnitName"
            currentParam.Value = itemUnit.UnitName
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "WeightUnit"
            currentParam.Value = IIf(itemUnit.WeightUnit = True, 1, 0)
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UserId"
            currentParam.Value = itemUnit.UserId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UnitAbbreviation"
            currentParam.Value = itemUnit.UnitAbbreviation
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UnitSysCode"
            currentParam.Value = itemUnit.UnitSysCode
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "IsPackageUnit"
            currentParam.Value = IIf(itemUnit.IsPackageUnit = True, 1, 0)
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PlumUnitAbbreviation"
            currentParam.Value = IIf(itemUnit.PlumUnitAbbr = String.Empty, System.DBNull.Value, itemUnit.PlumUnitAbbr)
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "EDISysCode"
            currentParam.Value = itemUnit.EDISysCode
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            'Add Unit_ID parameter to support UPDATE.
            currentParam = New DBParam
            currentParam.Name = "Unit_ID"
            currentParam.Value = IIf(itemUnit.UnitId > 0, itemUnit.UnitId, DBNull.Value)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("ItemUnitSave", paramList)

        End Sub

        Public Function GetItemUnits() As List(Of ItemUnitBO)
            Dim list As List(Of ItemUnitBO) = New List(Of ItemUnitBO)
            Dim ds As DataSet = factory.GetStoredProcedureDataSet("GetItemUnits")

            For Each dr As DataRow In ds.Tables(0).Rows
                With dr
                    list.Add(New ItemUnitBO( _
                                                .Item("Unit_Id"), _
                                                .Item("Unit_Name"), _
                                                CBool(.Item("Weight_Unit")), _
                                                .Item("Unit_Abbreviation"), _
                                                .Item("UnitSysCode"), _
                                                CBool(.Item("IsPackageUnit")), _
                                                IIf(.Item("User_Id") Is DBNull.Value, -1, .Item("User_Id")), _
                                                IIf(.Item("PlumUnitAbbr") Is DBNull.Value, String.Empty, .Item("PlumUnitAbbr")), _
                                                IIf(.Item("EDISysCode") Is DBNull.Value, String.Empty, .Item("EDISysCode")) _
                                            ) _
                            )
                End With
            Next

            ds.Dispose()

            Return list


        End Function

#End Region

#Region "Shared Methods"

        ''' <summary>
        ''' Retrieves select item UOM IDs and sets global variables.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub LoadItemUnits()
            logger.Debug("LoadItemUnits entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim unitSysCode As String
            Dim unitID As Integer

            Try
                With factory.GetStoredProcedureDataReader("dbo.GetItemUnits")
                    While .Read
                        unitSysCode = .GetString(.GetOrdinal("UnitSysCode"))
                        unitID = .GetInt32(.GetOrdinal("Unit_ID"))

                        Select Case unitSysCode
                            Case "unit" : giUnit = unitID
                            Case "case" : giCase = unitID
                            Case "box" : giBox = unitID
                            Case "lb" : giPound = unitID
                            Case "shp" : giShipper = unitID
                            Case "bag" : giBag = unitID
                        End Select
                    End While

                    .Close()
                End With

            Finally
                factory = Nothing

            End Try

            logger.DebugFormat("Item units loaded: unit={0}, case={1}, box={2}, lb={3}, shp={4}, bag={5}", giUnit, giCase, giBox, giPound, giShipper, giBag)
        End Sub

        ''' <summary>
        ''' gets list of ItemUnit data; filtered on weightUnit and allUnit boolean flags passed in
        ''' </summary>
        ''' <param name="weightUnit"></param>
        ''' <returns>ArrayList of ItemUnitBO objects</returns>
        ''' <remarks></remarks>
        Public Shared Function GetItemUnitListForPackageDesc(ByVal weightUnit As Boolean) As ArrayList

            logger.Debug("GetItemUnitListForPackageDesc Entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim itemUnit As ItemUnitBO
            Dim itemUnitList As New ArrayList
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "WeightsOnly"
                currentParam.Value = weightUnit
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                ' Tom Lux - Removed 2nd all-units arg because SP no longer includes it.

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetItemUnitsPDU", paramList)

                While results.Read
                    itemUnit = New ItemUnitBO()
                    itemUnit.UnitId = results.GetInt32(results.GetOrdinal("Unit_ID"))
                    itemUnit.WeightUnit = results.GetBoolean(results.GetOrdinal("Weight_Unit"))
                    itemUnit.UnitName = results.GetString(results.GetOrdinal("Unit_Name"))
                    itemUnit.UnitAbbreviation = results.GetString(results.GetOrdinal("Unit_Abbreviation"))

                    itemUnitList.Add(itemUnit)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetItemUnitListForPackageDesc Exit")

            Return itemUnitList
        End Function

        ''' <summary>
        ''' gets list of ItemUnit data; filtered on weightUnit and allUnit boolean flags passed in
        ''' </summary>
        ''' <param name="weightUnit"></param>
        ''' <returns>ArrayList of ItemUnitBO objects</returns>
        ''' <remarks></remarks>
        Public Shared Function GetItemUnitListForCost(ByVal weightUnit As Boolean, Optional ByVal allUnits As Boolean = False) As ArrayList

            logger.Debug("GetItemUnitListForCost Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim itemUnit As ItemUnitBO
            Dim itemUnitList As New ArrayList
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "WeightUnits"
                currentParam.Value = weightUnit
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "AllUnits"
                currentParam.Value = allUnits
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetItemUnitsCost", paramList)

                While results.Read
                    itemUnit = New ItemUnitBO()
                    itemUnit.UnitID = results.GetInt32(results.GetOrdinal("Unit_ID"))
                    itemUnit.WeightUnit = results.GetBoolean(results.GetOrdinal("Weight_Unit"))
                    itemUnit.UnitName = results.GetString(results.GetOrdinal("Unit_Name"))
                    itemUnit.UnitAbbreviation = results.GetString(results.GetOrdinal("Unit_Abbreviation"))

                    itemUnitList.Add(itemUnit)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetItemUnitListForCost Exit")

            Return itemUnitList
        End Function

        ''' <summary>
        ''' gets list of ItemUnit data; filtered on weightUnit boolean passed in
        ''' </summary>
        ''' <param name="weightUnit"></param>
        ''' <returns>ArrayList of ItemUnitBO objects</returns>
        ''' <remarks></remarks>
        Public Shared Function GetItemUnitList(ByVal weightUnit As Boolean, Optional ByVal allUnits As Boolean = False) As ArrayList

            logger.Debug("GetItemUnitList Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim itemUnit As ItemUnitBO
            Dim itemUnitList As New ArrayList
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "WeightUnits"
                currentParam.Value = weightUnit
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetItemUnitsVendor", paramList)

                While results.Read
                    itemUnit = New ItemUnitBO()
                    itemUnit.UnitID = results.GetInt32(results.GetOrdinal("Unit_ID"))
                    itemUnit.WeightUnit = results.GetBoolean(results.GetOrdinal("Weight_Unit"))
                    itemUnit.UnitName = results.GetString(results.GetOrdinal("Unit_Name"))
                    itemUnit.UnitAbbreviation = results.GetString(results.GetOrdinal("Unit_Abbreviation"))

                    itemUnitList.Add(itemUnit)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetItemUnitList Exit")


            Return itemUnitList
        End Function

        ''' <summary>
        ''' gets item package unit info and returns an ItemBO with those fields filled out
        ''' </summary>
        ''' <param name="itemInfo"></param>
        ''' <remarks></remarks>
        Public Shared Sub GetItemUnitInfo(ByRef itemInfo As ItemBO, Optional ByVal StoreNo As Integer = 0)

            logger.Debug("GetItemUnitInfo Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Value = itemInfo.Item_Key
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                If StoreNo = 0 Then
                    ' Execute Stored Procedure to Create/Maintain Price Batch Details for the offer
                    results = factory.GetStoredProcedureDataReader("GetItemUnitInfo", paramList)
                Else
                    ' setup the Store_No parameter
                    currentParam = New DBParam
                    currentParam.Name = "Store_No"
                    currentParam.Value = StoreNo
                    currentParam.Type = DBParamType.Int
                    paramList.Add(currentParam)

                    results = factory.GetStoredProcedureDataReader("GetItemUnitInfoStore", paramList)
                End If

                If results.Read Then
                    itemInfo.PackageDesc1 = results.GetDecimal(results.GetOrdinal("Package_Desc1"))
                    itemInfo.PackageDesc2 = results.GetDecimal(results.GetOrdinal("Package_Desc2"))
                    itemInfo.PackageUnitName = results.GetString(results.GetOrdinal("PU_Name"))

                    If StoreNo <> Nothing Then
                        itemInfo.ItemDescription = results.GetString(results.GetOrdinal("ItemDescription"))
                    End If
                End If
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "ItemDAO:GetItemUnitInfo")
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetItemUnitInfo Exit")
        End Sub


#End Region

    End Class

End Namespace