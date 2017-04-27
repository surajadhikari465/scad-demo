Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Imports log4net

Namespace WholeFoods.IRMA.ItemHosting.DataAccess

    Public Class ShelfLifeDAO

        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


        Public Shared Function GetShelfLifeList() As ArrayList

            logger.Debug("GetShelfLifeList Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim shelfLife As ShelfLifeBO
            Dim shelfLifeList As New ArrayList
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetShelfLifeAndID")

                While results.Read
                    shelfLife = New ShelfLifeBO()
                    shelfLife.ShelfLifeID = results.GetInt32(results.GetOrdinal("ShelfLife_ID"))
                    shelfLife.ShelfLifeName = results.GetString(results.GetOrdinal("ShelfLife_Name"))

                    shelfLifeList.Add(shelfLife)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetShelfLifeList Exit")


            Return shelfLifeList
        End Function

    End Class

End Namespace