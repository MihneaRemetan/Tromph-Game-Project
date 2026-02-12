using TrompfGame.Enums;
using System.Collections.Generic;

namespace TrompfGame.Core{
    public class Trick{ //runda din joc
        private readonly List<Player> players = new();
        private readonly List<Card> cards = new();

        public Suit? LeadSuit => cards.Count == 0 ? null : cards[0].Suit; //culoarea primei carti jucate

        public void AddPlay(Player player, Card card){
            players.Add(player);
            cards.Add(card); //adaug cartea jucata de jucator
        }

        public IReadOnlyList<Player> Players => players;
        public IReadOnlyList<Card> Cards => cards;
    }
}