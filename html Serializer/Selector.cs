using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace html_Serializer
{
    public class Selector
    {
        public string TagName{ get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        public Selector Parent { get; set; }
        public Selector child { get; set; }
        public static bool IsValidTagName(string tagName)
        {
            
                return HtmlHelper.Instence.tags1.Any(t => t == tagName) ||HtmlHelper.Instence.tags2.Any(t => t == tagName);

        }
        public static Selector ParseSelectorString(string s= "div.language-html#tt table ")
        {
            if (s == null)
                return null;
            
            Selector rootSelector = new Selector();
            Selector currentSelector = rootSelector;
            string[] s1 = s.Split(" ");
            rootSelector.Parent = null;
            string[] subparts  = new Regex("(?=[#\\.])").Split(s1[0]).Where(s => s.Length > 0).ToArray();
            foreach (string subpart in subparts)
            {
                if (subpart.StartsWith("#"))
                {
                    currentSelector.Id = subpart.Substring(1);
                }
                else if (subpart.StartsWith("."))
                {
                    currentSelector.Classes.Add(subpart.Substring(1));
                }
                else
                {
                    // בדיקה שהחלק הוא שם תגית תקין
                    if (IsValidTagName(subpart))
                    {
                        currentSelector.TagName = subpart;

                    }
                    else
                    {
                        throw new ArgumentException("Invalid tag name: " + subpart);
                    }
                }
            }

            for (var i=1;i< s1.Length;i++)
            {
                Selector newSelector = new Selector();
                newSelector.Parent = currentSelector;
                currentSelector.child = newSelector;
                currentSelector = newSelector;
                subparts = new Regex("(?=[#\\.])").Split(s1[i]).Where(s => s.Length > 0).ToArray();
                foreach (string subpart in subparts)
                {
                    if (subpart.StartsWith("#"))
                    {
                        currentSelector.Id = subpart.Substring(1);
                    }
                    else if (subpart.StartsWith("."))
                    {
                        currentSelector.Classes.Add(subpart.Substring(1));
                    }
                    else 
                    {
                        // בדיקה שהחלק הוא שם תגית תקין
                        if (IsValidTagName(subpart))
                        {
                            currentSelector.TagName = subpart;

                        }
                        else
                        {
                            throw new ArgumentException("Invalid tag name: " + subpart);
                        }
                    }
                }
               
            }
            return rootSelector;
        }
       
    }
}
