using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Duszko_Maria_Lab2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Duszko_Maria_Lab2.Pages.BookPages;

public class EditModel : BookCategoriesPageModel
{
    private readonly Duszko_Maria_Lab2Context _context;

    public EditModel(Duszko_Maria_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var book = await _context.Book
            .Include(b=> b.Author)
            .Include(b => b.Publisher)
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .FirstOrDefaultAsync(m => m.ID == id);
        if (book is null)
        {
            return NotFound();
        }
        Book = book;
        PopulateAssignedCategoryData(_context, Book);

        ViewData["PublisherID"] = new SelectList(_context.Set<Publisher>(), "ID", "PublisherName");
        ViewData["AuthorID"] = new SelectList(
    _context.Set<Author>()
        .AsEnumerable()
        .Select(a => new {
            a.ID,
            FullName = a.FirstName + " " + a.LastName
        }),
    "ID",
    "FullName",
    Book.AuthorID
);
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync(int? id, string[] selectedCategories)
    {
        if (id == null) 
        { 
            return NotFound(); 
        }

        var bookToUpdate = await _context.Book
            .Include(i => i.Author)
            .Include(i => i.Publisher)
            .Include(i => i.BookCategories)
                .ThenInclude(i => i.Category)
            .FirstOrDefaultAsync(s => s.ID == id); 
        
        if (bookToUpdate == null) 
        { 
            return NotFound(); 
        }

        if(await TryUpdateModelAsync<Book>(
            bookToUpdate,
            "Book",
             i => i.Title, i => i.Price, i => i.PublishingDate, i => i.PublisherID, i => i.AuthorID))
        {
            UpdateBookCategories(_context, selectedCategories, bookToUpdate);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
        UpdateBookCategories(_context, selectedCategories, bookToUpdate);
        PopulateAssignedCategoryData(_context, bookToUpdate);
        return Page();
        

        
    }

    private bool BookExists(int id)
    {
        return _context.Book.Any(e => e.ID == id);
    }
}
