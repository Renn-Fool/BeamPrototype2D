using System;
using CharacterMovement;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class LightbeamRide : MonoBehaviour
{
    public CharacterMovement2D movement;
    [SerializeField] public float rideSpeed = 10f;
    [SerializeField] private bool isRiding = false;
    private Vector2 travelDirection;
    private Rigidbody2D rb;
    [SerializeField] private LayerMask rideMask;

    public UnityEvent onStopRidingBeam;

    private void Start()
    {
        if (movement == null) movement = GetComponent<CharacterMovement2D>();
        rb = GetComponent<Rigidbody2D>();

        if (onStopRidingBeam == null)
            onStopRidingBeam = new UnityEvent();
    }

   

    
    public void RideBeam(Vector2 start, Vector2 end)
    {
        
        isRiding = true;
        travelDirection = (end - start).normalized;
        movement.CanMove = true;
        StartCoroutine(RideCoroutine(start, end));
    }

    private System.Collections.IEnumerator RideCoroutine(Vector2 start, Vector2 end)
    {
        float distance = Vector2.Distance(start, end);
        float travelTime = distance / rideSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < travelTime && isRiding)
        {
            transform.position = Vector2.Lerp(start, end, elapsedTime / travelTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (isRiding) transform.position = end;
        StopRiding();
        
    }

    //TODO BELOW
    private void JumpOffBeam()
    {
        if (!isRiding) return;
        isRiding = false;

        movement.CanMove = true;
        rb.linearVelocity = travelDirection * rb.linearVelocity.magnitude;
    }

    private void StopRiding()
    {
        isRiding = false;
        movement.CanMove = true;
        onStopRidingBeam.Invoke();
    }
}
