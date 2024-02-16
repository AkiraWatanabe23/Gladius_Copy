using UnityEngine;

[System.Serializable]
public class PlayerInput
{
    [SerializeField] private KeyCode _up = KeyCode.W;
    [SerializeField] private KeyCode _down = KeyCode.S;
    [SerializeField] private KeyCode _left = KeyCode.A;
    [SerializeField] private KeyCode _right = KeyCode.D;
    [SerializeField] private KeyCode _shoot = KeyCode.Space;

    public int Horizontal
    {
        get
        {
            int horizontal = 0;
            if (Input.GetKey(_left)) horizontal -= 1;
            if (Input.GetKey(_right)) horizontal += 1;
            return horizontal;
        }
    }

    public int Vertical
    {
        get
        {
            int vertical = 0;
            if (Input.GetKey(_up)) vertical += 1;
            if (Input.GetKey(_down)) vertical -= 1;
            return vertical;
        }
    }

    public bool HasShoot => Input.GetKey(_shoot);
}
