Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess
Imports log4net

Namespace WholeFoods.IRMA.Ordering.DataAccess

    Public Class CostAdjustmentReasonDAO
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


        Public Shared Function NewCostAdjustmentReason(ByVal description As String) As Integer
            logger.Debug("NewCostAdjustmentReason Entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim command As SqlCommand = factory.GetDataCommand("InsertCostAdjustmentReason", Nothing, True)
            Dim param As New SqlParameter("Description", SqlDbType.VarChar)

            param.Value = description
            command.Parameters.Add(param)

            param = New SqlParameter("IsDefault", SqlDbType.Bit)
            param.Value = False
            command.Parameters.Add(param)

            param = New SqlParameter("CostAdjustmentReason_ID", SqlDbType.Int)
            param.Direction = ParameterDirection.Output
            command.Parameters.Add(param)

            command.ExecuteNonQuery()

            Return CInt(param.Value)

            logger.Debug("NewCostAdjustmentReason exit" & CInt(param.Value).ToString)
        End Function

    End Class

End Namespace