using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

public class WinEnd : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private float cutsceneLength = 10f;
    [SerializeField] private float fadeoutDuration = 4f;
    [SerializeField] private Image fadeoutImage;
    private CinemachineCamera _cinemachineCamera;
    private SplineAnimate _animate;

    private void Start()
    {
        _cinemachineCamera = GetComponentInChildren<CinemachineCamera>();
        _animate = _cinemachineCamera.gameObject.GetComponent<SplineAnimate>();
    }
    [ContextMenu("Test")]
    public void StartExit()
    {
        //GameManager.DisableLoseConditions(true);
        StartCoroutine(FinalCutscene());
    }

    private IEnumerator FinalCutscene()
    {
        _cinemachineCamera.gameObject.GetComponent<CinemachineInputAxisController>().enabled = false;
        _cinemachineCamera.gameObject.GetComponent<CinemachinePanTilt>().enabled = false;
        _cinemachineCamera.gameObject.GetComponentInParent<PlayerController>().enabled = false;
        _animate.Container = splineContainer;
        _animate.Duration = cutsceneLength;
        _animate.Play();
        yield return new WaitForSeconds(cutsceneLength);
        fadeoutImage.gameObject.SetActive(true);
        float elapsed = 0f;
        while (elapsed < fadeoutDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeoutDuration);
            Color color = fadeoutImage.color;
            color.a = alpha;
            fadeoutImage.color = color;
            yield return null;
        }
        //GameManager.Instance.EndGame();
    }
}
