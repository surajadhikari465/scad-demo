Namespace Administration
    <Serializable()> _
    Public Class TaxJurisdiction
        Inherits WfmBusinessBase(Of TaxJurisdiction)

#Region "Event Handlers"

        'Private Sub mTaxJurisdiction_ListChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ListChangedEventArgs) Handles mTaxJurisdictions.ListChanged
        '    OnPropertyChanged("TaxJurisdictions")
        'End Sub

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()
            ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, "TaxJurisdictionName")
            ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs("TaxJurisdictionName", 30))
        End Sub
        'TODO:  TaxJurisdiction Type should factor into this!
        Private Shared Function NoDuplicateTaxJurisdictionNames(ByVal target As Object, ByVal e As Csla.Validation.RuleArgs) As Boolean
            If TaxJurisdiction.Exists(DirectCast(target, TaxJurisdiction).TaxJurisdictionID, DirectCast(target, TaxJurisdiction).TaxJurisdictionName) Then
                e.Description = "TaxJurisdiction Name is already in use."
                Return False  'Error condition.
            Else
                Return True  'No error condition.
            End If
        End Function

#End Region

#Region " Authorization Rules "

        Protected Overrides Sub AddAuthorizationRules()

            ' add AuthorizationRules here
            'AuthorizationRules.AllowWrite("TaxJurisdictionName", "TaxJurisdiction")
            'AuthorizationRules.AllowWrite("TaxJurisdictionDesc", "TaxJurisdiction")
            'AuthorizationRules.AllowWrite("Description", "TaxJurisdiction")

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

        Private mTaxJurisdictionID As Integer
        Private mTaxJurisdictionName As String = ""
        Private mTaxJurisdictionStoreCount As Integer
        Private mTaxJurisdictionClonedFromID As Integer = 0
        Private mTaxJurisdictionLastUpdate As Date = Nothing

#Region "Property Subs"

        <System.ComponentModel.Browsable(False)> _
        Public ReadOnly Property TaxJurisdictionID() As Integer
            Get
                Return mTaxJurisdictionID
            End Get
        End Property
        <System.ComponentModel.Browsable(False)> _
        Public ReadOnly Property TaxJurisdictionStoreCount() As Integer
            Get
                Return mTaxJurisdictionStoreCount
            End Get
        End Property

        Public Property TaxJurisdictionName() As String
            Get
                Return mTaxJurisdictionName
            End Get
            Set(ByVal value As String)
                If CanWriteProperty(True) Then
                    If mTaxJurisdictionName <> value Then
                        mTaxJurisdictionName = value
                        PropertyHasChanged()
                    End If
                End If
            End Set

        End Property

#End Region

        '<System.ComponentModel.Browsable(True)> _
        'Public Overrides ReadOnly Property IsDirty() As Boolean
        '    Get
        '        Return MyBase.IsDirty OrElse mTaxJurisdictions.IsDirty
        '    End Get
        'End Property

#End Region

#Region "Factory Methods"
        Private Sub New()
            MyBase.MarkAsChild()
            'must use paramaterized constructor
        End Sub

        Friend Sub New(ByVal dr As SafeDataReader)
            mTaxJurisdictionID = dr.GetInt32("TaxJurisdictionID")
            mTaxJurisdictionStoreCount = dr.GetInt32("StoreCount")
            mTaxJurisdictionName = dr.GetString("TaxJurisdictionDesc")
            mTaxJurisdictionLastUpdate = dr.GetDateTime("LastUpdate")
            MyBase.MarkOld()
            MyBase.MarkAsChild()
        End Sub
        Public Shared Function NewTaxJurisdiction(ByVal nTaxJurisdictionID As Integer, ByVal nTaxJurisdictionName As String) As TaxJurisdiction
            Dim ReturnValue As New TaxJurisdiction
            ReturnValue.mTaxJurisdictionID = -1
            ReturnValue.mTaxJurisdictionStoreCount = 0
            ReturnValue.mTaxJurisdictionName = nTaxJurisdictionName
            ReturnValue.mTaxJurisdictionClonedFromID = nTaxJurisdictionID
            ReturnValue.MarkNew()
            Return ReturnValue
        End Function

#End Region

#Region "Data Access Methods"

        Private Class Criteria
            Private mTaxJurisdictionID As Integer

            Private Sub New()
                'must use paramaterized constructor
            End Sub
            Public Sub New(ByVal TaxJurisdictionID As Integer)
                mTaxJurisdictionID = TaxJurisdictionID
            End Sub
            Public ReadOnly Property TaxJurisdictionID() As Integer
                Get
                    Return mTaxJurisdictionID
                End Get
            End Property
        End Class

        Protected Overrides Function GetIdValue() As Object
            Return mTaxJurisdictionID
        End Function


        Public Overrides Function Save() As TaxJurisdiction

            If IsDeleted AndAlso Not CanDeleteObject() Then
                Throw New System.Security.SecurityException("User not authorized to Delete a TaxJurisdiction")

            ElseIf IsNew AndAlso Not CanAddObject() Then
                Throw New System.Security.SecurityException("User not authorized to Create a new TaxJurisdiction")

            ElseIf Not CanEditObject() Then
                Throw New System.Security.SecurityException("User not authorized to Update a TaxJurisdiction")
            End If

            Dim ReturnItem As TaxJurisdiction = MyBase.Save

            Return ReturnItem

        End Function

        Friend Sub Insert(ByVal cn As SqlConnection)

            ' if we're not dirty then don't update the database
            If Not Me.IsDirty Then Exit Sub

            Using cm As SqlCommand = cn.CreateCommand
                cm.CommandText = "InsertCloneTaxJurisdiction"
                cm.Parameters.AddWithValue("@OldTaxJurisdictionID", mTaxJurisdictionClonedFromID)
                Dim param As New SqlParameter("@NewTaxJurisdictionID", SqlDbType.Int)
                param.Direction = ParameterDirection.Output
                cm.Parameters.Add(param)
                mTaxJurisdictionLastUpdate = DoInsertUpdate(cm)
                mTaxJurisdictionID = cm.Parameters.Item("@NewTaxJurisdictionID").Value
            End Using

        End Sub

        Friend Sub Update(ByVal cn As SqlConnection)

            ' if we're not dirty then don't update the database
            If Not Me.IsDirty Then Exit Sub

            Using cm As SqlCommand = cn.CreateCommand
                cm.CommandText = "TaxHosting_UpdateTaxJurisdiction"
                cm.Parameters.AddWithValue("@TaxJurisdictionID", mTaxJurisdictionID)
                cm.Parameters.AddWithValue("@lastUpdate", mTaxJurisdictionLastUpdate)
                mTaxJurisdictionLastUpdate = DoInsertUpdate(cm)
            End Using

        End Sub

        Private Function DoInsertUpdate(ByVal cm As SqlCommand) As Date
            cm.CommandType = CommandType.StoredProcedure
            cm.Parameters.AddWithValue("@TaxJurisdictionDesc", mTaxJurisdictionName)
            cm.Parameters.AddWithValue("@LastUpdateUserID", CType(Csla.ApplicationContext.User.Identity, IRMA.Library.Security.IrmaIdentity).UserID)
            Dim param As New SqlParameter("@newLastUpdate", SqlDbType.DateTime)
            param.Direction = ParameterDirection.Output
            cm.Parameters.Add(param)
            cm.ExecuteNonQuery()
            MarkOld()
            Return cm.Parameters("@newLastUpdate").Value
        End Function

        Friend Sub DeleteSelf(ByVal cn As SqlConnection)

            ' if we're not dirty then don't update the database
            If Not Me.IsDirty Then Exit Sub

            ' if we're new then don't update the database
            If Me.IsNew Then Exit Sub

            DeleteTaxJurisdiction(cn, mTaxJurisdictionID)
            MarkNew()

        End Sub

        Friend Shared Sub DeleteTaxJurisdiction( _
          ByVal cn As SqlConnection, ByVal id As Integer)

            Using cm As SqlCommand = cn.CreateCommand
                cm.CommandType = CommandType.StoredProcedure
                cm.CommandText = "TaxHosting_DeleteTaxJurisdiction"
                cm.Parameters.AddWithValue("@TaxJurisdictionID", id)
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

