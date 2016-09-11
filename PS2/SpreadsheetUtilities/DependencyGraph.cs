// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged. Changed Here!!!!
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        private class Node
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

        List<Node> Graph;
        
        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            Graph = new List<Node>();
        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get
            {
                return Graph.Count;
            }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
           get
            {
                IEnumerable<String> list = GetDependees(s);
                return list.Count();
            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            // If the graph is empty then we know there will be no dependents of s.
            if (Graph.Count() == 0)
            {
                return false;
            }
            // Now we know we can envoke GetDependents since there is something in the graph.
            else  
            {
                IEnumerable<String> list = GetDependents(s);
                if(list.Count() == 0)
                {
                    return false;
                } else
                {
                    return true;
                }
            }
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            // If the graph is empty then we know there will be no dependees of s.
            if (Graph.Count() == 0)
            {
                return false;
            }
            // Now we know we can envoke GetDependees since there is something in the graph.
            else
            {
                IEnumerable<String> list = GetDependees(s);
                if (list.Count() == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            List<String> list = new List<String>();
            if (Graph.Count == 0)
            {
                return list;
            }
            Node temp;
            for(int i = 0; i < Graph.Count; i++)
            {
                temp = Graph[i];
                if (temp.getDependee().Equals(s))
                {
                    list.Add(temp.getDependent());
                }
            }
            return list;
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            List<String> list = new List<String>();
            if (Graph.Count == 0)
            {
                return list;
            }
            Node temp;
            for (int i = 0; i < Graph.Count; i++)
            {
                temp = Graph[i];
                if (temp.getDependent().Equals(s))
                {
                    list.Add(temp.getDependee());
                }
            }
            return list;
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {
            int index;
            Node temp = new Node(s, t);
            if (myContains(temp, out index)) 
            {
                return;
            }
            Graph.Add(temp);
        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            int index;
            Node temp = new Node(s, t);
            if (!(myContains(temp, out index)))
            {
                return;
            }
            Graph.RemoveAt(index);
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            Node temp;
            int DependentCount = 0;
            for(int i = 0; i < Graph.Count; i++)
            {
                if(DependentCount >= newDependents.Count())
                {
                    break;
                }
                temp = Graph[i];
                if (temp.getDependee().Equals(s))
                {
                    temp.setDependent(newDependents.ElementAt(DependentCount));
                    Graph[i] = temp;
                    DependentCount++;
                }
            }
            if (DependentCount < newDependents.Count())
            {
                int DependentsUsed = DependentCount;
                for (int j = 0; j < (newDependents.Count() - DependentsUsed); j++)
                {
                    AddDependency(s, newDependents.ElementAt(DependentCount));
                    DependentCount++;
                }

            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            Node temp;
            int DependeeCount = 0;
            for (int i = 0; i < Graph.Count; i++)
            {
                if (DependeeCount >= newDependees.Count())
                {
                    break;
                }
                temp = Graph[i];
                if (temp.getDependent().Equals(s))
                {
                    temp.setDependee(newDependees.ElementAt(DependeeCount));
                    Graph[i] = temp;
                    DependeeCount++;
                }
            }
            if (DependeeCount < newDependees.Count())
            {
                int DependeesUsed = DependeeCount;
                for (int j = 0; j < (newDependees.Count() - DependeesUsed); j++)
                {
                    AddDependency(newDependees.ElementAt(DependeeCount), s);
                    DependeeCount++;
                }
            }
        }
        private bool myContains(Node node, out int x)
        {
            Node temp;
            for (int i = 0; i < Graph.Count; i++)
            {
                temp = Graph.ElementAt(i);
                if ((temp.getDependent().Equals(node.getDependent())) && (temp.getDependee().Equals(node.getDependee())))
                {
                    x = i;
                    return true;
                }
            }
            x = -1; // This should never be used if set to -1;
            return false;
        }

    }

}