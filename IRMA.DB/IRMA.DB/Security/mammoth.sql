CREATE SCHEMA [mammoth]
    AUTHORIZATION [dbo];


GO
GRANT SELECT
    ON SCHEMA::[mammoth] TO [IConInterface];


GO
GRANT DELETE
    ON SCHEMA::[mammoth] TO [MammothRole];


GO
GRANT INSERT
    ON SCHEMA::[mammoth] TO [MammothRole];


GO
GRANT SELECT
    ON SCHEMA::[mammoth] TO [MammothRole];


GO
GRANT UPDATE
    ON SCHEMA::[mammoth] TO [MammothRole];

