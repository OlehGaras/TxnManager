using Microsoft.AspNetCore.Http;
using TxnManager.Infrastructure;

namespace TxnManager.Web.ProblemDetails
{
    public class FileParseProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public FileParseProblemDetails(FileParseException exception)
        {
            this.Title = exception.Message;
            this.Status = StatusCodes.Status400BadRequest;
            this.Detail = "See errorRows property for details";
            this.Type = "https://transactionsmanager/validation-error";
            this.Extensions.Add("errorRows", exception.ErrorResults);
        }
    }
}