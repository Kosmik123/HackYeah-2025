using UnityEngine;

namespace HackYeah2025
{
    public class Clock : MonoBehaviour
    {
        [SerializeField]
        private WorldTime worldTime;
        [Space]
        [SerializeField]
        private Transform hourArm;
        [SerializeField]
        private Transform minuteArm;

        private void Reset()
        {
            worldTime = FindAnyObjectByType<WorldTime>();
        }

        private void Update()
        {
            SetHourArm();
            SetMinuteArm(); 
        }

        private void SetHourArm() => SetArmRotation(hourArm, 360 * worldTime.Hours / WorldTime.HoursInDay);
        private void SetMinuteArm() => SetArmRotation(minuteArm, 360 * worldTime.Minutes / WorldTime.MinutesInHour);

        private static void SetArmRotation (Transform arm, float angle)
        {
            if (arm)    
                arm.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);  
        }
    }
}
