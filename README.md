This project represents a C# implementation of the Trompf (Cruce) card game, developed for a university assignment. The application simulates a complete match with 4 players divided into 2 teams, played over 6 rounds.

Main Features:

24-card deck (ranks: 2, 3, 4, 9, 10, 11/Ace)

Card dealing (6 cards per player)

Bidding system (0–4 big points per player)

Automatic determination of the trump suit based on the first played card

Implementation of game rules for card selection

Score calculation using small and big points (1 big point = 33 small points, excluding 9s)

Technical Details:

Developed in C# using .NET

Object-Oriented Programming structure (Card, Player, Game, Trick classes)

Input validation and exception handling for invalid states (bidding errors, empty hands, incorrect initialization)

Clear separation between game logic and data structures

Execution:
The application runs in the console using the following commands:

dotnet build
dotnet run

The project fulfills all assignment requirements, including bidding logic, round simulation, scoring system, and rule enforcement.
