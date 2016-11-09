--Run this script in Oracle SQL Developer to identify items in regional IRMA
--but not in VIM. 

--UserName: vim_user_icon
--Password: v1mus3r1c0n
--Connection Type: TNS
--Network Alias:   VIMPRD

select
   ltrim(to_char(ir.UPC), '0')     REG_UPC,
   ir.REGION,
   ltrim(to_char(ir.NAT_UPC), '0') REG_NAT_UPC,
   ir.LONG_DESCRIPTION             REG_LONG_DESCRIPTION,
   ir.POS_DESCRIPTION              REG_POS_DESCRIPTION,
   ir.ITEM_SIZE                    REG_ITEM_SIZE,
   ir.ITEM_UOM                     REG_ITEM_SIZE_UOM,
   ir.BRAND                        REG_BRAND,
   CASE
   ir.ITEM_STATUS                  
   WHEN 'A' THEN 'Active'
   WHEN 'S' THEN 'Discontinued'
   WHEN 'N' THEN 'Not Available'
   END                             REG_ITEM_STATUS
from
   vim.item_region ir
   where not exists (select *
                       from vim.Item_Master im
                      where im.nat_upc = ir.upc
          )
order by ir.UPC, ir.NAT_UPC, REGION
