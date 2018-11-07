CREATE FUNCTION [dbo].[RegExMatch]
(@Input NVARCHAR (4000), @Pattern NVARCHAR (4000))
RETURNS BIT
AS
 EXTERNAL NAME [RegexProject].[RegexProject.UserDefinedFunctions].[RegExMatch]


GO
EXECUTE sp_addextendedproperty @name = N'AutoDeployed', @value = N'yes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'FUNCTION', @level1name = N'RegExMatch';


GO
EXECUTE sp_addextendedproperty @name = N'SqlAssemblyFile', @value = N'UserDefinedFunctions.vb', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'FUNCTION', @level1name = N'RegExMatch';


GO
EXECUTE sp_addextendedproperty @name = N'SqlAssemblyFileLine', @value = 16, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'FUNCTION', @level1name = N'RegExMatch';

