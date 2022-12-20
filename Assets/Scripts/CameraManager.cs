using System;
using Actions;
using UnitBased;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject _actionCameraGameObject;

    private void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;
        
        HideActionCamera();
    }
    
    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                
                Unit shooterUnit = shootAction.Unit;
                Unit targetUnit = shootAction.TargetUnit;
                Vector3 cameraCharacterHeight = Vector3.up * 1.7f;

                Vector3 shootDirection =
                    (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;

                float shoulderOffsetAmount = .5f;
                Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDirection * shoulderOffsetAmount;

                Vector3 actionCameraPosition = 
                    shooterUnit.GetWorldPosition() + 
                    cameraCharacterHeight + 
                    shoulderOffset + 
                    shootDirection * -1;

                _actionCameraGameObject.transform.position = actionCameraPosition;
                _actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);

                ShowActionCamera();
                break;
        }
    }

    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                HideActionCamera();
                break;
        }
    }

    private void ShowActionCamera()
    {
        _actionCameraGameObject.SetActive(true);
    }
    private void HideActionCamera()
    {
        _actionCameraGameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        BaseAction.OnAnyActionStarted -= BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted -= BaseAction_OnAnyActionCompleted;
    }
}
