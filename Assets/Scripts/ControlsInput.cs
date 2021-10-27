// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/ControlsInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @ControlsInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @ControlsInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ControlsInput"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""358861a1-0348-415a-9a67-8b54772e2c77"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""84eaa1d0-4d35-465f-ad1b-c0728b7b9961"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""NormalizeVector2"",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""PauseGame"",
                    ""type"": ""Button"",
                    ""id"": ""b4935c4b-56d6-4053-a2cc-43aee9ee6fac"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Trap"",
                    ""type"": ""Button"",
                    ""id"": ""5b09d7f7-b0fa-4b44-bd4f-30615ef0e976"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ToggleGUI"",
                    ""type"": ""Button"",
                    ""id"": ""66d026ea-074b-4662-8c14-49b462f9aab8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ShootStraight"",
                    ""type"": ""Button"",
                    ""id"": ""bc9fc792-a9e0-4419-920d-293729bd0f2e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Move Keys"",
                    ""id"": ""8da1105c-0113-4746-b508-6c48778b9655"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a882e42a-a50d-419d-85d8-0e7047be9312"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9d795f4a-056d-446a-8d4e-4896de40dd49"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d51ad5cd-7c55-49e5-9562-0f1ec27aed54"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""8f535968-1de8-45bb-a6ad-9e806ce7281d"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""3a40d23c-44c8-4703-9039-a74f54dce683"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PauseGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3476dbe8-f50f-4392-827c-ee6599bdb68c"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Trap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6e5375eb-fbae-42a9-84b1-815ece80362c"",
                    ""path"": ""<Keyboard>/semicolon"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleGUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""12fe1173-f0de-473e-8269-554551794376"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ShootStraight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_PauseGame = m_Player.FindAction("PauseGame", throwIfNotFound: true);
        m_Player_Trap = m_Player.FindAction("Trap", throwIfNotFound: true);
        m_Player_ToggleGUI = m_Player.FindAction("ToggleGUI", throwIfNotFound: true);
        m_Player_ShootStraight = m_Player.FindAction("ShootStraight", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_PauseGame;
    private readonly InputAction m_Player_Trap;
    private readonly InputAction m_Player_ToggleGUI;
    private readonly InputAction m_Player_ShootStraight;
    public struct PlayerActions
    {
        private @ControlsInput m_Wrapper;
        public PlayerActions(@ControlsInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @PauseGame => m_Wrapper.m_Player_PauseGame;
        public InputAction @Trap => m_Wrapper.m_Player_Trap;
        public InputAction @ToggleGUI => m_Wrapper.m_Player_ToggleGUI;
        public InputAction @ShootStraight => m_Wrapper.m_Player_ShootStraight;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @PauseGame.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPauseGame;
                @PauseGame.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPauseGame;
                @PauseGame.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPauseGame;
                @Trap.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTrap;
                @Trap.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTrap;
                @Trap.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTrap;
                @ToggleGUI.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleGUI;
                @ToggleGUI.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleGUI;
                @ToggleGUI.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnToggleGUI;
                @ShootStraight.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShootStraight;
                @ShootStraight.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShootStraight;
                @ShootStraight.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShootStraight;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @PauseGame.started += instance.OnPauseGame;
                @PauseGame.performed += instance.OnPauseGame;
                @PauseGame.canceled += instance.OnPauseGame;
                @Trap.started += instance.OnTrap;
                @Trap.performed += instance.OnTrap;
                @Trap.canceled += instance.OnTrap;
                @ToggleGUI.started += instance.OnToggleGUI;
                @ToggleGUI.performed += instance.OnToggleGUI;
                @ToggleGUI.canceled += instance.OnToggleGUI;
                @ShootStraight.started += instance.OnShootStraight;
                @ShootStraight.performed += instance.OnShootStraight;
                @ShootStraight.canceled += instance.OnShootStraight;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnPauseGame(InputAction.CallbackContext context);
        void OnTrap(InputAction.CallbackContext context);
        void OnToggleGUI(InputAction.CallbackContext context);
        void OnShootStraight(InputAction.CallbackContext context);
    }
}
