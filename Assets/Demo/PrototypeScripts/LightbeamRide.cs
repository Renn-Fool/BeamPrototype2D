using System;
using CharacterMovement;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class LightbeamRide : MonoBehaviour
{
    private CharacterMovement2D movement;
    private LightbeamController beamController;
    [SerializeField] public float rideSpeed = 10f;
    [SerializeField] public bool isRiding = false;
    private Vector2 travelDirection;
    private Rigidbody2D rb;
    private Collider2D currentBeam;
    public LayerMask beamLayer; // Assign the beam layer in the Inspector
    public float detectionRadius = 0.5f;
    public UnityEvent onStopRidingBeam;

    private Vector2 beamPlayerRideStart;
    private Vector2 playerPositionOnTrigger;
    [SerializeField] private string targetLayerName = "Lightbeam";

    private void Start()
    {
        if (movement == null) movement = GetComponent<CharacterMovement2D>();
        rb = GetComponent<Rigidbody2D>();

        if (onStopRidingBeam == null)
            onStopRidingBeam = new UnityEvent();
    }

    //private void Update()
    //{
    //    if (!isRiding)
    //    {
    //        CheckForBeam();
    //    }
    //}

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("IN");
        int layer = collision.gameObject.layer;
        playerPositionOnTrigger = transform.position;

        if (layer == LayerMask.NameToLayer(targetLayerName))
        {

            beamController = collision.GetComponent<LightbeamController>();
            beamPlayerRideStart = beamController.edgeCollider.ClosestPoint(playerPositionOnTrigger);

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("OUT");
        beamController = null;
        beamPlayerRideStart = Vector2.zero;
    }

    public void CheckForBeam()
    {
        Collider2D beam = Physics2D.OverlapCircle(transform.position, detectionRadius, beamLayer);

        if (beam != null && beam != currentBeam) // Ensure we are detecting a new beam
        {
            Debug.Log("Player detected beam, starting ride.");
            currentBeam = beam;
            RideBeam(beam);
        }
    }

    public void RideBeam(Collider2D beam)
    {
        
        LightbeamController beamController = beam.GetComponent<LightbeamController>();
        if (beamController != null)
        {
            isRiding = true;
            Vector2 beamStart = beamController.GetRideStart();// Use the beam's start and end points
            Vector2 beamEnd = beamController.GetBeamEnd();
            travelDirection = (beamEnd - beamStart).normalized;

            StartCoroutine(RideCoroutine(beamStart, beamEnd));
        }
    }

    private System.Collections.IEnumerator RideCoroutine(Vector2 start, Vector2 end)
    {
        movement.CanMove = false;
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
    public void JumpOffBeam()
    {
        if (isRiding == false) return;
        StopRiding();
        isRiding = false;
        movement.CanMove = true;
        movement.Jump();
        rb.linearVelocity = travelDirection * rb.linearVelocity.magnitude;
    }

    private void StopRiding()
    {
        isRiding = false;
        movement.CanMove = true;
        onStopRidingBeam.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
