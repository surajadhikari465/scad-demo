CREATE OR REPLACE FUNCTION gpm.addprice(
	gpm_id uuid,
	region text,
	scan_code text,
	item_id integer,
	business_unit_id integer,
	start_date timestamp without time zone,
	end_date timestamp without time zone,
	price numeric,
	percent_off numeric,
	price_type text,
	price_type_attribute text,
	sellable_uom text,
	currency_code text,
	multiple integer,
	tag_expiration_date timestamp without time zone)
    RETURNS integer
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
AS 
$$
    declare cnt int;
    BEGIN
        Insert into gpm.price
            (
                region,
                gpm_id,
                scan_code,
                item_id,
                business_unit_id,
                start_date,
                end_date,
                price,
                percent_off,
                price_type,
                price_type_attribute,
                sellable_uom,
                currency_code,
                multiple,
                tag_expiration_date,
                modified_date_utc
            ) values (
                  region,
                  gpm_id,
                  scan_code,
                  item_id,
                  business_unit_id,
                  start_date,
                  end_date,
                  price,
                  percent_off,
                  price_type,
                  price_type_attribute,
                  sellable_uom,
                  currency_code,
                  multiple,
                  tag_expiration_date,
                  null
            );
        get diagnostics cnt = row_count;

        return cnt;

    END;

$$;

ALTER FUNCTION gpm.addprice(uuid, text, text, integer, integer, timestamp without time zone, timestamp without time zone, numeric, numeric, text, text, text, text, integer, timestamp without time zone)
    OWNER TO postgres;
