//manages the animations of the enemy
public class AnimationManagerEnemy : AnimationManager<Enemy> 
{

    private EventManagerEnemy eventManager;

    protected override void Awake()
    {
        base.Awake();
        eventManager = controller.GetComponent<EventManagerEnemy>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        eventManager.onStartShooting += ShootAnimation;
        eventManager.onMovementStarted += StartWalking;
        eventManager.onMovementEnded += StopWalking;
        eventManager.onDead += Death;
        GameManager.instance.EventManager.onSpeedUpToggle += SetAnimatorSpeed;
        //if there's reload, set speed animation of reload based on attack rate
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameManager.instance.EventManager.onSpeedUpToggle -= SetAnimatorSpeed;
        eventManager.onStartShooting -= ShootAnimation;
        eventManager.onMovementStarted -= StartWalking;
        eventManager.onMovementEnded -= StopWalking;
        eventManager.onDead -= Death;
    }

    //sets the animator speed based on the game manager simulation speed
    private void SetAnimatorSpeed(int speed)
    {
        animator.speed = speed;
    }

    //starts movement
    private void StartWalking()
    {
        animator.SetBool(Constants.ANIMATION_MOVEMENT, true);
    }

    private void StopWalking()
    {
        animator.SetBool(Constants.ANIMATION_MOVEMENT, false);
    }

    //stops every other animation, dead
    private void Death(Enemy enemy)
    {
        StopWalking();
        StopShooting();
        animator.SetTrigger(Constants.ANIMATION_DEATH);
    }

    private void ShootAnimation()
    {
        animator.SetBool(Constants.ANIMATION_SHOOT, true);
    }

    private void StopShooting()
    {
        animator.SetBool(Constants.ANIMATION_SHOOT, false);
    }

    //sends message upwards to shoot
    public void MessageShoot()
    {
        SendMessageUpwards("Shoot"); 
    }
}
