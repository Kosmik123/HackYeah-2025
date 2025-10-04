using UnityEngine;

public class CrankInteract : MonoBehaviour, IInteractable
{
    public CrankAnim crank;
    public void Interact()
    {
        crank.MoveCrank();
    }
}
