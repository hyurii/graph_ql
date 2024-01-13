using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using IntVmeAPI.Schema.Mutations;

public class Subscription
{
    [Subscribe]
    public CourseResult CourseCreated([EventMessage] CourseResult course) => course;

    [Subscribe]
    public InstructorResult InstructorCreated([EventMessage] InstructorResult instructor) => instructor;

    public ValueTask<ISourceStream<CourseResult>> SubscribeToCourseUpdated(Guid courseId, ITopicEventReceiver topicEventReceiver)
    {
        string updateCourseTopic = $"{courseId}_{nameof(Subscription.CourseUpdated)}";
        return topicEventReceiver.SubscribeAsync<CourseResult>(updateCourseTopic);
    }

    [Subscribe(With = nameof(SubscribeToCourseUpdated))]
    public CourseResult CourseUpdated(Guid courseId, [EventMessage] CourseResult course) => course;

}