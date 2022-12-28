CREATE TABLE PLAYER (
    ID       UUID   PRIMARY KEY DEFAULT gen_random_uuid(),
    USERNAME TEXT   UNIQUE      NOT NULL,
    PASSWORD TEXT               NOT NULL,
    ROLE     INT                NOT NULL,
    MONEY    INT                NOT NULL,
    NAME     TEXT,
    BIO      TEXT,
    IMAGE    TEXT
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

CREATE TABLE DECK (
    ID        UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    PLAYER_ID UUID             NOT NULL,
    CARD_1_ID UUID             NOT NULL,
    CARD_2_ID UUID             NOT NULL,
    CARD_3_ID UUID             NOT NULL,
    CARD_4_ID UUID             NOT NULL,
    FOREIGN KEY (PLAYER_ID) REFERENCES PLAYER (ID)
);

CREATE TABLE TRADE (
    ID             UUID    PRIMARY KEY DEFAULT gen_random_uuid(),
    PLAYER_ID      UUID                NOT NULL,
    CARD_ID        UUID                NOT NULL,
    CARD_TYPE      INTEGER             NOT NULL,
    MINIMUM_DAMAGE DECIMAL             NOT NULL,
    FOREIGN KEY (PLAYER_ID) REFERENCES PLAYER (ID),
    FOREIGN KEY (CARD_ID)   REFERENCES CARD   (ID)
);
