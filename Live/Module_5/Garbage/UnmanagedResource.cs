using System;
using System.Collections.Generic;
using System.Text;

namespace Garbage
{
    public class UnmanagedResource : IDisposable
    {
        private static bool isOpen = false;
        private FileStream? file;

        public void Open()
        {
            if (isOpen)
            {
                Console.WriteLine("Helaas! Resource is al in gebruik");
                return;
            }
            isOpen = true;
            file = File.Create(@"file1.txt");
            Console.WriteLine("Resource is nu in gerbuik");
        }

        public void Close()
        {
            File.Delete("file1.txt");
            isOpen = false;
            Console.WriteLine("Resource is vrijgegeven");
        }

        protected void Ruimop(bool fromDispose)
        {
            if (fromDispose)
            {
                file?.Dispose();
            }
            Close();

        }

        public void Dispose()
        {
            Ruimop(true);
            GC.SuppressFinalize(this);
        }

        ~UnmanagedResource()
        {
            Ruimop(false);
        }
    }
}
