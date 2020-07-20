CREATE TABLE gpm.price(
    price_id BIGINT NOT NULL GENERATED ALWAYS AS IDENTITY,
    gpm_id UUID,
    region character varying(2) NOT NULL, 
    scan_code VARCHAR(13) NOT NULL,
    item_id INT NOT NULL,
    business_unit_id int NOT NULL,
    start_date TIMESTAMP(0) WITHOUT TIME ZONE NOT NULL,
    end_date TIMESTAMP(0) WITHOUT TIME ZONE,
    price NUMERIC(9,2) NOT NULL,
    percent_off NUMERIC(5,2),
    price_type VARCHAR(3) NOT NULL,
    price_type_attribute VARCHAR(10) NOT NULL,
    sellable_uom VARCHAR(3) NOT NULL,
    currency_code VARCHAR(3),
    multiple SMALLINT NOT NULL,
    tag_expiration_date TIMESTAMP(0) WITHOUT TIME ZONE,
    insert_date_utc TIMESTAMP(6) WITHOUT TIME ZONE NOT NULL CONSTRAINT df_price_insertdateutc CURRENT_TIMESTAMP,
    modified_date_utc TIMESTAMP(6) WITHOUT TIME ZONE
);

CREATE TABLE gpm.price_sequence (
    MessageSequenceID BIGINT NOT NULL GENERATED ALWAYS AS IDENTITY,
    item_id INT NOT NULL,
    business_unit_id int NOT NULL,
    patch_family_id VARCHAR(50) NOT NULL, -- ItemID-BU string
    sequence_id INT NOT NULL, -- sequence of message for patch family id
    message_id VARCHAR(50) NOT NULL,
    insert_date_utc TIMESTAMP(6) WITHOUT TIME ZONE NOT NULL CONSTRAINT df_pricesequence_insertdateutc CURRENT_TIMESTAMP,
    modified_date_utc TIMESTAMP(6) WITHOUT TIME ZONE
);