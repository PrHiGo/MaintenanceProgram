using MaintenanceProgram.Models.Entities;
using MaintenanceProgram.Models.Forms;

namespace MaintenanceProgram.Services;

internal class CommentService: GenericService<CommentEntity>
{
    public async Task<CommentEntity> CreateNewComment(CommentForm form)
    {
        var newComment = new CommentEntity()
        {
            Created = form.Created,
            Comment = form.Comment,
            
        };

        return null!;
    }
}

