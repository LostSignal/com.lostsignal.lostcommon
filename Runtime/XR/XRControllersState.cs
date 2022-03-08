//-----------------------------------------------------------------------
// <copyright file="XRControllersState.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

#if USING_UNITY_XR_MANAGEMENT

namespace Lost.XR
{
    using System.Collections.Generic;
    using System.Text;
    using UnityEngine;
    using UnityEngine.XR;
    using System.Runtime.CompilerServices;

    public struct Button
    {
        private bool isPressed;
        private bool wasPressedThisFrame;
        private bool wasReleasedThisFrame;
        private float pressedTime;

        public bool IsPressed
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.isPressed;
        }

        public bool WasPressedThisFrame
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.wasPressedThisFrame;
        }

        public bool WasReleasedThisFrame
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.wasReleasedThisFrame;
        }

        public float PressedTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.pressedTime;
        }

        public static Button Update(bool currentIsPressed, float currentPressedTime, bool newIsPressed, float deltaTime)
        {
            if (newIsPressed)
            {
                return currentIsPressed ?
                    new Button { isPressed = true, pressedTime = currentPressedTime + deltaTime } :
                    new Button { isPressed = true, wasPressedThisFrame = true };
            }
            else
            {
                return currentIsPressed == false ?
                    new Button { isPressed = false } :
                    new Button { isPressed = false, wasReleasedThisFrame = true };
            }
        }
    }

    public class XRControllersState
    {
        private static readonly StringBuilder StringBuilderCache = new StringBuilder();
        private static readonly List<InputDevice> InputDevicesList = new List<InputDevice>();
        private static readonly InputDeviceCharacteristics ControllerCharacteristics = InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.HeldInHand;

        public XRControllersState()
        {
            UnityEngine.XR.XRDevice.deviceLoaded += DeviceLoaded;
            UnityEngine.XR.InputDevices.deviceConnected += DeviceConntected;

            void DeviceLoaded(string name)
            {
                InputDevices.GetDevices(InputDevicesList);
            }

            void DeviceConntected(InputDevice inputDevice)
            {
                InputDevices.GetDevices(InputDevicesList);
            }
        }

        // Left Hand
        private bool isLeftControllerConnected;
        private float leftTrigger;
        private float leftGrip;
        private Vector2 leftStick;
        private Button leftStickClick;
        private Button leftPrimaryButtion;
        private Button leftSecondaryButton;
        private Button menu;

        // Right Hand
        private bool isRightControllerConnected;
        private float rightTrigger;
        private float rightGrip;
        private Vector2 rightStick;
        private Button rightStickClick;
        private Button rightPrimaryButton;
        private Button rightSecondaryButton;

        private int lastUpdatedFrame = -1;

        public bool IsLeftControllerConnected
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.isLeftControllerConnected;
        }

        public float LeftTrigger
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.leftTrigger;
        }

        public float LeftGrip
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.leftGrip;
        }

        public Vector2 LeftStick
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.leftStick;
        }

        public Button LeftStickClick
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.leftStickClick;
        }

        public Button LeftPrimaryButtion
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.leftPrimaryButtion;
        }
        public Button LeftSecondaryButton
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.leftSecondaryButton;
        }

        public Button Menu
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.menu;
        }

        private bool IsRightControllerConnected
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.isRightControllerConnected;
        }

        private float RightTrigger
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.rightTrigger;
        }

        private float RightGrip
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.rightGrip;
        }

        private Vector2 RightStick
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.rightStick;
        }

        private Button RightStickClick
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.rightStickClick;
        }

        private Button RightPrimaryButton
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.rightPrimaryButton;
        }

        private Button RightSecondaryButton
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.rightSecondaryButton;
        }

        public void UpdateValues()
        {
            int currentFrame = Time.frameCount;

            if (this.lastUpdatedFrame == currentFrame)
            {
                return;
            }

            float deltaTime = Time.deltaTime;

            this.lastUpdatedFrame = currentFrame;
            this.isLeftControllerConnected = false;
            this.isRightControllerConnected = false;

            foreach (var device in InputDevicesList)
            {
                if ((device.characteristics & ControllerCharacteristics) > 0)
                {
                    device.TryGetFeatureValue(CommonUsages.trigger, out float trigger);
                    device.TryGetFeatureValue(CommonUsages.grip, out float grip);
                    device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxis);
                    device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool primary2DAxisClick);
                    device.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButton);
                    device.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButton);
                    device.TryGetFeatureValue(CommonUsages.menuButton, out bool menuButton);

                    if ((device.characteristics & InputDeviceCharacteristics.Left) > 0)
                    {
                        this.isLeftControllerConnected = true;
                        this.leftTrigger = trigger;
                        this.leftGrip = grip;
                        this.leftStick = primary2DAxis;
                        this.leftStickClick = Button.Update(this.leftStickClick.IsPressed, this.leftStickClick.PressedTime, primary2DAxisClick, deltaTime);
                        this.leftPrimaryButtion = Button.Update(this.leftPrimaryButtion.IsPressed, this.leftPrimaryButtion.PressedTime, primaryButton, deltaTime);
                        this.leftSecondaryButton = Button.Update(this.leftSecondaryButton.IsPressed, this.leftSecondaryButton.PressedTime, secondaryButton, deltaTime);
                        this.menu = Button.Update(this.menu.IsPressed, this.menu.PressedTime, menuButton, deltaTime);
                    }
                    else if ((device.characteristics & InputDeviceCharacteristics.Right) > 0)
                    {
                        this.isRightControllerConnected = true;
                        this.rightTrigger = trigger;
                        this.rightGrip = grip;
                        this.rightStick = primary2DAxis;
                        this.rightStickClick = Button.Update(this.rightStickClick.IsPressed, this.rightStickClick.PressedTime, primary2DAxisClick, deltaTime);
                        this.rightPrimaryButton = Button.Update(this.rightPrimaryButton.IsPressed, this.rightPrimaryButton.PressedTime, primaryButton, deltaTime);
                        this.rightSecondaryButton = Button.Update(this.rightSecondaryButton.IsPressed, this.rightSecondaryButton.PressedTime, secondaryButton, deltaTime);
                    }
                }
            }
        }

        public StringBuilder GetOculusDebugOutput()
        {
            StringBuilderCache.Clear();

            StringBuilderCache.Append($"Left: Connected = {this.isLeftControllerConnected},");
            StringBuilderCache.Append($" Trigger = {this.leftTrigger:0.00},");
            StringBuilderCache.Append($" Grip = {this.leftGrip:0.00},");
            StringBuilderCache.Append($" Stick = {this.leftStick},");
            StringBuilderCache.Append($" Stick Click = {this.leftStickClick},");
            StringBuilderCache.AppendLine();

            StringBuilderCache.Append($"Right: Connected = {this.isRightControllerConnected},");
            StringBuilderCache.Append($" Trigger = {this.rightTrigger:0.00},");
            StringBuilderCache.Append($" Grip = {this.rightGrip:0.00},");
            StringBuilderCache.Append($" Stick = {this.rightStick},");
            StringBuilderCache.Append($" Stick Click = {this.rightStickClick},");
            StringBuilderCache.AppendLine();

            StringBuilderCache.Append($" A = {this.rightPrimaryButton.IsPressed},");
            StringBuilderCache.Append($" B = {this.rightSecondaryButton.IsPressed},");
            StringBuilderCache.Append($" X = {this.leftPrimaryButtion.IsPressed},");
            StringBuilderCache.Append($" Y = {this.leftSecondaryButton.IsPressed},");
            StringBuilderCache.Append($" Menu = {this.menu.IsPressed}");

            return StringBuilderCache;
        }


        /* ------ HAPTICS ------
        List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>(); 

        UnityEngine.XR.InputDevices.GetDevicesWithRole(UnityEngine.XR.InputDeviceRole.RightHanded, devices);

        foreach (var device in devices)
        {
            UnityEngine.XR.HapticCapabilities capabilities;
            if (device.TryGetHapticCapabilities(out capabilities))
            {
                    if (capabilities.supportsImpulse)
                    {
                        uint channel = 0;
                        float amplitude = 0.5f;
                        float duration = 1.0f;
                        device.SendHapticImpulse(channel, amplitude, duration);
                    }
            }
        }
        */
    }
}

#endif
