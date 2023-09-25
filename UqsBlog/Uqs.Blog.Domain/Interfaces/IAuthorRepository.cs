using Uqs.Blog.Domain.DomainObjects;

namespace Uqs.Blog.Domain.Interfaces;

public interface IAuthorRepository
{
    Author? GetById(int id);
}
