using UnityEngine;

public class GrowthStages : MonoBehaviour
{
    private int currentStage = 0;
    public bool hasChangedState = false;

    // This method will be called by the animation events
    public void OnStageChange(int stage)
    {
        currentStage = stage;
        Debug.Log("Growth stage changed to: " + currentStage);
    }

    // This method can be called by animation events to set hasChangedState to true
    public void SetChangedStateTrue()
    {
        hasChangedState = true;
        Debug.Log("hasChangedState set to true");
    }

    // This method can be called by animation events to set hasChangedState to false
    public void SetChangedStateFalse()
    {
        hasChangedState = false;
        Debug.Log("hasChangedState set to false");
    }

    public int GetCurrentStage()
    {
        return currentStage;
    }

    public bool HasChangedState()
    {
        return hasChangedState;
    }
}
