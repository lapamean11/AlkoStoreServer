﻿using AlkoStoreServer.Base;
using AlkoStoreServer.CustomAttributes;
using AlkoStoreServer.Models;
using AlkoStoreServer.Services.Interfaces;
using AlkoStoreServer.ViewHelpers.Inputs;
using AlkoStoreServer.ViewHelpers.Inputs.Interfaces;
using AlkoStoreServer.ViewHelpers.Interfaces;
using AlkoStoreServer.ViewHelpers.Renderer;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AlkoStoreServer.ViewHelpers
{
    public class HtmlRenderer : IHtmlRenderer
    {
        private readonly IAttributeService _attributeService;

        public IDictionary<Type, Type> _attributeMap = new Dictionary<Type, Type>
        {
            { typeof(string), typeof(TextInput) },
            { typeof(int?), typeof(TextInput) },
            { typeof(int), typeof(TextInput) },
            { typeof(List<ProductAttributeProduct>), typeof(AttributesInput) },
            { typeof(List<CategoryAttributeCategory>), typeof(AttributesInput) },
            { typeof(List<ProductStore>), typeof(ProductStoresInput) },
            { typeof(List<ProductCategory>), typeof(MultiSelectInput) },
            { typeof(AttributeType), typeof(SelectInput) },
            { typeof(Category), typeof(SelectInput) },
            { typeof(Role), typeof(SelectInput) }
        };

        public HtmlRenderer(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }

        public IHtmlContent RenderForm(Model model)
        {
            StringBuilder html = new StringBuilder();
            HtmlDocument doc = new HtmlDocument();

            HtmlNode wrapper = doc.CreateElement("form-inner-wrapper");

            PropertyInfo[] properties = model.GetType().GetProperties();

            foreach (var data in properties)
            {
                if (!Attribute.IsDefined(data, typeof(NoRenderAttribute)))
                {
                    var value = data.GetValue(model) ?? null;

                    IInput input = DefineInput(data);

                    if (input is ISelectInput selectInput)
                    {
                        var key = data.Name;
                        var relatedData = _attributeService.GetFormRelatedData(model);

                        if (relatedData.TryGetValue(key, out List<Model> values))
                        {
                            selectInput.SetSelectData(values);
                        }
                    }

                    if (input != null) input.SetValue(value);

                    InputRenderer renderer = new InputRenderer(input);

                    html.Append(renderer.Render());
                }
            }

            wrapper.InnerHtml += html.ToString();

            return new HtmlString(wrapper.OuterHtml);
        }

        public IInput DefineInput(PropertyInfo data)
        {
            foreach (var item in _attributeMap)
            {
                if (item.Key == data.PropertyType)
                {
                    return (IInput)Activator.CreateInstance(
                        item.Value,
                        data.Name
                    );
                }
            }

            return null;
        }
    }
}
