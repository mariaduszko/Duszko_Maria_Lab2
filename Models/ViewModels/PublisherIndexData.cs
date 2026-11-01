using Duszko_Maria_Lab2.Models;
namespace Duszko_Maria_Lab2.Models.ViewModels
{
    public class PublisherIndexData
    {

        public IEnumerable<Publisher> Publishers { get; set; }
        public IEnumerable<Book> Books { get; set; }

    }
}
