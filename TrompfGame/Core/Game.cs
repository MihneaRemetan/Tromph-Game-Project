using TrompfGame.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrompfGame.Core{
    public class Game{
        private readonly List<Player> players;
        private readonly List<Card> deck = new();
        private readonly List<Card> wonTeam1 = new();
        private readonly List<Card> wonTeam2 = new();

        private readonly Random rng;
        private Player? biddingWinner;
        private readonly bool guaranteeShouts;

        private Suit trump; 
        private bool trumpSet = false;
        private int startIndex;

        public Game(List<Player> players, int? seed = null, bool guaranteeShouts = false){
            if(players == null)
                throw new ArgumentNullException(nameof(players), "Players list cannot be null!");
            if(players.Count != 4)
                throw new ArgumentException($"The game must have exactly 4 players! Current count: {players.Count}");
            if(players.Any(p => string.IsNullOrWhiteSpace(p.Name)))
                throw new ArgumentException("All players must have valid names!");
            if(players.GroupBy(p => p.TeamID).Count() != 2)
                throw new ArgumentException("Players must be divided into exactly 2 teams!");
                
            this.players = players;
            this.guaranteeShouts = guaranteeShouts;
            rng = seed.HasValue ? new Random(seed.Value) : new Random();

            startIndex = rng.Next(0, 4);
        }

        private void BuildDeck(){
            deck.Clear();

            var ranks = new[]{Rank.Two, Rank.Three, Rank.Four, Rank.Nine, Rank.Ten, Rank.Ace};

            var suits = new[]{Suit.Red, Suit.Hearts, Suit.Green, Suit.Acron};

            foreach(var s in suits)
                foreach(var r in ranks)
                    deck.Add(new Card(s, r));
        }

        private void ShuffleDeck(){
            for(int i = deck.Count - 1; i > 0; i--){
                int j = rng.Next(i + 1);
                (deck[i], deck[j]) = (deck[j], deck[i]);
            }
        }

        private void PrepareShoutGuaranteedDeck(){ //pt a verifica strigari 
            BuildDeck();

            var suits = new[]{Suit.Red, Suit.Hearts, Suit.Green, Suit.Acron};
            var hands = new List<Card>[4];
            for(int i = 0; i < 4; i++)
                hands[i] = new List<Card>();

            Card Take(Suit s, Rank r){
                var card = deck.First(c => c.Suit == s && c.Rank == r);
                deck.Remove(card);
                return card;
            }

            //fiecare jucator primeste 3+4 de o culoare (garanteaza strigare)
            for(int i = 0; i < 4; i++){
                var suit = suits[i];
                hands[i].Add(Take(suit, Rank.Three));
                hands[i].Add(Take(suit, Rank.Four));
                hands[i].Add(Take(suit, Rank.Ace)); //carte mare pentru castig
            }

            //completeaza restul cartilor
            var remaining = deck.ToList();
            deck.Clear();

            int[] counts = {hands[0].Count, hands[1].Count, hands[2].Count, hands[3].Count};
            int idx = 0;
            foreach(var card in remaining){
                while(counts[idx] >= 6)
                    idx = (idx + 1) % 4;
                hands[idx].Add(card);
                counts[idx]++;
                idx = (idx + 1) % 4;
            }

            //construieste pachetul pentru deal
            var dealOrder = new List<Card>();
            for(int i = 0; i < 6; i++)
                for(int p = 0; p < 4; p++)
                    dealOrder.Add(hands[p][i]);

            deck.AddRange(dealOrder.AsEnumerable().Reverse());
        }
        public Card DrawCard(){
            var card = deck[^1];
            deck.RemoveAt(deck.Count - 1);

            return card;
        }

        private void DealCards(){
            foreach(var p in players)
                p.Hand.Clear();

            for(int i = 0; i < 6; i++)
                foreach(var p in players)
                    p.Hand.Add(DrawCard());

            Console.WriteLine("Cards shared!\n");

            foreach(var p in players){
                Console.WriteLine(p.Name + ":");
                foreach(var c in p.Hand)
                    Console.WriteLine(" " + c);

                Console.WriteLine();
            }
        }

        private void DetectAndDisplayShouts(Player player){
            //detecteaza strigari doar pentru jucatorul care pune prima carte
            var shouts = player.DetectShouts(trump);
            
            if(shouts.Any()){
                Console.WriteLine("\nDETECTED SHOUTS:");
                foreach(var shout in shouts){
                    player.Shouts.Add(shout);
                    Console.WriteLine($"{shout}");
                }
                Console.WriteLine();
            }
        }

        private int ReadBid(Player p){
            int bid;
            int attempts = 0;
            const int maxAttempts = 3;
            
            do{
                if(attempts >= maxAttempts)
                    throw new InvalidOperationException($"Failed to get valid bid from {p.Name} after {maxAttempts} attempts.");
                    
               Console.Write($"{p.Name}, please enter bid (0-4): ");
               attempts++;
            }
            while(!int.TryParse(Console.ReadLine(), out bid) || bid < 0 || bid > 4);

            if(bid < 0 || bid > 4)
                throw new ArgumentException($"Bid must be between 0 and 4, got {bid}");
                
            return bid;
        }

        private void RunBidding(){
            Console.WriteLine("\nBIDDING PHASE:");

            int biddingStart = rng.Next(0, 4);

            for (int i = 0; i < 4; i++){
                var p = players[(biddingStart + i) % 4];
                p.Bid = ReadBid(p);
            }

            int maxBid = players.Max(p => p.Bid);
            
            //cautam primul jucator in ordinea licitatiei care are bid-ul maxim
            for (int i = 0; i < 4; i++){
                var p = players[(biddingStart + i) % 4];
                if(p.Bid == maxBid){
                    biddingWinner = p;
                    break;
                }
            }
            
            if(biddingWinner == null)
                throw new InvalidOperationException("Bidding winner could not be determined!");
                
            startIndex = players.IndexOf(biddingWinner);

            Console.WriteLine($"\n{biddingWinner.Name} (Team {biddingWinner.TeamID}) wins bidding with {maxBid} points!\n");
        }

        private Card ChooseValidCard(Player player, Suit? leadSuit, Trick trick){
            if(player.Hand.Count == 0)
                throw new InvalidOperationException($"Player {player.Name} has no cards to play!");

            //Dacă nu există culoare de conducere, jucătorul alege strategic:
            //1.culoarea în care are cele mai multe cărți
            //2. la egalitate, culoarea cu totalul de puncte mici cel mai mare
            //3. joacă cea mai mare carte din acea culoare pentru a maximiza șansa de a câștiga runda
            if(leadSuit == null){
                var grouped = player.Hand
                    .GroupBy(c => c.Suit)
                    .Select(g => new{
                        Suit = g.Key,
                        Count = g.Count(),
                        Points = CalculateSmallPoints(g.ToList()),
                        HasAce = g.Any(c => c.Rank == Rank.Ace),
                        HasTen = g.Any(c => c.Rank == Rank.Ten)
                    })
                    .OrderByDescending(x => x.Count)
                    .ThenByDescending(x => x.Points)
                    .ThenByDescending(x => x.HasAce)
                    .ThenByDescending(x => x.HasTen)
                    .FirstOrDefault();

                if(grouped == null)
                    throw new InvalidOperationException($"{player.Name} has no cards to choose from!");

                var chosenSuitCards = player.Hand
                    .Where(c => c.Suit == grouped.Suit)
                    .OrderByDescending(c => (int)c.Rank)
                    .ToList();

                var card = chosenSuitCards.First();

                if(!player.Hand.Contains(card))
                    throw new InvalidOperationException($"Card {card} not found in {player.Name}'s hand!");

                return card;
            }

            //1.daca are culoarea de joc → obligatoriu (dar încearca sa bata cartea curenta dacă e posibil)
            var sameSuitCards = player.Hand
                .Where(c => c.Suit == leadSuit.Value)
                .OrderBy(c => (int)c.Rank) //pentru alegerea celei mai mici daca nu poate bate
                .ToList();

            if (sameSuitCards.Any()){
                //determinam cea mai buna carte din trick pana acum
                Card currentBest = trick.Cards[0];
                for(int k = 1; k < trick.Cards.Count; k++){
                    if(Beats(trick.Cards[k], currentBest, leadSuit.Value))
                        currentBest = trick.Cards[k];
                }

                //incercam sa gasim cea mai mica carte care bate currentBest
                var beatingCard = sameSuitCards
                    .OrderBy(c => (int)c.Rank)
                    .FirstOrDefault(c => Beats(c, currentBest, leadSuit.Value));

                var chosen = beatingCard ?? sameSuitCards.First();
                if(!player.Hand.Contains(chosen))
                    throw new InvalidOperationException($"Card {chosen} not found in {player.Name}'s hand!");
                return chosen;
            }

            //2.nu are culoare → verifica tromf
            var trumpCards = player.Hand
                .Where(c => c.Suit == trump)
                .OrderBy(c => (int)c.Rank) //joaca cel mai mic tromf daca nu poate bate
                .ToList();

            if (trumpCards.Any()){
                //daca cea mai buna carte din trick nu e tromf, orice tromf bate
                Card currentBest = trick.Cards[0];
                for(int k = 1; k < trick.Cards.Count; k++){
                    if(Beats(trick.Cards[k], currentBest, leadSuit.Value))
                        currentBest = trick.Cards[k];
                }

                //alege cel mai mic tromf care bate best-ul curent daca e posibil
                var beatingTrump = trumpCards
                    .OrderBy(c => (int)c.Rank)
                    .FirstOrDefault(c => Beats(c, currentBest, leadSuit.Value));

                var chosen = beatingTrump ?? trumpCards.First();
                if(!player.Hand.Contains(chosen))
                    throw new InvalidOperationException($"Trump card {chosen} not found in {player.Name}'s hand!");
                return chosen;
            }

            //3.nu are nici culoare, nici tromf → arunca cea mai mica
            var fallbackCard = player.Hand
                .OrderBy(c => (int)c.Rank)
                .First();
                
            if(!player.Hand.Contains(fallbackCard))
                throw new InvalidOperationException($"Fallback card {fallbackCard} not found in {player.Name}'s hand!");
                
            return fallbackCard;
        }

        private bool Beats(Card a, Card b, Suit lead){
            if(a.Suit == trump && b.Suit != trump)
                return true;

            if(a.Suit != trump && b.Suit == trump)
                return false;

            if(a.Suit == b.Suit)
                return a.Rank > b.Rank;

            if(a.Suit == lead && b.Suit != lead)
                return true;

            return false;
        }

        private int DetermineTrickWinner(Trick trick){
            Suit lead = trick.LeadSuit!.Value;
            int best = 0;

            for(int i = 1; i < 4; i++)
                if(Beats(trick.Cards[i], trick.Cards[best], lead))
                    best = i;

            return best;
        }

        private int CalculateSmallPoints(List<Card> cards){
            if(cards == null)
                throw new ArgumentNullException(nameof(cards), "Cards list cannot be null!");
                
            int amount = 0;

            foreach(var c in cards){
                if(c == null)
                    throw new ArgumentException("Card in list is null!");
                    
                //cartile de 9 nu se numara in punctaj
                if(c.Rank == Rank.Nine)
                    continue;

                //punctele mici: 2,3,4,10,11(As)
                if(c.Rank == Rank.Two)
                    amount += 2;
                else if(c.Rank == Rank.Three)
                    amount += 3;
                else if(c.Rank == Rank.Four)
                    amount += 4;
                else if(c.Rank == Rank.Ten)
                    amount += 10;
                else if(c.Rank == Rank.Ace)
                    amount += 11;
            }

            return amount;
        }

        private void CalculateScore(){
            if (biddingWinner == null)
                throw new InvalidOperationException("Bidding has not been established yet!");
            if (wonTeam1 == null || wonTeam2 == null)
                throw new InvalidOperationException("Teams cards not initialized!");

            int points1 = CalculateSmallPoints(wonTeam1);
            int points2 = CalculateSmallPoints(wonTeam2);

            //calculare puncte din strigari
            int shoutPoints1 = 0;
            int shoutPoints2 = 0;
            
            foreach(var player in players){
                int shoutPointsForPlayer = player.Shouts.Sum(s => s.Points);
                if(player.TeamID == 1){
                    shoutPoints1 += shoutPointsForPlayer;
                }
                else if(player.TeamID == 2){
                    shoutPoints2 += shoutPointsForPlayer;
                }
            }

            //total puncte mici 
            int totalPoints1 = points1 + shoutPoints1;
            int totalPoints2 = points2 + shoutPoints2;

            //un punct mare = 33 de puncte mici
            int big1 = totalPoints1 / 33;
            int big2 = totalPoints2 / 33;

            int biddingTeam = biddingWinner.TeamID;
            int bidValue = biddingWinner.Bid;

            Console.WriteLine("\nFINAL SCORE:");
            Console.WriteLine($"Team 1 small points from cards: {points1}");
            Console.WriteLine($"Team 1 shout points: {shoutPoints1}");
            Console.WriteLine($"Team 1 total small points: {totalPoints1} (big points: {big1})");
            Console.WriteLine();
            Console.WriteLine($"Team 2 small points from cards: {points2}");
            Console.WriteLine($"Team 2 shout points: {shoutPoints2}");
            Console.WriteLine($"Team 2 total small points: {totalPoints2} (big points: {big2})");
            Console.WriteLine();
            
            //verificare daca echipa care a licitatie a facut punctele licitate (cu strigari incluse)
            if(biddingTeam == 1){
                if(big1 >= bidValue){
                    Console.WriteLine($"Team 1 made the bid! +{bidValue}");
                } else {
                    Console.WriteLine($"Team 1 failed the bid! -{bidValue}");
                }
            }
            else if(biddingTeam == 2){
                if(big2 >= bidValue){
                    Console.WriteLine($"Team 2 made the bid! +{bidValue}");
                } else {
                    Console.WriteLine($"Team 2 failed the bid! -{bidValue}");
                }
            }
        }

        private int SmallPoints(Card c){
            if(c.Rank == Rank.Ten)
                return 10;

            if(c.Rank == Rank.Ace)
                return 11;

            return 0;
        }

        private void PlayGame(){ //6 runde
            Console.WriteLine("\nGAME PLAY\n");
            
            for(int i = 1; i <= 6; i++){
                Console.WriteLine($"\nROUND {i}:");
                
                if(players.Any(p => p.Hand.Count == 0))
                    throw new InvalidOperationException($"A player has no cards at round {i}!");
                    
                Trick trick = new Trick();

                for(int j = 0; j < 4; j++){
                    Player p = players[(startIndex + j) % 4];

                    if(p.Hand.Count == 0)
                        throw new InvalidOperationException($"{p.Name} has no cards to play in round {i}!");

                    Card played;
                    if(trick.Cards.Count == 0){
                        //prima carte din runda - verificam strigari doar pentru acest jucator
                        if(trumpSet){
                            DetectAndDisplayShouts(p);
                        }
                        
                        played = ChooseValidCard(p, null, trick);
                    }
                    else{
                        played = ChooseValidCard(p, trick.LeadSuit!.Value, trick);
                    }

                    if(!p.Hand.Contains(played))
                        throw new InvalidOperationException($"Card {played} not in {p.Name}'s hand!");

                    p.Hand.Remove(played);
                    trick.AddPlay(p, played);
                    Console.WriteLine($"{p.Name} (Team {p.TeamID}) plays: {played}");

                    if(!trumpSet){
                        trump = played.Suit;
                        trumpSet = true;
                        Console.WriteLine($"TRUMP SUIT ESTABLISHED: {trump}");
                    }
                }

                int winnerIndex = DetermineTrickWinner(trick);
                if(winnerIndex < 0 || winnerIndex >= trick.Players.Count)
                    throw new InvalidOperationException($"Invalid trick winner index: {winnerIndex}");
                    
                Player winner = trick.Players[winnerIndex];
                Console.WriteLine($"\nRound winner: {winner.Name} (Team {winner.TeamID})");

                if(winner.TeamID == 1){
                    wonTeam1.AddRange(trick.Cards);
                }
                else if(winner.TeamID == 2){
                    wonTeam2.AddRange(trick.Cards);
                }
                else{
                    throw new InvalidOperationException($"Invalid team ID: {winner.TeamID}");
                }

                startIndex = players.IndexOf(winner);
            }
        }

        public void StartGame(){
            try{
                Console.WriteLine();
                Console.WriteLine("             WELCOME TO TROMPF GAME              ");
                Console.WriteLine("             6 rounds of card play               ");
                Console.WriteLine();
                
                if(guaranteeShouts){
                    PrepareShoutGuaranteedDeck();
                    startIndex = 0;
                }
                else{
                    BuildDeck();
                    ShuffleDeck();
                }

                if(deck.Count != 24)
                    throw new InvalidOperationException($"Deck should have 24 cards, has {deck.Count}");
                DealCards();
                
                if(players.Any(p => p.Hand.Count != 6))
                    throw new InvalidOperationException("Not all players have 6 cards after dealing!");
                    
                RunBidding();
                PlayGame();
                CalculateScore();
                
                Console.WriteLine();
                Console.WriteLine("             GAME COMPLETED SUCCESSFULLY!                ");
                Console.WriteLine();
            }
            catch(ArgumentNullException ex){
                Console.WriteLine($"\n[NULL ERROR] {ex.Message}");
                throw;
            }
            catch(ArgumentException ex){
                Console.WriteLine($"\n[VALIDATION ERROR] {ex.Message}");
                throw;
            }
            catch(InvalidOperationException ex){
                Console.WriteLine($"\n[GAME ERROR] {ex.Message}");
                throw;
            }
            catch(Exception ex){
                Console.WriteLine($"\n[UNEXPECTED ERROR] {ex.Message}");
                throw;
            }
        }
    }
}