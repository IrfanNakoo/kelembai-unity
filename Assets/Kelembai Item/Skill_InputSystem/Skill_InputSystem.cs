using UnityEngine;
using UnityEngine.InputSystem;

public class Skill_InputSystem : MonoBehaviour
{
    public InputActionReference skill;

    private void OnEnable()
    {
        skill.action.started += SkillStarted;
    }

    private void OnDisable()
    {
        skill.action.started -= SkillStarted;
    }

    private void SkillStarted(InputAction.CallbackContext context)
    {
        Debug.Log("Skill activated");
    }
}