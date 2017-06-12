CREATE FUNCTION [dbo].[fn_GetInstanceDataFlagStoreValues]
(
	@FlagKey nvarchar(50)
)
RETURNS @storeReturnTable TABLE
(
	Store_No INT PRIMARY KEY,
	FlagValue BIT
)
AS
BEGIN

	DECLARE @FlagValue BIT = (SELECT FlagValue FROM InstanceDataFlags WHERE FlagKey = @FlagKey);

	INSERT INTO @storeReturnTable
	SELECT
		s.Store_No,
		ISNULL(so.FlagValue, @FlagValue) as FlagValue
	FROM
		Store s
		LEFT JOIN InstanceDataFlagsStoreOverride so ON s.Store_No = so.Store_No
													AND so.FlagKey = @FlagKey
	WHERE
		s.WFM_Store = 1
		OR s.Mega_Store = 1

	RETURN
END
