using System;
using Actions;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Transform _bulletProjectilePrefab;
    [SerializeField] private Transform _shootPointTransform;
    [SerializeField] private Animator _animator;
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int Shoot = Animator.StringToHash("Shoot");

    private void Awake()
    {
        if (TryGetComponent(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        _animator.SetBool(IsWalking, true);
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        _animator.SetBool(IsWalking, false);
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        _animator.SetTrigger(Shoot);
        Transform bulletProjectileTransform =
            Instantiate(_bulletProjectilePrefab, _shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();
        Vector3 targetUnitShootAtPosition = e.TargetUnit.GetWorldPosition();
        targetUnitShootAtPosition.y = _shootPointTransform.position.y;
        bulletProjectile.Setup(targetUnitShootAtPosition);
    }
}