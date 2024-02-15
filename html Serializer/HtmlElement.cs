using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace html_Serializer
{
    public class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }
        public HtmlElement()
        {
            Children = new List<HtmlElement>();
            Classes = new List<string>();
            Attributes=new List<string>();
        }
        public  IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> q = new Queue<HtmlElement>();
            HtmlElement h;
            q.Enqueue(this);
            h = q.Dequeue();
            for (int i = 0; i < h.Children.Count(); i++)
            {
                q.Enqueue(h.Children[i]);
            }
            while (q.Count > 0)
            {
                h = q.Dequeue();
                for (int i = 0; i < h.Children.Count(); i++)
                {
                    q.Enqueue(h.Children[i]);
                }
                yield return h;
            }
        }
        public  IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement h = this;
            while (h.Parent != null)
            {
                yield return h.Parent;
                h = h.Parent;
            }
        }

        public IEnumerable<HtmlElement> find(Selector s)
        {
            HashSet<HtmlElement> result = new HashSet<HtmlElement>();
            recorsya(this.Descendants(), s, result);
            return result;
        }
        public static void recorsya(IEnumerable<HtmlElement> l, Selector s,HashSet<HtmlElement>hs)
        {
            if (s == null)
                return;
            foreach(var element  in l)
            {
                if ((s.TagName== null ||s.TagName==element.Name)&& (s.Id == null || s.Id == element.Id)&& (s.Classes == null || s.Classes.All(c=>element.Classes.All(c1=>c==c1)))) 
                {
                   
                
                if (s.child==null)
                {
                    hs.Add(element);
                }
                    recorsya(element.Descendants(), s.child, hs);
                }
            }

        }
    }
}
