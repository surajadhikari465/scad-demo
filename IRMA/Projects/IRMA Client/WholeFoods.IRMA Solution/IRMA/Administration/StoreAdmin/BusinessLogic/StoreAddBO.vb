Imports WholeFoods.IRMA.Administration.StoreAdmin.DataAccess

Public Class StoreAddBO

#Region "Properties"
    Public Property StoreID As Integer
    Public Property StoreAbbr As String                ' Store Abbreviation
    Public Property StoreName As String
    Public Property StoreJurisdictionID As Integer         ' TaxJurisdictionID
    Public Property ZoneID As Integer                 ' Zone 
    Public Property TaxJurisdictionID As Integer         ' TaxJurisdictionID
    Public Property BusinessUnit_Id As Integer         ' Business Unit ID
    Public Property StoreStoreCount As Integer
    Public Property PlumStoreNo As Integer
    Public Property PSIStoreNo As Integer
    Public Property SourceStoreNo As Integer
    Public Property StoreLastUpdate As DateTime
    Public Property ISSPriceChgTypeID As Integer
    Public Property StoreSubTeamSubstitutions As String
    Public Property VendorName As String               ' name for store as vendor
    Public Property VendorAddress As String            ' New Store Address 
    Public Property VendorCity As String               ' New Store City
    Public Property VendorState As String              ' New Store State
    Public Property VendorZipCode As String            ' New Store Zip
    Public Property VendorCountry As String            ' New Store Country Code
    Public Property PeopleSoftVendorID As String       ' PeopleSoft Vendor Number
    Public Property IncSlim As Byte                    ' Include Slim entries in Cloning
    Public Property IncFutureSale As Byte              ' Include Future Sale entries in Cloning
    Public Property IncPromoPlanner As Byte            ' Include Promo Planner entries in Cloning
    Public Property GeoCode As String
    Public Property IsSourceStoreOnGpm As Boolean

#End Region
#Region "Subs and Functions"
    Public Overloads Shared Function NewStore() As StoreAddBO
        Dim ReturnStore As New StoreAddBO
        Return ReturnStore
    End Function
    Public Overloads Shared Function NewStore(ByVal nStoreID As Integer, ByVal nStoreAbbr As String, ByVal nStoreName As String, ByVal nStoreJurisdictionID As Integer,
    ByVal nZoneID As Integer, ByVal nTaxJurisdictionID As Integer, ByVal nBusinessUnit_Id As Integer, ByVal nPlumStoreNo As Integer, ByVal nPSIStoreNo As Integer,
    ByVal nSourceStoreNo As Integer, ByVal nISSPriceChgTypeID As Integer, ByVal nStoreSubTeamSubstitutions As String,
    ByVal nVendorName As String, ByVal nVendorAddress As String, ByVal nVendorCity As String, ByVal nVendorState As String, ByVal nVendorZipCode As String,
    ByVal nPeopleSoftVendorID As String, ByVal nIncSlim As Byte, ByVal nIncFutureSale As Byte, ByVal nIncPromoPlanner As Byte, ByVal nGeoCode As String,
    ByVal IsSourceStoreOnGpm As Boolean) _
        As StoreAddBO


        Dim ReturnValue As New StoreAddBO
        ReturnValue.StoreID = nStoreID                                 ' The new store no
        ReturnValue.StoreAbbr = nStoreAbbr                             ' Store Abbreviation
        ReturnValue.StoreName = nStoreName             ' Name of new store
        ReturnValue.StoreJurisdictionID = nStoreJurisdictionID         ' StoreJurisdictionID
        ReturnValue.ZoneID = nZoneID                                   ' Zone 
        ReturnValue.TaxJurisdictionID = nTaxJurisdictionID             ' TaxJurisdictionID
        ReturnValue.BusinessUnit_Id = nBusinessUnit_Id         ' Business Unit ID
        ReturnValue.PlumStoreNo = nPlumStoreNo           ' The PLUM store no
        ReturnValue.PSIStoreNo = nPSIStoreNo           ' The PLUM store no
        ReturnValue.SourceStoreNo = nSourceStoreNo           ' The source store no
        ReturnValue.ISSPriceChgTypeID = nISSPriceChgTypeID
        ReturnValue.StoreSubTeamSubstitutions = nStoreSubTeamSubstitutions               ' name for store as vendor
        ReturnValue.VendorName = nVendorName               ' name for store as vendor
        ReturnValue.VendorAddress = nVendorAddress            ' New Store Address 
        ReturnValue.VendorCity = nVendorCity               ' New Store City
        ReturnValue.VendorState = nVendorState              ' New Store State
        ReturnValue.VendorZipCode = nVendorZipCode            ' New Store Zip
        ReturnValue.PeopleSoftVendorID = nPeopleSoftVendorID       ' PeopleSoft Vendor Number
        ReturnValue.IncSlim = nIncSlim                   ' Include Slim entries in Cloning
        ReturnValue.IncFutureSale = nIncFutureSale              ' Include Future Sale entries in Cloning
        ReturnValue.IncPromoPlanner = nIncPromoPlanner
        ReturnValue.StoreStoreCount = 0
        ReturnValue.GeoCode = nGeoCode
        ReturnValue.IsSourceStoreOnGpm = IsSourceStoreOnGpm
        Return ReturnValue
    End Function

    Public Sub Save()
        Try
            StoreAddDAO.AddCloneStore(Me)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

End Class


Public Class SubTeamSubstitutionList
    Inherits System.Collections.CollectionBase

#Region "Subs and Functions"

    Public Shared Function NewList() As SubTeamSubstitutionList
        Dim ReturnList As New SubTeamSubstitutionList
        Return ReturnList
    End Function

    Private Sub New()

    End Sub


    Public Sub Remove(ByVal subStore As SubTeamSubstitutionInfo)

        Dim remItem As New SubTeamSubstitutionInfo
        For Each item As SubTeamSubstitutionInfo In Me.List
            If item.SubStoreID = subStore.SubStoreID And _
                item.SubStoreName = subStore.SubStoreName And _
                item.SubSubTeamID = subStore.SubSubTeamID And _
                item.SubSubTeamName = subStore.SubSubTeamName Then
                remItem = item
                Exit For
            End If
        Next
        List.Remove(remItem)
    End Sub

    Public Sub Add(ByVal subStore As SubTeamSubstitutionInfo)
        List.Add(subStore)
    End Sub

    Public Overrides Function ToString() As String
        Dim varSubTeamSubstitutionString As String = ""
        Dim RecordDelim As String
        RecordDelim = "|"
        For Each SubTeamSubstitutions As SubTeamSubstitutionInfo In Me
            varSubTeamSubstitutionString = varSubTeamSubstitutionString + SubTeamSubstitutions.ToString + RecordDelim
        Next
        Return varSubTeamSubstitutionString
    End Function
#End Region

End Class

Public Class SubTeamSubstitutionInfo

#Region "Properties"
    Public Property SubSubTeamID() As Integer
    Public Property SubSubTeamName() As String
    Public Property SubStoreID() As Integer
    Public Property SubStoreName() As String
#End Region
#Region "Subs and Functions"
    Public Sub New()
    End Sub
    Public Overrides Function ToString() As String
        Dim ColumnDelim As String
        ColumnDelim = ","
        Return SubStoreID.ToString + ColumnDelim + SubSubTeamID.ToString
    End Function

    Public Shared Function NewInfo(ByVal SubStore As SubStoreInfo, ByVal POSSubteam As POSSubteamInfo) As SubTeamSubstitutionInfo
        Dim ReturnInfo As New SubTeamSubstitutionInfo
        ReturnInfo.SubSubTeamID = POSSubteam.POSSubteamID
        ReturnInfo.SubSubTeamName = POSSubteam.POSSubteamName
        ReturnInfo.SubStoreID = SubStore.SubStoreID
        ReturnInfo.SubStoreName = SubStore.SubStoreName
        Return ReturnInfo
    End Function
#End Region

End Class

Public Class SubStoreInfo

#Region "Propeties"
    Public Property SubStoreID() As Integer
    Public Property SubStoreName() As String
#End Region
#Region "Subs and Functions"
    Friend Sub New()
    End Sub
#End Region

End Class

Public Class POSSubteamInfo

#Region "Properties"
    Public Property POSSubteamID() As Integer
    Public Property POSSubteamName() As String
#End Region
#Region "Subs and Functions"
    Friend Sub New()
    End Sub
#End Region

End Class