select 
	ib.Brand_Name 'Brand Name', 
	vb.IconBrandId 'Brand ID'
from ItemBrand ib
join ValidatedBrand vb on ib.Brand_ID = vb.IrmaBrandId
order by vb.IconBrandId