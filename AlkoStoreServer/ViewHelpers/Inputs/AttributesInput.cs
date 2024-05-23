using AlkoStoreServer.ViewHelpers.Inputs.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AlkoStoreServer.ViewHelpers.Inputs
{
    public class AttributesInput : IInput
    {
        private dynamic _value;

        private string _result = string.Empty;

        private string _name;

        public static IDictionary<string, Type> _attributeMap = new Dictionary<string, Type>
        {
            { "string", typeof(TextInput) },
            { "bool", typeof(CheckBoxInput) },
            { "text", typeof(TextInput) },
            { "int", typeof(TextInput) },
        };

        public AttributesInput(
            string name
        )
        {
            _name = name;
        }

        private string GetLabel()
        {
            return "<br/><label for=" + _name + ">" + _name + "</label><br/>";
        }

        public string Render()
        {
            _result += GetLabel();

            foreach (var item in _value)
            {
                var input = (IInput)Activator.CreateInstance(
                    _attributeMap[item.Attribute.AttributeType.Type],
                    item.Attribute.Name,
                    _name
                );

                input.SetValue( item.Value );

                _result += input.Render();
            }

            return _result;
        }

        public void SetValue(dynamic value)
        {
            _value = value;
        }
    }
}
