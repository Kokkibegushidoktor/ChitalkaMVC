
using Microsoft.AspNetCore.Hosting;

namespace ChitalkaMVC.Logic.Authors
{
    public class AuthorManager : IAuthorManager
    {
        private readonly ChitalkaContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public AuthorManager(ChitalkaContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        public async Task Create(Author item)
        {
            var image = item.AuthorImage;
            string root = _hostEnvironment.WebRootPath;
            string filename = Path.GetFileNameWithoutExtension(image.ImageFile.FileName);
            string extension = Path.GetExtension(image.ImageFile.FileName);
            image.ImageName = filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
            string path = Path.Combine(root + "/Images/Authors/", filename);
            using (var filestream = new FileStream(path, FileMode.Create))
            {
                await image.ImageFile.CopyToAsync(filestream);
            }
            _context.AuthorImages.Add(image);
            _context.Authors.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task<Author> Find(int id)
        {
            return await _context.Authors.Include(u => u.Country).Include(u=>u.AuthorImage).FirstOrDefaultAsync(item => item.Id == id);
        }

        public async Task<bool> Update(Author author)
        {
            try
            {
                _context.Update(author);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var item = Find(author.Id);
                if (item == null)
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
            return true;
        }

        public async Task Delete(int id)
        {
            var item = _context.Authors.FirstOrDefault(item => item.Id == id);
            if (item != null)
            {
                _context.Authors.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IList<Author>> GetAll() => await _context.Authors.Include(u => u.Country).Include(u => u.AuthorImage).ToListAsync();
    }
}
