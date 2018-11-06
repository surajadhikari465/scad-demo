Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports WholeFoods.Utility.DataAccess


Public Class DALManageUsers

    Private _userID As Integer
    Private con As String = ConfigurationManager.ConnectionStrings("MultiPOTool").ConnectionString
    Private _userFields As ArrayList

    Sub New()



    End Sub
    Public Property UserID() As Integer
        Get

        End Get
        Set(ByVal value As Integer)

        End Set
    End Property

    Public Property UserFields() As ArrayList
        Get

        End Get
        Set(ByVal value As ArrayList)

        End Set
    End Property

    Public Function IsAdmin(ByVal UserID As Integer) As Boolean

        Dim factory As New DataFactory(con, True)

        Dim sql As New StringBuilder
        Dim result As Object = Nothing

        sql.Append("select Administrator from Users where UserID = " & UserID.ToString())

        Try
            result = factory.ExecuteScalar(sql.ToString)
        Catch ex As Exception
            Debug.WriteLine(ex.InnerException)
            Debug.WriteLine(ex.Message)
        End Try

        Return CBool(result)

    End Function

    Public Function InsertUser(ByVal UserFields As ArrayList) As Integer


        Dim factory As New DataFactory(con, True)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam
        Dim results As ArrayList

        Try
            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "UserName"
            currentParam.Value = UserFields.Item(0)
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "RegionID"
            currentParam.Value = UserFields.Item(1)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "GlobalBuyer"
            currentParam.Value = UserFields.Item(2)
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Administrator"
            currentParam.Value = UserFields.Item(3)
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Active"
            currentParam.Value = UserFields.Item(4)
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)


            currentParam = New DBParam
            currentParam.Name = "Email"
            currentParam.Value = IIf(UserFields.Item(5) Is Nothing, DBNull.Value, UserFields.Item(5))
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "CCEmail"
            currentParam.Value = IIf(UserFields.Item(6) Is Nothing, DBNull.Value, UserFields.Item(6))
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Output"
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            results = factory.ExecuteStoredProcedure("InsertUser", paramList)
        Catch ex As Exception
            Throw New Exception("User Cannot be Inserted - ", ex.InnerException)
        End Try

        Return CInt(results.Item(0).ToString)

    End Function

    Public Sub UpdateUser(ByVal UserFields As ArrayList)
        Dim factory As New DataFactory(con, True)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam
        Dim results As ArrayList

        Try
            ' setup parameters for stored proc

            currentParam = New DBParam
            currentParam.Name = "UserID"
            currentParam.Value = UserFields.Item(0)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UserName"
            currentParam.Value = UserFields.Item(1)
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "RegionID"
            currentParam.Value = UserFields.Item(2)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "GlobalBuyer"
            currentParam.Value = UserFields.Item(3)
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Administrator"
            currentParam.Value = UserFields.Item(4)
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Active"
            currentParam.Value = UserFields.Item(5)
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Email"
            currentParam.Value = IIf(UserFields.Item(6) Is Nothing, DBNull.Value, UserFields.Item(6))
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "CCEmail"
            currentParam.Value = IIf(UserFields.Item(7) Is Nothing, DBNull.Value, UserFields.Item(7))
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)


            results = factory.ExecuteStoredProcedure("UpdateUser", paramList)

        Catch ex As Exception
            Throw New Exception("User Cannot be Updated - ", ex.InnerException)
        End Try

    End Sub

    Public Sub DeleteUser(ByVal UserID As Integer)

        Dim factory As New DataFactory(con, True)
        Try
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "UserID"
            currentParam.Value = UserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("DeleteUser", paramList)
        Catch ex As Exception
            Throw New Exception("User Cannot be Deleted - ", ex.InnerException)
        End Try

    End Sub

    Public Function GetAllUsers() As DataSet

        Dim factory As New DataFactory(con, True)
        Dim ds As DataSet

        Try

            ds = factory.GetStoredProcedureDataSet("GetAllUsers")

        Catch ex As Exception

            Throw ex

        End Try

        Return ds

    End Function

    Public Function GetRegions() As DataSet

        Dim factory As New DataFactory(con, True)
        Dim ds As DataSet

        Try

            ds = factory.GetDataSet("select RegionID, RegionName from Regions")

        Catch ex As Exception

            Throw ex

        End Try

        Return ds

    End Function

    Public Function GetAppVersion() As String

        Dim factory As New DataFactory(con, True)
        Dim results As String

        Try

            results = factory.ExecuteScalar("Select AppVersion_Client from version")

        Catch ex As Exception

            Throw ex

        End Try

        Return results

    End Function
End Class
