CREATE FUNCTION [dbo].[ToNumericString]
(
  @val varchar(255)
)
RETURNS varchar(255)
AS
BEGIN
    DECLARE @mask varchar(50) = '%[^0-9]%';

    While PatIndex(@mask, @val) > 0
      Set @val = Stuff(@val, PatIndex(@mask, @val), 1, '')

    Return @val
END
GO

GRANT EXECUTE ON OBJECT::[dbo].[ToNumericString] TO [IRMAAdminRole] AS [dbo];
GO

GRANT EXECUTE ON OBJECT::[dbo].[ToNumericString] TO [IRMAClientRole] AS [dbo];
GO
