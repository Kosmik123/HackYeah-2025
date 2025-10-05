using UnityEngine;

public class HeadController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float moveSpeed = 1;

    [Header("States")]
    [SerializeField]
    private float currentHeight;
    private float targetHeight;

    private void OnEnable()
    {
        targetHeight = currentHeight = transform.localPosition.y;
    }

    public void SetHeight(float height)
    {
        targetHeight = height;
    }

    void Update()
    {
        currentHeight = Mathf.MoveTowards(currentHeight, targetHeight, Time.deltaTime * moveSpeed);
        var position = transform.localPosition;
        position.y = currentHeight;
        transform.localPosition = position;
    }
}
