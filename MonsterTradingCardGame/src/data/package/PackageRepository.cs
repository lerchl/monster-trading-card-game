using MonsterTradingCardGame.Data.Cards;

namespace MonsterTradingCardGame.Data.Packages {

    internal class PackageRepository {

        private readonly CardRepository _cardRepository = new();

        // /////////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////////

        public List<Guid> FindAll() {
            return _cardRepository.FindAll()
                                  .Where(card => card.PackageId != null)
                                  .Select(card => card.PackageId)
                                  .Cast<Guid>()
                                  .Distinct()
                                  .ToList();
        }
    }
}
