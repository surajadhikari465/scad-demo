Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports Microsoft.VisualBasic
Imports msda = Microsoft.Practices.EnterpriseLibrary.Data
Imports StoreOrderGuide.Common

Public Class Dal
    Private Shared _defaultCommandTimeout As Integer = HttpContext.Current.Application("defaultCommandTimeout")
#Region "Catalog Class"
    Public Shared Sub PopulateCatalog(ByRef thisCatalog As Catalog, ByVal dr As Data.Common.DbDataReader)
        Try
            If dr.Read Then
                thisCatalog.CatalogID = GetValue(dr, "CatalogID", "")
                thisCatalog.ManagedBy = GetValue(dr, "ManagedBy", "")
                thisCatalog.ManagedByID = GetValue(dr, "ManagedByID", "")
                thisCatalog.CatalogCode = GetValue(dr, "CatalogCode", "")
                thisCatalog.Description = GetValue(dr, "Description", "")
                thisCatalog.Published = GetValue(dr, "Published", "")
                thisCatalog.ExpectedDate = GetValue(dr, "ExpectedDate", "")
                thisCatalog.SubTeam = GetValue(dr, "SubTeam", "")
                thisCatalog.InsertDate = GetValue(dr, "InsertDate", "")
                thisCatalog.UpdateDate = GetValue(dr, "UpdateDate", "")
                thisCatalog.InsertUser = GetValue(dr, "InsertUser", "")
                thisCatalog.UpdateUser = GetValue(dr, "UpdateUser", "")
            End If
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message, ex.InnerException)
        End Try
    End Sub

    Public Shared Function GetCatalogs(ByVal StoreID As Integer, ByVal SubTeamID As Integer, ByVal ZoneID As Integer, ByVal Published As Boolean, ByVal CatalogCode As String, ByVal Order As Boolean, ByVal CatalogID As Integer) As Data.DataSet
        Try
            Dim result As Data.DataSet
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_GetCatalogs")
                db.AddInParameter(cmd, "CatalogID", Data.SqlDbType.Int, CatalogID)
                db.AddInParameter(cmd, "StoreID", Data.SqlDbType.Int, StoreID)
                db.AddInParameter(cmd, "SubTeamID", Data.SqlDbType.Int, SubTeamID)
                db.AddInParameter(cmd, "ZoneID", Data.SqlDbType.Int, ZoneID)
                db.AddInParameter(cmd, "Published", Data.SqlDbType.Bit, Published)
                db.AddInParameter(cmd, "CatalogCode", Data.SqlDbType.VarChar, CatalogCode)
                db.AddInParameter(cmd, "Order", Data.SqlDbType.Bit, Order)
                cmd.CommandTimeout = _defaultCommandTimeout
                result = db.ExecuteDataSet(cmd)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function SetCatalog(ByRef thisCatalog As Catalog, ByVal CatalogID As Integer) As Boolean
        Try
            Dim result As Boolean = False
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_SetCatalog")
                db.AddInParameter(cmd, "CatalogID", Data.SqlDbType.Int, CatalogID)
                db.AddInParameter(cmd, "ManagedByID", Data.SqlDbType.Int, thisCatalog.ManagedByID)
                db.AddInParameter(cmd, "CatalogCode", Data.SqlDbType.VarChar, thisCatalog.CatalogCode)
                db.AddInParameter(cmd, "Description", Data.SqlDbType.VarChar, thisCatalog.Description)
                db.AddInParameter(cmd, "Details", Data.SqlDbType.VarChar, thisCatalog.Details)
                db.AddInParameter(cmd, "Published", Data.SqlDbType.Bit, thisCatalog.Published)
                db.AddInParameter(cmd, "SubTeam", Data.SqlDbType.Int, thisCatalog.SubTeam)
                db.AddInParameter(cmd, "ExpectedDate", Data.SqlDbType.Bit, thisCatalog.ExpectedDate)
                db.AddInParameter(cmd, "UpdateUser", Data.SqlDbType.VarChar, thisCatalog.UpdateUser)
                cmd.CommandTimeout = _defaultCommandTimeout
                result = (db.ExecuteNonQuery(cmd) > 0)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function AddCatalog(ByRef thisCatalog As Catalog, ByVal Copy As Boolean, ByVal CatalogID As Integer) As Boolean
        Try
            Dim result As Boolean = False
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_AddCatalog")
                db.AddInParameter(cmd, "CatalogID", Data.SqlDbType.Int, CatalogID)
                db.AddInParameter(cmd, "ManagedByID", Data.SqlDbType.Int, thisCatalog.ManagedByID)
                db.AddInParameter(cmd, "CatalogCode", Data.SqlDbType.VarChar, thisCatalog.CatalogCode)
                db.AddInParameter(cmd, "Description", Data.SqlDbType.VarChar, thisCatalog.Description)
                db.AddInParameter(cmd, "Published", Data.SqlDbType.Bit, thisCatalog.Published)
                db.AddInParameter(cmd, "SubTeam", Data.SqlDbType.Int, thisCatalog.SubTeam)
                db.AddInParameter(cmd, "ExpectedDate", Data.SqlDbType.Bit, thisCatalog.ExpectedDate)
                db.AddInParameter(cmd, "InsertUser", Data.SqlDbType.VarChar, thisCatalog.InsertUser)
                db.AddInParameter(cmd, "Details", Data.SqlDbType.VarChar, thisCatalog.Details)
                db.AddInParameter(cmd, "Copy", Data.SqlDbType.Bit, Copy)
                cmd.CommandTimeout = _defaultCommandTimeout
                result = (db.ExecuteNonQuery(cmd) > 0)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function DelCatalog(ByVal CatalogID As Integer) As Boolean
        Try
            Dim result As Boolean = False
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_DelCatalog")
                db.AddInParameter(cmd, "CatalogID", Data.SqlDbType.Int, CatalogID)
                cmd.CommandTimeout = _defaultCommandTimeout
                result = (db.ExecuteNonQuery(cmd) > 0)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function MassPublishCatalogs(ByVal CatalogIDs As String, ByVal Published As Boolean, ByVal UpdateUser As String) As Boolean
        Try
            Dim result As Boolean = False
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_MassPublishCatalogs")
                db.AddInParameter(cmd, "CatalogIDs", Data.SqlDbType.VarChar, CatalogIDs)
                db.AddInParameter(cmd, "Published", Data.SqlDbType.VarChar, Published)
                db.AddInParameter(cmd, "UpdateUser", Data.SqlDbType.VarChar, UpdateUser)
                cmd.CommandTimeout = _defaultCommandTimeout
                result = (db.ExecuteNonQuery(cmd) > 0)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function
#End Region

#Region "CatalogStore Class"
    Public Shared Function GetCatalogStores(ByVal CatalogID As Integer) As Data.DataSet
        Try
            Dim result As Data.DataSet
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_GetCatalogStores")
                db.AddInParameter(cmd, "CatalogID", Data.SqlDbType.Int, CatalogID)
                cmd.CommandTimeout = _defaultCommandTimeout
                result = db.ExecuteDataSet(cmd)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function AddCatalogStore(ByVal CatalogID As Integer, ByVal StoreID As Integer, ByVal UserName As String) As Boolean
        Try
            Dim result As Boolean = False
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_AddCatalogStore")
                db.AddInParameter(cmd, "CatalogID", Data.SqlDbType.Int, CatalogID)
                db.AddInParameter(cmd, "StoreNo", Data.SqlDbType.Int, StoreID)
                db.AddInParameter(cmd, "User", Data.SqlDbType.VarChar, UserName)
                cmd.CommandTimeout = _defaultCommandTimeout
                result = (db.ExecuteNonQuery(cmd) > 0)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function DelCatalogStore(ByVal CatalogStoreID As Integer) As Boolean
        Try
            Dim result As Boolean = False
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_DelCatalogStore")
                db.AddInParameter(cmd, "CatalogStoreID", Data.SqlDbType.Int, CatalogStoreID)
                cmd.CommandTimeout = _defaultCommandTimeout
                result = (db.ExecuteNonQuery(cmd) > 0)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function
#End Region

#Region "CatalogItem Class"
    Public Shared Function GetCatalogItems(ByVal CatalogID As Integer, ByVal StoreNo As Integer, ByVal Order As Boolean, ByVal Identifier As String, ByVal Description As String, ByVal SubTeamID As Integer, ByVal ClassID As Integer, ByVal Level3ID As Integer, ByVal BrandID As Integer) As Data.DataSet
        Try
            Dim result As Data.DataSet
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_GetCatalogItems")
                db.AddInParameter(cmd, "CatalogID", Data.SqlDbType.Int, CatalogID)
                db.AddInParameter(cmd, "CatalogItemID", Data.SqlDbType.Int, 0)
                db.AddInParameter(cmd, "StoreNo", Data.SqlDbType.Int, StoreNo)
                db.AddInParameter(cmd, "Order", Data.SqlDbType.Bit, Order)
                db.AddInParameter(cmd, "Identifier", Data.SqlDbType.VarChar, Identifier)
                db.AddInParameter(cmd, "Description", Data.SqlDbType.VarChar, Description)
                db.AddInParameter(cmd, "SubTeamID", Data.SqlDbType.Int, SubTeamID)
                db.AddInParameter(cmd, "ClassID", Data.SqlDbType.VarChar, ClassID)
                db.AddInParameter(cmd, "Level3ID", Data.SqlDbType.VarChar, Level3ID)
                db.AddInParameter(cmd, "BrandID", Data.SqlDbType.VarChar, BrandID)

                cmd.CommandTimeout = _defaultCommandTimeout

                result = db.ExecuteDataSet(cmd)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function DelCatalogItem(ByVal CatalogItemID As Integer) As Boolean
        Try
            Dim result As Boolean = False
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_DelCatalogItem")
                db.AddInParameter(cmd, "CatalogItemID", Data.SqlDbType.Int, CatalogItemID)
                cmd.CommandTimeout = _defaultCommandTimeout
                result = (db.ExecuteNonQuery(cmd) > 0)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function AddCatalogItem(ByVal CatalogID As Integer, ByVal ItemKey As Integer, ByVal UserName As String) As Boolean
        Try
            Dim result As Boolean = False
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_AddCatalogItem")
                db.AddInParameter(cmd, "CatalogID", Data.SqlDbType.Int, CatalogID)
                db.AddInParameter(cmd, "ItemKey", Data.SqlDbType.Int, ItemKey)
                db.AddInParameter(cmd, "User", Data.SqlDbType.VarChar, UserName)
                cmd.CommandTimeout = _defaultCommandTimeout
                result = (db.ExecuteNonQuery(cmd) > 0)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function SetCatalogItem(ByRef thisCatalogItem As CatalogItem, ByVal CatalogItemID As Integer) As Boolean
        Try
            Dim result As Boolean = False
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_SetCatalogItem")
                db.AddInParameter(cmd, "CatalogItemID", Data.SqlDbType.Int, thisCatalogItem.CatalogItemID)
                db.AddInParameter(cmd, "ItemNote", Data.SqlDbType.VarChar, thisCatalogItem.ItemNote)
                cmd.CommandTimeout = _defaultCommandTimeout
                result = (db.ExecuteNonQuery(cmd) > 0)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function
#End Region

#Region "Order Class"
    Public Shared Function AddOrder(ByRef thisOrder As Order, ByVal CatalogID As Integer) As Integer
        Try
            Dim result As Integer = 0
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_AddOrder")
                db.AddInParameter(cmd, "CatalogID", Data.SqlDbType.Int, CatalogID)
                db.AddInParameter(cmd, "VendorID", Data.SqlDbType.Int, thisOrder.VendorID)
                db.AddInParameter(cmd, "StoreID", Data.SqlDbType.Int, thisOrder.StoreID)
                db.AddInParameter(cmd, "UserID", Data.SqlDbType.Int, thisOrder.UserID)
                db.AddInParameter(cmd, "FromSubTeamID", Data.SqlDbType.Int, thisOrder.FromSubTeamID)
                db.AddInParameter(cmd, "ToSubTeamID", Data.SqlDbType.Int, thisOrder.ToSubTeamID)
                db.AddInParameter(cmd, "ExpectedDate", Data.SqlDbType.SmallDateTime, thisOrder.ExpectedDate)
                db.AddOutParameter(cmd, "CatalogOrderID", Data.SqlDbType.BigInt, 8)
                cmd.CommandTimeout = _defaultCommandTimeout
                db.ExecuteNonQuery(cmd)

                result = db.GetParameterValue(cmd, "CatalogOrderID")
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function SetOrder(ByRef thisOrder As Order, ByVal CatalogOrderID As Integer) As Data.DataSet
        Try
            Dim result As Data.DataSet
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_SetOrder")
                db.AddInParameter(cmd, "CatalogOrderID", Data.SqlDbType.Int, CatalogOrderID)

                cmd.CommandTimeout = _defaultCommandTimeout

                result = db.ExecuteDataSet(cmd)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function
#End Region

#Region "OrderItem Class"
    Public Shared Function AddOrderItem(ByRef thisOrderItem As OrderItem, ByVal CatalogOrderID As Integer) As Boolean
        Try
            Dim result As Boolean = False
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_AddOrderItem")
                db.AddInParameter(cmd, "CatalogItemID", Data.SqlDbType.Int, thisOrderItem.CatalogItemID)
                db.AddInParameter(cmd, "CatalogOrderID", Data.SqlDbType.Int, CatalogOrderID)
                db.AddInParameter(cmd, "Quantity", Data.SqlDbType.Int, thisOrderItem.Quantity)

                result = (db.ExecuteNonQuery(cmd) > 0)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function
#End Region

#Region "User Class"
    Public Shared Sub PopulateUser(ByRef thisUser As User, ByVal dr As Data.Common.DbDataReader)
        Try
            If dr.Read Then
                thisUser.Admin = GetValue(dr, "Admin", "")
                thisUser.Warehouse = GetValue(dr, "Warehouse", "")
                thisUser.Schedule = GetValue(dr, "Schedule", "")
                thisUser.Buyer = GetValue(dr, "Buyer", "")
                thisUser.SuperUser = GetValue(dr, "SuperUser", "")
                thisUser.StoreNo = GetValue(dr, "StoreNo", "")
                thisUser.Email = GetValue(dr, "Email", "")
                thisUser.UserName = GetValue(dr, "UserName", "")
                thisUser.UserID = GetValue(dr, "UserID", "")
            End If
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Shared Function GetUserDetails(ByRef thisUser As User) As Data.DataSet
        Try
            Dim result As Data.DataSet
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_GetUserDetails")
                db.AddInParameter(cmd, "UserName", Data.SqlDbType.VarChar, thisUser.UserName)
                cmd.CommandTimeout = _defaultCommandTimeout
                result = db.ExecuteDataSet(cmd)

                Using dr As Data.Common.DbDataReader = db.ExecuteReader(cmd)
                    PopulateUser(thisUser, dr)
                End Using
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function
#End Region

#Region "Admin Settings Class "
    Public Shared Sub PopulateAdminSetting(ByRef thisAdminSetting As AdminSetting, ByVal dr As Data.Common.DbDataReader)
        Try
            If dr.Read Then
                thisAdminSetting.AdminID = GetValue(dr, "AdminID", "")
                thisAdminSetting.AdminKey = GetValue(dr, "AdminKey", "")
                thisAdminSetting.AdminValue = GetValue(dr, "AdminValue", "")
            End If
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Shared Function AddAdminSetting(ByRef thisAdminSetting As AdminSetting) As Boolean
        Try
            Dim result As Boolean = False
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_AddAdminSetting")
                db.AddInParameter(cmd, "AdminKey", Data.SqlDbType.VarChar, thisAdminSetting.AdminKey)
                db.AddInParameter(cmd, "AdminValue", Data.SqlDbType.VarChar, thisAdminSetting.AdminValue)
                cmd.CommandTimeout = _defaultCommandTimeout
                result = (db.ExecuteNonQuery(cmd) > 0)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function GetAdminSettings() As Data.DataSet
        Try
            Dim result As Data.DataSet
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_GetAdminSettings")
                cmd.CommandTimeout = _defaultCommandTimeout
                result = db.ExecuteDataSet(cmd)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function GetAdminSetting(ByVal AdminKey As String) As String
        Try
            Dim result As String
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_GetAdminSetting")
                db.AddInParameter(cmd, "AdminKey", Data.SqlDbType.VarChar, AdminKey)
                cmd.CommandTimeout = _defaultCommandTimeout
                result = CType(Val(db.ExecuteScalar(cmd)), String)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function SetAdminSetting(ByRef thisAdminSetting As AdminSetting, ByVal AdminID As Integer) As Boolean
        Try
            Dim result As Boolean = False
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_SetAdminSetting")
                db.AddInParameter(cmd, "AdminID", Data.SqlDbType.Int, AdminID)
                db.AddInParameter(cmd, "AdminKey", Data.SqlDbType.VarChar, thisAdminSetting.AdminKey)
                db.AddInParameter(cmd, "AdminValue", Data.SqlDbType.VarChar, thisAdminSetting.AdminValue)
                cmd.CommandTimeout = _defaultCommandTimeout
                result = (db.ExecuteNonQuery(cmd) > 0)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function DelAdminSetting(ByVal AdminID As Integer) As Boolean
        Try
            Dim result As Boolean = False
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_DelAdminSetting")
                db.AddInParameter(cmd, "AdminID", Data.SqlDbType.Int, AdminID)
                cmd.CommandTimeout = _defaultCommandTimeout
                result = (db.ExecuteNonQuery(cmd) > 0)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function
#End Region

#Region "CatalogSchedule Class "
    Public Shared Sub PopulateCatalogSchedule(ByRef thisCatalogSchedule As CatalogSchedule, ByVal dr As Data.Common.DbDataReader)
        Try
            If dr.Read Then
                thisCatalogSchedule.CatalogScheduleID = GetValue(dr, "AdminID", "")
                thisCatalogSchedule.ManagedByID = GetValue(dr, "ManagedByID", "")
                thisCatalogSchedule.StoreNo = GetValue(dr, "StoreNo", "")
                thisCatalogSchedule.SubTeamNo = GetValue(dr, "SubTeamNo", "")
                thisCatalogSchedule.Mon = GetValue(dr, "Mon", "")
                thisCatalogSchedule.Tue = GetValue(dr, "Tue", "")
                thisCatalogSchedule.Wed = GetValue(dr, "Wed", "")
                thisCatalogSchedule.Thu = GetValue(dr, "Thu", "")
                thisCatalogSchedule.Fri = GetValue(dr, "Fri", "")
                thisCatalogSchedule.Sat = GetValue(dr, "Sat", "")
                thisCatalogSchedule.Sun = GetValue(dr, "Sun", "")
            End If
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Shared Function AddCatalogSchedule(ByRef thisCatalogSchedule As CatalogSchedule) As Boolean
        Try
            Dim result As Boolean = False
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_AddCatalogSchedule")
                db.AddInParameter(cmd, "ManagedByID", Data.SqlDbType.Int, thisCatalogSchedule.ManagedByID)
                db.AddInParameter(cmd, "StoreNo", Data.SqlDbType.Int, thisCatalogSchedule.StoreNo)
                db.AddInParameter(cmd, "SubTeamNo", Data.SqlDbType.Int, thisCatalogSchedule.SubTeamNo)
                db.AddInParameter(cmd, "Mon", Data.SqlDbType.Bit, thisCatalogSchedule.Mon)
                db.AddInParameter(cmd, "Tue", Data.SqlDbType.Bit, thisCatalogSchedule.Tue)
                db.AddInParameter(cmd, "Wed", Data.SqlDbType.Bit, thisCatalogSchedule.Wed)
                db.AddInParameter(cmd, "Thu", Data.SqlDbType.Bit, thisCatalogSchedule.Thu)
                db.AddInParameter(cmd, "Fri", Data.SqlDbType.Bit, thisCatalogSchedule.Fri)
                db.AddInParameter(cmd, "Sat", Data.SqlDbType.Bit, thisCatalogSchedule.Sat)
                db.AddInParameter(cmd, "Sun", Data.SqlDbType.Bit, thisCatalogSchedule.Sun)
                cmd.CommandTimeout = _defaultCommandTimeout
                result = db.ExecuteNonQuery(cmd)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function GetCatalogSchedules(ByRef thisCatalogSchedule As CatalogSchedule, ByVal CatalogScheduleID As Integer, ByVal ManagedByID As Integer, ByVal StoreNo As Integer, ByVal SubTeamNo As Integer) As Data.DataSet
        Try
            Dim result As Data.DataSet
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_GetCatalogSchedules")
                db.AddInParameter(cmd, "CatalogScheduleID", Data.SqlDbType.Int, CatalogScheduleID)
                db.AddInParameter(cmd, "ManagedByID", Data.SqlDbType.Int, ManagedByID)
                db.AddInParameter(cmd, "StoreNo", Data.SqlDbType.Int, StoreNo)
                db.AddInParameter(cmd, "SubTeamNo", Data.SqlDbType.Int, SubTeamNo)
                cmd.CommandTimeout = _defaultCommandTimeout
                result = db.ExecuteDataSet(cmd)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function SetCatalogSchedule(ByRef thisCatalogSchedule As CatalogSchedule, ByVal CatalogScheduleID As Integer) As Boolean
        Try
            Dim result As Boolean = False
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_SetCatalogSchedule")
                db.AddInParameter(cmd, "CatalogScheduleID", Data.SqlDbType.Int, CatalogScheduleID)
                db.AddInParameter(cmd, "ManagedByID", Data.SqlDbType.Int, thisCatalogSchedule.ManagedByID)
                db.AddInParameter(cmd, "StoreNo", Data.SqlDbType.Int, thisCatalogSchedule.StoreNo)
                db.AddInParameter(cmd, "SubTeamNo", Data.SqlDbType.Int, thisCatalogSchedule.SubTeamNo)
                db.AddInParameter(cmd, "Mon", Data.SqlDbType.Bit, thisCatalogSchedule.Mon)
                db.AddInParameter(cmd, "Tue", Data.SqlDbType.Bit, thisCatalogSchedule.Tue)
                db.AddInParameter(cmd, "Wed", Data.SqlDbType.Bit, thisCatalogSchedule.Wed)
                db.AddInParameter(cmd, "Thu", Data.SqlDbType.Bit, thisCatalogSchedule.Thu)
                db.AddInParameter(cmd, "Fri", Data.SqlDbType.Bit, thisCatalogSchedule.Fri)
                db.AddInParameter(cmd, "Sat", Data.SqlDbType.Bit, thisCatalogSchedule.Sat)
                db.AddInParameter(cmd, "Sun", Data.SqlDbType.Bit, thisCatalogSchedule.Sun)
                cmd.CommandTimeout = _defaultCommandTimeout
                result = (db.ExecuteNonQuery(cmd) > 0)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function DelCatalogSchedule(ByVal CatalogScheduleID As Integer) As Boolean
        Try
            Dim result As Boolean = False
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_DelCatalogSchedule")

                db.AddInParameter(cmd, "CatalogScheduleID", Data.SqlDbType.Int, CatalogScheduleID)
                cmd.CommandTimeout = _defaultCommandTimeout
                result = (db.ExecuteNonQuery(cmd) > 0)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function
#End Region

#Region "Utility Class"
    Public Shared Function AddError(ByVal ErrorMessage As Exception, ByVal UserName As String, ByVal UserComputer As String) As Integer
        Try
            Dim result As Integer = 0
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_AddError")
                db.AddInParameter(cmd, "UserName", Data.SqlDbType.VarChar, UserName)
                db.AddInParameter(cmd, "Workstation", Data.SqlDbType.VarChar, UserComputer)
                db.AddInParameter(cmd, "ErrorMessage", Data.SqlDbType.VarChar, ErrorMessage.Message)
                db.AddInParameter(cmd, "StackTrace", Data.SqlDbType.VarChar, ErrorMessage.StackTrace)
                db.AddOutParameter(cmd, "CatalogErrorID", Data.SqlDbType.BigInt, 8)
                cmd.CommandTimeout = _defaultCommandTimeout
                db.ExecuteNonQuery(cmd)

                result = db.GetParameterValue(cmd, "CatalogErrorID")
            End Using

            Return result
        Catch ex As Exception
            SendSupportEmail("StoreOrderGuide - Error", 0, ex.Message, ConfigurationManager.AppSettings("SupportEmail"))
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function GetManagedByList() As Data.DataSet
        Try
            Dim result As Data.DataSet
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_GetManagedByList")
                cmd.CommandTimeout = _defaultCommandTimeout
                result = db.ExecuteDataSet(cmd)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function GetStoreList() As Data.DataSet
        Try
            Dim result As Data.DataSet
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_GetStoreList")
                cmd.CommandTimeout = _defaultCommandTimeout
                result = db.ExecuteDataSet(cmd)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function GetSubTeamList(ByVal Catalog As Boolean) As Data.DataSet
        Try
            Dim result As Data.DataSet
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_GetSubTeamList")
                db.AddInParameter(cmd, "Catalog", Data.SqlDbType.Int, Catalog)
                cmd.CommandTimeout = _defaultCommandTimeout
                result = db.ExecuteDataSet(cmd)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function GetBrandList(ByVal Catalog As Boolean) As Data.DataSet
        Try
            Dim result As Data.DataSet
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_GetBrandList")
                db.AddInParameter(cmd, "Catalog", Data.SqlDbType.Int, Catalog)
                cmd.CommandTimeout = _defaultCommandTimeout
                result = db.ExecuteDataSet(cmd)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function GetClassList(ByVal Catalog As Boolean, ByVal SubTeamID As Integer) As Data.DataSet
        Try
            Dim result As Data.DataSet
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_GetClassList")
                db.AddInParameter(cmd, "Catalog", Data.SqlDbType.Int, Catalog)
                db.AddInParameter(cmd, "SubTeamID", Data.SqlDbType.Int, SubTeamID)
                cmd.CommandTimeout = _defaultCommandTimeout
                result = db.ExecuteDataSet(cmd)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function GetLevel3List(ByVal Catalog As Boolean, ByVal ClassID As Integer) As Data.DataSet
        Try
            Dim result As Data.DataSet
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_GetLevel3List")
                db.AddInParameter(cmd, "Catalog", Data.SqlDbType.Int, Catalog)
                db.AddInParameter(cmd, "ClassID", Data.SqlDbType.Int, ClassID)
                cmd.CommandTimeout = _defaultCommandTimeout
                result = db.ExecuteDataSet(cmd)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function GetVendorList(ByVal Catalog As Boolean) As Data.DataSet
        Try
            Dim result As Data.DataSet
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_GetVendorList")
                db.AddInParameter(cmd, "Catalog", Data.SqlDbType.Int, Catalog)
                cmd.CommandTimeout = _defaultCommandTimeout
                result = db.ExecuteDataSet(cmd)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function GetZoneList() As Data.DataSet
        Try
            Dim result As Data.DataSet
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_GetZoneList")
                cmd.CommandTimeout = _defaultCommandTimeout
                result = db.ExecuteDataSet(cmd)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Shared Function GetItemList(ByVal CatalogID As Integer, ByVal Identifier As String, ByVal Description As String, ByVal SubTeamID As Integer, ByVal ClassID As Integer, ByVal Level3ID As Integer, ByVal BrandID As Integer) As Data.DataSet
        Try
            Dim result As Data.DataSet
            Dim db As msda.Sql.SqlDatabase = GetItemCatalogDatabase()

            Using cmd As Data.Common.DbCommand = db.GetStoredProcCommand("SOG_GetItemList")
                db.AddInParameter(cmd, "CatalogID", Data.SqlDbType.Int, CatalogID)
                db.AddInParameter(cmd, "Identifier", Data.SqlDbType.VarChar, Identifier)
                db.AddInParameter(cmd, "Description", Data.SqlDbType.VarChar, Description)
                db.AddInParameter(cmd, "SubTeamID", Data.SqlDbType.Int, SubTeamID)
                db.AddInParameter(cmd, "ClassID", Data.SqlDbType.VarChar, ClassID)
                db.AddInParameter(cmd, "Level3ID", Data.SqlDbType.VarChar, Level3ID)
                db.AddInParameter(cmd, "BrandID", Data.SqlDbType.VarChar, BrandID)

                cmd.CommandTimeout = _defaultCommandTimeout

                result = db.ExecuteDataSet(cmd)
            End Using

            Return result
        Catch ex As Exception
            LogError(ex)
            Throw New Exception(ex.Message)
        End Try
    End Function
#End Region

End Class