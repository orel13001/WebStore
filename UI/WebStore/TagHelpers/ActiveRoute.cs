using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.TagHelpers
{
    [HtmlTargetElement(Attributes = AttributeName)]
    public class ActiveRoute : TagHelper
    {
        private const string AttributeName = "ws-is-active-route";
        private const string IgnoreAction = "ws-ignore-action";

        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }

        [HtmlAttributeName("asp-controller")]
        public string Controller { get; set; }

        [HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]
        public Dictionary<string, string> RouteValues { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            var ignore_action = output.Attributes.RemoveAll(IgnoreAction);


            output.Attributes.RemoveAll(AttributeName);

            if (IsActive(ignore_action))
                MakeActive(output);
        }


        private bool IsActive(bool IgnoreAction)
        {
            var route_values = ViewContext.RouteData.Values;

            var route_controller = route_values["controller"]?.ToString();
            var route_action = route_values["action"]?.ToString();


            if (!IgnoreAction && Action is { Length: > 0 } action && !string.Equals(action, route_action))
                return false;

            if (Controller is { Length: > 0 } controller && !string.Equals(controller, route_controller))
                return false;

            foreach (var (key, value) in RouteValues)
            {
                if (!route_values.ContainsKey(key) || route_values[key]?.ToString() != value)
                    return false;
            }

            return true;
        }


        private static void MakeActive(TagHelperOutput output)
        {
            var class_attribute = output.Attributes.FirstOrDefault(attr => attr.Name == "class");
            if (class_attribute != null)
                output.Attributes.Add("class", "active");
            else
            {
                if (class_attribute?.Value?.ToString()?.Contains("active") ?? false)
                    return;

                output.Attributes.SetAttribute("class", $"{class_attribute?.Value} active");
            }

        }
    }
}
