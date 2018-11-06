CREATE PROCEDURE dbo.GetPOHeader
    @OrderHeader_ID int
AS 
BEGIN
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
    SET NOCOUNT ON
    
    SELECT TOP 1
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
           OrderHeader.CloseDate,
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
           oto.Target As OverrideTransmissionTarget,
           OrderHeader.SupplyTransferToSubTeam
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
          ) ON (TransferToSubTeam.SubTeam_No = ISNULL(OrderHeader.SupplyTransferToSubTeam, OrderHeader.Transfer_To_SubTeam))
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
    WHERE OrderHeader.OrderHeader_ID = @OrderHeader_ID
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPOHeader] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPOHeader] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPOHeader] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPOHeader] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPOHeader] TO [IRMAReportsRole]
    AS [dbo];

