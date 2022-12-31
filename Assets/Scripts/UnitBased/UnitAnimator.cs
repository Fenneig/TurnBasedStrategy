using System;
using Actions;
using UnityEngine;
using Projectiles;

namespace UnitBased
{
    public class UnitAnimator : MonoBehaviour
    {
        [Header("Bullet references")]
        [SerializeField] private Transform _bulletProjectilePrefab;
        [SerializeField] private Transform _shootPointTransform;
        [Space][Header("Weapon prefabs references")]
        [SerializeField] private GameObject _rifleTransform;
        [SerializeField] private GameObject _swordTransform;
        [Space][Header("Other references")]
        [SerializeField] private Animator _animator;
        
        
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int Shoot = Animator.StringToHash("Shoot");
        private static readonly int SwordSlash = Animator.StringToHash("SwordSlash");

        private MoveAction _moveAction;
        private ShootAction _shootAction;
        private SwordAction _swordAction;

        private void Awake()
        {
            if (TryGetComponent(out MoveAction moveAction))
            {
                _moveAction = moveAction;
                moveAction.OnStartMoving += MoveAction_OnStartMoving;
                moveAction.OnStopMoving += MoveAction_OnStopMoving;
            }

            if (TryGetComponent(out ShootAction shootAction))
            {
                _shootAction = shootAction;
                shootAction.OnShoot += ShootAction_OnShoot;
            }

            if (TryGetComponent(out SwordAction swordAction))
            {
                _swordAction = swordAction;
                swordAction.OnSwordActionStarted += SwordAction_OnSwordActionStarted;
                swordAction.OnSwordActionCompleted += SwordAction_OnSwordActionCompleted;
            }
        }

        private void Start()
        {
            EquipRifle();
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
            Vector3 targetUnitShootAtPosition = e.TargetUnit.WorldPosition;
            targetUnitShootAtPosition.y = _shootPointTransform.position.y;
            bulletProjectile.Setup(targetUnitShootAtPosition);
        }

        private void SwordAction_OnSwordActionCompleted(object sender, EventArgs e)
        {
            EquipRifle();
        }
        private void SwordAction_OnSwordActionStarted(object sender, EventArgs e)
        {
            EquipSword();
            _animator.SetTrigger(SwordSlash);
        }

        private void OnDestroy()
        {
            if (_moveAction != null)
            {
                _moveAction.OnStartMoving -= MoveAction_OnStartMoving;
                _moveAction.OnStopMoving -= MoveAction_OnStopMoving;
            }

            if (_shootAction != null)
            {
                _shootAction.OnShoot -= ShootAction_OnShoot;
            }
            
            
            if (_swordAction != null)
            {
                _swordAction.OnSwordActionStarted -= SwordAction_OnSwordActionStarted;
                _swordAction.OnSwordActionCompleted -= SwordAction_OnSwordActionCompleted;
            }
        }

        private void EquipSword()
        {
            _rifleTransform.SetActive(false);
            _swordTransform.SetActive(true);
        }

        private void EquipRifle()
        {
            _rifleTransform.SetActive(true);
            _swordTransform.SetActive(false);
        }
    }
}