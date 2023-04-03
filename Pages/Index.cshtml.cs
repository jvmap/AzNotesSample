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
            return RedirectToAction(null);
        }
    }
}