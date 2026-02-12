using System.Collections.Generic;
using TrompfGame.Core;

namespace TrompfGame{
    class Program{
        static void DisplayTeams(List<Player> players){
            Console.WriteLine("\nTeams: ");

            var groupedPlayers = players.GroupBy(p => p.TeamID);

            foreach(var team in groupedPlayers){
                Console.WriteLine($"Team {team.Key}:");
                foreach(var player in team)
                    Console.WriteLine($" - {player.Name}");

                Console.WriteLine();
            }
        }
        static void Main(){
            try{
                var players = new List<Player>();
                
                Console.WriteLine("Player Registration:\n");
                
                bool validTeams = false;
                
                while(!validTeams){
                    players.Clear();
                    
                    for(int i = 0; i < 4; i++){
                        string playerName;
                        int team;
                        
                        //read player name
                        do{
                            Console.Write($"Player {i + 1} name: ");
                            playerName = Console.ReadLine()!;
                            
                            if(string.IsNullOrWhiteSpace(playerName)){
                                Console.WriteLine("Name cannot be empty. Please try again!\n");
                            }
                        } while(string.IsNullOrWhiteSpace(playerName));
                        
                        //read team (1 or 2)
                        do{
                            Console.Write($"{playerName}, which team (1 or 2)? ");
                            if(!int.TryParse(Console.ReadLine(), out team) || (team != 1 && team != 2)){
                                Console.WriteLine("Please enter 1 or 2!\n");
                                team = -1;
                            }
                        } while(team == -1);
                        
                        players.Add(new Player(playerName, team));
                        Console.WriteLine();
                    }
                    
                    //validate team distribution: exactly 2 players per team
                    var team1Count = players.Count(p => p.TeamID == 1);
                    var team2Count = players.Count(p => p.TeamID == 2);
                    
                    if(team1Count != 2 || team2Count != 2){
                        Console.WriteLine("ERROR: Each team must have exactly 2 players!");
                        Console.WriteLine($"Team 1: {team1Count} players, Team 2: {team2Count} players\n");
                        Console.WriteLine("Please enter the teams again:\n");
                    }
                    else{
                        validTeams = true;
                    }
                }

                DisplayTeams(players);

                Console.Write("\nDo you want cards with guaranteed shouts? (y/n): ");
                var shoutChoice = Console.ReadLine()?.ToLower();
                bool guaranteeShouts = shoutChoice == "y";
                
                int seed = 42;
                
                Game game = new Game(players, seed: seed, guaranteeShouts: guaranteeShouts);
                game.StartGame();
            }
            catch(ArgumentException ex){
                Console.WriteLine($"[Game Error]: {ex.Message}");
            }
            catch(InvalidOperationException ex){
                Console.WriteLine($"[Invalid operation]: {ex.Message}");
            }
            catch(Exception ex){
                Console.WriteLine($"[Unexpected error]: {ex.Message}");
            }
        }
    }
}