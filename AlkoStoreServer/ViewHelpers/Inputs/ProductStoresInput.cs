using AlkoStoreServer.Base;
using AlkoStoreServer.Models;
using AlkoStoreServer.ViewHelpers.Inputs.Interfaces;
using HtmlAgilityPack;

namespace AlkoStoreServer.ViewHelpers.Inputs
{
    public class ProductStoresInput : Input, ISelectInput
    {
        private List<Model> _selectData;


        public ProductStoresInput(string name) : base (name)
        {
            
        }

        public ProductStoresInput(string name, string namePrefix) : base(name, namePrefix)
        {

        }

        public string Render()
        {
            HtmlDocument doc = new HtmlDocument();

            List<string> selected = new List<string>();

            string entityKey = _selectData[0].GetType().Name + "Id";

            if (_value != null)
            {
                foreach (var item in _value)
                {
                    var id = item.GetType().GetProperty(entityKey).GetValue(item, null); // StoreId
                    selected.Add(id.ToString());
                }
            }

            int counter = 0;
            foreach (var item in _selectData)
            {
                var id = item.GetType().GetProperty("ID").GetValue(item, null).ToString();
                var storeName = item.GetType().GetProperty("Name").GetValue(item, null).ToString();

                HtmlNode inputWrapper = doc.CreateElement("div");
                inputWrapper.AddClass("store-inputs-wrapper");
                inputWrapper.AddClass("flex-col");

                HtmlNode checkBox = doc.CreateElement("input");
                checkBox.SetAttributeValue("type", "checkbox");
                checkBox.SetAttributeValue("name", _name + "[" + counter + "].StoreId");
                checkBox.SetAttributeValue("value", id);

                HtmlNode StoreName = doc.CreateElement("h5");
                HtmlTextNode Name = doc.CreateTextNode(storeName);
                StoreName.AppendChild(Name);

                HtmlNode titleWrapper = doc.CreateElement("div");
                titleWrapper.AddClass("store-title-wrapper flex items-center");
                titleWrapper.AppendChild(StoreName);
                titleWrapper.AppendChild(checkBox);

                //_result += StoreName.OuterHtml;

                TextInput priceInput = new TextInput(
                    _name + "[" + counter + "].Price",
                    "Price"
                );

                TextInput barcodeInput = new TextInput(
                    _name + "[" + counter + "].Barcode",
                    "Barcode"
                );

                TextInput qtyInput = new TextInput(
                    _name + "[" + counter + "].Qty",
                    "Qty"
                );

                if (selected.Contains(id))
                {
                    checkBox.SetAttributeValue("checked", "checked");

                    foreach (var data in _value)
                    {
                        if (data.GetType().GetProperty(entityKey).GetValue(data, null) == Int32.Parse(id)) //StoreId
                        {
                            priceInput.SetValue(Convert.ToString(data.Price));
                            barcodeInput.SetValue(Convert.ToString(data.Barcode));
                            qtyInput.SetValue(Convert.ToString(Convert.ToString(data.Qty)));
                        }
                    }
                }

                inputWrapper.InnerHtml += titleWrapper.OuterHtml;
                inputWrapper.InnerHtml += priceInput.Render();
                inputWrapper.InnerHtml += barcodeInput.Render();
                inputWrapper.InnerHtml += qtyInput.Render();

                _result += inputWrapper.OuterHtml;
                counter++;
            }

            HtmlNode wrapper = doc.CreateElement("div");
            wrapper.AddClass("input-section-wrapper flex-col");

            HtmlNode storesWrapper = doc.CreateElement("div");
            storesWrapper.AddClass("stores-wrapper flex-wrap gap1");

            storesWrapper.InnerHtml += _result;

            wrapper.InnerHtml += GetLabel();
            wrapper.InnerHtml += storesWrapper.OuterHtml;

            return wrapper.OuterHtml;
        }

        public void SetSelectData(List<Model> selectData)
        {
            _selectData = selectData;
        }
    }
}
