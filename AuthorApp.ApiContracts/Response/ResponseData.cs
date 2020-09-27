using System.Collections.Generic;

namespace AuthorApp.ApiContracts.Response
{
    public class ResponseData<T>
    {
        public List<T> Data { get;  }

        public int Total { get; }

        public ResponseData(List<T> data)
        {
            Data = data;
            Total = data.Count;
        }
    }
}
