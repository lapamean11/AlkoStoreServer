using AlkoStoreServer.ViewHelpers.Inputs.Interfaces;
using HtmlAgilityPack;

namespace AlkoStoreServer.ViewHelpers.Inputs
{
    public class TextInput : IInput
    {
        private dynamic _value;

        private string _result = string.Empty;

        private string _name;

        private string _prefixName = null;

        public TextInput(
            string name
        ) {
            _name = name;
        }

        public TextInput(
            string name,
            string namePrefix
        )
        {
            _name = name;
            _prefixName = namePrefix;
        }

        public void SetValue(dynamic value)
        { 
            _value = value;
        }

        private string GetLabel()
        {
            if (_prefixName != null) 
            { 
                return "<label for=" + _name.Replace(" ", "") + ">" + _prefixName + "</label>";
            }

            return "<label for=" + _name.Replace(" ", "") + ">" + _name + "</label>";
        }

        public string Render()
        {
            //_result += GetLabel();

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
            //_result += "<br/>";

            return _result;
        }
    }
}
