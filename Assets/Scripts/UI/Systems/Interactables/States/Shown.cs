using System.Collections;

namespace UI.Systems.Interactables.States
{
    public class Shown : InteractableState
    {
        public Shown(Interactable interactable) : base(interactable)
        {
        }
        
        public override IEnumerator UpdateDisplay()
        {
            Interactable.objectIcon.SetActive(true);
            Interactable.bindIcon.SetActive(false);

            return base.UpdateDisplay();
        }
    }
}