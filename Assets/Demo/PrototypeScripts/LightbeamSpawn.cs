using System;
using System.IO.Pipes;
using UnityEngine;

public class LightbeamSpawn : MonoBehaviour
{
    [SerializeField] private GameObject beamPrefab;
    public bool _isBeamSpawned = false;

    [SerializeField] private LightbeamController activeBeam;
    [SerializeField] private LightbeamRide lightbeamRide;
    private bool isListenerSet = false;


    private void Start()
    {
        lightbeamRide = GetComponent<LightbeamRide>();
    }
    
    private void Update()
    {
        if (lightbeamRide != null && lightbeamRide.isRiding == true && isListenerSet == false)
        {
            // Register the listener for the event from LightbeamRide
            lightbeamRide.onStopRidingBeam.AddListener(ReturnBeam);
            isListenerSet = true;
        }
        if (lightbeamRide == null && lightbeamRide.isRiding == false && isListenerSet == true)
        {
            // Register the listener for the event from LightbeamRide
            lightbeamRide.onStopRidingBeam.RemoveListener(ReturnBeam);
            isListenerSet = false;
        }
    }
    public void SpawnBeam(Vector2 start, Vector2 direction)
    {
      
        if (activeBeam != null && _isBeamSpawned == true)
        {

            ReturnBeam();
            return;
            
        }

        GameObject beamInstance = Instantiate(beamPrefab, start, Quaternion.identity);
        
        activeBeam = beamInstance.GetComponent<LightbeamController>();
        activeBeam.ExtendBeam(start, direction);
        activeBeam.gameObject.SetActive(true);
    }

    public void ReturnBeam()
    {
        _isBeamSpawned = false;
        Debug.Log("Beam Retracted");
        if (activeBeam != null)
        {
           Destroy(activeBeam.gameObject);
        }
    }
    
}
