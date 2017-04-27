Imports log4net
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Administration.ReasonCodes.DataAccess

Namespace WholeFoods.IRMA.Administration.ReasonCodes.BusinessLogic

    Public Class ReasonCodeMaintenanceBO

        Public Function getReasonCodeType() As DataTable
            Dim DAO As ReasonCodeMaintenanceDAO = New ReasonCodeMaintenanceDAO()
            Dim dt As DataTable

            dt = DAO.getReasonCodeType

            Return dt
        End Function

        Public Function getReasonCodeDetail() As DataTable
            Dim DAO As ReasonCodeMaintenanceDAO = New ReasonCodeMaintenanceDAO()
            Dim dt As DataTable

            dt = DAO.getReasonCodeDetail

            Return dt
        End Function

        Public Function checkIfReasonCodeDetailExists(ByVal Reasoncode As String) As Boolean
            Dim DAO As ReasonCodeMaintenanceDAO = New ReasonCodeMaintenanceDAO()
            Dim exists As Boolean

            If DAO.checkIfReasonCodeDetailExists(Reasoncode).Rows.Count > 0 Then
                exists = True
            End If

            Return exists
        End Function

        Public Function checkIfReasonCodeTypeExists(ByVal ReasoncodeTypeAbbr As String) As Boolean
            Dim DAO As ReasonCodeMaintenanceDAO = New ReasonCodeMaintenanceDAO()
            Dim dt As DataTable
            Dim exists As Boolean

            dt = DAO.checkIfReasonCodeTypeExists(ReasoncodeTypeAbbr)

            If dt.Rows.Count > 0 Then
                exists = True
            End If

            Return exists
        End Function

        Public Function getReasonCodeDetailsForType(ByVal ReasoncodeTypeAbbr As String) As DataTable
            Dim DAO As ReasonCodeMaintenanceDAO = New ReasonCodeMaintenanceDAO()
            Dim dt As DataTable

            dt = DAO.getReasonCodeDetailsForType(ReasoncodeTypeAbbr)

            Return dt
        End Function

        Public Sub createReasonCodeType(ByVal ReasonCodeTypeAbbr As String, ByVal ReasonCodeTypeDesc As String)

            Dim DAO As ReasonCodeMaintenanceDAO = New ReasonCodeMaintenanceDAO()

            DAO.createReasonCodeType(ReasonCodeTypeAbbr, ReasonCodeTypeDesc)

        End Sub

        Public Sub createReasonCodeDetail(ByVal ReasonCode As String, ByVal ReasonCodeDesc As String, ByVal ReasonCodeExtDesc As String)

            Dim DAO As ReasonCodeMaintenanceDAO = New ReasonCodeMaintenanceDAO()

            DAO.createReasonCodeDetail(ReasonCode, ReasonCodeDesc, ReasonCodeExtDesc)

        End Sub

        Public Sub updateReasonCodeDetail(ByVal ReasonCode As String, ByVal ReasonCodeDesc As String, ByVal ReasonCodeExtDesc As String)

            Dim DAO As ReasonCodeMaintenanceDAO = New ReasonCodeMaintenanceDAO()

            DAO.updateReasonCodeDetail(ReasonCode, ReasonCodeDesc, ReasonCodeExtDesc)

        End Sub

        Public Sub addReasonCodeMapping(ByVal ReasonCodeTypeAbbr As String, ByVal ReasonCode As String)

            Dim DAO As ReasonCodeMaintenanceDAO = New ReasonCodeMaintenanceDAO()

            DAO.addReasonCodeMapping(ReasonCodeTypeAbbr, ReasonCode)

        End Sub

        Public Sub disableReasonCodeMapping(ByVal ReasonCodeTypeAbbr As String, ByVal ReasonCode As String)

            Dim DAO As ReasonCodeMaintenanceDAO = New ReasonCodeMaintenanceDAO()

            DAO.disableReasonCodeMapping(ReasonCodeTypeAbbr, ReasonCode)

        End Sub
    End Class
End Namespace
