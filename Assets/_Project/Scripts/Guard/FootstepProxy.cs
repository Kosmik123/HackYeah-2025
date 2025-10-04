using UnityEngine;

public class FootstepProxy : MonoBehaviour
{
    [SerializeField] private Guard guardMovement;
    
    public void ProxyFootstep()
    {
        guardMovement.EmitFootstep();
    }
}
