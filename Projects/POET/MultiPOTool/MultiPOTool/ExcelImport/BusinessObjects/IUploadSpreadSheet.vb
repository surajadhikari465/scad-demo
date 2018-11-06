Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

Public Interface IUploadSpreadSheet

    Function InsertSessionHistory(ByVal param As ArrayList) As Integer

    Sub InsertOrderItem(ByVal OrderItemData As DataTable)

    Sub InsertOrderHeader(ByVal OrderHeaderData As DataTable)



End Interface
