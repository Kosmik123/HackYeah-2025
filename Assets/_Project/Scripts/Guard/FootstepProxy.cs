using UnityEngine;

public class FootstepProxy : MonoBehaviour
{
    [SerializeField] private GuardMovement guardMovement;
    
    public void ProxyFootstep()
    {
        guardMovement.EmitFootstep();
    }
}
