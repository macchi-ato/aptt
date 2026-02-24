using System.Collections.ObjectModel;
using mobile_app.Database;
using mobile_app.Models;

namespace mobile_app.View;

public partial class TermPage : ContentPage
{
    private readonly DatabaseService _db;
    private Term _term;
    public ObservableCollection<Course> Courses { get; set; } = new ObservableCollection<Course>();
    
    public TermPage(DatabaseService db, Term term)
	{
		InitializeComponent();
        _db = db;
        _term = term;
		BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadCourses();

        // Add and update term name label
        TermNameLabel.Text = _term.Name;
    }

    private async void OnBackClick(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void OnEditClick(object sender, EventArgs e)
    {
        var editTermPage = new EditTermPage(_db, _term);
        await Navigation.PushModalAsync(editTermPage);
    }

    private async void OnAddCourseClicked(object sender, EventArgs e)
    {
        var addCoursePage = new AddCoursePage(_db, _term);
        await Navigation.PushModalAsync(addCoursePage);
    }

    private async Task LoadCourses()
    {
        var coursesFromDb = await _db.GetCoursesAsync();
        Courses.Clear();

        foreach (var course in coursesFromDb)
        {
           if (course.TermId == _term.Id)
            {
                Courses.Add(course);
            } 
        }
    }

    private async void OnCourseSelected(Object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Course selectedCourse)
        {
            CourseListView.SelectedItem = null;

            await Navigation.PushModalAsync(new CoursePage(_db, selectedCourse));
        }
    }
}