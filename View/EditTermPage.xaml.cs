using mobile_app.Database;
using mobile_app.Models;

namespace mobile_app.View;

public partial class EditTermPage : ContentPage
{
    private readonly DatabaseService _db;
    private Term _term;
    public EditTermPage(DatabaseService db, Term term)
	{
		InitializeComponent();
		_db = db;
		_term = term;
		BindingContext = _term;
	}

	private async void OnSaveEditTerm(object sender, EventArgs e)
	{
        if (string.IsNullOrWhiteSpace(TermName.Text))
        {
            await DisplayAlert("Empty Fields", "Fields cannot be left empty.", "OK");
            return;
        }

        if (TermEndDate.Date <= TermStartDate.Date)
        {
            await DisplayAlert("Date Error", "Start date cannot be set before the end date.", "OK");
            return;
        }

        _term.Name = TermName.Text;
		_term.StartDate = TermStartDate.Date;
		_term.EndDate = TermEndDate.Date;

		await _db.UpdateTermAsync(_term);
		await Navigation.PopModalAsync();
	}

	private async void OnDeleteTerm(object sender, EventArgs e)
	{
		await _db.DeleteTermAsync(_term);
		await Shell.Current.GoToAsync("//MainPage");
    }
}