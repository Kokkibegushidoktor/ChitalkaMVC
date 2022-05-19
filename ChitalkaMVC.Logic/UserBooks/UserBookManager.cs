namespace ChitalkaMVC.Logic.UserBooks
{
    public class UserBookManager : IUserBookManager
    {
        private readonly ChitalkaContext _context;
        public UserBookManager(ChitalkaContext context)
        {
            _context = context;
        }
        public async Task Create(string username, int bookId)
        {
            var item = new UserBook { UserId = username, BookId = bookId };
            _context.UserBook.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(string username, int bookId)
        {
            var item = _context.UserBook.FirstOrDefault(ub => ub.BookId == bookId && ub.UserId == username);
            if (item != null)
            {
                _context.UserBook.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
