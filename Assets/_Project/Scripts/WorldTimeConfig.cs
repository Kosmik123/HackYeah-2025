using UnityEngine;

namespace HackYeah2025
{
    public class WorldTimeConfig : ScriptableObject
    {
        public const float MinutesInHour = 60f;
        public const float HoursInDay = 24f;

        [SerializeField]
        private float timeScale = 1f;
        public float TimeScale => timeScale;

        [SerializeField]
        private float startingMinutes = 1f;
        public float StartingMinutes => startingMinutes; 
    }

    [AddComponentMenu("")]
    public class WorldTime : MonoBehaviour
    {
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

        private WorldTimeConfig config;

        [SerializeField]
        private float totalMinutes;

        [Header("States")]
        [SerializeField]
        private float hours;
        [SerializeField]
        private float minutes;

        public static float Minutes => Instance.totalMinutes % WorldTimeConfig.MinutesInHour;
        public static float Hours => Instance.totalMinutes / WorldTimeConfig.MinutesInHour % WorldTimeConfig.HoursInDay;
        public static float Days => Instance.totalMinutes / (WorldTimeConfig.MinutesInHour * WorldTimeConfig.HoursInDay);
        public static int WholeMinutes => Mathf.FloorToInt(Minutes);
        public static int WholeHours => Mathf.FloorToInt(Hours);
        public static int WholeDays => Mathf.FloorToInt(Days);

        public static void Initialize()
        {
            var obj = new GameObject("World Time");
            DontDestroyOnLoad(obj);
            obj.AddComponent<WorldTime>();
        }

        private void Update()
        {
            totalMinutes += Time.deltaTime * config.TimeScale;
            hours = Hours;
            minutes = Minutes;
        }

        private static void CreateInstance()
        {
            var go = new GameObject("World Time");
            instance = go.AddComponent<WorldTime>();
            instance.config = Resources.Load<WorldTimeConfig>("World Time");
            instance.totalMinutes = instance.config.StartingMinutes;
        }
    }
}
