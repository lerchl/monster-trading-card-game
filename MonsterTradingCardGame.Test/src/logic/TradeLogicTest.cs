using System;
using MonsterTradingCardGame.Data.Cards;
using MonsterTradingCardGame.Data.Trade;
using MonsterTradingCardGame.Data.User;
using MonsterTradingCardGame.Logic;
using MonsterTradingCardGame.Logic.Exceptions;
using MonsterTradingCardGame.Server;
using Moq;
using NUnit.Framework;

namespace MonsterTradingCardGame.Test.Logic {

    internal class TradeLogicTest {

        private static readonly Token _token = new(Guid.NewGuid(), "username", UserRole.Regular);
        private static readonly Card _ownedCard = new(Guid.NewGuid(), "card", ElementType.Normal, 10, CardType.Monster, null, _token.UserId);
        private static readonly Card _notOwnedCard = new(Guid.NewGuid(), "card", ElementType.Normal, 10, CardType.Monster, null, Guid.NewGuid());

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        [Test]
        public void TestCreate() {
            // Arrange
            Trade trade = new(Guid.NewGuid(), _token.UserId, _ownedCard.Id, CardType.Monster, 10);
            Mock<CardRepository> cardRepositoryMock = new() { CallBase = true };
            cardRepositoryMock.Setup(r => r.FindById(_ownedCard.Id)).Returns(_ownedCard);
            Mock<TradeRepository> tradeRepositoryMock = new() { CallBase = true };
            tradeRepositoryMock.Setup(r => r.Save(trade)).Returns(trade);
            TradeLogic tradeLogic = new(tradeRepositoryMock.Object, cardRepositoryMock.Object, new());

            // Assert / Act
            Trade createdTrade = null!;
            Assert.DoesNotThrow(() => {
                createdTrade = tradeLogic.Create(_token, trade);
            });
            Assert.AreEqual(_token.UserId, createdTrade.UserId);
        }

        [Test]
        public void TestCreateCardNotOwned() {
            // Arrange
            Trade trade = new(Guid.NewGuid(), _token.UserId, _notOwnedCard.Id, CardType.Monster, 10);
            Mock<CardRepository> cardRepositoryMock = new() { CallBase = true };
            cardRepositoryMock.Setup(r => r.FindById(_notOwnedCard.Id)).Returns(_notOwnedCard);
            TradeLogic tradeLogic = new(new(), cardRepositoryMock.Object, new());

            // Assert / Act
            var e = Assert.Throws<ForbiddenException>(() => tradeLogic.Create(_token, trade))!;
            Assert.AreEqual("You can only create trade offers for your own cards", e.Message);
        }

        [Test]
        public void TestTrade() {
            // Arrange
            Trade trade 
        }
    }
}