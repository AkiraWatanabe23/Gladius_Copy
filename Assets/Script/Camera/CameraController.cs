using Constants;
using UnityEngine;

public class CameraController : MonoBehaviour, IPause
{
    [SerializeField]
    private float _cameraMoveSpeed = 1f;
    [SerializeField]
    private float _deadTime = 1f;
    [SerializeField]
    private Transform _leftDeadPoint = default;
    [SerializeField]
    private Transform _upperDeadPoint = default;
    [SerializeField]
    private Transform _lowerDeadPoint = default;

    private bool _isPause = false;
    private Transform _targetTransform = default;
    private float _deadTimer = 0f;
    private Transform _transform = default;

    protected bool IsEnterDeadZone
    {
        get
        {
            var positionFlag =
                _targetTransform.position.x <= _leftDeadPoint.position.x ||
                _targetTransform.position.y >= _upperDeadPoint.position.y ||
                _targetTransform.position.y <= _lowerDeadPoint.position.y;

            return positionFlag;
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
                if (IsEnterDeadZone) { Consts.Log("GameOver"); }
            }
        }
    }

    public void Initialize(GameObject target)
    {
        _transform = transform;
        _targetTransform = target.transform;
    }

    public void OnUpdate(float deltaTime)
    {
        if (_isPause) { return; }
        CameraMovement(deltaTime);

        if (!IsEnterDeadZone) { return; }
        DeadTimer += deltaTime;
    }

    private void CameraMovement(float deltaTime)
    {
        _transform.Translate(Vector2.right * _cameraMoveSpeed * deltaTime);
    }

    public void Pause() => _isPause = true;

    public void Resume() => _isPause = false;
}
