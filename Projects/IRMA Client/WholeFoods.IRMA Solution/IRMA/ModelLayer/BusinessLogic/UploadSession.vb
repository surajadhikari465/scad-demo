
Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient
Imports System.Data
Imports WholeFoods.IRMA.ModelLayer.DataAccess
Imports WholeFoods.IRMA.Common
Imports WholeFoods.IRMA.ExtendedItemMaintenance.Logic
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.ModelLayer.BusinessLogic

#Region "Public Enums"

    Public Enum SessionActions
        Import
        Export
        SaveNew
        SaveExisting
        Load
        LoadItems
    End Enum

#End Region

    ''' <summary>
    ''' Business Object for the UploadSession db table.
    '''
    ''' This class inherits persistent properties from the regenerable base class.
    ''' These properties map one-to-one to columns in the UploadSession db table.
    '''
    ''' MODIFY THIS CLASS, NOT THE BASE CLASS.
    '''
    ''' Created By:	David Marine
    ''' Created   :	Feb 12, 2007
    ''' </summary>
    ''' <remarks></remarks>
    Public Class UploadSession
        Inherits UploadSessionRegen

#Region "Overriden Fields and Properties"

        ''' <summary>
        ''' Gets the number of items (UploadRows) that have been validated
        ''' but only if this Upload Session has been uploaded. Returns zero otherwise.
        ''' </summary>
        ''' <value></value>
        ''' <returns>Integer</returns>
        ''' <remarks></remarks>
        Public Overrides Property ItemsProcessedCount() As System.Int32
            Get
                Dim theItemsProcessedCount As Integer = 0

                For Each theUploadRow As UploadRow In Me.UploadRowCollection

                    Dim upload_exclusion As Integer = 0
                    Dim theUploadValueForUploadExclusionFlag As UploadValue

                    theUploadValueForUploadExclusionFlag = theUploadRow.FindUploadValueByAttributeKey(EIM_Constants.UPLOAD_EXCLUSION_COLUMN)

                    If Not IsNothing(theUploadValueForUploadExclusionFlag) AndAlso CBool(theUploadValueForUploadExclusionFlag.Value) Then
                        upload_exclusion = 1
                    End If

                    If Not theUploadRow.IsDeleted And
                            Not theUploadRow.IsMarkedForDelete And theUploadRow.ValidationLevel <> EIM_Constants.ValidationLevels.Invalid And Not (theUploadRow.ValidationLevel = EIM_Constants.ValidationLevels.Warning And upload_exclusion = 1) Then
                        theItemsProcessedCount = theItemsProcessedCount + 1
                    End If
                Next

                Return theItemsProcessedCount
            End Get
            Set(ByVal value As System.Int32)
                MyBase.ItemsProcessedCount = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the number of items (UploadRows) that have been not validated
        ''' but only if this Upload Session has been uploaded. Returns zero otherwise.
        ''' </summary>
        ''' <value></value>
        ''' <returns>Integer</returns>
        ''' <remarks></remarks>
        Public Overrides Property ErrorsCount() As System.Int32
            Get
                Dim theErrorCount As Integer = 0

                For Each theUploadRow As UploadRow In Me.UploadRowCollection
                    If Not theUploadRow.IsDeleted And
                            Not theUploadRow.IsMarkedForDelete And theUploadRow.ValidationLevel = EIM_Constants.ValidationLevels.Invalid Then
                        theErrorCount = theErrorCount + 1
                    End If
                Next

                Return theErrorCount
            End Get
            Set(ByVal value As System.Int32)
                MyBase.ItemsProcessedCount = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the number of items (UploadRows) that have been not validated
        ''' but only if this Upload Session has been uploaded. Returns zero otherwise.
        ''' </summary>
        ''' <value></value>
        ''' <returns>Integer</returns>
        ''' <remarks></remarks>
        Public Property WarningCount() As System.Int32
            Get
                Dim theWarningCount As Integer = 0

                For Each theUploadRow As UploadRow In Me.UploadRowCollection
                    If Not theUploadRow.IsDeleted And
                            Not theUploadRow.IsMarkedForDelete And theUploadRow.ValidationLevel = EIM_Constants.ValidationLevels.Warning Then
                        theWarningCount = theWarningCount + 1
                    End If
                Next

                Return theWarningCount
            End Get
            Set(ByVal value As System.Int32)
                MyBase.ItemsProcessedCount = value
            End Set
        End Property

        Private _WarningCountItemDeauthForAllStores As Integer = 0

        ''' <summary>
        ''' Gets the number of items (UploadRows) that will be deauthorized for all stores 
        ''' </summary>
        ''' <value></value>
        ''' <returns>Integer</returns>
        ''' <remarks></remarks>
        Public Property WarningCountItemDeauthForAllStores() As System.Int32
            Get
                Return _WarningCountItemDeauthForAllStores
            End Get
            Set(ByVal value As System.Int32)
                _WarningCountItemDeauthForAllStores = value
            End Set
        End Property

        Private _WarningCountItemDeauthForStore As Integer = 0

        ''' <summary>
        ''' Gets the number of items (UploadRows) that will be deauthorized for store 
        ''' </summary>
        ''' <value></value>
        ''' <returns>Integer</returns>
        ''' <remarks></remarks>
        Public Property WarningCountItemDeauthForStore() As System.Int32
            Get
                Return _WarningCountItemDeauthForStore
            End Get
            Set(ByVal value As System.Int32)
                _WarningCountItemDeauthForStore = value
            End Set
        End Property

        Private _WarningCountPrimaryVendorSwap As Integer = 0

        ''' <summary>
        ''' Gets the number of items (UploadRows) where primary vendor will be swapped 
        ''' </summary>
        ''' <value></value>
        ''' <returns>Integer</returns>
        ''' <remarks></remarks>
        Public Property WarningCountPrimaryVendorSwap() As System.Int32
            Get
                Return _WarningCountPrimaryVendorSwap
            End Get
            Set(ByVal value As System.Int32)
                _WarningCountPrimaryVendorSwap = value
            End Set
        End Property


        ''' <summary>
        ''' Gets the count of all items (UploadRows) in this UploadSession.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Property ItemsLoadedCount() As System.Int32
            Get
                Dim theCount As Integer = 0

                For Each theUploadRow As UploadRow In Me.UploadRowCollection
                    If Not theUploadRow.IsDeleted And
                            Not theUploadRow.IsMarkedForDelete Then
                        theCount = theCount + 1
                    End If
                Next

                Return theCount
            End Get
            Set(ByVal value As System.Int32)
                MyBase.ItemsLoadedCount = value
            End Set
        End Property

#End Region

#Region "New Fields and Properties"

        Private _dataSet As DataSet
        Private _allUploadAttributesByName As BusinessObjectCollection = Nothing
        Private _allUploadAttributesByID As BusinessObjectCollection = Nothing
        Private _uploadAttributesByName As BusinessObjectCollection = Nothing
        Private _uploadAttributesByKey As BusinessObjectCollection = Nothing
        Private _slimDealDataGroupName As String
        Private _isdealChangeColumnName As String
        Private _progressCounter As Integer
        Private _progressComplete As Boolean

        ''' <summary>
        ''' Returns a DataSet with one DataTable filled with data
        ''' for each UploadType in this UploadSession.
        ''' Only builds dataset from the UploadSession data the first time it is called.
        ''' Call BuildDataSet() to build the dataset at any time.
        ''' </summary>
        ''' <value></value>
        ''' <returns>DataSet</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DataSet() As DataSet
            Get
                ' only buid the DataSet if it hasn't been yet
                If IsNothing(_dataSet) Then
                    BuildDataSet()
                End If

                Return _dataSet
            End Get
        End Property

        Public Property SlimDealDataGroupName() As String
            Get
                Return _slimDealDataGroupName
            End Get
            Set(ByVal value As String)
                _slimDealDataGroupName = value
            End Set
        End Property

        Public Property IsdealChangeColumnName() As String
            Get
                Return _isdealChangeColumnName
            End Get
            Set(ByVal value As String)
                _isdealChangeColumnName = value
            End Set
        End Property

        Public Property AllUploadAttributesByName() As BusinessObjectCollection
            Get
                If IsNothing(_allUploadAttributesByName) Then
                    BuildQuickLookUpCollections()
                End If
                Return _allUploadAttributesByName
            End Get
            Set(ByVal value As BusinessObjectCollection)
                _allUploadAttributesByName = value
            End Set
        End Property

        Public Property AllUploadAttributesByID() As BusinessObjectCollection
            Get
                If IsNothing(_allUploadAttributesByID) Then
                    BuildQuickLookUpCollections()
                End If
                Return _allUploadAttributesByID
            End Get
            Set(ByVal value As BusinessObjectCollection)
                _allUploadAttributesByID = value
            End Set
        End Property

        Public Property UploadAttributesByKey() As BusinessObjectCollection
            Get
                If IsNothing(_uploadAttributesByKey) Then
                    BuildQuickLookUpCollections()
                End If
                Return _uploadAttributesByKey
            End Get
            Set(ByVal value As BusinessObjectCollection)
                _uploadAttributesByKey = value
            End Set
        End Property

        Public Property UploadAttributesByName() As BusinessObjectCollection
            Get
                If IsNothing(_uploadAttributesByName) Then
                    BuildQuickLookUpCollections()
                End If
                Return _uploadAttributesByName
            End Get
            Set(ByVal value As BusinessObjectCollection)
                _uploadAttributesByName = value
            End Set
        End Property

        ''' <summary>
        ''' Build and return a UploadTypeCollection from
        ''' this UploadSession's UploadSessionUploadTypeCollection.
        ''' </summary>
        ''' <value></value>
        ''' <returns>Hashtable</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property UploadTypeCollection() As BusinessObjectCollection
            Get
                Dim theUploadTypeCollection As New BusinessObjectCollection()

                ' this will always have just a few UploadTypes so it is better
                ' to build it fresh each time it is needed
                For Each theUploadSessionUploadType As UploadSessionUploadType _
                        In Me.UploadSessionUploadTypeCollection

                    If Not theUploadSessionUploadType.IsMarkedForDelete Then
                        theUploadTypeCollection.
                            Add(theUploadSessionUploadType.UploadTypeCode, theUploadSessionUploadType.UploadType)
                    End If
                Next

                Return theUploadTypeCollection
            End Get
        End Property

        Public Property ProgressCounter() As Integer
            Get
                Return _progressCounter
            End Get
            Set(ByVal value As Integer)
                _progressCounter = value
            End Set
        End Property

        Public Property ProgressComplete() As Boolean
            Get
                Return _progressComplete
            End Get
            Set(ByVal value As Boolean)
                _progressComplete = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the count of all items (UploadRows) marked for delete.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property RowsMarkedForDeleteCount() As System.Int32
            Get
                Dim theCount As Integer = 0

                For Each theUploadRow As UploadRow In Me.UploadRowCollection
                    If theUploadRow.IsMarkedForDelete Then
                        theCount = theCount + 1
                    End If
                Next

                Return theCount
            End Get
        End Property

        ''' <summary>
        ''' Gets the count of all items (UploadRows) not marked for delete.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property RowsNotMarkedForDeleteCount() As System.Int32
            Get
                Dim theCount As Integer = 0

                For Each theUploadRow As UploadRow In Me.UploadRowCollection
                    If Not theUploadRow.IsMarkedForDelete Then
                        theCount = theCount + 1
                    End If
                Next

                Return theCount
            End Get
        End Property

#End Region

#Region "Constructors"

        Public Sub New()
            MyBase.New()
        End Sub

#End Region

#Region "Shared Methods"

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Returns the UploadSessionUploadType by UploadType code only if
        ''' it exists for the session and is not marked for delete.
        ''' </summary>
        ''' <param name="inUploadTypeCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FindUploadSessionUploadType(ByVal inUploadTypeCode As String, ByVal inNotMarkedForDelete As Boolean) As UploadSessionUploadType

            Dim theUploadSessionUploadType As UploadSessionUploadType = Nothing

            ' find the UploadSessionUploadType for the CurrentUploadTypeCode
            Dim theSearchResultCollection As BusinessObjectCollection =
                    Me.UploadSessionUploadTypeCollection.FindByPropertyValue("UploadTypeCode", inUploadTypeCode)

            Dim sessionHasCurrentUploadType As Boolean = theSearchResultCollection.Count > 0

            ' if found
            If sessionHasCurrentUploadType Then
                ' get the UploadSessionUploadType - should only be one
                theUploadSessionUploadType = CType(theSearchResultCollection.Item(0), UploadSessionUploadType)
            End If

            ' treat it as not existing if marked for delete
            If inNotMarkedForDelete And Not IsNothing(theUploadSessionUploadType) AndAlso theUploadSessionUploadType.IsMarkedForDelete Then
                theUploadSessionUploadType = Nothing
            End If

            Return theUploadSessionUploadType

        End Function

        ''' <summary>
        ''' Returns a collection of the UploadTypeAttributes for a given
        ''' UploadType code in the session's first UploadRow.
        ''' Returns Nothing if there is not at least one UploadRow.
        ''' </summary>
        ''' <param name="inUploadCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FindUploadTypeAttributesInFirstUploadRowByUploadTypeCode(ByVal inUploadCode As String, Optional ByVal isSlimFunctionalityEnabled As Boolean = True) As BusinessObjectCollection

            Dim theUploadTypeAttributeCollection As New BusinessObjectCollection()
            Dim theFirstUploadRow As UploadRow = Me.UploadRow

            If Not IsNothing(theFirstUploadRow) Then

                For Each theUploadValue As UploadValue In theFirstUploadRow.UploadValueCollection
                    For Each theUploadTypeAttribute As UploadTypeAttribute In theUploadValue.UploadAttribute.UploadTypeAttributeCollection

                        If (Not isSlimFunctionalityEnabled AndAlso (theUploadTypeAttribute.GroupName = Me.SlimDealDataGroupName _
                                                                    OrElse theUploadTypeAttribute.UploadAttribute.ColumnNameOrKey = Me.IsdealChangeColumnName)) Then
                            Continue For
                        End If

                        If theUploadTypeAttribute.UploadTypeCode.Equals(inUploadCode) AndAlso
                                Not theUploadTypeAttributeCollection.ContainsKey(theUploadTypeAttribute.PrimaryKey) Then

                            theUploadTypeAttributeCollection.Add(theUploadTypeAttribute.PrimaryKey, theUploadTypeAttribute)
                        End If
                    Next
                Next


                '2/14/2011 
                'add upload_exclusion column to any template for ITEM_MAINTENANCE, PRICE_UPLOAD, COST_UPLOAD if spreadsheet is imported and field is not there.
                'add delete_vendor, deauth_store columns to any template for COST_UPLOAD only if spreadsheet is imported and field is not there.

                Dim myUploadAttribute As UploadAttribute
                Dim allUploadTypeAttributes As BusinessObjectCollection = UploadTypeAttributeDAO.Instance.GetAllUploadTypeAttributes()

                For Each uploadTypeAttribute As UploadTypeAttribute In allUploadTypeAttributes
                    myUploadAttribute = FindUploadAttributeByID(uploadTypeAttribute.UploadAttributeID)

                    If Not IsNothing(myUploadAttribute) Then

                        If myUploadAttribute.ColumnNameOrKey = "upload_exclusion" Then
                            If uploadTypeAttribute.UploadTypeCode.Equals(inUploadCode) Then
                                If Not theUploadTypeAttributeCollection.ContainsKey(uploadTypeAttribute.PrimaryKey) Then
                                    theUploadTypeAttributeCollection.Add(uploadTypeAttribute.PrimaryKey, uploadTypeAttribute)
                                End If
                            End If
                        End If


                        If myUploadAttribute.ColumnNameOrKey = "delete_vendor" And inUploadCode = "COST_UPLOAD" Then
                            If uploadTypeAttribute.UploadTypeCode.Equals(inUploadCode) Then
                                If Not theUploadTypeAttributeCollection.ContainsKey(uploadTypeAttribute.PrimaryKey) Then
                                    theUploadTypeAttributeCollection.Add(uploadTypeAttribute.PrimaryKey, uploadTypeAttribute)
                                End If
                            End If
                        End If


                        If myUploadAttribute.ColumnNameOrKey = "deauth_store" And inUploadCode = "COST_UPLOAD" Then
                            If uploadTypeAttribute.UploadTypeCode.Equals(inUploadCode) Then
                                If Not theUploadTypeAttributeCollection.ContainsKey(uploadTypeAttribute.PrimaryKey) Then
                                    theUploadTypeAttributeCollection.Add(uploadTypeAttribute.PrimaryKey, uploadTypeAttribute)
                                End If
                            End If
                        End If

                    End If

                Next



            End If

            Return theUploadTypeAttributeCollection

        End Function

        ''' <summary>
        ''' Returns a collection of the UploadAttributes for a given
        ''' UploadType code in the session's first UploadRow.
        ''' Returns Nothing if there is not at least one UploadRow.
        ''' </summary>
        ''' <param name="inUploadCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function FindUploadAttributesInFirstUploadRowByUploadTypeCode(ByVal inUploadCode As String, Optional ByVal isSlimFunctionalityEnabled As Boolean = True) As BusinessObjectCollection

            Dim theUploadAttributeCollection As New BusinessObjectCollection()
            Dim theFirstUploadRow As UploadRow = Me.UploadRow

            If Not IsNothing(theFirstUploadRow) Then

                For Each theUploadValue As UploadValue In theFirstUploadRow.UploadValueCollection

                    '03/01/2011 do not export upload_exclusion column into a spreadsheet
                    If theUploadValue.ColumnNameOrKey <> "upload_exclusion" Then

                        For Each theUploadTypeAttribute As UploadTypeAttribute In theUploadValue.UploadAttribute.UploadTypeAttributeCollection

                            If (Not isSlimFunctionalityEnabled AndAlso (theUploadTypeAttribute.GroupName = Me.SlimDealDataGroupName _
                                                                         OrElse theUploadTypeAttribute.UploadAttribute.ColumnNameOrKey = Me.IsdealChangeColumnName)) Then
                                Continue For
                            End If

                            If theUploadTypeAttribute.UploadTypeCode.Equals(inUploadCode) Then

                                Try
                                    theUploadAttributeCollection.Add(theUploadTypeAttribute.UploadAttribute.PrimaryKey, theUploadTypeAttribute.UploadAttribute)
                                Catch
                                    Throw New Exception("The attribute " + theUploadTypeAttribute.UploadAttribute.Name + " has been found in an upload row more than once. ")
                                End Try
                            End If
                        Next

                    End If

                Next

            End If

            Return theUploadAttributeCollection

        End Function

        ''' <summary>
        ''' Returns the UpdateAttribute for the provided attribute name if
        ''' it can be found regardless of session's UploadTypes.
        ''' </summary>
        ''' <param name="attributeName"></param>
        ''' <returns>UpdateTypeAttribute</returns>
        ''' <remarks></remarks>
        Public Function FindUploadAttributeByName(ByVal attributeName As String) As UploadAttribute

            Dim theUploadAttribute As UploadAttribute = Nothing

            If Not String.IsNullOrEmpty(attributeName) Then
                theUploadAttribute = CType(Me.AllUploadAttributesByName.ItemByKey(attributeName.ToLower()), UploadAttribute)
            End If

            Return theUploadAttribute

        End Function

        ''' <summary>
        ''' Returns the UpdateAttribute for the provided attribute name if
        ''' it can be found for the session's UploadTypes.
        ''' </summary>
        ''' <param name="attributeName"></param>
        ''' <returns>UpdateTypeAttribute</returns>
        ''' <remarks></remarks>
        Public Function FindUploadAttributeByNameForSessionUploadTypes(ByVal attributeName As String) As UploadAttribute

            Dim theUploadAttribute As UploadAttribute = Nothing

            If Not String.IsNullOrEmpty(attributeName) Then
                theUploadAttribute = CType(Me.UploadAttributesByName.ItemByKey(attributeName.ToLower()), UploadAttribute)
            End If

            Return theUploadAttribute

        End Function

        ''' <summary>
        ''' Returns the UpdateAttribute for the provided attribute name if
        ''' it can be found regardless of session's UploadTypes.
        ''' </summary>
        ''' <param name="attributeID"></param>
        ''' <returns>UpdateTypeAttribute</returns>
        ''' <remarks></remarks>
        Public Function FindUploadAttributeByID(ByVal attributeID As Integer) As UploadAttribute

            Dim theUploadAttribute As UploadAttribute = Nothing

            theUploadAttribute = CType(Me.AllUploadAttributesByID.ItemByKey(attributeID), UploadAttribute)

            Return theUploadAttribute

        End Function

        ''' <summary>
        ''' Returns the UpdateAttribute for the provided attribute key if
        ''' it can be found for the session's UploadTypes.
        ''' </summary>
        ''' <param name="attributeKey"></param>
        ''' <returns>UpdateTypeAttribute</returns>
        ''' <remarks></remarks>
        Public Function FindUploadAttributeByKeyForSessionUploadTypes(ByVal attributeKey As String) As UploadAttribute

            Dim theUploadAttribute As UploadAttribute = Nothing

            If Not String.IsNullOrEmpty(attributeKey) Then
                theUploadAttribute = CType(Me.UploadAttributesByKey.ItemByKey(attributeKey.ToLower()), UploadAttribute)
            End If

            Return theUploadAttribute

        End Function

        ''' <summary>
        ''' Builds a DataSet with one DataTable filled with data
        ''' for each UploadType in this UploadSession.
        ''' Use the DataSet property to get DataSet.
        ''' </summary>
        ''' <remarks></remarks>
        Public Function BuildDataSet(Optional ByVal isSlimFunctionalityEnabled As Boolean = True) As UploadRowHolderCollection

            Dim theUploadRowHolderCollection As New UploadRowHolderCollection()

            _dataSet = New DataSet()

            Dim theDataTable As DataTable
            Dim theDataColumn As DataColumn
            Dim theDataRow As DataRow
            Dim theUploadAttribute As UploadAttribute

            ' reset the progress tracking properties
            Me.ProgressComplete = False
            Me.ProgressCounter = 0

            ' the max progress count reflects the number of DataTable rows that are loaded
            ' which is equal to the number of UploadRows times the number of UploadTypes
            ' the user selected
            Dim theProgressCountDouble As Double = 0
            Dim theMaxProgressCount As Integer = Me.UploadRowCollection.Count * Me.UploadTypeCollection.Count
            Dim theMaxProgressCountStep As Double = Me.UploadRowCollection.Count / theMaxProgressCount

            Try
                ' loop through all the UploadSessionUploadTypes that are not marked for delete
                For Each theUploadSessionUploadType As UploadSessionUploadType In Me.UploadSessionUploadTypeCollection

                    If Not theUploadSessionUploadType.IsMarkedForDelete Then

                        ' create one table for each UploadType in the UploadSession
                        ' and add it to the DataSet
                        theDataTable = New DataTable(theUploadSessionUploadType.UploadTypeCode)
                        _dataSet.Tables.Add(theDataTable)

                        ' add the UploadRow_ID column
                        theDataColumn = New DataColumn(EIM_Constants.UPLOADROW_ID_COLUMN_NAME)
                        theDataColumn.AllowDBNull = False
                        theDataColumn.DataType = UploadAttribute.MapFromDbDataType("int")
                        theDataTable.Columns.Add(theDataColumn)
                        ' add as table PK
                        theDataTable.PrimaryKey = New DataColumn() {theDataColumn}

                        ' add the item key column
                        theDataColumn = New DataColumn(EIM_Constants.ITEM_KEY_ATTR_KEY)
                        theDataColumn.AllowDBNull = False
                        theDataColumn.DataType = UploadAttribute.MapFromDbDataType("int")
                        theDataTable.Columns.Add(theDataColumn)

                        ' add the is identical column
                        theDataColumn = New DataColumn(EIM_Constants.IS_IDENTICAL_ITEM_ROW_COLUMN_NAME)
                        theDataColumn.AllowDBNull = False
                        theDataColumn.DataType = UploadAttribute.MapFromDbDataType("bit")
                        theDataTable.Columns.Add(theDataColumn)

                        ' add the is hidden column
                        theDataColumn = New DataColumn(EIM_Constants.IS_HIDDEN_COLUMN_NAME)
                        theDataColumn.AllowDBNull = False
                        theDataColumn.DataType = UploadAttribute.MapFromDbDataType("bit")
                        theDataTable.Columns.Add(theDataColumn)

                        ' add the is validated column
                        theDataColumn = New DataColumn(EIM_Constants.VALIDATION_LEVEL_COLUMN_NAME)
                        theDataColumn.AllowDBNull = False
                        theDataColumn.DataType = UploadAttribute.MapFromDbDataType("int")
                        theDataTable.Columns.Add(theDataColumn)

                        Dim theUploadTypeAttributes As BusinessObjectCollection =
                            FindUploadTypeAttributesInFirstUploadRowByUploadTypeCode(theUploadSessionUploadType.UploadTypeCode, isSlimFunctionalityEnabled)


                        ' now make sure we also have all the required attributes for the UploadType
                        Dim allUploadTypeAttributes As BusinessObjectCollection = UploadTypeAttributeDAO.Instance.GetAllUploadTypeAttributes()
                        For Each theUploadTypeAttribute As UploadTypeAttribute In allUploadTypeAttributes

                            If (Not isSlimFunctionalityEnabled AndAlso (theUploadTypeAttribute.GroupName = Me.SlimDealDataGroupName _
                                                                        OrElse theUploadTypeAttribute.UploadAttribute.ColumnNameOrKey = Me.IsdealChangeColumnName)) Then
                                Continue For
                            End If

                            If theUploadTypeAttribute.UploadTypeCode.Equals(theUploadSessionUploadType.UploadTypeCode) AndAlso
                                    Me.IsNewItemSessionFlag And theUploadTypeAttribute.UploadAttribute.IsRequiredValue Then
                                ' don't add it if it is already there
                                If Not theUploadTypeAttributes.ContainsKey(theUploadTypeAttribute.PrimaryKey) Then
                                    theUploadTypeAttributes.Add(theUploadTypeAttribute.PrimaryKey, theUploadTypeAttribute)
                                End If
                            End If

                        Next

                        ' now add columns for the configured attributes
                        For Each theUploadTypeAttribute As UploadTypeAttribute In theUploadTypeAttributes

                            If allUploadTypeAttributes.ContainsKey(theUploadTypeAttribute.UploadTypeAttributeID) Then
                                theUploadAttribute =
                                    Me.FindUploadAttributeByID(theUploadTypeAttribute.UploadAttributeID)


                                If Me.IsAttributeInTemplateForUploadType(theUploadTypeAttribute.UploadTypeCode, theUploadAttribute) Then

                                    If Not theDataTable.Columns.Contains(theUploadAttribute.Key) Then
                                        theDataColumn = New DataColumn(theUploadAttribute.Key)

                                        If theUploadAttribute.IsValueListControl Then
                                            ' always set the data type to be a string for
                                            ' an attribute configured to be a valuelist
                                            ' to allow for null values
                                            theDataColumn.DataType = GetType(System.String)
                                        Else
                                            theDataColumn.DataType = UploadAttribute.MapFromDbDataType(theUploadAttribute.DbDataType)
                                        End If

                                        ' allow all null values
                                        ' required values are managed through validation
                                        theDataColumn.AllowDBNull = True
                                        theDataTable.Columns.Add(theDataColumn)
                                    End If
                                End If

                            End If
                        Next

                        ' add the data
                        Dim rowHasData As Boolean = False
                        Dim isValid As Boolean

                        ' sort by item key if the upload type is ITEM_MAINTENANCE to
                        ' be able to remove duplicates
                        Me.UploadRowCollection.SortByPropertyValue("ItemKey")

                        Dim thePreviousUploadRow As UploadRow = Nothing
                        Dim theUploadRowHolder As UploadRowHolder

                        For Each theUploadRow As UploadRow In Me.UploadRowCollection

                            ' this association between an UploadRow, a DataRow, and a GridRow
                            ' will be used later during validation
                            theUploadRowHolder = theUploadRowHolderCollection.Add(theUploadRow)

                            theDataRow = theDataTable.NewRow()

                            ' now add data from configured attribute values
                            For Each theDataColumn In theDataTable.Columns

                                Dim theUploadValue As UploadValue = theUploadRow.FindUploadValueByAttributeKey(theDataColumn.ColumnName)

                                If IsNothing(theUploadValue) Then
                                    theDataRow.Item(theDataColumn.ColumnName) = DBNull.Value
                                Else
                                    ' add data only for the current UploadType of the outer loop
                                    ' which is building the DataTable
                                    If theUploadValue.IsForUpdateType(theUploadSessionUploadType.UploadTypeCode) Then

                                        isValid = CalculatorAndValidatorManager.ValidateDataType(theUploadValue.Value, theUploadValue.DbDataType)

                                        ' if is null or an invalid value for the datatype (unless a string)
                                        ' then set to DBNull
                                        If (IsNothing(theUploadValue.Value) Or String.IsNullOrEmpty(theUploadValue.Value)) Or
                                                (Not isValid And Not theUploadValue.DbDataType.ToLower().Equals("varchar")) Then
                                            theDataRow.Item(theUploadValue.Key) = DBNull.Value
                                        Else
                                            theDataRow.Item(theUploadValue.Key) = theUploadValue.TranslateUploadValueForDisplay()
                                        End If
                                        rowHasData = True
                                    End If
                                End If
                            Next

                            ' add the UploadRow_ID
                            theDataRow.Item(EIM_Constants.UPLOADROW_ID_COLUMN_NAME) = theUploadRow.UploadRowID

                            ' add the item key
                            theDataRow.Item(EIM_Constants.ITEM_KEY_ATTR_KEY) = theUploadRow.ItemKey

                            ' add the is validated value
                            theDataRow.Item(EIM_Constants.VALIDATION_LEVEL_COLUMN_NAME) = theUploadRow.ValidationLevel

                            theDataRow.Item(EIM_Constants.IS_IDENTICAL_ITEM_ROW_COLUMN_NAME) = False
                            theDataRow.Item(EIM_Constants.IS_HIDDEN_COLUMN_NAME) = False


                            '03/01/2011. Set delete_vendor, deauth_store to false for COST_UPLOAD from the spreadsheet if those columns were not in the spreadsheet
                            If theUploadSessionUploadType.UploadTypeCode.Equals(EIM_Constants.COST_UPLOAD_CODE) Then
                                If IsNothing(theDataRow.Item(EIM_Constants.COST_DELETE_VENDOR)) Then
                                    theDataRow.Item(EIM_Constants.COST_DELETE_VENDOR) = False
                                End If
                                If IsNothing(theDataRow.Item(EIM_Constants.COST_DEAUTH_STORE)) Then
                                    theDataRow.Item(EIM_Constants.COST_DEAUTH_STORE) = False
                                End If
                            End If


                            If Not IsNothing(thePreviousUploadRow) Then

                                Dim isItemMaintenanceUploadType As Boolean =
                                        theUploadSessionUploadType.UploadTypeCode.Equals(EIM_Constants.ITEM_MAINTENANCE_CODE)

                                ' if the upload type is Item Maintenance then mark rows that have the same item so they can be marked in the UI
                                If isItemMaintenanceUploadType And thePreviousUploadRow.ItemKey = theUploadRow.ItemKey Then

                                    theDataRow.Item(EIM_Constants.IS_IDENTICAL_ITEM_ROW_COLUMN_NAME) = True

                                End If
                            End If

                            thePreviousUploadRow = theUploadRow

                            If rowHasData Then

                                theDataTable.Rows.Add(theDataRow)

                                ' we will add the grid row later
                                theUploadRowHolder.GridAndDataRowList.Add(theUploadSessionUploadType.UploadTypeCode,
                                    New GridAndDataRowHolder(theUploadRow.UploadRowID, theUploadSessionUploadType.UploadTypeCode, Nothing, theDataRow))

                                rowHasData = False
                                ' increment progress counter
                                theProgressCountDouble = theProgressCountDouble + theMaxProgressCountStep
                                Me.ProgressCounter = CInt(Math.Floor(theProgressCountDouble))
                            End If
                        Next
                    End If
                Next
            Finally
                ' this is critical
                Me.ProgressComplete = True
            End Try

            Return theUploadRowHolderCollection

        End Function

        ''' <summary>
        ''' Load the data in the internal DataSet back into the UploadSession's data structure
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub LoadFromDataSet(ByVal afterImport As Boolean)

            Dim theUploadValue As UploadValue
            Dim theItemKey As Integer
            Dim theUploadRowId As Integer

            Dim theUploadType As UploadType
            Dim theUploadTypeAttribute As UploadTypeAttribute
            Dim theUploadAttribute As UploadAttribute
            Dim theDataValue As String
            Dim theUploadRow As UploadRow
            Dim theUploadTypeCollection As BusinessObjectCollection = Me.UploadTypeCollection

            Dim theRowsToRemove As New BusinessObjectCollection()
            If afterImport Then
                ' premark all rows for manual deletion
                For Each theUploadRow In Me.UploadRowCollection
                    theRowsToRemove.Add(theUploadRow.UploadRowID, theUploadRow)
                Next
            End If

            For Each theDataTable As DataTable In Me.DataSet.Tables

                ' for only the selected UploadTypes
                If UploadTypeCollection.ContainsKey(theDataTable.TableName) Then

                    For Each theDataRow As DataRow In theDataTable.Rows

                        theUploadRowId = Integer.Parse(theDataRow.Item(EIM_Constants.UPLOADROW_ID_COLUMN_NAME).ToString())
                        theItemKey = Integer.Parse(theDataRow.Item(EIM_Constants.ITEM_KEY_ATTR_KEY).ToString())

                        ' look for the existing UploadRow
                        theUploadRow = CType(Me.UploadRowCollection.ItemByKey(theUploadRowId), UploadRow)

                        If afterImport Then
                            ' clear this rows marked for delete flag
                            theRowsToRemove.RemoveByKey(theUploadRow.UploadRowID)
                        End If

                        For Each theDataColumn As DataColumn In theDataTable.Columns

                            ' the item key column in the DataSet is read only and does
                            ' not need to be loaded back into the data structure
                            If Not theDataColumn.ColumnName.Equals(EIM_Constants.UPLOADROW_ID_COLUMN_NAME, StringComparison.CurrentCultureIgnoreCase) And
                                    Not theDataColumn.ColumnName.Equals(EIM_Constants.ITEM_KEY_ATTR_KEY, StringComparison.CurrentCultureIgnoreCase) And
                                    Not theDataColumn.ColumnName.Equals(EIM_Constants.VALIDATION_LEVEL_COLUMN_NAME) And
                                    Not theDataColumn.ColumnName.Equals(EIM_Constants.IS_IDENTICAL_ITEM_ROW_COLUMN_NAME) And
                                    Not theDataColumn.ColumnName.Equals(EIM_Constants.IS_HIDDEN_COLUMN_NAME) Then

                                theUploadType = CType(theUploadTypeCollection.ItemByKey(theDataTable.TableName), UploadType)
                                theUploadTypeAttribute = theUploadType.FindUploadTypeAttributeByKey(theDataColumn.ColumnName)
                                theUploadAttribute = Me.FindUploadAttributeByID(theUploadTypeAttribute.UploadAttributeID)

                                theUploadValue = theUploadRow.FindUploadValueByAttributeKey(theDataColumn.ColumnName)

                                theDataValue = theDataRow.Item(theDataColumn.ColumnName).ToString()


                                ' force the date time format
                                If theUploadTypeAttribute.UploadAttribute.DbDataType.ToLower().Equals("datetime") Or
                                    theUploadTypeAttribute.UploadAttribute.DbDataType.ToLower().Equals("smalldatetime") Then

                                    If Not IsNothing(theDataValue) And Not String.IsNullOrEmpty(theDataValue) Then
                                        '20100216 - Dave Stacey - remove hard-coded date mask string w/configured one
                                        theDataValue = DateTime.Parse(theDataValue).ToString(gsUG_DateMask)

                                        If theUploadTypeAttribute.GroupName = "Flex Attributes" Then
                                            ' Because Flex Attribute date values are stored as strings (varchars) in UploadValue, this
                                            ' date value needs to be stored in a universal format.
                                            theDataValue = DateTime.Parse(theDataValue).ToString("yyyy-MM-dd")
                                        End If

                                    End If
                                End If

                                ' make sure we have a non null and non empty value
                                'If Not String.IsNullOrEmpty(theDataValue) Then
                                ' create the UploadValue if necessary
                                If IsNothing(theUploadValue) Then
                                    theUploadValue = New UploadValue(theUploadAttribute, theUploadRow)
                                End If
                                If String.IsNullOrEmpty(theDataValue) Then
                                    theDataValue = Nothing
                                    theUploadValue.Value = theDataValue
                                    theUploadValue.IsValueNull = True
                                Else
                                    theUploadValue.Value = theDataValue
                                    theUploadValue.Value = theUploadValue.TranslateUploadValueForPersistence()
                                End If
                            End If
                        Next
                    Next
                End If
            Next

            If afterImport Then
                ' now remove all the "marked" rows
                For Each theUploadRow In theRowsToRemove
                    Me.UploadRowCollection.RemoveByKey(theUploadRow.UploadRowID)
                Next
            End If

        End Sub

        Public Sub SetUploadTypes(ByVal uploadTypeCollection As BusinessObjectCollection)

            Dim theUploadSessionUploadType As UploadSessionUploadType

            ' got to clear out the current UploadSessionUploadType collection
            Me.UploadSessionUploadTypeCollection.Clear()

            ' add the provided UploadTypes to the current UploadSession
            For Each theUploadType As UploadType In uploadTypeCollection

                ' create a new one
                ' this will add the new UploadSessionUploadType to the UploadSessionUploadTypeCollection
                ' of the passed in UploadSession
                ' so we do not need to hold on to the new UploadSessionUploadType
                ' we can pass in Nothing for the UploadTypeTemplate as
                ' it should always default to the "All" attributes faux template
                ' and then the user can select the template they want
                theUploadSessionUploadType =
                    New UploadSessionUploadType(Nothing, theUploadType, Me)
            Next

            BuildQuickLookUpCollections()

        End Sub

        ''' <summary>
        ''' Return True if the given UploadAttribute is in the current template
        ''' for any of the UploadAttribute's UploadTypes.
        ''' </summary>
        ''' <param name="inUploadAttribute"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsAttributeInTemplateForAttributeUploadTypes(ByRef inUploadAttribute As UploadAttribute) As Boolean

            Dim isInCurrentTemplate As Boolean = False

            For Each theUploadTypeAttribute As UploadTypeAttribute In inUploadAttribute.UploadTypeAttributeCollection
                isInCurrentTemplate = IsAttributeInTemplateForUploadType(theUploadTypeAttribute.UploadTypeCode, inUploadAttribute)

                If isInCurrentTemplate Then
                    Exit For
                End If
            Next

            Return isInCurrentTemplate

        End Function

        ''' <summary>
        ''' Return True if the given UploadAttribute is in the current template
        ''' for the given UploadType code or if there is no.
        ''' </summary>
        ''' <param name="inUploadAttribute"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsAttributeInTemplateForUploadType(ByRef inUploadTypeCode As String, ByRef inUploadAttribute As UploadAttribute) As Boolean

            Dim isInCurrentTemplate As Boolean = False

            Dim theUploadSessionUploadType As UploadSessionUploadType = Me.FindUploadSessionUploadType(inUploadTypeCode, True)

            If IsNothing(theUploadSessionUploadType) Then
                ' there is no UploadType with that code for this session
                isInCurrentTemplate = False
            ElseIf theUploadSessionUploadType.IsMarkedForDelete Then
                ' there is an UploadType with that code for this session
                ' but it is marked for delete
                isInCurrentTemplate = False
            ElseIf IsNothing(theUploadSessionUploadType.UploadTypeTemplate) Then
                'there is no template so we allow all attributes configured for the UploadType
                isInCurrentTemplate = True
            Else
                ' there is a template so we need to look to see if the given attribute
                ' is part of it
                For Each theUploadTypeTemplateAttribute As UploadTypeTemplateAttribute _
                    In theUploadSessionUploadType.UploadTypeTemplate.UploadTypeTemplateAttributeCollection

                    'System.Console.WriteLine(theUploadTypeTemplateAttribute.UploadTypeAttribute.UploadAttribute.Key)

                    If theUploadTypeTemplateAttribute.UploadTypeAttribute.UploadAttribute.Key.ToLower().Equals(inUploadAttribute.Key) Then
                        isInCurrentTemplate = True
                        Exit For
                    End If
                Next

            End If

            Return isInCurrentTemplate

        End Function

        Public Function CopyUploadSessionUploadTypeCollection() As BusinessObjectCollection

            Dim theOriginalUploadSessionUploadTypeCollection As New BusinessObjectCollection()
            Dim theUploadSessionUploadTypeCopy As UploadSessionUploadType

            For Each theUploadSessionUploadType As UploadSessionUploadType In
                    Me.UploadSessionUploadTypeCollection

                ' make a copy of the original
                theUploadSessionUploadTypeCopy = CType(theUploadSessionUploadType.GetNewCopy(), UploadSessionUploadType)

                ' got to set the parent - copy doesn't do this
                theUploadSessionUploadTypeCopy.UploadSession = theUploadSessionUploadType.UploadSession
                theUploadSessionUploadTypeCopy.UploadType = theUploadSessionUploadType.UploadType

                ' hold on to it to restore later
                theOriginalUploadSessionUploadTypeCollection.
                    Add(theUploadSessionUploadTypeCopy.PrimaryKey, theUploadSessionUploadTypeCopy)

            Next

            Return theOriginalUploadSessionUploadTypeCollection

        End Function

        Public Function IsRequiredForUploadType(ByVal inUploadTypeAttribute As UploadTypeAttribute) As System.Boolean

            Return (Me.IsNewItemSessionFlag And Not inUploadTypeAttribute.IsReadOnlyForNewItems And inUploadTypeAttribute.IsRequiredForUploadTypeForNewItems) Or
                (Not Me.IsNewItemSessionFlag And Not inUploadTypeAttribute.IsReadOnlyForExistingItems And inUploadTypeAttribute.IsRequiredForUploadTypeForExistingItems)

        End Function


        Public Function IsReadOnly(ByVal inUploadTypeAttribute As UploadTypeAttribute) As System.Boolean

            Return (Me.IsNewItemSessionFlag And inUploadTypeAttribute.IsReadOnlyForNewItems) Or
                (Not Me.IsNewItemSessionFlag And inUploadTypeAttribute.IsReadOnlyForExistingItems)

        End Function

#End Region

#Region "Overloaded CRUD Methods"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="loggedInUserId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Save(ByVal loggedInUserId As Integer) As Boolean

            ' a negative pk value means we need to do an insert
            If Me.UploadSessionID < 0 Then
                Me.CreatedByUserID = loggedInUserId
                Me.CreatedDateTime = DateTime.Now
                Me.IsModifiedByUserIDNull = True
                Me.IsModifiedDateTimeNull = True
                ' pull this from the app.config file
                Me.EmailToAddress = ConfigurationServices.AppSettings("ItemBulkLoad_ToEmailAddress")
            Else
                Me.ModifiedByUserID = loggedInUserId
                Me.ModifiedDateTime = DateTime.Now
            End If

            ' these are overriden and calculated
            ' and are not ever set so we need to
            ' manually set their IsXXXNull flags to
            ' false
            Me.IsItemsLoadedCountNull = False
            Me.IsItemsProcessedCountNull = False
            Me.IsErrorsCountNull = False

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim theTransaction As Transaction = Nothing

            Try

                theTransaction = TransactionManager.Instance.NewTransaction()
                ' delete marked for delete data
                Delete(True)
                Save()
                theTransaction.Commit()

            Catch ex As Exception
                If Not IsNothing(theTransaction) Then
                    theTransaction.Rollback()
                End If
                Throw ex
            End Try


        End Function

        Public Sub CascadeDelete()

            UploadSessionDAO.Instance.CascadeDeleteUploadSession(Me)

        End Sub

#End Region

#Region "Private Methods"

        ''' <summary>
        '''  Build the quick look up Hashtables
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub BuildQuickLookUpCollections()

            ' build the quick lookup collections for only the
            ' configured UploadAttributes for this UploadSession
            _uploadAttributesByKey = New BusinessObjectCollection()
            _uploadAttributesByName = New BusinessObjectCollection()

            For Each theUploadSessionUploadType As UploadSessionUploadType _
                    In Me.UploadSessionUploadTypeCollection

                Dim hasSelectedTemplate As Boolean = Not IsNothing(theUploadSessionUploadType.UploadTypeTemplate)

                ' this is a temp hack to fix a bug
                theUploadSessionUploadType.IsUploadTypeTemplateIDNull = theUploadSessionUploadType.UploadTypeTemplateID = 0

                If Not theUploadSessionUploadType.IsMarkedForDelete Then

                    ' loop through all the attributes for the UploadType
                    For Each theUploadTypeAttribute As UploadTypeAttribute _
                           In theUploadSessionUploadType.UploadType.UploadTypeAttributeCollection

                        If Not _uploadAttributesByKey.ContainsKey(theUploadTypeAttribute.UploadAttribute.Key.ToLower()) AndAlso
                                Not _uploadAttributesByName.ContainsKey(theUploadTypeAttribute.UploadAttribute.Name.ToLower()) Then

                            _uploadAttributesByKey.Add(theUploadTypeAttribute.UploadAttribute.Key.ToLower(),
                                theUploadTypeAttribute.UploadAttribute)
                            _uploadAttributesByName.Add(theUploadTypeAttribute.UploadAttribute.Name.ToLower(),
                                theUploadTypeAttribute.UploadAttribute)
                        End If
                    Next

                    ' End If
                End If
            Next

            ' build the quick lookup collections for all configured UploadAttributes
            _allUploadAttributesByID = New BusinessObjectCollection()
            _allUploadAttributesByName = New BusinessObjectCollection()

            For Each theUploadAttribute As UploadAttribute _
                    In UploadAttributeDAO.Instance.GetAllUploadAttributes()

                If Not _allUploadAttributesByName.ContainsKey(theUploadAttribute.Name.ToLower()) Then
                    _allUploadAttributesByID.Add(theUploadAttribute.UploadAttributeID,
                        theUploadAttribute)
                    _allUploadAttributesByName.Add(theUploadAttribute.Name.ToLower(),
                        theUploadAttribute)
                End If

            Next


        End Sub

#End Region

    End Class

End Namespace

