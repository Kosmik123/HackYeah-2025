using System.Collections.Generic;
using UnityEngine;

public abstract class GuardAction
{ }

[CreateAssetMenu(menuName = "HackYeah2025/Guard/Behavior", order = 1)]
public class GuardBehavior : ScriptableObject
{
    [SerializeReference, SubclassSelector]
    private GuardAction[] actions;
    public IReadOnlyList<GuardAction> Actions => actions;
}
