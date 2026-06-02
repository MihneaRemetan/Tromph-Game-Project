using TrompfGame.Core;
using TrompfGame.Enums;

namespace TrompfGame.Tests;

public class PlayerTests
{
    //strigare mare
    [Fact]
    public void DetectShouts_WhenPlayerHasTrumpThreeAndFour_ReturnsBigShout()
    {
        var player = new Player("Mihnea", 1);

        player.Hand.Add(new Card(Suit.Red, Rank.Three));
        player.Hand.Add(new Card(Suit.Red, Rank.Four));

        var shouts = player.DetectShouts(Suit.Red);

        Assert.Single(shouts);
        Assert.Equal(ShoutType.Big, shouts[0].Type);
        Assert.Equal(40, shouts[0].Points);
    }

    //strigare mica
    [Fact]
    public void DetectShouts_WhenPlayerHasNonTrumpThreeAndFour_ReturnsSmallShout()
    {
        var player = new Player("Mihnea", 1);

        player.Hand.Add(new Card(Suit.Green, Rank.Three));
        player.Hand.Add(new Card(Suit.Green, Rank.Four));

        var shouts = player.DetectShouts(Suit.Red);

        Assert.Single(shouts);
        Assert.Equal(ShoutType.Small, shouts[0].Type);
        Assert.Equal(20, shouts[0].Points);
    }

    //verific sa nu fie strigari false
    [Fact]
    public void DetectShouts_WhenPlayerDoesNotHaveThreeAndFour_ReturnsEmptyList()
    {
        var player = new Player("Mihnea", 1);

        player.Hand.Add(new Card(Suit.Red, Rank.Three));
        player.Hand.Add(new Card(Suit.Red, Rank.Ace));

        var shouts = player.DetectShouts(Suit.Red);

        Assert.Empty(shouts);
    }

    //verific daca jucatorul are culoarea cautata
    [Fact]
    public void HasSuit_WhenPlayerHasSuit_ReturnsTrue()
    {
        var player = new Player("Mihnea", 1);
        player.Hand.Add(new Card(Suit.Red, Rank.Ace));

        bool result = player.HasSuit(Suit.Red);
        Assert.True(result);
    }

    //verific daca jucatorul nu are culoarea cautata
    [Fact]
    public void HasSuit_WhenPlayerDoesNotHaveSuit_ReturnsFalse()
    {
        var player = new Player("Mihnea", 1);
        player.Hand.Add(new Card(Suit.Green, Rank.Ace));

        bool result = player.HasSuit(Suit.Red);
        Assert.False(result);
    }

    //verific detectarea mai multor strigari simultan
    [Fact]
    public void DetectShouts_WhenPlayerHasTwoValidPairs_ReturnsTwoShouts()
    {
        var player = new Player("Mihnea", 1);

        player.Hand.Add(new Card(Suit.Red, Rank.Three));
        player.Hand.Add(new Card(Suit.Red, Rank.Four));

        player.Hand.Add(new Card(Suit.Green, Rank.Three));
        player.Hand.Add(new Card(Suit.Green, Rank.Four));

        var shouts = player.DetectShouts(Suit.Red);

        Assert.Equal(2, shouts.Count);
    }
}