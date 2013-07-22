using System;
using System.Collections.Generic;

namespace HtmlConverter
{
    internal class BrushFactory
    {
        private Dictionary<String, Brush> brushList = new Dictionary<String, Brush>();

        public void AddBrush(String ext, Brush brush)
        {
            this.brushList.Add(ext, brush);
        }

        public Brush GetBrush(String ext)
        {
            return this.brushList[ext];
        }

        public Boolean ContainsExt(String ext)
        {
            return this.brushList.ContainsKey(ext);
        }
    }
}
