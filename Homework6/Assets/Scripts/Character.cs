using UnityEngine;
using Random = UnityEngine.Random;

public class Character : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float sprintSpeed = 5.0f;
    public float rotationSpeed = 0.2f;
    public float animationBlendSpeed = 0.2f;
    public float jumpSpeed = 7f;

    private CharacterController controller;
    private Animator animator;
    private Camera characterCamera;
    private float rotationAngel = 0.0f;
    private float targetAnimationSpeed = 0.0f;

    private float speedY;
    private float currentAttack;
    private float gravity = -9.81f;
    public bool isJumping = false;
    private bool isDead = false;
    private bool isSpawn = false;
    private bool isSprint = false;
    private bool onCooldown = false;

    public CharacterController Controller
    {
        get { return controller = controller ? controller : GetComponent<CharacterController>(); }
    }

    public Camera CharacterCamera
    {
        get { return characterCamera = characterCamera ? characterCamera : FindObjectOfType<Camera>(); }
    }

    public Animator CharacterAnimator
    {
        get { return animator = animator ? animator : GetComponent<Animator>(); }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.F) && !isDead)
        {
            CharacterAnimator.SetTrigger("Death");
            isDead = true;
        }

        if (!CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Spawn") && !isSpawn)
        {
            Debug.Log("Spawn");
            isSpawn = true;
        }

        if (!isDead && isSpawn)
        {
            if (CharacterAnimator.GetBool("Ready"))
            {
                onCooldown = false;
            }

            if (!CharacterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attacks"))
            {
                onCooldown = false;
            }

            if (Input.GetMouseButtonDown(0) && !onCooldown)
            {
                currentAttack = Random.Range(0.0f, 4.0f);
                CharacterAnimator.SetFloat("Attack", currentAttack);
                CharacterAnimator.SetTrigger("Ready");
                onCooldown = true;
            }

            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");

            if (Input.GetButtonDown("Jump") && !isJumping)
            {
                isJumping = true;
                CharacterAnimator.SetTrigger("Jump");
                speedY += jumpSpeed;
            }

            if (!Controller.isGrounded)
            {
                speedY += gravity * Time.deltaTime;
            }
            else if (speedY < 0.0f)
            {
                speedY = 0.0f;
            }

            CharacterAnimator.SetFloat("SpeedY", speedY / jumpSpeed);

            if (isJumping && speedY < 0.0f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f, LayerMask.GetMask("Default")))
                {
                    isJumping = false;
                    CharacterAnimator.SetTrigger("Land");
                }
            }

            isSprint = Input.GetKey(KeyCode.LeftShift);
            Vector3 movement = new Vector3(horizontal, 0.0f, vertical);
            Vector3 rotatedMovement = Quaternion.Euler(0.0f, CharacterCamera.transform.rotation.eulerAngles.y, 0.0f) *
                                      movement.normalized;
            Vector3 verticalMovement = Vector3.up * speedY;
            float currentSpeed = isSprint ? sprintSpeed : moveSpeed;

            Controller.Move((verticalMovement + rotatedMovement * currentSpeed) * Time.deltaTime);

            if (rotatedMovement.sqrMagnitude > 0.0f)
            {
                rotationAngel = Mathf.Atan2(rotatedMovement.x, rotatedMovement.z) * Mathf.Rad2Deg;
                targetAnimationSpeed = isSprint ? 1.0f : 0.5f;
            }
            else
            {
                targetAnimationSpeed = 0.0f;
            }

            CharacterAnimator.SetFloat("Speed",
                Mathf.Lerp(CharacterAnimator.GetFloat("Speed"), targetAnimationSpeed, animationBlendSpeed));
            Quaternion currentRotation = Controller.transform.rotation;
            Quaternion targetRotation = Quaternion.Euler(0.0f, rotationAngel, 0.0f);
            Controller.transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, rotationSpeed);
        }
    }
}