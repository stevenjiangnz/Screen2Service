//using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.Utils
{
    public static class ContainerExtensions
    {
        public static bool Exists(this CloudBlobContainer container)
        {
            try
            {
                container.FetchAttributes();
                return true;
            }
            catch (StorageClientException e)
            {
                if (e.ErrorCode == StorageErrorCode.ResourceNotFound)
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
