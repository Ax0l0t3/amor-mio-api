namespace Framework.Interfaces
{
    public interface IObserver
    {
        Task Update(ISubject subject);
    }
}