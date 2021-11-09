using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class LookAtCameraDirection : MonoBehaviour
{
    public Transform cam;
    public Rigidbody ball;
    // Start is called before the first frame update
    private Quaternion rotation; 
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    Rigidbody clone;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotation = Quaternion.LookRotation(cam.forward, cam.up);

        /*if (Input.GetMouseButtonDown(0))
        {
            throwItem(rotation);
        }*/
    }
    
    void throwItem(Quaternion rotation)
    {
        //Spawn
        float x = this.gameObject.transform.position.x;
        float y = this.gameObject.transform.position.y+5;
        float z = this.gameObject.transform.position.z; 
            
        clone = Instantiate(ball, new Vector3(x,y,z), rotation);
        clone.useGravity = false;

        //move direction
        //clone.velocity = clone.transform.TransformDirection(Vector3.forward * 30);
        //Destroy(clone.gameObject, 3f);
    }

    private Vector3 mousePressDownPos;

    private Vector3 mouseReleasePos;


    private LineRenderer lineRenderer;
    private void OnMouseDown()
    {
        throwItem(rotation);
        updateLine();

    }
    private void OnMouseUp()
    {
        //move direction
        clone.velocity = clone.transform.TransformDirection(Vector3.forward * 30);
        clone.useGravity = true;
        Destroy(clone.gameObject, 3f);

    }
    
    public float angle;
    public int resolution;

    private float g;
    private float radianAngle; 
    void updateLine()
    {
        g = Mathf.Abs(Physics.gravity.y);
        //For creating line renderer object
        lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
        lineRenderer.startWidth = 0.11f;
        lineRenderer.endWidth = 0.11f;
        
        //Comment this out
        lineRenderer.positionCount = resolution+1;
        
        
        lineRenderer.useWorldSpace = true;
        
        
        lineRenderer.SetPosition(0, clone.transform.position); //x,y and z position of the starting point of the line
        lineRenderer.SetPosition(1, clone.transform.TransformDirection(Vector3.forward * 3600)); //x,y and z position of the end point of the line
        //lineRenderer.SetPositions(UpdateTrajectory(clone.transform.TransformDirection(Vector3.forward * 3600), clone, clone.transform.position));
        Destroy(lineRenderer.gameObject, 3f);
        
        
    }
    
    [SerializeField] private LineRenderer _lineRenderer;

    [SerializeField] [Range(3, 30)] private int _lineSegmentCount = 20;
    
    private List<Vector3> _linePoints = new List<Vector3>();
    
    public Vector3[] UpdateTrajectory(Vector3 forceVector, Rigidbody rigidbody, Vector3 startingPoint)
    {
        Vector3 velocity = (forceVector / rigidbody.mass) * Time.fixedDeltaTime;

        float FlightDuration = (2 * velocity.y) / Physics.gravity.y;

        float stepTime = FlightDuration / _lineSegmentCount; 
        
        _linePoints.Clear();

        for (int i = 0; i < _lineSegmentCount; i++)
        {
            float stepTimePassed = stepTime * i; 
            
            Vector3 MovementVector = new Vector3(velocity.x * stepTimePassed, velocity.y*stepTimePassed-0.5f * Physics.gravity.y*stepTimePassed*stepTimePassed, velocity.z*stepTimePassed);

            RaycastHit hit;
            if (Physics.Raycast(startingPoint, -MovementVector, out hit, MovementVector.magnitude))
            {
                break;
            }
            _linePoints.Add(-MovementVector + startingPoint);
        }

        //_lineRenderer.positionCount = _linePoints.Count;
        lineRenderer.positionCount = _linePoints.Count;
        return _linePoints.ToArray(); 
        _lineRenderer.SetPositions(_linePoints.ToArray());
    }
    
    

    


}

