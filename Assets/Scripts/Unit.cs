using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator _unitAnimator;
    
    private Vector3 _targetPosition;
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");

    private void Awake()
    {
        _targetPosition = transform.position;
    }

    private void Update()
    {
        float stoppingDistance = .1f;

        if (Vector3.Distance(transform.position, _targetPosition) > stoppingDistance)
        {
            Vector3 moveDirection = (_targetPosition - transform.position).normalized;
            
            float moveSpeed = 4f;
            transform.position += moveDirection * Time.deltaTime * moveSpeed;
            
            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
            
            _unitAnimator.SetBool(IsWalking, true);
        }
        else
        {
            _unitAnimator.SetBool(IsWalking, false);
        }
    }

    public void Move(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
}