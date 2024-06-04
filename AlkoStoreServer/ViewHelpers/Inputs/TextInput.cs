using AlkoStoreServer.ViewHelpers.Inputs.Interfaces;
using HtmlAgilityPack;

namespace AlkoStoreServer.ViewHelpers.Inputs
{
    public class TextInput : Input, IInput
    {
        public TextInput(
            string name
        ) : base (name)
        {

        }

        public TextInput(
            string name,
            string namePrefix
        ) : base (name, namePrefix)
        {

        }

        public string Render()
        {
            HtmlDocument doc = new HtmlDocument();

            HtmlNode input = doc.CreateElement("input");
            input.SetAttributeValue("type", "text");

            if (_value != null)
                input.SetAttributeValue("value", _value.ToString());

            input.SetAttributeValue("name", _name.Replace(" ", ""));

            HtmlNode wrapper = doc.CreateElement("div");
            wrapper.AddClass("input-wrapper");

            wrapper.InnerHtml += GetLabel();
            wrapper.InnerHtml += input.OuterHtml;

            _result += wrapper.OuterHtml;

            return _result;
        }
    }
}
