using FMODUnity;
using UnityEngine;

public class CrankAnim : MonoBehaviour
{
    private static readonly int Rotate = Animator.StringToHash("rotate");

    public enum Axis { X, Y, Z }

    private Animator anim;
    private StudioEventEmitter _emitter;
    
    [System.Serializable]
    public struct TargetScalePair
    {
        public Transform target;
        public float scale;
        public Axis axis;
    }
    
    public float rotation = 0.0f;
    public TargetScalePair[] targets;

    private bool isPlaying = false;
    
    void Awake()
    {
        anim = GetComponent<Animator>();
        _emitter = GetComponent<StudioEventEmitter>();
    }
    
    void Update()
    {
        foreach (var pair in targets)
        {
            if (pair.target != null)
            {
                Vector3 euler = pair.target.localEulerAngles;
                float rot = rotation * pair.scale;
                switch (pair.axis)
                {
                    case Axis.X: euler.x = rot; break;
                    case Axis.Y: euler.y = rot; break;
                    case Axis.Z: euler.z = rot; break;
                }
                pair.target.localEulerAngles = euler;
            }
        }
    }

    public void MoveCrank()
    {
        anim.SetTrigger(Rotate);
        if (isPlaying == false)
        {
            _emitter.Play();
            isPlaying = true;
        }
    }

    public void StopCrank()
    {
        isPlaying = false;
        _emitter.Stop();
    }

    public bool GetIsPlaying()
    {
        return isPlaying;
    }
}