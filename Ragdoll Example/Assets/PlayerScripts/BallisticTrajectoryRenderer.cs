using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ballistic trajectory renderer.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class BallisticTrajectoryRenderer : MonoBehaviour
{
    // Reference to the line renderer
    private LineRenderer line;

    // Initial trajectory position
    [SerializeField]
    private Vector3 startPosition;

    // Initial trajectory velocity
    [SerializeField]
    private Vector3 startVelocity;

    // Step distance for the trajectory
    [SerializeField]
    private float trajectoryVertDist = 0.25f;

    // Max length of the trajectory
    [SerializeField]
    private float maxCurveLength = 5;

    [Header("Debug")]
    // Flag for always drawing trajectory
    [SerializeField]
    private bool _debugAlwaysDrawTrajectory = false;

    public GameObject playPos;
    private Quaternion rotation;
    public Transform cam;
    public Rigidbody throwItem;
    Rigidbody clone;
    


    /// Method called on initialization.
    private void Awake()
    {
        // Get line renderer reference
        line = GetComponent<LineRenderer>();
    }

    /// Method called on every frame.
    private void Update()
    {
        // Draw trajectory while pressing button
        if (Input.GetButton("Fire2") || _debugAlwaysDrawTrajectory)
        {
            // Draw trajectory
            DrawTrajectory();
        }
        // Clear trajectory after releasing button
        if (Input.GetButtonUp("Fire2") && !_debugAlwaysDrawTrajectory)
        {
            // Clear trajectory
            ClearTrajectory();
        }
        
        Vector3 pPos = new Vector3(playPos.gameObject.transform.position.x, playPos.gameObject.transform.position.y+5, playPos.gameObject.transform.position.z);
        //this.startPosition = pPos;


        rotation = Quaternion.LookRotation(cam.forward, cam.up);

        //very expensive //todo --> make better
        clone = Instantiate(throwItem, pPos, rotation);
        //Optimizing performance by disabling collision
        clone.GetComponent<Rigidbody>().detectCollisions = false;

        clone.velocity = clone.transform.TransformDirection(Vector3.forward * 30);
        //this.startVelocity = clone.velocity;
        SetBallisticValues(pPos, clone.velocity);


        Destroy(clone.gameObject);
        
    }
    /// Sets ballistic values for trajectory.
  
    public void SetBallisticValues(Vector3 startPosition, Vector3 startVelocity)
    {
        this.startPosition = startPosition;
        //this.startPosition = playPos.gameObject.transform.position;
        this.startVelocity = startVelocity;
    }

    /// Draws the trajectory with line renderer.

    private void DrawTrajectory()
    {
        // Create a list of trajectory points
        var curvePoints = new List<Vector3>();
        curvePoints.Add(startPosition);
        // Initial values for trajectory
        var currentPosition = startPosition;
        var currentVelocity = startVelocity;
        // Init physics variables
        RaycastHit hit;
        Ray ray = new Ray(currentPosition, currentVelocity.normalized);
        // Loop until hit something or distance is too great
        while (!Physics.Raycast(ray, out hit, trajectoryVertDist) && Vector3.Distance(startPosition, currentPosition) < maxCurveLength)
        {
            // Time to travel distance of trajectoryVertDist
            var t = trajectoryVertDist / currentVelocity.magnitude;
            // Update position and velocity
            currentVelocity = currentVelocity + t * Physics.gravity;
            currentPosition = currentPosition + t * currentVelocity;
            // Add point to the trajectory
            curvePoints.Add(currentPosition);
            // Create new ray
            ray = new Ray(currentPosition, currentVelocity.normalized);
        }
        // If something was hit, add last point there
        if (hit.transform)
        {
            curvePoints.Add(hit.point);
        }
        // Display line with all points
        line.positionCount = curvePoints.Count;
        line.SetPositions(curvePoints.ToArray());

    }
    /// Clears the trajectory.
    private void ClearTrajectory()
    {
        // Hide line
        line.positionCount = 0;

    }


}