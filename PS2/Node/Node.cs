using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node
{
    public class Node
    {
        private  String Dependent;
        private  String Dependee;
        public Node(String One, String Two)
        {
            Dependee = One;
            Dependent = Two;
        }
        public string getDependent()
        {
            return Dependent;
        }
        public string getDependee()
        {
            return Dependee;
        }
        public void setDependent(String s)
        {
            Dependent = s;
        }
        public void setDependee(String s)
        {
            Dependee = s;
        }
    }
}
