using AlkoStoreServer.Base;
using AlkoStoreServer.ViewHelpers.Inputs.Interfaces;
using Microsoft.AspNetCore.Html;
using System.Reflection;

namespace AlkoStoreServer.ViewHelpers.Interfaces
{
    public interface IHtmlRenderer
    {
        public IHtmlContent RenderEditForm(Model model);

        public IInput DefineInput(PropertyInfo data);
    }
}
