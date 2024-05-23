using AlkoStoreServer.ViewHelpers.Inputs.Interfaces;

namespace AlkoStoreServer.ViewHelpers.Renderer
{
    public class InputRenderer
    {
        private IInput? _input;

        public InputRenderer(IInput input)
        {
            _input = input;
        }

        public string Render()
        {
            if (_input is null) return string.Empty;

            return _input.Render();
        }
    }
}
