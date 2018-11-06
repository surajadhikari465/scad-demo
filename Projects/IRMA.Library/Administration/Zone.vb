Imports WholeFoods.Utility
Imports System.Configuration
Namespace Administration
    <Serializable()> _
    Public Class Zone
        Inherits WfmBusinessBase(Of Zone)

#Region "Event Handlers"

        'Private Sub mZone_ListChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ListChangedEventArgs) Handles mZones.ListChanged
        '    OnPropertyChanged("Zones")
        'End Sub

#End Region

#Region " Validation Rules "

        Protected Overrides Sub AddBusinessRules()
            ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, "ZoneName")
            ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs("ZoneName", 30))
        End Sub
        'TODO:  Zone Type should factor into this!
        Private Shared Function NoDuplicateZoneNames(ByVal target As Object, ByVal e As Csla.Validation.RuleArgs) As Boolean
            If Zone.Exists(DirectCast(target, Zone).ZoneID, DirectCast(target, Zone).ZoneName) Then
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

        Private mZoneID As Integer
        Private mZoneName As String = ""
        Private mZoneGLMarketingExpenseAcct As Integer
        Private mZoneStoreCount As Integer
        Private mZoneRegionID As Integer = 0
        Private mZoneLastUpdate(7) As Byte
#Region "Property Variables"
        <NotUndoable()> Private mID As Integer = Nothing
        Private mTimestamp(7) As Byte
#End Region

#Region "Property Subs"

        <System.ComponentModel.Browsable(False)> _
        Public ReadOnly Property ZoneID() As Integer
            Get
                Return mZoneID
            End Get
        End Property
        <System.ComponentModel.Browsable(False)> _
        Public ReadOnly Property ZoneStoreCount() As Integer
            Get
                Return mZoneStoreCount
            End Get
        End Property
        <System.ComponentModel.Browsable(False)> _
        Public Property ZoneRegionID() As Integer
            Get
                Return mZoneRegionID
            End Get
            Set(ByVal value As Integer)
                If CanWriteProperty(True) Then
                    If mZoneRegionID <> value Then
                        mZoneRegionID = value
                        PropertyHasChanged()
                    End If
                End If
            End Set

        End Property

        Public Property ZoneName() As String
            Get
                Return mZoneName
            End Get
            Set(ByVal value As String)
                If CanWriteProperty(True) Then
                    If mZoneName <> value Then
                        mZoneName = value
                        PropertyHasChanged()
                    End If
                End If
            End Set

        End Property

        Public Property ZoneGLMarketingExpenseAcct() As Integer
            Get
                Return mZoneGLMarketingExpenseAcct
            End Get
            Set(ByVal value As Integer)
                If CanWriteProperty(True) Then
                    If mZoneGLMarketingExpenseAcct <> value Then
                        mZoneGLMarketingExpenseAcct = value
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
            mZoneID = dr.GetInt32("Zone_ID")
            mZoneStoreCount = dr.GetInt32("StoreCount")
            mZoneName = dr.GetString("Zone_Name")
            mZoneRegionID = dr.GetInt32("Region_ID")
            mZoneGLMarketingExpenseAcct = dr.GetInt32("GLMarketingExpenseAcct")
            dr.GetBytes("LastUpdate", 0, mZoneLastUpdate, 0, 8)
            MyBase.MarkOld()
            MyBase.MarkAsChild()
        End Sub
        Public Shared Function NewZone(ByVal nRegionID As Integer, ByVal nZoneName As String, ByVal nZoneGLMarketingExpenseAcct As Integer) As Zone
            Dim ReturnValue As New Zone
            ReturnValue.mZoneID = -1
            ReturnValue.mZoneStoreCount = 0
            ReturnValue.mZoneName = nZoneName
            ReturnValue.mZoneGLMarketingExpenseAcct = nZoneGLMarketingExpenseAcct
            ReturnValue.mZoneRegionID = nRegionID
            ReturnValue.MarkNew()
            Return ReturnValue
        End Function

#End Region

#Region "Data Access Methods"

        Private Class Criteria
            Private mZoneID As Integer

            Private Sub New()
                'must use paramaterized constructor
            End Sub
            Public Sub New(ByVal ZoneID As Integer)
                mZoneID = ZoneID
            End Sub
            Public ReadOnly Property ZoneID() As Integer
                Get
                    Return mZoneID
                End Get
            End Property
        End Class

        Protected Overrides Function GetIdValue() As Object
            Return mZoneID
        End Function


        Public Overrides Function Save() As Zone

            If IsDeleted AndAlso Not CanDeleteObject() Then
                Throw New System.Security.SecurityException("User not authorized to Delete a Zone")

            ElseIf IsNew AndAlso Not CanAddObject() Then
                Throw New System.Security.SecurityException("User not authorized to Create a new Zone")

            ElseIf Not CanEditObject() Then
                Throw New System.Security.SecurityException("User not authorized to Update a Zone")
            End If

            Dim ReturnItem As Zone = MyBase.Save

            Return ReturnItem

        End Function

        Friend Sub Insert(ByVal cn As SqlConnection)

            ' if we're not dirty then don't update the database
            If Not Me.IsDirty Then Exit Sub

            Using cm As SqlCommand = cn.CreateCommand
                cm.CommandText = "Administration_InsertZone"
                Dim param As New SqlParameter("@NewZoneID", SqlDbType.Int)
                param.Direction = ParameterDirection.Output
                cm.Parameters.Add(param)
                mZoneLastUpdate = DoInsertUpdate(cm)
                mZoneID = cm.Parameters.Item("@NewZoneID").Value
            End Using

        End Sub

        Friend Sub Update(ByVal cn As SqlConnection)

            ' if we're not dirty then don't update the database
            If Not Me.IsDirty Then Exit Sub

            Using cm As SqlCommand = cn.CreateCommand
                cm.CommandText = "Administration_UpdateZone "
                cm.Parameters.AddWithValue("@ZoneID", mZoneID)
                cm.Parameters.AddWithValue("@lastUpdate", mZoneLastUpdate)
                mZoneLastUpdate = DoInsertUpdate(cm)
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

        Private Function DoInsertUpdate(ByVal cm As SqlCommand) As Byte()
            cm.CommandType = CommandType.StoredProcedure
            cm.Parameters.AddWithValue("@Zone_Name", mZoneName)
            cm.Parameters.AddWithValue("@GLMarketingExpenseAcct", mZoneGLMarketingExpenseAcct)
            cm.Parameters.AddWithValue("@RegionID", mZoneRegionID)
            cm.Parameters.AddWithValue("@LastUpdateUserID", CType(Csla.ApplicationContext.User.Identity, IRMA.Library.Security.IrmaIdentity).UserID)
            Dim param As New SqlParameter("@newLastUpdate", SqlDbType.Timestamp)
            param.Direction = ParameterDirection.Output
            cm.Parameters.Add(param)
            cm.ExecuteNonQuery()
            MarkOld()
            Return CType(cm.Parameters("@newLastUpdate").Value, Byte())
        End Function

        Friend Sub DeleteSelf(ByVal cn As SqlConnection)

            ' if we're not dirty then don't update the database
            If Not Me.IsDirty Then Exit Sub

            ' if we're new then don't update the database
            If Me.IsNew Then Exit Sub

            DeleteZone(cn, mZoneID)
            MarkNew()

        End Sub

        Friend Shared Sub DeleteZone( _
          ByVal cn As SqlConnection, ByVal id As Integer)

            Using cm As SqlCommand = cn.CreateCommand
                cm.CommandType = CommandType.StoredProcedure
                cm.CommandText = "Administration_DeleteZone"
                cm.Parameters.AddWithValue("@ZoneID", id)
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

