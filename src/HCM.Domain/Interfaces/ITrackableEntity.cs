namespace HCM.Domain.Interfaces
{
    public interface ITrackableEntity
    {
        DateTime CreatedOn { get; set; }

        Guid CreatedById { get; set; }

        DateTime? ModifiedOn { get; set; }

        Guid? ModifiedById { get; set; }
    }
}
