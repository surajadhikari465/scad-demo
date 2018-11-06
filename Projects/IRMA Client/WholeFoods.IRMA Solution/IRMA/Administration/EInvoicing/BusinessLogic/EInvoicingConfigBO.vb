Imports log4net
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Administration.EInvoicing.DataAccess

Namespace WholeFoods.IRMA.Administration.EInvoicing.BusinessLogic


    Public Class EInvoicingConfigBO
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


        Public Function getConfigInfo() As DataTable
            Dim DAO As EinvoicingConfigDAO = New EinvoicingConfigDAO()
            Dim dt As DataTable

            dt = DAO.getConfigInfoDataTable

            Return dt
        End Function

        Public Function getSACTypes() As DataTable
            Dim DAO As EinvoicingConfigDAO = New EinvoicingConfigDAO()
            Dim dt As DataTable

            dt = DAO.getsacTypesDataTable

            Return dt
        End Function

        Public Sub SaveChanges(ByRef msg As String, ByVal isNew As Boolean)

            Dim DAO As EinvoicingConfigDAO = New EinvoicingConfigDAO
            Dim retval As String = String.Empty

            If ValidateData(msg) Then
                If isNew Then
                    DAO.insertConfigvalue(Me)

                Else
                    DAO.UpdateConfigValue(Me)
                End If

            End If


        End Sub

        Public Function ValidateData(ByRef msg As String) As Boolean
            Dim retval As Boolean = False

            retval = True
            Return retval
        End Function


        Private _ElementName As String
        Public Property ElementName() As String
            Get
                Return _ElementName
            End Get
            Set(ByVal value As String)
                _ElementName = value
            End Set
        End Property


        Private _Label As String
        Public Property Label() As String
            Get
                Return _Label
            End Get
            Set(ByVal value As String)
                _Label = value
            End Set
        End Property



        Private _IsSAC As Boolean
        Public Property IsSAC() As Boolean
            Get
                Return _IsSAC
            End Get
            Set(ByVal value As Boolean)
                _IsSAC = value
            End Set
        End Property


        Private _SACType As Integer
        Public Property SACType() As Integer
            Get
                Return _SACType
            End Get
            Set(ByVal value As Integer)
                _SACType = value
            End Set
        End Property



        Private _IsAllowance As Boolean
        Public Property IsAllowance() As Boolean
            Get
                Return _IsAllowance
            End Get
            Set(ByVal value As Boolean)
                _IsAllowance = value
            End Set
        End Property


        Private _SubTeam_no As Integer
        Public Property SubTeam_NO() As Integer
            Get
                Return _SubTeam_no
            End Get
            Set(ByVal value As Integer)
                _SubTeam_no = value
            End Set
        End Property


        Private _IsHeaderElement As Boolean
        Public Property IsHeaderElement() As Boolean
            Get
                Return _IsHeaderElement
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    _IsItemElement = False
                End If
                _IsHeaderElement = value
            End Set
        End Property


        Private _IsItemElement As Boolean
        Public Property IsItemElement() As Boolean
            Get
                Return _IsItemElement
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    _IsHeaderElement = False
                End If
                _IsItemElement = value
            End Set
        End Property


        Private _isDisabled As Boolean
        Public Property Disabled() As Boolean
            Get
                Return _isDisabled
            End Get
            Set(ByVal value As Boolean)
                _isDisabled = value
            End Set
        End Property



    End Class


End Namespace
