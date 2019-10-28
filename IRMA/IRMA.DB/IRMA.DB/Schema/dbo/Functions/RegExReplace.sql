CREATE FUNCTION [dbo].[RegExReplace]
(@expression NVARCHAR (4000), @pattern NVARCHAR (4000), @replace NVARCHAR (4000))
RETURNS NVARCHAR (4000)
AS
 EXTERNAL NAME [RegexProject].[RegexProject.UserDefinedFunctions].[RegExReplace]



GO
EXECUTE sp_addextendedproperty @name = N'AutoDeployed', @value = N'yes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'FUNCTION', @level1name = N'RegExReplace';


GO
EXECUTE sp_addextendedproperty @name = N'SqlAssemblyFile', @value = N'UserDefinedFunctions.vb', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'FUNCTION', @level1name = N'RegExReplace';


GO
EXECUTE sp_addextendedproperty @name = N'SqlAssemblyFileLine', @value = 16, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'FUNCTION', @level1name = N'RegExReplace';

