using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Optsol.Repository.Infra.EFCore.Base.Contexts
{
    public abstract class Context : DbContext
    {
        protected Context() : base() { }

        protected Context([NotNull] DbContextOptions options) : base(options) { }
    }
}
