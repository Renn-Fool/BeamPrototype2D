using CharacterMovement;
using UnityEngine;
using UnityEngine.InputSystem;

public class PrototypePlayerController : PlayerController
{
    private LightbeamSpawn fireBeam;
    private LightbeamRide lightbeamRide;
    [SerializeField] protected Transform _beamSpawnTransform;
    private CharacterMovement2D movement;


    private void Start()
    {
        _beamSpawnTransform = GetComponent<Transform>();
    }
    public override void OnFire(InputValue value)
    {
        Vector2 firePosition = _beamSpawnTransform.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 fireDirection = (mousePosition - firePosition).normalized;

        //Debug.Log($"Mouse Position: {mousePosition}, Fire Position: {firePosition}");
        //Debug.Log($"Fire Direction: {fireDirection}, Magnitude: {fireDirection.magnitude}");

        fireBeam._isBeamSpawned = true;
        fireBeam.SpawnBeam(firePosition, fireDirection);


    }
    public override void OnJump(InputValue value)
    {
        if(lightbeamRide.isRiding == true && movement.CanMove == false)
        {
            lightbeamRide.JumpOffBeam();
        }
    }
}
