namespace Optsol.Components.Repository.Infra.Mock.Entities.Core
{
    public class Person
    {
        public string Name { get; private set; }

        public string Family { get; private set; }

        public static Person Create(string name, string family) =>
            new()
            {
                Name = name,
                Family = family
            };
    }
}