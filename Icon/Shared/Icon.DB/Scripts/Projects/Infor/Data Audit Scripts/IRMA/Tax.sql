--Tax
--IRMA uses Icon and Infor's tax abbreviation as its tax class name so that is why they are the same in this extract
select 
	substring(tc.TaxClassDesc, 0, 8) 'Tax Class ID',
	tc.TaxClassDesc 'Tax Abbreviation' 
from TaxClass tc
order by tc.TaxClassDesc