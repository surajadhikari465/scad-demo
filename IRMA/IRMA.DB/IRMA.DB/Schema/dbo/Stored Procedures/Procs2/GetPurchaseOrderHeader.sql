	-- This replaces the WebService "GetPurchaseOrderHeader", which is nearly a straight call of GetPOHeader

	CREATE PROCEDURE dbo.GetPurchaseOrderHeader
		@OrderHeader_ID int
	AS 
	BEGIN
	    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
		SET NOCOUNT ON
	    
	SELECT 
		convert(varchar(13),@OrderHeader_ID) as PONumber,
		convert(varchar,OrderDate,101) as OrderDate,

				-- one potential difference is how this treats a '',
				-- might need a case statement to match the WebService exactly
			isnull(rtrim(ltrim(CompanyName)) + char(13),'') +
			isnull(rtrim(ltrim(Address_Line_1)) + char(13),'') +
			isnull(rtrim(ltrim(Address_Line_2)) + char(13),'') +
			isnull(rtrim(ltrim(City)) + ',','') + 
			isnull(rtrim(ltrim(State)) + char(13),'') +
			isnull(rtrim(ltrim(Zip_Code)) + char(13),'') +
			isnull(rtrim(ltrim(Country)) + char(13),'') +
			isnull('Phone: ' +rtrim(ltrim(Phone)) + char(13),'') +
			isnull('Fax: ' +rtrim(ltrim(Fax)) + char(13),'') 
		as VendorInfo,
		Vendor_Key as VendorVendorKey,
		case when OriginalOrderHeader_Id is not null 
			then convert(varchar(20),@OrderHeader_ID) 
				+ char(13) + char(10) +
				'CM OF PO ' + convert(varchar(20),OriginalOrderHeader_Id) 
				+ char(13) + char(10) +
				isnull(OrderHeaderDesc,'')
			else
				convert(varchar(20),@OrderHeader_ID) 
				+ char(13) + char(10) +
				isnull(OrderHeaderDesc,'')
			end
		as OrderHeaderDesc,
		QuantityDiscount,
		DiscountType,
		UserName,
	-- this SubTeam code was clearly in error in the webservice, not sure if I've reconstructed it correctly
		case when isnull(TransferToSubTeamName,'') = ''
			then TransferSubTeamName 
			else
				TransferToSubTeamName
			end
		as SubTeam,
		Return_Order,
			isnull(convert(varchar(30),Expected_Date),'') -- this may require additional formatting
		as Expected_Date,  
		
				-- one potential difference is how this treats a '',
				-- might need a case statement to match the WebService exactly
			isnull(rtrim(ltrim(RLCompanyName)) + char(13),'') +
			isnull(rtrim(ltrim(RLAddress_Line_1)) + char(13),'') +
			isnull(rtrim(ltrim(RLAddress_Line_2)) + char(13),'') +
			isnull(rtrim(ltrim(RLCity)) + ',','') + 
			isnull(rtrim(ltrim(RLState)) + char(13),'') +
			isnull(rtrim(ltrim(RLZip_Code)) + char(13),'') +
			isnull(rtrim(ltrim(RLCountry)) + char(13),'') +
			isnull('Phone: ' +rtrim(ltrim(RLPhone)) + char(13),'') +
			isnull('Fax: ' +rtrim(ltrim(RLFax)) + char(13),'') 
		as ReceiverInfo,
					-- one potential difference is how this treats a '',
				-- might need a case statement to match the WebService exactly
			isnull(rtrim(ltrim(PLCompanyName)) + char(13),'') +
			isnull(rtrim(ltrim(PLAddress_Line_1)) + char(13),'') +
			isnull(rtrim(ltrim(PLAddress_Line_2)) + char(13),'') +
			isnull(rtrim(ltrim(PLCity)) + ',','') + 
			isnull(rtrim(ltrim(PLState)) + char(13),'') +
			isnull(rtrim(ltrim(PLZip_Code)) + char(13),'') +
			isnull(rtrim(ltrim(PLCountry)) + char(13),'') +
			isnull('Phone: ' +rtrim(ltrim(PLPhone)) + char(13),'') +
			isnull('Fax: ' +rtrim(ltrim(PLFax)) + char(13),'') 
		as PurchaseInfo,
		OrderType_ID,
		ProductType_ID,
			(case when OrderType_Id = 1 then
				'Purchase '
			when OrderType_Id = 2 then
				'Distribution '
			when OrderType_Id = 3 then
				'Transfer '		
			else '' end) + 
			(case when ProductType_ID = 1 then
				'Product '
			when ProductType_ID = 2 then
				'Packaging '
			when ProductType_ID = 3 then
				'Supplies '		
			else '' end) + 
			'Invoice' + char(13) + 
			(case when OrderType_Id = 1 then
				TransferToSubTeamName
			when OrderType_Id = 2 then
				TransferSubTeamName + ' - ' + TransferToSubTeamName
			when OrderType_Id = 3 then
				TransferSubTeamName + ' - ' + TransferToSubTeamName
			else '' end)
		as Title,
			case when DiscountType = 1 then
				'- $' + convert(varchar(13),QuantityDiscount) + ' Cash'
			when DiscountType = 2 then
				'- ' + convert(varchar(13),QuantityDiscount) + '% Item Cost'
			when DiscountType = 4 then
				'- ' + convert(varchar(13),QuantityDiscount) + '% Landed Cost'
			else '' end
		as ShowDiscount
		
	FROM    
		(SELECT TOP 1
			   Users.Phone_Number,
			   Users.Fax_Number,
			   Users.Title,
			   Users.Printer,
			   Users.CoverPage,
			   Users.FullName,
			   Users.User_ID,
			   Users.Email,
			   OrderHeader.OrderHeaderDesc,
			   OrderHeader.Return_Order,
			   OrderHeader.CreatedBy, 
			   OrderHeader.OrderDate,
			   OrderHeader.Fax_Order,
			   OrderHeader.Email_Order,
			   OrderHeader.Expected_Date,
			   OrderHeader.QuantityDiscount, 
			   OrderHeader.DiscountType,
			   OrderHeader.ReceiveLocation_ID,
			   OrderHeader.PurchaseLocation_ID,
			   OrderHeader.Transfer_SubTeam, 
			   TransferSubTeam.SubTeam_Name AS TransferSubTeamName,
			   TransferToSubTeam.SubTeam_Name AS TransferToSubTeamName, 
			   Vendor.CompanyName,
			   Vendor.Address_Line_1,
			   Vendor.Address_Line_2,
			   Vendor.City,
			   Vendor.State,
			   Vendor.Zip_Code, 
			   Vendor.Country,
			   Vendor.Phone,
			   Vendor.Fax,
			   Vendor.Email As EmailAddr,
			   Vendor.Vendor_Key,
			   Vendor.Electronic_Transfer,
			   Vendor.File_Type,
			   RL.PayTo_CompanyName AS RLCompanyName,
			   RL.PayTo_Address_Line_1 AS RLAddress_Line_1, 
			   RL.PayTo_Address_Line_2 AS RLAddress_Line_2,
			   RL.PayTo_City AS RLCity,
			   RL.PayTo_State AS RLState, 
			   RL.PayTo_Zip_Code AS RLZip_Code,
			   RL.PayTo_Country AS RLCountry,
			   RL.PayTo_Phone AS RLPhone,
			   RL.PayTo_Fax AS RLFax,
			   PL.CompanyName AS PLCompanyName,
			   PL.Address_Line_1 AS PLAddress_Line_1,
			   PL.Address_Line_2 AS PLAddress_Line_2, 
			   PL.City AS PLCity, PL.State AS PLState,
			   PL.Zip_Code AS PLZip_Code,
			   PL.Country AS PLCountry,
			   PL.Phone AS PLPhone, 
			   PL.Fax AS PLFax,
			   ReturnOrderList.OrderHeader_ID AS OriginalOrderHeader_ID,
			   Users.UserName,
			   Vendor.FTP_Addr,
			   Vendor.FTP_Path,
			   Vendor.FTP_User,
			   Vendor.FTP_Password,
			   Vendor.PS_Vendor_ID,
			   s.BusinessUnit_ID As Store_No,
			   TUsers.Email As TEmail,
			   OrderHeader.OrderType_ID,
			   OrderHeader.ProductType_ID,
			   --Robert Shurbet TFS8316 20090216 Pull in for email po functionality  
			   OrderHeader.OverrideTransmissionMethod,
			   oto.Target As OverrideTransmissionTarget
		FROM SubTeam TransferToSubTeam (NOLOCK) RIGHT JOIN (
			   SubTeam TransferSubTeam (NOLOCK) RIGHT JOIN (
				 ReturnOrderList RIGHT JOIN (
				   Users (NOLOCK) INNER JOIN (
					 Vendor RL (NOLOCK) INNER JOIN (
					   Vendor PL (NOLOCK) INNER JOIN (
						  Vendor (NOLOCK) INNER JOIN OrderHeader ON (Vendor.Vendor_ID = OrderHeader.Vendor_ID)
						) ON (PL.Vendor_ID = OrderHeader.PurchaseLocation_ID)
					  ) ON (RL.Vendor_ID = OrderHeader.ReceiveLocation_ID)
					) ON (OrderHeader.CreatedBy = Users.User_ID) 
				  ) ON (ReturnOrderList.ReturnOrderHeader_ID = OrderHeader.OrderHeader_ID)
				) ON (TransferSubTeam.SubTeam_No = OrderHeader.Transfer_SubTeam)
			  ) ON (TransferToSubTeam.SubTeam_No = OrderHeader.Transfer_To_SubTeam)
			LEFT JOIN
				StoreSubTeam (nolock)
				ON StoreSubTeam.Store_No = RL.Store_No
				AND StoreSubTeam.SubTeam_No = OrderHeader.Transfer_To_SubTeam
			LEFT JOIN
				UserStoreTeamTitle US (nolock)
				ON US.Store_No = RL.Store_No AND US.Team_No = StoreSubTeam.Team_No
			LEFT JOIN
				Users TUsers (nolock)
				ON TUsers.User_ID = US.User_ID AND TUsers.Email IS NOT NULL
			LEFT JOIN
				OrderTransmissionOverride oto (nolock)  
				ON oto.OrderHeader_ID = OrderHeader.OrderHeader_ID
			INNER JOIN
				Store s (nolock)
				ON s.Store_No = RL.Store_No
		WHERE OrderHeader.OrderHeader_ID = @OrderHeader_ID) as GetPOHeader
	    
		SET NOCOUNT OFF
	END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPurchaseOrderHeader] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPurchaseOrderHeader] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPurchaseOrderHeader] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPurchaseOrderHeader] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPurchaseOrderHeader] TO [IRMAReportsRole]
    AS [dbo];

