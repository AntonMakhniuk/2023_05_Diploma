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
                    ""name"": ""AlignWithCamera"",
                    ""type"": ""Button"",
                    ""id"": ""e5de1bfa-6c14-42be-b71f-37d99f8fa03e"",
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
                },
                {
                    ""name"": ""InstrumentPrimary"",
                    ""type"": ""Button"",
                    ""id"": ""06370757-8d74-438c-a5fc-234cf5f42477"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""InstrumentSecondary"",
                    ""type"": ""Button"",
                    ""id"": ""b2505daa-6e37-4b97-b0f0-9b1bbd690924"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""6348877e-c5d8-41a3-912d-2826c54ab698"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Brakes"",
                    ""type"": ""Button"",
                    ""id"": ""f44241a1-ae12-49b1-968c-46e3a4f6f4e4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ToggleBombContainer"",
                    ""type"": ""Button"",
                    ""id"": ""d02f14e4-55c4-46ee-aeda-18a3c863062f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ToggleLaser"",
                    ""type"": ""Button"",
                    ""id"": ""129376ee-09e3-4437-9ad9-d91e94187d61"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""BothMouseButtonPushed"",
                    ""id"": ""9f4ddde9-9209-407e-bbb1-c5249994c1a3"",
                    ""path"": ""OneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AlignWithCamera"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""f20036ab-9cb5-4072-8477-ff25e7ed41e6"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AlignWithCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""binding"",
                    ""id"": ""850a0843-908d-4920-8fcd-350383647282"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AlignWithCamera"",
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
                    ""path"": ""<Keyboard>/q"",
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
                    ""path"": ""<Keyboard>/e"",
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
                },
                {
                    ""name"": """",
                    ""id"": ""169f05d3-c038-47c4-ae33-d1d2626bb92a"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InstrumentPrimary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""12e15f89-592b-4bca-8f66-9946a559425b"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InstrumentSecondary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Up/Down/Forward/Backward"",
                    ""id"": ""93e4117f-2630-4dcb-9185-3166cde7f4a0"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""0ff8fb32-fc32-43c5-9c54-d4fccb5321e7"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""339b26f0-e858-4021-9721-0adce4149b43"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b99d4a2f-e501-40c6-a64d-cd5a6db48d8e"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4be9077d-3c1f-4bfa-81ff-0883fdadd84d"",
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
                    ""id"": ""277b6764-9139-4d5b-a73c-3c024a818683"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Brakes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""baf57db0-9bae-4759-9aed-0dbb7ee92d4c"",
                    ""path"": ""<Keyboard>/9"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleBombContainer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""BothMouseButtons"",
                    ""id"": ""de32d885-7263-463e-873e-1709820524ff"",
                    ""path"": ""OneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AlignWithCamera"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""e988e783-28b7-4d47-bba0-5930542924c7"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AlignWithCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""binding"",
                    ""id"": ""f645a0c8-2261-41fc-adfb-cdab21b4a7fc"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AlignWithCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""3c2b93d3-0816-4461-b678-84864f1ba054"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleLaser"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""PlayerCamera"",
            ""id"": ""00054202-ff1e-4f31-a250-3e21c6a71386"",
            ""actions"": [
                {
                    ""name"": ""MouseCameraMovement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""f2a28994-8cd6-407f-b699-4abb462566b9"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a649b7da-7ebd-4869-b14f-e6fa898809ba"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseCameraMovement"",
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
        m_PlayerShip_AlignWithCamera = m_PlayerShip.FindAction("AlignWithCamera", throwIfNotFound: true);
        m_PlayerShip_RotateAlongY = m_PlayerShip.FindAction("RotateAlongY", throwIfNotFound: true);
        m_PlayerShip_RotateAlongZ = m_PlayerShip.FindAction("RotateAlongZ", throwIfNotFound: true);
        m_PlayerShip_ToggleTractorBeam = m_PlayerShip.FindAction("ToggleTractorBeam", throwIfNotFound: true);
        m_PlayerShip_ToggleDrill = m_PlayerShip.FindAction("ToggleDrill", throwIfNotFound: true);
        m_PlayerShip_ToggleGasCollector = m_PlayerShip.FindAction("ToggleGasCollector", throwIfNotFound: true);
        m_PlayerShip_InstrumentPrimary = m_PlayerShip.FindAction("InstrumentPrimary", throwIfNotFound: true);
        m_PlayerShip_InstrumentSecondary = m_PlayerShip.FindAction("InstrumentSecondary", throwIfNotFound: true);
        m_PlayerShip_Movement = m_PlayerShip.FindAction("Movement", throwIfNotFound: true);
        m_PlayerShip_Brakes = m_PlayerShip.FindAction("Brakes", throwIfNotFound: true);
        m_PlayerShip_ToggleBombContainer = m_PlayerShip.FindAction("ToggleBombContainer", throwIfNotFound: true);
        m_PlayerShip_ToggleLaser = m_PlayerShip.FindAction("ToggleLaser", throwIfNotFound: true);
        // PlayerCamera
        m_PlayerCamera = asset.FindActionMap("PlayerCamera", throwIfNotFound: true);
        m_PlayerCamera_MouseCameraMovement = m_PlayerCamera.FindAction("MouseCameraMovement", throwIfNotFound: true);
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
    private readonly InputAction m_PlayerShip_AlignWithCamera;
    private readonly InputAction m_PlayerShip_RotateAlongY;
    private readonly InputAction m_PlayerShip_RotateAlongZ;
    private readonly InputAction m_PlayerShip_ToggleTractorBeam;
    private readonly InputAction m_PlayerShip_ToggleDrill;
    private readonly InputAction m_PlayerShip_ToggleGasCollector;
    private readonly InputAction m_PlayerShip_InstrumentPrimary;
    private readonly InputAction m_PlayerShip_InstrumentSecondary;
    private readonly InputAction m_PlayerShip_Movement;
    private readonly InputAction m_PlayerShip_Brakes;
    private readonly InputAction m_PlayerShip_ToggleBombContainer;
    private readonly InputAction m_PlayerShip_ToggleLaser;
    public struct PlayerShipActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerShipActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @AlignWithCamera => m_Wrapper.m_PlayerShip_AlignWithCamera;
        public InputAction @RotateAlongY => m_Wrapper.m_PlayerShip_RotateAlongY;
        public InputAction @RotateAlongZ => m_Wrapper.m_PlayerShip_RotateAlongZ;
        public InputAction @ToggleTractorBeam => m_Wrapper.m_PlayerShip_ToggleTractorBeam;
        public InputAction @ToggleDrill => m_Wrapper.m_PlayerShip_ToggleDrill;
        public InputAction @ToggleGasCollector => m_Wrapper.m_PlayerShip_ToggleGasCollector;
        public InputAction @InstrumentPrimary => m_Wrapper.m_PlayerShip_InstrumentPrimary;
        public InputAction @InstrumentSecondary => m_Wrapper.m_PlayerShip_InstrumentSecondary;
        public InputAction @Movement => m_Wrapper.m_PlayerShip_Movement;
        public InputAction @Brakes => m_Wrapper.m_PlayerShip_Brakes;
        public InputAction @ToggleBombContainer => m_Wrapper.m_PlayerShip_ToggleBombContainer;
        public InputAction @ToggleLaser => m_Wrapper.m_PlayerShip_ToggleLaser;
        public InputActionMap Get() { return m_Wrapper.m_PlayerShip; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerShipActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerShipActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerShipActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerShipActionsCallbackInterfaces.Add(instance);
            @AlignWithCamera.started += instance.OnAlignWithCamera;
            @AlignWithCamera.performed += instance.OnAlignWithCamera;
            @AlignWithCamera.canceled += instance.OnAlignWithCamera;
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
            @InstrumentPrimary.started += instance.OnInstrumentPrimary;
            @InstrumentPrimary.performed += instance.OnInstrumentPrimary;
            @InstrumentPrimary.canceled += instance.OnInstrumentPrimary;
            @InstrumentSecondary.started += instance.OnInstrumentSecondary;
            @InstrumentSecondary.performed += instance.OnInstrumentSecondary;
            @InstrumentSecondary.canceled += instance.OnInstrumentSecondary;
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
            @Brakes.started += instance.OnBrakes;
            @Brakes.performed += instance.OnBrakes;
            @Brakes.canceled += instance.OnBrakes;
            @ToggleBombContainer.started += instance.OnToggleBombContainer;
            @ToggleBombContainer.performed += instance.OnToggleBombContainer;
            @ToggleBombContainer.canceled += instance.OnToggleBombContainer;
            @ToggleLaser.started += instance.OnToggleLaser;
            @ToggleLaser.performed += instance.OnToggleLaser;
            @ToggleLaser.canceled += instance.OnToggleLaser;
        }

        private void UnregisterCallbacks(IPlayerShipActions instance)
        {
            @AlignWithCamera.started -= instance.OnAlignWithCamera;
            @AlignWithCamera.performed -= instance.OnAlignWithCamera;
            @AlignWithCamera.canceled -= instance.OnAlignWithCamera;
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
            @InstrumentPrimary.started -= instance.OnInstrumentPrimary;
            @InstrumentPrimary.performed -= instance.OnInstrumentPrimary;
            @InstrumentPrimary.canceled -= instance.OnInstrumentPrimary;
            @InstrumentSecondary.started -= instance.OnInstrumentSecondary;
            @InstrumentSecondary.performed -= instance.OnInstrumentSecondary;
            @InstrumentSecondary.canceled -= instance.OnInstrumentSecondary;
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @Brakes.started -= instance.OnBrakes;
            @Brakes.performed -= instance.OnBrakes;
            @Brakes.canceled -= instance.OnBrakes;
            @ToggleBombContainer.started -= instance.OnToggleBombContainer;
            @ToggleBombContainer.performed -= instance.OnToggleBombContainer;
            @ToggleBombContainer.canceled -= instance.OnToggleBombContainer;
            @ToggleLaser.started -= instance.OnToggleLaser;
            @ToggleLaser.performed -= instance.OnToggleLaser;
            @ToggleLaser.canceled -= instance.OnToggleLaser;
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

    // PlayerCamera
    private readonly InputActionMap m_PlayerCamera;
    private List<IPlayerCameraActions> m_PlayerCameraActionsCallbackInterfaces = new List<IPlayerCameraActions>();
    private readonly InputAction m_PlayerCamera_MouseCameraMovement;
    public struct PlayerCameraActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerCameraActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @MouseCameraMovement => m_Wrapper.m_PlayerCamera_MouseCameraMovement;
        public InputActionMap Get() { return m_Wrapper.m_PlayerCamera; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerCameraActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerCameraActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerCameraActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerCameraActionsCallbackInterfaces.Add(instance);
            @MouseCameraMovement.started += instance.OnMouseCameraMovement;
            @MouseCameraMovement.performed += instance.OnMouseCameraMovement;
            @MouseCameraMovement.canceled += instance.OnMouseCameraMovement;
        }

        private void UnregisterCallbacks(IPlayerCameraActions instance)
        {
            @MouseCameraMovement.started -= instance.OnMouseCameraMovement;
            @MouseCameraMovement.performed -= instance.OnMouseCameraMovement;
            @MouseCameraMovement.canceled -= instance.OnMouseCameraMovement;
        }

        public void RemoveCallbacks(IPlayerCameraActions instance)
        {
            if (m_Wrapper.m_PlayerCameraActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerCameraActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerCameraActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerCameraActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerCameraActions @PlayerCamera => new PlayerCameraActions(this);
    public interface IPlayerShipActions
    {
        void OnAlignWithCamera(InputAction.CallbackContext context);
        void OnRotateAlongY(InputAction.CallbackContext context);
        void OnRotateAlongZ(InputAction.CallbackContext context);
        void OnToggleTractorBeam(InputAction.CallbackContext context);
        void OnToggleDrill(InputAction.CallbackContext context);
        void OnToggleGasCollector(InputAction.CallbackContext context);
        void OnInstrumentPrimary(InputAction.CallbackContext context);
        void OnInstrumentSecondary(InputAction.CallbackContext context);
        void OnMovement(InputAction.CallbackContext context);
        void OnBrakes(InputAction.CallbackContext context);
        void OnToggleBombContainer(InputAction.CallbackContext context);
        void OnToggleLaser(InputAction.CallbackContext context);
    }
    public interface IPlayerCameraActions
    {
        void OnMouseCameraMovement(InputAction.CallbackContext context);
    }
}
