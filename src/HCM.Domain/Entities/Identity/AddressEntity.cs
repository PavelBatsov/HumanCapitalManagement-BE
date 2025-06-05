using HCM.Domain.Constraints.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HCM.Domain.Entities.Identity
{
    public class AddressEntity : TrackableEntity<AddressEntity>
    {
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

        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual UserEntity User { get; set; }
    }
}
