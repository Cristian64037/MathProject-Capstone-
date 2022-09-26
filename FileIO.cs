
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MathProject_Capstone_
{
    public class FileIO
    {
        string fileName { get; set; }
        public FileIO(string fileName)
        {
            this.fileName = fileName;
        }
        public string[] readDataInToStringList()
        {

            var path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, fileName);
            try
            {
                string[] rows = File.ReadAllLines(path);
                return rows;
            }
            catch (Exception e)
            {
                Console.WriteLine("It is broke path:{0}", path.ToString());
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
            string[] r = new string[1];
            return r;
        }
    }
}