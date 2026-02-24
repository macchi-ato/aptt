using System.Collections.ObjectModel;
using mobile_app.Database;
using mobile_app.Models;

namespace mobile_app.View;

public partial class AddTermPage : ContentPage
{
	private readonly DatabaseService _db;
	public AddTermPage(DatabaseService db)
	{
        InitializeComponent();
        _db = db;
	}

	private async void OnSaveClicked(object sender, EventArgs e)
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

        var term = new Term
		{
            Name = TermName.Text,
			CreatedAt = DateTime.Now,
			StartDate = TermStartDate.Date,
			EndDate = TermEndDate.Date,
		};

        await _db.AddTermAsync(term);
		await Navigation.PopModalAsync();
    }
}