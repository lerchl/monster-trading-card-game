namespace MonsterTradingCardGame.Data.BattleNS {

    internal class Battle : Entity {

        [Column(Name = "USER_1_ID")]
        public Guid User1Id { get; set; }

        [Column(Name = "USER_2_ID")]
        public Guid User2Id { get; set; }

        public BattleStatus Status { get; set; }

        public DateTime Creation { get; set; }

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        public Battle(Guid id, Guid user1Id, Guid user2Id, BattleStatus status, DateTime creation) : base(id) {
            User1Id = user1Id;
            User2Id = user2Id;
            Status = status;
            Creation = creation;
        }

        public Battle(Guid user1Id) {
            User1Id = user1Id;
        }
    }
}
