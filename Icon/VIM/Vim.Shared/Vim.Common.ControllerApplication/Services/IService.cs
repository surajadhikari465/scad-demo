using System.Collections.Generic;
namespace Vim.Common.ControllerApplication.Services
{
    public interface IService<T>
    {
        void Process(List<T> data);
    }
}
