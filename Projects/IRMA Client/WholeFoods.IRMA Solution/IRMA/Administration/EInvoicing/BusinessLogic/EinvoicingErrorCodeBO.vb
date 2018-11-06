
Namespace WholeFoods.IRMA.Administration.EInvoicing.BusinessLogic
    Public Class EinvoicingErrorCodeBO
        Implements IDisposable




        Private _ErrorCodeId As Integer
        Public Property ErrorCodeId() As Integer
            Get
                Return _ErrorCodeId
            End Get
            Set(ByVal value As Integer)
                _ErrorCodeId = value
            End Set
        End Property



        Private _ErrorMessage As String
        Public Property ErrorMessage() As String
            Get
                Return _ErrorMessage
            End Get
            Set(ByVal value As String)
                _ErrorMessage = value
            End Set
        End Property


        Private _ErrorDescription As String
        Public Property ErrorDescription() As String
            Get
                Return _ErrorDescription
            End Get
            Set(ByVal value As String)
                _ErrorDescription = value
            End Set
        End Property


        Sub New()

        End Sub

        Sub New(ByVal ErrorCodeId As Integer, ByVal ErrorMessage As String, ByVal ErrorDescription As String)
            _ErrorCodeId = ErrorCodeId
            _ErrorMessage = ErrorMessage
            _ErrorDescription = ErrorDescription
        End Sub


#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class

End Namespace

