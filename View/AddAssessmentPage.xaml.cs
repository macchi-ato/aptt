namespace mobile_app.View;
using mobile_app.Models;
using mobile_app.Database;

public partial class AddAssessmentPage : ContentPage
{
    private readonly DatabaseService _db;
    private Course _course;
    public AddAssessmentPage(DatabaseService db, Course course)
	{
		InitializeComponent();
        _db = db;
        _course = course;
	}

    private async void OnSaveClicked(Object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(AssessmentName.Text))
        {
            await DisplayAlert("Empty Fields", "Fields cannot be left empty.", "OK");
            return;
        }

        if (TypePicker.SelectedItem == null)
        {
            await DisplayAlert("Assessment Type", "An assessment type must be selected.", "OK");
            return;
        }

        if (AssessmentEndDate.Date <= AssessmentStartDate.Date)
        {
            await DisplayAlert("Date Error", "Start date cannot be set before the end date.", "OK");
            return;
        }

        var assessmentsFromDb = await _db.GetAssessmentsAsync();

        foreach (var assessmentType in assessmentsFromDb)
        {
            if (assessmentType.Type == TypePicker.SelectedItem.ToString() && assessmentType.CourseId == _course.Id)
            {
                await DisplayAlert("Assessment Type", "An assessment of this type already exists.", "OK");
                return;
            }
        }

        var assessment = new Assessment
        {
            CourseId = _course.Id,
            Name = AssessmentName.Text,
            Type = TypePicker.SelectedItem as string,
            CreatedAt = DateTime.Now,
            StartDate = AssessmentStartDate.Date,
            EndDate = AssessmentEndDate.Date,
        };

        await _db.AddAssessmentAsync(assessment);
        await Navigation.PopModalAsync();
    }
}