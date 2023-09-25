using Uqs.Blog.Domain.DomainObjects;

namespace Uqs.Blog.Domain.Interfaces;
public interface IPostRepository
{
    int CreatePost(int authorId);
    Post? GetById(int postId);
    void Update(Post post);
}