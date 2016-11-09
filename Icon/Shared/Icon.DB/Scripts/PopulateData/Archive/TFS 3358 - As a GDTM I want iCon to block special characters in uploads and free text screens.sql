update Trait set traitPattern = '^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,60}$' where traitCode = 'PRD'
update Trait set traitPattern = '^[\x20-\x21\x23-\x2A\x2C-\x5A\x61-\x7A\x9C]{0,25}$' where traitCode = 'POS'
update Trait set traitPattern = '^[\d]{7} [\w \-\\/%<>&=\+]+$' where traitCode = 'ABR'