using TrompfGame.Core;

namespace TrompfGame.Tests;

public class GameTests
{
    //verific ca input ul sa nu fie null
    [Fact]
    public void Constructor_WhenPlayersListIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new Game(null!));
    }

    //constructorul trebuie sa aiba exact 4 jucatori
    [Fact]
    public void Constructor_WhenPlayerCountIsNotFour_ThrowsArgumentException()
    {
        var players = new List<Player>
        {
            new Player("A", 1),
            new Player("B", 1),
            new Player("C", 2)
        };

        Assert.Throws<ArgumentException>(() => new Game(players));
    }

    //echipele trebuie sa aiba exact 2 jucatori
    [Fact]
    public void Constructor_WhenPlayersAreNotDividedIntoTwoTeams_ThrowsArgumentException()
    {
        var players = new List<Player>
        {
            new Player("A", 1),
            new Player("B", 1),
            new Player("C", 1),
            new Player("D", 1)
        };

        Assert.Throws<ArgumentException>(() => new Game(players));
    }
}