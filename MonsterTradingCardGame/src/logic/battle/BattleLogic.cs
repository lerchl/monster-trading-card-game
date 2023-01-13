using MonsterTradingCardGame.Data.BattleNS;
using MonsterTradingCardGame.Data.Cards;
using MonsterTradingCardGame.Data.Deck;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Logic.BattleNS {

    internal class BattleLogic : Logic<BattleRepository, Battle> {

        private static readonly Logger<BattleLogic> _logger = new();

        private readonly DeckLogic _deckLogic = new();
        private readonly CardLogic _cardLogic = new();
        private readonly UserLogic _userLogic = new();

        // /////////////////////////////////////////////////////////////////////
        // Init
        // /////////////////////////////////////////////////////////////////////

        public BattleLogic() : base(new BattleRepository()) {
            // noop
        }

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        public static string Battle(Token token) {
            BattleQueueEntry entry = BattleHandler.Instance.QueueBattle(token.UserId);
            entry.ResetEvent.Wait();
            // battle log cannot be null here because
            // this thread waited for the battle to be
            // executed in a seperate thread
            return entry.Battle.BattleLog!.ToString();
        }

        public void ExecuteBattle(Battle battle) {
            BattleLog battleLog = new();
            Deck user1Deck = _deckLogic.FindByUser(battle.User1Id);
            Deck user2Deck = _deckLogic.FindByUser(battle.User2Id);

            List<Card> user1Cards = user1Deck.GetCardIds().Select(_cardLogic.FindById).ToList();
            List<Card> user2Cards = user2Deck.GetCardIds().Select(_cardLogic.FindById).ToList();

            Random random = new();
            int round = 1;
            battleLog.Add(0, $"User 1 ({battle.User1Id}) vs User 2 ({battle.User2Id})");
            while (user1Cards.Count > 0 && user2Cards.Count > 0 && round <= 100) {
                Card user1Card = user1Cards[random.Next(user1Cards.Count)];
                Card user2Card = user2Cards[random.Next(user2Cards.Count)];

                battleLog.Add(round++, $"User 1's {user1Card.Name} ({user1Card.ElementType}) " +
                        $"vs User 2's {user2Card.Name} ({user2Card.ElementType})");

                if (ConsiderSpeciality(user1Card, user2Card, user1Cards, user2Cards) ||
                        ConsiderSpeciality(user2Card, user1Card, user2Cards, user1Cards)) {
                    continue;
                }

                double damageUser1Card = user1Card.Damage;
                double damageUser2Card = user2Card.Damage;

                if (user1Card.CardType == CardType.Spell
                        || user2Card.CardType == CardType.Spell) {
                    damageUser1Card *= DamageMultiplier(user1Card.ElementType, user2Card.ElementType);
                    damageUser2Card *= DamageMultiplier(user2Card.ElementType, user1Card.ElementType);
                }

                if (user1Card.Damage > user2Card.Damage) {
                    user2Cards.Remove(user2Card);
                    user1Cards.Add(user2Card);
                } else if (user1Card.Damage < user2Card.Damage) {
                    user1Cards.Remove(user1Card);
                    user2Cards.Add(user1Card);
                }
            }

            // user 2 has won, if user 1 has no cards left
            // otherwise user 1 has won
            battle.WinnerId = user1Cards.Count == 0 ? battle.User2Id :
                    (user2Cards.Count == 0 ? battle.User1Id : null);
            Save(battle);

            // update elo
            if (battle.WinnerId != null) {
                battleLog.Add(round, $"User with the Id {battle.WinnerId} has won the battle.");
                _userLogic.UpdateElo(battle.User1Id, battle.User2Id, battle.WinnerId.Value);
            } else {
                battleLog.Add(round, "The battle has ended in a draw.");
            }

            battle.BattleLog = battleLog;
        }

        // /////////////////////////////////////////////////////////////////////
        // Utils
        // /////////////////////////////////////////////////////////////////////

        private static bool ConsiderSpeciality(Card user1Card, Card user2Card, List<Card> user1Cards, List<Card> user2Cards) {
            if ((user1Card.Name.Contains("Dragon") && user2Card.Name.Contains("Goblin"))
                    // Wizards can control Orks so they cannot be attacked
                    || (user1Card.Name.Contains("Wizard") && user2Card.Name.Contains("Orks"))
                    // The armor of Knights is so heavy that water Spells make them drown them instantly
                    || (user1Card.CardType == CardType.Spell && user1Card.ElementType == ElementType.Water && user2Card.Name.Contains("Knight"))
                    // The Kraken is immune against spells
                    || (user1Card.Name.Contains("Kraken") && user2Card.CardType == CardType.Spell)
                    // The fire Elves have known Dragons since they were little and can therefore evade their attacks
                    || (user1Card.Name.Contains("Elve") && user1Card.ElementType == ElementType.Fire && user2Card.Name.Contains("Dragon"))) {
                user2Cards.Remove(user2Card);
                user1Cards.Add(user2Card);
                return true;
            }

            return false;
        }

        private static double DamageMultiplier(ElementType attacking, ElementType defending) {
            if (attacking == defending) {
                return 1;
            } else if (attacking == ElementType.Normal) {
                return defending == ElementType.Fire ? 0.5 : 2;
            } else if (attacking == ElementType.Fire) {
                return defending == ElementType.Water ? 0.5 : 2;
            } else if (attacking == ElementType.Water) {
                return defending == ElementType.Normal ? 0.5 : 2;
            }

            _logger.Warn($"No calculations for attacking type {attacking}!");
            return 1;
        }
    }
}
