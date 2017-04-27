Imports WholeFoods.Utility
Imports System.Configuration
Namespace Administration
    <Serializable()> _
    Public Class SubStoreList
        Inherits WfmReadOnlyListBase(Of SubStoreList, SubStoreInfo)

#Region " Factory Methods "

        Public Overloads Shared Function GetList() As SubStoreList

            Return DataPortal.Fetch(Of SubStoreList)()

        End Function
        Public Overloads Shared Function GetList(ByVal SourceStore_No As Integer, ByVal SubTeam_No As Integer, ByVal PosWriterID As Integer) As SubStoreList

            Return DataPortal.Fetch(Of SubStoreList)(New FetchCriteria(SourceStore_No, SubTeam_No, PosWriterID))

        End Function

        Private Sub New()
            'Require use of factory methods

        End Sub

#End Region

#Region " Data Access "
        <Serializable()> _
        Private Class FetchCriteria
            Private mSourceStore_No As Integer
            Private mSubTeam_No As Integer
            Private mPosWriterID As Integer
            Friend Sub New()
                mSourceStore_No = -1
                mSubTeam_No = -1
                mPosWriterID = -1
            End Sub
            Friend Sub New(ByVal SourceStore_No As Integer, ByVal SubTeam_No As Integer, ByVal PosWriterID As Integer)
                mSourceStore_No = SourceStore_No
                mSubTeam_No = SubTeam_No
                mPosWriterID = PosWriterID
            End Sub
            Friend ReadOnly Property SourceStore_No() As Integer
                Get
                    Return mSourceStore_No
                End Get
            End Property
            Friend ReadOnly Property SubTeam_No() As Integer
                Get
                    Return mSubTeam_No
                End Get
            End Property
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
                        .CommandText = "Administration_GetSubStores"
                        .Parameters.AddWithValue("@SourceStore_No", Criteria.SourceStore_No)
                        .Parameters.AddWithValue("@SubTeam_No", Criteria.SubTeam_No)
                        .Parameters.AddWithValue("@POSFileWriterKey", Criteria.POSWriterID)
                        Me.IsReadOnly = False
                        Using dr As New SafeDataReader(.ExecuteReader)
                            While dr.Read()
                                Dim info As New SubStoreInfo(dr)
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

