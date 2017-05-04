Imports WholeFoods.Utility
Imports System.Configuration
Imports System.ComponentModel
Namespace Administration
    <Serializable()> _
    Public Class Store
        Inherits WfmBusinessBase(Of Store)


#Region "Event Handlers"

        'Private Sub mStore_ListChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ListChangedEventArgs) Handles mStores.ListChanged
        '    OnPropertyChanged("Stores")
        'End Sub

#End Region

#Region " Validation Rules "


        Protected Overrides Sub AddBusinessRules()

            ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.MinValue(Of Integer), New Csla.Validation.CommonRules.MinValueRuleArgs(Of Integer)("StoreID", 1))
            ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs("StoreID", 5))

            ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, "StoreName")
            ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs("StoreName", 50))

            ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, "StoreAbbr")
            ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs("StoreAbbr", 5))

            ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.MinValue(Of Integer), New Csla.Validation.CommonRules.MinValueRuleArgs(Of Integer)("BusinessUnit_Id", 1))
            ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs("BusinessUnit_Id", 5))

            ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, "VendorName")
            ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs("VendorName", 50))

            ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, "VendorAddress")
            ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs("VendorAddress", 50))

            ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, "VendorCity")
            ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs("VendorCity", 30))

            ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, "VendorState")
            ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs("VendorState", 2))

            ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringRequired, "VendorZipCode")
            ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs("VendorZipCode", 5))

            ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.MinValue(Of Integer), New Csla.Validation.CommonRules.MinValueRuleArgs(Of Integer)("PeopleSoftVendorID", 1))
            ValidationRules.AddRule(AddressOf Csla.Validation.CommonRules.StringMaxLength, New Csla.Validation.CommonRules.MaxLengthRuleArgs("PeopleSoftVendorID", 10))

        End Sub


#End Region

#Region " Authorization Rules "

        Protected Overrides Sub AddAuthorizationRules()

            ' add AuthorizationRules here
            'AuthorizationRules.AllowWrite("StoreName", "Store")
            'AuthorizationRules.AllowWrite("StoreDesc", "Store")
            'AuthorizationRules.AllowWrite("Description", "Store")

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

        Private mStoreID As Integer
        Private mStoreAbbr As String                ' Store Abbreviation
        Private mStoreName As String = ""
        Private mStoreJurisdictionID As Integer         ' TaxJurisdictionID
        Private mZoneID As Integer                 ' Zone 
        Private mTaxJurisdictionID As Integer         ' TaxJurisdictionID
        Private mBusinessUnit_Id As Integer         ' Business Unit ID
        Private mStoreStoreCount As Integer
        Private mPlumStoreNo As Integer = 0
        Private mPSIStoreNo As Integer = 0
        Private mSourceStoreNo As Integer = 0
        Private mStoreLastUpdate(7) As Byte
        Private mISSPriceChgTypeID As Integer = 0
        Private mStoreSubTeamSubstitutions As String
        Private mVendorName As String               ' name for store as vendor
        Private mVendorAddress As String            ' New Store Address 
        Private mVendorCity As String               ' New Store City
        Private mVendorState As String              ' New Store State
        Private mVendorZipCode As String            ' New Store Zip
        Private mVendorCountry As String            ' New Store Country Code
        Private mPeopleSoftVendorID As String       ' PeopleSoft Vendor Number
        'Private mGLMarketingExpenseAcct As Integer  'key for store as vendor
        Private mIncSlim As Byte                    ' Include Slim entries in Cloning
        Private mIncFutureSale As Byte              ' Include Future Sale entries in Cloning
        Private mIncPromoPlanner As Byte            ' Include Promo Planner entries in Cloning


#Region "Property Variables"
        <NotUndoable()> Private mID As Integer = Nothing
        Private mTimestamp(7) As Byte
#End Region

#Region "Property Subs"

        <System.ComponentModel.Browsable(False)> _
        Public Property StoreID() As Integer
            Get
                Return mStoreID
            End Get
            Set(ByVal value As Integer)
                If CanWriteProperty(True) Then
                    If mStoreID <> value Then
                        mStoreID = value
                    End If
                End If
                PropertyHasChanged()
            End Set
        End Property
        <System.ComponentModel.Browsable(False)> _
        Public ReadOnly Property StoreStoreCount() As Integer
            Get
                Return mStoreStoreCount
            End Get
        End Property

        Public Property StoreName() As String
            Get
                Return mStoreName
            End Get
            Set(ByVal value As String)
                If CanWriteProperty(True) Then
                    If mStoreName <> value Then
                        mStoreName = value
                    End If
                End If
                PropertyHasChanged()
            End Set

        End Property

        Public Property StoreAbbr() As String
            Get
                Return mStoreAbbr
            End Get
            Set(ByVal value As String)
                If CanWriteProperty(True) Then
                    If mStoreAbbr <> value Then
                        mStoreAbbr = value
                    End If
                End If
                PropertyHasChanged()
            End Set

        End Property

        Public Property BusinessUnit_Id() As Integer
            Get
                Return mBusinessUnit_Id
            End Get
            Set(ByVal value As Integer)
                If CanWriteProperty(True) Then
                    If mBusinessUnit_Id <> value Then
                        mBusinessUnit_Id = value
                    End If
                End If
                PropertyHasChanged()
            End Set

        End Property

        Public Property SourceStoreNo() As String
            Get
                Return mSourceStoreNo
            End Get
            Set(ByVal value As String)
                If CanWriteProperty(True) Then
                    If mSourceStoreNo <> value Then
                        mSourceStoreNo = value
                    End If
                End If
                PropertyHasChanged()
            End Set

        End Property


        Public Property VendorName() As String
            Get
                Return mVendorName
            End Get
            Set(ByVal value As String)
                If CanWriteProperty(True) Then
                    If mVendorName <> value Then
                        mVendorName = value
                    End If
                End If
                PropertyHasChanged()
            End Set

        End Property


        Public Property VendorAddress() As String
            Get
                Return mVendorAddress
            End Get
            Set(ByVal value As String)
                If CanWriteProperty(True) Then
                    If mVendorAddress <> value Then
                        mVendorAddress = value
                    End If
                End If
                PropertyHasChanged()
            End Set

        End Property


        Public Property VendorCity() As String
            Get
                Return mVendorCity
            End Get
            Set(ByVal value As String)
                If CanWriteProperty(True) Then
                    If mVendorCity <> value Then
                        mVendorCity = value
                    End If
                End If
                PropertyHasChanged()
            End Set

        End Property


        Public Property VendorState() As String
            Get
                Return mVendorState
            End Get
            Set(ByVal value As String)
                If CanWriteProperty(True) Then
                    If mVendorState <> value Then
                        mVendorState = value
                    End If
                End If
                PropertyHasChanged()
            End Set

        End Property

        Public Property VendorZipCode() As String
            Get
                Return mSourceStoreNo
            End Get
            Set(ByVal value As String)
                If CanWriteProperty(True) Then
                    If mVendorZipCode <> value Then
                        mVendorZipCode = value
                    End If
                End If
                PropertyHasChanged()
            End Set

        End Property


        Public Property VendorCountry() As String
            Get
                Return mVendorCountry
            End Get
            Set(ByVal value As String)
                If CanWriteProperty(True) Then
                    If mVendorCountry <> value Then
                        mVendorCountry = value
                    End If
                End If
                PropertyHasChanged()
            End Set

        End Property


        Public Property PeopleSoftVendorID() As Integer
            Get
                Return mPeopleSoftVendorID
            End Get
            Set(ByVal value As Integer)
                If CanWriteProperty(True) Then
                    If mPeopleSoftVendorID <> value Then
                        mPeopleSoftVendorID = value
                    End If
                End If
                PropertyHasChanged()
            End Set

        End Property
#End Region

        '<System.ComponentModel.Browsable(True)> _
        'Public Overrides ReadOnly Property IsDirty() As Boolean
        '    Get
        '        Return MyBase.IsDirty OrElse mStores.IsDirty
        '    End Get
        'End Property

#End Region

#Region "Factory Methods"
        'Private Sub New()
        '    'MyBase.MarkAsChild()
        '    'must use paramaterized constructor
        'End Sub
        Protected Sub New()
            AddBusinessRules()
        End Sub

        Friend Sub New(ByVal dr As SafeDataReader)
            mStoreID = dr.GetInt32("StoreID")
            mStoreStoreCount = dr.GetInt32("StoreCount")
            mStoreName = dr.GetString("StoreDesc")
            dr.GetBytes("LastUpdate", 0, mStoreLastUpdate, 0, 8)
            MyBase.MarkOld()
            'MyBase.MarkAsChild()
        End Sub
        Public Overloads Shared Function NewStore() As Store
            Dim ReturnStore As New Store
            Return ReturnStore
        End Function
        Public Overloads Shared Function NewStore(ByVal nStoreID As Integer, ByVal nStoreAbbr As String, ByVal nStoreName As String, ByVal nStoreJurisdictionID As Integer, _
        ByVal nZoneID As Integer, ByVal nTaxJurisdictionID As Integer, ByVal nBusinessUnit_Id As Integer, ByVal nPlumStoreNo As Integer, ByVal nPSIStoreNo As Integer, _
        ByVal nSourceStoreNo As Integer, ByVal nISSPriceChgTypeID As Integer, ByVal nStoreSubTeamSubstitutions As String, _
        ByVal nVendorName As String, ByVal nVendorAddress As String, ByVal nVendorCity As String, ByVal nVendorState As String, ByVal nVendorZipCode As String, _
        ByVal nPeopleSoftVendorID As String, ByVal nIncSlim As Byte, ByVal nIncFutureSale As Byte, ByVal nIncPromoPlanner As Byte) _
            As Store

            'Public Overloads Shared Function NewStore(ByVal nStoreID As String, ByVal nStoreAbbr As String, ByVal nStoreName As String, ByVal nStoreJurisdictionID As Integer, _
            'ByVal nZoneID As Integer, ByVal nTaxJurisdictionID As Integer, ByVal nBusinessUnit_Id As String, ByVal nPlumStoreNo As String, ByVal nPSIStoreNo As String, _
            'ByVal nSourceStoreNo As Integer, ByVal nISSPriceChgTypeID As Integer, ByVal nStoreSubTeamSubstitutions As String, _
            'ByVal nVendorName As String, ByVal nVendorAddress As String, ByVal nVendorCity As String, ByVal nVendorState As String, ByVal nVendorZipCode As String, _
            'ByVal nPeopleSoftVendorID As String, ByVal nIncSlim As Byte, ByVal nIncFutureSale As Byte, ByVal nIncPromoPlanner As Byte) _
            '    As Store

            'mStoreAdd = Store.NewStore(varStoreNumber, Me.txtStoreAbbrev.Text, Me.txtStoreName.Text, CInt(Me.cboStoreJurisdiction.SelectedValue), _
            'CInt(Me.cboZone.SelectedValue), CInt(Me.cboTaxJurisdiction.SelectedValue), varBusinessUnitID, varPlumStoreNo, _
            'varPSIStoreNo, varSourceStoreNo, CInt(Me.cboISSPriceChgType.SelectedValue), varStoreSubTeamSubstitutions, _
            'Me.txtVendorName.Text, Me.txtVendorAddress.Text, Me.txtVendorCity.Text, Me.txtVendorState.Text, Me.txtVendorZip.Text, Me.txtPeopleSoftID.Text, _
            'CByte(Me.chkIncSlim.Checked), CByte(Me.chkIncFutureSale.Checked), CByte(Me.chkIncPromoPlanner.Checked))


            Dim ReturnValue As New Store
            ReturnValue.mStoreID = nStoreID                                 ' The new store no
            ReturnValue.StoreID = ReturnValue.mStoreID
            ReturnValue.mStoreAbbr = nStoreAbbr                             ' Store Abbreviation
            ReturnValue.mStoreName = nStoreName             ' Name of new store
            ReturnValue.StoreName = ReturnValue.mStoreName             ' Name of new store
            ReturnValue.mStoreJurisdictionID = nStoreJurisdictionID         ' StoreJurisdictionID
            ReturnValue.mZoneID = nZoneID                                   ' Zone 
            ReturnValue.mTaxJurisdictionID = nTaxJurisdictionID             ' TaxJurisdictionID
            ReturnValue.mBusinessUnit_Id = nBusinessUnit_Id         ' Business Unit ID
            'ReturnValue.mStoreLastUpdate = nStoreLastUpdate
            ReturnValue.mPlumStoreNo = nPlumStoreNo           ' The PLUM store no
            ReturnValue.mPSIStoreNo = nPSIStoreNo           ' The PLUM store no
            ReturnValue.mSourceStoreNo = nSourceStoreNo           ' The source store no
            ReturnValue.mISSPriceChgTypeID = nISSPriceChgTypeID               ' name for store as vendor
            ReturnValue.mStoreSubTeamSubstitutions = nStoreSubTeamSubstitutions               ' name for store as vendor
            ReturnValue.mVendorName = nVendorName               ' name for store as vendor
            ReturnValue.mVendorAddress = nVendorAddress            ' New Store Address 
            ReturnValue.mVendorCity = nVendorCity               ' New Store City
            ReturnValue.mVendorState = nVendorState              ' New Store State
            ReturnValue.mVendorZipCode = nVendorZipCode            ' New Store Zip
            'ReturnValue.mVendorCountry = nVendorCountry            ' New Store Country Code
            ReturnValue.mPeopleSoftVendorID = nPeopleSoftVendorID       ' PeopleSoft Vendor Number
            'ReturnValue.mGLMarketingExpenseAcct = nGLMarketingExpenseAcct  'key for store as vendor
            ReturnValue.mIncSlim = nIncSlim                   ' Include Slim entries in Cloning
            ReturnValue.mIncFutureSale = nIncFutureSale              ' Include Future Sale entries in Cloning
            ReturnValue.mIncPromoPlanner = nIncPromoPlanner
            ReturnValue.mStoreStoreCount = 0
            ReturnValue.ValidationRules.GetBrokenRules()
            ReturnValue.MarkNew()
            'GetErrorInformation(ReturnValue).ToString()
            'Csla.Validation.ValidationRules.CheckRules()

            ReturnValue.ValidationRules.CheckRules()
            Return ReturnValue
        End Function

#End Region

#Region "Data Access Methods"

        Private Class Criteria
            Private mStoreID As Integer

            Private Sub New()
                'must use paramaterized constructor
            End Sub
            Public Sub New(ByVal StoreID As Integer)
                mStoreID = StoreID
            End Sub
            Public ReadOnly Property StoreID() As Integer
                Get
                    Return mStoreID
                End Get
            End Property
        End Class

        Protected Overrides Function GetIdValue() As Object
            Return mStoreID
        End Function


        Public Overrides Function Save() As Store

            Dim result As Store
            result = MyBase.Save()
            Return result

        End Function
        '<Transactional(TransactionalTypes.TransactionScope)> _
        Protected Overrides Sub DataPortal_Insert()
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
                Me.Insert(cn)
            End Using
        End Sub

        Friend Sub Insert(ByVal cn As SqlConnection)

            ' if we're not dirty then don't update the database
            If Not Me.IsDirty Then Exit Sub

            Using cm As SqlCommand = cn.CreateCommand
                cm.CommandText = "InsertCloneStore"
                cm.Parameters.AddWithValue("@NewStoreNo", mStoreID)
                cm.Parameters.AddWithValue("@StoreAbbr", mStoreAbbr)
                cm.Parameters.AddWithValue("@NewStoreName", mStoreName)
                cm.Parameters.AddWithValue("@StoreJurisdiction", mStoreJurisdictionID)
                cm.Parameters.AddWithValue("@ZoneID", mZoneID)
                cm.Parameters.AddWithValue("@TaxJurisdiction", mTaxJurisdictionID)
                cm.Parameters.AddWithValue("@BusinessUnit_Id", mBusinessUnit_Id)
                cm.Parameters.AddWithValue("@PSI_Store_No", mPSIStoreNo)
                cm.Parameters.AddWithValue("@Plum_Store_No", mPlumStoreNo)
                cm.Parameters.AddWithValue("@OldStoreNo", mSourceStoreNo)
                cm.Parameters.AddWithValue("@ISSPriceChgTypeID", mISSPriceChgTypeID)
                cm.Parameters.AddWithValue("@StoreSubTeamSubstitutions", mStoreSubTeamSubstitutions)
                cm.Parameters.AddWithValue("@VendorName", mVendorName)
                cm.Parameters.AddWithValue("@VendorAddress", mVendorAddress)
                cm.Parameters.AddWithValue("@VendorCity", mVendorCity)
                cm.Parameters.AddWithValue("@VendorState", mVendorState)
                cm.Parameters.AddWithValue("@VendorZipCode", mVendorZipCode)
                'cm.Parameters.AddWithValue("@GLMarketingExpenseAcct", mGLMarketingExpenseAcct)
                'cm.Parameters.AddWithValue("@NewVendorKey", mSourceStoreNo)
                cm.Parameters.AddWithValue("@PeopleSoftVendorID", mPeopleSoftVendorID)
                cm.Parameters.AddWithValue("@IncSlim", mIncSlim)
                cm.Parameters.AddWithValue("@IncFutureSale", mIncFutureSale)
                cm.Parameters.AddWithValue("@IncPromoPlanner", mIncPromoPlanner)
                'mStoreLastUpdate = DoInsertUpdate(cm)
                cm.CommandType = CommandType.StoredProcedure

                cm.CommandTimeout = 6000
                cm.ExecuteNonQuery()
                MarkOld()
                Save(True)
            End Using

        End Sub

        Friend Sub Update(ByVal cn As SqlConnection)

            ' if we're not dirty then don't update the database
            If Not Me.IsDirty Then Exit Sub

        End Sub

#End Region


    End Class
End Namespace

