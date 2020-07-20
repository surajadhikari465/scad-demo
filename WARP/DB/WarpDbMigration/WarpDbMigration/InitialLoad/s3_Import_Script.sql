-- The final parameter of the aws_s3.table_import_from_s3 function
-- needs to be filled in with the proper values
-- <s3 bucket name> = the name of the s3 bucket deployed with the environment
-- <price file name loaded to s3> = the name of the file key copied to S3

-- note: it seemed odd to put this as part of the scripts
-- because it meant it would have a dependency on a file being
-- copied to s3

SELECT aws_s3.table_import_from_s3(
    'gpm.price',
    'gpm_id,region,scan_code,item_id,business_unit_id,start_date,end_date,price,percent_off,price_type,price_type_attribute,sellable_uom,currency_code,multiple,tag_expiration_date,insert_date_utc,modified_date_utc',
    '(FORMAT csv, NULL '''', HEADER true, DELIMITER ''|'')',
    aws_commons.create_s3_uri('<s3 bucket name>','<price file name loaded to s3>.txt','us-east-1')
);