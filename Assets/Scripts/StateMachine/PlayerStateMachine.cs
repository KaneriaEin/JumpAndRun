using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    public Camera cam;
    public CinemachineFreeLook freeCam;
    public CinemachineVirtualCamera virtualCam;
    public GameObject virtualCam_AllPos;
    public Dictionary<string, GameObject> virtualCam_pos;

    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;

    int isWalkingHash;
    int isRunningHash;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    bool isMovementPressed;
    bool isWalkPressed;
    float rotaionFactorPerFrame = 15.0f;
    float runMultiplier = 10.0f;
    float Speed = 0;

    float gravity = -9.8f;
    float groundedGravity = -.05f;

    bool isJumpPressed = false;
    float initialJumpVelcocity;
    public float maxJumpHeight = 4.0f;
    float maxJumpTime = 0.3f;
    bool isJumping = false;
    int isJumpingHash;
    int jumpCountHash;
    bool requireNewJumpPress = false;
    int jumpCount = 0;
    Dictionary<int, float> initialJumpVelocities = new Dictionary<int, float>();
    Dictionary<int, float> jumpGravities = new Dictionary<int, float>();
    Coroutine currentJumpResetRoutine = null;

    //state machine
    PlayerBaseState _currentState;
    PlayerStateFactory _states;
    bool _isDuringAnim = false;
    SkillFrameController _skillctl;

    //setters and getters
    public CharacterController PlayerCharacterController { get { return characterController; } }
    public PlayerBaseState CurrentState{ get { return _currentState; } set { _currentState = value; } }
    public Animator Animator { get { return animator; } }
    public Coroutine CurrentJumpResetRoutine { get { return currentJumpResetRoutine; } set { currentJumpResetRoutine = value; } }
    public Dictionary<int, float> InitialJumpVelocities {  get { return initialJumpVelocities; } }
    public Dictionary<int, float> JumpGravities { get { return jumpGravities; } }
    public int JumpCount {  get { return jumpCount; } set { jumpCount = value; } }
    public int IsJumpingHash {  get { return isJumpingHash; } }
    public int JumpCountHash { get { return jumpCountHash; } }
    public int IsWalkingHash { get { return isWalkingHash; } }
    public int IsRunningHash { get { return isRunningHash; } }
    public bool RequireNewJumpPress { get { return requireNewJumpPress; } set { requireNewJumpPress = value; } }
    public bool IsJumping { set { isJumping = value; } }
    public bool IsJumpPressed { get { return isJumpPressed; } }
    public bool IsMovementPressed { get { return isMovementPressed; } }
    public bool IsWalkPressed { get { return isWalkPressed; } }
    public bool IsDuringAnim { get { return _isDuringAnim; } set { _isDuringAnim = value; } }
    public float PlayerSpeed { get { return Speed; } set { Speed = value; } }
    public float GroundedGravity { get { return groundedGravity; } }
    public float CurrentMovementY { get { return currentMovement.y; } set { currentMovement.y = value; } }
    public float CurrentMovementX { get { return currentMovement.x; } set { currentMovement.x = value; } }
    public float CurrentMovementZ { get { return currentMovement.z; } set { currentMovement.z = value; } }
    public float AppliedMovementY { get { return currentRunMovement.y; } set { currentRunMovement.y = value; } }
    public float AppliedMovementX { get { return currentRunMovement.x; } set { currentRunMovement.x = value; } }
    public float AppliedMovementZ { get { return currentRunMovement.z; } set { currentRunMovement.z = value; } }
    public float RunMultiplier { get { return runMultiplier; } }
    public float RotaionFactorPerFrame { get { return rotaionFactorPerFrame; } }
    public Vector2 CurrentMovementInput { get { return currentMovementInput; } }
    public Vector3 CurrentMovement {  get { return currentMovement; } }
    public Vector3 AppliedMovement {  get { return currentRunMovement; } }
    public Camera PlayerCamera { get { return cam; } }
    public SkillFrameController SkillCtl { get { return _skillctl; } }

    private void Awake()
    {
        // 给管理器赋值
        PlayerController.Instance.playerStateMachine = this;
        PlayerController.Instance.playerAudioSource = GetComponentInChildren<AudioSource>();

        // 设置引用变量
        cam = Camera.main;

        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        GetVirtualCameraPosition();

        //setup state
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();

        //保存 参数 hash值
        isWalkingHash = Animator.StringToHash("IsWalking");
        isRunningHash = Animator.StringToHash("IsRunning");
        //isJumpingHash = Animator.StringToHash("IsJumping");
        jumpCountHash = Animator.StringToHash("jumpCount");

        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Walk.started += onWalk;
        playerInput.CharacterControls.Walk.canceled += onWalk;
        playerInput.CharacterControls.Jump.started += onJump;
        playerInput.CharacterControls.Jump.canceled += onJump;

        setupJumpVariables();

        _skillctl = GetComponent<SkillFrameController>();
    }

    private void GetVirtualCameraPosition()
    {
        virtualCam_pos = new Dictionary<string, GameObject>();
        for (int i = 0; i < virtualCam_AllPos.transform.childCount; i++)
        {
            virtualCam_pos[virtualCam_AllPos.transform.GetChild(i).name] = virtualCam_AllPos.transform.GetChild(i).gameObject;
        }
    }

    void setupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;

        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelcocity = (2 * maxJumpHeight) / timeToApex;
        float secondJumpGravity = (-2 * (maxJumpHeight + 0)) / Mathf.Pow((timeToApex * 1f), 2);
        float secondJumpInitialVelocity = (2 * (maxJumpHeight + 0)) / (timeToApex * 1f);
        float thirdJumpGravity = (-2 * (maxJumpHeight + 0)) / Mathf.Pow((timeToApex * 1f), 2);
        float thirdJumpInitialVelocity = (2 * (maxJumpHeight + 0)) / (timeToApex * 1f);

        initialJumpVelocities.Add(1, initialJumpVelcocity);
        initialJumpVelocities.Add(2, secondJumpInitialVelocity);
        initialJumpVelocities.Add(3, thirdJumpInitialVelocity);

        jumpGravities.Add(0, gravity);
        jumpGravities.Add(1, gravity);
        jumpGravities.Add(2, secondJumpGravity);
        jumpGravities.Add(3, thirdJumpGravity);
    }


    void Start()
    {
        
    }

    void Update()
    {
        //handleRotation();
        if (PlayerController.Instance.canMove == true)
        {
            //handleMove();
        }
        _currentState.UpdateStates();
    }

    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
        requireNewJumpPress = false;
    }

    void onWalk(InputAction.CallbackContext context)
    {
        isWalkPressed = context.ReadValueAsButton();
    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        //Debug.Log(context.ReadValue<Vector2>());

        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();
        //Debug.LogFormat("after forward{0}", forward);
        //Debug.LogFormat("after right{0}", right);

        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        currentRunMovement.x = currentMovementInput.x * runMultiplier;
        currentRunMovement.z = currentMovementInput.y * runMultiplier;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;

        Speed = new Vector2(currentMovementInput.x, currentMovementInput.y).sqrMagnitude;
    }

    void handleMove()
    {
        UnityEngine.Profiling.Profiler.BeginSample("handleMove");
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        //Debug.LogFormat("forward{0} right{1}", forward, right);
        forward.Normalize();
        right.Normalize();

        forward.y = 1f;
        right.y = 1f;

        currentMovement.z = forward.z * currentMovementInput.y + right.z * currentMovementInput.x;
        currentMovement.x = forward.x * currentMovementInput.y + right.x * currentMovementInput.x;
        currentRunMovement.z = (forward.z * currentMovementInput.y + right.z * currentMovementInput.x) * runMultiplier;
        currentRunMovement.x = (forward.x * currentMovementInput.y + right.x * currentMovementInput.x) * runMultiplier;

        if (isWalkPressed)
        {
            characterController.Move(currentMovement * Time.deltaTime);
        }
        else
        {
            characterController.Move(currentRunMovement * Time.deltaTime);
        }
        UnityEngine.Profiling.Profiler.EndSample();
    }

    void handleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;

        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotaionFactorPerFrame * Time.deltaTime);
        }
    }

    private void SetIsDuringAnim(int flag)
    {
        _isDuringAnim = flag == 1 ? true : false;
    }

    private void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
}
