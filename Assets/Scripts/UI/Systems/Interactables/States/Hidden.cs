using System.Collections;

namespace UI.Systems.Interactables.States
{
    public class Hidden : InteractableState
    {
        public Hidden(Interactable interactable) : base(interactable)
        {
        }

        public override IEnumerator UpdateDisplay()
        {
            Interactable.objectIcon.SetActive(false);
            Interactable.bindIcon.SetActive(false);

            return base.UpdateDisplay();
        }
    }
}