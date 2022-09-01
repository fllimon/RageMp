using RageMpServer.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace RageMpServer.Extensions
{
    class DbInitializer
    {
        private static RageContext _context = null;

        public static RageContext GetInstance()
        {
            if (_context == null)
            {
                _context = new RageContext();
            }

            return _context;
        }
    }
}
