using UnityEngine;

public class AnimatorEventReciever : MonoBehaviour
{

    private PlayerController _playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerController = GetComponentInParent<PlayerController>();
        if(!_playerController) Debug.LogError($"Playercontroller not found for {this}");
    }

    public void DigAnimationEnded()
    {
        _playerController.DiggingAnimationEnded();
    }
}
