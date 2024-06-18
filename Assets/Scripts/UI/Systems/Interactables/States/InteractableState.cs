using System.Collections;

namespace UI.Systems.Interactables.States
{
    public abstract class InteractableState
    {
        protected readonly Interactable Interactable;

        protected InteractableState(Interactable interactable)
        {
            Interactable = interactable;
        }

        public virtual IEnumerator UpdateDisplay()
        {
            yield break;
        }
    }
}