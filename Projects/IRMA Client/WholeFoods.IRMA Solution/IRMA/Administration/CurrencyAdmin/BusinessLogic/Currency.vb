Imports CurrencyDAO
Public Class Currency
#Region "Properties"
    Public Property Code As String
    Public Property Description As String
    Public Property ID As Int16
#End Region

#Region "Subs and Functions"

    Public Sub New()
        Code = Nothing
        Description = Nothing
        ID = 0
    End Sub

    Public Sub NewCurrency()
        Try
            AddNewCurrency(Me)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function GetList() As DataTable
        Try
            Return GetCurrencyList()
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub Remove()
        Try
            DeleteCurrency(Me)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub Update()
        Try
            UpdateCurrency(Me)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region
End Class
