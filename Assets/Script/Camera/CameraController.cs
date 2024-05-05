using Constants;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float _deadTime = 1f;

    private GameObject _target = default;
    private bool _isEnterDeadZone = false;
    private float _deadTimer = 0f;

    protected bool IsEnterDeadZone
    {
        get => _isEnterDeadZone;
        private set
        {
            _isEnterDeadZone = value;
            if (!_isEnterDeadZone) { _deadTimer = 0f; }
        }
    }
    protected float DeadTimer
    {
        get => _deadTimer;
        private set
        {
            _deadTimer = value;
            if (_deadTimer >= _deadTime)
            {
                if (_isEnterDeadZone) { Consts.Log("GameOver"); }
            }
        }
    }

    public void Initialize(GameObject target)
    {
        _target = target;
        IsEnterDeadZone = false;
    }

    public void OnUpdate(float deltaTime)
    {
        if (!IsEnterDeadZone) { return; }

        DeadTimer += deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isEnterDeadZone) { return; }

        if (collision.gameObject == _target)
        {
            _isEnterDeadZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == _target)
        {
            IsEnterDeadZone = false;
        }
    }
}
