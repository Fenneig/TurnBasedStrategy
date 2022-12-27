using Actions;
using UnityEngine;

namespace Utils.Camera
{
    public class ScreenShakeActions : MonoBehaviour
    {
        private void Start()
        {
            ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
        }

        private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
        {
            ScreenShake.Instance.Shake(5f);
        }

        private void OnDestroy()
        {
            ShootAction.OnAnyShoot -= ShootAction_OnAnyShoot;
        }
    }
}