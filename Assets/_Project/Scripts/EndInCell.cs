using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

public class EndInCell : MonoBehaviour
{

    [SerializeField] private GameObject warden;
    [SerializeField] private float splineDuration = 3f;
    [SerializeField] private SplineContainer guardSpline;
    [SerializeField] private GameObject playerAwaitPosition;
    [SerializeField] private Image FadeOutImage;

    private void Start()
    {
        FadeOutImage.gameObject.SetActive(true);
        Color color = FadeOutImage.color;
        color.a = 0f;
        FadeOutImage.color = color;
        FadeOutImage.gameObject.SetActive(false);
    }
    
    public void EndGameInCell(bool crankEnd)
    {
        var guardAnimate = warden.GetComponent<SplineAnimate>();
        GetComponent<PlayerController>().enabled = false;
        guardAnimate.Container = guardSpline;
        guardAnimate.Duration = splineDuration;
        guardAnimate.Restart(false);
        transform.position = playerAwaitPosition.transform.position;
        transform.rotation = playerAwaitPosition.transform.rotation;
        guardAnimate.Play();
        StartCoroutine(FadeScreen(crankEnd));
    }

    private IEnumerator FadeScreen(bool crankEnd)
    {
        FadeOutImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(splineDuration/ 3);
        float elapsed = 0f;
        var fadeDuration = (splineDuration * 2) / 3;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            Color color = FadeOutImage.color;
            color.a = alpha;
            FadeOutImage.color = color;
            yield return null;
        }

        if (crankEnd)
        {
            yield return FindAnyObjectByType<DialogueSystem>().ShowDialogue("not_spinning");
        }
        else
        {
            yield return FindAnyObjectByType<DialogueSystem>().ShowDialogue("hole_visible");
        }
        //GameManager.Instance.EndGame();
    }
}
