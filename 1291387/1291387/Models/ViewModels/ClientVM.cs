using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _1291387.Models.ViewModels
{
    public class ClientVM
    {
        public int clientId { get; set; }
        [Required, StringLength(50), Display(Name = "Client Name")]
        public string clientName { get; set; }
        [Required, Display(Name = "Birth Date"), Column(TypeName = "date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime birthDate { get; set; }
        [Display(Name = "Age")]
        public int age { get; set; }
        public string picture { get; set; }
        [Display(Name = "Picture")]
        public HttpPostedFileBase pictureFile { get; set; }
        [Display(Name = "Inside Dhaka ?")]
        public bool insideDhaka { get; set; }
        public List<int> ProductList { get; set; } = new List<int>();
    }
}