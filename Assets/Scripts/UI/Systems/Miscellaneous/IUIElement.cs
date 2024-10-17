namespace UI.Systems.Miscellaneous
{
    public interface IUIElement
    {
        public void Initialize();
        
        public void UpdateElement();

        public void CloseElement();
    }
}