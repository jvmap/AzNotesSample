using AzNotesSample.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AzNotesSample.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IStorage _storage;

        [BindProperty]
        public string Text { get; set; } = string.Empty;

        [TempData]
        public string Message { get; set; } = string.Empty;

        public string StorageTechnology => _storage.DescriptiveText;

        public IndexModel(ILogger<IndexModel> logger, IStorage storage)
        {
            _logger = logger;
            _storage = storage;
        }

        public async Task OnGetAsync()
        {
            Text = await _storage.LoadAsync();
        }

        public async Task<ActionResult> OnPostAsync()
        {
            await _storage.SaveAsync(Text);
            Message = "Note saved";
            return RedirectToAction(null);
        }
    }
}