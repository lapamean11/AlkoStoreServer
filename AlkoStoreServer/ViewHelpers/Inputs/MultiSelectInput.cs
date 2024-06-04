using AlkoStoreServer.Base;
using AlkoStoreServer.ViewHelpers.Inputs.Interfaces;
using HtmlAgilityPack;

namespace AlkoStoreServer.ViewHelpers.Inputs
{
    public class MultiSelectInput : Input, ISelectInput
    {
        private List<Model> _selectData;

        public MultiSelectInput(string name) : base(name)
        {
            
        }

        public MultiSelectInput(string name, string namePrefix) : base(name, namePrefix)
        {

        }

        public void SetSelectData(List<Model> selectData)
        { 
            _selectData = selectData;
        }

        public string Render()
        {
            List<string> selected = new List<string>();

            var type = _selectData.First().GetType().Name;
            if (_value != null)
            {
                foreach (var item in _value)
                {
                    var id = item.GetType().GetProperty(type + "Id").GetValue(item, null);
                    selected.Add(id.ToString());
                }
            }

            HtmlDocument doc = new HtmlDocument();
            HtmlNode Select = doc.CreateElement("select");
            Select.SetAttributeValue("name", _name);
            Select.Attributes.Add("multiple", "multiple");

            int counter = 0;
            foreach (var item in _selectData)
            {

                var optionElement = doc.CreateElement("option");
                var id = item.GetType().GetProperty("ID").GetValue(item, null);
                var name = item.GetType().GetProperty("Name").GetValue(item, null);

                optionElement.Attributes.Add("value", id.ToString());
                //optionElement.SetAttributeValue("name", "Categories"+ "[" + counter + "]" + ".ID");
                optionElement.InnerHtml = name.ToString();

                if (selected.Contains(id.ToString()))
                {
                    optionElement.Attributes.Add("selected", "selected");
                }

                Select.AppendChild(optionElement);
                counter++;
            }

            HtmlNode wrapper = doc.CreateElement("div");
            wrapper.AddClass("input-wrapper");
            wrapper.AddClass("multiselect-wrapper");

            wrapper.InnerHtml += GetLabel();
            wrapper.InnerHtml += Select.OuterHtml;
            //wrapper.AppendChild(Select);

            _result += wrapper.OuterHtml;

            return _result;
        }
    }
}
