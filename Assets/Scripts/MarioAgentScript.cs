using UnityEngine; 
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MarioAgentScript : Agent
{
    public Transform groundCheck; // Reference to the GroundCheck object
    public LayerMask groundLayer;  // Reference to the ground layer
    public GameObject emptyBlockPrefab; // Reference to the empty block prefab

    private Rigidbody2D rb;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private bool isJumping;

    // Dynamic gapCheck distance, adjust based on Mario's movement
    private float baseGapCheckDistance = 1f; 

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnEpisodeBegin()
    {
        // Reset position and velocity at the start of each episode
        transform.localPosition = new Vector3(2, 2, 0); // Start within camera bounds
        rb.velocity = Vector2.zero;
        isJumping = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Mario's position and velocity
        sensor.AddObservation(transform.localPosition.x);  
        sensor.AddObservation(transform.localPosition.y);  
        sensor.AddObservation(rb.velocity.x);               
        sensor.AddObservation(rb.velocity.y);               

        // Ground check observation
        sensor.AddObservation(IsGrounded() ? 1f : 0f);     

        // Dynamic gap detection distance based on Mario's velocity
        float dynamicGapCheckDistance = baseGapCheckDistance + Mathf.Abs(rb.velocity.x) * 0.5f;

        // Gap detection in front of Mario
        bool gapDetected = !Physics2D.Raycast(transform.position, Vector2.down, dynamicGapCheckDistance, groundLayer);
        sensor.AddObservation(gapDetected ? 1f : 0f);

        // Enemy detection in front of Mario
        RaycastHit2D enemyHit = Physics2D.Raycast(transform.position, Vector2.right, dynamicGapCheckDistance, LayerMask.GetMask("Enemy"));
        sensor.AddObservation(enemyHit.collider != null ? 1f : 0f);

        // Obstacle detection in front of Mario (e.g., pipes or blocks)
        RaycastHit2D obstacleHit = Physics2D.Raycast(transform.position, Vector2.right, dynamicGapCheckDistance, LayerMask.GetMask("Default"));
        sensor.AddObservation(obstacleHit.collider != null ? 1f : 0f);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float move = 0f;

        // Action for moving left or right
        if (actions.DiscreteActions[0] == 1) move = 1f;    
        else if (actions.DiscreteActions[0] == 2) move = -1f; 

        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        // Reward for moving forward
        if (move > 0) AddReward(0.02f);  
        if (move < 0) AddReward(-0.02f); // Penalty for moving backward

        // Penalty for jumping when not grounded
        if (!IsGrounded() && actions.DiscreteActions[1] == 1)
        {
            AddReward(-0.05f); 
        }

        // Jump logic: only allow jump if grounded
        if (IsGrounded() && actions.DiscreteActions[1] == 1 && !isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true; // Prevent multiple jumps
        }

        // End episode if Mario falls too low
        if (transform.localPosition.y < -10f)
        {
            AddReward(-1.0f); 
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = 0;

        if (Input.GetKey(KeyCode.RightArrow)) discreteActionsOut[0] = 1; // Move right
        else if (Input.GetKey(KeyCode.LeftArrow)) discreteActionsOut[0] = 2; // Move left

        discreteActionsOut[1] = 0;
        if (Input.GetKey(KeyCode.Space)) discreteActionsOut[1] = 1; // Jump
    }

    private bool IsGrounded()
    {
        if (groundCheck == null) return false;

        // Use Raycast to check if Mario is grounded
        bool grounded = Physics2D.Raycast(groundCheck.position, Vector2.down, 2f, groundLayer);

        if (grounded)
        {
            isJumping = false; // Reset jump flag when grounded
        }
        return grounded;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            isJumping = false; // Reset jump state when landing on the ground
        }

        if (collision.gameObject.CompareTag("coin"))
        {
            // Check if Mario is hitting the block from below
            if (collision.contacts[0].point.y > transform.position.y)
            {
                Debug.Log("Hit a coin block!"); // Log when hitting a coin block
                AddReward(1.0f); // Reward for hitting the block

                // Instantiate an empty block at the mystery block's position
                Instantiate(emptyBlockPrefab, collision.transform.position, Quaternion.identity);

                Destroy(collision.gameObject); // Destroy the block
            }
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Check if Mario is landing on top of the enemy
            if (collision.contacts[0].point.y > transform.position.y)
            {
                Debug.Log("Defeated an enemy!"); // Log when an enemy is defeated
                rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Bounce off the enemy
                Destroy(collision.gameObject);
                AddReward(1.5f); // Higher reward for defeating an enemy
                // Do NOT call EndEpisode here to avoid resetting the position
            }
            else
            {
                Debug.Log("Hit by an enemy!"); // Log when hit by an enemy
                AddReward(-1.0f); // Penalty for being hit by an enemy
                EndEpisode(); // End the episode
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * 2f); // Adjusted distance for debugging
        }
    }
}
