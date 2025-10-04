using UnityEngine;

namespace HackYeah2025
{
    [System.Serializable]
    public struct ClockArm
    {
        public Transform Transform;
        public Vector3 Axis;
    }

    public class Clock : MonoBehaviour
    {
        [SerializeField]
        private ClockArm hourArm;
        [SerializeField]
        private ClockArm minuteArm;

        private void Update()
        {
            SetHourArm();
            SetMinuteArm();
        }

        private void SetHourArm() => SetArmRotation(hourArm, 360 * WorldTime.Hours / 12);
        private void SetMinuteArm() => SetArmRotation(minuteArm, 360 * WorldTime.Minutes / WorldTime.MinutesInHour);

        private static void SetArmRotation(ClockArm arm, float angle)
        {
            if (arm.Transform == null)
                return;

            arm.Transform.localRotation = Quaternion.AngleAxis(angle, arm.Axis);
        }
    }
}
