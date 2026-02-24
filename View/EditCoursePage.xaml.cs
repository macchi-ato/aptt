using mobile_app.Models;
using mobile_app.Database;
using Plugin.LocalNotification;
using System.ComponentModel.DataAnnotations;

namespace mobile_app.View;

public partial class EditCoursePage : ContentPage
{
    private readonly DatabaseService _db;
    private Course _course;
    public EditCoursePage(DatabaseService db, Course course)
	{
		InitializeComponent();
        _db = db;
        _course = course;
        BindingContext = _course;

        // Notification checkboxes
        if(_course.StartNotificationScheduled)
        {
            StartNotificationCheckbox.IsChecked = true;
        }
        else
        {
            StartNotificationCheckbox.IsChecked = false;
        }

        if (_course.EndNotificationScheduled)
        {
            EndNotificationCheckbox.IsChecked = true;
        }
        else
        {
            EndNotificationCheckbox.IsChecked = false;
        }

        Console.WriteLine(LocalNotificationCenter.Current.AreNotificationsEnabled());
    }

    private async void OnSaveEditCourse(object sender, EventArgs e)
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

        if (CourseEndDate.Date <= CourseStartDate.Date)
        {
            await DisplayAlert("Date Error", "Start date cannot be set before the end date.", "OK");
            return;
        }

        _course.Name = CourseName.Text;
        _course.Status = StatusPicker.SelectedItem as string;
        _course.InstructorName = CourseInstructorName.Text;
        _course.InstructorPhone = CourseInstructorPhone.Text;
        _course.InstructorEmail = CourseInstructorEmail.Text;
        _course.Notes = CourseNotes.Text;
        _course.StartDate = CourseStartDate.Date;
        _course.EndDate = CourseEndDate.Date;

        await _db.UpdateCourseAsync(_course);
        await Navigation.PopModalAsync();
    }

    private async void OnDeleteCourse(object sender, EventArgs e)
    {
        await _db.DeleteCourseAsync(_course);

        //go back to TermPage
        await Shell.Current.GoToAsync("..");
        await Shell.Current.GoToAsync("..");
    }

    private async void OnCheckedStartNotification(Object sender, EventArgs e)
    {
        if (StartNotificationCheckbox.IsChecked && !_course.StartNotificationScheduled)
        {
            var notifyTime = new DateTime(_course.StartDate.Year, _course.StartDate.Month, _course.StartDate.Day, 6, 0, 0);

            await LocalNotificationCenter.Current.RequestNotificationPermission();

            await LocalNotificationCenter.Current.Show(new NotificationRequest
            {
                NotificationId = _course.Id,
                Title = "Course Starting",
                Description = $"{_course.Name} starts today.",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = notifyTime
                }
            });

            _course.StartNotificationScheduled = true;
            _course.StartNotificationId = _course.Id;
            await _db.UpdateCourseAsync(_course);
        }
        else if (!StartNotificationCheckbox.IsChecked && _course.StartNotificationScheduled)
        {
            LocalNotificationCenter.Current.Cancel(_course.StartNotificationId.Value);
            _course.StartNotificationScheduled = false;
            _course.StartNotificationId = null;
            await _db.UpdateCourseAsync(_course);
        }
    }

    private async void OnCheckedEndNotification(Object sender, EventArgs e)
    {
        if (EndNotificationCheckbox.IsChecked && !_course.EndNotificationScheduled)
        {
            var notifyTime = new DateTime(_course.EndDate.Year, _course.EndDate.Month, _course.EndDate.Day, 6, 0, 0);

            await LocalNotificationCenter.Current.RequestNotificationPermission();

            await LocalNotificationCenter.Current.Show(new NotificationRequest
            {
                NotificationId = _course.Id + 1,
                Title = "Course Ending",
                Description = $"{_course.Name} ends today at midnight.",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = notifyTime
                }
            });

            _course.EndNotificationScheduled = true;
            _course.EndNotificationId = _course.Id;
            await _db.UpdateCourseAsync(_course);
        }
        else if (!EndNotificationCheckbox.IsChecked && _course.EndNotificationScheduled)
        {
            LocalNotificationCenter.Current.Cancel(_course.EndNotificationId.Value);
            _course.EndNotificationScheduled = false;
            _course.EndNotificationId = null;
            await _db.UpdateCourseAsync(_course);
        }
    }

    private async void OnShareNotes(Object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_course.Notes))
        {
            await DisplayAlert("No Notes", "There are no notes to share.", "OK");
            return;
        }

        await Share.Default.RequestAsync(new ShareTextRequest
        {
            Text = _course.Notes,
            Title = "Share Notes"
        });     
    }
}