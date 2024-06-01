using AlkoStoreServer.Base;
using AlkoStoreServer.ViewHelpers.Inputs.Interfaces;
using HtmlAgilityPack;

namespace AlkoStoreServer.ViewHelpers.Inputs
{
    public class SelectInput : ISelectInput
    {
        private dynamic _value;

        private string _result = string.Empty;

        private string _name;

        private List<Model> _selectData;

        public SelectInput(string name)
        {
            _name = name;
        }

        private string GetLabel()
        {
            return "<label for=" + _name.Replace(" ", "") + ">" + _name + "</label>";
        }

        public string Render()
        {
            int selectedId = 0;

            if (_value != null)
                selectedId = _value.ID;

            HtmlDocument doc = new HtmlDocument();
            HtmlNode Select = doc.CreateElement("select");
            Select.SetAttributeValue("name", _name + ".ID");

            foreach (var item in _selectData)
            {
                var optionElement = doc.CreateElement("option");
                var id = item.GetType().GetProperty("ID").GetValue(item, null);
                var name = item.GetType().GetProperty("Name").GetValue(item, null);

                optionElement.Attributes.Add("value", id.ToString());
                optionElement.InnerHtml = name.ToString();

                if (selectedId == (int)id)
                {
                    optionElement.Attributes.Add("selected", "selected");
                }

                Select.AppendChild(optionElement);
            }

            HtmlNode wrapper = doc.CreateElement("div");
            wrapper.AddClass("input-wrapper");

            wrapper.InnerHtml += GetLabel();
            wrapper.InnerHtml += Select.OuterHtml;
            //wrapper.AppendChild(Select);

            _result += wrapper.OuterHtml;

            return _result;
        }

        public void SetSelectData(List<Model> selectData)
        {
            _selectData = selectData;
        }

        public void SetValue(dynamic value)
        {
            _value = value;
        }
    }
}
