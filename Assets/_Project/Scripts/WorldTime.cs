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
        private float startingHour = 0;
        [SerializeField]
        private float startingMinute = 0;

        public static float Minutes => Instance.updater.Minutes;
        public static float Hours => Instance.updater.Hours;
        public static float Days => Instance.updater.Days;   
        public static int WholeMinutes => Mathf.FloorToInt(Minutes);
        public static int WholeHours => Mathf.FloorToInt(Hours);
        public static int WholeDays => Mathf.FloorToInt(Days);

        private static void CreateInstance()
        {
            instance = Resources.Load<WorldTime>("World Time");
            Updater.Initialize(instance.startingHour * 60 + instance.startingMinute);
        }

        private void TimeChanged() => OnTimeChanged?.Invoke();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AfterSceneLoad()
        {
            if (instance == null)
                CreateInstance();
        }
    }
}
