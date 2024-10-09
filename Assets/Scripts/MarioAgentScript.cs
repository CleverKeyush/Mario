using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MarioAgentScript : Agent
{
    public Transform groundCheck; // Reference to the GroundCheck object
    public LayerMask groundLayer;  // Reference to the ground layer

    private Rigidbody2D rb;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private bool isJumping;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnEpisodeBegin()
    {
        // Reset Mario's position only at the start of the episode, not after killing an enemy
        transform.localPosition = new Vector3(2, 2, 0); // Start within camera bounds
        rb.velocity = Vector2.zero;
        isJumping = false;

        Debug.Log($"Mario's position at start: {transform.localPosition}");
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(transform.localPosition.y);
        sensor.AddObservation(rb.velocity.x);
        sensor.AddObservation(rb.velocity.y);

        // Ground check observation
        bool grounded = IsGrounded();
        sensor.AddObservation(grounded ? 1f : 0f);

        Debug.Log($"Position X: {transform.localPosition.x}, Position Y: {transform.localPosition.y}, Velocity X: {rb.velocity.x}, Velocity Y: {rb.velocity.y}, Grounded: {grounded}");
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float move = 0f;

        if (actions.DiscreteActions[0] == 1) move = 1f;
        else if (actions.DiscreteActions[0] == 2) move = -1f;

        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        // Jump logic: Only allow jump if grounded
        if (IsGrounded() && actions.DiscreteActions[1] == 1 && !isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true; // Prevent multiple jumps
        }

        // End episode if Mario falls too low
        if (transform.localPosition.y < -10f)
        {
            EndEpisode();
        }

        Debug.Log($"Jump Action: {actions.DiscreteActions[1]}, Is Grounded: {IsGrounded()}");
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = 0;

        if (Input.GetKey(KeyCode.RightArrow)) discreteActionsOut[0] = 1;
        else if (Input.GetKey(KeyCode.LeftArrow)) discreteActionsOut[0] = 2;

        discreteActionsOut[1] = 0;
        if (Input.GetKey(KeyCode.Space)) discreteActionsOut[1] = 1;
    }

    private bool IsGrounded()
    {
        if (groundCheck == null) return false;

        // Use Raycast to check if Mario is grounded
        bool grounded = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.5f, groundLayer); // Increased raycast distance for reliability

        if (grounded)
        {
            isJumping = false; // Reset jump flag when grounded
        }

        Debug.Log($"Is Mario grounded? {grounded}");
        return grounded;
    }

    // OnCollisionEnter2D to reset isJumping when Mario lands on the ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isJumping = false; // Reset jump state when landing on the ground
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // Check if Mario is landing on top of the enemy
            if (collision.contacts[0].point.y > transform.position.y)
            {
                // Mario jumps after stomping on an enemy
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                // Optionally destroy enemy or trigger defeat animation
                Destroy(collision.gameObject);
            }
            else
            {
                // Mario takes damage or the episode ends, but we don't reset position here
                // Custom logic for handling enemy collision without resetting
                Debug.Log("Mario hit by an enemy");
            }
        }
    }

    // Optional: Visualize ground check area in the editor (for debugging)
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * 0.5f); // Adjusted distance for debugging
        }
    }
}
