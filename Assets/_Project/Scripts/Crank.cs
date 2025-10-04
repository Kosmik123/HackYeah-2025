using UnityEngine;

public class Crank : MonoBehaviour
{
    private static readonly int Rotate = Animator.StringToHash("rotate");

    private Animator anim;
    
    public Transform shaft;
    
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    
    [ContextMenu("Move Crank")]
    void MoveCrank()
    {
        anim.SetTrigger(Rotate);
    }

    public void OnRotation()
    {
        
    }
}
