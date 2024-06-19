using System.Collections;

namespace UI.Systems.Interactables.States
{
    public class Activated : InteractableState
    {
        public Activated(Interactable interactable) : base(interactable)
        {
        }
        
        public override IEnumerator UpdateDisplay()
        {
            Interactable.objectIconImage.enabled = false;
            Interactable.bindIconImage.enabled = false;

            return base.UpdateDisplay();
        }
    }
}