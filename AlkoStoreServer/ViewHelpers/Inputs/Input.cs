using HtmlAgilityPack;

namespace AlkoStoreServer.ViewHelpers.Inputs
{
    public class Input
    {
        protected dynamic _value;

        protected string _result = string.Empty;

        protected string _name;

        protected string _prefixName = null;

        public Input(string name)
        { 
            _name = name;
        }

        public Input(
            string name,
            string namePrefix
        )
        {
            _name = name;
            _prefixName = namePrefix;
        }

        protected string GetLabel()
        {
            HtmlDocument doc = new HtmlDocument();
            HtmlNode label = doc.CreateElement("label");
            label.AddClass("input-label");
            label.SetAttributeValue("for", _name.Replace(" ", ""));

            if (_prefixName != null)
            {
                label.InnerHtml += _prefixName;

                return label.OuterHtml;
                //return "<label for=" + _name.Replace(" ", "") + ">" + _prefixName + "</label>";
            }

            label.InnerHtml += _name;

            return label.OuterHtml;
            //return "<label for=" + _name.Replace(" ", "") + ">" + _name + "</label>";
        }

        public void SetValue(dynamic value)
        {
            _value = value;
        }
    }
}
