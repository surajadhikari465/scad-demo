Imports System.Data.SqlClient

Namespace WholeFoods.IRMA.Replenishment.PeopleSoftUpload.BusinessLogic
    Public Class PeopleSoftUploadBO
        ' -----------------------------------------------------------------
        ' Update History
        ' -----------------------------------------------------------------
        '
        ' Date          DEV         TFS             Comment
        ' 04/12/2010    Tom Lux     12198 (v3.6)    Removed the 'Uploaded Date' property (no other references found), 
        '                                           as value in the order is now set in a stored procedure.
        ' 05/28/2013    DN          12488           Removed the CDate() function for the column ENTERED_DT. 
        Private _orderHeaderID As Integer
        Private _VCHR_HDR_ROW_ID As String = Nothing
        Private _BUSINESS_UNIT_ID As Integer
        Private _VOUCHER_ID As String = Nothing
        Private _INVOICE_ID As String = Nothing
        Private _INVOICE_DT As Date
        Private _VENDOR_SETID As String = Nothing
        Private _VENDOR_ID As String = Nothing
        Private _VNDR_LOC As String = Nothing
        Private _ADDRESS_SEQ_NUM As String = Nothing
        Private _GRP_AP_ID As String = Nothing
        Private _ORIGIN As String = Nothing
        Private _OPRID As String = Nothing
        Private _VCHR_TTL_LINES As Integer
        Private _ACCOUNTING_DT As Date
        Private _POST_VOUCHER As String = Nothing
        Private _DST_CNTRL_ID As String = Nothing
        Private _VOUCHER_ID_RELATED As String = Nothing
        Private _GROSS_AMT As Decimal
        Private _DSCNT_AMT As String = Nothing
        Private _USETAX_CD As String = Nothing
        Private _SALETX_AMT As String = Nothing
        Private _SALETX_CD As String = Nothing
        Private _FREIGHT_AMT As String = Nothing
        Private _DUE_DT As String = Nothing
        Private _DSCNT_DUE_DT As String = Nothing
        Private _PYMNT_TERMS_CD As String = Nothing
        Private _ENTERED_DT As Date
        Private _TXN_CURRENCY_CD As String = Nothing
        Private _RT_TYPE As String = Nothing
        Private _RATE_MULT As String = Nothing
        Private _RATE_DIV As String = Nothing
        Private _VAT_ENTRD_AMT As String = Nothing
        Private _MATCH_ACTION As String = Nothing
        Private _MATCH_STATUS_VCHR As String = Nothing
        Private _BCM_TRAN_TYPE As String = Nothing
        Private _CNTRCT_ID As String = Nothing
        Private _REMIT_ADDR_SEQ_NUM As String = Nothing
        Private _CUR_RT_SOURCE As String = Nothing
        Private _DSCNT_AMT_FLG As String = Nothing
        Private _DUE_DT_FLG As String = Nothing
        Private _VCHR_APPRVL_FLG As String = Nothing
        Private _BUSPROCNAME As String = Nothing
        Private _APPR_RULE_SET As String = Nothing
        Private _VAT_DCLRTN_POINT As String = Nothing
        Private _VAT_CALC_TYPE As String = Nothing
        Private _VAT_ENTITY As String = Nothing
        Private _VAT_TXN_TYPE_CD As String = Nothing
        Private _TAX_CD_VAT As String = Nothing
        Private _VAT_RCRD_INPT_FLG As String = Nothing
        Private _VAT_RCRD_OUTPT_FLG As String = Nothing
        Private _VAT_RECOVERY_PCT As String = Nothing
        Private _VAT_CALC_GROSS_NET As String = Nothing
        Private _VAT_RECALC_FLG As String = Nothing
        Private _VAT_CALC_FRGHT_FLG As String = Nothing
        Private _VAT_RGSTRN_SELLER As String = Nothing
        Private _COUNTRY_SHIP_FROM As String = Nothing
        Private _COUNTRY_SHIP_TO As String = Nothing
        Private _COUNTRY_VAT_BILLFR As String = Nothing
        Private _COUNTRY_VAT_BILLTO As String = Nothing
        Private _VAT_TREATMENT_PUR As String = Nothing
        Private _VAT_EXCPTN_TYPE As String = Nothing
        Private _VAT_EXCPTN_CERTIF As String = Nothing
        Private _VAT_USE_ID As String = Nothing
        Private _DSCNT_PRORATE_FLG As String = Nothing
        Private _USETAX_PRORATE_FLG As String = Nothing
        Private _SALETX_PRORATE_FLG As String = Nothing
        Private _FRGHT_PRORATE_FLG As String = Nothing
        Private _IST_TXN_FLG As String = Nothing
        Private _DOC_TYPE As String = Nothing
        Private _DOC_SEQ_DATE As String = Nothing
        Private _DOC_SEQ_NBR As String = Nothing
        Private _VAT_CF_ANLSYS_TYPE As String = Nothing
        Private _DESCR254_MIXED As Integer
        Private _VCHR_LINE_ROW_ID As String = Nothing
        Private _LINE_BUSINESS_UNIT_ID As Integer
        Private _LINE_VOUCHER_ID As String = Nothing
        Private _VOUCHER_LINE_NUM As Integer
        Private _TOTAL_DISTRIBS As Integer
        Private _BUSINESS_UNIT_PO As String = Nothing
        Private _PO_ID As Integer = Nothing    'TFS 6926 - PO_ID changed to an Integer and added to AP Upload File
        Private _LINE_NBR As String = Nothing
        Private _SCHED_NBR As String = Nothing
        Private _DESCR As String = Nothing
        Private _MERCHANDISE_AMT As Decimal
        Private _ITEM_SETID As String = Nothing
        Private _INV_ITEM_ID As String = Nothing
        Private _QTY_VCHR As String = Nothing
        Private _STATISTIC_AMT As String = Nothing
        Private _UNIT_OF_MEASURE As String = Nothing
        Private _UNIT_PRICE As String = Nothing
        Private _SALETX_APPL_FLG As String = Nothing
        Private _USETAX_APPL_FLG As String = Nothing
        Private _DSCNT_APPL_FLG As String = Nothing
        Private _WTHD_SW As String = Nothing
        Private _BUSINESS_UNIT_RECV As String = Nothing
        Private _RECEIVER_ID As String = Nothing
        Private _RECV_LN_NBR As String = Nothing
        Private _RECV_SHIP_SEQ_NBR As String = Nothing
        Private _MATCH_LINE_OPT As String = Nothing
        Private _DISTRIB_MTHD_FLG As String = Nothing
        Private _BASE_CURRENCY As String = Nothing
        Private _CURRENCY_CD As String = Nothing
        Private _SHIPTO_ID As String = Nothing
        Private _SUT_BASE_ID As String = Nothing
        Private _TAX_CD_SUT As String = Nothing
        Private _SUT_EXCPTN_TYPE As String = Nothing
        Private _SUT_EXCPTN_CERTIF As String = Nothing
        Private _SUT_APPLICABILITY As String = Nothing
        Private _WTHD_SETID As String = Nothing
        Private _WTHD_CD As String = Nothing
        Private _VAT_APPL_FLG As String = Nothing
        Private _VAT_APPLICABILITY As String = Nothing
        Private _NATURE_OF_TXN1 As String = Nothing
        Private _NATURE_OF_TXN2 As String = Nothing
        Private _VCHR_DIST_ROW_ID As String = Nothing
        Private _DIST_BUSINESS_UNIT_ID As Integer
        Private _DIST_VOUCHER_ID As String = Nothing
        Private _DIST_VOUCHER_LINE_NUM As Integer
        Private _DISTRIB_LINE_NUM As Integer
        Private _BUSINESS_UNIT_GL As Integer
        Private _ACCOUNT As String
        Private _STATISTIC_CODE As String = Nothing
        Private _STATISTIC_AMOUNT As String = Nothing
        Private _JRNL_LN_REF As String = Nothing
        Private _OPEN_ITEM_STATUS As String = Nothing
        Private _DISTRIB_DESCR As String = Nothing
        Private _DIST_MERCHANDISE_AMT As Decimal
        Private _DISTRIB_PO_ID As Integer = Nothing     'TFS 6926 - DISTRIB_PO_ID changed to an Integer and added to AP Upload File
        Private _DISTRIB_LINE_NBR As String = Nothing
        Private _DISTRIB_SCHED_NBR As String = Nothing
        Private _PO_DIST_LINE_NUM As String = Nothing
        Private _BUSINESS_UNIT_PC As String = Nothing
        Private _ACTIVITY_ID As String = Nothing
        Private _ANALYSIS_TYPE As String = Nothing
        Private _RESOURCE_TYPE As String = Nothing
        Private _RESOURCE_CATEGORY As String = Nothing
        Private _RESOURCE_SUB_CAT As String = Nothing
        Private _ASSET_FLG As String = Nothing
        Private _BUSINESS_UNIT_AM As String = Nothing
        Private _ASSET_ID As String = Nothing
        Private _PROFILE_ID As String = Nothing
        Private _USETAX_AMT As String = Nothing
        Private _VAT_INV_AMT As String = Nothing
        Private _VAT_NONINV_AMT As String = Nothing
        Private _RECV_DIST_LINE_NUM As String = Nothing
        Private _DEPTID As String
        Private _PRODUCT As String
        Private _PROEJCT_ID As String = Nothing
        Private _AFFILIATE As String = Nothing
        Private _VAT_APORT_CNTRL As String = Nothing
        Private _ThirdPartyFreightInvoice As Boolean
        Private _VCHR_CURR_ROW_ID As String = Nothing
        Private _CURR_BUSINESS_UNIT_ID As Integer
        Private _CURR_VENDOR_CODE As String = Nothing
        Private _CURR_BU_CODE As String = Nothing

#Region "Property accessors"
        Public Property OrderHeaderID() As Integer
            Get
                Return _orderHeaderID
            End Get
            Set(ByVal value As Integer)
                _orderHeaderID = value
            End Set
        End Property

        Public Property VCHR_HDR_ROW_ID() As String
            Get
                Return _VCHR_HDR_ROW_ID
            End Get
            Set(ByVal value As String)
                _VCHR_HDR_ROW_ID = value
            End Set
        End Property

        Public Property BUSINESS_UNIT_ID() As Integer
            Get
                Return _BUSINESS_UNIT_ID
            End Get
            Set(ByVal value As Integer)
                _BUSINESS_UNIT_ID = value
            End Set
        End Property

        Public Property VOUCHER_ID() As String
            Get
                Return _VOUCHER_ID
            End Get
            Set(ByVal value As String)
                _VOUCHER_ID = value
            End Set
        End Property

        Public Property INVOICE_ID() As String
            Get
                Return _INVOICE_ID
            End Get
            Set(ByVal value As String)
                _INVOICE_ID = value
            End Set
        End Property

        Public Property INVOICE_DT() As Date
            Get
                Return _INVOICE_DT
            End Get
            Set(ByVal value As Date)
                _INVOICE_DT = value
            End Set
        End Property

        Public Property VENDOR_SETID() As String
            Get
                Return _VENDOR_SETID
            End Get
            Set(ByVal value As String)
                _VENDOR_SETID = value
            End Set
        End Property

        Public Property VENDOR_ID() As String
            Get
                Return _VENDOR_ID
            End Get
            Set(ByVal value As String)
                _VENDOR_ID = value
            End Set
        End Property

        Public Property VNDR_LOC() As String
            Get
                Return _VNDR_LOC
            End Get
            Set(ByVal value As String)
                _VNDR_LOC = value
            End Set
        End Property

        Public Property ADDRESS_SEQ_NUM() As String
            Get
                Return _ADDRESS_SEQ_NUM
            End Get
            Set(ByVal value As String)
                _ADDRESS_SEQ_NUM = value
            End Set
        End Property

        Public Property GRP_AP_ID() As String
            Get
                Return _GRP_AP_ID
            End Get
            Set(ByVal value As String)
                _GRP_AP_ID = value
            End Set
        End Property

        Public Property ORIGIN() As String
            Get
                Return _ORIGIN
            End Get
            Set(ByVal value As String)
                _ORIGIN = value
            End Set
        End Property

        Public Property OPRID() As String
            Get
                Return _OPRID
            End Get
            Set(ByVal value As String)
                _OPRID = value
            End Set
        End Property

        Public Property VCHR_TTL_LINES() As Integer
            Get
                Return _VCHR_TTL_LINES
            End Get
            Set(ByVal value As Integer)
                _VCHR_TTL_LINES = value
            End Set
        End Property

        Public Property ACCOUNTING_DT() As Date
            Get
                Return _ACCOUNTING_DT
            End Get
            Set(ByVal value As Date)
                _ACCOUNTING_DT = value
            End Set
        End Property

        Public Property POST_VOUCHER() As String
            Get
                Return _POST_VOUCHER
            End Get
            Set(ByVal value As String)
                _POST_VOUCHER = value
            End Set
        End Property

        Public Property DST_CNTRL_ID() As String
            Get
                Return _DST_CNTRL_ID
            End Get
            Set(ByVal value As String)
                _DST_CNTRL_ID = value
            End Set
        End Property

        Public Property VOUCHER_ID_RELATED() As String
            Get
                Return _VOUCHER_ID_RELATED
            End Get
            Set(ByVal value As String)
                _VOUCHER_ID_RELATED = value
            End Set
        End Property

        Public Property GROSS_AMT() As Decimal
            Get
                Return _GROSS_AMT
            End Get
            Set(ByVal value As Decimal)
                _GROSS_AMT = value
            End Set
        End Property

        Public Property DSCNT_AMT() As String
            Get
                Return _DSCNT_AMT
            End Get
            Set(ByVal value As String)
                _DSCNT_AMT = value
            End Set
        End Property

        Public Property USETAX_CD() As String
            Get
                Return _USETAX_CD
            End Get
            Set(ByVal value As String)
                _USETAX_CD = value
            End Set
        End Property

        Public Property SALETX_AMT() As String
            Get
                Return _SALETX_AMT
            End Get
            Set(ByVal value As String)
                _SALETX_AMT = value
            End Set
        End Property

        Public Property SALETX_CD() As String
            Get
                Return _SALETX_CD
            End Get
            Set(ByVal value As String)
                _SALETX_CD = value
            End Set
        End Property

        Public Property FREIGHT_AMT() As String
            Get
                Return _FREIGHT_AMT
            End Get
            Set(ByVal value As String)
                _FREIGHT_AMT = value
            End Set
        End Property

        Public Property DUE_DT() As String
            Get
                Return _DUE_DT
            End Get
            Set(ByVal value As String)
                _DUE_DT = value
            End Set
        End Property

        Public Property DSCNT_DUE_DT() As String
            Get
                Return _DSCNT_DUE_DT
            End Get
            Set(ByVal value As String)
                _DSCNT_DUE_DT = value
            End Set
        End Property

        Public Property PYMNT_TERMS_CD() As String
            Get
                Return _PYMNT_TERMS_CD
            End Get
            Set(ByVal value As String)
                _PYMNT_TERMS_CD = value
            End Set
        End Property

        Public Property ENTERED_DT() As Date
            Get
                Return _ENTERED_DT
            End Get
            Set(ByVal value As Date)
                _ENTERED_DT = value
            End Set
        End Property

        Public Property TXN_CURRENCY_CD() As String
            Get
                Return _TXN_CURRENCY_CD
            End Get
            Set(ByVal value As String)
                _TXN_CURRENCY_CD = value
            End Set
        End Property

        Public Property RT_TYPE() As String
            Get
                Return _RT_TYPE
            End Get
            Set(ByVal value As String)
                _RT_TYPE = value
            End Set
        End Property

        Public Property RATE_MULT() As String
            Get
                Return _RATE_MULT
            End Get
            Set(ByVal value As String)
                _RATE_MULT = value
            End Set
        End Property

        Public Property RATE_DIV() As String
            Get
                Return _RATE_DIV
            End Get
            Set(ByVal value As String)
                _RATE_DIV = value
            End Set
        End Property

        Public Property VAT_ENTRD_AMT() As String
            Get
                Return _VAT_ENTRD_AMT
            End Get
            Set(ByVal value As String)
                _VAT_ENTRD_AMT = value
            End Set
        End Property

        Public Property MATCH_ACTION() As String
            Get
                Return _MATCH_ACTION
            End Get
            Set(ByVal value As String)
                _MATCH_ACTION = value
            End Set
        End Property

        Public Property MATCH_STATUS_VCHR() As String
            Get
                Return _MATCH_STATUS_VCHR
            End Get
            Set(ByVal value As String)
                _MATCH_STATUS_VCHR = value
            End Set
        End Property

        Public Property BCM_TRAN_TYPE() As String
            Get
                Return _BCM_TRAN_TYPE
            End Get
            Set(ByVal value As String)
                _BCM_TRAN_TYPE = value
            End Set
        End Property

        Public Property CNTRCT_ID() As String
            Get
                Return _CNTRCT_ID
            End Get
            Set(ByVal value As String)
                _CNTRCT_ID = value
            End Set
        End Property

        Public Property REMIT_ADDR_SEQ_NUM() As String
            Get
                Return _REMIT_ADDR_SEQ_NUM
            End Get
            Set(ByVal value As String)
                _REMIT_ADDR_SEQ_NUM = value
            End Set
        End Property

        Public Property CUR_RT_SOURCE() As String
            Get
                Return _CUR_RT_SOURCE
            End Get
            Set(ByVal value As String)
                _CUR_RT_SOURCE = value
            End Set
        End Property

        Public Property DSCNT_AMT_FLG() As String
            Get
                Return _DSCNT_AMT_FLG
            End Get
            Set(ByVal value As String)
                _DSCNT_AMT_FLG = value
            End Set
        End Property

        Public Property DUE_DT_FLG() As String
            Get
                Return _DUE_DT_FLG
            End Get
            Set(ByVal value As String)
                _DUE_DT_FLG = value
            End Set
        End Property

        Public Property VCHR_APPRVL_FLG() As String
            Get
                Return _VCHR_APPRVL_FLG
            End Get
            Set(ByVal value As String)
                _VCHR_APPRVL_FLG = value
            End Set
        End Property

        Public Property BUSPROCNAME() As String
            Get
                Return _BUSPROCNAME
            End Get
            Set(ByVal value As String)
                _BUSPROCNAME = value
            End Set
        End Property

        Public Property APPR_RULE_SET() As String
            Get
                Return _APPR_RULE_SET
            End Get
            Set(ByVal value As String)
                _APPR_RULE_SET = value
            End Set
        End Property

        Public Property VAT_DCLRTN_POINT() As String
            Get
                Return _VAT_DCLRTN_POINT
            End Get
            Set(ByVal value As String)
                _VAT_DCLRTN_POINT = value
            End Set
        End Property

        Public Property VAT_CALC_TYPE() As String
            Get
                Return _VAT_CALC_TYPE
            End Get
            Set(ByVal value As String)
                _VAT_CALC_TYPE = value
            End Set
        End Property

        Public Property VAT_ENTITY() As String
            Get
                Return _VAT_ENTITY
            End Get
            Set(ByVal value As String)
                _VAT_ENTITY = value
            End Set
        End Property

        Public Property VAT_TXN_TYPE_CD() As String
            Get
                Return _VAT_TXN_TYPE_CD
            End Get
            Set(ByVal value As String)
                _VAT_TXN_TYPE_CD = value
            End Set
        End Property

        Public Property TAX_CD_VAT() As String
            Get
                Return _TAX_CD_VAT
            End Get
            Set(ByVal value As String)
                _TAX_CD_VAT = value
            End Set
        End Property

        Public Property VAT_RCRD_INPT_FLG() As String
            Get
                Return _VAT_RCRD_INPT_FLG
            End Get
            Set(ByVal value As String)
                _VAT_RCRD_INPT_FLG = value
            End Set
        End Property

        Public Property VAT_RCRD_OUTPT_FLG() As String
            Get
                Return _VAT_RCRD_OUTPT_FLG
            End Get
            Set(ByVal value As String)
                _VAT_RCRD_OUTPT_FLG = value
            End Set
        End Property

        Public Property VAT_RECOVERY_PCT() As String
            Get
                Return _VAT_RECOVERY_PCT
            End Get
            Set(ByVal value As String)
                _VAT_RECOVERY_PCT = value
            End Set
        End Property

        Public Property VAT_CALC_GROSS_NET() As String
            Get
                Return _VAT_CALC_GROSS_NET
            End Get
            Set(ByVal value As String)
                _VAT_CALC_GROSS_NET = value
            End Set
        End Property

        Public Property VAT_RECALC_FLG() As String
            Get
                Return _VAT_RECALC_FLG
            End Get
            Set(ByVal value As String)
                _VAT_RECALC_FLG = value
            End Set
        End Property

        Public Property VAT_CALC_FRGHT_FLG() As String
            Get
                Return _VAT_CALC_FRGHT_FLG
            End Get
            Set(ByVal value As String)
                _VAT_CALC_FRGHT_FLG = value
            End Set
        End Property

        Public Property VAT_RGSTRN_SELLER() As String
            Get
                Return _VAT_RGSTRN_SELLER
            End Get
            Set(ByVal value As String)
                _VAT_RGSTRN_SELLER = value
            End Set
        End Property

        Public Property COUNTRY_SHIP_FROM() As String
            Get
                Return _COUNTRY_SHIP_FROM
            End Get
            Set(ByVal value As String)
                _COUNTRY_SHIP_FROM = value
            End Set
        End Property

        Public Property COUNTRY_SHIP_TO() As String
            Get
                Return _COUNTRY_SHIP_TO
            End Get
            Set(ByVal value As String)
                _COUNTRY_SHIP_TO = value
            End Set
        End Property

        Public Property COUNTRY_VAT_BILLFR() As String
            Get
                Return _COUNTRY_VAT_BILLFR
            End Get
            Set(ByVal value As String)
                _COUNTRY_VAT_BILLFR = value
            End Set
        End Property

        Public Property COUNTRY_VAT_BILLTO() As String
            Get
                Return _COUNTRY_VAT_BILLTO
            End Get
            Set(ByVal value As String)
                _COUNTRY_VAT_BILLTO = value
            End Set
        End Property

        Public Property VAT_TREATMENT_PUR() As String
            Get
                Return _VAT_TREATMENT_PUR
            End Get
            Set(ByVal value As String)
                _VAT_TREATMENT_PUR = value
            End Set
        End Property

        Public Property VAT_EXCPTN_TYPE() As String
            Get
                Return _VAT_EXCPTN_TYPE
            End Get
            Set(ByVal value As String)
                _VAT_EXCPTN_TYPE = value
            End Set
        End Property

        Public Property VAT_EXCPTN_CERTIF() As String
            Get
                Return _VAT_EXCPTN_CERTIF
            End Get
            Set(ByVal value As String)
                _VAT_EXCPTN_CERTIF = value
            End Set
        End Property

        Public Property VAT_USE_ID() As String
            Get
                Return _VAT_USE_ID
            End Get
            Set(ByVal value As String)
                _VAT_USE_ID = value
            End Set
        End Property

        Public Property DSCNT_PRORATE_FLG() As String
            Get
                Return _DSCNT_PRORATE_FLG
            End Get
            Set(ByVal value As String)
                _DSCNT_PRORATE_FLG = value
            End Set
        End Property

        Public Property USETAX_PRORATE_FLG() As String
            Get
                Return _USETAX_PRORATE_FLG
            End Get
            Set(ByVal value As String)
                _USETAX_PRORATE_FLG = value
            End Set
        End Property

        Public Property SALETX_PRORATE_FLG() As String
            Get
                Return _SALETX_PRORATE_FLG
            End Get
            Set(ByVal value As String)
                _SALETX_PRORATE_FLG = value
            End Set
        End Property

        Public Property FRGHT_PRORATE_FLG() As String
            Get
                Return _FRGHT_PRORATE_FLG
            End Get
            Set(ByVal value As String)
                _FRGHT_PRORATE_FLG = value
            End Set
        End Property

        Public Property IST_TXN_FLG() As String
            Get
                Return _IST_TXN_FLG
            End Get
            Set(ByVal value As String)
                _IST_TXN_FLG = value
            End Set
        End Property

        Public Property DOC_TYPE() As String
            Get
                Return _DOC_TYPE
            End Get
            Set(ByVal value As String)
                _DOC_TYPE = value
            End Set
        End Property

        Public Property DOC_SEQ_DATE() As String
            Get
                Return _DOC_SEQ_DATE
            End Get
            Set(ByVal value As String)
                _DOC_SEQ_DATE = value
            End Set
        End Property

        Public Property DOC_SEQ_NBR() As String
            Get
                Return _DOC_SEQ_NBR
            End Get
            Set(ByVal value As String)
                _DOC_SEQ_NBR = value
            End Set
        End Property

        Public Property VAT_CF_ANLSYS_TYPE() As String
            Get
                Return _VAT_CF_ANLSYS_TYPE
            End Get
            Set(ByVal value As String)
                _VAT_CF_ANLSYS_TYPE = value
            End Set
        End Property

        Public Property DESCR254_MIXED() As Integer
            Get
                Return _DESCR254_MIXED
            End Get
            Set(ByVal value As Integer)
                _DESCR254_MIXED = value
            End Set
        End Property

        Public Property VCHR_LINE_ROW_ID() As String
            Get
                Return _VCHR_LINE_ROW_ID
            End Get
            Set(ByVal value As String)
                _VCHR_LINE_ROW_ID = value
            End Set
        End Property

        Public Property LINE_BUSINESS_UNIT_ID() As Integer
            Get
                Return _LINE_BUSINESS_UNIT_ID
            End Get
            Set(ByVal value As Integer)
                _LINE_BUSINESS_UNIT_ID = value
            End Set
        End Property

        Public Property LINE_VOUCHER_ID() As String
            Get
                Return _LINE_VOUCHER_ID
            End Get
            Set(ByVal value As String)
                _LINE_VOUCHER_ID = value
            End Set
        End Property

        Public Property VOUCHER_LINE_NUM() As Integer
            Get
                Return _VOUCHER_LINE_NUM
            End Get
            Set(ByVal value As Integer)
                _VOUCHER_LINE_NUM = value
            End Set
        End Property

        Public Property TOTAL_DISTRIBS() As Integer
            Get
                Return _TOTAL_DISTRIBS
            End Get
            Set(ByVal value As Integer)
                _TOTAL_DISTRIBS = value
            End Set
        End Property

        Public Property BUSINESS_UNIT_PO() As String
            Get
                Return _BUSINESS_UNIT_PO
            End Get
            Set(ByVal value As String)
                _BUSINESS_UNIT_PO = value
            End Set
        End Property

        Public Property PO_ID() As Integer
            Get
                Return _PO_ID
            End Get
            Set(ByVal value As Integer)
                _PO_ID = value
            End Set
        End Property

        Public Property LINE_NBR() As String
            Get
                Return _LINE_NBR
            End Get
            Set(ByVal value As String)
                _LINE_NBR = value
            End Set
        End Property

        Public Property SCHED_NBR() As String
            Get
                Return _SCHED_NBR
            End Get
            Set(ByVal value As String)
                _SCHED_NBR = value
            End Set
        End Property

        Public Property DESCR() As String
            Get
                Return _DESCR
            End Get
            Set(ByVal value As String)
                _DESCR = value
            End Set
        End Property
        Public Property MERCHANDISE_AMT() As Decimal
            Get
                Return _MERCHANDISE_AMT
            End Get
            Set(ByVal value As Decimal)
                _MERCHANDISE_AMT = value
            End Set
        End Property

        Public Property ITEM_SETID() As String
            Get
                Return _ITEM_SETID
            End Get
            Set(ByVal value As String)
                _ITEM_SETID = value
            End Set
        End Property

        Public Property INV_ITEM_ID() As String
            Get
                Return _INV_ITEM_ID
            End Get
            Set(ByVal value As String)
                _INV_ITEM_ID = value
            End Set
        End Property

        Public Property QTY_VCHR() As String
            Get
                Return _QTY_VCHR
            End Get
            Set(ByVal value As String)
                _QTY_VCHR = value
            End Set
        End Property

        Public Property STATISTIC_AMT() As String
            Get
                Return _STATISTIC_AMT
            End Get
            Set(ByVal value As String)
                _STATISTIC_AMT = value
            End Set
        End Property

        Public Property UNIT_OF_MEASURE() As String
            Get
                Return _UNIT_OF_MEASURE
            End Get
            Set(ByVal value As String)
                _UNIT_OF_MEASURE = value
            End Set
        End Property

        Public Property UNIT_PRICE() As String
            Get
                Return _UNIT_PRICE
            End Get
            Set(ByVal value As String)
                _UNIT_PRICE = value
            End Set
        End Property

        Public Property SALETX_APPL_FLG() As String
            Get
                Return _SALETX_APPL_FLG
            End Get
            Set(ByVal value As String)
                _SALETX_APPL_FLG = value
            End Set
        End Property

        Public Property USETAX_APPL_FLG() As String
            Get
                Return _USETAX_APPL_FLG
            End Get
            Set(ByVal value As String)
                _USETAX_APPL_FLG = value
            End Set
        End Property

        Public Property DSCNT_APPL_FLG() As String
            Get
                Return _DSCNT_APPL_FLG
            End Get
            Set(ByVal value As String)
                _DSCNT_APPL_FLG = value
            End Set
        End Property

        Public Property WTHD_SW() As String
            Get
                Return _WTHD_SW
            End Get
            Set(ByVal value As String)
                _WTHD_SW = value
            End Set
        End Property

        Public Property BUSINESS_UNIT_RECV() As String
            Get
                Return _BUSINESS_UNIT_RECV
            End Get
            Set(ByVal value As String)
                _BUSINESS_UNIT_RECV = value
            End Set
        End Property

        Public Property RECEIVER_ID() As String
            Get
                Return _RECEIVER_ID
            End Get
            Set(ByVal value As String)
                _RECEIVER_ID = value
            End Set
        End Property

        Public Property RECV_LN_NBR() As String
            Get
                Return _RECV_LN_NBR
            End Get
            Set(ByVal value As String)
                _RECV_LN_NBR = value
            End Set
        End Property

        Public Property RECV_SHIP_SEQ_NBR() As String
            Get
                Return _RECV_SHIP_SEQ_NBR
            End Get
            Set(ByVal value As String)
                _RECV_SHIP_SEQ_NBR = value
            End Set
        End Property

        Public Property MATCH_LINE_OPT() As String
            Get
                Return _MATCH_LINE_OPT
            End Get
            Set(ByVal value As String)
                _MATCH_LINE_OPT = value
            End Set
        End Property

        Public Property DISTRIB_MTHD_FLG() As String
            Get
                Return _DISTRIB_MTHD_FLG
            End Get
            Set(ByVal value As String)
                _DISTRIB_MTHD_FLG = value
            End Set
        End Property

        Public Property BASE_CURRENCY() As String
            Get
                Return _BASE_CURRENCY
            End Get
            Set(ByVal value As String)
                _BASE_CURRENCY = value
            End Set
        End Property

        Public Property CURRENCY_CD() As String
            Get
                Return _CURRENCY_CD
            End Get
            Set(ByVal value As String)
                _CURRENCY_CD = value
            End Set
        End Property

        Public Property SHIPTO_ID() As String
            Get
                Return _SHIPTO_ID
            End Get
            Set(ByVal value As String)
                _SHIPTO_ID = value
            End Set
        End Property

        Public Property SUT_BASE_ID() As String
            Get
                Return _SUT_BASE_ID
            End Get
            Set(ByVal value As String)
                _SUT_BASE_ID = value
            End Set
        End Property

        Public Property TAX_CD_SUT() As String
            Get
                Return _TAX_CD_SUT
            End Get
            Set(ByVal value As String)
                _TAX_CD_SUT = value
            End Set
        End Property

        Public Property SUT_EXCPTN_TYPE() As String
            Get
                Return _SUT_EXCPTN_TYPE
            End Get
            Set(ByVal value As String)
                _SUT_EXCPTN_TYPE = value
            End Set
        End Property

        Public Property SUT_EXCPTN_CERTIF() As String
            Get
                Return _SUT_EXCPTN_CERTIF
            End Get
            Set(ByVal value As String)
                _SUT_EXCPTN_CERTIF = value
            End Set
        End Property

        Public Property SUT_APPLICABILITY() As String
            Get
                Return _SUT_APPLICABILITY
            End Get
            Set(ByVal value As String)
                _SUT_APPLICABILITY = value
            End Set
        End Property

        Public Property WTHD_SETID() As String
            Get
                Return _WTHD_SETID
            End Get
            Set(ByVal value As String)
                _WTHD_SETID = value
            End Set
        End Property

        Public Property WTHD_CD() As String
            Get
                Return _WTHD_CD
            End Get
            Set(ByVal value As String)
                _WTHD_CD = value
            End Set
        End Property

        Public Property VAT_APPL_FLG() As String
            Get
                Return _VAT_APPL_FLG
            End Get
            Set(ByVal value As String)
                _VAT_APPL_FLG = value
            End Set
        End Property

        Public Property VAT_APPLICABILITY() As String
            Get
                Return _VAT_APPLICABILITY
            End Get
            Set(ByVal value As String)
                _VAT_APPLICABILITY = value
            End Set
        End Property

        Public Property NATURE_OF_TXN1() As String
            Get
                Return _NATURE_OF_TXN1
            End Get
            Set(ByVal value As String)
                _NATURE_OF_TXN1 = value
            End Set
        End Property

        Public Property NATURE_OF_TXN2() As String
            Get
                Return _NATURE_OF_TXN2
            End Get
            Set(ByVal value As String)
                _NATURE_OF_TXN2 = value
            End Set
        End Property

        Public Property VCHR_DIST_ROW_ID() As String
            Get
                Return _VCHR_DIST_ROW_ID
            End Get
            Set(ByVal value As String)
                _VCHR_DIST_ROW_ID = value
            End Set
        End Property

        Public Property DIST_BUSINESS_UNIT_ID() As Integer
            Get
                Return _DIST_BUSINESS_UNIT_ID
            End Get
            Set(ByVal value As Integer)
                _DIST_BUSINESS_UNIT_ID = value
            End Set
        End Property

        Public Property DIST_VOUCHER_ID() As String
            Get
                Return _DIST_VOUCHER_ID
            End Get
            Set(ByVal value As String)
                _DIST_VOUCHER_ID = value
            End Set
        End Property

        Public Property DIST_VOUCHER_LINE_NUM() As Integer
            Get
                Return _DIST_VOUCHER_LINE_NUM
            End Get
            Set(ByVal value As Integer)
                _DIST_VOUCHER_LINE_NUM = value
            End Set
        End Property

        Public Property DISTRIB_LINE_NUM() As Integer
            Get
                Return _DISTRIB_LINE_NUM
            End Get
            Set(ByVal value As Integer)
                _DISTRIB_LINE_NUM = value
            End Set
        End Property

        Public Property BUSINESS_UNIT_GL() As Integer
            Get
                Return _BUSINESS_UNIT_GL
            End Get
            Set(ByVal value As Integer)
                _BUSINESS_UNIT_GL = value
            End Set
        End Property

        Public Property ACCOUNT() As String
            Get
                Return _ACCOUNT
            End Get
            Set(ByVal value As String)
                _ACCOUNT = value
            End Set
        End Property

        Public Property STATISTIC_CODE() As String
            Get
                Return _STATISTIC_CODE
            End Get
            Set(ByVal value As String)
                _STATISTIC_CODE = value
            End Set
        End Property

        Public Property STATISTIC_AMOUNT() As String
            Get
                Return _STATISTIC_AMOUNT
            End Get
            Set(ByVal value As String)
                _STATISTIC_AMOUNT = value
            End Set
        End Property

        Public Property JRNL_LN_REF() As String
            Get
                Return _JRNL_LN_REF
            End Get
            Set(ByVal value As String)
                _JRNL_LN_REF = value
            End Set
        End Property

        Public Property OPEN_ITEM_STATUS() As String
            Get
                Return _OPEN_ITEM_STATUS
            End Get
            Set(ByVal value As String)
                _OPEN_ITEM_STATUS = value
            End Set
        End Property

        Public Property DISTRIB_DESCR() As String
            Get
                Return _DISTRIB_DESCR
            End Get
            Set(ByVal value As String)
                _DISTRIB_DESCR = value
            End Set
        End Property

        Public Property DIST_MERCHANDISE_AMT() As Decimal
            Get
                Return _DIST_MERCHANDISE_AMT
            End Get
            Set(ByVal value As Decimal)
                _DIST_MERCHANDISE_AMT = value
            End Set
        End Property

        Public Property DISTRIB_PO_ID() As Integer
            Get
                Return _DISTRIB_PO_ID
            End Get
            Set(ByVal value As Integer)
                _DISTRIB_PO_ID = value
            End Set
        End Property

        Public Property DISTRIB_LINE_NBR() As String
            Get
                Return _DISTRIB_LINE_NBR
            End Get
            Set(ByVal value As String)
                _DISTRIB_LINE_NBR = value
            End Set
        End Property

        Public Property DISTRIB_SCHED_NBR() As String
            Get
                Return _DISTRIB_SCHED_NBR
            End Get
            Set(ByVal value As String)
                _DISTRIB_SCHED_NBR = value
            End Set
        End Property

        Public Property PO_DIST_LINE_NUM() As String
            Get
                Return _PO_DIST_LINE_NUM
            End Get
            Set(ByVal value As String)
                _PO_DIST_LINE_NUM = value
            End Set
        End Property

        Public Property BUSINESS_UNIT_PC() As String
            Get
                Return _BUSINESS_UNIT_PC
            End Get
            Set(ByVal value As String)
                _BUSINESS_UNIT_PC = value
            End Set
        End Property

        Public Property ACTIVITY_ID() As String
            Get
                Return _ACTIVITY_ID
            End Get
            Set(ByVal value As String)
                _ACTIVITY_ID = value
            End Set
        End Property

        Public Property ANALYSIS_TYPE() As String
            Get
                Return _ANALYSIS_TYPE
            End Get
            Set(ByVal value As String)
                _ANALYSIS_TYPE = value
            End Set
        End Property

        Public Property RESOURCE_TYPE() As String
            Get
                Return _RESOURCE_TYPE
            End Get
            Set(ByVal value As String)
                _RESOURCE_TYPE = value
            End Set
        End Property

        Public Property RESOURCE_CATEGORY() As String
            Get
                Return _RESOURCE_CATEGORY
            End Get
            Set(ByVal value As String)
                _RESOURCE_CATEGORY = value
            End Set
        End Property

        Public Property RESOURCE_SUB_CAT() As String
            Get
                Return _RESOURCE_SUB_CAT
            End Get
            Set(ByVal value As String)
                _RESOURCE_SUB_CAT = value
            End Set
        End Property

        Public Property ASSET_FLG() As String
            Get
                Return _ASSET_FLG
            End Get
            Set(ByVal value As String)
                _ASSET_FLG = value
            End Set
        End Property

        Public Property BUSINESS_UNIT_AM() As String
            Get
                Return _BUSINESS_UNIT_AM
            End Get
            Set(ByVal value As String)
                _BUSINESS_UNIT_AM = value
            End Set
        End Property

        Public Property ASSET_ID() As String
            Get
                Return _ASSET_ID
            End Get
            Set(ByVal value As String)
                _ASSET_ID = value
            End Set
        End Property

        Public Property PROFILE_ID() As String
            Get
                Return _PROFILE_ID
            End Get
            Set(ByVal value As String)
                _PROFILE_ID = value
            End Set
        End Property

        Public Property USETAX_AMT() As String
            Get
                Return _USETAX_AMT
            End Get
            Set(ByVal value As String)
                _USETAX_AMT = value
            End Set
        End Property

        Public Property VAT_INV_AMT() As String
            Get
                Return _VAT_INV_AMT
            End Get
            Set(ByVal value As String)
                _VAT_INV_AMT = value
            End Set
        End Property

        Public Property VAT_NONINV_AMT() As String
            Get
                Return _VAT_NONINV_AMT
            End Get
            Set(ByVal value As String)
                _VAT_NONINV_AMT = value
            End Set
        End Property

        Public Property RECV_DIST_LINE_NUM() As String
            Get
                Return _RECV_DIST_LINE_NUM
            End Get
            Set(ByVal value As String)
                _RECV_DIST_LINE_NUM = value
            End Set
        End Property

        Public Property DEPTID() As String
            Get
                Return _DEPTID
            End Get
            Set(ByVal value As String)
                _DEPTID = value
            End Set
        End Property

        Public Property PRODUCT() As String
            Get
                Return _PRODUCT
            End Get
            Set(ByVal value As String)
                _PRODUCT = value
            End Set
        End Property

        Public Property PROEJCT_ID() As String
            Get
                Return _PROEJCT_ID
            End Get
            Set(ByVal value As String)
                _PROEJCT_ID = value
            End Set
        End Property

        Public Property AFFILIATE() As String
            Get
                Return _AFFILIATE
            End Get
            Set(ByVal value As String)
                _AFFILIATE = value
            End Set
        End Property

        Public Property VAT_APORT_CNTRL() As String
            Get
                Return _VAT_APORT_CNTRL
            End Get
            Set(ByVal value As String)
                _VAT_APORT_CNTRL = value
            End Set
        End Property

        Public Property ThirdPartyFreightInvoice() As Boolean
            Get
                Return _ThirdPartyFreightInvoice
            End Get
            Set(ByVal value As Boolean)
                _ThirdPartyFreightInvoice = value
            End Set
        End Property

        Public Property VCHR_CURR_ROW_ID() As String
            Get
                Return _VCHR_CURR_ROW_ID
            End Get
            Set(ByVal value As String)
                _VCHR_CURR_ROW_ID = value
            End Set
        End Property

        Public Property CURR_BUSINESS_UNIT_ID() As Integer
            Get
                Return _CURR_BUSINESS_UNIT_ID
            End Get
            Set(ByVal value As Integer)
                _CURR_BUSINESS_UNIT_ID = value
            End Set
        End Property

        Public Property CURR_VENDOR_CODE() As String
            Get
                Return _CURR_VENDOR_CODE
            End Get
            Set(ByVal value As String)
                _CURR_VENDOR_CODE = value
            End Set
        End Property

        Public Property CURR_BU_CODE() As String
            Get
                Return _CURR_BU_CODE
            End Get
            Set(ByVal value As String)
                _CURR_BU_CODE = value
            End Set
        End Property

#End Region

        ''' <summary>
        ''' Create a new instance of the business object, populating it with the results
        ''' from the call to the GetAPUploads stored procedure.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByRef results As SqlDataReader)
            If (Not results.IsDBNull(results.GetOrdinal("VCHR_HDR_ROW_ID"))) Then
                VCHR_HDR_ROW_ID = results.GetString(results.GetOrdinal("VCHR_HDR_ROW_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("BUSINESS_UNIT_ID"))) Then
                BUSINESS_UNIT_ID = results.GetInt32(results.GetOrdinal("BUSINESS_UNIT_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VOUCHER_ID"))) Then
                VOUCHER_ID = results.GetString(results.GetOrdinal("VOUCHER_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("INVOICE_ID"))) Then
                INVOICE_ID = results.GetString(results.GetOrdinal("INVOICE_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("INVOICE_DT"))) Then
                INVOICE_DT = results.GetDateTime(results.GetOrdinal("INVOICE_DT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VENDOR_SETID"))) Then
                VENDOR_SETID = results.GetString(results.GetOrdinal("VENDOR_SETID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VENDOR_ID"))) Then
                VENDOR_ID = results.GetString(results.GetOrdinal("VENDOR_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VNDR_LOC"))) Then
                VNDR_LOC = results.GetString(results.GetOrdinal("VNDR_LOC"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("ADDRESS_SEQ_NUM"))) Then
                ADDRESS_SEQ_NUM = results.GetString(results.GetOrdinal("ADDRESS_SEQ_NUM"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("GRP_AP_ID"))) Then
                GRP_AP_ID = results.GetString(results.GetOrdinal("GRP_AP_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("ORIGIN"))) Then
                ORIGIN = results.GetString(results.GetOrdinal("ORIGIN"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("OPRID"))) Then
                OPRID = results.GetString(results.GetOrdinal("OPRID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VCHR_TTL_LINES"))) Then
                VCHR_TTL_LINES = results.GetInt32(results.GetOrdinal("VCHR_TTL_LINES"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("ACCOUNTING_DT"))) Then
                ACCOUNTING_DT = results.GetDateTime(results.GetOrdinal("ACCOUNTING_DT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("POST_VOUCHER"))) Then
                POST_VOUCHER = results.GetString(results.GetOrdinal("POST_VOUCHER"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("DST_CNTRL_ID"))) Then
                DST_CNTRL_ID = results.GetString(results.GetOrdinal("DST_CNTRL_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VOUCHER_ID_RELATED"))) Then
                VOUCHER_ID_RELATED = results.GetString(results.GetOrdinal("VOUCHER_ID_RELATED"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("GROSS_AMT"))) Then
                GROSS_AMT = results.GetDecimal(results.GetOrdinal("GROSS_AMT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("DSCNT_AMT"))) Then
                DSCNT_AMT = results.GetString(results.GetOrdinal("DSCNT_AMT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("USETAX_CD"))) Then
                USETAX_CD = results.GetString(results.GetOrdinal("USETAX_CD"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("SALETX_AMT"))) Then
                SALETX_AMT = results.GetString(results.GetOrdinal("SALETX_AMT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("SALETX_CD"))) Then
                SALETX_CD = results.GetString(results.GetOrdinal("SALETX_CD"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("FREIGHT_AMT"))) Then
                FREIGHT_AMT = results.GetString(results.GetOrdinal("FREIGHT_AMT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("DUE_DT"))) Then
                DUE_DT = results.GetString(results.GetOrdinal("DUE_DT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("DSCNT_DUE_DT"))) Then
                DSCNT_DUE_DT = results.GetString(results.GetOrdinal("DSCNT_DUE_DT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("PYMNT_TERMS_CD"))) Then
                PYMNT_TERMS_CD = results.GetString(results.GetOrdinal("PYMNT_TERMS_CD"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("ENTERED_DT"))) Then
                ENTERED_DT = results.GetDateTime(results.GetOrdinal("ENTERED_DT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("TXN_CURRENCY_CD"))) Then
                TXN_CURRENCY_CD = results.GetString(results.GetOrdinal("TXN_CURRENCY_CD"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("RT_TYPE"))) Then
                RT_TYPE = results.GetString(results.GetOrdinal("RT_TYPE"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("RATE_MULT"))) Then
                RATE_MULT = results.GetString(results.GetOrdinal("RATE_MULT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("RATE_DIV"))) Then
                RATE_DIV = results.GetString(results.GetOrdinal("RATE_DIV"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VAT_ENTRD_AMT"))) Then
                VAT_ENTRD_AMT = results.GetString(results.GetOrdinal("VAT_ENTRD_AMT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("MATCH_ACTION"))) Then
                MATCH_ACTION = results.GetString(results.GetOrdinal("MATCH_ACTION"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("MATCH_STATUS_VCHR"))) Then
                MATCH_STATUS_VCHR = results.GetString(results.GetOrdinal("MATCH_STATUS_VCHR"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("BCM_TRAN_TYPE"))) Then
                BCM_TRAN_TYPE = results.GetString(results.GetOrdinal("BCM_TRAN_TYPE"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("CNTRCT_ID"))) Then
                CNTRCT_ID = results.GetString(results.GetOrdinal("CNTRCT_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("REMIT_ADDR_SEQ_NUM"))) Then
                REMIT_ADDR_SEQ_NUM = results.GetString(results.GetOrdinal("REMIT_ADDR_SEQ_NUM"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("CUR_RT_SOURCE"))) Then
                CUR_RT_SOURCE = results.GetString(results.GetOrdinal("CUR_RT_SOURCE"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("DSCNT_AMT_FLG"))) Then
                DSCNT_AMT_FLG = results.GetString(results.GetOrdinal("DSCNT_AMT_FLG"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("DUE_DT_FLG"))) Then
                DUE_DT_FLG = results.GetString(results.GetOrdinal("DUE_DT_FLG"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VCHR_APPRVL_FLG"))) Then
                VCHR_APPRVL_FLG = results.GetString(results.GetOrdinal("VCHR_APPRVL_FLG"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("BUSPROCNAME"))) Then
                BUSPROCNAME = results.GetString(results.GetOrdinal("BUSPROCNAME"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("APPR_RULE_SET"))) Then
                APPR_RULE_SET = results.GetString(results.GetOrdinal("APPR_RULE_SET"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VAT_DCLRTN_POINT"))) Then
                VAT_DCLRTN_POINT = results.GetString(results.GetOrdinal("VAT_DCLRTN_POINT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VAT_CALC_TYPE"))) Then
                VAT_CALC_TYPE = results.GetString(results.GetOrdinal("VAT_CALC_TYPE"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VAT_ENTITY"))) Then
                VAT_ENTITY = results.GetString(results.GetOrdinal("VAT_ENTITY"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VAT_TXN_TYPE_CD"))) Then
                VAT_TXN_TYPE_CD = results.GetString(results.GetOrdinal("VAT_TXN_TYPE_CD"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("TAX_CD_VAT"))) Then
                TAX_CD_VAT = results.GetString(results.GetOrdinal("TAX_CD_VAT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VAT_RCRD_INPT_FLG"))) Then
                VAT_RCRD_INPT_FLG = results.GetString(results.GetOrdinal("VAT_RCRD_INPT_FLG"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VAT_RCRD_OUTPT_FLG"))) Then
                VAT_RCRD_OUTPT_FLG = results.GetString(results.GetOrdinal("VAT_RCRD_OUTPT_FLG"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VAT_RECOVERY_PCT"))) Then
                VAT_RECOVERY_PCT = results.GetString(results.GetOrdinal("VAT_RECOVERY_PCT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VAT_CALC_GROSS_NET"))) Then
                VAT_CALC_GROSS_NET = results.GetString(results.GetOrdinal("VAT_CALC_GROSS_NET"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VAT_RECALC_FLG"))) Then
                VAT_RECALC_FLG = results.GetString(results.GetOrdinal("VAT_RECALC_FLG"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VAT_CALC_FRGHT_FLG"))) Then
                VAT_CALC_FRGHT_FLG = results.GetString(results.GetOrdinal("VAT_CALC_FRGHT_FLG"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VAT_RGSTRN_SELLER"))) Then
                VAT_RGSTRN_SELLER = results.GetString(results.GetOrdinal("VAT_RGSTRN_SELLER"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("COUNTRY_SHIP_FROM"))) Then
                COUNTRY_SHIP_FROM = results.GetString(results.GetOrdinal("COUNTRY_SHIP_FROM"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("COUNTRY_SHIP_TO"))) Then
                COUNTRY_SHIP_TO = results.GetString(results.GetOrdinal("COUNTRY_SHIP_TO"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("COUNTRY_VAT_BILLFR"))) Then
                COUNTRY_VAT_BILLFR = results.GetString(results.GetOrdinal("COUNTRY_VAT_BILLFR"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("COUNTRY_VAT_BILLTO"))) Then
                COUNTRY_VAT_BILLTO = results.GetString(results.GetOrdinal("COUNTRY_VAT_BILLTO"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VAT_TREATMENT_PUR"))) Then
                VAT_TREATMENT_PUR = results.GetString(results.GetOrdinal("VAT_TREATMENT_PUR"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VAT_EXCPTN_TYPE"))) Then
                VAT_EXCPTN_TYPE = results.GetString(results.GetOrdinal("VAT_EXCPTN_TYPE"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VAT_EXCPTN_CERTIF"))) Then
                VAT_EXCPTN_CERTIF = results.GetString(results.GetOrdinal("VAT_EXCPTN_CERTIF"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VAT_USE_ID"))) Then
                VAT_USE_ID = results.GetString(results.GetOrdinal("VAT_USE_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("DSCNT_PRORATE_FLG"))) Then
                DSCNT_PRORATE_FLG = results.GetString(results.GetOrdinal("DSCNT_PRORATE_FLG"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("USETAX_PRORATE_FLG"))) Then
                USETAX_PRORATE_FLG = results.GetString(results.GetOrdinal("USETAX_PRORATE_FLG"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("SALETX_PRORATE_FLG"))) Then
                SALETX_PRORATE_FLG = results.GetString(results.GetOrdinal("SALETX_PRORATE_FLG"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("FRGHT_PRORATE_FLG"))) Then
                FRGHT_PRORATE_FLG = results.GetString(results.GetOrdinal("FRGHT_PRORATE_FLG"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("IST_TXN_FLG"))) Then
                IST_TXN_FLG = results.GetString(results.GetOrdinal("IST_TXN_FLG"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("DOC_TYPE"))) Then
                DOC_TYPE = results.GetString(results.GetOrdinal("DOC_TYPE"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("DOC_SEQ_DATE"))) Then
                DOC_SEQ_DATE = results.GetString(results.GetOrdinal("DOC_SEQ_DATE"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("DOC_SEQ_NBR"))) Then
                DOC_SEQ_NBR = results.GetString(results.GetOrdinal("DOC_SEQ_NBR"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VAT_CF_ANLSYS_TYPE"))) Then
                VAT_CF_ANLSYS_TYPE = results.GetString(results.GetOrdinal("VAT_CF_ANLSYS_TYPE"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("DESCR254_MIXED"))) Then
                DESCR254_MIXED = results.GetInt32(results.GetOrdinal("DESCR254_MIXED"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VCHR_LINE_ROW_ID"))) Then
                VCHR_LINE_ROW_ID = results.GetString(results.GetOrdinal("VCHR_LINE_ROW_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("LINE_BUSINESS_UNIT_ID"))) Then
                LINE_BUSINESS_UNIT_ID = results.GetInt32(results.GetOrdinal("LINE_BUSINESS_UNIT_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("LINE_VOUCHER_ID"))) Then
                LINE_VOUCHER_ID = results.GetString(results.GetOrdinal("LINE_VOUCHER_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VOUCHER_LINE_NUM"))) Then
                VOUCHER_LINE_NUM = results.GetInt32(results.GetOrdinal("VOUCHER_LINE_NUM"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("TOTAL_DISTRIBS"))) Then
                TOTAL_DISTRIBS = results.GetInt32(results.GetOrdinal("TOTAL_DISTRIBS"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("BUSINESS_UNIT_PO"))) Then
                BUSINESS_UNIT_PO = results.GetString(results.GetOrdinal("BUSINESS_UNIT_PO"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("PO_ID"))) Then
                PO_ID = results.GetInt32(results.GetOrdinal("PO_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("LINE_NBR"))) Then
                LINE_NBR = results.GetString(results.GetOrdinal("LINE_NBR"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("SCHED_NBR"))) Then
                SCHED_NBR = results.GetString(results.GetOrdinal("SCHED_NBR"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("DESCR"))) Then
                DESCR = results.GetString(results.GetOrdinal("DESCR"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("MERCHANDISE_AMT"))) Then
                MERCHANDISE_AMT = results.GetDecimal(results.GetOrdinal("MERCHANDISE_AMT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("ITEM_SETID"))) Then
                ITEM_SETID = results.GetString(results.GetOrdinal("ITEM_SETID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("INV_ITEM_ID"))) Then
                INV_ITEM_ID = results.GetString(results.GetOrdinal("INV_ITEM_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("QTY_VCHR"))) Then
                QTY_VCHR = results.GetString(results.GetOrdinal("QTY_VCHR"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("STATISTIC_AMT"))) Then
                STATISTIC_AMT = results.GetString(results.GetOrdinal("STATISTIC_AMT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("UNIT_OF_MEASURE"))) Then
                UNIT_OF_MEASURE = results.GetString(results.GetOrdinal("UNIT_OF_MEASURE"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("UNIT_PRICE"))) Then
                UNIT_PRICE = results.GetString(results.GetOrdinal("UNIT_PRICE"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("SALETX_APPL_FLG"))) Then
                SALETX_APPL_FLG = results.GetString(results.GetOrdinal("SALETX_APPL_FLG"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("USETAX_APPL_FLG"))) Then
                USETAX_APPL_FLG = results.GetString(results.GetOrdinal("USETAX_APPL_FLG"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("FRGHT_PRORATE_FLG"))) Then
                FRGHT_PRORATE_FLG = results.GetString(results.GetOrdinal("FRGHT_PRORATE_FLG"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("DSCNT_APPL_FLG"))) Then
                DSCNT_APPL_FLG = results.GetString(results.GetOrdinal("DSCNT_APPL_FLG"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("WTHD_SW"))) Then
                WTHD_SW = results.GetString(results.GetOrdinal("WTHD_SW"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("TAX_CD_VAT"))) Then
                TAX_CD_VAT = results.GetString(results.GetOrdinal("TAX_CD_VAT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VAT_RECOVERY_PCT"))) Then
                VAT_RECOVERY_PCT = results.GetString(results.GetOrdinal("VAT_RECOVERY_PCT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("BUSINESS_UNIT_RECV"))) Then
                BUSINESS_UNIT_RECV = results.GetString(results.GetOrdinal("BUSINESS_UNIT_RECV"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("RECEIVER_ID"))) Then
                RECEIVER_ID = results.GetString(results.GetOrdinal("RECEIVER_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("RECV_LN_NBR"))) Then
                RECV_LN_NBR = results.GetString(results.GetOrdinal("RECV_LN_NBR"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("RECV_SHIP_SEQ_NBR"))) Then
                RECV_SHIP_SEQ_NBR = results.GetString(results.GetOrdinal("RECV_SHIP_SEQ_NBR"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("MATCH_LINE_OPT"))) Then
                MATCH_LINE_OPT = results.GetString(results.GetOrdinal("MATCH_LINE_OPT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("DISTRIB_MTHD_FLG"))) Then
                DISTRIB_MTHD_FLG = results.GetString(results.GetOrdinal("DISTRIB_MTHD_FLG"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("TXN_CURRENCY_CD"))) Then
                TXN_CURRENCY_CD = results.GetString(results.GetOrdinal("TXN_CURRENCY_CD"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("BASE_CURRENCY"))) Then
                BASE_CURRENCY = results.GetString(results.GetOrdinal("BASE_CURRENCY"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("CURRENCY_CD"))) Then
                CURRENCY_CD = results.GetString(results.GetOrdinal("CURRENCY_CD"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("SHIPTO_ID"))) Then
                SHIPTO_ID = results.GetString(results.GetOrdinal("SHIPTO_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("SUT_BASE_ID"))) Then
                SUT_BASE_ID = results.GetString(results.GetOrdinal("SUT_BASE_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("TAX_CD_SUT"))) Then
                TAX_CD_SUT = results.GetString(results.GetOrdinal("TAX_CD_SUT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("SUT_EXCPTN_TYPE"))) Then
                SUT_EXCPTN_TYPE = results.GetString(results.GetOrdinal("SUT_EXCPTN_TYPE"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("SUT_EXCPTN_CERTIF"))) Then
                SUT_EXCPTN_CERTIF = results.GetString(results.GetOrdinal("SUT_EXCPTN_CERTIF"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("SUT_APPLICABILITY"))) Then
                SUT_APPLICABILITY = results.GetString(results.GetOrdinal("SUT_APPLICABILITY"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("WTHD_SETID"))) Then
                WTHD_SETID = results.GetString(results.GetOrdinal("WTHD_SETID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("WTHD_CD"))) Then
                WTHD_CD = results.GetString(results.GetOrdinal("WTHD_CD"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VAT_APPL_FLG"))) Then
                VAT_APPL_FLG = results.GetString(results.GetOrdinal("VAT_APPL_FLG"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VAT_APPLICABILITY"))) Then
                VAT_APPLICABILITY = results.GetString(results.GetOrdinal("VAT_APPLICABILITY"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VAT_TXN_TYPE_CD"))) Then
                VAT_TXN_TYPE_CD = results.GetString(results.GetOrdinal("VAT_TXN_TYPE_CD"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("NATURE_OF_TXN1"))) Then
                NATURE_OF_TXN1 = results.GetString(results.GetOrdinal("NATURE_OF_TXN1"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("NATURE_OF_TXN2"))) Then
                NATURE_OF_TXN2 = results.GetString(results.GetOrdinal("NATURE_OF_TXN2"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VAT_USE_ID"))) Then
                VAT_USE_ID = results.GetString(results.GetOrdinal("VAT_USE_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VCHR_DIST_ROW_ID"))) Then
                VCHR_DIST_ROW_ID = results.GetString(results.GetOrdinal("VCHR_DIST_ROW_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("DIST_BUSINESS_UNIT_ID"))) Then
                DIST_BUSINESS_UNIT_ID = results.GetInt32(results.GetOrdinal("DIST_BUSINESS_UNIT_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("DIST_VOUCHER_ID"))) Then
                DIST_VOUCHER_ID = results.GetString(results.GetOrdinal("DIST_VOUCHER_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("DIST_VOUCHER_LINE_NUM"))) Then
                DIST_VOUCHER_LINE_NUM = results.GetInt32(results.GetOrdinal("DIST_VOUCHER_LINE_NUM"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("DISTRIB_LINE_NUM"))) Then
                DISTRIB_LINE_NUM = results.GetInt32(results.GetOrdinal("DISTRIB_LINE_NUM"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("BUSINESS_UNIT_GL"))) Then
                BUSINESS_UNIT_GL = results.GetInt32(results.GetOrdinal("BUSINESS_UNIT_GL"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("ACCOUNT"))) Then
                ACCOUNT = results.GetString(results.GetOrdinal("ACCOUNT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("STATISTIC_CODE"))) Then
                STATISTIC_CODE = results.GetString(results.GetOrdinal("STATISTIC_CODE"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("STATISTIC_AMOUNT"))) Then
                STATISTIC_AMOUNT = results.GetString(results.GetOrdinal("STATISTIC_AMOUNT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("QTY_VCHR"))) Then
                QTY_VCHR = results.GetString(results.GetOrdinal("QTY_VCHR"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("JRNL_LN_REF"))) Then
                JRNL_LN_REF = results.GetString(results.GetOrdinal("JRNL_LN_REF"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("OPEN_ITEM_STATUS"))) Then
                OPEN_ITEM_STATUS = results.GetString(results.GetOrdinal("OPEN_ITEM_STATUS"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("DISTRIB_DESCR"))) Then
                DISTRIB_DESCR = results.GetString(results.GetOrdinal("DISTRIB_DESCR"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("DIST_MERCHANDISE_AMT"))) Then
                DIST_MERCHANDISE_AMT = results.GetDecimal(results.GetOrdinal("DIST_MERCHANDISE_AMT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("BUSINESS_UNIT_PO"))) Then
                BUSINESS_UNIT_PO = results.GetString(results.GetOrdinal("BUSINESS_UNIT_PO"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("DISTRIB_PO_ID"))) Then
                DISTRIB_PO_ID = results.GetInt32(results.GetOrdinal("DISTRIB_PO_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("DISTRIB_LINE_NBR"))) Then
                DISTRIB_LINE_NBR = results.GetString(results.GetOrdinal("DISTRIB_LINE_NBR"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("DISTRIB_SCHED_NBR"))) Then
                DISTRIB_SCHED_NBR = results.GetString(results.GetOrdinal("DISTRIB_SCHED_NBR"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("PO_DIST_LINE_NUM"))) Then
                PO_DIST_LINE_NUM = results.GetString(results.GetOrdinal("PO_DIST_LINE_NUM"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("BUSINESS_UNIT_PC"))) Then
                BUSINESS_UNIT_PC = results.GetString(results.GetOrdinal("BUSINESS_UNIT_PC"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("ACTIVITY_ID"))) Then
                ACTIVITY_ID = results.GetString(results.GetOrdinal("ACTIVITY_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("ANALYSIS_TYPE"))) Then
                ANALYSIS_TYPE = results.GetString(results.GetOrdinal("ANALYSIS_TYPE"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("RESOURCE_TYPE"))) Then
                RESOURCE_TYPE = results.GetString(results.GetOrdinal("RESOURCE_TYPE"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("RESOURCE_CATEGORY"))) Then
                RESOURCE_CATEGORY = results.GetString(results.GetOrdinal("RESOURCE_CATEGORY"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("RESOURCE_SUB_CAT"))) Then
                RESOURCE_SUB_CAT = results.GetString(results.GetOrdinal("RESOURCE_SUB_CAT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("ASSET_FLG"))) Then
                ASSET_FLG = results.GetString(results.GetOrdinal("ASSET_FLG"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("BUSINESS_UNIT_AM"))) Then
                BUSINESS_UNIT_AM = results.GetString(results.GetOrdinal("BUSINESS_UNIT_AM"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("ASSET_ID"))) Then
                ASSET_ID = results.GetString(results.GetOrdinal("ASSET_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("PROFILE_ID"))) Then
                PROFILE_ID = results.GetString(results.GetOrdinal("PROFILE_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("FREIGHT_AMT"))) Then
                FREIGHT_AMT = results.GetString(results.GetOrdinal("FREIGHT_AMT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("SALETX_AMT"))) Then
                SALETX_AMT = results.GetString(results.GetOrdinal("SALETX_AMT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("USETAX_AMT"))) Then
                USETAX_AMT = results.GetString(results.GetOrdinal("USETAX_AMT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VAT_TXN_TYPE_CD"))) Then
                VAT_TXN_TYPE_CD = results.GetString(results.GetOrdinal("VAT_TXN_TYPE_CD"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VAT_INV_AMT"))) Then
                VAT_INV_AMT = results.GetString(results.GetOrdinal("VAT_INV_AMT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VAT_NONINV_AMT"))) Then
                VAT_NONINV_AMT = results.GetString(results.GetOrdinal("VAT_NONINV_AMT"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("BUSINESS_UNIT_RECV"))) Then
                BUSINESS_UNIT_RECV = results.GetString(results.GetOrdinal("BUSINESS_UNIT_RECV"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("RECEIVER_ID"))) Then
                RECEIVER_ID = results.GetString(results.GetOrdinal("RECEIVER_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("RECV_LN_NBR"))) Then
                RECV_LN_NBR = results.GetString(results.GetOrdinal("RECV_LN_NBR"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("RECV_SHIP_SEQ_NBR"))) Then
                RECV_SHIP_SEQ_NBR = results.GetString(results.GetOrdinal("RECV_SHIP_SEQ_NBR"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("RECV_DIST_LINE_NUM"))) Then
                RECV_DIST_LINE_NUM = results.GetString(results.GetOrdinal("RECV_DIST_LINE_NUM"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("DEPTID"))) Then
                DEPTID = Convert.ToString(results.GetInt32(results.GetOrdinal("DEPTID")))
            Else
                DEPTID = ""
            End If

            If (Not results.IsDBNull(results.GetOrdinal("PRODUCT"))) Then
                PRODUCT = Convert.ToString(results.GetInt32(results.GetOrdinal("PRODUCT")))
            Else
                PRODUCT = ""
            End If

            If (Not results.IsDBNull(results.GetOrdinal("PROEJCT_ID"))) Then
                PROEJCT_ID = results.GetString(results.GetOrdinal("PROEJCT_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("AFFILIATE"))) Then
                AFFILIATE = results.GetString(results.GetOrdinal("AFFILIATE"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VAT_APORT_CNTRL"))) Then
                VAT_APORT_CNTRL = results.GetString(results.GetOrdinal("VAT_APORT_CNTRL"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("OrderHeader_ID"))) Then
                OrderHeaderID = results.GetInt32(results.GetOrdinal("OrderHeader_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("Freight3Party"))) Then
                If results.GetInt32(results.GetOrdinal("Freight3Party")) = 1 Then
                    _ThirdPartyFreightInvoice = True
                Else
                    _ThirdPartyFreightInvoice = False
                End If
            End If

            If (Not results.IsDBNull(results.GetOrdinal("VCHR_CURR_ROW_ID"))) Then
                VCHR_CURR_ROW_ID = results.GetString(results.GetOrdinal("VCHR_CURR_ROW_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("CURR_BUSINESS_UNIT_ID"))) Then
                CURR_BUSINESS_UNIT_ID = results.GetInt32(results.GetOrdinal("CURR_BUSINESS_UNIT_ID"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("CURR_VENDOR_CODE"))) Then
                CURR_VENDOR_CODE = results.GetString(results.GetOrdinal("CURR_VENDOR_CODE"))
            End If

            If (Not results.IsDBNull(results.GetOrdinal("CURR_BU_CODE"))) Then
                CURR_BU_CODE = results.GetString(results.GetOrdinal("CURR_BU_CODE"))
            End If
        End Sub
    End Class
End Namespace
