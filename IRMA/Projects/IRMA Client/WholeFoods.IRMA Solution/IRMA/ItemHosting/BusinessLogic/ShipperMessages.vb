Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic

    Public Class ShipperMessages

#Region "Captions, Titles"

        Public Const CAPTION_SHIPPER_VIEW_ITEMS = "View Shipper Items"
        Public Const CAPTION_SHIPPER_ADD_ITEM = "Add Item to Shipper"
        Public Const CAPTION_SHIPPER_EDIT_ITEM = "Edit Item in Shipper"
        Public Const CAPTION_SHIPPER_DELETE_ITEM = "Delete Item from Shipper"
        Public Const CAPTION_SHIPPER_CONVERT_FROM_ITEM As String = "Convert Item to Shipper"
        Public Const CAPTION_SHIPPER_RESET_PACK_INFO As String = "Reset Package Quantity and Size"
        Public Const CAPTION_SHIPPER_CONVERT_TO_ITEM As String = "Convert Shipper to Item"
        Public Const CAPTION_SHIPPER_SAVE As String = "Save Item as a Shipper"
        Public Const CAPTION_SHIPPERITEM_SAVE As String = "Save Quantity for Item in a Shipper"

        Public Const SHIPPER_LIST_WINDOW_TITLE = "Shipper Details"

#End Region

#Region "Info Messages"

        Public Const INFO_SCREEN_ACTION_SELECT_ONE_ITEM As String = "Please select one item and try again."

#End Region

#Region "Confirmation Messages"

        Public Const CONFIRM_SHIPPER_RESET_PACK_INFO As String = "This will reset the package quantity and size." & vbCrLf & "Do you want to continue?"
        Public Const CONFIRM_SHIPPER_DELETE_ITEM As String = "This will remove the '{0}' ({1}) item from this Shipper." & vbCrLf & "Do you want to continue?"

#End Region

#Region "Shipper Error Messages"

        Public Const ERROR_SHIPPER_ITEM_KEY_LESS_THAN_ONE As String = "The specified item key is invalid because it is less than 1."
        Public Const ERROR_SHIPPER_CANNOT_BUILD_INVALID_IDENTIFIER As String = "The item identifier for a Shipper cannot be empty."
        Public Const ERROR_SHIPPER_CANNOT_BUILD_INVALID_DESC As String = "The item description for a Shipper cannot be empty."
        Public Const ERROR_SHIPPER_CANNOT_BUILD_INVALID_NON_SHIPPER As String = "The specified item is not a Shipper."
        Public Const ERROR_SHIPPER_CANNOT_BUILD_MISSING_SHIPPER_UOM As String = "The 'Shipper' UOM was not found and must be added before Shippers can be utilized."
        Public Const ERROR_SHIPPER_CANNOT_BUILD_MISSING_UNIT_UOM As String = "The 'Unit' UOM was not found and must be added before Shippers can be utilized."
        Public Const ERROR_SHIPPER_CANNOT_BUILD_DATA_NOT_FOUND As String = "No Shipper information was found."
        Public Const ERROR_SHIPPER_CANNOT_BUILD_DB_ERROR As String = "An error occurred while retrieving Shipper information."
        Public Const ERROR_SHIPPER_ITEM_EXISTS_IN_A_SHIPPER As String = "This item exists in one or more Shippers."
        Public Const ERROR_SHIPPER_CANNOT_ADD_ITEM_IS_SHIPPER As String = "This item cannot be added to a Shipper because it is a Shipper."
        Public Const ERROR_SHIPPER_CONVERT_TO_ITEM_SHIPPER_CONTAINS_ITEMS As String = "This Shipper cannot be converted to a normal item until all items in the Shipper are removed."
        Public Const ERROR_SHIPPER_ALREADY_CONTAINS_ITEM = "This item cannot be added because it already exists in the Shipper."
        Public Const ERROR_SHIPPER_DELETE_ITEM_NOT_IN_SHIPPER As String = "Cannot remove item from Shipper because it does not exist."
        Public Const ERROR_SHIPPER_CANNOT_SAVE_IF_EMPTY As String = "This Shipper does not contain any items.  Please add the items using the 'Shipper Items' button."
        Public Const WARN_SHIPPER_ITEM_NOT_SHIPPER_AFTER_ITEMS_BTN_CLICKED As String = "Shipper-Item button was clicked, but after data refresh, item was not a Shipper."
        Public Const ERROR_SHIPPER_COULD_NOT_UPDATE_PACK_INFO As String = "An error occurred while attempting to update the package information for this Shipper item to reflect its contents."
        Public Const ERROR_SHIPPER_ADD_ITEM_DB_ERROR As String = "An error occurred while adding an item to the Shipper."
        Public Const ERROR_SHIPPER_ADD_ITEM_OUTPUT As String = "Expected results from the add-item process were not found.  Please verify Shipper items and try again."

#End Region

#Region "Shipper-Item Messages"

        Public Const ERROR_SHIPPERITEM_INVALID_IDENTIFIER As String = "The item identifier for an item within a Shipper cannot be empty."
        Public Const ERROR_SHIPPERITEM_INVALID_DESC As String = "The item description for an item within a Shipper cannot be empty."
        Public Const ERROR_SHIPPERITEM_ITEM_KEY_LESS_THAN_ONE As String = "The item key for this Shipper-Item is invalid because it is less than one."
        Public Const ERROR_SHIPPERITEM_IDENTIFIER_TOO_LONG As String = "The identifier for this Shipper-Item is too long."
        Public Const ERROR_SHIPPERITEM_QTY_LESS_THAN_ONE As String = "The quantity for this Shipper-Item is invalid because it is less than one."
        Public Const ERROR_SHIPPERITEM_DURING_UPDATE_INFO As String = "An error occurred while attempting to update the information for this item in this Shipper."
        Public Const ERROR_SHIPPERITEM_SCREEN_NO_SHIPPERITEM_OBJ As String = "The Shipper-Item edit screen cannot be displayed because an internal object was not initialized."
        Public Const ERROR_SHIPPERITEM_SCREEN_LOAD As String = "The Shipper-Item edit screen cannot be displayed."

        Public Const CONFIRM_SHIPPERITEM_REENTER_QTY As String = "Do you want to re-enter the quantity?"

#End Region

    End Class

End Namespace
