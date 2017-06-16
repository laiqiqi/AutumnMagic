using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
public class Puzzle : VRTK_InteractableObject
{
    private VRTK_ControllerActions controllerActions;

    public override void Grabbed(GameObject grabbingObject)
    {
        base.Grabbed(grabbingObject);
        controllerActions = grabbingObject.GetComponent<VRTK_ControllerActions>();
        controllerActions.TriggerHapticPulse(3000.0f, 0.1f, 0.05f);
        GameManager.Instance.Audio.PlayOneShot(GameManager.Instance.SE_Clips[7]);
    }
    public override void Ungrabbed(GameObject previousGrabbingObject)
    {
        base.Ungrabbed(previousGrabbingObject);
        controllerActions.TriggerHapticPulse(3000.0f,0.1f, 0.01f);
        GameManager.Instance.Audio.PlayOneShot(GameManager.Instance.SE_Clips[6]);
    }
}
