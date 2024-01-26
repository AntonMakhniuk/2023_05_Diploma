//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Scripts/Player/PlayerInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""PlayerShip"",
            ""id"": ""40e0c021-8b2b-47c2-8c09-0c07232be718"",
            ""actions"": [
                {
                    ""name"": ""MouseCameraMovement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""79c76f7d-7069-4fd2-bbcf-8c723260f903"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Rotation"",
                    ""type"": ""Value"",
                    ""id"": ""95e41c8e-bc79-4079-b75b-c356a5d6506d"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Movement"",
                    ""type"": ""Button"",
                    ""id"": ""a45dd632-7566-4c7b-b7b2-eb9b6bc79c2a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0ca67ec3-3ae5-49d7-b7da-19364fa81e10"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseCameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Up/Down/Left/Right/LTilt/RTilt"",
                    ""id"": ""45060c5f-ac8c-45c3-9b76-ac244b74a3f7"",
                    ""path"": ""3DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotation"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""38630015-dfd8-4547-8190-c8e8a57b1101"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""649c0668-732d-4bb9-b7c4-0f6835ca7b5f"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""4a8dd3ff-1854-48c4-ba4a-80d586afcc66"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6827970b-59f9-4e15-9e2a-1329afa6b3c7"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""forward"",
                    ""id"": ""7fd15ae9-f3e6-4dfa-979b-fc1cb8430f63"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""backward"",
                    ""id"": ""fd46c5f2-c89d-47d0-b6c9-d4f0431c7c0e"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Forward/Backward"",
                    ""id"": ""852620a6-d135-49fd-b8ad-90d2ec75860f"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""75c27e4a-f1c2-4f53-96b2-1dabc1cd531a"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""97857c57-7643-4bc2-a9ee-09a83bca7171"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerShip
        m_PlayerShip = asset.FindActionMap("PlayerShip", throwIfNotFound: true);
        m_PlayerShip_MouseCameraMovement = m_PlayerShip.FindAction("MouseCameraMovement", throwIfNotFound: true);
        m_PlayerShip_Rotation = m_PlayerShip.FindAction("Rotation", throwIfNotFound: true);
        m_PlayerShip_Movement = m_PlayerShip.FindAction("Movement", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // PlayerShip
    private readonly InputActionMap m_PlayerShip;
    private List<IPlayerShipActions> m_PlayerShipActionsCallbackInterfaces = new List<IPlayerShipActions>();
    private readonly InputAction m_PlayerShip_MouseCameraMovement;
    private readonly InputAction m_PlayerShip_Rotation;
    private readonly InputAction m_PlayerShip_Movement;
    public struct PlayerShipActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerShipActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @MouseCameraMovement => m_Wrapper.m_PlayerShip_MouseCameraMovement;
        public InputAction @Rotation => m_Wrapper.m_PlayerShip_Rotation;
        public InputAction @Movement => m_Wrapper.m_PlayerShip_Movement;
        public InputActionMap Get() { return m_Wrapper.m_PlayerShip; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerShipActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerShipActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerShipActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerShipActionsCallbackInterfaces.Add(instance);
            @MouseCameraMovement.started += instance.OnMouseCameraMovement;
            @MouseCameraMovement.performed += instance.OnMouseCameraMovement;
            @MouseCameraMovement.canceled += instance.OnMouseCameraMovement;
            @Rotation.started += instance.OnRotation;
            @Rotation.performed += instance.OnRotation;
            @Rotation.canceled += instance.OnRotation;
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
        }

        private void UnregisterCallbacks(IPlayerShipActions instance)
        {
            @MouseCameraMovement.started -= instance.OnMouseCameraMovement;
            @MouseCameraMovement.performed -= instance.OnMouseCameraMovement;
            @MouseCameraMovement.canceled -= instance.OnMouseCameraMovement;
            @Rotation.started -= instance.OnRotation;
            @Rotation.performed -= instance.OnRotation;
            @Rotation.canceled -= instance.OnRotation;
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
        }

        public void RemoveCallbacks(IPlayerShipActions instance)
        {
            if (m_Wrapper.m_PlayerShipActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerShipActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerShipActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerShipActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerShipActions @PlayerShip => new PlayerShipActions(this);
    public interface IPlayerShipActions
    {
        void OnMouseCameraMovement(InputAction.CallbackContext context);
        void OnRotation(InputAction.CallbackContext context);
        void OnMovement(InputAction.CallbackContext context);
    }
}
