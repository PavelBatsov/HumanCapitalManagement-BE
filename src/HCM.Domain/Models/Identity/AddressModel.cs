using HCM.Domain.Constraints.Identity;
using System.ComponentModel.DataAnnotations;

namespace HCM.Domain.Models.Identity
{
    public class AddressModel
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(AddressConstraints.AddressMaxLength)]
        public string Address { get; set; }

        [Required]
        [MaxLength(AddressConstraints.CityMaxLength)]
        public string City { get; set; }

        [Required]
        [MaxLength(AddressConstraints.CountryMaxLength)]
        public string Country { get; set; }

        [Required]
        [MaxLength(AddressConstraints.PostCodeMaxLength)]
        public string PostCode { get; set; }
    }
}
