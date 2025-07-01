using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverAnimator : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (animator != null)
        {
            animator.SetBool("Hover", true);
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (animator != null)
        {
            animator.SetBool("Hover", false);
        }
    }
}
