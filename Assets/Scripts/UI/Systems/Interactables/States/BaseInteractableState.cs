namespace UI.Systems.Interactables.States
{
    public abstract class BaseInteractableState
    {
        protected readonly Interactable Context;

        protected BaseInteractableState(Interactable context)
        {
            Context = context;
        }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }

    public enum InteractableState
    {
        Hidden, Shown, Highlit, Activated
    }
}