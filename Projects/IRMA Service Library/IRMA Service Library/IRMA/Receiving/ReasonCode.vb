Imports WholeFoods.ServiceLibrary.DataAccess
Imports WholeFoods.ServiceLibrary.IRMA.Common
Imports System.Linq
Imports System.Text.RegularExpressions
Imports System.Xml
Imports System.Configuration

Namespace IRMA
    <DataContract()>
    Public Class ReasonCode

        <DataMember()>
        Public Property ReasonCodeID As Integer
        <DataMember()>
        Public Property ReasonCodeAbbreviation As String
        <DataMember()>
        Public Property ReasonCodeDescription As String

        Public Shared Function GetReasonCodesByType(ByVal reasonCodeType As String) As List(Of ReasonCode)

            logger.Info("GetReasonCodesByType() - Enter")

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim dataTable As DataTable
            Dim parameterList As New ArrayList
            Dim currentParameter As DBParam
            Dim reasonCodeList As New List(Of ReasonCode)


            ' ******* Parameters ************
            currentParameter = New DBParam
            currentParameter.Name = "ReasonCodeTypeAbbr"
            currentParameter.Value = reasonCodeType
            currentParameter.Type = DBParamType.String
            parameterList.Add(currentParameter)

            Try
                dataTable = factory.GetStoredProcedureDataTable("ReasonCodes_GetDetailsForType", parameterList)

                For Each dataRow As DataRow In dataTable.Rows
                    Dim reasonCode As New ReasonCode
                    reasonCode.ReasonCodeID = dataRow.Item("ReasonCodeDetailID")
                    reasonCode.ReasonCodeAbbreviation = dataRow.Item("ReasonCode")
                    reasonCode.ReasonCodeDescription = dataRow.Item("ReasonCodeDesc")
                    reasonCodeList.Add(reasonCode)
                Next

                Return reasonCodeList
            Catch ex As Exception
                Throw ex
            Finally
                connectionCleanup(factory)
            End Try
        End Function

    End Class
End Namespace