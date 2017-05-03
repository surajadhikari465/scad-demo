Imports System.Web
Imports System.Web.Services
Imports System.Collections.Generic

<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<System.Web.Script.Services.ScriptService()> _
Public Class AutoComplete
    Inherits System.Web.Services.WebService

    Public Sub AutoComplete()

    End Sub

    <WebMethod(), System.Web.Script.Services.ScriptMethod()> _
    Public Function GetBrandCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Dim subTeamID As Integer = 0

        Integer.TryParse(contextKey, subTeamID)

        Return ItemSearch.GetBrandNamesByStartsWith(prefixText, subTeamID)
    End Function

    <WebMethod(), System.Web.Script.Services.ScriptMethod()> _
    Public Function GetVendorCompletionList(ByVal prefixText As String, ByVal count As Integer) As String()
        Return ItemSearch.GetVendorNamesByStartsWith(prefixText)
    End Function

    <WebMethod(), System.Web.Script.Services.ScriptMethod()> _
    Public Function GetTaxClassCompletionList(ByVal prefixText As String, ByVal count As Integer) As String()
        Return ItemSearch.GetTaxClassNamesByStartsWith(prefixText)
    End Function
End Class
