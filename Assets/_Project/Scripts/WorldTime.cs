using UnityEngine;

namespace HackYeah2025
{
    public class WorldTime : ScriptableObject
    {
        public static WorldTime Instance { get; private set; }

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

        public static float Minutes => Instance.totalMinutes % MinutesInHour;
        public static float Hours => Instance.totalMinutes / MinutesInHour % HoursInDay;
        public static float Days => Instance.totalMinutes / (MinutesInHour * HoursInDay);
                
        public static int WholeMinutes => Mathf.FloorToInt(Minutes);
        public static int WholeHours => Mathf.FloorToInt(Hours);
        public static int WholeDays => Mathf.FloorToInt(Days);

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (Instance == null)
            {
                Instance = Resources.Load<WorldTime>("World Time");
                Instance.hideFlags = HideFlags.HideAndDontSave;
            }
        }

        private class WorldTimeUpdater : MonoBehaviour
        {
            private static WorldTimeUpdater instance;
            public static WorldTimeUpdater Instance
            {
                get
                {
                    if (instance == null)
                    {
                        var go = new GameObject("World Time Updater");
                        instance = go.AddComponent<WorldTimeUpdater>();
                        DontDestroyOnLoad(go);
                    }
                    return instance;
                }
            }

            private void Update()
            {

            }
        }
    }
}
