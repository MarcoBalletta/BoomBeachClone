# MiniclipTest

This project contains a building mode and simulation mode, based on Boom Beach game.

It's an event based project, where Game Manager, Buildings and Enemies are handled with states and events using the observer pattern and the state pattern.

In the menu the player can choose how many buildings to spawn and how many of each enemy to spawn.

The Game Manager is the only Singleton class, it has a state manager and an event manager that manages the phases of the game.

The Game Manager has 3 states: placing, building and simulation.

The building mode has 2 phases, the placing mode and the building mode.

The placing mode is the phase where the player can choose which building to build or to select an already built one.

The building mode is the phase where the player can move or rotate the selected building.

The simulation mode is triggered clicking the play button on the UI, spawns the enemy (with a poolying system) and saves the list of the enemies spawned.

Every building has also a state manager and an event manager.

The states are placed state and placing state.

The building starts in the placing states, where the placing UI is activated and lets the player rotate, place or deselect the selected building.

The placed state allows the building to hide the placing UI and show the health UI, showing the health bar if the building is a defense, after placing the

building it gets stored in a list inside the game manager, this list is used by the enemy to find the closest buikding and attack it.

The defense is a type of building, it's destroyable, it has a health component and an attack component, which allows the defense to research the enemies

in a certain range and to attack one enemy at a time.

The enemy is an AI-driven object, it has 3 states: research, movement and attack.

In the research state, it searches the closest defense to attack looking at the list of the defenses in the game manager.

In the movement state, it reaches the defense based on his range and visibility, if there's a building like a fence in the way the enemy keeps moving and

searching a good spot to attack the defense.

In the attack state, the enemy stops moving and attacks the defense until it's destroyed.

There are also SFX and VFX, called by events.

The building has FX when it gets placed.

The defense has FX when shoots or gets destroyed.

The enemy has FX when spawns, shoots or gets killed.

The game manager has background songs for placing and simulation mode.

The enemy is also animated, the shooting is based on animation events.

When the game ends there's an end game panel with a retry button and a return to menu button, and a win/lose text.
