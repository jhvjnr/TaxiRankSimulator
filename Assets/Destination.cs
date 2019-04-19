using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Destination {

    // Use this for initialization


    public static Dictionary<string, Destination> destinations = new Dictionary<string, Destination>();
    public static HashSet<string> destStrings = new HashSet<string>();
   // public enum dests { Strand, Stellenbosch, Jonkershoek, Bellville, CapeTown, Somerset, Mthatha};
    private string name;
    public long TotalCommuters{ get; set; }
    [XmlIgnore] public Dictionary<float, int> Fluxes = new Dictionary<float, int>();

    public Destination()
    {

    }

    public Destination(string name)
    {
        this.name = name;
        TotalCommuters = 0;
        if (!destStrings.Contains(this.name))
        {
            destinations.Add(this.name, this);
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
