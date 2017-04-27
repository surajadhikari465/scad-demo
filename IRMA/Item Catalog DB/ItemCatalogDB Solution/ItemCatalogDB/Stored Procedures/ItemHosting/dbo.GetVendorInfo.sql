IF EXISTS (
       SELECT *
       FROM   dbo.sysobjects
       WHERE  id = OBJECT_ID(N'[dbo].[GetVendorInfo]')
              AND OBJECTPROPERTY(id, N'IsProcedure') = 1
   )
    DROP PROCEDURE [dbo].[GetVendorInfo]
GO

CREATE PROCEDURE [dbo].[GetVendorInfo]
	@Vendor_ID INT
AS
   -- **************************************************************************
   -- Procedure: GetVendorInfo
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   --
   -- Modification History:
   -- Date			Init	Comment
   -- 11/10/2009	BBB		update existing SP to retrieve BusinessUnit_ID from Vendor table 
   -- 04/06/2010	BBB		Removed BusinessUnit_ID from output
   -- 08/03/2011    MD      Added EinvoiceRequired field TFS 2455
   -- 2012-10-09	KM		Added AllowReceiveAll field TFS 8122
   -- 2013-03-26    MZ		Added ActiveVendor in the SELECT query for MDM (Master Data Management).
   -- **************************************************************************
BEGIN

	SELECT
		v.Vendor_ID,
		v.Vendor_Key,
		v.USER_ID,
		v.CompanyName,
		v.Address_Line_1,
		v.Address_Line_2,
		v.City,
		v.STATE,
		v.Zip_Code,
		v.Country,
		v.County,
		v.Phone,
		v.Phone_Ext,
		v.Fax,
		v.PayTo_CompanyName,
		v.PayTo_Attention,
		v.PayTo_Address_Line_1,
		v.PayTo_Address_Line_2,
		v.PayTo_City,
		v.PayTo_State,
		v.PayTo_Zip_Code,
		v.PayTo_Country,
		v.PayTo_County,
		v.PayTo_Phone,
		v.PayTo_Phone_Ext,
		v.PayTo_Fax,
		v.PS_Vendor_ID,
		v.PS_Export_Vendor_ID,
		v.PS_Location_Code,
		v.PS_Address_Sequence,
		v.Comment,
		v.Customer,
		v.InternalCustomer,
		v.Store_No,
		v.WFM,
		v.Default_GLNumber,
		v.Non_Product_Vendor,
		v.Email,
		v.EFT,
		v.Po_Note,
		v.Receiving_Authorization_Note,
		v.Other_Name,
		v.CaseDistHandlingCharge,
		v.POTransmissionTypeID,
		v.EInvoicing,
		v.CurrencyID,
		v.AllowReceiveAll
		,IsLeadTimeVendor = dbo.fn_IsLeadTimeVendor(@Vendor_ID)
		,LeadTimeDays = v.leadtimedays
		,LeadTimeDayOfWeek = v.leadtimedayofweek
		,v.AccountingContactEmail
		,v.PaymentTermID
		,v.EinvoiceRequired
		,v.ShortpayProhibited
		,v.ActiveVendor
		,v.AllowBarcodePOReport
	FROM
		Vendor v
	WHERE
		v.Vendor_ID = @Vendor_ID
END
GO
