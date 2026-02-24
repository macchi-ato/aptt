using System.Collections.ObjectModel;
using mobile_app.Database;
using mobile_app.Models;
using mobile_app.View;

namespace mobile_app
{
    public partial class MainPage : ContentPage
    {
        private readonly DatabaseService _db;
        public ObservableCollection<Term> Terms { get; set; } = new ObservableCollection<Term>();

        public MainPage(DatabaseService db)
        {
            InitializeComponent();
            _db = db;
            BindingContext = this;
        }

        private async void OnClickAddTerm(Object sender, EventArgs e)
        {
            var addTermPage = new AddTermPage(_db);
            await Navigation.PushModalAsync(addTermPage);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadTerms();
        }

        private async Task LoadTerms()
        {
            var termsFromDb = await _db.GetTermsAsync();
            Terms.Clear();

            foreach (var term in termsFromDb)
            {
                Terms.Add(term);
            }
        }

        private async void OnTermSelected(Object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Term selectedTerm)
            {
                TermsListView.SelectedItem = null;

                await Navigation.PushModalAsync(new TermPage(_db, selectedTerm));
            }
        }       
    }

}
