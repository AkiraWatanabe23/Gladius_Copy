using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    [SerializeField]
    private SpawnSearchDirection _colliderDirection = SpawnSearchDirection.Left;

    public SpawnSearchDirection ColliderDirection => _colliderDirection;
}
