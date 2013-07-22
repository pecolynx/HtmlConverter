using System;
using System.IO;

using StringInject;

namespace HtmlConverter
{
    internal class IndexHtmlWriter
    {
        private String template;
        
        public IndexHtmlWriter(String template)
        {
            this.template = template;
        }

        public void Write(String filePath, String title)
        {
            var result = template.Inject(new
            {
                title,
            });

            File.WriteAllText(filePath, result);
        }
    }
}
