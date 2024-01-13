using HotChocolate.Data.Sorting;
using IntVmeAPI.Schema.Queries;

namespace IntVmeAPI.Schema.Sorters
{
    public class CourseSort : SortInputType<Course>
    {
        protected override void Configure(ISortInputTypeDescriptor<Course> descriptor)
        {
            descriptor.Ignore(c => c.Id);
            descriptor.Ignore(c => c.InstructorId);
            descriptor.Field(c => c.Name).Name("CourseName");

            base.Configure(descriptor);
        }
    }
}
