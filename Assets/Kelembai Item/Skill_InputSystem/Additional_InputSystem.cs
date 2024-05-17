//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.0
//     from Assets/Kelembai Item/Skill_InputSystem/Additional_InputSystem.inputactions
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

public partial class @Additional_InputSystem: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Additional_InputSystem()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Additional_InputSystem"",
    ""maps"": [
        {
            ""name"": ""Skill"",
            ""id"": ""a0fb37e0-4593-44df-b677-b208e747c0f1"",
            ""actions"": [
                {
                    ""name"": ""SupportShip"",
                    ""type"": ""Button"",
                    ""id"": ""f996e351-3f81-49db-ba1c-a9d1bc971eb5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""797d86ce-daa9-4930-b19d-cd3b2fd113e5"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""SkillControl"",
                    ""action"": ""SupportShip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""weapon"",
            ""id"": ""f3044b7b-7e0a-428d-a0ff-dc2a01501bd9"",
            ""actions"": [
                {
                    ""name"": ""Tertier Weapon"",
                    ""type"": ""Button"",
                    ""id"": ""40f30849-406d-4a19-832a-02db5f75a86c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e369c411-a1c8-4062-a23e-72cb2a9e73eb"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Weapon"",
                    ""action"": ""Tertier Weapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""SkillControl"",
            ""bindingGroup"": ""SkillControl"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Weapon"",
            ""bindingGroup"": ""Weapon"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Skill
        m_Skill = asset.FindActionMap("Skill", throwIfNotFound: true);
        m_Skill_SupportShip = m_Skill.FindAction("SupportShip", throwIfNotFound: true);
        // weapon
        m_weapon = asset.FindActionMap("weapon", throwIfNotFound: true);
        m_weapon_TertierWeapon = m_weapon.FindAction("Tertier Weapon", throwIfNotFound: true);
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

    // Skill
    private readonly InputActionMap m_Skill;
    private List<ISkillActions> m_SkillActionsCallbackInterfaces = new List<ISkillActions>();
    private readonly InputAction m_Skill_SupportShip;
    public struct SkillActions
    {
        private @Additional_InputSystem m_Wrapper;
        public SkillActions(@Additional_InputSystem wrapper) { m_Wrapper = wrapper; }
        public InputAction @SupportShip => m_Wrapper.m_Skill_SupportShip;
        public InputActionMap Get() { return m_Wrapper.m_Skill; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SkillActions set) { return set.Get(); }
        public void AddCallbacks(ISkillActions instance)
        {
            if (instance == null || m_Wrapper.m_SkillActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_SkillActionsCallbackInterfaces.Add(instance);
            @SupportShip.started += instance.OnSupportShip;
            @SupportShip.performed += instance.OnSupportShip;
            @SupportShip.canceled += instance.OnSupportShip;
        }

        private void UnregisterCallbacks(ISkillActions instance)
        {
            @SupportShip.started -= instance.OnSupportShip;
            @SupportShip.performed -= instance.OnSupportShip;
            @SupportShip.canceled -= instance.OnSupportShip;
        }

        public void RemoveCallbacks(ISkillActions instance)
        {
            if (m_Wrapper.m_SkillActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ISkillActions instance)
        {
            foreach (var item in m_Wrapper.m_SkillActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_SkillActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public SkillActions @Skill => new SkillActions(this);

    // weapon
    private readonly InputActionMap m_weapon;
    private List<IWeaponActions> m_WeaponActionsCallbackInterfaces = new List<IWeaponActions>();
    private readonly InputAction m_weapon_TertierWeapon;
    public struct WeaponActions
    {
        private @Additional_InputSystem m_Wrapper;
        public WeaponActions(@Additional_InputSystem wrapper) { m_Wrapper = wrapper; }
        public InputAction @TertierWeapon => m_Wrapper.m_weapon_TertierWeapon;
        public InputActionMap Get() { return m_Wrapper.m_weapon; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(WeaponActions set) { return set.Get(); }
        public void AddCallbacks(IWeaponActions instance)
        {
            if (instance == null || m_Wrapper.m_WeaponActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_WeaponActionsCallbackInterfaces.Add(instance);
            @TertierWeapon.started += instance.OnTertierWeapon;
            @TertierWeapon.performed += instance.OnTertierWeapon;
            @TertierWeapon.canceled += instance.OnTertierWeapon;
        }

        private void UnregisterCallbacks(IWeaponActions instance)
        {
            @TertierWeapon.started -= instance.OnTertierWeapon;
            @TertierWeapon.performed -= instance.OnTertierWeapon;
            @TertierWeapon.canceled -= instance.OnTertierWeapon;
        }

        public void RemoveCallbacks(IWeaponActions instance)
        {
            if (m_Wrapper.m_WeaponActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IWeaponActions instance)
        {
            foreach (var item in m_Wrapper.m_WeaponActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_WeaponActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public WeaponActions @weapon => new WeaponActions(this);
    private int m_SkillControlSchemeIndex = -1;
    public InputControlScheme SkillControlScheme
    {
        get
        {
            if (m_SkillControlSchemeIndex == -1) m_SkillControlSchemeIndex = asset.FindControlSchemeIndex("SkillControl");
            return asset.controlSchemes[m_SkillControlSchemeIndex];
        }
    }
    private int m_WeaponSchemeIndex = -1;
    public InputControlScheme WeaponScheme
    {
        get
        {
            if (m_WeaponSchemeIndex == -1) m_WeaponSchemeIndex = asset.FindControlSchemeIndex("Weapon");
            return asset.controlSchemes[m_WeaponSchemeIndex];
        }
    }
    public interface ISkillActions
    {
        void OnSupportShip(InputAction.CallbackContext context);
    }
    public interface IWeaponActions
    {
        void OnTertierWeapon(InputAction.CallbackContext context);
    }
}
