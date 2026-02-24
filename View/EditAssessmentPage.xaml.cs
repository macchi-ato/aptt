using mobile_app.Database;
using mobile_app.Models;
using Plugin.LocalNotification;

namespace mobile_app.View;

public partial class EditAssessmentPage : ContentPage
{
    private readonly DatabaseService _db;
    private Assessment _assessment;
    public EditAssessmentPage(DatabaseService db, Assessment assesment)
	{
		InitializeComponent();
        _db = db;
        _assessment = assesment;
        BindingContext = _assessment;

        // Notification checkboxes
        if (_assessment.StartNotificationScheduled)
        {
            StartNotificationCheckbox.IsChecked = true;
        }
        else
        {
            StartNotificationCheckbox.IsChecked = false;
        }

        if (_assessment.EndNotificationScheduled)
        {
            EndNotificationCheckbox.IsChecked = true;
        }
        else
        {
            EndNotificationCheckbox.IsChecked = false;
        }
    }

    private async void OnSave(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(AssessmentName.Text))
        {
            await DisplayAlert("Empty Fields", "Fields cannot be left empty.", "OK");
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
            if (assessmentType.Type == TypePicker.SelectedItem.ToString() && assessmentType.Id != _assessment.Id)
            {
                await DisplayAlert("Assessment Type", "An assessment of this type already exists.", "OK");
                return;
            }
        }

        _assessment.Name = AssessmentName.Text;
        _assessment.Type = TypePicker.SelectedItem as string;
        _assessment.StartDate = AssessmentStartDate.Date;
        _assessment.EndDate = AssessmentEndDate.Date;

        await _db.UpdateAssessmentAsync(_assessment);
        await Navigation.PopModalAsync();
    }

    private async void OnDelete(object sender, EventArgs e)
    {
        await _db.DeleteAssessmentAsync(_assessment);
        await Navigation.PopModalAsync();
    }

    private async void OnCheckedStartNotification(Object sender, EventArgs e)
    {
        if (StartNotificationCheckbox.IsChecked && !_assessment.StartNotificationScheduled)
        {
            var notifyTime = new DateTime(_assessment.StartDate.Year, _assessment.StartDate.Month, _assessment.StartDate.Day, 6, 0, 0);

            await LocalNotificationCenter.Current.RequestNotificationPermission();

            await LocalNotificationCenter.Current.Show(new NotificationRequest
            {
                NotificationId = _assessment.Id * 10,
                Title = "Assessment Starting",
                Description = $"{_assessment.Name} starts today.",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = notifyTime
                }
            });

            _assessment.StartNotificationScheduled = true;
            _assessment.StartNotificationId = _assessment.Id;
            await _db.UpdateAssessmentAsync(_assessment);
        }
        else if (!StartNotificationCheckbox.IsChecked && _assessment.StartNotificationScheduled)
        {
            LocalNotificationCenter.Current.Cancel(_assessment.StartNotificationId.Value);
            _assessment.StartNotificationScheduled = false;
            _assessment.StartNotificationId = null;
            await _db.UpdateAssessmentAsync(_assessment);
        }
    }

    private async void OnCheckedEndNotification(Object sender, EventArgs e)
    {
        if (EndNotificationCheckbox.IsChecked && !_assessment.EndNotificationScheduled)
        {
            var notifyTime = new DateTime(_assessment.EndDate.Year, _assessment.EndDate.Month, _assessment.EndDate.Day, 6, 0, 0);

            await LocalNotificationCenter.Current.RequestNotificationPermission();

            await LocalNotificationCenter.Current.Show(new NotificationRequest
            {
                NotificationId = _assessment.Id * 11,
                Title = "Assessment Ending",
                Description = $"{_assessment.Name} ends today at midnight.",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = notifyTime
                }
            });

            _assessment.EndNotificationScheduled = true;
            _assessment.EndNotificationId = _assessment.Id;
            await _db.UpdateAssessmentAsync(_assessment);
        }
        else if (!EndNotificationCheckbox.IsChecked && _assessment.EndNotificationScheduled)
        {
            LocalNotificationCenter.Current.Cancel(_assessment.EndNotificationId.Value);
            _assessment.EndNotificationScheduled = false;
            _assessment.EndNotificationId = null;
            await _db.UpdateAssessmentAsync(_assessment);
        }
    }
}