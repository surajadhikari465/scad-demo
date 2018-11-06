Imports Microsoft.VisualBasic

Public Class ISSConfiguration
    Implements IDisposable




    Private _ISS_AutoUploadFlag As Boolean
    Public Property ISS_AutoUploadFlag() As Boolean
        Get
            Return _ISS_AutoUploadFlag
        End Get
        Set(ByVal value As Boolean)
            _ISS_AutoUploadFlag = value
        End Set
    End Property


    Private _ISS_WeekendProcessFlag As Boolean
    Public Property ISS_WeekendProcessFlag() As Boolean
        Get
            Return _ISS_WeekendProcessFlag
        End Get
        Set(ByVal value As Boolean)
            _ISS_WeekendProcessFlag = value
        End Set
    End Property


    Private _ISS_AmountOffLimitPercentage As Single
    Public Property ISS_AmountOffLimitPercentage() As Single
        Get
            Return _ISS_AmountOffLimitPercentage
        End Get
        Set(ByVal value As Single)
            _ISS_AmountOffLimitPercentage = value
        End Set
    End Property


    Private _ISS_DurationDays As Integer
    Public Property ISS_DurationDays() As Integer
        Get
            Return _ISS_DurationDays
        End Get
        Set(ByVal value As Integer)
            _ISS_DurationDays = value
        End Set
    End Property


    Private _ISS_MinimumMarginPercentage As Single
    Public Property ISS_MinimumMarginPercentage() As Single
        Get
            Return _ISS_MinimumMarginPercentage
        End Get
        Set(ByVal value As Single)
            _ISS_MinimumMarginPercentage = value
        End Set
    End Property


    Private _ISS_ProcessDelayDays As Integer
    Public Property ISS_ProcessDelayDays() As Integer
        Get
            Return _ISS_ProcessDelayDays
        End Get
        Set(ByVal value As Integer)
            _ISS_ProcessDelayDays = value
        End Set
    End Property


    Private _ISS_SubteamExceptions As String
    Public Property ISS_SubteamExceptions() As String
        Get
            Return _ISS_SubteamExceptions
        End Get
        Set(ByVal value As String)
            _ISS_SubteamExceptions = value
        End Set
    End Property

    Sub New()

    End Sub

    Sub New( _
        ByVal SubTeamExceptions As String, _
        ByVal ProcessDelayDays As Integer, _
        ByVal MinimumMarginPercent As Single, _
        ByVal DurationDays As Integer, _
        ByVal AmountOffPercentLimit As Single, _
        ByVal WeekendProcessingFlag As Boolean, _
        ByVal AutoUploadFlag As Boolean)

        Me.ISS_AmountOffLimitPercentage = AmountOffPercentLimit
        Me.ISS_AutoUploadFlag = AutoUploadFlag
        Me.ISS_DurationDays = DurationDays
        Me.ISS_MinimumMarginPercentage = MinimumMarginPercent
        Me.ISS_ProcessDelayDays = ProcessDelayDays
        Me.ISS_SubteamExceptions = SubTeamExceptions
        Me.ISS_WeekendProcessFlag = WeekendProcessingFlag

    End Sub



    Private disposedValue As Boolean = False        ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: free managed resources when explicitly called
            End If

            ' TODO: free shared unmanaged resources
        End If
        Me.disposedValue = True
    End Sub

#Region " IDisposable Support "
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
