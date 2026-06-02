using TrompfGame.Core;
using TrompfGame.Enums;

namespace TrompfGame.Tests;

public class CardTests
{
    //verific metoda ToString din Card.cs
    [Fact]
    public void ToString_ReturnsRankAndSuit()
    {
        var card = new Card(Suit.Red, Rank.Ace);
        var result = card.ToString();
        Assert.Equal("Ace of Red", result);
    }
}