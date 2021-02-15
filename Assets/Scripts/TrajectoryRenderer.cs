using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;











/// <summary>
/// Code taken from Patryk Galach at https://www.patrykgalach.com/2020/03/23/drawing-ballistic-trajectory-in-unity/.
/// </summary>


















[RequireComponent(typeof(LineRenderer))]
public class TrajectoryRenderer : MonoBehaviour
{
    private LineRenderer line; // Reference to the line renderer
    [SerializeField] private Vector3 startPosition; // Initial trajectory position
    [SerializeField] private Vector3 startVelocity; // Initial trajectory velocity
    [SerializeField] private float trajectoryVertDist = 0.25f; // Step distance for the trajectory
    [SerializeField] private float maxCurveLength = 10; // Max length of the trajectory
    private bool m_bShouldDraw = false;
    private void Awake()
    {
        line = GetComponent<LineRenderer>(); // Get line renderer reference
        Assert.IsNotNull(line, "Error! The TrajectoryRenderer has no LineRenderer to render!");
    }
    private void Update()
    {
        if (m_bShouldDraw)
        {
            DrawTrajectory();
        }
        else
        {
            ClearTrajectory();
        }
    }
    public void SetInitialPositionAndVelocity(Vector3 position, Vector3 velocity)
    {
        startPosition = position;
        startVelocity = velocity;
    }
    /// <summary>
    /// Draws the trajectory with line renderer.
    /// </summary>
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
    /// <summary>
    /// Clears the trajectory.
    /// </summary>
    private void ClearTrajectory()
    {
        // Hide line
        line.positionCount = 0;
    }
    #region CALLBACKS
    public void OnSetShouldDraw(bool value)
    {
        m_bShouldDraw = value;
    }
    #endregion
}
