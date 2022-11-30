CREATE TABLE PLAYER (
    ID       UUID   PRIMARY KEY DEFAULT gen_random_uuid(),
    USERNAME TEXT   UNIQUE      NOT NULL,
    PASSWORD TEXT               NOT NULL,
    MONEY    INT                NOT NULL
);

CREATE TABLE CARD (
    ID           UUID    PRIMARY KEY DEFAULT gen_random_uuid(),
    NAME         TEXT                NOT NULL,
    ELEMENT_TYPE INTEGER             NOT NULL,
    DAMAGE       DECIMAL             NOT NULL,
    CARD_TYPE    INTEGER             NOT NULL,
    PACKAGE_ID   UUID,
    PLAYER_ID    UUID,
    FOREIGN KEY (PLAYER_ID) REFERENCES PLAYER (ID)
);
