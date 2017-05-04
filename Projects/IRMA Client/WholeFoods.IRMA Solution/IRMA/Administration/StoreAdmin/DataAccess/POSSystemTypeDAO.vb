Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Administration.StoreAdmin.DataAccess

    Public Class POSSystemTypeDAO

        ' Set the class type for logging
        Private Shared CLASSTYPE As Type = System.Type.GetType("IRMA.Administration.StoreAdmin.DataAccess.POSSystemTypeDAO")

        Public Shared Function GetPOSSystemTypes() As ArrayList
            Logger.LogDebug("GetPOSSystemTypes entry", CLASSTYPE)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim systemList As New ArrayList
            Dim pos As POSSystemType

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetPOSSystemTypes")

                While results.Read
                    pos = New POSSystemType()
                    pos.POSSystemID = results.GetInt32(results.GetOrdinal("POSSystemId"))
                    pos.POSSystemType = results.GetString(results.GetOrdinal("POSSystemType"))
                    systemList.Add(pos)
                End While
            Catch ex As Exception
                Throw ex
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Logger.LogDebug("GetPOSSystemTypes exit", CLASSTYPE)

            Return systemList
        End Function

    End Class

End Namespace