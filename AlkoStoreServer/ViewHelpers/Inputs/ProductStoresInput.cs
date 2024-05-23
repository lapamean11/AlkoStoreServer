using AlkoStoreServer.Base;
using AlkoStoreServer.ViewHelpers.Inputs.Interfaces;
using HtmlAgilityPack;

namespace AlkoStoreServer.ViewHelpers.Inputs
{
    public class ProductStoresInput : ISelectInput
    {
        private dynamic _value;

        private string _result = string.Empty;

        private string _name;

        private List<Model> _selectData;


        public ProductStoresInput(string name)
        {
            _name = name;
        }

        private string GetLabel()
        {
            return "<br/><label for=" + _name + ">" + _name + "</label><br/>";
        }

        public string Render()
        {
            _result += GetLabel();

            HtmlDocument doc = new HtmlDocument();

            List<string> selected = new List<string>();
            foreach (var item in _value)
            {
                var id = item.GetType().GetProperty("StoreId").GetValue(item, null);
                selected.Add(id.ToString());
            }

            foreach (var item in _selectData)
            {
                var id = item.GetType().GetProperty("ID").GetValue(item, null).ToString();
                var storeName = item.GetType().GetProperty("Name").GetValue(item, null).ToString();

                HtmlNode inputWrapper = doc.CreateElement("div");
                inputWrapper.AddClass("input-wrapper");
                inputWrapper.AddClass("flex-col");

                HtmlNode checkBox = doc.CreateElement("input");
                HtmlNode hiddenCheckBox = doc.CreateElement("input");

                hiddenCheckBox.SetAttributeValue("type", "hidden");
                hiddenCheckBox.SetAttributeValue("name", "StoreId");

                checkBox.SetAttributeValue("type", "checkbox");
                checkBox.SetAttributeValue("name", "StoreId");

                HtmlNode StoreName = doc.CreateElement("p");
                HtmlTextNode Name = doc.CreateTextNode(storeName);
                StoreName.AppendChild(Name);

                _result += StoreName.OuterHtml;

                HtmlNode PriceInput = doc.CreateElement("input");
                PriceInput.SetAttributeValue("type", "text");
                PriceInput.SetAttributeValue("name", "ProductStore.Price");

                HtmlNode BarcodeInput = doc.CreateElement("input");
                BarcodeInput.SetAttributeValue("type", "text");
                BarcodeInput.SetAttributeValue("name", "ProductStore.Barcode");

                if (selected.Contains(id))
                {
                    hiddenCheckBox.SetAttributeValue("value", "1");
                    checkBox.SetAttributeValue("value", "1");
                    checkBox.SetAttributeValue("checked", "checked");

                    foreach (var data in _value)
                    {
                        if (data.StoreId == Int32.Parse(id))
                        {
                            PriceInput.SetAttributeValue("value", Convert.ToString(data.Price));
                            BarcodeInput.SetAttributeValue("value", data.Barcode);
                        }
                    }
                }

                inputWrapper.InnerHtml += hiddenCheckBox.OuterHtml;
                inputWrapper.InnerHtml += checkBox.OuterHtml;

                inputWrapper.InnerHtml += PriceInput.OuterHtml;
                inputWrapper.InnerHtml += BarcodeInput.OuterHtml;

                _result += inputWrapper.OuterHtml;
            }

            HtmlNode wrapper = doc.CreateElement("div");
            wrapper.AddClass("input-section-wrapper");
            _result = (wrapper.InnerHtml += _result);

            return _result;
        }

        public void SetValue(dynamic value)
        {
            _value = value;
        }

        public void SetSelectData(List<Model> selectData)
        {
            _selectData = selectData;
        }
    }
}
