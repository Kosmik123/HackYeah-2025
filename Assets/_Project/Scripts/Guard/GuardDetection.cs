using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Splines;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class GuardDetection : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Guard _guard;
    [SerializeField] private GameObject crank;
    [SerializeField] private int maxFailAmount=3;
    [SerializeField] private EventReference hearthbeating;
    private bool _detect = false;
    private bool _prison;
    private bool _bed;
    private bool _crank;
    private int _failQuotaCounter;
    private bool _reachedCell;
    private SplineAnimate _splineAnimate;
    private EventInstance _hearthbeatingInstance;

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
        _hearthbeatingInstance = RuntimeManager.CreateInstance(hearthbeating);
        _hearthbeatingInstance.start();
        _detect = true;
        StartCoroutine(Detect());
    }

    public void StopLookingForPlayer()
    {
        _hearthbeatingInstance.stop(STOP_MODE.ALLOWFADEOUT);
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
            EndGameInCell(false);
        }
        else if (_crank)
        {
            Debug.Log("Failed at cranking too many times!");
            EndGameInCell(true);
        }
    }

    private void EndGameInCell(bool crankEnd)
    {
        _player.GetComponent<EndInCell>().EndGameInCell(crankEnd);
    }
}