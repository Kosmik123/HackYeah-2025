using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject bedZone;
    [SerializeField] private GameObject bed;
    [SerializeField] private int requiredCornersWithinBounds = 6;
    public static GameManager Instance { get; private set; }
    private BoxCollider bedZoneCollider;
    private BoxCollider bedCollider;

    private void Awake()
    {
        Instance = this;
        bedZoneCollider = bedZone.GetComponent<BoxCollider>();
        bedCollider = bed.GetComponent<BoxCollider>();
    }

    public bool CheckBedZone()
    {
        if (CheckHowMuchInBounds() < requiredCornersWithinBounds) return false;
        return true;
    }

    private int CheckHowMuchInBounds()
    {
        int cornersInside = 8;
        Vector3[] corners = GetWorldCorners(bedCollider);

        foreach (var corner in corners)
        {
            Vector3 localPoint = bedZoneCollider.transform.InverseTransformPoint(corner);
            localPoint -= bedZoneCollider.center;
            Vector3 halfSize = bedZoneCollider.size * 0.5f;
            if (Mathf.Abs(localPoint.x) > halfSize.x ||
                Mathf.Abs(localPoint.y) > halfSize.y ||
                Mathf.Abs(localPoint.z) > halfSize.z)
            {
                cornersInside--;
            }
        }

        return cornersInside;
    }

    private static Vector3[] GetWorldCorners(BoxCollider box)
    {
        Vector3[] corners = new Vector3[8];
        Vector3 halfSize = box.size * 0.5f;

        int i = 0;
        for (int x = -1; x <= 1; x += 2)
        {
            for (int y = -1; y <= 1; y += 2)
            {
                for (int z = -1; z <= 1; z += 2)
                {
                    Vector3 localCorner = box.center + Vector3.Scale(halfSize, new Vector3(x, y, z));
                    corners[i++] = box.transform.TransformPoint(localCorner);
                }
            }
        }

        return corners;
    }

    [ContextMenu("Test if bed is within zone")]
    private void TestBedWithinZone()
    {
        bedZoneCollider = bedZone.GetComponent<BoxCollider>();
        bedCollider = bed.GetComponent<BoxCollider>();
        Debug.Log(CheckHowMuchInBounds());
        Debug.Log(CheckHowMuchInBounds() < requiredCornersWithinBounds ? "Bed not within zone" : "Bed within zone");
    }
}