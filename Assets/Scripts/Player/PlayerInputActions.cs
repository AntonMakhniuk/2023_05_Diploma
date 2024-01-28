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
                },
                {
                    ""name"": ""AlignWithCamera"",
                    ""type"": ""Button"",
                    ""id"": ""e5de1bfa-6c14-42be-b71f-37d99f8fa03e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RotateAlongX"",
                    ""type"": ""Button"",
                    ""id"": ""20e14b37-b7b0-4820-8f7e-a77e462901f1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RotateAlongY"",
                    ""type"": ""Button"",
                    ""id"": ""4d9f5733-9df1-4dcb-b328-d24e8011a84d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RotateAlongZ"",
                    ""type"": ""Button"",
                    ""id"": ""9de35a2d-9a5d-47cb-9cb7-2a34e5948d36"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ToggleTractorBeam"",
                    ""type"": ""Button"",
                    ""id"": ""0546c9aa-ea63-4dcc-8469-54bf924fb546"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ToggleDrill"",
                    ""type"": ""Button"",
                    ""id"": ""eb13f302-d11f-4eed-b75f-da5959c0e791"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ToggleGasCollector"",
                    ""type"": ""Button"",
                    ""id"": ""30a32f41-8864-45a9-b2bf-8e92cbd67098"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
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
                },
                {
                    ""name"": """",
                    ""id"": ""398f6a8d-73ab-4f23-8e8b-60c0333971a9"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AlignWithCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Up/Down"",
                    ""id"": ""c15f978c-efcf-44b6-ac9c-95685c47a1ad"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateAlongX"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""f31702fa-3d26-4b90-8118-669824d83f54"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateAlongX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""e653d19b-d44c-4a81-b4e5-fa78089d56c0"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateAlongX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left/Right"",
                    ""id"": ""7497a25c-121b-4579-a5ac-667863ad3957"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateAlongY"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""f133044d-e21c-4947-b839-4b9e9306a16c"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateAlongY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""aa1bdbf1-fd54-4347-9d54-72747c44101c"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateAlongY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""LeftTilt/RightTilt"",
                    ""id"": ""c7198939-499a-4fa1-a516-d490a2f636c2"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateAlongZ"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""eba4ecc4-1d6b-4fb8-8c18-01ecab94f1bc"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateAlongZ"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""a11563c6-5314-44fe-86a5-1d8b9b205929"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateAlongZ"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""71cc4386-c5e5-411c-969f-63611f2d9ed5"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleDrill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""da645384-fbff-45ab-9ca9-b4c431cc0d1e"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleGasCollector"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4e09b738-071e-4b68-b10c-9dc2634a031f"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleTractorBeam"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
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
        m_PlayerShip_AlignWithCamera = m_PlayerShip.FindAction("AlignWithCamera", throwIfNotFound: true);
        m_PlayerShip_RotateAlongX = m_PlayerShip.FindAction("RotateAlongX", throwIfNotFound: true);
        m_PlayerShip_RotateAlongY = m_PlayerShip.FindAction("RotateAlongY", throwIfNotFound: true);
        m_PlayerShip_RotateAlongZ = m_PlayerShip.FindAction("RotateAlongZ", throwIfNotFound: true);
        m_PlayerShip_ToggleTractorBeam = m_PlayerShip.FindAction("ToggleTractorBeam", throwIfNotFound: true);
        m_PlayerShip_ToggleDrill = m_PlayerShip.FindAction("ToggleDrill", throwIfNotFound: true);
        m_PlayerShip_ToggleGasCollector = m_PlayerShip.FindAction("ToggleGasCollector", throwIfNotFound: true);
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
    private readonly InputAction m_PlayerShip_AlignWithCamera;
    private readonly InputAction m_PlayerShip_RotateAlongX;
    private readonly InputAction m_PlayerShip_RotateAlongY;
    private readonly InputAction m_PlayerShip_RotateAlongZ;
    private readonly InputAction m_PlayerShip_ToggleTractorBeam;
    private readonly InputAction m_PlayerShip_ToggleDrill;
    private readonly InputAction m_PlayerShip_ToggleGasCollector;
    public struct PlayerShipActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerShipActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @MouseCameraMovement => m_Wrapper.m_PlayerShip_MouseCameraMovement;
        public InputAction @Rotation => m_Wrapper.m_PlayerShip_Rotation;
        public InputAction @Movement => m_Wrapper.m_PlayerShip_Movement;
        public InputAction @AlignWithCamera => m_Wrapper.m_PlayerShip_AlignWithCamera;
        public InputAction @RotateAlongX => m_Wrapper.m_PlayerShip_RotateAlongX;
        public InputAction @RotateAlongY => m_Wrapper.m_PlayerShip_RotateAlongY;
        public InputAction @RotateAlongZ => m_Wrapper.m_PlayerShip_RotateAlongZ;
        public InputAction @ToggleTractorBeam => m_Wrapper.m_PlayerShip_ToggleTractorBeam;
        public InputAction @ToggleDrill => m_Wrapper.m_PlayerShip_ToggleDrill;
        public InputAction @ToggleGasCollector => m_Wrapper.m_PlayerShip_ToggleGasCollector;
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
            @AlignWithCamera.started += instance.OnAlignWithCamera;
            @AlignWithCamera.performed += instance.OnAlignWithCamera;
            @AlignWithCamera.canceled += instance.OnAlignWithCamera;
            @RotateAlongX.started += instance.OnRotateAlongX;
            @RotateAlongX.performed += instance.OnRotateAlongX;
            @RotateAlongX.canceled += instance.OnRotateAlongX;
            @RotateAlongY.started += instance.OnRotateAlongY;
            @RotateAlongY.performed += instance.OnRotateAlongY;
            @RotateAlongY.canceled += instance.OnRotateAlongY;
            @RotateAlongZ.started += instance.OnRotateAlongZ;
            @RotateAlongZ.performed += instance.OnRotateAlongZ;
            @RotateAlongZ.canceled += instance.OnRotateAlongZ;
            @ToggleTractorBeam.started += instance.OnToggleTractorBeam;
            @ToggleTractorBeam.performed += instance.OnToggleTractorBeam;
            @ToggleTractorBeam.canceled += instance.OnToggleTractorBeam;
            @ToggleDrill.started += instance.OnToggleDrill;
            @ToggleDrill.performed += instance.OnToggleDrill;
            @ToggleDrill.canceled += instance.OnToggleDrill;
            @ToggleGasCollector.started += instance.OnToggleGasCollector;
            @ToggleGasCollector.performed += instance.OnToggleGasCollector;
            @ToggleGasCollector.canceled += instance.OnToggleGasCollector;
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
            @AlignWithCamera.started -= instance.OnAlignWithCamera;
            @AlignWithCamera.performed -= instance.OnAlignWithCamera;
            @AlignWithCamera.canceled -= instance.OnAlignWithCamera;
            @RotateAlongX.started -= instance.OnRotateAlongX;
            @RotateAlongX.performed -= instance.OnRotateAlongX;
            @RotateAlongX.canceled -= instance.OnRotateAlongX;
            @RotateAlongY.started -= instance.OnRotateAlongY;
            @RotateAlongY.performed -= instance.OnRotateAlongY;
            @RotateAlongY.canceled -= instance.OnRotateAlongY;
            @RotateAlongZ.started -= instance.OnRotateAlongZ;
            @RotateAlongZ.performed -= instance.OnRotateAlongZ;
            @RotateAlongZ.canceled -= instance.OnRotateAlongZ;
            @ToggleTractorBeam.started -= instance.OnToggleTractorBeam;
            @ToggleTractorBeam.performed -= instance.OnToggleTractorBeam;
            @ToggleTractorBeam.canceled -= instance.OnToggleTractorBeam;
            @ToggleDrill.started -= instance.OnToggleDrill;
            @ToggleDrill.performed -= instance.OnToggleDrill;
            @ToggleDrill.canceled -= instance.OnToggleDrill;
            @ToggleGasCollector.started -= instance.OnToggleGasCollector;
            @ToggleGasCollector.performed -= instance.OnToggleGasCollector;
            @ToggleGasCollector.canceled -= instance.OnToggleGasCollector;
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
        void OnAlignWithCamera(InputAction.CallbackContext context);
        void OnRotateAlongX(InputAction.CallbackContext context);
        void OnRotateAlongY(InputAction.CallbackContext context);
        void OnRotateAlongZ(InputAction.CallbackContext context);
        void OnToggleTractorBeam(InputAction.CallbackContext context);
        void OnToggleDrill(InputAction.CallbackContext context);
        void OnToggleGasCollector(InputAction.CallbackContext context);
    }
}
