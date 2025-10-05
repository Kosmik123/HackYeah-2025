using UnityEngine;

public class AnimatorEventReciever : MonoBehaviour
{

    private PlayerController _playerController;

    private PlayerDiggingComponent _playerDiggingComponent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerController = GetComponentInParent<PlayerController>();
        if(!_playerController) Debug.LogError($"Playercontroller not found for {this}");
        _playerDiggingComponent = GetComponentInParent<PlayerDiggingComponent>();
        if (!_playerController) Debug.LogError($"Player digging component not found for {this}");

    }

    public void DigAnimationEnded()
    {
        _playerController.DiggingAnimationEnded();
        _playerDiggingComponent.AnimationEnded();
    }
}
