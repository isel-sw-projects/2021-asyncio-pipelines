using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lookwords.testRunnables
{
    public abstract class Runnable
    {
        protected string folderPath = @Environment.GetEnvironmentVariable("TESE_BOOKS_FOLDER_PATH");
        public abstract void Run();
    }
}
