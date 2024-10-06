using System;
using UnityEngine;
using static GameActions;

public class InputManager : MonoBehaviour, IDefaultActions 
{
    //public event Action<Vector2> OnMoveInput;
    public Vector2 InputDirection {  get; private set; }
    public static InputManager Current { get; private set; }

    private GameActions _myActions;

    public void Awake()
    {
        Current = this;
        _myActions = new GameActions();
        _myActions.Default.Enable();
        _myActions.Default.AddCallbacks(this);
    }

    public void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        InputDirection = context.ReadValue<Vector2>();
        //OnMoveInput?.Invoke(context.ReadValue<Vector2>());
    }
}