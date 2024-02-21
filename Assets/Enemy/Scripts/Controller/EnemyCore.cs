using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
/// <summary> BossEnemyのコアを担うクラス </summary>
public class EnemyCore : MonoBehaviour
{
    private GameObject _enemy = default;

    private void Start()
    {
        _enemy = transform.root.gameObject;

        var collider = GetComponent<CircleCollider2D>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IBulletData _))
        {
            Debug.Log("boss撃破");
            Destroy(_enemy);
        }
    }
}
