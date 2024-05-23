namespace AlkoStoreServer.ViewHelpers.Inputs.Interfaces
{
    public interface IInput
    {
        public string Render();

        public void SetValue(dynamic value);
    }
}
