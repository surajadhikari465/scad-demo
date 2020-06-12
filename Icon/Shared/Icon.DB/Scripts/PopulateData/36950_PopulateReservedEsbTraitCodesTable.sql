DECLARE @key VARCHAR(128) = '36950_PopulateReservedEsbTraitCodesTable';

IF(Not Exists(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @key))
BEGIN

IF (object_id('tempdb..#reservedEsb') IS NOT NULL)
	DROP TABLE #reservedEsb;

INSERT INTO [dbo].[ReservedEsbTraitCodes] (
	[TraitCode]
	,[Description]
	,[AttributeGroupId]
	)
SELECT a.TraitCode
	,a.Description
	,a.AttributeGroupId
FROM Attributes a

SELECT ta.traitCode
	,ta.traitDesc
	,ta.traitGroupID
INTO #reservedEsb
FROM trait ta

MERGE dbo.ReservedEsbTraitCodes WITH (
		UPDLOCK
		,ROWLOCK
		) r
USING #reservedEsb r2
	ON r2.traitcode = r.Traitcode
WHEN NOT MATCHED
	THEN
		INSERT (
			[TraitCode]
			,[Description]
			,[TraitGroupId]
			)
		VALUES (
			r2.traitcode
			,r2.traitdesc
			,r2.traitGroupId
			);

IF (object_id('tempdb..#reservedEsb') IS NOT NULL)
	DROP TABLE #reservedEsb;
END
ELSE
BEGIN
	print '[' + Convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @key;
END
GO