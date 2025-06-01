using HCM.Domain.Interfaces;

namespace HCM.Domain.Entities
{
    public class TrackableEntity<TEntity> : EntityBase<TEntity>, ITrackableEntity
        where TEntity : class
    {
        public DateTime CreatedOn { get; set; }

        public Guid CreatedById { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public Guid? ModifiedById { get; set; }
    }
}
