using System;

namespace HtmlConverter
{
    internal class Brush
    {
        public Brush(String javaScript, String style)
        {
            this.JavaScript = javaScript;
            this.Style = style;
        }

        public String JavaScript { get; private set; }

        public String Style { get; private set; }
    }
}
