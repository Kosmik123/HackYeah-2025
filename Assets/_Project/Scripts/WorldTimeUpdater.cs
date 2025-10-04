using UnityEngine;
namespace HackYeah2025
{
    public partial class WorldTime
    {
        [AddComponentMenu("")]
        private class Updater : MonoBehaviour
        {
            [SerializeField]
            private float totalMinutes;

            [Header("States")]
            [SerializeField]
            private float days;
            public float Days => days;

            [SerializeField]
            private float hours;
            public float Hours => hours;

            [SerializeField]
            private float minutes;
            public float Minutes => minutes;
            private int previousWholeMinutes = -1;

            public static void Initialize(float totalMinutes)
            {
                if (Instance.updater != null)
                    return;

                var obj = new GameObject("World Time");
                DontDestroyOnLoad(obj);
                Instance.updater = obj.AddComponent<Updater>();
            }

            private void Update()
            {
                totalMinutes += Time.deltaTime * Instance.TimeScale;
                minutes = totalMinutes % MinutesInHour;
                hours = totalMinutes / MinutesInHour % HoursInDay;
                days = totalMinutes / (MinutesInHour * HoursInDay);

                if (WholeMinutes != previousWholeMinutes)
                {
                    previousWholeMinutes = WholeMinutes;
                    Instance.TimeChanged();
                }
            }
        }

    }
}