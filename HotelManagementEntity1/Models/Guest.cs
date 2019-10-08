//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HotelManagementEntity1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Guest
    {

        public int guestId { get; set; }
        public string guestName { get; set; }
        public string guestContact { get; set; }
        [Required(ErrorMessage = "Please Enter Your Address")]
        [RegularExpression("/^[a-z]{0,50}$/", ErrorMessage = "Enter below 50 characters")]
        public string guestAddress { get; set; } 
        [Required(ErrorMessage = "Please Select Room Type")]  
        public string guestRoomType { get; set; }
        [Required(ErrorMessage = "Please Select Number Of Rooms")]
        [RegularExpression("([1-9][0-9]*([0-9]+)?|0+[0-9]*[1-9][0-9]*$.)", ErrorMessage = "Invalid entry")]
        public Nullable<int> guestQuantity { get; set; }
        [Required(ErrorMessage = "Please Select Checkin Date")]
        public Nullable<System.DateTime> guestCheckinDate { get; set; }
        public Nullable<int> guestNoOfDaysStay { get; set; }
        public Nullable<int> roomId { get; set; }
        public Nullable<int> guestCost { get; set; }
    
        public virtual Room Room { get; set; }
    }
}