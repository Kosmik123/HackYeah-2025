using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class GuardDetection : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Guard _guard;
    private bool _detect = false;
    private bool _prison;
    private bool _bed;
    private bool _reachedCell;
    private SplineAnimate _splineAnimate;

    void Start()
    {
        _splineAnimate = transform.gameObject.GetComponent<SplineAnimate>();
        _detect = true;
        _guard.OnPathEnded += OnPathEnded;
    }

    private void OnPathEnded(Guard obj)
    {
        if (_reachedCell)
        {
            StartLookingForPlayer();
        }
        _reachedCell = !_reachedCell;
    }

    // Update is called once per frame
    [ContextMenu("Test")]
    public void StartLookingForPlayer()
    {
        _detect = true;
        StartCoroutine(Detect());
    }

    public void StopLookingForPlayer()
    {
        Debug.Log("Guard is Leaving...");
        _detect = false;
        StopCoroutine(Detect());
    }

    private IEnumerator Detect()
    {
        var elapsed = 0f;
        Debug.Log("Guard is Detecting...");
        while (_detect)
        {
            _prison = GameManager.Instance.CheckPrisonZone();
            _bed = GameManager.Instance.CheckBedZone();
            if (_prison || _bed)
            {
                LooseConditionMet();
                _detect = false;
            }

            yield return new WaitForSeconds(1f);
        }

        yield return null;
    }

    private void LooseConditionMet()
    {
        if (_prison)
        {
            _player.GetComponent<PlayerCaught>().PlayerCaughtInVent();
        }
        else if (_bed)
        {
            Debug.Log("Guard noticed the hole");
        }
        else
        {
            Debug.LogError("Unhandled loose condition");
        }
    }
}