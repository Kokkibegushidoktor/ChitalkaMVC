namespace ChitalkaMVC.Logic.UserBooks
{
    public interface IUserBookManager
    {
        Task Create(string username, int bookId);
        Task Delete(string username, int bookId);
    }
}
