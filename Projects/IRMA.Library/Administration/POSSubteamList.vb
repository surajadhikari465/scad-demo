Imports WholeFoods.Utility
Imports System.Configuration
Namespace Administration
    <Serializable()> _
    Public Class POSSubteamList
        Inherits WfmReadOnlyListBase(Of POSSubteamList, POSSubteamInfo)

#Region " Factory Methods "

        Public Overloads Shared Function GetList() As POSSubteamList

            Return DataPortal.Fetch(Of POSSubteamList)(New FetchCriteria())

        End Function
        Public Overloads Shared Function GetList(ByVal PosWriterID As Integer) As POSSubteamList

            Return DataPortal.Fetch(Of POSSubteamList)(New FetchCriteria(PosWriterID))

        End Function

        Private Sub New()
            'Require use of factory methods

        End Sub

#End Region

#Region " Data Access "
        <Serializable()> _
        Private Class FetchCriteria
            Private mPosWriterID As Integer
            Friend Sub New()
                mPosWriterID = -1
            End Sub
            Friend Sub New(ByVal PosWriterID As Integer)
                mPosWriterID = PosWriterID
            End Sub
            Friend ReadOnly Property POSWriterID() As Integer
                Get
                    Return mPosWriterID
                End Get
            End Property
        End Class

        Private Class Criteria
            Private Sub New()
                'must use paramaterized contstructor
            End Sub
        End Class

        Private Overloads Sub DataPortal_Fetch(ByVal Criteria As FetchCriteria)
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
                        .CommandText = "Administration_GetPOSSubTeams"
                        .Parameters.AddWithValue("@POSFileWriterKey", Criteria.POSWriterID)
                        Me.IsReadOnly = False
                        Using dr As New SafeDataReader(.ExecuteReader)
                            While dr.Read()
                                Dim info As New POSSubteamInfo(dr)
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

