using UnityEngine;

public class Boss : IEnemy
{
    [SerializeField]
    private float _attackInterval = 1f;

    public float AttackInterval => _attackInterval;

    public bool IsMeasuring { get; set; }
    public Transform PlayerTransform { get; set; }

    public EnemyController EnemyController { get; set; }
    public GameObject Enemy { get; set; }
    public Transform Transform { get; set; }

    public void Init() { }

    public void Init(Transform playerTransform, GameObject corePrefab)
    {
        PlayerTransform = playerTransform;

        //Bossの場合、Coreが設定されているか調べる
        EnemyCore enemyCore = null;
        for (int i = 0; i < Transform.childCount; i++)
        {
            if (Transform.GetChild(i).gameObject.TryGetComponent(out enemyCore)) { return; }
        }
        if (enemyCore == null) //EnemyCoreの設定がなかった場合は生成、設定する
        {
            var core = Object.Instantiate(corePrefab);
            core.transform.parent = Transform;
            core.transform.localPosition = Vector2.zero;
        }
    }
}
