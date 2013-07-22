using System;
using System.Linq;
using System.IO;
using System.Web;

using StringInject;

namespace HtmlConverter
{
    internal class RightHtmlWriter
    {
        private String template;
        
        public RightHtmlWriter(String template)
        {
            this.template = template;
        }

        public void Write(Brush brush, String srcFilePath, String dstFilePath, Int32 depth)
        {
            var content = HttpUtility.HtmlEncode(File.ReadAllText(srcFilePath));
            var title = Path.GetFileName(srcFilePath);
            var path = String.Concat(Enumerable.Repeat("../", depth).ToArray());

            var javaScript = brush.JavaScript;
            var style = brush.Style;
            var result = template.Inject(new
            {
                path,
                javaScript,
                style,
                title,
                content,
            });

            File.WriteAllText(dstFilePath, result);
        }
    }
}
