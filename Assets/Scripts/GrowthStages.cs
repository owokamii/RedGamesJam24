using UnityEngine;

public class GrowthStages : MonoBehaviour
{
    private int currentStage = 0;

    // This method will be called by the animation events
    public void OnStageChange(int stage)
    {
        currentStage = stage;
        Debug.Log("Growth stage changed to: " + currentStage);
    }

    public int GetCurrentStage()
    {
        return currentStage;
    }
}
