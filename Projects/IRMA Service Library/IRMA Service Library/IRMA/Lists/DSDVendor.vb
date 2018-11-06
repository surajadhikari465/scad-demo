Imports WholeFoods.ServiceLibrary.DataAccess
Imports WholeFoods.ServiceLibrary.IRMA.Common

Namespace IRMA

    <DataContract()> _
    Public Class DSDVendor

        <DataMember()> _
        Private Property VendorID As Integer
        <DataMember()> _
        Private Property VendorName As String
        <DataMember()> _
        Private Property IsDSDVendor As Boolean

        Sub New()

        End Sub

        Sub New(ByRef dr As DataRow)
            Try
                If (Not IsDBNull(dr.Item("VendorID"))) Then
                    VendorID = dr.Item("VendorID")
                End If

                If (Not IsDBNull(dr.Item("VendorName"))) Then
                    VendorName = dr.Item("VendorName")
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Function IsDSDStoreVendorByUPC(ByVal UPC As String, ByVal Store_No As Integer) As Boolean

            Dim factory As New DataFactory(DataFactory.IRMAServiceLibrary)
            Dim dt As DataTable
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' ******* Parameters ************
            currentParam = New DBParam
            currentParam.Name = "UPC"
            currentParam.Value = UPC
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = Store_No
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            Try
                dt = factory.GetStoredProcedureDataTable("IsDSDStoreVendorByUPC", paramList)
                For Each dr As DataRow In dt.Rows
                    Me.IsDSDVendor = dr.Item("IsDSDVendor")
                Next
                Return Me.IsDSDVendor
            Catch ex As Exception
                Throw ex
            Finally
                connectionCleanup(factory)
            End Try

        End Function

    End Class
End Namespace