using System;
using UnityEngine;

public class ScenarioChangeZone : MonoBehaviour
{
    [SerializeField] private OutlineScenarioManager scenarioManager;
    [SerializeField] private ScenarioType zoneScenarioType;

    private void OnTriggerStay(Collider other)
    {
        if (scenarioManager.CurrentScenario == ScenarioType.None || scenarioManager.CurrentScenario == zoneScenarioType)
        {
            return;
        }
        
        if (other.CompareTag("Player"))
        {
            scenarioManager.ActivateScenario(zoneScenarioType);
        }
    }
}
