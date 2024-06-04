using AlkoStoreServer.Base;
using AlkoStoreServer.Models;
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
            HtmlDocument doc = new HtmlDocument();

            List<string> selected = new List<string>();

            string entityKey = _selectData[0].GetType().Name + "Id";
            /*var lola = _selectData[0].GetType();

            if (_selectData[0].GetType() == typeof(Product))
            {

            }*/

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

            //_result = (wrapper.InnerHtml += _result);

            return wrapper.OuterHtml;
        }

        public string Render2()
        {
            HtmlDocument doc = new HtmlDocument();

            List<string> selected = new List<string>();

            if (_value != null) 
            {
                foreach (var item in _value)
                {
                    var id = item.GetType().GetProperty("StoreId").GetValue(item, null);
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

                /*HtmlNode hiddenCheckBox = doc.CreateElement("input");

                hiddenCheckBox.SetAttributeValue("type", "hidden");
                hiddenCheckBox.SetAttributeValue("name", _name + "[" + counter + "].StoreId");
                hiddenCheckBox.SetAttributeValue("value", "0");*/

                HtmlNode checkBox = doc.CreateElement("input");

                checkBox.SetAttributeValue("type", "checkbox");
                checkBox.SetAttributeValue("name", _name + "[" + counter + "].StoreId");
                checkBox.SetAttributeValue("value", id);

                HtmlNode StoreName = doc.CreateElement("p");
                HtmlTextNode Name = doc.CreateTextNode(storeName);
                StoreName.AppendChild(Name);

                _result += StoreName.OuterHtml;

                HtmlNode PriceInput = doc.CreateElement("input");
                PriceInput.SetAttributeValue("type", "text");
                PriceInput.SetAttributeValue("name", _name + "[" + counter + "].Price");

                HtmlNode BarcodeInput = doc.CreateElement("input");
                BarcodeInput.SetAttributeValue("type", "text");
                BarcodeInput.SetAttributeValue("name", _name + "[" + counter + "].Barcode");

                HtmlNode QtyInput = doc.CreateElement("input");
                QtyInput.SetAttributeValue("type", "text");
                QtyInput.SetAttributeValue("name", _name + "[" + counter + "].Qty");

                if (selected.Contains(id))
                {
                    // hiddenCheckBox.SetAttributeValue("value", id); // 1
                    // checkBox.SetAttributeValue("value", id); // 1
                    checkBox.SetAttributeValue("checked", "checked");

                    foreach (var data in _value)
                    {
                        if (data.StoreId == Int32.Parse(id))
                        {
                            PriceInput.SetAttributeValue("value", Convert.ToString(data.Price));
                            BarcodeInput.SetAttributeValue("value", data.Barcode);
                            QtyInput.SetAttributeValue("value", Convert.ToString(data.Qty));
                        }
                    }
                }

                //inputWrapper.InnerHtml += GetLabel();
                //inputWrapper.InnerHtml += hiddenCheckBox.OuterHtml;
                inputWrapper.InnerHtml += checkBox.OuterHtml;

                inputWrapper.InnerHtml += PriceInput.OuterHtml;
                inputWrapper.InnerHtml += BarcodeInput.OuterHtml;
                inputWrapper.InnerHtml += QtyInput.OuterHtml;

                _result += inputWrapper.OuterHtml;
                counter++;
            }

            HtmlNode wrapper = doc.CreateElement("div");
            wrapper.AddClass("input-section-wrapper");
            wrapper.InnerHtml += GetLabel();
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
