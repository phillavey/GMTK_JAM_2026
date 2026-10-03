# GMTK Game Jam 2026

A fast-paced 2D Unity game prototype built for the GMTK Game Jam 2026. This project focuses on clean gameplay systems, strong event-driven architecture, and a gameplay loop built around room-based task management, item interactions, and time pressure.

## Overview

This project was built to explore a lightweight but scalable gameplay architecture under a compressed deadline. The game loop combines randomized room events, player-driven combat/tool use, and a mana-based spellcasting system to create a replayable chaotic house-clearing experience.

Rather than building a single monolithic script, the project separates responsibilities across controllers, item logic, UI, and event-driven systems to keep the codebase readable and extensible.

## Core Gameplay

- Randomized tasks spawn across multiple rooms
- Player uses inventory items to solve environmental problems
- Mana-based mechanics drive movement and spell progression
- Event-driven task completion updates the UI and gameplay state
- Time pressure and task churn create a high-energy loop

## Architecture

### Game Loop & Task Management
- `Assets/Scripts/Controllers/MasterGameController.cs`
- Handles task generation, room selection, object spawning, and completion tracking
- Uses a central `GAME_TASK` enum and `ROOM` enum to create a flexible, extensible task system
- Tracks active task counts to ensure task completion triggers the correct downstream behavior

### Event-Driven Communication
- `Assets/Scripts/EVENT_BUS.cs`
- Implements a lightweight global event bus for cross-system communication
- Allows gameplay, UI, and audio systems to remain decoupled
- Events include task starts, combat actions, item use, mana updates, time completion, and task resolution

### Player Systems
- `Assets/Scripts/PlayerCharacter.cs`
- Handles movement, sprinting, input, and high-level player actions
- Uses Unity's Input System to drive responsive action-based gameplay
- Integrates sprinting with mana consumption and screen feedback

### Inventory & Interaction
- `Assets/Scripts/Items/PlayerInventory.cs`
- Manages the currently held item, object throwing, swing animation, and aiming behavior
- Supports several item types including plunger, food, lantern, sage stick, and scissors
- Creates a flexible interaction model that supports both utility and combat-style actions

### Spellcasting & Progression
- `Assets/Scripts/Spellcasting.cs`
- Governs mana economy and spell progress
- Increases focus when tasks are completed
- Consumes mana for sprinting and seance-room interactions
- Resets and triggers spell resolution once progress thresholds are met

### UI and Feedback
- `Assets/Scripts/UI/UITasklist.cs`
- Generates live task labels based on event payloads
- Displays room-aware descriptions and icons for active objectives
- Removes entries when a task is completed

## Technical Highlights

- Event-driven architecture for decoupled gameplay systems
- Modular task spawning and lifecycle management
- Input-driven player control using Unity Input System
- Physics-based item throwing and object interaction
- Dynamic UI generation from runtime game state
- Clean separation of concerns across gameplay, UI, and controller logic
