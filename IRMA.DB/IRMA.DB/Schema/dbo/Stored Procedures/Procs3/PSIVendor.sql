CREATE PROCEDURE [dbo].[PSIVendor]
AS
-- 20071121 DaveStacey - Rewrote joins, added error handling
	BEGIN TRY
		SELECT V.Vendor_ID, V.Vendor_Key, V.CompanyName
		FROM dbo.Vendor V (nolock)
		WHERE EXISTS (SELECT siv.StoreItemVendorID 
						FROM dbo.StoreItemVendor siv (nolock) 
							JOIN dbo.StoreItem si (NOLOCK) ON si.Store_no = SIV.Store_no AND SI.Item_Key = SIV.Item_Key
						WHERE SI.Authorized = 1 AND siv.Vendor_ID = V.Vendor_ID AND siv.PrimaryVendor = 1)
			AND V.PS_Vendor_ID IS NOT NULL
	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT @ErrorMessage = ERROR_MESSAGE(),
			   @ErrorSeverity = ERROR_SEVERITY(),
			   @ErrorState = ERROR_STATE();

		RAISERROR ('PSIVendor failed with  @ErrorMessage' , -- Message text.
				   @ErrorSeverity, -- Severity.
				   @ErrorState -- State.
				   );
	END CATCH
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PSIVendor] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PSIVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PSIVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PSIVendor] TO [IRMAReportsRole]
    AS [dbo];

