using UnityEngine;

namespace HackYeah2025
{
    public class WorldTime : MonoBehaviour
    {
        public const float MinutesInHour = 60f;
        public const float HoursInDay = 24f;

        [SerializeField]
        private float totalMinutes;

        [SerializeField]
        private float timeScale = 1f;

        [Header("States")]
        [SerializeField]
        private float hours;
        [SerializeField]
        private float minutes;

        public float Minutes => totalMinutes % MinutesInHour;
        public float Hours => totalMinutes / MinutesInHour % HoursInDay;
        public float Days => totalMinutes / (MinutesInHour * HoursInDay);

        private void Update()
        {
            totalMinutes += Time.deltaTime * timeScale;
            hours = Hours;  
            minutes = Minutes;
        }
    }
}
