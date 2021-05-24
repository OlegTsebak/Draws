using System;

namespace Draws.Helpers.Attributes
{
    public class PageAttribute : Attribute
    {
        public string Name { get; set; }

        public PageAttribute()
        {
        }

        public PageAttribute(string name)
        {
            Name = name;
        }
    }
}