using BooksManager.Model;
using BooksManager.Persistence;
using BooksManager.Service;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace BooksManager.ViewModels
{
    public class BooksViewModel : ViewModelBase
    {
        private BookService _bookService;
        private MainWindowViewModel _mainWindowViewModel;

        private Book _book;
        private int _sortByCount;
        private string _sortByButtonText = "ID";

        public Book Book
        {
            get => _book;
            set => this.RaiseAndSetIfChanged(ref _book, value);
        }
        public string SortByButtonText
        {
            get => _sortByButtonText;
            set => this.RaiseAndSetIfChanged(ref _sortByButtonText, value);
        }

        private ObservableCollection<Book> _bookList;
        public ObservableCollection<Book> BookList
        {
            get => _bookList;
            set => this.RaiseAndSetIfChanged(ref _bookList, value);
        }

        public ReactiveCommand<Unit, Unit> RemoveBookCommand { get; }
        public ReactiveCommand<Unit, Unit> CreateBookCommand { get; }
        public ReactiveCommand<Unit, Unit> UpdateBookCommand { get; }
        public ReactiveCommand<Unit, Unit> SortByCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowCheckoutsViewCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowBookCategoriesViewCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowCalculatorViewCommand { get; }

        public BooksViewModel(MainWindowViewModel mainWindowViewModel, IAppDbContext appDbContext)
        {
            _mainWindowViewModel = mainWindowViewModel;

            RemoveBookCommand = ReactiveCommand.Create(RemoveBook);
            CreateBookCommand = ReactiveCommand.Create(() => _mainWindowViewModel.ShowCreateBookView());
            UpdateBookCommand = ReactiveCommand.Create(() => _mainWindowViewModel.ShowUpdateBookView(Book));
            SortByCommand = ReactiveCommand.Create(SortBy);
            ShowCheckoutsViewCommand = ReactiveCommand.Create(() => _mainWindowViewModel.ShowCheckoutsView());
            ShowBookCategoriesViewCommand = ReactiveCommand.Create(() => _mainWindowViewModel.ShowBookCategoriesView());
            ShowCalculatorViewCommand = ReactiveCommand.Create(() => _mainWindowViewModel.ShowCalculatorView());

            _bookService = new BookService(appDbContext);
            _bookList = new ObservableCollection<Book>(_bookService.GetBooks());
            SortBy();
        }

        private async void RemoveBook()
        {
            if (Book == null) return;

            await _bookService.RemoveBook(Book.Id);
            _bookList = new ObservableCollection<Book>(_bookService.GetBooks());
            this.RaisePropertyChanged(nameof(BookList));
        }

        private void SortBy()
        {
            switch (++_sortByCount % 8)
            {
                case 0:
                    _bookList = new ObservableCollection<Book>(_bookList.OrderBy(n => n.Title));
                    _sortByButtonText = "Sort: Title DESC";
                    break;
                case 1:
                    _bookList = new ObservableCollection<Book>(_bookList.OrderByDescending(n => n.Title));
                    _sortByButtonText = "Sort: Author ASC";
                    break;
                case 2:
                    _bookList = new ObservableCollection<Book>(_bookList.OrderBy(n => n.Author));
                    _sortByButtonText = "Sort: Author DESC";
                    break;
                case 3:
                    _bookList = new ObservableCollection<Book>(_bookList.OrderByDescending(n => n.Author));
                    _sortByButtonText = "Sort: Category ASC";
                    break;
                case 4:
                    _bookList = new ObservableCollection<Book>(_bookList.OrderBy(n => n.BookCategory.Name));
                    _sortByButtonText = "Sort: Category DESC";
                    break;
                case 5:
                    _bookList = new ObservableCollection<Book>(_bookList.OrderByDescending(n => n.BookCategory.Name));
                    _sortByButtonText = "Sort: Title ASC";
                    break;
                case 6:
                    _bookList = new ObservableCollection<Book>(_bookList.OrderBy(n => n.CreatedDate));
                    _sortByButtonText = "Sort: CreatedDate ASC";
                    break;
                case 7:
                    _bookList = new ObservableCollection<Book>(_bookList.OrderByDescending(n => n.CreatedDate));
                    _sortByButtonText = "Sort: CreatedDate DESC";
                    break;
            }

            this.RaisePropertyChanged(nameof(SortByButtonText));
            this.RaisePropertyChanged(nameof(BookList));
        }

    }
}
