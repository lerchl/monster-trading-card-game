using MonsterTradingCardGame.Data;
using MonsterTradingCardGame.Data.Cards;
using MonsterTradingCardGame.Logic;
using MonsterTradingCardGame.Logic.Exceptions;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Api.Endpoints {

    internal class Packages {
        private const string URL = "/packages";

        private static readonly PackageLogic _logic = new();

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        [ApiEndpoint(HttpMethod = EHttpMethod.POST, Url = URL)]
        public static Response CreatePackage([Bearer] Token  token,
                                             [Body]   Card[] cards) {
            try {
                return new(HttpCode.CREATED_201, _logic.CreatePackage(token, cards));
            } catch (ForbiddenException e) {
                return new(HttpCode.FORBIDDEN_403, e.Message);
            } catch (NoResultException e) {
                return new(HttpCode.NOT_FOUND_404, e.Message);
            } catch (ConflictException e) {
                return new(HttpCode.CONFLICT_409, e.Message);
            }
        }

        [ApiEndpoint(HttpMethod = EHttpMethod.POST, Url = "/transactions" + URL)]
        public static Response BuyPackages([Bearer] Token token) {
            try {
                return new(HttpCode.OK_200, _logic.AcquirePackage(token));
            } catch (ForbiddenException e) {
                return new(HttpCode.FORBIDDEN_403, e.Message);
            } catch (NoResultException e) {
                return new(HttpCode.NOT_FOUND_404, e.Message);
            }
        }
    }
}