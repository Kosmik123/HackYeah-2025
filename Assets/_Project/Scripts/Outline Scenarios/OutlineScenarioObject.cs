using UnityEngine;

public class OutlineScenarioObject : MonoBehaviour
{
    [SerializeField] private Outline.Mode mode;
    [SerializeField] private Color color = Color.white;
    [SerializeField, Range(0f, 10f)] private float width = 2f;
    
    private Outline outline;
    
    public void ShowOutline(bool show)
    {
        if (outline == null)
        {
            outline = gameObject.AddComponent<Outline>();
        }
        outline.OutlineMode = mode;
        outline.OutlineColor = color;
        outline.OutlineWidth = width;
        
        if (outline != null)
        {
            outline.enabled = show;
        }
    }
}
