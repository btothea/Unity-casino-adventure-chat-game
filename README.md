Cameron Anderson
Main Quest 2 – Casino Text Adventure
Overview

This project is a narrative-driven casino text adventure game. The player enters a mysterious casino and progresses 
through different rooms by gambling, interacting with NPCs, and making choices that affect the outcome of the story.

The game is built in Unity using ScriptableObjects to organize:
Rooms
Items
Quests
Dialogue
Endings

This submission represents the content skeleton of the game. All scenes, dialogue paths, and systems are created but 
not fully wired together yet.

Game Structure
Scenes Created
Lobby / Main Floor
Dealer Table
High Roller Room
Back Office
VIP Area
Ending States
Core Systems
Dialogue branching system
Flag-based progression
Quest tracking
Multiple endings

Dialogue & Choice Design
Each NPC includes:
Full dialogue trees
Player choice options
Conditional branches (flags)
End states based on decisions

Flags / Variables
Flag	               Default	                  Set Where	                                     Checked Where
moneyAmount	          0	            Dealer Bets, High Roller Table	                      Dealer, Gatekeeper, Endings
tookRisk	           False	               Big Bet, All-In, Final Bet                      	Big Winner Ending
hasVIPToken	         False	         Dealer (money ≥ 200) or Gambler Help Path	            Gatekeeper, VIP Room
knowsSecret  	       False              	Shady Gambler Dialogue	                     Dealer, Gatekeeper, Escape Ending
helpedGambler	       False	               Shady Gambler Help Choice                   	Escape the System Ending
firstBetComplete	   False	                  Dealer after first gamble	                  Main Floor Progression

Assets
All assets are organized in the project under:

Assets/Art (placeholders for now)
Assets/ScriptableObjects
Assets/Scenes
Assets/Scripts
Current State
All scenes created
All ScriptableObjects populated
All dialogue written
Branching paths defined
Flags and conditions implemented

The project is ready for final wiring and gameplay implementation.
