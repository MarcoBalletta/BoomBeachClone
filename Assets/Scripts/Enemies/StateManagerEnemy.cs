using UnityEngine;

/// <summary>
/// The enemy is an AI-driven object, it has 3 states: research, movement and attack.
///In the research state, it searches the closest defense to attack looking at the list of the defenses in the game manager.
///In the movement state, it reaches the defense based on his range and visibility, if there's a building like a fence in the way the enemy keeps moving and
///searching a good spot to attack the defense.
///In the attack state, the enemy stops moving and attacks the defense until it's destroyed.
/// </summary>
[RequireComponent(typeof(EventManagerEnemy))]
public class StateManagerEnemy : StateManager
{

    private EventManagerEnemy eventManager;

    public EventManagerEnemy EventManager { get => eventManager; }

    protected override void SetupStates()
    {
        listOfStates.Add(Constants.STATE_MOVEMENT, new StateMovement(this));
        listOfStates.Add(Constants.STATE_RESEARCH, new StateResearch(this));
        listOfStates.Add(Constants.STATE_ATTACK, new StateAttack(this));
    }

    protected override void Awake()
    {
        base.Awake();
        eventManager = GetComponent<EventManagerEnemy>();
    }

    private void Start()
    {
        ChangeState(Constants.STATE_RESEARCH);
    }

    protected override void Update()
    {
        base.Update();
    }
}
