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
    
    public partial class Meeting : Event
    {
        public int MeetingStartId { get; set; }
        public int MeetingEndId { get; set; }
        public string Location { get; set; }
    
        public virtual Schedule End { get; set; }
        public virtual Schedule Start { get; set; }
    }
}
