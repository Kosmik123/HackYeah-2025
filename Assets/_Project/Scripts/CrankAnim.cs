using UnityEngine;

[ExecuteAlways]
public class CrankAnim : MonoBehaviour
{
    public enum Axis { X, Y, Z }

    [System.Serializable]
    public struct TargetScalePair
    {
        public Transform target;
        public float scale;
        public Axis axis;
    }
    
    public float rotation = 0.0f;
    public TargetScalePair[] targets;

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
}