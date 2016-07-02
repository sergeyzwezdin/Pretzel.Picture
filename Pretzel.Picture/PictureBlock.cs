using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Pretzel.Logic.Extensibility;
using DotLiquid;


namespace Pretzel.Picture
{
    [Export(typeof(ITag))]
    public class PictureBlock : Block, ITag
    {
        private static readonly Regex CiteRegex = new Regex(@"<cite>([\W+\w+]*)</cite>", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline);
        private static readonly Regex HtmlTagRegex = new Regex(@"</*.+?>", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline);
        private static readonly Regex ImageClassesRegex = new Regex(@"[\w\-]+", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);

        protected string Src;

        protected string[] Params;

        public new string Name => "Picture";

        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);

            Params = Regex.Matches(markup, @"[\""].+?[\""]|[^ ]+")
                          .OfType<Match>()
                          .Select(_ => (_.Value ?? String.Empty).Trim(' ', '"'))
                          .ToArray();
        }

        public override void Render(Context context, TextWriter result)
        {
            if (Params.Any())
            {
                var caption = GetRenderedContent(context);

                result.WriteLine("<figure class=\"image\">");

                var classes = Params.Skip(1).SelectMany(p => ImageClassesRegex.Matches(p).OfType<Match>().Select(m=>m.Value)).ToArray();

                result.WriteLine("<img itemprop=\"image\" src=\"{0}\" title=\"{1}\" alt=\"{1}\" {2}/>", Uri.EscapeUriString(Params[0]),
                    HtmlTagRegex.Replace(CiteRegex.Replace(caption, String.Empty), String.Empty)
                    .Replace("\r", String.Empty).Replace("\n", String.Empty).Replace("\t", String.Empty).Replace("\"", String.Empty),
                    classes.Any() ? $"class=\"{String.Join(" ", classes)}\"" : String.Empty);

                result.Write("<figcaption>");
                base.Render(context, result);
                result.Write("</figcaption>");
                result.Write("</figure>");
            }
        }

        protected string GetRenderedContent(Context context)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    base.Render(context, writer);
                }

                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
    }
}