using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class GuardDetection : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Guard _guard;
    [SerializeField] private GameObject crank;
    [SerializeField] private int maxFailAmount=3;
    private bool _detect = false;
    private bool _prison;
    private bool _bed;
    private bool _crank;
    private int _failQuotaCounter;
    private bool _reachedCell;
    private SplineAnimate _splineAnimate;

    void Start()
    {
        _splineAnimate = transform.gameObject.GetComponent<SplineAnimate>();
        _detect = true;
        //_guard.OnPathEnded += OnPathEnded;
        _reachedCell = true;
    }

    /*private void OnPathEnded(Guard obj)
    {
        if (_reachedCell)
        {
            StartLookingForPlayer();
        }
        _reachedCell = true;
    }*/

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
            _crank = crank.GetComponent<CrankAnim>().GetIsPlaying();
            if (!_crank)
            {
                WarnPlayer();
                _failQuotaCounter++;
            }
            if (_prison || _bed)
            {
                LooseConditionMet();
                _detect = false;
            }

            yield return new WaitForSeconds(1f);
        }

        yield return null;
    }

    private void WarnPlayer()
    {
        if (_failQuotaCounter >= maxFailAmount)
        {
            _crank = true;
            LooseConditionMet();
            _detect = false;
            StopCoroutine(Detect());
        }
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
            EndGameInCell();
        }
        else if (_crank)
        {
            Debug.Log("Failed at cranking too many times!");
            EndGameInCell();
        }
    }

    private void EndGameInCell()
    {
        _player.GetComponent<EndInCell>().EndGameInCell();
    }
}