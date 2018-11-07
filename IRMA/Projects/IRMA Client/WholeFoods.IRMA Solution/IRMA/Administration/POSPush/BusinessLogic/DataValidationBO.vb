Namespace WholeFoods.IRMA.Administration.POSPush.BusinessLogic
    Public Class DataValidationBO

        Public Shared Function IsIntegerString(ByVal inVal As String) As Boolean
            Dim isInt As Boolean = True
            Dim test As Integer
            Try
                test = CType(inVal, Integer)
            Catch ex As Exception
                ' if the conversion fails for any reason, this is not a valid integer
                isInt = False
            End Try
            Return isInt
        End Function
    End Class
End Namespace

