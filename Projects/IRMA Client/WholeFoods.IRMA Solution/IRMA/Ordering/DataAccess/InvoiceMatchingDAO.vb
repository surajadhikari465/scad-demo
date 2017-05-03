Imports log4net
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility
Imports Infragistics.Win

Public Class InvoiceMatchingDAO

    Private Shared Logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim factory As New DataFactory(DataFactory.ItemCatalog)

    ''' <summary>
    ''' Gets All Invoice Tolerances
    ''' </summary>
    ''' <remarks>Decided to hit the database only one rather than three separate queries.</remarks>
    ''' <returns>Reader with 3 Record Sets -</returns>
    ''' 
    Public Function GetInvoiceTolerances() As SqlDataReader

        Dim reader As SqlDataReader = Nothing
        Dim factory As New DataFactory(DataFactory.ItemCatalog)


        Logger.Debug("GetAllInvoiceTolerances Entry")

        reader = factory.GetStoredProcedureDataReader("GetAllInvoiceTolerances")



        Logger.Debug("GetAllInvoiceTolerances Exit")

        Return reader

    End Function

    Public Function UpdateInvoiceToleranceStore(ByVal Store_No As Integer, ByVal Vendor_Tolerance As Object, ByVal Vendor_Tolerance_Amount As Object, ByVal User_ID As Integer) As Integer

        Dim result As New ArrayList
        Dim currentParam As DBParam
        Dim paramList As New ArrayList

        Logger.Debug("UpdateInvoiceToleranceStore Entry")

        'setup parameters for stored proc
        currentParam = New DBParam
        currentParam.Name = "Store_No"
        currentParam.Value = Store_No
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Vendor_Tolerance"
        If Not Vendor_Tolerance.GetType.Name = "DBNull" Then
            currentParam.Value = Vendor_Tolerance
        Else
            currentParam.Value = DBNull.Value
        End If
        currentParam.Type = DBParamType.Money
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Vendor_Tolerance_Amount"
        If Not Vendor_Tolerance_Amount.GetType.Name = "DBNull" Then
            currentParam.Value = Vendor_Tolerance_Amount
        Else
            currentParam.Value = DBNull.Value
        End If
        currentParam.Type = DBParamType.Decimal
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "User_ID"
        currentParam.Value = User_ID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)


        result = factory.ExecuteStoredProcedure("UpdateInvoiceToleranceStore", paramList)
        Logger.Debug("UpdateInvoiceToleranceStore Exit")
    End Function

    Public Function UpdateInvoiceTolerance(ByVal Vendor_Tolerance As Object, ByVal Vendor_Tolerance_Amount As Object, ByVal User_ID As Integer) As Integer
        Dim result As New ArrayList
        Logger.Debug("UpdateInvoiceTolerance Entry")


        Dim currentParam As DBParam
        Dim paramList As New ArrayList

        'setup parameters for stored proc

        currentParam = New DBParam
        currentParam.Name = "Vendor_Tolerance"
        If Not Vendor_Tolerance.GetType.Name = "DBNull" Then
            currentParam.Value = Vendor_Tolerance
        Else
            currentParam.Value = DBNull.Value
        End If
        currentParam.Type = DBParamType.Money
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Vendor_Tolerance_Amount"
        If Not Vendor_Tolerance_Amount.GetType.Name = "DBNull" Then
            currentParam.Value = Vendor_Tolerance_Amount
        Else
            currentParam.Value = DBNull.Value
        End If
        currentParam.Type = DBParamType.Decimal
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "User_ID"
        currentParam.Value = User_ID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        result = factory.ExecuteStoredProcedure("UpdateInvoiceTolerance", paramList)
        Logger.Debug("UpdateInvoiceTolerance Exit")
    End Function

    Public Function UpdateInvoiceToleranceVendor(ByVal Vendor_ID As Integer, ByVal Vendor_Tolerance As Object, ByVal Vendor_Tolerance_Amount As Object, ByVal User_ID As Integer) As Integer

        Dim result As New ArrayList
        Logger.Debug("UpdateInvoiceToleranceVendor Entry")

        Dim currentParam As DBParam
        Dim paramList As New ArrayList

        'setup parameters for stored proc
        currentParam = New DBParam
        currentParam.Name = "Vendor_ID"
        currentParam.Value = Vendor_ID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Vendor_Tolerance"
        If Not Vendor_Tolerance.GetType.Name = "DBNull" Then
            currentParam.Value = Vendor_Tolerance
        Else
            currentParam.Value = DBNull.Value
        End If
        currentParam.Type = DBParamType.Money
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Vendor_Tolerance_Amount"
        If Not Vendor_Tolerance_Amount.GetType.Name = "DBNull" Then
            currentParam.Value = Vendor_Tolerance_Amount
        Else
            currentParam.Value = DBNull.Value
        End If
        currentParam.Type = DBParamType.Decimal
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "User_ID"
        currentParam.Value = User_ID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)


        result = factory.ExecuteStoredProcedure("UpdateInvoiceToleranceVendor", paramList)
        Logger.Debug("UpdateInvoiceToleranceVendor Exit")
    End Function

    Public Function InsertInvoiceToleranceStore(ByVal Store_No As Integer, ByVal Vendor_Tolerance As Object, ByVal Vendor_Tolerance_Amount As Object, ByVal User_ID As Integer) As Integer

        Dim result As New ArrayList
        Logger.Debug("InsertInvoiceToleranceStore Entry")

        Dim currentParam As DBParam
        Dim paramList As New ArrayList

        'setup parameters for stored proc
        currentParam = New DBParam
        currentParam.Name = "Store_No"
        currentParam.Value = Store_No
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Vendor_Tolerance"
        If Not Vendor_Tolerance.GetType.Name = "DBNull" Then
            currentParam.Value = Vendor_Tolerance
        Else
            currentParam.Value = DBNull.Value
        End If
        currentParam.Type = DBParamType.Money
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Vendor_Tolerance_Amount"
        If Not Vendor_Tolerance_Amount.GetType.Name = "DBNull" Then
            currentParam.Value = Vendor_Tolerance_Amount
        Else
            currentParam.Value = DBNull.Value
        End If
        currentParam.Type = DBParamType.Decimal
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "User_ID"
        currentParam.Value = User_ID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        result = factory.ExecuteStoredProcedure("InsertInvoiceToleranceStore", paramList)


        Logger.Debug("InsertInvoiceToleranceStore exit")
    End Function

    Public Function InsertInvoiceToleranceVendor(ByVal Vendor_ID As Integer, ByVal Vendor_Tolerance As Object, ByVal Vendor_Tolerance_Amount As Object, ByVal User_ID As Integer) As Integer
        Dim result As New ArrayList
        Logger.Debug("InsertInvoiceToleranceVendor Entry")

        Dim currentParam As DBParam
        Dim paramList As New ArrayList

        'setup parameters for stored proc
        currentParam = New DBParam
        currentParam.Name = "Vendor_ID"
        currentParam.Value = Vendor_ID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Vendor_Tolerance"
        If Not Vendor_Tolerance.GetType.Name = "DBNull" Then
            currentParam.Value = Vendor_Tolerance
        Else
            currentParam.Value = DBNull.Value
        End If
        currentParam.Type = DBParamType.Money
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Vendor_Tolerance_Amount"
        If Not Vendor_Tolerance_Amount.GetType.Name = "DBNull" Then
            currentParam.Value = Vendor_Tolerance_Amount
        Else
            currentParam.Value = DBNull.Value
        End If
        currentParam.Type = DBParamType.Decimal
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "User_ID"
        currentParam.Value = User_ID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        result = factory.ExecuteStoredProcedure("InsertInvoiceToleranceVendor", paramList)
        Logger.Debug("InsertInvoiceToleranceVendor Exit")
    End Function

    Public Function DeleteInvoiceToleranceVendor(ByVal Vendor_ID As Integer) As Integer
        Dim result As New ArrayList
        Logger.Debug("DeleteInvoiceToleranceVendor Entry")

        Dim currentParam As DBParam
        Dim paramList As New ArrayList

        'setup parameters for stored proc
        currentParam = New DBParam
        currentParam.Name = "Vendor_ID"
        currentParam.Value = Vendor_ID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        result = factory.ExecuteStoredProcedure("DeleteInvoiceToleranceVendor", paramList)
        Logger.Debug("DeleteInvoiceToleranceVendor Exit")
    End Function

    Public Function DeleteInvoiceToleranceStore(ByVal Store_No As Integer) As Integer
        Dim result As New ArrayList
        Logger.Debug("DeleteInvoiceToleranceStore Entry")

        Dim currentParam As DBParam
        Dim paramList As New ArrayList

        'setup parameters for stored proc
        currentParam = New DBParam
        currentParam.Name = "Store_No"
        currentParam.Value = Store_No
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        result = factory.ExecuteStoredProcedure("DeleteInvoiceToleranceStore", paramList)
        Logger.Debug("DeleteInvoiceToleranceStore Exit")
    End Function

    Public Shared Function GetResolutionCodes(ByVal sReasonCodeTypeAbbr As String, ByVal iIncludeDisabled As Integer) As DataTable
        Dim dt As DataTable = Nothing
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim currentParam As DBParam
        Dim paramList As New ArrayList

        Try
            currentParam = New DBParam
            currentParam.Name = "ReasonCodeTypeAbbr"
            currentParam.Value = sReasonCodeTypeAbbr
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "IncludeDisabled"
            currentParam.Value = iIncludeDisabled
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            dt = factory.GetStoredProcedureDataTable("ReasonCodes_GetDetailsForType", paramList)

        Catch ex As Exception
            Throw ex
        End Try

        Return dt
    End Function
End Class
