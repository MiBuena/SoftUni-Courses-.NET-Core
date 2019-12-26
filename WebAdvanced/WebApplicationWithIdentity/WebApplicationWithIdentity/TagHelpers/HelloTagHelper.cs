using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationWithIdentity.TagHelpers
{
    [HtmlTargetElement("h1", Attributes = "target-name")]
    public class HelloTagHelper : TagHelper
    {
        private const string MessageFormat = "Hello, {0}";
        public string TargetName { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string formattedMessage = string.Format(MessageFormat, this.TargetName);
            output.Content.SetContent(formattedMessage);
        }
    }
}
