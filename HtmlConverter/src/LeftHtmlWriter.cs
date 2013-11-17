using System;
using System.Linq;
using System.Text;
using System.IO;
using StringInject;

namespace HtmlConverter
{
    internal class LeftHtmlWriter
    {
        private String template;

        public LeftHtmlWriter(String template)
        {
            this.template = template;
        }

        public void Write(String filePath, DirectoryNode root)
        {
            var content = Output(root, 0);
            var result = this.template.Inject(new
            {
                content,
            });

            File.WriteAllText(filePath, result);
        }

        public static String Output(DirectoryNode node, int depth)
        {
            var sb = new StringBuilder();

            var indentCharcter = "\t";
            var indent1 = String.Concat(Enumerable.Repeat(indentCharcter, depth).ToArray());
            var indent2 = String.Concat(Enumerable.Repeat(indentCharcter, depth + 1).ToArray());
            var indent3 = String.Concat(Enumerable.Repeat(indentCharcter, depth + 2).ToArray());
            var indent4 = String.Concat(Enumerable.Repeat(indentCharcter, depth + 3).ToArray());
            var indent5 = String.Concat(Enumerable.Repeat(indentCharcter, depth + 4).ToArray());

            sb.Append(indent1 + "<ul>\r\n");
            sb.Append(indent2 + "<li class=\"jstree-open\">\r\n");
            sb.Append(indent3 + "<a href=\"#\">" + node.Name + "</a>\r\n");

            sb.Append(indent3 + "<ul>\r\n");

            node.SubDirectoryList.ForEach(x => sb.Append(Output(x, depth + 3)));
            node.FileList.ForEach(x =>
            {
                sb.Append(indent4 + "<li name=\"" + x.Path.Replace('\\', '/') + ".html" + "\">\r\n");
                sb.Append(indent5 + "<a href=\"#\">" + x.Name + "</a>\r\n");
                sb.Append(indent4 + "</li>\r\n");
            });

            sb.Append(indent3 + "</ul>\r\n");

            sb.Append(indent2 + "</li>\r\n");
            sb.Append(indent1 + "</ul>\r\n");

            return sb.ToString();
        }
    }
}
