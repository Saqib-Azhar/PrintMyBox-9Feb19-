//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataCrawler
{
    using System;
    using System.Collections.Generic;
    
    public partial class PriceQuote
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Stock { get; set; }
        public string Color { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> Height { get; set; }
        public Nullable<int> Width { get; set; }
        public Nullable<int> Depth { get; set; }
        public string Comments { get; set; }
        public string File { get; set; }
        public Nullable<System.DateTime> SubmittedTime { get; set; }
        public Nullable<bool> Seen { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> SeenTime { get; set; }
    }
}
