using System.Collections.Generic;
using UnityEngine;

public class OutlineScenarioManager : MonoBehaviour
{
    public List<OutlineScenario> Scenarios;
    public ScenarioType CurrentScenario => scenarioType;

    private ScenarioType scenarioType;
    
    public void ActivateScenario(ScenarioType _scenarioType)
    {
        scenarioType = _scenarioType;
        
        foreach (var scenario in Scenarios)
        {
            var isActiveScenario = scenario.ScenarioType == scenarioType;
            foreach (var obj in scenario.ScenarioObjects)
            {
                if (obj == null)
                {
                    continue;
                }
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
