using System;
using UnityEngine;

public class TooltipReceiver : MonoBehaviour
{
    [SerializeField] private UISystem uiSystem;
    
    private TooltipObject prevObj;
    
    private void Update()
    {
        if (GameManager.Instance.InteractionBlocked)
        {
            return;
        }
        
        var ray = new Ray(transform.position, transform.forward);
        var hits = Physics.RaycastAll(ray, 2f);
        var found = false;
        foreach (var hit in hits)
        {
            var target = hit.collider.transform;
            while (target != null)
            {
                if (target.TryGetComponent<TooltipObject>(out var tooltipObj))
                {
                    found = true;
                
                    if (prevObj == tooltipObj)
                    {
                        return;
                    }

                    prevObj = tooltipObj;
                    uiSystem.ShowTooltip(tooltipObj.tooltip);
                    break;
                }
                
                target = target.parent;
            }

            if (found)
            {
                break;
            }
        }

        if (!found)
        {
            uiSystem.HideTooltip();
            prevObj = null;
        }
    }
}
