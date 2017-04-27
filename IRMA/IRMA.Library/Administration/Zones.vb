Imports WholeFoods.Utility
Imports System.Configuration
Namespace Administration
    <Serializable()> _
    Public Class Zones
        Inherits WfmBusinessListBase(Of Zones, Zone)

#Region " Business Methods "

        'Public Shadows Sub Remove(ByVal item As Zone)
        '    If item.CanDelete Then
        '        MyBase.Remove(item)
        '    End If
        'End Sub

#End Region
#Region " Factory Methods "

        Public Shared Function GetList() As Zones

            Return DataPortal.Fetch(Of Zones)()

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

        Public Overrides Function Save() As Zones
            ' see if save is allowed
            'If Not CanEditObject() Then
            '    Throw New System.Security.SecurityException( _
            '      "User not authorized to save roles")
            'End If

            ' do the save
            Dim result As Zones
            result = MyBase.Save()
            Return result
        End Function
        '<Transactional(TransactionalTypes.TransactionScope)> _
        Protected Overrides Sub DataPortal_Update()
            Me.RaiseListChangedEvents = False
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
                For Each item As Zone In DeletedList
                    item.DeleteSelf(cn)
                Next
                DeletedList.Clear()

                For Each item As Zone In Me
                    If item.IsNew Then
                        item.Insert(cn)
                    Else
                        item.Update(cn)
                    End If
                Next
            End Using
            Me.RaiseListChangedEvents = True
        End Sub


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
                        .CommandText = "Administration_GetZones"

                        Using dr As New SafeDataReader(.ExecuteReader)
                            While dr.Read()
                                Dim info As New Zone(dr)
                                Me.Add(info)
                            End While
                        End Using


                    End With
                End Using
            End Using
            RaiseListChangedEvents = True

        End Sub

#End Region

    End Class
End Namespace

