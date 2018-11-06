Option Strict Off
Imports Microsoft.Office.Interop.Excel
Imports System.Data
Public Class Excel

#Region "Privates"
    Private xlWorkSheet As Worksheet
    Private xlsFile As String
    Private Enum cell
        A
        B
        C
        D
        E
        F
        G
        H
        I
        J
        K
        L
        M
        N
        O
        P
        Q
        R
        S
        T
        U
        V
        W
        X
        Y
        Z
    End Enum
#End Region

#Region "Constructors and Initialization"

    Public Sub New(ByVal f As String)
        xlsFile = f
        xlWorkSheet = getWorkSheet(f)
    End Sub
    Public Sub New()
    End Sub

#End Region

#Region "Properties"
    Public Property file() As String
        Get
            Return xlsFile
        End Get
        Set(ByVal value As String)
            xlsFile = value
        End Set
    End Property
#End Region

#Region "Private Methods"
    Private Function getWorkSheet(ByVal f As String) As Worksheet
        Dim xlBook As Workbook
        Dim xlWorkSheet As Worksheet
        Dim xlSheet As Sheets
        xlBook = CType(GetObject(f), Workbook)
        xlSheet = xlBook.Worksheets
        xlWorkSheet = CType(xlSheet.Item(1), Worksheet)
        If xlWorkSheet.UsedRange.Rows.Count < 2 Then
            MsgBox("WorkSheet is empty !", MsgBoxStyle.Critical, xlBook.Name)
        End If
        Return xlWorkSheet
    End Function
#End Region

#Region "Public Methods"
    Public Overridable Function getDataTable() As System.Data.DataTable
        Dim dt As System.Data.DataTable = New System.Data.DataTable("ex")
        Dim row As System.Data.DataRow
        Dim i, l As Integer
        Dim names As String() = System.Enum.GetNames(GetType(cell))
        ' *** Loop through WorkShit ...err ..WORKSHEET ..***
        For i = 2 To xlWorkSheet.UsedRange.Rows.Count
            row = dt.NewRow()
            dt.Columns.Add(New DataColumn("UPC", GetType(String)))
            For l = 0 To xlWorkSheet.UsedRange.Columns.Count - 1
                row.Item(l) = xlWorkSheet.Range(names(l) & i.ToString).Value
            Next
            dt.Rows.Add(row)
        Next
        dt.AcceptChanges()
        Return dt
    End Function
    Public Overridable Function getDataTable(ByVal d As System.Data.DataTable) As System.Data.DataTable
        ' ************** Convert a WorkSheet into DataTable **********
        ' ************** for display in a Datagrid  ********************
        Dim row As System.Data.DataRow
        Dim i, l As Integer
        Dim names As String() = System.Enum.GetNames(GetType(cell))
        ' *** Loop through WorkShit ...err ..WORKSHEET ..***
        For i = 2 To xlWorkSheet.UsedRange.Rows.Count
            row = d.NewRow()
            For l = 0 To xlWorkSheet.UsedRange.Columns.Count - 1
                row.Item(l) = IIf(xlWorkSheet.Range(names(l) & i.ToString).Value Is Nothing, DBNull.Value, xlWorkSheet.Range(names(l) & i.ToString).Value)
            Next
            If Not row.Item(0) Is DBNull.Value Then
                d.Rows.Add(row)
            End If
        Next
        d.AcceptChanges()
        Return d
        ' ************************************************************
    End Function
    Public Overridable Function getDataTable(ByVal d As System.Data.DataTable, ByVal pb As ToolStripProgressBar) As System.Data.DataTable
        ' ************** Convert a WorkSheet into DataTable **********
        ' ************** for display in a Datagrid  ********************
        Dim row As System.Data.DataRow
        Dim i, l As Integer
        Dim names As String() = System.Enum.GetNames(GetType(cell))
        ' *** Loop through WorkShit ...err ..WORKSHEET ..***
        pb.Visible = True
        pb.Maximum = xlWorkSheet.UsedRange.Rows.Count
        pb.Step = 1
        For i = 2 To xlWorkSheet.UsedRange.Rows.Count
            pb.PerformStep()
            row = d.NewRow()
            For l = 0 To xlWorkSheet.UsedRange.Columns.Count - 1
                row.Item(l) = IIf(xlWorkSheet.Range(names(l) & i.ToString).Value Is Nothing, DBNull.Value, xlWorkSheet.Range(names(l) & i.ToString).Value)
            Next
            If Not row.Item(0) Is DBNull.Value Then
                d.Rows.Add(row)
            End If
        Next
        pb.Value = 0
        pb.Visible = False
        d.AcceptChanges()
        Return d
        ' ************************************************************
    End Function
    Public Overridable Function getNewWorkBook(ByVal dt As System.Data.DataTable) As Workbook
        ' ********* Convert a Datatable into a WorkSheet *******
        ' ********* to save out to an Excel File  ******
        Dim wb As Workbook = Nothing
        Return wb
        ' *******************************************************
    End Function
#End Region




End Class
