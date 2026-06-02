<div align="center">

# Trompf Card Game

**Console-based card game simulation • .NET • OOP Design**

</div>

---

## 1. Overview

This project represents a **C# implementation of the Trompf card game**, simulating a complete match with **4 players divided into 2 teams**, played over **6 rounds**.

The application focuses on accurately modeling core mechanics such as **bidding, trump selection, trick-taking, shout detection, and scoring logic**.

---

## 2. Main Features

- 24-card deck *(ranks: 2, 3, 4, 9, 10, Ace)*
- Card dealing *(6 cards per player)*
- Bidding system *(0–4 big points per player)*
- Automatic determination of the **trump suit** based on the first played card
- Shout detection *(small and big shouts)*
- Full implementation of game rules for valid card selection
- Score calculation using:
  - **Small points**
  - **Big points** *(1 big point = 33 small points, excluding 9s)*
- Input validation and exception handling
- Automated unit testing using **xUnit**

---

## 3. Technical Details

- Developed in **C# using .NET**
- Object-Oriented design with core classes:
  - `Card`
  - `Player`
  - `Game`
  - `Trick`
  - `Shout`
- Input validation and exception handling for:
  - Invalid bids
  - Empty hands
  - Incorrect game states
  - Invalid player configurations
- Clear separation between:
  - Game logic
  - Data structures
  - Testing logic

---

## 4. Game Flow

- **Start Game**
  - Create 24-card deck
  - Shuffle deck
  - Deal 6 cards to each player

- **Bidding Phase**
  - Players place bids (0–4 big points)
  - Determine highest bidder
  - Handle no-bid scenarios

- **Gameplay**
  - First card is played
  - Trump suit is set based on the first played card
  - Players play tricks according to game rules
  - Shouts are detected when applicable

- **Scoring**
  - Calculate small points
  - Add shout points
  - Convert small points into big points *(1 big point = 33 small points)*
  - Verify bidding outcome

- **Loop**
  - Repeat for 6 rounds

- **End Game**
  - Determine winning team

---

## 5. Setup & Installation

To run this project, you need to have **.NET SDK installed** on your system.

### 1. Install .NET SDK

Download and install the latest version from the official website:

https://dotnet.microsoft.com/en-us/download

Verify installation:

```bash
dotnet --version
```

### 2. Run the Application

Navigate to the project folder:

```bash
cd path/to/project
```

Then run:

```bash
dotnet build
dotnet run
```

---

## 6. Testing

The project includes a dedicated **xUnit test suite** used to validate core game functionality, validation rules, and edge cases.

### Implemented Tests

#### Player Logic
- Big shout detection
- Small shout detection
- Prevention of false shout detection
- Multiple shout detection
- Suit ownership verification (positive case)
- Suit ownership verification (negative case)

#### Game Validation
- Null player list validation
- Invalid player count validation
- Invalid team distribution validation

#### Card Functionality
- Card string representation validation

A total of **10 automated unit tests** have been implemented.

Run all tests using:

```bash
dotnet test
```

---

## 7. Notes

- The application runs in the **console (terminal-based interface)**
- Make sure you are inside the project directory containing the `.csproj` file
- If you encounter issues, ensure your .NET SDK is properly installed and added to PATH

---

## 8. Project Scope

This project demonstrates the implementation of a complete rule-based card game simulation, including:

- Bidding logic
- Round simulation
- Trump suit handling
- Shout detection
- Score calculation system
- Validation and exception handling
- Automated testing

---

## Why this project matters

This implementation demonstrates strong understanding of:

- **Object-Oriented Programming principles**
- Structuring a medium-sized application
- Translating real-world game rules into code
- Writing clean, maintainable, and testable code
- Input validation and exception handling
- Automated unit testing using **xUnit**
