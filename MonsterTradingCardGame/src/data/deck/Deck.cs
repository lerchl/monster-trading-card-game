namespace MonsterTradingCardGame.Data.Deck {

    internal class Deck : Entity {

        [Column(Name = "PLAYER_ID")]
        public Guid PlayerId { get; set; }

        [Column(Name = "CARD_1_ID")]
        public Guid Card1Id { get; set; }

        [Column(Name = "CARD_2_ID")]
        public Guid Card2Id { get; set; }

        [Column(Name = "CARD_3_ID")]
        public Guid Card3Id { get; set; }

        [Column(Name = "CARD_4_ID")]
        public Guid Card4Id { get; set; }

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        public Deck(Guid id, Guid playerId, Guid card1Id, Guid card2Id, Guid card3Id, Guid card4Id) : base(id) {
            PlayerId = playerId;
            Card1Id = card1Id;
            Card2Id = card2Id;
            Card3Id = card3Id;
            Card4Id = card4Id;
        }
    }
}
