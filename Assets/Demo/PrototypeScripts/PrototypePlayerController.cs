using CharacterMovement;
using UnityEngine;
using UnityEngine.InputSystem;

public class PrototypePlayerController : PlayerController
{
    private LightbeamSpawn fireBeam;
    private LightbeamRide lightbeamRide;
    [SerializeField] protected Transform _beamSpawnTransform;


    private void Start()
    {
        if (_beamSpawnTransform == null) // Prevents overwriting
        {
            _beamSpawnTransform = transform.Find("BeamBuddy");
        }

            fireBeam = GetComponent<LightbeamSpawn>();
        lightbeamRide = GetComponent<LightbeamRide>();

    }

    public override void OnFire(InputValue value)
    {
        Vector2 firePosition = _beamSpawnTransform.position;
        Debug.Log($"Beam Spawn Transform: {_beamSpawnTransform.name}");
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 fireDirection = (mousePosition - firePosition).normalized;

        //Debug.Log($"Mouse Position: {mousePosition}, Fire Position: {firePosition}");
        //Debug.Log($"Fire Direction: {fireDirection}, Magnitude: {fireDirection.magnitude}");

        fireBeam._isBeamSpawned = true;
        fireBeam.SpawnBeam(firePosition, fireDirection);


    }
    public override void OnJump(InputValue value)
    {
        if (Movement != null)
        {
            if (lightbeamRide.isRiding == true && Movement.CanMove == false)
            {
                lightbeamRide.JumpOffBeam();
            }
        }
    }
}
