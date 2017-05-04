CREATE PROCEDURE dbo.EPromotions_ValidatePromotionName
	@PromotionName varchar(100)
AS
BEGIN

	SET NOCOUNT ON;
	-- returns true if @PromotionName does not already exist
	-- returns false if @PromotionName is already used as a promotion name. 
	select case when exists ( select Offer_Id from PromotionalOffer where Description = @PromotionName ) then 'False' else 'True' end as IsValid
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_ValidatePromotionName] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_ValidatePromotionName] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_ValidatePromotionName] TO [IRMAReportsRole]
    AS [dbo];

