Option Strict Off
Option Explicit On

Imports System.Diagnostics
Imports System.Xml.Serialization
Imports System
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility

<System.Web.Services.WebServiceBindingAttribute(Name:="urn:item_master", [Namespace]:="http://dvotst7/dvo-tst/servlet/rpcrouter")> _
Public Class ElectronicOrderWebService
    Inherits System.Web.Services.Protocols.SoapHttpClientProtocol

    Public Shared SoapResponseString As String
    Public Shared ErrorCode As String
    Public Shared ErrorDescription As String
    Public Shared DVOPurchaseOrder As String
    Public Shared ReturnString As String

    Public Sub New()
        Me.Url = ConfigurationServices.AppSettings("ElectronicOrderingURL")
    End Sub

    <SoapDisplayExtension(), _
System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace:="urn:item_master", ResponseNamespace:="urn:item_master", Use:=System.Web.Services.Description.SoapBindingUse.Encoded, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)> _
    Public Function saveItemData(ByVal sPOData As String) As String
        Dim results() As Object = Me.Invoke("saveItemData", New Object() {sPOData})
        Dim resultString As String = ElectronicOrderWebService.ErrorCode & "|" & ElectronicOrderWebService.ErrorDescription & "|" & ElectronicOrderWebService.DVOPurchaseOrder
        Return resultString
    End Function

    <SoapDisplayExtension(), _
System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace:="urn:item_master", ResponseNamespace:="urn:item_master", Use:=System.Web.Services.Description.SoapBindingUse.Encoded, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)> _
    Public Function isCreditVendor(ByVal vendor As String) As String
        Dim results() As Object = Me.Invoke("isCreditVendor", New Object() {vendor})
        Dim resultString As String = ElectronicOrderWebService.ReturnString
        Return resultString
    End Function
End Class