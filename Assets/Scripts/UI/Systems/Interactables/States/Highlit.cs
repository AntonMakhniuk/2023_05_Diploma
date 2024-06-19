using System.Collections;

namespace UI.Systems.Interactables.States
{
    public class Highlit : InteractableState
    {
        public Highlit(Interactable interactable) : base(interactable)
        {
        }
        
        public override IEnumerator UpdateDisplay()
        {
            Interactable.objectIconImage.enabled = true;
            Interactable.bindIconImage.enabled = true;

            return base.UpdateDisplay();
        }
    }
}