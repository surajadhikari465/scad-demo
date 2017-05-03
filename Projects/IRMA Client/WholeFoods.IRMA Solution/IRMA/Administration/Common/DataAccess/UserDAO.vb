Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Administration.Common.DataAccess
    Public Class UserDAO
#Region "Read Methods"
        ''' <summary>
        ''' Read the complete list of Users.
        ''' </summary>
        ''' <exception cref="DataFactoryException" />
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetUsers() As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            ' Execute the stored procedure

            ' returns too much data for this grid, no need to create objects for every user when the grid is loaded.
            'Return factory.GetStoredProcedureDataSet("Administration_UserAdmin_GetAllUsers")

            'instead, only get what is needed to display in the grid and only build the user object when a user is selected for editing.
            Return factory.GetStoredProcedureDataSet("Administration_UserAdmin_GetUserList")

        End Function

        ''' <summary>
        ''' Populates the UserBO with the results for the ItemCatalog.User table.
        ''' </summary>
        ''' <param name="results"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function PopulateUserFromValidateResults(ByRef results As SqlDataReader) As UserBO
            Dim returnUser As New UserBO()
            If (Not results.IsDBNull(results.GetOrdinal("User_ID"))) Then
                returnUser.UserId = results.GetInt32(results.GetOrdinal("User_ID"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Accountant"))) Then
                returnUser.Accountant = results.GetBoolean(results.GetOrdinal("Accountant"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("AccountEnabled"))) Then
                returnUser.AccountEnabled = results.GetBoolean(results.GetOrdinal("AccountEnabled"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Buyer"))) Then
                returnUser.Buyer = results.GetBoolean(results.GetOrdinal("Buyer"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Coordinator"))) Then
                returnUser.Coordinator = results.GetBoolean(results.GetOrdinal("Coordinator"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("FacilityCreditProcessor"))) Then
                returnUser.FacilityCreditProcessor = results.GetBoolean(results.GetOrdinal("FacilityCreditProcessor"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Delete_Access"))) Then
                returnUser.DeleteAccess = results.GetBoolean(results.GetOrdinal("Delete_Access"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Distributor"))) Then
                returnUser.Distributor = results.GetBoolean(results.GetOrdinal("Distributor"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("FullName"))) Then
                returnUser.FullName = results.GetString(results.GetOrdinal("FullName"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Inventory_Administrator"))) Then
                returnUser.InventoryAdministrator = results.GetBoolean(results.GetOrdinal("Inventory_Administrator"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Item_Administrator"))) Then
                returnUser.ItemAdministrator = results.GetBoolean(results.GetOrdinal("Item_Administrator"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Lock_Administrator"))) Then
                returnUser.LockAdministrator = results.GetBoolean(results.GetOrdinal("Lock_Administrator"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("PO_Accountant"))) Then
                returnUser.POAccountant = results.GetBoolean(results.GetOrdinal("PO_Accountant"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("PriceBatchProcessor"))) Then
                returnUser.PriceBatchProcessor = results.GetBoolean(results.GetOrdinal("PriceBatchProcessor"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("RecvLog_Store_Limit"))) Then
                returnUser.RecvLogStoreLimit = results.GetInt32(results.GetOrdinal("RecvLog_Store_Limit"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("SuperUser"))) Then
                returnUser.SuperUser = results.GetBoolean(results.GetOrdinal("SuperUser"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Telxon_Cycle_Count"))) Then
                returnUser.TelxonCycleCount = results.GetBoolean(results.GetOrdinal("Telxon_Cycle_Count"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Telxon_Distribution"))) Then
                returnUser.TelxonDistribution = results.GetBoolean(results.GetOrdinal("Telxon_Distribution"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Telxon_Enabled"))) Then
                returnUser.TelxonEnabled = results.GetBoolean(results.GetOrdinal("Telxon_Enabled"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Telxon_Orders"))) Then
                returnUser.TelxonOrders = results.GetBoolean(results.GetOrdinal("Telxon_Orders"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Telxon_Store_Limit"))) Then
                returnUser.TelxonStoreLimit = results.GetInt32(results.GetOrdinal("Telxon_Store_Limit"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Telxon_SuperUser"))) Then
                returnUser.TelxonSuperuser = results.GetBoolean(results.GetOrdinal("Telxon_SuperUser"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Telxon_Transfers"))) Then
                returnUser.TelxonTransfers = results.GetBoolean(results.GetOrdinal("Telxon_Transfers"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Telxon_Waste"))) Then
                returnUser.TelxonWaste = results.GetBoolean(results.GetOrdinal("Telxon_Waste"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Vendor_Administrator"))) Then
                returnUser.VendorAdministrator = results.GetBoolean(results.GetOrdinal("Vendor_Administrator"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Vendor_Limit"))) Then
                returnUser.VendorLimit = results.GetInt32(results.GetOrdinal("Vendor_Limit"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Warehouse"))) Then
                returnUser.Warehouse = results.GetBoolean(results.GetOrdinal("Warehouse"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("PromoAccessLevel"))) Then
                returnUser.PromoAccessLevel = results.GetInt16(results.GetOrdinal("PromoAccessLevel"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("DCAdmin"))) Then
                returnUser.DCAdmin = results.GetBoolean(results.GetOrdinal("DCAdmin"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("VendorCostDiscrepancyAdmin"))) Then
                returnUser.VendorCostDiscrepancyAdmin = results.GetBoolean(results.GetOrdinal("VendorCostDiscrepancyAdmin"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("POApprovalAdmin"))) Then
                returnUser.POApprovalAdmin = results.GetBoolean(results.GetOrdinal("POApprovalAdmin"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("EInvoicing_Administrator"))) Then
                returnUser.EInvoicingAdministrator = results.GetBoolean(results.GetOrdinal("EInvoicing_Administrator"))
            End If

            ' SLIM web app
            If (Not results.IsDBNull(results.GetOrdinal("UserAdmin"))) Then
                returnUser.SLIMUserAdmin = results.GetBoolean(results.GetOrdinal("UserAdmin"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("ItemRequest"))) Then
                returnUser.SLIMItemRequest = results.GetBoolean(results.GetOrdinal("ItemRequest"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("VendorRequest"))) Then
                returnUser.SLIMVendorRequest = results.GetBoolean(results.GetOrdinal("VendorRequest"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("IRMAPush"))) Then
                returnUser.SLIMPushToIRMA = results.GetBoolean(results.GetOrdinal("IRMAPush"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("StoreSpecials"))) Then
                returnUser.SLIMStoreSpecials = results.GetBoolean(results.GetOrdinal("StoreSpecials"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("RetailCost"))) Then
                returnUser.SLIMRetailCost = results.GetBoolean(results.GetOrdinal("RetailCost"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("Authorizations"))) Then
                returnUser.SLIMAuthorizations = results.GetBoolean(results.GetOrdinal("Authorizations"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("WebQuery"))) Then
                returnUser.SLIMSecureQuery = results.GetBoolean(results.GetOrdinal("WebQuery"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("ScaleInfo"))) Then
                returnUser.SLIMScaleInfo = results.GetBoolean(results.GetOrdinal("ScaleInfo"))
            End If

            Return returnUser
        End Function

        Public Shared Function ValidateIRMALogin(ByRef currentUser As UserBO) As UserBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim returnUser As UserBO = Nothing

            'setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "@UserName"
            currentParam.Value = currentUser.UserName
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("ValidateLogin", paramList)

                ' Process the result record
                While (results.Read())
                    ' Initialize a new UserBO from the db record
                    returnUser = PopulateUserFromValidateResults(results)
                    returnUser.UserName = currentUser.UserName
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
            Return returnUser
        End Function

        Public Shared Function ValidateIRMALogin(ByVal currentUserName As String) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataTable = Nothing
            Dim returnUser As Boolean = False

            'setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "@UserName"
            currentParam.Value = currentUserName
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataTable("ValidateLogin", paramList)

                If results.Rows.Count > 0 Then
                    returnUser = True
                End If

            Catch e As DataFactoryException
                Logger.LogError("Exception: ", Nothing, e)
                'send message about exception
                ErrorHandler.ProcessError(ErrorType.DataFactoryException, SeverityLevel.Warning, e)
            End Try

            Return returnUser

        End Function

        Public Shared Function GetUser(ByVal UserID As Integer) As DataTable

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataTable = Nothing
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            Try

                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "User_ID"
                currentParam.Value = UserID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataTable("GetUser", paramList)

            Catch ex As Exception
                Throw ex
            End Try

            Return results

        End Function

        Public Shared Function GetUserSubTeam(ByVal UserID As Integer) As DataTable

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataTable = Nothing
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            Try

                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "User_ID"
                currentParam.Value = UserID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataTable("GetUsersSubteamAssignments", paramList)

            Catch ex As Exception
                Throw ex
            End Try

            Return results

        End Function

        Public Shared Function GetUserStoreTeamTitle(ByVal UserID As Integer) As DataTable

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataTable = Nothing
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            Try

                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "User_ID"
                currentParam.Value = UserID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataTable("GetUserStoreTeamTitleByUser", paramList)

            Catch ex As Exception
                Throw ex
            End Try

            Return results

        End Function
#End Region

#Region "Create, Update Methods"
        ''' <summary>
        ''' Creates the ArrayList for the parameters that are common between inserts and updates.
        ''' </summary>
        ''' <param name="currentUser"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function DefineUserParams(ByRef currentUser As UserBO) As ArrayList
            'setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "@AccountEnabled"
            currentParam.Value = currentUser.AccountEnabled
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@UserName"
            currentParam.Value = currentUser.UserName
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@FullName"
            currentParam.Value = currentUser.FullName
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@EMail"
            currentParam.Value = currentUser.Email
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@Pager_Email"
            currentParam.Value = currentUser.PagerEmail
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@Phone_Number"
            currentParam.Value = currentUser.PhoneNumber
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@Fax_Number"
            currentParam.Value = currentUser.FaxNumber
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@Title"
            If currentUser.Title = Nothing Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = currentUser.Title
            End If
            currentParam.Value = currentUser.Title
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@Printer"
            currentParam.Value = currentUser.Printer
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@CoverPage"
            currentParam.Value = currentUser.CoverPage
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@SuperUser"
            currentParam.Value = currentUser.SuperUser
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@PO_Accountant"
            currentParam.Value = currentUser.POAccountant
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@Accountant"
            currentParam.Value = currentUser.Accountant
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@Distributor"
            currentParam.Value = currentUser.Distributor
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@FacilityCreditProcessor"
            currentParam.Value = currentUser.FacilityCreditProcessor
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@Buyer"
            currentParam.Value = currentUser.Buyer
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@Coordinator"
            currentParam.Value = currentUser.Coordinator
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@Item_Administrator"
            currentParam.Value = currentUser.ItemAdministrator
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@Vendor_Administrator"
            currentParam.Value = currentUser.VendorAdministrator
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@Lock_Administrator"
            currentParam.Value = currentUser.LockAdministrator
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@Warehouse"
            currentParam.Value = currentUser.Warehouse
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@PriceBatchProcessor"
            currentParam.Value = currentUser.PriceBatchProcessor
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@Inventory_Administrator"
            currentParam.Value = currentUser.InventoryAdministrator
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@BatchBuildOnly"
            currentParam.Value = currentUser.BatchBuildOnly
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@DCAdmin"
            currentParam.Value = currentUser.DCAdmin
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@DeletePO"
            currentParam.Value = currentUser.DeletePO
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@TaxAdministrator"
            currentParam.Value = currentUser.TaxAdministrator
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@CostAdmin"
            currentParam.Value = currentUser.CostAdmin
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@VendorCostDiscrepancyAdmin"
            currentParam.Value = currentUser.VendorCostDiscrepancyAdmin
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@POApprovalAdmin"
            currentParam.Value = currentUser.POApprovalAdmin
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@POEditor"
            currentParam.Value = currentUser.POEditor
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@Telxon_Store_Limit"
            ' 0 indicates all stores so save a null
            If currentUser.TelxonStoreLimit = 0 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = currentUser.TelxonStoreLimit
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@RecvLog_Store_Limit"
            ' 0 indicates all stores so save a null
            If currentUser.TelxonStoreLimit = 0 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = currentUser.TelxonStoreLimit
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ''PROMO PLANNER
            currentParam = New DBParam
            currentParam.Name = "@PromoAccessLevel"
            ' if no value is set, save a null
            If Not currentUser.PromoAccessLevel = Nothing Then
                currentParam.Value = currentUser.PromoAccessLevel
            Else
                currentParam.Value = DBNull.Value
            End If
            currentParam.Type = DBParamType.SmallInt
            paramList.Add(currentParam)

            ''ADMIN PARAMETERS
            currentParam = New DBParam
            currentParam.Name = "@ApplicationConfigAdmin"
            currentParam.Value = currentUser.ApplicationConfigAdmin
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@SystemConfigurationAdministrator"
            currentParam.Value = currentUser.SystemConfigurationAdministrator
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@DataAdministrator"
            currentParam.Value = currentUser.DataAdministrator
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@JobAdministrator"
            currentParam.Value = currentUser.JobAdministrator
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@POSInterfaceAdministrator"
            currentParam.Value = currentUser.POSInterfaceAdministrator
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@StoreAdministrator"
            currentParam.Value = currentUser.StoreAdministrator
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@SecurityAdministrator"
            currentParam.Value = currentUser.SecurityAdministrator
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@UserMaintenance"
            currentParam.Value = currentUser.UserMaintenance
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            ''SLIM PARAMETERS

            currentParam = New DBParam
            currentParam.Name = "@UserAdmin"
            currentParam.Value = currentUser.SLIMUserAdmin
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@ItemRequest"
            currentParam.Value = currentUser.SLIMItemRequest
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@VendorRequest"
            currentParam.Value = currentUser.SLIMVendorRequest
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@IRMAPush"
            currentParam.Value = currentUser.SLIMPushToIRMA
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@StoreSpecials"
            currentParam.Value = currentUser.SLIMStoreSpecials
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@RetailCost"
            currentParam.Value = currentUser.SLIMRetailCost
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@Authorizations"
            currentParam.Value = currentUser.SLIMAuthorizations
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@ECommerce"
            currentParam.Value = currentUser.SLIMECommerce
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@WebQuery"
            currentParam.Value = currentUser.SLIMSecureQuery
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@ScaleInfo"
            currentParam.Value = currentUser.SLIMScaleInfo
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@EInvoicingAdmin"
            currentParam.Value = currentUser.EInvoicingAdministrator
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@Shrink"
            currentParam.Value = currentUser.Shrink
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@ShrinkAdmin"
            currentParam.Value = currentUser.ShrinkAdmin
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            Return paramList
        End Function

        ''' <summary>
        ''' Insert a new record into the Users table.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function AddUserRecord(ByRef currentUser As UserBO) As UserBO

            Logger.LogDebug("AddUserRecord entry", Nothing)

            Dim retVal As New ArrayList
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            ' setup parameters for stored proc
            Dim paramList As ArrayList = DefineUserParams(currentUser)

            Dim currentParam As DBParam

            ' add the param for the UserId OUTPUT
            currentParam = New DBParam
            currentParam.Name = "User_Id"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure to insert the new Users record.
            retVal = factory.ExecuteStoredProcedure("Administration_UserAdmin_InsertUser", paramList)

            currentUser.UserId = retVal(0)

            Return currentUser

            Logger.LogDebug("AddUserRecord exit", Nothing)

        End Function

        ''' <summary>
        ''' Update an existing record in the Users table.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub UpdateUserRecord(ByRef currentUser As UserBO)
            Logger.LogDebug("UpdateUserRecord entry", Nothing)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            ' setup parameters for stored proc
            Dim paramList As ArrayList = DefineUserParams(currentUser)
            Dim currentParam As DBParam

            ' add the param for the UserId
            currentParam = New DBParam
            currentParam.Name = "User_Id"
            currentParam.Value = currentUser.UserId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure to update a Users record.
            factory.ExecuteStoredProcedure("Administration_UserAdmin_UpdateUser", paramList)
            Logger.LogDebug("UpdateUserRecord exit", Nothing)
        End Sub

        Public Shared Function IsUserInSLIM(ByVal userID As Integer) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Return CType(factory.ExecuteScalar("SELECT dbo.fn_IsUserInSLIM('" & CStr(userID) & "')"), Boolean)
        End Function

        Public Function IsUserAssignedToTeam(ByVal iUserId As Integer, ByVal iSubTeamNo As Integer) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Return CType(factory.ExecuteScalar("SELECT dbo.fn_IsUserAssignedToTeam(" & CStr(iUserId) & ", " & CStr(iSubTeamNo) & ")"), Boolean)
        End Function
#End Region

#Region "Delete methods"
        ''' <summary>
        ''' Disable a user account in the Users table.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub DeleteUserRecord(ByRef currentUser As UserBO)
            Logger.LogDebug("DeleteUserRecord entry", Nothing)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            'setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' add the param for the UserId
            currentParam = New DBParam
            currentParam.Name = "User_Id"
            currentParam.Value = currentUser.UserId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure to delete the User record.
            factory.ExecuteStoredProcedure("Administration_UserAdmin_DeleteUser", paramList)
            Logger.LogDebug("DeleteUserRecord exit", Nothing)
        End Sub

        ''' <summary>
        ''' Disable a user account in the Users table.
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub DeleteUserRecord(ByVal UserID As Integer)
            Logger.LogDebug("DeleteUserRecord entry", Nothing)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            'setup parameters for stored proc
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' add the param for the UserId
            currentParam = New DBParam
            currentParam.Name = "User_Id"
            currentParam.Value = UserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ' Execute the stored procedure to delete the User record.
            factory.ExecuteStoredProcedure("Administration_UserAdmin_DeleteUser", paramList)
            Logger.LogDebug("DeleteUserRecord exit", Nothing)
        End Sub
#End Region
    End Class
End Namespace
