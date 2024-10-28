using System.Collections;

namespace UI.Systems.Interactables.States
{
    public class HiddenState : BaseInteractableState
    {
        public HiddenState(Interactable interactable) : base(interactable)
        {
        }

        public override void Enter()
        {
            Context.objectIconImage.enabled = false;
            Context.bindIconImage.enabled = false;
        }

        public override void Update()
        {
            // No special behavior
        }

        public override void Exit()
        {
            // No special behavior
        }
    }
}