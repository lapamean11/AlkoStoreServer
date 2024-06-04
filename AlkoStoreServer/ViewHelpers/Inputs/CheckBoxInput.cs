using AlkoStoreServer.ViewHelpers.Inputs.Interfaces;
using HtmlAgilityPack;

namespace AlkoStoreServer.ViewHelpers.Inputs
{
    public class CheckBoxInput : Input, IInput
    {

        public CheckBoxInput(string name) : base(name) 
        { 

        }

        public CheckBoxInput(string name, string namePrefix) : base(name, namePrefix) 
        { 

        }

        public string Render()
        {
            HtmlDocument doc = new HtmlDocument();

            HtmlNode input = doc.CreateElement("input");
            input.SetAttributeValue("type", "checkbox");
            input.SetAttributeValue("name", _name.Replace(" ", ""));
            input.SetAttributeValue("value", "1");
            input.AddClass("checkbox");

            if (_value != null && Int32.Parse(_value) == 1)
                input.SetAttributeValue("checked", "checked");

            HtmlNode wrapper = doc.CreateElement("div");
            wrapper.AddClass("input-wrapper");

            wrapper.InnerHtml += GetLabel();
            wrapper.InnerHtml += input.OuterHtml;

            _result += wrapper.OuterHtml;
            _result += "<br/>";

            return _result;
        }
    }
}
