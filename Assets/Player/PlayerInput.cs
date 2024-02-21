using UnityEngine;

[System.Serializable]
public class PlayerInput
{
    [SerializeField] private KeyCode _up = KeyCode.W;
    [SerializeField] private KeyCode _down = KeyCode.S;
    [SerializeField] private KeyCode _left = KeyCode.A;
    [SerializeField] private KeyCode _right = KeyCode.D;
    [SerializeField] private KeyCode _shoot = KeyCode.Space;
    [SerializeField] private MouseButtonType _mouseButtonType = MouseButtonType.None;

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

    public bool IsShootByKey => Input.GetKey(_shoot);

    public bool IsShootByMouse => _mouseButtonType == MouseButtonType.None ? false : Input.GetMouseButton((int)_mouseButtonType);
}

public enum MouseButtonType
{
    None = -1,
    RightButton = 0,
    LeftButton = 1,
    MiddleButton = 2,
}