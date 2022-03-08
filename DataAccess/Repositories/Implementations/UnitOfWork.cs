using DataAccess.DataContext;
using DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Genres = new GenreRepository(_context);
            Products = new ProductRepository(_context);
            Companies = new CompanyRepository(_context);
            ApplicationUsers = new ApplicationUserRepository(_context);
            ShoppingCarts = new ShoppingCartRepository(_context);
            OrderHeaders = new OrderHeaderRepository(_context);
            OrderDetails = new OrderDetailRepository(_context);
        }

        public IGenreRepository Genres { get; private set; }
        public IProductRepository Products { get; private set; }
        public ICompanyRepository Companies { get; private set; }
        public IApplicationUserRepository ApplicationUsers { get; private set; }
        public IShoppingCartRepository ShoppingCarts { get; private set;}
        public IOrderHeaderRepository OrderHeaders { get; private set; }
        public IOrderDetailRepository OrderDetails { get; private set; }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
