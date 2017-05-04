Imports log4net
Imports System.Net.Mail
Imports WholeFoods.ServiceLibrary.DataAccess
Imports WholeFoods.ServiceLibrary.WholeFoods.IRMA.Replenishment.EInvoicing.BusinessLogic

Namespace WholeFoods.IRMA.Replenishment.EInvoicing.DataAccess
    Public Class EInvoicingDAO
        Implements IDisposable

        Private logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Function ValidateDataElements(ByVal InvoiceId As Integer) As DataTable

            '######################################################################################
            ' Check the Elements loaded from the XML against the config table. 
            ' All Elements that do not exist in the config table will be returned in the datatable.
            ' They will also be inserted into the Config table and flagged as 'NeedsConfig'
            '######################################################################################


            Dim dt As DataTable
            Dim outputParameters As ArrayList = Nothing
            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam = Nothing

            currentParam = New DBParam
            currentParam.Name = "InvoiceID"
            currentParam.Value = InvoiceId
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)
            dt = factory.GetStoredProcedureDataTable("EInvoicing_ValidateDataElements", paramList)

            Return dt

        End Function

        Public Sub ClearEInvoicedata(ByVal InvoiceId As Nullable(Of Integer))
            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim outputList As New ArrayList
            Dim currentParam As DBParam = Nothing

            currentParam = New DBParam
            currentParam.Name = "InvoiceID"
            currentParam.Value = InvoiceId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)
            factory.ExecuteStoredProcedure("EInvoicing_ClearEInvoiceData", paramList)
        End Sub

        Public Function getVendors() As DataTable
            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)

            Return factory.GetStoredProcedureDataTable("getVendors")
        End Function

        Public Function getEInoviceXML(ByVal eInvoiceId As Integer) As String
            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim dt As DataTable


            Dim currentParam As DBParam = Nothing

            currentParam = New DBParam
            currentParam.Name = "EInvoiceId"
            currentParam.Value = eInvoiceId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            dt = factory.GetStoredProcedureDataTable("EInvoicing_GetXML", paramList)

            Return dt.Rows(0)("InvoiceXML").ToString()

        End Function


        Public Sub ArchiveEInvoice(ByVal EinvoiceId As Integer, ByVal ArchiveType As Integer)
            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim outputList As New ArrayList
            Dim currentParam As DBParam = Nothing

            currentParam = New DBParam
            currentParam.Name = "EInvoiceId"
            currentParam.Value = EinvoiceId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ArchiveType"
            currentParam.Value = ArchiveType
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("EInvoicing_ArchiveEInvoice", paramList)



        End Sub

        Public Sub UpdateInvoicePOInformation(ByVal EInvoiceId As Integer, ByVal NewPO As String, ByVal CurrentUser As Integer)
            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim outputList As New ArrayList
            Dim currentParam As DBParam = Nothing

            currentParam = New DBParam
            currentParam.Name = "EInvoiceId"
            currentParam.Value = EInvoiceId
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "NewPO"
            currentParam.Value = NewPO
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)


            currentParam = New DBParam
            currentParam.Name = "CurrentUser"
            currentParam.Value = CurrentUser
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("EInvoicing_UpdateInvoicePOInformation", paramList)
        End Sub

        Public Function MatchInvoiceToPO(ByVal InvoiceId As Integer, ByVal UpdateIRMAInvoiceData As Boolean) As Boolean


            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim outputList As New ArrayList
            Dim currentParam As DBParam = Nothing

            currentParam = New DBParam
            currentParam.Name = "InvoiceID"
            currentParam.Value = InvoiceId
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "@UpdateIRMAInvoiceData"
            currentParam.Value = IIf(UpdateIRMAInvoiceData, 1, 0)
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)


            currentParam = New DBParam
            currentParam.Name = "ReturnValue"
            currentParam.Value = Nothing
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            outputList = factory.ExecuteStoredProcedure("EInvoicing_MatchEInvoiceToPurchaseOrder", paramList)

            Return CType(outputList(0), Boolean)

        End Function

        'Public Sub SaveNotificationConfigValue(ByVal _address As String)
        '    Dim factory As New DataFactory(DataFactory.ItemCatalog)

        '    factory.Configuration_SetValue("EInvoicing", "NotificationAddress", _address)

        'End Sub

        'Public Function GetNotificationConfigValue() As String
        '    Dim factory As New DataFactory(DataFactory.ItemCatalog)


        '    Return factory.Configuration_GetValue("EInvoicing", "NotificationAddress")


        'End Function

        'Public Overridable Function getEInvoices() As DataTable
        '    Dim factory As New DataFactory(DataFactory.ItemCatalog)
        '    Dim paramList As New ArrayList
        '    Dim currentParam As DBParam = Nothing

        '    currentParam = New DBParam
        '    currentParam.Name = "Filter"
        '    currentParam.Value = String.Empty
        '    currentParam.Type = DBParamType.String
        '    paramList.Add(currentParam)

        '    Return factory.GetStoredProcedureDataTable("EInvoicing_getEInvoices", paramList)

        'End Function

        'Public Function getEInvoices(ByVal filter As String, ByVal bShowArchived As Boolean) As DataTable
        '    Dim factory As New DataFactory(DataFactory.ItemCatalog)
        '    Dim paramList As New ArrayList
        '    Dim currentParam As DBParam = Nothing

        '    If filter = "All" Then filter = String.Empty

        '    currentParam = New DBParam
        '    currentParam.Name = "Filter"
        '    currentParam.Value = filter
        '    currentParam.Type = DBParamType.String
        '    paramList.Add(currentParam)

        '    currentParam = New DBParam
        '    currentParam.Name = "ShowArchived"
        '    currentParam.Value = IIf(bShowArchived, 1, 0)
        '    currentParam.Type = DBParamType.Bit
        '    paramList.Add(currentParam)

        '    Return factory.GetStoredProcedureDataTable("EInvoicing_getEInvoices", paramList)
        'End Function

        Public Function getEInvoices(ByVal searchData As EinvoicingSearchBO) As DataTable
            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam = Nothing

            currentParam = New DBParam
            currentParam.Name = "ImportStartDate"
            currentParam.Value = IIf(searchData.ImportStartDate Is Nothing, DBNull.Value, searchData.ImportStartDate)
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ImportEndDate"
            currentParam.Value = IIf(searchData.ImportEndDate Is Nothing, DBNull.Value, searchData.ImportEndDate)
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)


            currentParam = New DBParam
            currentParam.Name = "InvoiceStartDate"
            currentParam.Value = IIf(searchData.InvoiceStartDate Is Nothing, DBNull.Value, searchData.InvoiceStartDate)
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)


            currentParam = New DBParam
            currentParam.Name = "InvoiceEndDate"
            currentParam.Value = IIf(searchData.InvoiceEndDate Is Nothing, DBNull.Value, searchData.InvoiceEndDate)
            currentParam.Type = DBParamType.DateTime
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PONum"
            currentParam.Value = IIf(String.IsNullOrEmpty(searchData.PONumber), DBNull.Value, searchData.PONumber)
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "InvoiceNum"
            currentParam.Value = IIf(String.IsNullOrEmpty(searchData.InvoiceNumber), DBNull.Value, searchData.InvoiceNumber)
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PSVendorID"
            currentParam.Value = IIf(String.IsNullOrEmpty(searchData.PSVendorId), DBNull.Value, searchData.PSVendorId)
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "BusinessUnit"
            currentParam.Value = IIf(String.IsNullOrEmpty(searchData.BusinessUnit), DBNull.Value, searchData.BusinessUnit)
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ErrorCodeId"
            currentParam.Value = IIf(searchData.ErrorCodeId Is Nothing, DBNull.Value, searchData.ErrorCodeId)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Status"
            currentParam.Value = IIf(String.IsNullOrEmpty(searchData.Status), DBNull.Value, searchData.Status)
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Archived"
            currentParam.Value = IIf(searchData.Archived Is Nothing, DBNull.Value, searchData.Archived)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            Return factory.GetStoredProcedureDataTable("EInvoicing_getEInvoices", paramList)
        End Function

        Public Function getPOsByPSVendor(ByVal originalPO As String, ByVal NewPurchaseOrder As String, ByVal PSVendorId As String) As DataTable

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam = Nothing



            currentParam = New DBParam
            currentParam.Name = "OriginalPO"
            currentParam.Value = originalPO
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "NewPO"
            currentParam.Value = NewPurchaseOrder
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)


            currentParam = New DBParam
            currentParam.Name = "PSVendorId"
            currentParam.Value = PSVendorId
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)


            Return factory.GetStoredProcedureDataTable("EInvoicing_getPOsByPSVendor", paramList)


        End Function


        Public Function CheckForError(ByVal InvoiceId As String) As DataTable
            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam = Nothing

            currentParam = New DBParam
            currentParam.Name = "InvoiceId"
            currentParam.Value = CInt(InvoiceId)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)


            Return factory.GetStoredProcedureDataTable("EInvoicing_CheckForError", paramList)
        End Function

        Public Function GetKnownElements() As DataTable
            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Return factory.GetStoredProcedureDataTable("EInvoicing_GetKnownElements")
        End Function


        Public Function GetKnownHeaderElements() As DataTable
            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Return factory.GetStoredProcedureDataTable("EInvoicing_GetKnownHeaderElements")
        End Function

        Public Function GetKnownItemElements() As DataTable
            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Return factory.GetStoredProcedureDataTable("EInvoicing_GetKnownItemElements")
        End Function

        Public Function GetKnownSACCodes() As DataTable
            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Return factory.GetStoredProcedureDataTable("EInvoicing_GetKnownSACCodes")
        End Function

        Public Function GetErrorHistory(ByVal EInvoiceId As Integer) As DataTable
            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam = Nothing

            currentParam = New DBParam
            currentParam.Name = "EInvoiceId"
            currentParam.Value = EInvoiceId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            Return factory.GetStoredProcedureDataTable("EInvoicing_GetErrorHistory", paramList)


        End Function

        Public Sub InsertErrorHistory()
            Dim msg As String = EInvoicing_CurrentInvoice.GenerateEmail()
            'Dim msg As String = Nothing
            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam = Nothing
            Dim EInvoiceId As Integer
            Dim sErrorInformation As String
            If Not EInvoicing_CurrentInvoice.EInvoicingId Is Nothing Then


                EInvoiceId = Integer.Parse(EInvoicing_CurrentInvoice.EInvoicingId)
                sErrorInformation = msg

                currentParam = New DBParam
                currentParam.Name = "EInvoiceId"
                currentParam.Value = EInvoiceId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ErrorInformation"
                currentParam.Value = sErrorInformation
                currentParam.Type = DBParamType.String

                paramList.Add(currentParam)

                factory.ExecuteStoredProcedure("EInvoicing_InsertErrorHistory", paramList)
            End If

        End Sub

        Public Sub SetInvoiceStatus(ByVal InvoiceId As Integer, ByVal ErrorCode As Integer, ByVal Status As String)

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim paramList As New ArrayList
            Dim outputList As New ArrayList
            Dim currentParam As DBParam = Nothing

            currentParam = New DBParam
            currentParam.Name = "InvoiceID"
            currentParam.Value = InvoiceId
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ErrorCode"
            currentParam.Value = ErrorCode
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)


            currentParam = New DBParam
            currentParam.Name = "Status"
            currentParam.Value = Status
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)


            factory.ExecuteStoredProcedure("EInvoicing_SetInvoiceStatus", paramList)
        End Sub


        Public Function getAllocatedCharges() As DataTable
            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Return factory.GetStoredProcedureDataTable("EInvoicing_GetAllocatedCharges")
        End Function



        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: free managed resources when explicitly called
                End If

                ' TODO: free shared unmanaged resources
            End If
            Me.disposedValue = True
        End Sub

#Region " IDisposable Support "
        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class
End Namespace