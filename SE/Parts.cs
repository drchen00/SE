using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE
{
    public class Parts
    {
        public string name;
        public string creator;
        public string creationTime;
        public string project;
        public string type;
        public string EnginDrw;
        public string AsmModel;

        public Parts(string name, string creator, string time, string project, string type, string EnginDrw, string AsmModel)
        {
            this.name = name;
            this.creator = creator;
            this.creationTime = time;
            this.project = project;
            this.type = type;
            this.EnginDrw = EnginDrw;
            this.AsmModel = AsmModel;
        }
    }
}
