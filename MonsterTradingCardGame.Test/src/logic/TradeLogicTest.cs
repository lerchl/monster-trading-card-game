using System;
using MonsterTradingCardGame.Data.Cards;
using MonsterTradingCardGame.Data.Deck;
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

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        [Test]
        public void TestCreate() {
            // Arrange
            Card myCard = CreateMyCard();
            Trade trade = new(Guid.NewGuid(), _token.UserId, myCard.Id, CardType.Monster, 10);

            Mock<CardRepository> cardRepositoryMock = new() { CallBase = true };
            cardRepositoryMock.Setup(r => r.FindById(myCard.Id)).Returns(myCard);
            Mock<TradeRepository> tradeRepositoryMock = new() { CallBase = true };
            tradeRepositoryMock.Setup(r => r.Save(trade)).Returns(trade);
            TradeLogic tradeLogic = new(tradeRepositoryMock.Object, cardRepositoryMock.Object, new());

            // Act
            Trade createdTrade = tradeLogic.Create(_token, trade);

            // Assert
            Assert.AreEqual(_token.UserId, createdTrade.UserId);
        }

        [Test]
        public void TestCreateCardNotOwned() {
            // Arrange
            Card notMyCard = CreateNotMyCard();
            Trade trade = new(Guid.NewGuid(), _token.UserId, notMyCard.Id, CardType.Monster, 10);

            Mock<CardRepository> cardRepositoryMock = new() { CallBase = true };
            cardRepositoryMock.Setup(r => r.FindById(notMyCard.Id)).Returns(notMyCard);
            TradeLogic tradeLogic = new(new(), cardRepositoryMock.Object, new());

            // Act / Assert
            var e = Assert.Throws<ForbiddenException>(() => tradeLogic.Create(_token, trade))!;
            Assert.AreEqual("You can only create trade offers for your own cards", e.Message);
        }

        [Test]
        public void TestTrade() {
            // Arrange
            Card myCard = CreateMyCard();
            Card notMyCard = CreateNotMyCard();
            Trade trade = new(Guid.NewGuid(), (Guid) notMyCard.UserId!, notMyCard.Id, CardType.Monster, 10);

            Guid myCardUserId = (Guid) myCard.UserId!;
            Guid notMyCardUserId = (Guid) notMyCard.UserId!;

            Mock<TradeRepository> tradeRepositoryMock = new() { CallBase = true };
            tradeRepositoryMock.Setup(r => r.FindById(trade.Id)).Returns(trade);
            tradeRepositoryMock.Setup(r => r.DeleteByCardId(notMyCard.Id));

            Mock<CardRepository> cardRepositoryMock = new() { CallBase = true };
            cardRepositoryMock.Setup(r => r.FindById(notMyCard.Id)).Returns(notMyCard);
            cardRepositoryMock.Setup(r => r.FindById(myCard.Id)).Returns(myCard);
            cardRepositoryMock.Setup(r => r.Save(It.IsAny<Card>())).Returns<Card>(c => c);

            Mock<DeckRepository> deckRepositoryMock = new() { CallBase = true };
            deckRepositoryMock.Setup(r => r.IsCardInDeck(_token.UserId, myCard.Id)).Returns(false);

            TradeLogic tradeLogic = new(tradeRepositoryMock.Object, cardRepositoryMock.Object, deckRepositoryMock.Object);

            // Act
            tradeLogic.Trade(_token, trade.Id, myCard.Id);

            // Assert
            Assert.AreEqual(myCardUserId, notMyCard.UserId);
            Assert.AreEqual(notMyCardUserId, myCard.UserId);

            tradeRepositoryMock.Verify(r => r.FindById(trade.Id), Times.Once);
            cardRepositoryMock.Verify(r => r.FindById(myCard.Id), Times.Once);
            deckRepositoryMock.Verify(r => r.IsCardInDeck(_token.UserId, myCard.Id), Times.Once);
            cardRepositoryMock.Verify(r => r.FindById(notMyCard.Id), Times.Once);
            cardRepositoryMock.Verify(r => r.Save(myCard), Times.Once);
            cardRepositoryMock.Verify(r => r.Save(notMyCard), Times.Once);
            tradeRepositoryMock.Verify(r => r.DeleteByCardId(notMyCard.Id), Times.Once);
        }

        [Test]
        public void TestTradeWithYourself() {
            // Arrange
            Card myCard = CreateMyCard();
            Card notMyCard = CreateNotMyCard();
            Trade trade = new(Guid.NewGuid(), _token.UserId, myCard.Id, CardType.Monster, 10);

            Guid myCardUserId = (Guid) myCard.UserId!;
            Guid notMyCardUserId = (Guid) notMyCard.UserId!;

            Mock<TradeRepository> tradeRepositoryMock = new() { CallBase = true };
            tradeRepositoryMock.Setup(r => r.FindById(trade.Id)).Returns(trade);

            Mock<CardRepository> cardRepositoryMock = new() { CallBase = true };
            cardRepositoryMock.Setup(r => r.FindById(notMyCard.Id)).Returns(notMyCard);

            Mock<DeckRepository> deckRepositoryMock = new() { CallBase = true };

            TradeLogic tradeLogic = new(tradeRepositoryMock.Object, cardRepositoryMock.Object, deckRepositoryMock.Object);

            // Act / Assert
            // using notMyCard to trade to be able to assert that owners are not changed
            var e = Assert.Throws<ForbiddenException>(() => tradeLogic.Trade(_token, trade.Id, notMyCard.Id))!;
            Assert.AreEqual("You can't trade with yourself", e.Message);

            Assert.AreEqual(myCardUserId, myCard.UserId);
            Assert.AreEqual(notMyCardUserId, notMyCard.UserId);

            tradeRepositoryMock.Verify(r => r.FindById(trade.Id), Times.Once);
            cardRepositoryMock.Verify(r => r.FindById(notMyCard.Id), Times.Once);
            deckRepositoryMock.Verify(r => r.IsCardInDeck(_token.UserId, myCard.Id), Times.Never);
            cardRepositoryMock.Verify(r => r.FindById(myCard.Id), Times.Never);
            cardRepositoryMock.Verify(r => r.Save(myCard), Times.Never);
            cardRepositoryMock.Verify(r => r.Save(notMyCard), Times.Never);
            tradeRepositoryMock.Verify(r => r.DeleteByCardId(notMyCard.Id), Times.Never);
        }

        [Test]
        public void TestTradeNotYourCard() {
            // Arrange
            Card myCard = CreateMyCard();
            Card notMyCard = CreateNotMyCard();
            Trade trade = new(Guid.NewGuid(), Guid.NewGuid(), notMyCard.Id, CardType.Monster, 10);

            Guid myCardUserId = (Guid) myCard.UserId!;
            Guid notMyCardUserId = (Guid) notMyCard.UserId!;

            Mock<TradeRepository> tradeRepositoryMock = new() { CallBase = true };
            tradeRepositoryMock.Setup(r => r.FindById(trade.Id)).Returns(trade);

            Mock<CardRepository> cardRepositoryMock = new() { CallBase = true };
            cardRepositoryMock.Setup(r => r.FindById(notMyCard.Id)).Returns(notMyCard);

            Mock<DeckRepository> deckRepositoryMock = new() { CallBase = true };

            TradeLogic tradeLogic = new(tradeRepositoryMock.Object, cardRepositoryMock.Object, deckRepositoryMock.Object);

            // Act / Assert
            var e = Assert.Throws<ForbiddenException>(() => tradeLogic.Trade(_token, trade.Id, notMyCard.Id))!;
            Assert.AreEqual("You can only trade your own cards", e.Message);

            Assert.AreEqual(myCardUserId, myCard.UserId);
            Assert.AreEqual(notMyCardUserId, notMyCard.UserId);

            tradeRepositoryMock.Verify(r => r.FindById(trade.Id), Times.Once);
            cardRepositoryMock.Verify(r => r.FindById(notMyCard.Id), Times.Once);
            deckRepositoryMock.Verify(r => r.IsCardInDeck(_token.UserId, myCard.Id), Times.Never);
            cardRepositoryMock.Verify(r => r.FindById(myCard.Id), Times.Never);
            cardRepositoryMock.Verify(r => r.Save(myCard), Times.Never);
            cardRepositoryMock.Verify(r => r.Save(notMyCard), Times.Never);
            tradeRepositoryMock.Verify(r => r.DeleteByCardId(notMyCard.Id), Times.Never);
        }

        [Test]
        public void TestTradeCardInDeck() {
            // Arrange
            Card myCard = CreateMyCard();
            Card notMyCard = CreateNotMyCard();
            Trade trade = new(Guid.NewGuid(), Guid.NewGuid(), notMyCard.Id, CardType.Monster, 10);

            Guid myCardUserId = (Guid) myCard.UserId!;
            Guid notMyCardUserId = (Guid) notMyCard.UserId!;

            Mock<TradeRepository> tradeRepositoryMock = new() { CallBase = true };
            tradeRepositoryMock.Setup(r => r.FindById(trade.Id)).Returns(trade);

            Mock<CardRepository> cardRepositoryMock = new() { CallBase = true };
            cardRepositoryMock.Setup(r => r.FindById(myCard.Id)).Returns(myCard);

            Mock<DeckRepository> deckRepositoryMock = new() { CallBase = true };
            deckRepositoryMock.Setup(r => r.IsCardInDeck(_token.UserId, myCard.Id)).Returns(true);

            TradeLogic tradeLogic = new(tradeRepositoryMock.Object, cardRepositoryMock.Object, deckRepositoryMock.Object);

            // Act / Assert
            var e = Assert.Throws<ForbiddenException>(() => tradeLogic.Trade(_token, trade.Id, myCard.Id))!;
            Assert.AreEqual("You can't trade cards that are in your deck", e.Message);

            Assert.AreEqual(myCardUserId, myCard.UserId);
            Assert.AreEqual(notMyCardUserId, notMyCard.UserId);

            tradeRepositoryMock.Verify(r => r.FindById(trade.Id), Times.Once);
            cardRepositoryMock.Verify(r => r.FindById(myCard.Id), Times.Once);
            deckRepositoryMock.Verify(r => r.IsCardInDeck(_token.UserId, myCard.Id), Times.Once);
            cardRepositoryMock.Verify(r => r.FindById(notMyCard.Id), Times.Never);
            cardRepositoryMock.Verify(r => r.Save(myCard), Times.Never);
            cardRepositoryMock.Verify(r => r.Save(notMyCard), Times.Never);
            tradeRepositoryMock.Verify(r => r.DeleteByCardId(notMyCard.Id), Times.Never);
        }

        [Test]
        public void TestTradeCardTypesDoNotMatch() {
            // Arrange
            Card myCard = CreateMyCard();
            Card notMyCard = CreateNotMyCard();
            Trade trade = new(Guid.NewGuid(), Guid.NewGuid(), notMyCard.Id, CardType.Spell, 10);

            Guid myCardUserId = (Guid) myCard.UserId!;
            Guid notMyCardUserId = (Guid) notMyCard.UserId!;

            Mock<TradeRepository> tradeRepositoryMock = new() { CallBase = true };
            tradeRepositoryMock.Setup(r => r.FindById(trade.Id)).Returns(trade);

            Mock<CardRepository> cardRepositoryMock = new() { CallBase = true };
            cardRepositoryMock.Setup(r => r.FindById(myCard.Id)).Returns(myCard);

            Mock<DeckRepository> deckRepositoryMock = new() { CallBase = true };
            deckRepositoryMock.Setup(r => r.IsCardInDeck(_token.UserId, myCard.Id)).Returns(false);

            TradeLogic tradeLogic = new(tradeRepositoryMock.Object, cardRepositoryMock.Object, deckRepositoryMock.Object);

            // Act / Assert
            var e = Assert.Throws<ForbiddenException>(() => tradeLogic.Trade(_token, trade.Id, myCard.Id))!;
            Assert.AreEqual("The trade expects a card of type " + trade.CardType, e.Message);

            Assert.AreEqual(myCardUserId, myCard.UserId);
            Assert.AreEqual(notMyCardUserId, notMyCard.UserId);

            tradeRepositoryMock.Verify(r => r.FindById(trade.Id), Times.Once);
            cardRepositoryMock.Verify(r => r.FindById(myCard.Id), Times.Once);
            deckRepositoryMock.Verify(r => r.IsCardInDeck(_token.UserId, myCard.Id), Times.Once);
            cardRepositoryMock.Verify(r => r.FindById(notMyCard.Id), Times.Never);
            cardRepositoryMock.Verify(r => r.Save(myCard), Times.Never);
            cardRepositoryMock.Verify(r => r.Save(notMyCard), Times.Never);
            tradeRepositoryMock.Verify(r => r.DeleteByCardId(notMyCard.Id), Times.Never);
        }

        [Test]
        public void TestTradeDamageNotEnough() {
            // Arrange
            Card myCard = CreateMyCard();
            Card notMyCard = CreateNotMyCard();
            Trade trade = new(Guid.NewGuid(), Guid.NewGuid(), notMyCard.Id, CardType.Monster, 11);

            Guid myCardUserId = (Guid) myCard.UserId!;
            Guid notMyCardUserId = (Guid) notMyCard.UserId!;

            Mock<TradeRepository> tradeRepositoryMock = new() { CallBase = true };
            tradeRepositoryMock.Setup(r => r.FindById(trade.Id)).Returns(trade);

            Mock<CardRepository> cardRepositoryMock = new() { CallBase = true };
            cardRepositoryMock.Setup(r => r.FindById(myCard.Id)).Returns(myCard);

            Mock<DeckRepository> deckRepositoryMock = new() { CallBase = true };
            deckRepositoryMock.Setup(r => r.IsCardInDeck(_token.UserId, myCard.Id)).Returns(false);

            TradeLogic tradeLogic = new(tradeRepositoryMock.Object, cardRepositoryMock.Object, deckRepositoryMock.Object);

            // Act / Assert
            var e = Assert.Throws<ForbiddenException>(() => tradeLogic.Trade(_token, trade.Id, myCard.Id))!;
            Assert.AreEqual("The trade expects a card with a minimum damage of " + trade.MinimumDamage, e.Message);

            Assert.AreEqual(myCardUserId, myCard.UserId);
            Assert.AreEqual(notMyCardUserId, notMyCard.UserId);

            tradeRepositoryMock.Verify(r => r.FindById(trade.Id), Times.Once);
            cardRepositoryMock.Verify(r => r.FindById(myCard.Id), Times.Once);
            deckRepositoryMock.Verify(r => r.IsCardInDeck(_token.UserId, myCard.Id), Times.Once);
            cardRepositoryMock.Verify(r => r.FindById(notMyCard.Id), Times.Never);
            cardRepositoryMock.Verify(r => r.Save(myCard), Times.Never);
            cardRepositoryMock.Verify(r => r.Save(notMyCard), Times.Never);
            tradeRepositoryMock.Verify(r => r.DeleteByCardId(notMyCard.Id), Times.Never);
        }

        // /////////////////////////////////////////////////////////////////////
        // Helpers
        // /////////////////////////////////////////////////////////////////////

        private static Card CreateMyCard() {
            return new(Guid.NewGuid(), "card", ElementType.Normal, 10, CardType.Monster, null, _token.UserId);
        }

        private static Card CreateNotMyCard() {
            return new(Guid.NewGuid(), "card", ElementType.Normal, 10, CardType.Monster, null, Guid.NewGuid());
        }
    }
}
