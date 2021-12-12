using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Optsol.Components.Repository.Infra.EntityFrameworkCore.Contexts
{
    public abstract class Context : DbContext
    {
        public Context() : base() { }

        public Context([NotNull] DbContextOptions options) : base(options) { }
    }
}
