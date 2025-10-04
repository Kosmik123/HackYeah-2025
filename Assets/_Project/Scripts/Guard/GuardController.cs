using UnityEngine;

public class GuardController : MonoBehaviour
{
    [SerializeReference, SubclassSelector]
    private GuardAction[] actions;

    [SerializeField]
    private int currentActionIndex = 0;
}