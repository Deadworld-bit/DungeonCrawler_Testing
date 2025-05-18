using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 10f;

    private Animator _animator;
    private Rigidbody _rigidbody;

    private Vector3 _moveDirection = Vector3.zero;
    private bool _isMoving;
    private int upperBodyLayerIndex; // Index of the UpperBodyLayer
    private bool isGunOut = false;   // Tracks if the gun is out

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        // Get the layer index for UpperBodyLayer
        upperBodyLayerIndex = _animator.GetLayerIndex("UpperBodyLayer");
    }

    private void Start()
    {
        // Initially, upper body is controlled by the base layer
        _animator.SetLayerWeight(upperBodyLayerIndex, 0f);
    }

    private void Update()
    {
        HandleInput();
        HandleRotation();
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleInput()
    {
        // Existing movement input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 input = new Vector3(horizontal, 0f, vertical).normalized;

        if (input.magnitude > 0.1f)
        {
            _moveDirection = transform.TransformDirection(input);
            _isMoving = true;
        }
        else
        {
            _moveDirection = Vector3.zero;
            _isMoving = false;
        }

        // Handle gun animation toggle with '1' key
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!isGunOut)
            {
                // Gun is holstered, pull it out
                isGunOut = true;
                _animator.SetLayerWeight(upperBodyLayerIndex, 1f); // Enable upper body layer
                _animator.SetTrigger("PullOut");                   // Trigger pull out animation
            }
            else
            {
                // Gun is out, hide it
                _animator.SetTrigger("Hide");                      // Trigger hide animation
            }
        }
    }

    private void HandleMovement()
    {
        if (_isMoving)
        {
            Vector3 newPosition = _rigidbody.position + _moveDirection * _moveSpeed * Time.fixedDeltaTime;
            _rigidbody.MovePosition(newPosition);
        }
    }

    private void HandleRotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, transform.position);
        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 mouseWorldPosition = ray.GetPoint(enter);
            Vector3 lookDirection = (mouseWorldPosition - transform.position).normalized;
            lookDirection.y = 0f;
            if (lookDirection.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            }
        }
    }

    private void UpdateAnimator()
    {
        _animator.SetBool("isMoving", _isMoving);
        Vector3 localMove = transform.InverseTransformDirection(_moveDirection);
        _animator.SetFloat("Horizontal", localMove.x);
        _animator.SetFloat("Vertical", localMove.z);
    }

    // Called by animation event at the end of the Hide animation
    public void OnHideAnimationFinished()
    {
        isGunOut = false;
        _animator.SetLayerWeight(upperBodyLayerIndex, 0f); // Disable upper body layer
    }
}