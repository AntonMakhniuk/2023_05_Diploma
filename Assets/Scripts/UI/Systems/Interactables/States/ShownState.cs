namespace UI.Systems.Interactables.States
{
    public class ShownState : BaseInteractableState
    {
        private const float ShownIconOpacity = 0.5f;
        
        public ShownState(Interactable context) : base(context)
        {
        }

        public override void Enter()
        {
            Context.objectIconImage.enabled = true;

            var imageColor = Context.objectIconImage.color;
            imageColor.a = ShownIconOpacity;

            Context.objectIconImage.color = imageColor;

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