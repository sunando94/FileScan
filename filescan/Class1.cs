using System;
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
            inputFiles = getFileNames(inputPath).ToList();
            outputFiles = getFileNames(outputPath).ToList();
            foreach (string file in inputFiles)
            {
                if (isPresentInOutput(file, outputFiles))
                    if (checkDateModified(inputPath + "\\" + file))
                        if (deleteFile(inputPath + "\\" + file))
                            Console.WriteLine("File deleted : {0}", inputPath + "\\" + file);
                        else
                            Console.WriteLine("File not deleted : {0}", inputPath + "\\" + file);
            }

        }
        private IEnumerable<string> getFileNames(string path)
        {
            return Directory.EnumerateFiles(path);
        }
        private bool isPresentInOutput(string fileName, IEnumerable<string> outFiles)
        {
            return outFiles.Contains(fileName);
        }
        private bool checkDateModified(string filename)
        {
            FileInfo fi = new FileInfo(filename);
            int i = fi.CreationTime.CompareTo(DateTime.Now.AddDays(-7));
            if (i == 0)
            {

                return true;
            }
            return false;
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


                }
                catch (Exception e)
                {
                    if (args.Length < 2)
                        Console.WriteLine("Input path and output path required");
                }
            }
        }
    
}

