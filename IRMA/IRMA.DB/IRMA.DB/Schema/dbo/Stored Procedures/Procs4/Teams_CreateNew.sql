CREATE PROCEDURE dbo.Teams_CreateNew
	@Team_No int, 
	@Team_Name varchar(100),
	@Team_Abbr varchar(10)
AS
BEGIN
	SET NOCOUNT ON;

	Insert Into Team
	(
		Team_No, 
		Team_Name, 
		Team_Abbreviation
	)
	Values
	(
		@Team_No,
		@Team_Name,
		@Team_Abbr
	)

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Teams_CreateNew] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Teams_CreateNew] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Teams_CreateNew] TO [IRMAClientRole]
    AS [dbo];

