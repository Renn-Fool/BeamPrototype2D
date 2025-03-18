using System.Collections.Generic;
using System.IO.Pipes;
using Unity.VisualScripting;
using UnityEngine;

public class LightbeamController : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private EdgeCollider2D edgeCollider;
    private LightbeamRide beamRide;
    [SerializeField] private List<Vector2> beamPoints = new List<Vector2>();

    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private string targetLayerName = "Character";
    [SerializeField] private float maxBeamLength = 500f;
    [SerializeField] private int maxReflections = 5;
    private Vector2 beamEnd;
    private Vector2 beamPlayerRideStart;
    private Vector2 playerPositionOnTrigger;


    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        edgeCollider.isTrigger = true;
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
    
        int layer = collision.gameObject.layer;
        playerPositionOnTrigger = collision.transform.position;
    
        if (layer == LayerMask.NameToLayer(targetLayerName))
        {
    
            beamRide = collision.GetComponent<LightbeamRide>();
            beamPlayerRideStart = edgeCollider.ClosestPoint(playerPositionOnTrigger);
            
        }
    }

    public void ExtendBeam(Vector2 start, Vector2 direction)
    {
        beamPoints.Clear();
        beamPoints.Add(start);

        Vector2 currentStart = start;
        Vector2 currentDirection = direction; //these current var's are here to determine the where in the reflection we are
        int reflections = 0;


        //Debug.DrawLine(start, direction * 100);
        //RaycastHit2D hit = Physics2D.Raycast(currentStart, currentDirection.normalized, maxBeamLength, collisionMask);

        while (reflections <= maxReflections)
        {
            Debug.DrawLine(start, direction * 100);
            RaycastHit2D hit = Physics2D.Raycast(currentStart, currentDirection.normalized, maxBeamLength, collisionMask);

            if (hit.collider != null)
            {
                
                beamPoints.Add(hit.point);
                Debug.Log($"Beam hit object: {hit.collider.name} at {beamEnd}");


                IReflective reflectiveObject = hit.collider.GetComponent<IReflective>();
                if (reflectiveObject != null && reflections < maxReflections - 1)
                {
                    currentDirection = reflectiveObject.ReflectBeam(currentDirection, hit.normal);
                    currentStart = hit.point;
                    reflections++;
                    continue;

                }

                break;

            }
            else
            {
                //if nothing is hit go the max distance
               beamPoints.Add(currentDirection.normalized * maxBeamLength);
               Debug.Log("Beam did not hit anything, setting beamEnd to max length.");

               break;  
            }
        }

        DrawBeam();
        
    }

    private void DrawBeam()
    {
        lineRenderer.positionCount = beamPoints.Count;
        for (int i = 0; i < beamPoints.Count; i++)
        {
            lineRenderer.SetPosition(i, beamPoints[i]);
        }

        SetEdgeCollider();
    }
    public void SetEdgeCollider()
    {
        if (beamPoints.Count > 1)
        {
            edgeCollider.SetPoints(beamPoints);
        }
    }

    public Vector2 GetBeamEnd()
    {
        return beamPoints[^1];
    }

    public Vector2 GetRideStart()
    {
        return beamPlayerRideStart;
    }

    
    private void OnDrawGizmos()
    {
        if (edgeCollider == null || lineRenderer == null) return;


        Gizmos.color = Color.red;


        List<Vector2> points = new List<Vector2>();
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            Vector3 lineRendererPoint = lineRenderer.GetPosition(i);
            points.Add(new Vector2(lineRendererPoint.x, lineRendererPoint.y));
        }


        for (int i = 0; i < points.Count - 1; i++)
        {
            Gizmos.DrawLine(points[i], points[i + 1]);
        }
    }

}
 
//TODO LIST
//beam spawn position offset forward a little in the direction of the mouse
//make sure player only starts moving when directly on beam
//make em reflectable
//player floats above beam and travels instead of directily on it