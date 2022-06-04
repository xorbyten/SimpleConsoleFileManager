using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleFileManager
{
    internal class DoWork
    {
        IFileIO fileIO;
        public DoWork(IFileIO io)
        {
            fileIO = io;
        }
        public void Start()
        {
            fileIO.Start();
        }
    }
}
