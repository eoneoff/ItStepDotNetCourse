//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Organizer
{
    using System;
    using System.Collections.Generic;
    
    public partial class Job : Event
    {
        public System.DateTime Start { get; set; }
        public System.DateTime Deadline { get; set; }

        public override DateTime TimeStamp
        {
            get
            {
                return (DateTime.Now < Start) ? Start : Deadline;
            }
        }
    }
}
