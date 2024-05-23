using AlkoStoreServer.ViewHelpers.Inputs.Interfaces;
using HtmlAgilityPack;

namespace AlkoStoreServer.ViewHelpers.Inputs
{
    public class CheckBoxInput : IInput
    {
        private dynamic _value;

        private string _result = string.Empty;

        private string _name;

        public CheckBoxInput(
            string name
        ) {
            _name = name;
        }

        public CheckBoxInput(
            string name,
            string namePrefix
        )
        {
            _name = namePrefix + "." + name;
        }

        public void SetValue(dynamic value)
        {
            _value = value;
        }

        private string GetLabel()
        {
            return "<label for=" + _name.Replace(" ", "") + ">" + _name + "</label>";
        }

        public string Render()
        {
            _result += GetLabel();

            HtmlDocument doc = new HtmlDocument();

            HtmlNode input = doc.CreateElement("input");
            HtmlNode hiddenInput = doc.CreateElement("input");

            hiddenInput.SetAttributeValue("type", "hidden");
            hiddenInput.SetAttributeValue("name", _name.Replace(" ", ""));
            hiddenInput.SetAttributeValue("value", Int32.Parse(_value) == 1 ? "1" : "0");
            hiddenInput.SetAttributeValue("id", _name.Replace(" ", ""));

            input.SetAttributeValue("type", "checkbox");
            input.SetAttributeValue("name", _name.Replace(" ", ""));
            input.SetAttributeValue("value", Int32.Parse(_value) == 1 ? "1" : "0");
            input.SetAttributeValue("id", _name.Replace(" ", ""));

            if (Int32.Parse(_value) == 1)
                input.SetAttributeValue("checked", "checked");

            HtmlNode wrapper = doc.CreateElement("div");
            wrapper.AddClass("input-wrapper");

            wrapper.InnerHtml += hiddenInput.OuterHtml;
            wrapper.InnerHtml += input.OuterHtml;

            _result += wrapper.OuterHtml;
            _result += "<br/>";

            return _result;
        }
    }
}
