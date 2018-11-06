if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateNoTagSubteamOverrides]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure dbo.UpdateNoTagSubteamOverrides
go

-- ==========================================================================================
-- Author:		Kyle Milner
-- Create date: 2015-10-10
-- Description:	Updates the default history threshold
--				for the three no-tag rules.
-- ==========================================================================================
-- Modification History:
-- Date       	Init  	TFS   				Comment
-- 11/15/2016	MZ   	21641(PBI17041)		Replace Update command with Merge command to allow
--                                          insertion of new records and deletion of unwanted 
--                                          NoTagThresholdSubteamOverride records.
-- ==========================================================================================

CREATE PROCEDURE [dbo].[UpdateNoTagSubteamOverrides]
	@SubteamOverrides dbo.NoTagSubteamOverrideType readonly
AS
BEGIN
	MERGE NoTagThresholdSubteamOverride AS T
	USING @SubteamOverrides AS S
	ON (T.SubteamNumber = S.SubteamNumber)
	WHEN NOT MATCHED BY TARGET 
		THEN INSERT(SubteamNumber, ThresholdValueDays) VALUES(S.SubteamNumber, S.ThresholdValue)
	WHEN MATCHED
		THEN UPDATE SET T.ThresholdValueDays = S.ThresholdValue;
END
GO