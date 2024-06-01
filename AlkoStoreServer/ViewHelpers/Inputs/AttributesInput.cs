using AlkoStoreServer.ViewHelpers.Inputs.Interfaces;
using HtmlAgilityPack;
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
            //_result += GetLabel();

            HtmlDocument doc = new HtmlDocument();

            int counter = 0;
            foreach (var item in _value)
            {
                var name = _name + "[" + counter + "]" + ".AttributeId"; //item.Attribute.ID
                /*var name2 = _name + "[" + counter + "]" + ".CategoryId";*/

                /*_result += "<input type=" + '"' + "hidden" + '"' + " name=" + '"' + name2 + '"' + " value=" + '"' + "0" + '"' + "/>";*/
                _result += "<input type=" + '"' + "hidden" + '"' + 
                                  " name=" + '"' + name + '"' + 
                                  " value=" + '"' + item.Attribute.ID + '"' + "/>";
                var input = (IInput)Activator.CreateInstance(
                    _attributeMap[item.Attribute.AttributeType.Type],
                    _name + "[" + counter + "]" + ".Value",
                    item.Attribute.Name

                //item.Attribute.Name,
                //_name
                );

                input.SetValue( item.Value );

                _result += input.Render();
                counter++;
            }

            HtmlNode wrapper = doc.CreateElement("div");
            wrapper.AddClass("attributes-input-wrapper");

            wrapper.InnerHtml += GetLabel();
            wrapper.InnerHtml += _result;

            return wrapper.OuterHtml;
        }

        public void SetValue(dynamic value)
        {
            _value = value;
        }
    }
}
