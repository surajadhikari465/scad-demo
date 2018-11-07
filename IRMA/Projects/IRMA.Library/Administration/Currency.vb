Imports WholeFoods.Utility
Imports System.Configuration
Namespace Administration
    <Serializable()> _
    Public Class Currency
        Inherits WfmBusinessBase(Of Currency)

#Region "Event Handlers"

        'Private Sub mZone_ListChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ListChangedEventArgs) Handles mZones.ListChanged
        '    OnPropertyChanged("Zones")
        'End Sub

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()
            ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, "CurrencyName")
            ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs("CurrencyName", 30))
        End Sub
        'TODO:  Zone Type should factor into this!
        Private Shared Function NoDuplicateCurrencyNames(ByVal target As Object, ByVal e As Csla.Validation.RuleArgs) As Boolean
            If Currency.Exists(DirectCast(target, Currency).CurrencyID, DirectCast(target, Currency).CurrencyName) Then
                e.Description = "Zone Name is already in use."
                Return False  'Error condition.
            Else
                Return True  'No error condition.
            End If
        End Function

#End Region

#Region " Authorization Rules "

        Protected Overrides Sub AddAuthorizationRules()

            ' add AuthorizationRules here
            'AuthorizationRules.AllowWrite("ZoneName", "Zone")
            'AuthorizationRules.AllowWrite("ZoneDesc", "Zone")
            'AuthorizationRules.AllowWrite("Description", "Zone")

        End Sub

        Public Shared Function CanAddObject() As Boolean

            'Return Csla.ApplicationContext.User.IsInRole("RecipeAdmin")
            Return True

        End Function

        Public Shared Function CanGetObject() As Boolean

            Return True

        End Function

        Public Shared Function CanDeleteObject() As Boolean

            Return True

            'Dim result As Boolean
            'If Csla.ApplicationContext.User.IsInRole("RecipeAdmin") Then
            '    result = True
            'End If
            'If Csla.ApplicationContext.User.IsInRole("RecipeAdmin") Then
            '    result = True
            'End If
            'Return result

        End Function

        Public Shared Function CanEditObject() As Boolean

            'Return Csla.ApplicationContext.User.IsInRole("ProjectManager")
            Return True

        End Function

#End Region

#Region "Business methods"

        Private mCurrencyID As Integer
        Private mCurrencyName As String = ""
        Private mCurrencyCode As String = ""
#Region "Property Variables"
        <NotUndoable()> Private mID As Integer = Nothing
        Private mTimestamp(7) As Byte
#End Region

#Region "Property Subs"

        <System.ComponentModel.Browsable(False)> _
        Public ReadOnly Property CurrencyID() As Integer
            Get
                Return mCurrencyID
            End Get
        End Property

        Public Property CurrencyCode() As String
            Get
                Return mCurrencyCode
            End Get
            Set(ByVal value As String)
                If CanWriteProperty(True) Then
                    If mCurrencyCode <> value Then
                        mCurrencyCode = value
                        PropertyHasChanged()
                    End If
                End If
            End Set
        End Property
        Public Property CurrencyName() As String
            Get
                Return mCurrencyName
            End Get
            Set(ByVal value As String)
                If CanWriteProperty(True) Then
                    If mCurrencyName <> value Then
                        mCurrencyName = value
                        PropertyHasChanged()
                    End If
                End If
            End Set
        End Property
#End Region

        '<System.ComponentModel.Browsable(True)> _
        'Public Overrides ReadOnly Property IsDirty() As Boolean
        '    Get
        '        Return MyBase.IsDirty OrElse mZones.IsDirty
        '    End Get
        'End Property

#End Region

#Region "Factory Methods"
        Private Sub New()
            MyBase.MarkAsChild()
            'must use paramaterized constructor
        End Sub

        Friend Sub New(ByVal dr As SafeDataReader)
            mCurrencyID = dr.GetInt32("CurrencyID")
            mCurrencyName = dr.GetString("CurrencyName")
            mCurrencyCode = dr.GetString("CurrencyCode")
            MyBase.MarkOld()
            MyBase.MarkAsChild()
        End Sub
        Public Shared Function NewCurrency(ByVal nCurrencyCode As String, ByVal nCurrencyName As String) As Currency
            Dim ReturnValue As New Currency
            ReturnValue.mCurrencyID = -1
            ReturnValue.mCurrencyCode = nCurrencyCode
            ReturnValue.mCurrencyName = nCurrencyName
            ReturnValue.MarkNew()
            Return ReturnValue
        End Function

#End Region

#Region "Data Access Methods"

        Private Class Criteria
            Private mCurrencyID As Integer

            Private Sub New()
                'must use paramaterized constructor
            End Sub
            Public Sub New(ByVal CurrencyID As Integer)
                mCurrencyID = CurrencyID
            End Sub
            Public ReadOnly Property CurrencyID() As Integer
                Get
                    Return mCurrencyID
                End Get
            End Property
        End Class

        Protected Overrides Function GetIdValue() As Object
            Return mCurrencyID
        End Function


        Public Overrides Function Save() As Currency

            If IsDeleted AndAlso Not CanDeleteObject() Then
                Throw New System.Security.SecurityException("User not authorized to Delete a Currency")

            ElseIf IsNew AndAlso Not CanAddObject() Then
                Throw New System.Security.SecurityException("User not authorized to Create a new Currency")

            ElseIf Not CanEditObject() Then
                Throw New System.Security.SecurityException("User not authorized to Update a Currency")
            End If

            Dim ReturnItem As Currency = MyBase.Save

            Return ReturnItem

        End Function

        Friend Sub Insert(ByVal cn As SqlConnection)

            ' if we're not dirty then don't update the database
            If Not Me.IsDirty Then Exit Sub

            Using cm As SqlCommand = cn.CreateCommand
                cm.CommandText = "InsertCurrency"
                Dim param As New SqlParameter("@CurrencyID", SqlDbType.Int)
                param.Direction = ParameterDirection.Output
                cm.Parameters.Add(param)
                DoInsertUpdate(cm)
                mCurrencyID = cm.Parameters.Item("@CurrencyID").Value
            End Using

        End Sub

        Friend Sub Update(ByVal cn As SqlConnection)

            ' if we're not dirty then don't update the database
            If Not Me.IsDirty Then Exit Sub

            Using cm As SqlCommand = cn.CreateCommand
                cm.CommandText = "UpdateCurrency"
                cm.Parameters.AddWithValue("@CurrencyID", mCurrencyID)
                DoInsertUpdate(cm)
            End Using

        End Sub

        'ALTER PROCEDURE [dbo].[InsertZone] 
        '	@Zone_Name varchar(50),	
        '	@GLMarketingExpenseAcct int,	
        '	@RegionID int,	
        '	@LastUpdateUserID Int,
        '	@NewZoneID Int output,
        '	@newLastUpdate timestamp output


        'CREATE PROCEDURE [dbo].[UpdateZone]
        '	@ZoneID int,
        '	@Zone_Name varchar(100),
        '	@GLMarketingExpenseAcct int,
        '	@RegionID int,
        '	@LastUpdate timestamp,
        '	@LastUpdateUserID Int,
        '	@newLastUpdate timestamp output

        Private Function DoInsertUpdate(ByVal cm As SqlCommand) As Integer
            cm.CommandType = CommandType.StoredProcedure
            cm.Parameters.AddWithValue("@CurrencyCode", mCurrencyCode)
            cm.Parameters.AddWithValue("@CurrencyName", mCurrencyName)
            cm.ExecuteNonQuery()
            MarkOld()
            Return 0
        End Function

        Friend Sub DeleteSelf(ByVal cn As SqlConnection)

            ' if we're not dirty then don't update the database
            If Not Me.IsDirty Then Exit Sub

            ' if we're new then don't update the database
            If Me.IsNew Then Exit Sub

            DeleteCurrency(cn, mCurrencyID)
            MarkNew()

        End Sub

        Friend Shared Sub DeleteCurrency( _
          ByVal cn As SqlConnection, ByVal id As Integer)

            Using cm As SqlCommand = cn.CreateCommand
                cm.CommandType = CommandType.StoredProcedure
                cm.CommandText = "DeleteCurrency"
                cm.Parameters.AddWithValue("@CurrencyID", id)
                cm.ExecuteNonQuery()
            End Using
        End Sub


#Region " Exists "

        Public Shared Function Exists(ByVal InventoryOrderID As Integer, ByVal OrderName As String) As Boolean

            Dim result As ExistsCommand
            result = DataPortal.Execute(Of ExistsCommand)(New ExistsCommand(InventoryOrderID, OrderName))
            Return result.Exists

        End Function

        <Serializable()> _
        Private Class ExistsCommand
            Inherits CommandBase

            Private mInventoryOrderID As Integer
            Private mOrderName As String
            Private mExists As Boolean

            Public ReadOnly Property Exists() As Boolean
                Get
                    Return mExists
                End Get
            End Property

            Public Sub New(ByVal InventoryOrderID As Integer, ByVal OrderName As String)
                mInventoryOrderID = InventoryOrderID
                mOrderName = OrderName
            End Sub

        End Class

#End Region

#End Region


    End Class
End Namespace

