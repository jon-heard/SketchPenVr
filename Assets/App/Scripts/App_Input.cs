// GENERATED AUTOMATICALLY FROM 'Assets/App/App_Input.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @App_Input : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @App_Input()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""App_Input"",
    ""maps"": [
        {
            ""name"": ""VrHeadTracking"",
            ""id"": ""614a55e5-9b58-4732-9f7e-723e74fcce5a"",
            ""actions"": [
                {
                    ""name"": ""Position"",
                    ""type"": ""Value"",
                    ""id"": ""cf5c394d-95b6-464b-af14-8f54f40841b5"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rotation"",
                    ""type"": ""Value"",
                    ""id"": ""78f5be0f-7612-470d-a2fc-a52c1941622d"",
                    ""expectedControlType"": ""Quaternion"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4f585168-5696-4f68-b1a4-661409d507d4"",
                    ""path"": ""<XRHMD>/centerEyePosition"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f5bcd636-016e-4192-a720-f9f4f6fb4338"",
                    ""path"": ""<XRHMD>/centerEyeRotation"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""VrLeftHandTracking"",
            ""id"": ""1d66efb8-ee73-4240-b556-025c37ad3244"",
            ""actions"": [
                {
                    ""name"": ""Position"",
                    ""type"": ""Value"",
                    ""id"": ""4d1cd3d5-6c86-41f3-a728-a861ca62b82b"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rotation"",
                    ""type"": ""Value"",
                    ""id"": ""ad4642f9-9c42-4170-8b3f-0fdbb5ee2964"",
                    ""expectedControlType"": ""Quaternion"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1e58e216-42a5-45ac-9cec-c0e263e25630"",
                    ""path"": ""<XRController>{LeftHand}/devicePosition"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8bbf0575-f406-47b1-8cf8-18b724f2a7d5"",
                    ""path"": ""<XRController>{LeftHand}/deviceRotation"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""VrRightHandTracking"",
            ""id"": ""d5521fba-ad4d-42cb-8554-15ab66c82efa"",
            ""actions"": [
                {
                    ""name"": ""Position"",
                    ""type"": ""Value"",
                    ""id"": ""6c483277-deb3-45f3-8d1f-805557de749e"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rotation"",
                    ""type"": ""Value"",
                    ""id"": ""ab1816b7-63cd-4da1-90c3-f813d405b320"",
                    ""expectedControlType"": ""Quaternion"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""23956980-ada0-4f97-8922-c0cc21692c15"",
                    ""path"": ""<XRController>{RightHand}/devicePosition"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f21ee03e-890b-40c9-9060-eb7a5896c4ef"",
                    ""path"": ""<XRController>{RightHand}/deviceRotation"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""VrLeftHandActions"",
            ""id"": ""ffef4156-1892-49b8-a378-8d748d29b2e5"",
            ""actions"": [
                {
                    ""name"": ""Grip"",
                    ""type"": ""Button"",
                    ""id"": ""512fb5c8-ede3-493c-92f5-7bf64a76d64f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TriggerPressure"",
                    ""type"": ""Value"",
                    ""id"": ""d16fcb64-4719-4d06-8d2a-07e8292edf17"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ThumbPressure"",
                    ""type"": ""Value"",
                    ""id"": ""67bcb26a-ea46-42a5-883d-7b4794c05b97"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ThumbDown"",
                    ""type"": ""Button"",
                    ""id"": ""59339095-f694-4254-b04a-ba1d58bd3f44"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ButtonBottom"",
                    ""type"": ""Button"",
                    ""id"": ""0f02e156-0b88-4d07-8973-7d6ad01c176b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ButtonTop"",
                    ""type"": ""Button"",
                    ""id"": ""6e130a93-c473-4797-9ab9-7f36d799590c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e1389d67-0979-43e0-b69e-2b846f43687c"",
                    ""path"": ""<XRController>{LeftHand}/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TriggerPressure"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""33977702-33b6-4cbf-808a-de9bde1fd2ba"",
                    ""path"": ""<XRController>{LeftHand}/thumbstick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ThumbPressure"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c8ea2ccb-bfe9-4e16-820b-6d69fa9a0adb"",
                    ""path"": ""<XRController>{LeftHand}/thumbstickClicked"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ThumbDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0709dcd0-0259-4d80-95b4-83b3c7c68137"",
                    ""path"": ""<XRController>{LeftHand}/gripPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Grip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8443548d-34a1-4baf-84fb-5868646b45f9"",
                    ""path"": ""<XRController>{LeftHand}/primaryButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ButtonBottom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d82c8ab4-c022-4f72-85dc-17bab3126455"",
                    ""path"": ""<XRController>{LeftHand}/secondaryButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ButtonTop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""VrRightHandActions"",
            ""id"": ""3b5abc8c-bf88-481f-8bd4-1f11114cf33b"",
            ""actions"": [
                {
                    ""name"": ""Grip"",
                    ""type"": ""Button"",
                    ""id"": ""52b786da-0d04-4a0d-9a53-f2b0a6bf2dde"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TriggerPressure"",
                    ""type"": ""Value"",
                    ""id"": ""1c916b8f-bc62-47fd-80e2-ff9d31a7cda0"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ThumbPressure"",
                    ""type"": ""Value"",
                    ""id"": ""310f2ffa-6179-44f2-a222-2a38f462759a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ThumbDown"",
                    ""type"": ""Button"",
                    ""id"": ""25c4066a-eaa5-4fd4-a52b-82f3952aed97"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ButtonBottom"",
                    ""type"": ""Button"",
                    ""id"": ""39116559-6497-4679-b12a-fe5faf4184b5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ButtonTop"",
                    ""type"": ""Button"",
                    ""id"": ""388351b9-1944-43a5-8aea-5273dc949634"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""df4acb43-ce48-4e19-8cd3-eba1cc617fa1"",
                    ""path"": ""<XRController>{RightHand}/gripPressed"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Grip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bdd44828-eb10-428c-a673-30ca78d0a06d"",
                    ""path"": ""<XRController>{RightHand}/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TriggerPressure"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""10bfdd91-d16f-4281-8b88-757111b84213"",
                    ""path"": ""<XRController>{RightHand}/thumbstick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ThumbPressure"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4efb81c2-698c-4ae5-9267-adfdcf15c6a5"",
                    ""path"": ""<XRController>{RightHand}/thumbstickClicked"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ThumbDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d06a6b0e-44cf-4a54-b0c0-df9933c705e7"",
                    ""path"": ""<XRController>{RightHand}/primaryButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ButtonBottom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""93ef5d7e-723b-4b5d-bcd6-c028a5a6378b"",
                    ""path"": ""<XRController>{RightHand}/secondaryButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ButtonTop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Mouse"",
            ""id"": ""f5c86e1f-a900-4039-83b1-b0c3b35e3953"",
            ""actions"": [
                {
                    ""name"": ""Position"",
                    ""type"": ""Value"",
                    ""id"": ""41941f57-2c05-4439-a834-5d5ab206b0f1"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LeftButton"",
                    ""type"": ""Button"",
                    ""id"": ""b09d5f0c-4bea-43f8-882a-7ae07207c3f1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f1ce7481-7068-4783-ac47-7e74a45af5ba"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""685ce3cd-1fa0-4a65-8b58-7540399b31fa"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""LeftHandActions"",
            ""bindingGroup"": ""LeftHandActions"",
            ""devices"": []
        }
    ]
}");
        // VrHeadTracking
        m_VrHeadTracking = asset.FindActionMap("VrHeadTracking", throwIfNotFound: true);
        m_VrHeadTracking_Position = m_VrHeadTracking.FindAction("Position", throwIfNotFound: true);
        m_VrHeadTracking_Rotation = m_VrHeadTracking.FindAction("Rotation", throwIfNotFound: true);
        // VrLeftHandTracking
        m_VrLeftHandTracking = asset.FindActionMap("VrLeftHandTracking", throwIfNotFound: true);
        m_VrLeftHandTracking_Position = m_VrLeftHandTracking.FindAction("Position", throwIfNotFound: true);
        m_VrLeftHandTracking_Rotation = m_VrLeftHandTracking.FindAction("Rotation", throwIfNotFound: true);
        // VrRightHandTracking
        m_VrRightHandTracking = asset.FindActionMap("VrRightHandTracking", throwIfNotFound: true);
        m_VrRightHandTracking_Position = m_VrRightHandTracking.FindAction("Position", throwIfNotFound: true);
        m_VrRightHandTracking_Rotation = m_VrRightHandTracking.FindAction("Rotation", throwIfNotFound: true);
        // VrLeftHandActions
        m_VrLeftHandActions = asset.FindActionMap("VrLeftHandActions", throwIfNotFound: true);
        m_VrLeftHandActions_Grip = m_VrLeftHandActions.FindAction("Grip", throwIfNotFound: true);
        m_VrLeftHandActions_TriggerPressure = m_VrLeftHandActions.FindAction("TriggerPressure", throwIfNotFound: true);
        m_VrLeftHandActions_ThumbPressure = m_VrLeftHandActions.FindAction("ThumbPressure", throwIfNotFound: true);
        m_VrLeftHandActions_ThumbDown = m_VrLeftHandActions.FindAction("ThumbDown", throwIfNotFound: true);
        m_VrLeftHandActions_ButtonBottom = m_VrLeftHandActions.FindAction("ButtonBottom", throwIfNotFound: true);
        m_VrLeftHandActions_ButtonTop = m_VrLeftHandActions.FindAction("ButtonTop", throwIfNotFound: true);
        // VrRightHandActions
        m_VrRightHandActions = asset.FindActionMap("VrRightHandActions", throwIfNotFound: true);
        m_VrRightHandActions_Grip = m_VrRightHandActions.FindAction("Grip", throwIfNotFound: true);
        m_VrRightHandActions_TriggerPressure = m_VrRightHandActions.FindAction("TriggerPressure", throwIfNotFound: true);
        m_VrRightHandActions_ThumbPressure = m_VrRightHandActions.FindAction("ThumbPressure", throwIfNotFound: true);
        m_VrRightHandActions_ThumbDown = m_VrRightHandActions.FindAction("ThumbDown", throwIfNotFound: true);
        m_VrRightHandActions_ButtonBottom = m_VrRightHandActions.FindAction("ButtonBottom", throwIfNotFound: true);
        m_VrRightHandActions_ButtonTop = m_VrRightHandActions.FindAction("ButtonTop", throwIfNotFound: true);
        // Mouse
        m_Mouse = asset.FindActionMap("Mouse", throwIfNotFound: true);
        m_Mouse_Position = m_Mouse.FindAction("Position", throwIfNotFound: true);
        m_Mouse_LeftButton = m_Mouse.FindAction("LeftButton", throwIfNotFound: true);
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

    // VrHeadTracking
    private readonly InputActionMap m_VrHeadTracking;
    private IVrHeadTrackingActions m_VrHeadTrackingActionsCallbackInterface;
    private readonly InputAction m_VrHeadTracking_Position;
    private readonly InputAction m_VrHeadTracking_Rotation;
    public struct VrHeadTrackingActions
    {
        private @App_Input m_Wrapper;
        public VrHeadTrackingActions(@App_Input wrapper) { m_Wrapper = wrapper; }
        public InputAction @Position => m_Wrapper.m_VrHeadTracking_Position;
        public InputAction @Rotation => m_Wrapper.m_VrHeadTracking_Rotation;
        public InputActionMap Get() { return m_Wrapper.m_VrHeadTracking; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(VrHeadTrackingActions set) { return set.Get(); }
        public void SetCallbacks(IVrHeadTrackingActions instance)
        {
            if (m_Wrapper.m_VrHeadTrackingActionsCallbackInterface != null)
            {
                @Position.started -= m_Wrapper.m_VrHeadTrackingActionsCallbackInterface.OnPosition;
                @Position.performed -= m_Wrapper.m_VrHeadTrackingActionsCallbackInterface.OnPosition;
                @Position.canceled -= m_Wrapper.m_VrHeadTrackingActionsCallbackInterface.OnPosition;
                @Rotation.started -= m_Wrapper.m_VrHeadTrackingActionsCallbackInterface.OnRotation;
                @Rotation.performed -= m_Wrapper.m_VrHeadTrackingActionsCallbackInterface.OnRotation;
                @Rotation.canceled -= m_Wrapper.m_VrHeadTrackingActionsCallbackInterface.OnRotation;
            }
            m_Wrapper.m_VrHeadTrackingActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Position.started += instance.OnPosition;
                @Position.performed += instance.OnPosition;
                @Position.canceled += instance.OnPosition;
                @Rotation.started += instance.OnRotation;
                @Rotation.performed += instance.OnRotation;
                @Rotation.canceled += instance.OnRotation;
            }
        }
    }
    public VrHeadTrackingActions @VrHeadTracking => new VrHeadTrackingActions(this);

    // VrLeftHandTracking
    private readonly InputActionMap m_VrLeftHandTracking;
    private IVrLeftHandTrackingActions m_VrLeftHandTrackingActionsCallbackInterface;
    private readonly InputAction m_VrLeftHandTracking_Position;
    private readonly InputAction m_VrLeftHandTracking_Rotation;
    public struct VrLeftHandTrackingActions
    {
        private @App_Input m_Wrapper;
        public VrLeftHandTrackingActions(@App_Input wrapper) { m_Wrapper = wrapper; }
        public InputAction @Position => m_Wrapper.m_VrLeftHandTracking_Position;
        public InputAction @Rotation => m_Wrapper.m_VrLeftHandTracking_Rotation;
        public InputActionMap Get() { return m_Wrapper.m_VrLeftHandTracking; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(VrLeftHandTrackingActions set) { return set.Get(); }
        public void SetCallbacks(IVrLeftHandTrackingActions instance)
        {
            if (m_Wrapper.m_VrLeftHandTrackingActionsCallbackInterface != null)
            {
                @Position.started -= m_Wrapper.m_VrLeftHandTrackingActionsCallbackInterface.OnPosition;
                @Position.performed -= m_Wrapper.m_VrLeftHandTrackingActionsCallbackInterface.OnPosition;
                @Position.canceled -= m_Wrapper.m_VrLeftHandTrackingActionsCallbackInterface.OnPosition;
                @Rotation.started -= m_Wrapper.m_VrLeftHandTrackingActionsCallbackInterface.OnRotation;
                @Rotation.performed -= m_Wrapper.m_VrLeftHandTrackingActionsCallbackInterface.OnRotation;
                @Rotation.canceled -= m_Wrapper.m_VrLeftHandTrackingActionsCallbackInterface.OnRotation;
            }
            m_Wrapper.m_VrLeftHandTrackingActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Position.started += instance.OnPosition;
                @Position.performed += instance.OnPosition;
                @Position.canceled += instance.OnPosition;
                @Rotation.started += instance.OnRotation;
                @Rotation.performed += instance.OnRotation;
                @Rotation.canceled += instance.OnRotation;
            }
        }
    }
    public VrLeftHandTrackingActions @VrLeftHandTracking => new VrLeftHandTrackingActions(this);

    // VrRightHandTracking
    private readonly InputActionMap m_VrRightHandTracking;
    private IVrRightHandTrackingActions m_VrRightHandTrackingActionsCallbackInterface;
    private readonly InputAction m_VrRightHandTracking_Position;
    private readonly InputAction m_VrRightHandTracking_Rotation;
    public struct VrRightHandTrackingActions
    {
        private @App_Input m_Wrapper;
        public VrRightHandTrackingActions(@App_Input wrapper) { m_Wrapper = wrapper; }
        public InputAction @Position => m_Wrapper.m_VrRightHandTracking_Position;
        public InputAction @Rotation => m_Wrapper.m_VrRightHandTracking_Rotation;
        public InputActionMap Get() { return m_Wrapper.m_VrRightHandTracking; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(VrRightHandTrackingActions set) { return set.Get(); }
        public void SetCallbacks(IVrRightHandTrackingActions instance)
        {
            if (m_Wrapper.m_VrRightHandTrackingActionsCallbackInterface != null)
            {
                @Position.started -= m_Wrapper.m_VrRightHandTrackingActionsCallbackInterface.OnPosition;
                @Position.performed -= m_Wrapper.m_VrRightHandTrackingActionsCallbackInterface.OnPosition;
                @Position.canceled -= m_Wrapper.m_VrRightHandTrackingActionsCallbackInterface.OnPosition;
                @Rotation.started -= m_Wrapper.m_VrRightHandTrackingActionsCallbackInterface.OnRotation;
                @Rotation.performed -= m_Wrapper.m_VrRightHandTrackingActionsCallbackInterface.OnRotation;
                @Rotation.canceled -= m_Wrapper.m_VrRightHandTrackingActionsCallbackInterface.OnRotation;
            }
            m_Wrapper.m_VrRightHandTrackingActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Position.started += instance.OnPosition;
                @Position.performed += instance.OnPosition;
                @Position.canceled += instance.OnPosition;
                @Rotation.started += instance.OnRotation;
                @Rotation.performed += instance.OnRotation;
                @Rotation.canceled += instance.OnRotation;
            }
        }
    }
    public VrRightHandTrackingActions @VrRightHandTracking => new VrRightHandTrackingActions(this);

    // VrLeftHandActions
    private readonly InputActionMap m_VrLeftHandActions;
    private IVrLeftHandActionsActions m_VrLeftHandActionsActionsCallbackInterface;
    private readonly InputAction m_VrLeftHandActions_Grip;
    private readonly InputAction m_VrLeftHandActions_TriggerPressure;
    private readonly InputAction m_VrLeftHandActions_ThumbPressure;
    private readonly InputAction m_VrLeftHandActions_ThumbDown;
    private readonly InputAction m_VrLeftHandActions_ButtonBottom;
    private readonly InputAction m_VrLeftHandActions_ButtonTop;
    public struct VrLeftHandActionsActions
    {
        private @App_Input m_Wrapper;
        public VrLeftHandActionsActions(@App_Input wrapper) { m_Wrapper = wrapper; }
        public InputAction @Grip => m_Wrapper.m_VrLeftHandActions_Grip;
        public InputAction @TriggerPressure => m_Wrapper.m_VrLeftHandActions_TriggerPressure;
        public InputAction @ThumbPressure => m_Wrapper.m_VrLeftHandActions_ThumbPressure;
        public InputAction @ThumbDown => m_Wrapper.m_VrLeftHandActions_ThumbDown;
        public InputAction @ButtonBottom => m_Wrapper.m_VrLeftHandActions_ButtonBottom;
        public InputAction @ButtonTop => m_Wrapper.m_VrLeftHandActions_ButtonTop;
        public InputActionMap Get() { return m_Wrapper.m_VrLeftHandActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(VrLeftHandActionsActions set) { return set.Get(); }
        public void SetCallbacks(IVrLeftHandActionsActions instance)
        {
            if (m_Wrapper.m_VrLeftHandActionsActionsCallbackInterface != null)
            {
                @Grip.started -= m_Wrapper.m_VrLeftHandActionsActionsCallbackInterface.OnGrip;
                @Grip.performed -= m_Wrapper.m_VrLeftHandActionsActionsCallbackInterface.OnGrip;
                @Grip.canceled -= m_Wrapper.m_VrLeftHandActionsActionsCallbackInterface.OnGrip;
                @TriggerPressure.started -= m_Wrapper.m_VrLeftHandActionsActionsCallbackInterface.OnTriggerPressure;
                @TriggerPressure.performed -= m_Wrapper.m_VrLeftHandActionsActionsCallbackInterface.OnTriggerPressure;
                @TriggerPressure.canceled -= m_Wrapper.m_VrLeftHandActionsActionsCallbackInterface.OnTriggerPressure;
                @ThumbPressure.started -= m_Wrapper.m_VrLeftHandActionsActionsCallbackInterface.OnThumbPressure;
                @ThumbPressure.performed -= m_Wrapper.m_VrLeftHandActionsActionsCallbackInterface.OnThumbPressure;
                @ThumbPressure.canceled -= m_Wrapper.m_VrLeftHandActionsActionsCallbackInterface.OnThumbPressure;
                @ThumbDown.started -= m_Wrapper.m_VrLeftHandActionsActionsCallbackInterface.OnThumbDown;
                @ThumbDown.performed -= m_Wrapper.m_VrLeftHandActionsActionsCallbackInterface.OnThumbDown;
                @ThumbDown.canceled -= m_Wrapper.m_VrLeftHandActionsActionsCallbackInterface.OnThumbDown;
                @ButtonBottom.started -= m_Wrapper.m_VrLeftHandActionsActionsCallbackInterface.OnButtonBottom;
                @ButtonBottom.performed -= m_Wrapper.m_VrLeftHandActionsActionsCallbackInterface.OnButtonBottom;
                @ButtonBottom.canceled -= m_Wrapper.m_VrLeftHandActionsActionsCallbackInterface.OnButtonBottom;
                @ButtonTop.started -= m_Wrapper.m_VrLeftHandActionsActionsCallbackInterface.OnButtonTop;
                @ButtonTop.performed -= m_Wrapper.m_VrLeftHandActionsActionsCallbackInterface.OnButtonTop;
                @ButtonTop.canceled -= m_Wrapper.m_VrLeftHandActionsActionsCallbackInterface.OnButtonTop;
            }
            m_Wrapper.m_VrLeftHandActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Grip.started += instance.OnGrip;
                @Grip.performed += instance.OnGrip;
                @Grip.canceled += instance.OnGrip;
                @TriggerPressure.started += instance.OnTriggerPressure;
                @TriggerPressure.performed += instance.OnTriggerPressure;
                @TriggerPressure.canceled += instance.OnTriggerPressure;
                @ThumbPressure.started += instance.OnThumbPressure;
                @ThumbPressure.performed += instance.OnThumbPressure;
                @ThumbPressure.canceled += instance.OnThumbPressure;
                @ThumbDown.started += instance.OnThumbDown;
                @ThumbDown.performed += instance.OnThumbDown;
                @ThumbDown.canceled += instance.OnThumbDown;
                @ButtonBottom.started += instance.OnButtonBottom;
                @ButtonBottom.performed += instance.OnButtonBottom;
                @ButtonBottom.canceled += instance.OnButtonBottom;
                @ButtonTop.started += instance.OnButtonTop;
                @ButtonTop.performed += instance.OnButtonTop;
                @ButtonTop.canceled += instance.OnButtonTop;
            }
        }
    }
    public VrLeftHandActionsActions @VrLeftHandActions => new VrLeftHandActionsActions(this);

    // VrRightHandActions
    private readonly InputActionMap m_VrRightHandActions;
    private IVrRightHandActionsActions m_VrRightHandActionsActionsCallbackInterface;
    private readonly InputAction m_VrRightHandActions_Grip;
    private readonly InputAction m_VrRightHandActions_TriggerPressure;
    private readonly InputAction m_VrRightHandActions_ThumbPressure;
    private readonly InputAction m_VrRightHandActions_ThumbDown;
    private readonly InputAction m_VrRightHandActions_ButtonBottom;
    private readonly InputAction m_VrRightHandActions_ButtonTop;
    public struct VrRightHandActionsActions
    {
        private @App_Input m_Wrapper;
        public VrRightHandActionsActions(@App_Input wrapper) { m_Wrapper = wrapper; }
        public InputAction @Grip => m_Wrapper.m_VrRightHandActions_Grip;
        public InputAction @TriggerPressure => m_Wrapper.m_VrRightHandActions_TriggerPressure;
        public InputAction @ThumbPressure => m_Wrapper.m_VrRightHandActions_ThumbPressure;
        public InputAction @ThumbDown => m_Wrapper.m_VrRightHandActions_ThumbDown;
        public InputAction @ButtonBottom => m_Wrapper.m_VrRightHandActions_ButtonBottom;
        public InputAction @ButtonTop => m_Wrapper.m_VrRightHandActions_ButtonTop;
        public InputActionMap Get() { return m_Wrapper.m_VrRightHandActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(VrRightHandActionsActions set) { return set.Get(); }
        public void SetCallbacks(IVrRightHandActionsActions instance)
        {
            if (m_Wrapper.m_VrRightHandActionsActionsCallbackInterface != null)
            {
                @Grip.started -= m_Wrapper.m_VrRightHandActionsActionsCallbackInterface.OnGrip;
                @Grip.performed -= m_Wrapper.m_VrRightHandActionsActionsCallbackInterface.OnGrip;
                @Grip.canceled -= m_Wrapper.m_VrRightHandActionsActionsCallbackInterface.OnGrip;
                @TriggerPressure.started -= m_Wrapper.m_VrRightHandActionsActionsCallbackInterface.OnTriggerPressure;
                @TriggerPressure.performed -= m_Wrapper.m_VrRightHandActionsActionsCallbackInterface.OnTriggerPressure;
                @TriggerPressure.canceled -= m_Wrapper.m_VrRightHandActionsActionsCallbackInterface.OnTriggerPressure;
                @ThumbPressure.started -= m_Wrapper.m_VrRightHandActionsActionsCallbackInterface.OnThumbPressure;
                @ThumbPressure.performed -= m_Wrapper.m_VrRightHandActionsActionsCallbackInterface.OnThumbPressure;
                @ThumbPressure.canceled -= m_Wrapper.m_VrRightHandActionsActionsCallbackInterface.OnThumbPressure;
                @ThumbDown.started -= m_Wrapper.m_VrRightHandActionsActionsCallbackInterface.OnThumbDown;
                @ThumbDown.performed -= m_Wrapper.m_VrRightHandActionsActionsCallbackInterface.OnThumbDown;
                @ThumbDown.canceled -= m_Wrapper.m_VrRightHandActionsActionsCallbackInterface.OnThumbDown;
                @ButtonBottom.started -= m_Wrapper.m_VrRightHandActionsActionsCallbackInterface.OnButtonBottom;
                @ButtonBottom.performed -= m_Wrapper.m_VrRightHandActionsActionsCallbackInterface.OnButtonBottom;
                @ButtonBottom.canceled -= m_Wrapper.m_VrRightHandActionsActionsCallbackInterface.OnButtonBottom;
                @ButtonTop.started -= m_Wrapper.m_VrRightHandActionsActionsCallbackInterface.OnButtonTop;
                @ButtonTop.performed -= m_Wrapper.m_VrRightHandActionsActionsCallbackInterface.OnButtonTop;
                @ButtonTop.canceled -= m_Wrapper.m_VrRightHandActionsActionsCallbackInterface.OnButtonTop;
            }
            m_Wrapper.m_VrRightHandActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Grip.started += instance.OnGrip;
                @Grip.performed += instance.OnGrip;
                @Grip.canceled += instance.OnGrip;
                @TriggerPressure.started += instance.OnTriggerPressure;
                @TriggerPressure.performed += instance.OnTriggerPressure;
                @TriggerPressure.canceled += instance.OnTriggerPressure;
                @ThumbPressure.started += instance.OnThumbPressure;
                @ThumbPressure.performed += instance.OnThumbPressure;
                @ThumbPressure.canceled += instance.OnThumbPressure;
                @ThumbDown.started += instance.OnThumbDown;
                @ThumbDown.performed += instance.OnThumbDown;
                @ThumbDown.canceled += instance.OnThumbDown;
                @ButtonBottom.started += instance.OnButtonBottom;
                @ButtonBottom.performed += instance.OnButtonBottom;
                @ButtonBottom.canceled += instance.OnButtonBottom;
                @ButtonTop.started += instance.OnButtonTop;
                @ButtonTop.performed += instance.OnButtonTop;
                @ButtonTop.canceled += instance.OnButtonTop;
            }
        }
    }
    public VrRightHandActionsActions @VrRightHandActions => new VrRightHandActionsActions(this);

    // Mouse
    private readonly InputActionMap m_Mouse;
    private IMouseActions m_MouseActionsCallbackInterface;
    private readonly InputAction m_Mouse_Position;
    private readonly InputAction m_Mouse_LeftButton;
    public struct MouseActions
    {
        private @App_Input m_Wrapper;
        public MouseActions(@App_Input wrapper) { m_Wrapper = wrapper; }
        public InputAction @Position => m_Wrapper.m_Mouse_Position;
        public InputAction @LeftButton => m_Wrapper.m_Mouse_LeftButton;
        public InputActionMap Get() { return m_Wrapper.m_Mouse; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MouseActions set) { return set.Get(); }
        public void SetCallbacks(IMouseActions instance)
        {
            if (m_Wrapper.m_MouseActionsCallbackInterface != null)
            {
                @Position.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnPosition;
                @Position.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnPosition;
                @Position.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnPosition;
                @LeftButton.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnLeftButton;
                @LeftButton.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnLeftButton;
                @LeftButton.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnLeftButton;
            }
            m_Wrapper.m_MouseActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Position.started += instance.OnPosition;
                @Position.performed += instance.OnPosition;
                @Position.canceled += instance.OnPosition;
                @LeftButton.started += instance.OnLeftButton;
                @LeftButton.performed += instance.OnLeftButton;
                @LeftButton.canceled += instance.OnLeftButton;
            }
        }
    }
    public MouseActions @Mouse => new MouseActions(this);
    private int m_LeftHandActionsSchemeIndex = -1;
    public InputControlScheme LeftHandActionsScheme
    {
        get
        {
            if (m_LeftHandActionsSchemeIndex == -1) m_LeftHandActionsSchemeIndex = asset.FindControlSchemeIndex("LeftHandActions");
            return asset.controlSchemes[m_LeftHandActionsSchemeIndex];
        }
    }
    public interface IVrHeadTrackingActions
    {
        void OnPosition(InputAction.CallbackContext context);
        void OnRotation(InputAction.CallbackContext context);
    }
    public interface IVrLeftHandTrackingActions
    {
        void OnPosition(InputAction.CallbackContext context);
        void OnRotation(InputAction.CallbackContext context);
    }
    public interface IVrRightHandTrackingActions
    {
        void OnPosition(InputAction.CallbackContext context);
        void OnRotation(InputAction.CallbackContext context);
    }
    public interface IVrLeftHandActionsActions
    {
        void OnGrip(InputAction.CallbackContext context);
        void OnTriggerPressure(InputAction.CallbackContext context);
        void OnThumbPressure(InputAction.CallbackContext context);
        void OnThumbDown(InputAction.CallbackContext context);
        void OnButtonBottom(InputAction.CallbackContext context);
        void OnButtonTop(InputAction.CallbackContext context);
    }
    public interface IVrRightHandActionsActions
    {
        void OnGrip(InputAction.CallbackContext context);
        void OnTriggerPressure(InputAction.CallbackContext context);
        void OnThumbPressure(InputAction.CallbackContext context);
        void OnThumbDown(InputAction.CallbackContext context);
        void OnButtonBottom(InputAction.CallbackContext context);
        void OnButtonTop(InputAction.CallbackContext context);
    }
    public interface IMouseActions
    {
        void OnPosition(InputAction.CallbackContext context);
        void OnLeftButton(InputAction.CallbackContext context);
    }
}
