Imports LoggerInspector
Imports WholeFoods.ServiceLibrary.DataAccess
Imports WholeFoods.ServiceLibrary.IRMA.Common

Namespace IRMA

    <ServiceBehavior(InstanceContextMode:=InstanceContextMode.PerCall)>
    <InspectorServiceBehavior>
    Public Class GatewayService
        Implements IGateway

#Region " Read Service Members"

#Region " Security"

        Public Function GetServerName() As String Implements IGateway.GetServerName
            logger.Info("GetServerName() - Enter")

            Try
                Dim sname As String = String.Empty
                sname = System.Net.Dns.GetHostName()
                Return sname

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetUserRoles(ByVal UserName As String) As List(Of Security.UserRole) Implements IGateway.GetUserRole
            logger.Info(String.Format("GetUserRoles() - Enter: UserName: {0}", UserName))

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim UserPermissions As New List(Of Security.UserRole)
            Dim dt As DataTable
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' ******* Parameters ************
            currentParam = New DBParam
            currentParam.Name = "UserName"
            currentParam.Value = UserName
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            Try
                dt = factory.GetStoredProcedureDataTable("GetUserPermissions", paramList)

                For Each dr As DataRow In dt.Rows
                    Dim UserRoles As New Security.UserRole
                    UserRoles.IsAccountant = dr.Item("Accountant")
                    UserRoles.IsAccountEnabled = dr.Item("AccountEnabled")
                    UserRoles.IsApplicationConfigAdmin = dr.Item("ApplicationConfigAdmin")
                    UserRoles.IsBuyer = dr.Item("Buyer")
                    UserRoles.IsCoordinator = dr.Item("Coordinator")
                    UserRoles.IsCostAdmin = dr.Item("CostAdmin")
                    UserRoles.IsDataAdministrator = dr.Item("DataAdministrator")
                    If dr.Item("DCAdmin") IsNot DBNull.Value Then
                        UserRoles.IsDCAdmin = dr.Item("DCAdmin")
                    Else
                        UserRoles.IsDCAdmin = False
                    End If
                    UserRoles.IsDistributor = dr.Item("Distributor")
                    UserRoles.IsEInvoicingAdministator = dr.Item("EInvoicing_Administrator")
                    UserRoles.IsFacilityCreditProcessor = dr.Item("FacilityCreditProcessor")
                    UserRoles.IsInventoryAdministrator = dr.Item("Inventory_Administrator")
                    UserRoles.IsItemAdministrator = dr.Item("Item_Administrator")
                    UserRoles.IsJobAdministrator = dr.Item("JobAdministrator")
                    UserRoles.IsLockAdministrator = dr.Item("Lock_Administrator")
                    UserRoles.IsPOAccountant = dr.Item("PO_Accountant")
                    UserRoles.IsPOApprovalAdmin = dr.Item("POApprovalAdmin")
                    UserRoles.IsPOSInterfaceAdministrator = dr.Item("POSInterfaceAdministrator")
                    UserRoles.IsPricebatchProcessor = dr.Item("PriceBatchProcessor")
                    UserRoles.IsSecurityAdministrator = dr.Item("SecurityAdministrator")
                    UserRoles.IsShrink = dr.Item("Shrink")
                    UserRoles.IsShrinkAdmin = dr.Item("ShrinkAdmin")
                    UserRoles.IsStoreAdministrator = dr.Item("StoreAdministrator")
                    UserRoles.IsSuperUser = dr.Item("SuperUser")
                    UserRoles.IsSystemConfigurationAdministrator = dr.Item("SystemConfigurationAdministrator")
                    UserRoles.IsUserMaintenance = dr.Item("UserMaintenance")
                    UserRoles.IsVendorAdministrator = dr.Item("Vendor_Administrator")
                    UserRoles.IsVendorCostDiscrepancyAdmin = dr.Item("VendorCostDiscrepancyAdmin")
                    UserRoles.IsWarehouse = dr.Item("Warehouse")

                    If dr.Item("Telxon_Store_Limit") IsNot DBNull.Value Then
                        UserRoles.TelxonStoreLimit = dr.Item("Telxon_Store_Limit")
                    Else
                        UserRoles.TelxonStoreLimit = -1
                    End If

                    UserRoles.UserID = dr.Item("User_ID")
                    UserPermissions.Add(UserRoles)

                Next
            Catch e As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try

            Return UserPermissions
        End Function

#End Region

#Region " Lookup Lists"
        'This stored proc grabs the available waste types and checks the SplitWasteCategory InstanceDataFlag
        Public Function GetInventoryAdjustmentReasons() As List(Of Lists.ShrinkAdjustmentReason) Implements IGateway.GetShrinkAdjustmentReasons
            logger.Info("GetInventoryAdjustmentReasons() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim InvAdjReasonslist As New List(Of Lists.ShrinkAdjustmentReason)
            Dim dt As DataTable

            Try
                dt = factory.GetStoredProcedureDataTable("GetAvailableWasteTypes")

                For Each dr As DataRow In dt.Rows
                    Dim Reasons As New Lists.ShrinkAdjustmentReason
                    Reasons.Abbreviation = dr.Item("Abbreviation")
                    Reasons.AdjustmentDescription = dr.Item("AdjustmentDescription")
                    Reasons.AdjustmentID = dr.Item("Adjustment_Id")
                    Reasons.InventoryAdjustmentCodeID = dr.Item("InventoryAdjustmentCode_ID")
                    InvAdjReasonslist.Add(Reasons)
                Next

            Catch e As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try

            Return InvAdjReasonslist
        End Function

        Public Function GetShrinkSubTypes() As List(Of ShrinkSubType) Implements IGateway.GetShrinkSubTypes
            logger.Info("ShrinkSubType - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim ShrinkSubTypesList As New List(Of ShrinkSubType)
            Dim dt As DataTable

            Try
                dt = factory.GetStoredProcedureDataTable("GetShrinkSubTypes")

                For Each dr As DataRow In dt.Rows
                    Dim ShrinkSubType As New ShrinkSubType
                    ShrinkSubType.ShrinkSubTypeID = dr.Item("ShrinkSubTypeID")
                    ShrinkSubType.ShrinkType = dr.Item("ShrinkType")
                    ShrinkSubType.ShrinkSubType = dr.Item("ShrinkSubType")
                    ShrinkSubType.ReasonCode = dr.Item("ReasonCode")
                    ShrinkSubType.LastUpdateUserId = dr.Item("LastUpdateUserId")
                    ShrinkSubType.LastUpdateDateTime = dr.Item("LastUpdateDateTime")
                    ShrinkSubType.Abbreviation = dr.Item("Abbreviation")
                    ShrinkSubType.InventoryAdjustmentCodeID = dr.Item("InventoryAdjustmentCodeID")
                    ShrinkSubTypesList.Add(ShrinkSubType)
                Next

            Catch e As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try

            Return ShrinkSubTypesList
        End Function

        Public Function GetCostAdjustmentsReasonCodes() As List(Of Lists.ReasonCode) Implements IGateway.GetDiscountReasonCodes
            logger.Info("GetCostAdjustmentsReasonCodes() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim reasoncodelist As New List(Of Lists.ReasonCode)
            Dim dt As DataTable
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' ******* Parameters ************
            currentParam = New DBParam
            currentParam.Name = "ReasonCodeTypeAbbr"
            currentParam.Value = "CA"
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            Try
                dt = factory.GetStoredProcedureDataTable("ReasonCodes_GetDetailsForType", paramList)

                Dim dv As DataView = New DataView(dt)

                For Each drv As DataRowView In dv
                    Dim reasoncode As New Lists.ReasonCode
                    reasoncode.ReasonCodeID = drv.Item("ReasonCodeDetailID")
                    reasoncode.ReasonCode = NotNull(drv.Item("ReasonCode"), "")
                    reasoncode.Description = drv.Item("ReasonCodeDesc")
                    reasoncodelist.Add(reasoncode)
                Next
            Catch e As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try

            Return reasoncodelist
        End Function

        Public Function GetRefusedItemsReasonCodes() As List(Of Lists.ReasonCode) Implements IGateway.GetRefusedItemsReasonCodes
            logger.Info("GetRefusedItemsReasonCodes() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim reasoncodelist As New List(Of Lists.ReasonCode)
            Dim dt As DataTable
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' ******* Parameters ************
            currentParam = New DBParam
            currentParam.Name = "ReasonCodeTypeAbbr"
            currentParam.Value = "RI"
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            Try
                dt = factory.GetStoredProcedureDataTable("ReasonCodes_GetDetailsForType", paramList)

                Dim dv As DataView = New DataView(dt)

                For Each drv As DataRowView In dv
                    Dim reasoncode As New Lists.ReasonCode
                    reasoncode.ReasonCodeID = drv.Item("ReasonCodeDetailID")
                    reasoncode.ReasonCode = NotNull(drv.Item("ReasonCode"), "")
                    reasoncode.Description = drv.Item("ReasonCodeDesc")
                    reasoncodelist.Add(reasoncode)
                Next
            Catch e As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try

            Return reasoncodelist
        End Function

        Public Function GetReasonCodesByType(ByVal reasonCodeType As String) As List(Of ReasonCode) Implements IGateway.GetReasonCodesByType
            logger.Info("GetReceivingDiscrepancyCodes() - Enter")

            Try
                Return ReasonCode.GetReasonCodesByType(reasonCodeType)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetSubteams() As List(Of Lists.Subteam) Implements IGateway.GetSubteams
            logger.Info("GetSubteams() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim subteamlist As New List(Of Lists.Subteam)
            Dim dt As DataTable

            Try
                dt = factory.GetStoredProcedureDataTable("GetSubteams")

                Dim dv As DataView = New DataView(dt)
                dv.Sort = " Subteam_Name"

                For Each drv As DataRowView In dv
                    Dim subteam As New Lists.Subteam
                    subteam.SubteamName = NotNull(drv.Item("Subteam_Name"), "")
                    subteam.SubteamNo = drv.Item("Subteam_No")
                    subteam.SubteamType = NotNull(drv.Item("SubteamType_ID"), 0)
                    subteam.SubteamIsUnrestricted = drv.Item("SubTeam_Unrestricted")
                    subteam.SubteamIsFixedSpoilage = drv.Item("FixedSpoilage")
                    subteamlist.Add(subteam)
                Next
            Catch e As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try

            Return subteamlist
        End Function

        Public Function GetSubteamByProductType(ByVal ProductType_ID As Integer) As List(Of Lists.Subteam) Implements IGateway.GetSubteamByProductType
            logger.Info("GetSubteamByProductType() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim subteamlist As New List(Of Lists.Subteam)
            Dim dt As DataTable
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' ******* Parameters ************
            currentParam = New DBParam
            currentParam.Name = "ProductType_ID"
            If ProductType_ID <> Nothing Then
                currentParam.Value = ProductType_ID
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            Try
                dt = factory.GetStoredProcedureDataTable("GetSubteamByProductType", paramList)

                Dim dv As DataView = New DataView(dt)
                dv.Sort = " Subteam_Name"

                For Each drv As DataRowView In dv
                    Dim subteam As New Lists.Subteam
                    subteam.SubteamName = NotNull(drv.Item("Subteam_Name"), "")
                    subteam.SubteamNo = drv.Item("Subteam_No")
                    subteam.SubteamIsUnrestricted = drv.Item("SubTeam_Unrestricted")
                    subteamlist.Add(subteam)
                Next
            Catch e As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try

            Return subteamlist
        End Function

        Public Function GetStores(Optional ByVal isVendorIDUsed As Boolean = False) As List(Of Lists.Store) Implements IGateway.GetStores
            logger.Info("GetStores() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim storelist As New List(Of Lists.Store)
            Dim dt As DataTable

            Try
                dt = factory.GetStoredProcedureDataTable("GetStorelist")

                Dim dv As DataView = New DataView(dt)
                dv.Sort = " Store_Name"

                For Each drv As DataRowView In dv
                    If Not IsDBNull(drv.Item("Vendor_ID")) Then
                        Dim store As New Lists.Store
                        store.StoreName = drv.Item("Store_Name")

                        If Not isVendorIDUsed Then
                            store.StoreNo = drv.Item("Store_No")
                        Else
                            store.StoreNo = drv.Item("Vendor_ID")
                        End If
                        storelist.Add(store)
                    End If
                Next

            Catch ex As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try

            Return storelist
        End Function

        Public Function GetItem(ByVal Item_Key As Integer, ByVal Identifier As String) As List(Of Lists.GetItem) Implements IGateway.GetItem
            logger.Info(String.Format("GetItem() - Enter: ItemKey: {0}, Identifier: {1}", Item_Key, Identifier))

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim ItemList As New List(Of Lists.GetItem)
            Dim dt As DataTable
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' ******* Parameters ************
            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            If Item_Key <> Nothing Then
                currentParam.Value = Item_Key
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Identifier"
            currentParam.Value = Identifier
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            Try
                dt = factory.GetStoredProcedureDataTable("GetItem", paramList)

                For Each dr As DataRow In dt.Rows
                    Dim Items As New Lists.GetItem
                    Items.ItemDescription = dr.Item("Item_Description")
                    Items.ItemKey = dr.Item("Item_Key")
                    Items.ItemSubteamNo = dr.Item("SubTeam_No")
                    Items.ItemSubteamName = dr.Item("SubTeam_Name")
                    Items.PackageDesc1 = dr.Item("Package_Desc1")
                    Items.PackageDesc2 = dr.Item("Package_Desc2")
                    Items.PackageUnitAbbr = dr.Item("Package_Unit_Abbr")
                    Items.SoldByWeight = dr.Item("Sold_By_Weight")
                    Items.CostedByWeight = dr.Item("CostedByWeight")
                    Items.RetailUnit = dr.Item("Retail_Unit_Name")
                    ItemList.Add(Items)
                Next

            Catch e As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try

            Return ItemList
        End Function

        Public Function GetInstanceDataFlag(ByVal FlagKey As String, ByVal Store_No As Integer) As Boolean Implements IGateway.GetInstanceDataFlag
            logger.Info("GetInstanceDataFlag() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim InstanceDataflag As New Boolean
            Dim dt As DataTable
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' ******* Parameters ************
            currentParam = New DBParam
            currentParam.Name = "Store_No"
            If Store_No <> Nothing Then
                currentParam.Value = Store_No
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "FlagKey"
            currentParam.Value = FlagKey
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            Try
                dt = factory.GetStoredProcedureDataTable("GetInstanceDataFlagValue", paramList)

                For Each dr As DataRow In dt.Rows
                    InstanceDataflag = dr.Item("FlagValue")
                Next
            Catch e As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try

            Return InstanceDataflag
        End Function

        Public Function GetItemUnits() As List(Of Lists.ItemUnit) Implements IGateway.GetItemUnits
            logger.Info("GetItemUnits() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim dt As DataTable
            Dim iuResults As New List(Of Lists.ItemUnit)

            Try
                dt = factory.GetStoredProcedureDataTable("GetItemUnits")
                For Each dr As DataRow In dt.Rows
                    Dim iu As New Lists.ItemUnit
                    iu.Unit_Id = dr.Item("Unit_ID")
                    iu.Unit_Name = String.Concat(dr.Item("Unit_Name"), "")
                    iu.Weight_Unit = dr.Item("Weight_Unit")
                    iu.User_Id = 0
                    iu.Unit_Abbreviation = dr.Item("Unit_Abbreviation")
                    iu.UnitSysCode = dr.Item("UnitSysCode")
                    iu.IsPackageUnit = dr.Item("IsPackageUnit")
                    iu.PlumUnitAbbr = String.Concat(dr.Item("PlumUnitAbbr"), "")
                    iu.EDISysCode = String.Concat(dr.Item("EDISysCode"), "")
                    iuResults.Add(iu)
                Next
                Return iuResults
            Catch ex As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try
        End Function

        Public Function GetInventoryLocations(ByVal lStoreNo As Long, ByVal lSubTeamNo As Long) As List(Of InventoryLocation) Implements IGateway.GetInventorylocations
            logger.Info("GetInventorylocations() - Enter")

            Try
                Dim cc As New CycleCount
                Return cc.GetInventoryLocations(lStoreNo, lSubTeamNo)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetDSDVendors(ByVal iStoreNo As Integer) As List(Of DSDVendor) Implements IGateway.GetDSDVendors
            logger.Info(String.Format("GetDSDVendors() - Enter: StoreNumber: {0}", iStoreNo))

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim dt As DataTable
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim DSDVendors As New List(Of DSDVendor)

            ' ******* Parameters ************
            currentParam = New DBParam
            currentParam.Name = "iStoreNo"
            If iStoreNo <> Nothing Then
                currentParam.Value = iStoreNo
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            Try
                dt = factory.GetStoredProcedureDataTable("GetDSDVendors", paramList)

                For Each dr As DataRow In dt.Rows
                    Dim DSDVendor As New DSDVendor(dr)
                    DSDVendors.Add(DSDVendor)
                Next
            Catch e As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try

            Return DSDVendors
        End Function

        Public Function GetOrderInvoiceCharges(ByVal orderHeaderID As Integer) As List(Of InvoiceCharge) Implements IGateway.GetOrderInvoiceCharges
            logger.Info(String.Format("GetOrderInvoiceCharges() - Enter: PO#: {0}", orderHeaderID))

            Try
                Return InvoiceCharge.GetOrderInvoiceCharges(orderHeaderID)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetGLAccountSubteams(ByVal orderHeaderID As Integer) As List(Of Lists.Subteam) Implements IGateway.GetGLAccountSubteams
            logger.Info(String.Format("GetGLAccountSubteams() - Enter: PO#: {0}", orderHeaderID))

            Try
                Return InvoiceCharge.GetGLAccountSubteams(orderHeaderID)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetAllocatedCharges() As List(Of InvoiceCharge) Implements IGateway.GetAllocatedCharges
            logger.Info("GetAllocatedCharges() - Enter")

            Try
                Return InvoiceCharge.GetAllocatedCharges()
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function ReparseEinvoice(ByVal eInvoiceID As Integer) As Result Implements IGateway.ReparseEinvoice
            logger.Info(String.Format("ReparseEinvoice() - Enter: eInvoiceID: {0}", eInvoiceID))

            Try
                Return Einvoice.ReparseEinvoice(eInvoiceID)

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetCurrencies() As List(Of Currency) Implements IGateway.GetCurrencies
            logger.Info("GetCurrencies() - Enter")

            Try
                Return Currency.GetCurrencies()
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function CountSustainabilityRankingRequiredItems(ByVal orderHeaderID As Integer) As Integer Implements IGateway.CountSustainabilityRankingRequiredItems
            logger.Info(String.Format("CountSustainabilityRankingRequiredItems() - Enter: PO#: {0}", orderHeaderID))

            Try
                Return Order.CountSustainabilityRankingRequiredItems(orderHeaderID)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetSystemDate() As DateTime Implements IGateway.GetSystemDate
            logger.Info("GetSystemDate() - Enter")

            Try
                Return Common.GetSystemDate()
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetOrderCurrency(ByVal orderHeaderId As Integer, ByVal regionCode As String) As String Implements IGateway.GetOrderCurrency
            logger.Info(String.Format("GetOrderCurrency() - Enter: PO#: {0}, Region: {1}", orderHeaderId, regionCode))

            Try
                Return Order.GetOrderCurrency(orderHeaderId, regionCode)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function IsDuplicateReceivingDocumentInvoiceNumber(ByVal InvoiceNumber As String, ByVal VendorId As Integer) As Boolean Implements IGateway.IsDuplicateReceivingDocumentInvoiceNumber
            logger.Info(String.Format("IsDuplicateReceivingDocumentInvoiceNumber() - Enter: InvoiceNumber: {0}, VendorId: {1}", InvoiceNumber, VendorId))

            Dim parameters As New ArrayList
            Dim parameter As DBParam

            parameter = New DBParam
            parameter.Name = "InvoiceNumber"
            parameter.Value = InvoiceNumber
            parameter.Type = DBParamType.String
            parameters.Add(parameter)

            parameter = New DBParam
            parameter.Name = "VendorId"
            parameter.Value = VendorId
            parameter.Type = DBParamType.Int
            parameters.Add(parameter)

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim dt As DataTable

            Try
                dt = factory.GetStoredProcedureDataTable("CheckForDuplicateReceivingDocumentInvoiceNumber", parameters)
            Catch e As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try

            Return dt.Rows.Count > 0

        End Function

#End Region

#End Region

#Region " Write Service Members"
        Public Function GetApplicationConfig(ByVal EnvironmentId As String, ByVal ApplicationId As String) As String Implements IGateway.GetApplicationConfig
            logger.Info("GetApplicationConfig() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim al As ArrayList = Nothing
            Dim results As String = String.Empty
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim dt As DataTable


            ' ******* Parameters ************
            currentParam = New DBParam
            currentParam.Name = "ApplicationID"
            currentParam.Value = ApplicationId
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "EnvironmentID"
            currentParam.Value = EnvironmentId
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            Try
                dt = factory.GetStoredProcedureDataTable("AppConfig_GetConfigDoc", paramList)
                results = dt.Rows(0)("Configuration").ToString()
            Catch ex As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try

            logger.Info(results)

            Return results
        End Function

        Public Function GetStoreFtpConfigDataForWriterType(ByVal FileWriterType As String) As List(Of Lists.StoreFTPConfig) Implements IGateway.GetStoreFtpConfigDataForWriterType
            logger.Info("GetStoreFtpConfigDataForWriterType() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim results As SqlClient.SqlDataReader = Nothing
            Dim currentParam As DBParam
            Dim paramList As ArrayList
            Dim ftpConfigInfo As Lists.StoreFTPConfig = Nothing
            Dim ConfigList As List(Of Lists.StoreFTPConfig) = New List(Of Lists.StoreFTPConfig)

            paramList = New ArrayList

            currentParam = New DBParam
            currentParam.Name = "FileWriterType"
            currentParam.Value = FileWriterType
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            Try
                results = factory.GetStoredProcedureDataReader("Replenishment_POSPush_GetFTPConfigForWriterType", paramList)

                While results.Read
                    ftpConfigInfo = New Lists.StoreFTPConfig(results)
                    ConfigList.Add(ftpConfigInfo)
                End While
            Catch ex As Exception
                Throw
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If

                connectionCleanup(factory)
            End Try

            Return ConfigList
        End Function

        Public Function GetStoreFtpConfigByStoreAndWriterType(ByVal store_no As Integer, ByVal Filewritertype As String) As List(Of Lists.StoreFTPConfig) Implements IGateway.GetStoreFtpConfigByStoreAndWriterType
            logger.Info("GetStoreFtpConfigByStoreAndWriterType() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim ConfigList As List(Of Lists.StoreFTPConfig) = New List(Of Lists.StoreFTPConfig)
            Dim results As SqlClient.SqlDataReader = Nothing
            Dim currentParam As DBParam
            Dim paramList As ArrayList
            Dim ftpConfigInfo As Lists.StoreFTPConfig

            paramList = New ArrayList

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = store_no
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "FileWriterType"
            currentParam.Value = Filewritertype
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            Try
                results = factory.GetStoredProcedureDataReader("Replenishment_POSPush_GetFTPConfigForStoreAndWriterType", paramList)

                While results.Read
                    ftpConfigInfo = New Lists.StoreFTPConfig(results)
                    ConfigList.Add(ftpConfigInfo)
                End While
            Catch ex As Exception
                Throw
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If

                connectionCleanup(factory)
            End Try

            Return ConfigList
        End Function

        Public Function UpdateOrderHeaderCosts(ByVal orderHeaderID As Integer) As Result Implements IGateway.UpdateOrderHeaderCosts
            logger.Info(String.Format("UpdateOrderHeaderCosts() - Enter: PO#: {0}", orderHeaderID))

            Try
                Return Order.UpdateOrderHeaderCosts(orderHeaderID)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

#Region " Inventory/Shrink"

        Public Function AddShrinkAdjustment(ByVal Adjustment As Inventory.Shrink) As Boolean Implements IGateway.AddShrinkAdjustment
            logger.Info("AddShrinkAdjustment() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim al As ArrayList = Nothing
            Dim SprocSuccess As Boolean
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' ******* Parameters ************
            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = Adjustment.StoreNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Item_Key"
            currentParam.Value = Adjustment.ItemKey
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Quantity"
            currentParam.Value = Adjustment.Quantity
            currentParam.Type = DBParamType.Decimal
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Weight"
            currentParam.Value = Adjustment.Weight
            currentParam.Type = DBParamType.Decimal
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Adjustment_ID"
            currentParam.Value = Adjustment.AdjustmentID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "AdjustmentReason"
            currentParam.Value = Adjustment.AdjustmentReason
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "CreatedBy"
            currentParam.Value = Adjustment.CreatedByUserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Subteam_No"
            currentParam.Value = Adjustment.SubteamNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InventoryAdjustmentCode"
            currentParam.Value = Adjustment.InventoryAdjustmentCodeAbbreviation
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UserName"
            currentParam.Value = Adjustment.UserName
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ShrinkSubTypeId"
            currentParam.Value = Adjustment.ShrinkSubTypeId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            Try
                al = factory.ExecuteStoredProcedure("InsertItemHistoryShrink", paramList)
                SprocSuccess = True
                Return SprocSuccess
            Catch ex As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try
        End Function
#End Region

#Region "Ordering"

        Public Function GetStoreItem(ByVal iStoreNo As Integer, _
                                     ByVal iTransferToSubteam_To As Integer, _
                                     ByVal iUser_ID As Integer, _
                                     ByVal iItem_Key As Integer, _
                                     ByVal sIdentifier As String) As StoreItem Implements IGateway.GetStoreItem

            logger.Info("GetStoreItem() - Enter")

            Dim retval As StoreItem = Nothing

            Try
                Dim si As New StoreItem
                retval = si.GetStoreItem(iStoreNo, iTransferToSubteam_To, iUser_ID, iItem_Key, sIdentifier)
            Catch ex As Exception
                Throw ex
            End Try

            Return retval
        End Function

        Public Function GetOrder(ByVal lOrderID As Long) As Order Implements IGateway.GetOrder
            logger.Info(String.Format("GetOrder() - Enter:  PO#: {0}", lOrderID))

            Try
                Dim o As New Order(lOrderID)
                Return o
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetVendorPackage(ByVal lStoreID As Integer, _
                                        ByVal vendorID As Integer, _
                                        ByVal sIdentifier As String) As Decimal Implements IGateway.GetVendorPackage

            logger.Info("GetVendorPackage() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            Try
                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = lStoreID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Vendor_ID"
                currentParam.Value = vendorID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Identifier"
                currentParam.Value = sIdentifier
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                Dim vp As Decimal
                vp = factory.GetStoredProcedureDataTable("WFMM_GetVendorPackage", paramList).Rows(0)("VendorPackage")
                Return vp

            Catch ex As Exception
                Throw ex
            Finally
                connectionCleanup(factory)
            End Try
        End Function

        Public Function GetItemMovement(ByVal lStoreID As Integer, _
                                        ByVal iTransferToSubteam_To As Integer, _
                                        ByVal sIdentifier As String, _
                                        ByVal adjustmentID As Integer) As List(Of Lists.ItemMovement) Implements IGateway.GetItemMovement

            logger.Info("GetItemMovement() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            Try
                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = lStoreID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Subteam_No"
                currentParam.Value = iTransferToSubteam_To
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Identifier"
                currentParam.Value = sIdentifier
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Adjustment_ID"
                currentParam.Value = adjustmentID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                Dim dtItemMovement As New DataTable
                Dim oiList As New List(Of Lists.ItemMovement)
                dtItemMovement = factory.GetStoredProcedureDataTable("WFMM_GetItemMovement", paramList)

                If dtItemMovement.Rows.Count > 0 Then
                    For Each dr As DataRow In dtItemMovement.Rows
                        Dim oi As New Lists.ItemMovement
                        oi.MovementDate = dr("MovementDate")
                        oi.MovementQty = dr("quantity")
                        oiList.Add(oi)
                    Next
                End If
                Return oiList
            Catch ex As Exception
                Throw ex
            Finally
                connectionCleanup(factory)
            End Try
        End Function

        Public Function GetItemBilledQuantity(ByVal lStoreID As Integer, _
                                        ByVal iTransferToSubteam_To As Integer, _
                                        ByVal sIdentifier As String) As List(Of Lists.ItemBilledQty) Implements IGateway.GetItemBilledQuantity

            logger.Info("GetItemBilledQuantity() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            Try
                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = lStoreID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Subteam_No"
                currentParam.Value = iTransferToSubteam_To
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Identifier"
                currentParam.Value = sIdentifier
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                Dim dt As New DataTable
                Dim iqList As New List(Of Lists.ItemBilledQty)

                dt = factory.GetStoredProcedureDataTable("WFMM_GetItemBilledQuantity", paramList)

                For Each dr As DataRow In dt.Rows
                    Dim iq As New Lists.ItemBilledQty
                    iq.BilledQty = dr("invoiceQty")
                    iq.OrderDate = dr("orderDate")
                    iq.OrderQty = dr("orderQty")
                    iq.InvNum = dr("invNumber")
                    iqList.Add(iq)
                Next

                Return iqList
            Catch ex As Exception
                Throw ex
            Finally
                connectionCleanup(factory)
            End Try
        End Function

        ''' <summary>
        '''  GetExternalOrders(ByVal lExternalSourceOrderID As Integer)  added 1/5/2012 rwa
        ''' </summary>
        ''' <param name="iExternalSourceOrder_ID"></param>
        ''' <param name="iStore_No"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetExternalOrders(ByVal iExternalSourceOrder_ID As Integer, ByVal iStore_No As Integer) As List(Of Lists.ExternalOrder) Implements IGateway.GetExternalOrders
            logger.Info(String.Format("GetExternalOrders() - Enter: ExternalSourceID: {0}, StoreNumber: {1}", iExternalSourceOrder_ID, iStore_No))

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim dt As DataTable
            Dim paramList As New ArrayList
            Dim currentParam As DBParam


            ' ******* Parameters ************
            currentParam = New DBParam
            currentParam.Name = "ExternalSourceOrder_ID"
            currentParam.Value = iExternalSourceOrder_ID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = iStore_No
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            Dim iuResults As New List(Of Lists.ExternalOrder)

            Try
                dt = factory.GetStoredProcedureDataTable("GetOrderID_for_ExternalSourceOrderID", paramList)
                For Each dr As DataRow In dt.Rows
                    Dim iu As New Lists.ExternalOrder
                    iu.OrderHeader_ID = dr.Item("OrderHeader_ID")
                    iu.Source = String.Concat(dr.Item("Source"), "")
                    iu.CompanyName = String.Concat(dr.Item("CompanyName"), "")
                    iuResults.Add(iu)
                Next
                Return iuResults
            Catch ex As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try

            Return Nothing
        End Function

        Public Function AddToOrderQueue(ByVal IsTransfer As Boolean, _
                                    ByVal IsCredit As Boolean, _
                                    ByVal Quantity As Decimal, _
                                    ByVal UnitID As Integer, _
                                    ByVal iUser_ID As Integer, _
                                    ByRef si As StoreItem) As Boolean Implements IGateway.AddToOrderQueue

            logger.Info("AddToOrderQueue() - Enter")

            Try
                si.AddToOrderQueue(IsTransfer, IsCredit, Quantity, UnitID, iUser_ID)
                Return True
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function AddToReprintSignQueue(ByVal lUser_ID As Long, _
                                 ByVal iSourceType As Integer, _
                                 ByVal sItemList As String,
                                 ByVal sItemListSeperator As String, _
                                 ByVal iStoreNo As Integer) As Boolean Implements IGateway.AddToReprintSignQueue

            logger.Info("AddToReprintSignQueue() - Enter")

            Try
                Dim si As New StoreItem
                Return si.AddToReprintSignQueue(lUser_ID, iSourceType, sItemList, sItemListSeperator, iStoreNo)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function UpdateRefusedQuantity(ByVal orderHeaderID As Integer, ByVal Identifier As String, ByVal Quantity As Decimal) As Result Implements IGateway.UpdateRefusedQuantity
            logger.Info("UpdateRefusedQuantity() - Enter")

            Try
                Dim oir As New OrderItemRefused
                Return oir.UpdateRefusedQuantity(orderHeaderID, Identifier, Quantity)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function InsertOrderItemRefused( _
                        ByVal iOrderHeader_ID As Integer, _
                        ByVal iOrderItem_ID As Integer, _
                        ByVal sIdentifier As String, _
                        ByVal sVIN As String, _
                        ByVal sDescription As String, _
                        ByVal sUnit As String, _
                        ByVal dInvoiceQuantity As Decimal, _
                        ByVal dInvoiceCost As Decimal, _
                        ByVal reasonCodeID As Integer) As Result Implements IGateway.InsertOrderItemRefused

            logger.Info(String.Format("InsertOrderItemRefused() - Enter: PO#: {0}, OrderItemID: {1}, Identifier: {2}", iOrderHeader_ID, iOrderItem_ID, sIdentifier))

            Try
                Dim oir As New OrderItemRefused
                oir.InsertOrderItemRefused(iOrderHeader_ID, iOrderItem_ID, sIdentifier, sVIN, sDescription, sUnit, dInvoiceQuantity, dInvoiceCost, reasonCodeID)
                Return oir.ResultObject
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function UpdateOrderItemRefused( _
                        ByVal iOrderItemRefused_ID As Integer, _
                        ByVal sIdentifier As String, _
                        ByVal sVIN As String, _
                        ByVal sDescription As String, _
                        ByVal sUnit As String, _
                        ByVal dInvoiceQuantity As Decimal, _
                        ByVal dInvoiceCost As Decimal, _
                        ByVal reasonCodeID As Integer, _
                        ByVal userAddedEntry As Integer) As Result Implements IGateway.UpdateOrderItemRefused

            logger.Info("UpdateOrderItemRefused() - Enter")

            Try
                Dim oir As New OrderItemRefused
                oir.UpdateOrderItemRefused(iOrderItemRefused_ID, sIdentifier, sVIN, sDescription, sUnit, dInvoiceQuantity, dInvoiceCost, reasonCodeID, userAddedEntry)
                Return oir.ResultObject
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function UpdateRefusedItemsList(ByVal ColumnValuesList As String, ByVal separator1 As String, ByVal separator2 As String) As Result Implements IGateway.UpdateRefusedItemsList
            logger.Info("UpdateRefusedItemsList() - Enter")

            Try
                Dim oir As New OrderItemRefused
                Return oir.UpdateRefusedItemsList(ColumnValuesList, separator1.Chars(0), separator2.Chars(0))
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function DeleteOrderItemRefused(ByVal iOrderItemRefused_ID As Integer) As Result Implements IGateway.DeleteOrderItemRefused
            logger.Info("DeleteOrderItemRefused() - Enter")

            Try
                Dim oir As New OrderItemRefused
                oir.DeleteOrderItemRefused(iOrderItemRefused_ID)
                Return oir.ResultObject
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetRefusedTotal(ByVal iOrderHeader_ID As Integer) As Decimal Implements IGateway.GetRefusedTotal
            logger.Info(String.Format("GetRefusedTotal() - Enter: PO#: {0}", iOrderHeader_ID))

            Try
                Dim oir As New OrderItemRefused
                Return oir.GetRefusedTotal(iOrderHeader_ID)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function ReceiveOrderItem(ByVal dQuantity As Decimal, _
                                    ByVal dWeight As Decimal, _
                                    ByVal dtDate As DateTime, _
                                    ByVal bCorrection As Boolean, _
                                    ByVal iOrderItem_ID As Integer, _
                                    ByVal reasonCodeID As Integer, _
                                    Optional ByVal dPackSize As Decimal = 0, _
                                    Optional ByVal UserID As Long = 0) As Result Implements IGateway.ReceiveOrderItem

            logger.Info("ReceiveOrderItem() - Enter")

            Try
                Dim oi As New OrderItem
                oi.ReceiveOrderItem(dQuantity, dWeight, dtDate, bCorrection, iOrderItem_ID, reasonCodeID, dPackSize, UserID)
                Return oi.ResultObject
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetStoreItemOrderInfo(ByVal iStoreNo As Integer, _
                                              ByVal iTransferToSubteamNo As Integer, _
                                              ByVal iItemKey As Integer) As StoreItemOrderInfo Implements IGateway.GetStoreItemOrderInfo

            logger.Info("GetStoreItemOrderInfo() - Enter")

            Try
                Dim sio As New StoreItemOrderInfo
                Return sio.GetStoreItemOrderInfo(iStoreNo, iTransferToSubteamNo, iItemKey)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetStoreItemCycleCountInfo(ByVal si As StoreItem, _
                                           Optional ByVal lInventoryLocationID As Long = 0) As CycleCountInfo Implements IGateway.GetStoreItemCycleCountInfo

            logger.Info("GetStoreItemCycleCountInfo() - Enter")

            Try
                Return si.GetStoreItemCycleCountInfo(lInventoryLocationID)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function RunPOSPush(ByVal sAppPath As String, ByVal sRegion As String, ByVal sConnectionString As String, ByVal sEmailAddress As String) As Boolean Implements IGateway.RunPOSPush
            logger.Info("RunPOSPush() - Enter")

            Try
                Dim push As New POSPush
                push.RunPOSPush(sAppPath, sRegion, sConnectionString, sEmailAddress)
                Return True
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetTransferItem(ByVal iItem_Key As Integer, _
                             ByVal sIdentifier As String, _
                             ByVal iProductType_ID As Integer, _
                             ByVal iVendorStore_No As Integer, _
                             ByVal iVendor_ID As Integer, _
                             ByVal iTransfer_SubTeam As Integer, _
                             ByVal iSupplySubTeam_No As Integer) As StoreItem Implements IGateway.GetTransferItem

            logger.Info("GetTransferItem() - Enter")

            Dim retval As StoreItem = Nothing

            Try
                Dim item As New StoreItem
                retval = item.GetTransferItem(iItem_Key, sIdentifier, iProductType_ID, iVendorStore_No, iVendor_ID, iTransfer_SubTeam, iSupplySubTeam_No)
            Catch ex As Exception
                Throw ex
            End Try

            Return retval
        End Function

        Public Function CreateDSDOrder(ByVal order As Order) As Result Implements IGateway.CreateDSDOrder
            logger.Info("CreateDSDOrder() - Enter")

            Try
                order.DSDOrder = True
                order.Electronic_Order = 1
                order.FromQueue = False
                order.Expected_Date = Now()
                order.Fax_Order = False

                ' Order Types:
                ' Purchase = 1
                ' Distribution = 2
                ' Transfer = 3
                ' Flowthru = 4

                ' Type 1 is the default for DSD orders.
                order.OrderType_Id = 1

                logger.Info("CreateOrder() - Enter")
                order.ResultObject = order.CreateOrder()

                If order.ResultObject.IRMA_PONumber = -1 Then
                    logger.Info("DeleteOrder() - Enter")
                    order.DeleteOrder()
                    Return order.ResultObject
                End If

                logger.Info("SendOrder() - Enter")
                order.ResultObject = order.SendOrder()
                If order.ResultObject.IRMA_PONumber = -1 Then
                    logger.Info("DeleteOrder() - Enter")
                    order.DeleteOrder()
                    Return order.ResultObject
                End If

                logger.Info("SendElectronicOrder() - Enter")
                order.ResultObject = order.SendElectronicOrder()

                If order.ResultObject.IRMA_PONumber = -1 Then
                    logger.Info("DeleteOrder() - Enter")
                    order.DeleteOrder()
                    Return order.ResultObject
                End If

                logger.Info("ReceiveOrder() - Enter")
                order.ResultObject = order.ReceiveOrder()
                If order.ResultObject.IRMA_PONumber = -1 Then
                    logger.Info("DeleteOrder() - Enter")
                    order.DeleteOrder()
                    Return order.ResultObject
                End If

                logger.Info("CloseOrder() - Enter")
                order.ResultObject = order.CloseOrder(order.OrderHeader_ID, order.CreatedBy)

                If order.ResultObject.IRMA_PONumber = -1 Then
                    logger.Info("DeleteOrder() - Enter")
                    order.DeleteOrder()
                    Return order.ResultObject
                End If

                Return order.ResultObject

            Catch ex As Exception
                logger.Info(ex)
                Throw ex
            End Try
        End Function

        Public Function CreateTransferOrder(ByVal order As Order) As Result Implements IGateway.CreateTransferOrder
            logger.Info("CreateTransferOrder() - Enter")

            Try
                ' Set Manual transmission for all transfer orders.
                order.Electronic_Order = 0
                order.FromQueue = False

                ' Order Types:
                ' Purchase = 1
                ' Distribution = 2
                ' Transfer = 3
                ' Flowthru = 4

                ' Type 3 is default for all transfer orders.
                order.OrderType_Id = 3

                order.Fax_Order = False

                logger.Info("CreateOrder() - Enter")
                order.ResultObject = order.CreateOrder()

                If order.ResultObject.IRMA_PONumber = -1 Then
                    logger.Info("DeleteOrder() - Enter")
                    order.DeleteOrder()
                    Return order.ResultObject
                End If

                If order.Vendor_ID = order.ReceiveLocation_ID Then

                    ' Intrastore Transfer (transfer within same store).
                    logger.Info("IntraStore_AutoSendReceiveCloseOrder() - Enter")
                    order.ResultObject = order.IntraStore_AutoSendReceiveCloseOrder()

                    If order.ResultObject.IRMA_PONumber = -1 Then
                        logger.Info("DeleteOrder() - Enter")
                        order.DeleteOrder()
                        Return order.ResultObject
                    End If

                Else

                    ' Interstore Transfer (transfer between different stores).
                    logger.Info("SendOrder() - Enter")
                    order.ResultObject = order.SendOrder()

                    If order.ResultObject.IRMA_PONumber = -1 Then
                        logger.Info("DeleteOrder() - Enter")
                        order.DeleteOrder()
                        Return order.ResultObject
                    End If
                End If

                Return order.ResultObject

            Catch ex As Exception
                logger.Info(ex)
                Throw ex

            End Try
        End Function

        Public Function CreateOrder(ByVal order As Order) As Result Implements IGateway.CreateOrder
            logger.Info("CreateOrder() - Enter")

            Try
                order.CreateOrder()
                Return order.ResultObject

            Catch ex As Exception
                logger.Info(ex)
                Throw ex

            End Try
        End Function

        Public Function SendOrder(ByVal order As Order) As Result Implements IGateway.SendOrder
            logger.Info(String.Format("SendOrder() - Enter: PO#: {0}", order.OrderHeader_ID))

            Try
                order.SendOrder()
                Return order.ResultObject

            Catch ex As Exception
                logger.Info(ex)
                Throw ex

            End Try
        End Function

        Public Function ReceiveOrder(ByVal order As Order) As Result Implements IGateway.ReceiveOrder
            logger.Info(String.Format("ReceiveOrder() - Enter: PO#: {0}", order.OrderHeader_ID))

            Try
                order.ReceiveOrder()
                Return order.ResultObject

            Catch ex As Exception
                logger.Info(ex)
                Throw ex

            End Try
        End Function

        Public Function CloseOrder(ByVal OrderHeader_ID As Integer, ByVal User_ID As Integer) As Result Implements IGateway.CloseOrder
            logger.Info(String.Format("CloseOrder() - Enter: PO#: {0}", OrderHeader_ID))

            Dim order As New Order

            Try
                order.CloseOrder(OrderHeader_ID, User_ID)
                Return order.ResultObject

            Catch ex As Exception
                logger.Info(ex)
                Throw ex

            End Try
        End Function

        Public Function DeleteOrder(ByVal order As Order) As Boolean Implements IGateway.DeleteOrder
            logger.Info(String.Format("DeleteOrder() - Enter: PO#: {0}", order.OrderHeader_ID))

            Try
                order.DeleteOrder()
                Return True

            Catch ex As Exception
                logger.Info(ex)
                Throw ex

            End Try
        End Function

        Public Function IsValidRefusedItemList(ByVal orderHeaderID As Integer) As Boolean Implements IGateway.IsValidRefusedItemList
            logger.Info(String.Format("IsRefusalAllowed() - Enter: PO#: {0}", orderHeaderID))

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim returnVal As Boolean

            Try
                returnVal = CType(factory.ExecuteScalar("SELECT dbo.fn_IsValidRefusedItemList(" & CStr(orderHeaderID) & ")"), Boolean)
                Return returnVal

            Catch ex As Exception
                Throw

            Finally
                connectionCleanup(factory)

            End Try
        End Function

        Public Function IsRefusalAllowed(ByVal orderHeaderID As Integer) As Boolean Implements IGateway.IsRefusalAllowed
            logger.Info(String.Format("IsRefusalAllowed() - Enter: PO#: {0}", orderHeaderID))

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim returnVal As Boolean

            Try
                returnVal = CType(factory.ExecuteScalar("SELECT dbo.fn_IsRefusalAllowed(" & CStr(orderHeaderID) & ")"), Boolean)
                Return returnVal

            Catch ex As Exception
                Throw

            Finally
                connectionCleanup(factory)

            End Try
        End Function

        Public Function GetRefusedQuantity(ByVal orderHeaderID As Integer, ByVal Identifier As String) As Decimal Implements IGateway.GetRefusedQuantity
            logger.Info(String.Format("GetRefusedQuantity() - Enter: PO#: {0}, Identifier: {1}", orderHeaderID, Identifier))

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim returnVal As Decimal

            Try
                returnVal = CType(factory.ExecuteScalar("SELECT dbo.fn_GetRefusedQuantityByIdentifier(" & CStr(orderHeaderID) & ", '" & Identifier & "')"), Decimal)
                Return returnVal

            Catch ex As Exception
                Throw

            Finally
                connectionCleanup(factory)

            End Try

        End Function

        Public Function StoreSubTeamRelationshipExists(ByVal sStore_No As String, ByVal sTransfer_To_SubTeam As String) As Boolean Implements IGateway.StoreSubTeamRelationshipExists
            logger.Info("StoreSubTeamRelationshipExists() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim returnVal As Boolean

            Try
                returnVal = CType(factory.ExecuteScalar("SELECT dbo.fn_StoreSubTeamExists(" & sStore_No & ", " & sTransfer_To_SubTeam & ")"), Boolean)
                Return returnVal
            Catch ex As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try
        End Function

        Public Function GetOrderHeaderByIdentifier(ByVal UPC As String, _
                                                   ByVal StoreNumber As Integer) As List(Of Order) Implements IGateway.GetOrderHeaderByIdentifier

            logger.Info(String.Format("GetOrderHeaderByIdentifier() - Enter: Identifier: {0}, StoreNumber: {1}", UPC, StoreNumber))

            Try
                Return Order.GetOrderHeaderByIdentifier(UPC, StoreNumber)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetReceivingListEinvoiceExceptions(ByVal OrderHeader_ID As Integer) As List(Of OrderItem) Implements IGateway.GetReceivingListEinvoiceExceptions
            logger.Info("GetReceivingListEinvoiceExceptions() - Enter")

            Try
                Dim oi As New OrderItem
                Return oi.GetReceivingListEinvoiceExceptions(OrderHeader_ID)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetOrderItemsRefused(ByVal OrderHeader_ID As Integer) As List(Of OrderItemRefused) Implements IGateway.GetOrderItemsRefused
            logger.Info(String.Format("GetOrderItemsRefused() - Enter: PO#: {0}", OrderHeader_ID))

            Try
                Dim oir As New OrderItemRefused
                Return oir.GetOrderItemsRefused(OrderHeader_ID)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function UpdateReceivingDiscrepancyCode(ByVal ReasonCodeList As String, ByVal separator1 As String, ByVal separator2 As String) As Result Implements IGateway.UpdateReceivingDiscrepancyCode
            logger.Info("UpdateReceivingDiscrepancyCode() - Enter")

            Try
                Dim order As New Order
                Return order.UpdateReceivingDiscrepancyCode(ReasonCodeList, separator1.Chars(0), separator2.Chars(0))
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function IsDSDStoreVendorByUPC(ByVal UPC As String, ByVal Store_No As Integer) As Boolean Implements IGateway.IsDSDStoreVendorByUPC
            logger.Info(String.Format("IsDSDStoreVendorByUPC() - Enter: Identifier: {0}, StoreNumber: {1}", UPC, Store_No))

            Try
                Dim dsdvendor As New DSDVendor
                Return dsdvendor.IsDSDStoreVendorByUPC(UPC, Store_No)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function UpdateOrderBeforeClose(ByVal OrderHeader_ID As Integer, _
                                            ByVal InvoiceNumber As String, _
                                            ByVal InvoiceDate As Date, _
                                            ByVal InvoiceCost As Decimal, _
                                            ByVal VendorDoc_ID As String, _
                                            ByVal VendorDocDate As Date, _
                                            ByVal SubTeam_No As Integer, _
                                            ByVal PartialShipment As Boolean) As Result Implements IGateway.UpdateOrderBeforeClose

            logger.Info(String.Format("UpdateOrderBeforeClose() - Enter: PO#: {0}", OrderHeader_ID))

            Try
                Dim order As New Order
                Return order.UpdateOrderBeforeClose(OrderHeader_ID, InvoiceNumber, InvoiceDate, InvoiceCost, VendorDoc_ID, _
                                                    VendorDocDate, SubTeam_No, PartialShipment)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function AddInvoiceCharge(ByVal orderHeaderID As Integer, ByVal SACTypeID As Integer, ByVal description As String, _
                                         ByVal subteamNumber As Integer, ByVal allowance As Boolean, ByVal amount As Decimal) As Result Implements IGateway.AddInvoiceCharge
            logger.Info(String.Format("AddInvoiceCharge() - Enter: PO#: {0}", orderHeaderID))

            Try
                Return InvoiceCharge.AddInvoiceCharge(orderHeaderID, SACTypeID, description, subteamNumber, allowance, amount)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function RemoveInvoiceCharge(ByVal chargeID As Integer) As Result Implements IGateway.RemoveInvoiceCharge
            logger.Info("RemoveInvoiceCharge() - Enter")

            Try
                Return InvoiceCharge.RemoveInvoiceCharge(chargeID)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function CheckInvoiceNumber(ByVal Vendor_ID As Integer, ByVal InvoiceNumber As String, ByVal OrderHeader_ID As Integer) As Result Implements IGateway.CheckInvoiceNumber

            logger.Info(String.Format("CheckInvoiceNumber() - Enter: VendorID: {0}, InvoiceNumber: {1}, PO#: {2}", Vendor_ID, InvoiceNumber, OrderHeader_ID))

            Try
                Dim order As New Order
                Return order.CheckInvoiceNumber(Vendor_ID, InvoiceNumber, OrderHeader_ID)
            Catch ex As Exception
                logger.Info(ex)
                Throw ex
            End Try
        End Function

        Public Function ReOpenOrder(ByVal OrderHeader_ID As Integer) As Result Implements IGateway.ReOpenOrder
            logger.Info(String.Format("ReOpenOrder() - Enter: PO#: {0}", OrderHeader_ID))

            Try
                Dim order As New Order
                Return order.ReOpenOrder(OrderHeader_ID)
            Catch ex As Exception
                logger.Info(ex)
                Throw ex
            End Try
        End Function

        Public Function RefuseReceiving(ByVal orderHeaderID As Integer, ByVal userID As Integer, ByVal refuseReceivingReasonCodeID As Integer) As Result Implements IGateway.RefuseReceiving
            logger.Info(String.Format("RefuseReceiving() - Enter: PO#: {0}", orderHeaderID))

            Try
                Dim order As New Order
                Return order.RefuseReceiving(orderHeaderID, userID, refuseReceivingReasonCodeID)
            Catch ex As Exception
                logger.Info(ex)
                Throw ex
            End Try
        End Function

        Public Function CalculateConversion(ByVal InUnit As String, ByVal OutUnit As String, ByVal Amount As Decimal) As Decimal Implements IGateway.CalculateConversion
            logger.Info("CalculateConversion() - Enter")

            Try
                Dim calc As New ConversionCalculator
                Return calc.CalculateConversion(InUnit, OutUnit, Amount)
            Catch ex As Exception
                logger.Info(ex)
                Throw ex
            End Try
        End Function

        Public Function GetInUnits() As String() Implements IGateway.GetInUnits
            logger.Info("GetInUnits() - Enter")

            Try
                Dim calc As New ConversionCalculator
                Return calc.GetInUnits()
            Catch ex As Exception
                logger.Info(ex)
                Throw ex
            End Try
        End Function

        Public Function GetOutUnits(ByVal InUnit As String) As String() Implements IGateway.GetOutUnits
            logger.Info("GetOutUnits() - Enter")

            Try
                Dim calc As New ConversionCalculator
                Return calc.GetOutUnits(InUnit)
            Catch ex As Exception
                logger.Info(ex)
                Throw ex
            End Try
        End Function
#End Region

#Region "CycleCount"
        Public Function GetCycleCount(ByVal lStoreNo As Long, ByVal lSubTeamNo As Long) As CycleCount Implements IGateway.GetCycleCount
            logger.Info("GetCycleCount() - Enter")

            Try
                Dim cc As New CycleCount
                Return cc.GetCycleCount(lStoreNo, lSubTeamNo)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function CreateCycleCountHeader(ByVal lMasterCountID As Long, _
                                       ByVal dStartScan As DateTime, _
                                       ByVal lInventoryLocationId As Long, _
                                       ByVal bExternal As Boolean) As Object Implements IGateway.CreateCycleCountHeader

            logger.Info("CreateCycleCountHeader() - Enter")

            Try
                Dim cc As New CycleCount
                Return cc.CreateCycleCountHeader(lMasterCountID, dStartScan, lInventoryLocationId, bExternal)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Sub AddCycleCountItem(ByVal lItemKey As Long, _
                             ByVal dQuantity As Decimal, _
                             ByVal dWeight As Decimal, _
                             ByVal dPackSize As Decimal, _
                             ByVal bIsCaseCnt As Boolean, _
                             ByVal lCycleCountID As Long, _
                             Optional ByVal lInvLocID As Long = Nothing) Implements IGateway.AddCycleCountItem

            logger.Info("AddCycleCountItem() - Enter")

            Try
                Dim cc As New CycleCount
                cc.AddCycleCountItem(lItemKey, dQuantity, dWeight, dPackSize, bIsCaseCnt, lCycleCountID, lInvLocID)
            Catch ex As Exception

            End Try
        End Sub

#End Region

#Region "Mars"

        Public Function SetStopSaleForItem(ByVal storeNumber As Integer, ByVal itemIdentifier As String, ByVal stopSale As Boolean) As Boolean Implements IGateway.SetStopSaleForItem
            logger.Info("SetStopSaleForItem() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)

            Dim paramsList = New ArrayList From
                {
                    New DBParam With {.Name = "StoreNumber", .Value = storeNumber, .Type = DBParamType.Int},
                    New DBParam With {.Name = "ItemIdentifier", .Value = itemIdentifier, .Type = DBParamType.String},
                    New DBParam With {.Name = "StopSale", .Value = stopSale, .Type = DBParamType.Bit}
                }

            Try
                factory.ExecuteStoredProcedure("Set_Stop_Sale_For_Item", paramsList)
                Return True
            Catch ex As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try
        End Function

#End Region

#End Region

#Region " Debug Service Members"
        Public Function ReturnServiceConnectionString() As String Implements IGateway.ReturnServiceConnectionString
            logger.Info("ReturnServiceConnectionString() - Enter")

            Dim retval As String = String.Empty
            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)

            Try
                retval = factory.ConnectString
            Catch ex As Exception
                Throw
            Finally
                connectionCleanup(factory)
            End Try

            Return retval
        End Function

        Public Function LoggerTest() As String Implements IGateway.LoggerTest
            logger.Info("LoggerTest() - Enter")

            Dim a As Integer = 1
            Dim b As Integer = 0
            Dim c As Integer = 0

            Try
                a = b \ c
            Catch ex As Exception
                logger.Error(ex.Message.ToString(), ex)
            Finally
                logger.Info("LoggerTest() - Exit")
            End Try

            Return "Great Success"
        End Function

#End Region

    End Class
End Namespace