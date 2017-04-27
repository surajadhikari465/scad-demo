
Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ModelLayer.DataAccess
Imports WholeFoods.IRMA.Common.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.BusinessLogic

    ''' <summary>
    ''' Business Object for the UploadAttribute db table.
    '''
    ''' This class inherits persistent properties from the regenerable base class.
    ''' These properties map one-to-one to columns in the UploadAttribute db table.
    '''
    ''' MODIFY THIS CLASS, NOT THE BASE CLASS.
    '''
    ''' Created By:	David Marine
    ''' Created   :	Feb 12, 2007
    ''' </summary>
    ''' <remarks></remarks>
    Public Class UploadAttribute
        Inherits UploadAttributeRegen


#Region "Overriden Fields and Properties"

        Private _uploadValueCollection As BusinessObjectCollection = Nothing

        ''' <summary>
        ''' Gets and sets this uploadAttribute's collection of UploadValues.
        ''' This is a critical override of the base's corresponding property.
        ''' It is done because we never want an UploadAttribute to go out and get
        ''' its collection of UploadValues which would be a very large number.
        ''' </summary>
        ''' <value></value>
        ''' <returns>BusinessObjectCollection</returns>
        ''' <remarks></remarks>
        Public Overrides Property UploadValueCollection() As BusinessObjectCollection
            Get
                If IsNothing(_uploadValueCollection) Then
                    _uploadValueCollection = New BusinessObjectCollection()

                    ' assign this Business Object as the children's parent
                    For Each theUploadValue As UploadValue In _
                      _uploadValueCollection
                        theUploadValue.UploadAttribute = CType(Me, UploadAttribute)
                    Next

                End If
                Return _uploadValueCollection
            End Get
            Set(ByVal value As BusinessObjectCollection)
                _uploadValueCollection = value
            End Set
        End Property


        ''' <summary>
        ''' Overriden to allow regional overriding of the IsRequiredValue
        ''' flag value in the EIM configuration.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Property IsRequiredValue() As System.Boolean
            Get
                Dim _isRequiredValue As Boolean = MyBase.IsRequiredValue

                Return _isRequiredValue AndAlso IsRequiredValueForRegion()
            End Get
            Set(ByVal value As System.Boolean)

                MyBase.IsRequiredValue = value
            End Set
        End Property


#End Region

#Region "New Fields and Properties"

        ''' <summary>
        ''' Returns a concatonation of the table and column names to which the
        ''' attribute maps.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Key() As String
            Get
                Return Me.TableName.ToLower() + "_" + Me.ColumnNameOrKey.ToLower()
            End Get
        End Property

        Public ReadOnly Property IsNumeric() As Boolean
            Get
                Return Me.DbDataType.ToLower().Equals("int") Or _
                    Me.DbDataType.ToLower().Equals("tinyint") Or _
                    Me.DbDataType.ToLower().Equals("decimal") Or _
                    Me.DbDataType.ToLower().Equals("money") Or _
                    Me.DbDataType.ToLower().Equals("smallmoney")
            End Get
        End Property

        Public ReadOnly Property IsValueListControl() As Boolean
            Get
                Return Me.ControlType.ToLower().Equals("valuelist")
            End Get
        End Property

        Public ReadOnly Property IsJurisdictionAttribute() As Boolean
            Get
                Dim localIsJurisdictionAttribute As Boolean = False

                For Each theJurisdictionAttributeKey As String In EIM_Constants.JURISDICTION_ATTR_KEY_ARRAY

                    If Me.Key.Equals(theJurisdictionAttributeKey) Then
                        localIsJurisdictionAttribute = True
                        Exit For
                    End If
                Next

                Return localIsJurisdictionAttribute
            End Get
        End Property

#End Region

#Region "Constructors"

        Public Sub New()
            MyBase.New()
        End Sub


#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Returns true if the UploadTypeAttribute is allowed based
        ''' on region-specific business logic.
        ''' </summary>
        ''' <remarks></remarks>
        Public Function IsAllowedForRegion() As Boolean

            Dim localIsAllowedForRegion As Boolean = True

            Dim useFourLevelHierarchy As Boolean = InstanceDataDAO.IsFlagActiveCached("FourLevelHierarchy")

            ' only allow level 3 and level 4 if appropriate for the region
            localIsAllowedForRegion = _
                    useFourLevelHierarchy Or _
                    (Not useFourLevelHierarchy And _
                    (Not Me.Key.ToLower().Equals(EIM_Constants.ITEM_LEVEL_3_ATTR_KEY) And _
                    Not Me.Key.ToLower().Equals(EIM_Constants.ITEM_LEVEL_4_ATTR_KEY)))

            If localIsAllowedForRegion Then

                Dim useStoreJurisdictions As Boolean = InstanceDataDAO.IsFlagActiveCached("UseStoreJurisdictions")

                ' only allow if appropriate for the region
                localIsAllowedForRegion = _
                        useStoreJurisdictions Or _
                        (Not useStoreJurisdictions And _
                        (Not Me.Key.ToLower().Equals(EIM_Constants.ITEM_JURISDICTION_ID_ATTR_KEY) And _
                        Not Me.Key.ToLower().Equals(EIM_Constants.ITEM_IS_DEFAULT_JURISDICTION_ATTR_KEY)))
            End If

            Return localIsAllowedForRegion

        End Function

        ''' <summary>
        ''' Allows regional overriding of the IsRequiredValue
        ''' flag value in the EIM configuration.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function IsRequiredValueForRegion() As System.Boolean

            If IsKeyEqual(EIM_Constants.ITEM_LABELTYPE_ID_ATTR_KEY) AndAlso _
                    InstanceDataDAO.IsFlagActiveCached("Required_LabelType") Then

                Return False
            ElseIf IsKeyEqual(EIM_Constants.ITEM_MANAGED_BY_ID) AndAlso _
                    InstanceDataDAO.IsFlagActiveCached("Required_ManagedBy") Then

                Return False
            Else
                Return True
            End If

        End Function
        Public Function IsKeyEqual(ByVal inKey As String) As Boolean

            Return Object.Equals(Me.Key.ToLower(), inKey.ToLower())

        End Function

#End Region

#Region "Shared Methods"

        Public Shared Function MapFromDbDataType(ByRef dbType As String) As System.Type

            Dim systemType As System.Type = Nothing

            Select Case dbType.ToLower()
                Case "char"
                    systemType = System.Type.GetType("System.String")
                Case "varchar"
                    systemType = System.Type.GetType("System.String")
                Case "bit"
                    systemType = System.Type.GetType("System.Boolean")
                Case "decimal"
                    systemType = System.Type.GetType("System.Double")
                Case "money"
                    systemType = System.Type.GetType("System.Double")
                Case "smallmoney"
                    systemType = System.Type.GetType("System.Double")
                Case "smalldatetime"
                    systemType = System.Type.GetType("System.DateTime")
                Case "datetime"
                    systemType = System.Type.GetType("System.DateTime")
                Case "int"
                    systemType = System.Type.GetType("System.Int32")
                Case "smallint"
                    systemType = System.Type.GetType("System.Int32")
                Case "tinyint"
                    systemType = System.Type.GetType("System.Int32")
            End Select

            Return systemType

        End Function

        Public Shared Function IsDbDataTypeNumeric(ByRef dbType As String) As Boolean

            Dim isNumeric As Boolean = False

            Select Case dbType.ToLower()
                Case "decimal"
                    isNumeric = True
                Case "money"
                    isNumeric = True
                Case "smallmoney"
                    isNumeric = True
                Case "int"
                    isNumeric = True
                Case "smallint"
                    isNumeric = True
                Case "tinyint"
                    isNumeric = True
            End Select

            Return isNumeric

        End Function

#End Region

    End Class

End Namespace

