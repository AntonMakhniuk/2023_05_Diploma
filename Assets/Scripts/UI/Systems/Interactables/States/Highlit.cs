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
            Interactable.objectIcon.SetActive(true);
            Interactable.bindIcon.SetActive(true);

            return base.UpdateDisplay();
        }
    }
}