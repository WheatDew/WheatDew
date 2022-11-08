using UnityEngine;
using UnityEngine.EventSystems;

[AddComponentMenu("")] // Don't display in add component menu
public class CharacterMovement : MonoBehaviour
{
    public bool useCharacterForward = false;
    public bool lockToCameraForward = false;
    public float turnSpeed = 10f;
    public KeyCode sprintJoystick = KeyCode.JoystickButton2;
    public KeyCode sprintKeyboard = KeyCode.Space;

    private float turnSpeedMultiplier;
    private float speed = 0f;
    private float direction = 0f;
    private bool isSprinting = false;
    private Animator anim;
    private Vector3 targetDirection;
    private Vector2 input;
    private Quaternion freeRotation;
    private Camera mainCamera;
    private float velocity;
    public static bool isMoving=true;


    //额外的增量
    public CWeapon weapon;
    public bool fighting=false;
    public bool isPlayer = false;
    private Rigidbody rigidbody;

    // Use this for initialization
    void Start()
    {
        //初始化数据
        anim = GetComponent<Animator>();
        mainCamera = Camera.main;

        rigidbody = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                isPlayer = true;
            }

            if (isMoving)
            {
                input.x = Input.GetAxis("Horizontal");
                input.y = Input.GetAxis("Vertical");

                

                // set speed to both vertical and horizontal inputs
                if (useCharacterForward)
                    speed = Mathf.Abs(input.x) + input.y;
                else
                    speed = Mathf.Abs(input.x) + Mathf.Abs(input.y);

                speed = Mathf.Clamp(speed, 0f, 1f);
                speed = Mathf.SmoothDamp(anim.GetFloat("Speed"), speed, ref velocity, 0.1f);
                anim.SetFloat("Speed", speed);

                if (input.y < 0f && useCharacterForward)
                    direction = input.y;
                else
                    direction = 0f;

                anim.SetFloat("Direction", direction);

                // set sprinting
                isSprinting = ((Input.GetKey(sprintJoystick) || Input.GetKey(sprintKeyboard)) && input != Vector2.zero && direction >= 0f);
                anim.SetBool("isSprinting", isSprinting);

                // Update target direction relative to the camera view (or not if the Keep Direction option is checked)
                UpdateTargetDirection();
                if (input != Vector2.zero && targetDirection.magnitude > 0.1f)
                {
                    Vector3 lookDirection = targetDirection.normalized;
                    freeRotation = Quaternion.LookRotation(lookDirection, transform.up);
                    var diferenceRotation = freeRotation.eulerAngles.y - transform.eulerAngles.y;
                    var eulerY = transform.eulerAngles.y;

                    if (diferenceRotation < 0 || diferenceRotation > 0) eulerY = freeRotation.eulerAngles.y;
                    var euler = new Vector3(0, eulerY, 0);

                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(euler), turnSpeed * turnSpeedMultiplier * Time.deltaTime);
                }
            }

            rigidbody.velocity = transform.TransformDirection(speed * Vector3.forward*3);

            //Attack
            if (fighting && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                anim.SetTrigger("Attack");

            }
        }
        
    }

    public void DisplayWeapon()
    {
        weapon.gameObject.SetActive(true);

        WeaponBoxActive();
    }

    public void HiddenWeapon()
    {
        if (!anim.GetBool("Attack"))
            weapon.gameObject.SetActive(false);

        WeaponBoxFreeze();
    }

    public void WeaponBoxActive()
    {
        weapon.box.enabled = true;
    }

    public void WeaponBoxFreeze()
    {
        weapon.box.enabled = false;
    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Weapon"&&other.gameObject!=weapon)
        {
            if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            anim.SetTrigger("Death");

        }
    }


    public virtual void UpdateTargetDirection()
    {
        if (!useCharacterForward)
        {
            turnSpeedMultiplier = 1f;
            var forward = mainCamera.transform.TransformDirection(Vector3.forward);
            forward.y = 0;

            //get the right-facing direction of the referenceTransform
            var right = mainCamera.transform.TransformDirection(Vector3.right);

            // determine the direction the player will face based on input and the referenceTransform's right and forward directions
            targetDirection = input.x * right + input.y * forward;
        }
        else
        {
            turnSpeedMultiplier = 0.2f;
            var forward = transform.TransformDirection(Vector3.forward);
            forward.y = 0;

            //get the right-facing direction of the referenceTransform
            var right = transform.TransformDirection(Vector3.right);
            targetDirection = input.x * right + Mathf.Abs(input.y) * forward;
        }
    }
}
