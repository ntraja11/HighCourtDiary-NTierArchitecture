namespace CourtDiary.Data.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IOrganizationRepository Organizations { get; }
        ICaseRepository Cases { get; }
        IHearingRepository Hearings { get; }
        Task SaveAsync();
    }
}
