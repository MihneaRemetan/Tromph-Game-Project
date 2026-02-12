using TrompfGame.Enums;
using System.Collections.Generic;
using System.Linq;

namespace TrompfGame.Core{
    public class Player{
        public string Name { get; }
        public int TeamID { get; }
        public int Bid { get; set; } //licitatie
        
        public List<Card> Hand { get; } = new();
        public List<Shout> Shouts { get; } = new();

        public Player(string Name, int TeamID){
            this.Name = Name;
            this.TeamID = TeamID;
        }

        public bool HasSuit(Suit suit){
            return Hand.Any(c => c.Suit == suit);
        }

        public List<Shout> DetectShouts(Suit trump){
            var detectedShouts = new List<Shout>();
            
            //verifica pentru perechi de 3 si 4 din aceeasi culoare
            var groupedBySuit = Hand.GroupBy(c => c.Suit).ToList();
            
            foreach(var group in groupedBySuit){
                var cardsInSuit = group.ToList();
                
                //verifica daca are si 3 si 4
                bool hasThree = cardsInSuit.Any(c => c.Rank == Rank.Three);
                bool hasFour = cardsInSuit.Any(c => c.Rank == Rank.Four);
                
                if(hasThree && hasFour){
                    ShoutType shoutType;
                    
                    //daca sunt 3-4 de tromf = strigare mare(40p)
                    if(group.Key == trump){
                        shoutType = ShoutType.Big;
                    }
                    else{
                        //strigare mica(20p)
                        shoutType = ShoutType.Small;
                    }
                    
                    //luam cartile de 3 si 4
                    var shoutCards = cardsInSuit
                        .Where(c => c.Rank == Rank.Three || c.Rank == Rank.Four)
                        .ToList();
                    
                    detectedShouts.Add(new Shout(shoutType, shoutCards, this));
                }
            }
            
            return detectedShouts;
        }
    }
}