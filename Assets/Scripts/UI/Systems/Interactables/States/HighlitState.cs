namespace UI.Systems.Interactables.States
{
    public class HighlitState : BaseInteractableState
    {
        private const float HighlitImageColorAlpha = 1f;
        
        public HighlitState(Interactable interactable) : base(interactable)
        {
        }

        public override void Enter()
        {
            Context.objectIconImage.enabled = true;

            var imageColor = Context.objectIconImage.color;
            imageColor.a = HighlitImageColorAlpha;

            Context.objectIconImage.color = imageColor;

            Context.bindIconImage.enabled = true;
            Context.bindIconImage.color = imageColor;
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