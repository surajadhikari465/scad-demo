Imports WholeFoods.Utility
Imports System.Security.Principal

Namespace Security

    <Serializable()> _
    Public Class IrmaIdentity
        Inherits ReadOnlyBase(Of IrmaIdentity)
        Implements IIdentity
        Implements WFM.Library.Security.IWMFIdentity


#Region " Business Methods "
        Protected Overrides Function GetIdValue() As Object
            Return mName
        End Function

        Public ReadOnly Property UserID() As Integer Implements WFM.Library.Security.IWMFIdentity.UserID
            Get
                Return mUserID
            End Get
        End Property

        Public ReadOnly Property Store_Limit() As Integer
            Get
                Return mStoreLimit
            End Get
        End Property
        Public ReadOnly Property VendorLimit() As Integer
            Get
                Return mVendorLimit
            End Get
        End Property
        Public ReadOnly Property RecvLog_Store_Limit() As Integer
            Get
                Return mRecvLog_Store_Limit
            End Get
        End Property
#Region " IsInRole "
        Private mRoles As New List(Of String)

        Friend Overloads Function IsInRole(ByVal role As String) As Boolean

            'Dim MasterList As Security.Admin.RoleList = Security.Admin.RoleList.GetList

            Return mRoles.Contains(role)

        End Function

#End Region

#Region " IIdentity "

        Private mIsAuthenticated As Boolean
        Private mName As String = ""
        Private mUserID As Integer = 0
        Private mStoreLimit As Integer = 0
        Private mVendorLimit As Integer = 0
        Private mRecvLog_Store_Limit As Integer = 0

        Public ReadOnly Property AuthenticationType() As String _
        Implements System.Security.Principal.IIdentity.AuthenticationType
            Get
                Return "Csla"
            End Get
        End Property

        Public ReadOnly Property IsAuthenticated() As Boolean _
        Implements System.Security.Principal.IIdentity.IsAuthenticated
            Get
                Return mIsAuthenticated
            End Get
        End Property

        Public ReadOnly Property Name() As String _
        Implements System.Security.Principal.IIdentity.Name
            Get
                Return mName
            End Get
        End Property

#End Region

#End Region

#Region " Factory Methods "

        Friend Shared Function UnauthenticatedIdentity() As IrmaIdentity
            Return New IrmaIdentity
        End Function

        Friend Shared Function GetIdentity(ByVal username As String) As IrmaIdentity
            Return DataPortal.Fetch(Of IrmaIdentity)(New Criteria(username))
        End Function

        Private Sub New()
            ' require use of factory methods
        End Sub

#End Region

#Region " Data Access "

        <Serializable()> _
        Private Class Criteria

            Private mUsername As String

            Public ReadOnly Property Username() As String
                Get
                    Return mUsername
                End Get
            End Property

            Public Sub New(ByVal username As String)
                mUsername = username
            End Sub

        End Class

        Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)

            '20081107 Unencrypt Database string for CSLA
            Dim enc As Encryption.Encryptor = New Encryption.Encryptor()
            Dim decrypted As String = enc.Decrypt(Database.IRMAConnection.ToString)

            Using cn As New SqlConnection(decrypted)
                cn.Open()
                Using cm As SqlCommand = cn.CreateCommand
                    cm.CommandText = "ValidateLogin"
                    cm.CommandType = CommandType.StoredProcedure
                    cm.Parameters.AddWithValue("@UserName", criteria.Username)
                    Using dr As New SafeDataReader(cm.ExecuteReader())

                        If dr.Read() Then
                            If dr.GetBoolean("AccountEnabled") = False Then
                                'Throw New IRMA.Library.Security.Exceptions.AccountDisabled
                            Else
                                mName = criteria.Username
                                mUserID = dr.GetInt32("User_ID")
                                'If dr.GetBoolean("SuperUser") = True Then mRoles.Add("SuperUser")
                                'If dr.GetBoolean("Delete_Access") = True Or mRoles.Contains("SuperUser") Then mRoles.Add("Delete_Access")
                                'If dr.GetBoolean("PO_Accountant") = True Or mRoles.Contains("SuperUser") Then mRoles.Add("PO_Accountant")
                                'If dr.GetBoolean("Accountant") = True Or mRoles.Contains("PO_Accountant") Then mRoles.Add("Accountant")
                                'If dr.GetBoolean("Vendor_Administrator") = True Or mRoles.Contains("SuperUser") Then mRoles.Add("Vendor_Administrator")
                                'If dr.GetBoolean("Item_Administrator") = True Or mRoles.Contains("CentralPricing") Then mRoles.Add("Item_Administrator")
                                'If dr.GetBoolean("PriceBatchProcessor") = True Or mRoles.Contains("SuperUser") Then mRoles.Add("PriceBatchProcessor")
                                'If dr.GetBoolean("coordinator") = True Or mRoles.Contains("SuperUser") Or mRoles.Contains("PO_Accountant") Then mRoles.Add("coordinator")
                                'If dr.GetBoolean("Buyer") = True Or mRoles.Contains("coordinator") Or mRoles.Contains("SuperUser") Or mRoles.Contains("PO_Accountant") Then mRoles.Add("Buyer")
                                'If dr.GetBoolean("FacilityCreditProcessor") = True Or mRoles.Contains("SuperUser") Then mRoles.Add("FacilityCreditProcessor")
                                'If dr.GetBoolean("Distributor") = True Or mRoles.Contains("PO_Accountant") Then mRoles.Add("Distributor")
                                'If dr.GetBoolean("Lock_Administrator") = True Or mRoles.Contains("SuperUser") Then mRoles.Add("Lock_Administrator")
                                'If dr.GetBoolean("Inventory_Administrator") = True Or mRoles.Contains("SuperUser") Then mRoles.Add("Inventory_Administrator")
                                'mStoreLimit = dr.GetInt32("Telxon_Store_Limit")
                                'mVendorLimit = dr.GetInt32("Vendor_Limit")
                                'mRecvLog_Store_Limit = dr.GetInt32("RecvLog_Store_Limit")
                                'If dr.GetBoolean("Warehouse") Or mRoles.Contains("SuperUser") Then mRoles.Add("Warehouse")
                                'If dr.GetBoolean("DCFlatRateAdmin") Or mRoles.Contains("SuperUser") Then mRoles.Add("DCFlatRateMarkupAdministrator")
                                'If dr.GetBoolean("Cost_Administrator") Or mRoles.Contains("SuperUser") Then mRoles.Add("Cost_Administrator")

                                mIsAuthenticated = True
                            End If
                        Else
                            mName = ""
                            mIsAuthenticated = False
                            mRoles.Clear()
                            'Throw New IRMA.Library.Security.Exceptions.IrmaAccountNotFound
                        End If
                    End Using
                End Using
            End Using


        End Sub

#End Region
    End Class


End Namespace

