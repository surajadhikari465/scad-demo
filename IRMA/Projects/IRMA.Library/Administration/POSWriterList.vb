Imports WholeFoods.Utility
Imports System.Configuration
Namespace Administration
    <Serializable()> _
    Public Class POSWriterList
        Inherits WfmReadOnlyListBase(Of POSWriterList, POSWriterInfo)

#Region " Factory Methods "

        Public Overloads Shared Function GetList() As POSWriterList

            Return DataPortal.Fetch(Of POSWriterList)()

        End Function

        Private Sub New()
            'Require use of factory methods

        End Sub

#End Region

#Region " Data Access "
        <Serializable()> _
        Private Class Criteria
            Private Sub New()
                'must use paramaterized contstructor
            End Sub
        End Class

        Private Overloads Sub DataPortal_Fetch()
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
                        .CommandText = "Administration_POSPush_GetPOSWriters"
                        .Parameters.AddWithValue("@FileWriterType", "POS")
                        .Parameters.AddWithValue("@ScaleWriterTypeDesc1", "")
                        Me.IsReadOnly = False
                        Using dr As New SafeDataReader(.ExecuteReader)
                            While dr.Read()
                                Dim info As New POSWriterInfo(dr)
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

