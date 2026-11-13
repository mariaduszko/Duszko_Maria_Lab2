using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Duszko_Maria_Lab2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace Duszko_Maria_Lab2.Pages.BookPages;

[Authorize(Roles = "Admin")]
public class CreateModel : BookCategoriesPageModel
{
    private readonly Duszko_Maria_Lab2Context _context;

    public CreateModel(Duszko_Maria_Lab2Context context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        ViewData["PublisherID"] = new SelectList(_context.Set<Publisher>(), "ID", "PublisherName");
        ViewData["AuthorID"] = new SelectList(
    _context.Set<Author>()
        .AsEnumerable()
        .Select(a => new {
            a.ID,
            FullName = a.FirstName + " " + a.LastName
        }),
    "ID",
    "FullName"
);
        var book = new Book();
        book.BookCategories = new List<BookCategory>();

        PopulateAssignedCategoryData(_context, book);
        return Page();
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync(string[] selectedCategories)
    {
        if (!ModelState.IsValid)
        {
            PopulateAssignedCategoryData(_context, Book);
            return Page();
        }
        var newBook = new Book();
        if(selectedCategories != null)
        {
            newBook.BookCategories = new List<BookCategory>();
            foreach (var cat in selectedCategories)
            {
                var catToAdd = new BookCategory
                {
                    CategoryID = int.Parse(cat)
                };
                newBook.BookCategories.Add(catToAdd);
            }
        }

        Book.BookCategories = newBook.BookCategories;
        _context.Book.Add(Book);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
