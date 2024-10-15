using Unity.MLAgents;

public class MarioAcademy : Academy
{
    public int totalEpisodes;

    public override void InitializeAcademy()
    {
        Debug.Log("Mario Academy Initialized");
        totalEpisodes = 0; // Initialize episode counter
    }

    public override void AcademyReset()
    {
        Debug.Log("Mario Academy Reset");
        totalEpisodes++; // Increment episode counter
    }

    public override void AcademyStep()
    {
        // Optional: Add logic to run every step
    }
}