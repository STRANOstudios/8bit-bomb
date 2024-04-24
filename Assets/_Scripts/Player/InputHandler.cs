using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name Rederences")]
    [SerializeField] private string actionMapNamePlayer = "Player";
    [SerializeField] private string actionMapNameSystem = "System";

    [Header("Action Name Refernces")]
    [SerializeField] private string move = "move";
    [SerializeField] private string bomb = "bomb";
    [SerializeField] private string escape = "escape";

    private InputAction moveAction;
    private InputAction bombAction;
    private InputAction escapeAction;

    public Vector2 MoveInput { get; private set; }
    public bool BombInput { get; private set; }
    public bool EscapeInput { get; private set; }

    public static InputHandler Instance { get; private set; }

    private void Awake()
    {
        #region Singleton

        //if (Instance && Instance != this)
        //{
        //    Destroy(this);
        //    return;
        //}
        Instance = this;
        //DontDestroyOnLoad(transform.root.gameObject);

        #endregion

        moveAction = playerControls.FindActionMap(actionMapNamePlayer).FindAction(move);
        bombAction = playerControls.FindActionMap(actionMapNamePlayer).FindAction(bomb);

        escapeAction = playerControls.FindActionMap(actionMapNameSystem).FindAction(escape);

        RegisterInputActions();
    }

    void RegisterInputActions()
    {
        moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => MoveInput = Vector2.zero;

        bombAction.performed += context => BombInput = true;
        bombAction.canceled += context => BombInput = false;

        escapeAction.performed += context => EscapeInput = true;
        escapeAction.canceled += context => EscapeInput = false;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        bombAction.Enable();

        escapeAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        bombAction.Disable();

        escapeAction.Disable();
    }
}
