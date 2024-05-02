using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoGimmick : MonoBehaviour
{
    [SerializeField]
    private BombDirection _bombDirection = BombDirection.ThreeDirections;
    [SerializeField]
    private GameObject _volcanicBomb = default;
    [SerializeField]
    private Transform _bombMuzzle = default;
    [Min(1)]
    [Tooltip("一度に撃ちだす数")]
    [SerializeField]
    private int _bombCount = 1;
    [Tooltip("火山弾を撃ちだす力")]
    [SerializeField]
    private int _bombPower = 5;
    [Tooltip("火山弾の攻撃力")]
    [SerializeField]
    private int _volcanicAttackValue = 1;
    [SerializeField]
    private float _bombInterval = 1f;

    [Header("For Gimmick")]
    [SerializeField]
    private float _searchRadius = 1f;

    private float _bombTimer = 0f;
    private bool _isPlayingEffect = false;
    private IEnumerator _bombCoroutine = default;

    /// <summary> 火山弾を撃ちだすときの、一発毎の生成間隔 </summary>
    private readonly float _loopBombInterval = 0.3f;
    private readonly Dictionary<BombDirection, Vector2Int[]> _bombDirectionDict
        = new()
        {
            { BombDirection.ThreeDirections, new Vector2Int[]{ new(-1, 1), Vector2Int.up, new(1, 1) } },
            { BombDirection.Upper, new Vector2Int[]{ Vector2Int.up } },
            { BombDirection.VShaped, new Vector2Int[]{ new(-1, 1), new(1, 1) } }
        };

    protected bool Explosive
        => (GameManager.Instance.PlayerTransform.position - transform.position).sqrMagnitude <= _searchRadius * _searchRadius;

    private void Update()
    {
        if ((!_isPlayingEffect && Explosive) || (_isPlayingEffect && _bombTimer >= _bombInterval))
        {
            _isPlayingEffect = true;
            _bombTimer = 0f;
            _bombCoroutine = Bomb();
        }
        else if (_bombCoroutine != null && !_bombCoroutine.MoveNext()) { _bombCoroutine = null; }
        else if (_bombCoroutine == null) { _bombTimer += Time.deltaTime; }
    }

    private IEnumerator Bomb()
    {
        var directionArray = _bombDirectionDict[_bombDirection];

        for (int i = 0; i < _bombCount; i++)
        {
            AudioManager.Instance.PlaySE(SEType.Eruption);
            var bomb = GameManager.Instance.ObjectPool.SpawnObject(_volcanicBomb);
            bomb.transform.position = _bombMuzzle.position;
            var rb = bomb.GetComponent<Rigidbody2D>();
            if (directionArray.Length > 1)
            {
                var randomIndex = Random.Range(0, directionArray.Length);
                rb.AddForce(directionArray[randomIndex] * _bombPower, ForceMode2D.Impulse);
            }
            else { rb.AddForce(directionArray[0] * _bombPower, ForceMode2D.Impulse); }

            bomb.GetComponent<VolcanicBomb>().Initialize(_volcanicAttackValue);
            for (float timer = 0f; timer <= _loopBombInterval; timer += Time.deltaTime) { yield return null; }
        }
        yield return null;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _searchRadius);
    }
#endif
}

public enum BombDirection
{
    ThreeDirections,
    Upper,
    VShaped
}
