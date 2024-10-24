using Microsoft.AspNetCore.Http;
using TxnManager.Domain.Model;

namespace TxnManager.Web.ProblemDetails
{
    public class BusinessRuleValidationProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        public BusinessRuleValidationProblemDetails(BusinessRuleValidationException exception)
        {
            this.Title = exception.Message;
            this.Status = StatusCodes.Status422UnprocessableEntity;
            this.Detail = exception.Details;
            this.Type = "https://transactionsmanager/validation-error";
        }
    }
}