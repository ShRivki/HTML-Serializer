using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using html_Serializer;

HttpClient client = new HttpClient();
var response = await client.GetAsync("https://learn.malkabruk.co.il/practicode/projects/pract-2/#html-serializer");
var html = await response.Content.ReadAsStringAsync();
var cleanHtml = new Regex("[\\r\\n\\t]").Replace(new Regex("\\s{2,}").Replace(html, ""), "");
//cleanHtml = Regex.Replace(cleanHtml, @"^\s*$", string.Empty, RegexOptions.Multiline);
var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0);
HtmlElement htmlElement = new HtmlElement();
HtmlElement root = htmlElement;
Selector.ParseSelectorString();

foreach (string s in htmlLines)
{

    string word = s.Split(' ')[0];
    if (word.Length < 0)
    {
        continue;
    }
    if (word.Equals("/html"))
    {
        break;
    }
    if (word.Length > 0 && word[0] == '/')
    {
        htmlElement = htmlElement.Parent;
    }
    else if (IsTag2(word) || IsTag1(word))
    {

        if (word.Equals("html"))
        {
            htmlElement.Name = word;
            root = htmlElement;
            htmlElement.InnerHtml = "";
            root.Parent = null;
            

        }
        else
        {
            HtmlElement child = new HtmlElement();
            child.Parent = htmlElement;
            att(s, child);
            child.Name = word;
            htmlElement.Children.Add(child);
            if (!IsTag2(word) && !s.EndsWith("/"))
            {
                htmlElement = child;
            }
        }


    }
    else
    {
        htmlElement.InnerHtml += s;
    }
}
int i = 3;
Selector s2 = Selector.ParseSelectorString("div a.md-skip");
var result =root.find(s2);
Selector s3 = Selector.ParseSelectorString("div a.md-skip");
var result2 = root.find(s3);
Selector s4 = Selector.ParseSelectorString("div span.md-ellipsis");
var result3 = root.find(s4);
Console.WriteLine("sssssssssss");
int f = 3;
int f1= 3;
static void att(string s, HtmlElement h)
{
    var att = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(s);
    if (att.FirstOrDefault(t => t.ToString().Split("=")[0] == "id") != null)
    {
        h.Id = att.FirstOrDefault(t => t.ToString().Split("=")[0] == "id").ToString().Split("=")[1].Replace("\"", "");
        //var id = new Regex("id=\"(.*?)\"").Matches(s).ToString();

    }
    else
    {
        h.Id = "";
    }
    if (s.Contains("class"))
    {
        var classes = new Regex("class=\"(.*?)\"").Matches(s).ToString();

        //MatchCollection matches = Regex.Matches(s, "class=\"(.*?)\"");
        //classes = classes.Replace("class=", " ");
        var c = att.FirstOrDefault(t => t.ToString().Split("=")[0] == "class").ToString().Split("=")[1].Replace("\"", "");
        h.Classes = c.Split(' ').ToList();

    }
    h.Attributes.AddRange(att.Select(t => t.ToString().Replace("\"", "")));
}
static bool IsTag1(string tag)
{
    return HtmlHelper.Instence.tags1.Any(t => t == tag);
}
static bool IsTag2(string tag)
{
    return HtmlHelper.Instence.tags2.Any(t => t == tag);
}
