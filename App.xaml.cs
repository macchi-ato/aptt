using mobile_app.Database;
using mobile_app.Models;

namespace mobile_app
{
    public partial class App : Application
    {
        private readonly DatabaseService _db;
        public App(DatabaseService db)
        {
            InitializeComponent();
            _db = db;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // block thread to wait for seed data before displaying UI
            Task.Run(async () => await SeedData()).Wait();
            return new Window(new AppShell());
        }

        public async Task SeedData()
        {
            var terms = await _db.GetTermsAsync();

            if (terms.Count == 0)
            {
                var term = new Term
                {
                    Name = "Spring Term",
                    StartDate = new DateTime(2025, 1, 1),
                    EndDate = new DateTime(2025, 5, 28)
                };
                await _db.AddTermAsync(term);

                var course = new Course
                {
                    TermId = term.Id,
                    Name = "Software Engineering",
                    Status = "Plan to Take",
                    StartDate = new DateTime(2025, 1, 1),
                    EndDate = new DateTime(2025, 5, 28),
                    InstructorName = "Anika Patel",
                    InstructorPhone = "555-123-4567",
                    InstructorEmail = "anika.patel@strimeuniversity.edu"
                };
                await _db.AddCourseAsync(course);

                var objectiveAssessment = new Assessment
                {
                    CourseId = course.Id,
                    Name = "Mobile App Development",
                    Type = "Objective",
                    StartDate = new DateTime(2025, 4, 1),
                    EndDate = new DateTime(2025, 5, 1),
                };

                var performanceAssessment = new Assessment
                {
                    CourseId = course.Id,
                    Name = "Capstone",
                    Type = "Performance",
                    StartDate = new DateTime(2025, 5, 1),
                    EndDate = new DateTime(2025, 5, 28),
                };

                await _db.AddAssessmentAsync(objectiveAssessment);
                await _db.AddAssessmentAsync(performanceAssessment);
            }
        }
    }
}