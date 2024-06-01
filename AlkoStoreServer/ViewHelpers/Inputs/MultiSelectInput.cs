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

        /*public string Render()
        {
            _result = GetLabel();

            List<string> selected = new List<string>();
            var lol = _value;
            if (_value != null)
            {
                foreach (var item in _value)
                {
                    var id = item.GetType().GetProperty("ID").GetValue(item, null);
                    selected.Add(id.ToString());
                }
            }

            HtmlDocument doc = new HtmlDocument();

            int counter = 0;
            foreach (var item in _selectData)
            {
                var id = item.GetType().GetProperty("ID").GetValue(item, null);
                var name = item.GetType().GetProperty("Name").GetValue(item, null);

                HtmlNode input = doc.CreateElement("input");
                HtmlNode hiddenInput = doc.CreateElement("input");

                hiddenInput.SetAttributeValue("type", "hidden");
                hiddenInput.SetAttributeValue("name", _name + "[" + counter + "].ID");
                hiddenInput.SetAttributeValue("value", id.ToString());

                input.SetAttributeValue("type", "checkbox");
                input.SetAttributeValue("name", _name + "[" + counter + "].ID");
                input.SetAttributeValue("value", id.ToString());

                if (selected.Contains(id.ToString()))
                {
                    input.SetAttributeValue("checked", "checked");
                }

                _result += "<div>" + name + "</div>";
                _result += hiddenInput.OuterHtml;
                _result += input.OuterHtml;

                counter++;
            }

            HtmlNode wrapper = doc.CreateElement("div");
            wrapper.AddClass("input-wrapper");
            wrapper.AddClass("multiselect-wrapper");

            wrapper.InnerHtml = _result;

            return wrapper.OuterHtml;
        }*/

        public string Render()
        {
            //_result = GetLabel();

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

        public void SetValue(dynamic value)
        {
            _value = value;
        }
    }
}
