Imports WholeFoods.IRMA.Administration.Common.DataAccess
Imports WholeFoods.IRMA.Administration.POSPush.BusinessLogic
Imports WholeFoods.IRMA.Administration.POSPush.DataAccess
Imports WholeFoods.IRMA.Replenishment.Common.DataAccess
Imports WholeFoods.Utility

Namespace WholeFoods.IRMA.Administration.Common.BusinessLogic
    Public Enum RegionBOStatus
        Valid
        Error_Required_CorpScaleWriter
        Error_Required_ZoneScaleWriter
    End Enum

    Public Class RegionBO
#Region "Property Definitions"
        Private _primaryRegionName As String
        Private _primaryRegionCode As String
        Private _corpScaleWriter As New StoreScaleConfigBO
        Private _zoneScaleWriter As New StoreScaleConfigBO
        Private _regionalStore As StoreBO = Nothing
        Private _useRegionalScaleFlag As Boolean
#End Region

#Region "constructors and helper methods to initialize the data"
        ''' <summary>
        ''' Create a new instance of the RegionBO, populating it with the
        ''' regional configuration instance data and scale writer settings.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            ' Populate the object with the regional instance data
            RegionDAO.GetRegionalInstanceData(Me)

            ' Get the regional store information
            _regionalStore = RegionDAO.GetRegionalOfficeStore()

            ' Populate the object with the scale writer data
            RefreshScaleConfigurations()

        End Sub
#End Region

#Region "Create and Update Methods"
        ''' <summary>
        ''' Refresh the scale configuration settings for the regional office.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub RefreshScaleConfigurations()
            ' Make sure the regional office store data has been set
            If _regionalStore Is Nothing Then
                _regionalStore = RegionDAO.GetRegionalOfficeStore()
            End If

            ' Retreive the store scale configurations for the regional store
            _corpScaleWriter = StoreWriterConfigDAO.GetStoreScaleConfiguration(_regionalStore.StoreNo, POSWriterBO.SCALE_WRITER_TYPE_CORPORATE)
            ' Does the region use SmartX for the Zone Pricing?  If so, this is a different ScaleWriterType
            If InstanceDataDAO.IsFlagActive("UseSmartXPriceData") Then
                _zoneScaleWriter = StoreWriterConfigDAO.GetStoreScaleConfiguration(_regionalStore.StoreNo, POSWriterBO.SCALE_WRITER_TYPE_SMARTX_ZONE)
            Else
                _zoneScaleWriter = StoreWriterConfigDAO.GetStoreScaleConfiguration(_regionalStore.StoreNo, POSWriterBO.SCALE_WRITER_TYPE_ZONE)
            End If
        End Sub
#End Region

#Region "Business rules"

        ''' <summary>
        ''' performs data validation and returns array of POSWriterBOStatus values
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidateRegionData() As ArrayList
            Dim statusList As New ArrayList

            'if the regional scale hosting option is selected, both file writers must be defined
            If InstanceDataDAO.IsFlagActive("UseRegionalScaleFile") Then
                If Me.CorpScaleWriter.ScaleFileWriterKey Is Nothing Or (Me.CorpScaleWriter.ScaleFileWriterKey IsNot Nothing AndAlso Me.CorpScaleWriter.ScaleFileWriterKey.Trim.Equals("")) Then
                    statusList.Add(RegionBOStatus.Error_Required_CorpScaleWriter)
                End If

                If Me.ZoneScaleWriter.ScaleFileWriterKey Is Nothing Or (Me.ZoneScaleWriter.ScaleFileWriterKey IsNot Nothing AndAlso Me.ZoneScaleWriter.ScaleFileWriterKey.Trim.Equals("")) Then
                    statusList.Add(RegionBOStatus.Error_Required_ZoneScaleWriter)
                End If
            End If

            If statusList.Count = 0 Then
                'data is valid
                statusList.Add(RegionBOStatus.Valid)
            End If

            Return statusList
        End Function

#End Region

#Region "Property access methods"
        Public Property PrimaryRegionName() As String
            Get
                Return _primaryRegionName
            End Get
            Set(ByVal value As String)
                _primaryRegionName = value
            End Set
        End Property

        Public Property PrimaryRegionCode() As String
            Get
                Return _primaryRegionCode
            End Get
            Set(ByVal value As String)
                _primaryRegionCode = value
            End Set
        End Property

        Public Property CorpScaleWriter() As StoreScaleConfigBO
            Get
                Return _corpScaleWriter
            End Get
            Set(ByVal value As StoreScaleConfigBO)
                _corpScaleWriter = value
            End Set
        End Property

        Public Property ZoneScaleWriter() As StoreScaleConfigBO
            Get
                Return _zoneScaleWriter
            End Get
            Set(ByVal value As StoreScaleConfigBO)
                _zoneScaleWriter = value
            End Set
        End Property

        Public Property RegionalStore() As StoreBO
            Get
                Return _regionalStore
            End Get
            Set(ByVal value As StoreBO)
                _regionalStore = value
            End Set
        End Property

        Public Property UseRegionalScaleFlag() As Boolean
            Get
                Return _useRegionalScaleFlag
            End Get
            Set(ByVal value As Boolean)
                _useRegionalScaleFlag = value
            End Set
        End Property

#End Region

    End Class
End Namespace
