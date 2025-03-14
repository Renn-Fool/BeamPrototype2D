using CharacterMovement;
using UnityEngine;
using UnityEngine.InputSystem;

public class PrototypePlayerController : PlayerController
{
    [SerializeField] private LightbeamSpawn fireBeam;
    [SerializeField] private LightbeamRide lightbeamRide;
    [SerializeField] protected Transform _beamSpawnTransform;

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
}
