using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public interface IStreamable
    {
        void ToStream(Stream stream);
    }
}
