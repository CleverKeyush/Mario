using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MarioAgentScript : Agent
{
    public Transform groundCheck; // Reference to the GroundCheck object
    public LayerMask groundLayer; // Reference to the ground layer (set to "Default")

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
        // Reset Mario's position at the start of each episode
        transform.localPosition = new Vector3(-5, 0, 0);
        rb.velocity = Vector2.zero;
        isJumping = false;

        // Ensure groundCheck exists before proceeding
        if (groundCheck == null)
        {
            Debug.LogError("groundCheck object is not assigned or has been destroyed.");
            return;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Add a null check for groundCheck to avoid observation errors
        if (groundCheck == null)
        {
            Debug.LogWarning("groundCheck object is missing during observations. Skipping observation collection.");
            return; // Avoid collecting observations if groundCheck is missing
        }

        // Add observations
        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(transform.localPosition.y);
        sensor.AddObservation(rb.velocity.x);
        sensor.AddObservation(rb.velocity.y);

        // Ground check observation
        bool grounded = IsGrounded();
        sensor.AddObservation(grounded ? 1f : 0f);

        // Debugging information
        Debug.Log($"Position X: {transform.localPosition.x}, Position Y: {transform.localPosition.y}, Velocity X: {rb.velocity.x}, Velocity Y: {rb.velocity.y}, Grounded: {grounded}");
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (groundCheck == null)
        {
            Debug.LogWarning("groundCheck is missing during action. Skipping action execution.");
            return; // Avoid applying actions if groundCheck is missing
        }

        // Action[0] controls movement: 0 = no move, 1 = move right, 2 = move left
        float move = 0f;
        if (actions.DiscreteActions[0] == 1) move = 1f;
        else if (actions.DiscreteActions[0] == 2) move = -1f;

        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        // Action[1] controls jumping: 0 = no jump, 1 = jump
        if (IsGrounded() && actions.DiscreteActions[1] == 1 && !isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true;  // Track if Mario has jumped to avoid double-jumps
        }

        // End episode if Mario falls below a certain Y level
        if (transform.localPosition.y < -10f)
        {
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        // Manual keyboard controls: Arrow keys for movement, spacebar for jump
        discreteActionsOut[0] = 0; // No movement
        if (Input.GetKey(KeyCode.RightArrow)) discreteActionsOut[0] = 1;
        else if (Input.GetKey(KeyCode.LeftArrow)) discreteActionsOut[0] = 2;

        discreteActionsOut[1] = 0; // No jump
        if (Input.GetKey(KeyCode.Space)) discreteActionsOut[1] = 1;
    }

    private bool IsGrounded()
    {
        // Add a null check for groundCheck before calling OverlapCircle
        if (groundCheck == null) return false;

        // Cast a small circle at the groundCheck position to check for collisions with the ground layer
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if Mario collides with ground
        if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            // When Mario hits the ground, reset jump state
            isJumping = false;
        }
    }
}
