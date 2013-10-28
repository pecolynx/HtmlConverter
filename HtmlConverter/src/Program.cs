using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Xml.Linq;

namespace HtmlConverter
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2 && args.Length != 3)
            {
                Console.WriteLine("Invalid argument.");
                return;
            }

            var asm = Assembly.GetEntryAssembly();
            var exeDirPath = Path.GetDirectoryName(asm.Location);

            var srcDirPath = args[0];
            var dstDirPath = args[1];


            String brushFilePath;
            if (args.Length == 2)
            {
                brushFilePath = Path.Combine(exeDirPath, "brushlist.xml");
            }
            else
            {
                brushFilePath = args[2];
            }

            Console.WriteLine("SourceDirectory : " + srcDirPath);
            Console.WriteLine("DestinationDirectory : " + dstDirPath);
            Console.WriteLine("BrushListFile : " + brushFilePath);

            if (!Directory.Exists(srcDirPath))
            {
                Console.WriteLine("Source directory not exists.");
                return;
            }

            if (!File.Exists(brushFilePath))
            {
                Console.WriteLine("BrushList file not exists.");
                return;
            }

            var brushList = XElement.Load(new StreamReader(brushFilePath));

            var brushDictionary = new Dictionary<String, Brush>();
            brushDictionary.Add("as3", new Brush("shBrushAS3.js", "as3"));
            brushDictionary.Add("bash", new Brush("shBrushBash.js", "bash"));
            brushDictionary.Add("coldfusion", new Brush("shBrushColdFusion.js", "coldfusion"));
            brushDictionary.Add("cpp", new Brush("shBrushCpp.js", "cpp"));
            brushDictionary.Add("csharp", new Brush("shBrushCSharp.js", "csharp"));
            brushDictionary.Add("css", new Brush("shBrushCss.js", "css"));
            brushDictionary.Add("delphi", new Brush("shBrushDelphi.js", "delphi"));
            brushDictionary.Add("diff", new Brush("shBrushDiff.js", "diff"));
            brushDictionary.Add("erlang", new Brush("shBrushErlang.js", "erlang"));
            brushDictionary.Add("groovy", new Brush("shBrushGroovy.js", "groovy"));
            brushDictionary.Add("java", new Brush("shBrushJava.js", "java"));
            brushDictionary.Add("javafx", new Brush("shBrushJavaFX.js", "javafx"));
            brushDictionary.Add("jscript", new Brush("shBrushJScript.js", "jscript"));
            brushDictionary.Add("perl", new Brush("shBrushPerl.js", "perl"));
            brushDictionary.Add("php", new Brush("shBrushPhp.js", "php"));
            brushDictionary.Add("plain", new Brush("shBrushPlain.js", "plain"));
            brushDictionary.Add("powershell", new Brush("shBrushPowerShell.js", "powershell"));
            brushDictionary.Add("python", new Brush("shBrushPython.js", "python"));
            brushDictionary.Add("ruby", new Brush("shBrushRuby.js", "ruby"));
            brushDictionary.Add("scala", new Brush("shBrushScala.js", "scala"));
            brushDictionary.Add("sql", new Brush("shBrushSql.js", "sql"));
            brushDictionary.Add("vb", new Brush("shBrushVb.js", "vb"));
            brushDictionary.Add("xml", new Brush("shBrushXml.js", "xml"));

            var brushFactory = new BrushFactory();
            foreach (var x in brushList.Elements("brush"))
            {
                if (x.Attribute("ext") == null)
                {
                    Console.WriteLine("Attribute not found : ext");
                    return;
                }
                if (x.Attribute("type") == null)
                {
                    Console.WriteLine("Attribute not found : type");
                    return;
                }

                var brush = x.Attribute("type").Value;
                if (!brushDictionary.ContainsKey(brush))
                {
                    Console.WriteLine("Key not found. : " + brush);
                    return;
                }

                var ext = x.Attribute("ext").Value;
                brushFactory.AddBrush("." + ext, brushDictionary[brush]);
            }

            var reader = new DirectoryReader(brushFactory);
            reader.Read(srcDirPath, dstDirPath);
            
            var files = reader.FilePathList;
            var root = reader.Root;
            var targetLength = srcDirPath.Length;
            var htmlDirPath = reader.HtmlDirectoryPath;

            var title = Path.GetFileName(srcDirPath);

            files.ToList().ForEach(x =>
            {
                var dir = Path.GetDirectoryName(htmlDirPath + x.Substring(targetLength));
                Directory.CreateDirectory(dir);
            });

            // index output
            var indexWriter = new IndexHtmlWriter(File.ReadAllText(Path.Combine(exeDirPath, "template\\index.html.txt")));
            indexWriter.Write(Path.Combine(dstDirPath, "index.html"), title);

            // left output
            var leftWriter = new LeftHtmlWriter(File.ReadAllText(Path.Combine(exeDirPath, "template\\left.html.txt")));
            leftWriter.Write(Path.Combine(dstDirPath, "left.html"), root);

            // right output
            var rightWriter = new RightHtmlWriter(File.ReadAllText(Path.Combine(exeDirPath, "template\\right.html.txt")));
            files.ToList().ForEach(x =>
            {
                var srcPath = x;
                var dstPath = htmlDirPath + x.Substring(targetLength) + ".html";
                var depth = CountChar(x.Substring(targetLength + 1), Path.DirectorySeparatorChar);
                var brush = brushFactory.GetBrush(Path.GetExtension(x).ToLower());

                rightWriter.Write(brush, srcPath, dstPath, depth);
            });

            // resource copy
            var resourcesDir = Path.Combine(exeDirPath, "resources");
            var resourcesDirLength = resourcesDir.Length;
            Directory.GetFiles(resourcesDir, "*", SearchOption.AllDirectories).ToList().ForEach(x =>
            {
                Directory.CreateDirectory(Path.Combine(dstDirPath, Path.GetDirectoryName(x).Substring(resourcesDirLength + 1)));
                File.Copy(x, Path.Combine(dstDirPath, x.Substring(resourcesDirLength + 1)), true);
            });

            // right html copy
            File.Copy(Path.Combine(exeDirPath, "template\\right.html"), Path.Combine(dstDirPath, "right.html"), true);

            Console.WriteLine("Done.");
        }

        public static int CountChar(string s, char c)
        {
            return s.Length - s.Replace(c.ToString(), "").Length;
        }

    }
}
