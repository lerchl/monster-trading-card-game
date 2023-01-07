namespace MonsterTradingCardGame.Data.BattleNS {

    internal class Battle : Entity {

        [Column(Name = "PLAYER_1_ID")]
        public Guid User1Id { get; set; }

        [Column(Name = "PLAYER_2_ID")]
        public Guid User2Id { get; set; }

        [Column(Name = "WINNER_ID")]
        public Guid? WinnerId { get; set; }

        [Transient]
        public BattleLog? BattleLog { get; set; }

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        public Battle(Guid id, Guid user1Id, Guid user2Id, Guid? winnerId) : base(id) {
            User1Id = user1Id;
            User2Id = user2Id;
            WinnerId = winnerId;
        }

        public Battle(Guid userId) : base() {
            User1Id = userId;
        }
    }
}
