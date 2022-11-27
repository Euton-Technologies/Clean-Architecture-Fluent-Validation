namespace MoviesExample.Application.Common.Models;
public abstract class DeletePayload : Payload
{
    public DeletePayload(IReadOnlyList<UserError>? errors = null)
        : base(errors)
    { }

    public String DeletedId { get; set; } = default!;
}
