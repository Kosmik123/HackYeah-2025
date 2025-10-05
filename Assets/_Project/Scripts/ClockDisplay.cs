using UnityEngine;
using TMPro;
using HackYeah2025;

public class ClockDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text display;

    private void OnEnable()
    {
        WorldTime.Instance.OnTimeChanged += Instance_OnTimeChanged;   
    }

    private void Instance_OnTimeChanged()
    {
        int hour = WorldTime.WholeHours;
        int minute = WorldTime.WholeMinutes;
        display.SetText($"{hour}:{minute:D2}");
    }

    private void OnDisable()
    {
        WorldTime.Instance.OnTimeChanged -= Instance_OnTimeChanged;   
    }
}
