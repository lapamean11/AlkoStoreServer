using AlkoStoreServer.Base;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlkoStoreServer.ViewHelpers.Inputs.Interfaces
{
    public interface ISelectInput : IInput
    {
        public void SetSelectData(List<Model> selectData);
    }
}
