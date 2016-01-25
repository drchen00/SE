using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE
{
    public class ProjectofParts
    {
        public string name;
        public List<Parts> partList;

        public ProjectofParts(string name)
        {
            this.name = name;
            partList = new List<Parts>();
        }

        public void addPart(Parts part)
        {
            partList.Add(part);
        }

        public void partListSort()
        {
            partList.Sort(delegate(Parts x, Parts y)
            {
                if (x.name == null && y.name == null) return 0;
                else if (x.name == null) return -1;
                else if (y.name == null) return 1;                
                else return x.name.CompareTo(y.name);
            });
        }
    }
}
