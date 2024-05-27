using Microsoft.EntityFrameworkCore;
using Kluby.Models;

namespace Kluby.Data;

public class KlubyDBContext : DbContext
{
    public KlubyDBContext(DbContextOptions<KlubyDBContext> options) : base(options)
    {
        Database.EnsureCreated();

        Users = Set<UserModel>() as DbSet<UserModel>;
    }
    
    public DbSet<UserModel> Users { get; set; }
}