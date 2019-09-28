// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
//               (Clarified meaning of dependent and dependee.)
//               (Clarified names in solution/project structure.)

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
    /// set, and the element is already in the set, the set remains unchanged.
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

        private int size; //keeps a track of dictionary size.
       
        Dictionary<string, HashSet<string>> dependents;
        Dictionary<string, HashSet<string>> dependees;
        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
           dependents = new Dictionary<string,HashSet<string>>();
           dependees = new Dictionary<string, HashSet<string>>();
            size = 0;
                          
        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        { 
           
            get {
                return size;
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
            
            get {

                if (!dependees.ContainsKey(s))
                {
                    return 0;
                }
                    

                return dependees[s].Count;

            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// <returns> if dependents contains key s and if count is not zerro returns true else false. </returns>
        /// </summary>
        public bool HasDependents(string s)
        {
           
            if (dependents.ContainsKey(s) && dependents.Count != 0)
            {
                if (dependents[s].Count != 0)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            //if contains key if s and dependees count is not zer then returns true.
            if(dependees.ContainsKey(s) && dependees.Count != 0)
            {
                if (dependees[s].Count != 0)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            //new hashset to create a copy.
            HashSet<string> setOfStringsInDependents = new HashSet<string>();
            if (!HasDependents(s))
            {
                return setOfStringsInDependents;
            }
            //this function does a iteration and stores every dependents in setOfStringInDependents.
            setOfStringsInDependents = new HashSet<string>(dependents[s]);
            return setOfStringsInDependents;

        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            //new hashset to create a copy.
            HashSet<string> setOfStringsInDependees = new HashSet<string>();
            if (!HasDependees(s))
            {
                return setOfStringsInDependees;
            }
            //this function does a iteration and stores every dependees in setOfStringInDependents.
            setOfStringsInDependees = new HashSet<string>(dependees[s]);

            return setOfStringsInDependees;

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


            if (!dependees.ContainsKey(t))
            {
                //if dependees does not contain key new make a new hashset to store dependents.
                dependees.Add(t, new HashSet<string>());
              
               
            }
            //if dependees contains key we add s to hashset t.
            bool addedFirst = dependees[t].Add(s);


            if (!dependents.ContainsKey(s))
            {
                //this function does a iteration and stores every dependees in setOfStringInDependents.
                dependents.Add(s, new HashSet<string>());
               

            }
            //if dependents contains key we add s to hashset t.
            bool addedSecond =  dependents[s].Add(t);

            //increment of size if we sucessfully add dependent and dependees.
            if(addedFirst || addedSecond)
            {
                size++;
            }
           

        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            bool removedFirst = false;
            bool removedSecond = false;
            //if dependees contains key t we remove s form hashset and store boolean.
            if (dependees.ContainsKey(t))
            {
                 removedFirst = dependees[t].Remove(s);
            }
            //if dependents contains key t we remove s form hashset and store boolean.
            if (dependents.ContainsKey(s))
            {
               removedSecond = dependents[s].Remove(t);
            }
            
            //if dependents of dependees are remoed we decrement size.
            if(removedFirst || removedSecond)
            {
                size--;
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (dependents.ContainsKey(s)==false)
            {
                foreach(String temp in newDependents)
                {
                    this.AddDependency(s, temp);
                }
            }
            else
            {
                HashSet<String> element = dependents[s];
                String[] array = element.ToArray();
                foreach(String temp in array)
                {
                    this.RemoveDependency(s, temp);
                }
                foreach (String temp in newDependents)
                {
                    this.AddDependency(s, temp);
                }
            }

            //HashSet<string> softCopy = new HashSet<string>();

            //if (!dependents.ContainsKey(s))
           // {
             //   dependents.Add(s, new HashSet<string>(newDependents));
            //}
            //if (dependents.ContainsKey(s))
            //{
            //    //firstly, you need to remove s's dependees firstly. (who is dependees with s)
            //    //clean up s 's dependents. 
            //    foreach (string valueInDependents in dependents[s])
            //    {
            //        softCopy.Add(valueInDependents);
            //    }
            //    foreach (string valueInDependents in softCopy)
            //    {
            //        RemoveDependency(s, valueInDependents);
            //    }
            //    foreach (string valueInDependents in newDependents)
            //    {
            //        AddDependency(s, valueInDependents);
            //    }
            //}

            //if (dependents.ContainsKey(s))
            //{
            //    //if dependents contains key s, & dependents hashset count is not 0, we clear depende
            //    //nts hashset.
            //    if (!dependents[s].Count.Equals(0))
            //        dependents[s].Clear();

            //}
            ////if dependents does not cointain key we add new hashset.
            //if (!dependents.ContainsKey(s))
            //{
            //    dependents.Add(s, new HashSet<string>());

            //}
            ////looping over newDependents and adding temp to dependees and also linking both together.
            //foreach (string temp in newDependents)
            //{

            //    dependents[s].Add(temp);
            //    if (!dependees.ContainsKey(temp))
            //    {
            //        dependees.Add(temp, new HashSet<string>());

            //    }
            //    dependees[temp].Add(s);
            //}

        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {

            if (dependees.ContainsKey(s) == false)
            {
                foreach (String temp in newDependees)
                {
                    this.AddDependency(temp, s);
                }
            }
            else
            {
                HashSet<String> element = dependees[s];
                String[] array = element.ToArray();
                foreach (String temp in array)
                {
                    this.RemoveDependency(temp, s);
                }
                foreach (String temp in newDependees)
                {
                    this.AddDependency(temp, s);
                }
            }

        }

    }

}

                                                            