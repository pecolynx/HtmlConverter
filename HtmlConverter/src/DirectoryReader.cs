using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HtmlConverter
{
    internal class DirectoryReader
    {
        private BrushFactory brushFactory;

        public DirectoryReader(BrushFactory brushFactory)
        {
            this.brushFactory = brushFactory;
        }

        public void Read(String srcDirectoryPath, String dstDirectoryPath)
        {
            this.HtmlDirectoryPath = Path.Combine(dstDirectoryPath, "html");

            var title = Path.GetFileName(srcDirectoryPath);

            var directoryList = new Dictionary<String, DirectoryNode>();
            var fileList = new Dictionary<String, FileNode>();

            this.Root = new DirectoryNode(title);
            directoryList.Add(".\\html", Root);

            var targetLength = srcDirectoryPath.Length;
            this.FilePathList = Directory.GetFiles(srcDirectoryPath, "*", SearchOption.AllDirectories)
                .Where(x => brushFactory.ContainsExt(Path.GetExtension(x).ToLower()));

            foreach (var x in FilePathList.Select(x => this.HtmlDirectoryPath + x.Substring(targetLength)))
            {
                var dir = Path.GetDirectoryName(x);

                var relativeDirPath = "." + dir.Substring(dstDirectoryPath.Length);
                var tmp = relativeDirPath;
                while (true)
                {
                    if (!directoryList.ContainsKey(tmp))
                    {
                        directoryList.Add(tmp, new DirectoryNode(Path.GetFileName(tmp)));
                    }

                    tmp = Path.GetDirectoryName(tmp);
                    if (tmp == ".")
                    {
                        break;
                    }
                }

                var relativeFilePath = "." + x.Substring(dstDirectoryPath.Length);
                if (!fileList.ContainsKey(relativeFilePath))
                {
                    fileList.Add(relativeFilePath, new FileNode(Path.GetFileName(x), relativeFilePath));
                }
            }

            foreach (var file in fileList.OrderBy(x => x.Key))
            {
                var dirPath = Path.GetDirectoryName(file.Key);
                directoryList[dirPath].AddFile(file.Value);
            }

            foreach (var dir in directoryList.OrderBy(x => x.Key))
            {
                var dirPath = Path.GetDirectoryName(dir.Key);
                if (directoryList.ContainsKey(dirPath))
                {
                    directoryList[dirPath].AddDirectory(dir.Value);
                }
                else
                {
                    Console.WriteLine("not contains : " + dirPath);
                }
            }


        }

        public String HtmlDirectoryPath { get; private set; }
        public DirectoryNode Root { get; private set; }
        public IEnumerable<String> FilePathList { get; private set; }
    }
}
