using System.Collections.Generic;
using TrompfGame.Enums;

namespace TrompfGame.Core{
    public enum ShoutType{
        Small = 20,   
        Big = 40      
    }

    public class Shout{
        public ShoutType Type { get; }
        public List<Card> Cards { get; }
        public Player Player { get; }
        public int Points => (int)Type;

        public Shout(ShoutType type, List<Card> cards, Player player){
            Type = type;
            Cards = new List<Card>(cards);
            Player = player;
        }

        public override string ToString(){
            string shoutName = Type == ShoutType.Big ? "BIG SHOUT (40p)" : "SMALL SHOUT (20p)";
            return $"{Player.Name} - {shoutName}: {string.Join(", ", Cards)}";
        }
    }
}
