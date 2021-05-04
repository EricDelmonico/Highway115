// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/IntroControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @IntroControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @IntroControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""IntroControls"",
    ""maps"": [
        {
            ""name"": ""Intro"",
            ""id"": ""27eda1f8-3084-4a3c-9a2a-48e2de8de033"",
            ""actions"": [
                {
                    ""name"": ""NextImage"",
                    ""type"": ""Button"",
                    ""id"": ""aa82c965-1c50-4dfd-9693-be5a7c068bc1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""aeac231f-7da7-4641-ac3a-4247eb4bd463"",
                    ""path"": ""<Keyboard>/anyKey"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NextImage"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Intro
        m_Intro = asset.FindActionMap("Intro", throwIfNotFound: true);
        m_Intro_NextImage = m_Intro.FindAction("NextImage", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Intro
    private readonly InputActionMap m_Intro;
    private IIntroActions m_IntroActionsCallbackInterface;
    private readonly InputAction m_Intro_NextImage;
    public struct IntroActions
    {
        private @IntroControls m_Wrapper;
        public IntroActions(@IntroControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @NextImage => m_Wrapper.m_Intro_NextImage;
        public InputActionMap Get() { return m_Wrapper.m_Intro; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(IntroActions set) { return set.Get(); }
        public void SetCallbacks(IIntroActions instance)
        {
            if (m_Wrapper.m_IntroActionsCallbackInterface != null)
            {
                @NextImage.started -= m_Wrapper.m_IntroActionsCallbackInterface.OnNextImage;
                @NextImage.performed -= m_Wrapper.m_IntroActionsCallbackInterface.OnNextImage;
                @NextImage.canceled -= m_Wrapper.m_IntroActionsCallbackInterface.OnNextImage;
            }
            m_Wrapper.m_IntroActionsCallbackInterface = instance;
            if (instance != null)
            {
                @NextImage.started += instance.OnNextImage;
                @NextImage.performed += instance.OnNextImage;
                @NextImage.canceled += instance.OnNextImage;
            }
        }
    }
    public IntroActions @Intro => new IntroActions(this);
    public interface IIntroActions
    {
        void OnNextImage(InputAction.CallbackContext context);
    }
}
