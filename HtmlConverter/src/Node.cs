using System;
using System.Collections.Generic;

namespace HtmlConverter
{
    internal class Node
    {
        public Node(String name)
        {
            this.Name = name;
        }

        public String Name { get; private set; }
    }

    internal class DirectoryNode : Node
    {
        public List<DirectoryNode> SubDirectoryList { get; private set; }
        public List<FileNode> FileList { get; private set; }

        public DirectoryNode(String name)
            : base(name)
        {
            this.SubDirectoryList = new List<DirectoryNode>();
            this.FileList = new List<FileNode>();
        }

        public void AddDirectory(DirectoryNode subDirectory)
        {
            this.SubDirectoryList.Add(subDirectory);
        }

        public void AddFile(FileNode file)
        {
            this.FileList.Add(file);
        }
    }

    internal class FileNode : Node
    {
        public FileNode(String name, String path)
            : base(name)
        {
            this.Path = path;
        }

        public String Path { get; private set; }
    }
}
