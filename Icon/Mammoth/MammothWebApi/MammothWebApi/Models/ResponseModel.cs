using System;

namespace MammothWebApi.Models
{
    public class ResponseModel<T>
    {
        public DateTime CreatedAt { get; set; }
        public string Url { get; set; }
        public T Model { get; set; }
    }
}