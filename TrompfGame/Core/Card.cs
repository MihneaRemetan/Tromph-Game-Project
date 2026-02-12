using TrompfGame.Enums;

namespace TrompfGame.Core{
    public class Card{
        public Suit Suit { get; }
        public Rank Rank { get; }

        public Card(Suit suit, Rank rank){
            this.Suit = suit;
            this.Rank = rank;
        }

        public override string ToString(){
            return $"{Rank} of {Suit}";
        }
    }
}