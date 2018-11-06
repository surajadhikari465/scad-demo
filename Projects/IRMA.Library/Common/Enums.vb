Namespace Common
    ''' <summary>
    ''' Thse are the store types we currently have in IRMA
    ''' </summary>
    ''' <remarks>
    ''' changing these values would mean changing the stored procedures 
    ''' that use them (for ease of maintanance there is a function that 
    ''' will return. this enum in table form: fn_GetEnumStoreTypes().  
    ''' For an example of a stored procedure that uses it check GetStoresByStoreType
    ''' </remarks>
    Public Module Enums
        Public Enum eStoreTypes
            WFM = 2
            Mega = 4
            DistributionCenter = 8
            ManufacturingFacility = 16
            WholeFoodsBusinessUnit = 32 'defined as FN_GetCustomerType() = 1 (which is defined as Store.Internal = 0 and BusinessUnit_ID not null)
            RegionalBusinessUnit = 64 'defined as FN_GetCustomerType() = 2 (which is defined as Store.Internal = 1 and BusinessUnit_ID not null)
            ExternalBusinessUnit = 128 'defined as FN_GetCustomerType() = 3 (which is defined as Store.Internal = 0 and BusinessUnit_ID is null)
        End Enum

        ''' <summary>
        ''' enum for "Extra List Items", used in some GetList factory methods <see cref=" SubTeamList.GetList" />. That is to say, 
        ''' if you wnat your List object to contain items like a Blank item, or an item called "NONE" or an item called "All", you would use this 
        ''' enum as a paramater in the GetList factory method.
        ''' </summary>
        ''' <remarks>
        ''' The reason these must be "powers of 2" is so we can do things like 
        ''' Dim NewList as SubTeamList = SubTeamList.GetList(eExtraListItems.None or eExtraListItems.All) to get a 
        ''' list of subteams that (in addition to the subteams) would have a "none" item and a "All Sub Teams" item.
        ''' </remarks>
        Public Enum eExtraListItems
            None = 2
            Blank = 4
            All = 8
        End Enum

        ''' <summary>
        ''' These are the types of Sub Teams we have in IRMA.  
        ''' </summary>
        ''' <remarks> These values must be powers of 2, and must match the eSubTeamType values returned by fn_GetEnumSubTeamTypes.
        ''' fn_GetEnumSubTeamTypes is used by the database to return a table that maps the "power of 2" enums to the 
        ''' value of SubTeamType in the database.  Changing any of these values would mean changing that function.  Adding a 
        ''' value means modifying this function to include the new value.
        ''' 
        ''' The reason these must be "powers of 2" is so we can do things like 
        ''' dim NewList as SubTeamList = SubTeamList.GetList(eSubTeamType.Supplies or eSubTeamType.Packaging) to get a 
        ''' list of all packaging and supplies subteam.</remarks>
        Public Enum eSubTeamTypes As Byte
            Retail = 2 'Value in the database = 1
            Manufacturing = 4 'Value in the database = 2
            RetailManufacturing = 8 'Value in the database = 3
            Expense = 16 'Value in the database = 4
            Packaging = 32 'Value in the database = 5
            Supplies = 64 'Value in the database = 6
            Front_End = 128 'Value in the database = 7
            AllSubTeams = 254 ' = eSubTeamType.Retail or eSubTeamType.Manufacturing or eSubTeamType.RetailManufacturing
            ' or eSubTeamType.Expense or eSubTeamType.Packaging or eSubTeamType.Supplies or eSubTeamType.Front_End
        End Enum

        Public Enum eVendorType As Byte
            ExternalVendor = 2 'defined as external to whole foods.
            InternalDCorManufacturer = 4 'defined as internal to whole foods and the region, and either a Distribution center or Manufacturer
            InternalStoreVendor = 8 'defined as internal to whole foods and the region, and not a Distribution center or Manufacturer
            WFMVendor = 16 'defined as Internal to whole foods, but external to the region
        End Enum

        Public Enum eAvgUnitWeightList As Byte
            OnlyIRMAItemsAssociatedWithRIPEItems = 0
            AllIRMAItems = 1
        End Enum

    End Module
End Namespace

