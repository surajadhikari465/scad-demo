CREATE PROCEDURE dbo.Teams_SaveChanges
	@Team_No int, 
	@Team_Name varchar(100),
	@Team_Abbr varchar(10)
AS
BEGIN
	SET NOCOUNT ON;

	Update  Team
	Set		Team_Name = @Team_Name,
			Team_Abbreviation = @Team_Abbr
	Where	Team_No = @Team_No
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Teams_SaveChanges] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Teams_SaveChanges] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Teams_SaveChanges] TO [IRMAClientRole]
    AS [dbo];

