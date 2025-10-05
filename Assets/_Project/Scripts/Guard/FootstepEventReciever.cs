using UnityEngine;

public class FootstepEventReciever : MonoBehaviour
{
    private Guard guard;
    private void Start()
    {
        guard = GetComponentInParent<Guard>();
    }

    public void OnFootstep()
    {
        guard.EmitFootstep();
    }
}
