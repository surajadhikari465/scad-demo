using System;

namespace Mammoth.Common.Email
{
    public interface IEmailMessageBuilder<T>
    {
        string BuildMessage(T data);
        string BuildMessage(T data, Exception e);
    }
}
