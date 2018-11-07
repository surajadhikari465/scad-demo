Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Web.UI.Page
Imports Microsoft.Office.Interop




Public Class DALSpreadsheetImport

    Private _con As String

    Sub New(ByVal connection As String)

        _con = connection

    End Sub

    ''' <summary>
    ''' READ IN EXCEL SPREADSHEET
    ''' </summary>
    Public Function GetSpreadSheet(ByVal sheetName As String) As DataSet

        Dim objCon As New OleDb.OleDbConnection()
        Dim ds As New DataSet("ExcelResults")

        objCon = Connect()

        ds = GetAllRows(objCon, sheetName)

        Close(objCon)

        Return ds

    End Function
    Private Function GetAllRows(ByRef objCon As OleDb.OleDbConnection, ByVal sheetName As String) As DataSet

        Dim results As New DataSet("ExcelRows")

        Dim com As New OleDb.OleDbCommand("select * from [" & sheetName & "$]", objCon)
        Dim da As New OleDb.OleDbDataAdapter()

        da.SelectCommand = com
        da.Fill(results)

        Return results

    End Function

    Public Function GetSheetName(ByVal fileName As String) As String

        Dim workbook As Infragistics.Excel.Workbook = Infragistics.Excel.Workbook.Load(fileName, False)
        Return workbook.Worksheets(0).Name.ToString

    End Function

    Private Function Connect() As OleDb.OleDbConnection

        Dim objCon As New OleDb.OleDbConnection(_con)
        objCon.Open()

        Return objCon

    End Function

    Private Sub Close(ByRef con As OleDb.OleDbConnection)

        con.Close()

    End Sub
End Class
