using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;

public class WandController : MonoBehaviour
{

    public SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)controllerIndex); } }

    public uint controllerIndex;

    public Vector3 velocity { get { return controller.velocity; } }
    public Vector3 angularVelocity { get { return controller.angularVelocity; } }

    void Start()
    {
        controllerIndex = (uint)this.GetComponent<SteamVR_TrackedObject>().index;
    }

    void Update()
    {
        controllerIndex = (uint)this.GetComponent<SteamVR_TrackedObject>().index;
    }

    public float TriggerAxis()
    {
        if (controller == null)
            return 0;

        return controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis1).x;
    }

    public Vector2 TouchPadAxis()
    {
        if (controller == null)
            return new Vector2();

        return controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
    }

    public bool MenuDown()
    {
        return controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu);
    }

    public bool MenuUp()
    {
        return controller.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu);
    }

    public bool MenuPress()
    {
        return controller.GetPress(SteamVR_Controller.ButtonMask.ApplicationMenu);
    }

    public bool GripDown()
    {
        return controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip);
    }

    public bool GripUp()
    {
        return controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip);
    }

    public bool GripPress()
    {
        return controller.GetPress(SteamVR_Controller.ButtonMask.Grip);
    }

    public bool TouchpadDown()
    {
        return controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad);
    }

    public bool TouchpadUp()
    {
        return controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad);
    }

    public bool TouchpadPress()
    {
        return controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad);
    }

    public bool DpadUP()
    {
        if (controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).y > 0.5f) return true;
        return false;
    }

    public bool DpadDOWN()
    {
        if (controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).y < -0.5f) return true;
        return false;
    }

    public bool DpadRIGHT()
    {
        if (controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).x > 0.5f) return true;
        return false;
    }

    public bool DpadLEFT()
    {
        if (controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).x < -0.5f) return true;
        return false;
    }

}

