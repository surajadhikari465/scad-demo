Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Imports log4net

Namespace WholeFoods.IRMA.ItemHosting.DataAccess
    Public Class ItemManagerDAO
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


        Public Shared Function GetItemManagers() As DataTable

            logger.Debug("GetItemManagers Entry")

            Dim dtItemManager As New DataTable()
            Dim row As DataRow
            Dim Keys(0) As DataColumn
            Dim dc As DataColumn

            dc = New DataColumn("Manager_ID", GetType(Byte))
            dtItemManager.Columns.Add(dc)
            Keys(0) = dc
            dtItemManager.PrimaryKey = Keys

            dtItemManager.Columns.Add(New DataColumn("Value", GetType(String)))
            Try
                gRSRecordset = SQLOpenRecordSet("EXEC GetItemManagers", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                System.Windows.Forms.Cursor.Current = Cursors.WaitCursor

                Do While Not gRSRecordset.EOF
                    row = dtItemManager.NewRow
                    row("Manager_ID") = gRSRecordset.Fields("Manager_ID").Value
                    row("Value") = gRSRecordset.Fields("Value").Value
                    dtItemManager.Rows.Add(row)
                    gRSRecordset.MoveNext()
                Loop

            Finally
                If gRSRecordset IsNot Nothing Then
                    gRSRecordset.Close()
                    gRSRecordset = Nothing
                End If
            End Try
            System.Windows.Forms.Cursor.Current = Cursors.Default

            logger.Debug("GetItemManagers Exit")
            Return dtItemManager

        End Function

    End Class
End Namespace
