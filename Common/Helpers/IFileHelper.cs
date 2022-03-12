using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common.Helpers
{
    public interface IFileHelper
    {
        // Metodo que recibe la foto (en memoria) y devuelve un array de Bytes
        byte[] ReadFully(Stream input);
    }
}
