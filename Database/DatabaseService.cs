using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mobile_app.Models;
using SQLite;

namespace mobile_app.Database
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "terms.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Term>().Wait();
            _database.CreateTableAsync<Course>().Wait();
            _database.CreateTableAsync<Assessment>().Wait();
        }

        // Terms
        public Task<int> AddTermAsync(Term term)
        {
            return _database.InsertAsync(term);
        }

        public Task<List<Term>> GetTermsAsync()
        {
            return _database.Table<Term>().ToListAsync();
        }

        public Task<int> UpdateTermAsync(Term term)
        {
            return _database.UpdateAsync(term);
        }

        public Task<int> DeleteTermAsync(Term term)
        {
            return _database.DeleteAsync(term);
        }

        // Courses
        public Task<int> AddCourseAsync(Course course)
        {
            return _database.InsertAsync(course);
        }

        public Task<List<Course>> GetCoursesAsync()
        {
            return _database.Table<Course>().ToListAsync();
        }

        public Task<int> UpdateCourseAsync(Course course)
        {
            return _database.UpdateAsync(course);
        }

        public Task<int> DeleteCourseAsync(Course course)
        {
            return _database.DeleteAsync(course);
        }

        public Task<Course> GetCourseByIdAsync(int courseId)
        {
            return _database.Table<Course>().Where(e => e.Id == courseId).FirstOrDefaultAsync();
        }

        // Assessments
        public Task<int> AddAssessmentAsync(Assessment assessment)
        {
            return _database.InsertAsync(assessment);
        }

        public Task<List<Assessment>> GetAssessmentsAsync()
        {
            return _database.Table<Assessment>().ToListAsync();
        }

        public Task<int> UpdateAssessmentAsync(Assessment assessment)
        {
            return _database.UpdateAsync(assessment);
        }

        public Task<int> DeleteAssessmentAsync(Assessment assessment)
        {
            return _database.DeleteAsync(assessment);
        }
    }
}
