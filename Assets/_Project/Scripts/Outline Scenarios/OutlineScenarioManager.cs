using System.Collections.Generic;
using UnityEngine;

public class OutlineScenarioManager : MonoBehaviour
{
    public List<OutlineScenario> Scenarios;

    private ScenarioType scenarioType;
    
    public void ActivateScenario(ScenarioType _scenarioType)
    {
        scenarioType = _scenarioType;
        
        foreach (var scenario in Scenarios)
        {
            var isActiveScenario = scenario.ScenarioType == scenarioType;
            foreach (var obj in scenario.ScenarioObjects)
            {
                obj.ShowOutline(isActiveScenario);
            }
        }
    }

    [ContextMenu("Toggle Scenario")]
    private void ToggleScenario()
    {
        ActivateScenario((ScenarioType)(((int)scenarioType + 1) % ((int)ScenarioType.Cave + 1)));
    }
}
