using AlkoStoreServer.ViewHelpers.Inputs.Interfaces;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AlkoStoreServer.ViewHelpers.Inputs
{
    public class AttributesInput : Input, IInput
    {
        private static IDictionary<string, Type> _attributeMap = new Dictionary<string, Type>
        {
            { "string", typeof(TextInput) },
            { "bool", typeof(CheckBoxInput) },
            { "text", typeof(TextInput) },
            { "int", typeof(TextInput) },
        };

        public AttributesInput(
            string name
        ) : base( name )
        {

        }

        public AttributesInput(
            string name,
            string namePrefix
        ) : base(name, namePrefix)
        {

        }

        public string Render()
        {
            HtmlDocument doc = new HtmlDocument();

            int counter = 0;
            foreach (var item in _value)
            {
                var name = _name + "[" + counter + "]" + ".AttributeId"; //item.Attribute.ID

                _result += "<input type=" + '"' + "hidden" + '"' + 
                                  " name=" + '"' + name + '"' + 
                                  " value=" + '"' + item.Attribute.ID + '"' + "/>";
                var input = (IInput)Activator.CreateInstance(
                    _attributeMap[item.Attribute.AttributeType.Type],
                    _name + "[" + counter + "]" + ".Value",
                    item.Attribute.Name
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
    }
}
