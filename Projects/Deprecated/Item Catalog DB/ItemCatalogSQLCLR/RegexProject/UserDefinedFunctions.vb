Imports System
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports Microsoft.SqlServer.Server
Imports System.Text.RegularExpressions


Partial Public Class UserDefinedFunctions
    <Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function RegExMatch(ByVal Input As String, _
    ByVal Pattern As String) As SqlBoolean
        Dim RegexObj As Regex = New Regex(Pattern)
        Return RegexObj.IsMatch(Input)
    End Function

    <Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function RegExReplace(ByVal expression As SqlString, ByVal pattern As SqlString, ByVal replace As SqlString) As SqlString
        If expression.IsNull Or pattern.IsNull Or replace.IsNull Then
            Return SqlString.Null
        End If
        Dim RegexObj As Regex = New Regex(pattern.ToString())
        Return New SqlString(RegexObj.Replace(expression.ToString(), replace.ToString()))
    End Function
End Class

