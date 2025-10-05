using System.Collections;
using FMODUnity;
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
    [SerializeField] private EventReference jumpScareSound;
    [SerializeField] private EventReference tenseSounds;

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
        int closestSplineIndex = 0;
        float closestDistance = float.MaxValue;
        float3 closestPoint = float3.zero;
        float closestT = 0f;
        GetComponent<PlayerController>().enabled = false;
        
        warden.transform.position = wardenWaitingPosition.transform.position;
        warden.transform.rotation = wardenWaitingPosition.transform.rotation;


        float3 nearest;
        float t;
        for (int i = 0; i < ventSpline.Splines.Count; i++)
        {
            var spline = ventSpline.Splines[i];
            SplineUtility.GetNearestPoint(spline, transform.position, out nearest, out t);
            float dist = math.distance(nearest, transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestSplineIndex = i;
                closestPoint = nearest;
                closestT = t;
            }
        }
        var splineAnimate = GetComponent<SplineAnimate>();
        var furtherIndex = closestSplineIndex == 0 ? 1 : 0;

        ventSpline.RemoveSplineAt(furtherIndex);
        splineAnimate.Container = ventSpline;
        splineAnimate.Duration = playerPullingTime;

        // Move the animated object to the nearest point
        splineAnimate.NormalizedTime = closestT;
        splineAnimate.Restart(false); // false = don�t reset time to 0

        transform.position = closestPoint;

        // Optional: align to spline tangent
        //float3 tangent = SplineUtility.EvaluateTangent(ventSpline, t);
        //transform.rotation = Quaternion.LookRotation(tangent);

        // Start animating from this point
        splineAnimate.Play();
        StartCoroutine(FadeScreen());
    }

    private IEnumerator FadeScreen()
    {
        RuntimeManager.PlayOneShot(tenseSounds);
        FadeOutImage.gameObject.SetActive(true);
        yield return new WaitForSeconds((playerPullingTime * 2 )/ 3);
        float elapsed = 0f;
        var fadeDuration = (playerPullingTime+2) / 3;
        RuntimeManager.PlayOneShot(jumpScareSound);
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
