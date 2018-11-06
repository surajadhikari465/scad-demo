Imports WholeFoods.ServiceLibrary.DataAccess
Imports log4net
Imports log4net.Config
Imports System.Reflection

<Assembly: log4net.Config.XmlConfigurator(Watch:=True)> 

Namespace IRMA
    Public Class Common
        Private Shared GS As GatewayService = New GatewayService()
        Public Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
        Public Shared ADOAppender As log4net.Appender.AdoNetAppender = SetADOAppender(GS.ReturnServiceConnectionString())
        Public Shared resourceManager As New Resources.ResourceManager("WholeFoods.ServiceLibrary.Resource", Assembly.GetExecutingAssembly())

        Public Enum ReceivingErrorCodes
            OrderClosed = 1
        End Enum

        Public Structure tItemUnit
            Dim Unit_ID As Short
            Dim Weight_Unit As Boolean
            Dim IsPackageUnit As Boolean
            Dim SystemCode As String
        End Structure

        Private Shared aWeight_Unit() As tItemUnit
        Public Shared giUnit As Short = 0
        Public Shared giShipper, giCase, giBox, giPound, giBag As Short

        ''' <summary>
        ''' Set up connection for lgging into database
        ''' </summary>
        ''' <param name="connectionString">Connection string</param>
        ''' <remarks></remarks>
        Public Shared Function SetADOAppender(ByVal connectionString As String) As log4net.Appender.AdoNetAppender
            Dim log4netHierarchy As log4net.Repository.Hierarchy.Hierarchy = CType(log4net.LogManager.GetRepository(), Repository.Hierarchy.Hierarchy)
            Dim adoAppender As log4net.Appender.AdoNetAppender

            adoAppender = CType(log4netHierarchy.Root.GetAppender("ADONetAppender"), Appender.AdoNetAppender)

            If Not adoAppender Is Nothing Then
                adoAppender.ConnectionString = connectionString
                adoAppender.ActivateOptions()
            End If

            Return adoAppender
        End Function

        Public Shared Function NotNull(Of T)(ByVal Value As T, ByVal DefaultValue As T) As T
            If Value Is Nothing OrElse IsDBNull(Value) Then
                Return DefaultValue
            Else
                Return Value
            End If
        End Function

        Public Shared Sub connectionCleanup(ByRef factory As DataFactory)
            If Not factory.Connection Is Nothing Then
                If factory.Connection.State <> ConnectionState.Closed Then
                    factory.Connection.Close()
                End If
                factory.Connection.Dispose()
            End If
        End Sub

        Public Shared Function GetSystemDate() As DateTime
            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim outParamList As New ArrayList
            Dim currentTime As DateTime = Date.Now

            Try
                currentTime = factory.ExecuteScalar("GetSystemDate")
                Return currentTime
            Catch ex As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try
        End Function

        Public Shared Function CostConversion(ByVal Amount As Decimal, ByVal FromUnit As Integer, ByVal ToUnit As Integer, ByVal PD1 As Decimal, ByVal PD2 As Decimal, ByVal PDU As Integer, ByRef Total_Weight As Decimal, ByRef Received As Decimal) As Decimal
            logger.Debug("CostConversion Entry")

            Dim fu As tItemUnit
            Dim tu As tItemUnit
            Dim result As Decimal

            ' TODO Documentation needed: What does PD1, PD2. PDU mean? Product descriptions?
            ' PD1 = VendorCostHistory.Package_Desc1 = Case Size
            ' PD2 = Item.Package_Desc2 = Item Size (12 part of "12 OZ")
            ' PDU = Item.Package_Unit_Id = Item Size UOM ( "OZ" part of "12 OZ")

            fu = GetItemUnit(FromUnit)
            tu = GetItemUnit(ToUnit)

            '-- Adjust PD1 based on the weight received if dealing with packages
            If Total_Weight <> 0 And Received <> 0 And (fu.IsPackageUnit Or tu.IsPackageUnit) Then PD1 = Total_Weight / (Received * PD2)

            If FromUnit <> ToUnit Then
                If fu.Weight_Unit Then
                    If tu.IsPackageUnit Then
                        ' Convert A per weight cost, to a case cost (LB cost * Case Size * Item Size)
                        result = Amount * PD1 * PD2
                    Else
                        result = Amount
                    End If
                Else
                    If Not fu.IsPackageUnit Then
                        If tu.IsPackageUnit Then
                            ' Convert A unit cost, to a case cost (unit cost * Case Size)
                            result = Amount * PD1
                        Else
                            result = Amount
                        End If
                    Else 'FromUnit is a package
                        ' Added logic to check for ToUnit also being a package unit, for CASE to BOX conversions. RS 10/26 TFS: 11332
                        If tu.IsPackageUnit Then
                            result = Amount
                        Else
                            ' Convert A case cost, to a unit cost (case cost / (Item Size if it's a weight unit, or 1)
                            result = Amount / (PD1 * IIf(tu.Weight_Unit, PD2, 1))
                        End If
                    End If
                End If
            Else
                ' From and To units are the same, cost stays the same
                result = Amount
            End If

            CostConversion = result
        End Function

        Public Shared Function GetItemUnit(ByVal lUnit_ID As Integer) As tItemUnit
            Dim iLoop As Integer
            Dim result As tItemUnit = Nothing

            If IsNothing(aWeight_Unit) Then
                PopulateAWeight()
            End If

            For iLoop = 1 To UBound(aWeight_Unit)
                If aWeight_Unit(iLoop).Unit_ID = lUnit_ID Then
                    result = aWeight_Unit(iLoop)
                    Exit For
                End If
            Next iLoop

            GetItemUnit = result
        End Function

        Private Shared Sub PopulateAWeight()
            Dim index As Short = -1
            ReDim aWeight_Unit(index)

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)

            Try
                Dim dt As DataTable = factory.GetStoredProcedureDataTable("GetUnitAndID")

                For Each dr As DataRow In dt.Rows
                    index = aWeight_Unit.GetUpperBound(0) + 1

                    ReDim Preserve aWeight_Unit(index)

                    aWeight_Unit(index).Unit_ID = dr.Item("Unit_ID")
                    aWeight_Unit(index).Weight_Unit = dr.Item("Weight_Unit")
                    aWeight_Unit(index).SystemCode = dr.Item("UnitSysCode")
                    aWeight_Unit(index).IsPackageUnit = dr.Item("IsPackageUnit")
                Next

            Catch ex As Exception
                Common.logger.Info("PopulateAWeight failed to Execute stored procedure 'GetUnitAndID'! ", ex)

            Finally
                connectionCleanup(factory)
            End Try
        End Sub

        Public Shared Sub LoadItemUnits()
            logger.Debug("LoadItemUnits entry")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
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
            Catch ex As Exception
                Common.logger.Info("LoadItemUnits failed to Execute stored procedure 'GetItemUnits'! ", ex)

            Finally
                connectionCleanup(factory)

            End Try
        End Sub

    End Class
   
End Namespace