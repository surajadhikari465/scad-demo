
Namespace WholeFoods.IRMA.ModelLayer.BusinessLogic


    Public Class EIM_Constants

#Region "Attribute Key Constants"

        Public Const ITEM_MAINTENANCE_CODE As String = "ITEM_MAINTENANCE"
        Public Const PRICE_UPLOAD_CODE As String = "PRICE_UPLOAD"
        Public Const COST_UPLOAD_CODE As String = "COST_UPLOAD"

        Public Const ITEM_KEY_COLUMN_NAME As String = "item_key"
        Public Const ITEM_KEY_ATTR_KEY As String = "item_item_key"
        Public Const ITEM_SUBTEAM_NO_ATTR_KEY As String = "item_subteam_no"
        Public Const ITEM_CATEGORY_ID_ATTR_KEY As String = "item_category_id"
        Public Const ITEM_LEVEL_3_ATTR_KEY As String = "calculated_prodhierarchylevel3_id"
        Public Const ITEM_LEVEL_4_ATTR_KEY As String = "item_prodhierarchylevel4_id"

        Public Const ITEM_IS_DEFAULT_JURISDICTION_ATTR_KEY As String = "item_isdefaultjurisdiction"
        Public Const ITEM_JURISDICTION_ID_ATTR_KEY As String = "item_storejurisdictionid"

        ' these are jurisdiction specific attributes
        Public Const ITEM_ITEM_DESCRIPTION_ATTR_KEY As String = "item_item_description"
        Public Const ITEM_SIGN_DESCRIPTION_ATTR_KEY As String = "item_sign_description"
        Public Const ITEM_PACKAGE_DESC1_ATTR_KEY As String = "item_package_desc1"
        Public Const ITEM_PACKAGE_DESC2_ATTR_KEY As String = "item_package_desc2"
        Public Const ITEM_PACKAGE_UNIT_ID_ATTR_KEY As String = "item_package_unit_id"
        Public Const ITEM_RETAIL_UNIT_ID_ATTR_KEY As String = "item_retail_unit_id"
        Public Const ITEM_VENDOR_UNIT_ID_ATTR_KEY As String = "item_vendor_unit_id"
        Public Const ITEM_DISTRIBUTION_UNIT_ID_ATTR_KEY As String = "item_distribution_unit_id"
        Public Const ITEM_POS_DESCRIPTION_ATTR_KEY As String = "item_pos_description"
        Public Const ITEM_RETAIL_SALE As String = "item_retail_sale"
        Public Const ITEM_BRAND_ID_ATTR_KEY As String = "item_brand_id"
        Public Const ITEM_TAXCLASS_ID_ATTR_KEY As String = "item_taxclassid"
        Public Const ITEM_FOOD_STAMPS_ATTR_KEY As String = "item_food_stamps"
        Public Const ITEM_DISCOUNTABLE_ATTR_KEY As String = "item_discountable"
        Public Const ITEM_PRICE_REQUIRED_ATTR_KEY As String = "item_price_required"
        Public Const ITEM_QUANTITY_REQUIRED_ATTR_KEY As String = "item_quantity_required"
        Public Const ITEM_MANUFACTURING_UNIT_ID_ATTR_KEY As String = "item_manufacturing_unit_id"
        Public Const ITEM_QTYPROHIBIT_ATTR_KEY As String = "item_qtyprohibit"
        Public Const ITEM_GROUPLIST_ATTR_KEY As String = "item_grouplist"
        Public Const ITEM_CASE_DISCOUNT_ATTR_KEY As String = "item_case_discount"
        Public Const ITEM_COUPON_MULTIPLIER_ATTR_KEY As String = "item_coupon_multiplier"
        Public Const ITEM_MISC_TRANSACTION_SALE_ATTR_KEY As String = "item_misc_transaction_sale"
        Public Const ITEM_MISC_TRANSACTION_REFUND_ATTR_KEY As String = "item_misc_transaction_refund"
        Public Const ITEM_ICE_TARE_ATTR_KEY As String = "item_ice_tare"
        Public Const ITEM_LABELTYPE_ID_ATTR_KEY As String = "item_labeltype_id"
        Public Const ITEM_MANAGED_BY_ID As String = "item_managed_by_id"
        Public Const ITEM_COSTEDBYWEIGHT As String = "item_costedbyweight"
        Public Const ITEM_COUNTRYOFPROC_ATTR_KEY As String = "item_countryproc_id"
        Public Const ITEM_FSAELIGIBLE_ATTR_KEY As String = "item_fsa_eligible"
        Public Const ITEM_INGREDIENT_ATTR_KEY As String = "item_ingredient"
        Public Const ITEM_LOCKAUTH_ATTR_KEY As String = "item_lockauth"
        Public Const ITEM_NOTAVAILABLE_ATTR_KEY As String = "item_not_available"
        Public Const ITEM_NOTAVAILABLENOTE_ATTR_KEY As String = "item_not_availablenote"
        Public Const ITEM_ORIGIN_ATTR_KEY As String = "item_origin_id"
        Public Const ITEM_PRODUCTCODE_ATTR_KEY As String = "item_product_code"
        Public Const ITEM_SUSTAINABILITYRANKINGREQUIRED_ATTR_KEY As String = "item_sustainabilityrankingrequired"
        Public Const ITEM_SUSTAINABILITYRANKING_ATTR_KEY As String = "item_sustainabilityrankingid"
        Public Const ITEM_UNITPRICECATEGORY_ATTR_KEY As String = "item_unit_price_category"
        Public Const ITEM_ORGANIC_ATTR_KEY As String = "item_organic"

        Public Const ITEM_AVERAGE_UNIT_WEIGHT As String = "item_average_unit_weight"

        Public Const ITEMIDENTIFIER_IDENTIFIER_ATTRIBUTE_NAME As String = "identifier"
        Public Const ITEMIDENTIFIER_IDENTIFIER_ATTR_KEY As String = "itemidentifier_identifier"
        Public Const ITEMIDENTIFIER_IS_SCALE_IDENT_ATTR_KEY As String = "itemidentifier_scale_identifier"
        Public Const ITEMIDENTIFIER_IDENTIFIERTYPE_ATTR_KEY As String = "itemidentifier_identifiertype"
        Public Const ITEMIDENTIFIER_NUM_DIGITS_ATTR_KEY As String = "itemidentifier_numpludigitssenttoscale"
        Public Const ITEM_CHAINS_ATTR_KEY As String = "calculated_itemchains"
        Public Const UPLOADROW_ID_COLUMN_NAME As String = "uploadrow_id"

        Public Const ITEM_KEY_ATTRIBUTE_NAME As String = "item key"

        Public Const IS_IDENTICAL_ITEM_ROW_COLUMN_NAME As String = "identical_item_row"
        Public Const IS_HIDDEN_COLUMN_NAME As String = "is_hidden_row"

        ' scale data attribute keys
        Public Const ITEM_SCALEDESC1_ATTR_KEY As String = "itemscale_scale_description1"
        Public Const ITEM_SCALEDESC2_ATTR_KEY As String = "itemscale_scale_description2"
        Public Const ITEM_SCALEDESC3_ATTR_KEY As String = "itemscale_scale_description3"
        Public Const ITEM_SCALEDESC4_ATTR_KEY As String = "itemscale_scale_description4"
        Public Const ITEMSCALE_NUTRIFACT_ID_ATTR_KEY As String = "itemscale_nutrifact_id"
        Public Const ITEMSCALE_SCALE_EXTRATEXT_LABEL_TYPE_ATTR_KEY As String = "scale_extratext_scale_labeltype_id"
        Public Const ITEMSCALE_SCALE_EXTRATEXT_ID_ATTR_KEY As String = "itemscale_scale_extratext_id"
        Public Const ITEMSCALE_FORCEDTARE_ATTR_KEY As String = "itemscale_forcetare"
        Public Const ITEMSCALE_SCALE_TARE_ID_ATTR_KEY As String = "itemscale_scale_tare_id"
        Public Const ITEMSCALE_SCALE_ALTERNATE_TARE_ID_ATTR_KEY As String = "itemscale_scale_alternate_tare_id"
        Public Const ITEMSCALE_SCALE_LABELSTYLE_ID_ATTR_KEY As String = "itemscale_scale_labelstyle_id"
        Public Const ITEMSCALE_SCALE_EATBY_ID_ATTR_KEY As String = "itemscale_scale_eatby_id"
        Public Const ITEMSCALE_SCALE_GRADE_ID_ATTR_KEY As String = "itemscale_scale_grade_id"
        Public Const ITEMSCALE_SHELFLIFE_LENGTH_ATTR_KEY As String = "itemscale_shelflife_length"
        Public Const ITEMSCALE_SCALE_RANDOMWEIGHTTYPE_ID_ATTR_KEY As String = "itemscale_scale_randomweighttype_id"
        Public Const ITEMSCALE_SCALE_SCALEUOMUNIT_ID_ATTR_KEY As String = "itemscale_scale_scaleuomunit_id"
        Public Const ITEMSCALE_SCALE_FIXEDWEIGHT_ATTR_KEY As String = "itemscale_scale_fixedweight"
        Public Const ITEMSCALE_SCALE_BYCOUNT_ATTR_KEY As String = "itemscale_scale_bycount"
        Public Const ITEMSCALE_PRINTBLANKSHELFLIFE_ATTR_KEY As String = "itemscale_printblankshelflife"
        Public Const ITEMSCALE_PRINTBLANKPACKDATE_ATTR_KEY As String = "itemscale_printblankpackdate"
        Public Const ITEMSCALE_PRINTBLANKUNITPRICE_ATTR_KEY As String = "itemscale_printblankunitprice"
        Public Const ITEMSCALE_PRINTBLANKEATBY_ATTR_KEY As String = "itemscale_printblankeatby"
        Public Const ITEMSCALE_PRINTBLANKWEIGHT_ATTR_KEY As String = "itemscale_printblankweight"
        Public Const ITEMSCALE_PRINTBLANKTOTALPRICE_ATTR_KEY As String = "itemscale_printblanktotalprice"
        Public Const SCALE_EXTRATEXT_EXTRATEXT As String = "scale_extratext_extratext"
        Public Const SCALE_STORAGEDATA_STORAGEDATA As String = "scale_storagedata_storagedata"
        Public Const SCALE_ALLERGEN_ALLERGENS As String = "scale_allergen_allergens"
        Public Const SCALE_INGREDIENT_INGREDIENTS As String = "scale_ingredient_ingredients"

        ' These are the item and item scale attributes with jurisdictional overrides.
        ' Updated to include new 4.8 override values.
        Public Shared JURISDICTION_ATTR_KEY_ARRAY As String() = New String() {EIM_Constants.ITEM_IS_DEFAULT_JURISDICTION_ATTR_KEY,
                            EIM_Constants.ITEM_JURISDICTION_ID_ATTR_KEY,
                            EIM_Constants.ITEM_ITEM_DESCRIPTION_ATTR_KEY,
                            EIM_Constants.ITEM_POS_DESCRIPTION_ATTR_KEY,
                            EIM_Constants.ITEM_SIGN_DESCRIPTION_ATTR_KEY,
                            EIM_Constants.ITEM_PACKAGE_DESC1_ATTR_KEY,
                            EIM_Constants.ITEM_PACKAGE_DESC2_ATTR_KEY,
                            EIM_Constants.ITEM_PACKAGE_UNIT_ID_ATTR_KEY,
                            EIM_Constants.ITEM_RETAIL_UNIT_ID_ATTR_KEY,
                            EIM_Constants.ITEM_DISTRIBUTION_UNIT_ID_ATTR_KEY,
                            EIM_Constants.ITEM_VENDOR_UNIT_ID_ATTR_KEY,
                            EIM_Constants.ITEM_FOOD_STAMPS_ATTR_KEY,
                            EIM_Constants.ITEM_DISCOUNTABLE_ATTR_KEY,
                            EIM_Constants.ITEM_PRICE_REQUIRED_ATTR_KEY,
                            EIM_Constants.ITEM_QUANTITY_REQUIRED_ATTR_KEY,
                            EIM_Constants.ITEM_MANUFACTURING_UNIT_ID_ATTR_KEY,
                            EIM_Constants.ITEM_QTYPROHIBIT_ATTR_KEY,
                            EIM_Constants.ITEM_CASE_DISCOUNT_ATTR_KEY,
                            EIM_Constants.ITEM_COUPON_MULTIPLIER_ATTR_KEY,
                            EIM_Constants.ITEM_MISC_TRANSACTION_SALE_ATTR_KEY,
                            EIM_Constants.ITEM_ICE_TARE_ATTR_KEY,
                            EIM_Constants.ITEM_SCALEDESC1_ATTR_KEY,
                            EIM_Constants.ITEM_SCALEDESC2_ATTR_KEY,
                            EIM_Constants.ITEM_SCALEDESC3_ATTR_KEY,
                            EIM_Constants.ITEM_SCALEDESC4_ATTR_KEY,
                            EIM_Constants.ITEMSCALE_SCALE_EXTRATEXT_LABEL_TYPE_ATTR_KEY,
                            EIM_Constants.SCALE_EXTRATEXT_EXTRATEXT,
                            EIM_Constants.ITEMSCALE_SCALE_TARE_ID_ATTR_KEY,
                            EIM_Constants.ITEMSCALE_SCALE_SCALEUOMUNIT_ID_ATTR_KEY,
                            EIM_Constants.ITEMSCALE_SCALE_RANDOMWEIGHTTYPE_ID_ATTR_KEY,
                            EIM_Constants.ITEMSCALE_SCALE_FIXEDWEIGHT_ATTR_KEY,
                            EIM_Constants.ITEMSCALE_SCALE_BYCOUNT_ATTR_KEY,
                            EIM_Constants.ITEMSCALE_SHELFLIFE_LENGTH_ATTR_KEY,
                            EIM_Constants.ITEM_BRAND_ID_ATTR_KEY,
                            EIM_Constants.ITEM_COSTEDBYWEIGHT,
                            EIM_Constants.ITEM_COUNTRYOFPROC_ATTR_KEY,
                            EIM_Constants.ITEM_FSAELIGIBLE_ATTR_KEY,
                            EIM_Constants.ITEM_INGREDIENT_ATTR_KEY,
                            EIM_Constants.ITEM_LABELTYPE_ID_ATTR_KEY,
                            EIM_Constants.ITEM_LOCKAUTH_ATTR_KEY,
                            EIM_Constants.ITEM_NOTAVAILABLE_ATTR_KEY,
                            EIM_Constants.ITEM_NOTAVAILABLENOTE_ATTR_KEY,
                            EIM_Constants.ITEM_ORIGIN_ATTR_KEY,
                            EIM_Constants.ITEM_PRODUCTCODE_ATTR_KEY,
                            EIM_Constants.ITEM_SUSTAINABILITYRANKING_ATTR_KEY,
                            EIM_Constants.ITEM_SUSTAINABILITYRANKINGREQUIRED_ATTR_KEY,
                            EIM_Constants.ITEM_UNITPRICECATEGORY_ATTR_KEY}


        Public Const PROMO_PLANNER_IS_FOR_PROMO_PLANNER_ATTR_KEY As String = "calculated_isforpromoplanner"
        Public Const PROMO_PLANNER_PROJUNITS As String = "calculated_projunits"
        Public Const PROMO_PLANNER_COMMENT1 As String = "calculated_comment1"
        Public Const PROMO_PLANNER_COMMENT2 As String = "calculated_comment2"
        Public Const PROMO_PLANNER_BILLBACK As String = "calculated_billback"

        Public Const STORE_NO_ATTR_KEY As String = "store_store_no"
        Public Const STORE_NAME_ATTR_KEY As String = "store_store_name"

        Public Const PRICE_SALE_START_DATE_ATTR_KEY As String = "price_sale_start_date"
        Public Const PRICE_SALE_END_DATE_ATTR_KEY As String = "price_sale_end_date"
        Public Const PRICE_START_DATE_ATTR_KEY As String = "calculated_reg_price_start_date"
        Public Const PRICE_CHANGE_TYPE_ATTR_KEY As String = "price_pricechgtypeid"
        Public Const PRICE_ATTR_KEY As String = "price_posprice"
        Public Const PRICE_MULTIPLE_ATTR_KEY As String = "price_multiple"
        Public Const PRICE_PROMO_ATTR_KEY As String = "price_possale_price"
        Public Const PRICE_MSRP_PRICE_ATTR_KEY As String = "price_msrpprice"
        Public Const PRICE_MSRP_MULTIPLE_ATTR_KEY As String = "price_msrpmultiple"
        Public Const PRICE_PROMO_MULTIPLE_ATTR_KEY As String = "price_sale_multiple"
        Public Const PRICE_IS_CHANGE_ATTR_KEY As String = "calculated_ispricechange"
        Public Const PRICE_IS_AUTHORIZED_ATTR_KEY As String = "calculated_isauthorized"
        Public Const PRICE_LINKED_ITEM_IDENTIFIER_ATTR_KEY As String = "price_linkeditem"

        Public Const COST_ATTR_KEY As String = "vendorcosthistory_unitcost"
        Public Const COST_PROMOTIONAL_ATTR_KEY As String = "vendorcosthistory_promotional"
        Public Const COST_START_DATE_ATTR_KEY As String = "vendorcosthistory_startdate"
        Public Const COST_END_DATE_ATTR_KEY As String = "vendorcosthistory_enddate"
        Public Const COST_VEND_PKG_DSCR_ATTR_KEY As String = "vendorcosthistory_package_desc1"
        Public Const COST_COST_UNIT_ATTR_KEY As String = "vendorcosthistory_costunit_id"
        Public Const COST_FREIGHT_UNIT_ATTR_KEY As String = "vendorcosthistory_freightunit_id"
        Public Const COST_UNIT_FREIGHT_ATTR_KEY As String = "vendorcosthistory_unitfreight"
        Public Const COST_DISCOUNT_ATTR_KEY As String = "vendorcosthistory_netdiscount"
        Public Const COST_NET_COST_ATTR_KEY As String = "vendorcosthistory_netcost"
        Public Const COST_VENDOR_ATTR_KEY As String = "itemvendor_vendor_id"
        Public Const COST_ITEM_ID_ATTR_KEY As String = "itemvendor_item_id"
        Public Const COST_PRIMARY_VENDOR As String = "storeitemvendor_primaryvendor"
        Public Const COST_IGNORECASEPACK_ATTR_KEY As String = "itemvendor_ignorecasepack"
        Public Const COST_RETAILCASEPACK_ATTR_KEY As String = "itemvendor_retailcasepack"

        Public Const COST_DELETE_VENDOR As String = "calculated_delete_vendor"
        Public Const COST_DEAUTH_STORE As String = "calculated_deauth_store"
        Public Const UPLOAD_EXCLUSION_COLUMN As String = "calculated_upload_exclusion"

        Public Const COST_IS_CHANGE_ATTR_KEY As String = "calculated_iscostchange"

        Public Const DEAL_IS_CHANGE_ATTR_KEY As String = "calculated_isdealchange"
        Public Const DISCOUNT_AMOUNT_ATTR_KEY As String = "slim_vendordealview_discount"
        Public Const ALLOWANCE_AMOUNT_ATTR_KEY As String = "slim_vendordealview_allowance"
        Public Const DISCOUNT_START_DATE_ATTR_KEY As String = "slim_vendordealview_discountstartdate"
        Public Const DISCOUNT_END_DATE_ATTR_KEY As String = "slim_vendordealview_discountenddate"
        Public Const ALLOWANCE_START_DATE_ATTR_KEY As String = "slim_vendordealview_allowancestartdate"
        Public Const ALLOWANCE_END_DATE_ATTR_KEY As String = "slim_vendordealview_allowanceenddate"

        Public Const COST_PKG_COST_ATTR_KEY As String = "vendorcosthistory_unitcost"
        Public Const PRICE_MARGIN_ATTR_KEY As String = "calculated_margin"

        Public Const VALIDATION_LEVEL_COLUMN_NAME As String = "validation_level"


        ' group keys
        Public Const GROUP_HIERARCHY_DATA_KEY As String = "Hierarchy Data"
        Public Const GROUP_SCALE_DATA_KEY As String = "Scale Data"

#End Region

#Region "Misc Constants"

        Public Enum ValidationLevels
            Valid = 0
            Warning = 1
            Invalid = 2
        End Enum

        Public Const CHAIN_NAMES_NOT_FOUND_ID As Integer = -1

        Public Const LONG_TEXT_SIZE As Integer = 100
        Public Const LONG_TEXT_COLUMN_WIDTH As Integer = 150

        Public Const MATCHVENDORVIN_TEMPLATE As Integer = 13

#End Region

#Region "Database Value Constants"

        Public Const REG_PRICE_VALIDATION_CODE_NO_PRIMARY_VENDOR As Integer = 103
        Public Const PROMO_PRICE_VALIDATION_CODE_NO_PRIMARY_VENDOR As Integer = 212

#End Region

#Region "Look and feel Constants"

        Public Shared GRID_CELL_BACKGROUND_COLOR_DISABLED As Color = Color.LightGray
        Public Shared GRID_CELL_BACKGROUND_COLOR_CUSTOMPOPUP As Color = Color.LightSteelBlue
        Public Shared GRID_CELL_BACKGROUND_COLOR_VALIDATION_WARNING As Color = Color.Yellow
        Public Shared GRID_CELL_BACKGROUND_COLOR_VALIDATION_ERROR As Color = Color.Salmon

#End Region

    End Class

End Namespace
