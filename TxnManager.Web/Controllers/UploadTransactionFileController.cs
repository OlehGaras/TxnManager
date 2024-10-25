using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TxnManager.Domain.Service.Abstractions;
using TxnManager.Web.Dto;

namespace TxnManager.Controllers
{
    public class UploadTransactionFileController : Controller
    {
        private readonly IUploadService _uploadService;

        public UploadTransactionFileController(IUploadService uploadService)
        {
            _uploadService = uploadService;
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Upload(UploadDocumentDto doc)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }

            using (var stream = doc.FormFile.OpenReadStream())
            {
                await _uploadService.UploadTransactionsFileAsync(stream, doc.GetExtension(),
                    CancellationToken.None);
            }

            // we could return Ok() here
            // but I return View() to be able to upload new files.
            return View();
        }
    }
}