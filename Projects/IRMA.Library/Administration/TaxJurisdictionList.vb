Imports WholeFoods.Utility
Imports System.Configuration
Namespace Administration
    <Serializable()> _
    Public Class TaxJurisdictionList
        Inherits WfmReadOnlyListBase(Of TaxJurisdictionList, TaxJurisdictionInfo)

#Region " Factory Methods "

        Public Overloads Shared Function GetList(ByVal IncludeNoneMember As Boolean) As TaxJurisdictionList

            Return DataPortal.Fetch(Of TaxJurisdictionList)(New Criteria(IncludeNoneMember))

        End Function

        Private Sub New()
            'Require use of factory methods

        End Sub

#End Region

#Region " Data Access "
        <Serializable()> _
        Private Class Criteria
            Private mIncludeNoneMember As Boolean
            Private Sub New()
                'must use paramaterized contstructor
            End Sub
            Public Sub New(ByVal IncludeNoneMember As Boolean)
                mIncludeNoneMember = IncludeNoneMember
            End Sub
            Public ReadOnly Property IncludeNoneMember() As Boolean
                Get
                    Return mIncludeNoneMember
                End Get
            End Property
        End Class

        Private Overloads Sub DataPortal_Fetch(ByVal criteria As Criteria)
            RaiseListChangedEvents = False
            ' are connection string encrypted?
            Dim encrypted As Boolean = CType(ConfigurationManager.AppSettings("encryptedConnectionStrings"), Boolean)
            Dim connString As String
            If encrypted Then
                '20081107 Unencrypt Database string for CSLA
                Dim enc As Encryption.Encryptor = New Encryption.Encryptor()
                connString = enc.Decrypt(Database.IRMAConnection.ToString)
            Else
                connString = Database.IRMAConnection.ToString
            End If

            Using cn As New SqlConnection(connString)
                cn.Open()
                Using cm As SqlCommand = cn.CreateCommand
                    With cm
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = "TaxHosting_GetTaxJurisdictions"
                        Me.IsReadOnly = False
                        If criteria.IncludeNoneMember Then Me.Add(New TaxJurisdictionInfo)
                        Using dr As New SafeDataReader(.ExecuteReader)
                            While dr.Read()
                                Dim info As New TaxJurisdictionInfo(dr)
                                Me.Add(info)
                            End While
                        End Using
                        Me.IsReadOnly = True
                    End With

                End Using
            End Using
            RaiseListChangedEvents = True

        End Sub

#End Region

    End Class
End Namespace

