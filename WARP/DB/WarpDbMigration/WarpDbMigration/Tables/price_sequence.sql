CREATE TABLE gpm.price_sequence (
    MessageSequenceID BIGINT NOT NULL GENERATED ALWAYS AS IDENTITY,
    item_id INT NOT NULL,
    business_unit_id int NOT NULL,
    patch_family_id VARCHAR(50) NOT NULL, -- ItemID-BU string
    sequence_id INT NOT NULL, -- sequence of message for patch family id
    message_id VARCHAR(50) NOT NULL,
    insert_date_utc TIMESTAMP(6) WITHOUT TIME ZONE NOT NULL,
    modified_date_utc TIMESTAMP(6) WITHOUT TIME ZONE
);