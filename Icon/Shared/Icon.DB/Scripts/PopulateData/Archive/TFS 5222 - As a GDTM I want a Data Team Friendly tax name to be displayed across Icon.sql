 set identity_insert dbo.Trait on
  insert into dbo.Trait( [traitID]
      ,[traitCode]
      ,[traitPattern]
      ,[traitDesc]
      ,[traitGroupID]) values (67, 'TRM', '^[\w \-\\/%<>&=\+]{1,50}$', 'Tax Romance', 7)
set identity_insert dbo.Trait off