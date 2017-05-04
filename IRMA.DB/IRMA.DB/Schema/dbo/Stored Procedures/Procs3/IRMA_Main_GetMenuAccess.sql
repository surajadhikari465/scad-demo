CREATE PROCEDURE dbo.IRMA_Main_GetMenuAccess
AS
BEGIN
SELECT [MenuAccessID]
      ,[MenuName]
      ,[Visible]
  FROM [MenuAccess] WITH (NOLOCK)
  ORDER BY [MenuName]
  END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IRMA_Main_GetMenuAccess] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[IRMA_Main_GetMenuAccess] TO [IRMAClientRole]
    AS [dbo];

