using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class AnimationAndMovementController : MonoBehaviour
{
    public Camera cam;

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
    float maxJumpTime = 0.8f;
    bool isJumping = false;
    //int isJumpingHash;
    int jumpCountHash;
    bool isJumpAnimating = false;
    int jumpCount = 0;
    Dictionary<int , float> initialJumpVelocities = new Dictionary<int , float>();
    Dictionary<int , float> jumpGravities = new Dictionary<int , float>();
    Coroutine currentJumpResetRoutine = null;

    private void Awake()
    {
        cam = Camera.main;

        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

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

    IEnumerator jumpResetRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        jumpCount = 0;
        animator.SetInteger(jumpCountHash, jumpCount);
    }

    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed= context.ReadValueAsButton();
    }

    void onWalk(InputAction.CallbackContext context)
    {
        isWalkPressed = context.ReadValueAsButton();
    }

    void handleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;

        Quaternion currentRotation = transform.rotation;

        if(isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotaionFactorPerFrame * Time.deltaTime);
        }

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

    void handleJump()
    {
        if (!isJumping && characterController.isGrounded && isJumpPressed)
        {
            if (jumpCount < 3 && currentJumpResetRoutine != null)
            {
                StopCoroutine(currentJumpResetRoutine);
            }
            //animator.SetBool(isJumpingHash, true);
            isJumpAnimating = true;
            isJumping = true;
            jumpCount += 1;
            animator.SetInteger(jumpCountHash, jumpCount);
            currentMovement.y = initialJumpVelocities[jumpCount] * .5f * PlayerController.Instance.jmpFactor;
            currentRunMovement.y = initialJumpVelocities[jumpCount] * .5f * PlayerController.Instance.jmpFactor;
        }
        else if (!isJumpPressed && isJumping && characterController.isGrounded)
        {
            isJumping = false;
        }
    }

    void handleGravity()
    {
        //Debug.LogFormat("isgrounded: {0}",characterController.isGrounded);
        bool isFalling = currentMovement.y <= 0.0f || !isJumpPressed;
        float fallMultiplier = 2.0f;

        if(characterController.isGrounded)
        {
            if(isJumpAnimating)
            {
                //animator.SetBool(isJumpingHash, false);
                isJumpAnimating = false;
                if (jumpCount != 3)
                {
                    currentJumpResetRoutine = StartCoroutine(jumpResetRoutine());
                }
                if (jumpCount == 3)
                {
                    jumpCount = 0;
                    animator.SetInteger(jumpCountHash, jumpCount);
                }
            }
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;
        }
        else if (isFalling)
        {
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentRunMovement.y + (jumpGravities[jumpCount] * fallMultiplier * Time.deltaTime);
            float nextYVelocity = Mathf.Max((previousYVelocity + newYVelocity) * .5f, -20.0f);
            currentMovement.y = nextYVelocity;
            currentRunMovement.y = nextYVelocity;
        }
        else
        {
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentRunMovement.y + (jumpGravities[jumpCount] * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * .5f;
            currentMovement.y = nextYVelocity;
            currentRunMovement.y = nextYVelocity;
        }
    }

    void handleAnimation()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        animator.SetFloat("Speed", this.Speed);
        if (isMovementPressed && isRunning == false)
        {
            animator.SetBool(isRunningHash, true);
        }
        else if (!isMovementPressed && isRunning == true)
        {
            animator.SetBool(isRunningHash, false);
        }
        if ((isMovementPressed && isWalkPressed) && isWalking == false)
        {
            animator.SetBool(isWalkingHash, true);
        }
        else if ((!isMovementPressed || !isWalkPressed) && isWalking == true)
        {
            animator.SetBool(isWalkingHash, false);
        }
    }

    void Update()
    {
        handleRotation();
        handleAnimation();
        if(PlayerController.Instance.canMove == true)
        {
            handleMove();
        }
        handleGravity();
        handleJump();
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
