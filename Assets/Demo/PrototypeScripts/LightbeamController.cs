using System.Collections.Generic;
using System.IO.Pipes;
using Unity.VisualScripting;
using UnityEngine;

public class LightbeamController : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private List<Transform> nodes;
    [SerializeField] private EdgeCollider2D edgeCollider;
    private List<Vector2> colliderPoints = new List<Vector2>();
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private string targetLayerName = "Character";
    [SerializeField] private float maxBeamLength = 500f;

    [SerializeField] private Vector2 beamStart;
    [SerializeField] private Vector2 beamEnd;
    private LightbeamRide beamRide;
    private bool beamActive;


    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        edgeCollider.isTrigger = true;
    }

    private void Update()
    {
        SetEdgeCollider(lineRenderer);
    }
    private void SetEdgeCollider(LineRenderer lineRenderer)
    {
        List<Vector2> edges = new List<Vector2>();

        for (int point = 0; point<lineRenderer.positionCount; point++)
        {
            Vector3 lineRendererPoint = lineRenderer.GetPosition(point);
            edges.Add(new Vector2(lineRendererPoint.x, lineRendererPoint.y));   
        }
        edgeCollider.SetPoints(edges);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player entered the beam. Starting ride.");
        int layer = collision.gameObject.layer;
        if (layer == LayerMask.NameToLayer(targetLayerName))
        {
            Debug.Log("Player entered the beam. Starting ride.");
            beamRide = collision.GetComponent<LightbeamRide>();
            if(beamRide != null)
            {
                beamRide.RideBeam(beamStart, beamEnd);
                Debug.Log("Player entered the beam. Starting ride.");
            }
        }
    }
    public void FireBeam(Vector2 start, Vector2 direction)
    {
       
        beamStart = start;
        Debug.DrawLine(start, direction * 100);
        RaycastHit2D hit = Physics2D.Raycast(beamStart, direction.normalized, maxBeamLength, collisionMask);
        Debug.Log($"Fire Direction: {direction}, Magnitude: {direction.magnitude}");


        if (hit.collider != null)
        {
            beamEnd = hit.point;
            Debug.Log($"Beam hit object: {hit.collider.name} at {beamEnd}");
        }
        else
        {
            beamEnd = beamStart + (direction.normalized * maxBeamLength);
            Debug.Log("Beam did not hit anything, setting beamEnd to max length.");
        }

        DrawBeam();
        beamActive = true;
    }

    private void DrawBeam()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, beamStart);
        lineRenderer.SetPosition(1, beamEnd);
    }

    public Vector2 GetBeamStart()
    {
        return beamStart;
    }

    public Vector2 GetBeamEnd()
    {
        return beamEnd;
    }
}
