using HotChocolate.Data.Filters;
using IntVmeAPI.Schema.Queries;

namespace IntVmeAPI.Schema.Filters
{
    public class CourseFilter : FilterInputType<Course>
    {
        protected override void Configure(IFilterInputTypeDescriptor<Course> descriptor)
        {
            descriptor.Ignore(c => c.Students);
            base.Configure(descriptor);
        }
    }
}
