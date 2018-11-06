Imports log4net
Namespace WholeFoods.IRMA.Administration.Common.BusinessLogic
    Public Class ValidationBO
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Private _validationCode As Integer
        Private _validationCodeDesc As String
        Private _validationCodeType As Integer
        Private _validationCodeTypeDesc As String

        Public Property ValidationCode() As Integer
            Get
                Return _validationCode
            End Get
            Set(ByVal value As Integer)
                _validationCode = value
            End Set
        End Property

        Public Property ValidationCodeDesc() As String
            Get
                Return _validationCodeDesc
            End Get
            Set(ByVal value As String)
                _validationCodeDesc = value
            End Set
        End Property

        Public Property ValidationCodeType() As Integer
            Get
                Return _validationCodeType
            End Get
            Set(ByVal value As Integer)
                _validationCodeType = value
            End Set
        End Property

        Public Property ValidationCodeTypeDesc() As String
            Get
                Return _validationCodeTypeDesc
            End Get
            Set(ByVal value As String)
                _validationCodeTypeDesc = value
            End Set
        End Property

    End Class
End Namespace
