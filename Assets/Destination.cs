using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Destination {

    // Use this for initialization


    public static HashSet<Destination> destinations = new HashSet<Destination>();
    public static HashSet<string> destStrings = new HashSet<string>();
   // public enum dests { Strand, Stellenbosch, Jonkershoek, Bellville, CapeTown, Somerset, Mthatha};
    private string name;

    public Destination()
    {

    }

    public Destination(string name)
    {
        this.name = name;
        if (!destStrings.Contains(this.name))
        {
            destinations.Add(this);
            destStrings.Add(this.name);
        }

    }


    // override object.Equals
    public override bool Equals(object obj)
    {
        //       
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237  
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (Destination)obj;
        
        if (this.name == other.name)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return name.GetHashCode();
    }
    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }
}
