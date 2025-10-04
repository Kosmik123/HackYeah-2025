using UnityEngine;

namespace HackYeah2025
{
    public partial class WorldTime : ScriptableObject
    {
        public static event System.Action OnTimeChanged;

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

        private void TimeChanged() => OnTimeChanged?.Invoke();
    }
}
