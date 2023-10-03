using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace ApkaJezykowa.Main
{
  internal class UploadImage
  {
    public void ImageUploader()
    {
      var blobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=languageappfiles;AccountKey=Lq2MOaQcGN9BZ5iJW7iScsNgYceKRLS5h/ox76xVe6Dl40ZHfCHLQfs7UZCZs65TYqRfwxs6hRC9+AStvMK0nQ==;EndpointSuffix=core.windows.net";
      var blobStorageContainerName = "lessonimages";

      var container = new BlobContainerClient(blobStorageConnectionString, blobStorageContainerName);
      //BlobClient blob = container.GetBlobClient(blobName);
    }
  }
}
