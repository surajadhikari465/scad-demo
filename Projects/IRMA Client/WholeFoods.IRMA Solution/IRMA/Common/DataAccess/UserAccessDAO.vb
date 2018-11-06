Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Common.BusinessLogic
Imports WholeFoods.Utility.DataAccess
Imports log4net

Namespace WholeFoods.IRMA.Common.DataAccess
    Public Class UserAccessDAO

        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        ''' <summary>
        ''' Gets list of facilities by VendorName and Vendor_ID
        ''' </summary>
        ''' <returns>ArrayList of StoreListBO objects</returns>
        ''' <remarks></remarks>
        Public Shared Function GetUserDetails() As ArrayList

            logger.Debug("GetUserDetails Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim usersBO As UserAccessBO
            Dim UserList As New ArrayList
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetUsers")

                ' Adding Empty(To use it as optional)parameter to usersList which allows to select user.
                usersBO = New UserAccessBO()
                usersBO.UserID = 0
                usersBO.UserName = ""
                UserList.Add(usersBO)

                While results.Read
                    usersBO = New UserAccessBO()
                    usersBO.UserID = results.GetInt32(results.GetOrdinal("User_ID"))
                    usersBO.UserName = results.GetString(results.GetOrdinal("UserName"))
                    UserList.Add(usersBO)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetUserDetails Exit")


            Return UserList
        End Function
    End Class
End Namespace

