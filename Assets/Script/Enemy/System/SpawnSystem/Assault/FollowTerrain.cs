using UnityEngine;

/// <summary> 地形に沿う </summary>
public class FollowTerrain : IEnemyGeneration
{
    private readonly RaycastHit2D[] _floorHitResult = new RaycastHit2D[10];
    private readonly RaycastHit2D[] _wallHitResult = new RaycastHit2D[10];
    private readonly RaycastHit2D[] _rotateHitResult = new RaycastHit2D[10];

    public void Movement(EnemyController enemy)
    {
        if (enemy.EnemySystem is not Assault) { return; }

        var assault = (Assault)enemy.EnemySystem;
        if (assault.IsRotate) { Rotate(assault); return; }

        if (GroundSearch(assault)) { assault.Rb2d.velocity = assault.MoveDirection * enemy.MoveSpeed; }
    }

    private bool GroundSearch(Assault assault)
    {
        var floorHitCount =
            Physics2D.RaycastNonAlloc(assault.Transform.position + (Vector3.up * 0.8f), -assault.Transform.up, _floorHitResult, 1f);
        var wallHitCount =
            Physics2D.RaycastNonAlloc(assault.Transform.position + Vector3.up, -assault.Transform.right, _wallHitResult, 1f);

        Debug.DrawRay(assault.Transform.position + (Vector3.up * 0.8f), -assault.Transform.up, Color.red, 10f);
        Debug.DrawRay(assault.Transform.position + Vector3.up, -assault.Transform.right, Color.white, 10f);

        if (floorHitCount > 1)
        {
            var isFloorHit = false;
            Debug.Log(floorHitCount);
            for (int i = 0; i < floorHitCount; i++)
            {
                if (!_floorHitResult[i].collider.gameObject.TryGetComponent(out Ground _)) { continue; }

                Debug.Log(_floorHitResult[i].collider.gameObject.name);
                isFloorHit = true;
                var hitTransform = _floorHitResult[i].collider.transform.right;
                hitTransform.x *= -1;
                assault.MoveDirection = hitTransform;
            }
            return isFloorHit;
        }
        else if (wallHitCount > 1)
        {
            var isWallHit = false;
            Debug.Log(wallHitCount);
            for (int i = 0; i < wallHitCount; i++)
            {
                if (!_wallHitResult[i].collider.gameObject.TryGetComponent(out Ground _)) { continue; }

                Debug.Log(_wallHitResult[i].collider.gameObject.name);
                isWallHit = true;
                var hitTransform = _wallHitResult[i].collider.transform.right;
                hitTransform.x *= -1;
                assault.MoveDirection = hitTransform;
            }
            return isWallHit;
        }
        else
        {
            assault.IsRotate = true;

            //回転のみを行うために移動を一旦停止する
            assault.MoveDirection = Vector2.zero;
            assault.Rb2d.velocity = Vector2.zero;
            return false;
        }
    }

    private void Rotate(Assault assault)
    {
        assault.Transform.Rotate(0f, 0f, Time.deltaTime * 90f);

        var rotateHitCount =
            Physics2D.RaycastNonAlloc(assault.Transform.position + (Vector3.up * 0.8f), -assault.Transform.up, _rotateHitResult, 1f);
        Debug.DrawRay(assault.Transform.position + (Vector3.up * 0.8f), -assault.Transform.up, Color.green, 10f);

        if (rotateHitCount > 1)
        {
            Debug.Log(rotateHitCount);
            for (int i = 0; i < rotateHitCount; i++)
            {
                if (!_rotateHitResult[i].collider.gameObject.TryGetComponent(out Ground _)) { continue; }

                Debug.Log(_rotateHitResult[i].collider.gameObject.name);
                assault.IsRotate = false;
                break;
            }
        }
        else { Debug.Log("hit only self"); }
    }
}
