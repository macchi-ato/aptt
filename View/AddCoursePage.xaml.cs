using mobile_app.Models;
using mobile_app.Database;

namespace mobile_app.View;

public partial class AddCoursePage : ContentPage
{
    private readonly DatabaseService _db;
    private Term _term;
	public AddCoursePage(DatabaseService db, Term term)
	{
		InitializeComponent();
		_db = db;
		_term = term;
	}

	private async void OnSaveClicked(Object sender, EventArgs e)
	{
        if (string.IsNullOrWhiteSpace(CourseName.Text))
        {
            await DisplayAlert("Course Name", "Course name cannot be left empty.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(CourseInstructorName.Text))
        {
            await DisplayAlert("Instructor Name", "Instructor name cannot be left empty.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(CourseInstructorPhone.Text))
        {
            await DisplayAlert("Instructor Phone", "Instructor phone cannot be left empty.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(CourseInstructorEmail.Text))
        {
            await DisplayAlert("Instructor Email", "Instructor Email cannot be left empty.", "OK");
            return;
        }

        if (StatusPicker.SelectedItem == null)
		{
            await DisplayAlert("Course Status", "A course status must be selected.", "OK");
            return;
        }

        if (CourseEndDate.Date <= CourseStartDate.Date)
        {
            await DisplayAlert("Date Error", "Start date cannot be set before the end date.", "OK");
            return;
        }

        var course = new Course
		{
			TermId = _term.Id,
			Name = CourseName.Text,
            InstructorName = CourseInstructorName.Text,
            InstructorPhone = CourseInstructorPhone.Text,
            InstructorEmail = CourseInstructorEmail.Text,
            Notes = CourseNotes.Text,
			Status = StatusPicker.SelectedItem as string,
			CreatedAt = DateTime.Now,
			StartDate = CourseStartDate.Date,
			EndDate = CourseEndDate.Date,
		};

		await _db.AddCourseAsync(course);
		await Navigation.PopModalAsync();
	}
}