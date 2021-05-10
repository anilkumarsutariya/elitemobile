using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EliteTimeSheetMobile
{
   public interface ISave
    {
        Task SaveAndView(string filename, string contentType, MemoryStream stream);
        Task<string> SaveSignature(Stream bitmap, string filename);
        Task<string> GetSignaturePath(string filename);

        
    }
}



