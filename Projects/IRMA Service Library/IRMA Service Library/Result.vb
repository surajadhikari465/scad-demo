Namespace IRMA
    <DataContract()>
    Public Class Result

        <DataMember()>
        Public Property IRMA_PONumber As Integer
        <DataMember()>
        Public Property Status As Boolean
        <DataMember()>
        Public Property FunctionName As String
        <DataMember()>
        Public Property ErrorMessage As String
        <DataMember()>
        Public Property Exception As String
        <DataMember()>
        Public Property SQLString As String
        <DataMember()>
        Public Property DVO_PONumber As Integer
        <DataMember()>
        Public Property Flag As Boolean ' Used as another generic status flag (e.g. Is order suspended?)
        <DataMember()>
        Public Property Count As Integer
        <DataMember()>
        Public Property ErrorCode As Integer

        Sub New()

        End Sub

        ' TODO: If more parameters are needed for the Load method, we should overload it instead of using optional parameters

        Public Sub Load(ByVal IRMA_PONumber As Integer, ByVal Status As Boolean, ByVal FunctionName As String, Optional ErrorMessage As String = Nothing, _
                        Optional Exception As String = Nothing, Optional SQLString As String = Nothing, Optional DVO_PONumber As String = Nothing)
            Me.IRMA_PONumber = IRMA_PONumber
            Me.Status = Status
            Me.FunctionName = FunctionName
            If Not ErrorMessage Is Nothing Then
                Me.ErrorMessage = ErrorMessage
            End If
            If Not Exception Is Nothing Then
                Me.Exception = Exception
            End If
            If Not FunctionName Is Nothing Then
                Me.FunctionName = FunctionName
            End If
            If Not SQLString Is Nothing Then
                Me.SQLString = SQLString
            End If
            If Not DVO_PONumber Is Nothing Then
                Me.DVO_PONumber = DVO_PONumber
            End If
        End Sub

    End Class
End Namespace
