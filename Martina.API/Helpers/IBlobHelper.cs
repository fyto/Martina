using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Martina.API.Helpers
{
    public interface IBlobHelper
    {
        // Interfaz de usuario
        Task<Guid> UploadBlobAsync(IFormFile file, string containerName, string keys);

        // API
        Task<Guid> UploadBlobAsync(byte[] file, string containerName, string keys);

        // Seeder
        Task<Guid> UploadBlobAsync(string image, string containerName);

        Task DeleteBlobAsync(Guid id, string containerName);
    }
}
