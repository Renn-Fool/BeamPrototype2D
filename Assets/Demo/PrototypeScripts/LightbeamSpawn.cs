using System;
using System.IO.Pipes;
using UnityEngine;

public class LightbeamSpawn : MonoBehaviour
{
    [SerializeField] private GameObject beamPrefab;
    public bool _isBeamSpawned = false;

    [SerializeField] private LightbeamController activeBeam;
    [SerializeField] private LightbeamRide lightbeamRide;

    private void Update()
    {
        if (lightbeamRide != null)
        {
            // Register the listener for the event from LightbeamRide
            lightbeamRide.onStopRidingBeam.AddListener(ReturnBeam);
        }
    }
    public void SpawnBeam(Vector2 start, Vector2 direction)
    {
        //start = _beamSpawnTransform.transform.position;
        if (activeBeam != null && _isBeamSpawned == true)
        {
            Debug.Log("Beam Retracted");
            ReturnBeam();
            return;
            
        }

        GameObject beamInstance = Instantiate(beamPrefab, start, Quaternion.identity);
        
        activeBeam = beamInstance.GetComponent<LightbeamController>();
        activeBeam.FireBeam(start, direction);
    }

    public void ReturnBeam()
    {
        _isBeamSpawned = false;
        Destroy(activeBeam.gameObject);
    }
    private void OnDestroy()
    {
        if (lightbeamRide != null)
        {
            // Unregister the listener to avoid memory leaks
            lightbeamRide.onStopRidingBeam.RemoveListener(ReturnBeam);
        }
    }
}
