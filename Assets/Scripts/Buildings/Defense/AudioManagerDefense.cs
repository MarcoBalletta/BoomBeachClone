using UnityEngine;

/// <summary>
/// The audio manager of the defense, with the shooting clip that starts when the defense shoots
/// </summary>
public class AudioManagerDefense : AudioManagerBuilding
{
    [SerializeField] protected AudioClip shootClip;

    protected Defense Controller { get => (controller as Defense); }

    protected override void OnEnable()
    {
        base.OnEnable();
        Controller.EventManager.onShoot += ShootSound;
    }

    private void ShootSound(Transform transform)
    {
        PlayOneShotAudio(shootClip);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Controller.EventManager.onShoot -= ShootSound;
    }
}
