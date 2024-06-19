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
            Interactable.objectIconImage.enabled = true;
            Interactable.bindIconImage.enabled = false;

            return base.UpdateDisplay();
        }
    }
}