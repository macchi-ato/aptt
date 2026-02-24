using System.Collections.ObjectModel;
using mobile_app.Database;
using mobile_app.Models;

namespace mobile_app.View;

public partial class CoursePage : ContentPage
{
    private readonly DatabaseService _db;
    private Course _course;
    public ObservableCollection<Assessment> Assessments { get; set; } = new ObservableCollection<Assessment>();
	public CoursePage(DatabaseService db, Course course)
	{
		InitializeComponent();
        _db = db;
        _course = course;
        BindingContext = this;    
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadAssessments();

        var updatedCourse = await _db.GetCourseByIdAsync(_course.Id);
        if (updatedCourse != null)
        {
            _course = updatedCourse;
            BindingContext = this;
        }

        CourseNameLabel.Text = _course.Name;
        InstructorName.Text = _course.InstructorName;
        InstructorPhone.Text = _course.InstructorPhone;
        InstructorEmail.Text = _course.InstructorEmail;
        Notes.Text = _course.Notes;
        StatusLabel.Text = _course.Status;
        DateLabel.Text = _course.DateRange;
    }

    private async void OnBackClick(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void OnEditClick(object sender, EventArgs e)
    {
        var editCoursePage = new EditCoursePage(_db, _course);
        await Navigation.PushModalAsync(editCoursePage);
    }

    private async void OnAddAssessment(object sender, EventArgs e)
    {
        var addAssessmentPage = new AddAssessmentPage(_db, _course);
        await Navigation.PushModalAsync(addAssessmentPage);
    }

    private async Task LoadAssessments()
    {
        var assessmentsFromDb = await _db.GetAssessmentsAsync();
        Assessments.Clear();

        foreach (var assessment in assessmentsFromDb)
        {
            if (assessment.CourseId == _course.Id)
            {
                Assessments.Add(assessment);
            }
        }
    }

    private async void OnAssessmentSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Assessment selectedAssessment)
        {
            AssessmentListView.SelectedItem = null;

            await Navigation.PushModalAsync(new EditAssessmentPage(_db, selectedAssessment));
        }
    }
}