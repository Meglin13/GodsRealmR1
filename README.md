# God's Realm Devlog #

## 2022

### 01.08-10.08
- Player controls and movement
- Simple Combat System including Attack and Block
- Sprint, Dodge and Stamina
- Currencies (Gold and Tokens)
- Characters Animations
- Crit Stats
- Death and Restart
- Character and Enemy Stats

### 11.09-23.09
- Character and Enemy StateMachine
- Character looks at nearest enemy while attacking
- Character class for each character. ICharacter interface implementation + CharacterScript inheritance

### 02.10-09.10
- AIUtilities class for NPC AI and target finding
- Character Switching

### 16.10-30.10 
- Character Switching using keyboard nums
- IDamageable interface
- PartyManager
- BlendTree for movement animations
- RequireComponent in scripts
- Combat System Rework (Combo attacks, Raycast hits)
- Simple Minimap
- Animation Overrides

### 24.11-12.12
- Added URP
- Stats rework
- Damage Formulas
- Minimap improvements
-- Active character marker
-- Teammebmers and Enemies markers
- Movement relative to the camera
- Fixed attacks skip

### 12.12-26.12
- Bars update
- Block and Dodge rework using StateMachine
- Throwables script
- Modifiers (permanent and temporary) and Team Buffs
- Added first Character's Abilities
#### UI
-- Main Menu
-- Settings menu
-- Loading Screen
- Healthbar initilize in script
- Parry and Parry Crit Attack
- Elements System and Elemental Resistance
#### Fixes
- Fixed NullReferenceException after Player's death

## 2023

### 04.01-31.01
- Quality settings
- Stunned State
- Blocking attacks takes stamina, if stamina is less than block cost then character going to stun
- Map markers update after Enemy spawn
#### UI
- Pause menu
- UI InputAction
- UI Manager
#### Fixes
- Parry Attack does not work
- Super fast camera rotation in Main Menu
- Player get stunned after attack

### 01.02-19.02
- Current monitor resolution and refresh rate in dropdown in Setting Menu
- Added events to scripts
- Mana recovery bonus and mana comsumptions stats
- Entities Scripts rework
- Inventory fully implmented
#### Fixes
- Minimap camera movement
- Parry buff does not applies
- UI Localization brokes buttons functionality

### 08.03-12.03
- Interation system
- Random loot
- Chest stars
- Rooms prefab and RoomScript
#### Fixes
- Ardalion spawn multiple bombs
- Player stuns after final attack

### 15.03-02.04
- Dungeon generation
- Vision overlay
- Camera shaking after taking damage
- Saving System

### 04.04-30.04 
- AOE damage
- Difficulty selection
- Potions
- Character 3D models
- Equipment modifiers list in Inventory
- Loading Tutorial when no saves
 
### 01.05-15.05
- Object pooling
- Dialogue System
- Party setup menu and Party loading in RunScene
- Tutorials window
- Enemy spawner
