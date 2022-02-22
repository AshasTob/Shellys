using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderTimeOutService.BlobService
{
    public interface IBlobStorage
    {
        Task UploadContentBlobAsync(string content, string fileName);
    }
}
