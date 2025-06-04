using HCM.Domain.Constraints.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCM.Domain.Entities.Identity
{
    public class AddressEntity : TrackableEntity<AddressEntity>
    {
        [MaxLength(AddressConstraints.AddressMaxLength)]
        public string Address { get; set; }

        [MaxLength(AddressConstraints.CityMaxLength)]
        public string City { get; set; }

        [MaxLength(AddressConstraints.CountryMaxLength)]
        public string Country { get; set; }

        [MaxLength(AddressConstraints.PostCodeMaxLength)]
        public string PostCode { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual UserEntity User { get; set; }
    }
}
