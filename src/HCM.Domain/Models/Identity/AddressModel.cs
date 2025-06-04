using HCM.Domain.Constraints.Identity;
using System.ComponentModel.DataAnnotations;

namespace HCM.Domain.Models.Identity
{
    public class AddressModel
    {
        public Guid Id { get; set; }

        [MaxLength(AddressConstraints.AddressMaxLength)]
        public string Address { get; set; }

        [MaxLength(AddressConstraints.CityMaxLength)]
        public string City { get; set; }

        [MaxLength(AddressConstraints.CountryMaxLength)]
        public string Country { get; set; }

        [MaxLength(AddressConstraints.PostCodeMaxLength)]
        public string PostCode { get; set; }
    }
}
