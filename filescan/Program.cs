using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace filescan
{
    class FileSearch
    {
        private string inputPath;
        private string outputPath;
        private List<string> inputFiles;
        private List<string> outputFiles;
        public FileSearch(string inputPath, string outputPath)
        {
            this.inputPath = inputPath;
            this.outputPath = outputPath;
        }
        public void execute()
        {
            inputFiles = getFiles(inputPath).ToList();
            outputFiles = getFiles(outputPath).ToList();
            foreach (string file in inputFiles)
           {
                if (isPresentInOutput(getFileName(file), outputFiles))
                    if (checkDateModified(outputPath + "\\" + getFileName(file)))
                        if (deleteFile(outputPath + "\\" + getFileName(file)))
                         copyFile(file, outputPath + "\\" + getFileName(file));               
            }

        }
        private string getFileName(string file)
        {
            return new FileInfo(file).Name;
        }
        private IEnumerable<string> getFiles(string path)
        {
            
            return Directory.EnumerateFiles(path);
        }
        private bool isPresentInOutput(string fileName, IEnumerable<string> outFiles)
        {
            return outFiles.Contains(fileName,new FileNameComparer());
        }
        private bool checkDateModified(string filename)
        {
            FileInfo fi = new FileInfo(filename);

            System.Diagnostics.Debug.WriteLine(DateTime.Now.AddDays(-7), "compare date");
            System.Diagnostics.Debug.WriteLine(DateTime.Compare(fi.LastWriteTime, DateTime.Now.AddDays(-7)), "compare date");
            return (DateTime.Compare(fi.LastWriteTime,DateTime.Now.AddDays(-7)) < 0 && DateTime.Compare(fi.CreationTime, DateTime.Now.AddDays(-7)) < 0) ? true : false;
        }
        private bool copyFile(string filename,string dest)
        {
            try
            {
                new FileInfo(filename).CopyTo(dest);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool deleteFile(string filename)
        {
            try
            {
                new FileInfo(filename).Delete();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
    public class MainClass
    {
        public static void Main(string[] args)
        {

            try
            {
                FileSearch fs = new FileSearch(args[0], args[1]);
                fs.execute();
                Console.ReadKey();

            }
            catch
            {
                if (args.Length < 2)
                    Console.WriteLine("Input path and output path required");
            }
        }
    }
    class FileNameComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return x.Contains(y);
        }

        
        public int GetHashCode(string obj)
        {
            throw new NotImplementedException();
        }

      
    }
}

