using HackYeah2025;
using UnityEngine;

[System.Serializable]
public abstract class GuardEventCondition
{
    public abstract bool IsMet();
}

public enum TimeConditionType
{
    After,
    Before,
}

[System.Serializable]
public class HourCondition : GuardEventCondition
{
    [SerializeField]
    private TimeConditionType conditionType;
    [SerializeField]
    private int hour;
    
    public override bool IsMet()
    {
        return conditionType == TimeConditionType.Before
            ? WorldTime.Hours < hour
            : WorldTime.Hours >= hour;
    }
}

[System.Serializable]
public class DayCondition : GuardEventCondition
{
    [SerializeField]
    private TimeConditionType conditionType;
    [SerializeField]
    private int day;
    
    public override bool IsMet()
    {
        return conditionType == TimeConditionType.Before
            ? WorldTime.Days < day
            : WorldTime.Days >= day;
    }
}

