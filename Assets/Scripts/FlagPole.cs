using System.Collections;
using UnityEngine;

public class FlagPole : MonoBehaviour
{
    public Transform flag;
    public Transform poleBottom;
    public Transform castle;
    public float speed = 6f;
    public float levelCompleteWaitTime = 2f;  // Adjustable wait time after completing the level

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
            StartCoroutine(MoveTo(flag, poleBottom.position));
            StartCoroutine(LevelCompleteSequence(player));
        }
    }

    private IEnumerator LevelCompleteSequence(Player player)
    {
        player.movement.enabled = false;

        yield return MoveTo(player.transform, poleBottom.position);
        yield return MoveTo(player.transform, player.transform.position + Vector3.right);
        yield return MoveTo(player.transform, player.transform.position + Vector3.right + Vector3.down);
        yield return MoveTo(player.transform, castle.position);

        player.gameObject.SetActive(false);

        yield return new WaitForSeconds(levelCompleteWaitTime);

        if (GameManager.Instance != null)
        {
            AdvanceToNextLevel();
        }
        else
        {
            Debug.LogWarning("GameManager instance not found! Cannot load next level.");
        }
    }

    private void AdvanceToNextLevel()
    {
        int currentWorld = GameManager.Instance.world;
        int currentStage = GameManager.Instance.stage;

        // Assuming each world has a certain number of stages
        int maxStagesInWorld = 3; // Example: 3 stages per world

        if (currentStage < maxStagesInWorld)
        {
            GameManager.Instance.LoadLevel(currentWorld, currentStage + 1); // Go to next stage in current world
        }
        else
        {
            GameManager.Instance.LoadLevel(currentWorld + 1, 1); // Move to the first stage of the next world
        }
    }

    private IEnumerator MoveTo(Transform subject, Vector3 position)
    {
        while (Vector3.Distance(subject.position, position) > 0.125f)
        {
            subject.position = Vector3.MoveTowards(subject.position, position, speed * Time.deltaTime);
            yield return null;
        }

        subject.position = position;
    }
}
