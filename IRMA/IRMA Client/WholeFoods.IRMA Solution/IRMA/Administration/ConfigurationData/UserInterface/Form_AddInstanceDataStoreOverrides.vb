Imports Infragistics.Win.UltraWinGrid
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Replenishment.Common.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Public Class Form_AddInstanceDataStoreOverrides

#Region "properties"

    Private _flagKey As String
    Private _regionalFlagValue As Boolean
    Private _deletedFlags As ArrayList
    Private _addedFlags As New ArrayList

    Public Property FlagKey() As String
        Get
            Return _flagKey
        End Get
        Set(ByVal value As String)
            _flagKey = value
        End Set
    End Property

    Public Property RegionalFlagValue() As Boolean
        Get
            Return _regionalFlagValue
        End Get
        Set(ByVal value As Boolean)
            _regionalFlagValue = value
        End Set
    End Property

    Public Property DeletedFlags() As ArrayList
        Get
            Return _deletedFlags
        End Get
        Set(ByVal value As ArrayList)
            _deletedFlags = value
        End Set
    End Property

#End Region

#Region "Events raised by this form"
    ''' <summary>
    ''' This event is raised when a child form makes a change that requires the
    ''' data on the calling form to be updated.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event UpdateCallingForm(ByVal addedStores As ArrayList)
#End Region

    Private Sub Form_AddInstanceDataStoreOverrides_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.CenterToParent()

        'load grid with data
        BindData()
    End Sub

    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click
        Me.Close()
    End Sub

    Private Sub Button_OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_OK.Click
        If ApplyChanges() Then
            Me.Close()
        End If
    End Sub

    Private Sub BindData()
        'get list of stores that have not been overridden for the given FlagKey value
        Me.UltraGrid_StoreOverrides.DataSource = InstanceDataDAO.GetAvailableStoreOverrides(_flagKey, _deletedFlags)
    End Sub

    Private Function ApplyChanges() As Boolean
        Dim success As Boolean
        Dim currentRow As UltraGridRow
        Dim instanceDAO As New InstanceDataDAO
        Dim transaction As SqlTransaction = Nothing
        Dim currentFlag As New InstanceDataFlagsBO
        Dim hasChanges As Boolean
        Dim deletedFlagsEnum As IEnumerator = Nothing
        Dim currentDeletedFlag As InstanceDataFlagsBO
        Dim isSkipAddingDeletedFlag As Boolean

        If UltraGrid_StoreOverrides.Selected.Rows.Count > 0 Then
            Try
                deletedFlagsEnum = _deletedFlags.GetEnumerator

                'create transaction to share connection for all flags to delete
                transaction = instanceDAO.GetTransaction

                'loop through all selected stores
                For Each currentRow In UltraGrid_StoreOverrides.Selected.Rows
                    isSkipAddingDeletedFlag = False

                    deletedFlagsEnum.Reset()

                    currentFlag.StoreNo = CType(currentRow.Cells("StoreNo").Value, Integer)
                    currentFlag.FlagKey = _flagKey
                    currentFlag.FlagValue = (Not _regionalFlagValue)

                    'if current flag is the deleted flags list, then it is already in the database 
                    'because it hasn't been saved to the database yet;  do not add this flag, but indicate
                    'that it has been added so it will be removed from the deleted list on the parent form
                    While deletedFlagsEnum.MoveNext
                        currentDeletedFlag = CType(deletedFlagsEnum.Current, InstanceDataFlagsBO)

                        If currentDeletedFlag.FlagKey = currentFlag.FlagKey AndAlso currentDeletedFlag.StoreNo = currentFlag.StoreNo Then
                            isSkipAddingDeletedFlag = True
                            Exit While
                        End If
                    End While

                    If Not isSkipAddingDeletedFlag Then
                        instanceDAO.InsertInstanceDataFlagsStoreOverride(currentFlag, transaction)
                    End If

                    _addedFlags.Add(currentFlag)

                    hasChanges = True
                Next

                transaction.Commit()
                success = True

                If hasChanges Then
                    'update parent form's grid
                    RaiseEvent UpdateCallingForm(_addedFlags)
                End If
            Catch ex As Exception

                transaction.Rollback()

                'inform user
                Logger.LogError("Exception: ", Me.GetType(), ex)
                'display a message to the user
                DisplayErrorMessage(ERROR_DB)
                'send message about exception
                Dim args(1) As String
                args(0) = "Form_AddInstanceDataStoreOverrides form: ApplyChanges function"
                ErrorHandler.ProcessError(WholeFoods.Utility.ErrorType.DataFactoryException, args, SeverityLevel.Warning)
            End Try
        Else
            'prompt user to select a row
            MessageBox.Show(ResourcesAdministration.GetString("msg_selectStoreToOverride"), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand)
        End If

        Return success
    End Function

End Class