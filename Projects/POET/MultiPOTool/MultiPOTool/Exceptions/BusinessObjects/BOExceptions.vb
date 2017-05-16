Imports Microsoft.VisualBasic
Imports System.Web.UI.Page
Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports WholeFoods.Utility.SMTP
Imports System.Net


Public Class BOExceptions

    Public Function GetExceptionsByUserID(ByVal UserID As Integer) As DataSet

        Dim mu As New DAOExceptions

        Return mu.GetExceptionsByUserID(UserID)

    End Function
    Public Function HasSpecialCharecter(ByVal str As String) As Boolean

        Dim r As Regex = New Regex("^[A-Za-z0-9 ]+$")
        Dim m As Match = r.Match(str)
        If (m.Success) Then
            Return False
        End If
        Return True
    End Function
    Public Function IsNotLetters(ByVal str As String) As Boolean

        Dim r As Regex = New Regex("^[A-Za-z]+$")
        Dim m As Match = r.Match(str)
        If (m.Success) Then
            Return False
        End If
        Return True
    End Function
    Public Function GetSessionsWithExceptionsByUserID(ByVal UserID As Integer) As DataSet

        Dim mu As New DAOExceptions

        Return mu.GetSessionsWithExceptionsByUserID(UserID)

    End Function

    Public Function GetExceptionsByUploadSession(ByVal UploadSessionHistoryID As Integer) As DataSet

        Dim mu As New DAOExceptions
        Dim ds As New DataSet

        Dim dt1 As New DataTable("ExceptionHeaders")
        Dim dt2 As New DataTable("ExceptionItems")

        Try
            dt1 = mu.GetExceptionHeadersByUploadSession(UploadSessionHistoryID)
            dt2 = mu.GetExceptionItemsByUploadSession(UploadSessionHistoryID)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try

        ds.Tables.Add(dt1)
        ds.Tables.Add(dt2)

        ' *** Create Relation *****
        Dim DC1() As DataColumn
        Dim DC2() As DataColumn
        DC1 = New DataColumn() {ds.Tables(0).Columns("Identifier"), ds.Tables(0).Columns("ItemBrand"), ds.Tables(0).Columns("ItemDescription"), ds.Tables(0).Columns("Subteam"), ds.Tables(0).Columns("VendorName"), ds.Tables(0).Columns("RegionName")}
        DC2 = New DataColumn() {ds.Tables(1).Columns("Identifier"), ds.Tables(1).Columns("ItemBrand"), ds.Tables(1).Columns("ItemDescription"), ds.Tables(1).Columns("Subteam"), ds.Tables(1).Columns("VendorName"), ds.Tables(1).Columns("RegionName")}
        Dim ExRelations As New DataRelation("Exceptions", DC1, DC2)
        ds.Relations.Add(ExRelations)

        Return ds

    End Function
    Public Sub SendErrorEmail(ByVal strPageName As String, ByVal strFunction As String, ByVal strError As String)
        Dim environment As String = ConfigurationManager.AppSettings("environment")
        Dim msgSubject As String = environment & " - Error Occured in POET Website"

        Dim strBody As String
        Dim intCtr As Integer
        Dim coll As System.Collections.Specialized.NameValueCollection


        Dim msgTo As String = ConfigurationManager.AppSettings("Developer-ErrorEmail")
        Dim msgCC As String = ConfigurationManager.AppSettings("Developer-ErrorEmail-CC")

        Const msgFrom As String = "POET@wholefoods.com"
        Const host As String = "smtp.wholefoods.com"

        Dim smtp As New SMTP(host)
        Try

         
            strBody = "<b><u>ERROR INFORMATION</u></b><br />\n"

            strBody += "<b>Error on page: </b>" & strPageName & "<br />\n"
            strBody += "<b>Function: </b>" & strFunction & "<br />"
            strBody += "<b>Error Description: </b>" & strError & "<br />\n"
            strBody += "<b>Date & Time: </b>" & Now & "<br /><br />\n" & vbCrLf

            strBody += "<b><u>SERVER INFORMATION</u></b><br />\n"
            strBody += "Server: " & Dns.GetHostName()
            strBody += "Version: " & BOPONumbers.GetVersion.Tables(0).Rows(0)("version")
            'coll = System.Web.HttpContext.Current.Request.ServerVariables
            'For intCtr = 0 To coll.Keys.Count - 1
            '    strBody += coll.Keys(intCtr) & ": " & coll.Item(intCtr) & "<br />\n"
            'Next

            strBody += "<br /><br />Good Luck the Web Development Team"
            strBody += "</font></td></tr></table></body></html>"


            smtp.send(strBody, msgTo, msgCC, msgFrom, msgSubject)

        Catch ex As Exception
            Debug.WriteLine(ex.Message)

        End Try


    End Sub
End Class

