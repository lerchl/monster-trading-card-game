CREATE TABLE PLAYER (
    ID       UUID   PRIMARY KEY DEFAULT gen_random_uuid(),
    USERNAME TEXT   UNIQUE      NOT NULL,
    PASSWORD TEXT               NOT NULL
);

CREATE TABLE CARD (
    ID           UUID    PRIMARY KEY DEFAULT gen_random_uuid(),
    NAME         TEXT    UNIQUE      NOT NULL,
    ELEMENT_TYPE INTEGER             NOT NULL,
    DAMAGE       DECIMAL             NOT NULL,
    CARD_TYPE    INTEGER             NOT NULL,
    PACKAGE_ID   UUID
);
