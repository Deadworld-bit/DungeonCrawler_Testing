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
    private int _upperBodyLayerIndex;
    private bool _isGunOut = false;

    private void Awake()
    {
        CacheComponents();
        SetRigidbodyConstraints();
        GetUpperBodyLayerIndex();
    }

    private void Start()
    {
        SetUpperBodyLayerWeight(0f);
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

    private void CacheComponents()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void SetRigidbodyConstraints()
    {
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void GetUpperBodyLayerIndex()
    {
        _upperBodyLayerIndex = _animator.GetLayerIndex("UpperBodyLayer");
    }

    private void HandleInput()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;

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

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ToggleGun();
        }
    }

    private void ToggleGun()
    {
        if (!_isGunOut)
        {
            _isGunOut = true;
            SetUpperBodyLayerWeight(1f);
            _animator.SetTrigger("PullOut");
        }
        else
        {
            _animator.SetTrigger("Hide");
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

    private void SetUpperBodyLayerWeight(float weight)
    {
        _animator.SetLayerWeight(_upperBodyLayerIndex, weight);
    }

    public void OnHideAnimationFinished()
    {
        _isGunOut = false;
        SetUpperBodyLayerWeight(0f);
    }
}