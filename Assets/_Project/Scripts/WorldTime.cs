using UnityEngine;

namespace HackYeah2025
{
    public class WorldTime : ScriptableObject
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
            }
        }

        public const float MinutesInHour = 60f;
        public const float HoursInDay = 24f;

        private static WorldTime instance;
        public static WorldTime Instance
        {
            get
            {
                if (instance == null)
                    CreateInstance();
                return instance;
            }
        }

        private Updater updater; 

        [SerializeField]
        private float timeScale = 1f;
        public float TimeScale => timeScale;

        [SerializeField]
        private float startingMinutes = 0;
        public float StartingMinutes => startingMinutes;

        public static float Minutes => Instance.updater.Minutes;
        public static float Hours => Instance.updater.Hours;
        public static float Days => Instance.updater.Days;   
        public static int WholeMinutes => Mathf.FloorToInt(Minutes);
        public static int WholeHours => Mathf.FloorToInt(Hours);
        public static int WholeDays => Mathf.FloorToInt(Days);

        private static void CreateInstance()
        {
            instance = Resources.Load<WorldTime>("World Time");
            Updater.Initialize(instance.startingMinutes);
        }
    }
}
