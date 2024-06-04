namespace UI.Systems
{
    public interface IUIElement
    {
        public void Initialize();
        
        public void UpdateElement();

        public void CloseElement();
    }
}