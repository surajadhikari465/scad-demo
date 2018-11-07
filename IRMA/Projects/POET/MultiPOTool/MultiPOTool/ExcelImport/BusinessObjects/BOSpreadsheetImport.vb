Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

Public Class BOSpreadsheetImport

    Private _fileName As String
    Private _sheetName As String
    Private _connection As String

    Public Sub New(ByVal FilePath As String)

        FileName = FilePath
        _connection = BuildExcelConnectionString()
        SheetName = GetSheetName()

    End Sub

    Public Property SheetName() As String
        Get
            Return _sheetName
        End Get
        Set(ByVal value As String)
            _sheetName = value
        End Set
    End Property

    Public Property FileName() As String
        Get
            Return _fileName
        End Get
        Set(ByVal value As String)
            _fileName = value
        End Set
    End Property

    Public Function GetAllExcelRows() As DataSet

        Dim ds As New DataSet("ExcelResults")
        Dim az As New DALSpreadsheetImport(_connection)

        ds = az.GetSpreadSheet(SheetName)

        '05/03/2012, Faisal Ahmed - This following block removes blank rows
        If ds.Tables.Count > 0 Then
            Dim Str As String = String.Empty
            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Str = String.Empty
                Dim lst As New List(Of Object)(ds.Tables(0).Rows(i).ItemArray)
                For Each s As Object In lst
                    Str &= s.ToString
                Next
                If String.IsNullOrEmpty(Str) Then
                    ds.Tables(0).Rows(i).Delete()
                End If
            Next

            ds.Tables(0).AcceptChanges()
        End If

        Return ds

    End Function

    Public Function GetSheetName() As String
        Dim az As New DALSpreadsheetImport(_connection)
        GetSheetName = az.GetSheetName(_fileName)
    End Function

    ''' <summary>
    ''' BUILD THE CONNECTION STRING
    ''' </summary>
    Public Function BuildExcelConnectionString() As String

        Dim con As New StringBuilder

        ' **** New Data Provider *** Excel 2007 ******
        con.Append("Provider=Microsoft.ACE.OLEDB.12.0;")
        con.Append("Data Source=")
        con.Append(_fileName)
        con.Append(";Extended Properties='Excel 12.0 Xml;IMEX=1; HDR=NO'")


        Return con.ToString
    End Function
End Class
