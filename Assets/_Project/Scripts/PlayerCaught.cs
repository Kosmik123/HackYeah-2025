using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

public class PlayerCaught : MonoBehaviour
{
    [SerializeField] private GameObject warden;
    [SerializeField] private GameObject wardenWaitingPosition;
    [SerializeField] private float playerPullingTime = 5f;
    [SerializeField] private float distanceToStop = 1f;
    [SerializeField] private SplineContainer ventSpline;
    [SerializeField] private Image FadeOutImage;

    private void Start()
    {
        FadeOutImage.gameObject.SetActive(true);
        Color color = FadeOutImage.color;
        color.a = 0f;
        FadeOutImage.color = color;
        FadeOutImage.gameObject.SetActive(false);
    }

    [ContextMenu("Test")]
    public void PlayerCaughtInVent()
    {
        GetComponent<PlayerController>().enabled = false;
        warden.transform.position = wardenWaitingPosition.transform.position;
        var splineAnimate = GetComponent<SplineAnimate>();
        splineAnimate.Container = ventSpline;
        splineAnimate.Duration = playerPullingTime;
        float3 nearest;
        float t;
        SplineUtility.GetNearestPoint(ventSpline.Spline, transform.position, out nearest, out t);

        // Move the animated object to the nearest point
        splineAnimate.NormalizedTime = t;
        splineAnimate.Restart(false); // false = don’t reset time to 0

        // Snap the GameObject to the spline
        transform.position = nearest;

        // Optional: align to spline tangent
        //float3 tangent = SplineUtility.EvaluateTangent(ventSpline, t);
        //transform.rotation = Quaternion.LookRotation(tangent);

        // Start animating from this point
        splineAnimate.Play();
        StartCoroutine(FadeScreen());
    }

    private IEnumerator FadeScreen()
    {
        FadeOutImage.gameObject.SetActive(true);
        yield return new WaitForSeconds((playerPullingTime * 2 )/ 3);
        float elapsed = 0f;
        var fadeDuration = (playerPullingTime+2) / 3;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            Color color = FadeOutImage.color;
            color.a = alpha;
            FadeOutImage.color = color;
            yield return null;
        }
        //GameManager.Instance.EndGame();
    }
}
