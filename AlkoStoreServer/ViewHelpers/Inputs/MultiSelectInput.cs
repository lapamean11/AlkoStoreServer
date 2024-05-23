using AlkoStoreServer.Base;
using AlkoStoreServer.ViewHelpers.Inputs.Interfaces;
using HtmlAgilityPack;

namespace AlkoStoreServer.ViewHelpers.Inputs
{
    public class MultiSelectInput : ISelectInput
    {
        private dynamic _value;

        private string _result = string.Empty;

        private string _name;

        private List<Model> _selectData;

        public MultiSelectInput(string name)
        {
            _name = name;
        }

        public void SetSelectData(List<Model> selectData)
        { 
            _selectData = selectData;
        }

        private string GetLabel()
        {
            return "<label for=" + _name.Replace(" ", "") + ">" + _name + "</label>";
        }

        public string Render()
        {
            _result = GetLabel();

            List<string> selected = new List<string>();
            foreach (var item in _value)
            { 
                var id = item.GetType().GetProperty("ID").GetValue(item, null);
                selected.Add(id.ToString());
            }

            HtmlDocument doc = new HtmlDocument();
            HtmlNode Select = doc.CreateElement("select");
            Select.SetAttributeValue("name", _name);
            Select.Attributes.Add("multiple", "multiple");

            foreach (var item in _selectData)
            {
                var optionElement = doc.CreateElement("option");
                var id = item.GetType().GetProperty("ID").GetValue(item, null);
                var name = item.GetType().GetProperty("Name").GetValue(item, null);

                optionElement.Attributes.Add("value", id.ToString());
                optionElement.InnerHtml = name.ToString();

                if (selected.Contains(id.ToString()))
                {
                    optionElement.Attributes.Add("selected", "selected");
                }

                Select.AppendChild(optionElement);
            }

            HtmlNode wrapper = doc.CreateElement("div");
            wrapper.AddClass("input-wrapper");

            wrapper.AppendChild(Select);

            _result += wrapper.OuterHtml;

            return _result;
        }

        public void SetValue(dynamic value)
        {
            _value = value;
        }
    }
}
