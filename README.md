01.08-10.08
- Player controls and movement
- Simple Combat System including Attack and Block
- Sprint, Dodge and Stamina
- Currencies (Gold and Tokens)
- Characters Animations
- Crit Stats
- Death and Restart
- Character and Enemy Stats

11.09-23.09
- Character and Enemy StateMachine
- Character looks at nearest enemy while attacking
- Character class for each character. ICharacter interface implementation + CharacterScript inheritance

02.10-09.10
- AIUtilities class for NPC AI and target finding
- Character Switching

16.10-30.10
- Character Switching using keyboard nums
- IDamageable interface
- PartyManager
- BlendTree for movement animations
- RequireComponent in scripts
- Combat System Rework (Combo attacks, Raycast hits)
- Simple Minimap
- Animation Overrides

24.11-12.12
- Stats rework
- Damage Formulas implementing
- Active character marker
- Movement relative to the camera
- Minimap improvements
- Added URP
- Fixed attacks skip

12.12-19.12
- Bars update
- Block and Dodge rework using StateMachine
- Fixed NullReferenceException after Player's death
- Bullets script
- Magic attacks using bullets
- Modifiers (permanent and temporary) and Team Buffs
- Added first Character's Abilities

19.12-26.12
- Main Menu
- Settings menu
- Loading Screen
- Healthbar initilize in script
- Parry and Parry Crit Attack
- Elements System and Elemental Resistance

04.01-31.01
- Pause menu
- UI InputAction
- UI Manager
- Quality settings
- Stunned State
- Blocking attacks takes stamina, if stamina is less than block cost then character going to stun