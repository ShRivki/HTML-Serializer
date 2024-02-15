using System.Text.Json;
using System.IO;
using System.Text.RegularExpressions;

namespace html_Serializer
{
    public class HtmlHelper
    {
        private readonly static HtmlHelper _instence = new HtmlHelper();
        public static HtmlHelper Instence => _instence;
        public List<string> tags1 { get; set; }
        public List<string> tags2 { get; set; }
        private HtmlHelper() {
            var d = File.ReadAllText("HtmlTags.json");
            tags1  = JsonSerializer.Deserialize<List<string>>(d);

            var d2 = File.ReadAllText("HtmlVoidTags.json");
            tags2 = JsonSerializer.Deserialize<List<string>>(d2);


        }
        
    }
}
