<div align="center">

# Trompf Card Game

**Console-based card game simulation • .NET • OOP Design**

</div>

---

## 1. Overview

This project represents a **C# implementation of the Trompf card game**, simulating a complete match with **4 players divided into 2 teams**, played over **6 rounds**.

The application focuses on accurately modeling core mechanics such as **bidding, trump selection, trick-taking, and scoring logic**.

---

## 2. Main Features

- 24-card deck *(ranks: 2, 3, 4, 9, 10, 11/Ace)*
- Card dealing *(6 cards per player)*
- Bidding system *(0–4 big points per player)*
- Automatic determination of the **trump suit** based on the first played card
- Full implementation of game rules for valid card selection
- Score calculation using:
  - **Small points**
  - **Big points** *(1 big point = 33 small points, excluding 9s)*

---

## 3. Technical Details

- Developed in **C# using .NET**
- Object-Oriented design with core classes:
  - `Card`
  - `Player`
  - `Game`
  - `Trick`
- Input validation and exception handling for:
  - Invalid bids
  - Empty hands
  - Incorrect game states
- Clear separation between:
  - Game logic
  - Data structures

---

## 4. Game Flow

```
+----------------------+
|      Start Game      |
+----------+-----------+
           |
           v
+----------------------+
| Create 24-card deck  |
+----------+-----------+
           |
           v
+----------------------+
|    Shuffle deck      |
+----------+-----------+
           |
           v
+-------------------------------+
| Deal 6 cards to each player   |
+---------------+---------------+
                |
                v
+----------------------+
|    Bidding Phase     |
+----------+-----------+
           |
           v
+-------------------------------+
| Determine highest bidder      |
+---------------+---------------+
                |
                v
+----------------------+
|   Play first card    |
+----------+-----------+
           |
           v
+---------------------------------------------+
| Set trump suit based on first played card   |
+--------------------+------------------------+
                     |
                     v
+----------------------------------+
| Play tricks according to rules   |
+----------------+-----------------+
                 |
                 v
+----------------------+
| Calculate small pts  |
+----------+-----------+
           |
           v
+-------------------------------+
| Convert to big points         |
+---------------+---------------+
                |
                v
+----------------------+
| Update team scores   |
+----------+-----------+
           |
           v
+-------------------------------+
| Repeat until 6 rounds         |
+---------------+---------------+
                |
                v
+----------------------+
| Determine winner     |
+----------------------+
```

---

## 5. Setup & Installation

To run this project, you need to have **.NET SDK installed** on your system.

### 1. Install .NET SDK

Download and install the latest version from the official website:  
https://dotnet.microsoft.com/en-us/download

Verify installation:

dotnet --version

---

### 2. Run the Application

Navigate to the project folder:

cd path/to/project

Then run:

dotnet build  
dotnet run  

---

## 6. Notes

- The application runs in the **console (terminal-based interface)**
- Make sure you are inside the project directory containing the `.csproj` file
- If you encounter issues, ensure your .NET SDK is properly installed and added to PATH

---

## 7. Project Scope

This project fulfills all assignment requirements, including:
- Bidding logic
- Round simulation
- Score calculation system
- Full rule enforcement

---

## Why this project matters

This implementation demonstrates strong understanding of:
- **Object-Oriented Programming principles**
- Structuring a medium-sized application
- Translating real-world game rules into code
- Writing clean, maintainable logic with proper validation
