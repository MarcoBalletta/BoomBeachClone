using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    //Game states
    public const string STATE_PLACING = "Placing";
    public const string STATE_BUILDING_MODE = "BuildingMode";
    public const string STATE_SIMULATION = "Simulation";
    //BuildingStates
    public const string STATE_PLACED = "Placed";
    public const string STATE_SIMULATION_RESEARCH = "SimulationResearch";
    public const string STATE_SIMULATION_ATTACK = "SimulationAttack";
    //EnemyStates
    public const string STATE_RESEARCH = "Research";
    public const string STATE_MOVEMENT = "Movement";
    public const string STATE_ATTACK = "Attack";
    //End game texts
    public const string WIN_TEXT = "You won!";
    public const string LOSE_TEXT = "You lost!";
    //Scenes names
    public const string MENU_SCENE_NAME = "MenuScene";
    public const string COMBAT_SCENE_NAME = "CombatScene";
    //EnemyAnimationTrigger
    public const string ANIMATION_RESEARCH = "Research";
    public const string ANIMATION_MOVEMENT = "Movement";
    public const string ANIMATION_SHOOT = "Shoot";
    public const string ANIMATION_DEATH = "Death";
}
